using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsInterCompanyTransferEntriesList : PhoenixBasePage
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
            toolbar.AddImageButton("../Accounts/AccountsInterCompanyTransferEntriesList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvInterCompanyList')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Accounts/AccountsInterCompanyTransferEntriesFilter.aspx", "Find", "search.png", "FIND");

            MenuInterCompanyList.AccessRights = this.ViewState;
            MenuInterCompanyList.MenuList = toolbar.Show();
          //  MenuInterCompanyList.SetTrigger(pnlOffSettingList);

            if (!IsPostBack)
            {
                //PhoenixToolbar toolbarmenu = new PhoenixToolbar();
                //toolbarmenu.AddButton("Contra Voucher","CONTRAVOUCHERS");
                //toolbarmenu.AddButton("Contra Voucher Line Items", "CONTRAVOUCHERLINEITEMS");

                //MenuOffSetting.AccessRights = this.ViewState;
                //MenuOffSetting.MenuList = toolbarmenu.Show();
                //MenuOffSetting.SelectedMenuIndex = 0;

                cmdHiddenSubmit.Attributes["style"] = "display:none";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["PAGEURL"] = null;

               
                if (Request.QueryString["offsettinglineitemid"] != null)
                    ViewState["OFFSETTINGLINEITEMID"] = Request.QueryString["offsettinglineitemid"].ToString();
                if (ViewState["OFFSETTINGLINEITEMID"] != null)
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferContraVouchersList.aspx?offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&callfrom=OTHER" + "&offsettingvoucherid=" + ViewState["OFFSETTINGVOUCHERID"];
                }

                gvInterCompanyList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDVOUCHERLINEITEMNO", "FLDACCOUNTCODE", "FLDREFERENCEDOCUMENTNO", "FLDCURRENCYNAME", "FLDTRANSACTIONAMOUNT", "FLDPREVIOUSVOUCHERAMOUNT"
                                 , "FLDBALANCEAMOUNT", "FLDVOUCHERSTATUS", "FLDCREATEDBYUSERNAME", "FLDSHORTCODE", "FLDLONGDESCRIPTION"};

        string[] alCaptions = { "Voucher Number","Voucher LineItemNo", "Account Code", "Reference Number", "Currency Code", "Prime Amount", "Previous Voucher Amount  "
                                  , "Balnce Voucher Amount ","Voucher Status","User Name", "Company Name","Long Description" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentInterCompanyEntriesSelection;

        DataSet ds;
        if (Filter.CurrentInterCompanyEntriesSelection != null)
        {
            ds = PhoenixAccountsOffSettingEntries.InterCompanyVoucherSearch(

                                              PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCode")) : null
                                              , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherFromdate")) : null
                                              , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherTodate")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("userlist")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("ddlCurrencyCode")) : null
                                              , nvc != null ? General.GetNullableDecimal(nvc.Get("txtAmountFrom")) : null
                                              , nvc != null ? General.GetNullableDecimal(nvc.Get("txtAmountTo")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumber")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtRefenceNumber")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherLongDescription")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtLineItemLongDescription")) : null
                                              , sortexpression, sortdirection
                                              , (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                              , ref iRowCount, ref iTotalPageCount
                                              , nvc != null ? General.GetNullableInteger(nvc.Get("voucherstatus")) : null
                                              , null
                                      );
        }
        else
        {
            ds = PhoenixAccountsOffSettingEntries.InterCompanyVoucherSearch(
                                              PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                            , null, null, null, null, null, null, null, null, null, null
                                            , null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                                            , iRowCount , ref iRowCount, ref iTotalPageCount
                                            , null, null
                                     );
        }


        Response.AddHeader("Content-Disposition", "attachment; filename=InterCompanyEntriesReceivedList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Inter-Company Entries Received From Other Company</h3></td>");
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
     //   UserControlTabs ucTabs = (UserControlTabs)sender;
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
    protected void MenuInterCompanyList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvInterCompanyList.Rebind();
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

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDVOUCHERLINEITEMNO", "FLDACCOUNTCODE", "FLDREFERENCEDOCUMENTNO", "FLDCURRENCYNAME", "FLDTRANSACTIONAMOUNT", "FLDPREVIOUSVOUCHERAMOUNT"
                                 , "FLDBALANCEAMOUNT", "FLDVOUCHERSTATUS", "FLDCREATEDBYUSERNAME", "FLDSHORTCODE", "FLDLONGDESCRIPTION"};

        string[] alCaptions = { "Voucher Number","Voucher LineItemNo", "Account Code", "Reference Number", "Currency Code", "Prime Amount", "Previous Voucher Amount  "
                                  , "Balnce Voucher Amount ","Voucher Status","User Name", "Company Name","Long Description" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentInterCompanyEntriesSelection;

        DataSet ds;
        if (Filter.CurrentInterCompanyEntriesSelection != null)
        {         
            ds = PhoenixAccountsOffSettingEntries.InterCompanyVoucherSearch(

                                              PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCode")) : null
                                              , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherFromdate")) : null
                                              , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherTodate")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("userlist")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("ddlCurrencyCode")) : null
                                              , nvc != null ? General.GetNullableDecimal(nvc.Get("txtAmountFrom")) : null
                                              , nvc != null ? General.GetNullableDecimal(nvc.Get("txtAmountTo")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumber")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtRefenceNumber")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherLongDescription")) : null
                                              , nvc != null ? General.GetNullableString(nvc.Get("txtLineItemLongDescription")) : null
                                              , sortexpression, sortdirection
                                              , (int)ViewState["PAGENUMBER"], gvInterCompanyList.PageSize
                                              , ref iRowCount, ref iTotalPageCount
                                              , nvc != null ? General.GetNullableInteger(nvc.Get("voucherstatus")) : null
                                              , null
                                      );
        }
        else
        {
            ds = PhoenixAccountsOffSettingEntries.InterCompanyVoucherSearch(
                                              PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                            , null, null, null, null, null, null, null, null, null, null
                                            , null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                                            , gvInterCompanyList.PageSize, ref iRowCount, ref iTotalPageCount
                                            , null, null
                                     );
        }

        General.SetPrintOptions("gvInterCompanyList", "Inter-Company Entries Received From Other Company", alCaptions, alColumns, ds);


        gvInterCompanyList.DataSource = ds;
        gvInterCompanyList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;



        if (ds.Tables[0].Rows.Count > 0)
        {
            
            SetRowSelection();

            if (Session["INTERCOMPANYLINEITEM"] == null)
            {
                foreach (GridDataItem item in gvInterCompanyList.Items)
                {
                    System.Collections.Hashtable htLineItem = new System.Collections.Hashtable();
                    RadLabel lblAccountId = (RadLabel)gvInterCompanyList.Items[item.ItemIndex].FindControl("lblAccountId");
                    RadLabel lblBaseCurrencyRate = (RadLabel)gvInterCompanyList.Items[item.ItemIndex].FindControl("lblBaseCurrencyRate");
                    RadLabel lblReportRate = (RadLabel)gvInterCompanyList.Items[item.ItemIndex].FindControl("lblReportRate");
                    RadLabel lblCurrency = (RadLabel)gvInterCompanyList.Items[item.ItemIndex].FindControl("lblCurrency");
                    RadLabel lblBalnceAmount = (RadLabel)gvInterCompanyList.Items[item.ItemIndex].FindControl("lblBalnceAmount");
                    RadLabel lblSubAccountMapId = (RadLabel)gvInterCompanyList.Items[item.ItemIndex].FindControl("lblSubAccountMapId");

                    if (lblAccountId != null && lblBaseCurrencyRate != null && lblReportRate != null && lblCurrency != null && lblBalnceAmount != null && lblSubAccountMapId != null)
                    {
                        htLineItem["FLDACCOUNTID"] = lblAccountId.Text;
                        htLineItem["FLDBASEEXCHANGERATE"] = lblBaseCurrencyRate.Text;
                        htLineItem["FLDREPORTEXCHANGERATE"] = lblReportRate.Text;
                        htLineItem["FLDCURRENCYCODE"] = lblCurrency.Text;
                        htLineItem["FLDTRANSACTIONAMOUNT"] = lblBalnceAmount.Text;
                        htLineItem["FLDBUDGETCODE"] = lblSubAccountMapId.Text;
                        Session["INTERCOMPANYLINEITEM"] = htLineItem;
                    }
                }
            }
            else
            {
               // BindPageURL(0);
            }
            if (ViewState["OFFSETTINGLINEITEMID"] == null )
            {
              
                ViewState["OFFSETTINGLINEITEMID"] = ds.Tables[0].Rows[0]["FLDVOUCHERLINEITEMID"].ToString();
                ViewState["OFFSETTINGVOUCHERID"] = ds.Tables[0].Rows[0]["FLDVOUCHERID"].ToString();
             
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferContraVouchersList.aspx?offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&callfrom=OTHER" + "&offsettingvoucherid=" + ViewState["OFFSETTINGVOUCHERID"];
            }

        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferContraVouchersList.aspx?callfrom=OTHER&offsettingvoucherid=" + ViewState["OFFSETTINGVOUCHERID"] + "&offsettinglineitemid=";
            }
          
        }
    
    }
    //private void BindData()
    //{
    //    int iRowCount = 0;
    //    int iTotalPageCount = 0;

    //    string[] alColumns = { "FLDVOUCHERNUMBER", "FLDVOUCHERLINEITEMNO", "FLDACCOUNTCODE", "FLDREFERENCEDOCUMENTNO", "FLDCURRENCYNAME", "FLDTRANSACTIONAMOUNT", "FLDPREVIOUSVOUCHERAMOUNT"
    //                             , "FLDBALANCEAMOUNT", "FLDVOUCHERSTATUS", "FLDCREATEDBYUSERNAME", "FLDSHORTCODE", "FLDLONGDESCRIPTION"};

    //    string[] alCaptions = { "Voucher Number","Voucher LineItemNo", "Account Code", "Reference Number", "Currency Code", "Prime Amount", "Previous Voucher Amount  "
    //                              , "Balnce Voucher Amount ","Voucher Status","User Name", "Company Name","Long Description" };

    //    string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
    //    int? sortdirection = null;
    //    if (ViewState["SORTDIRECTION"] != null)
    //        sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

    //    NameValueCollection nvc = Filter.CurrentInterCompanyEntriesSelection;

    //    DataSet ds;
    //    if (Filter.CurrentInterCompanyEntriesSelection != null)
    //    {
    //        ds = PhoenixAccountsOffSettingEntries.InterCompanyVoucherSearch(

    //                                          PhoenixSecurityContext.CurrentSecurityContext.CompanyID
    //                                          , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCode")) : null
    //                                          , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherFromdate")) : null
    //                                          , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherTodate")) : null
    //                                          , nvc != null ? General.GetNullableString(nvc.Get("userlist")) : null
    //                                          , nvc != null ? General.GetNullableString(nvc.Get("ddlCurrencyCode")) : null
    //                                          , nvc != null ? General.GetNullableDecimal(nvc.Get("txtAmountFrom")) : null
    //                                          , nvc != null ? General.GetNullableDecimal(nvc.Get("txtAmountTo")) : null
    //                                          , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumber")) : null
    //                                          , nvc != null ? General.GetNullableString(nvc.Get("txtRefenceNumber")) : null
    //                                          , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherLongDescription")) : null
    //                                          , nvc != null ? General.GetNullableString(nvc.Get("txtLineItemLongDescription")) : null
    //                                          , sortexpression, sortdirection
    //                                          , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
    //                                          , ref iRowCount, ref iTotalPageCount
    //                                          , nvc != null ? General.GetNullableInteger(nvc.Get("voucherstatus")) : null
    //                                          , null
    //                                  );
    //    }
    //    else
    //    {
    //        ds = PhoenixAccountsOffSettingEntries.InterCompanyVoucherSearch(
    //                                          PhoenixSecurityContext.CurrentSecurityContext.CompanyID
    //                                        , null, null, null, null, null, null, null, null, null, null
    //                                        , null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
    //                                        , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount
    //                                        , null, null
    //                                 );
    //    }

    //    General.SetPrintOptions("gvInterCompanyList", "Inter-Company Entries Received From Other Company", alCaptions, alColumns, ds);

    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        gvInterCompanyList.DataSource = ds;
    //        gvInterCompanyList.DataBind();
    //        SetRowSelection();

    //        if (Session["INTERCOMPANYLINEITEM"] == null)
    //        {
    //            System.Collections.Hashtable htLineItem = new System.Collections.Hashtable();
    //            Label lblAccountId = (Label)gvInterCompanyList.Rows[0].FindControl("lblAccountId");
    //            Label lblBaseCurrencyRate = (Label)gvInterCompanyList.Rows[0].FindControl("lblBaseCurrencyRate");
    //            Label lblReportRate = (Label)gvInterCompanyList.Rows[0].FindControl("lblReportRate");
    //            Label lblCurrency = (Label)gvInterCompanyList.Rows[0].FindControl("lblCurrency");
    //            Label lblBalnceAmount = (Label)gvInterCompanyList.Rows[0].FindControl("lblBalnceAmount");
    //            Label lblSubAccountMapId = (Label)gvInterCompanyList.Rows[0].FindControl("lblSubAccountMapId");

    //            if (lblAccountId != null && lblBaseCurrencyRate != null && lblReportRate != null && lblCurrency != null && lblBalnceAmount != null && lblSubAccountMapId != null)
    //            {
    //                htLineItem["FLDACCOUNTID"] = lblAccountId.Text;
    //                htLineItem["FLDBASEEXCHANGERATE"] = lblBaseCurrencyRate.Text;
    //                htLineItem["FLDREPORTEXCHANGERATE"] = lblReportRate.Text;
    //                htLineItem["FLDCURRENCYCODE"] = lblCurrency.Text;
    //                htLineItem["FLDTRANSACTIONAMOUNT"] = lblBalnceAmount.Text;
    //                htLineItem["FLDBUDGETCODE"] = lblSubAccountMapId.Text;
    //                Session["INTERCOMPANYLINEITEM"] = htLineItem;
    //            }
    //        }
    //        else
    //        {
    //            BindPageURL(gvInterCompanyList.SelectedIndex);
    //        }
    //        if (ViewState["OFFSETTINGLINEITEMID"] == null)
    //        {
    //            //Label lblVoucherLineId = ((Label)gvInterCompanyList.Rows[rowindex].FindControl("lblVoucherLineId"));
    //            ViewState["OFFSETTINGLINEITEMID"] = ds.Tables[0].Rows[0]["FLDVOUCHERLINEITEMID"].ToString();
    //            ViewState["OFFSETTINGVOUCHERID"] = ds.Tables[0].Rows[0]["FLDVOUCHERID"].ToString();
    //            gvInterCompanyList.SelectedIndex = 0;
    //        }

    //        if (ViewState["PAGEURL"] == null)
    //        {
    //            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferContraVouchersList.aspx?offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&callfrom=OTHER" + "&offsettingvoucherid=" + ViewState["OFFSETTINGVOUCHERID"];
    //        }

    //    }
    //    else
    //    {
    //        if (ViewState["PAGEURL"] == null)
    //        {
    //            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferContraVouchersList.aspx?callfrom=OTHER&offsettingvoucherid=" + ViewState["OFFSETTINGVOUCHERID"] + "&offsettinglineitemid=";
    //        }
    //        DataTable dt = ds.Tables[0];
    //        ShowNoRecordsFound(dt, gvInterCompanyList);
    //    }

    //    ViewState["ROWCOUNT"] = iRowCount;
    //    ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    //}


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvInterCompanyList.Rebind();
            if (Session["New"].ToString() == "Y")
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
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
      
    }
   
    protected void gvInterCompanyList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInterCompanyList.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvInterCompanyList_SortCommand(object sender, GridSortCommandEventArgs e)
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

   
    //protected void gvInterCompanyList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    gvInterCompanyList.SelectedIndexes.Add(e.NewSelectedIndex);

    //    BindPageURL(e.NewSelectedIndex);
    //}

    protected void gvInterCompanyList_ItemCommand(object sender, GridCommandEventArgs e)
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
    //protected void gvInterCompanyList_RowCommand(object sender, GridViewCommandEventArgs e)
    //{


    //    GridView _gridView = (GridView)sender;
    //    if (e.CommandName.ToUpper().Equals("SORT"))
    //        return;
    //    if (e.CommandName.ToUpper().Equals("EDIT"))
    //    {
    //        int iRowno;
    //        iRowno = int.Parse(e.CommandArgument.ToString());
    //        BindPageURL(iRowno);

    //    }
    //}

    private void SetRowSelection()
    {
        gvInterCompanyList.SelectedIndexes.Clear();
        if (ViewState["OFFSETTINGLINEITEMID"] != null)
        {
            foreach (GridDataItem item in gvInterCompanyList.Items)
            {
                if (item.GetDataKeyValue("FLDVOUCHERLINEITEMID").ToString().Equals(ViewState["OFFSETTINGLINEITEMID"].ToString()))
                {
                    // gvInterCompanyList.SelectedIndex = i;
                    RadLabel lblVouherId = (RadLabel)gvInterCompanyList.Items[item.ItemIndex].FindControl("lblVouherId");
                    RadLabel lblVoucherLineId = ((RadLabel)gvInterCompanyList.Items[item.ItemIndex].FindControl("lblVoucherLineId"));
                    RadLabel lblOffSettingVoucherId = (RadLabel)gvInterCompanyList.Items[item.ItemIndex].FindControl("lblVoucherIdNo");
                    if (lblVouherId != null)
                        ViewState["VoucherIdNo"] = lblVouherId.Text;
                    if (lblVoucherLineId != null)
                        ViewState["OFFSETTINGLINEITEMID"] = lblVoucherLineId.Text.ToString();
                    if (lblOffSettingVoucherId != null)
                        ViewState["OFFSETTINGVOUCHERID"] = lblOffSettingVoucherId.Text.ToString();

                }
            }
        }
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvInterCompanyList.Items[rowindex];
            RadLabel lblVoucherLineId = ((RadLabel)gvInterCompanyList.Items[rowindex].FindControl("lblVoucherLineId"));
            if (lblVoucherLineId != null)
                ViewState["OFFSETTINGLINEITEMID"] = lblVoucherLineId.Text.ToString();
            System.Collections.Hashtable htLineItem = new System.Collections.Hashtable();

            RadLabel lblAccountId = (RadLabel)gvInterCompanyList.Items[rowindex].FindControl("lblAccountId");
            RadLabel lblBaseCurrencyRate = (RadLabel)gvInterCompanyList.Items[rowindex].FindControl("lblBaseCurrencyRate");
            RadLabel lblReportRate = (RadLabel)gvInterCompanyList.Items[rowindex].FindControl("lblReportRate");
            RadLabel lblCurrency = (RadLabel)gvInterCompanyList.Items[rowindex].FindControl("lblCurrency");
            RadLabel lblBalnceAmount = (RadLabel)gvInterCompanyList.Items[rowindex].FindControl("lblBalnceAmount");
            RadLabel lblSubAccountMapId = (RadLabel)gvInterCompanyList.Items[rowindex].FindControl("lblSubAccountMapId");
            RadLabel lblOffSettingVoucherId = (RadLabel)gvInterCompanyList.Items[rowindex].FindControl("lblVoucherIdNo");

            if (lblAccountId != null && lblBaseCurrencyRate != null && lblReportRate != null && lblCurrency != null && lblBalnceAmount != null && lblSubAccountMapId != null)
            {
                htLineItem["FLDACCOUNTID"] = lblAccountId.Text;
                htLineItem["FLDBASEEXCHANGERATE"] = lblBaseCurrencyRate.Text;
                htLineItem["FLDREPORTEXCHANGERATE"] = lblReportRate.Text;
                htLineItem["FLDCURRENCYCODE"] = lblCurrency.Text;
                htLineItem["FLDTRANSACTIONAMOUNT"] = lblBalnceAmount.Text;
                htLineItem["FLDBUDGETCODE"] = lblSubAccountMapId.Text;
                Session["INTERCOMPANYLINEITEM"] = htLineItem;
            }
            if (lblOffSettingVoucherId != null)
                ViewState["OFFSETTINGVOUCHERID"] = lblOffSettingVoucherId.Text.ToString();
            LinkButton lnkVoucherId = ((LinkButton)gvInterCompanyList.Items[rowindex].FindControl("lnkVoucherId"));

            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferContraVouchersList.aspx?offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&callfrom=OTHER" + "&offsettingvoucherid=" + ViewState["OFFSETTINGVOUCHERID"];
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    //private void BindPageURL(int rowindex)
    //{
    //    try    
    //    {
    //        Label lblVoucherLineId = ((Label)gvInterCompanyList.Rows[rowindex].FindControl("lblVoucherLineId"));
    //        if (lblVoucherLineId != null)
    //            ViewState["OFFSETTINGLINEITEMID"] = lblVoucherLineId.Text.ToString();
    //        System.Collections.Hashtable htLineItem = new System.Collections.Hashtable();

    //        Label lblAccountId = (Label)gvInterCompanyList.Rows[rowindex].FindControl("lblAccountId");
    //        Label lblBaseCurrencyRate = (Label)gvInterCompanyList.Rows[rowindex].FindControl("lblBaseCurrencyRate");
    //        Label lblReportRate = (Label)gvInterCompanyList.Rows[rowindex].FindControl("lblReportRate");
    //        Label lblCurrency = (Label)gvInterCompanyList.Rows[rowindex].FindControl("lblCurrency");
    //        Label lblBalnceAmount = (Label)gvInterCompanyList.Rows[rowindex].FindControl("lblBalnceAmount");
    //        Label lblSubAccountMapId = (Label)gvInterCompanyList.Rows[rowindex].FindControl("lblSubAccountMapId");
    //        Label lblOffSettingVoucherId = (Label)gvInterCompanyList.Rows[rowindex].FindControl("lblVoucherIdNo");

    //        if (lblAccountId != null && lblBaseCurrencyRate != null && lblReportRate != null && lblCurrency != null && lblBalnceAmount != null && lblSubAccountMapId != null)
    //        {
    //            htLineItem["FLDACCOUNTID"] = lblAccountId.Text;
    //            htLineItem["FLDBASEEXCHANGERATE"] = lblBaseCurrencyRate.Text;
    //            htLineItem["FLDREPORTEXCHANGERATE"] = lblReportRate.Text;
    //            htLineItem["FLDCURRENCYCODE"] = lblCurrency.Text;
    //            htLineItem["FLDTRANSACTIONAMOUNT"] = lblBalnceAmount.Text;
    //            htLineItem["FLDBUDGETCODE"] = lblSubAccountMapId.Text;
    //            Session["INTERCOMPANYLINEITEM"] = htLineItem;
    //        }
    //        if (lblOffSettingVoucherId != null)
    //            ViewState["OFFSETTINGVOUCHERID"] = lblOffSettingVoucherId.Text.ToString();
    //        LinkButton lnkVoucherId = ((LinkButton)gvInterCompanyList.Rows[rowindex].FindControl("lnkVoucherId"));

    //        ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferContraVouchersList.aspx?offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&callfrom=OTHER" + "&offsettingvoucherid=" + ViewState["OFFSETTINGVOUCHERID"];
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //private void SetRowSelection()
    //{
    //    gvInterCompanyList.SelectedIndex = 0;
    //    if (ViewState["OFFSETTINGLINEITEMID"] != null)
    //    {
    //        for (int i = 0; i < gvInterCompanyList.Rows.Count; i++)
    //        {
    //            if (gvInterCompanyList.DataKeys[i].Value.ToString().Equals(ViewState["OFFSETTINGLINEITEMID"].ToString()))
    //            {
    //                gvInterCompanyList.SelectedIndex = i;
    //                Label lblVouherId = (Label)gvInterCompanyList.Rows[i].FindControl("lblVouherId");
    //                Label lblVoucherLineId = ((Label)gvInterCompanyList.Rows[i].FindControl("lblVoucherLineId"));
    //                Label lblOffSettingVoucherId = (Label)gvInterCompanyList.Rows[i].FindControl("lblVoucherIdNo");
    //                if (lblVouherId != null)
    //                    ViewState["VoucherIdNo"] = lblVouherId.Text;
    //                if (lblVoucherLineId != null)
    //                    ViewState["OFFSETTINGLINEITEMID"] = lblVoucherLineId.Text.ToString();
    //                if (lblOffSettingVoucherId != null)
    //                    ViewState["OFFSETTINGVOUCHERID"] = lblOffSettingVoucherId.Text.ToString();

    //            }
    //        }
    //    }
    //}

}
