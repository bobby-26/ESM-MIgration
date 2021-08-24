using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Accounts_AccountsSoaCheckingBudgetSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Report", "REPORT", ToolBarDirection.Right);
            toolbar.AddButton("Budget", "BUDGET",ToolBarDirection.Right);
            
            MenuBudget.AccessRights = this.ViewState;
            MenuBudget.MenuList = toolbar.Show();

            MenuBudget.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["accountid"].ToString() != null)
                    ViewState["accountid"] = Request.QueryString["accountid"].ToString();
                else
                    ViewState["accountid"] = "";

                if (Request.QueryString["accountid"].ToString() != null)
                    ViewState["accountcode"] = Request.QueryString["accountcode"].ToString();
                else
                    ViewState["accountcode"] = "";

                if (Request.QueryString["debitnoteref"].ToString() != null)
                    ViewState["debitnoteref"] = Request.QueryString["debitnoteref"].ToString();
                else
                    ViewState["debitnoteref"] = "";

                if (Request.QueryString["ownerid"].ToString() != null)
                    ViewState["ownerid"] = Request.QueryString["ownerid"].ToString();
                else
                    ViewState["ownerid"] = "";

                if (Request.QueryString["voucherdate"] != null)
                    ViewState["voucherdate"] = Request.QueryString["voucherdate"].ToString();
                else
                    ViewState["voucherdate"] = "";

            }
            BindData();
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

        string[] alColumns = { "FLDSERIALNO", "FLDPARTNUMBER", "FLDNAME", "FLDUNITNAME", "FLDORDEREDQUANTITY" };
        string[] alCaptions = { "Sr.No", "Part Number", "Part Name", "Unit", "Ordered Quantity" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());


        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixAccountsSoaChecking.StatementOfAccountsBudgetSearch(General.GetNullableInteger(ViewState["accountid"].ToString()), ViewState["accountcode"].ToString(), ViewState["debitnoteref"].ToString(), int.Parse(ViewState["ownerid"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOwnersAccount.DataSource = ds;
            gvOwnersAccount.DataBind();

        }
        else
        {
            DataTable dt = ds.Tables[0];
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            gvOwnersAccount.SelectedIndex = -1;
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvOwnersAccount_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblAccountId = (Label)e.Row.FindControl("lblAccountId");
            Label lblDebitNoteReference = (Label)e.Row.FindControl("lblSoaReference");
            Label lblOwnerid = (Label)e.Row.FindControl("lblOwnerId");

            LinkButton lbr = (LinkButton)e.Row.FindControl("lnkAccountCOde");

            ImageButton cmdVoucher = (ImageButton)e.Row.FindControl("cmdVoucher");
            ImageButton cmdQueries = (ImageButton)e.Row.FindControl("cmdQueries");

            DataRowView drv = (DataRowView)e.Row.DataItem;

            if (drv["FLDVERIFIED"].ToString() == "1")
            {
                cmdVoucher.Visible = true;
            }

            if (drv["FLDQUERIES"].ToString() == "0")
            {
                cmdQueries.Visible = true;
            }
        }
    }

    protected void gvOwnersAccount_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            //Label lblBudgetCodeId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblBudgetCodeId");

            LinkButton lnkBudgetCOde = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkBudgetCOde");

            Filter.CurrentOwnerBudgetCodeSelection = lnkBudgetCOde.Text;
            Filter.CurrentSOARowSelectedFilter = "1"; 
            String scriptpopupclose = String.Format("javascript:fnReloadList('Filter', null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);

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

    protected void gvOwnersAccount_PreRender(object sender, EventArgs e)
    {
        GridDecorator.MergeRows(gvOwnersAccount);
    }

    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string strCurrentBudgetGroup = ((Label)gridView.Rows[rowIndex].FindControl("lblBudgetGroup")).Text;
                string strPreviousBudgetGroup = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblBudgetGroup")).Text;

                if (strCurrentBudgetGroup == strPreviousBudgetGroup)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                    previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;
                }
            }
        }
    }
    protected void MenuBudget_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;


        if (CommandName.ToUpper().Equals("REPORT"))
        {
            string prams = "";

            prams += "&accountid=" + ViewState["accountid"].ToString();
            prams += "&accountcode=" + ViewState["accountcode"].ToString();
            prams += "&debitnoteref=" + ViewState["debitnoteref"].ToString();
            prams += "&ownerid=" + ViewState["ownerid"].ToString();
            prams += "&voucherdate=" + ViewState["voucherdate"].ToString();          
            prams += exceloptions();

            Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=SOACHECKINGBUDGETCODE" + prams, false);
        }
    }
    protected string exceloptions()
    {
        DataSet ds = new DataSet();

        ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 181);

        string options = "";

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["FLDSHORTNAME"].ToString().Equals("EXL"))
                options += "&showexcel=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("WRD"))
                options += "&showword=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("PDF"))
                options += "&showpdf=" + dr["FLDHARDNAME"].ToString();
        }
        return options;
    }
}
