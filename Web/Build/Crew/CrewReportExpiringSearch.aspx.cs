using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.CrewCommon;
using System.Data;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Crew_CrewReportExpiringSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("All Expiring Documents", "ALLEXPIRINGDOCUMENTS", ToolBarDirection.Right);
        toolbar.AddButton("Medical", "MEDICAL", ToolBarDirection.Right);
        toolbar.AddButton("Travel Documents", "TDOCU", ToolBarDirection.Right);
        toolbar.AddButton("Other Documents", "ODOCU", ToolBarDirection.Right);
        toolbar.AddButton("Course", "COURSE", ToolBarDirection.Right);
        toolbar.AddButton("Licence", "LICENSE", ToolBarDirection.Right);
        
        MenuCrewSelection.AccessRights = this.ViewState;
        MenuCrewSelection.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
           
        }
        ifMoreInfo.Attributes["src"] = "../Crew/CrewReportExpiringLicense.aspx";
        MenuCrewSelection.SelectedMenuIndex = 5;

    }
    protected void MenuCrewSelectionType_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("LICENSE"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportExpiringLicense.aspx";
            MenuCrewSelection.SelectedMenuIndex = 5;
        }
        else if (CommandName.ToUpper().Equals("COURSE"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportExpiringCourse.aspx";
            MenuCrewSelection.SelectedMenuIndex = 4;
        }
        else if (CommandName.ToUpper().Equals("ODOCU"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportExpiringOtherDocuments.aspx";
            MenuCrewSelection.SelectedMenuIndex = 3;
        }
        else if (CommandName.ToUpper().Equals("TDOCU"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportExpiringTravelDocuments.aspx";
            MenuCrewSelection.SelectedMenuIndex = 2;
        }
        else if (CommandName.ToUpper().Equals("MEDICAL"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportExpiringMedical.aspx";
            MenuCrewSelection.SelectedMenuIndex = 1;
        }
        else if (CommandName.ToUpper().Equals("ALLEXPIRINGDOCUMENTS"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportAllExpiringDocuments.aspx";
            MenuCrewSelection.SelectedMenuIndex = 0;
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportExpiringLicense.aspx";
            MenuCrewSelection.SelectedMenuIndex = 5;
        }
    }
}
