using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections;
using System.IO;
using System.Web;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTracker_DefectTrackerStrategyReport : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvDefectList.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvDefectList.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        foreach (GridViewRow r in gvDefectListPending.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvDefectListPending.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarbuglist = new PhoenixToolbar();
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerStrategyReport.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerStrategyReport.aspx", "Search", "search.png", "SEARCH");
            MenuDefectTracker.AccessRights = this.ViewState;
            MenuDefectTracker.MenuList = toolbarbuglist.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER_FIXED"] = 1;
                ViewState["SORTEXPRESSION_FIXED"] = null;
                ViewState["SORTDIRECTION_FIXED"] = null;

                ViewState["PAGENUMBER_PENDING"] = 1;
                ViewState["SORTEXPRESSION_PENDING"] = null;
                ViewState["SORTDIRECTION_PENDING"] = null;

                BindYear();
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                ddlYear.SelectedIndex = Int32.Parse(DateTime.Today.Year.ToString());
            }
            ViewState["STARTDATE"] = DateTime.Today.Day.ToString() + "/" + ddlMonth.SelectedValue.ToString() + "/" + ddlYear.SelectedValue.ToString();
            BindData();
            SetPageNavigator_fixed();
            SetPageNavigator_pending();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void StrategyReport_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER_FIXED"] = 1;
            ViewState["SORTEXPRESSION_FIXED"] = null;
            ViewState["SORTDIRECTION_FIXED"] = null;

            ViewState["PAGENUMBER_PENDING"] = 1;
            ViewState["SORTEXPRESSION_PENDING"] = null;
            ViewState["SORTDIRECTION_PENDING"] = null;
            BindData();
            SetPageNavigator_fixed();
            SetPageNavigator_pending();
        }

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
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

    protected void gvDefectList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbE = (LinkButton)e.Row.FindControl("lnkSubject");
            if (lbE != null)
            {
                Label lblbugdtkey = (Label)e.Row.FindControl("lblUniqueID");
                if (SessionUtil.CanAccess(this.ViewState, lbE.CommandName))
                    lbE.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugEdit.aspx?dtkey=" + lblbugdtkey.Text + "'); return false;");
            }
        }
    }

    protected void gvDefectList_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();

    }

    protected void gvDefectList_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDefectList, "Select$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDefectListPending_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbE = (LinkButton)e.Row.FindControl("lnkSubject");
            if (lbE != null)
            {
                Label lblbugdtkey = (Label)e.Row.FindControl("lblUniqueID");
                if (SessionUtil.CanAccess(this.ViewState, lbE.CommandName))
                    lbE.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugEdit.aspx?dtkey=" + lblbugdtkey.Text + "'); return false;");
            }
        }
    }

    protected void gvDefectListPending_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindData();
    }

    protected void gvDefectListPending_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDefectListPending, "Select$" + e.Row.RowIndex.ToString(), false);
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

        int iRowCount_fixed = 0;
        int iTotalPageCount_fixed = 0;

        if (ViewState["ROWCOUNT_FIXED"] == null || Int32.Parse(ViewState["ROWCOUNT_FIXED"].ToString()) == 0)
            iRowCount_fixed = 10;
        else
            iRowCount_fixed = Int32.Parse(ViewState["ROWCOUNT_FIXED"].ToString());

        int iRowCount_pending = 0;
        int iTotalPageCount_pending = 0;

        if (ViewState["ROWCOUNT_PENDING"] == null || Int32.Parse(ViewState["ROWCOUNT_PENDING"].ToString()) == 0)
            iRowCount_pending = 10;
        else
            iRowCount_pending = Int32.Parse(ViewState["ROWCOUNT_PENDING"].ToString());

        DataSet ds = PhoenixDefectTracker.StrategyReport(DateTime.Parse(ViewState["STARTDATE"].ToString()),
                                                        (int)ViewState["PAGENUMBER_FIXED"],
                                                        10,
                                                        ref iRowCount_fixed,
                                                        ref iTotalPageCount_fixed,
                                                        (int)ViewState["PAGENUMBER_PENDING"],
                                                        10,
                                                        ref iRowCount_pending,
                                                        ref iTotalPageCount_pending
                                                        );
        DataTable dt1 = ds.Tables[0];
        DataTable dt2 = ds.Tables[1];

        if (dt1.Rows.Count > 0)
        {
            gvDefectList.DataSource = dt1;
            gvDefectList.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt1, gvDefectList);
        }

        if (dt2.Rows.Count > 0)
        {
            gvDefectListPending.DataSource = dt2;
            gvDefectListPending.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt2, gvDefectListPending);
        }
        ViewState["ROWCOUNT_FIXED"] = iRowCount_fixed;
        ViewState["TOTALPAGECOUNT_FIXED"] = iTotalPageCount_fixed;
        ViewState["ROWCOUNT_PENDING"] = iRowCount_pending;
        ViewState["TOTALPAGECOUNT_PENDING"] = iTotalPageCount_pending;
    }

    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
        {
            ListItem li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
            ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByValue(i.ToString()));
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }

    protected void ShowExcel()
    {
        DataSet ds = PhoenixDefectTracker.StrategyReport(DateTime.Parse(ViewState["STARTDATE"].ToString()));
        DataTable dt1 = ds.Tables[0];
        DataTable dt2 = ds.Tables[1];
        if ((dt1.Rows.Count > 0) && (dt2.Rows.Count > 0))
        {
            string strpath = HttpContext.Current.Request.MapPath("~/Attachments/DefectTracker/");
            string filename = strpath + "Strategy.xls";
            PhoenixDefectTracker2XL.Export2Excel(filename, DateTime.Parse(ViewState["STARTDATE"].ToString()));
        }

    }

    protected void cmdGo_Click_fixed(object sender, EventArgs e)
    {
        gvDefectList.EditIndex = -1;
        gvDefectList.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtnopage_fixed.Text, out result))
        {
            ViewState["PAGENUMBER_FIXED"] = Int32.Parse(txtnopage_fixed.Text);

            if ((int)ViewState["TOTALPAGECOUNT_FIXED"] < Int32.Parse(txtnopage_fixed.Text))
                ViewState["PAGENUMBER_FIXED"] = ViewState["TOTALPAGECOUNT_FIXED"];

            if (0 >= Int32.Parse(txtnopage_fixed.Text))
                ViewState["PAGENUMBER_FIXED"] = 1;

            if ((int)ViewState["PAGENUMBER_FIXED"] == 0)
                ViewState["PAGENUMBER_FIXED"] = 1;

            txtnopage_fixed.Text = ViewState["PAGENUMBER_FIXED"].ToString();
        }
        BindData();
        SetPageNavigator_fixed();
        SetPageNavigator_pending();
    }

    protected void PagerButtonClick_fixed(object sender, CommandEventArgs ce)
    {
        gvDefectList.SelectedIndex = -1;
        gvDefectList.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER_FIXED"] = (int)ViewState["PAGENUMBER_FIXED"] - 1;
        else
            ViewState["PAGENUMBER_FIXED"] = (int)ViewState["PAGENUMBER_FIXED"] + 1;

        BindData();
        SetPageNavigator_fixed();
        SetPageNavigator_pending();
    }

    private Boolean IsPreviousEnabled_fixed()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER_FIXED"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT_FIXED"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled_fixed()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER_FIXED"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT_FIXED"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    private void SetPageNavigator_fixed()
    {
        cmdPrevious_fixed.Enabled = IsPreviousEnabled_fixed();
        cmdNext_fixed.Enabled = IsNextEnabled_fixed();
        lblPagenumber_fixed.Text = "Page " + ViewState["PAGENUMBER_FIXED"].ToString();
        lblPages_fixed.Text = " of " + ViewState["TOTALPAGECOUNT_FIXED"].ToString() + " Pages. ";
        lblRecords_fixed.Text = "(" + ViewState["ROWCOUNT_FIXED"].ToString() + " records found)";
    }

    protected void cmdGo_Click_pending(object sender, EventArgs e)
    {
        gvDefectListPending.EditIndex = -1;
        gvDefectListPending.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtnopage_pending.Text, out result))
        {
            ViewState["PAGENUMBER_PENDING"] = Int32.Parse(txtnopage_pending.Text);

            if ((int)ViewState["TOTALPAGECOUNT_PENDING"] < Int32.Parse(txtnopage_pending.Text))
                ViewState["PAGENUMBER_PENDING"] = ViewState["TOTALPAGECOUNT_PENDING"];

            if (0 >= Int32.Parse(txtnopage_pending.Text))
                ViewState["PAGENUMBER_PENDING"] = 1;

            if ((int)ViewState["PAGENUMBER_PENDING"] == 0)
                ViewState["PAGENUMBER_PENDING"] = 1;

            txtnopage_pending.Text = ViewState["PAGENUMBER_PENDING"].ToString();
        }
        BindData();
        SetPageNavigator_pending();
    }

    protected void PagerButtonClick_pending(object sender, CommandEventArgs ce)
    {
        gvDefectListPending.SelectedIndex = -1;
        gvDefectListPending.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER_PENDING"] = (int)ViewState["PAGENUMBER_PENDING"] - 1;
        else
            ViewState["PAGENUMBER_PENDING"] = (int)ViewState["PAGENUMBER_PENDING"] + 1;

        BindData();
        SetPageNavigator_pending();
    }

    private Boolean IsPreviousEnabled_pending()
    {
        int iCurrentPageNumber_pending;
        int iTotalPageCount_pending;

        iCurrentPageNumber_pending = (int)ViewState["PAGENUMBER_PENDING"];
        iTotalPageCount_pending = (int)ViewState["TOTALPAGECOUNT_PENDING"];

        if (iTotalPageCount_pending == 0)
            return false;

        if (iCurrentPageNumber_pending > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled_pending()
    {
        int iCurrentPageNumber_pending;
        int iTotalPageCount_pending;

        iCurrentPageNumber_pending = (int)ViewState["PAGENUMBER_PENDING"];
        iTotalPageCount_pending = (int)ViewState["TOTALPAGECOUNT_PENDING"];

        if (iCurrentPageNumber_pending < iTotalPageCount_pending)
        {
            return true;
        }
        return false;
    }

    private void SetPageNavigator_pending()
    {
        cmdPrevious_pending.Enabled = IsPreviousEnabled_pending();
        cmdNext_pending.Enabled = IsNextEnabled_pending();
        lblPagenumber_pending.Text = "Page " + ViewState["PAGENUMBER_PENDING"].ToString();
        lblPages_pending.Text = " of " + ViewState["TOTALPAGECOUNT_PENDING"].ToString() + " Pages. ";
        lblRecords_pending.Text = "(" + ViewState["ROWCOUNT_PENDING"].ToString() + " records found)";
    }
}
