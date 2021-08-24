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

public partial class DocumentManagementFMSShipboardFormsAttachmentUpload : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            ViewState["DWPAID"] = string.Empty;
            if (Request.QueryString["dwpaid"] != null)
            {
                ViewState["DWPAID"] = Request.QueryString["dwpaid"];
            }
            ViewState["WOID"] = string.Empty;
            if (Request.QueryString["woid"] != null)
            {
                ViewState["WOID"] = Request.QueryString["woid"];
            }
            if (Request.QueryString["FORMID"] != null)
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
            }
            else
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            if (Request.QueryString["DocSource"] != null)
            {
                ViewState["DocSource"] = Request.QueryString["DocSource"];
            }
            //BindMapping();
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
                    Guid? formrevisionid = Guid.Empty;

                    if (ViewState["FORMID"].ToString() != null)
                    {
                        PhoenixDocumentManagementForm.FMSShipboardFormUploadInsert(new Guid(ViewState["FORMID"].ToString())
                        , ref formrevisionid
                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , General.GetNullableGuid(ViewState["DWPAID"].ToString())
                        , General.GetNullableGuid(ViewState["WOID"].ToString()));

                        PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(formrevisionid.ToString()), PhoenixModule.DOCUMENTMANAGEMENT, null, "", string.Empty, "FMSSHIPBOARDUPLOAD", PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                        // PhoenixDocumentManagementForm.FormDueUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid( ViewState["DWPAID"].ToString()));
                        if (ViewState["DocSource"] != null)
                            PhoenixCommonFileAttachment.AttachmentExist(PhoenixModule.DOCUMENTMANAGEMENT, new Guid(formrevisionid.ToString()), ViewState["DocSource"].ToString(), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

                        lblMessage.Text = "Form is uploaded.";
                        lblMessage.ForeColor = System.Drawing.Color.Blue;
                        lblMessage.Visible = true;
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "CloseUrlModelWindow()", true);
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
