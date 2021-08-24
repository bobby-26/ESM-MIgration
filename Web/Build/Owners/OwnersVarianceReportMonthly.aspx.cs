using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class OwnersVarianceReportMonthly : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["PAGENUMBERAPPROVEDPO"] = 1;
            ViewState["PAGENUMBERCOMMITTEDPO"] = 1;
            ViewState["PAGENUMBERACTUALPO"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["BudgetGroupId"] = "";
            ViewState["TYPE"] = "MONTHLY";
            BindHeader();
        }

        toolbar.AddButton("Monthly", "MONTHLY");
        toolbar.AddButton("Year To Date", "YEAR");
        toolbar.AddButton("Take Over", "TAKEOVER");
        MenuBudgetTab.AccessRights = this.ViewState;
        MenuBudgetTab.MenuList = toolbar.Show();
        

        toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Owners/OwnersVarianceReportMonthly.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvBudgetGroupAllocation')", "Print Grid", "icon_print.png", "PRINT");
        MenuCommonBudgetGroupAllocation.AccessRights = this.ViewState;
        MenuCommonBudgetGroupAllocation.MenuList = toolbar.Show();
        MenuCommonBudgetGroupAllocation.SetTrigger(pnlCommonBudgetGroupAllocation);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        // Order Not Placed
        toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Owners/OwnersVarianceReportMonthly.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvBudgetPeriodAllocation')", "Print Grid", "icon_print.png", "PRINT");
        MenuApprovedPO.AccessRights = this.ViewState;
        MenuApprovedPO.MenuList = toolbar.Show();
        MenuApprovedPO.SetTrigger(pnlCommonBudgetGroupAllocation);
        // Committed PO
        toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Owners/OwnersVarianceReportMonthly.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvCommittedPO')", "Print Grid", "icon_print.png", "PRINT");
        MenuCommittedPO.AccessRights = this.ViewState;
        MenuCommittedPO.MenuList = toolbar.Show();
        MenuCommittedPO.SetTrigger(pnlCommonBudgetGroupAllocation);
        
        BindBudgetGroup(); 
        BindBudgetPeriod();
        SetPageNavigatorApprovedPO();
        BindBudgetCommittedPO();
        SetPageNavigatorCommittedPO();
    }

 
    protected void BudgetTab_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("MONTHLY"))
        {
            ViewState["TYPE"] = "MONTHLY";
            
        }
        else if (dce.CommandName.ToUpper().Equals("YEAR"))
        {
            ViewState["TYPE"] = "YEAR";

        }
        else if (dce.CommandName.ToUpper().Equals("TAKEOVER"))
        {

            ViewState["TYPE"] = "TAKEOVER";
        }
        BindBudgetGroup();
        BindBudgetPeriod();
        SetPageNavigatorApprovedPO();
        BindBudgetCommittedPO();
        SetPageNavigatorCommittedPO();
    }

    protected void ShowExcel()
    {
        
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDBUDGETGROUP", "FLDCOMMITTEDAMOUNT", "FLDBILLEDAMOUNT", "FLDBUDGETPERMONTH", "FLDVARIANCE","FLDVARIANCEPERCENTAGE" };
        string[] alCaptions = { "Budget Group", "Committed Amount", "Billed Amount", "Budget Amount", "Variance", "Variance %" };

        NameValueCollection nvc = Filter.CurrentVarianceReport;

        if (ViewState["TYPE"].ToString() == "MONTHLY")
        {
            ds = PhoenixOwnersVarianceReport.OwnerVarianceMonthlyAmount(
                 new Guid(nvc.Get("lblDebitNoteReferenceId").ToString())
                 ,int.Parse(nvc.Get("lblAccountId").ToString())
                 );
        }
        else if (ViewState["TYPE"].ToString() == "YEAR")
        {
            ds = PhoenixOwnersVarianceReport.OwnerVarianceYearToDateAmount(
                 new Guid(nvc.Get("lblDebitNoteReferenceId").ToString())
                 , int.Parse(nvc.Get("lblAccountId").ToString())
                 );
        }
        else
        {
            ds = PhoenixOwnersVarianceReport.OwnerVarianceTakeOverAmount(
                 new Guid(nvc.Get("lblDebitNoteReferenceId").ToString())
                 , int.Parse(nvc.Get("lblAccountId").ToString())
                 );
        }
        Response.AddHeader("Content-Disposition", "attachment; filename=VarianceReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Variance Report</h3></td>");
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
    protected void MenuApprovedPO_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcelApprovedPO();
        }
    }
    protected void MenuCommittedPO_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcelCommittedPO();
        }
    }
 
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {        
        BindBudgetGroup();        
    }

    private void BindHeader()
    {
        NameValueCollection nvc = Filter.CurrentVarianceReport;

        txtAccountCode.Text = nvc.Get("lblAccountCode").ToString();
        txtStatementRef.Text = nvc.Get("lblDebitNoteReference").ToString();
    }
    private void BindBudgetGroup()
    {
        string[] alColumns = { "FLDBUDGETGROUP", "FLDCOMMITTEDAMOUNT", "FLDBILLEDAMOUNT", "FLDBUDGETPERMONTH", "FLDVARIANCE", "FLDVARIANCEPERCENTAGE" };
        string[] alCaptions = { "Budget Group", "Committed Amount", "Billed Amount", "Budget Amount", "Variance", "Variance %" };

        DataSet ds = new DataSet();
        NameValueCollection nvc = Filter.CurrentVarianceReport;

        if (ViewState["TYPE"].ToString() == "MONTHLY")
        {
            ds = PhoenixOwnersVarianceReport.OwnerVarianceMonthlyAmount(
                 new Guid(nvc.Get("lblDebitNoteReferenceId").ToString())
                 , int.Parse(nvc.Get("lblAccountId").ToString())
                 );
        }
        else if (ViewState["TYPE"].ToString() == "YEAR")
        {
            ds = PhoenixOwnersVarianceReport.OwnerVarianceYearToDateAmount(
                 new Guid(nvc.Get("lblDebitNoteReferenceId").ToString())
                 , int.Parse(nvc.Get("lblAccountId").ToString())
                 );
        }
        else
        {
            ds = PhoenixOwnersVarianceReport.OwnerVarianceTakeOverAmount(
                 new Guid(nvc.Get("lblDebitNoteReferenceId").ToString())
                 , int.Parse(nvc.Get("lblAccountId").ToString())
                 );
        }

        General.SetPrintOptions("gvBudgetGroupAllocation", "Variance Report", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBudgetGroupAllocation.DataSource = ds;
            gvBudgetGroupAllocation.DataBind();

            if (General.GetNullableInteger(ViewState["BudgetGroupId"].ToString()) == null)
                ViewState["BudgetGroupId"] = ds.Tables[0].Rows[0]["FLDBUDGETGROUPID"].ToString();
            SetRowSelectionOfBudgetGroup();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvBudgetGroupAllocation);

            gvBudgetGroupAllocation.SelectedIndex = -1;
            gvBudgetGroupAllocation.EditIndex = -1;
        }
                
    }

    private void BindBudgetPeriod()
    {
        int iRowCountApprovedPO = 0;
        int iTotalPageCountApprovedPO = 0;
       
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDPONUMBER", "FLDVENDORNAME", "FLDAMOUNT", "FLDCOMMITTEDDATE", "FLDDATEOFREVERSAL", "FLDVOUCHERNUMBER", "FLDSTATEMENTREF" };
        string[] alCaptions = { "PO Number", "Supplier Name", "Amount(USD)", "Committed Date", "Reversed Date", "Voucher Number", "Statement Reference" };

        NameValueCollection nvc = Filter.CurrentVarianceReport;

        ds = PhoenixOwnersVarianceReport.OwnerVarianceMonthlyCommittedAmount(
            new Guid(nvc.Get("lblDebitNoteReferenceId").ToString())
                 ,int.Parse(nvc.Get("lblAccountId").ToString())
            , General.GetNullableInteger(ViewState["BudgetGroupId"].ToString())
            , (int)ViewState["PAGENUMBERAPPROVEDPO"]
            , General.ShowRecords(null)
            , ref iRowCountApprovedPO
            , ref iTotalPageCountApprovedPO);

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
        General.SetPrintOptions("gvBudgetPeriodAllocation", "Committed PO", alCaptions, alColumns, ds);
        
        ViewState["ROWCOUNTAPPROVEDPO"] = iRowCountApprovedPO;
        ViewState["TOTALPAGECOUNTAPPROVEDPO"] = iTotalPageCountApprovedPO;
    }

    private void BindBudgetCommittedPO()
    {
        int iRowCountCommittedPO = 0;
        int iTotalPageCountCommittedPO = 0;
   
        DataSet ds = new DataSet();
        NameValueCollection nvc = Filter.CurrentVarianceReport;

        string[] alColumns = { "FLDSTATEMENTREFERENCE", "FLDVOUCHERROW", "FLDAMOUNT", "FLDBUDGETCODE", "FLDBUDGETGROUP", "FLDPARTICULARS" };
        string[] alCaptions = { "Statement Reference", "Voucher Row", "Amount (USD)","Budget Code","Budget Group","Particulars" };

        if (ViewState["TYPE"].ToString() == "MONTHLY")
        {
            ds = PhoenixOwnersVarianceReport.OwnerVarianceMonthlyBilledAmount(
                  new Guid(nvc.Get("lblDebitNoteReferenceId").ToString())
                  , int.Parse(nvc.Get("lblAccountId").ToString())
                  , General.GetNullableInteger(ViewState["BudgetGroupId"].ToString())
                  , (int)ViewState["PAGENUMBERCOMMITTEDPO"]
                  , General.ShowRecords(null)
                  , ref iRowCountCommittedPO
                  , ref iTotalPageCountCommittedPO);
        }
        else if (ViewState["TYPE"].ToString() == "YEAR")
        {
            ds = PhoenixOwnersVarianceReport.OwnerVarianceYearToDateBilledAmount(
                  new Guid(nvc.Get("lblDebitNoteReferenceId").ToString())
                  , int.Parse(nvc.Get("lblAccountId").ToString())
                  , General.GetNullableInteger(ViewState["BudgetGroupId"].ToString())
                  , (int)ViewState["PAGENUMBERCOMMITTEDPO"]
                  , General.ShowRecords(null)
                  , ref iRowCountCommittedPO
                  , ref iTotalPageCountCommittedPO);
        }
        else
        {
            ds = PhoenixOwnersVarianceReport.OwnerVarianceTakeOverBilledAmount(
                  new Guid(nvc.Get("lblDebitNoteReferenceId").ToString())
                  , int.Parse(nvc.Get("lblAccountId").ToString())
                  , General.GetNullableInteger(ViewState["BudgetGroupId"].ToString())
                  , (int)ViewState["PAGENUMBERCOMMITTEDPO"]
                  , General.ShowRecords(null)
                  , ref iRowCountCommittedPO
                  , ref iTotalPageCountCommittedPO);
        }



      

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCommittedPO.DataSource = ds;
            gvCommittedPO.DataBind();


        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCommittedPO);
        }
        General.SetPrintOptions("gvCommittedPO", "Billed Vouchers", alCaptions, alColumns, ds);

        ViewState["ROWCOUNTCOMMITTEDPO"] = iRowCountCommittedPO;
        ViewState["TOTALPAGECOUNTCOMMITTEDPO"] = iTotalPageCountCommittedPO;
    }

    protected void gvBudgetPeriodAllocation_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            
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
                SetPageNavigatorApprovedPO();

                BindBudgetCommittedPO();
                SetPageNavigatorCommittedPO();

            }
            else
            {
                _gridView.EditIndex = -1;
                
            }
            BindBudgetGroup();
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
       
    }

    protected void gvBudgetGroupAllocation_RowEditing(object sender, GridViewEditEventArgs de)
    {
       
    }

    protected void gvBudgetPeriodAllocation_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }

    protected void gvBudgetPeriodAllocation_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
        }
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

    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    gvBudgetGroupAllocation.SelectedIndex = -1;
    //    gvBudgetGroupAllocation.EditIndex = -1;

    //    BindBudgetGroup();
    //}

    protected void cmdGo_ClickApprovedPO(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBERAPPROVEDPO"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNTAPPROVEDPO"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBERAPPROVEDPO"] = ViewState["TOTALPAGECOUNTAPPROVEDPO"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBERAPPROVEDPO"] = 1;

            if ((int)ViewState["PAGENUMBERAPPROVEDPO"] == 0)
                ViewState["PAGENUMBERAPPROVEDPO"] = 1;

            txtnopage.Text = ViewState["PAGENUMBERAPPROVEDPO"].ToString();
        }
        BindBudgetPeriod();
        SetPageNavigatorApprovedPO();
    }

    protected void PagerButtonClickApprovedPO(object sender, CommandEventArgs ce)
    {
        gvBudgetPeriodAllocation.SelectedIndex = -1;
        gvBudgetPeriodAllocation.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBERAPPROVEDPO"] = (int)ViewState["PAGENUMBERAPPROVEDPO"] - 1;
        else
            ViewState["PAGENUMBERAPPROVEDPO"] = (int)ViewState["PAGENUMBERAPPROVEDPO"] + 1;

        BindBudgetPeriod();
        SetPageNavigatorApprovedPO();
    }

    private void SetPageNavigatorApprovedPO()
    {
        cmdPrevious.Enabled = IsPreviousEnabledApprovedPO();
        cmdNext.Enabled = IsNextEnabledApprovedPO();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBERAPPROVEDPO"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNTAPPROVEDPO"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNTAPPROVEDPO"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledApprovedPO()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERAPPROVEDPO"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTAPPROVEDPO"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabledApprovedPO()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERAPPROVEDPO"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTAPPROVEDPO"];

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

    private void SetRowSelectionOfBudgetGroup()
    {
        gvBudgetGroupAllocation.SelectedIndex = -1;

            for (int i = 0; i < gvBudgetGroupAllocation.Rows.Count; i++)
            {
                if (gvBudgetGroupAllocation.DataKeys[i].Value.ToString().Equals(ViewState["BudgetGroupId"].ToString()))
                {
                    gvBudgetGroupAllocation.SelectedIndex = i;
                }
            }
        
        
        
    }
    protected void ShowExcelApprovedPO()
    {
        int iRowCountApprovedPO = 0;
        int iTotalPageCountApprovedPO = 0;
        
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDPONUMBER", "FLDVENDORNAME", "FLDAMOUNT", "FLDCOMMITTEDDATE", "FLDDATEOFREVERSAL", "FLDVOUCHERNUMBER", "FLDSTATEMENTREF" };
        string[] alCaptions = { "PO Number", "Supplier Name", "Amount(USD)", "Committed Date", "Reversed Date", "Voucher Number", "Statement Reference" };

        if (ViewState["ROWCOUNTAPPROVEDPO"] == null || Int32.Parse(ViewState["ROWCOUNTAPPROVEDPO"].ToString()) == 0)
            iRowCountApprovedPO = 10;
        else
            iRowCountApprovedPO = Int32.Parse(ViewState["ROWCOUNTAPPROVEDPO"].ToString());


        NameValueCollection nvc = Filter.CurrentVarianceReport;

        ds = PhoenixOwnersVarianceReport.OwnerVarianceMonthlyCommittedAmount(
            new Guid(nvc.Get("lblDebitNoteReferenceId").ToString())
                 , int.Parse(nvc.Get("lblAccountId").ToString())
            , General.GetNullableInteger(ViewState["BudgetGroupId"].ToString())
            , (int)ViewState["PAGENUMBERAPPROVEDPO"]
            , iRowCountApprovedPO
            , ref iRowCountApprovedPO
            , ref iTotalPageCountApprovedPO);

        Response.AddHeader("Content-Disposition", "attachment; filename=CommittedPO.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Committed PO</h3></td>");
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
    protected void ShowExcelCommittedPO()
    {
        int iRowCountCommittedPO = 0;
        int iTotalPageCountCommittedPO = 0;
        //int? sortdirection = null;
        //int? iFinancialYear = null;
        //string vesselid = "";

        DataSet ds = new DataSet();
        NameValueCollection nvc = Filter.CurrentVarianceReport;

        string[] alColumns = { "FLDSTATEMENTREFERENCE", "FLDVOUCHERROW", "FLDAMOUNT", "FLDBUDGETCODE", "FLDBUDGETGROUP", "FLDPARTICULARS" };
        string[] alCaptions = { "Statement Reference", "Voucher Row", "Amount (USD)", "Budget Code", "Budget Group", "Particulars" };

        if (ViewState["ROWCOUNTCOMMITTEDPO"] == null || Int32.Parse(ViewState["ROWCOUNTCOMMITTEDPO"].ToString()) == 0)
            iRowCountCommittedPO = 10;
        else
            iRowCountCommittedPO = Int32.Parse(ViewState["ROWCOUNTCOMMITTEDPO"].ToString());

        if (ViewState["TYPE"].ToString() == "MONTHLY")
        {
            ds = PhoenixOwnersVarianceReport.OwnerVarianceMonthlyBilledAmount(
                  new Guid(nvc.Get("lblDebitNoteReferenceId").ToString())
                  , int.Parse(nvc.Get("lblAccountId").ToString())
                  , General.GetNullableInteger(ViewState["BudgetGroupId"].ToString())
                  , (int)ViewState["PAGENUMBERCOMMITTEDPO"]
                  , iRowCountCommittedPO
                  , ref iRowCountCommittedPO
                  , ref iTotalPageCountCommittedPO);
        }
        else if (ViewState["TYPE"].ToString() == "YEAR")
        {
            ds = PhoenixOwnersVarianceReport.OwnerVarianceYearToDateBilledAmount(
                  new Guid(nvc.Get("lblDebitNoteReferenceId").ToString())
                  , int.Parse(nvc.Get("lblAccountId").ToString())
                  , General.GetNullableInteger(ViewState["BudgetGroupId"].ToString())
                  , (int)ViewState["PAGENUMBERCOMMITTEDPO"]
                  , iRowCountCommittedPO
                  , ref iRowCountCommittedPO
                  , ref iTotalPageCountCommittedPO);
        }
        else
        {
            ds = PhoenixOwnersVarianceReport.OwnerVarianceTakeOverBilledAmount(
                  new Guid(nvc.Get("lblDebitNoteReferenceId").ToString())
                  , int.Parse(nvc.Get("lblAccountId").ToString())
                  , General.GetNullableInteger(ViewState["BudgetGroupId"].ToString())
                  , (int)ViewState["PAGENUMBERCOMMITTEDPO"]
                  , iRowCountCommittedPO
                  , ref iRowCountCommittedPO
                  , ref iTotalPageCountCommittedPO);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=BilledVouchers.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Billed Vouchers</h3></td>");
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
    protected void gvCommittedPO_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

        }
    }
    protected void gvCommittedPO_RowCommand(object sender, GridViewCommandEventArgs e)
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
                
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCommittedPO_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }
    protected void gvCommittedPO_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }
    protected void cmdGo_ClickCommittedPO(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopageCommittedPO.Text, out result))
        {
            ViewState["PAGENUMBERCOMMITTEDPO"] = Int32.Parse(txtnopageCommittedPO.Text);

            if ((int)ViewState["TOTALPAGECOUNTCOMMITTEDPO"] < Int32.Parse(txtnopageCommittedPO.Text))
                ViewState["PAGENUMBERCOMMITTEDPO"] = ViewState["TOTALPAGECOUNTCOMMITTEDPO"];

            if (0 >= Int32.Parse(txtnopageCommittedPO.Text))
                ViewState["PAGENUMBERCOMMITTEDPO"] = 1;

            if ((int)ViewState["PAGENUMBERCOMMITTEDPO"] == 0)
                ViewState["PAGENUMBERCOMMITTEDPO"] = 1;

            txtnopageCommittedPO.Text = ViewState["PAGENUMBERCOMMITTEDPO"].ToString();
        }
        BindBudgetCommittedPO();
        SetPageNavigatorCommittedPO();
    }

    protected void PagerButtonClickCommittedPO(object sender, CommandEventArgs ce)
    {
        gvCommittedPO.SelectedIndex = -1;
        gvCommittedPO.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBERCOMMITTEDPO"] = (int)ViewState["PAGENUMBERCOMMITTEDPO"] - 1;
        else
            ViewState["PAGENUMBERCOMMITTEDPO"] = (int)ViewState["PAGENUMBERCOMMITTEDPO"] + 1;

        BindBudgetCommittedPO();
        SetPageNavigatorCommittedPO();
    }

    private void SetPageNavigatorCommittedPO()
    {
        cmdPreviousCommittedPO.Enabled = IsPreviousEnabledCommittedPO();
        cmdNextCommittedPO.Enabled = IsNextEnabledCommittedPO();
        lblPagenumberCommittedPO.Text = "Page " + ViewState["PAGENUMBERCOMMITTEDPO"].ToString();
        lblPagesCommittedPO.Text = " of " + ViewState["TOTALPAGECOUNTCOMMITTEDPO"].ToString() + " Pages. ";
        lblRecordsCommittedPO.Text = "(" + ViewState["ROWCOUNTCOMMITTEDPO"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledCommittedPO()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERCOMMITTEDPO"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTCOMMITTEDPO"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabledCommittedPO()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERCOMMITTEDPO"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTCOMMITTEDPO"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

}
