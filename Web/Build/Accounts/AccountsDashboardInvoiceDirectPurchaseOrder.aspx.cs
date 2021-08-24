using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Accounts_AccountsDashboardInvoiceDirectPurchaseOrder : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // string pni = ViewState["ISPNI"].ToString();
            SessionUtil.PageAccessRights(this.ViewState);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");
            if (!IsPostBack)
            {
                ifMoreInfo.Attributes["src"] = "AccountsInvoiceDirectPurchaseOrderGeneral.aspx?qinvoicecode=" + ViewState["INVOICECODE"] + "&ispni=" + ViewState["ISPNI"];
                ViewState["VESSELID"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ORDERID"] = null;
                gvDPO.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (ViewState["ORDERID"] != null)
                EnableVisibleTab();
            else
            {
                toolbar.AddButton("General", "GENERAL");
                toolbar.AddButton("P&I Case", "PNICASE");
                toolbar.AddButton("Line Item", "LINEITEM");
                toolbar.AddButton("History", "HISTORY");
                MenuDPO.AccessRights = this.ViewState;
                MenuDPO.MenuList = toolbar.Show();

                MenuDPO.SelectedMenuIndex = 0;
            }


            //DateTime now = DateTime.Now;

            //string FromDate = now.Date.AddMonths(-1).ToShortDateString();
            //string ToDate = DateTime.Now.ToShortDateString();

            //ViewState["FROMDATE"] = FromDate.ToString();
            //ViewState["TODATE"] = ToDate.ToString();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceDirectPurchaseOrder.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvDPO')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsDirectPOFilter.aspx", "Find", "search.png", "FIND");
            //toolbargrid.AddImageLink("javascript:parent.Openpopup('codehelp1','','AccountsAmosInvoiceLineItem.aspx?qinvoicecode=" + ViewState["INVOICECODE"].ToString() + "'); return false;", "Add", "Add.png", "ADD");
            MenuAddAmosPO.AccessRights = this.ViewState;
            MenuAddAmosPO.MenuList = toolbargrid.Show();
            if (ViewState["ISPNI"] != null && ViewState["ISPNI"].ToString() == "0")
                MenuDPO.FindControl("PNICASE").EnableViewState = false;
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDPO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = "AccountsInvoiceDirectPurchaseOrderGeneral.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?qinvoicecode=" + ViewState["INVOICECODE"] + (ViewState["ORDERID"] != null ? "&orderid=" + ViewState["ORDERID"] + "&vslid=" + ViewState["VESSELID"] : string.Empty);
            }
            else if (CommandName.ToUpper().Equals("LINEITEM") && ViewState["ORDERID"] != null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = "AccountsInvoiceDirectPurchaseOrderLineItem.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?qinvoicecode=" + ViewState["INVOICECODE"] + (ViewState["ORDERID"] != null ? "&orderid=" + ViewState["ORDERID"] + "&vslid=" + ViewState["VESSELID"] : string.Empty);
            }
            else if (CommandName.ToUpper().Equals("PNICASE") && ViewState["ORDERID"] != null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = "AccountsDirectPOPISplit.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?qinvoicecode=" + ViewState["INVOICECODE"] + (ViewState["ORDERID"] != null ? "&orderid=" + ViewState["ORDERID"] + "&vslid=" + ViewState["VESSELID"] : string.Empty);
            }
            else if (CommandName.ToUpper().Equals("HISTORY") && ViewState["ORDERID"] != null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = "AccountsInvoiceDirectPurchaseHistory.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?orderid=" + ViewState["ORDERID"];
            }
            else
                MenuDPO.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuAddAmosPO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

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
    public void BindData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alCaptions = {
                                "PO Number",
                                "Vendor",
                                "Vessel",
                                "Currency",
                                "PO Received Date",
                                "Po Status",
                                "Amount",
                                "Description",
                                "Invoice Number",
                                "Invoice Status"
                              };

            string[] alColumns = {  "FLDFORMNO",
                                "FLDNAME",
                                "FLDVESSELNAME",
                                "FLDCURRENCYNAME",
                                "FLDRECEIVEDDATE",
                                "FLDPOSTATUS",
                                "FLDESTIMATEAMOUNT",
                                "FLDDESCRIPTION",
                                "FLDINVOICENUMBER",
                                "FLDHARDNAME"
                             };


            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentDirectPOSelection;

            DataTable dt = PhoenixAccountsInvoice.InvoiceDirectPurchaseOrderFilterSearch(null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlInvoiceType")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtOrderNumber")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("ucVessel")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtReceivedFromdateSearch")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtReceivedTodateSearch")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlStatus")) : null
                , sortexpression, sortdirection
                , gvDPO.CurrentPageIndex + 1
                , gvDPO.PageSize, ref iRowCount, ref iTotalPageCount
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucPortMulti")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETAFrom")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETATO")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETDFrom")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETDTO")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtDescription")) : null
                );

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvDPO", "Direct PO", alCaptions, alColumns, ds);

            gvDPO.DataSource = dt;
            gvDPO.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            if (dt.Rows.Count > 0)
            {

                if (ViewState["ORDERID"] == null)
                {
                    ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
                    ViewState["ORDERID"] = dt.Rows[0]["FLDORDERID"].ToString();
                }
                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "AccountsInvoiceDirectPurchaseOrderGeneral.aspx";
                }
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"] + "?qinvoicecode=" + ViewState["INVOICECODE"] + "&orderid=" + ViewState["ORDERID"] + "&vslid=" + ViewState["VESSELID"];
                SetTabHighlight();
                SetRowSelection();
            }
            EnableVisibleTab();

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
                                "PO Number",
                                "Vendor",
                                "Vessel",
                                "Currency",
                                "PO Received Date",
                                "Po Status",
                                "Amount",
                                "Description",
                                "Invoice Number",
                                "Invoice Status"
                              };

        string[] alColumns = {  "FLDFORMNO",
                                "FLDNAME",
                                "FLDVESSELNAME",
                                "FLDCURRENCYNAME",
                                "FLDRECEIVEDDATE",
                                "FLDPOSTATUS",
                                "FLDESTIMATEAMOUNT",
                                "FLDDESCRIPTION",
                                "FLDINVOICENUMBER",
                                "FLDHARDNAME"
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

        NameValueCollection nvc = Filter.CurrentDirectPOSelection;

        DataTable dt = PhoenixAccountsInvoice.InvoiceDirectPurchaseOrderFilterSearch(null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlInvoiceType")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtOrderNumber")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("ucVessel")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtReceivedFromdateSearch")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtReceivedTodateSearch")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlStatus")) : null
                , sortexpression, sortdirection
                , gvDPO.CurrentPageIndex + 1
                , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucPortMulti")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETAFrom")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETATO")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETDFrom")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucETDTO")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtDescription")) : null
                );

        ds.Tables.Add(dt.Copy());

        Response.AddHeader("Content-Disposition", "attachment; filename=DirectPO.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Direct PO</h3></td>");
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            EnableVisibleTab();
            ViewState["ORDERID"] = null;
            gvDPO.CurrentPageIndex = 0;
            gvDPO.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDPO_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        try
        {
            if (e.Item is GridDataItem)
            {
                RadLabel lblIsPoTaggedToInvoice = (RadLabel)e.Item.FindControl("lblIsPoTaggedToInvoice");
                RadLabel lblOrderId = (RadLabel)e.Item.FindControl("lblOrderId");
                ImageButton cmdPOApprove = (ImageButton)e.Item.FindControl("cmdPOApprove");
                RadLabel lblVendorId = (RadLabel)e.Item.FindControl("lblVendorId");
            }
            if (e.Item is GridDataItem)
            {
                Guid? lblDtkey = General.GetNullableGuid(drv["FLDDTKEY"].ToString());
                int? attachmentFlag = General.GetNullableInteger(drv["FLDATTACHMENTFLAG"].ToString());
                ImageButton cmdAttachment = (ImageButton)e.Item.FindControl("cmdAttachment");
                if (cmdAttachment != null)
                {
                    if (attachmentFlag == 1)
                    {
                        cmdAttachment.Attributes.Add("onclick", "Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                            + PhoenixModule.ACCOUNTS + "&U=" + attachmentFlag.ToString() + "');return true;");
                    }
                    else
                    {
                        cmdAttachment.Attributes.Add("onclick", "Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                            + PhoenixModule.ACCOUNTS + "');return true;");
                    }
                    cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
                }
                ImageButton cmdNoAttachment = (ImageButton)e.Item.FindControl("cmdNoAttachment");
                if (cmdNoAttachment != null)
                {
                    if (attachmentFlag == 1)
                    {
                        cmdNoAttachment.Attributes.Add("onclick", "Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                            + PhoenixModule.ACCOUNTS + "');return true;");
                        cmdNoAttachment.Attributes.Add("onclick", "Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                            + PhoenixModule.ACCOUNTS + "&U=" + attachmentFlag.ToString() + "');return true;");
                    }
                    else
                    {
                        cmdNoAttachment.Attributes.Add("onclick", "Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                                            + PhoenixModule.ACCOUNTS + "');return true;");
                    }
                    cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
                }


                ImageButton iab = (ImageButton)e.Item.FindControl("cmdAttachment");
                ImageButton inab = (ImageButton)e.Item.FindControl("cmdNoAttachment");
                if (iab != null) iab.Visible = true;
                if (inab != null) inab.Visible = false;
                int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());
                if (n == 0)
                {
                    if (iab != null) iab.Visible = false;
                    if (inab != null) inab.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void gvDPO_Sorting(object sender, GridSortCommandEventArgs e)
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
    protected void gvDPO_SelectedIndexChanged(object sender, GridViewSelectEventArgs e)
    {
        gvDPO.SelectedIndexes.Add(e.NewSelectedIndex);
        ViewState["ORDERID"] = ((LinkButton)gvDPO.Items[e.NewSelectedIndex].FindControl("lblOrderId")).CommandArgument;

        BindData();
    }
    protected void gvDPO_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridEditableItem eeditedItem = e.Item as GridEditableItem;
            //RadGrid _gridView = (RadGrid)sender;

            //int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["ORDERID"] = ((RadLabel)e.Item.FindControl("lblOrderId")).Text;
                gvDPO.Rebind();
                //string depositid = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblDepositid")).Text;
            }

            //ViewState["ORDERID"] = ((RadLabel)e.Item.FindControl("lblOrderId")).Text;
            //ViewState["VESSELID"] = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
            //Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SetTabHighlight()
    {
        try
        {
            {
                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("AccountsInvoiceDirectPurchaseOrderGeneral.aspx"))
                {
                    MenuDPO.SelectedMenuIndex = 0;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("AccountsInvoiceDirectPurchaseOrderLineItem.aspx"))
                {
                    MenuDPO.SelectedMenuIndex = 2;
                }
                else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("AccountsDirectPOPISplit.aspx"))
                {
                    MenuDPO.SelectedMenuIndex = 1;
                }
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
        gvDPO.SelectedIndexes.Clear();
        gvDPO.EditIndexes.Clear();
        gvDPO.DataSource = null;
        gvDPO.Rebind();
    }
    protected void gvDPO_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDPO.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    public void EnableVisibleTab()
    {
        DataTable dt = PhoenixAccountsInvoice.InvoiceDirectPOEdit(new Guid(ViewState["ORDERID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (dt.Rows[0]["FLDPNIYN"].ToString() == "0")
            {
                toolbar.AddButton("General", "GENERAL");
                //toolbar.AddButton("P&I Case", "PNICASE");
                toolbar.AddButton("Line Item", "LINEITEM");
                toolbar.AddButton("History", "HISTORY");
                MenuDPO.AccessRights = this.ViewState;
                MenuDPO.MenuList = toolbar.Show();

            }
            else
            {

                DataSet ds = PhoenixAccountsPNIIntergeration.MedicalPNIPoMappingList(new Guid(ViewState["ORDERID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    toolbar.AddButton("General", "GENERAL");
                    toolbar.AddButton("P&I Case", "PNICASE");
                    toolbar.AddButton("Line Item", "LINEITEM");
                    toolbar.AddButton("History", "HISTORY");
                    MenuDPO.AccessRights = this.ViewState;
                    MenuDPO.MenuList = toolbar.Show();
                    // MenuDPO.SelectedMenuIndex = 0;
                }
                else
                {
                    toolbar.AddButton("General", "GENERAL");
                    toolbar.AddButton("P&I Case", "PNICASE");
                    //toolbar.AddButton("Line Item", "LINEITEM");
                    toolbar.AddButton("History", "HISTORY");
                    MenuDPO.AccessRights = this.ViewState;
                    MenuDPO.MenuList = toolbar.Show();
                }

                //MenuDPO.SelectedMenuIndex = 0;
            }
        }
    }

    private void SetRowSelection()
    {
        string a = ViewState["ORDERID"].ToString();

        gvDPO.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvDPO.Items)
        {
            if (item.GetDataKeyValue("FLDORDERID").ToString().Equals(ViewState["ORDERID"].ToString()))
            {
                gvDPO.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }


}
