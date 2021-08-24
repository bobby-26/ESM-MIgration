using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System.Globalization;

public partial class PurchaseCommittedCostBreakUpRevoked : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);    
        if (!IsPostBack)
        {
            if (Request.QueryString["vesselid"] != null)
            {
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
            }

            if (Request.QueryString["accountid"] != null)
            {
                ViewState["ACCOUNTID"] = Request.QueryString["accountid"].ToString();
            }

            if (Request.QueryString["year"] != null)
            {
                ViewState["YEAR"] = Request.QueryString["year"].ToString();
            }

            if (Request.QueryString["month"] != null)
            {
                ViewState["MONTH"] = Request.QueryString["month"].ToString();
                txtPeriodName.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(ViewState["MONTH"].ToString()));
                lblMonthHdr.Text = txtPeriodName.Text;
            }

            if (Request.QueryString["budgetgroupid"] != null)
            {
                ViewState["BUDGETGROUPID"] = Request.QueryString["budgetgroupid"].ToString();
            }

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["PAGENUMBER1"] = 1;
            ViewState["SORTEXPRESSION1"] = null;
            ViewState["SORTDIRECTION1"] = null;
        }
        BindData();
        SetPageNavigator();
    }   

    //protected void CostBreakup_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    //    if (dce.CommandName.ToUpper().Equals("FIND"))
    //    {
    //        ViewState["PAGENUMBER"] = 1;
    //        BindData();
    //        SetPageNavigator();
    //    }
    //    if (dce.CommandName.ToUpper().Equals("EXCEL"))
    //    {
    //        //ShowExcel();
    //    }
    //}

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseCommittedCosts.BudgetCommittedCostBreakupSearch(
             2 // commit or paid -- Revoked
            , General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : "")
            , General.GetNullableInteger(ViewState["ACCOUNTID"] != null ? ViewState["ACCOUNTID"].ToString() : "")
            , General.GetNullableInteger(ViewState["MONTH"] != null ? ViewState["MONTH"].ToString() : "")
            , General.GetNullableInteger(ViewState["YEAR"] != null ? ViewState["YEAR"].ToString() : "")
            , General.GetNullableInteger(ViewState["BUDGETGROUPID"] != null ? ViewState["BUDGETGROUPID"].ToString() : "")
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        //General.SetPrintOptions("gvVesselList", "Registers", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCostBreakup.DataSource = ds;
            gvCostBreakup.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCostBreakup);
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            lblVesselHdr.Text = ds.Tables[1].Rows[0]["FLDVESSELNAME"].ToString();
            lblBudgetGroupHdr.Text = ds.Tables[1].Rows[0]["FLDBUDGETGROUPNAME"].ToString();
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvCostBreakup.SelectedIndex = -1;
        gvCostBreakup.EditIndex = -1;
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

    //////////////////////////////////////////////////   Reversed Committed GridView /////////////////////////////////////////

    //protected void CostBreakup_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    //    if (dce.CommandName.ToUpper().Equals("FIND"))
    //    {
    //        ViewState["PAGENUMBER"] = 1;
    //        BindData();
    //        SetPageNavigator();
    //    }
    //    if (dce.CommandName.ToUpper().Equals("EXCEL"))
    //    {
    //        //ShowExcel();
    //    }
    //}

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
