using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;

public partial class DashboardRankKPI : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvKpi.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvKpi.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["measureid"] != null)
                    ViewState["MEASUREID"] = Request.QueryString["measureid"].ToString();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidKpi(string fromc, string toc)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((toc == null) || (toc == ""))
            ucError.ErrorMessage = "Range From is required.";

        if ((fromc == null) || (fromc == ""))
            ucError.ErrorMessage = "Range To is required.";

        return (!ucError.IsError);
    }

    protected void gvKpi_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string rankid = ((UserControlRank)_gridView.FooterRow.FindControl("UcRankAdd")).SelectedRank;
                string fromc = (((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtFromCAdd")).Text);
                string toc = (((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtToCAdd")).Text);
                string color = ((DropDownList)_gridView.FooterRow.FindControl("ddlColorAdd")).SelectedValue;

                PhoenixDashboardOption.DashboardRankKPIInsert(new Guid(ViewState["MEASUREID"].ToString())
                    , General.GetNullableInteger(rankid), General.GetNullableInteger(fromc), General.GetNullableInteger(toc)
                    , color, null);

                BindData();

                String script = "javascript:fnReloadList('codehelp1', 'true', 'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string kpiid = (((Label)_gridView.Rows[nCurrentRow].FindControl("lblkpi")).Text);
                PhoenixDashboardOption.DashboardRankKPIDelete(new Guid(kpiid));

                String script = "javascript:fnReloadList('codehelp1', 'true', 'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvKpi_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlColorEdit = (DropDownList)e.Row.FindControl("ddlColorEdit");
                if (ddlColorEdit != null)
                {
                    ddlColorEdit.DataSource = PhoenixCommonDashboard.DashboardCssColorList();
                    ddlColorEdit.DataBind();
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList ddlColorAdd = (DropDownList)e.Row.FindControl("ddlColorAdd");
                if (ddlColorAdd != null)
                {
                    ddlColorAdd.DataSource = PhoenixCommonDashboard.DashboardCssColorList();
                    ddlColorAdd.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvKpi_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvKpi_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvKpi, "Edit$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvKpi_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvKpi_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            string kpiid = (((Label)_gridView.Rows[nCurrentRow].FindControl("lblkpiEdit")).Text);
            string rankid = ((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("UcRankEdit")).SelectedRank;
            string fromc = (((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtFromCEdit")).Text);
            string toc = (((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtToCEdit")).Text);
            string color = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlColorEdit")).SelectedValue;

            PhoenixDashboardOption.DashboardRankKPIInsert(new Guid(ViewState["MEASUREID"].ToString())
                , General.GetNullableInteger(rankid), General.GetNullableInteger(fromc), General.GetNullableInteger(toc)
                , color, General.GetNullableGuid(kpiid));

            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();

            String script = "javascript:fnReloadList('codehelp1', 'true', 'true');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvKpi_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvKpi_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvKpi.EditIndex = -1;
        gvKpi.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvKpi.SelectedIndex = -1;
        gvKpi.EditIndex = -1;
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
        gvKpi.SelectedIndex = -1;
        gvKpi.EditIndex = -1;
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
        {
            return true;
        }

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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvKpi.SelectedIndex = -1;
        gvKpi.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixDashboardOption.DashboardRankKPISearch(
            new Guid(ViewState["MEASUREID"].ToString()),
            sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvKpi.DataSource = ds;
            gvKpi.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvKpi);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        SetPageNavigator();
    }
}
