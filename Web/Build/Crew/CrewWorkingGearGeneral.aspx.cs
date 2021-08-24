using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewWorkingGearGeneral : PhoenixBasePage
{
    //private string empid = string.Empty;
    private string neededid = null;
    private string empid = null;
    private string vesselid = null;
    private string crewplanid = null;
    private string r = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //empid = Request.QueryString["empid"];
        
        if (!Page.IsPostBack)
        {
            if (Session["NEEDEDID"] != null)
            {
                neededid = Session["NEEDEDID"].ToString();
            }
            if (Session["empid"] != null)
            {
                empid = Session["empid"].ToString(); ;
            }
            if (Session["vesselid"] != null)
            {
                vesselid = Session["vesselid"].ToString(); 
            }
            if (Session["crewplanid"] != null)
            {
                crewplanid = Session["crewplanid"].ToString();
            }
            if (Session["r"] != null)
            {
                r = Session["r"].ToString();
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //if (Request.QueryString["doa"] == null && Request.QueryString["crewplan"] != "1")
            
            toolbarmain.AddButton("List", "LIST");
            toolbarmain.AddButton("Request", "REQUEST");
            toolbarmain.AddButton("Quotation", "QUOTATION");
            toolbarmain.AddButton("Receive", "RECEIVE");

            CrewActivity.AccessRights = this.ViewState;
            CrewActivity.MenuList = toolbarmain.Show();
            CrewActivity.SelectedMenuIndex = 1;

                ifMoreInfo.Attributes["src"] = "../Crew/CrewWorkGearNeededItem.aspx?Neededid=" + neededid + "&empid=" + empid  + "&vesselid=" + vesselid + "&crewplanid=" + crewplanid + "&r=" + r;
        }
    }

    protected void CrewActivity_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../Crew/CrewWorkingGearOrderForm.aspx?Neededid=" + neededid + "&empid=" + empid + "&vesselid=" + vesselid + "&crewplanid=" + crewplanid + "&r=" + r);
            //ifMoreInfo.Attributes["src"] = "../Crew/CrewWorkingGearOrderForm.aspx?Neededid=" + neededid;
        }
        if (dce.CommandName.ToUpper().Equals("REQUEST"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewWorkGearNeededItem.aspx?Neededid=" + neededid + "&empid=" + empid + "&vesselid=" + vesselid + "&crewplanid=" + crewplanid + "&r=" + r;
        }
        if (neededid != null && neededid != "")
        {
            if (dce.CommandName.ToUpper().Equals("QUOTATION"))
            {
                ifMoreInfo.Attributes["src"] = "../Crew/CrewWGQuotationAgentDetail.aspx?Neededid=" + neededid + "&empid=" + empid + "&vesselid=" + vesselid + "&crewplanid=" + crewplanid + "&r=" + r;
            }
            if (dce.CommandName.ToUpper().Equals("RECEIVE"))
            {
                ifMoreInfo.Attributes["src"] = "../Crew/CrewWorkingGearOrderReceive.aspx?Neededid=" + neededid + "&empid=" + empid + "&vesselid=" + vesselid + "&crewplanid=" + crewplanid + "&r=" + r;
            }
        }
    }
}
