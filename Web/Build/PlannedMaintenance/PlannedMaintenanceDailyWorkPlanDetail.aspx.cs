using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Reports;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceDailyWorkPlanDetail : PhoenixBasePage
{  
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBERA"] = 1;
            ViewState["SORTEXPRESSIONA"] = null;
            ViewState["SORTDIRECTIONA"] = null;

            ViewState["PAGENUMBER"] = 1;           
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["PAGENUMBERC"] = 1;
            ViewState["SORTEXPRESSIONC"] = null;
            ViewState["SORTDIRECTIONC"] = null;            
            ViewState["PLANID"] = Request.QueryString["p"];
            ViewState["PROCESSID"] = string.Empty;
            DataSet ds = PhoenixRegistersHard.EditHardCode(1, 51, "DEK");
            if(ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
            {
                ViewState["DEK"] = ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString();
            }
            ds = PhoenixRegistersHard.EditHardCode(1, 51, "ENG");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["ENG"] = ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["eid"]))
            {
                ViewState["PROCESSID"] = Request.QueryString["eid"];
            }
            ViewState["PDATE"] = string.Empty;
            gvMPartB.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvMPartC.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvActivity.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            SetHeading();
            RadWindow_NavigateUrl.NavigateUrl = "../Dashboard/DashboardTechnicalJobCategoryPlanned.aspx?p=" + ViewState["PLANID"].ToString()+"&td="+ ViewState["DATE"];
            PhoenixPlannedMaintenanceDailyWorkPlan.MergeActivityOperation(new Guid(ViewState["PLANID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        //toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
        toolbar.AddButton("Report", "REPORT", ToolBarDirection.Right);
        toolbar.AddLinkButton("javascript:$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl='PlannedMaintenanceDailyWorkPlanActivityReschedule.aspx?iscopy=1&fdate=" + ViewState["DATE"].ToString() + "';showDialog('Copy');", "Copy", "COPY", ToolBarDirection.Right);
        MenuMain.AccessRights = this.ViewState;
        MenuMain.MenuList = toolbar.Show();

        string toolbox = "javascript:openNewWindow('codehelp1','Daily Work Plan Meeting - [Click on add button to conduct meeting]','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDailyWorkPlanMeeting.aspx?dwpid=" + ViewState["PLANID"] + "&td=" + ViewState["DATE"] + "'); return false;";
        bool disable = false;
        DateTime d = General.GetNullableDateTime(ViewState["DATE"].ToString()).Value;
        if (d < DateTime.Now.Date)
        {
            disable = true;
            gvActivityChart.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
        }
        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceDailyWorkPlanDetail.aspx?p=" + ViewState["PLANID"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvActivity')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        if (!disable && (d >= DateTime.Now.Date && d < DateTime.Now.Date.AddDays(6)))
        {
            //toolbar.AddFontAwesomeButton("javascript:$modalWindow.modalWindowID = '" + modalPopup.ClientID + "';showDialog('Add'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('0,ADD," + gvActivity.ClientID + "');", "Add Operations", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("javascript:$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl='../PlannedMaintenance/PlannedMaintenanceDailyWorkPlanDetailActivity.aspx?p=" + ViewState["PLANID"].ToString() + "&td=" + ViewState["DATE"] + "';showDialog('Add');", "Add Operations", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            //toolbar.AddFontAwesomeButton(toolbox, "Add Daily Work Plan Meeting", "<i class=\"fas fa-toolbox\"></i>", "TOOLBOX");
            //toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceDailyWorkPlanDetail.aspx?p=" + ViewState["PLANID"].ToString(), "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=18&reportcode=DASHBOARDDAILYWORKPLAN&p=" + ViewState["PLANID"].ToString() + "&vid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "&process=" + ViewState["PROCESSID"].ToString() + "&showmenu=0&showword=NO&showexcel=NO'); return false;", "Report", "<i class=\"fas fa-chart-bar\"></i>", "REPORTS");
        }        
        toolbar.AddFontAwesomeButton(toolbox, "Daily Work Plan Meeting", "<i class=\"fas fa-toolbox\"></i>", "TOOLBOX");
        toolbar.AddFontAwesomeButton("javascript:expandcollapse('gvActivity');", "Expand/Collapse", "<i class=\"fas fa-expand-alt\"></i>", "EXPANDCOLLAPSE");
        MenuPartA.Title = "Part A: Operations";
        MenuPartA.AccessRights = this.ViewState;
        MenuPartA.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceDailyWorkPlanDetail.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMPartB')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        if (!disable)
        {
            toolbar.AddFontAwesomeButton("javascript:$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl='../Dashboard/DashboardTechnicalJobCategoryPlanned.aspx?p=" + ViewState["PLANID"].ToString() + "&td=" + ViewState["PDATE"] + "';showDialog('Add');", "Add Work Orders", "<i class=\"fa fa-plus-circle\"></i>", "ADDWO");
        }
        toolbar.AddFontAwesomeButton("javascript:expandcollapse('gvMPartB');", "Expand/Collapse", "<i class=\"fas fa-expand-alt\"></i>", "EXPANDCOLLAPSE");
        MenuPartB.Title = "Part B: Planned Maintenance - Engine";
        MenuPartB.AccessRights = this.ViewState;
        MenuPartB.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceDailyWorkPlanDetail.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMPartC')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        if (!disable)
        {
            toolbar.AddFontAwesomeButton("javascript:$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl='../Dashboard/DashboardTechnicalJobCategoryPlanned.aspx?p=" + ViewState["PLANID"].ToString() + "&td=" + ViewState["PDATE"] + "';showDialog('Add');", "Add Work Orders", "<i class=\"fa fa-plus-circle\"></i>", "ADDWO");
        }
        toolbar.AddFontAwesomeButton("javascript:expandcollapse('gvMPartC');", "Expand/Collapse", "<i class=\"fas fa-expand-alt\"></i>", "EXPANDCOLLAPSE");
        MenuPartC.Title = "Part C: Planned Maintenance - Deck";
        MenuPartC.AccessRights = this.ViewState;
        MenuPartC.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Reset", "RESET", ToolBarDirection.Right);
        MenuCrewHrsReset.AccessRights = this.ViewState;
        MenuCrewHrsReset.MenuList = toolbar.Show();
    }
    protected void MenuPartA_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PROCESSID"] = string.Empty;
                gvMPartB.Rebind();
                gvActivity.Rebind();
                gvMPartC.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelOperation();
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPartB_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel(ViewState["ENG"].ToString(), "Part B: Planned Maintenance - Engine");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPartC_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel(ViewState["DEK"].ToString(), "Part C: Planned Maintenance - Deck");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MainMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("~/PlannedMaintenance/PlannedMaintenanceDailyWorkPlanSchedule.aspx");
            }
            else if (CommandName.ToUpper().Equals("ISSUE"))
            {
                PhoenixPlannedMaintenanceDailyWorkPlan.Issue(new Guid(ViewState["PLANID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                SetHeading();
                ucStatus.Text = "Daily Work Plan Issued Successfully";
            }
            else if (CommandName.ToUpper().Equals("REPORT"))
            {
                DataSet ds = PhoenixReportsDashboard.DailyWorkPlanReportPDF(General.GetNullableGuid(ViewState["PLANID"].ToString())
                                                                            , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                                            , General.GetNullableInteger(ViewState["PROCESSID"].ToString()));

                string Tmpfilelocation = string.Empty; string[] reportfile = new string[0];
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("applicationcode", "18");
                nvc.Add("reportcode", "DASHBOARDDAILYWORKPLAN");
                nvc.Add("CRITERIA", "");
                Session["PHOENIXREPORTPARAMETERS"] = nvc;

                Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
                string filename = "DASHBOARDDAILYWORKPLAN_" + General.GetDateTimeToString(ds.Tables[3].Rows[0]["FLDDATE"].ToString()).Replace("/", "_") + ".pdf";
                Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);

                PhoenixSsrsReportsCommon.getVersion();
                PhoenixSsrsReportsCommon.getLogo();
                PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation, 1);
                Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
                //Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
                //String scriptpopup = String.Format("javascript:top.openNewWindow('DailyWorkPlan','Report', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=18&reportcode=DASHBOARDDAILYWORKPLAN&p=" + ViewState["PLANID"].ToString() + "vid="+ PhoenixSecurityContext.CurrentSecurityContext.VesselID + "&showmenu=0&showword=NO&showexcel=NO');return true;");
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcelOperation()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDELEMENTNAME", "FLDACTIVITYNAME", "FLDESTSTARTTIME", "FLDDURATION", "FLDPERSONINCHARGENAME", "FLDOTHERMEMBERSNAME" };
        string[] alCaptions = { "Process", "Activity", "Est Start Time", "Duration", "PIC", "Other Members" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSIONA"] == null) ? null : (ViewState["SORTEXPRESSIONA"].ToString());
        if (ViewState["SORTDIRECTIONA"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONA"].ToString());

        if (ViewState["ROWCOUNTA"] == null || Int32.Parse(ViewState["ROWCOUNTA"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTA"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceDailyWorkPlan.SearchActivty(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                      , General.GetNullableGuid(ViewState["PLANID"].ToString())
                                      , General.GetNullableInteger(ViewState["PROCESSID"].ToString())
                                       , sortexpression, sortdirection
                                         , 1
                                         , iRowCount
                                         , ref iRowCount
                                         , ref iTotalPageCount);

        General.ShowExcel("Part A: Operation", ds.Tables[0], alColumns, alCaptions, null, null);
    }
    protected void ShowExcel(string dept, string title)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDWORKORDERNUMBER", "FLDCATEGORY", "FLDPLANNINGDUEDATE", "FLDPLANINGDURATIONINDAYS", "FLDPLANNINGESTIMETDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
        string[] alCaptions = { "Work Order No.", "Category", "Planned Date", "Duration Days", "Duration Hours", "Assigned To", "Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceDailyWorkPlan.SearchWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        , General.GetNullableGuid(ViewState["PLANID"].ToString())
                                        , General.GetNullableInteger(dept)
                                         , sortexpression, sortdirection
                                         , 1
                                         , iRowCount
                                         , ref iRowCount
                                         , ref iTotalPageCount);

        General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, null, null);
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDCATEGORY", "FLDPLANNINGDUEDATE", "FLDPLANINGDURATIONINDAYS", "FLDPLANNINGESTIMETDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
            string[] alCaptions = { "Work Order No.", "Category", "Planned Date", "Duration Days", "Duration Hours", "Assigned To", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixPlannedMaintenanceDailyWorkPlan.SearchWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        , General.GetNullableGuid(ViewState["PLANID"].ToString())
                                        , General.GetNullableInteger(ViewState["ENG"].ToString())
                                         , sortexpression, sortdirection
                                         , gvMPartB.CurrentPageIndex + 1
                                         , gvMPartB.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount);

            
            General.SetPrintOptions("gvMPartB", "Part B: Maintenance - Engine", alCaptions, alColumns, ds);

            gvMPartB.DataSource = ds;
            gvMPartB.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataPartA()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDELEMENTNAME", "FLDACTIVITYNAME", "FLDESTSTARTTIME", "FLDDURATION", "FLDPERSONINCHARGENAME", "FLDOTHERMEMBERSNAME" };
            string[] alCaptions = { "Process", "Activity", "Est Start Time", "Duration", "PIC", "Other Members" };

            string sortexpression = (ViewState["SORTEXPRESSIONA"] == null) ? null : (ViewState["SORTEXPRESSIONA"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONA"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONA"].ToString());

            DataSet ds = PhoenixPlannedMaintenanceDailyWorkPlan.SearchActivty(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        , General.GetNullableGuid(ViewState["PLANID"].ToString())         
                                        , General.GetNullableInteger(ViewState["PROCESSID"].ToString())
                                         , sortexpression, sortdirection
                                         , gvActivity.CurrentPageIndex + 1
                                         , gvActivity.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount);

           
            General.SetPrintOptions("gvActivity", "Part A: Operations", alCaptions, alColumns, ds);

            gvActivity.DataSource = ds;
            gvActivity.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNTA"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataPartC()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDCATEGORY", "FLDPLANNINGDUEDATE", "FLDPLANINGDURATIONINDAYS", "FLDPLANNINGESTIMETDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
            string[] alCaptions = { "Work Order No.", "Category", "Planned Date", "Duration Days", "Duration Hours", "Assigned To", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSIONC"] == null) ? null : (ViewState["SORTEXPRESSIONC"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONC"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONC"].ToString());

            DataSet ds = PhoenixPlannedMaintenanceDailyWorkPlan.SearchWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        , General.GetNullableGuid(ViewState["PLANID"].ToString())
                                        , General.GetNullableInteger(ViewState["DEK"].ToString())
                                         , sortexpression, sortdirection
                                         , gvMPartC.CurrentPageIndex + 1
                                         , gvMPartC.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount);

            General.SetPrintOptions("gvMPartC", "Part C: Maintenance - Deck", alCaptions, alColumns, ds);

            gvMPartC.DataSource = ds;
            gvMPartC.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNTC"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMPartB_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper() == "EDITR")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDWODETAILID").ToString();
                string DWPID = ((RadLabel)e.Item.FindControl("lblWdid")).Text;
                string plandate = ((RadLabel)e.Item.FindControl("lblPlanDate")).Text;
                string script = "function sd(){$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl='PlannedMaintenanceDailyWorkPlanDetailEdit.aspx?planid=" + DWPID + "&woid=" + id + "&d=" + General.GetDateTimeToString(ViewState["DATE"].ToString()) + "&gid=" + grid.ClientID + "';showDialog('Edit Plan - " + plandate + "');Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDWODETAILID").ToString();
                PhoenixPlannedMaintenanceDailyWorkPlan.DeleteWorkOrder(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                grid.Rebind();
                string script = "refreshScheduler();refreshDashboard();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "RESCHEDULE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string groupId = item.GetDataKeyValue("FLDWOGROUPID").ToString();
                LinkButton lnkWo = ((LinkButton)e.Item.FindControl("lblWO"));
                string script = "$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl = '../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReschedule.aspx?groupId=" + groupId + "';showDialog('Reschedule - " + lnkWo.Text + "');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMPartB_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            bool disable = false;
            if (General.GetNullableDateTime(ViewState["DATE"].ToString()).Value < DateTime.Now.Date)
            {
                disable = true;
            }
            RadGrid grid = (RadGrid)sender;
            DataSet ds = (DataSet)grid.DataSource;
            DataRowView drv = (DataRowView)e.Item.DataItem;            

            LinkButton lnkWo = ((LinkButton)e.Item.FindControl("lblWO"));
            if (lnkWo != null && drv != null)
            {
                lnkWo.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Work Orders','PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + drv["FLDWOGROUPID"].ToString() + "'); return false;");
            }
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = !disable && SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                if (drv != null && drv["FLDISPLANNED"].ToString().Equals("0"))
                {
                    del.Visible = false;
                }
            }
            LinkButton res = ((LinkButton)e.Item.FindControl("cmdReschedule"));
            if (res != null && drv != null)
            {
                res.Visible = !disable && SessionUtil.CanAccess(this.ViewState, res.CommandName);
                if (drv != null && drv["FLDISPLANNED"].ToString().Equals("0"))
                {
                    res.Visible = false;
                }
            }
            LinkButton ed = ((LinkButton)e.Item.FindControl("cmdEdit"));
            if (ed != null && drv != null)
            {
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            }
            HtmlGenericControl spnyellow = ((HtmlGenericControl)e.Item.FindControl("spnyellow"));
            HtmlGenericControl spnred = ((HtmlGenericControl)e.Item.FindControl("spnred"));
            if(spnyellow != null && spnred != null && drv != null)
            {
                DateTime? due = General.GetNullableDateTime(drv["FLDPLANNINGDUEDATE"].ToString());
                DateTime? mdue = General.GetNullableDateTime(drv["FLDMINIMUMDUE"].ToString());
                spnyellow.Visible = false;
                spnyellow.Attributes["title"] = "WO is overdue";
                spnred.Visible = false;
                spnred.Attributes["title"] = "WO Job is Overdue";
                if (due.HasValue && due.Value < DateTime.Now)
                {
                    spnyellow.Visible = true;
                }
                if (due.HasValue && mdue.HasValue && mdue.Value < DateTime.Now)
                {
                    spnyellow.Visible = false;
                    spnred.Visible = true;
                }
            }
            HtmlImage img = (HtmlImage)e.Item.FindControl("imgPMS");
            if (img != null)
            {
                img.Src = Session["images"] + "/" + GetImage(string.Empty);
            }
            RadLabel lblwo = ((RadLabel)e.Item.FindControl("lblWorkOrder"));
            if (lblwo != null && drv != null)
            {
                string s = drv["FLDWORKORDERNAME"].ToString();
                if (drv["FLDISCOMPLETED"].ToString().Equals("1"))
                {
                    s = s + " Done on " + General.GetDateTimeToString(drv["FLDDONEDATE"].ToString());
                    spnyellow.Visible = false;
                    spnred.Visible = false;
                    del.Visible = false;
                    res.Visible = false;
                }
                else
                {
                    s = s + " Due on " + General.GetDateTimeToString(drv["FLDPLANNINGDUEDATE"].ToString());
                }
                lblwo.Text = s;                
            }
            RadLabel lblOtherMembers = (RadLabel)e.Item.FindControl("lblOtherMembers");
            if (e.Item is GridDataItem)
            {
                if (ds != null)
                {
                    DataTable dtWorkDetails = ds.Tables[1];
                    string empsignoflist = drv["FLDOTHERMEMBERS"].ToString().Trim().TrimStart(',').TrimEnd(',');
                    if (General.GetNullableString(empsignoflist) != null)
                    {
                        string[] signofflist = empsignoflist.Split(',');
                        bool t1 = false;
                        bool t2 = false;
                        string name = "";
                        foreach (string s in signofflist)
                        {
                            t1 = false;
                            t2 = false;
                            DataRow[] drname = dtWorkDetails.Select("FLDEMPLOYEESIGNONOFFID = " + s);
                            DataRow[] dr = dtWorkDetails.Select("FLDEMPLOYEESIGNONOFFID = '" + s + "' AND FLDREPORTINGHOUR > "
                                    + (General.GetNullableInteger(drv["FLDESTSTARTTIME"].ToString()) != null ? drv["FLDESTSTARTTIME"].ToString() : "0") + " AND FLDREPORTINGHOUR <="
                                    + (General.GetNullableInteger(drv["FLDENDHOUR"].ToString()) != null ? drv["FLDENDHOUR"].ToString() : "0") + " AND FLDNONCOMPLIANCE<>''");
                            
                            if (dr.Length > 0 && General.GetNullableString(dr[0]["FLDNONCOMPLIANCE"].ToString()) != null)
                            {
                                t1 = true;
                            }

                            DataRow[] dr1 = dtWorkDetails.Select("FLDEMPLOYEESIGNONOFFID = '" + s + "' AND FLDNONCOMPLIANCE<>''");

                            //if (dr1.Length > 0 && General.GetNullableString(dr1[0]["FLDNONCOMPLIANCE"].ToString()) != null)
                            //{
                            //    t2 = true;
                            //}
                            if (drname.Length > 0 && t1 && t2)
                                name = name + drname[0]["FLDRANKCODE"].ToString() + " " + drname[0]["FLDEMPLOYEENAME"].ToString() + "<html><font size=3 color=red>!</font><font size=3 color=orange> !</font>" + " ";//"<html><size=16><font><color = red>!</color> <color = orange>!";
                            else if (drname.Length > 0 && t1 && !t2)
                                name = name + drname[0]["FLDRANKCODE"].ToString() + " " + drname[0]["FLDEMPLOYEENAME"].ToString() + "<html><font size=3 color=red>!</font> ";
                            else if (drname.Length > 0 && !t1 && t2)
                                name = name + drname[0]["FLDRANKCODE"].ToString() + " " + drname[0]["FLDEMPLOYEENAME"].ToString() + "<html><font size=3 color=orange>!</font> ";
                            else if (drname.Length > 0)
                                name = name + drname[0]["FLDRANKCODE"].ToString() + " " + drname[0]["FLDEMPLOYEENAME"].ToString();

                            name += ',';
                        }
                        lblOtherMembers.Text = name.TrimEnd(',').TrimStart(',');
                        //if (tooltip)
                        //{
                        //    lblOtherMembers.ToolTip = "<html><font size=3 color=red>!</font> NC during activity period <br/> <html><font size=3 color=orange>!</font> NC in current day";
                        //    RadToolTipManager1.TargetControls.Add(lblOtherMembers.ClientID,true);
                        //}

                    }
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMPartB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMPartB.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMPartC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERC"] = ViewState["PAGENUMBERC"] != null ? ViewState["PAGENUMBERC"] : gvMPartC.CurrentPageIndex + 1;
            BindDataPartC();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMPartB_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void gvMPartC_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSIONC"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTIONC"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTIONC"] = "1";
                break;
        }
    }
        
    private void SetHeading()
    {
        string heading = "Daily Work Plan No. : {no} | Date : {date} | Vessel Status : {status}";
        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.Edit(new Guid(ViewState["PLANID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if(dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            heading = heading.Replace("{no}", dr["FLDPLANNO"].ToString())
                                .Replace("{date}", General.GetDateTimeToString(dr["FLDDATE"].ToString()))
                                .Replace("{status}", dr["FLDVESSELSTATUSNAME"].ToString());
            ViewState["DATE"] = dr["FLDDATE"].ToString();
            ViewState["PDATE"] = DateTime.Parse(dr["FLDDATE"].ToString()).Date.AddDays(7);
            if (General.GetNullableDateTime(ViewState["DATE"].ToString()).Value > DateTime.Now.Date.AddDays(7))
            {
                RadTabStrip1.FindTabByText("Activities Chart").Enabled = false;
                RadTabStrip1.FindTabByText("Crew Work Hours").Enabled = false;
            }
        }
        lblHeading.Text = heading;
    }

    protected void gvActivity_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            bool disable = false;
            if (General.GetNullableDateTime(ViewState["DATE"].ToString()).Value < DateTime.Now.Date)
            {
                disable = true;
            }
            RadGrid grid = (RadGrid)sender;
            DataSet ds = (DataSet)grid.DataSource;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = disable ? false : SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            }
            LinkButton ra = (LinkButton)e.Item.FindControl("lnkRA");
            if (ra != null)
            {
                if (!drv["FLDRISKASSESSMENTPROCESSID"].ToString().Equals(Guid.Empty.ToString()))
                    ra.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','RA','" + Session["sitepath"] + "/Inspection/InspectionRAProcessExtn.aspx?processid="+ drv["FLDRISKASSESSMENTPROCESSID"].ToString() + "&status='); return false;");
                else
                    ra.Enabled = false;
            }
            if(ra != null)
            {
                string[] forms = drv["FLDFORMLIST"].ToString().Split('`');
                if(forms.Length > 0)
                {
                    foreach(string s in forms)
                    {
                        if (s.Trim().Length == 0) continue;
                        LinkButton lnk = new LinkButton();
                        string[] data = s.Split('~');
                        lnk.Text = data[1] + ",";
                        lnk.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Forms & Checklist','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + data[0] + "&FORMREVISIONID=" + data[2] + "&dwpaid="+ drv["FLDDAILYPLANACTIVITYID"].ToString() + "'); return false;");
                        e.Item.Cells[grid.Columns.FindByUniqueName("FLDFORMLIST").OrderIndex].Controls.Add(lnk);
                        //e.Item.Cells[grid.Columns.FindByUniqueName("FLDFORMLIST").OrderIndex].Controls.Add(new Literal() { Text = "<br/>" });
                    }
                }
            }
            LinkButton postpone = (LinkButton)e.Item.FindControl("cmdReschedule");
            if (postpone != null)
            {
                postpone.Visible = SessionUtil.CanAccess(this.ViewState, postpone.CommandName);
                if (drv["FLDISPLANNED"].ToString() != "1" || disable) postpone.Visible = false;
            }
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null)
            {
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);                
            }
            HtmlImage img = (HtmlImage)e.Item.FindControl("imgActivity");
            if (img != null)
            {
                img.Src = Session["images"] + "/" + GetImage(drv["FLDCODE"].ToString());          
            }

            RadLabel lblOtherMembers = (RadLabel)e.Item.FindControl("lblOtherMembers");
            if (e.Item is GridDataItem)
            {
                if (ds != null)
                {
                    DataTable dtWorkDetails = ds.Tables[1];
                    string empsignoflist = drv["FLDOTHERMEMBERS"].ToString().Trim().TrimStart(',').TrimEnd(',');
                    if (General.GetNullableString(empsignoflist) != null)
                    {
                        string[] signofflist = empsignoflist.Split(',');
                        bool t1 = false;
                        bool t2 = false;
                        //bool tooltip = false;
                        string name = "";
                        foreach (string s in signofflist)
                        {
                            t1 = false;
                            t2 = false;
                            DataRow[] drname = dtWorkDetails.Select("FLDEMPLOYEESIGNONOFFID = " + s);
                            DataRow[] dr = dtWorkDetails.Select("FLDEMPLOYEESIGNONOFFID = '" + s + "' AND FLDREPORTINGHOUR > "
                                    + (General.GetNullableInteger(drv["FLDESTSTARTTIME"].ToString()) != null ? drv["FLDESTSTARTTIME"].ToString() : "0") + " AND FLDREPORTINGHOUR <="
                                    + (General.GetNullableInteger(drv["FLDENDHOUR"].ToString()) != null ? drv["FLDENDHOUR"].ToString() : "0") + " AND FLDNONCOMPLIANCE<>''");

                            if (dr.Length > 0 && General.GetNullableString(dr[0]["FLDNONCOMPLIANCE"].ToString()) != null)
                            {
                                t1 = true;
                                //tooltip = true;
                            }

                            DataRow[] dr1 = dtWorkDetails.Select("FLDEMPLOYEESIGNONOFFID = '" + s + "' AND FLDNONCOMPLIANCE<>''");

                            //if (dr1.Length > 0 && General.GetNullableString(dr1[0]["FLDNONCOMPLIANCE"].ToString()) != null)
                            //{
                            //    t2 = true;
                            //    //tooltip = true;
                            //}
                            if (drname.Length > 0 && t1 && t2)
                                name = name + drname[0]["FLDRANKCODE"].ToString() + " " + drname[0]["FLDEMPLOYEENAME"].ToString() + "<html><font size=3 color=red>!</font><font size=3 color=orange> !</font>" + " ";//"<html><size=16><font><color = red>!</color> <color = orange>!";
                            else if (drname.Length > 0 && t1 && !t2)
                                name = name + drname[0]["FLDRANKCODE"].ToString() + " " + drname[0]["FLDEMPLOYEENAME"].ToString() + "<html><font size=3 color=red>!</font> ";
                            else if (drname.Length > 0 && !t1 && t2)
                                name = name + drname[0]["FLDRANKCODE"].ToString() + " " + drname[0]["FLDEMPLOYEENAME"].ToString() + "<html><font size=3 color=orange>!</font> ";
                            else if (drname.Length > 0)
                                name = name + drname[0]["FLDRANKCODE"].ToString() + " " + drname[0]["FLDEMPLOYEENAME"].ToString();

                            name += ',';

                        }
                        lblOtherMembers.Text = name.TrimEnd(',').TrimStart(',');
                        //if (tooltip)
                        //{
                        //    lblOtherMembers.ToolTip = "<html><font size=3 color=red>!</font> NC during activity period <br/> <html><font size=3 color=orange>!</font> NC in current day";
                        //    RadToolTipManager1.TargetControls.Add(lblOtherMembers.ClientID,true);
                        //}

                    }
                }
            }        

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvActivity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERA"] = ViewState["PAGENUMBERA"] != null ? ViewState["PAGENUMBERA"] : gvActivity.CurrentPageIndex + 1;
            BindDataPartA();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvActivity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBERA"] = null;
            }
            else if (e.CommandName.ToUpper() == "EDITR")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDAILYPLANACTIVITYID").ToString();
                string DailyWPID = ((RadLabel)e.Item.FindControl("lbladid")).Text;
                string DailyWPDate = ((RadLabel)e.Item.FindControl("lbladdate")).Text;
                string script = "function sd(){$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl='PlannedMaintenanceDailyWorkPlanDetailEdit.aspx?planid=" + DailyWPID + "&id=" + id + "&d=" + General.GetDateTimeToString(DailyWPDate) + "&gid=" + grid.ClientID + "';showDialog('Edit','true'); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {
                string id = grid.SelectedValue.ToString();
                string eststarttime = ((RadComboBox)e.Item.FindControl("ddlEstStartTime")).SelectedValue;
                string duration = ((RadComboBox)e.Item.FindControl("ddlDuration")).SelectedValue;
                string picid = ((RadComboBox)e.Item.FindControl("ddlPersonIncharge")).SelectedValue;
                string picname = ((RadComboBox)e.Item.FindControl("ddlPersonIncharge")).Text;
                var collection = ((RadComboBox)e.Item.FindControl("ddlCrewList")).CheckedItems;
                string csvOtherMembers = string.Empty;
                string csvOtherMembersName = string.Empty;
                if (collection.Count != 0)
                {
                    csvOtherMembers = ",";
                    csvOtherMembersName = ",";
                    foreach (var item in collection)
                    {
                        csvOtherMembers = csvOtherMembers + item.Value + ",";
                        csvOtherMembersName = csvOtherMembersName + item.Text + ",";
                    }
                }

                PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWorkOrder(new Guid(id)
                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(eststarttime), byte.Parse(duration)
                    , General.GetNullableInteger(picid), picname, csvOtherMembers, csvOtherMembersName);
                BindDataPartA();
                gvActivity.Rebind();
            }
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDAILYPLANACTIVITYID").ToString();
                PhoenixPlannedMaintenanceDailyWorkPlan.DeleteActivity(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                grid.Rebind();
                string script = "refreshScheduler();refreshDashboard();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "RESCHEDULE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDAILYPLANACTIVITYID").ToString();
                RadLabel lblActivity = ((RadLabel)e.Item.FindControl("lblActivityName"));
                string script = "function sd(){$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl='PlannedMaintenanceDailyWorkPlanActivityReschedule.aspx?id=" + id + "&gid=" + grid.ClientID + "';showDialog('Reschedule - " + lblActivity.Text + "'); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "COPY")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDAILYPLANACTIVITYID").ToString();
                RadLabel lblActivity = ((RadLabel)e.Item.FindControl("lblActivityName"));
                string script = "function sd(){$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl='PlannedMaintenanceDailyWorkPlanActivityReschedule.aspx?id=" + id + "&iscopy=1&cdate=" + General.GetDateTimeToString(DateTime.Now) + "&gid=" + grid.ClientID + "';showDialog('Copy - " + lblActivity.Text + "'); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvActivity_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSIONA"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTIONA"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTIONA"] = "1";
                break;
        }
    }
    
    protected string[] BindDuration()
    {
        return new string[] { "0000", "0100", "0200", "0300", "0400", "0500", "0600", "0700", "0800", "0900", "1000", "1100", "1200", "1300", "1400", "1500", "1600", "1700", "1800", "1900", "2000", "2100", "2200", "2300", "2400" };
    }
            
    protected void RadAjaxPanel2_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        var args = e.Argument;
        var array = args.Split(',');
        var id = array[0];
        var cmd = array[1];
        var gridid = array[2];        
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        ViewState["PROCESSID"] = string.Empty;
        gvMPartB.Rebind();
        gvMPartC.Rebind();        
        gvActivityChart.Rebind();
        gvCrewWorkHrs.Rebind();
        gvActivity.Rebind();
    }    
    protected void gvCrewWorkHrs_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.Item is GridDataItem)
        {
            DataSet ds = (DataSet)grid.DataSource;
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            DataTable dtWorkDetails = ds.Tables[1];
            DataTable dtnc = ds.Tables[2];
            DataTable dtactivity = ds.Tables[3];
            foreach (GridColumn c in grid.Columns)
            {
                if (c.UniqueName != "FLDEMPLOYEENAME" && c.UniqueName != "FLDRANKNAME" && c.UniqueName != "RESET")
                    item[c.UniqueName].Text = "";
                if (c.UniqueName != "RESET")
                    item[c.UniqueName].Attributes.Add("TotalHours", drv[c.UniqueName].ToString());
                
                if (c.UniqueName != "FLDEMPLOYEENAME" && c.UniqueName != "FLDRANKNAME" && c.UniqueName != "RESET" && drv[c.UniqueName].ToString() == "1" )
                {
                    item[c.UniqueName].BorderWidth = Unit.Parse("1px");
                    item[c.UniqueName].BorderColor = ColorTranslator.FromHtml("#e7e7e7");                                        

                    DataRow[] dr = dtWorkDetails.Select("FLDEMPLOYEEID = '" + drv["FLDEMPLOYEEID"].ToString() + "' AND FLDHOUR = '" + c.UniqueName + "'");

                    DataRow[] dr1 = dtactivity.Select("FLDSIGNONOFFID = '" + drv["FLDEMPLOYEESIGNONOFFID"].ToString() + "' AND "+c.UniqueName+" > FLDESTSTARTTIME AND "+ c.UniqueName+" <= FLDENDHOUR ");

                    string nactivity = "";
                    foreach(DataRow dactivity in dr1)
                    {
                        nactivity = nactivity + dactivity["FLDACTIVITYNAME"].ToString() + " <br/>";
                    }
                    item[c.UniqueName].ToolTip = nactivity;
                    RadToolTipManager1.TargetControls.Add(item[c.UniqueName].ClientID, true);

                    if (dr.Length > 0 && General.GetNullableString(dr[0]["FLDNONCOMPLIANCE"].ToString()) != null)
                    {
                        string nc = dr[0]["FLDNONCOMPLIANCE"].ToString().Trim().TrimStart(',');
                        string nctext = "";
                        string[] ncarray = nc.Split(',');
                        foreach (string t in ncarray)
                        {
                            DataRow[] drnc = dtnc.Select("FLDSHORTNAME= '" + t + "'");
                            if (drnc.Length > 0)
                                nctext = nctext + drnc[0]["FLDSHORTNAME"].ToString() + " - " + drnc[0]["FLDQUICKNAME"].ToString() + " <br/>";
                        }

                        item[c.UniqueName].BackColor = System.Drawing.Color.Red;
                        //item[c.UniqueName].ToolTip = nctext;
                    }
                    //else if(dr.Length > 0 && General.GetNullableString(dr[0]["FLDNONCOMPLIANCE"].ToString()) != null && General.GetNullableString(nactivity) == null)
                    //{
                    //    item[c.UniqueName].BackColor = System.Drawing.Color.Orange;
                    //}
                    else
                        item[c.UniqueName].BackColor = System.Drawing.Color.Gray;

                }
            }
        }
    }
    protected void gvCrewWorkHrs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            DataSet ds = PhoenixPlannedMaintenanceDailyWorkPlan.CrewWorkHoursList(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(ViewState["PLANID"].ToString()));
            grid.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvActivityChart_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        DataSet ds = PhoenixPlannedMaintenanceDailyWorkPlan.ListActivityCharts(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(ViewState["PLANID"].ToString()));
        grid.DataSource = ds;
    }

    protected void gvActivityChart_ItemDataBound(object sender, GridItemEventArgs e)
    {
        bool disable = false;
        if (General.GetNullableDateTime(ViewState["DATE"].ToString()).Value < DateTime.Now.Date)
        {
            disable = true;
        }
        RadGrid grid = (RadGrid)sender;
        DataSet ds = (DataSet)grid.DataSource;
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            for (int i = 1; i <= 24; i++)
            {
                GridColumn c = grid.Columns.FindByUniqueName("FLD" + i.ToString().PadLeft(2, '0'));


                DataTable dtactivity = ds.Tables[1];
                DataRow[] dr1 = dtactivity.Select("FLDDAILYPLANACTIVITYID = '" + drv["FLDDAILYPLANACTIVITYID"].ToString() + "' AND " + i + " > FLDSTARTTIME AND " + i + " <= FLDENDTIME ");

                string crewmember = "";
                foreach (DataRow dactivity in dr1)
                {
                    crewmember = crewmember + dactivity["FLDMEMBERNAME"].ToString() + " <br/>";
                }
                item[c.UniqueName].ToolTip = crewmember;
                RadToolTipManager1.TargetControls.Add(item[c.UniqueName].ClientID, true);

                if (drv["FLDCOLOR"].ToString() != string.Empty && drv[c.UniqueName].ToString() == "1")
                {
                    item[c.UniqueName].BorderWidth = Unit.Parse("1px");
                    item[c.UniqueName].BorderColor = ColorTranslator.FromHtml("White");
                    item[c.UniqueName].BackColor = ColorTranslator.FromHtml(drv["FLDCOLOR"].ToString());
                    item[c.UniqueName].Text = "";
                }
            }
        }
        LinkButton cac = (LinkButton)e.Item.FindControl("cmdCancelActivity");
        if (cac != null)
        {
            cac.Visible = disable ? false : SessionUtil.CanAccess(this.ViewState, cac.CommandName);
            cac.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you sure you want to cancel this activity?'); return false;");
            if (drv["FLDISOPERATION"].ToString().ToUpper() == "M")
            {
                cac.Visible = false;
            }
        }
        LinkButton cwo = (LinkButton)e.Item.FindControl("cmdCancelWO");
        if (cwo != null)
        {
            cwo.Visible = disable ? false : SessionUtil.CanAccess(this.ViewState, cwo.CommandName);
            cwo.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you sure you want to cancel this work order?'); return false;");
            if (drv["FLDISOPERATION"].ToString().ToUpper() == "O")
            {
                cwo.Visible = false;
            }
        }
        LinkButton copy = (LinkButton)e.Item.FindControl("cmdCopy");
        if (copy != null)
        {
            copy.Visible = !disable && SessionUtil.CanAccess(this.ViewState, copy.CommandName);
            //copy.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you sure you want to copy this activity?'); return false;");
            if (drv["FLDISOPERATION"].ToString().ToUpper() == "M")
            {
                copy.Visible = false;
            }
        }
    }

    protected void gvActivityChart_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            if (e.CommandName.ToUpper() == "CANCELACTIVITY")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDAILYPLANACTIVITYID").ToString();
                PhoenixPlannedMaintenanceDailyWorkPlan.DeleteActivity(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                grid.Rebind();
                gvActivity.Rebind();
                gvMPartB.Rebind();
                gvMPartC.Rebind();
                gvCrewWorkHrs.Rebind();
                string script = "refreshDashboard()";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "CANCELWO")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDAILYPLANACTIVITYID").ToString();
                PhoenixPlannedMaintenanceDailyWorkPlan.DeleteWorkOrder(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                grid.Rebind();
                gvActivity.Rebind();
                gvMPartB.Rebind();
                gvMPartC.Rebind();
                gvCrewWorkHrs.Rebind();
                string script = "refreshDashboard()";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            //else if (e.CommandName.ToUpper() == "COPY")
            //{
            //    GridDataItem item = (GridDataItem)e.Item;
            //    string id = item.GetDataKeyValue("FLDDAILYPLANACTIVITYID").ToString();
            //    PhoenixPlannedMaintenanceDailyWorkPlan.CopyActivity(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            //    grid.Rebind();

            //    gvActivity.Rebind();
            //    gvMPartB.Rebind();
            //    gvMPartC.Rebind();
            //    gvCrewWorkHrs.Rebind();
            //    string script = "refreshDashboard()";
            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            //}
            else if (e.CommandName.ToUpper() == "COPY")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDAILYPLANACTIVITYID").ToString();
                RadLabel lblActivity = ((RadLabel)e.Item.FindControl("lblActivityName"));
                string script = "function sd(){$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl='PlannedMaintenanceDailyWorkPlanActivityReschedule.aspx?id=" + id + "&iscopy=1&cdate=" + General.GetDateTimeToString(DateTime.Now) + "&gid=" + grid.ClientID + "';showDialog('Copy - " + lblActivity.Text + "'); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmitActivity_Click(object sender, EventArgs e)
    {
        gvActivity.Rebind();
        gvActivityChart.Rebind();
        gvCrewWorkHrs.Rebind();
    }
    protected void ddlEstStartTime_Load(object sender, EventArgs e)
    {
        RadComboBox duration = (RadComboBox)sender;
        duration.DataSource = BindDuration();
        duration.DataBind();
    }
    protected void gvActivityChart_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        foreach (GridBatchEditingCommand cmd in e.Commands)
        {
            string eststarttime = cmd.NewValues["FLDESTSTARTTIME"].ToString();
            string duration = cmd.NewValues["FLDDURATION"].ToString();
            eststarttime = eststarttime.Substring(0, eststarttime.Length - 2);
            duration = duration.Substring(0, duration.Length - 2);
            if (cmd.NewValues["FLDISOPERATION"].ToString().Equals("O"))
            {
                PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivity(new Guid(cmd.NewValues["FLDDAILYPLANACTIVITYID"].ToString())
                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(eststarttime)
                                            , byte.Parse(duration)
                                            , null, null, null, null);
            }
            else
            {
                PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWorkOrder(new Guid(cmd.NewValues["FLDDAILYPLANACTIVITYID"].ToString())
                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(eststarttime)
                                            , byte.Parse(duration)
                                            , null, null, null, null);
            }
        }
        gvActivity.Rebind();
        gvMPartB.Rebind();
        gvMPartC.Rebind();
        gvCrewWorkHrs.Rebind();
    }
    private string GetImage(string Code)
    {
        string img;
        switch (Code)
        {
            case "CAR":
                img = "cargo.png";
                break;
            case "NAV":
                img = "Navigation and Mooring.png";
                break;
            case "CAT":
                img = "Catering and Housekeeping.png";
                break;
            case "BUN":
                img = "BunkersLube.png";
                break;
            case "AUD":
                img = "Audits and Inspections.png";
                break;
            case "ENV":
                img = "EnvironmentalManagement.png";
                break;
            case "EMG":
                img = "Emergency Preparedness.png";
                break;
            case "CRW":
                img = "crew.png";
                break;
            default:
                img = "PlannedMaintenance.png";
                break;
        }
        return img;
    }
    protected string PadZero(string padstring)
    {
        if (padstring.Length == 1)
        {
            padstring = padstring.PadLeft(2, '0');
        }
        if (padstring.Length == 2)
        {
            padstring = padstring.PadRight(4, '0');
        }
        return padstring;
    }

    protected void gvCrewWorkHrs_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "RowClick")
            {

                foreach (GridTableCell cell in gvCrewWorkHrs.SelectedCells)
                {
                    GridDataItem item = (GridDataItem)cell.Parent;
                    //decimal hrs = 0.0m;

                    //if (General.GetNullableDecimal(cell.Text) == null || General.GetNullableDecimal(cell.Text) == 0)
                    //    hrs = 1.0m;
                    //else if (General.GetNullableDecimal(cell.Text) != null && General.GetNullableDecimal(cell.Text) < 1)
                    //    hrs = 0.0m;
                    //else if (General.GetNullableDecimal(cell.Text) != null && General.GetNullableDecimal(cell.Text) == 1)
                    //    hrs = 0.5m;

                    decimal hrs = 0.0m;
                    hrs = decimal.Parse(cell.Attributes["TotalHours"]);
                    if (hrs > 0)
                        hrs = 0.0m;
                    else
                        hrs = 1.0m;

                    if (General.GetNullableInteger(item.GetDataKeyValue("FLDREPORTINGDAY").ToString()) != null
                        && General.GetNullableInteger(item.GetDataKeyValue("FLDSHIPCALENDARID").ToString()) != null
                        && General.GetNullableInteger(item.GetDataKeyValue("FLDEMPLOYEEID").ToString()) != null
                        )
                    {
                        PhoenixPlannedMaintenanceDailyWorkPlan.PlanHoursUpdate(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                , int.Parse(item.GetDataKeyValue("FLDSHIPCALENDARID").ToString())
                                                                , int.Parse(item.GetDataKeyValue("FLDEMPLOYEEID").ToString())
                                                                , int.Parse(item.GetDataKeyValue("FLDREPORTINGDAY").ToString())
                                                                , int.Parse(cell.Column.UniqueName)
                                                                , hrs
                                                                );
                    }




                }
                gvCrewWorkHrs.Rebind();
            }
            if (e.CommandName == "RESET")
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPlannedMaintenanceDailyWorkPlan.ResetPlanHours(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , int.Parse(item.GetDataKeyValue("FLDEMPLOYEESIGNONOFFID").ToString())
                        , General.GetNullableGuid(ViewState["PLANID"].ToString()));

                gvCrewWorkHrs.Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCrewHrsReset_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("RESET"))
            {
                if (General.GetNullableGuid(ViewState["PLANID"].ToString()) != null)
                {
                    PhoenixPlannedMaintenanceDailyWorkPlan.AsyncResetWorkHours(PhoenixSecurityContext.CurrentSecurityContext.VesselID, ViewState["PLANID"].ToString());
                    ucNotification.Show("Reset initiated. It will be reflected in your screen after few minutes. Meanwhile you can work on other screens in phoenix");
                }                    
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
