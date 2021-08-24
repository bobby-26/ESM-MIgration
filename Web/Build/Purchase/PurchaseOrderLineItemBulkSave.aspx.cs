using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class PurchaseOrderLineItemBulkSave : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolSave = new PhoenixToolbar();
            toolSave.AddButton("Close", "CANCEL", ToolBarDirection.Right);
            //toolSave.AddImageLink("javascript:bulkSaveOfOrderLine();return false;", "Save","","BULKSAVE",ToolBarDirection.Right);
            toolSave.AddButton("Save","BULKSAVE",ToolBarDirection.Right);
            menuSaveDetails.AccessRights = this.ViewState;
            menuSaveDetails.MenuList = toolSave.Show();

            if (!IsPostBack)
            {
                rgvLine.PageSize = 1000;
                
                if (Request.QueryString["orderid"] != null)
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
          
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void menuSaveDetails_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            
            if (CommandName.ToUpper().Equals("CANCEL"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " closeTelerikWindow('Filter_1','Filter','true');";
                Script += "</script>" + "\n";
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
            }
            if (CommandName.ToUpper().Equals("BULKSAVE"))
            {
                string requestedqty = ",";
                string orderedqty   = ",";
                string receivedqty  = ",";
                string orderlineid  = ",";
                string unitid       = ",";
                string orderid = "";
                foreach (GridDataItem item in rgvLine.Items)
                {
                    requestedqty += ((UserControlDecimal)item["REQUESTEDQTY"].FindControl("txtRequestedQuantityEdit")).Text + ',';
                    orderedqty += ((UserControlDecimal)item["ORDEREDQTY"].FindControl("txtOrderedQuantityEdit")).Text + ',';
                    receivedqty += ((UserControlDecimal)item["RECEIVEDQTY"].FindControl("txtReceivedQuantityEdit")).Text + ',';
                    orderlineid += item.GetDataKeyValue("FLDORDERLINEID").ToString() + ",";
                    unitid      += ((RadDropDownList)item["UNIT"].FindControl("ddlUnit")).SelectedValue + ',';
                    orderid = item.GetDataKeyValue("FLDORDERID").ToString();
                }

                PhoenixPurchaseOrderLine.UpdateOrderLineBulk(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                orderlineid,
                                new Guid(orderid),
                                requestedqty,
                                orderedqty,
                                receivedqty,
                                unitid);

                ucStatus.Text = "Orderline details updated successfully";
                //string Script = "";
                //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                ////Script += " fnReloadList();";
                //Script += " savedata('" + requestedqty + "', '" + orderedqty + "', '" + receivedqty + "', '" + unitid + "', '" + orderlineid + "', '" + orderid + "');";
                ////Script += " fnReloadList();";
                //Script += "</script>" + "\n";

                //RadScriptManager.RegisterStartupScript(this, this.GetType(),
                //                 "BookMarkScript", Script, false);
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
        int iRowCount = 1000;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;

        DataSet ds = PhoenixPurchaseOrderLine.OrderLineSearch(new Guid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection,
            sortexpression, sortdirection, 1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);
        
        rgvLine.DataSource = ds;
        rgvLine.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
   
    }
    protected void rgvLine_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            RadLabel lblComponentName = (RadLabel)item["NAME"].FindControl("lblComponentName");

            UserControlDecimal txtReceivedQuantityEdit = (UserControlDecimal)item["RECEIVEDQTY"].FindControl("txtReceivedQuantityEdit");

            if (Filter.CurrentPurchaseStockType.Equals("STORE"))
            {
                lblComponentName.Visible = false;
                
            }



            RadDropDownList ddlUnit = (RadDropDownList)item["UNIT"].FindControl("ddlUnit");
            DataRowView drv = (DataRowView)item.DataItem;
            if (ddlUnit != null)
            {
                ddlUnit.DataSource = PhoenixRegistersUnit.ListPurchaseUnit(item.GetDataKeyValue("FLDPARTID").ToString(), Filter.CurrentPurchaseStockType
                                                                     , Filter.CurrentPurchaseVesselSelection);

                ddlUnit.DataBind();
                ddlUnit.SelectedValue = drv["FLDUNITID"].ToString();
            }
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
    //protected void gvVendorItem_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow &&
    //        (e.Row.RowState == DataControlRowState.Normal ||
    //        e.Row.RowState == DataControlRowState.Alternate))
    //    {
    //        e.Row.TabIndex = 0;
    //        e.Row.Attributes["onclick"] =
    //          string.Format("javascript:SelectRow(this, {0}, null);", e.Row.RowIndex);
    //        e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event);";
    //        e.Row.Attributes["onselectstart"] = "javascript:return false;";
    //    }
    //}
    //protected void rgvLine_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    //{
        
    //}
    //protected void rgvLine_ItemUpdated(object source, GridUpdatedEventArgs e)
    //{
    //    try
    //    {

    //    }
    //    catch(Exception ex)
    //    {
    //        e.KeepInEditMode = true;
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    GridEditableItem item = (GridEditableItem)e.Item;
    //    String id = item.GetDataKeyValue("FLDORDERLINEID").ToString();

        
    //}
    private bool IsValidRemark()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ViewState["quotationlineid"] == null || ViewState["quotationlineid"].ToString().Trim().Equals(""))
            ucError.ErrorMessage = "Line item selection is required. ";

        //if (rate.Trim().Equals("") || rate == "0")
        //    ucError.ErrorMessage = "Item Rate is required.";

        //if (quantity.Trim().Equals("") || quantity == "0")
        //    ucError.ErrorMessage = "Quantity  is required.";

        //if (General.GetNullableGuid(ViewState["quotationid"].ToString()) == null)
        //    ucError.ErrorMessage = "Quotationid is required.";

        return (!ucError.IsError);
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
