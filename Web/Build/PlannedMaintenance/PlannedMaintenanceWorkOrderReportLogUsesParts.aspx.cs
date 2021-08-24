using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderReportLogUsesParts : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogUsesParts.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvUsesParts')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenugvUsesParts.MenuList = toolbar.Show();
        //MenugvUsesParts.SetTrigger(pnlgvUsesParts);
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["WORKORDERID"] = String.Empty;
            ViewState["WORKORDERREPORTID"] = "";
            if (Request.QueryString["WORKORDERID"] != null)
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
            ViewState["VESSELID"] = "0";
            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"];
            else
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            getReportId();
            gvUsesParts.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
 
    }
    private void getReportId()
    {
        try
        {
            if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
            {
                DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.EditWorkOrderReport(new Guid(Request.QueryString["WORKORDERID"]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["ISREPORTPATRS"] = dr["FLDREPORTUSEDPARTS"].ToString();
                    ViewState["WORKORDERREPORTID"] = dr["FLDWORKORDERREPORTID"].ToString();
                    ViewState["OPERATIONMODE"] = "EDIT";
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDLOCATIONNAME", "FLDQUANTITY", "FLDUNITNAME" };
        string[] alCaptions = { "Part Number", "Part Name", "Location", "Quantity", "Unit" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.WorkOrderReportPartConsumedSearchNew(General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
                                                                                                , int.Parse(ViewState["VESSELID"].ToString())
                                                                                                , sortexpression
                                                                                                , sortdirection
                                                                                                , gvUsesParts.CurrentPageIndex + 1
                                                                                                , gvUsesParts.PageSize
                                                                                                , ref iRowCount
                                                                                                , ref iTotalPageCount);
        General.SetPrintOptions("gvUsesParts", "Stock Used", alCaptions, alColumns, ds);

        gvUsesParts.DataSource = ds;
        gvUsesParts.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
 
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDLOCATIONNAME", "FLDQUANTITY", "FLDUNITNAME" };
        string[] alCaptions = { "Part Number", "Part Name", "Location", "Quantity", "Unit" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPlannedMaintenanceWorkOrderReport.WorkOrderReportPartConsumedSearchNew(General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
                                                                                        , int.Parse(ViewState["VESSELID"].ToString())
                                                                                        , sortexpression
                                                                                        , sortdirection
                                                                                        , gvUsesParts.CurrentPageIndex + 1
                                                                                        , gvUsesParts.PageSize
                                                                                        , ref iRowCount
                                                                                        , ref iTotalPageCount);
        General.ShowExcel("Stock Used", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void gvUsesParts_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            gvUsesParts.ExportSettings.Excel.Format = GridExcelExportFormat.Biff;
            gvUsesParts.ExportSettings.IgnorePaging = true;
            gvUsesParts.ExportSettings.ExportOnlyData = true;
            gvUsesParts.ExportSettings.OpenInNewWindow = true;
            ShowExcel();
        }

    }
    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        //{
        //    GridEditableItem item = e.Item as GridEditableItem;

        //    string disciplineCode = ((RadTextBox)e.Item.FindControl("txtDisciplineCodeEdit")).Text;
        //    string disciplineName = ((RadTextBox)e.Item.FindControl("txtDisciplineNameEdit")).Text;
        //}
    }

}
