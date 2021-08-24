using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;


public partial class AccountsInternalMonthlyBilling : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddImageButton("../Accounts/AccountsInternalMonthlyBilling.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvMonthlyBilling')", "Print Grid", "icon_print.png", "PRINT");
        toolbar.AddImageButton("../Accounts/AccountsInternalMonthlyBilling.aspx", "Find", "search.png", "FIND");
        toolbar.AddImageButton("../Accounts/AccountsInternalMonthlyBilling.aspx", "Find", "clear-filter.png", "CLEAR");

        MenuCommonBudgetGroupAllocation.AccessRights = this.ViewState;
        MenuCommonBudgetGroupAllocation.MenuList = toolbar.Show();
        //  MenuCommonBudgetGroupAllocation.SetTrigger(pnlCommonBudgetGroupAllocation);

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Vessels", "VESSEL");
        toolbar.AddButton("Line Items", "LINEITEM");
        //toolbar.AddButton("Invoices", "INVOICE");
        //toolbar.AddButton("As Incurred Billings", "INCURREDBILLINGS");

        MenuBudgetTab.AccessRights = this.ViewState;
        MenuBudgetTab.MenuList = toolbar.Show();
        MenuBudgetTab.SelectedMenuIndex = 0;

        if (!IsPostBack)
        {

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvMonthlyBilling.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            if (Request.QueryString["PORTAGEBILLID"] != null && Request.QueryString["PORTAGEBILLID"].ToString() != "")
                ViewState["PORTAGEBILLID"] = Request.QueryString["PORTAGEBILLID"].ToString();
            else
                ViewState["VESSELBUDGETALLOCATIONID"] = "";
        }

    }
    protected void Rebind()
    {
        gvMonthlyBilling.SelectedIndexes.Clear();
        gvMonthlyBilling.EditIndexes.Clear();
        gvMonthlyBilling.DataSource = null;
        gvMonthlyBilling.Rebind();
    }
    protected void gvMonthlyBilling_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMonthlyBilling.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BudgetTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("LINEITEM"))
        {
            Response.Redirect("../Accounts/AccountsInternalMonthlyBillingLineItem.aspx?PORTAGEBILLID=" + ViewState["PORTAGEBILLID"] + "&internalmonthlybillingid=" + ViewState["INTERNALMONTHLYBILLINGID"] + "&vesselID=" + ViewState["VesselId"].ToString(), false);
        }
        //if (CommandName.ToUpper().Equals("INVOICE"))
        //{
        //    Response.Redirect("../Accounts/AccountsInternalMonthlyBillingInvoice.aspx?PORTAGEBILLID=" + ViewState["PORTAGEBILLID"] + "&vesselId=" + ViewState["VesselId"] + "&startDate=" + ViewState["StartDate"] + "&endDate=" + ViewState["EndDate"], false);
        //}
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDESCRIPTION", "FLDACCOUNTCODE", "FLDFROMDATE", "FLDTODATE", "FLDPORTAGEBILLSTATUS", "FLDCHARGINGSTATUS" };
        string[] alCaptions = { "Vessel", "Account Code", "From Date", "To Date", "Portage Bill Status", "Charging Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentInternalBillingSelection;

        DataSet ds = PhoenixAccountsInternalBilling.PortageBillSearch(
                                                                      nvc != null ? General.GetNullableInteger(nvc.Get("ucVessel")) : null
                                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ucMonth")) : null
                                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ucFinancialYear")) : null
                                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlChargingStatus")) : null
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , (int)ViewState["PAGENUMBER"]
                                                                    , gvMonthlyBilling.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);

        General.SetPrintOptions("gvMonthlyBilling", "Portage Bill List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["PORTAGEBILLID"] == null)
            {
                ViewState["INTERNALMONTHLYBILLINGID"] = ds.Tables[0].Rows[0]["FLDINTERNALMONTHLYBILLINGID"].ToString();
                ViewState["PORTAGEBILLID"] = ds.Tables[0].Rows[0]["FLDPORTAGEBILLID"].ToString();
                ViewState["VesselId"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["StartDate"] = ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString();
                ViewState["EndDate"] = ds.Tables[0].Rows[0]["FLDTODATE"].ToString();
            }
            SetRowSelection();
        }

        gvMonthlyBilling.DataSource = ds;
        gvMonthlyBilling.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression;
        int? sortdirection = null;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDESCRIPTION", "FLDACCOUNTCODE", "FLDFROMDATE", "FLDTODATE", "FLDPORTAGEBILLSTATUS", "FLDCHARGINGSTATUS" };
        string[] alCaptions = { "Vessel", "Account Code", "From Date", "To Date", "Portage Bill Status", "Charging Status" };

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentInternalBillingSelection;

        ds = PhoenixAccountsInternalBilling.PortageBillSearch(
                                                          nvc != null ? General.GetNullableInteger(nvc.Get("ucVessel")) : null
                                                        , nvc != null ? General.GetNullableInteger(nvc.Get("ucMonth")) : null
                                                        , nvc != null ? General.GetNullableInteger(nvc.Get("ucFinancialYear")) : null
                                                        , nvc != null ? General.GetNullableInteger(nvc.Get("ddlChargingStatus")) : null
                                                        , sortexpression
                                                        , sortdirection
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                        , ref iRowCount
                                                        , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=PortageBillList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Portage Bill List</h3></td>");
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
        if (CommandName.ToUpper().Equals("FIND"))
        {
            SetFilterSelection();
            Rebind();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ucFinancialYear.SelectedQuick = "";
            ucMonth.SelectedHard = "";
            ucVessel.SelectedVessel = "";
            SetFilterSelection();
            Rebind();
        }
    }

    protected void gvMonthlyBilling_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvMonthlyBilling_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int iRowno = e.Item.ItemIndex;

                //    int iRowno = int.Parse(e.CommandArgument.ToString());
                BindPageURL(iRowno);
                if (ViewState["PORTAGEBILLID"] != null && ViewState["PORTAGEBILLID"].ToString() != "" && General.GetNullableGuid(ViewState["INTERNALMONTHLYBILLINGID"].ToString()) == null)
                {
                    PhoenixAccountsInternalBilling.InternalMonthlyBillingInsert(
                                                                               PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , new Guid(ViewState["PORTAGEBILLID"].ToString()));
                }
                if (ViewState["PORTAGEBILLID"] != null && ViewState["PORTAGEBILLID"].ToString() != "")
                    Response.Redirect("../Accounts/AccountsInternalMonthlyBillingLineItem.aspx?PORTAGEBILLID=" + ViewState["PORTAGEBILLID"].ToString() + "&internalmonthlybillingid=" + ViewState["INTERNALMONTHLYBILLINGID"] + "&vesselID=" + ViewState["VesselId"].ToString(), false);
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMonthlyBilling_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvMonthlyBilling.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
    }

    protected void gvMonthlyBilling_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
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

    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvMonthlyBilling.Items[rowindex];
            RadLabel lblPortageBillId = ((RadLabel)gvMonthlyBilling.Items[rowindex].FindControl("lblPortageBillId"));
            RadLabel lblInternalMonthlyBillingId = ((RadLabel)gvMonthlyBilling.Items[rowindex].FindControl("lblInternalMonthlyBillingId"));
            RadLabel lblVesselId = ((RadLabel)gvMonthlyBilling.Items[rowindex].FindControl("lblVesselId"));

            if (lblPortageBillId != null)
                ViewState["PORTAGEBILLID"] = lblPortageBillId.Text;
            if (lblInternalMonthlyBillingId != null)
                ViewState["INTERNALMONTHLYBILLINGID"] = lblInternalMonthlyBillingId.Text;
            else
                ViewState["INTERNALMONTHLYBILLINGID"] = "";
            ViewState["VesselId"] = lblVesselId.Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    private void SetRowSelection()
    {
        gvMonthlyBilling.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvMonthlyBilling.Items)
        {
            if (item.GetDataKeyValue("FLDPORTAGEBILLID").ToString().Equals(ViewState["PORTAGEBILLID"].ToString()))
            {
                gvMonthlyBilling.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }
    private void SetFilterSelection()
    {
        NameValueCollection criteria = new NameValueCollection();

        criteria.Clear();
        criteria.Add("ucVessel", ucVessel.SelectedVessel);
        criteria.Add("ucFinancialYear", ucFinancialYear.SelectedQuick);
        criteria.Add("ucMonth", ucMonth.SelectedHard);
        criteria.Add("ddlChargingStatus", ddlChargingStatus.SelectedValue.ToString());

        Filter.CurrentInternalBillingSelection = criteria;
    }
    protected void ucFinancialYear_Changed(object sender, EventArgs e)
    {
        SetFilterSelection();
        gvMonthlyBilling.Rebind();
    }
    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        SetFilterSelection();
        gvMonthlyBilling.Rebind();
    }

}
