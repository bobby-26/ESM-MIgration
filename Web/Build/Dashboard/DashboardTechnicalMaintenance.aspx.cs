using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Dashboard_DashboardTechnicalMaintenance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //PhoenixToolbar toolbar = new PhoenixToolbar();
        //toolbar.AddFontAwesomeButton("../Dashboard/DashboardTechnicalMaintenance.aspx?grdid=gvMaintenancePlanned", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        //toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMaintenancePlanned')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        //toolbar.AddFontAwesomeButton("javascript:expandcollapse('gvMaintenancePlanned');", "Expand/Collapse", "<i class=\"fas fa-expand-alt\"></i>", "EXPANDCOLLAPSE");
        //MenuPlanned.Title = "Maintenance Planned";
        //MenuPlanned.AccessRights = this.ViewState;
        //MenuPlanned.MenuList = toolbar.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Dashboard/DashboardTechnicalMaintenance.aspx?" + Request.QueryString.ToString() + "&grdid =gvMaintenanceProgress", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMaintenanceProgress')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Dashboard/DashboardTechnicalMaintenance.aspx?" + Request.QueryString.ToString(), "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("javascript:$modalWindow.modalWindowUrl = '../Dashboard/DashboardTechnicalJobCategoryPlanned.aspx?td=" + General.GetDateTimeToString(DateTime.Now) + "';showDialog('Add');", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDWO");
        //toolbar.AddFontAwesomeButton("javascript:expandcollapse('gvMaintenanceProgress');", "Expand/Collapse", "<i class=\"fas fa-expand-alt\"></i>", "EXPANDCOLLAPSE");
        //MenuProgress.Title = "Maintenance In Progress";
        MenuProgress.AccessRights = this.ViewState;
        MenuProgress.MenuList = toolbar.Show();

        //toolbar = new PhoenixToolbar();
        //toolbar.AddFontAwesomeButton("../Dashboard/DashboardTechnicalMaintenance.aspx?grdid=gvMaintenanceCompleted", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        //toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMaintenanceCompleted')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        //toolbar.AddFontAwesomeButton("javascript:expandcollapse('gvMaintenanceCompleted');", "Expand/Collapse", "<i class=\"fas fa-expand-alt\"></i>", "EXPANDCOLLAPSE");
        //MenuCompleted.Title = "Maintenance Completed";
        //MenuCompleted.AccessRights = this.ViewState;
        //MenuCompleted.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            int pagesize = General.ShowRecords(null);
            gvMaintenanceProgress.PageSize = pagesize;
            //gvMaintenancePlanned.PageSize = pagesize;
            //gvMaintenanceCompleted.PageSize = pagesize;   
            ViewState["WONO"] = string.Empty;
            ViewState["WONAME"] = string.Empty;          
            ViewState["FDATE"] = string.Empty;
            ViewState["TDATE"] = string.Empty;
            ViewState["STATUS"] = Request.QueryString["status"] ?? string.Empty;
            ViewState["DEPT"] = string.Empty;
            ViewState["TEAMMEMBER"] = string.Empty;
            if (Request.QueryString["ft"] != null && Request.QueryString["ft"].ToString() == "t")
            {
                ViewState["FDATE"] = General.GetDateTimeToString(DateTime.Now);
                ViewState["TDATE"] = General.GetDateTimeToString(DateTime.Now);
            }
            if (Request.QueryString["td"] != null && Request.QueryString["td"].ToString() == "t")
            {
                ViewState["TDATE"] = General.GetDateTimeToString(DateTime.Now);
            }
            ViewState["ISACTION"] = "1";
        }
    }
    protected void MenuMaintenance_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string gridId = Request.QueryString["grdid"];
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDPLANNINGDUEDATE", "FLDDATE", "FLDESTSTARTTIME", "FLDDURATION", "FLDSTARTTIMEHR", "FLDENDTIMEHR", "FLDOTHERMEMBERSNAME", "FLDMAINTENANCESTATUS" };
                string[] alCaptions = { "Work Order No.", "Work Order", "Due On", "Planned On", "Est. Start Time", "Est. End Time", "Start Time", "End Time", "Team Members", "Status" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION" + gridId] == null) ? null : (ViewState["SORTEXPRESSION" + gridId].ToString());
                if (ViewState["SORTDIRECTION" + gridId] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT" + gridId] == null || Int32.Parse(ViewState["ROWCOUNT" + gridId].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT" + gridId].ToString());

                string heading = "Maintenance";
                //int status = 0;
                //if (gridId.Contains(gvMaintenancePlanned.ClientID))
                //{
                //    heading = "Maintenance Planned";
                //    status = 1;
                //}
                //else if (gridId.Contains(gvMaintenanceProgress.ClientID))
                //{
                //    heading = "Maintenance In Progress";
                //    status = 2;
                //}
                //else if (gridId.Contains(gvMaintenanceCompleted.ClientID))
                //{
                //    heading = "Maintenance Completed";
                //    status = 3;
                //}
                DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.SearchIssuedWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                         , General.GetNullableInteger(ViewState["STATUS"].ToString())
                                         , sortexpression, sortdirection
                                         , 1
                                         , iRowCount
                                         , ref iRowCount
                                         , ref iTotalPageCount
                                         , ViewState["WONO"].ToString()
                                         , ViewState["WONAME"].ToString()
                                         , General.GetNullableDateTime(ViewState["FDATE"].ToString())
                                         , General.GetNullableDateTime(ViewState["TDATE"].ToString())
                                         , General.GetNullableInteger(ViewState["DEPT"].ToString())
                                         , ViewState["TEAMMEMBER"].ToString());

                General.ShowExcel(heading, dt, alColumns, alCaptions, null, null);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["WONO"] = string.Empty;
                ViewState["WONAME"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                ViewState["STATUS"] = string.Empty;
                ViewState["DEPT"] = string.Empty;
                ViewState["TEAMMEMBER"] = string.Empty;
                gvMaintenanceProgress.MasterTableView.GetColumn("FLDWORKORDERNUMBER").CurrentFilterValue = ViewState["WONO"].ToString();
                gvMaintenanceProgress.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue = ViewState["WONAME"].ToString();
                gvMaintenanceProgress.MasterTableView.GetColumn("FLDSTATUS").CurrentFilterValue = ViewState["STATUS"].ToString();
                gvMaintenanceProgress.MasterTableView.GetColumn("FLDDEPARTMENTID").CurrentFilterValue = ViewState["DEPT"].ToString();
                gvMaintenanceProgress.MasterTableView.GetColumn("FLDDATE").CurrentFilterValue = string.Empty;
                gvMaintenanceProgress.MasterTableView.GetColumn("FLDOTHERMEMBERSNAME").CurrentFilterValue = string.Empty;
                gvMaintenanceProgress.CurrentPageIndex = 0;
                gvMaintenanceProgress.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMaintenance_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid grid = sender as RadGrid;
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridFilteringItem)
        {
            grid.MasterTableView.GetColumn("FLDWORKORDERNUMBER").CurrentFilterValue = ViewState["WONO"].ToString();
            grid.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue = ViewState["WONAME"].ToString();
            grid.MasterTableView.GetColumn("FLDSTATUS").CurrentFilterValue = ViewState["STATUS"].ToString();
            grid.MasterTableView.GetColumn("FLDDEPARTMENTID").CurrentFilterValue = ViewState["DEPT"].ToString();
            grid.MasterTableView.GetColumn("FLDDATE").CurrentFilterValue = ViewState["FDATE"].ToString() + '~' + ViewState["TDATE"].ToString();
            grid.MasterTableView.GetColumn("FLDOTHERMEMBERSNAME").CurrentFilterValue = ViewState["TEAMMEMBER"].ToString();
        }
        HtmlImage imgYellow = ((HtmlImage)e.Item.FindControl("imgYellow"));
        if (imgYellow != null)
        {
            if (drv["FLDISPLANCARRYOVER"].ToString().Equals("1"))
                imgYellow.Src = Session["images"] + "/yellow.png";
            else
                imgYellow.Visible = false;
        }
        LinkButton lnkWo = ((LinkButton)e.Item.FindControl("lblWO"));        
        if (lnkWo != null && drv != null)
        {           
            lnkWo.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Work Orders','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + drv["FLDWOGROUPID"].ToString() + "'); return false;");
        }
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null)
        {
            edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);            
            if (drv["FLDISINPROGRESS"].ToString().Equals("1") || drv["FLDISREPORTPENDING"].ToString().Equals("1") 
                || drv["FLDISCOMPLETED"].ToString().Equals("1"))
                edit.Visible = false;
        }
        LinkButton start = (LinkButton)e.Item.FindControl("cmdStart");
        if (start != null)
        {
            start.Visible = SessionUtil.CanAccess(this.ViewState, start.CommandName);
            start.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Start Maintenance','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceTimeSheet.aspx?wo=" + drv["FLDWOGROUPID"].ToString() + "&wopi=" + drv["FLDWODETAILID"].ToString() + "&a=2'); return false;");
            if (drv["FLDISINPROGRESS"].ToString().Equals("1") || drv["FLDISREPORTPENDING"].ToString().Equals("1")
                || drv["FLDISCOMPLETED"].ToString().Equals("1") || drv["FLDISPLANCARRYOVER"].ToString().Equals("1"))
                start.Visible = false;            
        }
        LinkButton complete = (LinkButton)e.Item.FindControl("cmdComplete");
        if (complete != null)
        {
            complete.Visible = SessionUtil.CanAccess(this.ViewState, complete.CommandName);
            complete.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Complete Maintenance','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceTimeSheet.aspx?wo=" + drv["FLDWOGROUPID"].ToString() + "&wopi=" + drv["FLDWODETAILID"].ToString() + "&a=3'); return false;");
            if (drv["FLDISCOMPLETED"].ToString() == "1")
                complete.Visible = false;
            //if (drv["FLDISPLANCARRYOVER"].ToString().Equals("0") && drv["FLDISINPROGRESS"].ToString().Equals("0")) 
            complete.Visible = false;
        }
        LinkButton reschedule = (LinkButton)e.Item.FindControl("cmdReschedule");
        if (reschedule != null)
        {
            reschedule.Visible = SessionUtil.CanAccess(this.ViewState, reschedule.CommandName);
            reschedule.Visible = false;
            if (drv["FLDISPLANNED"].ToString().Equals("1") && drv["FLDISPLANCARRYOVER"].ToString().Equals("0"))
                reschedule.Visible = true;
        }
        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null)
        {
            cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            cancel.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you sure you want cancel this record?'); return false;");
            if (drv["FLDISCOMPLETED"].ToString() == "1" || drv["FLDISUNPLANNEDJOB"].ToString() == "0" || drv["FLDISREPORTPENDING"].ToString().Equals("1"))
                cancel.Visible = false;
        }
        LinkButton tb = (LinkButton)e.Item.FindControl("cmdToolBox");
        if (tb != null)
        {
            tb.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupToolBoxMeet.aspx?groupId=" + drv["FLDWOGROUPID"] + "&WONUMBER=" + drv["FLDWORKORDERNUMBER"] + "&wopid=" + drv["FLDWODETAILID"].ToString() + "'; showDialog('Toolbox Meet - " + drv["FLDWORKORDERNAME"].ToString() + "')");
            tb.Visible = SessionUtil.CanAccess(this.ViewState, tb.CommandName);
            if (drv["FLDTOOLBOXMET"].ToString() == "0")
            {
                tb.ToolTip = "Toolbox meet not done or conditions (e.g. RA, PTW, Spares) not met";
                tb.Attributes["style"] = "color:red !important";
            }
            else if (drv["FLDTOOLBOXMET"].ToString() == "1")
            {
                tb.ToolTip = "Conditions met, Toolbox meet can be conducted but not yet done";
                tb.Attributes["style"] = "color:#9b870c !important";
            }
            else if (drv["FLDTOOLBOXMET"].ToString() == "2")
            {
                tb.ToolTip = "Toolbox meet done";
                tb.Attributes["style"] = "color:green !important";
            }
            if (drv["FLDISCOMPLETED"].ToString() == "1" || drv["FLDISREPORTPENDING"].ToString().Equals("1"))
                tb.Visible = false;
        }
        LinkButton Report = (LinkButton)e.Item.FindControl("cmdReport");
        if (Report != null)
        {
            Report.Visible = SessionUtil.CanAccess(this.ViewState, Report.CommandName);
            Report.Attributes.Add("onclick", "javascript:openNewWindow('maintjob', '" + drv["FLDWORKORDERNUMBER"].ToString() + " - " + drv["FLDWORKORDERNAME"].ToString() + "', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenancesSubWorkOrderReport.aspx?attachment=" + drv["FLDATTACHMENTCOUNT"].ToString() + "&groupId=" + drv["FLDWOGROUPID"] + "&UnplannedJob=" + drv["FLDISUNPLANNEDJOB"] + "'); ");
            if (drv["FLDISREPORTPENDING"].ToString().Equals("0"))
                Report.Visible = false;
        }
        //string url = string.Empty;
        //if (drv != null)
        //{
        //    url = "javascript:top.openNewWindow('wo','Activity - {{activity}}','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceActivity.aspx?groupId=" + drv["FLDWOGROUPID"].ToString() + "'); return false;";
        //}       
        //RadLabel lblCompletionStatus = (RadLabel)e.Item.FindControl("lblCompletionStatus");
        RadRadioButtonList divProgress = (RadRadioButtonList)e.Item.FindControl("rblInProgress");
        if(divProgress != null)
        {
            if (drv["FLDISCOMPLETED"].ToString() == "1" || drv["FLDISPLANNED"].ToString().Equals("1") ||
                 (drv["FLDISPLANCARRYOVER"].ToString().Equals("0") && drv["FLDISINPROGRESS"].ToString().Equals("0")))
                divProgress.Visible = false;           
        }
        RadRadioButtonList divPlanned = (RadRadioButtonList)e.Item.FindControl("rblPlanned");
        if (divPlanned != null)
        {
            if (drv["FLDISCOMPLETED"].ToString() == "1" || drv["FLDISINPROGRESS"].ToString().Equals("1") || drv["FLDISPLANCARRYOVER"].ToString().Equals("0"))
                divPlanned.Visible = false;
        }        
        if (drv != null && divProgress != null)
        {
            if(drv["FLDISREPORTPENDING"].ToString() == "1")
            {
                divProgress.Visible = false;
                divPlanned.Visible = false;
            }
            //else
            //{
            //    lblCompletionStatus.Visible = false;
            //}
        }
    }
    protected void gvMaintenance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDPLANNINGDUEDATE", "FLDDATE", "FLDESTSTARTTIME", "FLDDURATION", "FLDSTARTTIMEHR", "FLDENDTIMEHR", "FLDOTHERMEMBERSNAME", "FLDMAINTENANCESTATUS" };
            string[] alCaptions = { "Work Order No.", "Work Order", "Due On", "Planned On", "Est. Start Time", "Est. End Time", "Start Time", "End Time", "Team Members", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION" + grid.ClientID] == null) ? null : (ViewState["SORTEXPRESSION" + grid.ClientID].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION" + grid.ClientID] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION" + grid.ClientID].ToString());

            //int status = 0;
            //if (grid.ClientID.Contains(gvMaintenancePlanned.ClientID))
            //{
            //    status = 1;
            //}
            //else if (grid.ClientID.Contains(gvMaintenanceProgress.ClientID))
            //{
            //    status = 2;
            //}
            //else if (grid.ClientID.Contains(gvMaintenanceCompleted.ClientID))
            //{
            //    status = 3;
            //}
            DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.SearchIssuedWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                         , General.GetNullableInteger(ViewState["STATUS"].ToString())
                                         , sortexpression, sortdirection
                                         , grid.CurrentPageIndex + 1
                                         , grid.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount
                                         , ViewState["WONO"].ToString()
                                         , ViewState["WONAME"].ToString()
                                         , General.GetNullableDateTime(ViewState["FDATE"].ToString())
                                         , General.GetNullableDateTime(ViewState["TDATE"].ToString())
                                         , General.GetNullableInteger(ViewState["DEPT"].ToString())
                                         , ViewState["TEAMMEMBER"].ToString());

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions(grid.ClientID, "Maintenance", alCaptions, alColumns, ds);
            //grid.Columns.FindByUniqueName("Action").Visible = ViewState["ISACTION"].ToString() != "0";
            //if (ViewState["ISACTION"].ToString() != "0")
            //{
            //    grid.Columns.FindByUniqueName("FLDSTATUS").HeaderStyle.Width = 75;
            //}
            //else
            //{
            //    grid.Columns.FindByUniqueName("FLDSTATUS").HeaderStyle.Width = 150;
            //}
            grid.DataSource = dt;
            grid.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT" + grid.ClientID] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMaintenance_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            if (e.CommandName.ToUpper() == "START")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDWODETAILID").ToString();
                //PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWorkOrderStatus(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID, 2);
                grid.Rebind();
                //gvMaintenanceProgress.Rebind();
                string script = "refresh()";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "COMPLETE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDWODETAILID").ToString();
                //PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWorkOrderStatus(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID, 3);
                grid.Rebind();
                //gvMaintenanceCompleted.Rebind();
                string script = "refresh()";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "CANCEL")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string groupId = item.GetDataKeyValue("FLDWOGROUPID").ToString();
                string id = item.GetDataKeyValue("FLDWODETAILID").ToString();
                PhoenixPlannedMaintenanceWorkOrderGroup.GroupWoDelete(new Guid(groupId));
                PhoenixPlannedMaintenanceDailyWorkPlan.DeleteWorkOrder(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                grid.Rebind();
                string script = "refresh()";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "RESCHEDULE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string groupId = item.GetDataKeyValue("FLDWOGROUPID").ToString();
                LinkButton lnkWo = ((LinkButton)e.Item.FindControl("lblWO"));
                string id = item.GetDataKeyValue("FLDWODETAILID").ToString();
                string script = "$modalWindow.modalWindowUrl = '../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReschedule.aspx?groupId=" + groupId + "&woplanid=" + id + "';showDialog('Reschedule - " + lnkWo.Text + "');";                
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "EDITR")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDWODETAILID").ToString();
                LinkButton lbl = ((LinkButton)e.Item.FindControl("lblWO"));
                RadLabel date = ((RadLabel)e.Item.FindControl("lblDate"));
                RadLabel planid = ((RadLabel)e.Item.FindControl("lblDailyWorkPlanId"));
                string script = "function sd(){$modalWindow.modalWindowUrl='../PlannedMaintenance/PlannedMaintenanceDailyWorkPlanDetailEdit.aspx?woid=" + id + "&d=" + General.GetDateTimeToString(date.Text) + "&gid=" + grid.ClientID + "&planid=" + planid.Text + "';showDialog('Edit - " + lbl.Text + "');; Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                grid.CurrentPageIndex = 0;

                ViewState["WONO"] = grid.MasterTableView.GetColumn("FLDWORKORDERNUMBER").CurrentFilterValue;
                ViewState["WONAME"] = grid.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue;
                ViewState["STATUS"] = grid.MasterTableView.GetColumn("FLDSTATUS").CurrentFilterValue;
                ViewState["DEPT"] = grid.MasterTableView.GetColumn("FLDDEPARTMENTID").CurrentFilterValue;
                ViewState["TEAMMEMBER"] = grid.MasterTableView.GetColumn("FLDOTHERMEMBERSNAME").CurrentFilterValue;
                string daterange = grid.MasterTableView.GetColumn("FLDDATE").CurrentFilterValue;                
                if (daterange != "")
                {
                    ViewState["FDATE"] = daterange.Split('~')[0];
                    ViewState["TDATE"] = daterange.Split('~')[1];
                }
                else
                {
                    ViewState["FDATE"] = General.GetDateTimeToString(DateTime.Now.AddMonths(-1));
                    ViewState["TDATE"] = General.GetDateTimeToString(DateTime.Now);
                }
                ViewState["ISACTION"] = "1";
                if (ViewState["STATUS"].ToString() == ViewState["CAD"].ToString()
                    || ViewState["STATUS"].ToString() == ViewState["CMP"].ToString())
                    ViewState["ISACTION"] = "0";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //gvMaintenancePlanned.Rebind();
        gvMaintenanceProgress.Rebind();
        //gvMaintenanceCompleted.Rebind();
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
    protected void ddlStatus_DataBinding(object sender, EventArgs e)
    {
        RadComboBox status = (RadComboBox)sender;        
        DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, 1, "ASG,INP,CMP,CAD,REV");
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
            dt = ds.Tables[0];
        DataRow[] dr = dt.Select("FLDSHORTNAME='CAD'"); 
        if (dr != null && dr.Length > 0)
        {
            dr[0]["FLDHARDNAME"] = "Not Done"; 
            ViewState["CAD"] = dr[0]["FLDHARDCODE"].ToString();
            dt.Rows.Remove(dr[0]);
            dt.AcceptChanges();
        }        
        dr = dt.Select("FLDSHORTNAME='REV'");
        if (dr != null && dr.Length > 0)
        {
            dr[0]["FLDHARDNAME"] = "Report Pending";
        }
        dr = dt.Select("FLDSHORTNAME='CMP'"); // finds all rows with id==2 and selects first or null if haven't found any
        if (dr != null && dr.Length > 0)
        {
            ViewState["CMP"] = dr[0]["FLDHARDCODE"].ToString();
        }
        status.DataValueField = "FLDHARDCODE";
        status.DataTextField = "FLDHARDNAME";
        status.DataSource = dt;
        status.Items.Add(new RadComboBoxItem("Planned / In-Progress", ""));        
    }

    protected void ddlDepartment_DataBinding(object sender, EventArgs e)
    {
        RadComboBox status = (RadComboBox)sender;
        DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 51, 1, "DEK,ENG");
        status.DataValueField = "FLDHARDCODE";
        status.DataTextField = "FLDHARDNAME";
        status.DataSource = ds;
        status.Items.Add(new RadComboBoxItem("All", ""));
    }
    protected void cmdHiddenActivity_Click1(object sender, EventArgs e)
    {
        try
        {
            RadButton btn = (RadButton)sender;
            string args = btn.CommandArgument;
            string[] val = args.Split('~');
            string completionstatus = val[0];
            string id = val[1];
            string status = val[2];
            Guid? ActivityId = General.GetNullableGuid(id);
            int? Status = General.GetNullableInteger(status);


            PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWorkOrderStatus(new Guid(id)
                           , PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(status), DateTime.Now, General.GetNullableInteger(completionstatus), null, null);
            PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(ActivityId, 2, General.GetNullableInteger(completionstatus), null, null);

            PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWRH(ActivityId, 2, General.GetNullableInteger(completionstatus));


            if (completionstatus == "5")
            {
                PhoenixPlannedMaintenanceDailyWorkPlan.CarryForwardWorkOrder(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            }
            gvMaintenanceProgress.Rebind();
        }
        catch (Exception ex)
        {
            RadWindowManager1.RadAlert("<span class=\"bold-red\">" + ex.Message.Replace("'", "&lsquo;") + "<span>", 330, 180, "Message", null);
        }
    }
}