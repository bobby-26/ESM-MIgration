using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;

public partial class DocumentManagementDocumentnNewSectionRevisionEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuSave.AccessRights = this.ViewState;
            MenuSave.MenuList = toolbar.Show();

            if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
                ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
            else
                ViewState["DOCUMENTID"] = "";

            if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != "")
                ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();
            else
                ViewState["SECTIONID"] = "";

            if (Request.QueryString["REVISONID"] != null && Request.QueryString["REVISONID"].ToString() != "")
                ViewState["REVISONID"] = Request.QueryString["REVISONID"].ToString();
            else
                ViewState["REVISONID"] = "";

            GetData();
        }
    }

    private void GetData()
    {
        if (General.GetNullableGuid(ViewState["SECTIONID"].ToString()) != null)
        {
            DataSet ds = PhoenixDocumentManagementDocumentSection.DocumentSectionNewEdit(new Guid(ViewState["SECTIONID"].ToString()));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string strResult = HttpUtility.HtmlDecode(dr["FLDCURRENTCONTENT"].ToString());
                ucEditor.Content = strResult;
                ViewState["dtkey"] = dr["FLDDTKEY"].ToString();

                MenuSave.Title = MenuSave.Title + " /" + dr["FLDSECTIONDETAILS"].ToString();
            }
        }
    }

    private void GetRevisionData()
    {
        if (General.GetNullableGuid(ViewState["REVISONID"].ToString()) != null)
        {
            DataSet ds = PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonEdit(new Guid(ViewState["REVISONID"].ToString()));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string strResult = HttpUtility.HtmlDecode(dr["FLDTEXT"].ToString());
                ucEditor.Content = null;
                ucEditor.Content = strResult;
                ViewState["dtkey"] = dr["FLDDTKEY"].ToString();

                MenuSave.Title = MenuSave.Title + " /" + dr["FLDSECTIONDETAILS"].ToString();
            }
        }
    }

    protected void MenuSave_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string strContent = ucEditor.Content;

                if (!IsValidRevison(
                          ViewState["DOCUMENTID"].ToString()
                        , ViewState["SECTIONID"].ToString()
                        , strContent)
                    )
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixDocumentManagementDocumentSection.DocumentSectionNewContentUpdate(
                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , strContent
                                , new Guid(ViewState["SECTIONID"].ToString())
                                , new Guid(ViewState["DOCUMENTID"].ToString())
                        );
                ucStatus.Text = "Changes are updated.";

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void btnInsertPic_Click(object sender, EventArgs e)
    {
        try
        {
            HttpFileCollection filecolln = Request.Files;
            if (filecolln.Count > 0 && ucEditor.ActiveMode == AjaxControlToolkit.HTMLEditor.ActiveModeType.Design)
            {
                Guid gFileName = PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.DOCUMENTMANAGEMENT, null, ".jpg,.png,.gif");
                DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["dtkey"].ToString()));
                DataRow[] dr = dt.Select("FLDDTKEY = '" + gFileName.ToString() + "'");
                if (dr.Length > 0)
                {
                    string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                    if (!path.EndsWith("/"))
                        path = path + "/";

                    ucEditor.Content = ucEditor.Content + "<img src=\"" + "../Common/Download.aspx?dtkey=" + dr[0]["FLDDTKEY"].ToString() + "\" />";
                    //ucEditor.Content = ucEditor.Content + "<img src=\"" + ".." + "/attachments/" + dr[0]["FLDFILEPATH"].ToString() + "\" />";
                }

            }
            else
            {
                ucError.Text = Request.Files.Count > 0 ? "You are not in design mode" : "No Picture selected.";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidRevison(string documentid, string sectionid, string content)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(documentid) == null)
            ucError.ErrorMessage = "Document is not selected.";

        if (General.GetNullableGuid(sectionid) == null)
            ucError.ErrorMessage = "Section is not selected.";

        if (General.GetNullableString(content) == null)
            ucError.ErrorMessage = "Content is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        GetData();
    }
}
