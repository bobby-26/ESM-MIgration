using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web;


public partial class Inspection_InspectionWRHNCSummaryCountbySysCause : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT");
            MenuReportsFilter.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddImageButton("../Inspection/InspectionWRHNCSummaryCountbySysCause.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbar1.AddImageButton("../Inspection/InspectionWRHNCSummaryCountbySysCause.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null; ViewState["SUMMARY"] = "1";
                div2.Visible = false;
                bindSystemCauseList();
            }
            ShowReport();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {

                ucFromDate.Text = "";
                ucToDate.Text = "";
                ucVessel.SelectedVesselValue = "";
                ucFleet.SelectedFleetValue = "";
                foreach (ListItem item in chkboxlstSysCause.Items)
                {
                    item.Selected = false;

                    if (item.Text == ("--Check All--"))
                    {
                        item.Attributes.Add("onclick", "checkUnchekAll(this);");
                    }
                }
                ShowReport();
                SetPageNavigator();

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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;


            //if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            //{
            //    InspectionFilter.CurrentWRHNcTypeCountbySysCauseFilter = null;
            //    sessionFilterValues();
            //}
            //if (dce.CommandName.ToUpper().Equals("SHOWVISUAL"))
            //{
            //    sessionFilterValues();
            //    Response.Redirect("../Inspection/InspectionWRHNCTypeCountbySysCauseVisual.aspx");
            //}

            ViewState["PAGENUMBER"] = 1;
            ShowReport();
            SetPageNavigator();
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
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDSYSTEMCAUSE", "FLDPERIOD", "FLDNCCOUNT" };
        string[] alCaptions = { "System Cause", "Month", "Total No. of NC`s" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionWRHNCTypeCountbySysCause.NcSummaryCountbySystemCause((ucFleet.SelectedList.ToString()) == "," ? null : General.GetNullableString(ucFleet.SelectedList.ToString())
                                                                                    , (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString())
                                                                                    , selectedSysCause() == "" ? null : selectedSysCause()
                                                                                    , General.GetNullableDateTime(ucFromDate.Text)
                                                                                    , General.GetNullableDateTime(ucToDate.Text)
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
        foreach (ListItem item in chkboxlstSysCause.Items)
        {
            if (item.Text == ("--Check All--"))
            {
                item.Attributes.Add("onclick", "checkUnchekAll(this);");
            }
        }

        ViewState["SHOWREPORT"] = 1;
        divPage.Visible = true;
        divTab1.Visible = true;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSYSTEMCAUSE", "FLDPERIOD", "FLDNCCOUNT" };
        string[] alCaptions = { "System Cause", "Month", "Total No. of NC`s" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        ds = PhoenixInspectionWRHNCTypeCountbySysCause.NcSummaryCountbySystemCause((ucFleet.SelectedList.ToString()) == "," ? null : General.GetNullableString(ucFleet.SelectedList.ToString())
                                                                                 , (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString())
                                                                                 , selectedSysCause() == "" ? null : selectedSysCause()
                                                                                 , General.GetNullableDateTime(ucFromDate.Text)
                                                                                 , General.GetNullableDateTime(ucToDate.Text)
                                                                                 , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                 , General.ShowRecords(null)
                                                                                 , ref iRowCount
                                                                                 , ref iTotalPageCount);

        General.SetPrintOptions("gvCrew", "WRH NC Summary Count by System Cause", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrew.DataSource = ds;
            gvCrew.DataBind();
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCrew);
            ViewState["ROWSINGRIDVIEW"] = 0;
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvCrew.EditIndex = -1;
        gvCrew.SelectedIndex = -1;
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
        ShowReport();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvCrew.SelectedIndex = -1;
        gvCrew.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        ShowReport();
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
        SetPageNavigator();
    }
    protected void gvCrew_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvCrew.EditIndex = -1;
        gvCrew.SelectedIndex = -1;
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

    public void bindSystemCauseList()
    {
        chkboxlstSysCause.DataSource = PhoenixInspectionWRHNCTypeCountbyReason.getReasonList(86);
        chkboxlstSysCause.DataBind();
    }
    protected void OnDataBound(object sender, EventArgs e)
    {
        ListItem li = new ListItem("--Check All--", "0");
        li.Attributes.Add("onclick", "checkUnchekAll(this);");
        chkboxlstSysCause.Items.Insert(0, li);
    }

    public string selectedSysCause()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ListItem item in chkboxlstSysCause.Items)
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
}
