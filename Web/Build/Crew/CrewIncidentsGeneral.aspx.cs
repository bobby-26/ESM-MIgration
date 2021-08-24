using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewIncidentsGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();  
        toolbarmain.AddButton("Compliments ", "COMPLIEMENTS");
        toolbarmain.AddButton("Complaints", "COMPLAINTS");
        CrewIncidents.AccessRights = this.ViewState;
        CrewIncidents.MenuList = toolbarmain.Show();
        CrewIncidents.SelectedMenuIndex = 0;

        if (!Page.IsPostBack)
        {
          
            ifMoreInfo.Attributes["src"] = "../Crew/CrewIncidents.aspx?t=c";
        }
    }

    protected void CrewIncidents_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        //if (dce.CommandName.ToUpper().Equals("INCIDENTS"))
        //{
        //    ifMoreInfo.Attributes["src"] = "CrewIncidents.aspx?t=i";
        //}
        if (CommandName.ToUpper().Equals("COMPLIEMENTS"))
        {
            ifMoreInfo.Attributes["src"] = "CrewIncidents.aspx?t=c";
        }
        else if (CommandName.ToUpper().Equals("COMPLAINTS"))
        {
            CrewIncidents.SelectedMenuIndex = 1;
            ifMoreInfo.Attributes["src"] = "CrewIncidents.aspx?t=p";
        }  
    }
}
