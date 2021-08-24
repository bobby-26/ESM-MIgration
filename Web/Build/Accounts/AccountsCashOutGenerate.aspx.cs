using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Collections;
using Telerik.Web.UI;


public partial class AccountsCashOutGenerate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsCashOutGenerate.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVoucherDetails')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsInvoicePaymentVoucherFilter.aspx?source=cashoutgenerate", "Find", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //  MenuOrderForm.SetTrigger(pnlOrderForm);

            if (!IsPostBack)
            {
                Session["New"] = "N";
                if (Session["CHECKED_ITEMS"] != null)
                    Session.Remove("CHECKED_ITEMS");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["voucherid"] = null;
                ViewState["PAGEURL"] = null;

                gvVoucherDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Generate Request for All Payment Vouchers", "GENERATEALL", ToolBarDirection.Right);
            toolbarmain.AddButton("Generate Request", "NEW", ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            //  MenuOrderFormMain.SetTrigger(pnlOrderForm);

            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVoucherDetails_SortCommand(object sender, GridSortCommandEventArgs e)
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
            if (CommandName.ToUpper().Equals("NEW"))
            {
                ArrayList SelectedPvs = new ArrayList();
                string selectedagents = ",";
                if (Session["CHECKED_ITEMS"] != null)
                {
                    SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
                    if (SelectedPvs != null && SelectedPvs.Count > 0)
                    {
                        foreach (Guid index in SelectedPvs)
                        { selectedagents = selectedagents + index + ","; }
                    }
                }

                if (SelectedPvs.Count > 0)
                {
                    PhoenixAccountsCashOut.CashOutGenerate(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, PhoenixSecurityContext.CurrentSecurityContext.UserCode, selectedagents.Length > 1 ? selectedagents : null);
                    gvVoucherDetails.Rebind();
                }
                else
                {
                    ucError.ErrorMessage = "Please select Paymentment Voucher.";
                    ucError.Visible = true;
                    return;
                }
            }

            if (CommandName.ToUpper().Equals("GENERATEALL"))
            {
                PhoenixAccountsCashOut.CashOutGenerate(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                gvVoucherDetails.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        Response.Redirect("../Accounts/AccountsCashOutGenerate.aspx");
    }


    
    //protected void CheckAll(Object sender, EventArgs e)
    //{
    //    string[] ctl = Request.Form.GetValues("__EVENTTARGET");

    //    if (ctl != null && ctl[0].ToString() == "gvVoucherDetails$ctl01$chkAllCashOut")
    //    {
    //        CheckBox chkAll = (CheckBox)gvVoucherDetails.HeaderRow.FindControl("chkAllCashOut");
    //        foreach (GridViewRow row in gvVoucherDetails.Rows)
    //        {
    //            CheckBox cbSelected = (CheckBox)row.FindControl("chkSelect");
    //            if (cbSelected != null)
    //            {
    //                if (chkAll.Checked == true)
    //                {
    //                    cbSelected.Checked = true;
    //                }
    //                else
    //                {
    //                    cbSelected.Checked = false;
    //                }
    //            }
    //        }
    //    }
    //}

    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvVoucherDetails$ctl00$ctl02$ctl01$chkAllCashOut")
        {

            GridHeaderItem headerItem = gvVoucherDetails.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkAllCashOut");
            foreach (GridDataItem row in gvVoucherDetails.MasterTableView.Items)
            {
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("chkSelect");
                if (cbSelected != null)
                {
                    if (chkAll.Checked == true)
                    {
                        cbSelected.Checked = true;
                    }
                    else
                    {
                        cbSelected.Checked = false;
                    }
                }
           }
        }
    }
   
    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedPvs = new ArrayList();
        Guid index = new Guid();
        int i = 0;
        foreach (GridDataItem gvrow in gvVoucherDetails.MasterTableView.Items)
        {
            bool result = false;
            index = new Guid(gvVoucherDetails.MasterTableView.Items[i].GetDataKeyValue("FLDPAYMENTVOUCHERID").ToString());
        
            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }
           // Check in the Session
           if (Session["CHECKED_ITEMS"] != null)
                SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!SelectedPvs.Contains(index))
                    SelectedPvs.Add(index);
            }
            else
                SelectedPvs.Remove(index);
            i = i + 1;
        }
        if (SelectedPvs != null && SelectedPvs.Count > 0)
            Session["CHECKED_ITEMS"] = SelectedPvs;
    }

    private void GetSelectedPvs()
    {
        if (Session["CHECKED_ITEMS"] != null)
        {
            ArrayList SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
            Guid index = new Guid();
            if (SelectedPvs != null && SelectedPvs.Count > 0)
            {
                foreach (GridDataItem gvrow in gvVoucherDetails.MasterTableView.Items)
                {
                  RadCheckBox chk = (RadCheckBox)(gvrow.Cells[0].FindControl("chkSelect"));
                   index = new Guid(gvVoucherDetails.MasterTableView.Items[0].GetDataKeyValue("FLDPAYMENTVOUCHERID").ToString());
                    if (SelectedPvs.Contains(index))
                    {
                        RadCheckBox myCheckBox = (RadCheckBox)gvrow.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }
    }
   
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency", "Amount" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDAMOUNT" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsCashOut.CashOutPaymentVoucherSearch("", null
                                                                      , null
                                                                      , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                      , 48
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
    protected void gvVoucherDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string voucherfromdate = General.GetNullableDateTime("txtVoucherFromdateSearch").ToString().Trim();
        string vouchertodate = General.GetNullableDateTime("txtVoucherTodateSearch").ToString().Trim();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (Filter.CurrentInvoicePaymentVoucherSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentInvoicePaymentVoucherSelection;
            ds = PhoenixAccountsCashOut.CashOutPaymentVoucherSearch(General.GetNullableString(nvc.Get("txtVoucherNumberSearch").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("txtMakerId").ToString().Trim())
                    , General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim())
                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                    , 48
                    //, General.GetNullableString(nvc.Get("txtVoucherFromdateSearch").ToString().Trim())
                    //, General.GetNullableString(nvc.Get("txtVoucherTodateSearch").ToString().Trim())
                    , voucherfromdate
                    , vouchertodate
                    , General.GetNullableInteger(nvc.Get("ucTechFleet").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtInvoiceNumber").ToString().Trim())
                    , General.GetNullableString(nvc.Get("txtPurchaseInvoiceVoucherNumber").ToString().Trim())
                    , null
                    , sortexpression, sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , gvVoucherDetails.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

        }
        else
        {
            ds = PhoenixAccountsCashOut.CashOutPaymentVoucherSearch("", null, null
                                       , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                       , 48
                                       , string.Empty
                                       , string.Empty
                                       , null
                                       , null
                                       , null
                                       , null
                                       , sortexpression, sortdirection
                                       , (int)ViewState["PAGENUMBER"]
                                       , gvVoucherDetails.PageSize
                                       , ref iRowCount, ref iTotalPageCount);
        }




        string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency", "Amount" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDAMOUNT" };

        General.SetPrintOptions("gvVoucherDetails", "Invoice Voucher", alCaptions, alColumns, ds);

        gvVoucherDetails.DataSource = ds;
        gvVoucherDetails.VirtualItemCount = iRowCount;
      
        if (ds.Tables[0].Rows.Count > 0)
        {
           

            if (ViewState["voucherid"] == null)
            {
                ViewState["voucherid"] = ds.Tables[0].Rows[0]["FLDPAYMENTVOUCHERID"].ToString();
            }
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

 


    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();

    }


    //protected void SaveCheckedValues(Object sender, EventArgs e)
    //{
    //    ArrayList SelectedPvs = new ArrayList();
    //    Guid index = new Guid();
    //    foreach (GridViewRow gvrow in gvVoucherDetails.Rows)
    //    {
    //        bool result = false;
    //        index = new Guid(gvVoucherDetails.DataKeys[gvrow.RowIndex].Value.ToString());

    //        if (((CheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
    //        {
    //            result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
    //        }

    //        // Check in the Session
    //        if (Session["CHECKED_ITEMS"] != null)
    //            SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
    //        if (result)
    //        {
    //            if (!SelectedPvs.Contains(index))
    //                SelectedPvs.Add(index);
    //        }
    //        else
    //            SelectedPvs.Remove(index);
    //    }
    //    if (SelectedPvs != null && SelectedPvs.Count > 0)
    //        Session["CHECKED_ITEMS"] = SelectedPvs;
    //}

    //private void GetSelectedPvs()
    //{
    //    if (Session["CHECKED_ITEMS"] != null)
    //    {
    //        ArrayList SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
    //        Guid index = new Guid();
    //        if (SelectedPvs != null && SelectedPvs.Count > 0)
    //        {
    //            foreach (GridViewRow row in gvVoucherDetails.Rows)
    //            {
    //                CheckBox chk = (CheckBox)(row.Cells[0].FindControl("chkSelect"));
    //                index = new Guid(gvVoucherDetails.DataKeys[row.RowIndex].Value.ToString());
    //                if (SelectedPvs.Contains(index))
    //                {
    //                    CheckBox myCheckBox = (CheckBox)row.FindControl("chkSelect");
    //                    myCheckBox.Checked = true;
    //                }
    //            }
    //        }
    //    }
    //}


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvVoucherDetails.Rebind();
    }
}
