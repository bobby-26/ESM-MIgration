using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Common;
using System.Collections;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Integration;
using System.Text;
using SouthNests.Phoenix.Dashboard;

public partial class DocumentManagement_DocumentManagementAdminDocumentListNewUpdate : PhoenixBasePage
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
            ViewState["Revisionid"] = string.Empty;
            ucCompanyEdit.Enabled = false;
            ucCompanyEdit.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
            btnShowCategory.Attributes.Add("onclick", "return showPickList('spnPickListCategory', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDocumentCategory.aspx', true); ");
            if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != string.Empty)
            {
                ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
                DocumentEdit();
            }
            else
                ViewState["DOCUMENTID"] = ""; 

            if (Request.QueryString["Revisionid"] != null && Request.QueryString["Revisionid"].ToString() != string.Empty)
            {
                ViewState["Revisionid"] = Request.QueryString["Revisionid"].ToString();
            }

            if (Request.QueryString["pageno"] != null)
                ViewState["pageno"] = Request.QueryString["pageno"].ToString();

            if (Request.QueryString["pagesize"] != null && Request.QueryString["pagesize"].ToString() != "")
                ViewState["pagesize"] = Request.QueryString["pagesize"].ToString();
            else
                ViewState["pagesize"] = "";

        }
    }

    protected void MenuCommentsEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDocument(
                     ((UserControlMaskNumber)FindControl("txtSequenceNumberEdit")).Text,
                     General.GetNullableString(txtDocumentNameEdit.Text),
                    General.GetNullableString(txtCategoryidEdit.Text),
                      General.GetNullableInteger(ucCompanyEdit.SelectedCompany)
                     ))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateDocument(
                     General.GetNullableString(txtDocumentNameEdit.Text)
                    , General.GetNullableString(txtCategoryidEdit.Text)
                    , chkActiveYNEdit.Checked == true ? 1 : 0
                    , ViewState["DOCUMENTID"].ToString()
                    , 0
                    , int.Parse(((UserControlMaskNumber)FindControl("txtSequenceNumberEdit")).Text)
                    , General.GetNullableInteger(ucCompanyEdit.SelectedCompany)
                    , General.GetNullableDateTime(ucPublishedDateEdit.Text)
                    , ViewState["Revisionid"].ToString()
                    , cbWholeReadyn.Checked == true ? 1 : 0
                 );

                ucStatus.Text = "Document details updated.";
            }

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentListNew.aspx?pageno=" + ViewState["pageno"].ToString()+"&pagesize="+ViewState["pagesize"].ToString());
            }
        }
        catch (Exception ex)
        {
            //ucError.ErrorMessage = ex.Source;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void DocumentEdit()
    {
        if (General.GetNullableGuid(ViewState["DOCUMENTID"].ToString()) != null)
        {
            DataSet ds = PhoenixDocumentManagementDocument.DocumentEdit(new Guid(ViewState["DOCUMENTID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtSequenceNumberEdit.Text = dr["FLDSERIALNUMBER"].ToString();
                txtDocumentNameEdit.Text = dr["FLDDOCUMENTNAME"].ToString();
                //chkActiveYNEdit.Text = dr["FLDACTIVEYN"].ToString();
                txtCategoryidEdit.Text = dr["FLDCATEGORYID"].ToString();
                ucCompanyEdit.SelectedCompany = dr["FLDCOMPANYID"].ToString();
                ucPublishedDateEdit.Text = dr["FLDAPPROVEDDATE"].ToString();
                txtCategory.Text = dr["FLDCATEGORYNAME"].ToString();
                if (ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1")
                {
                    chkActiveYNEdit.Checked = true;
                }
                else
                {
                    chkActiveYNEdit.Checked = false;
                }
                if (ds.Tables[0].Rows[0]["FLDWHOLEMANUALREADYN"].ToString() == "1")
                {
                    cbWholeReadyn.Checked = true;
                }
                else
                {
                    cbWholeReadyn.Checked = false;
                }
            }
        }
    }
    private void UpdateDocument(string documentname, string categorid, int? activyn, string documentid, int? documenttype, int serialnumber, int? companyid, DateTime? publisheddate, string revisionid, int? Wholemanualreadyn)
    {

        PhoenixDocumentManagementDocument.DocumentUpdate(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , documentname
            , new Guid(categorid)
            , activyn
            , new Guid(documentid)
            , documenttype
            , serialnumber
            , companyid
            , publisheddate
            , General.GetNullableGuid(revisionid.ToString())
            , Wholemanualreadyn);
    }
    private bool IsValidDocument(string serialnumber, string documentname, string categoryid, int? companyid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(serialnumber) == null)
            ucError.ErrorMessage = "Serial Number is required.";

        if (General.GetNullableString(documentname) == null)
            ucError.ErrorMessage = "Document name is required.";

        if (General.GetNullableGuid(categoryid) == null)
            ucError.ErrorMessage = "Document category is required.";

        if (General.GetNullableInteger(companyid.ToString()) == null)
            ucError.ErrorMessage = "Company is required.";

        return (!ucError.IsError);
    }

    protected void txtComponentId_TextChanged(object sender, EventArgs e)
    {
        //BindJobs(txtComponentId.Text);
    }
}
