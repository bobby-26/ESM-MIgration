using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderGroupReportList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReportList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvMaintenanceLog')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupLogFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReportList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuDivWorkOrder.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["WORKORDERGROUPID"] = null;

                gvMaintenanceLog.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "FIND")
            {
                gvMaintenanceLog.CurrentPageIndex = 0;
                gvMaintenanceLog.Rebind();
            }
            else if (CommandName.ToUpper() == "EXCEL")
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper() == "CLEAR")
            {
                ViewState["WORKORDERGROUPID"] = null;
                Filter.CurrentWorkOrderReportLogFilter = null;
                gvMaintenanceLog.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDJOBCATEGORY", "FLDDONEDATE", "FLDDISCIPLINENAME", "FLDPLANNINGESTIMETDURATION" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Category", "Done Date", "Responsibility", "Total Duration" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = null;

            if (Filter.CurrentWorkOrderReportLogFilter != null)
            {
                NameValueCollection nvc = Filter.CurrentWorkOrderFilter;
                ds = PhoenixPlannedMaintenanceWorkOrderGroupLog.GroupLogSearch(General.GetNullableString(nvc.Get("txtWorkOrderNumber"))
                    , General.GetNullableString(nvc.Get("txtWorkOrderName"))
                    , null
                    , General.GetNullableInteger(nvc.Get("ucCompCategory"))
                    , General.GetNullableDateTime(nvc.Get("txtDateFrom"))
                    , General.GetNullableDateTime(nvc.Get("txtDateTo"))
                    , sortexpression, sortdirection
                    , gvMaintenanceLog.CurrentPageIndex + 1
                    , gvMaintenanceLog.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , General.GetNullableInteger(nvc.Get("rcbIsPlanned")));
            }
            else
            {
                ds = PhoenixPlannedMaintenanceWorkOrderGroupLog.GroupLogSearch(null, null, null, null, null, null, sortexpression, sortdirection, gvMaintenanceLog.CurrentPageIndex + 1, gvMaintenanceLog.PageSize, ref iRowCount, ref iTotalPageCount, null);
            }

            General.SetPrintOptions("gvMaintenanceLog", "Work Order Group Log", alCaptions, alColumns, ds);

            gvMaintenanceLog.DataSource = ds;
            gvMaintenanceLog.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["WORKORDERGROUPID"] == null)
                {
                    ViewState["WORKORDERGROUPID"] = ds.Tables[0].Rows[0]["FLDWORKORDERGROUPID"].ToString();
                }
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDJOBCATEGORY", "FLDDONEDATE", "FLDDISCIPLINENAME", "FLDPLANNINGESTIMETDURATION" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Category", "Done Date", "Responsibility", "Total Duration" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            DataSet ds = null;
            if (Filter.CurrentWorkOrderReportLogFilter != null)
            {
                NameValueCollection nvc = Filter.CurrentWorkOrderFilter;
                ds = PhoenixPlannedMaintenanceWorkOrderGroupLog.GroupLogSearch(General.GetNullableString(nvc.Get("txtWorkOrderNumber"))
                    , General.GetNullableString(nvc.Get("txtWorkOrderName"))
                    , null
                    , General.GetNullableInteger(nvc.Get("ucCompCategory"))
                    , General.GetNullableDateTime(nvc.Get("txtDateFrom"))
                    , General.GetNullableDateTime(nvc.Get("txtDateTo"))
                    , sortexpression, sortdirection
                    , gvMaintenanceLog.CurrentPageIndex + 1
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount
                    , General.GetNullableInteger(nvc.Get("rcbIsPlanned")));
            }
            else
            {
                ds = PhoenixPlannedMaintenanceWorkOrderGroupLog.GroupLogSearch(null, null, null, null, null, null, sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), iRowCount, ref iRowCount, ref iTotalPageCount, null);
            }
            General.ShowExcel("Maintenance Log", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMaintenanceLog_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvMaintenanceLog_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            gvMaintenanceLog.ExportSettings.Excel.Format = GridExcelExportFormat.Biff;
            gvMaintenanceLog.ExportSettings.IgnorePaging = true;
            gvMaintenanceLog.ExportSettings.ExportOnlyData = true;
            gvMaintenanceLog.ExportSettings.OpenInNewWindow = true;
            ShowExcel();
        }
        if (e.CommandName == RadGrid.RebindGridCommandName)
        {
            gvMaintenanceLog.CurrentPageIndex = 0;
        }
        if (e.CommandName == "Select")
        {
            ViewState["WORKORDERGROUPID"] = ((RadLabel)e.Item.FindControl("lblWorkOrderGroupId")).Text;
            Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReportLogList.aspx?islog=1&Workordergroupid=" + ViewState["WORKORDERGROUPID"]);
        }
    }

    protected void gvMaintenanceLog_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton history = (LinkButton)e.Item.FindControl("cmdhistory");
            if (history != null)
            {
                history.Visible = SessionUtil.CanAccess(this.ViewState, history.CommandName);
                if (drv["FLDISVERIFICATION"].ToString() != "1")
                    history.Visible = false;

                history.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkorderVerificationHistory.aspx?groupid=" + drv["FLDWORKORDERGROUPID"] + "&Vesselid=" + drv["FLDVESSELID"] + "');");
            }
        }
    }
}