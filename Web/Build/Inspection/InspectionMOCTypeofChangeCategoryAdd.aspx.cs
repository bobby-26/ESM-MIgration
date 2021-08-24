using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionMOCTypeofChangeCategoryAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Save", "Update", ToolBarDirection.Right);

        MenuRegistersTypeofChangeCategoryAdd.AccessRights = this.ViewState;
        MenuRegistersTypeofChangeCategoryAdd.MenuList = toolbar.Show();
        if (!IsPostBack)
        {   
            BindMOC();
        }
        MenuRegistersTypeofChangeCategoryAdd.SelectedMenuIndex = 0;
    }

    protected void MenuRegistersTypeofChangeCategoryAdd_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("UPDATE"))
        {   
            if (!IsValidMOCCategory(txtcode.Text,txtname.Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixInspectionMOCCategory.MOCCategoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , txtname.Text
                                                            , txtcode.Text);
        }

        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }
    protected void BindMOC()
    {
    }

    private bool IsValidMOCCategory(string shortcode, string category)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (shortcode.Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (category.Trim().Equals(""))
            ucError.ErrorMessage = "Category is required.";

        return (!ucError.IsError);
    }


}