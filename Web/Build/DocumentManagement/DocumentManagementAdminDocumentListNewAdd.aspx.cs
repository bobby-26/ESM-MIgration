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


public partial class DocumentManagement_DocumentManagementAdminDocumentListNewAdd : PhoenixBasePage
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
            ucCompany.Enabled = false;
            ucCompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();

            if (Request.QueryString["pageno"] != null && Request.QueryString["pageno"].ToString() != "")
                ViewState["pageno"] = Request.QueryString["pageno"].ToString();
            else
                ViewState["pageno"] = "";

            if (Request.QueryString["pagesize"] != null && Request.QueryString["pagesize"].ToString() != "")
                ViewState["pagesize"] = Request.QueryString["pagesize"].ToString();
            else
                ViewState["pagesize"] = "";

            btnShowCategory.Attributes.Add("onclick", "return showPickList('spnPickListCategory', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDocumentCategory.aspx', true); ");
        }
    }

    private void InsertDocument(string documentname, string categorid, int? activyn, int? documenttype, int serialnumber, int? companyid, int? WholeManualRead)
    {
        PhoenixDocumentManagementDocument.DocumentInsert(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , documentname
            , new Guid(categorid)
            , activyn
            , documenttype
            , serialnumber
            , companyid
            , WholeManualRead
            );
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
                    ((UserControlMaskNumber)FindControl("txtSequenceNumberAdd")).Text
                     , General.GetNullableString(txtDocumentNameAdd.Text)
                     , General.GetNullableString(txtCategoryidAdd.Text)
                     , General.GetNullableInteger(ucCompany.SelectedCompany)
                     ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertDocument(
                    General.GetNullableString(txtDocumentNameAdd.Text)
                    , General.GetNullableString(txtCategoryidAdd.Text)
                    , chkActiveYNAdd.Checked == true ? 1 : 0
                    , 0
                    , int.Parse(((UserControlMaskNumber)FindControl("txtSequenceNumberAdd")).Text)
                    , General.GetNullableInteger(ucCompany.SelectedCompany)
                    , cbWholeManualRead.Checked == true ? 1 : 0
                );
                ucStatus.Text = "Document is added.";

                txtSequenceNumberAdd.Text = "";
                txtDocumentNameAdd.Text = "";
                txtCategoryidAdd.Text = "";
                txtCategory.Text = "";
                cbWholeManualRead.Checked = false;
                ViewState["DOCUMENTID"] = "";
            }

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentListNew.aspx?pageno=" + ViewState["pageno"].ToString()+ "&pagesize="+ViewState["pagesize"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
