using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Accounts;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsSundryPurchaseLineItem : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            menulineitem.Title = "Line Item";
            menulineitem.AccessRights = this.ViewState;
            menulineitem.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

                ViewState["ORDERID"] = Request.QueryString["ORDERID"];

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ACTIVE"] = "1";

                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            if (ViewState["ORDERID"] != null)
                EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));

            //  BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditBondProvision(Guid gOrderId)
    {
        DataTable dt = PhoenixAccountsSundryPurchase.EditSundryPurchase(gOrderId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ViewState["EXCHANGERATE"] = dr["FLDEXCHANGERATE"].ToString();
            ViewState["ACTIVE"] = dr["FLDACTIVEYN"].ToString();
            StoreMenu(dr["FLDSTOCKTYPE"].ToString());
        }
    }
    protected void MenuStoreItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY", "FLDUNITPRICE", "FLDTOTALPRICE" };
            string[] alCaptions = { "Number", "Name", "Unit", "Quantity", "Unit Price", "Total Price" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixAccountsSundryPurchase.SearchSundryPurchaseLineItem(General.GetNullableGuid(ViewState["ORDERID"] != null ? ViewState["ORDERID"].ToString() : string.Empty)
                , sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
               ref iRowCount,
               ref iTotalPageCount);
            string title = "Purchase of Bonded Stores, Phone Cards and Provisions";
            if (ds.Tables[0].Rows.Count > 0)
            {
                title += "<br/> Order No : " + ds.Tables[0].Rows[0]["FLDREFERENCENO"].ToString() + "<br/> Order Date : " + DateTime.Parse(ds.Tables[0].Rows[0]["FLDORDERDATE"].ToString()).ToString("dd/MM/yyyy");
            }
            General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

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
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY", "FLDUNITPRICE", "FLDTOTALPRICE" };
            string[] alCaptions = { "Number", "Name", "Unit", "Quantity", "Unit Price", "Total Price" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixAccountsSundryPurchase.SearchSundryPurchaseLineItem(General.GetNullableGuid(ViewState["ORDERID"] != null ? ViewState["ORDERID"].ToString() : string.Empty)
                , sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), gvCrewSearch.PageSize,
               ref iRowCount,
               ref iTotalPageCount);
            string title = "Purchase of Bonded Stores, Phone Cards and Provisions";
            if (ds.Tables[0].Rows.Count > 0)
            {
                title += "Order No : " + ds.Tables[0].Rows[0]["FLDREFERENCENO"].ToString() + "<br/> Order Date : " + DateTime.Parse(ds.Tables[0].Rows[0]["FLDORDERDATE"].ToString()).ToString("dd/MM/yyyy");
            }
            General.SetPrintOptions("gvCrewSearch", title, alCaptions, alColumns, ds);

            gvCrewSearch.DataSource = ds;
            gvCrewSearch.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                Guid id = Guid.Parse(((RadLabel)e.Item.FindControl("lblorderid")).Text);
                string quantity = ((UserControlMaskNumber)e.Item.FindControl("txtQuantity")).Text;
                string price = ((UserControlMaskNumber)e.Item.FindControl("txtUnitPrice")).Text;
                if (!IsValidOrderLineItem(price, quantity))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsSundryPurchase.UpdateSundryPurchaseLineItem(id, decimal.Parse(quantity), decimal.Parse(price));
                EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
                gvCrewSearch.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                Guid id = Guid.Parse(((RadLabel)e.Item.FindControl("lblorderid")).Text);
                PhoenixAccountsSundryPurchase.DeleteSundryPurchaseLineItem(id);
                EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
                gvCrewSearch.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private decimal TotalPrice = 0, LocalPrice = 0, DiscountPrice = 0;
    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {

        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)

        {
            if (ViewState["ACTIVE"].ToString() == "1")
            {
                e.Item.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCrewSearch, "Edit$" + e.Item.ToString(), false);
                //  SetKeyDownScroll(sender,e);
            }
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            if (ViewState["ACTIVE"].ToString() != "1") { db.Visible = false; ed.Visible = false; }
            decimal.TryParse(drv["FLDTOTALAMOUNT"].ToString(), out TotalPrice);
            decimal.TryParse(drv["FLDTOTALLOCALPRICE"].ToString(), out LocalPrice);
            decimal.TryParse(drv["FLDDISCOUNTLOCALPRICE"].ToString(), out DiscountPrice);
        }

        else if (e.Item is GridFooterItem)
        {
            decimal d;

            e.Item.Cells[7].Text = LocalPrice.ToString() + "<br/>" + DiscountPrice.ToString("0.00") + "<br/>" + TotalPrice.ToString() + "<br/>" + (decimal.TryParse(ViewState["EXCHANGERATE"].ToString(), out d) ? Math.Round((TotalPrice / d), 2).ToString() : string.Empty);
            e.Item.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            e.Item.Font.Bold = true;
        }
    }


    //protected void gvCrewSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    DataRowView drv = (DataRowView)e.Row.DataItem;
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (ViewState["ACTIVE"].ToString() == "1")
    //        {
    //            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCrewSearch, "Edit$" + e.Row.RowIndex.ToString(), false);
    //            SetKeyDownScroll(sender, e);
    //        }
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //        if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

    //        ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
    //        if (ViewState["ACTIVE"].ToString() != "1") { db.Visible = false; ed.Visible = false; }
    //        decimal.TryParse(drv["FLDTOTALAMOUNT"].ToString(), out TotalPrice);
    //        decimal.TryParse(drv["FLDTOTALLOCALPRICE"].ToString(), out LocalPrice);
    //        decimal.TryParse(drv["FLDDISCOUNTLOCALPRICE"].ToString(), out DiscountPrice);
    //    }
    //    else if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        decimal d;

    //        e.Row.Cells[5].Text = LocalPrice.ToString() + "<br/>" + DiscountPrice.ToString("0.00") + "<br/>" + TotalPrice.ToString() + "<br/>" + (decimal.TryParse(ViewState["EXCHANGERATE"].ToString(), out d) ? Math.Round((TotalPrice / d), 2).ToString() : string.Empty);
    //        e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
    //        e.Row.Font.Bold = true;
    //    }
    //}




    //protected void gvCrewSearch_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {
    //        Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
    //        string quantity = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantity")).Text;
    //        string price = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtUnitPrice")).Text;
    //        if (!IsValidOrderLineItem(price, quantity))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        PhoenixAccountsSundryPurchase.UpdateSundryPurchaseLineItem (id, decimal.Parse(quantity), decimal.Parse(price));
    //        EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}

    private bool IsValidOrderLineItem(string price, string quantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDecimal(price).HasValue)
        {
            ucError.ErrorMessage = "Unit price is required.";
        }
        else if (General.GetNullableDecimal(price).HasValue && General.GetNullableDecimal(price).Value <= 0)
        {
            ucError.ErrorMessage = "Unit price should be greater than zero.";
        }

        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Quantity is required.";
        }
        else if (General.GetNullableDecimal(quantity).HasValue && General.GetNullableDecimal(quantity).Value <= 0)
        {
            ucError.ErrorMessage = "Quantity should be greater than zero.";
        }

        return (!ucError.IsError);
    }
    private void StoreMenu(string storeclass)
    {
        if (ViewState["ACTIVE"].ToString() == "1")
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsSundryPurchaseLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCrewSearch')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("javascript:return showPickList('spnPickListStore', 'codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsSundryPurchaseItem.aspx?storeclass=" + storeclass + "&orderid=" + ViewState["ORDERID"] + "', true);", "Store Item", "add.png", "ADDSTORE");
            MenuStoreItem.AccessRights = this.ViewState;
            MenuStoreItem.MenuList = toolbar.Show();
            // MenuStoreItem.SetTrigger(pnlNTBRManager);
        }
        if (ViewState["ACTIVE"].ToString() == "0")
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsSundryPurchaseLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCrewSearch')", "Print Grid", "icon_print.png", "PRINT");
            MenuStoreItem.AccessRights = this.ViewState;
            MenuStoreItem.MenuList = toolbar.Show();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    //{
    //    int nextRow = 0;
    //    GridView _gridView = (GridView)sender;

    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
    //    {
    //        int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

    //        String script = "var keyValue = SelectSibling(event); ";
    //        script += " if(keyValue == 38) {";  //Up Arrow
    //        nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

    //        script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
    //        script += "}";

    //        script += " if(keyValue == 40) {";  //Down Arrow
    //        nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

    //        script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
    //        script += "}";
    //        script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
    //        e.Row.Attributes["onkeydown"] = script;
    //    }
    //}
}
