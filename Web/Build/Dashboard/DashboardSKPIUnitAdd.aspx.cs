using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;


public partial class Inspection_InspectionPIUnitAdd : PhoenixBasePage
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
                string unitcode = General.GetNullableString(Radpiunitcodeentry.Text);
                string unit = General.GetNullableString(Radpiunitnameentry.Text);

                if (!IsValidShippingPIUnitDetails(unitcode, unit))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKPI.PIUnitInsert(rowusercode, unitcode, unit);

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

    private bool IsValidShippingPIUnitDetails(string unitcode, string unit)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (unitcode == null)
        {
            ucError.ErrorMessage = "Unit Code.";
        }
        if (unit == null)
        {
            ucError.ErrorMessage = "Unit Name.";
        }

        return (!ucError.IsError);
    }
}