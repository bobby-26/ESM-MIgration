using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Accounts_AccountsAirfarePaymentVoucherGenerate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            txtBankID.Attributes.Add("style", "visibility:hidden;");
            txtSupplierId.Attributes.Add("style", "visibility:hidden");


            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddButton("Search", "VIEW");
            toolbar.AddButton("Generate Payment Voucher", "GENERATE",ToolBarDirection.Right);
            MenuPVGenerate.AccessRights = this.ViewState;
            MenuPVGenerate.MenuList = toolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsAirfarePaymentVoucherGenerate.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvPVGenerate')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsAirfarePaymentVoucherGenerate.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsAirfarePaymentVoucherGenerate.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuList.AccessRights = this.ViewState;
            MenuList.MenuList = toolbargrid.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucCurrency.SelectedCurrency = "4";
                BindFilterData();
            }

            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAccountsSupplier.aspx', true); ");
            if (ViewState["CURRENCY"] == null)
            {
                ViewState["CURRENCY"] = 4;
                imgBankPicklist.Attributes.Add("onclick", "return showPickList('spnPickListBank', 'codehelp1', '', '../Common/CommonPickListBankInformationAddress.aspx?addresscode=" + txtSupplierId.Text + "&currency=" + ViewState["CURRENCY"] + "', true);");
            }
            else
            {
                imgBankPicklist.Attributes.Add("onclick", "return showPickList('spnPickListBank', 'codehelp1', '', '../Common/CommonPickListBankInformationAddress.aspx?addresscode=" + txtSupplierId.Text + "&currency=" + ucCurrency.SelectedCurrency + "', true);");
            }
            if (Filter.Airfarepayment != null)
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidData(txtSupplierCode.Text,
                     txtToDate.Text, ucCurrency.SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }

                NameValueCollection nvc = new NameValueCollection();
                SetFilter();
                BindData();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearFilter();
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvPVGenerate.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPVGenerate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName; 

            if (CommandName.ToUpper().Equals("VIEW"))
            {
                if (!IsValidData(txtSupplierCode.Text,
                     txtToDate.Text, ucCurrency.SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
            }
            else if (CommandName.ToUpper().Equals("GENERATE"))
            {
                if (!IsValidData(txtSupplierCode.Text,
                     txtToDate.Text, ucCurrency.SelectedCurrency, txtBankID.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsAirfarePaymentVoucherGenerate.AirfarePVGenerate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                    int.Parse(txtSupplierId.Text),
                    Convert.ToDateTime(txtToDate.Text),
                    int.Parse(ucCurrency.SelectedCurrency),
                    int.Parse(txtBankID.Text)
                    );
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvPVGenerate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblInvoiceId = (RadLabel)e.Item.FindControl("lblInvoiceId");
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null && lblInvoiceId != null)
            {
                cmdEdit.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../Accounts/AccountsAirfarePaymentVoucherGenerateEdit.aspx?AGENTINVOICEID=" + lblInvoiceId.Text + "'); return true;");
            }

            RadLabel lblRemarks = (RadLabel)e.Item.FindControl("lblRemarksDetail");
            UserControlToolTip ucRemarks = (UserControlToolTip)e.Item.FindControl("ucRemarksDetail");
            if (lblRemarks != null)
            {
                lblRemarks.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucRemarks.ToolTip + "', 'visible');");
                lblRemarks.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucRemarks.ToolTip + "', 'hidden');");
            }

        }
    }

    protected void CreditNote_SetExchangeRate(object sender, EventArgs e)
    {
        if (ucCurrency.SelectedCurrency.ToUpper() != "DUMMY")
        {
            imgBankPicklist.Attributes.Add("onclick", "return showPickList('spnPickListBank', 'codehelp1', '', '../Common/CommonPickListBankInformationAddress.aspx?addresscode=" + txtSupplierId.Text + "&currency=" + ucCurrency.SelectedCurrency + "', true);");
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDINVOICENUMBER", "FLDTICKETNO", "FLDCHARGEABLEAMOUNT", "FLDPAYABLEAMOUNT", "FLDPASSENGERNAME", "FLDFLIGHTDATE", "FLDSECTOR", "FLDACCOUNTCODE", "FLDSUBACCOUNT", "FLDOWNERBUDGETGROUP", "FLDPAYINGCOMPANYCODE", "FLDREMARKS", "FLDCANCELLEDYN" };
            string[] alCaptions = { "Invoice Number", "Ticket No", "Charged Amount", "Amount Payable", "Passenger Name", "Flight Date", "Sector", "Vessel/Non Vessel Account", "Budget Code/Sub Code", "Owner Budget Code","Paying Company","Remarks", "Cancelled Y/N" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = new NameValueCollection();
            nvc = Filter.Airfarepayment;

            DataSet ds = PhoenixAccountsAirfarePaymentVoucherGenerate.SearchAirfareInvoiceContextCompany(
                General.GetNullableInteger(nvc != null ? nvc.Get("txtSupplierId") : string.Empty),
                 General.GetNullableDateTime(nvc != null ? nvc.Get("txtToDate") : string.Empty),
                General.GetNullableInteger(nvc != null ? nvc.Get("ucCurrency") : string.Empty),
                sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                gvPVGenerate.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("gvPVGenerate", "Generate Payment Voucher", alCaptions, alColumns, ds);

                gvPVGenerate.DataSource = ds;
            gvPVGenerate.VirtualItemCount = iRowCount;

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

        string[] alColumns = { "FLDINVOICENUMBER", "FLDTICKETNO", "FLDCHARGEABLEAMOUNT", "FLDPAYABLEAMOUNT", "FLDPASSENGERNAME", "FLDFLIGHTDATE", "FLDSECTOR", "FLDACCOUNTCODE", "FLDSUBACCOUNT", "FLDOWNERBUDGETGROUP", "FLDPAYINGCOMPANYCODE", "FLDREMARKS", "FLDCANCELLEDYN" };
        string[] alCaptions = { "Invoice Number", "Ticket No", "Charged Amount", "Amount Payable", "Passenger Name", "Flight Date", "Sector", "Vessel/Non Vessel Account", "Budget Code/Sub Code", "Owner Budget Code", "Paying Company", "Remarks", "Cancelled Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        NameValueCollection nvc = new NameValueCollection();
        nvc = Filter.Airfarepayment;
        DataSet ds = PhoenixAccountsAirfarePaymentVoucherGenerate.SearchAirfareInvoiceContextCompany(
               General.GetNullableInteger(nvc != null ? nvc.Get("txtSupplierId") : string.Empty),
                General.GetNullableDateTime(nvc != null ? nvc.Get("txtToDate") : string.Empty),
               General.GetNullableInteger(nvc != null ? nvc.Get("ucCurrency") : string.Empty),
               sortexpression, sortdirection,
               (int)ViewState["PAGENUMBER"],
               PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
               ref iRowCount,
               ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename= PaymentVoucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Generate Payment Voucher</h3></td>");
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

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    private bool IsValidData(string supplierCode, string toDate, string Currency , string bank)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (supplierCode.Trim().Equals(""))
            ucError.ErrorMessage = "Supplier details required.";

        if (toDate == null)
            ucError.ErrorMessage = "To date is required.";

        if (Currency.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Currency is required.";

        if (bank.Trim().Equals(""))
            ucError.ErrorMessage = "Bank details required.";

        return (!ucError.IsError);

    }

    private bool IsValidData(string supplierCode, string toDate, string Currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (supplierCode.Trim().Equals(""))
            ucError.ErrorMessage = "Supplier details required.";

        if (toDate == null)
            ucError.ErrorMessage = "To date is required.";

        if (Currency.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Currency is required.";

        return (!ucError.IsError);

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void txtSupplierId_TextChanged(object sender, EventArgs e)
    {
        ViewState["CURRENCY"] = 4;
        imgBankPicklist.Attributes.Add("onclick", "return showPickList('spnPickListBank', 'codehelp1', '', '../Common/CommonPickListBankInformationAddress.aspx?addresscode=" + txtSupplierId.Text + "&currency=" + ViewState["CURRENCY"] + "', true);");
    }

    protected void gvPVGenerate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsAirfarePaymentVoucherGenerate.TravelAgentPVRemove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((RadLabel)e.Item.FindControl("lblInvoiceId")).Text));
            }

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

    private void SetFilter()
    {
        try
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Clear();
            nvc.Add("txtSupplierId", txtSupplierId.Text);
            nvc.Add("txtToDate", txtToDate.Text);
            nvc.Add("ucCurrency", ucCurrency.SelectedCurrency);
            nvc.Add("txtSupplierCode", txtSupplierCode.Text);
            nvc.Add("txtSupplierName", txtSupplierName.Text);
            Filter.Airfarepayment = nvc;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFilterData()
    {
        try
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc = Filter.Airfarepayment;

            ucCurrency.SelectedCurrency = nvc != null ? (nvc.Get("ucCurrency").ToUpper() == "DUMMY" ? "" : nvc.Get("ucCurrency")) : "4";
            txtToDate.Text = nvc != null ? nvc.Get("txtToDate") : "";
            txtSupplierId.Text = nvc != null ? nvc.Get("txtSupplierId") : "";
            txtSupplierCode.Text = nvc != null ? nvc.Get("txtSupplierCode") : "";
            txtSupplierName.Text = nvc != null ? nvc.Get("txtSupplierName") : "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearFilter()
    {
        NameValueCollection nvc = new NameValueCollection();
        Filter.Airfarepayment = null;
        nvc.Clear();
        txtSupplierId.Text = "";
        txtToDate.Text = "";
        txtSupplierCode.Text = "";
        txtSupplierName.Text = "";
    }

    protected void gvPVGenerate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
