using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class OptionsAlertsTask : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string tasktype = Request.QueryString["tasktype"].ToString();
        Filter.CurrentAlertTaskType = tasktype;
        GetAlertItems(tasktype);
    }

    private void GetAlertItems(string tasktype)
    {
        if (tasktype.Equals("1"))
        {
            gvAlertsTask.DataSource = PhoenixRegistersAlerts.NewRequisitions(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            gvAlertsTask.DataBind();
        }

        if (tasktype.Equals("2"))
        {
            gvAlertsTask.DataSource = PhoenixRegistersAlerts.QuotationsAwaitingApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            gvAlertsTask.DataBind();
        }

        if (tasktype.Equals("3"))
        {
            gvAlertsTask.DataSource = PhoenixRegistersAlerts.POWaitingToBeIssued(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            gvAlertsTask.DataBind();
        }

        if (tasktype.Equals("4"))
        {
            gvAlertsTask.DataSource = PhoenixRegistersAlerts.QuotationsNotReceived(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            gvAlertsTask.DataBind();
        }

        if (tasktype.Equals("9"))
        {
            gvAlertsTask.DataSource = PhoenixRegistersAlerts.CertificatesDueIn45Days(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            gvAlertsTask.DataBind();
        }

        if (tasktype.Equals("10"))
        {
            gvAlertsTask.DataSource = PhoenixRegistersAlerts.CertificatesOverDue(PhoenixSecurityContext.CurrentSecurityContext.UserCode,PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            gvAlertsTask.DataBind();
        }

        if (tasktype.Equals("11"))
        {
            gvAlertsTask.DataSource = PhoenixRegistersAlerts.JobsOverDue(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            gvAlertsTask.DataBind();
        }
        if (tasktype.Equals("19"))
        {
            gvAlertsTask.DataSource = PhoenixRegistersAlerts.VendorDeliveryDate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            gvAlertsTask.DataBind();
        }
        if (tasktype.Equals("20"))
        {
            gvAlertsTask.DataSource = PhoenixPurchaseQuotation.QuotationsAwaitingApproval5KUSD(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            gvAlertsTask.DataBind();
        }
        if (tasktype.Equals("21"))
        {
            gvAlertsTask.DataSource = PhoenixPurchaseQuotation.QuotationsAwaitingApproval10KUSD(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            gvAlertsTask.DataBind();
        }
        if (tasktype.Equals("27"))
        {
            gvAlertsTask.DataSource = PhoenixInspectionAuditSchedule.InternalAuditsToBeReviewed(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            gvAlertsTask.DataBind();
        }
    }

    protected void gvAlertsTask_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string tasktype = Request.QueryString["tasktype"].ToString();


        if (int.Parse(tasktype) == 1 || int.Parse(tasktype) == 2 || int.Parse(tasktype) == 3 || int.Parse(tasktype) == 4 || int.Parse(tasktype) == 19 
            || int.Parse(tasktype) == 20 || int.Parse(tasktype) == 21)
        {
            ShowOrderForm(sender, e, tasktype);
        }

        if (int.Parse(tasktype) == 9 || int.Parse(tasktype) == 10)
        {
            ShowVesselMaster(sender, e, tasktype);
        }

        if (int.Parse(tasktype) == 11)
        {
            ShowWorkOrder(sender, e, tasktype);
        }
        if (int.Parse(tasktype) == 27)
        {
            ShowAuditReport(sender, e, tasktype);
        } 
    }

    protected void gvAlertsTask_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (int.Parse(Request.QueryString["tasktype"].ToString()) == 11)
            {
                LinkButton lb = (LinkButton)e.Row.FindControl("lblDescription");
                if (lb != null) lb.Attributes.Add("onclick", "showVessel(); return true;");
            }
        }
    }

    private void ShowVesselMaster(object sender, GridViewCommandEventArgs e, string tasktype)
    {
        int nRow = int.Parse(e.CommandArgument.ToString());

        GridView _gridView = (GridView)sender;

        string[] arr = ((Label)_gridView.Rows[nRow].FindControl("lblExpression")).Text.Split(',');

        PhoenixRegistersAlerts.AlertViewHistoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            int.Parse(tasktype), ((Label)_gridView.Rows[nRow].FindControl("lblTaskKey")).Text);

        DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        PhoenixGeneralSettings.CurrentGeneralSetting = new GeneralSetting(ds);
        SessionUtil.ReBuildMenu();

        Filter.CurrentVesselMasterFilter = arr[1];
        Filter.CurrentCertificateSurveyVesselFilter = arr[1];
        string NAME = arr[0];

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "parent.showVessel();";
        Script += "parent.OpenSearchPage('PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx?certificatename="+arr[0]+"', 'CSY-CST');";
        Script += "</script>" + "\n";

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    private void ShowWorkOrder(object sender, GridViewCommandEventArgs e, string tasktype)
    {
        int nRow = int.Parse(e.CommandArgument.ToString());
        GridView _gridView = (GridView)sender;


        string[] arr = ((Label)_gridView.Rows[nRow].FindControl("lblExpression")).Text.Split(',');

        PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(arr[1]);
        PhoenixSecurityContext.CurrentSecurityContext.VesselName = arr[2];

        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("txtWorkOrderNumber", arr[0]);
        criteria.Add("txtWorkOrderName", string.Empty);
        criteria.Add("txtComponentNumber", string.Empty);
        criteria.Add("txtComponentName", string.Empty);
        criteria.Add("ucRank", string.Empty);
        criteria.Add("txtDateFrom", string.Empty);
        criteria.Add("txtDateTo", string.Empty);
        criteria.Add("status", string.Empty);
        criteria.Add("planning", string.Empty);
        criteria.Add("jobclass", string.Empty);
        criteria.Add("ucMainType", string.Empty);
        criteria.Add("ucMainCause", string.Empty);
        criteria.Add("ucMaintClass", string.Empty);
        criteria.Add("chkUnexpected", string.Empty);
        criteria.Add("txtPriority", string.Empty);
        criteria.Add("chkDefect", string.Empty);
        Filter.CurrentWorkOrderFilter = criteria;

        DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        PhoenixGeneralSettings.CurrentGeneralSetting = new GeneralSetting(ds);
        SessionUtil.ReBuildMenu();  

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "parent.showVessel();";
        Script += "parent.refreshApplicationTitle();";
        Script += "</script>" + "\n";

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

        Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrder.aspx", false);


    }

    private void ShowOrderForm(object sender, GridViewCommandEventArgs e, string tasktype)
    {
        NameValueCollection criteria = new NameValueCollection();

        int nRow = int.Parse(e.CommandArgument.ToString());
        GridView _gridView = (GridView)sender;

        PhoenixRegistersAlerts.AlertViewHistoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            int.Parse(tasktype), ((Label)_gridView.Rows[nRow].FindControl("lblTaskKey")).Text);

        string[] arr = ((Label)_gridView.Rows[nRow].FindControl("lblExpression")).Text.Split(',');

        PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(arr[2]);
        Filter.CurrentPurchaseVesselSelection = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        PhoenixSecurityContext.CurrentSecurityContext.VesselName = arr[3];

        criteria.Clear();
        criteria.Add("ucVessel", arr[2]);
        criteria.Add("ddlStockType", arr[1]);
        criteria.Add("txtNumber", arr[0]);
        criteria.Add("txtTitle", "");
        criteria.Add("txtVendorid", "");
        criteria.Add("txtDeliveryLocationId", "");
        criteria.Add("txtBudgetId", "");
        criteria.Add("txtBudgetgroupId", "");
        criteria.Add("ucFinacialYear", "");
        criteria.Add("ucFormState", "");
        criteria.Add("ucApproval", "");
        criteria.Add("UCrecieptCondition", "");
        criteria.Add("UCPeority", "");
        criteria.Add("ucFormStatus", "");
        criteria.Add("ucFormType", "");
        criteria.Add("ucComponentclass", "");
        criteria.Add("txtMakerReference", "");
        criteria.Add("txtOrderedDate", "");
        criteria.Add("txtOrderedToDate", "");
        criteria.Add("txtCreatedDate", "");
        criteria.Add("txtCreatedToDate", "");
        criteria.Add("txtApprovedDate", "");
        criteria.Add("txtApprovedToDate", "");
        Filter.CurrentOrderFormFilterCriteria = criteria;

        DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        PhoenixGeneralSettings.CurrentGeneralSetting = new GeneralSetting(ds);
        SessionUtil.ReBuildMenu();

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "parent.showVessel();";        
        Script += "parent.OpenSearchPage('Purchase/PurchaseForm.aspx', 'PUR-FRM');";
        Script += "</script>" + "\n";

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

        //Response.Redirect("../Purchase/PurchaseForm.aspx", false);

    }

    private void ShowAuditReport(object sender, GridViewCommandEventArgs e, string tasktype)
    {
        NameValueCollection criteria = new NameValueCollection();

        int nRow = int.Parse(e.CommandArgument.ToString());
        GridView _gridView = (GridView)sender;

        PhoenixRegistersAlerts.AlertViewHistoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            int.Parse(tasktype), ((Label)_gridView.Rows[nRow].FindControl("lblTaskKey")).Text);

        string[] arr = ((Label)_gridView.Rows[nRow].FindControl("lblExpression")).Text.Split(',');

        PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(arr[1]);        
        PhoenixSecurityContext.CurrentSecurityContext.VesselName = arr[2];

        criteria.Clear();

        criteria.Add("ucAuditType", string.Empty);
        criteria.Add("ucAuditCategory", string.Empty);
        criteria.Add("ucAudit", string.Empty);
        criteria.Add("ucPort", string.Empty);
        criteria.Add("ucStatus", string.Empty);
        criteria.Add("ucVessel", string.Empty);
        criteria.Add("ucTechFleet", string.Empty);
        criteria.Add("txtDoneFrom", string.Empty);
        criteria.Add("txtDoneTo", string.Empty);
        criteria.Add("txtRefNo", arr[0]);
        criteria.Add("ucVesselType", string.Empty);
        criteria.Add("ucAddrOwner", string.Empty);
        criteria.Add("ucCharterer", string.Empty);
        criteria.Add("txtExternalInspector", string.Empty);
        criteria.Add("txtExternalOrganization", string.Empty);
        criteria.Add("ddlInspectorName", string.Empty);
        criteria.Add("ucPortTo", string.Empty);
        criteria.Add("ddlDefType", string.Empty);
        criteria.Add("ucChapter", string.Empty);
        criteria.Add("txtKey", string.Empty);

        Filter.CurrentAuditScheduleFilterCriteria = criteria;

        DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        PhoenixGeneralSettings.CurrentGeneralSetting = new GeneralSetting(ds);
        SessionUtil.ReBuildMenu();

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "parent.showVessel();";
        Script += "parent.OpenSearchPage('Inspection/InspectionAuditRecordList.aspx?callfrom=record&menu=y', 'QUA-ADI-LOG');";
        Script += "</script>" + "\n";

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

        //Response.Redirect("../Inspection/InspectionAuditRecordList.aspx?callfrom=record&menu=y", false);
    }
}
