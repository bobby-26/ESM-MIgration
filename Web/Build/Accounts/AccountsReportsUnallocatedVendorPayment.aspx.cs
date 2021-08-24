using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsReportsUnallocatedVendorPayment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarMain = new PhoenixToolbar();
            toolbarMain.AddFontAwesomeButton("../Accounts/AccountsReportsUnallocatedVendorPayment.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarMain.AddFontAwesomeButton("javascript:CallPrint('gvSupplierLedger')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarMain.AddFontAwesomeButton("../Accounts/AccountsReportsUnallocatedVendorPayment.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuledgerMain.AccessRights = this.ViewState;
            MenuledgerMain.MenuList = toolbarMain.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            Menuledger.AccessRights = this.ViewState;
            Menuledger.MenuList = toolbargrid.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            txtFromSupplierId.Attributes.Add("style", "display:none;");
            txtFromSupplierName.Attributes.Add("style", "display:none;");
            txtToSupplierId.Attributes.Add("style", "display:none;");
            txtToSupplierName.Attributes.Add("style", "display:none;");

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

                NameValueCollection nvc = Filter.UnallocatedVendorPayment;
                if (nvc != null)
                {
                    txtFromSupplierId.Text = nvc.Get("txtFromSupplierId").ToString();
                    txtFromSupplierName.Text = nvc.Get("txtFromSupplierName").ToString();
                    txtFromSupplier.Text = nvc.Get("txtFromSupplier").ToString();
                    txtToSupplierId.Text = nvc.Get("txtToSupplierId").ToString();
                    txtToSupplierName.Text = nvc.Get("txtToSupplierName").ToString();
                    txtToSupplier.Text = nvc.Get("txtToSupplier").ToString();
                    if (nvc.Get("ucFromDate") != null)
                        ucFromDate.Text = nvc.Get("ucFromDate").ToString();
                    if (nvc.Get("ucToDate") != null)
                        ucToDate.Text = nvc.Get("ucToDate").ToString();
                    txtVoucherNumber.Text = nvc.Get("txtVoucherNumber").ToString();
                }
                gvSupplierLedger.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            btnPickFromSupplier.Attributes.Add("onclick", "return showPickList('spnPickListFromSupplier', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131', false); ");
            btnPickToSupplier.Attributes.Add("onclick", "return showPickList('spnPickListToSupplier', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131', false); ");
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
                Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerPurchaseInvoice.aspx?sourcePage=5");
            }
            else if (CommandName.ToUpper().Equals("JOURNAL"))
            {
                Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerJournalVoucher.aspx?sourcePage=5");
            }
            else if (CommandName.ToUpper().Equals("BANK"))
            {
                Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerBankPayment.aspx?sourcePage=5");
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
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidData(ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=UNALLOCATEDVENDORPAYMENT&fromSupplier=" + txtFromSupplier.Text +
                        "&toSupplier=" + txtToSupplier.Text + "&fromDate=" + ucFromDate.Text + "&toDate=" + ucToDate.Text + "&VoucherNumber=" + txtVoucherNumber.Text, false);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidData(string fromdate, string todate)
    {
        DateTime resultdate;

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To date should be later than from date";
        }

        ucError.HeaderMessage = "Please provide the following required information";
        return (!ucError.IsError);
    }
    protected void MenuledgerMain_TabStripCommand(object sender, EventArgs e)
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
                criteria.Add("txtFromSupplierId", txtFromSupplierId.Text.Trim());
                criteria.Add("txtFromSupplier", txtFromSupplier.Text.Trim());
                criteria.Add("txtFromSupplierName", txtFromSupplierName.Text.Trim());
                criteria.Add("txtToSupplierId", txtToSupplierId.Text.Trim());
                criteria.Add("txtToSupplier", txtToSupplier.Text.Trim());
                criteria.Add("txtToSupplierName", txtToSupplierName.Text.Trim());
                criteria.Add("ucFromDate", ucFromDate.Text);
                criteria.Add("ucToDate", ucToDate.Text);
                criteria.Add("txtVoucherNumber", txtVoucherNumber.Text);

                Filter.UnallocatedVendorPayment = criteria;
                if (!IsValidData(ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFromSupplier.Text = "";
                txtFromSupplierId.Text = "";
                txtFromSupplierName.Text = "";
                txtToSupplier.Text = "";
                txtToSupplierId.Text = "";
                txtToSupplierName.Text = "";
                ucFromDate.Text = "";
                ucToDate.Text = "";
                txtVoucherNumber.Text = "";

                NameValueCollection criteria = new NameValueCollection();

                criteria.Clear();
                criteria.Add("txtFromSupplierId", txtFromSupplierId.Text.Trim());
                criteria.Add("txtFromSupplier", txtFromSupplier.Text.Trim());
                criteria.Add("txtFromSupplierName", txtFromSupplierName.Text.Trim());
                criteria.Add("txtToSupplierId", txtToSupplierId.Text.Trim());
                criteria.Add("txtToSupplier", txtToSupplier.Text.Trim());
                criteria.Add("txtToSupplierName", txtToSupplierName.Text.Trim());
                criteria.Add("ucFromDate", ucFromDate.Text);
                criteria.Add("ucToDate", ucToDate.Text);
                criteria.Add("txtVoucherNumber", txtVoucherNumber.Text);

                Filter.UnallocatedVendorPayment = criteria;
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

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVOUCHERDATE", "FLDVOUCHERNUMBER", "FLDREFERENCEDOCUMENTNO", "FLDDRPRIMEAMOUNT", "FLDCRPRIMEAMOUNT", "FLDPRIMEAMOUNT" };
            string[] alCaptions = { "Date", "Voucher Number", "Reference", "Debit", "Credit", "Balance" };

            DataSet ds = new DataSet();
            ds = PhoenixAccountsSupplierSubsidiaryLedger.UnallocatedVendorPaymentList(txtFromSupplier.Text, txtToSupplier.Text,
                                                                                        General.GetNullableDateTime(ucFromDate.Text),
                                                                                        General.GetNullableDateTime(ucToDate.Text),
                                                                                        txtVoucherNumber.Text,
                                                                                      int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                        gvSupplierLedger.PageSize,
                                                                                       ref iRowCount,
                                                                                       ref iTotalPageCount);


            General.SetPrintOptions("gvSupplierLedger", "Unallocated Vendor Payment", alCaptions, alColumns, ds);
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

        ds = PhoenixAccountsSupplierSubsidiaryLedger.UnallocatedVendorPaymentList(txtFromSupplier.Text, txtToSupplier.Text,
                                                                                    General.GetNullableDateTime(ucFromDate.Text),
                                                                                    General.GetNullableDateTime(ucToDate.Text),
                                                                                    txtVoucherNumber.Text,
                                                                                   (int)ViewState["PAGENUMBER"],
                                                                                   PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                                                   ref iRowCount,
                                                                                   ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=UnallocatedVendorPayment.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Unallocated Vendor Payment</h3></td>");
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
            RadLabel lblSupplierId = ((RadLabel)gvSupplierLedger.Items[rowindex].FindControl("lblSupplierId"));

            if (lblVoucherType != null && lblVoucherNumber != null)
            {
                if (lblVoucherType.Text == "68")
                {
                    Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerBankPayment.aspx?sourcePage=5&SupplierId=" + lblSupplierId.Text +
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
}
