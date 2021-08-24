using System;
using System.Collections.Specialized;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewMedicalRequestFilter : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();       
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        PlanRelieverFilterMain.AccessRights = this.ViewState;
        PlanRelieverFilterMain.MenuList = toolbar.Show();
       
    }

    protected void PlanRelieverFilterMain_TabStripCommand(object sender, EventArgs e)
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
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
            criteria.Add("ddlRank", ddlRank.SelectedRank);
            criteria.Add("ddlCourse", ddlCourse.SelectedCourse);
            criteria.Add("txtFromDate", txtFromDate.Text);
            criteria.Add("txtToDate", txtToDate.Text);
			criteria.Add("txtFileNo", txtFileNo.Text);
            criteria.Add("chkInactive", chkInactive.Checked==true ? "0" : string.Empty);
            Filter.CurrentCourseRequestFilter = criteria;           
        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }    
}
