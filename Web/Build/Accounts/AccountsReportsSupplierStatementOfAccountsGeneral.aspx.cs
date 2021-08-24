using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsReportsSupplierStatementOfAccountsGeneral : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsReportsSupplierStatementOfAccountsGeneral.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSupplierLedger')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsReportsSupplierStatementOfAccountsGeneral.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsReportsSupplierStatementOfAccountsGeneral.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            Menuledger.AccessRights = this.ViewState;
            Menuledger.MenuList = toolbargrid.Show();
            txtSupplierId.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Others", "JOURNAL", ToolBarDirection.Right);
            toolbar.AddButton("Bank Payment", "BANK", ToolBarDirection.Right);
            toolbar.AddButton("Purchase Invoice", "PURCHASE", ToolBarDirection.Right);
            toolbar.AddButton("General", "GENERAL", ToolBarDirection.Right);
            MenuSubsidiaryLedger.AccessRights = this.ViewState;
            MenuSubsidiaryLedger.MenuList = toolbar.Show();
            MenuSubsidiaryLedger.SelectedMenuIndex = 3;

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;

                NameValueCollection nvc = Filter.SupplierStatementOfAccounts;
                if (nvc != null)
                {
                    txtSupplierId.Text = nvc.Get("txtSupplierId").ToString();
                    txtSupplierName.Text = nvc.Get("txtSupplierName").ToString();
                    txtSupplierCode.Text = nvc.Get("txtSupplierCode").ToString();
                    ucDate.Text = nvc.Get("ucDate");
                    if (General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString()) != null)
                        ddlCurrencyCode.SelectedCurrency = nvc.Get("ddlCurrencyCode").ToString();
                    txtVoucherNumber.Text = nvc.Get("txtVoucherNumber").ToString();
                }
            }
            btnPickSupplier.Attributes.Add("onclick", "return showPickList('spnPickListSupplier', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131', true); ");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvSupplierLedger.SelectedIndexes.Clear();
        gvSupplierLedger.EditIndexes.Clear();
        gvSupplierLedger.DataSource = null;
        gvSupplierLedger.Rebind();
    }

    protected void gvSupplierLedger_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSupplierLedger.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSubsidiaryLedger_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("PURCHASE"))
            {
                Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerPurchaseInvoice.aspx?sourcePage=2");
            }
            else if (CommandName.ToUpper().Equals("JOURNAL"))
            {
                Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerJournalVoucher.aspx?sourcePage=2");
            }
            else if (CommandName.ToUpper().Equals("BANK"))
            {
                Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerBankPayment.aspx?sourcePage=2");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Menuledger_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvSupplierLedger.EditIndexes.Clear();
                gvSupplierLedger.SelectedIndexes.Clear();
                ViewState["PAGENUMBER"] = 1;

                NameValueCollection criteria = new NameValueCollection();

                criteria.Clear();
                criteria.Add("txtSupplierId", txtSupplierId.Text.Trim());
                criteria.Add("txtSupplierCode", txtSupplierCode.Text.Trim());
                criteria.Add("txtSupplierName", txtSupplierName.Text.Trim());
                criteria.Add("ucDate", ucDate.Text);
                criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
                criteria.Add("txtVoucherNumber", txtVoucherNumber.Text);

                Filter.SupplierStatementOfAccounts = criteria;
                if (!IsValidData(txtSupplierCode.Text, ucDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtSupplierCode.Text = "";
                txtSupplierId.Text = "";
                txtSupplierName.Text = "";
                ucDate.Text = "";
                ddlCurrencyCode.SelectedCurrency = "";
                txtVoucherNumber.Text = "";
                lblstrAmountTotal.Text = "";
               // lblftrstrAmountTotal.Text = "";

                NameValueCollection criteria = new NameValueCollection();

                criteria.Clear();
                criteria.Add("txtSupplierId", txtSupplierId.Text.Trim());
                criteria.Add("txtSupplierCode", txtSupplierCode.Text.Trim());
                criteria.Add("txtSupplierName", txtSupplierName.Text.Trim());
                criteria.Add("ucDate", ucDate.Text);
                criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
                criteria.Add("txtVoucherNumber", txtVoucherNumber.Text);

                Filter.SupplierStatementOfAccounts = criteria;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVOUCHERDATE", "FLDVOUCHERNUMBER", "FLDREFERENCEDOCUMENTNO", "FLDDRPRIMEAMOUNT", "FLDCRPRIMEAMOUNT", "FLDPRIMEAMOUNT" };
            string[] alCaptions = { "Date", "Voucher Number", "Reference", "Debit", "Credit", "Balance" };

            DataSet ds = new DataSet();
            if (txtSupplierId.Text != string.Empty)
            {
                ds = PhoenixAccountsSupplierSubsidiaryLedger.SupplierStatementOfAccounts(int.Parse(txtSupplierId.Text),
                                                                                        Convert.ToDateTime(ucDate.Text),
                                                                                        General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency),
                                                                                        txtVoucherNumber.Text,
                                                                                        (int)ViewState["PAGENUMBER"],
                                                                                        General.ShowRecords(null),
                                                                                        ref iRowCount,
                                                                                        ref iTotalPageCount);
            }
            else
            {
                ds = PhoenixAccountsSupplierSubsidiaryLedger.SupplierStatementOfAccounts(null,
                                                                                        null,
                                                                                        null,
                                                                                        "",
                                                                                        (int)ViewState["PAGENUMBER"],
                                                                                        General.ShowRecords(null),
                                                                                        ref iRowCount,
                                                                                        ref iTotalPageCount);
            }

            General.SetPrintOptions("gvSupplierLedger", "Supplier Statement Of Accounts", alCaptions, alColumns, ds);
            gvSupplierLedger.DataSource = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblstrlessThirty.Text = String.Format("{0:n}", ds.Tables[0].Rows[0]["FLDLESSTHIRTYDAY"].ToString());
                lblstrlessSixty.Text = String.Format("{0:n}", ds.Tables[0].Rows[0]["FLDLESSSIXTHDAY"].ToString());
                lblstrlessNinety.Text = String.Format("{0:n}", ds.Tables[0].Rows[0]["FLDLESSNINETYDAY"].ToString());
                lblstrmoreNinety.Text = String.Format("{0:n}", ds.Tables[0].Rows[0]["FLDMORENINETYDAY"].ToString());
                lblstrUnallocated.Text = String.Format("{0:n}", ds.Tables[0].Rows[0]["FLDUNALLOCATED"].ToString());
                lblstrAmountTotal.Text = String.Format("{0:n}", ds.Tables[0].Rows[0]["FLDBALANCE"].ToString());

            }
            gvSupplierLedger.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidData(string supplierCode, string date)
    {
        DateTime resultdate;
        if (supplierCode.Trim().Equals(""))
            ucError.ErrorMessage = "Supplier code is required";

        //if (date.Trim().Equals(""))
        //    ucError.ErrorMessage = "As on date  is required";

        //if (date.Trim().Equals("null"))
        //    ucError.ErrorMessage = "As on date  is required";

        if (string.IsNullOrEmpty(date))
            ucError.ErrorMessage = "As on date  is required";

        if (DateTime.TryParse(date, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
            ucError.ErrorMessage = "Date should be earlier than current date";

        ucError.HeaderMessage = "Please provide the following required information";
        return (!ucError.IsError);
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVOUCHERDATE", "FLDVOUCHERNUMBER", "FLDREFERENCEDOCUMENTNO", "FLDDRPRIMEAMOUNT", "FLDCRPRIMEAMOUNT", "FLDPRIMEAMOUNT" };
        string[] alCaptions = { "Date", "Voucher Number", "Reference", "Debit", "Credit", "Balance" };


        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsSupplierSubsidiaryLedger.SupplierStatementOfAccounts(General.GetNullableInteger(txtSupplierId.Text),
                                                                                General.GetNullableDateTime(ucDate.Text),
                                                                                General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency),
                                                                                txtVoucherNumber.Text,
                                                                                (int)ViewState["PAGENUMBER"],
                                                                                PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SupplierSOA.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Supplier Statement Of Accounts</h3></td>");
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

    protected void gvSupplierLedger_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            int iRowno = e.Item.ItemIndex;
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(iRowno);
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
            return;
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblVoucherType = ((RadLabel)gvSupplierLedger.Items[rowindex].FindControl("lblVoucherType"));
            LinkButton lblVoucherNumber = ((LinkButton)gvSupplierLedger.Items[rowindex].FindControl("lblVoucherNumber"));

            if (lblVoucherType != null && lblVoucherNumber != null)
            {
                if (lblVoucherType.Text == "72")
                {
                    Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerPurchaseInvoice.aspx?sourcePage=2&SupplierId=" + txtSupplierId.Text +
                                            "&VoucherNumber=" + lblVoucherNumber.Text);
                }
                else if (lblVoucherType.Text == "76")
                {
                    Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerJournalVoucher.aspx?sourcePage=2&SupplierId=" + txtSupplierId.Text +
                                            "&VoucherNumber=" + lblVoucherNumber.Text);
                }
                else if (lblVoucherType.Text == "68")
                {
                    Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerBankPayment.aspx?sourcePage=2&SupplierId=" + txtSupplierId.Text +
                                            "&VoucherNumber=" + lblVoucherNumber.Text);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidData(txtSupplierCode.Text, ucDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=SUPPLIERSTATEMENTOFACCOUNTS&SupplierId=" + txtSupplierId.Text +
                        "&date=" + ucDate.Text + "&CurrencyId=" + ddlCurrencyCode.SelectedCurrency + "&VoucherNumber=" + txtVoucherNumber.Text, false);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
