using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class DashboardPhoenix : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
            Response.Redirect("PhoenixLogout.aspx");

        cmdHiddenPick.Attributes.Add("style", "visibility:hidden");
        gvAlert.DataSource = PhoenixRegistersAlerts.GetTaskTypeList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
        gvAlert.DataBind();

        gvFavorites.DataSource = PhoenixRegistersAlerts.GetMenuFavorites(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        gvFavorites.DataBind();

        fraDashboard.Attributes["src"] = "../Options/OptionsVesselSynchronizationStatus.aspx";

        ucDashboardTitle.Text = "Dashboard - Vessel Synchronization Status";

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Dashboard", "NEW");
        toolbar.AddButton("Old Dashboard", "OLD");

        MenuVesselList.MenuList = toolbar.Show();
        MenuVesselList.SelectedMenuIndex = 1;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvFavorites.DataSource = PhoenixRegistersAlerts.GetMenuFavorites(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        gvFavorites.DataBind();
    }

    protected void gvAlert_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        int currentRow = int.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToUpper().Equals("ALERTTASK"))
        {
            Label lbl = (Label)_gridview.Rows[currentRow].FindControl("lblTaskType");
            LinkButton lb = (LinkButton)_gridview.Rows[currentRow].FindControl("lblDescription");
            
            ucDashboardTitle.Text = "Dashboard - " + lb.Text;

            if (lbl.Text == "5" || lbl.Text == "6" || lbl.Text == "7" || lbl.Text == "12")
                fraDashboard.Attributes["src"] = "../Options/OptionsAlertFollowUp.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "8")
                fraDashboard.Attributes["src"] = "../Options/OptionsCrewLicenceAlert.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;            
            //else if (lbl.Text == "14")
            //    fraDashboard.Attributes["src"] = "../Options/OptionsAlertInvoiceStatus.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "15")
                fraDashboard.Attributes["src"] = "../Options/OptionsAlertSupplierDiscrepancyInvoiceList.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "16")
                fraDashboard.Attributes["src"] = "../Options/OptionsAlertCurrencyDiscrepancyInvoiceList.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "17" || lbl.Text == "18" || lbl.Text == "28")
                fraDashboard.Attributes["src"] = "../Options/OptionsAlertInvoiceReconciliationApproval.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "22")
                fraDashboard.Attributes["src"] = "../Options/OptionsCrewLFTDueAlert.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "23")
                fraDashboard.Attributes["src"] = "../Options/OptionsCrewNotContactedAlert.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text + "&duration=1";
            else if (lbl.Text == "24")
                fraDashboard.Attributes["src"] = "../Options/OptionsCrewNotContactedAlert.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text + "&duration=3";
            else if (lbl.Text == "25")
                fraDashboard.Attributes["src"] = "../Options/OptionsCrewNotContactedAlert.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text + "&duration=6";
            else if (lbl.Text == "26")
                fraDashboard.Attributes["src"] = "../Options/OptionsCrewNotContactedAlert.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text + "&duration=12";
            else if (lbl.Text == "27")
                fraDashboard.Attributes["src"] = "../Options/OptionsAlertAuditReport.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "31")
                fraDashboard.Attributes["src"] = "../Options/OptionsCrewAppraisalAlert.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;    
            else if (lbl.Text == "32")
                fraDashboard.Attributes["src"] = "../Options/OptionsAlertVesselCommunication.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "33")
                fraDashboard.Attributes["src"] = "../Options/OptionsVesselAccountsProvisionOnBoardAlert.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else
                fraDashboard.Attributes["src"] = "../Options/OptionsAlertsTask.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;

        }
    }

    protected void gvAlert_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }

    protected void MenuVesselList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("OLD"))
        {
            Response.Redirect("../Dashboard/DashboardPhoenix.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("NEW"))
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                Response.Redirect("DashboardOffice.aspx", false);
            else
                Response.Redirect("DashboardVesselParticulars.aspx", false);
        }
    }

    protected void gvAlert_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
        }
    }

    protected void CheckBoxClicked(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        int nCurrentRow = Int32.Parse(cb.Text);
        string alertid = ((Label)gvAlert.Rows[nCurrentRow].FindControl("lblAlertCode")).Text;
    }

    protected void gvFavorites_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView _gv = (GridView)sender;
            DataRowView drv = (DataRowView)e.Row.DataItem;
            LinkButton lb = (LinkButton)e.Row.FindControl("lbDescription");
            if (lb != null) lb.Attributes["onclick"] = "javascript:OpenSearchPage('" + drv["FLDURL"].ToString() + "'," + drv["FLDMENUCODE"].ToString()  + ");";
        }
    }
}
