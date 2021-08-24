using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Reports;
using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsInvoicePaymentVoucherMaster : PhoenixBasePage
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
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsInvoicePaymentVoucherMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVoucherDetails')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsInvoicePaymentVoucherFilter.aspx?source=paymentvoucher", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsInvoicePaymentVoucherMaster.aspx", "Payment Voucher Invoice reference Export to Excel", "icon_xls.png", "EXCEL1");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //MenuOrderForm.SetTrigger(pnlOrderForm);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Line Items", "LINEITEM", ToolBarDirection.Right);
            toolbarmain.AddButton("Voucher", "VOUCHER", ToolBarDirection.Right);

            // MenuOrderFormMain.SetTrigger(pnlOrderForm);
            gvVoucherDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            if (!IsPostBack)
            {
                Session["New"] = "N";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["voucherid"] = null;
                ViewState["vouchertype"] = null;
                ViewState["vouchersubtype"] = null;
                ViewState["PAGEURL"] = null;
                gvVoucherDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (Request.QueryString["voucherid"] != null)
                {
                    ViewState["voucherid"] = Request.QueryString["voucherid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoicePaymentVoucher.aspx?voucherid=" + ViewState["voucherid"].ToString();
                    DataSet ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherEdit(ViewState["voucherid"].ToString());
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["vouchertype"] = dr["FLDTYPE"].ToString();
                    ViewState["vouchersubtype"] = dr["FLDSUBTYPE"].ToString();
                }
            }

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();

            MenuOrderFormMain.SelectedMenuIndex = 1;
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
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoicePaymentVoucher.aspx?voucherid=" + ViewState["voucherid"];
            }

            string VoucherType;
            string VoucherSubType;
            if (ViewState["vouchertype"] != null && ViewState["vouchertype"].ToString() != string.Empty)
            {
                VoucherType = ViewState["vouchertype"].ToString();
                VoucherSubType = ViewState["vouchersubtype"].ToString();
            }
            else
            {
                VoucherType = "";
                VoucherSubType = "";
            }

            if (CommandName.ToUpper().Equals("LINEITEM") && ViewState["voucherid"] != null && ViewState["voucherid"].ToString() != string.Empty)
            {
                if (ViewState["vouchertype"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 221, "INV")
                        && ViewState["vouchersubtype"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 213, "AVN"))
                {
                    Response.Redirect("../Accounts/AccountsAirfarePaymentVoucherDetails.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&type=" + ViewState["vouchertype"].ToString());
                }
                if (VoucherType == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 221, "CTM"))
                {
                    Response.Redirect("../Accounts/AccountsCtmPaymentVoucherLineItemDetails.aspx?voucherid=" + ViewState["voucherid"].ToString());
                }
                else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "ALT") && VoucherSubType == PhoenixCommonRegisters.GetHardCode(1, 213, "RMA"))
                {
                    Response.Redirect("../Accounts/AccountsAllotmentPVLineItemDetails.aspx?voucherid=" + ViewState["voucherid"]);
                }
                else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "APT") && VoucherSubType == PhoenixCommonRegisters.GetHardCode(1, 124, "DEP"))
                {
                    Response.Redirect("../Accounts/AccountsPaymentVoucherDeposit.aspx?voucherid=" + ViewState["voucherid"]);
                }
                else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "APT"))
                {
                    Response.Redirect("../Accounts/AccountsPaymentVoucherAdvanceRequests.aspx?voucherid=" + ViewState["voucherid"]);
                }
                else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "ALT"))
                {
                    Response.Redirect("../Accounts/AccountsPaymentVoucherAllotmentRequest.aspx?voucherid=" + ViewState["voucherid"]);
                }
                else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "TCL"))
                {
                    Response.Redirect("../Accounts/AccountsPaymentVoucherVesselVisitTravelClaim.aspx?voucherid=" + ViewState["voucherid"]);
                }
                else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "EAD"))
                {
                    Response.Redirect("../Accounts/AccountsPaymentVoucherVesselVisitTravelAdvance.aspx?voucherid=" + ViewState["voucherid"]);
                }
                else
                    Response.Redirect("../Accounts/AccountsInvoicePaymentVoucherLineItemDetails.aspx?voucherid=" + ViewState["voucherid"].ToString());

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

        string[] alCaptions = { "Voucher Number", "Voucher Date", "Payee", "Currency", "Amount", "Fleet", "Status", "Remittance Request Id" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDPAYMENTVOUCHERFLEET", "FLDPAYMENTVOUCHERSTATUS", "FLDREMITTANCENUMBER" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (Filter.CurrentInvoicePaymentVoucherSelection != null)
        {
            string payee;
            string type;

            NameValueCollection nvc = Filter.CurrentInvoicePaymentVoucherSelection;
            payee = nvc.Get("txtMakerId").ToString().Trim();
            type = nvc.Get("ddlType").ToString().Trim();

            if ((nvc.Get("txtuserid").ToString() != null) && (nvc.Get("txtuserid").ToString().Trim().Length > 0))  // For traveladvance bug id - 23164
            {
                payee = nvc.Get("txtuserid").ToString().Trim();
                type = "1588";
            }

            ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherSearch(General.GetNullableString(nvc.Get("txtVoucherNumberSearch").ToString().Trim())
                    , General.GetNullableInteger(payee)
                    , General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim())
                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                    , General.GetNullableInteger(nvc.Get("ddlVoucherStatus").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("chkShowRemittancenotGenerated").ToString())
                    , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherFromdateSearch")) : null
                    , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherTodateSearch")) : null
                    , General.GetNullableInteger(nvc.Get("ucTechFleet").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtInvoiceNumber").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtPurchaseInvoiceVoucherNumber").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("chkReportNotTaken").ToString())
                    , General.GetNullableInteger(nvc.Get("ddlSource").ToString().Trim())
                    , General.GetNullableInteger(type)
                    , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);

        }
        else
        {
            ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherSearch("", null, null
                                            , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                            , null
                                            , null
                                            , string.Empty
                                            , string.Empty
                                            , null
                                            , null
                                            , null
                                            , null
                                            , null
                                            , null
                                            , sortexpression, sortdirection
                                            , (int)ViewState["PAGENUMBER"]
                                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                            , ref iRowCount, ref iTotalPageCount);
        }

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

    protected void ShowExcelAdvance()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Supplier Name", "Invoice Reference No", "Payment Currency", "Payment Amount", "Booking MS.No", "PMV Number", "Banking Details" };
        string[] alColumns = { "FLDNAME", "FLDINVOICESUPPLIERREFERENCE", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDPURCHASEINVOICEVOUCHERNUMBER", "FLDPAYMENTVOUCHERNUMBER", "FLDBANKDETAILS" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (Filter.CurrentInvoicePaymentVoucherSelection != null)
        {
            string payee;
            string type;

            NameValueCollection nvc = Filter.CurrentInvoicePaymentVoucherSelection;
            payee = nvc.Get("txtMakerId").ToString().Trim();
            type = nvc.Get("ddlType").ToString().Trim();

            if ((nvc.Get("txtuserid").ToString() != null) && (nvc.Get("txtuserid").ToString().Trim().Length > 0))  // For traveladvance bug id - 23164
            {
                payee = nvc.Get("txtuserid").ToString().Trim();
                type = "1588";
            }

            ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherExcel(General.GetNullableString(nvc.Get("txtVoucherNumberSearch").ToString().Trim())
                    , General.GetNullableInteger(payee)
                    , General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim())
                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                    , General.GetNullableInteger(nvc.Get("ddlVoucherStatus").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("chkShowRemittancenotGenerated").ToString())
                    , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherFromdateSearch")) : null
                    , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherTodateSearch")) : null
                    , General.GetNullableInteger(nvc.Get("ucTechFleet").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtInvoiceNumber").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtPurchaseInvoiceVoucherNumber").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("chkReportNotTaken").ToString())
                    , General.GetNullableInteger(nvc.Get("ddlSource").ToString().Trim())
                    , General.GetNullableInteger(type)
                    , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);

        }
        else
        {
            ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherExcel("", null, null
                                            , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                            , null
                                            , null
                                            , string.Empty
                                            , string.Empty
                                            , null
                                            , null
                                            , null
                                            , null
                                            , null
                                            , null
                                            , sortexpression, sortdirection
                                            , (int)ViewState["PAGENUMBER"]
                                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                            , ref iRowCount, ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=InvoicePaymentVoucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>PaymentVoucher Invoice Reference</h3></td>");
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
            if (CommandName.ToUpper().Equals("EXCEL1"))
            {
                ShowExcelAdvance();
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

        if (Filter.CurrentInvoicePaymentVoucherSelection != null)
        {
            string payee;
            string type;
            NameValueCollection nvc = Filter.CurrentInvoicePaymentVoucherSelection;
            payee = nvc.Get("txtMakerId").ToString().Trim();
            type = nvc.Get("ddlType").ToString().Trim();

            if ((nvc.Get("txtuserid").ToString() != null) && (nvc.Get("txtuserid").ToString().Trim().Length > 0))  // For traveladvance bug id - 23164
            {
                payee = nvc.Get("txtuserid").ToString().Trim();
                type = "1588";
            }
            ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherSearch(General.GetNullableString(nvc.Get("txtVoucherNumberSearch").ToString().Trim())
        , General.GetNullableInteger(payee)
        , General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim())
        , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
        , General.GetNullableInteger(nvc.Get("ddlVoucherStatus").ToString().Trim())
        , General.GetNullableInteger(nvc.Get("chkShowRemittancenotGenerated").ToString())
        , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherFromdateSearch")) : null
        , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherTodateSearch")) : null
        , General.GetNullableInteger(nvc.Get("ucTechFleet").ToString().Trim())
        , General.GetNullableString(nvc.Get("txtInvoiceNumber").ToString().Trim())
        , General.GetNullableString(nvc.Get("txtPurchaseInvoiceVoucherNumber").ToString().Trim())
        , General.GetNullableInteger(nvc.Get("chkReportNotTaken").ToString())
        , General.GetNullableInteger(nvc.Get("ddlSource").ToString().Trim())
        , General.GetNullableInteger(type)
        , sortexpression, sortdirection
        , (int)ViewState["PAGENUMBER"]
        , gvVoucherDetails.PageSize
        , ref iRowCount
        , ref iTotalPageCount
        , General.GetNullableString(nvc.Get("txtSupplierReferenceSearch").ToString().Trim()));

        }
        else
        {
            ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherSearch(General.GetNullableString(""), null, null
                                            , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                            , null
                                            , null
                                            , null
                                            , null
                                            , null
                                            , null
                                            , null
                                            , null
                                            , null
                                            , null
                                            , sortexpression
                                            , sortdirection
                                            , (int)ViewState["PAGENUMBER"]
                                            , gvVoucherDetails.PageSize
                                            , ref iRowCount, ref iTotalPageCount
                                            , null);
        }


        string[] alCaptions = { "Voucher Number", "Voucher Date", "Payee", "Currency", "Amount", "Fleet", "Status", "Remittance Request Id" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDPAYMENTVOUCHERFLEET", "FLDPAYMENTVOUCHERSTATUS", "FLDREMITTANCENUMBER" };

        General.SetPrintOptions("gvVoucherDetails", "Accounts Voucher", alCaptions, alColumns, ds);

        gvVoucherDetails.DataSource = ds;
        gvVoucherDetails.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["voucherid"] == null)
            {
                ViewState["voucherid"] = ds.Tables[0].Rows[0]["FLDPAYMENTVOUCHERID"].ToString();
            }
            if (ViewState["vouchertype"] == null)
            {
                ViewState["vouchertype"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
            }
            if (ViewState["vouchersubtype"] == null)
            {
                ViewState["vouchersubtype"] = ds.Tables[0].Rows[0]["FLDSUBTYPE"].ToString();
            }
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoicePaymentVoucher.aspx?voucherid=" + ViewState["voucherid"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoicePaymentVoucher.aspx?voucherid=";
            }
            SetRowSelection();
        }

        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        gvVoucherDetails.Columns[3].Visible = (showcreditnotedisc == 1) ? true : false;
        gvVoucherDetails.Columns[4].Visible = (showcreditnotedisc == 1) ? true : false;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void SetRowSelection()
    {
        foreach (GridDataItem item in gvVoucherDetails.Items)
        {

            if (item.GetDataKeyValue("FLDPAYMENTVOUCHERID").ToString().Equals(ViewState["voucherid"].ToString()))
            {
                gvVoucherDetails.SelectedIndexes.Add(item.ItemIndex);
                PhoenixAccountsInvoicePaymentVoucher.InvoiceVoucherNumber = ((LinkButton)gvVoucherDetails.Items[item.ItemIndex].FindControl("lnkVoucherId")).Text;
            }
        }
    }

    protected void gvVoucherDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton cmdApprove = (ImageButton)e.Item.FindControl("cmdApprove");
            if (cmdApprove != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName)) cmdApprove.Visible = false;
        }

        if (e.Item is GridDataItem)
        {

            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
                && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {

                ImageButton app = (ImageButton)e.Item.FindControl("cmdApprove");
                LinkButton lnkVoucherId = (LinkButton)e.Item.FindControl("lnkVoucherId");
                RadLabel lblVoucherType = (RadLabel)e.Item.FindControl("lblVoucherType");
                RadLabel lblSuppCode = (RadLabel)e.Item.FindControl("lblSuppCode");
                RadLabel lblStatusCode = (RadLabel)e.Item.FindControl("lblStatusCode");
                if (app != null)
                    if (!SessionUtil.CanAccess(this.ViewState, app.CommandName)) app.Visible = false;
                if (lnkVoucherId != null)
                    if (!SessionUtil.CanAccess(this.ViewState, lnkVoucherId.CommandName)) lnkVoucherId.Visible = false;
                if (lblStatusCode != null && lblStatusCode.Text != "" && int.Parse(lblStatusCode.Text) == 48)
                {
                    if (app != null)
                        app.Enabled = false;
                }
                if (app != null)
                {
                    if (lblVoucherType != null && lblVoucherType.Text != "")
                    {
                        //VoucherType is 0 for payment vouchers (supplier type invoice = 239) other wise 1
                        string vouchertype = lblVoucherType.Text == "239" ? "0" : "1";
                        app.Attributes.Add("onclick", "openNewWindow('PaymentVoucherApproval', '', '" + Session["sitepath"] + "/Common/CommonApproval.aspx?docid=" + lnkVoucherId.CommandArgument + "&mod=" + PhoenixModule.ACCOUNTS + "&type=" + 459 + "&suppliercode=" + lblSuppCode.Text + "&vouchertype=" + vouchertype + "&user=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "');return false;");
                    }
                }
                else
                {
                    app.Visible = false;
                }
                ImageButton imbApprovalHistory = (ImageButton)e.Item.FindControl("imbApprovalHistory");
                if (imbApprovalHistory != null && lnkVoucherId != null)
                {
                    imbApprovalHistory.Attributes.Add("onclick", "openNewWindow('PaymentVoucherApproval', '', '" + Session["sitepath"] + "/Accounts/AccountsInvoicePaymentVoucherApprovalRevokalHistory.aspx?voucherid=" + lnkVoucherId.CommandArgument + "&vouchernumber=" + lnkVoucherId.Text + "');return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, imbApprovalHistory.CommandName)) imbApprovalHistory.Visible = false;
                }

                ImageButton imgReportHistory = (ImageButton)e.Item.FindControl("imgReportHistory");
                RadLabel lblReporttakencount = (RadLabel)e.Item.FindControl("lblReporttakencount");

                if (lblReporttakencount.Text == "0")
                {
                    imgReportHistory.Visible = false;
                }
                else
                {
                    imgReportHistory.Attributes.Add("onclick", "openNewWindow('PaymentVoucherReport', '', '" + Session["sitepath"] + "/Accounts/AccountsPurchaseInvoiceReportViewHistory.aspx?voucherid=" + lnkVoucherId.CommandArgument + "&vouchernumber=" + lnkVoucherId.Text + "');return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, imgReportHistory.CommandName)) imgReportHistory.Visible = false;
                }

                RadLabel lblTypesandSource = (RadLabel)e.Item.FindControl("lblTypesandSource");
                UserControlToolTip ucToolTipTypesandSource = (UserControlToolTip)e.Item.FindControl("ucToolTipTypesandSource");

                if (lblTypesandSource != null)
                {
                    lblTypesandSource.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipTypesandSource.ToolTip + "', 'visible');");
                    lblTypesandSource.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipTypesandSource.ToolTip + "', 'hidden');");
                }

                RadLabel lblStatusShortName = (RadLabel)e.Item.FindControl("lblStatusShortName");
                UserControlToolTip ucToolTipStatus = (UserControlToolTip)e.Item.FindControl("ucToolTipStatus");

                if (lblStatusShortName != null)
                {
                    lblStatusShortName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipStatus.ToolTip + "', 'visible');");
                    lblStatusShortName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipStatus.ToolTip + "', 'hidden');");
                }
            }
        }
    }

    protected void gvVoucherDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            BindPageURL(e.Item.ItemIndex);
        }
        if (e.CommandName.ToUpper().Equals("RECALCULATE"))
        {
            LinkButton lblVoucherLineId = ((LinkButton)e.Item.FindControl("lnkVoucherId"));
            ViewState["voucherid"] = lblVoucherLineId.CommandArgument;
            PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherRecalculate(General.GetNullableGuid(ViewState["voucherid"].ToString()));
            Rebind();
        }
        if (e.CommandName.ToUpper().Equals("GENERATE"))
        {
            // int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            LinkButton lblVoucherLineId = ((LinkButton)e.Item.FindControl("lnkVoucherId"));
            ViewState["voucherid"] = lblVoucherLineId.CommandArgument;
            //if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            //{
            //    GenerateSSRSPDF();
            //}
            //else
            //{
            GeneratePDF();
            //}
        }

        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            BindPageURL(e.Item.ItemIndex);
            SetRowSelection();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvVoucherDetails.Rebind();
    }


    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["voucherid"] = ((LinkButton)gvVoucherDetails.Items[rowindex].FindControl("lnkVoucherId")).CommandArgument;
            ViewState["vouchertype"] = ((RadLabel)gvVoucherDetails.Items[rowindex].FindControl("lblType")).Text;
            ViewState["vouchersubtype"] = ((RadLabel)gvVoucherDetails.Items[rowindex].FindControl("lblSubType")).Text;
            PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvVoucherDetails.Items[rowindex].FindControl("lnkVoucherId")).Text;
            string type = ((RadLabel)gvVoucherDetails.Items[rowindex].FindControl("lblVoucherTypes")).Text;
            string source = ((RadLabel)gvVoucherDetails.Items[rowindex].FindControl("lblVoucherSource")).Text;

            if (ViewState["vouchertype"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 221, "INV")
                && ViewState["vouchersubtype"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 213, "AVN"))
            {
                Response.Redirect("../Accounts/AccountsAirfarePaymentVoucherDetails.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&type=" + ViewState["vouchertype"].ToString());
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoicePaymentVoucher.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&type=" + ViewState["vouchertype"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoucherDetails_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        ViewState["voucherid"] = ((LinkButton)gvVoucherDetails.Items[e.NewSelectedIndex].FindControl("lnkVoucherId")).CommandArgument;
        ViewState["vouchertype"] = ((RadLabel)gvVoucherDetails.Items[e.NewSelectedIndex].FindControl("lblType")).Text;
        ViewState["vouchersubtype"] = ((RadLabel)gvVoucherDetails.Items[e.NewSelectedIndex].FindControl("lblSubType")).Text;
        BindPageURL(e.NewSelectedIndex);
    }


    private void GeneartePaymentVoucherPDF()
    {
        string VoucherType;
        string VoucherSubType;
        if (ViewState["vouchertype"] != null && ViewState["vouchertype"].ToString() != string.Empty)
        {
            VoucherType = ViewState["vouchertype"].ToString();
            VoucherSubType = ViewState["vouchersubtype"].ToString();
        }
        else
        {
            VoucherType = "";
            VoucherSubType = "";
        }
        if (ViewState["vouchertype"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 221, "INV")
                        && ViewState["vouchersubtype"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 213, "AVN"))
        {
            //Response.Redirect("../Accounts/AccountsAirfarePaymentVoucherDetails.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&type=" + ViewState["vouchertype"].ToString());
            Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?paymentvoucherinvoiceid=" + ViewState["voucherid"] + "&applicationcode=5&reportcode=AIRFAREPAYMENTVOUCHER&showexcel=no", false);
        }
        if (VoucherType == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 221, "CTM"))
        {
            string voucherid = (ViewState["voucherid"] == null) ? null : (ViewState["voucherid"].ToString());
            PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherReportviewDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["voucherid"].ToString());
            Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?voucherid=" + voucherid
                                        + "&vesselid=null&applicationcode=5&reportcode=CTMPAYMENTVOUCHER&showexcel=no", false);
            //Response.Redirect("../Accounts/AccountsCtmPaymentVoucherLineItemDetails.aspx?voucherid=" + ViewState["voucherid"].ToString());
        }
        else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "APT") && VoucherSubType == PhoenixCommonRegisters.GetHardCode(1, 124, "DEP"))
        {

            //string voucherid = (ViewState["voucherid"] == null) ? null : (ViewState["voucherid"].ToString());

            //PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherReportviewDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["voucherid"].ToString());

            //Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?&voucherid=" + voucherid +
            //                           "&applicationcode=5&reportcode=DEPOSITPAYMENTVOUCHER&showexcel=no", false);
            ////Response.Redirect("../Accounts/AccountsPaymentVoucherDeposit.aspx?voucherid=" + ViewState["voucherid"]);
        }
        else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "APT"))
        {
            //Response.Redirect("../Accounts/AccountsPaymentVoucherAdvanceRequests.aspx?voucherid=" + ViewState["voucherid"]);
            string voucherid = (ViewState["voucherid"] == null) ? null : (ViewState["voucherid"].ToString());

            PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherReportviewDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["voucherid"].ToString());

            Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?&voucherid=" + voucherid +
                                       "&applicationcode=5&reportcode=ADVANCEPAYMENTVOUCHER&showexcel=no", false);
        }
        //else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "ALT"))
        //{
        //    Response.Redirect("../Accounts/AccountsPaymentVoucherAllotmentRequest.aspx?voucherid=" + ViewState["voucherid"]);
        //}
        //else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "TCL"))
        //{
        //    Response.Redirect("../Accounts/AccountsPaymentVoucherVesselVisitTravelClaim.aspx?voucherid=" + ViewState["voucherid"]);
        //}
        else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "EAD"))
        {
            //Response.Redirect("../Accounts/AccountsPaymentVoucherVesselVisitTravelAdvance.aspx?voucherid=" + ViewState["voucherid"]);
            PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherReportviewDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["voucherid"].ToString());
            Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?paymentvoucherinvoiceid=" + ViewState["voucherid"] + "&applicationcode=5&reportcode=TRAVELADVANCEPAYMENTVOUCHER&showexcel=no", false);
        }
        else
        {
            //Response.Redirect("../Accounts/AccountsInvoicePaymentVoucherLineItemDetails.aspx?voucherid=" + ViewState["voucherid"].ToString());
            string voucherid = (ViewState["voucherid"] == null) ? null : (ViewState["voucherid"].ToString());

            PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherReportviewDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["voucherid"].ToString());

            Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?paymentvoucherinvoiceid=" + null
                                        + "&voucherid=" + voucherid
                                        + "&invoicecode=" + null
                                        + "&orderid=" + null
                                        + "&vesselid=null&applicationcode=5&reportcode=INVOICEPAYMENTVOUCHER&showexcel=no", false);
        }
    }





    protected void GeneratePDF()
    {
        string[] _reportfile = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        string _filename = "";
        DataSet ds = new DataSet();

        NameValueCollection criteria = new NameValueCollection();

        criteria.Add("applicationcode", "5");

        _reportfile = new string[6];

        string VoucherType;
        string VoucherSubType;

        VoucherType = "";
        VoucherSubType = "";

        DataSet dsD = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherEdit(ViewState["voucherid"].ToString());
        if (dsD.Tables.Count > 0)
        {
            if (dsD.Tables[0].Rows.Count > 0)
            {
                VoucherType = dsD.Tables[0].Rows[0]["FLDTYPE"].ToString();
                VoucherSubType = dsD.Tables[0].Rows[0]["FLDSUBTYPE"].ToString();
            }
        }


        if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "ALT") || VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "TCL"))
        {
            ucError.ErrorMessage = "There is no report for this Payment type";
            ucError.Visible = true;
            return;
        }

        if (VoucherType == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 221, "INV")
                         && VoucherSubType == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 213, "AVN"))
        {
            _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/AirfarePaymentVoucher_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");
            criteria.Add("reportcode", "AIRFAREPAYMENTVOUCHER");
            _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsAirfarePaymentVoucherGenerate.rpt");
            _reportfile[1] = "ReportsAccountsAirfarePaymentVoucherVessel.rpt";
            _reportfile[2] = "ReportsAccountsAirfarePaymentVoucherInvoice.rpt";
        }
        else if (VoucherType == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 221, "CTM"))
        {
            _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/CtmPaymentVoucher_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");
            PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherReportviewDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["voucherid"].ToString());
            criteria.Add("reportcode", "CTMPAYMENTVOUCHER");
            _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsCtmPaymentVoucher.rpt");
            _reportfile[1] = "ReportsAccountsCtmPaymentVoucherHeader.rpt";
        }
        else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "EAD"))
        {
            _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/TravelAdvancePaymentVoucher_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");
            PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherReportviewDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["voucherid"].ToString());
            criteria.Add("reportcode", "TRAVELADVANCEPAYMENTVOUCHER");
            _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportAccountsTravelAdvancePaymentVoucherLineitem.rpt");
            _reportfile[1] = "ReportAccountsTravelAdvancePaymentVoucher.rpt";
            _reportfile[2] = "ReportAccountsTravelAdvanceApprovalHistory.rpt";
            _reportfile[3] = "ReportAccountsTravelAdvancePaymentVoucherReportViewHistory.rpt";
        }
        else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "APT"))
        {
            if (VoucherSubType == PhoenixCommonRegisters.GetHardCode(1, 124, "DEP"))
            {
                _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/DepositPaymenVoucher.pdf");
                PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherReportviewDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["voucherid"].ToString());
                criteria.Add("reportcode", "DEPOSITPAYMENTVOUCHER");
                _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsDepositPaymentVoucher.rpt");
                _reportfile[1] = "ReportsAccountsDepositPaymentApproval.rpt";
                _reportfile[2] = "ReportsAccountsDepositPaymentHistory.rpt";
            }
            else
            {
                _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/AdvancePaymenVoucher.pdf");
                PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherReportviewDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["voucherid"].ToString());
                criteria.Add("reportcode", "ADVANCEPAYMENTVOUCHER");
                _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsAdvancePaymentVoucher.rpt");
                _reportfile[1] = "ReportsAccountsAdvancePaymentApproval.rpt";
                _reportfile[2] = "ReportsAccountsAdvancePaymentHistory.rpt";
            }
        }
        else
        {
            criteria.Add("reportcode", "INVOICEPAYMENTVOUCHER");
            _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/InvoicePaymentVoucher_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");
            PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherReportviewDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["voucherid"].ToString());
            _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsInvoicePaymentVoucher.rpt");
            _reportfile[1] = "ReportsAccountsInvoicePaymentVoucherHeader.rpt";
            _reportfile[2] = "ReportsAccountsInvoicePaymentVoucherAdvancePayment.rpt";
            _reportfile[3] = "ReportsAccountsInvoicePaymentVoucherCreditNotes.rpt";
            _reportfile[4] = "ReportsAccountsInvoicePaymentVoucherPaymentApproval.rpt";
            _reportfile[5] = "ReportsAccountsInvoicePaymentVoucherViewHistory.rpt";

        }

        string invoiceid = (ViewState["INVOICEID"] == null) ? null : (ViewState["INVOICEID"].ToString());
        string paymentvoucherinvoiceid = (ViewState["PAYMENTVOUCHERINVOICEID"] == null) ? null : (ViewState["PAYMENTVOUCHERINVOICEID"].ToString());
        string voucherid = (ViewState["voucherid"] == null) ? null : (ViewState["voucherid"].ToString());

        criteria.Add("voucherid", General.GetNullableGuid(voucherid).ToString());
        if ((VoucherType == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 221, "INV")
                      && VoucherSubType == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 213, "AVN")) || (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "EAD")))
            criteria.Add("paymentvoucherinvoiceid", General.GetNullableGuid(voucherid).ToString());
        else
            criteria.Add("paymentvoucherinvoiceid", "");
        criteria.Add("invoicecode", "");
        criteria.Add("orderid", "");
        criteria.Add("vesselid", "");

        Session["PHOENIXREPORTPARAMETERS"] = criteria;

        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        ds = PhoenixReportsCommon.GetReportAndSubReportData(nvc, ref _reportfile, ref _filename);
        if (ds.Tables.Count > 0)
        {
            if (VoucherType == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 221, "INV")
                      && VoucherSubType == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 213, "AVN"))
            {
                if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                {
                    _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/AirfarePaymentVoucher_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");
                    GenerateSSRSPDF(_filename, ds, "AIRFAREPAYMENTVOUCHER");
                }
                else
                {
                    _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/AirfarePaymentVoucher_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");
                    _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsAirfarePaymentVoucherGenerate.rpt");
                    _reportfile[1] = "ReportsAccountsAirfarePaymentVoucherVessel.rpt";
                    _reportfile[2] = "ReportsAccountsAirfarePaymentVoucherInvoice.rpt";
                }
            }
            else if (VoucherType == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 221, "CTM"))
            {
                if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                {
                    _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/CtmPaymentVoucher_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");
                    GenerateSSRSPDF(_filename, ds, "CTMPAYMENTVOUCHER");
                }
                else
                {
                    _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/CtmPaymentVoucher_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");
                    _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsCtmPaymentVoucher.rpt");
                    _reportfile[1] = "ReportsAccountsCtmPaymentVoucherHeader.rpt";
                }
            }
            else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "APT"))
            {
                if (VoucherSubType == PhoenixCommonRegisters.GetHardCode(1, 124, "DEP"))
                {
                    if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                    {
                        _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/DepositPaymenVoucher.pdf");
                        GenerateSSRSPDF(_filename, ds, "DEPOSITPAYMENTVOUCHER");
                    }
                    else
                    {
                        _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/DepositPaymenVoucher.pdf");
                        _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsDepositPaymentVoucher.rpt");
                        _reportfile[1] = "ReportsAccountsDepositPaymentApproval.rpt";
                        _reportfile[2] = "ReportsAccountsDepositPaymentHistory.rpt";
                    }
                }
                else
                {
                    if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                    {
                        _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/AdvancePaymenVoucher.pdf");
                        GenerateSSRSPDF(_filename, ds, "ADVANCEPAYMENTVOUCHER");
                    }
                    else
                    {
                        _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/AdvancePaymenVoucher.pdf");
                        _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsAdvancePaymentVoucher.rpt");
                        _reportfile[1] = "ReportsAccountsAdvancePaymentApproval.rpt";
                        _reportfile[2] = "ReportsAccountsAdvancePaymentHistory.rpt";
                    }
                }
            }
            else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "EAD"))
            {
                if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                {
                    _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/TravelAdvancePaymentVoucher_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");
                    GenerateSSRSPDF(_filename, ds, "TRAVELADVANCEPAYMENTVOUCHER");
                }
                else
                {
                    _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/TravelAdvancePaymentVoucher_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");
                    _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportAccountsTravelAdvancePaymentVoucherLineitem.rpt");
                    _reportfile[1] = "ReportAccountsTravelAdvancePaymentVoucher.rpt";
                    _reportfile[2] = "ReportAccountsTravelAdvanceApprovalHistory.rpt";
                    _reportfile[3] = "ReportAccountsTravelAdvancePaymentVoucherReportViewHistory.rpt";
                }
            }
            else if (VoucherSubType == PhoenixCommonRegisters.GetHardCode(1, 124, "DEP"))
            {

            }
            else
            {
                if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                {
                    _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/InvoicePaymentVoucher_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");
                    GenerateSSRSPDF(_filename, ds, "INVOICEPAYMENTVOUCHER");
                }
                else
                {
                    _filename = HttpContext.Current.Server.MapPath("../Attachments/TEMP/InvoicePaymentVoucher_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".pdf");
                    _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsInvoicePaymentVoucher.rpt");
                    _reportfile[1] = "ReportsAccountsInvoicePaymentVoucherHeader.rpt";
                    _reportfile[2] = "ReportsAccountsInvoicePaymentVoucherAdvancePayment.rpt";
                    _reportfile[3] = "ReportsAccountsInvoicePaymentVoucherCreditNotes.rpt";
                    _reportfile[4] = "ReportsAccountsInvoicePaymentVoucherPaymentApproval.rpt";
                    _reportfile[5] = "ReportsAccountsInvoicePaymentVoucherViewHistory.rpt";
                }
            }

            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType != "1")
            {
                string filename = Path.GetFileName(_filename);
                string newfullfilename = Path.GetDirectoryName(filename) + ".pdf";

                PhoenixReportClass.ExportReportPDF(_reportfile, ref _filename, ds);
                Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + _filename + "&type=pdf");
            }
        }

    }


    public void GenerateSSRSPDF(string _filename, DataSet ds, string reportcode)
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];

        nvc.Remove("applicationcode");
        nvc.Add("applicationcode", "5");
        nvc.Remove("reportcode");
        nvc.Add("reportcode", reportcode.ToString());
        nvc.Add("CRITERIA", "");
        Session["PHOENIXREPORTPARAMETERS"] = nvc;
        string[] rdlcfilename = new string[11];

        //  PhoenixSsrsReportsCommon.GetInterface(rdlcfilename, ds, ExportFileFormat.PDF, "GenerateSSRSPDF", ref _filename);
        PhoenixSSRSReportClass.ExportSSRSReport(rdlcfilename, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, "GenerateSSRSPDF", ref _filename);
        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + _filename + "&type=pdf");
    }

    protected void Rebind()
    {
        gvVoucherDetails.SelectedIndexes.Clear();
        gvVoucherDetails.EditIndexes.Clear();
        gvVoucherDetails.DataSource = null;
        gvVoucherDetails.Rebind();
    }

    protected void gvVoucherDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
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
}

