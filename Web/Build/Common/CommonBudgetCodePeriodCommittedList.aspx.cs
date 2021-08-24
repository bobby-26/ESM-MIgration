using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class CommonBudgetCodePeriodCommittedList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["VESSELBUDGETID"] = null;
            ViewState["BUDGETALLOCATIONID"] = null;
            ViewState["PERIODALLOCATIONID"] = null;

            if (Request.QueryString["vesselaccountid"] != null)
                ViewState["VESSELACCOUNTID"] = Request.QueryString["vesselaccountid"].ToString();
            if (Request.QueryString["periodallocationid"] != null)
                ViewState["PERIODALLOCATIONID"] = Request.QueryString["periodallocationid"];

            gvBudgetCodeAllocation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }

        toolbar = new PhoenixToolbar();

        toolbar.AddImageButton("../Common/CommonBudgetCodePeriodCommittedList.aspx?vesselaccountid=" + Request.QueryString["vesselaccountid"].ToString() + "&finyear=" + Request.QueryString["finyear"].ToString() + "&budgetgroupid=" + Request.QueryString["budgetgroupid"].ToString() + "&period=" + Request.QueryString["period"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvBudgetCodeAllocation')", "Print Grid", "icon_print.png", "PRINT");

        MenuCommonBudgetGroupAllocation.AccessRights = this.ViewState;
        MenuCommonBudgetGroupAllocation.MenuList = toolbar.Show();
        //MenuCommonBudgetGroupAllocation.SetTrigger(pnlCommonBudgetGroupAllocation);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

       // BindBudgetCode();
       // SetPageNavigator();
    }

    
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDBUDGETCODE", "FLDCOMMITTEDAMOUNT", "FLDPAIDAMOUNT", "FLDTOTALEXPENDITURE", "FLDBUDGETAMOUNT", "FLDALLOWANCE", "FLDMONTHLYVARIANCE", "FLDMANAGEMENTVARIANCE" };
        string[] alCaptions = { "Budget Code", "Committed", "Paid", "Total", "Budget Amount", "Allowance", "Variance", "Mngt Variance" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonBudgetCodeAllocation.BudgetCodePeriodCommittedSearch(
          Request.QueryString["vesselaccountid"] != null ? General.GetNullableInteger(Request.QueryString["vesselaccountid"].ToString()) : null
           , Request.QueryString["finyear"] != null ? General.GetNullableInteger(Request.QueryString["finyear"].ToString()) : null
           , Request.QueryString["budgetgroupid"] != null ? General.GetNullableInteger(Request.QueryString["budgetgroupid"].ToString()) : null
           , Request.QueryString["period"] != null ? General.GetNullableInteger(Request.QueryString["period"].ToString()) : null
           , (int)ViewState["PAGENUMBER"]
           , iRowCount
           , ref iRowCount
           , ref iTotalPageCount, new Guid(ViewState["PERIODALLOCATIONID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=BudgetCodeAllocation.xls");
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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        // BindBudgetCode();
        gvBudgetCodeAllocation.Rebind();
    }

    private void BindBudgetCode()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDBUDGETCODE", "FLDCOMMITTEDAMOUNT", "FLDPAIDAMOUNT", "FLDTOTALEXPENDITURE", "FLDBUDGETAMOUNT", "FLDALLOWANCE", "FLDMONTHLYVARIANCE", "FLDMANAGEMENTVARIANCE" };
        string[] alCaptions = { "Budget Code", "Committed", "Paid", "Total", "Budget Amount", "Allowance", "Variance", "Mngt Variance" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCommonBudgetCodeAllocation.BudgetCodePeriodCommittedSearch(
           Request.QueryString["vesselaccountid"] != null ? General.GetNullableInteger(Request.QueryString["vesselaccountid"].ToString()) : null
           , Request.QueryString["finyear"] != null ? General.GetNullableInteger(Request.QueryString["finyear"].ToString()) : null
           , Request.QueryString["budgetgroupid"] != null ? General.GetNullableInteger(Request.QueryString["budgetgroupid"].ToString()) : null
           , Request.QueryString["period"] != null ? General.GetNullableInteger(Request.QueryString["period"].ToString()) : null
           , (int)ViewState["PAGENUMBER"]
           , gvBudgetCodeAllocation.PageSize
           , ref iRowCount
           , ref iTotalPageCount, new Guid(ViewState["PERIODALLOCATIONID"].ToString()));

        General.SetPrintOptions("gvBudgetCodeAllocation", "Budget Code Allocation", alCaptions, alColumns, ds);

            if (!IsPostBack)
            {
                txtPeriodName.Text = ds.Tables[0].Rows[0]["FLDPERIODNAME"].ToString();
                txtYear.Text = ds.Tables[0].Rows[0]["FLDYEAR"].ToString();
                txtBudgetGroup.Text = ds.Tables[0].Rows[0]["FLDBUDGETGROUP"].ToString();
            }
        gvBudgetCodeAllocation.DataSource = ds;
        gvBudgetCodeAllocation.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvBudgetCodeAllocation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBudgetCodeAllocation.CurrentPageIndex + 1;
            BindBudgetCode();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBudgetCodeAllocation_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvBudgetCodeAllocation_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {

        BindBudgetCode();
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
