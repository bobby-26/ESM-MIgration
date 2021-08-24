using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class InspectionReviewPlannerReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            ucCharterer.AddressType = ((int)PhoenixAddressType.CHARTERER).ToString();
            ucPrincipal.AddressType = ((int)PhoenixAddressType.PRINCIPAL).ToString();

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Inspection/InspectionReviewPlannerReport.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvReviewPlanner')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Inspection/InspectionReviewPlannerReport.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Inspection/InspectionReviewPlannerReport.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuReviewPlanner.AccessRights = this.ViewState;
            MenuReviewPlanner.MenuList = toolbargrid.Show();
            MenuReviewPlanner.SetTrigger(pnlReviewPlanner);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;

                
                Filter.CurrentReviewPlannerReport = null;
                                
            }
            
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void gvReviewPlanner_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvReviewPlanner.EditIndex = -1;
        gvReviewPlanner.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
        SetPageNavigator();
    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Reference Number", "Name", "Vessel", "Last Done Date", "Due Date", "Planned Date", "Name Of Inspector", "Port of Audit", "Status" };
        string[] alColumns = { "FLDREFERENCENUMBER", "FLDREVIEWNAME", "FLDVESSELSHORTCODE", "FLDLASTDONEDATE", "FLDREVIEWSTARTDATE", "FLDRANGEFROMDATE", "FLDNAMEOFINSPECTOR", "FLDSEAPORTNAME", "FLDSTATUSNAME" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentReviewPlannerReport;

        ds = PhoenixInspectionReports.AuditInspectionPlannerReport(nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselName").ToString()) : null
                 , nvc != null ? General.GetNullableInteger(nvc.Get("ucFleet").ToString().Trim()) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucPrincipal").ToString().Trim()) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucCharterer").ToString().Trim()) : null
                , nvc != null ? General.GetNullableInteger(null) : null 
                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucDueDateFrom")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucDueDateTo")) : null
                , null
                , null
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , iRowCount
                , ref iRowCount
                , ref iTotalPageCount
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : null);
                
        Response.AddHeader("Content-Disposition", "attachment; filename=ReviewPlannerReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Audit/Inspection Planner</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void MenuReviewPlanner_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();//Filter.CurrentReconciledInvoiceFilterSelection = null;
            }
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                Filter.CurrentReconciledInvoiceFilterSelection = null;
                NameValueCollection criteria = new NameValueCollection();                              

                criteria.Clear();
                criteria.Add("ucVesselName", ucVesselName.SelectedVessel);
                criteria.Add("ucFleet", ucFleet.SelectedFleet);
                criteria.Add("ucPrincipal", ucPrincipal .SelectedAddress);
                criteria.Add("ucCharterer", ucCharterer .SelectedAddress);
                criteria.Add("ucDueDateFrom",ucDueDateFrom.Text);
                criteria.Add("ucDueDateTo", ucDueDateTo.Text);
                criteria.Add("ucStatus", ucStatus.SelectedHard);

                Filter.CurrentReviewPlannerReport = criteria;
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentReviewPlannerReport = null;
                ucDueDateFrom.Text = ucDueDateTo.Text = "";
                ucVesselName.SelectedVessel = ucFleet.SelectedFleet = ucPrincipal .SelectedAddress = ucCharterer .SelectedAddress = "";
                ucStatus.SelectedHard = "";

                BindData();
                SetPageNavigator();
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
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        
        NameValueCollection nvc = Filter.CurrentReviewPlannerReport;

        ds = PhoenixInspectionReports.AuditInspectionPlannerReport(nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselName").ToString()) : null
                 , nvc != null ? General.GetNullableInteger(nvc.Get("ucFleet").ToString().Trim()) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucPrincipal").ToString().Trim()) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucCharterer").ToString().Trim()) : null
                , nvc != null ? General.GetNullableInteger (null):null  
                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucDueDateFrom")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucDueDateTo")) : null
                , null
                , null
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , General.ShowRecords(null)
                , ref iRowCount
                , ref iTotalPageCount
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : null);          

        string[] alCaptions = { "Reference Number", "Name", "Vessel", "Last Done Date", "Due Date", "Planned Date", "Name Of Inspector", "Port of Audit", "Status" };
        string[] alColumns = { "FLDREFERENCENUMBER", "FLDREVIEWNAME", "FLDVESSELSHORTCODE", "FLDLASTDONEDATE", "FLDREVIEWSTARTDATE", "FLDRANGEFROMDATE", "FLDNAMEOFINSPECTOR", "FLDSEAPORTNAME", "FLDSTATUSNAME" };

        General.SetPrintOptions("gvReviewPlanner", "Audit/Inspection Planner", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvReviewPlanner.DataSource = ds;
            gvReviewPlanner.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvReviewPlanner);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            gvReviewPlanner.SelectedIndex = -1;
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReviewPlanner_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        try
        {
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvReviewPlanner.SelectedIndex = -1;
            gvReviewPlanner.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
}
