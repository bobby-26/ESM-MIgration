using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewWorkingGearIssue : PhoenixBasePage
{
    private string empid = string.Empty;
    private string vesselid = string.Empty;
    private string rankid = string.Empty;
    private string issueid = string.Empty;
    private string crewplanid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        empid = Request.QueryString["empid"];
        vesselid = Request.QueryString["vesselid"];
        rankid = Request.QueryString["rankid"];
        issueid = Request.QueryString["issueid"];
        crewplanid = Request.QueryString["crewplanid"];
        if (!Page.IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Issue as Set", "SET");
            toolbarmain.AddButton("Issue as Item", "ITEM");
            toolbarmain.AddButton("Back", "BACK");
            CrewWorkGearIssue.AccessRights = this.ViewState;
            CrewWorkGearIssue.MenuList = toolbarmain.Show();

            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "selectTab('Issue as Set', 'SET');";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            ifMoreInfo.Attributes["src"] = "../Crew/CrewWorkingGearIssuanceSet.aspx?empid=" + empid + "&vesselid=" + vesselid + "&rankid=" + rankid + "&issueid=" + issueid;
            CrewWorkGearIssue.SelectedMenuIndex = 0;
        }
    }

    protected void CrewWorkGearIssue_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SET"))
        {
            ifMoreInfo.Attributes["src"] = "CrewWorkingGearIssuanceSet.aspx?empid=" + empid + "&vesselid=" + vesselid + "&rankid=" + rankid + "&issueid=" + issueid;
        }
        else if (dce.CommandName.ToUpper().Equals("ITEM"))
        {
            ifMoreInfo.Attributes["src"] = "CrewWorkingGearIssuanceItems.aspx?empid=" + empid + "&vesselid=" + vesselid + "&rankid=" + rankid + "&issueid=" + issueid;
        }
        else if (dce.CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("CrewWorkingGearIssuance.aspx?empid=" + empid + "&vslid=" + vesselid + "&r=1&ntbr=0&ext=0&inact=0&issueid=" + issueid + "&crewplanid=" + crewplanid + "", false);
        }
    }
}
