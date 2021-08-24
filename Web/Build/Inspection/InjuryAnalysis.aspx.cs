using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web;
using Telerik.Web.UI;

public partial class Inspection_InjuryAnalysis : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Visual", "SHOWVISUAL", ToolBarDirection.Right);
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.MenuList = toolbar.Show();
            MenuReportsFilter.SelectedMenuIndex = 1;

            toolbar = new PhoenixToolbar();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Inspection/InjuryAnalysis.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar1.AddFontAwesomeButton("../Inspection/InjuryAnalysis.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SUMMARY"] = "1";
                // div2.Visible = false;
                BindYear();
                sessionFilterValues();

            }

            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        // bind filter criteria if any
        BindFilterCriteria();
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                InspectionFilter.CurrentInspectionInjuryAnalysisFilter = null;
                ddlYear.SelectedValue = System.DateTime.Today.Year.ToString();
                lstQuarter.SelectedValue = null;
                lstMonth.SelectedValue = null;
                ucFleet.SelectedFleetValue = "";
                ucVessel.SelectedVessel = null;
                ucRank.SelectedRankValue = "";

                ShowReport();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                InspectionFilter.CurrentInspectionInjuryAnalysisFilter = null;
                sessionFilterValues();
            }

            if (CommandName.ToUpper().Equals("SHOWVISUAL"))
            {
                sessionFilterValues();
                Response.Redirect("../Inspection/InjuryAnalysisDataVisual.aspx");
            }

            ViewState["PAGENUMBER"] = 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindFilterCriteria()
    {

        NameValueCollection nvc = InspectionFilter.CurrentInspectionInjuryAnalysisFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
        }

        string Year = CheckIsNull(nvc.Get("ddlYear"));
        CheckUncheck(Year, "ddlYear");
        string Quarter = CheckIsNull(nvc.Get("lstQuarter"));
        CheckUncheck(Quarter, "Quarter");
        string Month = CheckIsNull(nvc.Get("lstMonth"));
        CheckUncheck(Month, "Month");
        ucVessel.SelectedVessel = CheckIsNull(nvc.Get("ucVessel"));
        ucRank.selectedlist = CheckIsNull(nvc.Get("ucRank"));
        ucFleet.SelectedList = CheckIsNull(nvc.Get("ucFleet"));
        ShowReport();
    }

    private string CheckIsNull(string value)
    {
        return value == null ? "" : value;
    }
    public NameValueCollection sessionFilterValues()
    {
        NameValueCollection nvc = new NameValueCollection();

        if (IsPostBack)
        {
            nvc.Clear();
            nvc.Add("ddlYear", YearList());
            nvc.Add("lstMonth", selectedMonthList());
            nvc.Add("lstQuarter", selectedQuarterList());
            nvc.Add("ucFleet", ucFleet.SelectedList.ToString());
            nvc.Add("ucVessel", ucVessel.SelectedVessel.ToString());
            nvc.Add("ucRank", ucRank.selectedlist.ToString());

            InspectionFilter.CurrentInspectionInjuryAnalysisFilter = nvc;
        }
        return InspectionFilter.CurrentInspectionInjuryAnalysisFilter;
    }

    protected void CheckUncheck(string Values, string ID)
    {
        string[] values = Values.Split(',');
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = values[i].Trim();
            if (ID == "ddlYear")
            {
                ddlYear.Items.FindByValue(values[i]).Selected = true;
            }
            else if (ID == "Quarter")
                lstQuarter.Items.FindByValue(values[i]).Selected = true;
            else if (ID == "Month")
                lstMonth.Items.FindByValue(values[i]).Selected = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDINJUREDEMPLOYEENAME", "FLDAGE", "FLDRANKCODE", "FLDSIMS", "FLDFLEETCODE", "FLDVESSELTYPECATEGORYNAME", "FLDINCIDENTREFNO", "FLDINCIDENTMONTHNAME"
                , "FLDCONSEQUENCECATEGORYNAME", "FLDINCIDENTDATE", "FLDTYPEOFINJURYNAME","FLDPARTOFTHEBODYINJUREDNAME","FLDDAYNIGHT"};
        string[] alCaptions = { "EmployeeName", "Age", "Rank", "SIM's Y/N", "Fleet", "VesselType", "Ref.No", "Month","Cons.Category","Incident Date","Type of Injury"
                ,"Part of the body","Day/Night" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionInjuryAnalysis.getDataInjuryAnalysis(YearList()
                                                                    , selectedQuarterList() == "" ? null : selectedQuarterList()
                                                                    , selectedMonthList() == "" ? null : selectedMonthList()
                                                                    , (ucFleet.SelectedList.ToString()) == "," ? null : General.GetNullableString(ucFleet.SelectedList.ToString())
                                                                    , (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString())
                                                                    , (ucRank.selectedlist.ToString()) == "," ? null : General.GetNullableString(ucRank.selectedlist.ToString())
                                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , General.ShowRecords(null)
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=" + ucTitle.Text + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + ucTitle.Text + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");

        if (ViewState["SUMMARY"].ToString() == "1")
        {
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            Response.Write("<tr>");
            //for (int i = 0; i < alCaptions1.Length; i++)
            //{
            //    Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            //    Response.Write("<b>" + alCaptions1[i] + "</b>");
            //    Response.Write("</td>");
            //}
            Response.Write("</tr>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
    }

    private void ShowReport()
    {
        ViewState["SHOWREPORT"] = 1;
        //divPage.Visible = true;
        //divTab1.Visible = true;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDINJUREDEMPLOYEENAME", "FLDAGE", "FLDRANKCODE", "FLDSIMS", "FLDFLEETCODE", "FLDVESSELTYPECATEGORYNAME", "FLDINCIDENTREFNO", "FLDINCIDENTMONTHNAME"
                , "FLDCONSEQUENCECATEGORYNAME", "FLDINCIDENTDATE", "FLDTYPEOFINJURYNAME","FLDPARTOFTHEBODYINJUREDNAME","FLDDAYNIGHT"};
        string[] alCaptions = { "EmployeeName", "Age", "Rank", "SIM's Y/N", "Fleet", "VesselType", "Ref.No", "Month","Cons.Category","Incident Date","Type of Injury"
                ,"Part of the body","Day/Night" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //if (filterPurchase.CurrentPurchaseEfficiencyFilter != null)
        //{
        //    NameValueCollection Filter = filterPurchase.CurrentPurchaseEfficiencyFilter;

        //    ds = PhoenixInspectionInjuryAnalysis.getDataInjuryAnalysis(General.GetNullableString(Filter.Get("ddlYear").ToString())
        //                                                    , General.GetNullableString(Filter.Get("lstQuarter").ToString())
        //                                                    , General.GetNullableString(Filter.Get("lstMonth").ToString())
        //                                                    , General.GetNullableString(Filter.Get("chkGroupList").ToString())
        //                                                    , General.GetNullableString(Filter.Get("lstPurchaseLocation").ToString())
        //                                                    , General.GetNullableString(Filter.Get("ddlType"))
        //                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
        //                                                    , General.ShowRecords(null)
        //                                                    , ref iRowCount
        //                                                    , ref iTotalPageCount);
        //}
        //else
        //{
        ds = PhoenixInspectionInjuryAnalysis.getDataInjuryAnalysis(YearList()
                                                                 , selectedQuarterList() == "" ? null : selectedQuarterList()
                                                                 , selectedMonthList() == "" ? null : selectedMonthList()
                                                                 , (ucFleet.SelectedList.ToString()) == "," ? null : General.GetNullableString(ucFleet.SelectedList.ToString())
                                                                 , (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString())
                                                                 , (ucRank.selectedlist.ToString()) == "," ? null : General.GetNullableString(ucRank.selectedlist.ToString())
                                                                 , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                 , General.ShowRecords(null)
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);
        //}


        gvCrew.DataSource = ds;

    }

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    gvCrew.EditIndex = -1;
    //    gvCrew.SelectedIndex = -1;
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    ShowReport();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvCrew.SelectedIndex = -1;
    //    gvCrew.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    ShowReport();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}
    protected void gvCrew_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ShowReport();
    }
    protected void gvCrew_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        ShowReport();
    }
    protected void gvCrew_RowCommand(object sender, GridViewCommandEventArgs e)
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

    protected void BindYear()
    {
        int index = 1;
        ddlYear.Items.Insert(0, new ListItem("--Select--", ""));
        for (int i = (DateTime.Today.Year); i >= 2010; i--)
        {
            ListItem li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Insert(index, new ListItem(li.ToString(), li.ToString()));
            index++;
        }

        if (InspectionFilter.CurrentInspectionInjuryAnalysisFilter == null)
        {
            if (ddlYear.SelectedValue == "")
                ddlYear.SelectedValue = DateTime.Today.Year.ToString();
        }
    }

    private string YearList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ListItem item in ddlYear.Items)
        {
            if (item.Selected == true)
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }
    private string selectedMonthList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ListItem item in lstMonth.Items)
        {
            if (item.Text != "--Select--")
            {
                if (item.Selected == true)
                {
                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }
            }
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }

    private string selectedQuarterList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ListItem item in lstQuarter.Items)
        {
            if (item.Text != "--Select--")
            {
                if (item.Selected == true)
                {
                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }
            }
        }
        if (strlist.Length > 1)
        {
            strlist.Remove(strlist.Length - 1, 1);
        }
        return strlist.ToString();
    }

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ShowReport();
    }
}
