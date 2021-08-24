using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewBatchListReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Crew/CrewBatchListReport.aspx?" + Request.QueryString, "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvBatchList')", "Print Grid", "icon_print.png", "PRINT");
        toolbar.AddImageButton("../Crew/CrewBatchListReport.aspx?" + Request.QueryString, "Find", "search.png", "FIND");
        toolbar.AddImageButton("../Crew/CrewBatchListReport.aspx?" + Request.QueryString, "Clear Filter", "clear-filter.png", "CLEAR");
        //toolbar.AddImageLink("../Crew/CrewBatchListReport.aspx?" + Request.QueryString, "Add Batch", "add.png", "ADD");

        MenuCrewBatchList.AccessRights = this.ViewState;
        MenuCrewBatchList.MenuList = toolbar.Show();
        MenuCrewBatchList.SetTrigger(pnlBatch);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
           
            //cmdHiddenSubmit.Attributes.Add("style", "display:none;");            
            txtInstituteId.Attributes.Add("style", "display:none;");          

            DateTime today = DateTime.Today;
            var thisWeekStart = today.AddDays(-(int)today.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            txtStartDate.Text = thisWeekStart.ToString();
            txtEndDate.Text = thisWeekEnd.ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void gvBatchList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#bbddff'");
            // when mouse leaves the row, change the bg color to its original value   
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
        }
    }
    protected void MenuCrewBatchList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDBATCHNO", "FLDNAME", "FLDHARDNAME" };
            string[] alCaptions = { "S.No", "Batch No", "Institute" ,"Status"};

            string sortexpression, courseInstituteId = null;
            int? sortdirection = 1;

            if (Request.QueryString["courseInstituteId"] != null)
                courseInstituteId = Request.QueryString["courseInstituteId"].ToString();

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            //if (!string.IsNullOrEmpty(ucBatchStatus.SelectedHard) && ucBatchStatus.SelectedHard.ToUpper() != "DUMMY")
            //    status = ucBatchStatus.SelectedHard;

            DataSet ds = PhoenixCrewInstituteBatch.CrewInstituteBatchSearch(General.GetNullableString(txtBatchNoSearch.Text)
                                                                     , General.GetNullableInteger(txtInstituteId.Text)
                                                                     , General.GetNullableInteger(ddlCourse.SelectedCourse)
                                                                     , null
                                                                     , txtInstituteName.Text
                                                                     , General.GetNullableDateTime(txtStartDate.Text)
                                                                     , General.GetNullableDateTime(txtEndDate.Text)
                                                                     , General.GetNullableInteger(ucBatchStatus.SelectedHard)
                                                                     , sortexpression
                                                                     , sortdirection
                                                                     , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                     , General.ShowRecords(null)
                                                                     , ref iRowCount
                                                                     , ref iTotalPageCount);

            if (ds.Tables.Count > 0)
                General.ShowExcel("Batch List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        if (dce.CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["PAGENUMBER"] = 1;
            txtnopage.Text = "";
            txtBatchNoSearch.Text = "";
            //txtStartDate.Text = "";
            //txtEndDate.Text = "";
            txtInstituteId.Text = "";
            txtInstituteName.Text = "";
            ddlCourse.SelectedCourse = "";
            ucBatchStatus.SelectedHard = "0";
            BindData();
            SetPageNavigator();
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string status = null;

        string[] alColumns = { "FLDROWNUMBER", "FLDBATCHNO", "FLDNAME", "FLDHARDNAME" };
        string[] alCaptions = { "S.No", "Batch No", "Institute", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (!string.IsNullOrEmpty(ucBatchStatus.SelectedHard) && ucBatchStatus.SelectedHard.ToUpper() != "DUMMY")
            status = ucBatchStatus.SelectedHard;
        string courseInstituteId = null;
        if (Request.QueryString["courseInstituteId"] != null)
            courseInstituteId = Request.QueryString["courseInstituteId"].ToString();

        if (string.IsNullOrEmpty(txtStartDate.Text) || string.IsNullOrEmpty(txtEndDate.Text))
        {
            ucError.Text = "Duration is required";
            return;
        }
        DataSet ds = PhoenixCrewInstituteBatch.CrewInstituteBatchSearch(General.GetNullableString(txtBatchNoSearch.Text)
                                                                     , General.GetNullableInteger(txtInstituteId.Text)
                                                                     , General.GetNullableInteger(ddlCourse.SelectedCourse)
                                                                     , null
                                                                     , General.GetNullableString(txtInstituteName.Text)
                                                                     , General.GetNullableDateTime(txtStartDate.Text)
                                                                     , General.GetNullableDateTime(txtEndDate.Text)
                                                                     , General.GetNullableInteger(ucBatchStatus.SelectedHard)
                                                                     , sortexpression
                                                                     , sortdirection
                                                                     , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                     , General.ShowRecords(null)
                                                                     , ref iRowCount
                                                                     , ref iTotalPageCount);
        General.SetPrintOptions("gvBatchList", "Batch List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBatchList.DataSource = ds;
            gvBatchList.DataBind();
            gvBatchList.SelectedIndex = 0;
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvBatchList);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvBatchList.SelectedIndex = -1;
            gvBatchList.EditIndex = -1;
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
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
        {
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            txtInstituteId.Text = nvc[1];
            txtInstituteName.Text = nvc[2];

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}