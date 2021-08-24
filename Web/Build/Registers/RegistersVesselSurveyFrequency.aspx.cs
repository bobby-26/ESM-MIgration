using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class Registers_RegistersVesselSurveyFrequency : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../Registers/RegistersVesselSurveyFrequency.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvSurveyFreq')", "Print Grid", "icon_print.png", "PRINT");
                MenuRegistersSurveyFreq.AccessRights = this.ViewState;
                MenuRegistersSurveyFreq.MenuList = toolbar.Show();

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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        //int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSURVEYTYPENAME", "FLDFREQUENCY", "FLDWINDOWPERIODBEFORE", "FLDWINDOWPERIODAFTER" };
        string[] alCaptions = { "Survey Type", "Frequency(Months)", "Window Period Before(Months)", "Window Period After(Months)" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //ds = PhoenixRegistersVesselSurveyFrequency.SurveyFrequencySearch(
        //    1,
        //    iRowCount,
        //    ref iRowCount,
        //    ref iTotalPageCount);

        General.ShowExcel("Survey Frequency", ds.Tables[0], alColumns, alCaptions, null, null);
    }

    protected void MenuRegistersSurveyFreq_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        //int iRowCount = 0;
        //int iTotalPageCount = 0;

        string[] alColumns = { "FLDSURVEYTYPENAME", "FLDFREQUENCY", "FLDWINDOWPERIODBEFORE", "FLDWINDOWPERIODAFTER" };
        string[] alCaptions = { "Survey Type", "Frequency(Months)","Window Period Before(Months)","Window Period After(Months)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        //DataSet ds = PhoenixRegistersVesselSurveyFrequency.SurveyFrequencySearch(
        //    (int)ViewState["PAGENUMBER"]
        //    , General.ShowRecords(null)
        //    , ref iRowCount
        //    , ref iTotalPageCount);

        //General.SetPrintOptions("gvSurveyFreq", "Survey Frequency", alCaptions, alColumns, ds);

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    gvSurveyFreq.DataSource = ds;
        //    gvSurveyFreq.DataBind();
        //}
        //else
        //{
        //    DataTable dt = ds.Tables[0];
        //    ShowNoRecordsFound(dt, gvSurveyFreq);
        //}

        //ViewState["ROWCOUNT"] = iRowCount;
        //ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvSurveyFreq_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
        SetPageNavigator();
    }

    protected void gvSurveyFreq_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();
        ((UserControlMaskNumber)_gridView.Rows[de.NewEditIndex].FindControl("txtFrequencyEdit")).Focus();
    }

    protected void gvSurveyFreq_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidFilter(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtFrequencyEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }

            //UpdateSurveyFrequency(
            //        ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSurveyTypeId")).Text
            //         , ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtFrequencyEdit")).Text
            //         , ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtWinPeriodBeforeEdit")).Text
            //         , ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtWinPeriodAfterEdit")).Text
            //         );
            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSurveyFreq_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            Label lblShortCode = (Label)e.Row.FindControl("lblShortCode");
           
            if (eb != null && lblShortCode != null)
                eb.Visible = (SessionUtil.CanAccess(this.ViewState, eb.CommandName) && !lblShortCode.Text.Trim().Equals("PRC"));

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvSurveyFreq.EditIndex = -1;
        gvSurveyFreq.SelectedIndex = -1;
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvSurveyFreq.SelectedIndex = -1;
        gvSurveyFreq.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
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

    //private void UpdateSurveyFrequency(string SurveyTypeId, string Frequency,string WinPeriodBefore,string WinPeriodAfter)
    //{
    //    PhoenixRegistersVesselSurveyFrequency.UpdateSurveyFrequency(
    //        PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //        , General.GetNullableInteger(SurveyTypeId)
    //       , General.GetNullableInteger(Frequency)
    //       , General.GetNullableInteger(WinPeriodBefore)
    //       , General.GetNullableInteger(WinPeriodAfter));
    //}


    private bool IsValidFilter(string Frequency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Frequency.Equals(""))
            ucError.ErrorMessage = "Frequency is required.";

        return (!ucError.IsError);
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

 
}
