using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Globalization;
using Telerik.Web.UI;


public partial class CommonBudgetCommittedCostBreakUp : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (!IsPostBack)
        {
            if (Request.QueryString["commitmentid"].ToString() != null)
            {
                ViewState["COMMITMENTID"] = Request.QueryString["commitmentid"].ToString();
            }

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

            gvCostBreakup.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvReversedCommit.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }

        toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Common/CommonBudgetCommittedCostBreakUp.aspx?vesselid=" + Request.QueryString["vesselid"].ToString(), "Export to Excel", "icon_xls.png", "EXCEL");
        toolbar.AddImageLink("javascript:CallPrint('gvCostBreakup')", "Print Grid", "icon_print.png", "PRINT");

        MenuCostBreakup.AccessRights = this.ViewState;
        MenuCostBreakup.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Common/CommonBudgetCommittedCostBreakUp.aspx?vesselid=" + Request.QueryString["vesselid"].ToString(), "Export to Excel", "icon_xls.png", "EXCEL");
        toolbar.AddImageLink("javascript:CallPrint('gvReversedCommit')", "Print Grid", "icon_print.png", "PRINT");

        MenuCostBreakupReverse.AccessRights = this.ViewState;
        MenuCostBreakupReverse.MenuList = toolbar.Show();

        //BindData();
        //SetPageNavigator();
        //BindDataReverseCommit();
        //SetPageNavigator1();
    }   

    protected void CostBreakup_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvCostBreakup.Rebind();
           // BindData();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDREFERENCENUMBER", "FLDNAME", "FLDDESCRIPTION", "FLDSUBACCOUNT", "FLDAMOUNTINUSD", "FLDDATEOFAPPROVALXL", "FLDPORT", "FLDETA", "FLDVOUCHERNUMBER" };
        string[] alCaptions = { "PO Number", "Vendor", "Description", "Budget Code", "Amount (USD)", "Order Date", "Port", "ETA", "Voucher Number" };

        DataSet ds = PhoenixCommonBudgetGroupAllocation.BudgetCommittedCostBreakupSearchForAllocation(
            General.GetNullableGuid(ViewState["COMMITMENTID"] != null ? ViewState["COMMITMENTID"].ToString() : "")
            , 1 // commit or paid -- new commit
            , General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : "")
            , General.GetNullableInteger(ViewState["ACCOUNTID"] != null ? ViewState["ACCOUNTID"].ToString() : "")
            , General.GetNullableInteger(ViewState["MONTH"] != null ? ViewState["MONTH"].ToString() : "")
            , General.GetNullableInteger(ViewState["YEAR"] != null ? ViewState["YEAR"].ToString() : "")
            , General.GetNullableInteger(ViewState["BUDGETGROUPID"] != null ? ViewState["BUDGETGROUPID"].ToString() : "")
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvCostBreakup.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvCostBreakup", "New Committed", alCaptions, alColumns, ds);

        gvCostBreakup.DataSource = ds;
        gvCostBreakup.VirtualItemCount = iRowCount;

        if (ds.Tables[1].Rows.Count > 0)
        {
            lblVesselHdr.Text = ds.Tables[1].Rows[0]["FLDVESSELNAME"].ToString();
            lblBudgetGroupHdr.Text = ds.Tables[1].Rows[0]["FLDBUDGETGROUPNAME"].ToString();
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

   
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDREFERENCENUMBER", "FLDNAME", "FLDDESCRIPTION", "FLDSUBACCOUNT", "FLDAMOUNTINUSD", "FLDDATEOFAPPROVALXL", "FLDPORT", "FLDETA", "FLDVOUCHERNUMBER" };
        string[] alCaptions = { "PO Number", "Vendor", "Description", "Budget Code", "Amount (USD)", "Order Date", "Port", "ETA", "Voucher Number" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonBudgetGroupAllocation.BudgetCommittedCostBreakupSearchForAllocation(
            General.GetNullableGuid(ViewState["COMMITMENTID"] != null ? ViewState["COMMITMENTID"].ToString() : "")
            , 1 // commit or paid -- new commit
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

        Response.AddHeader("Content-Disposition", "attachment; filename=CostBreakup.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Cost Breakup - New Committed</h3></td>");
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
    protected void gvCostBreakup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCostBreakup.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCostBreakup_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
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



    //////////////////////////////////////////////////   Reversed Committed GridView /////////////////////////////////////////

    protected void MenuCostBreakupReverse_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvReversedCommit.Rebind();
            //BindData();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcelReverse();
        }
    }

    private void BindDataReverseCommit()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION1"] == null) ? null : (ViewState["SORTEXPRESSION1"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION1"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION1"].ToString());

        string[] alColumns = { "FLDREFERENCENUMBER", "FLDNAME", "FLDDESCRIPTION", "FLDSUBACCOUNT", "FLDAMOUNTINUSD", "FLDDATEOFAPPROVALXL", "FLDPORT", "FLDETA", "FLDVOUCHERNUMBER" };
        string[] alCaptions = { "PO Number", "Vendor", "Description", "Budget Code", "Amount (USD)", "Order Date", "Port", "ETA", "Voucher Number" };

        DataSet ds = PhoenixCommonBudgetGroupAllocation.BudgetCommittedCostBreakupSearchForAllocation(
            General.GetNullableGuid(ViewState["COMMITMENTID"] != null ? ViewState["COMMITMENTID"].ToString() : "")
            , 2 // commit or paid -- new commit
            , General.GetNullableInteger(ViewState["VESSELID"] != null ? ViewState["VESSELID"].ToString() : "")
            , General.GetNullableInteger(ViewState["ACCOUNTID"] != null ? ViewState["ACCOUNTID"].ToString() : "")
            , General.GetNullableInteger(ViewState["MONTH"] != null ? ViewState["MONTH"].ToString() : "")
            , General.GetNullableInteger(ViewState["YEAR"] != null ? ViewState["YEAR"].ToString() : "")
            , General.GetNullableInteger(ViewState["BUDGETGROUPID"] != null ? ViewState["BUDGETGROUPID"].ToString() : "")
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER1"],
            gvReversedCommit.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvReversedCommit", "Reverse Committed", alCaptions, alColumns, ds);

            gvReversedCommit.DataSource = ds;
            gvReversedCommit.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT1"] = iRowCount;
        ViewState["TOTALPAGECOUNT1"] = iTotalPageCount;
    }

   
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void ShowExcelReverse()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDREFERENCENUMBER", "FLDNAME", "FLDDESCRIPTION", "FLDSUBACCOUNT", "FLDAMOUNTINUSD", "FLDDATEOFAPPROVALXL", "FLDPORT", "FLDETA", "FLDVOUCHERNUMBER" };
        string[] alCaptions = { "PO Number", "Vendor", "Description", "Budget Code", "Amount (USD)", "Order Date", "Port", "ETA", "Voucher Number" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonBudgetGroupAllocation.BudgetCommittedCostBreakupSearchForAllocation(
            General.GetNullableGuid(ViewState["COMMITMENTID"] != null ? ViewState["COMMITMENTID"].ToString() : "")
            , 2 // commit or paid -- new commit
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

        Response.AddHeader("Content-Disposition", "attachment; filename=CostBreakup.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Cost Breakup - Reverse Committed</h3></td>");
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
    protected void gvReversedCommit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER1"] = ViewState["PAGENUMBER1"] != null ? ViewState["PAGENUMBER1"] : gvReversedCommit.CurrentPageIndex + 1;
            BindDataReverseCommit();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvReversedCommit_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER1"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



}
