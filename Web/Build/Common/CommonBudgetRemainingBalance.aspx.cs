using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class CommonBudgetRemainingBalance : PhoenixBasePage
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
            ViewState["SHOWDATAYN"] = "1";

            ucVesselAccount.DataTextField = "FLDVESSELACCOUNTNAME";
            ucVesselAccount.DataValueField = "FLDVESSELACCOUNTID";
            ucVesselAccount.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(null, 1);
            ucVesselAccount.DataBind();

            ucFinancialYear.SelectedText = DateTime.Now.Year.ToString();
            ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
        }

        toolbar.AddButton("Show", "SHOW");
        //toolbar.AddButton("Budget", "BUDGET");
        MenuBudgetTab.AccessRights = this.ViewState;
        MenuBudgetTab.MenuList = toolbar.Show();
        

        toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Common/CommonBudgetRemainingBalance.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvBudgetGroupAllocation')", "Print Grid", "icon_print.png", "PRINT");
        MenuCommonBudgetGroupAllocation.AccessRights = this.ViewState;
        MenuCommonBudgetGroupAllocation.MenuList = toolbar.Show();
        MenuCommonBudgetGroupAllocation.SetTrigger(pnlCommonBudgetGroupAllocation);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        // Order Not Placed
        toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Common/CommonBudgetRemainingBalance.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvBudgetPeriodAllocation')", "Print Grid", "icon_print.png", "PRINT");
        MenuApprovedPO.AccessRights = this.ViewState;
        MenuApprovedPO.MenuList = toolbar.Show();
        MenuApprovedPO.SetTrigger(pnlCommonBudgetGroupAllocation);
        // Committed PO
        toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Common/CommonBudgetRemainingBalance.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvCommittedPO')", "Print Grid", "icon_print.png", "PRINT");
        MenuCommittedPO.AccessRights = this.ViewState;
        MenuCommittedPO.MenuList = toolbar.Show();
        MenuCommittedPO.SetTrigger(pnlCommonBudgetGroupAllocation);
        //Actual PO
        toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Common/CommonBudgetRemainingBalance.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvActualPO')", "Print Grid", "icon_print.png", "PRINT");
        MenuActualPO.AccessRights = this.ViewState;
        MenuActualPO.MenuList = toolbar.Show();
        MenuActualPO.SetTrigger(pnlCommonBudgetGroupAllocation);

        BindBudgetGroup(General.GetNullableInteger(ViewState["SHOWDATAYN"].ToString())); // ONLY HEADERS WILL BIND
        BindBudgetPeriod();
        SetPageNavigatorApprovedPO();
        BindBudgetCommittedPO();
        SetPageNavigatorCommittedPO();
        BindBudgetActualPO();
        SetPageNavigatorActualPO();
                
    }

 
    protected void BudgetTab_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SHOW"))
        {
            if (!IsvalidData())
            {
                ViewState["SHOWDATAYN"] = "1";
                ucError.Visible = true;
                //return;
            }
            else
                ViewState["SHOWDATAYN"] = "";

            BindBudgetGroup(General.GetNullableInteger(ViewState["SHOWDATAYN"].ToString()));
            BindBudgetPeriod();
            SetPageNavigatorApprovedPO();
            BindBudgetCommittedPO();
            SetPageNavigatorCommittedPO();
            BindBudgetActualPO();
            SetPageNavigatorActualPO();
        }
        
    }

    protected void ShowExcel()
    {
        
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDBUDGETGROUP", "FLDYETTOORDERAMOUNT", "FLDCOMMITTEDAMOUNT", "FLDACTUALAMOUNT", "FLDBUDGETPERMONTH", "FLDREMAININGBUDGET" };
        string[] alCaptions = { "Budget Group", "Not Yet Ordered", "Committed", "Actual", "Budget Amount", "Budget Remaining" };
        
        ds = PhoenixCommonBudgetOpeningBalance.RemainingBudgetSearch(
             General.GetNullableInteger(ucVesselAccount.SelectedValue)
             , General.GetNullableInteger(ucFinancialYear.SelectedQuick)
             , General.GetNullableInteger(ddlMonth.SelectedValue)
             ,null
                    );

        Response.AddHeader("Content-Disposition", "attachment; filename=RemainingBudget.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Remaining Budget</h3></td>");
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
    protected void MenuActualPO_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcelActualPO();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {        
        BindBudgetGroup(null);        
    }

    private void BindBudgetGroup(int? showdatayn)
    {
        string[] alColumns = { "FLDBUDGETGROUP", "FLDYETTOORDERAMOUNT", "FLDCOMMITTEDAMOUNT", "FLDACTUALAMOUNT", "FLDBUDGETPERMONTH", "FLDREMAININGBUDGET" };
        string[] alCaptions = { "Budget Group", "Not Yet Ordered", "Committed", "Actual", "Budget Amount", "Budget Remaining" };

        DataSet ds = PhoenixCommonBudgetOpeningBalance.RemainingBudgetSearch(
             General.GetNullableInteger(ucVesselAccount.SelectedValue)
             , General.GetNullableInteger(ddlMonth.SelectedValue)
             , General.GetNullableInteger(ucFinancialYear.SelectedQuick)
             ,showdatayn
                    );

        General.SetPrintOptions("gvBudgetGroupAllocation", "Budget Group Allocation", alCaptions, alColumns, ds);

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
        //int? sortdirection = null;
        //int? iFinancialYear = null;
        //string vesselid = "";

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDFORMNO", "FLDAMOUNT", "FLDSUBACCOUNT" };
        string[] alCaptions = { "PO Number", "Approved Amount(USD)", "Budget Code" };

        ds = PhoenixCommonBudgetOpeningBalance.RemainingBudgetNotCommittedPOSearch(
            General.GetNullableInteger(ucVesselAccount.SelectedValue)
            , General.GetNullableInteger(ddlMonth.SelectedValue)
            , General.GetNullableInteger(ucFinancialYear.SelectedQuick)
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
        General.SetPrintOptions("gvBudgetPeriodAllocation", "Approved PO", alCaptions, alColumns, ds);
        
        ViewState["ROWCOUNTAPPROVEDPO"] = iRowCountApprovedPO;
        ViewState["TOTALPAGECOUNTAPPROVEDPO"] = iTotalPageCountApprovedPO;
    }

    private void BindBudgetCommittedPO()
    {
        int iRowCountCommittedPO = 0;
        int iTotalPageCountCommittedPO = 0;
        //int? sortdirection = null;
        //int? iFinancialYear = null;
        //string vesselid = "";

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDFORMNO", "FLDAMOUNT", "FLDSUBACCOUNT" };
        string[] alCaptions = { "PO Number", "Committed Amount(USD)", "Budget Code" };

        ds = PhoenixCommonBudgetOpeningBalance.RemainingBudgetCommittedPOSearch(
            General.GetNullableInteger(ucVesselAccount.SelectedValue)
            , General.GetNullableInteger(ddlMonth.SelectedValue)
            , General.GetNullableInteger(ucFinancialYear.SelectedQuick)
            , General.GetNullableInteger(ViewState["BudgetGroupId"].ToString())
            , (int)ViewState["PAGENUMBERCOMMITTEDPO"]
            , General.ShowRecords(null)
            , ref iRowCountCommittedPO
            , ref iTotalPageCountCommittedPO);

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
        General.SetPrintOptions("gvCommittedPO", "Committed PO", alCaptions, alColumns, ds);

        ViewState["ROWCOUNTCOMMITTEDPO"] = iRowCountCommittedPO;
        ViewState["TOTALPAGECOUNTCOMMITTEDPO"] = iTotalPageCountCommittedPO;
    }

    private void BindBudgetActualPO()
    {
        int iRowCountActualPO = 0;
        int iTotalPageCountActualPO = 0;
        //int? sortdirection = null;
        //int? iFinancialYear = null;
        //string vesselid = "";

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVOUCHERNO", "FLDAMOUNT", "FLDSUBACCOUNT" };
        string[] alCaptions = { "Voucher Number", "Actual Amount(USD)", "Budget Code" };

        ds = PhoenixCommonBudgetOpeningBalance.RemainingBudgetActualPOSearch(
            General.GetNullableInteger(ucVesselAccount.SelectedValue)
            , General.GetNullableInteger(ddlMonth.SelectedValue)
            , General.GetNullableInteger(ucFinancialYear.SelectedQuick)
            , General.GetNullableInteger(ViewState["BudgetGroupId"].ToString())
            , (int)ViewState["PAGENUMBERACTUALPO"]
            , General.ShowRecords(null)
            , ref iRowCountActualPO
            , ref iTotalPageCountActualPO);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvActualPO.DataSource = ds;
            gvActualPO.DataBind();


        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvActualPO);
        }
        General.SetPrintOptions("gvActualPO", "Actual PO", alCaptions, alColumns, ds);

        ViewState["ROWCOUNTACTUALPO"] = iRowCountActualPO;
        ViewState["TOTALPAGECOUNTACTUALPO"] = iTotalPageCountActualPO;
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

                BindBudgetActualPO();
                SetPageNavigatorActualPO();
            }
            else
            {
                _gridView.EditIndex = -1;
                
            }
            BindBudgetGroup(null);
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
            ImageButton ibRemarks = (ImageButton)e.Row.FindControl("cmdRemarks");
            Label lblbudgetgroup = (Label)e.Row.FindControl("lblBudgetGroupId");
            if (ibRemarks != null && lblbudgetgroup!=null)
            {
                ibRemarks.Attributes.Add("onclick"
                    , "parent.Openpopup('codehelp1', '', 'CommonRemainingBudgetRemarks.aspx?vesselaccountid=" + ucVesselAccount.SelectedValue
                    + "&month=" + ddlMonth.SelectedValue + "&year=" + ucFinancialYear.SelectedQuick
                    + "&budgetgroupid=" + lblbudgetgroup.Text + "');return false;");
            }
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

        BindBudgetGroup(null);
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
        //int? sortdirection = null;
        //int? iFinancialYear = null;
        //string vesselid = "";

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDFORMNO", "FLDAMOUNT", "FLDSUBACCOUNT" };
        string[] alCaptions = { "PO Number", "Approved Amount(USD)", "Budget Code"};

        if (ViewState["ROWCOUNTAPPROVEDPO"] == null || Int32.Parse(ViewState["ROWCOUNTAPPROVEDPO"].ToString()) == 0)
            iRowCountApprovedPO = 10;
        else
            iRowCountApprovedPO = Int32.Parse(ViewState["ROWCOUNTAPPROVEDPO"].ToString());

        ds = PhoenixCommonBudgetOpeningBalance.RemainingBudgetNotCommittedPOSearch(
            General.GetNullableInteger(ucVesselAccount.SelectedValue)
            ,General.GetNullableInteger(ddlMonth.SelectedValue)
            ,General.GetNullableInteger(ucFinancialYear.SelectedQuick)
            , General.GetNullableInteger(ViewState["BudgetGroupId"].ToString())
            , (int)ViewState["PAGENUMBERAPPROVEDPO"]
            , iRowCountApprovedPO
            , ref iRowCountApprovedPO
            , ref iTotalPageCountApprovedPO);

        Response.AddHeader("Content-Disposition", "attachment; filename=ApprovedPO.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Approved PO</h3></td>");
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

        string[] alColumns = { "FLDFORMNO", "FLDAMOUNT", "FLDSUBACCOUNT" };
        string[] alCaptions = { "PO Number", "Committed Amount(USD)", "Budget Code" };

        if (ViewState["ROWCOUNTCOMMITTEDPO"] == null || Int32.Parse(ViewState["ROWCOUNTCOMMITTEDPO"].ToString()) == 0)
            iRowCountCommittedPO = 10;
        else
            iRowCountCommittedPO = Int32.Parse(ViewState["ROWCOUNTCOMMITTEDPO"].ToString());

        ds = PhoenixCommonBudgetOpeningBalance.RemainingBudgetCommittedPOSearch(
            General.GetNullableInteger(ucVesselAccount.SelectedValue)
            , General.GetNullableInteger(ddlMonth.SelectedValue)
            , General.GetNullableInteger(ucFinancialYear.SelectedQuick)
            , General.GetNullableInteger(ViewState["BudgetGroupId"].ToString())
            , (int)ViewState["PAGENUMBERCOMMITTEDPO"]
            , iRowCountCommittedPO
            , ref iRowCountCommittedPO
            , ref iTotalPageCountCommittedPO);

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
    protected void ShowExcelActualPO()
    {
        int iRowCountActualPO = 0;
        int iTotalPageCountActualPO = 0;
        //int? sortdirection = null;
        //int? iFinancialYear = null;
        //string vesselid = "";

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVOUCHERNO", "FLDAMOUNT", "FLDSUBACCOUNT" };
        string[] alCaptions = { "Voucher Number", "Actual Amount(USD)", "Budget Code" };

        if (ViewState["ROWCOUNTACTUALPO"] == null || Int32.Parse(ViewState["ROWCOUNTACTUALPO"].ToString()) == 0)
            iRowCountActualPO = 10;
        else
            iRowCountActualPO = Int32.Parse(ViewState["ROWCOUNTACTUALPO"].ToString());

        ds = PhoenixCommonBudgetOpeningBalance.RemainingBudgetActualPOSearch(
            General.GetNullableInteger(ucVesselAccount.SelectedValue)
            , General.GetNullableInteger(ddlMonth.SelectedValue)
            , General.GetNullableInteger(ucFinancialYear.SelectedQuick)
            , General.GetNullableInteger(ViewState["BudgetGroupId"].ToString())
            , (int)ViewState["PAGENUMBERACTUALPO"]
            , iRowCountActualPO
            , ref iRowCountActualPO
            , ref iTotalPageCountActualPO);

        Response.AddHeader("Content-Disposition", "attachment; filename=ActualPO.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Actual PO</h3></td>");
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

    protected void gvActualPO_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

        }
    }
    protected void gvActualPO_RowCommand(object sender, GridViewCommandEventArgs e)
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
    protected void gvActualPO_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }
    protected void gvActualPO_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

    protected void cmdGo_ClickActualPO(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopageActualPO.Text, out result))
        {
            ViewState["PAGENUMBERACTUALPO"] = Int32.Parse(txtnopageActualPO.Text);

            if ((int)ViewState["TOTALPAGECOUNTACTUALPO"] < Int32.Parse(txtnopageActualPO.Text))
                ViewState["PAGENUMBERACTUALPO"] = ViewState["TOTALPAGECOUNTACTUALPO"];

            if (0 >= Int32.Parse(txtnopageActualPO.Text))
                ViewState["PAGENUMBERACTUALPO"] = 1;

            if ((int)ViewState["PAGENUMBERACTUALPO"] == 0)
                ViewState["PAGENUMBERACTUALPO"] = 1;

            txtnopageActualPO.Text = ViewState["PAGENUMBERACTUALPO"].ToString();
        }
        BindBudgetActualPO();
        SetPageNavigatorActualPO();
    }

    protected void PagerButtonClickActualPO(object sender, CommandEventArgs ce)
    {
        gvActualPO.SelectedIndex = -1;
        gvActualPO.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBERACTUALPO"] = (int)ViewState["PAGENUMBERACTUALPO"] - 1;
        else
            ViewState["PAGENUMBERACTUALPO"] = (int)ViewState["PAGENUMBERACTUALPO"] + 1;

        BindBudgetActualPO();
        SetPageNavigatorActualPO();
    }

    private void SetPageNavigatorActualPO()
    {
        cmdPreviousActualPO.Enabled = IsPreviousEnabledActualPO();
        cmdNextActualPO.Enabled = IsNextEnabledActualPO();
        lblPagenumberActualPO.Text = "Page " + ViewState["PAGENUMBERACTUALPO"].ToString();
        lblPagesActualPO.Text = " of " + ViewState["TOTALPAGECOUNTACTUALPO"].ToString() + " Pages. ";
        lblRecordsActualPO.Text = "(" + ViewState["ROWCOUNTACTUALPO"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabledActualPO()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERACTUALPO"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTACTUALPO"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabledActualPO()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBERACTUALPO"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNTACTUALPO"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    private bool IsvalidData()
    {
        DateTime ownerreporting;
        int year;
        if (General.GetNullableInteger(ucVesselAccount.SelectedValue) == null)
            ucError.ErrorMessage = "Vessel Account is Required.";
        else
        {
            DataSet ds = PhoenixCommonBudgetOpeningBalance.OwnerReportOpeningBalanceEdit(
                                General.GetNullableInteger(ucVesselAccount.SelectedValue)
                                );
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableInteger(ucFinancialYear.SelectedQuick) != null && General.GetNullableInteger(ddlMonth.SelectedValue) != null)
                {
                    if (General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDOWNERPHOENIXSTARTDATE"].ToString()) == null)
                        ucError.ErrorMessage = "Year,Month that has been selected is on or later than Phoenix Owner Reporting Start Date entered under Opening Balances.";
                    else
                    {
                        ownerreporting = DateTime.Parse(ds.Tables[0].Rows[0]["FLDOWNERPHOENIXSTARTDATE"].ToString());
                        year = int.Parse(PhoenixCommonBudgetOpeningBalance.GetQuickShortName(int.Parse(ucFinancialYear.SelectedQuick)));
                        if (year <= ownerreporting.Year && int.Parse(ddlMonth.SelectedValue) < ownerreporting.Month)
                            ucError.ErrorMessage = "Year Month that has been selected is on or later than Phoenix Owner Reporting Start Date entered under Opening Balances.";
                    }
                }

            }
        }
        if (General.GetNullableInteger(ucFinancialYear.SelectedQuick) == null)
            ucError.ErrorMessage = "Year is required.";
        if (General.GetNullableInteger(ddlMonth.SelectedValue) == null)
            ucError.ErrorMessage = "Month is required.";
                return (!ucError.IsError);
    }
}
