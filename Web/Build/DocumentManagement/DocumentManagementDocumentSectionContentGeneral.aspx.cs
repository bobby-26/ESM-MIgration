using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;
using System.Text.RegularExpressions;

public partial class DocumentManagementDocumentSectionContentGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        //toolbar.AddButton("Cancel", "CLOSE");   

        MenuSave.AccessRights = this.ViewState;
        MenuSave.MenuList = toolbar.Show();

        if (!IsPostBack)
        {


            if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
            {
                ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
                GetDocumentName();
            }
            else
                ViewState["DOCUMENTID"] = "";

            if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != "")
                ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();
            else
                ViewState["SECTIONID"] = "";

            if (Request.QueryString["REVISONID"] != null && Request.QueryString["REVISONID"].ToString() != "")
            {
                ViewState["REVISONID"] = Request.QueryString["REVISONID"].ToString();
                //GetRevisionData();
            }
            else
            {
                ViewState["REVISONID"] = "";
                //GetData(); 
            }
            GetData();
            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "")
                ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();
            else
                ViewState["callfrom"] = "";

        }




    }

    private void GetDocumentName()
    {
        if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementDocument.DocumentEdit(new Guid(ViewState["DOCUMENTID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                MenuSave.Title = dr["FLDDOCUMENTNAME"].ToString();
            }
        }
    }

    private void GetData()
    {
        if (General.GetNullableGuid(ViewState["SECTIONID"].ToString()) != null)
        {
            DataSet ds = PhoenixDocumentManagementDocumentSection.DocumentSectionEdit(new Guid(ViewState["SECTIONID"].ToString()));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                string strResult = HttpUtility.HtmlDecode(dr["FLDCURRENTCONTENT"].ToString());

                //string str = @"<object id=""ieooui"" classid=""clsid:38481807-CA0E-42D2-BF39-B33AF135CC4D"" />";
                //strResult = strResult.Replace(str, "");
                //str = @"</object>";
                //strResult = strResult.Replace(str, "");

                string repl = "../Common/Download.aspx?dtkey=";

                string find2 = "../attachments/DOCUMENTMANAGEMENT/";

                string findOffshore = "https://apps.southnests.com/PhoenixOffshore/attachments/DOCUMENTMANAGEMENT/";

                string findOffshore2 = "http://119.81.76.123:80/PhoenixOffshore/attachments/DOCUMENTMANAGEMENT/";

                string findOffshore3 = "http://119.81.76.123/PhoenixOffshore/attachments/DOCUMENTMANAGEMENT/";

                string find = "https://apps.southnests.com/Phoenix/attachments/DOCUMENTMANAGEMENT/";

                string find3 = "http://119.81.76.123:80/Phoenix/attachments/DOCUMENTMANAGEMENT/";

                string find4 = "http://119.81.76.123/Phoenix/attachments/DOCUMENTMANAGEMENT/";

                if (strResult.Contains(find2))
                {
                    strResult = strResult.Replace(find2, repl);
                }


                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                {

                    if (strResult.Contains(findOffshore))
                    {
                        strResult = strResult.Replace(findOffshore, repl);
                    }

                    if (strResult.Contains(findOffshore2))
                    {
                        strResult = strResult.Replace(findOffshore, repl);
                    }

                    if (strResult.Contains(findOffshore3))
                    {
                        strResult = strResult.Replace(findOffshore3, repl);
                    }
                }
                else
                {
                    if (strResult.Contains(find))
                    {
                        strResult = strResult.Replace(find, repl);
                    }

                    if (strResult.Contains(find3))
                    {
                        strResult = strResult.Replace(find3, repl);
                    }

                    if (strResult.Contains(find4))
                    {
                        strResult = strResult.Replace(find4, repl);
                    }

                }

                strResult = Regex.Replace(strResult, "\\d+/\\d+/", "");

                strResult = strResult.Replace(".jpg", "");
                strResult = strResult.Replace(".png", "");
                strResult = strResult.Replace(".tif", "");

                ucEditor.Content = strResult;
                ViewState["dtkey"] = dr["FLDDTKEY"].ToString();

                MenuSave.Title = MenuSave.Title + " /" + dr["FLDSECTIONDETAILS"].ToString();
                //ucTitle.Text = ucTitle.Text + " /" + dr["FLDSECTIONDETAILS"].ToString() + " ( " + dr["FLDCURRENTREVISION"].ToString() + ")";
            }
        }
    }

    private void GetRevisionData()
    {
        if (General.GetNullableGuid(ViewState["REVISONID"].ToString()) != null)
        {
            DataSet ds = PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonsEdit(new Guid(ViewState["REVISONID"].ToString()));
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
                if (General.GetNullableGuid(ViewState["REVISONID"].ToString()) == null)
                {
                    Guid? revisonid = Guid.Empty;
                    Guid? revisiondtkey = Guid.Empty;
                    Guid? sectionid = Guid.Empty;


                    PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , strContent
                        , new Guid(ViewState["SECTIONID"].ToString())
                        , new Guid(ViewState["DOCUMENTID"].ToString())
                        , ref revisonid
                        , ref revisiondtkey
                        );

                    ViewState["REVISONID"] = revisonid;
                    ViewState["REVISONDTKEY"] = revisiondtkey;

                    ucStatus.Text = "Changes are updated.";

                    string scriptRefreshDontClose = "";
                    scriptRefreshDontClose += "<script language='javaScript' id='View'>" + "\n";
                    scriptRefreshDontClose += "fnReloadList('VesselOtherCompany', 'refreshiframe', 'keepOpen');";
                    scriptRefreshDontClose += "</script>" + "\n";

                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "View", scriptRefreshDontClose, false);                   
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AddressAddNew", scriptRefreshDontClose, false);
                }
                else
                {
                    PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonUpdate(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , strContent
                            , new Guid(ViewState["SECTIONID"].ToString())
                            , new Guid(ViewState["DOCUMENTID"].ToString())
                            , new Guid(ViewState["REVISONID"].ToString())
                            );

                    ucStatus.Text = "Changes are updated.";

                    string scriptRefreshDontClose = "";
                    scriptRefreshDontClose += "<script language='javaScript' id='View'>" + "\n";
                    scriptRefreshDontClose += "fnReloadList('VesselOtherCompany', 'refreshiframe', 'keepOpen');";
                    scriptRefreshDontClose += "</script>" + "\n";

                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "View", scriptRefreshDontClose, false);

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AddressAddNew", scriptRefreshDontClose, false);
                }
                GetData();
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
                if (General.GetNullableGuid(ViewState["REVISONID"].ToString()) == null)
                {
                    string strContent = ucEditor.Content;
                    Guid? revisonid = Guid.Empty;
                    Guid? revisiondtkey = Guid.Empty;
                    PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonInsert(
                       PhoenixSecurityContext.CurrentSecurityContext.UserCode
                       , strContent
                       , new Guid(ViewState["SECTIONID"].ToString())
                       , new Guid(ViewState["DOCUMENTID"].ToString())
                       , ref revisonid
                       , ref revisiondtkey
                       );


                    ViewState["REVISONID"] = revisonid;
                    ViewState["REVISONDTKEY"] = revisiondtkey;
                }

                Guid gFileName = PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["REVISONDTKEY"].ToString()), PhoenixModule.DOCUMENTMANAGEMENT, null, ".jpg,.png,.gif", string.Empty, "UPLOADEDDOCIMAGE");
                DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["REVISONDTKEY"].ToString()));
                DataRow[] dr = dt.Select("FLDDTKEY = '" + gFileName.ToString() + "'");
                if (dr.Length > 0)
                {
                    string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                    if (!path.EndsWith("/"))
                        path = path + "/";

                    ucEditor.Content = ucEditor.Content + "<img src=\"" + "../Common/Download.aspx?dtkey=" + dr[0]["FLDDTKEY"].ToString() + "\" />";
                    //ucEditor.Content = ucEditor.Content + "<img src=\"../Common/Download.aspx?dtkey=" + dta.Rows[0]["FLDDTKEY"].ToString(); "\" />";

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
        ViewState["PAGENUMBER"] = 1;
        GetData();
    }

}
