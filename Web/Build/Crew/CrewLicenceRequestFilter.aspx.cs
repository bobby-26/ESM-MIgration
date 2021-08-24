using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewLicenceRequestFilter : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
			PlanRelieverFilterMain.AccessRights = this.ViewState;
            PlanRelieverFilterMain.MenuList = toolbar.Show();           
           //ddlVessel.VesselList = PhoenixRegistersVessel.ListVessel(null, "", 0);
            if (!string.IsNullOrEmpty(Request.QueryString["e"]))
            {
                ucError.ErrorMessage = "Either Flag or Vessel is mandatory.";
                ucError.Visible = true;
            }
        }
    }

    protected void PlanRelieverFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		
        if (dce.CommandName.ToUpper().Equals("GO"))
        {
			NameValueCollection criteria = new NameValueCollection();
			if (!IsValidFilter())
			{
				ucError.Visible = true;
				return;
			}
			criteria.Clear();
			criteria.Add("ddlFlag", ddlFlag.SelectedFlag);
			criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
			criteria.Add("txtFromDate", txtFromDate.Text);
			criteria.Add("txtToDate", txtToDate.Text);
			criteria.Add("txtName", txtName.Text);
			criteria.Add("txtFileno", txtFileno.Text);
			criteria.Add("ucStatus", ucStatus.SelectedHard);
            criteria.Add("ucRankList", ucRankList.selectedlist);
			Filter.CurrentLicenceRequestFilterSelection = criteria;
			
        }
		String scriptpopupclose = String.Format("javascript:fnReloadList('Filter', null,null);");
		ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
		
    }

    private bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

       
		//if (ddlVessel.SelectedVessel == "" && General.GetNullableInteger(ddlFlag.SelectedFlag) == null)
		//{
		//    ucError.ErrorMessage = "Either Flag or Vessel is mandatory.";
		//}

        if (ddlVessel.SelectedVessel.Contains(",") && General.GetNullableInteger(ddlFlag.SelectedFlag) == null)
        {
            ucError.ErrorMessage = "Select Single Vessel if Flag is not selected.";
        }

        return (!ucError.IsError);
    }

    protected void ddlFlag_TextChangedEvent(object sender, EventArgs e)
    {
        ddlVessel.Flag = General.GetNullableInteger(ddlFlag.SelectedFlag) == null ? "" : ddlFlag.SelectedFlag;
        //ddlVessel.VesselList = PhoenixRegistersVessel.ListVessel(General.GetNullableInteger(ddlFlag.SelectedFlag), "", 0);
        ddlVessel.VesselList = PhoenixRegistersVessel.ListAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlFlag.SelectedFlag), "");
    }
}
