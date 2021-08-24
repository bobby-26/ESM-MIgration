using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsPaymentvoucherGenerate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsPaymentvoucherGenerate.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");
            // toolbargrid.AddImageButton("../Accounts/AccountsInvoiceFilter.aspx?qcalfrom=INVOICE&qcallfrom=inv", "Find", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Generate Payment Voucher", "GENERATEPAYMENTVOUCHER", ToolBarDirection.Right);
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                Session["New"] = "N";


                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["invoicecode"] = null;
                ViewState["PAGEURL"] = null;
                gvInvoice.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            // MenuOrderFormMain.SelectedMenuIndex = 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvInvoice_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("GENERATEPAYMENTVOUCHER"))
            {
                try
                {
                    PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../Accounts/AccountsPaymentvoucherGenerate.aspx");
                BindData();
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
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = {
                                "Supplier Code",
                                "Supplier Name",
                                "Invoice Reference",
                                "Received Date",
                                "Invoice Amount",
                                "Currency Code",
                                "Invoice Number",
                                "Entered Date",
                                "Invoice Type"
                              };

        string[] alColumns = {  "FLDCODE",
                                "FLDNAME",
                                "FLDINVOICESUPPLIERREFERENCE",
                                "FLDINVOICERECEIVEDDATE",
                                "FLDINVOICEAMOUNT",
                                "FLDCURRENCYNAME",
                                "FLDINVOICENUMBER",
                                "FLDINVOICEDATE",
                                "FLDINVOICETYPENAME"
                             };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentInvoiceSelection;
        ds = PhoenixAccountsInvoice.InvoiceSearch(
                                                  nvc != null ? General.GetNullableInteger(nvc.Get("ddlInvoiceType")) : null
                                                , 371
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtOrderNumber").ToString().Trim()) : null
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceNumberSearch").ToString().Trim()) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtVendorId")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("ddlCurrencyCode").ToString().Trim()) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("ucVessel").ToString().Trim()) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtSupplierReferenceSearch").ToString().Trim()) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceFromdateSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceTodateSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtReceivedFromdateSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtReceivedTodateSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("companylist")) : PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString()
                                                , null
                                                , null
                                                , null
                                                , null
                                                , null
                                                , null
                                                , sortexpression, sortdirection
                                                , (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountsInvoice.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Invoice</h3></td>");
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentInvoiceSelection;

        ds = PhoenixAccountsInvoice.InvoiceSearch(
                                                  nvc != null ? General.GetNullableInteger(nvc.Get("ddlInvoiceType")) : null
                                                , 371
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtOrderNumber")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceNumberSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtVendorId")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("ddlCurrencyCode")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("ucVessel")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtSupplierReferenceSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceFromdateSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceTodateSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtReceivedFromdateSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtReceivedTodateSearch")) : string.Empty
                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString()
                                                , null
                                                , null
                                                , null
                                                , null
                                                , null
                                                , null
                                                , sortexpression, sortdirection
                                                , (int)ViewState["PAGENUMBER"], gvInvoice.PageSize
                                                , ref iRowCount, ref iTotalPageCount);


        string[] alCaptions = {
                                "Supplier Code",
                                "Supplier Name",
                                "Invoice Reference",
                                "Received Date",
                                "Invoice Amount",
                                "Currency Code",
                                "Invoice Number",
                                "Entered Date",
                                "Invoice Type"
                              };

        string[] alColumns = {  "FLDCODE",
                                "FLDNAME",
                                "FLDINVOICESUPPLIERREFERENCE",
                                "FLDINVOICERECEIVEDDATE",
                                "FLDINVOICEAMOUNT",
                                "FLDCURRENCYNAME",
                                "FLDINVOICENUMBER",
                                "FLDINVOICEDATE",
                                "FLDINVOICETYPENAME"
                             };

        General.SetPrintOptions("gvInvoice", "Accounts Invoice", alCaptions, alColumns, ds);


        gvInvoice.DataSource = ds;
        gvInvoice.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["invoicecode"] == null)
            {
                ViewState["invoicecode"] = ds.Tables[0].Rows[0]["FLDINVOICECODE"].ToString();
            }
            SetRowSelection();
        }
    }
    protected void gvInvoice_ItemCommand(object sender, GridCommandEventArgs e)
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
    private void SetRowSelection()
    {
        gvInvoice.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvInvoice.Items)
        {
            if (item.GetDataKeyValue("FLDINVOICECODE").ToString().Equals(ViewState["invoicecode"].ToString()))
            {
                gvInvoice.SelectedIndexes.Add(item.ItemIndex);
                PhoenixAccountsVoucher.VoucherNumber = ((RadLabel)gvInvoice.Items[item.ItemIndex].FindControl("lblInvoiceid")).Text;
            }
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvInvoice.Items[rowindex];
            RadTextBox tb = ((RadTextBox)gvInvoice.Items[rowindex].FindControl("txtInvoiceCode"));
            if (tb != null)
                ViewState["invoicecode"] = tb.Text;
            RadLabel lbl = ((RadLabel)gvInvoice.Items[item.ItemIndex].FindControl("lblInvoiceid"));
            if (lbl != null)
                PhoenixAccountsVoucher.VoucherNumber = lbl.Text;
            //if (ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString()!=string.Empty)
            //ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoice.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInvoice_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvInvoice.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
    }
}
