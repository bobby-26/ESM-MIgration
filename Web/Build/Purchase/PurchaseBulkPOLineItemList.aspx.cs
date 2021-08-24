using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class PurchaseBulkPOLineItemList : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (!IsPostBack)
            {
                gvBulkPOLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                Session["NEWBULKLINEITEM"] = "N";

                toolbar.AddFontAwesomeButton("../Purchase/PurchaseBulkPOLineItemList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
                toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBulkPOLineItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

                MenuBulkPurchase.AccessRights = this.ViewState;
                MenuBulkPurchase.MenuList = toolbar.Show();

                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Bulk PO", "BULKPO");
                toolbar.AddButton("Line Items", "LINEITEM");
                toolbar.AddButton("Vessels", "VESSEL");
                toolbar.AddButton("Attachments", "ATTACHMENTS");

                MenuBulkPO.AccessRights = this.ViewState;
                MenuBulkPO.MenuList = toolbar.Show();
                MenuBulkPO.SelectedMenuIndex = 1;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (Filter.CurrentSelectedBulkOrderId != null && Filter.CurrentSelectedBulkOrderId.ToString() != "")
                    ViewState["ORDERID"] = Filter.CurrentSelectedBulkOrderId.ToString();
                else
                    ViewState["ORDERID"] = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuBulkPO_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("BULKPO"))
        {
            if (Filter.CurrentSelectedBulkPOType == "1")
                Response.Redirect("../Purchase/PurchaseBulkPOList.aspx", false);
            else
                Response.Redirect("../Purchase/PurchaseBulkDirectPOList.aspx", false);
        }
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

    protected void MenuBulkPurchase_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvBulkPOLineItem.Rebind();
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

        string[] alColumns = { "FLDLINEITEMNAME", "FLDLINEITEMNUMBER", "FLDBUDGETCODE", "FLDUNITNAME", "FLDORDERQUANTITY", "FLDRECEIVEDQUANTITY", "FLDPRICE", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Item Name", "Item Number", "Budget Code", "Unit", "Total Order Quantity", "Received Quantity", "Unit Price", "Total" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOLineItemSearch(
                                                                 General.GetNullableGuid(ViewState["ORDERID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvBulkPOLineItem.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvBulkPOLineItem", "Bulk Purchase Order Line Items", alCaptions, alColumns, ds);

        gvBulkPOLineItem.DataSource = ds;
        gvBulkPOLineItem.VirtualItemCount=iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ViewState["LINEITEMID"] == null)
            {
                ViewState["LINEITEMID"] = ds.Tables[0].Rows[0]["FLDLINEITEMID"].ToString();
                gvBulkPOLineItem.SelectedIndexes.Clear();
            }
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseBulkPOLineItemGeneral.aspx?LINEITEMID=" + ViewState["LINEITEMID"];
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseBulkPOLineItemGeneral.aspx?LINEITEMID=";
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDLINEITEMNAME", "FLDLINEITEMNUMBER", "FLDBUDGETCODE", "FLDUNITNAME", "FLDORDERQUANTITY", "FLDRECEIVEDQUANTITY", "FLDPRICE", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Item Name", "Item Number", "Budget Code", "Unit", "Total Order Quantity", "Received Quantity", "Unit Price", "Total" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseBulkPurchase.BulkPOLineItemSearch(
                                                         General.GetNullableGuid(ViewState["ORDERID"].ToString())
                                                        , sortexpression
                                                        , sortdirection
                                                        , 1
                                                        , gvBulkPOLineItem.VirtualItemCount > 0 ? gvBulkPOLineItem.VirtualItemCount:10
                                                        , ref iRowCount
                                                        , ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=BulkPOLineItemList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Bulk Purchase Order Line Items</h3></td>");
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
            ViewState["LINEITEMID"] = ((Label)gvBulkPOLineItem.Items[rowindex].FindControl("lblLineItemId")).Text;            
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseBulkPOLineItemGeneral.aspx?LINEITEMID=" + ViewState["LINEITEMID"].ToString();
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
            BindData();
            gvBulkPOLineItem.Rebind();
            if (Session["NEWBULKLINEITEM"] != null && Session["NEWBULKLINEITEM"].ToString() == "Y")
            {
                gvBulkPOLineItem.SelectedIndexes.Clear();
                Session["NEWBULKLINEITEM"] = "N";
                BindPageURL(0);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DeleteBulkPOLineItem(Guid lineitemid)
    {
        PhoenixPurchaseBulkPurchase.BulkPOLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, lineitemid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvBulkPOLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDLINEITEMNAME", "FLDLINEITEMNUMBER", "FLDBUDGETCODE", "FLDUNITNAME", "FLDORDERQUANTITY", "FLDRECEIVEDQUANTITY", "FLDPRICE", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Item Name", "Item Number", "Budget Code", "Unit", "Total Order Quantity", "Received Quantity", "Unit Price", "Total" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseBulkPurchase.BulkPOLineItemSearch(
                                                                 General.GetNullableGuid(ViewState["ORDERID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvBulkPOLineItem.CurrentPageIndex+1
                                                                , gvBulkPOLineItem.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvBulkPOLineItem", "Bulk Purchase Order Line Items", alCaptions, alColumns, ds);

        gvBulkPOLineItem.DataSource = ds;
        gvBulkPOLineItem.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ViewState["LINEITEMID"] == null)
            {
                ViewState["LINEITEMID"] = ds.Tables[0].Rows[0]["FLDLINEITEMID"].ToString();
                gvBulkPOLineItem.SelectedIndexes.Clear();
            }
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseBulkPOLineItemGeneral.aspx?LINEITEMID=" + ViewState["LINEITEMID"];
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseBulkPOLineItemGeneral.aspx?LINEITEMID=";
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvBulkPOLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }

    protected void gvBulkPOLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //ViewState["RowIndex"] = e.Item.RowIndex - 2;
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                BindPageURL(item.ItemIndex);
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                BindPageURL(item.ItemIndex);
                if (General.GetNullableGuid(ViewState["LINEITEMID"].ToString()) != null)
                {
                    DeleteBulkPOLineItem(new Guid(((Label)item.FindControl("lblLineItemId")).Text));
                    ViewState["LINEITEMID"] = null;
                    Filter.CurrentSelectedBulkOrderLineItemId = null;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBulkPOLineItem_SortCommand(object sender, GridSortCommandEventArgs e)
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
