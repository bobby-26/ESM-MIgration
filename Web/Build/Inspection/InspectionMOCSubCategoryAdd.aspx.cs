using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionMOCSubCategoryAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Save", "Update", ToolBarDirection.Right);

        MenuRegistersSubCategoryAdd.AccessRights = this.ViewState;
        MenuRegistersSubCategoryAdd.MenuList = toolbar.Show();
        if (!IsPostBack)
        {  
            BindMOC();
        }
        MenuRegistersSubCategoryAdd.SelectedMenuIndex = 0;
    }
    protected void MenuRegistersSubCategoryAdd_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("UPDATE") )
        {
            if (!IsValidCountry(txtShortCodeEdit.Text, txtCategoryEdit.Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionMOC.MOCSubCategoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                       , txtCategoryEdit.Text
                                                                                       , txtShortCodeEdit.Text);
        }
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }
    private bool IsValidCountry(string shortcode, string category)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (shortcode.Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (category.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";
        
        return (!ucError.IsError);
    }
    protected void BindMOC()
    {  
    }
}