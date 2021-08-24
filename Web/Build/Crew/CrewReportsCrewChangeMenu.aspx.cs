using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.CrewCommon;
using System.Data;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class Crew_CrewReportsCrewChangeMenu : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("JSU PD", "JSUPD", ToolBarDirection.Right);
        toolbar.AddButton("JSU NOK", "JSUNOK", ToolBarDirection.Right);
        toolbar.AddButton("Crew List", "CREWLIST", ToolBarDirection.Right);
        
        MenuCrewSelection.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            
        }

        ifMoreInfo.Attributes["src"] = "../Crew/CrewReportsCrewChange.aspx";
        MenuCrewSelection.SelectedMenuIndex = 2;

    }
    protected void MenuCrewSelectionType_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("CREWLIST"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportsCrewChange.aspx";
            MenuCrewSelection.SelectedMenuIndex = 2;
        }
        else if (CommandName.ToUpper().Equals("JSUNOK"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportsCrewNok.aspx";
            MenuCrewSelection.SelectedMenuIndex = 1;
        }
        else if (CommandName.ToUpper().Equals("JSUPD"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportsCrewPd.aspx";
            MenuCrewSelection.SelectedMenuIndex = 0;
        }
        
        else
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportsCrewChange.aspx";
        }
    }
}
