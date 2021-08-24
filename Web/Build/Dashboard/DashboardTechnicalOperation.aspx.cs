using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Dashboard_DashboardTechnicalOperation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
       
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Dashboard/DashboardTechnicalOperation.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvProgress')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Dashboard/DashboardTechnicalOperation.aspx?" + Request.QueryString.ToString(), "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("javascript:$modalWindow.modalWindowUrl='../PlannedMaintenance/PlannedMaintenanceDailyWorkPlanDetailActivity.aspx';showDialog('Add',true);", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDWO");
        //toolbar.AddFontAwesomeButton("javascript:expandcollapse('gvProgress');", "Expand/Collapse", "<i class=\"fas fa-expand-alt\"></i>", "EXPANDCOLLAPSE");
        //MenuProgress.Title = "Operation In Progress";
        MenuProgress.AccessRights = this.ViewState;
        MenuProgress.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PROCESS"] = string.Empty;
            ViewState["ACTIVITY"] = string.Empty;
            ViewState["DATE"] = string.Empty;
            ViewState["EXPCLP"] = string.Empty;
            int pagesize = General.ShowRecords(null);
            gvProgress.PageSize = pagesize;
            //gvPlanned.PageSize = pagesize;
            //gvCompleted.PageSize = pagesize;
            ViewState["FDATE"] = string.Empty;
            ViewState["TDATE"] = string.Empty;
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
            ViewState["STATUS"] = Request.QueryString["status"] ?? string.Empty;
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
                string gridId = "gvProgress";
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDELEMENTNAME", "FLDACTIVITYNAME", "FLDDATE", "FLDESTSTARTTIME", "FLDDURATION", "FLDSTARTTIMEHR", "FLDENDTIMEHR", "FLDOTHERMEMBERSNAME", "FLDRANUMBER", "FLDOPERATIONSTATUS" };
                string[] alCaptions = { "Process", "Activity", "Date", "Est. Start Time", "Est. End Time", "Start Time", "End Time", "Team Members", "RA", "Status" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION" + gridId] == null) ? null : (ViewState["SORTEXPRESSION" + gridId].ToString());
                if (ViewState["SORTDIRECTION" + gridId] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT" + gridId] == null || Int32.Parse(ViewState["ROWCOUNT" + gridId].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT" + gridId].ToString());

                string heading = "Operation";

                DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.SearchActivityOperation(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                             , null
                                             , ViewState["PROCESS"].ToString()
                                             , ViewState["ACTIVITY"].ToString()
                                             , General.GetNullableDateTime(ViewState["DATE"].ToString())
                                             , sortexpression, sortdirection
                                             , 1
                                             , iRowCount
                                             , ref iRowCount
                                             , ref iTotalPageCount
                                             , General.GetNullableDateTime(ViewState["FDATE"].ToString())
                                             , General.GetNullableDateTime(ViewState["TDATE"].ToString())
                                             , ViewState["TEAMMEMBER"].ToString());

                General.ShowExcel(heading, dt, alColumns, alCaptions, null, null);
               
            }            
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PROCESS"] = string.Empty;
                ViewState["ACTIVITY"] = string.Empty;
                ViewState["DATE"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                ViewState["STATUS"] = Request.QueryString["status"] ?? string.Empty;
                ViewState["TEAMMEMBER"] = string.Empty;
                gvProgress.CurrentPageIndex = 0;
                gvProgress.MasterTableView.GetColumn("FLDELEMENTNAME").CurrentFilterValue = ViewState["PROCESS"].ToString();
                gvProgress.MasterTableView.GetColumn("FLDACTIVITYNAME").CurrentFilterValue = ViewState["ACTIVITY"].ToString();
                gvProgress.MasterTableView.GetColumn("FLDSTATUS").CurrentFilterValue = ViewState["STATUS"].ToString();
                gvProgress.MasterTableView.GetColumn("FLDDATE").CurrentFilterValue = string.Empty;
                gvProgress.MasterTableView.GetColumn("FLDOTHERMEMBERSNAME").CurrentFilterValue = ViewState["TEAMMEMBER"].ToString();
                gvProgress.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOperation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid grid = sender as RadGrid;
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridFilteringItem)
        {
            grid.MasterTableView.GetColumn("FLDELEMENTNAME").CurrentFilterValue = ViewState["PROCESS"].ToString();
            grid.MasterTableView.GetColumn("FLDACTIVITYNAME").CurrentFilterValue = ViewState["ACTIVITY"].ToString();
            grid.MasterTableView.GetColumn("FLDSTATUS").CurrentFilterValue = ViewState["STATUS"].ToString();           
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
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null)
        {
            edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);            
            if (drv["FLDISINPROGRESS"].ToString().Equals("1") || drv["FLDISCANCELLED"].ToString().Equals("1")
                || drv["FLDISCOMPLETED"].ToString().Equals("1"))
                edit.Visible = false;
        }
        LinkButton start = (LinkButton)e.Item.FindControl("cmdStart");
        if (start != null)
        {
            start.Visible = SessionUtil.CanAccess(this.ViewState, start.CommandName);
            start.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Start Operation','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceTimeSheet.aspx?act=" + drv["FLDACTIVITYID"].ToString() + "&actpi=" + drv["FLDDAILYPLANACTIVITYID"].ToString() + "&a=2', true); return false;");
            if (drv["FLDISCOMPLETED"].ToString() == "1" || drv["FLDISCANCELLED"].ToString().Equals("1"))
                start.Visible = false;
            if (drv["FLDISINPROGRESS"].ToString().Equals("1"))
                start.Visible = false;
            if (drv["FLDISPLANCARRYOVER"].ToString().Equals("1")) start.Visible = false;
        }
        LinkButton complete = (LinkButton)e.Item.FindControl("cmdComplete");
        if (complete != null)
        {
            complete.Visible = SessionUtil.CanAccess(this.ViewState, complete.CommandName);
            complete.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Complete Operation','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceTimeSheet.aspx?act=" + drv["FLDACTIVITYID"].ToString() + "&actpi=" + drv["FLDDAILYPLANACTIVITYID"].ToString() + "&a=3', true); return false;");
            if (drv["FLDISCOMPLETED"].ToString() == "1")
                complete.Visible = false;
            if (drv["FLDISPLANCARRYOVER"].ToString().Equals("0") && drv["FLDISINPROGRESS"].ToString().Equals("0")) complete.Visible = false;
            complete.Visible = false;
        }
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            if (drv["FLDISCOMPLETED"].ToString() == "1" || drv["FLDISCANCELLED"].ToString().Equals("1") || drv["FLDISINPROGRESS"].ToString().Equals("1"))
                del.Visible = false;
        }
        LinkButton reschedule = (LinkButton)e.Item.FindControl("cmdReschedule");
        if (reschedule != null)
        {
            reschedule.Visible = SessionUtil.CanAccess(this.ViewState, reschedule.CommandName);
            if (drv["FLDISCOMPLETED"].ToString() == "1")
                reschedule.Visible = false;
            if (drv["FLDISINPROGRESS"].ToString().Equals("1"))
                reschedule.Visible = false;
            reschedule.Visible = false;
        }
        LinkButton timesheet = (LinkButton)e.Item.FindControl("cmdTimeSheet");
        if (timesheet != null)
        {
            timesheet.Visible = SessionUtil.CanAccess(this.ViewState, timesheet.CommandName);
            timesheet.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','Time Sheet','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceTimeSheet.aspx?act=" + drv["FLDACTIVITYID"].ToString() + "&actpi=" + drv["FLDDAILYPLANACTIVITYID"].ToString() + "&ea=1&ev=AOP'); return false;");
            if (drv["FLDISINPROGRESS"].ToString().Equals("0"))
                timesheet.Visible = false;
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
                    //e.Item.Cells[grid.Columns.FindByUniqueName("FLDFORMLIST").OrderIndex].Controls.Add(new Literal() { Text = "<br/>" });
                }
            }
        }
        RadLabel lblCompletionStatus = (RadLabel)e.Item.FindControl("lblCompletionStatus");
        RadRadioButtonList divProgress = (RadRadioButtonList)e.Item.FindControl("rblInProgress");
        if (divProgress != null)
        {
            if (drv["FLDISCOMPLETED"].ToString() == "1" || drv["FLDISPLANNED"].ToString().Equals("1") 
                || drv["FLDISCANCELLED"].ToString().Equals("1") || (drv["FLDISPLANCARRYOVER"].ToString().Equals("0") && drv["FLDISINPROGRESS"].ToString().Equals("0")))
                divProgress.Visible = false;
        }
        RadRadioButtonList divPlanned = (RadRadioButtonList)e.Item.FindControl("rblPlanned");
        if (divPlanned != null)
        {
            if (drv["FLDISCOMPLETED"].ToString() == "1" || drv["FLDISINPROGRESS"].ToString().Equals("1") 
                || drv["FLDISCANCELLED"].ToString().Equals("1") || drv["FLDISPLANCARRYOVER"].ToString().Equals("0"))
                divPlanned.Visible = false;
        }
        //if(drv != null && lblCompletionStatus != null)
        //{
        //    if (drv["FLDISINPROGRESS"].ToString().Equals("1") || drv["FLDISPLANNED"].ToString().Equals("1"))
        //        lblCompletionStatus.Visible = false;
        //}
    }
    protected void gvOperation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDELEMENTNAME", "FLDACTIVITYNAME", "FLDDATE", "FLDESTSTARTTIME", "FLDDURATION", "FLDSTARTTIMEHR", "FLDENDTIMEHR", "FLDOTHERMEMBERSNAME", "FLDRANUMBER", "FLDOPERATIONSTATUS" };
            string[] alCaptions = { "Process", "Activity", "Date", "Est. Start Time", "Est. End Time", "Start Time", "End Time", "Team Members", "RA", "Status" };

            //string[] alColumnsProg = { "FLDELEMENTNAME", "FLDACTIVITYNAME", "FLDSTARTTIME", "FLDDURATION", "FLDOTHERMEMBERSNAME", "FLDRANUMBER" };
            //string[] alCaptionsProg = { "Element", "Activity", "Start Time", "Est. End Time", "Other Members", "RA" };

            //string[] alColumnsComp = { "FLDELEMENTNAME", "FLDACTIVITYNAME", "FLDSTARTTIME", "FLDCOMPLETEDTIME", "FLDOTHERMEMBERSNAME", "FLDRANUMBER" };
            //string[] alCaptionsComp = { "Element", "Activity", "Start Time", "End Time", "Other Members", "RA" };

            string sortexpression = (ViewState["SORTEXPRESSION" + grid.ClientID] == null) ? null : (ViewState["SORTEXPRESSION" + grid.ClientID].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION" + grid.ClientID] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION" + grid.ClientID].ToString());
           
            DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.SearchActivityOperation(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                         , General.GetNullableInteger(ViewState["STATUS"].ToString())
                                         , ViewState["PROCESS"].ToString()
                                         , ViewState["ACTIVITY"].ToString()
                                         , General.GetNullableDateTime(ViewState["DATE"].ToString())
                                         , sortexpression, sortdirection
                                         , grid.CurrentPageIndex + 1
                                         , grid.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount
                                         , General.GetNullableDateTime(ViewState["FDATE"].ToString())
                                         , General.GetNullableDateTime(ViewState["TDATE"].ToString())
                                         , ViewState["TEAMMEMBER"].ToString());

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
    protected void gvOperation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            if (e.CommandName.ToUpper() == "START")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDAILYPLANACTIVITYID").ToString();
                //PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityOperation(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID, 2);
                //grid.Rebind();
                //gvProgress.Rebind();
                string script = "refresh()";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "COMPLETE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDAILYPLANACTIVITYID").ToString();
                //PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityOperation(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID, 3);
                grid.Rebind();
                //gvCompleted.Rebind();
                string script = "refresh()";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDAILYPLANACTIVITYID").ToString();
                PhoenixPlannedMaintenanceDailyWorkPlan.DeleteActivity(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                //grid.Rebind();
                string script = "refresh();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "RESCHEDULE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDAILYPLANACTIVITYID").ToString();
                RadLabel lbl = ((RadLabel)e.Item.FindControl("lblActivity"));
                string script = "function sd(){$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl='../PlannedMaintenance/PlannedMaintenanceDailyWorkPlanActivityReschedule.aspx?id=" + id + "&gid=" + grid.ClientID + "';showDialog('Reschedule - " + lbl.Text + "'); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (e.CommandName.ToUpper() == "EDITR")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDDAILYPLANACTIVITYID").ToString();
                RadLabel lbl = ((RadLabel)e.Item.FindControl("lblActivity"));
                RadLabel date = ((RadLabel)e.Item.FindControl("lblDate"));
                RadLabel planid = ((RadLabel)e.Item.FindControl("lblDailyWorkPlanId"));
                string script = "function sd(){$modalWindow.modalWindowID = '" + RadWindow_NavigateUrl.ClientID + "';$modalWindow.modalWindowUrl='../PlannedMaintenance/PlannedMaintenanceDailyWorkPlanDetailEdit.aspx?id=" + id + "&d=" + General.GetDateTimeToString(date.Text) + "&gid=" + grid.ClientID + "&planid=" + planid.Text + "';showDialog('Edit - " + lbl.Text + "',true); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                grid.CurrentPageIndex = 0;

                ViewState["PROCESS"] = grid.MasterTableView.GetColumn("FLDELEMENTNAME").CurrentFilterValue;
                ViewState["ACTIVITY"] = grid.MasterTableView.GetColumn("FLDACTIVITYNAME").CurrentFilterValue;
                ViewState["STATUS"] = grid.MasterTableView.GetColumn("FLDSTATUS").CurrentFilterValue;
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
        //gvPlanned.Rebind();
        gvProgress.Rebind();
        //gvCompleted.Rebind();
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
        DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, 1, "ASG,INP,CMP,CAD");
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
            dt = ds.Tables[0];
        DataRow[] dr = dt.Select("FLDSHORTNAME='CAD'"); // finds all rows with id==2 and selects first or null if haven't found any
        if (dr != null && dr.Length > 0)
        {
            dr[0]["FLDHARDNAME"] = "Not Done"; //changes the Product_name
            ViewState["CAD"] = dr[0]["FLDHARDCODE"].ToString();
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

    protected void cmdHiddenActivity_Click1(object sender, EventArgs e)
    {
        try
        {
            RadButton btn = (RadButton)sender;
            string args = btn.CommandArgument;
            string[] val = args.Split('~');
           
            string ID = val[0];
            string status = val[1];

            Guid? ActivityId = General.GetNullableGuid(ID);
            int? Status = General.GetNullableInteger(status);

            PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityOperation(ActivityId, PhoenixSecurityContext.CurrentSecurityContext.VesselID
                              , 3, DateTime.Now, Status, null, null);

            PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(ActivityId, 1, Status, null, null);

            PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWRH(ActivityId, 1, Status);
            gvProgress.Rebind();
        }
        catch (Exception ex)
        {
            RadWindowManager1.RadAlert("<span class=\"bold-red\">" + ex.Message.Replace("'", "&lsquo;") + "<span>", 330, 180, "Message", null);
        }
    }
}