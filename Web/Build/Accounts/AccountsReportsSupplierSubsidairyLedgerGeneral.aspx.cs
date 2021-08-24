using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsReportsSupplierSubsidairyLedgerGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            Menuledger.AccessRights = this.ViewState;
            Menuledger.MenuList = toolbargrid.Show();
            txtSupplierId.Attributes.Add("style", "display:none;");
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbarMain = new PhoenixToolbar();

            toolbarMain.AddFontAwesomeButton("../Accounts/AccountsReportsSupplierSubsidairyLedgerGeneral.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarMain.AddFontAwesomeButton("javascript:CallPrint('gvSupplierLedger')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarMain.AddFontAwesomeButton("../Accounts/AccountsReportsSupplierSubsidairyLedgerGeneral.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarMain.AddFontAwesomeButton("../Accounts/AccountsReportsSupplierSubsidairyLedgerGeneral.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuledgerGrid.AccessRights = this.ViewState;
            MenuledgerGrid.MenuList = toolbarMain.Show();

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

                NameValueCollection nvc = Filter.SupplierSubsidairyLedger;
                if (nvc != null)
                {
                    txtSupplierId.Text = nvc.Get("txtSupplierId").ToString();
                    txtSupplierName.Text = nvc.Get("txtSupplierName").ToString();
                    txtSupplierCode.Text = nvc.Get("txtSupplierCode").ToString();
                    ucFromDate.Text = nvc.Get("ucFromDate").ToString();
                    ucToDate.Text = nvc.Get("ucToDate").ToString();
                    txtVoucherNumber.Text = nvc.Get("txtVoucherNumber").ToString();
                }
            }

            btnPickSupplier.Attributes.Add("onclick", "return showPickList('spnPickListSupplier', 'codehelp1', '', '../Common/CommonPickListAddress.aspx', true); ");
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
                Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerPurchaseInvoice.aspx?sourcePage=1");
            }
            if (CommandName.ToUpper().Equals("JOURNAL"))
            {
                Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerJournalVoucher.aspx?sourcePage=1");
            }
            else if (CommandName.ToUpper().Equals("BANK"))
            {
                Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerBankPayment.aspx?sourcePage=1");
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
                if (!IsValidData(txtSupplierCode.Text, ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=SUPPLIERSUBSIDIARYLEDGER&supplierCode=" + txtSupplierId.Text +
                        "&fromDate=" + ucFromDate.Text + "&toDate=" + ucToDate.Text + "&VoucherNumber=" + txtVoucherNumber.Text, false);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidData(string supplierCode, string fromdate, string todate)
    {
        DateTime resultdate;

        if (supplierCode.Trim().Equals(""))
            ucError.ErrorMessage = "Supplier code is required";

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
        else if (DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "To date should be earlier than current date";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To date should be later than from date";
        }

        ucError.HeaderMessage = "Please provide the following required information";
        return (!ucError.IsError);
    }

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDSTATUS", "FLDVOUCHERDATE", "FLDVOUCHERNUMBER", "FLDREFERENCEDOCUMENTNO", "FLDDRPRIMEAMOUNT", "FLDCRPRIMEAMOUNT", "FLDDRBASEAMOUNT", "FLDCRBASEAMOUNT" };
            string[] alCaptions = { "Status", "Date", "Transaction Number", "Document Number", "Prime Debit", "Prime Credit", "Base Debit", " 	Base Credit" };

            DataSet ds = PhoenixAccountsSupplierSubsidiaryLedger.GeneralSubsidiaryLedgerList(General.GetNullableInteger(txtSupplierId.Text),
                                                                                    General.GetNullableDateTime(ucFromDate.Text),
                                                                                    General.GetNullableDateTime(ucToDate.Text),
                                                                                    txtVoucherNumber.Text,
                                                                                    (int)ViewState["PAGENUMBER"],
                                                                                    General.ShowRecords(null),
                                                                                    ref iRowCount,
                                                                                    ref iTotalPageCount);

            General.SetPrintOptions("gvSupplierLedger", "Subsidary Ledger", alCaptions, alColumns, ds);

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

    protected void Rebind()
    {
        gvSupplierLedger.SelectedIndexes.Clear();
        gvSupplierLedger.EditIndexes.Clear();
        gvSupplierLedger.DataSource = null;
        gvSupplierLedger.Rebind();
    }

    protected void MenuledgerGrid_TabStripCommand(object sender, EventArgs e)
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
                criteria.Add("ucFromDate", ucFromDate.Text);
                criteria.Add("ucToDate", ucToDate.Text);
                criteria.Add("txtVoucherNumber", txtVoucherNumber.Text);

                Filter.SupplierSubsidairyLedger = criteria;

                if (!IsValidData(txtSupplierCode.Text, ucFromDate.Text, ucToDate.Text))
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
                ucFromDate.Text = "";
                ucToDate.Text = "";
                txtVoucherNumber.Text = "";

                NameValueCollection criteria = new NameValueCollection();

                criteria.Clear();
                criteria.Add("txtSupplierId", txtSupplierId.Text.Trim());
                criteria.Add("txtSupplierCode", txtSupplierCode.Text.Trim());
                criteria.Add("txtSupplierName", txtSupplierName.Text.Trim());
                criteria.Add("ucFromDate", ucFromDate.Text);
                criteria.Add("ucToDate", ucToDate.Text);
                criteria.Add("txtVoucherNumber", txtVoucherNumber.Text);

                Filter.SupplierSubsidairyLedger = criteria;

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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSTATUS", "FLDVOUCHERDATE", "FLDVOUCHERNUMBER", "FLDREFERENCEDOCUMENTNO", "FLDDRPRIMEAMOUNT", "FLDCRPRIMEAMOUNT", "FLDDRBASEAMOUNT", "FLDCRBASEAMOUNT" };
        string[] alCaptions = { "Status", "Date", "Transaction Number", "Document Number", "Prime Debit", "Prime Credit", "Base Debit", " 	Base Credit" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsSupplierSubsidiaryLedger.GeneralSubsidiaryLedgerList(General.GetNullableInteger(txtSupplierId.Text),
                                                                                General.GetNullableDateTime(ucFromDate.Text),
                                                                                General.GetNullableDateTime(ucToDate.Text),
                                                                                txtVoucherNumber.Text,
                                                                                (int)ViewState["PAGENUMBER"],
                                                                                PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SubsidaryLedger.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Subsidary Ledger</h3></td>");
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

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblVoucherType = ((RadLabel)gvSupplierLedger.Items[rowindex].FindControl("lblVoucherType"));
            LinkButton lblTransactionNumber = ((LinkButton)gvSupplierLedger.Items[rowindex].FindControl("lblTransactionNumber"));

            if (lblVoucherType != null && lblTransactionNumber != null)
            {
                if (lblVoucherType.Text == "72")
                {
                    Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerPurchaseInvoice.aspx?sourcePage=1&SupplierId=" + txtSupplierId.Text +
                                            "&VoucherNumber=" + lblTransactionNumber.Text);
                }
                else if (lblVoucherType.Text == "76")
                {
                    Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerJournalVoucher.aspx?sourcePage=1&SupplierId=" + txtSupplierId.Text +
                                            "&VoucherNumber=" + lblTransactionNumber.Text);
                }
                else if (lblVoucherType.Text == "68")
                {
                    Response.Redirect("../Accounts/AccountsReportsSupplierSubsidairyLedgerBankPayment.aspx?sourcePage=1&SupplierId=" + txtSupplierId.Text +
                                            "&VoucherNumber=" + lblTransactionNumber.Text);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSupplierLedger_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            int iRowno;
            iRowno = e.Item.ItemIndex;
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
}
