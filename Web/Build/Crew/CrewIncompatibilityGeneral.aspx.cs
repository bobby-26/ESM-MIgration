using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CrewIncompatibilityGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!Page.IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Compatibility", "COMPATIBILITY");
            toolbarmain.AddButton("Employee List ", "EMPLOYEE");                     
            CrewIncidents.MenuList = toolbarmain.Show();
            CrewIncidents.SelectedMenuIndex = 0;          
            ifMoreInfo.Attributes["src"] = "../Crew/CrewIncompatibilityList.aspx?empid=" + Request.QueryString["empid"];
        }
    }

    protected void CrewIncidents_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        
        if (CommandName.ToUpper().Equals("COMPATIBILITY"))
        {
            ifMoreInfo.Attributes["src"] = "CrewIncompatibilityList.aspx?empid=" + Request.QueryString["empid"];
        }
        else if (CommandName.ToUpper().Equals("EMPLOYEE"))
        {
            ifMoreInfo.Attributes["src"] = "CrewIncompatibilityCrewList.aspx?empid=" + Request.QueryString["empid"];
        }       
    }
}
