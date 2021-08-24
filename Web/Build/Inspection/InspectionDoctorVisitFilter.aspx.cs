using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class InspectionDoctorVisitFilter : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
       
        PlanRelieverFilterMain.AccessRights = this.ViewState;
        PlanRelieverFilterMain.MenuList = toolbar.Show();
        //if (!IsPostBack)
        //{
			
        //}
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
            criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
            criteria.Add("ucVesselType", ucVesselType.SelectedVesseltype);
            criteria.Add("txtIllnessFrom", txtIllnessFrom.Text);
            criteria.Add("txtIllnessTo", txtIllnessTo.Text);
            criteria.Add("txtClosingFrom", txtClosingFrom.Text);
            criteria.Add("txtClosingTo", txtClosingTo.Text);
            criteria.Add("ddlStatus", ddlStatus.SelectedValue);
            criteria.Add("txtName", txtName.Text);
            criteria.Add("txtFileNo", txtFileNo.Text);

            Filter.CurrentDoctorVisitFilter = criteria;           
        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }    
}
