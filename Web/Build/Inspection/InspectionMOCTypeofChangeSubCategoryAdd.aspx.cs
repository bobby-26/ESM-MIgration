using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionMOCTypeofChangeSubCategoryAdd : PhoenixBasePage
{  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "Add", ToolBarDirection.Right);

            MenuMOCSubCategoryAdd.AccessRights = this.ViewState;
            MenuMOCSubCategoryAdd.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {   
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                BindMOCCategory();
            }
            //BindData();
            MenuMOCSubCategoryAdd.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void MenuMOCSubCategoryAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCountry(ddlCategory.SelectedValue
                   , txtCodeAdd.Text
                   , txtSubCategoryAdd.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionMOC.MOCSubCategoryProposalApprovalInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                 , txtSubCategoryAdd.Text
                                                                 , txtCodeAdd.Text
                                                                 , General.GetNullableGuid(ddlCategory.SelectedValue)
                                                                 , General.GetNullableGuid(ddlProposerRoleAdd.SelectedValue)
                                                                 , General.GetNullableGuid(ddlTempApproverRoleAdd.SelectedValue)
                                                                 , General.GetNullableGuid(ddlPermanantApproverRoleAdd.SelectedValue)
                                                                 , General.GetNullableGuid(ddlresponsiblepersonadd.SelectedValue)                                      
                                                                 );
            }
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    protected void BindMOCCategory()
    {
        string category = Request.QueryString["categoryid"];

        ddlCategory.DataSource = PhoenixInspectionMOCCategory.MOCCategoryList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlCategory.DataTextField = "FLDMOCCATEGORYNAME";
        ddlCategory.DataValueField = "FLDMOCCATEGORYID";
        ddlCategory.DataBind();
        ddlCategory.SelectedValue = category;
        ddlCategory.Enabled = false;
        
            ddlProposerRoleAdd.DataSource = PhoenixInspectionMOCApproverRole.MOCApproverRoleList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            ddlProposerRoleAdd.DataBind();
            ddlProposerRoleAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

            ddlTempApproverRoleAdd.DataSource = PhoenixInspectionMOCApproverRole.MOCApproverRoleList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            ddlTempApproverRoleAdd.DataBind();
            ddlTempApproverRoleAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

            ddlPermanantApproverRoleAdd.DataSource = PhoenixInspectionMOCApproverRole.MOCApproverRoleList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            ddlPermanantApproverRoleAdd.DataBind();
            ddlPermanantApproverRoleAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        
            ddlresponsiblepersonadd.DataSource = PhoenixInspectionMOCApproverRole.MOCApproverRoleList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            ddlresponsiblepersonadd.DataBind();
        ddlresponsiblepersonadd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
    protected void BindData()
    {

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
}