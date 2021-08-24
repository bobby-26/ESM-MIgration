using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;


public partial class InspectionMOCRoleConfigurationAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Save", "Update", ToolBarDirection.Right);
            
            MenuMOCRoleConfigurationAdd.AccessRights = this.ViewState;
            MenuMOCRoleConfigurationAdd.MenuList = toolbar.Show();

            if (!IsPostBack)
            {   
                BindMOCRoleConfig();
            }
            MenuMOCRoleConfigurationAdd.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuMOCRoleConfigurationAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("UPDATE") )
                {
                if (!IsValidMOCCategory(txtShortCodeEdit.Text,
                                    txtRoleEdit.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionMOCApproverRole.MOCApproverRoleInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               , txtRoleEdit.Text
                                                                , txtShortCodeEdit.Text);
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

    private void BindData()
    {
    }

    protected void BindMOCRoleConfig()
    {
        
    }
    private bool IsValidMOCCategory(string shortcode, string category)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (shortcode.Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (category.Trim().Equals(""))
            ucError.ErrorMessage = "Role is required.";

        return (!ucError.IsError);
    }
}