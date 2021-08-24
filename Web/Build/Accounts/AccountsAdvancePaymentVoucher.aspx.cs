using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class AccountsAdvancePaymentVoucher : PhoenixBasePage
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
            toolbargrid.AddImageButton("../Accounts/AccountsAdvancePaymentVoucher.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVoucherDetails')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsAdvancePaymentVoucherFilter.aspx", "Find", "search.png", "FIND");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //MenuOrderForm.SetTrigger(pnlOrderForm);

            if (!IsPostBack)
            {
                Session["New"] = "N";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["voucherid"] = null;
                ViewState["PAGEURL"] = null;

                gvVoucherDetails.PageSize = General.ShowRecords(gvVoucherDetails.PageSize);

                if (Request.QueryString["voucherid"] != null)
                {
                    ViewState["voucherid"] = Request.QueryString["voucherid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdvancePaymentVoucherGeneral.aspx?voucherid=" + ViewState["voucherid"];
                }
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Line Items", "LINEITEM", ToolBarDirection.Right);
            toolbarmain.AddButton("General", "VOUCHER", ToolBarDirection.Right);
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            //MenuOrderFormMain.SetTrigger(pnlOrderForm);

            MenuOrderFormMain.SelectedMenuIndex = 1;
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoucherDetails_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression.Replace(" ASC", "").Replace(" DESC", ""); ;

        switch (se.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        gvVoucherDetails.Rebind();
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                ViewState["PAGEURL"] = "../Accounts/AccountsAdvancePaymentVoucherGeneral.aspx?voucherid=";
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdvancePaymentVoucherGeneral.aspx?voucherid=" + ViewState["voucherid"];
            }
            if (CommandName.ToUpper().Equals("LINEITEM") && ViewState["voucherid"] != null && ViewState["voucherid"].ToString() != string.Empty)
            {
                ViewState["PAGEURL"] = "../Accounts/AccountsAdvancePaymentVoucherLineItem.aspx?voucherid=";
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdvancePaymentVoucherLineItem.aspx?voucherid=" + ViewState["voucherid"];
            }
            else
                MenuOrderFormMain.SelectedMenuIndex = 1;
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

        string[] alCaptions = { "Voucher Number", "Supplier Name", "Currency", "Status" };
        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDNAME", "FLDCURRENCYCODE", "FLDPAYMENTVOUCHERSTATUS" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentAdvancePaymentVoucherSelection;

        ds = PhoenixAccountsAdvancePaymentVoucher.PaymentVoucherSearch(
                                                             nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumberSearch")) : null
                                                           , nvc != null ? General.GetNullableInteger(nvc.Get("txtMakerId")) : null
                                                           , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
                                                           , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                           , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherFromdateSearch")) : null
                                                           , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherTodateSearch")) : null
                                                           , sortexpression
                                                           , sortdirection
                                                           , gvVoucherDetails.CurrentPageIndex + 1
                                                           , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                           , ref iRowCount
                                                           , ref iTotalPageCount
                                                           , nvc != null ? General.GetNullableInteger(nvc.Get("txtSupplierCode")) : null);

        Response.AddHeader("Content-Disposition", "attachment; filename=AdvancePaymentVoucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Advance Payment Voucher</h3></td>");
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
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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

    protected void gvVoucherDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
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

        NameValueCollection nvc = Filter.CurrentAdvancePaymentVoucherSelection;

        ds = PhoenixAccountsAdvancePaymentVoucher.PaymentVoucherSearch(
                                                             nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumberSearch")) : null
                                                           , nvc != null ? General.GetNullableInteger(nvc.Get("txtMakerId")) : null
                                                           , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
                                                           , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                           , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherFromdateSearch")) : null
                                                           , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherTodateSearch")) : null
                                                           , sortexpression
                                                           , sortdirection
                                                           , gvVoucherDetails.CurrentPageIndex + 1
                                                           , gvVoucherDetails.PageSize
                                                           , ref iRowCount
                                                           , ref iTotalPageCount
                                                           , nvc != null ? General.GetNullableInteger(nvc.Get("txtSupplierCode")) : null);

        string[] alCaptions = { "Voucher Number", "Supplier Name", "Currency", "Status" };
        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDNAME", "FLDCURRENCYCODE", "FLDPAYMENTVOUCHERSTATUS" };

        General.SetPrintOptions("gvVoucherDetails", "Advance Payment Voucher", alCaptions, alColumns, ds);

        gvVoucherDetails.DataSource = ds;
        gvVoucherDetails.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["voucherid"] == null)
            {
                ViewState["voucherid"] = ds.Tables[0].Rows[0]["FLDADVANCEPAYMENTVOUCHERID"].ToString();
                //gvVoucherDetails.SelectedIndex = 0;
            }

            if (ViewState["PAGEURL"] == null)
            {
                ViewState["PAGEURL"] = "../Accounts/AccountsAdvancePaymentVoucherGeneral.aspx?voucherid=";
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdvancePaymentVoucherGeneral.aspx?voucherid=" + ViewState["voucherid"].ToString();

            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdvancePaymentVoucherGeneral.aspx";
            }
        }

    }

    private void SetRowSelection()
    {
        gvVoucherDetails.SelectedIndexes.Clear();

        foreach(GridDataItem item in gvVoucherDetails.Items)
        {
            if(item.GetDataKeyValue("FLDADVANCEPAYMENTVOUCHERID").ToString().Equals(ViewState["voucherid"].ToString()))
            {
                gvVoucherDetails.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }

    protected void gvVoucherDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {

                ImageButton app = (ImageButton)e.Item.FindControl("cmdApprove");
                ImageButton cmdApproved = (ImageButton)e.Item.FindControl("cmdApproved");
                RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
                LinkButton lnkVoucherId = (LinkButton)e.Item.FindControl("lnkVoucherId");
                RadLabel lblVoucherType = (RadLabel)e.Item.FindControl("lblVoucherType");
                RadLabel lblSupplierCode = (RadLabel)e.Item.FindControl("lblSuppCode");
                if (!SessionUtil.CanAccess(this.ViewState, app.CommandName)) app.Visible = false;
                if (!SessionUtil.CanAccess(this.ViewState, cmdApproved.CommandName)) cmdApproved.Visible = false;
                if (!SessionUtil.CanAccess(this.ViewState, lnkVoucherId.CommandName)) lnkVoucherId.Visible = false;
                if (lblStatus != null)
                {
                    if (lblStatus.Text.ToString() == "Approved")
                    {
                        app.Visible = false;
                        cmdApproved.Visible = true;
                    }
                }
                if (app != null)
                {
                    if (lblVoucherType != null && lblVoucherType.Text != "")
                    {
                        //VoucherType is 0 for Advance payment vouchers (Techincal Purchase = 583 ) other wise 1
                        string vouchertype = lblVoucherType.Text == "583" ? "0" : "1";
                        app.Attributes.Add("onclick", "parent.openNewWindow('approval', '', 'Common/CommonApproval.aspx?docid=" + lnkVoucherId.CommandArgument + "&mod=" + PhoenixModule.ACCOUNTS + "&type=" + 459 + "&suppliercode=" + lblSupplierCode.Text + "&vouchertype=" + vouchertype + "&user=" + 1 + "&ifr=0');return false;");
                    }
                }
                else
                {
                    app.Visible = false;
                }
            
        }
    }

    protected void gvVoucherDetails_RowCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        RadGrid _gridView = (RadGrid)sender;

        int nCurrentRow = e.Item.ItemIndex;

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            BindPageURL(nCurrentRow);
            SetRowSelection();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvVoucherDetails.Rebind();
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
            ViewState["voucherid"] = ((LinkButton)gvVoucherDetails.Items[rowindex].FindControl("lnkVoucherId")).CommandArgument;
            PhoenixAccountsAdvancePaymentVoucher.InvoiceVoucherNumber = ((LinkButton)gvVoucherDetails.Items[rowindex].FindControl("lnkVoucherId")).Text;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdvancePaymentVoucherGeneral.aspx?voucherid=" + ViewState["voucherid"].ToString();
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
        BindPageURL(e.NewSelectedIndex);
    }
}
