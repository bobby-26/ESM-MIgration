using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;


public partial class VesselAccountsCrewAdmin :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ResetMenu();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void ResetMenu()
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Sign On Off List", "SIGNONOFFLIST");
            toolbar.AddButton("Missing SignOff", "MISSINGSIGNOFF");
            MenuCrewAdmin.AccessRights = this.ViewState;
            MenuCrewAdmin.MenuList = toolbar.Show();
            MenuCrewAdmin.SelectedMenuIndex = 0;

            ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsAdminSignOnOffList.aspx";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewAdmin_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SIGNONOFFLIST"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsAdminSignOnOffList.aspx";
            }
            else if (dce.CommandName.ToUpper().Equals("MISSINGSIGNOFF"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsMissingVesselSignOff.aspx";
            }
            ifMoreInfo.Attributes["src"] = ViewState["CURRENTTAB"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
}
