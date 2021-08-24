using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsOrderFormLineItem : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);


            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["ORDERID"] = Request.QueryString["ORDERID"];
                ViewState["ISSTOCKYN"] = Request.QueryString["ISSTOCKYN"].ToString();
                ViewState["NEWPROCESS"] = Request.QueryString["NEWPROCESS"] == null ? null : Request.QueryString["NEWPROCESS"];
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ACTIVE"] = "1";
                ViewState["CURRENCY"] = null;
                ViewState["REQNO"] = "";
                if (ViewState["ORDERID"] != null)
                    EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("General", "GENERAL");
            toolbar.AddButton("Line Item", "LINEITEM");
            if (ViewState["ISSTOCKYN"].ToString() == "0")
                toolbar.AddButton("Employee List", "EMPSTOCK");
            toolbar.AddButton("List", "LIST");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbar.Show();
            MenuOrderForm.SelectedMenuIndex = 1;
            StoreMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditBondProvision(Guid gOrderId)
    {
        DataTable dt = PhoenixVesselAccountsOrderForm.EditOrderForm(gOrderId, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ViewState["CURRENCY"] = dr["FLDCURRENCYCODE"].ToString();
            txtvessel.Text = dr["FLDVESSELNAME"].ToString();
            txtRefNo.Text = dr["FLDREFERENCENO"].ToString();
            ViewState["EXCHANGERATE"] = dr["FLDEXCHANGERATE"].ToString();
            ViewState["ACTIVE"] = dr["FLDACTIVEYN"].ToString();
            StoreMenu();
            txtOrderDate.Text = dr["FLDORDERDATE"].ToString();
            ViewState["RPTCURRENCYCODE"] = dr["FLDRPTCURRENCYCODE"].ToString();
            ViewState["REQNO"]= dr["FLDREFERENCENO"].ToString();
        }
    }
    protected void Rebind()
    {
        gvCrewSearch.SelectedIndexes.Clear();
        gvCrewSearch.EditIndexes.Clear();
        gvCrewSearch.DataSource = null;
        gvCrewSearch.Rebind();
    }
    protected void OrderForm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsOrderFormGeneral.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"] + "&ISSTOCKYN=" + ViewState["ISSTOCKYN"].ToString() + "&NEWPROCESS=" + ViewState["NEWPROCESS"], false);
            }
            else if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsOrderFormLineItem.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"] + "&ISSTOCKYN=" + ViewState["ISSTOCKYN"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsOrderForm.aspx?storeclass=" + Request.QueryString["storeclass"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("EMPSTOCK"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsEmployeeBondRequisitionLineItem.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"] + "&ISSTOCKYN=" + ViewState["ISSTOCKYN"].ToString() + "&NewProcess=" + ViewState["NEWPROCESS"]+"&REQNO="+ ViewState["REQNO"], false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStoreItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
                ShowExcel();
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
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDORDEREDQUANTITY", "FLDQUANTITY", "FLDUNITPRICE", "FLDTOTALPRICE" };
            string[] alCaptions = { "Number", "Name", "Unit", "Ordered Quantity", "Quantity", "Unit Price", "Total Price" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixVesselAccountsOrderForm.SearchOrderFormLineItem(General.GetNullableGuid(ViewState["ORDERID"] != null ? ViewState["ORDERID"].ToString() : string.Empty)
                , sortexpression, sortdirection,
               1, iRowCount,
               ref iRowCount,
               ref iTotalPageCount);
            string title = "Purchase of Bond";
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
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDORDEREDQUANTITY", "FLDQUANTITY", "FLDUNITPRICE", "FLDTOTALPRICE" };
            string[] alCaptions = { "Number", "Name", "Unit", "Ordered Quantity", "Quantity", "Unit Price", "Total Price" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds = PhoenixVesselAccountsOrderForm.SearchOrderFormLineItem(General.GetNullableGuid(ViewState["ORDERID"] != null ? ViewState["ORDERID"].ToString() : string.Empty)
                , sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), gvCrewSearch.PageSize,
               ref iRowCount,
               ref iTotalPageCount);
            string title = "Purchase of Bond";
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
    protected void gvCrewSearch_EditCommand(object sender, GridCommandEventArgs e)
    {
        gvCrewSearch.SelectedIndexes.Clear();
        GridDataItem item = (GridDataItem)e.Item;

        gvCrewSearch.SelectedIndexes.Add(e.Item.ItemIndex);
    }
    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblorderlineid")).Text.Trim();

                PhoenixVesselAccountsOrderForm.DeleteOrderFormLineItem(new Guid(id));
                EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
                Rebind();
            }
            else if (e.CommandName == "Page")
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
    private decimal TotalPrice = 0, LocalPrice = 0, DiscountPrice = 0, ExchangePrice = 0, DeliveryChages = 0;
    protected void gvCrewSearch_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            // GridEditableItem item = (GridEditableItem)e.Item;

            string id = ((RadLabel)e.Item.FindControl("lblorderlineid")).Text.Trim();
            string lquantity = ((UserControlMaskNumber)e.Item.FindControl("lblQuantity")).Text;
            string lprice = ((UserControlMaskNumber)e.Item.FindControl("lblUnitPrice")).Text;
            string quantity = ((UserControlMaskNumber)e.Item.FindControl("txtQuantity")).Text;
            string price = ((UserControlMaskNumber)e.Item.FindControl("txtUnitPrice")).Text;
            if (!IsValidOrderLineItem(price, quantity))
            {
                ucError.Visible = true;
                return;
            }
            if (lquantity != quantity || lprice != price)
                PhoenixVesselAccountsOrderForm.UpdateOrderFormLineItem(new Guid(id), decimal.Parse(quantity), General.GetNullableDecimal(price));
            EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            e.Item.Cells[7].Text = "Unit Price(" + ViewState["CURRENCY"].ToString() + ")";
            e.Item.Cells[8].Text = "Total Price(" + ViewState["CURRENCY"].ToString() + ")";
        }
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (ViewState["ACTIVE"].ToString() == "1")
            {
                if (db != null)
                    e.Item.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCrewSearch, "Edit$" + e.Item.RowIndex.ToString(), false);
                SetKeyDownScroll(sender, e);
            }

            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            if (ViewState["ISSTOCKYN"].ToString().Equals("0"))
            {
                ed.Visible = false;
                db.Visible = false;
            }
            if (ViewState["ACTIVE"].ToString() != "1") { db.Visible = false; ed.Visible = false; }
            decimal.TryParse(drv["FLDTOTALAMOUNT"].ToString(), out TotalPrice);
            decimal.TryParse(drv["FLDDELIVERYCHARGESWITHDISCOUNT"].ToString(), out DeliveryChages);
            decimal.TryParse(drv["FLDTOTALLOCALPRICE"].ToString(), out LocalPrice);
            decimal.TryParse(drv["FLDDISCOUNTLOCALPRICE"].ToString(), out DiscountPrice);
            decimal.TryParse(drv["FLDEXCHANGEPRICE"].ToString(), out ExchangePrice);
        }
        if (e.Item is GridFooterItem)
        {
            RadLabel lblNetAmountUSD = (RadLabel)e.Item.FindControl("lblNetAmountUSD");
            if (lblNetAmountUSD != null)
                lblNetAmountUSD.Text = "Net Amount(" + ViewState["RPTCURRENCYCODE"].ToString() + ")";
            RadLabel lbltotamtinvoice = (RadLabel)e.Item.FindControl("lbltotamtinvoice");
            if (lbltotamtinvoice != null)
                lbltotamtinvoice.Text = LocalPrice.ToString("0.00");

            RadLabel lblLessDiscountLocalCurValue = (RadLabel)e.Item.FindControl("lblLessDiscountLocalCurValue");
            if (lblLessDiscountLocalCurValue != null)
                lblLessDiscountLocalCurValue.Text = DiscountPrice.ToString("0.00");
            RadLabel lblchargesValue = (RadLabel)e.Item.FindControl("lblchargesValue");
            if (lblchargesValue != null)
                lblchargesValue.Text = DeliveryChages.ToString("0.00");
            RadLabel lblNetAmtLocalCurValue = (RadLabel)e.Item.FindControl("lblNetAmtLocalCurValue");
            if (lblNetAmtLocalCurValue != null)
                lblNetAmtLocalCurValue.Text = TotalPrice.ToString();
            RadLabel lblnetAmountValue = (RadLabel)e.Item.FindControl("lblnetAmountValue");
            if (lblnetAmountValue != null)
                lblnetAmountValue.Text = ExchangePrice.ToString("0.00");
        }
    }
    protected void gvCrewSearch_PreRender(object sender, EventArgs e)
    {
        //GridHeaderItem headerItem = gvCrewSearch.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
        //if (headerItem != null)
        //{
        //    //  headerItem["MAKERREF"].Text = "Product Code";
        //}
        GridFooterItem footerItem = gvCrewSearch.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
        //fetch the data with footerItem["ColumnUniqueName"].Text
        if (footerItem != null)
        {
            footerItem["ORDERLINE"].ColumnSpan = 7;

        }
    }
    private bool IsValidOrderLineItem(string price, string quantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Quantity is required.";
        }

        return (!ucError.IsError);
    }
    private void StoreMenu()
    {
        if (ViewState["ACTIVE"].ToString() == "1")
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsOrderFormLineItem.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            if (ViewState["ISSTOCKYN"].ToString().Equals("1"))
                toolbar.AddFontAwesomeButton("javascript:return showPickList('spnPickListStore', 'codehelp1', '','VesselAccounts/VesselAccountsOrderFormStoreItemSelection.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"] + "', true);", "Store Item", "<i class=\"fa fa-plus-circle\"></i>", "ADDSTORE");
            MenuStoreItem.AccessRights = this.ViewState;
            MenuStoreItem.MenuList = toolbar.Show();

        }
        else
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsOrderFormLineItem.aspx?storeclass=" + Request.QueryString["storeclass"].ToString() + "&orderid=" + ViewState["ORDERID"] + "&pageno=" + ViewState["PAGENUMBER"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuStoreItem.AccessRights = this.ViewState;
            MenuStoreItem.MenuList = toolbar.Show();
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetKeyDownScroll(object sender, GridItemEventArgs e)
    {
        int nextRow = 0;
        RadGrid _gridView = (RadGrid)sender;

        if (e.Item is GridEditableItem)
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Item.RowIndex == 0) ? nRows : e.Item.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Item.RowIndex == nRows) ? 0 : e.Item.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Item.Attributes["onkeydown"] = script;
        }
    }
}
