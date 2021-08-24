using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewNewApplicantActivitylGeneral : PhoenixBasePage
{
    private string empid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        empid = Request.QueryString["empid"];
        if (!Page.IsPostBack)
        {
            ViewState["launchedfrom"] = "";
            if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
            {
                ViewState["launchedfrom"] = Request.QueryString["launchedfrom"].ToString();                
            }
           
           
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "selectTab('CrewActivity', 'DOA');";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            ifMoreInfo.Attributes["src"] = "../Crew/CrewNewApplicantDateOfAvailability.aspx?empid=" + empid;
        }
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("DOA", "DOA",ToolBarDirection.Right);
        //toolbarmain.AddButton("Status", "INACTIVE");
        CrewActivity.AccessRights = this.ViewState;
       
        CrewActivity.MenuList = toolbarmain.Show();
    }

    protected void CrewActivity_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("INACTIVE"))
        {
            ifMoreInfo.Attributes["src"] = "CrewNewApplicantInActive.aspx?empid=" + empid;
        }
        else if (CommandName.ToUpper().Equals("DOA") && ViewState["launchedfrom"].ToString() == "")
        {
            ifMoreInfo.Attributes["src"] = "CrewNewApplicantDateOfAvailability.aspx?empid=" + empid;
        }
        else if (CommandName.ToUpper().Equals("DOA") && ViewState["launchedfrom"].ToString() != "")
        {
            ifMoreInfo.Attributes["src"] = "CrewNewApplicantDateOfAvailability.aspx?empid=" + empid + "&launchedfrom=" + ViewState["launchedfrom"].ToString(); 
        }
    }
}
