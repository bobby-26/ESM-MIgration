using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;

public partial class Inspection_InspectionSPIAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);

        Tabstripspiaddmenu.MenuList = toolbargrid.Show();
    }

    protected void spiaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string id = General.GetNullableString(Radspiidentry.Text);
                string shortcode = General.GetNullableString(Radspishortcodeentry.Text);
                string title = General.GetNullableString(Radspinameentry.Text);
                string description = General.GetNullableString(Radspidescriptionentry.Text);
                if (!IsValidShippingSPIDetails(id, shortcode, title))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKSPI.SPIInsert(rowusercode, id, shortcode, title, description);


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

    private bool IsValidShippingSPIDetails(string id, string shortcode, string title)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (id == null)
        {
            ucError.ErrorMessage = "SPI ID.";
        }
        if (shortcode == null)
        {
            ucError.ErrorMessage = "SPI Short Code.";
        }
        if (title == null)
        {
            ucError.ErrorMessage = "SPI Title.";
        }
        return (!ucError.IsError);
    }

}