using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Purchase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PurchaseOrderLineItemSpareSelect : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
            toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
            MenuStoreItemInOutTransaction.AccessRights = this.ViewState;
            MenuStoreItemInOutTransaction.MenuList = toolbarmain.Show();
                                   
            if (!IsPostBack)
            {
                rgvLine.PageSize = General.ShowRecords(null);
                ViewState["orderid"] = "0";
                BindComponent();
                ViewState["COMPONENTID"] = null;
                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                }
                if (Request.QueryString["COMPONENTID"] != null)
                {
                    ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                    ddlComponent.SelectedValue = Request.QueryString["COMPONENTID"].ToString();
                }
                if (Request.QueryString["storeitemnumber"] != null)
                    txtPartNumber.Text = Request.QueryString["storeitemnumber"].ToString();
                ddlStockClassType.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();

                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                {
                    ddlStockClassType.Visible = true;
                    if (Filter.CurrentPurchaseStockClass == "")
                        Filter.CurrentPurchaseStockClass = "407";
                    ddlStockClassType.SelectedHard = Filter.CurrentPurchaseStockClass;
                    lblClassName.Visible = true;
                    lblClassName.Text = "Store Type";
                    txtComponent.Visible = false;
                    ddlComponent.Visible = false;
                }
                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["FIRSTINITIALIZED"] = true;

                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                    lblMakerRef.Text = "Product Code";
                else
                    lblMakerRef.Text = "Maker Reference";
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void BindComponent()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        ds = PhoenixCommonInventory.ComponentSearch(Filter.CurrentPurchaseVesselSelection, null, null,
                null, null, null, null, null,
                null,
                null, null, null, null,
                1,
                10000,
                ref iRowCount,
                ref iTotalPageCount);

        ddlComponent.DataSource = ds;
        ddlComponent.DataBind();
        ddlComponent.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

    }
    protected void StoreItemInOutTransaction_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " closeTelerikWindow('Filter_1','detail','true');";
                Script += "</script>" + "\n";
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
            }

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                rgvLine.CurrentPageIndex = 0;
                rgvLine.EditIndexes.Add(0);
                rgvLine.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void rgvLine_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 10;
        int iTotalPageCount = 10;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string stockclass = "";
        if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
            stockclass = ddlStockClassType.SelectedHard == "" ? Filter.CurrentPurchaseStockClass : ddlStockClassType.SelectedHard;

        DataSet ds = PhoenixPurchaseOrderLine.SearchWantedLineItems(General.GetNullableGuid(ddlComponent.SelectedValue), Filter.CurrentPurchaseVesselSelection, General.GetNullableGuid(ViewState["orderid"].ToString())
                         , txtPartNumber.Text, txtItemName.Text, General.GetNullableString(txtMakerReference.Text)
                         , null
                         , null, General.GetNullableInteger(stockclass), sortexpression, sortdirection
                         , rgvLine.CurrentPageIndex + 1
                         , rgvLine.PageSize
                         , ref iRowCount
                         , ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
            {
                txtComponent.Text = ds.Tables[0].Rows[0]["FLDCOMPONENTNAME"].ToString();
            }

        }

        rgvLine.DataSource = ds;
        rgvLine.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void rgvLine_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem item = (GridEditableItem)e.Item;
         
            InsertOrderLineItem(
                   item.GetDataKeyValue("FLDITEMID").ToString(),
                   item.GetDataKeyValue("FLDORDERLINEID").ToString(),
                   item.GetDataKeyValue("FLDCOMPONENTID").ToString(),
                   ((RadNumericTextBox)item["QUANTITY"].FindControl("txtQuantityEdit")).Text
                );
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void rgvLine_EditCommand(object sender, GridCommandEventArgs e)
    {
        rgvLine.SelectedIndexes.Clear();
        GridDataItem item = (GridDataItem)e.Item;

        rgvLine.SelectedIndexes.Add(e.Item.ItemIndex);
    }
    private void InsertOrderLineItem(string storeitemid, string orderlineid, string componentid, string quantity)
    {
        try
        {
            if (quantity.Trim() == "")
            {
                return;
            }
            if (!IsValidLineItemEdit(quantity))
            {
                ucError.Visible = true;
                return;
            }
            if (General.GetNullableGuid(orderlineid) == null)
            {
                PhoenixPurchaseOrderLine.InsertOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(storeitemid), General.GetNullableGuid(componentid), new Guid(ViewState["orderid"].ToString())
                     , General.GetNullableDecimal(quantity), Filter.CurrentPurchaseStockType.ToString());
            }
            else
            {
                PhoenixPurchaseOrderLine.UpdateOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(orderlineid), General.GetNullableDecimal(quantity));
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidLineItemEdit(string quantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        decimal result;

        if (quantity.Trim() != "")
        {
            if (decimal.TryParse(quantity, out result) == false)
                ucError.ErrorMessage = "Item requested quantity should be a valid numeric value.";
        }
        return (!ucError.IsError);
    }
    protected void rgvLine_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel lb = (RadLabel)item["NAME"].FindControl("lblIsInMarket");
                Int64 result = 0;

                if (Int64.TryParse(lb.Text, out result) && result==0)
                {
                    item.ForeColor = System.Drawing.Color.Red;
                }

                RadNumericTextBox tb = (RadNumericTextBox)item["QUANTITY"].FindControl("txtQuantityEdit");
                if (tb != null) tb.Focus();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void rgvLine_PreRender(object sender, EventArgs e)
    {
        GridHeaderItem headerItem = rgvLine.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
        if (headerItem != null && Filter.CurrentPurchaseStockType == "STORE")
        {
            headerItem["MAKERREF"].Text = "Product Code";
        }

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            rgvLine.CurrentPageIndex = 0;
            rgvLine.EditIndexes.Add(0);
            rgvLine.Rebind();
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
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

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