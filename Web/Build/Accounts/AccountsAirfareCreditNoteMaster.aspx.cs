using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using Telerik.Web.UI;

public partial class AccountsAirfareCreditNoteMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsAirfareCreditNoteMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvCreditDebitNote')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsAirfareCreditNoteMasterFilter.aspx?Source=Register", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsAirfareCreditNoteMaster.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuCreditDebitNote.AccessRights = this.ViewState;
            MenuCreditDebitNote.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                Session["New"] = "N";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["AirfareCreditNoteId"] = null;
                ViewState["PAGEURL"] = null;
                ViewState["TabFlag"] = null;
                ViewState["VoucherId"] = null;
                TabStripBind("2");

                if (Request.QueryString["AirfareCreditNoteId"] != null)
                {
                    ViewState["AirfareCreditNoteId"] = Request.QueryString["AirfareCreditNoteId"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuCreditDebitNoteTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("CREDITNOTE"))
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAirfareCreditNote.aspx?AirfareCreditNoteId=" + ViewState["AirfareCreditNoteId"];
            }
            if (CommandName.ToUpper().Equals("LINEITEM") && ViewState["AirfareCreditNoteId"] != null && ViewState["AirfareCreditNoteId"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsAirfareCreditNoteLineItemDetails.aspx?AirfareCreditNoteId=" + ViewState["AirfareCreditNoteId"].ToString() + "&VoucherId=" + ViewState["VoucherId"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("REBATE RECEIVABLE"))
            {
                Response.Redirect("../Accounts/AccountsAirfareCreditNoteRebateReceivable.aspx?AirfareCreditNoteId=" + ViewState["AirfareCreditNoteId"].ToString(), false);
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
                                "Vendor Credit Note No",
                                "Currency",
                                "Amount",
                                "Credit Note Register No",
                                "Status"
                              };

        string[] alColumns = {
                                "FLDADDRESSCODE",
                                "FLDSUPPLIERNAME",
                                "FLDREFERENCENO",
                                "FLDCURRENCYCODE",
                                "FLDAMOUNT",
                                "FLDCNREGISTERNO",
                                "FLDSTATUSNAME"
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


        NameValueCollection nvc = Filter.CurrentAirfareCreditNoteRegisterFilter;


        ds = PhoenixAccountsAirfareCreditNote.AirfareCreditNoteSearch(
              nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtCreditNoteNumber")) : string.Empty
            , nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNumber")) : string.Empty
            , null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucDocFromDate")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucDocToDate")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReceivedFromDate")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReceivedDate")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNo")) : string.Empty
            , sortexpression, sortdirection
            , gvCreditDebitNote.CurrentPageIndex + 1
            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
            , ref iRowCount, ref iTotalPageCount
            , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentVoucher")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlBillToCompany")) : null);

        Response.AddHeader("Content-Disposition", "attachment; filename=CreditNote.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Credit Note</h3></td>");
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
    protected void MenuCreditDebitNote_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentAirfareCreditNoteRegisterFilter = null;
                gvCreditDebitNote.CurrentPageIndex = 0;
                Rebind();
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


        NameValueCollection nvc = Filter.CurrentAirfareCreditNoteRegisterFilter;

        ds = PhoenixAccountsAirfareCreditNote.AirfareCreditNoteSearch(
                                                 nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtCreditNoteNumber")) : string.Empty
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNumber")) : string.Empty
                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucCashStatus").ToString()) : null
                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucDocFromDate")) : null
                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucDocToDate")) : null
                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReceivedFromDate")) : null
                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReceivedDate")) : null
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNo")) : string.Empty
                                                , sortexpression, sortdirection
                                                , gvCreditDebitNote.CurrentPageIndex + 1
                                                , gvCreditDebitNote.PageSize
                                                , ref iRowCount, ref iTotalPageCount
                                                , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentVoucher")) : null
                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlBillToCompany")) : null
                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkshowzeroamount")) : null);

        string[] alCaptions = {
                                "Supplier Code",
                                "Supplier Name",
                                "Vendor Credit Note No",
                                "Currency",
                                "Amount",
                                "Credit Note Register No",
                                "Status"
                              };

        string[] alColumns = {
                                "FLDADDRESSCODE",
                                "FLDSUPPLIERNAME",
                                "FLDREFERENCENO",
                                "FLDCURRENCYCODE",
                                "FLDAMOUNT",
                                "FLDCNREGISTERNO",
                                "FLDSTATUSNAME"
                             };

        General.SetPrintOptions("gvCreditDebitNote", "Credit Note", alCaptions, alColumns, ds);

        gvCreditDebitNote.DataSource = ds;
        gvCreditDebitNote.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["AirfareCreditNoteId"] == null)
            {
                ViewState["AirfareCreditNoteId"] = ds.Tables[0].Rows[0]["FLDAIRFARECREDITNOTEID"].ToString();
                ViewState["VoucherId"] = ds.Tables[0].Rows[0]["FLDVOUCHERID"].ToString();
                // gvCreditDebitNote.SelectedIndex = 0;
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAirfareCreditNote.aspx?AirfareCreditNoteId=" + ViewState["AirfareCreditNoteId"].ToString();
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAirfareCreditNote.aspx?AirfareCreditNoteId=" + ViewState["AirfareCreditNoteId"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAirfareCreditNote.aspx?AirfareCreditNoteId=";
            }
            DataTable dt = ds.Tables[0];
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void SetRowSelection()
    {
        string a = ViewState["AirfareCreditNoteId"].ToString();
        //gvCreditDebitNote.SelectedIndex = -1;

        foreach (GridDataItem item in gvCreditDebitNote.Items)
        {

            if (item.GetDataKeyValue("FLDAIRFARECREDITNOTEID").ToString().Equals(ViewState["AirfareCreditNoteId"].ToString()))
            {
                gvCreditDebitNote.SelectedIndexes.Add(item.ItemIndex);
                ViewState["TabFlag"] = ((RadLabel)gvCreditDebitNote.Items[item.ItemIndex].FindControl("lbltabflag")).Text;
                ViewState["VoucherId"] = ((RadLabel)gvCreditDebitNote.Items[item.ItemIndex].FindControl("lblVoucherId")).Text;
                TabStripBind(ViewState["TabFlag"].ToString());
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvCreditDebitNote.Rebind();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["AirfareCreditNoteId"] = ((RadLabel)gvCreditDebitNote.Items[rowindex].FindControl("lblCreditDebitNoteId")).Text;
            ViewState["TabFlag"] = ((RadLabel)gvCreditDebitNote.Items[rowindex].FindControl("lbltabflag")).Text;
            ViewState["VoucherId"] = ((RadLabel)gvCreditDebitNote.Items[rowindex].FindControl("lblVoucherId")).Text;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAirfareCreditNote.aspx?AirfareCreditNoteId=" + ViewState["AirfareCreditNoteId"].ToString();
            TabStripBind(ViewState["TabFlag"].ToString());
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void TabStripBind(string flag)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        if (flag == "2" || flag == "0")
            toolbarmain.AddButton("Rebate Receivable", "REBATE RECEIVABLE", ToolBarDirection.Right);
        if (flag == "1" || flag == "0")
            toolbarmain.AddButton("Line Items", "LINEITEM", ToolBarDirection.Right);
        toolbarmain.AddButton("Credit Notes", "CREDITNOTE", ToolBarDirection.Right);
        MenuCreditDebitNoteTab.AccessRights = this.ViewState;
        MenuCreditDebitNoteTab.MenuList = toolbarmain.Show();
        MenuCreditDebitNoteTab.SelectedMenuIndex = 1;

    }
    protected void Rebind()
    {
        gvCreditDebitNote.SelectedIndexes.Clear();
        gvCreditDebitNote.EditIndexes.Clear();
        gvCreditDebitNote.DataSource = null;
        gvCreditDebitNote.Rebind();
    }
    protected void gvCreditDebitNote_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvCreditDebitNote_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName == "ChangePageSize")
            {
                return;
            }

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(nCurrentRow);
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
}
