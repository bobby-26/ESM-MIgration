using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;

public partial class Inspection_InspectionPIScopeAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();

        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        Tabstripspiaddmenu.MenuList = toolbargrid.Show();
    }
    protected void piaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string scopecode = General.GetNullableString(Radpiscopecodeentry.Text);
                string scope = General.GetNullableString(Radpiscopenameentry.Text);

                if (!IsValidShippingPIscopeDetails(scopecode, scope))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKPI.PIScopeInsert(rowusercode, scopecode, scope);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private bool IsValidShippingPIscopeDetails(string scopecode, string scope)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (scopecode == null)
        {
            ucError.ErrorMessage = "Scope Code.";
        }
        if (scope == null)
        {
            ucError.ErrorMessage = "Scope Name.";
        }

        return (!ucError.IsError);
    }
}