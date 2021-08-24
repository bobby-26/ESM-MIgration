using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.CrewCommon;
using System.Data;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class Crew_CrewListforSingleVesselSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Safe Scale", "SAFESCALE", ToolBarDirection.Right);
        toolbar.AddButton("Owner Scale", "OWNERSCALE", ToolBarDirection.Right);
        toolbar.AddButton("Actual", "ACTUAL",ToolBarDirection.Right);
        
        
        MenuCrewSelection.MenuList = toolbar.Show();

        ifMoreInfo.Attributes["src"] = "../Crew/CrewSingleVesselActual.aspx";
        MenuCrewSelection.SelectedMenuIndex = 2;

    }
    protected void MenuCrewSelectionType_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ACTUAL"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSingleVesselActual.aspx";
            MenuCrewSelection.SelectedMenuIndex = 2;
        }
        else if (CommandName.ToUpper().Equals("OWNERSCALE"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSingleVesselOwnerScale.aspx";
            MenuCrewSelection.SelectedMenuIndex = 1;
        }
        else if (CommandName.ToUpper().Equals("SAFESCALE"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSingleVesselSafeScale.aspx";
            MenuCrewSelection.SelectedMenuIndex = 0;
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewSingleVesselActual.aspx";
        }
    }
}