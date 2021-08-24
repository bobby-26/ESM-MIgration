using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsInvoicePaymentVoucherAdminMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsInvoicePaymentVoucherAdminMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVoucherDetails')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsInvoicePaymentVoucherAdminFilter.aspx?source=paymentvoucher", "Find", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                Session["New"] = "N";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["voucherid"] = null;
                ViewState["vouchertype"] = null;
                ViewState["PAGEURL"] = null;
                gvVoucherDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (Request.QueryString["voucherid"] != null)
                {
                    ViewState["voucherid"] = Request.QueryString["voucherid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoicePaymentVoucherAdmin.aspx?voucherid=" + ViewState["voucherid"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency", "Amount", "Status", "Remittance Request Id", "Type", "Source" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDPAYMENTVOUCHERSTATUS", "FLDREMITTANCENUMBER", "FLDPAYMENTTYPE", "FLDSOURCE" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (Filter.CurrentInvoicePaymentVoucherAdminSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentInvoicePaymentVoucherAdminSelection;
            ds = PhoenixAccountsInvoicePaymentVoucherAdmin.PaymentVoucherSearch(nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumberSearch")) : null
                    , General.GetNullableInteger(nvc.Get("txtMakerId").ToString().Trim())
                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlVoucherStatus")) : null
                    , General.GetNullableInteger(nvc.Get("chkShowRemittancenotGenerated").ToString())
                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherFromdateSearch")) : null
                    , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherTodateSearch")) : null
                    , null
                    , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceNumber")) : null
                    , nvc != null ? General.GetNullableString(nvc.Get("txtPurchaseInvoiceVoucherNumber")) : null
                    , General.GetNullableInteger(nvc.Get("chkReportNotTaken").ToString())
                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlSource")) : null
                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlType")) : null
                    , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);

        }
        else
        {
            ds = PhoenixAccountsInvoicePaymentVoucherAdmin.PaymentVoucherSearch("", null, null
                                            , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                            , null
                                            , null
                                            , General.GetNullableDateTime("")
                                            , General.GetNullableDateTime("")
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
        Response.Write("<td><h3>Payment Voucher</h3></td>");
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

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (Filter.CurrentInvoicePaymentVoucherAdminSelection != null)
            {
                NameValueCollection nvc = Filter.CurrentInvoicePaymentVoucherAdminSelection;
                ds = PhoenixAccountsInvoicePaymentVoucherAdmin.PaymentVoucherSearch(nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumberSearch")) : null
                        , General.GetNullableInteger(nvc.Get("txtMakerId").ToString().Trim())
                        , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
                        , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                        , nvc != null ? General.GetNullableInteger(nvc.Get("ddlVoucherStatus")) : null
                        , General.GetNullableInteger(nvc.Get("chkShowRemittancenotGenerated").ToString())
                        , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherFromdateSearch")) : null
                        , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherTodateSearch")) : null
                        , null
                        , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceNumber")) : null
                        , nvc != null ? General.GetNullableString(nvc.Get("txtPurchaseInvoiceVoucherNumber")) : null
                        , General.GetNullableInteger(nvc.Get("chkReportNotTaken").ToString())
                        , nvc != null ? General.GetNullableInteger(nvc.Get("ddlSource")) : null
                        , nvc != null ? General.GetNullableInteger(nvc.Get("ddlType")) : null
                        , sortexpression, sortdirection
                        , gvVoucherDetails.CurrentPageIndex + 1
                        , gvVoucherDetails.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount);

            }
            else
            {
                ds = PhoenixAccountsInvoicePaymentVoucherAdmin.PaymentVoucherSearch(null, null, null
                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                , null
                                                , null
                                                , General.GetNullableDateTime("")
                                                , General.GetNullableDateTime("")
                                                , null
                                                , null
                                                , null
                                                , null
                                                , null
                                                , null
                                                , sortexpression
                                                , sortdirection
                                                , gvVoucherDetails.CurrentPageIndex + 1
                                                , gvVoucherDetails.PageSize
                                                , ref iRowCount, ref iTotalPageCount);
            }


            string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency", "Amount", "Status", "Remittance Request Id", "Type", "Source" };
            string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDPAYMENTVOUCHERSTATUS", "FLDREMITTANCENUMBER", "FLDPAYMENTTYPE", "FLDSOURCE" };

            General.SetPrintOptions("gvVoucherDetails", "Payment Voucher", alCaptions, alColumns, ds);

            gvVoucherDetails.DataSource = ds;
            gvVoucherDetails.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["voucherid"] == null)
                {
                    ViewState["voucherid"] = ds.Tables[0].Rows[0]["FLDPAYMENTVOUCHERID"].ToString();
                    //  gvVoucherDetails.SelectedIndex = 0;
                }
                if (ViewState["vouchertype"] == null)
                {
                    ViewState["vouchertype"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                }
                if (ViewState["PAGEURL"] == null)
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoicePaymentVoucherAdmin.aspx?voucherid=" + ViewState["voucherid"].ToString();
                }
                SetRowSelection();
            }
            else
            {
                if (ViewState["PAGEURL"] == null)
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoicePaymentVoucherAdmin.aspx?voucherid=";
                }
                DataTable dt = ds.Tables[0];
                // ShowNoRecordsFound(dt, gvVoucherDetails);
            }

            short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
            gvVoucherDetails.Columns[3].Visible = (showcreditnotedisc == 1) ? true : false;
            gvVoucherDetails.Columns[4].Visible = (showcreditnotedisc == 1) ? true : false;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
            RadLabel lblVoucherType = (RadLabel)e.Item.FindControl("lblVoucherType");
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton cmdApprove = (ImageButton)e.Item.FindControl("cmdApprove");
            if (cmdApprove != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName)) cmdApprove.Visible = false;
            }
        }

        if (e.Item is GridDataItem)
        {

            // if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
            // && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
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
                    imbApprovalHistory.Attributes.Add("onclick", "openNewWindow('PaymentVoucherApproval', '', '" + Session["sitepath"] + "/Accounts/AccountsInvoicePaymentVoucherApprovalHistory.aspx?voucherid=" + lnkVoucherId.CommandArgument + "&vouchernumber=" + lnkVoucherId.Text + "');return false;");
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
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
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
            PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvVoucherDetails.Items[rowindex].FindControl("lnkVoucherId")).Text;
            string type = ((RadLabel)gvVoucherDetails.Items[rowindex].FindControl("lblVoucherTypes")).Text;

            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoicePaymentVoucherAdmin.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&type=" + ViewState["vouchertype"].ToString();
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
        BindPageURL(e.NewSelectedIndex);
    }

    protected void Rebind()
    {
        gvVoucherDetails.SelectedIndexes.Clear();
        gvVoucherDetails.EditIndexes.Clear();
        gvVoucherDetails.DataSource = null;
        gvVoucherDetails.Rebind();
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
}
