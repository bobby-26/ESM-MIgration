using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DocumentManagement;
using System.Web;
using System.Drawing;

public partial class Dashboard_DashboardVessel : PhoenixBasePage
{

    DateTime localtime = DateTime.Now;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        btnDMS.Visible = false;// SessionUtil.CanAccess(this.ViewState, "DMS");
        btnDeckLog.Visible = SessionUtil.CanAccess(this.ViewState, "DECKLOG");
        btnEngineLog.Visible = SessionUtil.CanAccess(this.ViewState, "ENGINELOG");
        btnMarpolLog.Visible = SessionUtil.CanAccess(this.ViewState, "MARPOLLOG");        
        lnkDailyWorkPlan.Visible = SessionUtil.CanAccess(this.ViewState, "DAILYWORKPLAN");
        btnNoonReport.Visible = SessionUtil.CanAccess(this.ViewState, "NOONREPORT");
        btnDepartureReport.Visible = SessionUtil.CanAccess(this.ViewState, "DEPARTUREREPORT");
        btnArrivalReport.Visible = SessionUtil.CanAccess(this.ViewState, "ARRIVALREPORT");
        btnShifitingReport.Visible = SessionUtil.CanAccess(this.ViewState, "SHIFITINGREPORT");

        divOperations.Visible = SessionUtil.CanAccess(this.ViewState, "OPERATIONS");
        divOrdersInformation.Visible = SessionUtil.CanAccess(this.ViewState, "ORDERINFORMATION");
        divTimeSheet.Visible = SessionUtil.CanAccess(this.ViewState, "TIMESHEET");
        //divAccidentalNearMissPanel.Visible = SessionUtil.CanAccess(this.ViewState, "ACCIDENTALNEARMISS");
        divFormsRAWorkPermit.Visible = SessionUtil.CanAccess(this.ViewState, "FORMSRAWORKPERMIT");
        divPersonnelOnDuty.Visible = SessionUtil.CanAccess(this.ViewState, "PERSONNELONDUTY");
        divAlertsPanel.Visible = SessionUtil.CanAccess(this.ViewState, "ALERTSPANEL");
        //divMaintenance.Visible = SessionUtil.CanAccess(this.ViewState, "MAINTENANCE");
        divStoresSpares.Visible = SessionUtil.CanAccess(this.ViewState, "STORESSPARES");
        divPlannedMaintenance.Visible = SessionUtil.CanAccess(this.ViewState, "PLANNEDMAINTENANCE");
        divCrewDocPanel.Visible = SessionUtil.CanAccess(this.ViewState, "CREWDOC");
        //divCat4TestsChecks.Visible = SessionUtil.CanAccess(this.ViewState, "CAT4TESTS");


        if (!IsPostBack)
        {
            LoadInfoEPSS();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                //PhoenixDashboardTechnical.AsyncWorkHoursPopulate(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                PhoenixDashboardCrew.AsyncCrewExpiryDocument();
                PhoenixDashboardTechnical.AsyncDailyWorkPlanPopulate(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.Date);
            }
            ViewState["ISOFF"] = "1";
            FetchVesselUser();

        }
        lblVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
        SetVoyageInfo();

        //LoadForm();
        //LoadNonRoutineRA();
        //LoadPTW();
        //LoadOrderInformation();
        //LoadPurchase();
        DMSLoginUser();
        DMSUser();
        if (ViewState["ISOFF"].ToString() != "1")
        {
            gvNearMiss.Visible = false;
            divAlertsPanel.Visible = false;
            divCrewDocPanel.Visible = false;
        }
        else
        {
            //LoadTask();
            //LoadCertificate();
            //LoadExpireDocument();
            //LoadOverduedrillandtrainingdata();
            //LoadUnsafeAccidentNearMiss();
        }                
    }
    private void SetVoyageInfo()
    {
        string template = "<i class=\"fa fa-calendar\"></i>&nbsp;Date : {date} | <i class=\"fa fa-clock - o\"></i>&nbsp;Time : {time} &nbsp;UTC {shipmean}| {from} {to}&nbsp;";
        
        DataTable dt;
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            dt = PhoenixCommonDashboard.VesselOfficeDetailsEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        else
            dt = PhoenixCommonDashboard.FetchVesselTime(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
       
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            template = template.Replace("{date}", General.GetDateTimeToString(dr["FLDLOCALTIME"].ToString()));

            localtime = General.GetNullableDateTime(dr["FLDLOCALTIME"].ToString()) != null ? DateTime.Parse(dr["FLDLOCALTIME"].ToString()) : DateTime.Now;
            Session["localtime"] = localtime.ToString();

            template = template.Replace("{time}", "<a href=\"javascript: var options ={ disableMinMax: true}; top.openNewWindow('smt', 'Ship Mean Time', '" + Session["sitepath"] + "/Dashboard/DashboardRHWorkCalenderGeneral.aspx?shipresthourid=" + dr["FLDSHIPCALENDARID"].ToString() + "', false, '1000', '400', null, null,{helpWinURL:'Help/Panel Overview_Ship Mean Time.pdf'});\" title=\"Change Ship Mean Time\" style=\"color: white; text-decoration: underline;\">" + DateTime.Parse(dr["FLDLOCALTIME"].ToString()).ToString("h:mm tt") + "</a>");
            
            string port = dr["FLDPREVIOUSPORT"].ToString();
           
            template = template.Replace("{shipmean}", General.GetNullableDateTime(dr["FLDUTCTIME"].ToString())!=null ? Convert.ToDateTime(dr["FLDUTCTIME"].ToString()).ToString("dd/MM/yyyy HH:mm") : ""  );
            template = template.Replace("{shipmean}", string.Empty);

            if (dr["FLDVESSELSTATUS"].ToString().ToLower().Replace(" ", "") == "inport")
            {
                template = template.Replace("{from}", "At " + dr["FLDNEXTPORT"].ToString());
                template = template.Replace("{to}", string.Empty);
            }
            else
            {
                DataSet dtsrp = PhoenixDashboardTechnical.ShipRiskProfileInfoEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(dr["FLDNEXTPORTID"].ToString()), General.GetNullableInteger(dr["FLDVESSELID"].ToString()));
                DataRow drsrp = dtsrp.Tables[1].Rows[0];
                template = template.Replace("{from}", "From : " + dr["FLDPREVIOUSPORT"].ToString());

                if (dtsrp.Tables[0].Rows.Count> 0 && dtsrp.Tables[0].Rows[0]["FLDPSCMOU"].ToString() != "")
                {
                    template = template.Replace("{to}", "| To : <a id=\"lnkSRP\" runat=\"server\" style=\"color: {{PortColor}} \" href=\"javascript: top.openNewWindow('dpopup', 'Ship Risk Profile', 'Inspection/InspectionDashboardShipRiskProfile.aspx?Vesselid={{querystring}}',false, 450, 250,'');\">" + dr["FLDNEXTPORT"].ToString()) + "</a>";
                    template = template.Replace("{{PortColor}}", drsrp["FLDPORTCOLOR"].ToString());
                    template = template.Replace("{{querystring}}", dr["FLDVESSELID"].ToString() + "&Portid=" + dr["FLDNEXTPORTID"].ToString() + "&Portname=" + dr["FLDNEXTPORT"].ToString());
                }
                else
                {
                    template = template.Replace("{to}", "| To : " + dr["FLDNEXTPORT"].ToString());
                }
            }
        }
        else
        {
            template = template.Replace("{from}", string.Empty);
            template = template.Replace("{to}", string.Empty);
        }
        ltrlVoyageInfo.Text = template;

    }    
    //private void LoadUnsafeAccidentNearMiss()
    //{
        //DataSet ds = PhoenixDashboardTechnical.DashboardUnsaceAccidentNearMissSummary(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        //DataTable dt = ds.Tables[0];
        //if (dt.Rows.Count == 0)
        //{
        //    dt.Rows.Add(dt.NewRow());
        //    dt.Rows[0]["FLDUNSAFEACTNEW"] = 0;
        //    dt.Rows[0]["FLDUNSAFEACTPENDING"] = 0;
        //}
        //if (dt.Rows.Count > 0)
        //{
        //    DataRow dr = dt.Rows[0];

        //    btnUSNew.InnerHtml = "New" + "<br>" + dr["FLDUNSAFEACTNEW"].ToString();
        //    btnUSNew.Attributes.Add("onclick", "javascript: top.openNewWindow('ucats','UC/UACTs - New(24hrs)','Inspection/InspectionDashboardUnsafeActsConditions.aspx?status=NEW'); return false;");

        //    btnUSPending.InnerHtml = "Pend" + "<br>" + dr["FLDUNSAFEACTPENDING"].ToString();
        //    btnUSPending.Attributes.Add("onclick", "javascript: top.openNewWindow('ucats','UC/UACTs - Pending','Inspection/InspectionDashboardUnsafeActsConditions.aspx?status=PENDING'); return false;");

        //}

        //dt = ds.Tables[1];
        //if (dt.Rows.Count == 0)
        //{
        //    dt.Rows.Add(dt.NewRow());
        //    dt.Rows[0]["FLDNEARMISSNEW"] = 0;
        //    dt.Rows[0]["FLDNEARMISSPENDING"] = 0;
        //    dt.Rows[0]["FLDACCIDENTNEW"] = 0;
        //    dt.Rows[0]["FLDACCIDENTPENDING"] = 0;
        //}
        //if (dt.Rows.Count > 0)
        //{
        //    DataRow dr = dt.Rows[0];

        //    btnNMNew.InnerHtml = "New" + "<br>" + dr["FLDNEARMISSNEW"].ToString();
        //    btnNMNew.Attributes.Add("onclick", "javascript: top.openNewWindow('Incident','Near Miss - New(24hrs)','Inspection/InspectionDashboardIncidentNearMissList.aspx?code=NMNEW'); return false;");

        //    btnNMPending.InnerHtml = "Pend" + "<br>" + dr["FLDNEARMISSPENDING"].ToString();
        //    btnNMPending.Attributes.Add("onclick", "javascript: top.openNewWindow('Incident','Near Miss - Pending','Inspection/InspectionDashboardIncidentNearMissList.aspx?code=NMPENDING'); return false;");

        //    btnINCNew.InnerHtml = "New" + "<br>" + dr["FLDACCIDENTNEW"].ToString();
        //    btnINCNew.Attributes.Add("onclick", "javascript: top.openNewWindow('Incident','Accident - New(24hrs)','Inspection/InspectionDashboardIncidentNearMissList.aspx?code=INCNEW'); return false;");

        //    btnINCPending.InnerHtml = "Pend" + "<br>" + dr["FLDACCIDENTPENDING"].ToString();
        //    btnINCPending.Attributes.Add("onclick", "javascript: top.openNewWindow('Incident','Accident - Pending','Inspection/InspectionDashboardIncidentNearMissList.aspx?code=INCPENDING'); return false;");
        //}
    //}

    //private void LoadPurchase()
    //{
        //string url = "javascript:top.openNewWindow('detail','Store & Spares','Purchase/PurchaseForm.aspx');";
        //DataSet ds = PhoenixDashboardTechnical.DashboardPurhaseSummary(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        //if (ds.Tables.Count > 0)
        //{
        //    DataRow dr = ds.Tables[0].Rows[0];
        //    btnReqProgress.InnerText = dr["FLDREQPRG"].ToString();
        //    btnReqProgress.Attributes.Add("onclick", "javascript:top.openNewWindow('detail','Store & Spares','Purchase/PurchaseForm.aspx?dfilter=FLDREQPRG&vslid="+PhoenixSecurityContext.CurrentSecurityContext.VesselID+"');");

        //    dr = ds.Tables[1].Rows[0];
        //    btnReqPen.InnerText = dr["FLDREQPEN"].ToString();
        //    btnReqPen.Attributes.Add("onclick", "javascript:top.openNewWindow('detail','Store & Spares','Purchase/PurchaseForm.aspx?dfilter=FLDREQPEN&vslid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID+"');");

        //    dr = ds.Tables[2].Rows[0];
        //    btnReqPenAck.InnerText = dr["FLDREQPENACK"].ToString();
        //    btnReqPenAck.Attributes.Add("onclick", "javascript:top.openNewWindow('detail','Store & Spares','Purchase/PurchaseForm.aspx?dfilter=FLDREQPENACK&vslid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID+"');");
        //}
    //}
    //private void LoadMaitenanceSummary()
    //{
    //    string url = "javascript:top.openNewWindow('maint','Maintenance','Dashboard/DashboardTechnicalMaintenance.aspx?status={{status}}',true);";
    //    DataSet ds = PhoenixDashboardTechnical.DashboardMaintenanceSummary(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //    {
    //        DataRow dr = ds.Tables[0].Rows[0];
    //        btnMaintenanceProgress.InnerText = dr["FLDINPROGRESS"].ToString().Equals("") ? "0" : dr["FLDINPROGRESS"].ToString();
    //        btnMaintenanceProgress.Attributes.Add("onclick", url.Replace("{{status}}", dr["FLDINPROGRESSSTATUS"].ToString()));
    //        btnMaintenancePlanned.InnerText = dr["FLDPLANNED"].ToString().Equals("") ? "0" : dr["FLDPLANNED"].ToString();
    //        btnMaintenancePlanned.Attributes.Add("onclick", url.Replace("{{status}}", dr["FLDPLANNEDSTATUS"].ToString()));
    //        btnReportPending.InnerText = dr["FLDREPORTPENDING"].ToString().Equals("") ? "0" : dr["FLDREPORTPENDING"].ToString();
    //        btnReportPending.Attributes.Add("onclick", url.Replace("{{status}}", dr["FLDREPORTPENDINGSTATUS"].ToString()));
    //        btnMaintenanceCompleted.InnerText = dr["FLDCOMPLETED"].ToString().Equals("") ? "0" : dr["FLDCOMPLETED"].ToString();
    //        btnMaintenanceCompleted.Attributes.Add("onclick", url.Replace("{{status}}", dr["FLDCOMPLETEDSTATUS"].ToString() + "&ft=t"));

    //        url = "javascript:top.openNewWindow('maint','Operation','Dashboard/DashboardTechnicalOperation.aspx?status={{status}}',true);";
    //        dr = ds.Tables[1].Rows[0];
    //        btnOperationProgress.InnerText = dr["FLDINPROGRESS"].ToString().Equals("") ? "0" : dr["FLDINPROGRESS"].ToString();
    //        btnOperationProgress.Attributes.Add("onclick", url.Replace("{{status}}", dr["FLDINPROGRESSSTATUS"].ToString()));
    //        btnOperationPlanned.InnerText = dr["FLDPLANNED"].ToString().Equals("") ? "0" : dr["FLDPLANNED"].ToString();
    //        btnOperationPlanned.Attributes.Add("onclick", url.Replace("{{status}}", dr["FLDPLANNEDSTATUS"].ToString()));
    //        btnOperationCompleted.InnerText = dr["FLDCOMPLETED"].ToString().Equals("") ? "0" : dr["FLDCOMPLETED"].ToString();
    //        btnOperationCompleted.Attributes.Add("onclick", url.Replace("{{status}}", dr["FLDCOMPLETEDSTATUS"].ToString() + "&ft=t"));
    //    }
    //}
    protected void NoonReport_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/VesselPosition/VesselPositionNoonReportList.aspx");
    }
    protected void DepartureReport_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/VesselPosition/VesselPositionDepartureReport.aspx");
    }
    protected void ArrivalReport_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/VesselPosition/VesselPositionArrivalReport.aspx");
    }
    protected void Shifting_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/VesselPosition/VesselPositionShiftingReport.aspx");
    }
    protected void Drill_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/Inspection/InspectionDrillSchedule.aspx");
    }

    protected void gvTimeSheet_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataTable dt = PhoenixPlannedMaintenanceTimeSheet.Search(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        , General.GetNullableDateTime(General.GetDateTimeToString(DateTime.Now.ToString()))
                                        , General.GetNullableDateTime(General.GetDateTimeToString(DateTime.Now.ToString()))
                                        , sortexpression, sortdirection
                                        , grid.CurrentPageIndex + 1
                                        , grid.PageSize
                                        , ref iRowCount
                                        , ref iTotalPageCount);

        grid.DataSource = dt;
        grid.VirtualItemCount = iRowCount;
    }
    protected void gvTimeSheet_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (drv != null && drv["FLDISDELETED"].ToString() == "1")
        {
            e.Item.Attributes.Add("style", "text-decoration: line-through;");
        }
    }
    //private void LoadTask()
    //{
    //    string url = "javascript:top.openNewWindow('maint','Shipboard Tasks - {{due}}','Inspection/InspectionDashboardShipBoardTasks.aspx{{overdue}}');";
    //    DataSet ds = PhoenixDashboardTechnical.DashboardTaskSummary(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    //    if (ds.Tables.Count > 0)
    //    {
    //        DataRow dr = ds.Tables[0].Rows[0];
    //        btnTaskDue.InnerHtml = "Due<br />" + (dr["FLDDUE"].ToString().Equals("") ? "0" : dr["FLDDUE"].ToString());
    //        btnTaskDue.Attributes.Add("onclick", url.Replace("{{due}}", "Due").Replace("{{overdue}}", "?OVERDUEYN=1"));

    //        btnTaskDue15.InnerHtml = "15D<br />" + (dr["FLDDUE15DAYS"].ToString().Equals("") ? "0" : dr["FLDDUE15DAYS"].ToString());
    //        btnTaskDue15.Attributes.Add("onclick", url.Replace("{{due}}", "Due 15D").Replace("{{overdue}}", ""));
    //    }
    //}
    protected void BtnCrewList_Click(object sender, EventArgs e)
    {
        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("ddlVessel", PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
        Filter.CurrentCrewListSelection = nvc;
        string script = "top.openNewWindow('crwlst', 'Crew List', 'Crew/CrewList.aspx');";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void RadPivotGrid1_NeedDataSource(object sender, PivotGridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixDashboardTechnical.DashboardPersonalOnDuty(PhoenixSecurityContext.CurrentSecurityContext.VesselID
            , localtime, localtime.Hour + 1);
        RadPivotGrid1.DataSource = ds;
    }

    protected void RadPivotGrid1_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
    {
        RadPivotGrid grid = (RadPivotGrid)sender;
        if (e.Cell is PivotGridDataCell)
        {
            PivotGridDataCell cell = (PivotGridDataCell)e.Cell;
            if (cell.CellType == PivotGridDataCellType.DataCell)
            {
                DataSet ds = (DataSet)grid.DataSource;
                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];
                string row = cell.ParentRowIndexes.Length > 0 ? cell.ParentRowIndexes[0].ToString() : string.Empty;
                string col = cell.ParentColumnIndexes.Length > 0 ? cell.ParentColumnIndexes[0].ToString() : string.Empty;
                DataRow[] dr = dt.Select("FLDMANAGEMENT = '" + row + "' AND FLDDEPARTMENT='" + col + "'");
                string employees = string.Empty;
                foreach (DataRow d in dr)
                {
                    //string remainnghours = string.Empty;
                    //DataRow[] rdr = dt1.Select("FLDEMPLOYEEID = " + d["FLDEMPLOYEEID"].ToString() + "");
                    //if (rdr.Length > 0)
                    //    remainnghours = rdr[0]["FLDREMAININGHOURS"].ToString();
                    employees += "<a href=\"javascript:top.openNewWindow('wo', 'Off Duty', 'Dashboard/DashboardRestHourDuty.aspx?d=off&e=" + d["FLDEMPLOYEEID"].ToString() + "',false,400,300);\" >" + d["FLDEMPLOYEENAME"].ToString()  + "</a><br />";
                }
                cell.Text = employees.Equals("") ? "N/A" : employees;
            }
        }
    }
    
    private void LoadForm()
    {
        //DataTable dt = PhoenixDashboardTechnical.DashboardFormCheckListDue(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        //btnFormCheckListDue.InnerHtml = "Req - " + dt.Rows.Count.ToString();
        //dt = PhoenixDashboardTechnical.DashboardFormCheckListStatusList(PhoenixSecurityContext.CurrentSecurityContext.VesselID, "INUSE");
        //btnFormCheckListInUse.InnerHtml = "In Use - " + dt.Rows.Count.ToString();
        //dt = PhoenixDashboardTechnical.DashboardFormCheckListStatusList(PhoenixSecurityContext.CurrentSecurityContext.VesselID, "APPROVAL");
        //btnFormCheckListApproval.InnerHtml = "Pndg Review - " + dt.Rows.Count.ToString();
    }
    private void LoadOrderInformation()
    {
        //string url = "javascript:top.openNewWindow('maint','Orders & Information - {{caption}}','PlannedMaintenance/PlannedMaintenanceOrderInformation.aspx?t={{type}}&status=1,2,3&fd={{date}}');";
        //DataTable dt = PhoenixDashboardTechnical.DashboardOrderInformation(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        //btnMasterOrders.Attributes.Add("onclick", url.Replace("{{type}}", "1").Replace("{{caption}}", "Master\\'s Orders"));
        //btnCEOrders.Attributes.Add("onclick", url.Replace("{{type}}", "2").Replace("{{caption}}", "C/E\\'s Orders"));
        //btnCOOrders.Attributes.Add("onclick", url.Replace("{{type}}", "3").Replace("{{caption}}", "C/O\\'s Orders"));
        //btnMasterOrders.InnerHtml = "0";
        //btnCEOrders.InnerHtml = "0";
        //btnCOOrders.InnerHtml = "0";
        //foreach (DataRow dr in dt.Rows)
        //{
        //    if (dr["FLDTYPE"].ToString() == "1")
        //    {
        //        btnMasterOrders.InnerHtml = dr["FLDCOUNT"].ToString();
        //        btnMasterOrders.Attributes["onclick"] = btnMasterOrders.Attributes["onclick"].Replace("{{date}}", General.GetDateTimeToString(dr["FLDFROMDDATE"]));
        //    }
        //    if (dr["FLDTYPE"].ToString() == "2")
        //    {
        //        btnCEOrders.InnerHtml = dr["FLDCOUNT"].ToString();
        //        btnCEOrders.Attributes["onclick"] = btnCEOrders.Attributes["onclick"].Replace("{{date}}", General.GetDateTimeToString(dr["FLDFROMDDATE"]));
        //    }
        //    if (dr["FLDTYPE"].ToString() == "3")
        //    {
        //        btnCOOrders.InnerHtml = dr["FLDCOUNT"].ToString();
        //        btnCOOrders.Attributes["onclick"] = btnCOOrders.Attributes["onclick"].Replace("{{date}}", General.GetDateTimeToString(dr["FLDFROMDDATE"]));
        //    }
        //}
        //btnMasterOrders.Attributes["onclick"] = btnMasterOrders.Attributes["onclick"].Replace("{{date}}", string.Empty);
        //btnCEOrders.Attributes["onclick"] = btnCEOrders.Attributes["onclick"].Replace("{{date}}", string.Empty);
        //btnCOOrders.Attributes["onclick"] = btnCOOrders.Attributes["onclick"].Replace("{{date}}", string.Empty);
    }
    private void LoadNonRoutineRA()
    {
        //int iRowCount = 0;
        //int iTotalPageCount = 0;
        //PhoenixDashboardTechnical.DashboardNonRoutineRASearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(30)
        //                                , string.Empty, null
        //                                , 1
        //                                , 10
        //                                , ref iRowCount
        //                                , ref iTotalPageCount);

        //btnRADue.InnerHtml = "Req - " + iRowCount.ToString();
        //DataTable dt = PhoenixDashboardTechnical.DashboardNonRoutineRAStatusCount(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(30));
        //if (dt.Rows.Count > 0)
        //{
        //    DataRow dr = dt.Rows[0];
        //    btnRADRT.InnerHtml = "In Use - " + dr["FLDDRAFTCOUNT"].ToString();
        //    btnRADRT.Attributes.Add("onclick", "javascript: top.openNewWindow('DailyWorkPlan','Non Routine RAs - In Use','Dashboard/DashboardTechnicalRAStatus.aspx?c=DRT'); return false;");

        //    btnRAAFU.InnerHtml = "Pndg Approval - " + dr["FLDPENDINGCOUNT"].ToString();
        //    btnRAAFU.Attributes.Add("onclick", "javascript: top.openNewWindow('DailyWorkPlan','Non Routine RAs - Pending Approval','Dashboard/DashboardTechnicalRAStatus.aspx?c=POC'); return false;");

        //    btnRAApproved.InnerHtml = "Apprvd - " + dr["FLDAPPROVEDCOUNT"].ToString();
        //    btnRAApproved.Attributes.Add("onclick", "javascript: top.openNewWindow('DailyWorkPlan','Non Routine RAs - Approvd','Dashboard/DashboardTechnicalRAStatus.aspx?c=AFU'); return false;");

        //    btnRAExpired.InnerHtml = "Expir - " + dr["FLDEXPIREDCOUNT"].ToString();
        //    btnRAExpired.Attributes.Add("onclick", "javascript: top.openNewWindow('DailyWorkPlan','Non Routine RAs - Expired','Dashboard/DashboardTechnicalRAStatus.aspx?c=EXP'); return false;");

        //}
    }
    private void LoadPTW()
    {
        //int iRowCount = 0;
        //int iTotalPageCount = 0;
        //DataTable dt = PhoenixDashboardTechnical.DashboardPTWDueSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(7)
        //                                , null, null
        //                                , 1
        //                                , 10
        //                                , ref iRowCount
        //                                , ref iTotalPageCount);

        //btnPTWDue.InnerHtml = "Req - " + iRowCount.ToString();
        //dt = PhoenixDashboardTechnical.DashboardPTWStatusSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(7), "1,4"
        //    , null, null, 1, 10, ref iRowCount, ref iTotalPageCount);
        //btnPTWInUse.InnerHtml = "In Use - " + iRowCount.ToString();
        //dt = PhoenixDashboardTechnical.DashboardPTWStatusSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(7), "5"
        //    , null, null, 1, 10, ref iRowCount, ref iTotalPageCount);
        //btnPTWExpires.InnerHtml = "Expir - " + iRowCount.ToString();
    }
    //private void LoadCertificate()
    //{
    //    DataTable dt = PhoenixDashboardTechnical.DashboardCertificateDueCount(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    //    foreach (DataRow dr in dt.Rows)
    //    {
    //        Control r = Page.FindControl("btnCertificateDue" + dr["FLDDAYS"].ToString());
    //        if (r != null)
    //        {
    //            HtmlButton btn = (HtmlButton)r;
    //            btn.InnerHtml = dr["FLDCAPTION"].ToString() + "<br />" + dr["FLDCOUNT"].ToString();
    //            btn.Attributes.Add("onclick", "javascript: top.openNewWindow('DailyWorkPlan','Certificates','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx?d=" + dr["FLDDAYS"].ToString()+ "')");
    //        }
    //    }
    //}
    protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        if (e.Argument.ToUpper() == "OPERATION")
        {
            gvMaintOperation.Rebind();
        }
        if (e.Argument.ToUpper() == "ORDERSINFOMATION")
        {
            gvOrderInfo.Rebind();
        }
        if(e.Argument.ToUpper() == "PURCHASE")
        {
            gvSpareStores.Rebind();
        }
        if (e.Argument.ToUpper() == "ACCIDENTSNEARMISSES")
        {
            gvNearMiss.Rebind();
        }
        if (e.Argument.ToUpper() == "WRH")
        {
            RadPivotGrid1.Rebind();
        }
        if (e.Argument.ToUpper() == "TIMESHEET")
        {
            gvTimeSheet.Rebind();
        }
        if (e.Argument.ToUpper() == "SMT")
        {
            SetVoyageInfo();
            RadPivotGrid1.Rebind();
        }
    }
    //private void LoadExpireDocument()
    //{
    //    DataTable dt = PhoenixDashboardCrew.CrewExpiryDocumentSummary(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    //    if (dt.Rows.Count > 0)
    //    {
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            Control r = Page.FindControl("btncrewdoc" + dr["FLDDAYS"].ToString());
    //            if (r != null)
    //            {
    //                HtmlButton btn = (HtmlButton)r;
    //                btn.InnerHtml = dr["FLDCAPTION"].ToString() + "<br />" + dr["FLDCOUNT"].ToString();
    //                btn.Attributes.Add("onclick", "javascript: top.openNewWindow('DailyWorkPlan','Certificates','" + Session["sitepath"] + "/Dashboard/DashboardCrewExpiryDocumentDetails.aspx?vid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "&due=" + dr["FLDDAYS"].ToString() + "')");
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Control r = Page.FindControl("btncrewdoc0");
    //        HtmlButton btn = (HtmlButton)r;
    //        btn.Attributes.Add("onclick", "javascript: top.openNewWindow('DailyWorkPlan','Certificates','" + Session["sitepath"] + "/Dashboard/DashboardCrewExpiryDocumentDetails.aspx?vid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "&due=0')");
    //        Control r1 = Page.FindControl("btncrewdoc15");
    //        HtmlButton btn1 = (HtmlButton)r1;
    //        btn1.Attributes.Add("onclick", "javascript: top.openNewWindow('DailyWorkPlan','Certificates','" + Session["sitepath"] + "/Dashboard/DashboardCrewExpiryDocumentDetails.aspx?vid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "&due=15')");
    //        Control r2 = Page.FindControl("btncrewdoc45");
    //        HtmlButton btn2 = (HtmlButton)r2;
    //        btn2.Attributes.Add("onclick", "javascript: top.openNewWindow('DailyWorkPlan','Certificates','" + Session["sitepath"] + "/Dashboard/DashboardCrewExpiryDocumentDetails.aspx?vid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "&due=45')");
    //    }
    //}

    //protected void btnReqProgress_ServerClick(object sender, EventArgs e)
    //{
    //    Filter.CurrentOrderFormFilterCriteria = null;
    //    Filter.CurrentPurchaseDashboardCode = "FLDREQPRG";
    //    Response.Redirect("~/Purchase/PurchaseForm.aspx");
    //}

    //protected void btnReqPen_ServerClick(object sender, EventArgs e)
    //{
    //    Filter.CurrentOrderFormFilterCriteria = null;
    //    Filter.CurrentPurchaseDashboardCode = "FLDREQPEN";
    //    Response.Redirect("~/Purchase/PurchaseForm.aspx");
    //}

    //protected void btnReqPenAck_ServerClick(object sender, EventArgs e)
    //{
    //    Filter.CurrentOrderFormFilterCriteria = null;
    //    Filter.CurrentPurchaseDashboardCode = "FLDREQPENACK";
    //    Response.Redirect("~/Purchase/PurchaseForm.aspx");
    //}

    protected void DrillTraining_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/Inspection/InspectionTrainingSchedule.aspx");
    }

    //public void LoadOverduedrillandtrainingdata()
    //{
    //    int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
    //    int vesslid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
    //    int overduedrills = 0;
    //    int overduetraining = 0;
    //    PhoenixInspectionDrillSummary.Overduedrillcount(rowusercode, vesslid, ref overduedrills);
    //    PhoenixInspectionTrainingSummary.Overduetrainingcount(rowusercode, vesslid, ref overduetraining);



    //    Control r = Page.FindControl("btnoverduedrills");
    //    HtmlButton btn = (HtmlButton)r;
    //    btn.InnerHtml = "Due" + "<br/>" + overduedrills.ToString();

    //    Control r1 = Page.FindControl("btnoverduetraining");
    //    HtmlButton btn1 = (HtmlButton)r1;
    //    btn1.InnerHtml = "Due" + "<br/>" + overduetraining.ToString();

    //}
    private void LoadInfoEPSS()
    {
        DataTable dt = PhoenixDashboardOption.ListDashboardInfoEPSS();
        foreach (DataRow dr in dt.Rows)
        {
            Control ctrl = Page.FindControl(dr["FLDDASHBOARDID"].ToString() + "Info");
            if (ctrl != null)
            {
                HtmlGenericControl gc = (HtmlGenericControl)ctrl;
                if (!dr["FLDHELPLINK"].ToString().Equals(""))
                {
                    gc.Attributes.Add("onclick", "javascript: top.openNewWindow('EPSSINFO','" + dr["FLDDASHBOARD"].ToString() + "','" + dr["FLDHELPLINK"].ToString() + "'); return false;");
                }
                else
                {
                    gc.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('EPSSINFO','" + dr["FLDDASHBOARD"].ToString() + "','" + Session["sitepath"] + "/Dashboard/DashboardInfo.aspx?id=" + dr["FLDID"].ToString() + "',false, 320, 250,'','',options); return false;");
                }
                gc.Visible = true;
                gc.Attributes["style"] = "padding: 0px 5px; font-size: 10px;cursor:pointer";

            }
            ctrl = Page.FindControl(dr["FLDDASHBOARDID"].ToString() + "EPSS");
            if (ctrl != null && dr["FLDEPSSLINK"].ToString() != string.Empty)
            {
                HtmlGenericControl lnk = (HtmlGenericControl)ctrl;
                //lnk.HRef = dr["FLDEPSSLINK"].ToString();
                lnk.Visible = true;
                lnk.Attributes.Add("style", "margin-left: 1px;padding: 0px 5px; font-size: 10px;cursor:pointer");
                lnk.Attributes.Add("onclick", "javascript: top.openNewWindow('EPSSINFO','" + dr["FLDDASHBOARD"].ToString() + "','" + dr["FLDEPSSLINK"].ToString() + "'); return false;");
            }
        }
    }
    protected void GvPMS_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        DataTable dt = PhoenixDashboardTechnical.DashboardVesselPMS(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        var halfDT = dt.Copy();
        if (halfDT.Rows.Count > 0)
        {
            var lastRowIndex = halfDT.Rows.Count - 1;
            var halfwayIndex = halfDT.Rows.Count / 2;
            if (grid.ClientID.ToLower() == "gvpms")
            {
                for (int i = lastRowIndex; i > halfwayIndex; i--)
                {
                    halfDT.Rows.RemoveAt(i);
                }
                halfDT.AcceptChanges();
            }
            else
            {
                for (int i = halfwayIndex; i > -1; i--)
                {
                    halfDT.Rows.RemoveAt(i);
                }
                halfDT.AcceptChanges();
            }
        }
        grid.DataSource = halfDT;
    }
    protected void GvPMS_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem item = e.Item as GridDataItem;
            LinkButton cnt = (LinkButton)item.FindControl("lnkCount");

            if (drv["FLDURL"].ToString() != string.Empty && cnt != null)
            {
                //string querystring = "?code=" + drv["FLDMEASURECODE"].ToString();
                //string link = drv["FLDURL"].ToString();
                //int index = link.IndexOf('?');
                //if (index > -1)
                //{
                //    querystring = querystring.Replace("?", "&");
                //}
                //cnt.Attributes["onclick"] = "javascript: top.openNewWindow('detail','" + drv["FLDMEASURE"].ToString() + "','" + link + querystring + "'); return false;";
            }
            else
            {
                cnt.Enabled = false;
                cnt.Attributes["style"] = "color: black";
            }
        }
    }
    private void FetchVesselUser()
    {
        DataTable dt = PhoenixDashboardTechnical.FetchDashboardVesselUser();
        foreach (DataRow dr in dt.Rows)
        {
            ViewState["ISOFF"] = dr["FLDISOFFICER"].ToString();
        }
    }

    protected void GvPMS_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridDataItem item = (GridDataItem)e.Item;
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper() == "MEASURE")
        {
            string link = ((RadLabel)item.FindControl("lblURL")).Text;
            string querystring = "?code=" + ((RadLabel)item.FindControl("lblMeasureCode")).Text;
            string measure = ((RadLabel)item.FindControl("lblMeasure")).Text;
            if (link != string.Empty)
            {
                int index = link.IndexOf('?');
                if (index > -1)
                {
                    querystring = querystring.Replace("?", "&");
                }
                Filter.CurrentWorkOrderFilter = null;
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", "javascript: top.openNewWindow('detail','" + measure + "','" + link + querystring + "&title="+ measure + "');", true);
            }
        }
    }
    private void DMSUser()
    {
        //DataTable dt = PhoenixDocumentManagementDashBoard.DocumentVesselUnreadCount(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        //DataRow dr = dt.Rows[0];
        //btnHSEQA.InnerHtml = dr["FLDVESSELUNREADCOUNT"].ToString();

        //btnHSEQA.Attributes.Add("onclick", "javascript:parent.openNewWindow('Users','Unread User List','DocumentManagement/DocumentManegementDashBoardUnreadUserList.aspx'); return false;");
    }
    private void DMSLoginUser()
    {
        //DataTable dt = PhoenixDocumentManagementDashBoard.DocumentVesselLoginUnreadCount();
        //if (dt.Rows.Count > 0)

        //{
        //    DataRow dr = dt.Rows[0];
        //    btnHSEQAInfo.InnerHtml = dr["FLDUNREADCOUNT"].ToString();

        //    btnHSEQAInfo.Attributes.Add("onclick", "javascript:parent.openNewWindow('Users','Unread Document List','DocumentManagement/DocumentManagementDashboardUserUnreadDocuments.aspx'); return false;");

        //    if (dr["FLDHSEQADASHBOARDYN"].ToString() == "0" && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        //    {
        //        btnHSEQA.Visible = false;
        //    }


        //}
        //else
        //{
        //    btnHSEQAInfo.InnerHtml = "0";
        //}
    }
    protected void gvAlerts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        DataTable dt = PhoenixDashboardTechnical.ListCrewDocumentAlert(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        grid.DataSource = dt;
    }

    protected void gvAlerts_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkOverdue = (LinkButton)e.Item.FindControl("lnkOverdue");
            LinkButton lnk30Days = (LinkButton)e.Item.FindControl("lnk15Days");
            LinkButton lnk60Days = (LinkButton)e.Item.FindControl("lnk45Days");

            RadLabel lblOverdueurl = (RadLabel)e.Item.FindControl("lblOverdueurl");
            RadLabel lbl30Daysurl = (RadLabel)e.Item.FindControl("lbl15Daysurl");
            RadLabel lbl60Daysurl = (RadLabel)e.Item.FindControl("lbl45Daysurl");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            if ((lnkOverdue != null) && (lnkOverdue.Text != "") && (lnkOverdue.Text != null))
            {
                lnkOverdue.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Overdue','" + lblOverdueurl.Text + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - Overdue'); return false;");
                if (!string.IsNullOrEmpty(drv["FLDOVERDUECOUNT"].ToString()) && int.Parse(drv["FLDOVERDUECOUNT"].ToString()) > 0)
                    lnkOverdue.ForeColor = System.Drawing.Color.Red;
            }

            if ((lnk30Days != null) && (lnk30Days.Text != "") && (lnk30Days.Text != null))
            {
                lnk30Days.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - 30 Days','" + lbl30Daysurl.Text + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - 30 Days'); return false;");
            }

            if ((lnk60Days != null) && (lnk60Days.Text != "") && (lnk60Days.Text != null))
            {
                lnk60Days.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - 60 Days','" + lbl60Daysurl.Text + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - 60 Days'); return false;");
            }
            if(drv["FLDOVERDUECOUNT"].ToString().Equals(""))
            {
                lnkOverdue.Text = "N/A";
                lnkOverdue.Enabled = false;
                lnkOverdue.ForeColor = Color.FromName("#1e395b");
            }
            if (drv["FLD15COUNT"].ToString().Equals(""))
            {
                lnk30Days.Text = "N/A";
                lnk30Days.Enabled = false;
                lnk30Days.ForeColor = Color.FromName("#1e395b");
            }
            if (drv["FLD45COUNT"].ToString().Equals(""))
            {
                lnk60Days.Text = "N/A";
                lnk60Days.Enabled = false;
                lnk60Days.ForeColor = Color.FromName("#1e395b");
            }
        }
    }

    protected void gvWRH_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        DataTable dt = PhoenixDashboardTechnical.ListWorkandRestHourtAlert(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        grid.DataSource = dt;
    }

    protected void gvWRH_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkSeafarerCount = (LinkButton)e.Item.FindControl("lnkSeafarerCount");
            LinkButton lnkHODCount = (LinkButton)e.Item.FindControl("lnkHODCount");
            LinkButton lblMasterCount = (LinkButton)e.Item.FindControl("lblMasterCount");

            RadLabel lnkSeafarerUrl = (RadLabel)e.Item.FindControl("lblOverdueurl");
            RadLabel lnkHODUrl = (RadLabel)e.Item.FindControl("lnkHODUrl");
            RadLabel lblMasterUrl = (RadLabel)e.Item.FindControl("lblMasterUrl");
            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            if ((lnkSeafarerCount != null) && (lnkSeafarerCount.Text != "") && (lnkSeafarerCount.Text != null) && drv["FLDSEAFARERURL"].ToString() != string.Empty)
            {
                lnkSeafarerCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Seafarer','" + drv["FLDSEAFARERURL"] + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - Overdue'); return false;");                
            }
            else
            {
                lnkSeafarerCount.Enabled = false;
                lnkSeafarerCount.ForeColor = Color.FromName("#1e395b");
            }
            if ((lnkHODCount != null) && (lnkHODCount.Text != "") && (lnkHODCount.Text != null) && drv["FLDHODURL"].ToString() != string.Empty)
            {
                lnkHODCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - HOD','" + drv["FLDHODURL"] + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - 30 Days'); return false;");
            }
            else
            {
                lnkHODCount.Enabled = false;
                lnkHODCount.ForeColor = Color.FromName("#1e395b");
            }
            if ((lblMasterCount != null) && (lblMasterCount.Text != "") && (lblMasterCount.Text != null) && drv["FLDMASTERURL"].ToString() != string.Empty)
            {
                lblMasterCount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + " - Master','" + drv["FLDMASTERURL"] + "&title=" + HttpUtility.UrlEncode(lblmeasure.Text) + " - 60 Days'); return false;");
            }
            else
            {
                lblMasterCount.Enabled = false;
                lblMasterCount.ForeColor = Color.FromName("#1e395b");
            }
            if (drv["FLDSEAFARERCOUNT"].ToString().Equals(""))
            {
                lnkSeafarerCount.Text = "N/A";
                lnkSeafarerCount.Enabled = false;
                lnkSeafarerCount.ForeColor = Color.FromName("#1e395b");
            }
            if (drv["FLDHODCOUNT"].ToString().Equals(""))
            {
                lnkHODCount.Text = "N/A";
                lnkHODCount.Enabled = false;
                lnkHODCount.ForeColor = Color.FromName("#1e395b");
            }
            if (drv["FLDMASTERCOUNT"].ToString().Equals(""))
            {
                lblMasterCount.Text = "N/A";
                lblMasterCount.Enabled = false;
                lblMasterCount.ForeColor = Color.FromName("#1e395b");
            }
        }
    }

    protected void gvMaintOperation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        DataSet dt = PhoenixDashboardTechnical.DashboardMaintenanceSummary(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        grid.DataSource = dt;
    }

    protected void gvMaintOperation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkMaintenanceCount = (LinkButton)e.Item.FindControl("lnkMaintenanceCount");
            LinkButton lnkOperationCount = (LinkButton)e.Item.FindControl("lnkOperationCount");
            if (drv["FLDTITLE"].ToString().ToUpper().Contains("COMPLETED"))
            {
                lnkMaintenanceCount.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Maintenance - " + drv["FLDTITLE"] + "','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalMaintenance.aspx?status=" + drv["FLDHARDCODE"] + "&ft=t',true); return false;");
                lnkOperationCount.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Operation - " + drv["FLDTITLE"] + "','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalOperation.aspx?status=" + drv["FLDHARDCODE"] + "&ft=t',true); return false;");
            }
            else
            {
                lnkMaintenanceCount.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Maintenance - " + drv["FLDTITLE"] + "','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalMaintenance.aspx?status=" + drv["FLDHARDCODE"] + "&td=t',true); return false;");
                lnkOperationCount.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Operation - " + drv["FLDTITLE"] + "','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalOperation.aspx?status=" + drv["FLDHARDCODE"] + "&td=t',true); return false;");
            }
            if (drv["FLDOPERATIONCOUNT"].ToString().Equals(""))
            {
                lnkOperationCount.Text = "N/A";
                lnkOperationCount.Enabled = false;
                lnkOperationCount.ForeColor = Color.FromName("#1e395b");
                lnkOperationCount.Attributes.Remove("onclick");
            }
        }
    }

    protected void gvOrderInfo_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;      
        DataTable dt = PhoenixDashboardTechnical.DashboardOrderInformation(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        DataTable dt1 = PhoenixDocumentManagementDashBoard.DocumentVesselLoginUnreadCount();
        DataTable dt2 = PhoenixDocumentManagementDashBoard.DocumentVesselUnreadCount(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        DataRow dr = dt.NewRow();
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            dr["FLDTYPE"] = -1;
            dr["FLDTYPENAME"] = "Unread Document List";
            dr["FLDCOUNT"] = (dt1.Rows.Count > 0 ? dt1.Rows[0]["FLDUNREADCOUNT"].ToString() : "0");
            dt.Rows.Add(dr);
        }
        string HSEQADashboardyn = dt2.Rows.Count > 0 ? dt2.Rows[0]["FLDHSEQADASHBOARDYN"].ToString() : "0";

        if (HSEQADashboardyn != "0" || PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            dr = dt.NewRow();
            dr["FLDTYPE"] = -2;
            dr["FLDTYPENAME"] = "Unread User List";
            dr["FLDCOUNT"] = (dt2.Rows.Count > 0 ? dt2.Rows[0]["FLDVESSELUNREADCOUNT"].ToString() : "0");
            dt.Rows.Add(dr);
        }
        grid.DataSource = dt;
    }

    protected void gvOrderInfo_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkCount = (LinkButton)e.Item.FindControl("lnkCount");
            if (drv["FLDTYPE"].ToString() == "-1")
            {
                lnkCount.Attributes.Add("onclick", "javascript:top.openNewWindow('Users','Unread Document List','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDashboardUserUnreadDocuments.aspx',true); return false;");
            }
            else if (drv["FLDTYPE"].ToString() == "-2")
            {
                lnkCount.Attributes.Add("onclick", "javascript:top.openNewWindow('Users','Unread User List','" + Session["sitepath"] + "/DocumentManagement/DocumentManegementDashBoardUnreadUserList.aspx',true); return false;");
            }
            else
            {
                lnkCount.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Orders & Information - " + drv["FLDTYPENAME"] + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceOrderInformation.aspx?t=" + drv["FLDTYPE"] + "&status=1,2,3&fd=" + General.GetDateTimeToString(drv["FLDFROMDDATE"]) + "',true); return false;");
            }
        }
    }
    private DataTable GetFormsRA()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("FLDMEASURENAME");
        dt.Columns.Add("FLDREQUIREDCOUNT");
        dt.Columns.Add("FLDREQUIREDURL");
        dt.Columns.Add("FLDINUSECOUNT");
        dt.Columns.Add("FLDINUSEURL");
        dt.Columns.Add("FLDPENDINGAPPROVALCOUNT");
        dt.Columns.Add("FLDPENDINGAPPROVALURL");
        dt.Columns.Add("FLDAPPROVEDCOUNT");
        dt.Columns.Add("FLDAPPROVEDURL");
        dt.Columns.Add("FLDEXPIREDCOUNT");
        dt.Columns.Add("FLDEXPIREDURL");
        DataRow dr = dt.NewRow();
        dr["FLDMEASURENAME"] = "Non Routine RAs";       
        int iRowCount = 0;
        int iTotalPageCount = 0;
        PhoenixDashboardTechnical.DashboardNonRoutineRASearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(7)
                                        , string.Empty, null
                                        , 1
                                        , 10
                                        , ref iRowCount
                                        , ref iTotalPageCount);
        dr["FLDREQUIREDCOUNT"] = iRowCount;
        dr["FLDREQUIREDURL"] = "/Dashboard/DashboardTechnicalRA.aspx";
        DataTable result = PhoenixDashboardTechnical.DashboardNonRoutineRAStatusCount(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(7));
        DataRow resultdr = result.Rows[0];
        dr["FLDINUSECOUNT"] = resultdr["FLDDRAFTCOUNT"].ToString();
        dr["FLDINUSEURL"] = "/Dashboard/DashboardTechnicalRAStatus.aspx?c=DRT";
        dr["FLDPENDINGAPPROVALCOUNT"] = resultdr["FLDPENDINGCOUNT"].ToString();
        dr["FLDPENDINGAPPROVALURL"] = "/Dashboard/DashboardTechnicalRAStatus.aspx?c=POC";
        dr["FLDAPPROVEDCOUNT"] = resultdr["FLDAPPROVEDCOUNT"].ToString();
        dr["FLDAPPROVEDURL"] = "/Dashboard/DashboardTechnicalRAStatus.aspx?c=AFU";
        dr["FLDEXPIREDCOUNT"] = resultdr["FLDEXPIREDCOUNT"].ToString();
        dr["FLDEXPIREDURL"] = "/Dashboard/DashboardTechnicalRAStatus.aspx?c=EXP";
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["FLDMEASURENAME"] = "Work Permits";
        PhoenixDashboardTechnical.DashboardPTWDueSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(7)
                                        , null, null
                                        , 1
                                        , 10
                                        , ref iRowCount
                                        , ref iTotalPageCount);
        dr["FLDREQUIREDCOUNT"] = iRowCount;
        dr["FLDREQUIREDURL"] = "/Dashboard/DashboardTechnicalPTWDue.aspx";
        PhoenixDashboardTechnical.DashboardPTWStatusSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(7), "1,4,7"
            , null, null, 1, 10, ref iRowCount, ref iTotalPageCount);
        dr["FLDINUSECOUNT"] = iRowCount;
        dr["FLDINUSEURL"] = "/Dashboard/DashboardTechnicalPTWStatus.aspx?s=1,4,7";
        PhoenixDashboardTechnical.DashboardPTWStatusSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(7), "8"
           , null, null, 1, 10, ref iRowCount, ref iTotalPageCount);
        dr["FLDPENDINGAPPROVALCOUNT"] = iRowCount;
        dr["FLDPENDINGAPPROVALURL"] = "/Dashboard/DashboardTechnicalPTWStatus.aspx?s=8";
        dr["FLDAPPROVEDCOUNT"] = iRowCount;
        dr["FLDAPPROVEDURL"] = "/Dashboard/DashboardTechnicalPTWStatus.aspx?s=8";

        PhoenixDashboardTechnical.DashboardPTWStatusSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Now.AddDays(7), "5"
           , null, null, 1, 10, ref iRowCount, ref iTotalPageCount);
        dr["FLDEXPIREDCOUNT"] = iRowCount;
        dr["FLDEXPIREDURL"] = "/Dashboard/DashboardTechnicalPTWStatus.aspx?s=5";

        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr["FLDMEASURENAME"] = "Checklists & Forms";
        result = PhoenixDashboardTechnical.DashboardFormCheckListDue(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        dr["FLDREQUIREDCOUNT"] = result.Rows.Count;
        dr["FLDREQUIREDURL"] = "/Dashboard/DashboardTechnicalDMFForms.aspx";
        result = PhoenixDashboardTechnical.DashboardFormCheckListStatusList(PhoenixSecurityContext.CurrentSecurityContext.VesselID, "INUSE");
        dr["FLDINUSECOUNT"] = result.Rows.Count.ToString();
        dr["FLDINUSEURL"] = "/Dashboard/DashboardTechnicalDMFForms.aspx?s=INUSE";
        result = PhoenixDashboardTechnical.DashboardFormCheckListStatusList(PhoenixSecurityContext.CurrentSecurityContext.VesselID, "APPROVAL");
        dr["FLDPENDINGAPPROVALCOUNT"] = result.Rows.Count.ToString();
        dr["FLDPENDINGAPPROVALURL"] = "/Dashboard/DashboardTechnicalDMFForms.aspx?s=APPROVAL";
        dt.Rows.Add(dr);
        return dt;
    }

    protected void gvFormsRA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        DataTable dt = GetFormsRA();
        grid.DataSource = dt;
    }

    protected void gvFormsRA_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkReqCount = (LinkButton)e.Item.FindControl("lnkReqCount");
            LinkButton lnkInUseCount = (LinkButton)e.Item.FindControl("lnkInUseCount");
            LinkButton lblPendingCount = (LinkButton)e.Item.FindControl("lblPendingCount");
            LinkButton lblApprovedCount = (LinkButton)e.Item.FindControl("lblApprovedCount");
            LinkButton lblExpiredCount = (LinkButton)e.Item.FindControl("lblExpiredCount");

            
            lnkReqCount.Attributes.Add("onclick", "javascript:top.openNewWindow('formsra','"+ drv["FLDMEASURENAME"] + " - Required','" + Session["sitepath"] + drv["FLDREQUIREDURL"] + "',true); return false;");
            lnkInUseCount.Attributes.Add("onclick", "javascript:top.openNewWindow('formsra','" + drv["FLDMEASURENAME"] + " - In Use','" + Session["sitepath"] + drv["FLDINUSEURL"] + "',true); return false;");
            lblPendingCount.Attributes.Add("onclick", "javascript:top.openNewWindow('formsra','" + drv["FLDMEASURENAME"] + " - Pending Approval','" + Session["sitepath"] + drv["FLDPENDINGAPPROVALURL"] + "',true); return false;");
            lblApprovedCount.Attributes.Add("onclick", "javascript:top.openNewWindow('formsra','" + drv["FLDMEASURENAME"] + " - Approved','" + Session["sitepath"] + drv["FLDAPPROVEDURL"] + "',true); return false;");
            lblExpiredCount.Attributes.Add("onclick", "javascript:top.openNewWindow('formsra','" + drv["FLDMEASURENAME"] + " - Expired','" + Session["sitepath"] + drv["FLDEXPIREDURL"] + "',true); return false;");
            
            if (drv["FLDAPPROVEDCOUNT"].ToString().Equals(""))
            {
                lblApprovedCount.Text = "N/A";
                lblApprovedCount.Enabled = false;
                lblApprovedCount.ForeColor = Color.FromName("#1e395b");
                lblApprovedCount.Attributes.Remove("onclick");
            }
            if (drv["FLDEXPIREDCOUNT"].ToString().Equals(""))
            {
                lblExpiredCount.Text = "N/A";
                lblExpiredCount.Enabled = false;
                lblExpiredCount.ForeColor = Color.FromName("#1e395b");
                lblExpiredCount.Attributes.Remove("onclick");
            }
        }
    }

    protected void gvNearMiss_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        DataTable dt = GetUnSafeActNearMiss();
        grid.DataSource = dt;
    }

    protected void gvNearMiss_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkNew = (LinkButton)e.Item.FindControl("lnkNew");
            LinkButton lnkPending = (LinkButton)e.Item.FindControl("lnkPending");

            lnkNew.Attributes.Add("onclick", drv["FLDNEWURL"].ToString());
            lnkPending.Attributes.Add("onclick", drv["FLDPENDINGURL"].ToString());            
            
        }
    }
    private DataTable GetUnSafeActNearMiss()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("FLDMEASURENAME");
        dt.Columns.Add("FLDNEWCOUNT");
        dt.Columns.Add("FLDNEWURL");
        dt.Columns.Add("FLDPENDINGCOUNT");
        dt.Columns.Add("FLDPENDINGURL");
        DataRow dr = dt.NewRow();
        dr["FLDMEASURENAME"] = "UC/UACTs";
        dr["FLDNEWURL"] = "javascript: top.openNewWindow('ucats','UC/UACTs - New(24hrs)','Inspection/InspectionDashboardUnsafeActsConditions.aspx?status=NEW'); return false;";
        dr["FLDPENDINGURL"] = "javascript: top.openNewWindow('ucats','UC/UACTs - Pending','Inspection/InspectionDashboardUnsafeActsConditions.aspx?status=PENDING'); return false;";
        DataSet ds = PhoenixDashboardTechnical.DashboardUnsaceAccidentNearMissSummary(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        DataTable result = ds.Tables[0];
        if (result.Rows.Count == 0)
        {
            dr["FLDNEWCOUNT"] = 0;
            dr["FLDPENDINGCOUNT"] = 0;
        }
        else
        {
            DataRow rdr = result.Rows[0];

            dr["FLDNEWCOUNT"] =  rdr["FLDUNSAFEACTNEW"].ToString();            
            dr["FLDPENDINGCOUNT"] = rdr["FLDUNSAFEACTPENDING"].ToString();            

        }
        dt.Rows.Add(dr);
       
        dr = dt.NewRow();
        DataRow dr1 = dt.NewRow();

        dr["FLDMEASURENAME"] = "Near Miss";
        dr1["FLDMEASURENAME"] = "Accidents";
        dr["FLDNEWURL"] = "javascript: top.openNewWindow('Incident','Near Miss - New(24hrs)','Inspection/InspectionDashboardIncidentNearMissList.aspx?code=NMNEW'); return false;";
        dr["FLDPENDINGURL"] = "javascript: top.openNewWindow('Incident','Near Miss - Pending','Inspection/InspectionDashboardIncidentNearMissList.aspx?code=NMPENDING'); return false;";
        dr1["FLDNEWURL"] = "javascript: top.openNewWindow('Incident','Accident - New(24hrs)','Inspection/InspectionDashboardIncidentNearMissList.aspx?code=INCNEW'); return false;";
        dr1["FLDPENDINGURL"] = "javascript: top.openNewWindow('Incident','Accident - Pending','Inspection/InspectionDashboardIncidentNearMissList.aspx?code=INCPENDING'); return false;";
        result = ds.Tables[1];
        if (result.Rows.Count == 0)
        {
            dr["FLDNEWCOUNT"] = 0;
            dr["FLDPENDINGCOUNT"] = 0;
            dr1["FLDNEWCOUNT"] = 0;
            dr1["FLDPENDINGCOUNT"] = 0;
        }
        else
        {
            DataRow rdr = result.Rows[0];

            dr["FLDNEWCOUNT"] = rdr["FLDNEARMISSNEW"].ToString();
            dr["FLDPENDINGCOUNT"] = rdr["FLDNEARMISSPENDING"].ToString();
            dr1["FLDNEWCOUNT"] = rdr["FLDACCIDENTNEW"].ToString();
            dr1["FLDPENDINGCOUNT"] = rdr["FLDACCIDENTPENDING"].ToString();           
        }
        dt.Rows.Add(dr);
        dt.Rows.Add(dr1);
        return dt;
    }

    protected void gvSpareStores_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        DataSet ds = PhoenixDashboardTechnical.DashboardPurhaseSummary(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        grid.DataSource = ds;
    }

    protected void gvSpareStores_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkCount = (LinkButton)e.Item.FindControl("lnkCount");

            lnkCount.Attributes.Add("onclick", "javascript:top.openNewWindow('detail','Store & Spares - " + drv["FLDTYPENAME"] + "','" + Session["sitepath"] + "/Purchase/PurchaseForm.aspx?dfilter=" + drv["FLDFILTER"] + "&vslid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "',true); return false;");
        }
    }
}