using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CommonBudgetOpeningBalances : PhoenixBasePage
{
    public decimal AccumulatedExpenseTotal = 0;
    public decimal AccumulatedBudgetTotal = 0;
    public decimal YtdExpensesTotal = 0;
    public decimal YtdBudgetTotal = 0;
    public string strAccumulatedExpensesTotal = string.Empty;
    public string strAccumulatedBudgetTotal = string.Empty;
    public string strYtdExpensesTotal = string.Empty;
    public string strYtdBudgetTotal = string.Empty;
    //public string VesselAccID = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {        
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Common/CommonBudgetOpeningBalances.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvFinancialYearSetup')", "Print Grid", "icon_print.png", "PRINT");
        //toolbar.AddImageLink("../Common/CommonBudgetOpeningBalancesGenerate.aspx", "Add", "add.png", "ADD");
        //toolbar.AddImageButton("../Common/CommonBudgetVesselFinancialYear.aspx", "Find", "search.png", "FIND");
        toolbar.AddImageButton("../Common/CommonBudgetOpeningBalances.aspx", "Add", "add.png", "ADD");
        MenuFinancialYearSetup.AccessRights = this.ViewState;
        MenuFinancialYearSetup.MenuList = toolbar.Show();
        //MenuFinancialYearSetup.SetTrigger(pnlFinancialYearSetup);

        PhoenixToolbar toolbarsave = new PhoenixToolbar();
        toolbarsave.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuBudgetTab.AccessRights = this.ViewState;
        MenuBudgetTab.MenuList = toolbarsave.Show();

        if (!IsPostBack)
        {
            ViewState["OWNERREPORTBALANCEID"] = "";
            ucVesselAccount.DataTextField = "FLDVESSELACCOUNTNAME";
            ucVesselAccount.DataValueField = "FLDVESSELACCOUNTID";
            ucVesselAccount.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(null, 1);
            ucVesselAccount.DataBind();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
         

            gvFinancialYearSetup.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
     
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;              
        int? sortdirection = null;
      

        DataSet ds = new DataSet();

      
        string[] alColumns = { "FLDACCOUNTDESCRIPTION", "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDACCUMULATEDGLBALANCES", "FLDACCUMULATEDCOMMITTEDCOST", "FLDACCUMULATEDTOTALEXPENSE", "FLDACCUMULATEDBUDGET", "FLDYTDGLBALANCE", "FLDYTDCOMMITTEDCOST", "FLDYTDTOTALEXPENSE", "FLDYTDBUDGET", "FLDSTARTACCUMULATEDCOMMITTEDCOST" };
        string[] alCaptions = { "Vessel Account Code/ Description", "Budget Code", "Budget Code Description", "Accumulated GL Balance", "Accumulated Committed Cost", "Accumulate Total Expense", "Accumulated Budget", "YTD GL Balance", "YTD Committed Cost", "YTD Total Expense", "YTD Budget", "FY Start Accumulated Committed Cost " };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonBudgetOpeningBalance.BudgetOpeningBalanceSearch(
            General.GetNullableGuid(ViewState["OWNERREPORTBALANCEID"].ToString())
            , (int)ViewState["PAGENUMBER"]
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount
            , ref AccumulatedExpenseTotal
            , ref AccumulatedBudgetTotal
            , ref YtdExpensesTotal
            , ref YtdBudgetTotal
            );
        strAccumulatedExpensesTotal = String.Format("{0:n}", AccumulatedExpenseTotal);
        strAccumulatedBudgetTotal = String.Format("{0:n}", AccumulatedBudgetTotal);
        strYtdExpensesTotal = String.Format("{0:n}", YtdExpensesTotal);
        strYtdBudgetTotal = String.Format("{0:n}", YtdBudgetTotal);
        DataTable dt1 = ds.Tables[0];
        foreach (DataRow row in dt1.Rows)
        {
            row["FLDACCUMULATEDGLBALANCES"] = string.Format(String.Format("{0:#####.00}", row["FLDACCUMULATEDGLBALANCES"].ToString()));
            row["FLDACCUMULATEDBUDGET"] = string.Format(String.Format("{0:#####.00}", row["FLDACCUMULATEDBUDGET"].ToString()));
            row["FLDYTDGLBALANCE"] = string.Format(String.Format("{0:#####.00}", row["FLDYTDGLBALANCE"].ToString()));
            row["FLDYTDBUDGET"] = string.Format(String.Format("{0:#####.00}", row["FLDYTDBUDGET"].ToString()));
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=BudgetOpeningBalance.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Budget Opening Balances</h3></td>");
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

    protected void BudgetTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {

            if (General.GetNullableDateTime(ucOwnerReportingStart.Text) == null)
            {
                ucError.HeaderMessage = "Please provide the following required information";
                ucError.ErrorMessage = "Owner Reporting Start Date is Required";
                ucError.Visible = true;
                return;
            }
            PhoenixCommonBudgetOpeningBalance.OwnerReportOpeningBalanceInsert(int.Parse(ucVesselAccount.SelectedValue)
                                                        , General.GetNullableDateTime(ucOwnerReportingStart.Text)
                                                        , chkCommittedCostsIncluded.Checked.Value ? 1 : 0
                                                        , General.GetNullableGuid(ViewState["OWNERREPORTBALANCEID"].ToString())
                                                        );
            OwnerReportOpeningBalEdit();
        }
    } 

    protected void FinancialYearSetup_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;                
                BindData();
              
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
             
                if (General.GetNullableInteger(ucVesselAccount.SelectedValue.ToString()) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Select the Vessel Account";
                    ucError.Visible = true;
                    return;
                }
                String scriptpopup = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Common/CommonBudgetOpeningBalancesGenerate.aspx?vesselid=" + ucVesselAccount.SelectedValue.ToString() + "&ownerreportbalance=" + ViewState["OWNERREPORTBALANCEID"].ToString()+"');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
        catch (Exception ex)
        {            
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDACCOUNTDESCRIPTION", "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDACCUMULATEDGLBALANCES", "FLDACCUMULATEDCOMMITTEDCOST", "FLDACCUMULATEDTOTALEXPENSE", "FLDACCUMULATEDBUDGET", "FLDYTDGLBALANCE", "FLDYTDCOMMITTEDCOST", "FLDYTDTOTALEXPENSE", "FLDYTDBUDGET", "FLDSTARTACCUMULATEDCOMMITTEDCOST" };
        string[] alCaptions = { "Vessel Account Code/ Description", "Budget Code", "Budget Code Description", "Accumulated GL Balance", "Accumulated Committed Cost", "Accumulate Total Expense", "Accumulated Budget", "YTD GL Balance", "YTD Committed Cost", "YTD Total Expense", "YTD Budget", "FY Start Accumulated Committed Cost " };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixCommonBudgetOpeningBalance.BudgetOpeningBalanceSearch(
            General.GetNullableGuid(ViewState["OWNERREPORTBALANCEID"].ToString())
            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            , gvFinancialYearSetup.PageSize
            , ref iRowCount
            , ref iTotalPageCount
            , ref AccumulatedExpenseTotal
            , ref AccumulatedBudgetTotal
            , ref YtdExpensesTotal
            , ref YtdBudgetTotal);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvFinancialYearSetup.DataSource = ds;
      

            DataTable dt1 = ds.Tables[0];
            foreach (DataRow row in dt1.Rows)
            {
                row["FLDACCUMULATEDGLBALANCES"] = string.Format(String.Format("{0:#####.00}", row["FLDACCUMULATEDGLBALANCES"].ToString()));
                row["FLDACCUMULATEDBUDGET"] = string.Format(String.Format("{0:#####.00}", row["FLDACCUMULATEDBUDGET"].ToString()));
                row["FLDYTDGLBALANCE"] = string.Format(String.Format("{0:#####.00}", row["FLDYTDGLBALANCE"].ToString()));
                row["FLDYTDBUDGET"] = string.Format(String.Format("{0:#####.00}", row["FLDYTDBUDGET"].ToString()));
            }
        }
        else
        {
            gvFinancialYearSetup.DataSource = ds;
        }

        gvFinancialYearSetup.VirtualItemCount = iRowCount;
        General.SetPrintOptions("gvFinancialYearSetup", "Budget Opening Balances", alCaptions, alColumns, ds);

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        strAccumulatedExpensesTotal = String.Format("{0:n}", AccumulatedExpenseTotal);
        strAccumulatedBudgetTotal = String.Format("{0:n}", AccumulatedBudgetTotal);
        strYtdExpensesTotal = String.Format("{0:n}", YtdExpensesTotal);
        strYtdBudgetTotal = String.Format("{0:n}", YtdBudgetTotal);
    }

 
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
     
    }
         

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    private void SetKeyDownScroll(object sender, GridItemEventArgs e)
    {
        try
        {

            int nextRow = 0;
            RadGrid _gridView = (RadGrid)sender;

            if (e.Item is GridDataItem)
            {
                int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

                String script = "var keyValue = SelectSibling(event); ";
                script += " if(keyValue == 38) {";  //Up Arrow
                nextRow = (e.Item.RowIndex == 0) ? nRows : e.Item.RowIndex - 1;

                script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
                script += "}";

                script += " if(keyValue == 40) {";  //Down Arrow
                nextRow = (e.Item.RowIndex == nRows) ? 0 : e.Item.RowIndex + 1;

                script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
                script += "}";
                script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
                e.Item.Attributes["onkeydown"] = script;

            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void ucVesselAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        OwnerReportOpeningBalEdit();
        gvFinancialYearSetup.Rebind();
     
    }
    private void OwnerReportOpeningBalEdit()
    {
        DataSet ds = PhoenixCommonBudgetOpeningBalance.OwnerReportOpeningBalanceEdit(
         General.GetNullableInteger(ucVesselAccount.SelectedValue)
         );

        if (ds.Tables[0].Rows.Count > 0)
        {
            ucFinancialYearStart.Text = ds.Tables[0].Rows[0]["FLDFINANCIALYEARSTARTDATE"].ToString();
            ucOwnerReportingStart.Text = ds.Tables[0].Rows[0]["FLDOWNERPHOENIXSTARTDATE"].ToString();
            if (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDCOMMITTEDCOSTSINCLUDEDYN"].ToString()) == 1)
                chkCommittedCostsIncluded.Checked = true;
            else
                chkCommittedCostsIncluded.Checked = false;

            ViewState["OWNERREPORTBALANCEID"] = ds.Tables[0].Rows[0]["FLDOWNERREPORTOPENINGBALANCEID"].ToString();
        }
        BindData();
       
    }
    private bool IsValidData()
    {
        if (General.GetNullableGuid(ViewState["OWNERREPORTBALANCEID"].ToString()) == null)
            ucError.ErrorMessage = "Please Save Vessel Account, Vessel Financial Year Start Date & Owner Reporting Phoenix Start Date before updating Budget Data.";

        return (!ucError.IsError);
    }


    protected void gvFinancialYearSetup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFinancialYearSetup.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFinancialYearSetup_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCommonBudgetOpeningBalance.BudgetOpeningBalanceInsert(
                                                             General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblBudgetId")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucAccumulatedGlBal")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucAccumulatedNotShownAmt")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucAccumulatedBudget")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucYTDGLBal")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucYTDNotShownAmount")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucYTDBudget")).Text)
                                                            , General.GetNullableGuid(((RadLabel)e.Item.FindControl("LblOpeningBalanceId")).Text)
                                                            , General.GetNullableGuid(ViewState["OWNERREPORTBALANCEID"].ToString())
                                                            , General.GetNullableInteger(ucVesselAccount.SelectedValue)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucAccumulatedCommittedCost")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucYTDCommittedCost")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucStartAccumulatedCommittedCost")).Text)
                                                           );
             
                BindData();
                gvFinancialYearSetup.Rebind();
                //SetPageNavigator();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFinancialYearSetup_ItemDataBound(object sender, GridItemEventArgs e)
    {
        ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        ImageButton add = (ImageButton)e.Item.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);


        if (e.Item is GridDataItem)
        {

            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            SetKeyDownScroll(sender, e);
        }
    }

    protected void gvFinancialYearSetup_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
