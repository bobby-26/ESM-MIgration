using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class Crew_CrewReportExpiringMissingSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Missing Documents", "MISSING", ToolBarDirection.Right);
        toolbar.AddButton("Expiring Documents", "EXPIRING", ToolBarDirection.Right);

        MenuCrewSelection.AccessRights = this.ViewState;
        MenuCrewSelection.MenuList = toolbar.Show();

        if (!IsPostBack)
        {

        }
        ifMoreInfo.Attributes["src"] = "../Crew/CrewReportExpiringSearch.aspx";
        MenuCrewSelection.SelectedMenuIndex = 1;

    }
    protected void MenuCrewSelectionType_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXPIRING"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportExpiringSearch.aspx";
            MenuCrewSelection.SelectedMenuIndex = 1;
        }
        else if (CommandName.ToUpper().Equals("MISSING"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportMissingSearch.aspx";
            MenuCrewSelection.SelectedMenuIndex = 0;
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportExpiringSearch.aspx";
            MenuCrewSelection.SelectedMenuIndex = 1;
        }
    }
}
