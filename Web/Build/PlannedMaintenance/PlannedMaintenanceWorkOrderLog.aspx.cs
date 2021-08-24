using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;

public partial class PlannedMaintenance_PlannedMaintenanceWorkOrderLog : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderLog.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvWorkOrderLog')", "Print Grid", "icon_print.png", "PRINT");
        MenugvWorkOrderLog.MenuList = toolbar.Show();
        MenugvWorkOrderLog.SetTrigger(pnlgvWorkOrderLog);
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["WORKORDERID"] = null;
        }
        BindData();

    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDWORKDONEDATE", "FLDWORKDURATION", "FLDWORKDONEBY", "FLDSPENTHOURS", "FLDOVERDUEDAYS" };
        string[] alCaptions = { "Work Order Number", "Work Order Title", "Work Done Date", "Duration", "Work Done By","Spent Hours","Over Due Days" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds=null;
        if (Filter.CurrentWorkOrderLogFilter != null)
        {
            NameValueCollection nvc = Filter.CurrentWorkOrderLogFilter;
            ds = PhoenixPlannedMaintenanceWorkOrder.WorkOrderLogSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , General.GetNullableString(nvc.Get("txtWorkOrderNumber").ToString())
                , General.GetNullableString(nvc.Get("txtWorkOrderTitle").ToString())
                , General.GetNullableDateTime(nvc.Get("txtWorkDoneDateFrom").ToString())
                , General.GetNullableDateTime(nvc.Get("txtWorkDoneDateTo").ToString())
                , General.GetNullableString(nvc.Get("ucWorkDoneBy").ToString())
                , sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
        }
        General.SetPrintOptions("gvWorkOrderLog", "Work Order Log", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvWorkOrderLog.DataSource = ds;
            gvWorkOrderLog.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvWorkOrderLog);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }
    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }
    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }
    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvWorkOrderLog.SelectedIndex = -1;
        gvWorkOrderLog.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvWorkOrderLog.EditIndex = -1;
        gvWorkOrderLog.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }
    protected void gvWorkOrderLog_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

    }
    protected void gvWorkOrderLog_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkOrderLog_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvWorkOrderLog.EditIndex = -1;
        gvWorkOrderLog.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void gvWorkOrderLog_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDWORKDONEDATE", "FLDWORKDURATION", "FLDWORKDONEBY", "FLDSPENTHOURS", "FLDOVERDUEDAYS" };
        string[] alCaptions = { "Work Order Number", "Work Order Title", "Work Done Date", "Duration", "Work Done By", "Spent Hours", "Over Due Days" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (Filter.CurrentWorkOrderLogFilter != null)
        {
            NameValueCollection nvc = Filter.CurrentWorkOrderLogFilter;
            ds = PhoenixPlannedMaintenanceWorkOrder.WorkOrderLogSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableString(nvc.Get("txtWorkOrderNumber").ToString()), General.GetNullableString(nvc.Get("txtWorkOrderTitle").ToString()), General.GetNullableDateTime(nvc.Get("txtWorkDoneDateFrom").ToString()), General.GetNullableDateTime(nvc.Get("txtWorkDoneDateTo").ToString()), General.GetNullableString(nvc.Get("ucWorkDoneBy").ToString()), sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
        }
        General.ShowExcel("WorkOrderLog", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        
    }
}
