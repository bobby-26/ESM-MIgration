using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceOperationsMaintenanceCalendarDay : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)

    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceOperationsMaintenanceCalendarDay.aspx?grdid=gvMaintCompleted", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMaintCompleted')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:expandcollapse('gvMaintCompleted');", "Expand/Collapse", "<i class=\"fas fa-expand-alt\"></i>", "EXPANDCOLLAPSE");
        MenuMaintCompleted.Title = "Maintenance";
        MenuMaintCompleted.AccessRights = this.ViewState;
        MenuMaintCompleted.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceOperationsMaintenanceCalendarDay.aspx?grdid=gvOperationsCompleted", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOperationsCompleted')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:expandcollapse('gvOperationsCompleted');", "Expand/Collapse", "<i class=\"fas fa-expand-alt\"></i>", "EXPANDCOLLAPSE");
        MenuOperationsCompleted.Title = "Operations";
        MenuOperationsCompleted.AccessRights = this.ViewState;
        MenuOperationsCompleted.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PROCESS"] = string.Empty;
            ViewState["ACTIVITY"] = string.Empty;
            ViewState["WONUMBER"] = string.Empty;
            ViewState["WONAME"] = string.Empty;

            ViewState["EXPCLP"] = string.Empty;
            txtStartDate.SelectedDate = DateTime.Today;
            int pagesize = General.ShowRecords(null);
            gvMaintCompleted.PageSize = pagesize;
            gvOperationsCompleted.PageSize = pagesize;
            PopuldateElement();
        }
    }
    protected void MenuOperationsCompleted_TabStripCommand(object sender, EventArgs e)
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

                string[] alColumns = { "FLDELEMENTNAME", "FLDACTIVITYNAME", "FLDACTUALSTARTTIME", "FLDACTUALCOMPLETEDTIME", "FLDOTHERMEMBERSNAME", "FLDRANUMBER"};
                string[] alCaptions = { "Element", "Activity", "Start Time", "End Time", "Other Members", "RA" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION" + gridId] == null) ? null : (ViewState["SORTEXPRESSION" + gridId].ToString());
                if (ViewState["SORTDIRECTION" + gridId] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT" + gridId] == null || Int32.Parse(ViewState["ROWCOUNT" + gridId].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT" + gridId].ToString());

                string heading = string.Empty;

                heading = "Operations";
                    DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.SearchActivityOperationByDate(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                 , ViewState["PROCESS"].ToString()
                                                 , ViewState["ACTIVITY"].ToString()
                                                 , General.GetNullableDateTime(txtStartDate.SelectedDate.ToString())
                                                 , sortexpression, sortdirection
                                                 , 1
                                                 , iRowCount
                                                 , ref iRowCount
                                                 , ref iTotalPageCount);

                    General.ShowExcel(heading, dt, alColumns, alCaptions, null, null);                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuMaintCompleted_TabStripCommand(object sender, EventArgs e)
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

                string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDACTUALSTARTTIME", "FLDACTUALCOMPLETEDTIME", "FLDOTHERMEMBERSNAME", "FLDDEPT" };
                string[] alCaptions = { "Work Order No.", "Work Order", "Start Time", "End Time", "Other Members", "Dept" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION" + gridId] == null) ? null : (ViewState["SORTEXPRESSION" + gridId].ToString());
                if (ViewState["SORTDIRECTION" + gridId] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT" + gridId] == null || Int32.Parse(ViewState["ROWCOUNT" + gridId].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT" + gridId].ToString());

                string heading = string.Empty;

                heading = "Maintenance";
                DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.SearchIssuedWorkOrderByDate(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                             , ViewState["WONUMBER"].ToString()
                                             , ViewState["WONAME"].ToString()
                                             , General.GetNullableDateTime(txtStartDate.SelectedDate.ToString())
                                             , sortexpression, sortdirection
                                             , 1
                                             , iRowCount
                                             , ref iRowCount
                                             , ref iTotalPageCount);

                General.ShowExcel(heading, dt, alColumns, alCaptions, null, null);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOperationsCompleted_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
  
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                grid.CurrentPageIndex = 0;

                ViewState["PROCESS"] = grid.MasterTableView.GetColumn("FLDELEMENTNAME").CurrentFilterValue;
                ViewState["ACTIVITY"] = grid.MasterTableView.GetColumn("FLDACTIVITYNAME").CurrentFilterValue;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOperationsCompleted_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid grid = sender as RadGrid;
        DataRowView drv = (DataRowView)e.Item.DataItem;
        LinkButton start = (LinkButton)e.Item.FindControl("cmdStart");
        if (start != null)
        {
            start.Visible = SessionUtil.CanAccess(this.ViewState, start.CommandName);
            start.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Start Operation','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceTimeSheet.aspx?act=" + drv["FLDACTIVITYID"].ToString() + "&actpi=" + drv["FLDDAILYPLANACTIVITYID"].ToString() + "&a=2'); return false;");
        }
        LinkButton complete = (LinkButton)e.Item.FindControl("cmdComplete");
        if (complete != null)
        {
            complete.Visible = SessionUtil.CanAccess(this.ViewState, complete.CommandName);
            complete.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Complete Operation','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceTimeSheet.aspx?act=" + drv["FLDACTIVITYID"].ToString() + "&actpi=" + drv["FLDDAILYPLANACTIVITYID"].ToString() + "&a=3'); return false;");
        }
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
        }
        LinkButton reschedule = (LinkButton)e.Item.FindControl("cmdReschedule");
        if (reschedule != null)
        {
            reschedule.Visible = SessionUtil.CanAccess(this.ViewState, reschedule.CommandName);
        }
        LinkButton timesheet = (LinkButton)e.Item.FindControl("cmdTimeSheet");
        if (timesheet != null)
        {
            timesheet.Visible = SessionUtil.CanAccess(this.ViewState, timesheet.CommandName);
            timesheet.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Time Sheet','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceTimeSheet.aspx?act=" + drv["FLDACTIVITYID"].ToString() + "&actpi=" + drv["FLDDAILYPLANACTIVITYID"].ToString() + "&ea=1&ev=AOP'); return false;");
        }
        LinkButton ra = (LinkButton)e.Item.FindControl("lnkRA");
        if (ra != null)
        {
            if (!drv["FLDRISKASSESSMENTPROCESSID"].ToString().Equals(Guid.Empty.ToString()))
                ra.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','RA','" + Session["sitepath"] + "/Inspection/InspectionRAProcessExtn.aspx?processid=" + drv["FLDRISKASSESSMENTPROCESSID"].ToString() + "&status='); return false;");
            else
                ra.Enabled = false;
        }
        if (ra != null)
        {
            string[] forms = drv["FLDFORMLIST"].ToString().Split('`');
            if (forms.Length > 0)
            {
                foreach (string s in forms)
                {
                    if (s.Trim().Length == 0) continue;
                    LinkButton lnk = new LinkButton();
                    string[] data = s.Split('~');
                    lnk.Text = data[1] + ",";
                    if (data[3] != string.Empty)
                    {
                        DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(data[3]));
                        if (dt.Rows.Count > 0)
                        {
                            DataRow drRow = dt.Rows[0];
                            lnk.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Forms & Checklist - [" + data[1] + "]','" + Session["sitepath"] + "/Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString() + "'); return false;");
                        }
                    }
                    else
                    {
                        lnk.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Forms & Checklist','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + data[0] + "&FORMREVISIONID=" + data[2] + "&dwpaid=" + drv["FLDDAILYPLANACTIVITYID"].ToString() + "'); return false;");
                    }
                    e.Item.Cells[grid.Columns.FindByUniqueName("FLDFORMLIST").OrderIndex].Controls.Add(lnk);
                    e.Item.Cells[grid.Columns.FindByUniqueName("FLDFORMLIST").OrderIndex].Controls.Add(new Literal() { Text = "<br/>" });
                }
            }
        }
    }

    protected void gvMaintCompleted_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;

            if (e.CommandName == RadGrid.FilterCommandName)
            {
                grid.CurrentPageIndex = 0;

                ViewState["WONUMBER"] = grid.MasterTableView.GetColumn("FLDWORKORDERNUMBER").CurrentFilterValue;
                ViewState["WONAME"] = grid.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMaintCompleted_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        LinkButton lnkWo = ((LinkButton)e.Item.FindControl("lblWO"));
        if (lnkWo != null && drv != null)
        {
            lnkWo.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Work Orders','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + drv["FLDWOGROUPID"].ToString() + "'); return false;");
        }
        LinkButton start = (LinkButton)e.Item.FindControl("cmdStart");
        if (start != null)
        {
            start.Visible = SessionUtil.CanAccess(this.ViewState, start.CommandName);
            start.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Start Maintenance','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceTimeSheet.aspx?wo=" + drv["FLDWOGROUPID"].ToString() + "&wopi=" + drv["FLDWODETAILID"].ToString() + "&a=2'); return false;");
        }
        LinkButton complete = (LinkButton)e.Item.FindControl("cmdComplete");
        if (complete != null)
        {
            complete.Visible = SessionUtil.CanAccess(this.ViewState, complete.CommandName);
            complete.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Complete Maintenance','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceTimeSheet.aspx?wo=" + drv["FLDWOGROUPID"].ToString() + "&wopi=" + drv["FLDWODETAILID"].ToString() + "&a=3'); return false;");
        }
        LinkButton reschedule = (LinkButton)e.Item.FindControl("cmdReschedule");
        if (reschedule != null)
        {
            reschedule.Visible = SessionUtil.CanAccess(this.ViewState, reschedule.CommandName);
        }
        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null)
        {
            start.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            cancel.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you sure you want cancel this record?'); return false;");
        }
        LinkButton tb = (LinkButton)e.Item.FindControl("cmdToolBox");
        if (tb != null)
        {
            tb.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupToolBoxMeet.aspx?groupId=" + drv["FLDWOGROUPID"] + "&WONUMBER=" + drv["FLDWORKORDERNUMBER"] + "'; showDialog('Toolbox Meet - " + drv["FLDWORKORDERNAME"].ToString() + "')");
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
        }
        LinkButton Report = (LinkButton)e.Item.FindControl("cmdReport");
        if (Report != null)
        {
            Report.Visible = SessionUtil.CanAccess(this.ViewState, Report.CommandName);
            Report.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenancesSubWorkOrderReport.aspx?attachment=" + drv["FLDATTACHMENTCOUNT"].ToString() + "&groupId=" + drv["FLDWOGROUPID"] + "&UnplannedJob=" + drv["FLDISUNPLANNEDJOB"] + "'); ");
        }
    }

    protected void gvMaintCompleted_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDACTUALSTARTTIME", "FLDACTUALCOMPLETEDTIME", "FLDOTHERMEMBERSNAME", "FLDDEPT" };
            string[] alCaptions = { "Work Order No.", "Work Order", "Start Time", "End Time", "Other Members", "Dept" };

            string sortexpression = (ViewState["SORTEXPRESSION" + grid.ClientID] == null) ? null : (ViewState["SORTEXPRESSION" + grid.ClientID].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION" + grid.ClientID] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION" + grid.ClientID].ToString());
            
            DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.SearchIssuedWorkOrderByDate(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                         , ViewState["WONUMBER"].ToString()
                                         , ViewState["WONAME"].ToString()
                                         , General.GetNullableDateTime(txtStartDate.SelectedDate.ToString())
                                         , sortexpression, sortdirection
                                         , grid.CurrentPageIndex + 1
                                         , grid.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions(grid.ClientID, "Maintenance", alCaptions, alColumns, ds);

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
    protected void gvOperationsCompleted_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDELEMENTNAME", "FLDACTIVITYNAME", "FLDACTUALSTARTTIME", "FLDACTUALCOMPLETEDTIME", "FLDOTHERMEMBERSNAME", "FLDRANUMBER" };
            string[] alCaptions = { "Element", "Activity", "Start Time", "End Time", "Other Members", "RA" };

            string sortexpression = (ViewState["SORTEXPRESSION" + grid.ClientID] == null) ? null : (ViewState["SORTEXPRESSION" + grid.ClientID].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION" + grid.ClientID] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION" + grid.ClientID].ToString());
           
                DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.SearchActivityOperationByDate(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                             , ViewState["PROCESS"].ToString()
                                             , ViewState["ACTIVITY"].ToString()
                                             , General.GetNullableDateTime(txtStartDate.SelectedDate.ToString())
                                             , sortexpression, sortdirection
                                             , grid.CurrentPageIndex + 1
                                             , grid.PageSize
                                             , ref iRowCount
                                             , ref iTotalPageCount);

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());
                General.SetPrintOptions(grid.ClientID, "Operation", alCaptions, alColumns, ds);

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
    private void PopuldateElement()
    {
        DataSet ds = PhoenixInspectionRiskAssessmentCategoryExtn.ListDailyworkplanProcess();
        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                cblElement.Items.Add(new ButtonListItem(dr["FLDNAME"].ToString(), dr["FLDCATEGORYID"].ToString()));
            }
        }
    }
    protected void cblElement_SelectedIndexChanged(object sender, EventArgs e)
    {
        cblActivity.DataSource = null;
        cblActivity.Items.Clear();
        foreach (var item in cblElement.SelectedItems)
        {
            DataTable dt = GetActivity(int.Parse(item.Value));
            foreach (DataRow dr in dt.Rows)
            {
                ButtonListItem itm = new ButtonListItem(dr["FLDNAME"].ToString(), dr["FLDACTIVITYID"].ToString() + "~" + dr["FLDCATEGORYID"].ToString());
                cblActivity.Items.Add(itm);
            }
        }
    }
    private DataTable GetActivity(int ElementId)
    {
        DataSet ds = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentActivity(ElementId);
        return ds.Tables[0];
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                string csvActivity = string.Empty;
                foreach (var item in cblActivity.SelectedItems)
                {
                    csvActivity = csvActivity + item.Value + ",";
                }
                csvActivity = csvActivity.Trim(',');
                PhoenixPlannedMaintenanceDailyWorkPlan.InsertActivity(null, PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , csvActivity);
                cblActivity.DataSource = null;
                cblActivity.Items.Clear();
                foreach (var item in cblElement.SelectedItems)
                {
                    item.Selected = false;
                }
                string script = "function f(){CloseModelWindow(); refresh();Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            catch (Exception ex)
            {
                RequiredFieldValidator Validator = new RequiredFieldValidator();
                Validator.ErrorMessage = "* " + ex.Message;
                Validator.ValidationGroup = "group1";
                Validator.IsValid = false;
                Validator.Visible = false;
                Page.Form.Controls.Add(Validator);
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {        
        gvMaintCompleted.Rebind();
        gvOperationsCompleted.Rebind();
    }
    
    protected void txtStartDate_TextChangedEvent(object sender, EventArgs e)
    {
        gvMaintCompleted.Rebind();
        gvOperationsCompleted.Rebind();
    }

    protected void Prev_ServerClick(object sender, EventArgs e)
    {
        if (txtStartDate.SelectedDate.HasValue)
        {
            txtStartDate.SelectedDate = txtStartDate.SelectedDate.Value.AddDays(-1);
        }
        gvMaintCompleted.Rebind();
        gvOperationsCompleted.Rebind();
    }
    protected void Next_ServerClick(object sender, EventArgs e)
    {
        if (txtStartDate.SelectedDate.HasValue)
        {
            txtStartDate.SelectedDate = txtStartDate.SelectedDate.Value.AddDays(1);
        }
        gvMaintCompleted.Rebind();
        gvOperationsCompleted.Rebind();
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
}