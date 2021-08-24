using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class DocumentManagementFMSDrawingAttachmentUpload : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            ViewState["VESSELID"] = "";
            if (Request.QueryString["VESSELID"] != null)
            {
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            }
            if (Request.QueryString["drawingid"] != null)
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                ViewState["DRAWINGID"] = Request.QueryString["drawingid"].ToString();
            }
            else
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            BindMapping();
            MenuFMSFileNo.AccessRights = this.ViewState;
            MenuFMSFileNo.MenuList = toolbar.Show();
            ViewState["DTKEY"] = null;

        }

    }

    protected void BindMapping()
    {
        DataSet ds = PhoenixRegisterFMSMail.EditFMSDrawingAttachments(new Guid(ViewState["DRAWINGID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            MenuFMSFileNo.Title = ds.Tables[0].Rows[0]["FLDSUBCATEGORYNAME"].ToString();
        }
    }

    protected void FMSFileNo_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                Guid? guid = Guid.Empty;

                if (!IsValidFileNo())
                {
                    ucError.Visible = true;
                    return;
                }

                if (Request.Files["txtFileUpload"].ContentLength > 0)
                {

                    if (ViewState["DRAWINGID"].ToString() != null)
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["DRAWINGID"].ToString()), PhoenixModule.DOCUMENTMANAGEMENT, null, "", string.Empty, "FMSDRAWINGUPLOAD", int.Parse(ViewState["VESSELID"].ToString()));

                        lblMessage.Text = "Form is uploaded.";
                        lblMessage.ForeColor = System.Drawing.Color.Blue;
                        lblMessage.Visible = true;
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('codehelp1', 'upload');", true);
                }
                //BindMenu();

            }
            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                // Response.Redirect("../Registers/RegistersFMSFileNoList.aspx");
                Response.Redirect("../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DRAWINGID"] + "&mod=" + PhoenixModule.DOCUMENTMANAGEMENT + "&type=FMSFILENO" + "&BYVESSEL=false&cmdname=FMSDRAWINGUPLOAD");
            }

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

        //String script = String.Format("javascript:fnReloadList('code1');");
        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void cmdUpload_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Visible = true;
        }
    }

    private bool IsValidFileNo()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Request.Files["txtFileUpload"].ContentLength == 0)
            ucError.ErrorMessage = "File upload is required.";

        return (!ucError.IsError);
    }
}
