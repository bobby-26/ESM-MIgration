using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceTimeSheetList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceTimeSheetList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvTimeSheet')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbarmain.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceTimeSheetList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuTimeSheet.AccessRights = this.ViewState;
        MenuTimeSheet.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["FDATE"] = DateTime.Now.AddMonths(-1).ToString();
            ViewState["TDATE"] = DateTime.Now.ToString();
            ViewState["STATUS"] = string.Empty;
            ViewState["OPERATION"] = string.Empty;
            ViewState["ENTEREDBY"] = string.Empty;
            ViewState["SOURCE"] = string.Empty;
            gvTimeSheet.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void MenuTimeSheet_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDVESSELSTATUSNAME", "FLDOPERATION", "FLDDATETIME", "FLDDETAIL", "FLDTYPE", "FLDCREATEDBYNAME", "FLDCREATEDDATE" };
                string[] alCaptions = { "Vessel Status", "Operation", "Time", "Details", "Source", "Entered By", "Entered Date" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataTable dt = PhoenixPlannedMaintenanceTimeSheet.Search(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                         , General.GetNullableDateTime(General.GetDateTimeToString(ViewState["FDATE"].ToString()))
                                         , General.GetNullableDateTime(General.GetDateTimeToString(ViewState["TDATE"]))
                                         , sortexpression, sortdirection
                                         , 1
                                         , iRowCount
                                         , ref iRowCount
                                         , ref iTotalPageCount);

                General.ShowExcel("Time Sheet", dt, alColumns, alCaptions, null, null);
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["STATUS"] = string.Empty;
                ViewState["OPERATION"] = string.Empty;
                ViewState["ENTEREDBY"] = string.Empty;
                ViewState["SOURCE"] = string.Empty;
                gvTimeSheet.MasterTableView.GetColumn("FLDVESSELSTATUSNAME").CurrentFilterValue = ViewState["STATUS"].ToString();
                gvTimeSheet.MasterTableView.GetColumn("FLDOPERATION").CurrentFilterValue = ViewState["OPERATION"].ToString();
                gvTimeSheet.MasterTableView.GetColumn("FLDTYPE").CurrentFilterValue = ViewState["SOURCE"].ToString();
                gvTimeSheet.MasterTableView.GetColumn("FLDCREATEDBYNAME").CurrentFilterValue = ViewState["ENTEREDBY"].ToString();                
                gvTimeSheet.CurrentPageIndex = 0;
                gvTimeSheet.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTimeSheet_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELSTATUSNAME", "FLDOPERATION", "FLDDATETIME", "FLDDETAIL", "FLDTYPE", "FLDCREATEDBYNAME", "FLDCREATEDDATE" };
        string[] alCaptions = { "Vessel Status", "Operation", "Time", "Details", "Source", "Entered By", "Entered Date" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataTable dt = PhoenixPlannedMaintenanceTimeSheet.Search(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        , General.GetNullableDateTime(General.GetDateTimeToString(ViewState["FDATE"].ToString()))
                                        , General.GetNullableDateTime(General.GetDateTimeToString(ViewState["TDATE"]))
                                        , General.GetNullableGuid(ViewState["STATUS"].ToString())
                                        , ViewState["OPERATION"].ToString()
                                        , General.GetNullableInteger(ViewState["SOURCE"].ToString())
                                        , ViewState["ENTEREDBY"].ToString()
                                        , sortexpression, sortdirection
                                        , grid.CurrentPageIndex + 1
                                        , grid.PageSize
                                        , ref iRowCount
                                        , ref iTotalPageCount);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions(grid.ClientID, "Timesheet", alCaptions, alColumns, ds);

        grid.DataSource = dt;
        grid.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvTimeSheet_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid grid = sender as RadGrid;
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridFilteringItem)
        {
            grid.MasterTableView.GetColumn("FLDVESSELSTATUSNAME").CurrentFilterValue = ViewState["STATUS"].ToString();
            grid.MasterTableView.GetColumn("FLDOPERATION").CurrentFilterValue = ViewState["OPERATION"].ToString();
            grid.MasterTableView.GetColumn("FLDTYPE").CurrentFilterValue = ViewState["SOURCE"].ToString();
            grid.MasterTableView.GetColumn("FLDCREATEDBYNAME").CurrentFilterValue = ViewState["ENTEREDBY"].ToString();
            grid.MasterTableView.GetColumn("FLDDATETIME").CurrentFilterValue = ViewState["FDATE"].ToString() + '~' + ViewState["TDATE"].ToString();

        }
        LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
        if (db != null)
        {
            db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            if (drv != null && drv["FLDISDELETED"].ToString() == "1")
            {
                e.Item.Attributes.Add("style", "text-decoration: line-through;");
                db.Visible = false;
            }
        }        
    }

    protected void gvTimeSheet_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.CommandName.ToUpper() == "DELETE")
        {
            GridDataItem item = (GridDataItem)e.Item;
            string id = item.GetDataKeyValue("FLDTIMESHEETID").ToString();
            PhoenixPlannedMaintenanceTimeSheet.Delete(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);            
        }
        else if (e.CommandName == RadGrid.FilterCommandName)
        {
            
            string daterange = grid.MasterTableView.GetColumn("FLDDATETIME").CurrentFilterValue;
            if (daterange != "")
            {
                ViewState["FDATE"] = daterange.Split('~')[0];
                ViewState["TDATE"] = daterange.Split('~')[1];
            }
            string status = grid.MasterTableView.GetColumn("FLDVESSELSTATUSNAME").CurrentFilterValue;
            ViewState["STATUS"] = status;
            string operation = grid.MasterTableView.GetColumn("FLDOPERATION").CurrentFilterValue;
            ViewState["OPERATION"] = operation;
            string source = grid.MasterTableView.GetColumn("FLDTYPE").CurrentFilterValue;
            ViewState["SOURCE"] = source;
            string enteredby = grid.MasterTableView.GetColumn("FLDCREATEDBYNAME").CurrentFilterValue;
            ViewState["ENTEREDBY"] = enteredby;
            
        }
    }

    protected void ddlVesselStatus_DataBinding(object sender, EventArgs e)
    {
        RadComboBox status = sender as RadComboBox;
        DataSet ds = PhoenixRegistersDMROperationalTask.DMROperationalTaskList();
        status.DataSource = ds;
        status.DataTextField = "FLDTASKNAME";
        status.DataValueField = "FLDOPERATIONALTASKID";
        status.Items.Insert(0, new RadComboBoxItem("--All--", string.Empty));
    }
}