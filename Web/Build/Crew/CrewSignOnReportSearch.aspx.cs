using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class Crew_CrewSignOnReportSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Vesselwise", "VESSEL", ToolBarDirection.Right);
        toolbar.AddButton("Rankwise", "RANK", ToolBarDirection.Right);
        toolbar.AddButton("Managerwise", "MANAGER", ToolBarDirection.Right);


        MenuCrewSelection.MenuList = toolbar.Show();

        if (!IsPostBack)
        {

        }
        ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOnManagerWise.aspx";
        MenuCrewSelection.SelectedMenuIndex = 2;

    }
    protected void MenuCrewSelectionType_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("MANAGER"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOnManagerwise.aspx";
            MenuCrewSelection.SelectedMenuIndex = 2;
        }
        else if (CommandName.ToUpper().Equals("RANK"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOnRankwise.aspx";
            MenuCrewSelection.SelectedMenuIndex = 1;
        }
        else if (CommandName.ToUpper().Equals("VESSEL"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOnVesselwise.aspx";
            MenuCrewSelection.SelectedMenuIndex = 0;
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOnManagerwise.aspx";
            MenuCrewSelection.SelectedMenuIndex = 2;
        }
    }
}
