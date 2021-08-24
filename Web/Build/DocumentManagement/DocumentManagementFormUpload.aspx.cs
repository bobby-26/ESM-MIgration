using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Common;

public partial class DocumentManagementFormUpload : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["attachmentdtkey"] = "";
            ViewState["path"] = "";
            ViewState["formid"] = "";
            ViewState["formrevisionid"] = "";
            ViewState["formrevisiondtkey"] = "";
            ViewState["formrevisionapprovedyn"] = "";

            if (Request.QueryString["formid"] != null && Request.QueryString["formid"].ToString() != string.Empty)
            {
                ViewState["formid"] = Request.QueryString["formid"].ToString();
            }
            else
                ViewState["formid"] = "";

            if (Request.QueryString["formrevisiondtkey"] != null && Request.QueryString["formrevisiondtkey"].ToString() != string.Empty)
            {
                ViewState["formrevisiondtkey"] = Request.QueryString["formrevisiondtkey"].ToString();
                GetUploadFormDetails();
            }
            else
                ViewState["formrevisiondtkey"] = "";

            if (Request.QueryString["formrevisionid"] != null && Request.QueryString["formrevisionid"].ToString() != string.Empty)
            {
                ViewState["formrevisionid"] = Request.QueryString["formrevisionid"].ToString();
                DataSet ds = PhoenixDocumentManagementForm.FormRevisionEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , General.GetNullableGuid(ViewState["formid"].ToString())
                                            , General.GetNullableGuid(Request.QueryString["formrevisionid"].ToString()));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    //ViewState["formrevisiondtkey"] = dr["FLDDTKEY"].ToString();
                    ViewState["formrevisionapprovedyn"] = dr["FLDAPPROVEDYN"].ToString();
                }
            }
            else
                ViewState["formrevisionid"] = "";                        
            
            cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }

    protected void GetUploadFormDetails()
    {
        if (General.GetNullableGuid(ViewState["formrevisiondtkey"].ToString()) != null)
        {
            DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["formrevisiondtkey"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                lnkfilename.NavigateUrl = "../Common/download.aspx?dtkey=" + dr["FLDDTKEY"].ToString();
                //lnkfilename.NavigateUrl = Session["sitepath"] + "/attachments/" + dr["FLDFILEPATH"].ToString();
                lnkfilename.Text = dr["FLDFILENAME"].ToString();
                ViewState["path"] = dr["FLDFILEPATH"].ToString();
                ViewState["attachmentdtkey"] = dr["FLDDTKEY"].ToString();
            }
        }
    }

    protected void cmdUpload_Click(object sender, EventArgs e)
    {
        try 
        {
            if (Request.Files["txtFileUpload"].ContentLength > 0)
            {
                Guid? formrevisionid = Guid.Empty;                

                if (General.GetNullableGuid(ViewState["formrevisiondtkey"].ToString()) == null || General.GetNullableInteger(ViewState["formrevisionapprovedyn"].ToString()) == 1)
                {
                    PhoenixDocumentManagementForm.FormRevisionInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["formid"].ToString())
                        , null
                        , ref formrevisionid);

                    DataSet ds = PhoenixDocumentManagementForm.FormRevisionEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , General.GetNullableGuid(ViewState["formid"].ToString())
                                            , formrevisionid);
                    ViewState["formrevisionid"] = formrevisionid.ToString();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        ViewState["formrevisiondtkey"] = dr["FLDDTKEY"].ToString();
                        ViewState["formrevisionapprovedyn"] = dr["FLDAPPROVEDYN"].ToString();
                        ViewState["attachmentdtkey"] = "";
                        GetUploadFormDetails();
                    }
                }

                if (string.IsNullOrEmpty(ViewState["attachmentdtkey"].ToString()))
                {
                    PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["formrevisiondtkey"].ToString()), PhoenixModule.DOCUMENTMANAGEMENT, null, "", string.Empty, "UPLOADEDFORM");

                    lblMessage.Text = "Form is uploaded.";
                    lblMessage.ForeColor = System.Drawing.Color.Blue;
                    lblMessage.Visible = true;

                    ViewState["attachmentdtkey"] = "";
                    GetUploadFormDetails();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null, true);", true);
                }
                else
                {
                    PhoenixCommonFileAttachment.UpdateAttachment(Request.Files, new Guid(ViewState["attachmentdtkey"].ToString()), PhoenixModule.DOCUMENTMANAGEMENT, "");

                    lblMessage.Text = "Form is uploaded.";
                    lblMessage.ForeColor = System.Drawing.Color.Blue;
                    lblMessage.Visible = true;

                    ViewState["attachmentdtkey"] = "";
                    GetUploadFormDetails();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null, true);", true);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Visible = true;
        }
    }

    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        try 
        {
            if (General.GetNullableGuid(ViewState["attachmentdtkey"].ToString()) != null)
            {
                PhoenixCommonFileAttachment.AttachmentDelete(new Guid(ViewState["attachmentdtkey"].ToString()));
                //String path = MapPath("~/attachments/" + ViewState["path"].ToString());
                //System.IO.File.Delete(path);

                if (General.GetNullableGuid(ViewState["formrevisionid"].ToString()) != null)
                {
                    PhoenixDocumentManagementForm.FormRevisionDelete(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["formrevisionid"].ToString()));
                }

                lnkfilename.NavigateUrl = "";
                lnkfilename.Text = "View Uploaded Form";
                lblMessage.Text = "Form is deleted.";
                lblMessage.ForeColor = System.Drawing.Color.Blue;
                lblMessage.Visible = true;

                ViewState["formrevisiondtkey"] = "";
                ViewState["attachmentdtkey"] = "";
                GetUploadFormDetails();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null, true);", true);
            }
        }        
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Visible = true;
        }
    }

    protected void MenuLocation_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    }    
}
