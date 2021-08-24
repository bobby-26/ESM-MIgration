using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class AccountsRemittanceInvoiceMaster : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Invoice", "LINEITEM", ToolBarDirection.Right);
            toolbarmain.AddButton("Remittance", "VOUCHER", ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            MenuOrderFormMain.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["voucherid"] = null;
                ViewState["PAGEURL"] = null;
                ViewState["INDEXNO"] = 0;
                gvVoucherDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (Request.QueryString["REMITTANCEID"] != null)
                {
                    ViewState["REMITTANCEID"] = Request.QueryString["REMITTANCEID"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceInvoice.aspx?REMITTANCEID=" + ViewState["REMITTANCEID"] + "&INVOICEDTKEY=" + ViewState["INVOICEDTKEY"];
                }
            }

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsRemittanceInvoiceMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVoucherDetails')", "Print Grid", "icon_print.png", "PRINT");
            //toolbargrid.AddImageButton("../Accounts/AccountsInvoicePaymentVoucherFilter.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsRemittanceSupplierHistory.aspx?REMITTANCEID=" + ViewState["REMITTANCEID"].ToString() + "'); return false;", "History", "query.png", "HISTORY");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsRemittanceInvoiceMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvAdvancePayment')", "Print Grid", "icon_print.png", "PRINT");


            MenuAdvancePayments.AccessRights = this.ViewState;
            MenuAdvancePayments.MenuList = toolbar.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsRemittanceMaster.aspx?REMITTENCEID=" + ViewState["REMITTANCEID"].ToString());
            }
            MenuOrderFormMain.SelectedMenuIndex = 0;
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

        string[] alCaptions = { "Invoice Number", "Supplier Invoice Number", "Invoice Payable Amount", "Payment Voucher Number" };
        string[] alColumns = { "FLDINVOICENUMBER", "FLDSUPPLIERREFERENCE", "FLDINVOICEAMOUNT", "FLDPAYMENTVOUCHERNUMBER" };


        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsRemittance.RemittanceInvoiceList(new Guid(ViewState["REMITTANCEID"].ToString())
                    , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=InvoicePaymentVoucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Voucher</h3></td>");
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
                Rebind();
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
        int index;
        DataSet ds = new DataSet();
        DataSet dsCtm = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsRemittance.RemittanceInvoiceList(new Guid(ViewState["REMITTANCEID"].ToString())
                    , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvVoucherDetails.PageSize, ref iRowCount, ref iTotalPageCount);

        //dsCtm = PhoenixAccountsRemittance.RemittanceCtmList(new Guid(ViewState["REMITTANCEID"].ToString())
        //            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);


        string[] alCaptions = { "Invoice Number", "Supplier Invoice Number", "Invoice Payable Amount", "Payment Voucher Number" };
        string[] alColumns = { "FLDINVOICENUMBER", "FLDSUPPLIERREFERENCE", "FLDINVOICEAMOUNT", "FLDPAYMENTVOUCHERNUMBER" };

        General.SetPrintOptions("gvVoucherDetails", "Invoice Voucher", alCaptions, alColumns, ds);

        gvVoucherDetails.DataSource = ds;
        gvVoucherDetails.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)  // Invoice
        {
            int rowcount = ds.Tables[0].Rows.Count;
            ViewState["rowcount"] = rowcount;
            if (ViewState["voucherid"] == null)
            {
                index = 0;
                NameValueCollection nvc = Filter.CurrentRemittenceinvoicegvindex;
                if (Filter.CurrentRemittenceinvoicegvindex != null)
                {
                    index = Convert.ToInt32(nvc.Get("gvindexno").ToString().Trim());
                    if (index <= rowcount - 1)
                    {
                        ViewState["INDEXNO"] = index;
                    }
                    else
                    {
                        index = rowcount - 1;
                        ViewState["INDEXNO"] = index;
                    }
                }
                ViewState["voucherid"] = ds.Tables[0].Rows[index]["FLDPAYMENTVOUCHERID"].ToString();
                ViewState["INVOICEDTKEY"] = ds.Tables[0].Rows[index]["FLDDTKEY"].ToString();
                ViewState["INVOICEBANKINFOPAGENUMBER"] = ds.Tables[0].Rows[index]["FLDINVOICEBANKINFOPAGENUMBER"].ToString();
                ViewState["INVOICECODE"] = ds.Tables[0].Rows[index]["FLDINVOICECODE"].ToString();
            }

            if (ViewState["PAGEURL"] == null)
            {
                index = 0;
                NameValueCollection nvc = Filter.CurrentRemittenceinvoicegvindex;
                if (Filter.CurrentRemittenceinvoicegvindex != null)
                {
                    index = Convert.ToInt32(nvc.Get("gvindexno").ToString().Trim());
                    if (index <= rowcount - 1)
                    {
                        ViewState["INDEXNO"] = index;
                    }
                    else
                    {
                        index = rowcount - 1;
                        ViewState["INDEXNO"] = index;
                    }
                }

                ViewState["voucherid"] = ds.Tables[0].Rows[index]["FLDPAYMENTVOUCHERID"].ToString();
                ViewState["INVOICEDTKEY"] = ds.Tables[0].Rows[index]["FLDDTKEY"].ToString();
                ViewState["INVOICEBANKINFOPAGENUMBER"] = ds.Tables[0].Rows[index]["FLDINVOICEBANKINFOPAGENUMBER"].ToString();
                ViewState["INVOICECODE"] = ds.Tables[0].Rows[index]["FLDINVOICECODE"].ToString();
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceInvoice.aspx?REMITTANCEID=" + ViewState["REMITTANCEID"] + "&INVOICEDTKEY=" + ViewState["INVOICEDTKEY"] + "&INVOICEBANKINFOPAGENUMBER=" + ViewState["INVOICEBANKINFOPAGENUMBER"] + "&gvindex=" + ViewState["INDEXNO"];
            }
            SetRowSelection();
        }

        else
        {
            if (ViewState["PAGEURL"] == null)
            {

                index = -1;
                ViewState["INDEXNO"] = index;
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceInvoice.aspx?REMITTANCEID=" + ViewState["REMITTANCEID"] + "&INVOICEDTKEY=" + "&gvindex=" + ViewState["INDEXNO"];
            }
            DataTable dt = ds.Tables[0];
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvVoucherDetails.SelectedIndexes.Clear();
        gvVoucherDetails.EditIndexes.Clear();
        gvVoucherDetails.DataSource = null;
        gvVoucherDetails.Rebind();
    }
    private void SetRowSelection()
    {
        //int index = 0;
        //NameValueCollection nvc = Filter.CurrentRemittenceinvoicegvindex;
        //if (Filter.CurrentRemittenceinvoicegvindex != null)
        //{
        //    int rowcount = Convert.ToInt32(ViewState["rowcount"].ToString());
        //    if (Filter.CurrentRemittenceinvoicegvindex != null)
        //    {
        //        index = Convert.ToInt32(nvc.Get("gvindexno").ToString().Trim());
        //        if (index <= rowcount - 1)
        //        {
        //            ViewState["INDEXNO"] = index;
        //        }
        //        else
        //        {
        //            index = rowcount - 1;
        //            ViewState["INDEXNO"] = index;
        //        }
        //    }
        gvVoucherDetails.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvVoucherDetails.Items)
        {
            if (item.GetDataKeyValue("FLDINVOICECODE").ToString().Equals(ViewState["INVOICECODE"].ToString()))
            {
                gvVoucherDetails.SelectedIndexes.Add(item.ItemIndex);
                PhoenixAccountsInvoicePaymentVoucher.InvoiceVoucherNumber = ((LinkButton)gvVoucherDetails.Items[item.ItemIndex].FindControl("lnkInvoice")).Text;
            }
        }
        //}    
    }

    protected void gvVoucherDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
        }
    }

    protected void gvVoucherDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            int iRowno;
            iRowno = e.Item.ItemIndex;
            ViewState["INDEXNO"] = iRowno;
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("gvindexno", ViewState["INDEXNO"].ToString());
            Filter.CurrentRemittenceinvoicegvindex = criteria;
            BindPageURL(iRowno);
            Rebind();
        }
        if (e.CommandName.ToUpper().Equals("PAGE"))
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();

        if (Session["New"].ToString() == "Y")
        {
            Session["New"] = "N";
            BindPageURL(0);
        }
    }


    private void BindPageURL(int rowindex)
    {
        try
        {
            gvVoucherDetails.MasterTableView.Items[rowindex].Selected = true;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceInvoice.aspx?REMITTANCEID=" + ViewState["REMITTANCEID"] + "&INVOICEDTKEY=" + ViewState["INVOICEDTKEY"] + "&INVOICEBANKINFOPAGENUMBER=" + ViewState["INVOICEBANKINFOPAGENUMBER"] + "&gvindex=" + rowindex;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoucherDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvVoucherDetails.SelectedIndexes.Add(e.NewSelectedIndex);
        ViewState["INVOICEDTKEY"] = ((LinkButton)gvVoucherDetails.Items[e.NewSelectedIndex].FindControl("lnkInvoice")).CommandArgument;
        ViewState["INVOICEBANKINFOPAGENUMBER"] = ((RadLabel)gvVoucherDetails.Items[e.NewSelectedIndex].FindControl("lblBankInfoPageNumber")).Text;
        ViewState["INVOICECODE"] = ((RadLabel)gvVoucherDetails.Items[e.NewSelectedIndex].FindControl("lblInvoiceCode")).Text;
        SetRowSelection();
        BindPageURL(e.NewSelectedIndex);
    }
    protected void MenuAdvancePayments_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelAdvancePayments();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataAdvancePayments()
    {

        DataSet ds = new DataSet();

        ds = PhoenixAccountsRemittance.RemittanceAdvancePaymentList(new Guid(ViewState["REMITTANCEID"].ToString()));

        string[] alCaptions = { "Reference Number", "Source", "Payable Amount", "Payment Voucher Number" };
        string[] alColumns = { "FLDREFERENCENUMBER", "FLDSOURCE", "FLDAMOUNT", "FLDPAYMENTVOUCHERNUMBER" };

        General.SetPrintOptions("gvAdvancePayment", "Advance Payment CTM", alCaptions, alColumns, ds);

        gvAdvancePayment.DataSource = ds;

    }
    protected void ShowExcelAdvancePayments()
    {
        DataSet ds = new DataSet();

        string[] alCaptions = { "Reference Number", "Source", "Payable Amount", "Payment Voucher Number" };
        string[] alColumns = { "FLDREFERENCENUMBER", "FLDSOURCE", "FLDAMOUNT", "FLDPAYMENTVOUCHERNUMBER" };


        ds = PhoenixAccountsRemittance.RemittanceAdvancePaymentList(new Guid(ViewState["REMITTANCEID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=AdvancePaymentCTM.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Voucher</h3></td>");
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
    protected void gvAdvancePayment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton lnkRef = (LinkButton)e.Item.FindControl("lnkReferenceNo");
            RadLabel lblDtkey = (RadLabel)e.Item.FindControl("lblDtkey");

            if (lnkRef != null && lblDtkey != null)
                lnkRef.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text + "&mod="
                       + PhoenixModule.ACCOUNTS + "&u=1" + "');return true;");
        }
    }
    protected void gvAdvancePayment_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvVoucherDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVoucherDetails.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAdvancePayment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAdvancePayment.CurrentPageIndex + 1;
            BindDataAdvancePayments();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
