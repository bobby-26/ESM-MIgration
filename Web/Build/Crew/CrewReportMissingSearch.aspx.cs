using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class Crew_CrewReportMissingSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("All Missing Documents", "ALLMISSINGDOCUMENTS", ToolBarDirection.Right);
        toolbar.AddButton("Medical", "MEDICAL", ToolBarDirection.Right);
        toolbar.AddButton("Travel Documents", "TDOCU", ToolBarDirection.Right);
        toolbar.AddButton("Course", "COURSE", ToolBarDirection.Right);
        toolbar.AddButton("Licence", "LICENSE", ToolBarDirection.Right);

        MenuCrewSelection.AccessRights = this.ViewState;
        MenuCrewSelection.MenuList = toolbar.Show();

        if (!IsPostBack)
        {

        }
        ifMoreInfo.Attributes["src"] = "../Crew/CrewReportMissingLicense.aspx";
        MenuCrewSelection.SelectedMenuIndex = 4;

    }
    protected void MenuCrewSelectionType_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("LICENSE"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportMissingLicense.aspx";
            MenuCrewSelection.SelectedMenuIndex = 4;
        }
        else if (CommandName.ToUpper().Equals("COURSE"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportMissingCourse.aspx";
            MenuCrewSelection.SelectedMenuIndex = 3;
        }
        else if (CommandName.ToUpper().Equals("TDOCU"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportMissingTravelDocuments.aspx";
            MenuCrewSelection.SelectedMenuIndex = 2;
        }
        else if (CommandName.ToUpper().Equals("MEDICAL"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportMissingMedical.aspx";
            MenuCrewSelection.SelectedMenuIndex = 1;
        }
        else if (CommandName.ToUpper().Equals("ALLMISSINGDOCUMENTS"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportAllMissingDocuments.aspx";
            MenuCrewSelection.SelectedMenuIndex = 0;
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportMissingLicense.aspx";
            MenuCrewSelection.SelectedMenuIndex = 4;
        }
    }
}
