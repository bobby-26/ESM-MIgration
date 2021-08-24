using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.CrewCommon;
using System.Data;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class Crew_CrewReportDataExportPDMSList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Personal", "PERSONAL",ToolBarDirection.Left);
        toolbar.AddButton("Licence ", "LICENCE", ToolBarDirection.Left);
        toolbar.AddButton("Course", "COURSE", ToolBarDirection.Left);
        toolbar.AddButton("Company Experience", "COMPEXP", ToolBarDirection.Left);
        toolbar.AddButton("Other Experience", "OTHEXP", ToolBarDirection.Left);
        toolbar.AddButton("Official Documents", "OFFICIAL", ToolBarDirection.Left);

        MenuCrewSelection.AccessRights = this.ViewState;
        MenuCrewSelection.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            MenuCrewSelection.SelectedMenuIndex = 0;
        }
        ifMoreInfo.Attributes["src"] = "../Crew/CrewReportDataExportPDMSPersonal.aspx";
    
    }
    protected void MenuCrewSelectionType_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("PERSONAL"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportDataExportPDMSPersonal.aspx";
            MenuCrewSelection.SelectedMenuIndex = 0;
        }
        else if (CommandName.ToUpper().Equals("LICENCE"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportDataExportPDMSLicence.aspx";
            MenuCrewSelection.SelectedMenuIndex = 1;
        }
        else if (CommandName.ToUpper().Equals("COURSE"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportDataExportPDMSCourse.aspx";
            MenuCrewSelection.SelectedMenuIndex = 2;
        }
        else if (CommandName.ToUpper().Equals("COMPEXP"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportDataExportPDMSCompanyExp.aspx";
            MenuCrewSelection.SelectedMenuIndex = 3;
        }
        else if (CommandName.ToUpper().Equals("OTHEXP"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportDataExportPDMSOtherExp.aspx";
            MenuCrewSelection.SelectedMenuIndex = 4;
        }
        else if (CommandName.ToUpper().Equals("OFFICIAL"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewReportDataExportPDMSOtherDocuments.aspx";
            MenuCrewSelection.SelectedMenuIndex = 5;
        }
    }
}
