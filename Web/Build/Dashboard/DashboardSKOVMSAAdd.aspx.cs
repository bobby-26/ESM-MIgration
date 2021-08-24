using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;


public partial class Dashboard_DashboardSKOVMSAAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();

        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        Tabstripspiaddmenu.MenuList = toolbargrid.Show();
    }

   

    protected void Tabstripspiaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string id = General.GetNullableString(radtbidentry.Text);
                string shortcode = General.GetNullableString(radtbshortcodeentry.Text);
                string description = General.GetNullableString(radtbdescriptionentry.Text);
                if (!IsValidTMSAElementDetails(id, shortcode, description))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoemixDashboardSKOVMSA.TMSAElementInsert(rowusercode, id, shortcode,description);

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

    private bool IsValidTMSAElementDetails(string id, string shortcode , string Description)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (id == null)
        {
            ucError.ErrorMessage = "TMSA Element ID.";
        }
        if (shortcode == null)
        {
            ucError.ErrorMessage = "TMSA Element Short Code.";
        }
        if (Description == null)
        {
            ucError.ErrorMessage = "TMSA Element Description.";
        }

        return (!ucError.IsError);
    }
}