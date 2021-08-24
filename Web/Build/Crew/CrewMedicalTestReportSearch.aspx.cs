using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class Crew_CrewMedicalTestReportSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Vaccination", "VACCINATION", ToolBarDirection.Right);
        toolbar.AddButton("Medical Cost", "MEDICALCOST", ToolBarDirection.Right);
        toolbar.AddButton("Medical Test", "MEDICALTEST",ToolBarDirection.Right);
        
        
        MenuCrewSelection.MenuList = toolbar.Show();
        MenuCrewSelection.SelectedMenuIndex = 2;
        ifMoreInfo.Attributes["src"] = "../Crew/CrewReportMedicalTest.aspx";
 
    }
    protected void MenuCrewSelectionType_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("MEDICALTEST"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportMedicalTest.aspx";
            MenuCrewSelection.SelectedMenuIndex = 2;
        }
        else if (CommandName.ToUpper().Equals("MEDICALCOST"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportMedicalCost.aspx";
            MenuCrewSelection.SelectedMenuIndex = 1;
        }
        else if (CommandName.ToUpper().Equals("VACCINATION"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportMedicalVaccination.aspx";
            MenuCrewSelection.SelectedMenuIndex = 0;
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportMedicalTest.aspx";
        }

    }
}
