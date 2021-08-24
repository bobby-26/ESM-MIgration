using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class PurchaseBulkPOList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (!IsPostBack)
        {
            gvBulkPO.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            toolbar.AddFontAwesomeButton("../Purchase/PurchaseBulkPOList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBulkPO')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseBulkPOFilter.aspx?callfrom=technicalpo", "Find", "<i class=\"fas fa-search\"></i>", "FIND");

            MenuBulkPurchase.AccessRights = this.ViewState;
            MenuBulkPurchase.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Bulk PO", "BULKPO");
            toolbar.AddButton("Line Items", "LINEITEM");
            toolbar.AddButton("Vessels", "VESSEL");
            toolbar.AddButton("Attachments", "ATTACHMENTS");

            MenuBulkPO.AccessRights = this.ViewState;
            MenuBulkPO.MenuList = toolbar.Show();
            MenuBulkPO.SelectedMenuIndex = 0;

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Filter.CurrentSelectedBulkOrderId != null && Filter.CurrentSelectedBulkOrderId.ToString() != "")
                ViewState["ORDERID"] = Filter.CurrentSelectedBulkOrderId.ToString();
            else
                ViewState["ORDERID"] = "";

            if (Request.QueryString["CallFrom"] != null && Request.QueryString["CallFrom"].ToString() != "")
                Filter.CurrentSelectedBulkOrderId = null;

            Filter.CurrentSelectedBulkPOType = "1";
        }        
    }

    protected void MenuBulkPurchase_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvBulkPO.CurrentPageIndex = 0;
            gvBulkPO.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            gvBulkPO.CurrentPageIndex = 0;
            gvBulkPO.Rebind();
        }      
    }

    protected void MenuBulkPO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("LINEITEM"))
        {
            Response.Redirect("../Purchase/PurchaseBulkPOLineItemList.aspx", false);
        }
        if (CommandName.ToUpper().Equals("VESSEL"))
        {
            Response.Redirect("../Purchase/PurchaseBulkPOLineItemByVesselList.aspx");
        }
        if (CommandName.ToUpper().Equals("ATTACHMENTS"))
        {
            Response.Redirect("../Purchase/PurchaseBulkPOAttachments.aspx");
        } 
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFORMNUMBER", "FLDFORMTITLE", "FLDCURRENCYCODE", "FLDVENDORCODE", "FLDINVOICEREFERENCENUMBER", "FLDINVOICENUMBER", "FLDAPPROVALSTATUS" };
        string[] alCaptions = { "Bulk Purchase Reference Number", "Form Title", "Currency", "Vendor", "Invoice Reference Number", "Invoice Number", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentBulkPO;

        DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOSearch(
            nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNumber")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucCurrency")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceReferenceNumber")) : null
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , gvBulkPO.PageSize
            , ref iRowCount
            , ref iTotalPageCount
            , 1 //technical PO
            , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceNumber")) : null);

        General.SetPrintOptions("gvBulkPO", "Bulk Purchase Order", alCaptions, alColumns, ds);

        gvBulkPO.DataSource = ds;
        gvBulkPO.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Filter.CurrentSelectedBulkOrderId == null)
            {
                ViewState["ORDERID"] = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
                Filter.CurrentSelectedBulkOrderId = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
            }
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseBulkPOGeneral.aspx?ORDERID=" + ViewState["ORDERID"]+"&POTYPE=1";
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseBulkPOGeneral.aspx?ORDERID=&POTYPE=1";
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDFORMNUMBER", "FLDFORMTITLE", "FLDCURRENCYCODE", "FLDVENDORCODE", "FLDINVOICEREFERENCENUMBER", "FLDINVOICENUMBER", "FLDAPPROVALSTATUS" };
        string[] alCaptions = { "Bulk Purchase Reference Number", "Form Title", "Currency", "Vendor", "Invoice Reference Number", "Invoice Number", "Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //string BudgetBillingSearch = (txtSearchBudgetBillingList.Text == null) ? "" : txtSearchBudgetBillingList.Text;

        NameValueCollection nvc = Filter.CurrentBulkPO;

        ds = PhoenixPurchaseBulkPurchase.BulkPOSearch(
           nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNumber")) : null
           , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
           , nvc != null ? General.GetNullableInteger(nvc.Get("ucCurrency")) : null
           , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceReferenceNumber")) : null
           , sortexpression
           , sortdirection
           , (int)ViewState["PAGENUMBER"]
           , iRowCount
           , ref iRowCount
           , ref iTotalPageCount
           , 1 //technical PO
           , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceNumber")) : null);

        Response.AddHeader("Content-Disposition", "attachment; filename=BulkPOList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Bulk Purchase Order</h3></td>");
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

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["ORDERID"] = ((Label)gvBulkPO.Items[rowindex].FindControl("lblOrderId")).Text;
            Filter.CurrentSelectedBulkOrderId = ((Label)gvBulkPO.Items[rowindex].FindControl("lblOrderId")).Text;
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseBulkPOGeneral.aspx?ORDERID=" + ViewState["ORDERID"].ToString()+"&POTYPE=1";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Filter.CurrentSelectedBulkOrderId == null)
                ViewState["ORDERID"] = null;
            gvBulkPO.Rebind();
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

    protected void gvBulkPO_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFORMNUMBER", "FLDFORMTITLE", "FLDCURRENCYCODE", "FLDVENDORCODE", "FLDINVOICEREFERENCENUMBER", "FLDINVOICENUMBER", "FLDAPPROVALSTATUS" };
        string[] alCaptions = { "Bulk Purchase Reference Number", "Form Title", "Currency", "Vendor", "Invoice Reference Number", "Invoice Number", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentBulkPO;

        DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOSearch(
            nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNumber")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucCurrency")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceReferenceNumber")) : null
            , sortexpression
            , sortdirection
            , gvBulkPO.CurrentPageIndex+1
            , gvBulkPO.PageSize
            , ref iRowCount
            , ref iTotalPageCount
            , 1 //technical PO
            , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceNumber")) : null);

        General.SetPrintOptions("gvBulkPO", "Bulk Purchase Order", alCaptions, alColumns, ds);

        gvBulkPO.DataSource = ds;
        gvBulkPO.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Filter.CurrentSelectedBulkOrderId == null)
            {
                ViewState["ORDERID"] = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
                Filter.CurrentSelectedBulkOrderId = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
            }
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseBulkPOGeneral.aspx?ORDERID=" + ViewState["ORDERID"] + "&POTYPE=1";
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseBulkPOGeneral.aspx?ORDERID=&POTYPE=1";
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvBulkPO_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            if (db != null)
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

            LinkButton cmdPOApprove = (LinkButton)e.Item.FindControl("cmdPOApprove");
            LinkButton cmdCopy = (LinkButton)e.Item.FindControl("cmdCopy");
            LinkButton cmdPost = (LinkButton)e.Item.FindControl("cmdPost");
            LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
            Label lblApprovalStatus = (Label)e.Item.FindControl("lblApprovalStatus");
            Label lblCopiedStatus = (Label)e.Item.FindControl("lblCopiedStatus");
            Label lblCancelYN = (Label)e.Item.FindControl("lblCancelYN");


            if (lblApprovalStatus != null && lblApprovalStatus.Text == "1")
            {
                if (cmdPOApprove != null)
                    cmdPOApprove.Visible = false;
                if (cmdCopy != null)
                    cmdCopy.Visible = true;
                if (cmdPost != null)
                    cmdPost.Visible = true;

                if (lblCopiedStatus != null && lblCopiedStatus.Text != "" && int.Parse(lblCopiedStatus.Text) > 0)
                {
                    if (cmdCopy != null)
                        cmdCopy.Visible = false;
                    if (cmdCancel != null)
                        cmdCancel.Visible = false;
                }

                if (lblCopiedStatus != null && lblCopiedStatus.Text != "" && int.Parse(lblCopiedStatus.Text) > 1)
                {
                    if (cmdCopy != null)
                        cmdCopy.Visible = false;
                    if (cmdPost != null)
                        cmdPost.Visible = false;
                    if (cmdCancel != null)
                        cmdCancel.Visible = false;
                }
            }

            if (lblCancelYN != null && General.GetNullableInteger(lblCancelYN.Text) == 1)
            {
                if (cmdCopy != null)
                    cmdCopy.Visible = false;
                if (cmdPost != null)
                    cmdPost.Visible = false;
                if (cmdPOApprove != null)
                    cmdPOApprove.Visible = false;
                if (cmdCancel != null)
                    cmdCancel.Visible = false;

            }


            if (cmdPOApprove != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdPOApprove.CommandName)) cmdPOApprove.Visible = false;
            if (cmdCopy != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdCopy.CommandName)) cmdCopy.Visible = false;
            if (cmdPost != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdPost.CommandName)) cmdPost.Visible = false;
        }
    }

    protected void gvBulkPO_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                BindPageURL(item.ItemIndex);
            }
            if (e.CommandName.ToUpper().Equals("POAPPROVE"))
            {
                if (Filter.CurrentSelectedBulkOrderId != null && Filter.CurrentSelectedBulkOrderId.ToString() != "")
                {
                    Guid orderid = new Guid(Filter.CurrentSelectedBulkOrderId.ToString());
                    PhoenixPurchaseBulkPurchase.BulkPOApprovalStatusUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, orderid);
                    gvBulkPO.Rebind();
                }
            }
            if (e.CommandName.ToUpper().Equals("POCOPYTOORDERFORM"))
            {
                if (Filter.CurrentSelectedBulkOrderId != null && Filter.CurrentSelectedBulkOrderId.ToString() != "")
                {
                    Guid orderid = new Guid(Filter.CurrentSelectedBulkOrderId.ToString());
                    PhoenixPurchaseBulkPurchase.BulkPOCopyToOrderForm(PhoenixSecurityContext.CurrentSecurityContext.UserCode, orderid);
                    gvBulkPO.Rebind();
                }
            }
            if (e.CommandName.ToUpper().Equals("POPOST"))
            {
                if (Filter.CurrentSelectedBulkOrderId != null && Filter.CurrentSelectedBulkOrderId.ToString() != "")
                {
                    Guid orderid = new Guid(Filter.CurrentSelectedBulkOrderId.ToString());
                    PhoenixPurchaseBulkPurchase.BulkPOCopyToInvoice(PhoenixSecurityContext.CurrentSecurityContext.UserCode, orderid);
                    gvBulkPO.Rebind();
                }
            }
            if (e.CommandName.ToUpper().Equals("CANCEL"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPurchaseBulkPurchase.BulkPOCancel(new Guid(((Label)item.FindControl("lblOrderId")).Text));
                gvBulkPO.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBulkPO_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
