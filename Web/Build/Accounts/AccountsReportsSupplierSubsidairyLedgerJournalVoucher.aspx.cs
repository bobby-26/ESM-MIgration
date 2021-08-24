using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsReportsSupplierSubsidairyLedgerJournalVoucher : PhoenixBasePage
{
    public decimal totalAmount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsReportsSupplierSubsidairyLedgerJournalVoucher.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSupplierLedger')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsReportsSupplierSubsidairyLedgerJournalVoucher.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            Menuledger.AccessRights = this.ViewState;
            Menuledger.MenuList = toolbargrid.Show();
            txtSupplierId.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Others", "JOURNAL", ToolBarDirection.Right);
            toolbar.AddButton("Bank Payment", "BANK", ToolBarDirection.Right);
            toolbar.AddButton("Purchase Invoice", "PURCHASE", ToolBarDirection.Right);
            toolbar.AddButton("General", "GENERAL", ToolBarDirection.Right);
            MenuSubsidiaryLedger.AccessRights = this.ViewState;
            MenuSubsidiaryLedger.MenuList = toolbar.Show();
            MenuSubsidiaryLedger.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["SOURCE"] = Request.QueryString["sourcePage"];

                ViewState["SupplierId"] = Request.QueryString["SupplierId"];
                ViewState["VoucherNumber"] = Request.QueryString["VoucherNumber"];

                txtSupplierId.Text = Request.QueryString["SupplierId"];
                txtVoucherNumber.Text = Request.QueryString["VoucherNumber"];
            }
            btnPickSupplier.Attributes.Add("onclick", "return showPickList('spnPickListSupplier', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131', false); ");
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
            else if (CommandName.ToUpper().Equals("PURCHASE"))
            {
                Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerPurchaseInvoice.aspx?sourcePage=" + ViewState["SOURCE"].ToString());
            }
            else if (CommandName.ToUpper().Equals("BANK"))
            {
                Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerBankPayment.aspx?sourcePage=" + ViewState["SOURCE"].ToString());
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
                totalAmount = 0;
                if (!IsValidData(txtSupplierCode.Text, txtVoucherNumber.Text))
                {
                    ucError.Visible = true;
                    return;
                }
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

    public void BindData()
    {
        try
        {
            string[] alColumns = { "FLDREFERENCEDOCUMENTNO", "FLDALLOCATEDVOUCHERNUMBER", "FLDLONGDESCRIPTION", "FLDCURRENCYCODE", "FLDAMOUNT" };
            string[] alCaptions = { "Reference Number", "MS Number", "Description", "Currency", "Amount" };

            DataSet ds = new DataSet();
            if (txtSupplierId.Text != string.Empty)
            {
                ds = PhoenixAccountsSupplierSubsidiaryLedger.JournalSubsidiaryLedger(General.GetNullableInteger(txtSupplierId.Text),
                                                                                                txtVoucherNumber.Text);
            }
            else
            {
                ds = PhoenixAccountsSupplierSubsidiaryLedger.JournalSubsidiaryLedger(null, txtVoucherNumber.Text);
            }

            General.SetPrintOptions("gvSupplierLedger", "Supplier Statement Of Accounts", alCaptions, alColumns, ds);
            gvSupplierLedger.DataSource = ds;

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
            BindData();
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

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDREFERENCEDOCUMENTNO", "FLDALLOCATEDVOUCHERNUMBER", "FLDLONGDESCRIPTION", "FLDCURRENCYCODE", "FLDAMOUNT" };
        string[] alCaptions = { "Reference Number", "MS Number", "Description", "Currency", "Amount" };


        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsSupplierSubsidiaryLedger.JournalSubsidiaryLedger(General.GetNullableInteger(txtSupplierId.Text),
                                                                                                txtVoucherNumber.Text);

        Response.AddHeader("Content-Disposition", "attachment; filename=JournalVoucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Journal Voucher</h3></td>");
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
