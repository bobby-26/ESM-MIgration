using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsReportsSupplierSubsidairyLedgerBankPayment : PhoenixBasePage
{
    public decimal totalAmount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsReportsSupplierSubsidairyLedgerBankPayment.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSupplierLedger')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsReportsSupplierSubsidairyLedgerBankPayment.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            Menuledger.AccessRights = this.ViewState;
            Menuledger.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbargridUnacc = new PhoenixToolbar();
            toolbargridUnacc.AddFontAwesomeButton("../Accounts/AccountsReportsSupplierSubsidairyLedgerBankPayment.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargridUnacc.AddFontAwesomeButton("javascript:CallPrint('gvSupplierLedger')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuledgerUnaccount.AccessRights = this.ViewState;
            MenuledgerUnaccount.MenuList = toolbargridUnacc.Show();
            txtSupplierId.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBERUNALL"] = 1;
                ViewState["SOURCE"] = Request.QueryString["sourcePage"];
                gvSupplierLedger.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvUnaccounted.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["SupplierId"] = Request.QueryString["SupplierId"];
                ViewState["VoucherNumber"] = Request.QueryString["VoucherNumber"];

                txtSupplierId.Text = Request.QueryString["SupplierId"];
                txtVoucherNumber.Text = Request.QueryString["VoucherNumber"];
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (ViewState["SOURCE"].ToString() == "5")
            {
                toolbar.AddButton("Others", "JOURNAL", ToolBarDirection.Right);
                toolbar.AddButton("Bank Payment", "BANK", ToolBarDirection.Right);
                toolbar.AddButton("Purchase Invoice", "PURCHASE", ToolBarDirection.Right);
                toolbar.AddButton("General", "GENERAL", ToolBarDirection.Right);
                MenuSubsidiaryLedger.AccessRights = this.ViewState;
                MenuSubsidiaryLedger.MenuList = toolbar.Show();
                MenuSubsidiaryLedger.SelectedMenuIndex = 1;
            }
            else
            {
                toolbar.AddButton("Others", "JOURNAL", ToolBarDirection.Right);
                toolbar.AddButton("Bank Payment", "BANK", ToolBarDirection.Right);
                toolbar.AddButton("Purchase Invoice", "PURCHASE", ToolBarDirection.Right);
                toolbar.AddButton("General", "GENERAL", ToolBarDirection.Right);
                MenuSubsidiaryLedger.AccessRights = this.ViewState;
                MenuSubsidiaryLedger.MenuList = toolbar.Show();
                MenuSubsidiaryLedger.SelectedMenuIndex = 1;
            }

            btnPickSupplier.Attributes.Add("onclick", "return showPickList('spnPickListSupplier', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131', false); ");
            Rebind();
            Rebind2();
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

    protected void Rebind2()
    {
        gvUnaccounted.SelectedIndexes.Clear();
        gvUnaccounted.EditIndexes.Clear();
        gvUnaccounted.DataSource = null;
        gvUnaccounted.Rebind();
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

    protected void gvUnaccounted_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUnaccounted.CurrentPageIndex + 1;
            BindData2();
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
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                if (ViewState["SOURCE"].ToString() == "1")
                    Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerGeneral.aspx");
                if (ViewState["SOURCE"].ToString() == "2")
                    Response.Redirect("../Accounts/AccountsReportsSupplierStatementOfAccountsGeneral.aspx");
                if (ViewState["SOURCE"].ToString() == "3")
                    Response.Redirect("../Accounts/AccountsReportsDetailedVendorAging.aspx");
                if (ViewState["SOURCE"].ToString() == "5")
                    Response.Redirect("../Accounts/AccountsReportsUnallocatedVendorPayment.aspx");
            }
            else if (CommandName.ToUpper().Equals("JOURNAL"))
            {
                Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerJournalVoucher.aspx?sourcePage=" + ViewState["SOURCE"].ToString());
            }
            else if (CommandName.ToUpper().Equals("PURCHASE"))
            {
                Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerPurchaseInvoice.aspx?sourcePage=" + ViewState["SOURCE"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuledgerUnaccount_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelUnacc();
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
                gvUnaccounted.EditIndexes.Clear();
                gvUnaccounted.SelectedIndexes.Clear();
                totalAmount = 0;
                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBERUNALL"] = 1;
                if (!IsValidData(txtSupplierCode.Text, txtVoucherNumber.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
                BindData2();
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
    protected void gvSupplierLedger_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                RadLabel lblPrimeAmount = (RadLabel)e.Item.FindControl("lblPrimeAmount");
                if (lblPrimeAmount.Text != string.Empty)
                    totalAmount = totalAmount + Convert.ToDecimal(lblPrimeAmount.Text);
            }
            txtAmount.Text = totalAmount.ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvSupplierLedger_ItemCommand(object sender, GridCommandEventArgs e)
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

    protected void gvUnaccounted_ItemCommand(object sender, GridCommandEventArgs e)
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
    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROW", "FLDREFERENCEDOCUMENTNO", "FLDINVOICENUMBER", "FLDVESSELNAME", "FLDPURCHASEORDERNUMBER", "FLDCURRENCYCODE", "FLDACCOUNTNUMBER", "FLDINVOICEAMOUNT", "FLDSTATUS" };
            string[] alCaptions = { "S.No.", "Reference Number", "Phoenix Posted No.", "Vessel Name", "PO Number", "Currency", "Bank Entry Number", "Prime Amount", "Status" };

            DataSet ds = new DataSet();
            if (txtSupplierId.Text != string.Empty)
            {
                ds = PhoenixAccountsSupplierSubsidiaryLedger.BankPaymentSubsidiaryLedger(General.GetNullableInteger(txtSupplierId.Text),
                                                                                        txtVoucherNumber.Text,
                                                                                        (int)ViewState["PAGENUMBER"],
                                                                                        General.ShowRecords(null),
                                                                                        ref iRowCount,
                                                                                        ref iTotalPageCount);
            }
            else
            {
                ds = PhoenixAccountsSupplierSubsidiaryLedger.BankPaymentSubsidiaryLedger(null, txtVoucherNumber.Text,
                                                                                        (int)ViewState["PAGENUMBER"],
                                                                                        General.ShowRecords(null),
                                                                                        ref iRowCount,
                                                                                        ref iTotalPageCount);
            }

            General.SetPrintOptions("gvSupplierLedger", "Bank Payment", alCaptions, alColumns, ds);

            gvSupplierLedger.DataSource = ds;
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

    public void BindData2()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROW", "FLDINVOICESUPPLIERREFERENCE", "FLDINVOICENUMBER", "FLDVESSELNAME", "FLDCURRENCYCODE", "FLDINVOICEAMOUNT" };
            string[] alCaptions = { "S.No.", "Reference Number", "Phoenix Invoice No.", "Vessel Name", "Currency", "Amount" };

            DataSet ds = new DataSet();
            if (txtSupplierId.Text != string.Empty)
            {
                ds = PhoenixAccountsSupplierSubsidiaryLedger.BankPaymentSubsidiaryLedgerUnaccounted(General.GetNullableInteger(txtSupplierId.Text),
                                                                                        txtVoucherNumber.Text,
                                                                                        (int)ViewState["PAGENUMBERUNALL"],
                                                                                        General.ShowRecords(null),
                                                                                        ref iRowCount,
                                                                                        ref iTotalPageCount);
            }
            else
            {
                ds = PhoenixAccountsSupplierSubsidiaryLedger.BankPaymentSubsidiaryLedgerUnaccounted(null, txtVoucherNumber.Text,
                                                                                        (int)ViewState["PAGENUMBERUNALL"],
                                                                                        General.ShowRecords(null),
                                                                                        ref iRowCount,
                                                                                        ref iTotalPageCount);
            }

            General.SetPrintOptions("gvUnaccounted", "Unallocated list", alCaptions, alColumns, ds);
            gvUnaccounted.DataSource = ds;
            gvUnaccounted.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNTUNALL"] = iRowCount;
            ViewState["TOTALPAGECOUNTUNALL"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
        string[] alColumns = { "FLDROW", "FLDREFERENCEDOCUMENTNO", "FLDINVOICENUMBER", "FLDVESSELNAME", "FLDPURCHASEORDERNUMBER", "FLDCURRENCYCODE", "FLDACCOUNTNUMBER", "FLDINVOICEAMOUNT", "FLDSTATUS" };
        string[] alCaptions = { "S.No.", "Reference Number", "Phoenix Posted No.", "Vessel Name", "PO Number", "Currency", "Bank Entry Number", "Prime Amount", "Status" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsSupplierSubsidiaryLedger.BankPaymentSubsidiaryLedger(General.GetNullableInteger(txtSupplierId.Text),
                                                                                txtVoucherNumber.Text,
                                                                                (int)ViewState["PAGENUMBER"],
                                                                                PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=BankPayment.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Bank Payment</h3></td>");
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

    protected void ShowExcelUnacc()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROW", "FLDINVOICESUPPLIERREFERENCE", "FLDINVOICENUMBER", "FLDVESSELNAME", "FLDCURRENCYCODE", "FLDINVOICEAMOUNT" };
        string[] alCaptions = { "S.No.", "Reference Number", "Phoenix Invoice No.", "Vessel Name", "Currency", "Amount" };

        if (ViewState["ROWCOUNTUNALL"] == null || Int32.Parse(ViewState["ROWCOUNTUNALL"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTUNALL"].ToString());

        ds = PhoenixAccountsSupplierSubsidiaryLedger.BankPaymentSubsidiaryLedgerUnaccounted(General.GetNullableInteger(txtSupplierId.Text),
                                                                                txtVoucherNumber.Text,
                                                                                (int)ViewState["PAGENUMBERUNALL"],
                                                                                PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Unallocatedlist.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Unallocated list</h3></td>");
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

    public bool IsValidData(string supplierCode, string voucherNumber)
    {
        if (supplierCode.Trim().Equals(""))
            ucError.ErrorMessage = "Supplier code is required";

        if (voucherNumber.Trim().Equals(""))
            ucError.ErrorMessage = "Voucher number  is required";

        ucError.HeaderMessage = "Please provide the following required information";
        return (!ucError.IsError);
    }
}
