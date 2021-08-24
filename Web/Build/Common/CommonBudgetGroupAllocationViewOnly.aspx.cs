using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class CommonBudgetGroupAllocationViewOnly : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvBudgetGroupAllocation.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {

            }
        }
        if (!IsPostBack)
        {
            BindVesselAllocation();
            BindBudgetGroup();
            BindBudgetPeriod();
        }

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (!IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Back", "BACK");
            MenuBudget.MenuList = toolbarmain.Show();
            MenuBudget.SetTrigger(pnlCommonBudgetGroupAllocation);

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
        }

        toolbar.AddImageButton("../Common/CommonBudgetGroupAllocationViewOnly.aspx?vesselid=" + Request.QueryString["vesselid"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvBudgetGroupAllocation')", "Print Grid", "icon_print.png", "PRINT");

        MenuCommonBudgetGroupAllocation.MenuList = toolbar.Show();
        MenuCommonBudgetGroupAllocation.SetTrigger(pnlCommonBudgetGroupAllocation);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        BindVesselAllocation();
        BindBudgetGroup();
        BindBudgetPeriod();

        SetPageNavigator();
    }

    protected void MenuBudget_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Common/CommonPickListBudget.aspx?vesselid=" + Request.QueryString["vesselid"].ToString()
                    + "&budgetgroup=" + Request.QueryString["budgetgroup"].ToString() 
                    + "&hardtypecode=" + Request.QueryString["hardtypecode"].ToString()
                    + "&budgetdate=" + Request.QueryString["budgetdate"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Owner_Changed(object sender, EventArgs e)
    {        
        BindVesselAllocation();
        BindBudgetGroup();
        BindBudgetPeriod();
    }

    protected void FinancialYear_Changed(object sender, EventArgs e)
    {
        BindVesselAllocation();
        BindBudgetGroup();
        BindBudgetPeriod();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDBUDGETGROUPNAME", "FLDBUDGETAMOUNT", "FLDACCESSNAME" };
        string[] alCaptions = { "Budget Group", "Budget Amount", "Access" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonBudgetGroupAllocation.BudgetGroupAllocationSearch(
             General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null ? 0 : General.GetNullableInteger(ucFinancialYear.SelectedQuick)
             , General.GetNullableInteger(Filter.CurrentBudgetAllocationVesselFilter == null ? "0" : Filter.CurrentBudgetAllocationVesselFilter)
             , null
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=BudgetGroupAllocation.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Budget Group Allocation</h3></td>");
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

    protected void CommonBudgetGroupAllocation_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindBudgetGroup();
    }

    private void BindVesselAllocation()
    {
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string vesselid = Request.QueryString["vesselid"].ToString();
        string ownerid = "", currentfinyear = "";

        DataSet dsVessel = PhoenixRegistersVessel.EditVessel(int.Parse(vesselid));

        if (dsVessel.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsVessel.Tables[0].Rows[0];

            ownerid = dr["FLDOWNER"].ToString();
            currentfinyear = dr["FLDCURRENTFINYEARID"].ToString();
        }
        ucFinancialYear.Enabled = false;

        if (!IsPostBack)
        {
            ucFinancialYear.SelectedQuick = currentfinyear;
        }

        DataSet ds = PhoenixCommonBudgetGroupAllocation.BudgetVesselAllocationSearch(
              General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null ? 0 : General.GetNullableInteger(ucFinancialYear.SelectedQuick)
             , General.GetNullableInteger(vesselid), null
            );

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVesselAllocation.DataSource = ds;
            gvVesselAllocation.DataBind();

            if (!IsPostBack)
            {
                gvVesselAllocation.SelectedIndex = 0;
                Filter.CurrentBudgetAllocationVesselFilter = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVesselAllocation);

            Filter.CurrentBudgetAllocationVesselFilter = null;
            gvVesselAllocation.SelectedIndex = -1;
            gvVesselAllocation.EditIndex = -1;
        }
    }

    private void BindBudgetGroup()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDBUDGETGROUPNAME", "FLDBUDGETAMOUNT", "FLDACCESSNAME" };
        string[] alCaptions = { "Budget Group", "Budget Amount", "Access" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCommonBudgetGroupAllocation.BudgetGroupAllocationSearch(
            General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null ? 0 : General.GetNullableInteger(ucFinancialYear.SelectedQuick)
             , General.GetNullableInteger((Filter.CurrentBudgetAllocationVesselFilter == null || Filter.CurrentBudgetAllocationVesselFilter.Trim().Equals("")) ? "0" : Filter.CurrentBudgetAllocationVesselFilter)
             , null
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvBudgetGroupAllocation", "Budget Group Allocation", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBudgetGroupAllocation.DataSource = ds;
            gvBudgetGroupAllocation.DataBind();

            if (!IsPostBack)
            {
                gvBudgetGroupAllocation.SelectedIndex = 0;
                ViewState["BudgetGroupId"] = ds.Tables[0].Rows[0]["FLDBUDGETGROUPID"].ToString();
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvBudgetGroupAllocation);

            gvBudgetGroupAllocation.SelectedIndex = -1;
            gvBudgetGroupAllocation.EditIndex = -1;
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void BindBudgetPeriod()
    {
        DataSet ds = PhoenixCommonBudgetGroupAllocation.BudgetPeriodAllocationSearch(
            Int32.Parse(((ViewState["BudgetGroupId"] == null) || (ViewState["BudgetGroupId"].ToString().Equals(""))) ? "0" : ViewState["BudgetGroupId"].ToString())
             , General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null ? 0 : General.GetNullableInteger(ucFinancialYear.SelectedQuick)
             , General.GetNullableInteger(Filter.CurrentBudgetAllocationVesselFilter == null ? "0" : Filter.CurrentBudgetAllocationVesselFilter)
             , null);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBudgetPeriodAllocation.DataSource = ds;
            gvBudgetPeriodAllocation.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvBudgetPeriodAllocation);
        }
    }

    protected void gvBudgetPeriodAllocation_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;

            HeaderCell = new TableCell();
            HeaderCell.Text = "Monthly Totals";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 5;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Accumulated";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvBudgetPeriodAllocation.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }

    protected void gvVesselAllocation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindVesselAllocation();
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                Filter.CurrentBudgetAllocationVesselFilter = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text;

                _gridView.SelectedIndex = nCurrentRow;
                BindBudgetGroup();
                BindBudgetPeriod();
            }
            else
            {
                _gridView.EditIndex = -1;
                BindVesselAllocation();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselAllocation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindVesselAllocation();
    }

    protected void gvVesselAllocation_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //ImageButton db = (ImageButton)e.Row.FindControl("cmdHistory");

            //Label lblOwnerId = (Label)e.Row.FindControl("lblOwnerId");
            //Label lblFinancialYear = (Label)e.Row.FindControl("lblFinancialYearId");
            //Label lblVesselId = (Label)e.Row.FindControl("lblVesselId");

            //if (lblFinancialYear != null)
            //{
            //    db.Attributes.Add("onclick"
            //        , "parent.Openpopup('codehelp1', '', 'CommonBudgetAllocationHistory.aspx?VesselId=" + lblVesselId.Text + "&OwnerId=" + lblOwnerId.Text + "&FinYear=" + lblFinancialYear.Text + "');return false;");
            //}
        }
    }

    protected void gvVesselAllocation_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;

            BindVesselAllocation();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudgetGroupAllocation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                ViewState["BudgetGroupId"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBudgetGroupId")).Text;
                _gridView.SelectedIndex = nCurrentRow;
                BindBudgetPeriod();
            }
            else
            {
                _gridView.EditIndex = -1;
                BindBudgetGroup();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudgetPeriodAllocation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
            }
            else
            {
                _gridView.EditIndex = -1;
                BindBudgetGroup();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudgetGroupAllocation_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindBudgetGroup();
    }

    protected void gvBudgetGroupAllocation_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindBudgetGroup();
    }

    protected void gvBudgetPeriodAllocation_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }

    protected void gvBudgetGroupAllocation_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
        }
    }

    protected void gvBudgetGroupAllocation_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvBudgetGroupAllocation.SelectedIndex = -1;
        gvBudgetGroupAllocation.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindBudgetGroup();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvBudgetGroupAllocation.SelectedIndex = -1;
        gvBudgetGroupAllocation.EditIndex = -1;

        BindBudgetGroup();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
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
        BindBudgetGroup();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvBudgetGroupAllocation.SelectedIndex = -1;
        gvBudgetGroupAllocation.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindBudgetGroup();
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
