using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using System.Data;
using Telerik.Web.UI;
public partial class Crew_CrewSignOffReportSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Vesselwise", "VESSEL", ToolBarDirection.Right);
        toolbar.AddButton("Reasonwise", "REASON", ToolBarDirection.Right);
        toolbar.AddButton("Rankwise", "RANK", ToolBarDirection.Right);
        toolbar.AddButton("POEA Format", "POEAF", ToolBarDirection.Right);
        toolbar.AddButton("Managerwise", "MANAGER", ToolBarDirection.Right);

        MenuCrewSelection.MenuList = toolbar.Show();

        if (!IsPostBack)
        {

        }
        ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOffManagerWise.aspx";
        MenuCrewSelection.SelectedMenuIndex = 4;

    }
    protected void MenuCrewSelectionType_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("MANAGER"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOffManagerWise.aspx";
            MenuCrewSelection.SelectedMenuIndex = 4;
        }
        else if (CommandName.ToUpper().Equals("RANK"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOffRankWise.aspx";
            MenuCrewSelection.SelectedMenuIndex = 2;
        }
        else if (CommandName.ToUpper().Equals("VESSEL"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOffVesselWise.aspx";
            MenuCrewSelection.SelectedMenuIndex = 0;
        }
        else if (CommandName.ToUpper().Equals("REASON"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOffReasonWise.aspx";
            MenuCrewSelection.SelectedMenuIndex = 1;
        }
        else if (CommandName.ToUpper().Equals("POEAF"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOffPOEAFormat.aspx";
            MenuCrewSelection.SelectedMenuIndex = 3;
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOnManagerWise.aspx";
            MenuCrewSelection.SelectedMenuIndex = 4;
        }
    }
}
