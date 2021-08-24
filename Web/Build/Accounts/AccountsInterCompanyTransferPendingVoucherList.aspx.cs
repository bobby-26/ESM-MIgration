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

public partial class AccountsInterCompanyTransferPendingVoucherList : PhoenixBasePage
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
            toolbar.AddImageButton("../Accounts/AccountsInterCompanyTransferPendingVoucherList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvInterCompanyPendingEntriesList')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Accounts/AccountsInterCompanyTransferEntriesFilter.aspx?callfrom=pendingvouchers", "Find", "search.png", "FIND");

            MenuInterCompanyList.AccessRights = this.ViewState;
            MenuInterCompanyList.MenuList = toolbar.Show();
          //  MenuInterCompanyList.SetTrigger(pnlOffSettingList);

            if (!IsPostBack)
            {               
                cmdHiddenSubmit.Attributes["style"] = "display:none";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["PAGEURL"] = null;

              

                if (Request.QueryString["offsettinglineitemid"] != null)
                    ViewState["OFFSETTINGLINEITEMID"] = Request.QueryString["offsettinglineitemid"].ToString();

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
                                  , "Balnce Voucher Amount ","Voucher Status","UserName", "Target Company","Long Description" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        
        DataSet ds = PhoenixAccountsOffSettingEntries.InterCompanyVoucherSearch(
                                                              PhoenixSecurityContext.CurrentSecurityContext.CompanyID
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
                                                              , null
                                                              , sortexpression, sortdirection
                                                              , (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                              , ref iRowCount, ref iTotalPageCount
                                                              , 767
                                                              , 1
                                                          );       

        Response.AddHeader("Content-Disposition", "attachment; filename=InterCompanyPendingEntriesList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Inter-Company Pending Voucher List</h3></td>");
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
    protected void MenuInterCompanyList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDVOUCHERLINEITEMNO", "FLDACCOUNTCODE", "FLDREFERENCEDOCUMENTNO", "FLDCURRENCYNAME", "FLDTRANSACTIONAMOUNT", "FLDPREVIOUSVOUCHERAMOUNT"
                                 , "FLDBALANCEAMOUNT", "FLDVOUCHERSTATUS", "FLDCREATEDBYUSERNAME", "FLDSHORTCODE", "FLDLONGDESCRIPTION"};

        string[] alCaptions = { "Voucher Number","Voucher LineItemNo", "Account Code", "Reference Number", "Currency Code", "Prime Amount", "Previous Voucher Amount  "
                                  , "Balnce Voucher Amount ","Voucher Status","UserName", "Target Company","Long Description" };

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
                                              , 1
                                      );
        }
        else
        {
            ds = PhoenixAccountsOffSettingEntries.InterCompanyVoucherSearch(
                                              PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                            , null, null, null, null, null, null, null, null, null, null
                                            , null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                                            , gvInterCompanyList.PageSize, ref iRowCount, ref iTotalPageCount
                                            , 767, 1
                                     );
        }
        General.SetPrintOptions("gvInterCompanyPendingEntriesList", "Inter-Company Pending Voucher List", alCaptions, alColumns, ds);

        gvInterCompanyList.DataSource = ds;
        gvInterCompanyList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
           
            if (ViewState["OFFSETTINGLINEITEMID"] == null )
            {                
                ViewState["OFFSETTINGLINEITEMID"] = ds.Tables[0].Rows[0]["FLDVOUCHERLINEITEMID"].ToString();
                ViewState["OFFSETTINGVOUCHERID"] = ds.Tables[0].Rows[0]["FLDVOUCHERID"].ToString();
             
            }
            SetRowSelection();
           // BindPageURL(0);
       }
    }

 
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
   
   

 

    //protected void gvInterCompanyList_RowDataBound(object sender, GridViewRowEventArgs e)
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
    //protected void gvInterCompanyList_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;
    //    BindData();
    //    SetPageNavigator();
    //}
    //protected void gvInterCompanyList_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    gvInterCompanyList.SelectedIndex = se.NewSelectedIndex;       
    //    BindPageURL(se.NewSelectedIndex);
    //}
    //protected void gvInterCompanyList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    gvInterCompanyList.SelectedIndexes.Add(e.NewSelectedIndex);
    //    BindPageURL(e.NewSelectedIndex);
    //}

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

   

    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvInterCompanyList.Items[rowindex];
            RadLabel lblVoucherLineId = ((RadLabel)gvInterCompanyList.Items[rowindex].FindControl("lblVoucherLineId"));
            if (lblVoucherLineId != null)
                ViewState["OFFSETTINGLINEITEMID"] = lblVoucherLineId.Text.ToString();                                  
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    
    private void SetRowSelection()
    {
        gvInterCompanyList.SelectedIndexes.Clear();
        if (ViewState["OFFSETTINGLINEITEMID"] != null)
        {
            foreach (GridDataItem item in gvInterCompanyList.Items)
            {
                if (item.GetDataKeyValue("FLDVOUCHERLINEITEMID").ToString().Equals(ViewState["OFFSETTINGLINEITEMID"].ToString()))
                {
                    gvInterCompanyList.SelectedIndexes.Add(item.ItemIndex);
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
