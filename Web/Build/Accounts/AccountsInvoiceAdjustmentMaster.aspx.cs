using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;


public partial class AccountsInvoiceAdjustmentMaster : PhoenixBasePage
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
            //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceAdjustmentMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceBasicFilter.aspx?qcalfrom=ADJUSTMENT", "Find", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //MenuOrderForm.SetTrigger(pnlOrderForm);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("History", "HISTORY", ToolBarDirection.Right);
            toolbarmain.AddButton("Queries", "QUERIES", ToolBarDirection.Right);
            toolbarmain.AddButton("Attachments", "ATTACHMENTS", ToolBarDirection.Right);
            toolbarmain.AddButton("PO", "PO", ToolBarDirection.Right);
            toolbarmain.AddButton("Invoice", "INVOICE", ToolBarDirection.Right);


            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.Title = "Invoice Matching";
            MenuOrderFormMain.MenuList = toolbarmain.Show();

            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");
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
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceAdjustment.aspx?qinvoicecode=" + ViewState["invoicecode"];
                }
            }
            MenuOrderFormMain.SelectedMenuIndex = 4;
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

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("INVOICE") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceAdjustment.aspx?qinvoicecode=" + ViewState["invoicecode"];
            }
            if (CommandName.ToUpper().Equals("PO") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceLineItemDetails.aspx?qinvoicecode=" + ViewState["invoicecode"] + "&qcallfrom=AD");
            }
            if (CommandName.ToUpper().Equals("ATTACHMENTS") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=AD");
            }
            if (CommandName.ToUpper().Equals("HISTORY") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceHistory.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString() + "&qfrom=AD", false);
            }
            if (CommandName.ToUpper().Equals("QUERIES") && ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceQueryMaster.aspx?qinvoicecode=" + Filter.CurrentInvoiceCodeSelection + "&qcallfrom=AD", false);
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

        string[] alCaptions = {
                                "Supplier Code",
                                "Supplier Name",
                                "Invoice Reference",
                                "Received Date",
                                "Invoice Amount",
                                //"Vessel Name",
                                "Currency Code",
                                "Invoice Status",
                                "Invoice Number",
                                "Invoice Date",
                                "Invoice Type",
                                "Remarks",
                                "Invoice PIC"
                              };

        string[] alColumns = {  "FLDCODE",
                                "FLDSUPPLIERNAME",
                                "FLDINVOICESUPPLIERREFERENCE",
                                "FLDINVOICERECEIVEDDATE",
                                "FLDINVOICEAMOUNT",
                                //"FLDVESSELLIST",
                                "FLDCURRENCYNAME",
                                "FLDINVOICESTATUSNAME",
                                "FLDINVOICENUMBER",
                                "FLDINVOICEDATE",
                                "FLDINVOICETYPENAME",
                                "FLDREMARKS",
                                "FLDPICUSERNAME"
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
        if (Filter.InvRegAdvFilterYN != null && Filter.InvRegAdvFilterYN.ToString() == "Y")
        {
            ds = PhoenixAccountsInvoice.InvoiceSearch(
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
                                                    , null
                                                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtRemarks")) : string.Empty
                                                    , nvc != null ? General.GetNullableString(nvc.Get("ddlPurchaserList")) : string.Empty
                                                    , nvc != null ? General.GetNullableString(nvc.Get("ddlSuptList")) : string.Empty
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("chkPriorityInv")) : null
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ucPIC")) : null
                                                    , sortexpression, sortdirection
                                                    , (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                    , ref iRowCount, ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixAccountsInvoice.InvoiceReconcileDefaultSearch(
                                                     nvc != null ? General.GetNullableInteger(nvc.Get("ddlInvoiceType")) : null
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ucInvoiceStatus")) : null
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtOrderNumber")) : null
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceNumberSearch")) : null
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtSupplierReferenceSearch")) : null
                                                   , nvc != null ? General.GetNullableDateTime(nvc.Get("txtReceivedFromdateSearch")) : null
                                                   , nvc != null ? General.GetNullableDateTime(nvc.Get("txtReceivedTodateSearch")) : null
                                                   , nvc != null ? General.GetNullableString(nvc.Get("txtRemarks")) : null
                                                   , nvc != null ? General.GetNullableInteger(nvc.Get("ddlPurchaserList")) : null
                                                   , sortexpression, sortdirection
                                                   , (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                   , ref iRowCount, ref iTotalPageCount
                                                   );
        }
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
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentInvoiceSelection;

        if (Filter.InvRegAdvFilterYN != null && Filter.InvRegAdvFilterYN.ToString() == "Y")
        {
            ds = PhoenixAccountsInvoice.InvoiceSearch(
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
                                                    , null
                                                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtRemarks")) : string.Empty
                                                    , nvc != null ? General.GetNullableString(nvc.Get("ddlPurchaserList")) : string.Empty
                                                    , nvc != null ? General.GetNullableString(nvc.Get("ddlSuptList")) : string.Empty
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("chkPriorityInv")) : null
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ucPIC")) : null
                                                    , sortexpression, sortdirection
                                                    , (int)ViewState["PAGENUMBER"], gvInvoice.PageSize
                                                    , ref iRowCount, ref iTotalPageCount);

        }
        else
        {
            ds = PhoenixAccountsInvoice.InvoiceReconcileDefaultSearch(
                                                      nvc != null ? General.GetNullableInteger(nvc.Get("ddlInvoiceType")) : null
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ucInvoiceStatus")) : null
                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtOrderNumber")) : null
                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceNumberSearch")) : null
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtSupplierReferenceSearch")) : null
                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtReceivedFromdateSearch")) : null
                                                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtReceivedTodateSearch")) : null
                                                    , nvc != null ? General.GetNullableString(nvc.Get("txtRemarks")) : null
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlPurchaserList")) : null
                                                    , sortexpression, sortdirection
                                                    , (int)ViewState["PAGENUMBER"], gvInvoice.PageSize
                                                    , ref iRowCount, ref iTotalPageCount
                                                    );
        }
        string[] alCaptions = {
                                "Supplier Code",
                                "Supplier Name",
                                "Invoice Reference",
                                "Received Date",
                                "Invoice Amount",
                                //"Vessel Name",
                                "Currency Code",
                                "Invoice Status",
                                "Invoice Number",
                                "Invoice Date",
                                "Invoice Type",
                                "Invoice PIC"
                              };

        string[] alColumns = {  "FLDCODE",
                                "FLDSUPPLIERNAME",
                                "FLDINVOICESUPPLIERREFERENCE",
                                "FLDINVOICERECEIVEDDATE",
                                "FLDINVOICEAMOUNT",
                                //"FLDVESSELLIST",
                                "FLDCURRENCYNAME",
                                "FLDINVOICESTATUSNAME",
                                "FLDINVOICENUMBER",
                                "FLDINVOICEDATE",
                                "FLDINVOICETYPENAME",
                                "FLDPICUSERNAME"
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
                // gvInvoice.SelectedIndex = 0;
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceAdjustment.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceAdjustment.aspx?voucherid=";
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
        if (e.Item is GridDataItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton cmdPOApprove = (ImageButton)e.Item.FindControl("cmdPOApprove");
            if (cmdPOApprove != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdPOApprove.CommandName)) cmdPOApprove.Visible = false;

            ImageButton imgAttachment = (ImageButton)e.Item.FindControl("imgAttachment");
            if (imgAttachment != null)
                if (!SessionUtil.CanAccess(this.ViewState, imgAttachment.CommandName)) imgAttachment.Visible = false;

            //Label lblVesselName = (Label)e.Row.FindControl("lblVesselName");
            //UserControlToolTip uctVessel = (UserControlToolTip)e.Row.FindControl("ucToolTipVessel");
            //lblVesselName.Attributes.Add("onmouseover", "showTooltip(ev,'" + uctVessel.ToolTip + "', 'visible');");
            //lblVesselName.Attributes.Add("onmouseout", "showTooltip(ev,'" + uctVessel.ToolTip + "', 'hidden');");


            //Label lblInvoiceStatus = (Label)e.Row.FindControl("lblInvoiceStatus");
            //Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            //UserControlToolTip ucToolTipStatus = (UserControlToolTip)e.Row.FindControl("ucToolTipStatus");
            //lblInvoiceStatus.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipStatus.ToolTip + "', 'visible');");
            //lblInvoiceStatus.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipStatus.ToolTip + "', 'hidden');");

            //if (lblStatus!= null && lblStatus.Text.Length > 0)
            //{
            //    string s = lblStatus.Text;
            //    string result = "";
            //    string[] words = s.Split(',');
            //    foreach (string word in words)
            //    {
            //        result = result + word + " </br> ";
            //    }
            //    ucToolTipStatus.Text = result;
            //}


            RadLabel lblPrioritySupplier = (RadLabel)e.Item.FindControl("lblPrioritySupplier");
            ImageButton imbSupplierMismatch = (ImageButton)e.Item.FindControl("imbSupplierMismatch");
            ImageButton imbCurrencyMismatch = (ImageButton)e.Item.FindControl("imbCurrencyMismatch");
            ImageButton imbMultiplePOs = (ImageButton)e.Item.FindControl("imbMultiplePOs");
            ImageButton imbPriorityInvoice = (ImageButton)e.Item.FindControl("imbPriorityInvoice");

            if (lblPrioritySupplier != null)
            {
                if (!string.IsNullOrEmpty(lblPrioritySupplier.Text) && int.Parse(lblPrioritySupplier.Text) == 1 && imbPriorityInvoice != null)
                    imbPriorityInvoice.Visible = true;
            }
            if (imbCurrencyMismatch != null)
                if (!SessionUtil.CanAccess(this.ViewState, imbCurrencyMismatch.CommandName)) imbCurrencyMismatch.Visible = false;
            if (imbSupplierMismatch != null)
                if (!SessionUtil.CanAccess(this.ViewState, imbSupplierMismatch.CommandName)) imbSupplierMismatch.Visible = false;
            if (imbPriorityInvoice != null)
                if (!SessionUtil.CanAccess(this.ViewState, imbPriorityInvoice.CommandName)) imbPriorityInvoice.Visible = false;
            if (imbMultiplePOs != null)
                if (!SessionUtil.CanAccess(this.ViewState, imbMultiplePOs.CommandName)) imbMultiplePOs.Visible = false;

            RadTextBox txtInvoiceCode = (RadTextBox)e.Item.FindControl("txtInvoiceCode");
            ImageButton cmdMoreInfo = (ImageButton)e.Item.FindControl("cmdMoreInfo");
            if (cmdMoreInfo != null)
            {
                //cmdMoreInfo.Attributes.Add("onclick", "openNewWindow('codeHelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsInvoiceMoreInfo.aspx?invoiceCode=" + txtInvoiceCode.Text + "');return false;");

                cmdMoreInfo.Attributes.Add("onclick", "openNewWindow('codeHelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsInvoiceMoreInfo.aspx?invoiceCode=" + txtInvoiceCode.Text + "');return false;");
            }
        }


        if (e.Item is GridDataItem)
        {
            if (e.Item.DataItem != null)
            {
                ImageButton img1 = (ImageButton)e.Item.FindControl("imgAttachment");
                //Label lblControl = (Label)e.Row.Cells[2].FindControl("lblAttachmentexists");
                RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblInvoiceStatus");
                ImageButton imgPOApprove = (ImageButton)e.Item.FindControl("cmdPOApprove");
                //if (lblControl.Text == "1")
                //{
                //    img1.Visible = true;
                //}
                //else { img1.Visible = false; }

                RadLabel lblMismatched = (RadLabel)e.Item.FindControl("lblMismatched");
                if (lblMismatched.Text == "1")
                {
                    e.Item.ForeColor = System.Drawing.Color.Red;
                }
                if (lblStatus != null && lblStatus.Text != "" && imgPOApprove != null)
                {
                    if (lblStatus.Text == "Reconciled waiting for Approval")
                    {
                        imgPOApprove.Enabled = true;
                        imgPOApprove.Visible = true;
                    }
                    if (!SessionUtil.CanAccess(this.ViewState, imgPOApprove.CommandName)) imgPOApprove.Visible = false;
                }

            }
        }
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
        else if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            int iRowno;
            iRowno = e.Item.ItemIndex;
            BindPageURL(iRowno);
        }

        else if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
        {
            Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode=" + ((RadTextBox)e.Item.FindControl("txtInvoiceCode")).Text + "&qfrom=AD");
        }
        //else if (e.CommandName.ToUpper().Equals("HISTORY"))
        //{
        //    Response.Redirect("../Accounts/AccountsInvoiceEarmark.aspx?qinvoicecode=" + ((RadTextBox)_gridView.Rows[iRowno].FindControl("txtInvoiceCode")).Text+"&qfrom=AD");
        //}
        if (e.CommandName.ToUpper().Equals("POAPPROVE"))
        {
            if (ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != "")
            {
                string strOrderId = ViewState["invoicecode"].ToString();
                UpdateApprovalStatus(strOrderId);
            }
            Rebind();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Session["New"].ToString() == "Y")
        {
            Session["New"] = "N";
            BindPageURL(0);
        }
        Rebind();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["invoicecode"] = ((RadTextBox)gvInvoice.Items[rowindex].FindControl("txtInvoiceCode")).Text;
            PhoenixAccountsVoucher.InvoiceNumber = ((RadLabel)gvInvoice.Items[rowindex].FindControl("lblInvoiceid")).Text;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoiceAdjustment.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvInvoice.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
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
    protected void Rebind()
    {
        gvInvoice.SelectedIndexes.Clear();
        gvInvoice.EditIndexes.Clear();
        gvInvoice.DataSource = null;
        gvInvoice.Rebind();
    }
    private void UpdateApprovalStatus(string invoicecode)
    {
        PhoenixAccountsInvoice.InvoiceOrderFormStatgingApprove(new Guid(invoicecode), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    }
}
