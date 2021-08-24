using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsOffSettingEntriesListWOSubAccount : PhoenixBasePage
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
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsOffSettingEntriesListWOSubAccount.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOffSettingList')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Accounts/AccountsOffSettingEntriesFilterWOSubAccount.aspx", "Find", "search.png", "FIND");

            MenuOffSettingList.AccessRights = this.ViewState;
            MenuOffSettingList.MenuList = toolbar.Show();
            //  MenuOffSettingList.SetTrigger(pnlOffSettingList);

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes["style"] = "display:none";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["PAGEURL"] = null;

                gvOffSettingList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["offsettinglineitemid"] != null)
                    ViewState["OFFSETTINGLINEITEMID"] = Request.QueryString["offsettinglineitemid"].ToString();
                if (ViewState["OFFSETTINGLINEITEMID"] != null)
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsOffSettingContraVocListWOSubAccount.aspx?offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"];
                }

            }
            //BindData();           

            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDVOUCHERLINEITEMNO", "FLDDESCRIPTION", "FLDREFERENCEDOCUMENTNO", "FLDCURRENCYNAME", "FLDTRANSACTIONAMOUNT"
                                 , "FLDPREVIOUSVOUCHERAMOUNT", "FLDBALANCEAMOUNT", "FLDOFFSETVOUCHERSTATUS", "FLDCREATEDBYUSERNAME", "FLDLONGDESCRIPTION"};

        string[] alCaptions = { "Voucher Number","Voucher LineItemNo", "Account", "Reference Document No", "Currency Name", "Prime Amount", "Allocated Voucher Amount"
                                  , "Balance Voucher Amount", "Voucher Status","UserName","Long Description" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataSet ds;

        NameValueCollection nvc = Filter.CurrentOffSettingEntriesSelection;

        if (Filter.CurrentOffSettingEntriesSelection != null)
        {
            ds = PhoenixAccountsOffSettingEntries.OffSettingVoucherSearchWithoutSubAccount(
                                               PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCode")) : null
                                              , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherFromdate")) : null
                                              , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherTodate")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("userlist")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("currencylist")) : null
                                              , nvc != null ? General.GetNullableDecimal(nvc.Get("txtAmountFrom")) : null
                                              , nvc != null ? General.GetNullableDecimal(nvc.Get("txtAmountTo")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumber")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtRefenceNumber")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherLongDescription")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtLineItemLongDescription")) : null
                                              , sortexpression, sortdirection
                                              , (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                              , ref iRowCount, ref iTotalPageCount
                                              , nvc != null ? General.GetNullableInteger(nvc.Get("offsetvoucherstatus")) : null
                                       );
        }
        else
        {
            ds = PhoenixAccountsOffSettingEntries.OffSettingVoucherSearchWithoutSubAccount(
                                              PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                            , null, null, null, null, null, null, null, null, null, null
                                            , null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount
                                            , null
                                     );
        }



        Response.AddHeader("Content-Disposition", "attachment; filename=OffSettingEntriesList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Off-Setting Entries List</h3></td>");
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
    protected void MenuOffSetting_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
       // UserControlTabs ucTabs = (UserControlTabs)sender;
        try
        {
            //if (dce.CommandName.ToUpper().Equals("CONTRAVOUCHERLINEITEMS"))
            //{
            //    Response.Redirect("../Accounts/AccountsOffSettingContraVoucherLineItemDetails.aspx?VOUCHERLINEITEMID=" + ViewState["VOUCHERLINEITEMID"] + "&contravoucherid=" + Session["CONTRAVOUCHERID"]);
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuOffSettingList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDVOUCHERLINEITEMNO", "FLDDESCRIPTION", "FLDREFERENCEDOCUMENTNO", "FLDCURRENCYNAME", "FLDTRANSACTIONAMOUNT"
                                 , "FLDPREVIOUSVOUCHERAMOUNT", "FLDBALANCEAMOUNT", "FLDOFFSETVOUCHERSTATUS", "FLDCREATEDBYUSERNAME", "FLDLONGDESCRIPTION"};

        string[] alCaptions = { "Voucher Number","Voucher LineItemNo", "Account", "Reference Document No", "Currency Name", "Prime Amount", "Allocated Voucher Amount"
                                  , "Balance Voucher Amount", "Voucher Status","UserName","Long Description" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds;
        NameValueCollection nvc = Filter.CurrentOffSettingEntriesSelection;

        if (Filter.CurrentOffSettingEntriesSelection != null)
        {
            ds = PhoenixAccountsOffSettingEntries.OffSettingVoucherSearchWithoutSubAccount(
                                              PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCode")) : null
                                              , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherFromdate")) : null
                                              , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherTodate")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("userlist")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("currencylist")) : null
                                              , nvc != null ? General.GetNullableDecimal(nvc.Get("txtAmountFrom")) : null
                                              , nvc != null ? General.GetNullableDecimal(nvc.Get("txtAmountTo")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumber")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtRefenceNumber")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherLongDescription")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtLineItemLongDescription")) : null
                                              , sortexpression, sortdirection
                                              , (int)ViewState["PAGENUMBER"], gvOffSettingList.PageSize
                                              , ref iRowCount, ref iTotalPageCount
                                              , nvc != null ? General.GetNullableInteger(nvc.Get("offsetvoucherstatus")) : null
                                      );
        }
        else
        {
            ds = PhoenixAccountsOffSettingEntries.OffSettingVoucherSearchWithoutSubAccount(
                                              PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                            , null, null, null, null, null, null, null, null, null, null
                                            , null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                                            , gvOffSettingList.PageSize, ref iRowCount, ref iTotalPageCount
                                            , null
                                     );
        }



        General.SetPrintOptions("gvOffSettingList", "Off-Setting Entries List", alCaptions, alColumns, ds);

        gvOffSettingList.DataSource = ds;
        gvOffSettingList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (ds.Tables[0].Rows.Count > 0)
        {

           
            if (ViewState["OFFSETTINGLINEITEMID"] == null)
            {
                ViewState["OFFSETTINGLINEITEMID"] = ds.Tables[0].Rows[0]["FLDVOUCHERLINEITEMID"].ToString();
                // gvOffSettingList.SelectedIndex = 0;
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsOffSettingContraVocListWOSubAccount.aspx?offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"];
            }
            SetRowSelection();

        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsOffSettingContraVocListWOSubAccount.aspx?offsettinglineitemid=";
            }

        }

      
    }



    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvOffSettingList.Rebind();
            if (Session["New"] != null && Session["New"].ToString() == "Y")
            {

                Session["New"] = "N";
                BindPageURL(0);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    BindData();

    //}

    //public StateBag ReturnViewState()
    //{
    //    return ViewState;
    //}

    //protected void gvOffSettingList_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }

    //}

    protected void gvOffSettingList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOffSettingList.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffSettingList_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvOffSettingList_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvOffSettingList.SelectedIndexes.Add(se.NewSelectedIndex);
        BindPageURL(se.NewSelectedIndex);
    }
    protected void gvOffSettingList_ItemCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            int nCurrentRow = e.Item.ItemIndex;
            BindPageURL(nCurrentRow);
            SetRowSelection();

        }
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvOffSettingList.Items[rowindex];
            RadLabel lblVoucherLineId = ((RadLabel)gvOffSettingList.Items[rowindex].FindControl("lblVoucherLineId"));
            if (lblVoucherLineId != null)
                ViewState["OFFSETTINGLINEITEMID"] = item.GetDataKeyValue("FLDVOUCHERLINEITEMID");

            LinkButton lnkVoucherId = ((LinkButton)gvOffSettingList.Items[rowindex].FindControl("lnkVoucherId"));
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsOffSettingContraVocListWOSubAccount.aspx?offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"];
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetRowSelection()
    {
        gvOffSettingList.SelectedIndexes.Clear();
        if (ViewState["OFFSETTINGLINEITEMID"] != null)
        {
            foreach (GridDataItem item in gvOffSettingList.Items)
            {
                if (item.GetDataKeyValue("FLDVOUCHERLINEITEMID").ToString().Equals(ViewState["OFFSETTINGLINEITEMID"].ToString()))
                {
                    gvOffSettingList.SelectedIndexes.Add(item.ItemIndex);
                    RadLabel lblVouherId = (RadLabel)gvOffSettingList.Items[item.ItemIndex].FindControl("lblVouherId");
                    RadLabel lblVoucherLineId = ((RadLabel)gvOffSettingList.Items[item.ItemIndex].FindControl("lblVoucherLineId"));
                    if (lblVouherId != null)
                        ViewState["VoucherIdNo"] = lblVouherId.Text;
                    if (lblVoucherLineId != null)
                        ViewState["OFFSETTINGLINEITEMID"] = lblVoucherLineId.Text.ToString();

                }
            }
        }
    }

}
