using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using System.Data;
using Telerik.Web.UI;
public partial class CrewSignOnOffFlagBasedVessel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            
            toolbar.AddButton("Flagged Vessels", "FLAGGEDVESSELS");
            toolbar.AddButton("All Vessels", "ALLVESSELS");
            MenuCrewSignOffVesselType.MenuList = toolbar.Show();
        }
        ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOnOffFlaggedVesselsReport.aspx";
        MenuCrewSignOffVesselType.SelectedMenuIndex = 0;
    }
    protected void MenuCrewSignOffVesselType_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SINGAPOREFLAGGED"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOnOffFlaggedVesselsReport.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("ALLVESSELS"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSignOffAllFlaggedVesselsReport.aspx";
        }     
    }
}
