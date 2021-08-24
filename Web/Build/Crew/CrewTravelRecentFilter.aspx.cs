using System;
using System.Collections.Specialized;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class Crew_CrewTravelRecentFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);        
        TravelPlanFilter.AccessRights = this.ViewState;
        TravelPlanFilter.MenuList = toolbar.Show();       
    }

    protected void TravelPlanFilter_TabStripCommand(object sender, EventArgs e)
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
            criteria.Add("txtReqNo", txtReqNo.Text.Trim());
            criteria.Add("txtFileno", txtFileNo.Text.Trim());
            criteria.Add("txtName", txtName.Text.Trim());
            criteria.Add("ddlRank", ddlRank.SelectedRank);
            criteria.Add("ucZone", ucZone.SelectedZone);

            Filter.CurrentTravelRecentFilter = criteria;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        if (CommandName.ToUpper().Equals("GO"))
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }    
}