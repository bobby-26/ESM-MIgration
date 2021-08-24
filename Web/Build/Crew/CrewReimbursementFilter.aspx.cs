using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewReimbursementFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuPD.AccessRights = this.ViewState;
        MenuPD.MenuList = toolbar.Show();
        if (Filter.CurrentMenuCodeSelection == "CRW-OPR-REM")
            trAccountOf.Visible = true;

    }

    protected void PD_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        
        string Script = "";
        Script += "<script language='JavaScript' id='BookMarkScript'>";
        Script += "fnReloadList();";
        Script += "</script>";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("ddlRank", ddlRank.SelectedRank);
            criteria.Add("txtName", txtName.Text);
            criteria.Add("txtFileNo", txtFileNo.Text);
            criteria.Add("chkActive", ddlStatus.SelectedValue);
            criteria.Add("ddlApproved", ddlApproved.SelectedValue);
            criteria.Add("txtFromDate", txtFromDate.Text);
            criteria.Add("txtToDate", txtToDate.Text);
            criteria.Add("ddlEarDed", ddlEarDed.SelectedValue);
            criteria.Add("ddlPurpose", ddlPurpose.SelectedHard);
            criteria.Add("ddlVesselAccountof", ddlVesselAccountof.SelectedVessel);
            criteria.Add("ddlChargeableVessel", ddlChargeableVessel.SelectedVessel);
            criteria.Add("txtApprovedDateFrom", txtApprovedDateFrom.Text);
            criteria.Add("txtApprovedDateTo", txtApprovedDateTo.Text);
            criteria.Add("ddlPaymentMode", ddlPaymentMode.SelectedHard);
            Filter.CrewReimbursementFilterSelection = criteria;
        }
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        
    }

    protected void ddlEarDed_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableInteger(ddlEarDed.SelectedValue).HasValue && General.GetNullableInteger(ddlEarDed.SelectedValue).Value < 0)
            {
                ddlPurpose.HardList = PhoenixRegistersReimbursementRecovery.ListReimbursementRecovery(1, 0, 1, null);
            }
            else
            {
                ddlPurpose.HardList = PhoenixRegistersReimbursementRecovery.ListReimbursementRecovery(0, 0, 1, null);
            }
        }
        catch
        {
        }
    }
}
