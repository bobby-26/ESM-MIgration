using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsInvoiceMasterForPurchase : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //cmdHiddenSubmit.Attributes.Add("style", "Display:None;");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceMasterForPurchase.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceFilter.aspx?qcalfrom=INVOICEFORPURCHASE&qcallfrom=inv", "Find", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //  MenuOrderForm.SetTrigger(pnlOrderForm);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");


            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Invoice", "INVOICE");
            toolbarmain.AddButton("PO", "PO");
            toolbarmain.AddButton("Direct PO", "DPO");
            toolbarmain.AddButton("Attachments", "ATTACHMENTS");
            toolbarmain.AddButton("History", "HISTORY");

            MenuOrderFormMain.AccessRights = this.ViewState;

            MenuOrderFormMain.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                Session["New"] = "N";

                // MenuOrderFormMain.SetTrigger(pnlOrderForm);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["invoicecode"] = null;
                ViewState["PAGEURL"] = null;
                gvInvoice.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                string vessellist = null;
                DataSet ds = PhoenixAccountsInvoice.VesselList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ref vessellist);
                ViewState["Vessellist"] = vessellist;

                if (Request.QueryString["qinvoicecode"] != null)
                {
                    ViewState["invoicecode"] = Request.QueryString["qinvoicecode"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"];
                }
            }
            MenuOrderFormMain.SelectedMenuIndex = 0;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInvoice_Sorting(object sender, GridSortCommandEventArgs e)
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
    protected void gvInvoice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInvoice.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvInvoice_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        ViewState["invoicecode"] = null;

    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("INVOICE"))
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"];
            }
            if (CommandName.ToUpper().Equals("PO") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceLineItemDetails.aspx?qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=invoiceforpurchase");
            }
            if (CommandName.ToUpper().Equals("DPO") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceDirectPO.aspx?qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=invoiceforpurchase");
            }
            if (CommandName.ToUpper().Equals("ATTACHMENTS") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=invoiceforpurchase");
            }
            if (CommandName.ToUpper().Equals("HISTORY") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceHistory.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=invoiceforpurchase");
            }
            else
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

        string[] alCaptions = { "PO Number",
                                "Invoice Number",
                                "Supplier Name",
                                "Supplier Invoice Reference",
                                "Received Date",
                                "Vessel Name",
                                "Currency Code",
                                "Invoice Amount",
                                "Invoice Status",
                                                                               
                                //"Entered Date", 
                                //"Month",
                                //"Invoice Type"                                
                              };

        string[] alColumns = {  "FLDPURCHASEORDERNUMBER",
                                "FLDINVOICENUMBER",
                                "FLDNAME",
                                "FLDINVOICESUPPLIERREFERENCE",
                                "FLDINVOICERECEIVEDDATE",
                                "FLDVESSELNAME",
                                "FLDCURRENCYNAME",
                                "FLDINVOICEAMOUNT",
                                "FLDINVOICESTATUSNAME",
                               
                                //"FLDINVOICEDATE",
                                //"FLDMONTHNAME",
                                //"FLDINVOICETYPENAME"
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
        ds = PhoenixAccountsInvoice.InvoiceRegisterSearchForPurchase(
                                                  nvc != null ? General.GetNullableInteger(nvc.Get("ddlInvoiceType")) : null
                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucInvoiceStatus")) : null
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
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtRemarks")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("ddlPurchaserList")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("ddlSuptList")) : string.Empty
                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucPIC")) : null
                                                , sortexpression, sortdirection
                                                , (int)ViewState["PAGENUMBER"]
                                                , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
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

        ds = PhoenixAccountsInvoice.InvoiceRegisterSearchForPurchase(
                                                  nvc != null ? General.GetNullableInteger(nvc.Get("ddlInvoiceType")) : null
                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucInvoiceStatus")) : null
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtOrderNumber")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceNumberSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtVendorId")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("ddlCurrencyCode")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("ucVessel")) : (string)ViewState["Vessellist"]
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtSupplierReferenceSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceFromdateSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceTodateSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtReceivedFromdateSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtReceivedTodateSearch")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("companylist")) : PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString()
                                                , null
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtRemarks")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("ddlPurchaserList")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("ddlSuptList")) : string.Empty
                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucPIC")) : null
                                                , sortexpression, sortdirection
                                                , (int)ViewState["PAGENUMBER"], gvInvoice.PageSize
                                                , ref iRowCount, ref iTotalPageCount);


        string[] alCaptions = { "PO Number",
                                "Invoice Number",
                                "Supplier Name",
                                "Supplier Invoice Reference",
                                "Received Date",
                                "Vessel Name",
                                "Currency Code",
                                "Invoice Amount",
                                "Invoice Status",
                                                                                        
                                //"Entered Date", 
                                //"Month",
                                //"Invoice Type"                                
                              };

        string[] alColumns = {  "FLDPURCHASEORDERNUMBER",
                                "FLDINVOICENUMBER",
                                "FLDNAME",
                                "FLDINVOICESUPPLIERREFERENCE",
                                "FLDINVOICERECEIVEDDATE",
                                "FLDVESSELNAME",
                                "FLDCURRENCYNAME",
                                "FLDINVOICEAMOUNT",
                                "FLDINVOICESTATUSNAME",
                               
                                //"FLDINVOICEDATE",
                                //"FLDMONTHNAME",
                                //"FLDINVOICETYPENAME"
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
                //gvInvoice.SelectedIndex = 0;
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceForPurchase.aspx?voucherid=";
            }
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
                PhoenixAccountsVoucher.InvoiceNumber = ((RadLabel)gvInvoice.Items[item.ItemIndex].FindControl("lblInvoiceid")).Text;
            }
        }
    }


    protected void gvInvoice_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton imgAttachment = (ImageButton)e.Item.FindControl("imgAttachment");
            if (imgAttachment != null)
                if (!SessionUtil.CanAccess(this.ViewState, imgAttachment.CommandName)) imgAttachment.Visible = false;
        }

    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvInvoice_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            int iRowno;
            iRowno = e.Item.ItemIndex;
            BindPageURL(iRowno);
            SetRowSelection();
        }
        if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
        {
            Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode=" + ((RadTextBox)e.Item.FindControl("txtInvoiceCode")).Text + "&qfrom=invoiceforpurchase");
        }
    }

    // protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    // {
    //     BindData();
    //     if (Session["New"].ToString() == "Y")
    //     {
    //         gvInvoice.SelectedIndex = 0;
    //         Session["New"] = "N";
    //         BindPageURL(gvInvoice.SelectedIndex);
    //     }
    // }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadTextBox tb = ((RadTextBox)gvInvoice.Items[rowindex].FindControl("txtInvoiceCode"));
            if (tb != null)
                ViewState["invoicecode"] = tb.Text;
            RadLabel lbl = ((RadLabel)gvInvoice.Items[rowindex].FindControl("lblInvoiceid"));
            if (lbl != null)
                PhoenixAccountsVoucher.InvoiceNumber = lbl.Text;
            if (ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceForPurchase.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    // protected void gvInvoice_SelectedIndexChanged(object sender, EventArgs e)
    // {
    //     RadGrid grid = (RadGrid)sender;
    //     ViewState["invoicecode"] = grid.SelectedValue;
    //     ifMoreInfo.Attributes["src"] = ViewState["URL"] + ViewState["invoicecode"].ToString();
    //     SetRowSelection();
    // }

    protected void gvInvoice_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvInvoice.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
    }
}
