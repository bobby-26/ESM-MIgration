using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class Crew_CrewTravelPlanFilter : PhoenixBasePage
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
            //criteria.Add("txtReqNo", txtReqNo.Text);
            criteria.Add("txtFileno", txtFileNo.Text.Trim());
            criteria.Add("txtName", txtName.Text.Trim());
            criteria.Add("ddlRank", ddlRank.SelectedRank);
            criteria.Add("ucZone", ucZone.SelectedZone);
        //  criteria.Add("chkPlanned", chkPlanned.Checked ? "0" : string.Empty);

            Filter.CurrentTravelPlanFilter = criteria;
        }
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);        
    }    

}