using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CrewActivityGeneral : PhoenixBasePage
{
    private string empid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        empid = Request.QueryString["empid"];
        if (!Page.IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            if (Request.QueryString["doa"] == null && Request.QueryString["crewplan"] != "1")
                toolbarmain.AddButton("DOA", "DOA");
            ViewState["status"] = Request.QueryString["status"];

            if (Request.QueryString["sg"] == null || Request.QueryString["ds"] == "1")
            {
                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                {
                    toolbarmain.AddButton("Send Data To Vessel", "SIGNON");
                }
                else
                {
                    toolbarmain.AddButton("Sign On", "SIGNON");
                }
            }
            if (Request.QueryString["so"] == null && Request.QueryString["sg"] == null)
            {
                toolbarmain.AddButton("Sign Off ", "SIGNOFF");
            }
            if (Request.QueryString["ntbr"] == null)
            {
                toolbarmain.AddButton("NTBR/DE-NTBR", "NTBR", CreateNTBRSubTab(string.Empty, 0, string.Empty));
            }
            if (Request.QueryString["ext"] == null)
            {
                toolbarmain.AddButton("Extend/Reduce", "EXTENDREDUCE");
            }
            if (Request.QueryString["inact"] == null)
            {
                toolbarmain.AddButton("In-Active", "INACTIVE");
            }
            if (Request.QueryString["prde"] == null)
            {
                toolbarmain.AddButton("Promotion/Demotion", "PROMDEM");
            }
            if (Request.QueryString["empH"] == null)
            {
                toolbarmain.AddButton("Employee History", "EMPHIS");
            }

            CrewActivity.AccessRights = this.ViewState;
            CrewActivity.MenuList = toolbarmain.Show();
            CrewActivity.SelectedMenuIndex = 0;

            if (Request.QueryString["doa"] == null && Request.QueryString["crewplan"] != "1")
            {
                if (Request.QueryString["launchedfrom"] == null || Request.QueryString["launchedfrom"] == "")
                {
                    ifMoreInfo.Attributes["src"] = "../Crew/CrewDateOfAvailability.aspx?empid=" + empid + "&status=" + Request.QueryString["status"];
                }
                else if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"] != "")
                    ifMoreInfo.Attributes["src"] = "../Crew/CrewDateOfAvailability.aspx?empid=" + empid + "&status=" + Request.QueryString["status"] + "&launchedfrom=" + Request.QueryString["launchedfrom"];
            }
            else if (Request.QueryString["sg"] == null)
            {
                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                    ifMoreInfo.Attributes["src"] = "CrewSignOn.aspx?empid=" + empid + (Request.QueryString["r"] != null ? "&r=1" : string.Empty)
                        + "&vslid=" + Request.QueryString["vslid"]
                        + "&ds=" + Request.QueryString["ds"]
                        + "&pl=" + Request.QueryString["pl"]
                        + "&launchedfrom=" + Request.QueryString["launchedfrom"]
                        + "&trainingmatrixid=" + Request.QueryString["trainingmatrixid"];
                else
                    ifMoreInfo.Attributes["src"] = "CrewSignOn.aspx?empid=" + empid + (Request.QueryString["r"] != null ? "&r=1" : string.Empty) + "&vslid=" + Request.QueryString["vslid"] + "&ds=" + Request.QueryString["ds"];
            }
        }
    }
    private PhoenixToolbar CreateNTBRSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("NTBR", "NTBR");
        toolbarsub.AddButton("DE-NTBR", "DENTBR");
        if (Cmd == "NTBR")
        {
            if (SessionUtil.CanAccess(this.ViewState, "NTBR"))
                Page = "../Crew/CrewNTBR.aspx?empid=" + empid;
        }
        else if (Cmd == "DENTBR")
        {
            if (SessionUtil.CanAccess(this.ViewState, "DENTBR"))
                Page = "../Crew/CrewDENTBR.aspx?empid=" + empid;
        }
        ifMoreInfo.Attributes["src"] = Page;
        return toolbarsub;
    }
    protected void CrewActivity_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SIGNON"))
        {
            if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                ifMoreInfo.Attributes["src"] = "CrewSignOn.aspx?empid=" + empid + (Request.QueryString["r"] != null ? "&r=1" : string.Empty)
                        + "&vslid=" + Request.QueryString["vslid"]
                        + "&ds=" + Request.QueryString["ds"]
                        + "&pl=" + Request.QueryString["pl"]
                        + "&launchedfrom=" + Request.QueryString["launchedfrom"]
                        + "&trainingmatrixid=" + Request.QueryString["trainingmatrixid"];
            else
                ifMoreInfo.Attributes["src"] = "CrewSignOn.aspx?empid=" + empid + (Request.QueryString["r"] != null ? "&r=1" : string.Empty) + "&vslid=" + Request.QueryString["vslid"] + "&ds=" + Request.QueryString["ds"];
        }
        else if (CommandName.ToUpper().Equals("SIGNOFF"))
        {
            ifMoreInfo.Attributes["src"] = "CrewSignOff.aspx?empid=" + empid + (Request.QueryString["r"] != null ? "&r=1" : string.Empty);
        }
        else if (CommandName.ToUpper().Equals("PLAN"))
        {
            ifMoreInfo.Attributes["src"] = "CrewPlan.aspx?empid=" + empid;
        }
        else if (CommandName.ToUpper().Equals("EXTENDREDUCE"))
        {
            ifMoreInfo.Attributes["src"] = "CrewSignOnExtendReduce.aspx?empid=" + empid + (Request.QueryString["r"] != null ? "&r=1" : string.Empty);
        }
        else if (CommandName.ToUpper().Equals("NTBR"))
        {
            CreateNTBRSubTab("../Crew/CrewNTBR.aspx?empid=" + int.Parse(empid), 1, "NTBR");
        }
        else if (CommandName.ToUpper().Equals("DENTBR"))
        {
            CreateNTBRSubTab("../Crew/CrewDENTBR.aspx?empid=" + int.Parse(empid), 2, "DENTBR");

        }
        else if (CommandName.ToUpper().Equals("DOA"))
        {
            ifMoreInfo.Attributes["src"] = "CrewDateOfAvailability.aspx?empid=" + empid + "&status=" + ViewState["status"];
        }
        else if (CommandName.ToUpper().Equals("TRAVELVESSEL"))
        {
            ifMoreInfo.Attributes["src"] = "CrewTravelToVessel.aspx?empid=" + empid;
        }
        else if (CommandName.ToUpper().Equals("INACTIVE"))
        {
            ifMoreInfo.Attributes["src"] = "CrewInActive.aspx?empid=" + empid;
        }
        else if (CommandName.ToUpper().Equals("PROMDEM"))
        {
            ifMoreInfo.Attributes["src"] = "CrewPromotionDemotion.aspx?empid=" + empid;
        }
        else if (CommandName.ToUpper().Equals("EMPHIS"))
        {
            ifMoreInfo.Attributes["src"] = "CrewActivityEmployeeHistory.aspx?empid=" + empid;
        }
    }
}
