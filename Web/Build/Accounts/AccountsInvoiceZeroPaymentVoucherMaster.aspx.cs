using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class AccountsInvoiceZeroPaymentVoucherMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceZeroPaymentVoucherMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVoucherDetails')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsInvoicePaymentVoucherFilter.aspx?source=ZeroPV", "Find", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Voucher", "VOUCHER", ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();

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
                if (Request.QueryString["voucherid"] != null)
                {
                    ViewState["voucherid"] = Request.QueryString["voucherid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoicePaymentVoucher.aspx?voucherid=" + ViewState["voucherid"].ToString();
                    DataSet ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherEdit(ViewState["voucherid"].ToString());
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["vouchertype"] = dr["FLDTYPE"].ToString();
                    ViewState["vouchersubtype"] = dr["FLDSUBTYPE"].ToString();
                }
                MenuOrderFormMain.SelectedMenuIndex = 0;
                gvVoucherDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

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
            if (ViewState["vouchertype"] != null && ViewState["vouchertype"].ToString() != string.Empty)
                VoucherType = ViewState["vouchertype"].ToString();
            else
                VoucherType = "";

            if (CommandName.ToUpper().Equals("LINEITEM") && ViewState["voucherid"] != null && ViewState["voucherid"].ToString() != string.Empty)
            {
                if (ViewState["vouchertype"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 221, "INV")
                        && ViewState["vouchersubtype"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 213, "AVN"))
                {
                    Response.Redirect("../Accounts/AccountsAirfarePaymentVoucherDetails.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&type=" + ViewState["vouchertype"].ToString() + "&callfrom=ZEROPV");
                }
                if (VoucherType == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 221, "CTM"))
                    Response.Redirect("../Accounts/AccountsCtmPaymentVoucherLineItemDetails.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&callfrom=ZEROPV");
                else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "APT"))
                {
                    Response.Redirect("../Accounts/AccountsPaymentVoucherAdvanceRequests.aspx?voucherid=" + ViewState["voucherid"] + "&callfrom=ZEROPV");
                }
                else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "ALT"))
                {
                    Response.Redirect("../Accounts/AccountsPaymentVoucherAllotmentRequest.aspx?voucherid=" + ViewState["voucherid"] + "&callfrom=ZEROPV");
                }
                else if (VoucherType == PhoenixCommonRegisters.GetHardCode(1, 221, "TCL"))
                {
                    Response.Redirect("../Accounts/AccountsPaymentVoucherVesselVisitTravelClaim.aspx?voucherid=" + ViewState["voucherid"] + "&callfrom=ZEROPV");
                }

                else
                    Response.Redirect("../Accounts/AccountsInvoicePaymentVoucherLineItemDetails.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&callfrom=ZEROPV");

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

        string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency", "Amount", "Fleet", "Status", "Remittance Request Id" };
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
            NameValueCollection nvc = Filter.CurrentInvoicePaymentVoucherSelection;
            ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherSearch(General.GetNullableString(nvc.Get("txtVoucherNumberSearch").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("txtMakerId").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim())
                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                    , General.GetNullableInteger(nvc.Get("ddlVoucherStatus").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("chkShowRemittancenotGenerated").ToString())
                    , General.GetNullableString(nvc.Get("txtVoucherFromdateSearch").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtVoucherTodateSearch").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ucTechFleet").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtInvoiceNumber").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtPurchaseInvoiceVoucherNumber").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("chkReportNotTaken").ToString())
                    , General.GetNullableInteger(nvc.Get("ddlSource").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlType").ToString().Trim())
                    , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                    , ref iRowCount, ref iTotalPageCount
                    , 1
                    , General.GetNullableInteger(nvc.Get("ddlAllocationStatus").ToString()));

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
                                            , ref iRowCount, ref iTotalPageCount
                                            , 1
                                            , 0);
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;

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
        string FromdateSearch = General.GetNullableDateTime("txtVoucherFromdateSearch").ToString().Trim();
        string TodateSearch = General.GetNullableDateTime("txtVoucherTodateSearch").ToString().Trim();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (Filter.CurrentInvoicePaymentVoucherSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentInvoicePaymentVoucherSelection;
            ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherSearch(General.GetNullableString(nvc.Get("txtVoucherNumberSearch").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("txtMakerId").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim())
                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                    , General.GetNullableInteger(nvc.Get("ddlVoucherStatus").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("chkShowRemittancenotGenerated").ToString())
                    , FromdateSearch
                    , TodateSearch
                    , General.GetNullableInteger(nvc.Get("ucTechFleet").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtInvoiceNumber").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtPurchaseInvoiceVoucherNumber").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("chkReportNotTaken").ToString())
                    , General.GetNullableInteger(nvc.Get("ddlSource").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlType").ToString().Trim())
                    , sortexpression, sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , gvVoucherDetails.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , 1
                    , General.GetNullableInteger(nvc.Get("ddlAllocationStatus").ToString()));

        }
        else
        {
            ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherSearch("", null, null
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
                                            , 1
                                            , 0);
        }


        string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency", "Amount", "Fleet", "Status", "Remittance Request Id" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDPAYMENTVOUCHERFLEET", "FLDPAYMENTVOUCHERSTATUS", "FLDREMITTANCENUMBER" };

        General.SetPrintOptions("gvVoucherDetails", "Accounts Voucher", alCaptions, alColumns, ds);

        gvVoucherDetails.DataSource = ds;
        gvVoucherDetails.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
        }

        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        gvVoucherDetails.Columns[3].Visible = (showcreditnotedisc == 1) ? true : false;
        gvVoucherDetails.Columns[4].Visible = (showcreditnotedisc == 1) ? true : false;


    }

    private void SetRowSelection()
    {
        gvVoucherDetails.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvVoucherDetails.Items)
        {
            if (item.GetDataKeyValue("FLDPAYMENTVOUCHERID").ToString().Equals(ViewState["voucherid"].ToString()))
            {
                gvVoucherDetails.SelectedIndexes.Add(item.ItemIndex);
                PhoenixAccountsInvoicePaymentVoucher.InvoiceVoucherNumber = ((LinkButton)gvVoucherDetails.Items[item.ItemIndex].FindControl("lnkVoucherId")).Text;
            }
        }
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
    protected void Rebind()
    {
        gvVoucherDetails.SelectedIndexes.Clear();
        gvVoucherDetails.EditIndexes.Clear();
        gvVoucherDetails.DataSource = null;
        gvVoucherDetails.Rebind();
    }

    protected void gvVoucherDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

        }

        if (e.Item is GridDataItem)
        {

            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
                && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                ImageButton cmdrepost = (ImageButton)e.Item.FindControl("cmdrepost");
                RadLabel lblrepost = (RadLabel)e.Item.FindControl("lblrepost");
                ImageButton cmdAllocate = (ImageButton)e.Item.FindControl("cmdAllocate");

                if (lblrepost.Text == "0")
                {
                    cmdrepost.Visible = false;
                    cmdAllocate.Visible = true;
                }
                else
                {
                    cmdrepost.Visible = true;
                    cmdAllocate.Visible = false;
                }
            }
        }
    }

    protected void gvVoucherDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
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
            }
            if (e.CommandName.ToUpper().Equals("ALLOCATE"))
            {
                int iRowno;
                iRowno = int.Parse(e.CommandArgument.ToString());
                PhoenixAccountsRemittance.ZeroPaymentVoucherAllocation(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                       , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , new Guid(((RadLabel)e.Item.FindControl("lblVoucherId")).Text)
                                                                       );
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("REPOST"))
            {
                int iRowno;
                iRowno = int.Parse(e.CommandArgument.ToString());
                PhoenixAccountsRemittance.ZeroPaymentVoucherAllocationRepost(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                       , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , new Guid(((RadLabel)e.Item.FindControl("lblVoucherId")).Text)
                                                                       );
                Rebind();
            }

            // PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(lnkVoucherId.CommandArgument));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    // protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    // {
    //     BindData();
    //     SetPageNavigator();
    //
    //     if (Session["New"].ToString() == "Y")
    //     {
    //         gvVoucherDetails.SelectedIndex = 0;
    //         Session["New"] = "N";
    //         BindPageURL(gvVoucherDetails.SelectedIndex);
    //     }       
    // }


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
        gvVoucherDetails.SelectedIndexes.Add(e.NewSelectedIndex);
        ViewState["voucherid"] = ((LinkButton)gvVoucherDetails.Items[e.NewSelectedIndex].FindControl("lnkVoucherId")).CommandArgument;
        ViewState["vouchertype"] = ((RadLabel)gvVoucherDetails.Items[e.NewSelectedIndex].FindControl("lblType")).Text;
        ViewState["vouchersubtype"] = ((RadLabel)gvVoucherDetails.Items[e.NewSelectedIndex].FindControl("lblSubType")).Text;
        BindPageURL(e.NewSelectedIndex);
    }
}
