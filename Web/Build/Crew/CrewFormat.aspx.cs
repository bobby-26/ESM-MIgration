using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
public partial class Crew_CrewFormat : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            if(Request.QueryString["rid"]!=null && Request.QueryString["rid"].ToString() =="1")
            {
                ifMoreInfo.Attributes["src"] = "../Crew/CrewReportAsPerScreenFormat1.aspx";

            }
            else if (Request.QueryString["rid"] != null && Request.QueryString["rid"].ToString() == "2")
            {
                ifMoreInfo.Attributes["src"] = "../Crew/CrewReportRevisedIMO.aspx";
            }
            else if (Request.QueryString["rid"] != null && Request.QueryString["rid"].ToString() == "3")
            {
                ifMoreInfo.Attributes["src"] = "../Crew/CrewReportUSList.aspx";
            }
            else if (Request.QueryString["rid"] != null && Request.QueryString["rid"].ToString() == "4")
            {
                ifMoreInfo.Attributes["src"] = "../Crew/CrewReportSpecificList.aspx";
            }
            else if (Request.QueryString["rid"] != null && Request.QueryString["rid"].ToString() == "5")
            {
                ifMoreInfo.Attributes["src"] = "../Crew/CrewReportRevisedCrewList.aspx";
            }
            else if (Request.QueryString["rid"] != null && Request.QueryString["rid"].ToString() == "6")
            {
                ifMoreInfo.Attributes["src"] = "../Crew/CrewReportIMOFormat.aspx";
            }

        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Back", "BACK",ToolBarDirection.Right);
      
        MenuCrewOCIMF.AccessRights = this.ViewState;
        MenuCrewOCIMF.MenuList = toolbar.Show();
    }


    protected void MenuCrewOCIMF_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../Crew/CrewList.aspx");
        }
    }
}