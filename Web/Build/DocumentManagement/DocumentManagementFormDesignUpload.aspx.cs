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
using Telerik.Web.UI;


public partial class DocumentManagement_DocumentManagementFormDesignUpload : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuCommentsEdit.AccessRights = this.ViewState;
        MenuCommentsEdit.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["attachmentdtkey"] = "";
            ViewState["path"] = "";
            ViewState["FORMID"] = "";
            ViewState["formrevisionid"] = "";
            ViewState["formrevisiondtkey"] = "";
            ViewState["formrevisionapprovedyn"] = "";

            btnShowCategory.Attributes.Add("onclick", "return showPickList('spnPickListCategory', 'codehelp1', '', '../Common/CommonPickListDesignedForm.aspx', true); ");

            if (Request.QueryString["FORMID"] != null && Request.QueryString["FORMID"].ToString() != string.Empty)
            {
                ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
            }
            else
                ViewState["FORMID"] = "";

            if (Request.QueryString["formrevisiondtkey"] != null && Request.QueryString["formrevisiondtkey"].ToString() != string.Empty)
            {
                ViewState["formrevisiondtkey"] = Request.QueryString["formrevisiondtkey"].ToString();
            }
            else
                ViewState["formrevisiondtkey"] = "";

            FormEdit();

            if (Request.QueryString["formrevisionid"] != null && Request.QueryString["formrevisionid"].ToString() != string.Empty)
            {
                ViewState["formrevisionid"] = Request.QueryString["formrevisionid"].ToString();
                DataSet ds = PhoenixDocumentManagementForm.FormRevisionEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , General.GetNullableGuid(ViewState["FORMID"].ToString())
                                            , General.GetNullableGuid(Request.QueryString["formrevisionid"].ToString()));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["formrevisiondtkey"] = dr["FLDDTKEY"].ToString();
                    ViewState["formrevisionapprovedyn"] = dr["FLDAPPROVEDYN"].ToString();
                }
            }
            else
                ViewState["formrevisionid"] = "";

        }

    }

    protected void FormEdit()
    {
        if (General.GetNullableGuid(ViewState["FORMID"].ToString()) != null)
        {
            DataSet dt = PhoenixDocumentManagementForm.FormDesignEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["FORMID"].ToString()));
            if (dt.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dt.Tables[0].Rows[0];
                txtFormNo.Text = dr["FLDFORMNO"].ToString();
                txtFormName.Text = dr["FLDCAPTION"].ToString();
                txtFormDesignid.Text = dr["FLDFORMBUILDERID"].ToString();
                ViewState["DESGINFORMID"] = dr["FLDFORMBUILDERID"].ToString();
                txtFormDesign.Text = dr["FLDFORMNAME"].ToString();
            }
        }
    }

    protected void MenuCommentsEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (ViewState["FORMID"] != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixDocumentManagementForm.FormDesignUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , General.GetNullableGuid(ViewState["FORMID"].ToString())
                                                                                        , General.GetNullableGuid(txtFormDesignid.Text)
                                                                             );
                    Guid? formrevisionid = Guid.Empty;

                    if (General.GetNullableGuid(ViewState["formrevisiondtkey"].ToString()) == null || General.GetNullableInteger(ViewState["formrevisionapprovedyn"].ToString()) == 1)
                    {
                        PhoenixDocumentManagementForm.FormDesignRevisionInsert(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , new Guid(ViewState["FORMID"].ToString())
                            , null
                            , ref formrevisionid
                            , General.GetNullableGuid(txtFormDesignid.Text));

                        DataSet ds = PhoenixDocumentManagementForm.FormRevisionEdit(
                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , General.GetNullableGuid(ViewState["FORMID"].ToString())
                                                , formrevisionid);
                        ViewState["formrevisionid"] = formrevisionid.ToString();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];
                            ViewState["formrevisiondtkey"] = dr["FLDDTKEY"].ToString();
                            ViewState["formrevisionapprovedyn"] = dr["FLDAPPROVEDYN"].ToString();
                        }
                    }

                    FormEdit();
                }
            }

            ucStatus.Text = "Saved successfully.";
            //String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFormList.aspx?FORMID=" + ViewState["FORMID"]);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }



    }


}