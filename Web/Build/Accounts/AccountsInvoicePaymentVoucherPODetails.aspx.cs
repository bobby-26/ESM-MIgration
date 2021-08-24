using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsInvoicePaymentVoucherPODetails : PhoenixBasePage
{
    public string strReportCurrencyCode = string.Empty;
    public decimal dTotalCommittedAmount = 0;
    public decimal dTotalChargedAmount = 0;
    public decimal dTotalVesselAmount = 0;
    public decimal dTotalPayableAmount = 0;
    public decimal dTotalGSTClaimAmount = 0;
    public decimal dTotalIncomeExpenseAmount = 0;
    public decimal dTotalRebatereceivableAmount = 0;
    public decimal dTotalPayingAmount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            txtVendorId.Attributes.Add("style", "visibility:hidden");

            if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
                ViewState["voucherid"] = Request.QueryString["voucherid"];
            if (Request.QueryString["orderid"] != null && Request.QueryString["orderid"] != string.Empty)
                ViewState["orderid"] = Request.QueryString["orderid"];
            BindHeaderData();
            PhoenixToolbar toolbargridCreditNotes = new PhoenixToolbar();
            toolbargridCreditNotes.AddImageLink("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListCreditNote.aspx?mode=custom&CURRENCYCODE=" + ViewState["CurrencyCode"].ToString() + "&SUPPCODE=" + ViewState["SuppCode"].ToString() + "', true);", "Credit Notes List", "add.png", "ADDCREDITNOTE");
            MenuOrderFormMain.AccessRights = this.ViewState;
            // MenuOrderFormMain.SetTrigger(pnlStockItemEntry);
            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                Title1.Text = "Invoice Payment Voucher Details";
            }
            BindInvoicePOData();
            BindInvoicePOReceiptData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {

    }

    private void BindHeaderData()
    {
        if (ViewState["voucherid"] != null)
        {
            DataSet ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherEdit(ViewState["voucherid"].ToString());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtVoucherNumber.Text = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();
                    txtVendorId.Text = dr["FLDSUPPLIERCODE"].ToString();
                    txtVendorCode.Text = dr["FLDCODE"].ToString();
                    txtVendorName.Text = dr["FLDNAME"].ToString();
                    ddlCurrency.SelectedCurrency = dr["FLDCURRENCY"].ToString();
                    ViewState["CurrencyCode"] = dr["FLDCURRENCY"].ToString();
                    ViewState["SuppCode"] = dr["FLDSUPPLIERCODE"].ToString();
                }
            }
        }
    }

    private void BindInvoicePOData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDBUDGETID", "FLDCURRENCYNAME", "FLDTRANSACTIONAMOUNT", "FLDBASEAMOUNT", "FLDREPORTAMOUNT" };
        string[] alCaptions = { "Account Code", "Account Description", "Sub Account", "Transaction Currency", "Transaction Amount", "Base Amount", "Report Amount" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string invoiceid = (ViewState["INVOICEID"] == null) ? null : (ViewState["INVOICEID"].ToString());
        ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherPOSearch(ViewState["voucherid"].ToString(), ViewState["orderid"].ToString());

        gvInvoicePo.DataSource = ds;
        gvInvoicePo.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            strReportCurrencyCode = ds.Tables[0].Rows[0]["FLDREPORTCURRENCYCODE"].ToString();
            if (ViewState["ORDERID"] == null)
            {
                ViewState["ORDERID"] = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
                //   gvInvoicePo.SelectedIndex = 0;
                ViewState["ORDERNUMBER"] = ds.Tables[0].Rows[0]["FLDFORMNO"].ToString();
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
            //  ShowNoRecordsFound(dt, gvInvoicePo);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvInvoice", "Voucher Line Item", alCaptions, alColumns, ds);
    }


    private void BindInvoicePOReceiptData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDBUDGETID", "FLDCURRENCYNAME", "FLDTRANSACTIONAMOUNT", "FLDBASEAMOUNT", "FLDREPORTAMOUNT" };
        string[] alCaptions = { "Account Code", "Account Description", "Sub Account", "Transaction Currency", "Transaction Amount", "Base Amount", "Report Amount" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsInvoicePaymentVoucher.InvoicePaymentVoucherPOReceiptDetails(new Guid(ViewState["orderid"].ToString()),
                                                                                        (int)ViewState["PAGENUMBER"],
                                                                                        General.ShowRecords(null),
                                                                                        ref iRowCount,
                                                                                        ref iTotalPageCount
                                                                                        );


        gvPOReceipt.DataSource = ds;
        gvInvoicePo.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvInvoice", "Voucher Line Item", alCaptions, alColumns, ds);
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            ViewState["INVOICEID"] = null;
            ViewState["ORDERID"] = null;

            dTotalCommittedAmount = 0;
            dTotalChargedAmount = 0;
            dTotalVesselAmount = 0;
            dTotalPayableAmount = 0;
            dTotalGSTClaimAmount = 0;
            dTotalIncomeExpenseAmount = 0;
            dTotalRebatereceivableAmount = 0;
            dTotalPayingAmount = 0;
            BindInvoicePOData();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void gvInvoicePOTotal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            RadLabel lblCommittedAmt = (RadLabel)e.Row.FindControl("lblCommittedAmt");
            RadLabel lblChargedAmt = (RadLabel)e.Row.FindControl("lblChargedAmt");
            RadLabel lblVesselAmt = (RadLabel)e.Row.FindControl("lblVesselAmt");
            RadLabel lblPayableAmt = (RadLabel)e.Row.FindControl("lblPayableAmt");
            RadLabel lblGSTClaimAmt = (RadLabel)e.Row.FindControl("lblGSTClaimAmt");
            RadLabel lblIncomeAmt = (RadLabel)e.Row.FindControl("lblIncomeAmt");
            RadLabel lblRebatesAmt = (RadLabel)e.Row.FindControl("lblRebatesAmt");
            RadLabel lblPayingAmt = (RadLabel)e.Row.FindControl("lblPayingAmt");
            if (lblCommittedAmt != null && lblCommittedAmt.Text != "")
                dTotalCommittedAmount = dTotalCommittedAmount + decimal.Parse(lblCommittedAmt.Text);
            if (lblChargedAmt != null && lblChargedAmt.Text != "")
                dTotalChargedAmount = dTotalChargedAmount + decimal.Parse(lblChargedAmt.Text);
            if (lblVesselAmt != null && lblVesselAmt.Text != "")
                dTotalVesselAmount = dTotalVesselAmount + decimal.Parse(lblVesselAmt.Text);
            if (lblPayableAmt != null && lblPayableAmt.Text != "")
                dTotalPayableAmount = dTotalPayableAmount + decimal.Parse(lblPayableAmt.Text);
            if (lblGSTClaimAmt != null && lblGSTClaimAmt.Text != "")
                dTotalGSTClaimAmount = dTotalGSTClaimAmount + decimal.Parse(lblGSTClaimAmt.Text);
            if (lblIncomeAmt != null && lblIncomeAmt.Text != "")
                dTotalIncomeExpenseAmount = dTotalIncomeExpenseAmount + decimal.Parse(lblIncomeAmt.Text);
            if (lblRebatesAmt != null && lblRebatesAmt.Text != "")
                dTotalRebatereceivableAmount = dTotalRebatereceivableAmount + decimal.Parse(lblRebatesAmt.Text);
            if (lblPayingAmt != null && lblPayingAmt.Text != "")
                dTotalPayingAmount = dTotalPayingAmount + decimal.Parse(lblPayingAmt.Text);
        }
    }

    protected void gvInvoicePo_PreRender(object sender, EventArgs e)
    {
        GridDecorator.MergeRows(gvInvoicePo);
    }

    public class GridDecorator
    {
        public static void MergeRows(RadGrid gridView)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string strCurrentBudgetGroupCode = ((RadLabel)gridView.Items[rowIndex].FindControl("lblBudgetGroupId")).Text;
                string strPreviousBudgetGroupCode = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblBudgetGroupId")).Text;

                string strCurrentInvoicenumber = ((RadLabel)gridView.Items[rowIndex].FindControl("lblInvoiceNumber")).Text;
                string strPreviousInvoicenumber = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblInvoiceNumber")).Text;
                {

                    if (strCurrentInvoicenumber == strPreviousInvoicenumber)
                    {
                        row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                               previousRow.Cells[0].RowSpan + 1;
                        previousRow.Cells[0].Visible = false;

                        row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                               previousRow.Cells[1].RowSpan + 1;
                        previousRow.Cells[1].Visible = false;

                    }

                    if ((strCurrentBudgetGroupCode == strPreviousBudgetGroupCode) & (strCurrentInvoicenumber == strPreviousInvoicenumber))
                    {
                        row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                        previousRow.Cells[3].RowSpan + 1;
                        previousRow.Cells[3].Visible = false;

                        row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                        previousRow.Cells[4].RowSpan + 1;
                        previousRow.Cells[4].Visible = false;

                        row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                        previousRow.Cells[5].RowSpan + 1;
                        previousRow.Cells[5].Visible = false;

                        row.Cells[6].RowSpan = previousRow.Cells[6].RowSpan < 2 ? 2 :
                        previousRow.Cells[6].RowSpan + 1;
                        previousRow.Cells[6].Visible = false;

                        row.Cells[7].RowSpan = previousRow.Cells[7].RowSpan < 2 ? 2 :
                        previousRow.Cells[7].RowSpan + 1;
                        previousRow.Cells[7].Visible = false;

                        row.Cells[8].RowSpan = previousRow.Cells[8].RowSpan < 2 ? 2 :
                                               previousRow.Cells[8].RowSpan + 1;
                        previousRow.Cells[8].Visible = false;

                        row.Cells[9].RowSpan = previousRow.Cells[9].RowSpan < 2 ? 2 :
                                               previousRow.Cells[9].RowSpan + 1;
                        previousRow.Cells[9].Visible = false;

                    }
                }
            }
        }
    }

    protected void gvInvoicePo_NeedDataSource(object sender,  GridNeedDataSourceEventArgs e)
    {
        BindInvoicePOData();
    }

    protected void gvPOReceipt_NeedDataSource(object sender,  GridNeedDataSourceEventArgs e)
    {
        BindInvoicePOReceiptData();
    }
}
