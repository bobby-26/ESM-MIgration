using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class Crew_CrewPlanRelieverFilter : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            toolbar.AddButton("Cancel", "CANCEL");
            PlanRelieverFilterMain.AccessRights = this.ViewState;
            PlanRelieverFilterMain.MenuList = toolbar.Show();            
        }
    }

    protected void PlanRelieverFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();           
            //criteria.Add("chkAdditionalRank", chkAdditionalRank.Checked ? "1" : string.Empty);
            criteria.Add("ucVslType", ucVslType.SelectedVesseltype);
            criteria.Add("txtVslTypeExperience", txtVslTypeExperience.Text);
            criteria.Add("ucEngType", ucEngType.SelectedEngineTypeList);
            criteria.Add("txtEngTypeExperience", txtEngTypeExperience.Text);
            criteria.Add("ucRank", ucRank.selectedlist);
            criteria.Add("txtRankExperience", txtRankExperience.Text);
            criteria.Add("txtCompanyExperience", txtCompanyExperience.Text);
            //criteria.Add("chkZone", chkZone.Checked ? "1" : string.Empty);
            //criteria.Add("chkPool", chkPool.Checked ? "1" : string.Empty);
            criteria.Add("chkNationality", chkNationality.Checked ? "1" : string.Empty);
            criteria.Add("chkPlanned", chkPlanned.Checked ? "1" : string.Empty);
            criteria.Add("chkDocuments", string.Empty); //chkDocuments.Checked ? "1" : string.Empty);
			criteria.Add("txtDOAFromDate", txtDOAFromDate.Text);
			criteria.Add("txtDOAToDate", txtDOAFromTo.Text);
            criteria.Add("ucPrinicpal", ucPrinicpal.SelectedAddress);
            Filter.CurrentPlanRelieverFilterSelection = criteria;
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
