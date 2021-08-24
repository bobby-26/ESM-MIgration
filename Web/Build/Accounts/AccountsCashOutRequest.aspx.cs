using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class AccountsCashOutRequest : PhoenixBasePage
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

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Line Items", "LINEITEMS", ToolBarDirection.Right);
            toolbarmain.AddButton("Cash Request", "CASHREQ", ToolBarDirection.Right);

            //toolbarmain.AddButton("Submit for MD Approval", "SUBMITFORMDAPPROVAL");


            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            //  MenuOrderFormMain.SetTrigger(pnlRemittance);
            MenuOrderFormMain.SelectedMenuIndex = 1;

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsCashOutRequest.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvCashOut')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsCashFilter.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsCashOutRequest.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //   MenuOrderForm.SetTrigger(pnlRemittance);

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REMITTENCEID"] = null;


                gvCashOut.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["cashpaymentid"] != null)
                {
                    ViewState["cashpaymentid"] = Request.QueryString["cashpaymentid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCashOutRequestGeneral.aspx?cashpaymentid=" + ViewState["cashpaymentid"];
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
    protected void gvCashOut_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvCashOut.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
    }
    protected void gvCashOut_SortCommand(object sender, GridSortCommandEventArgs e)
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

            if (CommandName.ToUpper().Equals("CASHREQ"))
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCashOutRequestGeneral.aspx?cashpaymentid=" + ViewState["cashpaymentid"];
            }
            if (CommandName.ToUpper().Equals("LINEITEMS") && ViewState["cashpaymentid"] != null && ViewState["cashpaymentid"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsCashOutRequestLineItem.aspx?cashpaymentid=" + ViewState["cashpaymentid"]);
            }
            if (CommandName.ToUpper().Equals("SUBMITFORMDAPPROVAL"))
            {
                PhoenixAccountsCashOut.CashOutAwaitingApprovalUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, null);
                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
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

        string[] alCaptions = { "Cash Request ", "Supplier ", "Code", "Payment mode", "Cash Account", "Currency", "Amount", "Source", "Status" };
        string[] alColumns = { "FLDCASHPAYMENTNUMBER", "FLDSUPPLIERNAME", "FLDSUPPLIERCODE", "FLDPAYMENTTYPE", "FLDDESCRIPTION", "FLDCURRENCYCODE", "FLDCASHPAYMENTAMOUNT", "FLDSOURCE", "FLDCASHPAYMENTSTATUS" };


        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        if (Filter.CurrentCashOutSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentCashOutSelection;
            ds = PhoenixAccountsCashOut.CashOutSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, General.GetNullableString(nvc.Get("txtCashNumberSearch").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim())
                    , null
                    , null
                    , General.GetNullableInteger(nvc.Get("ucCashStatus").ToString().Trim())
                    , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount,
                   General.GetNullableInteger(nvc.Get("txtuserid").ToString().Trim()),
                   General.GetNullableInteger(nvc.Get("txtAccountId").ToString().Trim()));
        }
        else
        {
            ds = PhoenixAccountsCashOut.CashOutSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, null, null, null, null, null
                                                       , sortexpression, sortdirection
                                                       , (int)ViewState["PAGENUMBER"]
                                                       , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                       , ref iRowCount, ref iTotalPageCount, null);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=CashRequest.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Cash Request</h3></td>");
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

            if (CommandName.ToUpper().Equals("CLEAR"))
            {

                Filter.CurrentCashOutSelection = null;
                //  txtnopage.Text = string.Empty;
                ViewState["PAGENUMBER"] = 1;
                Rebind();



            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCashOut.SelectedIndexes.Clear();
        gvCashOut.EditIndexes.Clear();
        gvCashOut.DataSource = null;
        gvCashOut.Rebind();
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

        if (Filter.CurrentCashOutSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentCashOutSelection;
            ds = PhoenixAccountsCashOut.CashOutSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, General.GetNullableString(nvc.Get("txtCashNumberSearch").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim())
                    , null
                    , null
                    , General.GetNullableInteger(nvc.Get("ucCashStatus").ToString().Trim())
                    , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvCashOut.PageSize, ref iRowCount, ref iTotalPageCount,
                   General.GetNullableInteger(nvc.Get("txtuserid").ToString().Trim()),
                   General.GetNullableInteger(nvc.Get("txtAccountId").ToString().Trim()));
        }
        else
        {

            ds = PhoenixAccountsCashOut.CashOutSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, null, null, null, null, null
                                                       , sortexpression, sortdirection
                                                       , (int)ViewState["PAGENUMBER"]
                                                       , gvCashOut.PageSize
                                                       , ref iRowCount, ref iTotalPageCount, null);
        }

        string[] alCaptions = { "Cash Request ", "Supplier ", "Code", "Payment mode", "Cash Account", "Currency", "Amount", "Source", "Status" };
        string[] alColumns = { "FLDCASHPAYMENTNUMBER", "FLDSUPPLIERNAME", "FLDSUPPLIERCODE", "FLDPAYMENTTYPE", "FLDDESCRIPTION", "FLDCURRENCYCODE", "FLDCASHPAYMENTAMOUNT", "FLDSOURCE", "FLDCASHPAYMENTSTATUS" };

        General.SetPrintOptions("gvCashOut", "Cash Request", alCaptions, alColumns, ds);

        gvCashOut.DataSource = ds;
        gvCashOut.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ViewState["cashpaymentid"] == null)
            {
                ViewState["cashpaymentid"] = ds.Tables[0].Rows[0]["FLDCASHPAYMENTID"].ToString();
                //gvCashOut.SelectedIndex = 0;
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCashOutRequestGeneral.aspx?cashpaymentid=" + ViewState["cashpaymentid"].ToString();
            }
            SetRowSelection();
            //  BindPageURL(0);
        }
        else
        {

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCashOutRequestGeneral.aspx";
            }
        }

    }




    protected void gvCashOut_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCashOut.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCashOut_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                //  GridDataItem item = (GridDataItem)e.Item;
                DataRowView drv = (DataRowView)e.Item.DataItem;


                ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (drv["FLDISATTACHMENT"].ToString() == "0")
                        att.ImageUrl = Session["images"] + "/no-attachment.png";
                    //att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    //    + PhoenixModule.ACCOUNTS + "'); return false;");
                    att.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                      + PhoenixModule.ACCOUNTS + "');return true;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvCashOut_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            if (ViewState["SORTEXPRESSION"] != null)
    //            {
    //                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //                if (img != null)
    //                {
    //                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                        img.Src = Session["images"] + "/arrowUp.png";
    //                    else
    //                        img.Src = Session["images"] + "/arrowDown.png";

    //                    img.Visible = true;
    //                }
    //            }
    //        }

    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
    //            if (att != null)
    //            {
    //                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
    //                if (drv["FLDISATTACHMENT"].ToString() == "0")
    //                    att.ImageUrl = Session["images"] + "/no-attachment.png";
    //                att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
    //                    + PhoenixModule.ACCOUNTS + "'); return false;");
    //            }
    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();

    }


    private void SetRowSelection()
    {
        gvCashOut.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvCashOut.Items)
        {
            if (item.GetDataKeyValue("FLDCASHPAYMENTID").ToString().Equals(ViewState["cashpaymentid"].ToString()))
            {
                gvCashOut.SelectedIndexes.Add(item.ItemIndex);
                PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvCashOut.Items[item.ItemIndex].FindControl("lnkCashOutid")).Text;
            }
        }
    }



    //private void SetRowSelection()
    //{
    //    gvCashOut.SelectedIndex = -1;
    //    for (int i = 0; i < gvCashOut.Rows.Count; i++)
    //    {
    //        if (gvCashOut.DataKeys[i].Value.ToString().Equals(ViewState["cashpaymentid"].ToString()))
    //        {
    //            gvCashOut.SelectedIndex = i;
    //            PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvCashOut.Rows[i].FindControl("lnkCashOutid")).Text;
    //        }
    //    }
    //}


    protected void gvCashOut_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
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
            if (e.CommandName.ToUpper().Equals("PRINT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int nCurrentRow = e.Item.ItemIndex;
                BindPageURL(nCurrentRow);
                SetRowSelection();
                String scriptpopup = String.Format(
                        "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=CASHREQUEST&cashpaymentid=" + ViewState["cashpaymentid"].ToString() + "');");

                //cmdMoreInfo.Attributes.Add("onclick", "openNewWindow('codeHelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsInvoiceMoreInfo.aspx?invoiceCode=" + txtInvoiceCode.Text + "');return false;");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }



    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvCashOut.Items[rowindex];
            ViewState["cashpaymentid"] = ((RadLabel)gvCashOut.Items[rowindex].FindControl("lblCashOutId")).Text;
            PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvCashOut.Items[rowindex].FindControl("lnkCashOutid")).Text;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCashOutRequestGeneral.aspx?cashpaymentid=" + ViewState["cashpaymentid"].ToString();
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
    //        ViewState["cashpaymentid"] = ((Label)gvCashOut.Rows[rowindex].FindControl("lblCashOutId")).Text;
    //        PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvCashOut.Rows[rowindex].FindControl("lnkCashOutid")).Text;
    //        ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCashOutRequestGeneral.aspx?cashpaymentid=" + ViewState["cashpaymentid"].ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    //{
    //    BindData();
    //    SetPageNavigator();
    //    if (Session["New"].ToString() == "Y")
    //    {
    //        gvCashOut.SelectedIndex = 0;
    //        Session["New"] = "N";
    //        BindPageURL(gvCashOut.SelectedIndex);
    //    }
    //}
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();

        if (Session["New"].ToString() == "Y")
        {
            Session["New"] = "N";
            BindPageURL(0);
        }
    }


}
