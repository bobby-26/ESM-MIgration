using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;

public partial class Dashboard_DashboardAlerts : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    
        gvAlert.DataSource = PhoenixRegistersAlerts.GetTaskTypeList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
        gvAlert.DataBind();

        DataSet dsedit = PhoenixCommonDashboard.DashboardVesselAdminEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (dsedit.Tables[0].Rows.Count > 0)
            ViewState["FLDDTKEY"] = dsedit.Tables[0].Rows[0]["FLDDTKEY"].ToString();
        else
            ViewState["FLDDTKEY"] = "";

       ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "resize();", true);
    }
    protected void gvAlert_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        int currentRow = int.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToUpper().Equals("ALERTTASK"))
        {
            Label lbl = (Label)_gridview.Rows[currentRow].FindControl("lblTaskType");
            LinkButton lb = (LinkButton)_gridview.Rows[currentRow].FindControl("lblDescription");

            ucDashboardTitle.Text = "Alerts - " + lb.Text;

            if (lbl.Text == "5" || lbl.Text == "6" || lbl.Text == "7")
                fraDashboard.Attributes["src"] = "../Options/OptionsAlertFollowUp.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "8")
                fraDashboard.Attributes["src"] = "../Options/OptionsCrewLicenceAlert.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            //else if (lbl.Text == "14")
            //    fraDashboard.Attributes["src"] = "../Options/OptionsAlertInvoiceStatus.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "12")
                fraDashboard.Attributes["src"] = "../Options/OptionsCrewTraineeAppraisalAlert.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
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
			else if (lbl.Text == "34")
				fraDashboard.Attributes["src"] = "../Options/OptionsCrewExpiringCourseCertificateAlert.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "37")
                fraDashboard.Attributes["src"] = "../Options/OptionsWorkOrderDue.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text; 
            else if (lbl.Text == "38")
                fraDashboard.Attributes["src"] = "../Options/OptionsWorkOrderDue2Months.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "39")
                fraDashboard.Attributes["src"] = "../Options/OptionsWorkOrderCompleted.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "40")
                fraDashboard.Attributes["src"] = "../Options/OptionsWorkOrderDefectiveDue.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "41")
                fraDashboard.Attributes["src"] = "../Options/OptionsCriticalSpareItemBelowMinLevel.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "42")
                fraDashboard.Attributes["src"] = "../Options/OptionsVesselCertificateDueOverdue.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "43")
                fraDashboard.Attributes["src"] = "../Options/OptionsPurchaseRequisition.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "44")
                fraDashboard.Attributes["src"] = "../Options/OptionsWorkOrderOverDueReport.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else if (lbl.Text == "45")
                fraDashboard.Attributes["src"] = "../Options/OptionsCrewOffshoreTrainingNeedAlert.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;
            else
                fraDashboard.Attributes["src"] = "../Options/OptionsAlertsTask.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text;

        }
    }

    protected void gvAlert_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }

    protected void gvAlert_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
        }
    }


}
