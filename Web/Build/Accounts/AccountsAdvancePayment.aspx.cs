using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsAdvancePayment : PhoenixBasePage
{
    string status;
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

            status = "582,583,968,1233";

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsAdvancePayment.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvAdvancePayment')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsAdvancePaymentFilter.aspx?qcalfrom=INVOICE&status=" + Request.QueryString.Count, "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsAdvancePaymentAdvancedFilter.aspx?qcalfrom=INVOICE&status=" + Request.QueryString.Count, "Advance Find", "search.png", "FINDADVANCED");
            toolbargrid.AddImageButton("../Accounts/AccountsAdvancePayment.aspx", "Clear", "clear-filter.png", "CLEAR");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();            

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ADVANCEPAYMENTID"] = null;
                ViewState["PAGEURL"] = null;                

                DateTime now = DateTime.Now;

                gvAdvancePayment.PageSize = General.ShowRecords(gvAdvancePayment.PageSize);

                //string FromDate = now.Date.AddMonths(-1).ToShortDateString();
                //string ToDate = DateTime.Now.ToShortDateString();

                //ViewState["FROMDATE"] = FromDate.ToString();
                //ViewState["TODATE"] = ToDate.ToString();

                if (Request.QueryString["ADVANCEPAYMENTID"] != null)
                {
                    ViewState["ADVANCEPAYMENTID"] = Request.QueryString["ADVANCEPAYMENTID"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdvancePaymentGeneral.aspx?ADVANCEPAYMENTID=" + ViewState["ADVANCEPAYMENTID"];
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAdvancePayment_Sorting(object sender, GridSortCommandEventArgs se)
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
        gvAdvancePayment.Rebind();
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdvancePaymentGeneral.aspx?ADVANCEPAYMENTID=" + ViewState["ADVANCEPAYMENTID"];
                ViewState["PAGEURL"] = "../Accounts/AccountsAdvancePaymentGeneral.aspx?ADVANCEPAYMENTID=" + ViewState["ADVANCEPAYMENTID"];
            }
            if (CommandName.ToUpper().Equals("ATTACHMENTS"))
            {
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.ACCOUNTS;
                ViewState["PAGEURL"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.ACCOUNTS;
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

        string[] alColumns = { "FLDADVANCEPAYMENTNUMBER", "FLDAMOUNT", "FLDNAME", "FLDTYPEDESCRIPTION", "FLDREFERENCEDOCUMENT", "FLDPAYDATE", "FLDCURRENCYCODE", "FLDHARDNAME", "FLDVESSELNAME", "FLDPAYMENTVOUCHERNUMBER", "FLDINVOICENUMBER", "FLDINVOICESTATUSNAME", "FLDCREDITNOTEVOUCHERNO", "FLDFUNDSTATUS" };
        string[] alCaptions = { "Number", "Payment Amount", "Supplier", "Type", "Payment Reference", "Date", "Currency", "Payment Status", "Vessel Involved", "Payment Voucher", "Invoice number", "Invoice status", "Credit Note Voucher No", "Fund Status" }; 
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentAdvancePaymentSelection;
        if (nvc == null || nvc.Get("FindType").ToString().ToUpper().Equals("FIND"))
        {
            ds = PhoenixAccountsAdvancePayment.AdvancePaymentSearch(
                                                                nvc != null ? General.GetNullableString(nvc.Get("txtPaymentNumber")) : null
                                                              , null
                                                              , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                              , nvc != null ? General.GetNullableString(nvc.Get("txtSupplierReference")) : null
                                                              , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentFromdate")) : null
                                                              , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentTodate")) : null
                                                              , sortexpression
                                                              , sortdirection
                                                              , gvAdvancePayment.CurrentPageIndex + 1
                                                              , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                              , ref iRowCount
                                                              , ref iTotalPageCount
                                                              , nvc != null ? General.GetNullableInteger(nvc.Get("txtSupplierCode")) : null);

        }
        else
        {
            if(General.GetNullableInteger(nvc.Get("chkPOCancelled")) != 1)
            { 
                ds = PhoenixAccountsAdvancePayment.AdvancePaymentSearch(
                                                                    nvc != null ? General.GetNullableString(nvc.Get("txtPaymentNumber")) : null
                                                                  , null
                                                                  , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                  , nvc != null ? General.GetNullableString(nvc.Get("txtSupplierReference")) : null
                                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
                                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("ucPaymentStatus")) : null
                                                                  , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentFromdate")) : null
                                                                  , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentTodate")) : null
                                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("ucType")) : null
                                                                  , sortexpression
                                                                  , sortdirection
                                                                  , gvAdvancePayment.CurrentPageIndex + 1
                                                                  , iRowCount
                                                                  , ref iRowCount
                                                                  , ref iTotalPageCount
                                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("txtSupplierCode")) : null
                                                                  , status
                                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("ucInvoiceStatus")) : null                                                                  
                                                                  );
            }
            else
            {
                ds = PhoenixAccountsAdvancePayment.AdvancePaymentPOCancelledSearch(
                                                                    nvc != null ? General.GetNullableString(nvc.Get("txtPaymentNumber")) : null
                                                                  , null
                                                                  , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                  , nvc != null ? General.GetNullableString(nvc.Get("txtSupplierReference")) : null
                                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
                                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("ucPaymentStatus")) : null
                                                                  , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentFromdate")) : null
                                                                  , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentTodate")) : null
                                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("ucType")) : null
                                                                  , sortexpression
                                                                  , sortdirection
                                                                  , gvAdvancePayment.CurrentPageIndex + 1
                                                                  , iRowCount
                                                                  , ref iRowCount
                                                                  , ref iTotalPageCount
                                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("txtSupplierCode")) : null
                                                                  , status
                                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("ucInvoiceStatus")) : null
                                                                  , nvc != null ? General.GetNullableInteger(nvc.Get("chkPOCancelled")) : null
                                                                  );
            }
        }
        Response.AddHeader("Content-Disposition", "attachment; filename=AdvancePayment.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Advance Payment</h3></td>");
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
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                
                Filter.CurrentAdvancePaymentSelection = null;
                BindData();
                gvAdvancePayment.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAdvancePayment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }


    private void BindData()
    {

        string[] alColumns = { "FLDADVANCEPAYMENTNUMBER", "FLDAMOUNT", "FLDNAME", "FLDTYPEDESCRIPTION", "FLDREFERENCEDOCUMENT", "FLDPAYDATE", "FLDCURRENCYCODE", "FLDHARDNAME", "FLDVESSELNAME", "FLDPAYMENTVOUCHERNUMBER", "FLDINVOICENUMBER", "FLDINVOICESTATUSNAME", "FLDCREDITNOTEVOUCHERNO", "FLDFUNDSTATUS" };
        string[] alCaptions = { "Number", "Payment Amount", "Supplier", "Type", "Payment Reference", "Date", "Currency", "Payment Status", "Vessel Involved", "Payment Voucher", "Invoice number", "Invoice status", "Credit Note Voucher No", "Fund Status"  };
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentAdvancePaymentSelection;

        if (nvc == null || nvc.Get("FindType").ToString().ToUpper().Equals("FIND"))
        {
            ds = PhoenixAccountsAdvancePayment.AdvancePaymentSearch(
                                                                 nvc != null ? General.GetNullableString(nvc.Get("txtPaymentNumber")) : null
                                                               , null
                                                               , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                               , nvc != null ? General.GetNullableString(nvc.Get("txtSupplierReference")) : null
                                                               , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentFromdate")) : null
                                                               , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentTodate")) : null
                                                               , sortexpression
                                                               , sortdirection
                                                               , gvAdvancePayment.CurrentPageIndex + 1
                                                               , gvAdvancePayment.PageSize
                                                               , ref iRowCount
                                                               , ref iTotalPageCount
                                                               , nvc != null ? General.GetNullableInteger(nvc.Get("txtSupplierCode")) : null
                                                               //, nvc != null ? General.GetNullableString(nvc.Get("ucVessel")) : null
                                                               );
        }
        else
        {
            if (General.GetNullableInteger(nvc.Get("chkPOCancelled")) != 1)
            { 
                ds = PhoenixAccountsAdvancePayment.AdvancePaymentSearch(
                                                                 nvc != null ? General.GetNullableString(nvc.Get("txtPaymentNumber")) : null
                                                               , null
                                                               , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                               , nvc != null ? General.GetNullableString(nvc.Get("txtSupplierReference")) : null
                                                               , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
                                                               , nvc != null ? General.GetNullableInteger(nvc.Get("ucPaymentStatus")) : null
                                                               , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentFromdate")) : null
                                                               , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentTodate")) : null
                                                               , nvc != null ? General.GetNullableInteger(nvc.Get("ucType")) : null
                                                               , sortexpression
                                                               , sortdirection
                                                               , gvAdvancePayment.CurrentPageIndex + 1
                                                               , gvAdvancePayment.PageSize
                                                               , ref iRowCount
                                                               , ref iTotalPageCount
                                                               , nvc != null ? General.GetNullableInteger(nvc.Get("txtSupplierCode")) : null
                                                               , status
                                                               , nvc != null ? General.GetNullableString(nvc.Get("ucVessel")) : null
                                                               , nvc != null ? General.GetNullableInteger(nvc.Get("ucInvoiceStatus")) : null                                                               
                                                               );
            }
            else
            {
                ds = PhoenixAccountsAdvancePayment.AdvancePaymentPOCancelledSearch(
                                                                 nvc != null ? General.GetNullableString(nvc.Get("txtPaymentNumber")) : null
                                                               , null
                                                               , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                               , nvc != null ? General.GetNullableString(nvc.Get("txtSupplierReference")) : null
                                                               , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
                                                               , nvc != null ? General.GetNullableInteger(nvc.Get("ucPaymentStatus")) : null
                                                               , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentFromdate")) : null
                                                               , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentTodate")) : null
                                                               , nvc != null ? General.GetNullableInteger(nvc.Get("ucType")) : null
                                                               , sortexpression
                                                               , sortdirection
                                                               , gvAdvancePayment.CurrentPageIndex + 1
                                                               , gvAdvancePayment.PageSize
                                                               , ref iRowCount
                                                               , ref iTotalPageCount
                                                               , nvc != null ? General.GetNullableInteger(nvc.Get("txtSupplierCode")) : null
                                                               , status
                                                               , nvc != null ? General.GetNullableString(nvc.Get("ucVessel")) : null
                                                               , nvc != null ? General.GetNullableInteger(nvc.Get("ucInvoiceStatus")) : null
                                                               , nvc != null ? General.GetNullableInteger(nvc.Get("chkPOCancelled")) : null
                                                               );

            }
        }
        General.SetPrintOptions("gvAdvancePayment", "Advance Payment", alCaptions, alColumns, ds);

        gvAdvancePayment.DataSource = ds;
        gvAdvancePayment.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["ADVANCEPAYMENTID"] == null)
            {
                ViewState["ADVANCEPAYMENTID"] = ds.Tables[0].Rows[0]["FLDADVANCEPAYMENTID"].ToString();
                //gvAdvancePayment.SelectedIndex = 0;
            }

            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdvancePaymentGeneral.aspx?ADVANCEPAYMENTID=" + ViewState["ADVANCEPAYMENTID"].ToString();
            }            
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdvancePaymentGeneral.aspx";
            }
        }
        SetRowSelection();
    }

    private void SetRowSelection()
    {
        gvAdvancePayment.SelectedIndexes.Clear();

        foreach (GridDataItem item in gvAdvancePayment.Items)
        {

            if (item.GetDataKeyValue("FLDADVANCEPAYMENTID").ToString().Equals(ViewState["ADVANCEPAYMENTID"].ToString()))
            {
                gvAdvancePayment.SelectedIndexes.Add(item.ItemIndex);
                //PhoenixAccountsVoucher.VoucherNumber = ((RadLabel)gvAdvancePayment.Items[item.ItemIndex].FindControl("lblAdvancePaymentid")).Text;
            }
        }
    }

    protected void gvAdvancePayment_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            RadLabel lblTypeID = (RadLabel)e.Item.FindControl("lblTypeID");

            ImageButton cmdNoAtt = (ImageButton)e.Item.FindControl("cmdNoAtt");
            ImageButton cmdAtt = (ImageButton)e.Item.FindControl("cmdAtt");

            int? attachmentFlag = General.GetNullableInteger(drv["FLDATTACHMENTFLAG"].ToString());

            if (cmdAtt != null)
            {
                if (lblIsAtt.Text == "1")
                {
                    cmdAtt.Visible = true;
                    cmdNoAtt.Visible = false;
                }
                if (attachmentFlag == 0)
                {
                    cmdAtt.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.ACCOUNTS + "&DocSource=PARTPAID');return true;");
                }
                else
                {
                    cmdAtt.Attributes.Add("onclick", "openNewWindow('NAFA','','Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                   + PhoenixModule.ACCOUNTS + "&u=1&DocSource=PARTPAID');return true;");

                }
            }
            if (cmdNoAtt != null)
            {
                if (lblIsAtt.Text == string.Empty)
                {
                    cmdAtt.Visible = false;
                    cmdNoAtt.Visible = true;
                }
                if (attachmentFlag == 0)
                {
                    cmdNoAtt.Attributes.Add("onclick", "openNewWindow('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.ACCOUNTS + "&DocSource=PARTPAID');return true;");
                }
                else
                {
                    cmdNoAtt.Attributes.Add("onclick", "openNewWindow('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                   + PhoenixModule.ACCOUNTS + "&u=1&DocSource=PARTPAID');return true;");

                }
            }

            ImageButton cmdApprovalHistory = (ImageButton)e.Item.FindControl("cmdApprovalHistory");
            if (cmdApprovalHistory != null && drv["FLDTYPE"].ToString() == "583")
            {
                cmdApprovalHistory.Attributes.Add("onclick", "openNewWindow('codehelp','','Accounts/AccountsApprovalHistory.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "'); return false;");
            }
            else if (cmdApprovalHistory != null && drv["FLDTYPE"].ToString() == "1233")
            {
                cmdApprovalHistory.Attributes.Add("onclick", "openNewWindow('codehelp','','Accounts/AccountsApprovalHistory.aspx?potype=DirectPO&docid=" + drv["FLDORDERID"].ToString() + "'); return false;");
            }
            else if (cmdApprovalHistory != null && drv["FLDTYPE"].ToString() == "968")
            {
                cmdApprovalHistory.Attributes.Add("onclick", "openNewWindow('codehelp','','Accounts/AccountsApprovalHistory.aspx?potype=BondProvision&docid=" + drv["FLDORDERID"].ToString() + "'); return false;");
            }
            else 
            {
                cmdApprovalHistory.Attributes.Add("onclick", "openNewWindow('codehelp','','Accounts/AccountsApprovalHistory.aspx?docid=" + drv["FLDORDERPARTPAIDID"].ToString() + "'); return false;");
            }

            if (!e.Item.ItemType.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.ItemType.Equals(DataControlRowState.Edit))
            {
                LinkButton ll = (LinkButton)e.Item.FindControl("lblLeave");
                RadLabel lb = (RadLabel)e.Item.FindControl("lblOrderid");
                RadLabel l = (RadLabel)e.Item.FindControl("lblTypeID");
                RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
                if (!SessionUtil.CanAccess(this.ViewState, ll.CommandName)) ll.Visible = false;
                if (l.Text.Equals(PhoenixCommonRegisters.GetHardCode(1, 124, "LIC")))
                    ll.Attributes.Add("onclick", "openNewWindow('codehelp','','Crew/CrewLicenceRequestLineItem.aspx?orderid=" + lb.Text + "&refno=" + ll.Text + "');return false;");
                if (l.Text.Equals(PhoenixCommonRegisters.GetHardCode(1, 124, "SAS")))
                    ll.Attributes.Add("onclick", "openNewWindow('codehelp1','','Purchase/PurchaseFormLineItem.aspx?orderid=" + lb.Text + "&refno=" + ll.Text + " +&vesselid=" + lblVesselId.Text + "');return false;");

            }

        }
    }
    protected void gvAdvancePayment_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            int iRowno = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            if (e.CommandName.ToUpper().Equals("ROWCLICK") || e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                Guid documentid = new Guid(item.GetDataKeyValue("FLDADVANCEPAYMENTID").ToString());
                gvAdvancePayment.SelectedIndexes.Add(e.Item.ItemIndex);
                BindPageURL(e.Item.ItemIndex);
            }

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(iRowno);
                SetRowSelection();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvAdvancePayment.Rebind();
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
            GridDataItem item = (GridDataItem)gvAdvancePayment.Items[rowindex];
            //ViewState["ADVANCEPAYMENTID"] = item.GetDataKeyValue("FLDADVANCEPAYMENTID");
            ViewState["ADVANCEPAYMENTID"] = ((RadLabel)gvAdvancePayment.Items[rowindex].FindControl("lblAdvancePaymentid")).Text;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAdvancePaymentGeneral.aspx?ADVANCEPAYMENTID=" + ViewState["ADVANCEPAYMENTID"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvAdvancePayment_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    gvAdvancePayment.SelectedIndexes.Add(e.NewSelectedIndex);
    //    BindPageURL(e.NewSelectedIndex);
    //}
}
