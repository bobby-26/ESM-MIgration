using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionMOCTypeofChangeSubCategoryEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            txtCodeEdit.ReadOnly = true;
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Save", "Update", ToolBarDirection.Right);

            MenuMOCSubCategoryEdit.AccessRights = this.ViewState;
            MenuMOCSubCategoryEdit.MenuList = toolbar.Show();
            txtCategory.ReadOnly = true;

            if (!IsPostBack)
            {
                ViewState["mocsubcategoryid"] = "";

                if (Request.QueryString["mocsubcategoryid"] != null && Request.QueryString["mocsubcategoryid"].ToString() != string.Empty)
                    ViewState["mocsubcategoryid"] = Request.QueryString["mocsubcategoryid"].ToString();

                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

                BindMOC();
            }
            MenuMOCSubCategoryEdit.SelectedMenuIndex = 0;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void MenuMOCSubCategoryEdit_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("UPDATE") && Request.QueryString["mocsubcategoryid"] != null)
        {
            string subcategoryid = Request.QueryString["mocsubcategoryid"];

            if (!IsValidCountry(    lblcategory2.Text
                                 , txtCodeEdit.Text
                                 , txtSubCategoryEdit.Text))               
            {                                                              
                ucError.Visible = true;                                                                                                                          
                return;
            }

        }
        PhoenixInspectionMOC.MOCSubCategoryProposalApprovalUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , Guid.Parse(lblSubCategoryIdEdit.Text)
                                                        , txtSubCategoryEdit.Text
                                                        , txtCodeEdit.Text
                                                        , General.GetNullableGuid(lblcategory2.Text)
                                                        , General.GetNullableGuid(ddlProposerRoleEdit.SelectedValue)
                                                        , General.GetNullableGuid(ddlTempApproverRoleEdit.SelectedValue)
                                                        , General.GetNullableGuid(ddlPermanantApproverRoleEdit.SelectedValue)
                                                        , General.GetNullableGuid(ddlresponsiblepersonedit.SelectedValue));

        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private void BindData()
    {
    }
    private bool IsValidCountry(string category, string shortcode, string subcategory)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(category) == null)
            ucError.ErrorMessage = "Category is required.";

        if (shortcode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (subcategory.Trim().Equals(""))
            ucError.ErrorMessage = "SubCategory is required.";

        return (!ucError.IsError);
    }
    
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void BindMOC()
    {
        if (ViewState["mocsubcategoryid"] != null && ViewState["mocsubcategoryid"].ToString() != string.Empty)
        {
            DataSet dt = PhoenixInspectionMOCCategory.MOCSubCategoryMappingEdit(General.GetNullableGuid(ViewState["mocsubcategoryid"].ToString()));

            if (dt.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dt.Tables[0].Rows[0];
                txtCodeEdit.Text = dr["FLDSHORTCODE"].ToString();
                txtSubCategoryEdit.Text = dr["FLDMOCSUBCATEGORYNAME"].ToString();
                lblcategory2.Text = dr["FLDMOCCATEGORYID"].ToString();
                txtCategory.Text = dr["FLDMOCCATEGORYNAME"].ToString();
                lblSubCategoryIdEdit.Text = dr["FLDMOCSUBCATEGORYID"].ToString();
                lblProposerRole.Text = dr["FLDPROPOSERROLEID"].ToString();
                lblTempApproverRole.Text = dr["FLDTEMPORARYAPPROVERROLEID"].ToString();
                lblPermanantApproverRole.Text = dr["FLDPERMANENTAPPROVERROLEID"].ToString();
                lblresponsiblepersonrole.Text = dr["FLDRESPONSIBLEPERSONROLEID"].ToString();
            }
        }
       
        int a = PhoenixSecurityContext.CurrentSecurityContext.UserCode;

        ddlProposerRoleEdit.DataSource = PhoenixInspectionMOCApproverRole.MOCApproverRoleList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlProposerRoleEdit.DataBind();
        ddlProposerRoleEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlProposerRoleEdit.SelectedValue = lblProposerRole.Text;

        ddlTempApproverRoleEdit.DataSource = PhoenixInspectionMOCApproverRole.MOCApproverRoleList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlTempApproverRoleEdit.DataBind();
        ddlTempApproverRoleEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlTempApproverRoleEdit.SelectedValue = lblTempApproverRole.Text;

        ddlPermanantApproverRoleEdit.DataSource = PhoenixInspectionMOCApproverRole.MOCApproverRoleList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlPermanantApproverRoleEdit.DataBind();
        ddlPermanantApproverRoleEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlPermanantApproverRoleEdit.SelectedValue = lblPermanantApproverRole.Text;

        ddlresponsiblepersonedit.DataSource = PhoenixInspectionMOCApproverRole.MOCApproverRoleList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlresponsiblepersonedit.DataBind();
        ddlresponsiblepersonedit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlresponsiblepersonedit.SelectedValue = lblresponsiblepersonrole.Text;
    }
}