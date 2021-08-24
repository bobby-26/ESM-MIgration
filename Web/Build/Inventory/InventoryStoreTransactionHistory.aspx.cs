using System;
using System.Data;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventoryStoreTransactionHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreTransactionHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStoreEntryDetail')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreTransactionHistory.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuGridStoreInOut.AccessRights = this.ViewState;  
            MenuGridStoreInOut.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                gvStoreEntryDetail.PageSize = General.ShowRecords(null);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["STOREITEMDISPOSITIONID"] = null;
                ddlDispositionType.HardTypeCode = ((int)PhoenixHardTypeCode.TRANSACTIONTYPE).ToString();
                ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();                 
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDDISPOSITIODATE", "FLDNUMBER", "FLDNAME", "FLDTRANSACTIONTYPENAME", "FLDDISPOSITIONQUANTITY", "FLDROB", "FLDPURCHASEPRICE", "FLDFORMNUMBER", "FLDREPORTEDBY" };
            string[] alCaptions = { "Transaction Date", "Item Number", "Item Name", "Transaction Type", "Quantity", "ROB", "Purchase Price", "Order No.", "Reported By" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixInventoryStoreItemTransaction.StoreItemTransactionHistorySearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                               string.IsNullOrEmpty(txtItemNumber.Text) ? txtItemNumber.Text : txtItemNumber.TextWithLiterals
                               , string.IsNullOrEmpty(txtItemNumberTo.Text) ? txtItemNumberTo.Text : txtItemNumberTo.TextWithLiterals
                               , txtItemName.Text
                               , General.GetNullableInteger(ddlDispositionType.SelectedHard)
                               , General.GetNullableInteger(ddlStockClass.SelectedHard)
                               , null, General.GetNullableDateTime(txtDispositionDate.Text), General.GetNullableDateTime(txtDispositionTodate.Text),
                               sortexpression, sortdirection, gvStoreEntryDetail.CurrentPageIndex + 1,
                              gvStoreEntryDetail.PageSize,
                              ref iRowCount,
                              ref iTotalPageCount);

            General.SetPrintOptions("gvStoreEntryDetail", "Inventory Store Item Transaction History Details", alCaptions, alColumns, ds);

            gvStoreEntryDetail.DataSource = ds;
            gvStoreEntryDetail.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreEntryDetail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvStoreEntryDetail_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;

        RadLabel lblStoreItemId = (RadLabel)eeditedItem.FindControl("lblStoreItemId");
        RadLabel lblStoreItemDispositionHeaderId = (RadLabel)eeditedItem.FindControl("lblStoreItemDispositionHeaderId");
        RadLabel lblStoreItemDispositionId = (RadLabel)eeditedItem.FindControl("lblStoreItemDispositionId");
        RadLabel lblDispositionQuantity = (RadLabel)eeditedItem.FindControl("lblDispositionQuantity");

        DeleteStoreItemDisposition(lblStoreItemId.Text
               , lblStoreItemDispositionHeaderId.Text
               , lblStoreItemDispositionId.Text
               , lblDispositionQuantity.Text);
        BindData();
        gvStoreEntryDetail.Rebind();

    }
    protected void gvStoreEntryDetail_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
    }
    protected void gvStoreEntryDetail_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
    protected void MenuGridStoreInOut_TabStripCommand(object sender, EventArgs e)
    {
        try
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
            BindData();
            gvStoreEntryDetail.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {

            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDDISPOSITIODATE", "FLDNUMBER", "FLDNAME", "FLDTRANSACTIONTYPENAME", "FLDDISPOSITIONQUANTITY", "FLDROB", "FLDPURCHASEPRICE", "FLDFORMNUMBER", "FLDREPORTEDBY" };
            string[] alCaptions = { "Transaction Date", "Item Number", "Item Name", "Transaction Type", "Quantity", "ROB", "Purchase Price", "Order Number", "Reported By" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = gvStoreEntryDetail.PageSize;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string dispositionheaderid = "";


            string storeitemid = (Request.QueryString["STOREITEMID"] == null) ? null : (Request.QueryString["STOREITEMID"].ToString());

            if (Filter.CurrentStoreItemDispositionHeaderId != null)
            {
                NameValueCollection nvc = Filter.CurrentStoreItemDispositionHeaderId;
                dispositionheaderid = nvc.Get("DISPOSITIONHEADERID").ToString();
            }
            DataSet ds = PhoenixInventoryStoreItemTransaction.StoreItemTransactionHistorySearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                            string.IsNullOrEmpty(txtItemNumber.Text) ? txtItemNumber.Text : txtItemNumber.TextWithLiterals
                            , string.IsNullOrEmpty(txtItemNumberTo.Text) ? txtItemNumberTo.Text : txtItemNumberTo.TextWithLiterals
                            , txtItemName.Text
                            , General.GetNullableInteger(ddlDispositionType.SelectedHard)
                            ,General.GetNullableInteger(ddlStockClass.SelectedHard)
                            , null, General.GetNullableDateTime(txtDispositionDate.Text), General.GetNullableDateTime(txtDispositionTodate.Text),
                            sortexpression, sortdirection, 1, iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount);
            Response.AddHeader("Content-Disposition", "attachment; filename=StoreItemTransactionHistory.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Inventory Store Item Transaction History Details</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }  
    private void DeleteStoreItemDisposition(string storeitemid,string storeitemdispositionheaderid,string storeitemdispositionid, string storeitemdispositionquantity)
    {
        try
        {
            PhoenixInventoryStoreItemTransaction.StoreItemTransactionEntryDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , new Guid(storeitemid)
                , new Guid(storeitemdispositionheaderid)  
                , new Guid(storeitemdispositionid)  
                , Convert.ToDecimal(storeitemdispositionquantity));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreEntryDetail_SortCommand(object sender, GridSortCommandEventArgs e)
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
