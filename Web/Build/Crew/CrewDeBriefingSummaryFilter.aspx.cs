using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewDeBriefingSummaryFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        
        MenuPD.AccessRights = this.ViewState;
        MenuPD.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            
            ucstatus.SelectedHard = "1097";
        }
    }

    protected void PD_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection summary = new NameValueCollection();

            summary.Clear();

            summary.Add("ddlRank", ddlRank.SelectedRank);
            summary.Add("ddlVessel", UcVessel.SelectedVessel);
            summary.Add("txtFrom", txtFromdate.Text);
            summary.Add("txtTo", txtTodate.Text);
            summary.Add("ucstatus", ucstatus.SelectedHard);

            Filter.DebriefingsummarySearch = summary;
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
