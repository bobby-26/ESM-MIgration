using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;
public partial class VesselAccountsRHConfigurationMain : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
           
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Daily", "DAILYSETTING");
            toolbarmain.AddButton("Fixed", "FIXEDSETTING");
            MenuFormRH.AccessRights = this.ViewState;
            MenuFormRH.MenuList = toolbarmain.Show();
            MenuFormRH.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsRHWorkCalendarShip.aspx";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuFormRH_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIXEDSETTING"))
            {
                MenuFormRH.SelectedMenuIndex = 1;
                ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsRHConfiguration.aspx";
            }
            else if (CommandName.ToUpper().Equals("DAILYSETTING"))
            {
                MenuFormRH.SelectedMenuIndex = 0;
                ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsRHWorkCalendarShip.aspx";
            }
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "resizeScript", "resize();", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
