using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsInvoiceMaster : PhoenixBasePage
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

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceBasicFilter.aspx?qcalfrom=INVOICE&qcallfrom=inv", "Find", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //MenuOrderForm.SetTrigger(pnlOrderForm);
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>ResizeGridViewHeader('gvInvoice');</script>");

            if (!IsPostBack)
            {
                Filter.CurrentInvoiceCodeSelection = null;
                Session["New"] = "N";

                //MenuOrderFormMain.SetTrigger(pnlOrderForm);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["invoicecode"] = null;
                ViewState["PAGEURL"] = null;
                string vessellist = null;
                DataSet ds = PhoenixAccountsInvoice.VesselList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ref vessellist);
                ViewState["Vessellist"] = vessellist;
                if (Request.QueryString["qinvoicecode"] != null)
                {
                    ViewState["invoicecode"] = Request.QueryString["qinvoicecode"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoice.aspx?qinvoicecode=" + ViewState["invoicecode"];
                }
                gvInvoice.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            if (Filter.CurrentInvoiceCodeSelection != null)
                ViewState["invoicecode"] = Filter.CurrentInvoiceCodeSelection;

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Invoice", "INVOICE");
            toolbarmain.AddButton("PO", "PO");
            toolbarmain.AddButton("Direct PO", "DPO");
            toolbarmain.AddButton("Attachments", "ATTACHMENTS");
            toolbarmain.AddButton("Queries", "QUERIES");
            toolbarmain.AddButton("History", "HISTORY");

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();

            MenuOrderFormMain.SelectedMenuIndex = 0;
            // BindData();
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
            if (CommandName.ToUpper().Equals("INVOICE"))
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoice.aspx?qinvoicecode=" + Filter.CurrentInvoiceCodeSelection;
            }
            if (CommandName.ToUpper().Equals("PO") && Filter.CurrentInvoiceCodeSelection != null && Filter.CurrentInvoiceCodeSelection != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceLineItemDetails.aspx?qinvoicecode=" + Filter.CurrentInvoiceCodeSelection + "&qcallfrom=invoice");
            }
            if (CommandName.ToUpper().Equals("DPO") && Filter.CurrentInvoiceCodeSelection != null && Filter.CurrentInvoiceCodeSelection != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceDirectPO.aspx?qinvoicecode=" + Filter.CurrentInvoiceCodeSelection + "&qcallfrom=invoice");
            }
            if (CommandName.ToUpper().Equals("ATTACHMENTS") && Filter.CurrentInvoiceCodeSelection != null && Filter.CurrentInvoiceCodeSelection != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode=" + Filter.CurrentInvoiceCodeSelection + "&qfrom=invoice");
            }
            if (CommandName.ToUpper().Equals("HISTORY") && Filter.CurrentInvoiceCodeSelection != null && Filter.CurrentInvoiceCodeSelection != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceHistory.aspx?qinvoicecode=" + Filter.CurrentInvoiceCodeSelection + "&qfrom=invoice", false);
            }
            if (CommandName.ToUpper().Equals("QUERIES") && Filter.CurrentInvoiceCodeSelection != null && Filter.CurrentInvoiceCodeSelection != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInvoiceQueryMaster.aspx?qinvoicecode=" + Filter.CurrentInvoiceCodeSelection + "&qcallfrom=invoice", false);
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
                                "PO Number",
                                "Invoice Reference",
                                "Received Date",
                                "Invoice Amount",
                                "Vessel Name",
                                "Currency Code",
                                "Invoice Status",
                                "Invoice Number",
                                "Invoice Date",
                                "Received Month",
                                "Invoice Type",
                                "Remarks",
                                "Purchaser UserName",
                                "Purchser Name",
                                "Supdt UserName",
                                "Supdt Name",
                                "Invoice PIC",
                                "Present Status"
                              };

        string[] alColumns = {
                                "FLDCODE",
                                "FLDSUPPLIERNAME",
                                "FLDPOLIST",
                                "FLDINVOICESUPPLIERREFERENCE",
                                "FLDINVOICERECEIVEDDATE",
                                "FLDINVOICEAMOUNT",
                                "FLDVESSELLIST",
                                "FLDCURRENCYNAME",
                                "FLDINVOICESTATUSNAME",
                                "FLDINVOICENUMBER",
                                "FLDINVOICEDATE",
                                "FLDMONTHNAME",
                                "FLDINVOICETYPENAME",
                                "FLDREMARKS",
                                "FLDPURCHASERUSERNAME",
                                "FLDPURCHASERNAME",
                                "FLDPURCHASESUPDTUSERNAME",
                                "FLDPURCHASESUPDTNAME",
                                "FLDPICUSERNAME",
                                "FLDREASONFORHOLD"
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


        ds = PhoenixAccountsInvoice.InvoiceRegisterSearchForExcel(
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
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("chkPriorityInv")) : null
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ucPIC")) : null
                                                    , nvc != null ? General.GetNullableString(nvc.Get("invoiceStatusList")) : null
                                                    , sortexpression, sortdirection
                                                    , 1
                                                    , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                    , ref iRowCount, ref iTotalPageCount
                                                    , nvc != null ? General.GetNullableString(nvc.Get("chkPortList")) : null);
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
        if (Filter.InvRegAdvFilterYN != null && Filter.InvRegAdvFilterYN.ToString() == "Y")
        {
            ds = PhoenixAccountsInvoice.InvoiceRegisterSearch(
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
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("chkPriorityInv")) : null
                                                    , nvc != null ? General.GetNullableInteger(nvc.Get("ucPIC")) : null
                                                    , nvc != null ? General.GetNullableString(nvc.Get("invoiceStatusList")) : null
                                                    , sortexpression, sortdirection
                                                    , (int)ViewState["PAGENUMBER"], gvInvoice.PageSize
                                                    , ref iRowCount, ref iTotalPageCount
                                                    , nvc != null ? General.GetNullableString(nvc.Get("chkPortList")) : null);
        }
        else
        {
            ds = PhoenixAccountsInvoice.InvoiceRegisterDefaultSearch(
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
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "Key", "<script>FreezeGridViewHeader('gvInvoice', 300, false);</script>", false);
            if (ViewState["invoicecode"] == null)
            {
                ViewState["invoicecode"] = ds.Tables[0].Rows[0]["FLDINVOICECODE"].ToString();
                //gvInvoice.SelectedIndex = 0;
            }

            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoice.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString();
            SetRowSelection();
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoice.aspx?qinvoicecode=";
            DataTable dt = ds.Tables[0];
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
                //PhoenixAccountsVoucher.VoucherNumber = ((Label)gvInvoice.Rows[i].FindControl("lblInvoiceid")).Text;
                PhoenixAccountsVoucher.InvoiceNumber = ((RadLabel)gvInvoice.Items[item.ItemIndex].FindControl("lblInvoiceid")).Text;
            }
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

    protected void gvInvoice_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridHeaderItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Item is GridDataItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

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

            //if (lblStatus != null && lblStatus.Text.Length > 0)
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
            ImageButton imbPriorityInvoice = (ImageButton)e.Item.FindControl("imbPriorityInvoice");

            if (lblPrioritySupplier != null)
            {
                if (!string.IsNullOrEmpty(lblPrioritySupplier.Text) && int.Parse(lblPrioritySupplier.Text) == 1 && imbPriorityInvoice != null)
                    imbPriorityInvoice.Visible = true;
            }
            if (imbSupplierMismatch != null)
                if (!SessionUtil.CanAccess(this.ViewState, imbSupplierMismatch.CommandName)) imbSupplierMismatch.Visible = false;
            if (imbPriorityInvoice != null)
                if (!SessionUtil.CanAccess(this.ViewState, imbPriorityInvoice.CommandName)) imbPriorityInvoice.Visible = false;

            RadTextBox txtInvoiceCode = (RadTextBox)e.Item.FindControl("txtInvoiceCode");
            ImageButton cmdMoreInfo = (ImageButton)e.Item.FindControl("cmdMoreInfo");
            if (cmdMoreInfo != null)
            {
                cmdMoreInfo.Attributes.Add("onclick", "openNewWindow('codeHelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsInvoiceMoreInfo.aspx?invoiceCode=" + txtInvoiceCode.Text + "');return false;");

            }

            //int i = e.Row.RowIndex + 1;
            //LinkButton lnkInvoice = (LinkButton)e.Row.FindControl("lnkInvoice");
            //if (lnkInvoice != null)
            //{

            //    lnkInvoice.Attributes.Add("onclick", "SetIframe('" + e.Row.ClientID + "','" + txtInvoiceCode.Text + "','" + i + "'); return false;");
            //}

            //LinkButton lnkInvoiceRef = (LinkButton)e.Row.FindControl("lnkInvoiceRef");
            //if (lnkInvoiceRef != null)
            //{
            //    lnkInvoiceRef.Attributes.Add("onclick", "SetIframe('" + e.Row.ClientID + "','" + txtInvoiceCode.Text + "','" + i + "'); return false;");

            //}

            RadLabel lblInvoiceType = (RadLabel)e.Item.FindControl("lblInvoiceType");
            if (lblInvoiceType != null)
            {
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipType");
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblInvoiceType.ClientID;
            }

            //RadLabel lblInvoiceType = (RadLabel)e.Item.FindControl("lblInvoiceType");
            //UserControlToolTip ucToolTipType = (UserControlToolTip)e.Item.FindControl("ucToolTipType");
            //if (lblInvoiceType != null && ucToolTipType != null)
            //{
            //    lblInvoiceType.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipType.ToolTip + "', 'visible');");
            //    lblInvoiceType.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipType.ToolTip + "', 'hidden');");
            //}


            RadLabel lblInvoiceStatus = (RadLabel)e.Item.FindControl("lblInvoiceStatus");
            if (lblInvoiceStatus != null)
            {
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipStatus");
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblInvoiceStatus.ClientID;
            }

            RadLabel lblSupplierName = (RadLabel)e.Item.FindControl("lblSupplierName");
            if (lblSupplierName != null)
            {
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipSupplier");
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblSupplierName.ClientID;
            }

        }

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (e.Row.DataItem != null)
        //    {                
        //        ImageButton img1 = (ImageButton)e.Row.FindControl("imgAttachment");
        //        Label lblControl = (Label)e.Row.Cells[2].FindControl("lblAttachmentexists");
        //        if (lblControl.Text == "1")
        //        {
        //            img1.Visible = true;
        //        }
        //        else { img1.Visible = false; }                
        //    }
        //}
        // if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit && e.Row.RowState != (DataControlRowState.Alternate | DataControlRowState.Edit))
        // {
        //     //LinkButton _doubleClickButton = (LinkButton)e.Row.FindControl("lnkDoubleClick");
        //     //string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
        // }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }


    protected void gvInvoice_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            int iRowno;
            iRowno = e.Item.ItemIndex;
            BindPageURL(iRowno);
            Rebind();
        }
        if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
        {
            Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode=" + ((RadTextBox)e.Item.FindControl("txtInvoiceCode")).Text + "&qfrom=INVOICE");
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["invoicecode"] = null;
        BindData();
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
            RadTextBox tb = ((RadTextBox)gvInvoice.Items[rowindex].FindControl("txtInvoiceCode"));
            if (tb != null)
                ViewState["invoicecode"] = tb.Text;
            RadLabel lbl = ((RadLabel)gvInvoice.Items[rowindex].FindControl("lblInvoiceid"));
            if (lbl != null)
                PhoenixAccountsVoucher.InvoiceNumber = lbl.Text;
            if (ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoice.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString();
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
    protected void gvInvoice_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvInvoice.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
        //ViewState["invoicecode"] = gvInvoice.DataKeys[e.NewSelectedIndex].Value.ToString();
        Rebind();
    }
}
