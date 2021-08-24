using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
public partial class Crew_CrewAppraisalPendingcommentSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        PlanRelieveeFilterMain.AccessRights = this.ViewState;
        PlanRelieveeFilterMain.MenuList = toolbar.Show();

        NameValueCollection nvc = Filter.CurrentPendingCommentsFilter;
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            ucVessel.Enabled = false; 
        }
    }
    protected void PlanRelieveeFilterMain_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {

            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();
            criteria.Add("txtFileNo", txtFileNo.Text != "" ? txtFileNo.Text :null );
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("txtaprFrom", txtaprFrom.Text);
            criteria.Add("txtaprTo", txtaprTo.Text);
            criteria.Add("ucPool", ucPool.SelectedPool != "Dummy" && ucPool.SelectedPool != "" ? ucPool.SelectedPool : null);//ucPool.SelectedPool);
            criteria.Add("ucZone", ucZone.selectedlist != "Dummy" && ucZone.selectedlist != ""? ucZone.selectedlist : null);
            criteria.Add("ucRank", ucRank.SelectedRank);
            Filter.CurrentPendingCommentsFilter = criteria;
        }
        Response.Redirect("CrewAppraisalPendingSeafarercomments.aspx");
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
