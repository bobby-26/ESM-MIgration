using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsOrderFormStoreItemSelection : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuStockItem.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            if ((Request.QueryString["txtnumber"] != null) && (Request.QueryString["txtnumber"] != ""))
                txtNumberSearch.Text = Request.QueryString["txtnumber"].ToString().TrimEnd("00.00.00".ToCharArray()).TrimEnd("__.__.__".ToCharArray());
            if ((Request.QueryString["txtname"] != null) && (Request.QueryString["txtname"] != ""))
                txtStockItemNameSearch.Text = Request.QueryString["txtname"].ToString();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvStoreItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void MenuStockItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvStoreItem.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixVesselAccountsOrderForm.SearchOrderFormStoreItem(new Guid(Request.QueryString["ORDERID"]),
                PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtNumberSearch.Text, txtStockItemNameSearch.Text,
                null, null, General.GetNullableInteger(Request.QueryString["storeclass"].ToString()), null, sortexpression, sortdirection,
               gvStoreItem.CurrentPageIndex+1, gvStoreItem.PageSize,ref iRowCount,ref iTotalPageCount);
            gvStoreItem.DataSource = dt;
            gvStoreItem.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem item = (GridEditableItem)e.Item;
                  
            string quantity = ((RadNumericTextBox)item.FindControl("txtQuantity")).Text;
            string unitprice = ((UserControlMaskNumber)item.FindControl("txtUnitPrice")).Text;
            if (General.GetNullableDecimal(quantity).HasValue)
            {
                PhoenixVesselAccountsOrderForm.InsertOrderFormLineItem(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    new Guid(Request.QueryString["ORDERID"]), item.GetDataKeyValue("FLDSTOREITEMID").ToString(), quantity, unitprice);
                string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false); 
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreItem_EditCommand(object sender, GridCommandEventArgs e)
    {
        gvStoreItem.SelectedIndexes.Clear();
        GridDataItem item = (GridDataItem)e.Item;
        gvStoreItem.SelectedIndexes.Add(e.Item.ItemIndex);
    }

    protected void gvStoreItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel lb = (RadLabel)item.FindControl("lblIsInMarket");
                Int64 result = 0;

                if (Int64.TryParse(drv["FLDISINMARKET"].ToString(), out result))
                {
                    item.ForeColor = System.Drawing.Color.Red;
                }

                RadNumericTextBox tb = (RadNumericTextBox)item.FindControl("txtQuantity");
                if (tb != null) tb.Focus();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreItem_PreRender(object sender, EventArgs e)
    {
        //GridHeaderItem headerItem = gvStoreItem.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
        //if (headerItem != null && Filter.CurrentPurchaseStockType == "STORE")
        //{
        //    headerItem["MAKERREF"].Text = "Product Code";
        //}
    }
    protected void gvStoreItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvStoreItem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataTable)_gridView.DataSource).Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }
    }
}
