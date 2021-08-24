using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Text;
using Telerik.Web.UI;
using System.Collections;

public partial class PurchaseQuotationLineItemBulkSave : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                rgvLine.PageSize = 1000;

                if (Request.QueryString["VendorName"] != null)
                    ViewState["vendorname"] = Request.QueryString["VendorName"].ToString();
                else
                    ViewState["vendorname"] = "";

                menuSaveDetails.Title = "Quotation Lineitem [ " + PhoenixPurchaseOrderForm.FormNumber + " Vendor : " + ViewState["vendorname"].ToString() + " ]";
                PhoenixToolbar toolSave = new PhoenixToolbar();
                //toolSave.AddButton("Save", "SAVE");
                toolSave.AddButton("Close", "CANCEL",ToolBarDirection.Right);
                //toolSave.AddButton( "Save", "BULKSAVE",ToolBarDirection.Right);
                menuSaveDetails.MenuList = toolSave.Show();

                if (Request.QueryString["quotationid"] != null)
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                }
                else
                {
                    ViewState["quotationid"] = "";
                }
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
                Script += " closeTelerikWindow('codehelp1','detail','true');";
                Script += "</script>" + "\n";
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
            }
            //if (CommandName.ToUpper().Equals("BULKSAVE"))
            //{
            //    string quantity = ",";
            //    string quotedprice = ",";
            //    string discount = ",";
            //    string unitid = ",";
            //    string delivery = ",";
            //    string quotationlineid = "";
            //    string quotationid = "";
            //    foreach (GridDataItem item in rgvLine.Items)
            //    {
            //        quantity += ((UserControlDecimal)item["QUOTEDQTY"].FindControl("txtQuantityEdit")).Text + ',';
            //        quotedprice += ((UserControlDecimal)item["PRICE"].FindControl("txtQuotedPriceEdit")).Text + ',';
            //        discount += ((UserControlDecimal)item["DISCOUNT"].FindControl("txtDiscountEdit")).Text + ',';
            //        unitid += ((RadDropDownList)item["UNIT"].FindControl("ddlUnit")).SelectedValue + ','; 
            //        delivery += ((UserControlDecimal)item["DELTIME"].FindControl("txtDeliveryEdit")).Text + ',';
            //        quotationlineid += item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString() + ',';
            //        quotationid = item.GetDataKeyValue("FLDQUOTATIONID").ToString();
            //    }

            //    PhoenixPurchaseQuotationLine.UpdateQuotationLineBulk(
            //           PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            //           quotationlineid,
            //           new Guid(quotationid),
            //           quantity,
            //           quotedprice,
            //           discount,
            //           unitid,
            //           delivery);

            //    ucStatus.Text = "Quotation line item details updated successfully.";
            //}
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

        rgvLine.MasterTableView.Columns.Clear();

        GridBoundColumn serialno = new GridBoundColumn();
        rgvLine.MasterTableView.Columns.Add(serialno);
        serialno.HeaderText = "S.No";
        serialno.UniqueName = "FLDROWNUMBER";
        serialno.ReadOnly = true;
        serialno.DataField = "FLDROWNUMBER";
        serialno.DataType = typeof(System.Int32);
        serialno.HeaderStyle.Width = Unit.Parse("40px");
        serialno.ItemStyle.Width = Unit.Parse("40px");

        GridBoundColumn partNumber = new GridBoundColumn();
        rgvLine.MasterTableView.Columns.Add(partNumber);
        partNumber.HeaderText = "Part Number";
        partNumber.UniqueName = "FLDNUMBER";
        partNumber.ReadOnly = true;
        partNumber.DataField = "FLDNUMBER";
        partNumber.DataType = typeof(System.String);
        partNumber.HeaderStyle.Width = Unit.Parse("10%");
        partNumber.ItemStyle.Width = Unit.Parse("10%");

        GridBoundColumn partName = new GridBoundColumn();
        rgvLine.MasterTableView.Columns.Add(partName);
        partName.HeaderText = "Item Name";
        partName.UniqueName = "FLDNAME";
        partName.ReadOnly = true;
        partName.DataField = "FLDNAME";
        partName.DataType = typeof(System.String);
        partName.HeaderStyle.Width = Unit.Parse("40%");
        partName.ItemStyle.Width = Unit.Parse("40%");


        GridBoundColumn markeRef = new GridBoundColumn();
        rgvLine.MasterTableView.Columns.Add(markeRef);
        markeRef.HeaderText = "Maker Reference";
        markeRef.UniqueName = "FLDMAKERREFERENCE";
        markeRef.ReadOnly = true;
        markeRef.DataField = "FLDMAKERREFERENCE";
        markeRef.DataType = typeof(System.String);

        GridNumericColumn rob = new GridNumericColumn();
        rgvLine.MasterTableView.Columns.Add(rob);
        rob.HeaderText = "ROB";
        rob.UniqueName = "FLDROBQUANTITY";
        rob.ReadOnly = true;
        rob.DataField = "FLDROBQUANTITY";
        rob.DataFormatString = "{0:n0}";
        rob.DataType = typeof(System.Decimal);
        rob.DecimalDigits = 2;
        rob.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

        GridNumericColumn quantity = new GridNumericColumn();
        rgvLine.MasterTableView.Columns.Add(quantity);
        quantity.HeaderText = "Quantity";
        quantity.UniqueName = "FLDORDEREDQUANTITY";
        quantity.ReadOnly = true;
        quantity.DataField = "FLDORDEREDQUANTITY";
        quantity.DataType = typeof(System.Decimal);
        quantity.DataFormatString = "{0:n0}";
        quantity.DecimalDigits = 2;
        quantity.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

        GridBoundColumn unit = new GridBoundColumn();
        rgvLine.MasterTableView.Columns.Add(unit);
        unit.HeaderText = "Unit";
        unit.UniqueName = "FLDUNITNAME";
        unit.ReadOnly = true;
        unit.DataField = "FLDUNITNAME";
        unit.DataType = typeof(System.String);

        GridNumericColumn quotedQty = new GridNumericColumn();
        rgvLine.MasterTableView.Columns.Add(quotedQty);
        quotedQty.HeaderText = "Quoted Qty";
        quotedQty.UniqueName = "FLDQUANTITY";
       // quotedQty.ReadOnly = true;
        quotedQty.DataField = "FLDQUANTITY";
        quotedQty.DataType = typeof(System.Decimal);
        quotedQty.DataFormatString = "{0:n0}";
        quotedQty.DecimalDigits = 2;
        quotedQty.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
        

        GridBoundColumn price = new GridBoundColumn();
        rgvLine.MasterTableView.Columns.Add(price);
        price.HeaderText = "Price";
        price.UniqueName = "FLDQUOTEDPRICE";
        //price.ReadOnly = true;
        price.DataField = "FLDQUOTEDPRICE";
        price.DataType = typeof(System.Decimal);
        //price.DecimalDigits = 3;
        price.DataFormatString = "{0:0.000}";
        price.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

        GridNumericColumn discount = new GridNumericColumn();
        rgvLine.MasterTableView.Columns.Add(discount);
        discount.HeaderText = "Discount";
        discount.UniqueName = "FLDDISCOUNT";
       // discount.ReadOnly = true;
        discount.DataField = "FLDDISCOUNT";
        discount.DataType = typeof(System.Decimal);
        discount.DecimalDigits = 2;
        quotedQty.DataFormatString = "{0:n0}";
        discount.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

        GridNumericColumn deltime = new GridNumericColumn();
        rgvLine.MasterTableView.Columns.Add(deltime);
        deltime.HeaderText = "Del.Time";
        deltime.UniqueName = "FLDDELIVERYTIME";
        //deltime.ReadOnly = true;
        deltime.DataField = "FLDDELIVERYTIME";
        deltime.DataType = typeof(System.Int32);
        deltime.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

        DataSet ds = PhoenixPurchaseQuotationLine.QuotationLineSearch("",
            ViewState["quotationid"].ToString(), sortexpression, sortdirection, rgvLine.CurrentPageIndex+1,
            rgvLine.PageSize,
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
            //RadLabel lblSerialNo = (RadLabel)item["SNO"].FindControl("lblSerialNo");
            //lblSerialNo.Text = ((rgvLine.PageSize * rgvLine.CurrentPageIndex) + (item.ItemIndex + 1)).ToString();
            //RadLabel lblComponentName = (RadLabel)item["NAME"].FindControl("lblComponentName");

            
            //if (Filter.CurrentPurchaseStockType.Equals("STORE"))
            //{
            //    lblComponentName.Visible = false;
            //}

            //RadDropDownList ddlUnit = (RadDropDownList)item["UNIT"].FindControl("ddlUnit");
            //DataRowView drv = (DataRowView)item.DataItem;
            //if (ddlUnit != null)
            //{
            //    ddlUnit.DataSource = PhoenixRegistersUnit.ListPurchaseUnit(item.GetDataKeyValue("FLDPARTID").ToString(), Filter.CurrentPurchaseStockType
            //                                                         , Filter.CurrentPurchaseVesselSelection);

            //    ddlUnit.DataBind();
            //    ddlUnit.SelectedValue = drv["FLDUNITID"].ToString();
            //}
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
    protected void rgvLine_PreRender(object sender, EventArgs e)
    {
        GridHeaderItem headerItem = rgvLine.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
        if (headerItem != null && Filter.CurrentPurchaseStockType == "STORE")
        {
            headerItem["FLDMAKERREFERENCE"].Text = "Product Code";
        }
        GridTableView masterTable = ((RadGrid)sender).MasterTableView;

        GridNumericColumnEditor editor = masterTable.GetBatchColumnEditor("FLDQUANTITY") as GridNumericColumnEditor;
        RadNumericTextBox numBox = editor.NumericTextBox;
        numBox.IncrementSettings.InterceptArrowKeys = false;
        numBox.IncrementSettings.InterceptMouseWheel = false;

        GridNumericColumnEditor editor1 = masterTable.GetBatchColumnEditor("FLDDISCOUNT") as GridNumericColumnEditor;
        RadNumericTextBox numBox1 = editor1.NumericTextBox;
        numBox1.IncrementSettings.InterceptArrowKeys = false;
        numBox1.IncrementSettings.InterceptMouseWheel = false;

        //GridNumericColumnEditor editor2 = masterTable.GetBatchColumnEditor("FLDQUOTEDPRICE") as GridNumericColumnEditor;
        //RadNumericTextBox numBox2 = editor2.NumericTextBox;
        //numBox2.IncrementSettings.InterceptArrowKeys = false;
        //numBox2.IncrementSettings.InterceptMouseWheel = false;

        GridNumericColumnEditor editor3 = masterTable.GetBatchColumnEditor("FLDDELIVERYTIME") as GridNumericColumnEditor;
        RadNumericTextBox numBox3 = editor3.NumericTextBox;
        numBox3.IncrementSettings.InterceptArrowKeys = false;
        numBox3.IncrementSettings.InterceptMouseWheel = false;

    }
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

    protected void rgvLine_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        try
        {
            string quantity = ",";
            string quotedprice = ",";
            string discount = ",";
            string unitid = ",";
            string delivery = ",";
            string quotationlineid = "";
            string quotationid = ""; 


            foreach (GridBatchEditingCommand command in e.Commands)
            {
                Hashtable newValues = command.NewValues;
                Hashtable oldValues = command.OldValues;

                if (newValues["FLDQUOTEDPRICE"]!=null && General.GetNullableDecimal(newValues["FLDQUOTEDPRICE"].ToString()) != null && newValues["FLDUNITID"]!=null && General.GetNullableInteger(newValues["FLDUNITID"].ToString()) != null)
                {
                    if (newValues["FLDQUANTITY"] != oldValues["FLDQUANTITY"] || newValues["FLDDISCOUNT"] != oldValues["FLDDISCOUNT"]
                        || newValues["FLDQUOTEDPRICE"] != oldValues["FLDQUOTEDPRICE"] || newValues["FLDDELIVERYTIME"] != oldValues["FLDDELIVERYTIME"])
                    {
                        quantity += newValues["FLDQUANTITY"].ToString() + ',';
                        quotedprice += (newValues["FLDQUOTEDPRICE"]!=null? newValues["FLDQUOTEDPRICE"].ToString() : "") + ',';
                        discount += (newValues["FLDDISCOUNT"] != null ? (General.GetNullableDecimal(newValues["FLDDISCOUNT"].ToString()) != null ? newValues["FLDDISCOUNT"].ToString() : "0") : "0") + ',';
                        unitid += newValues["FLDUNITID"].ToString() + ',';
                        delivery += (newValues["FLDDELIVERYTIME"] != null ? (General.GetNullableDecimal(newValues["FLDDELIVERYTIME"].ToString())!=null? newValues["FLDDELIVERYTIME"].ToString():"") : "") + ',';
                        quotationlineid += newValues["FLDQUOTATIONLINEID"].ToString() + ',';
                        quotationid = newValues["FLDQUOTATIONID"].ToString();
                    }
                }
            }
            if (General.GetNullableGuid(quotationid) != null)
                PhoenixPurchaseQuotationLine.UpdateQuotationLineBulk(
                           PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                           quotationlineid,
                           new Guid(quotationid),
                           quantity,
                           quotedprice,
                           discount,
                           unitid,
                           delivery);

            //    foreach (GridDataItem item in rgvLine.Items)
            //    {
            //        quantity += ((UserControlDecimal)item["QUOTEDQTY"].FindControl("txtQuantityEdit")).Text + ',';
            //        quotedprice += ((UserControlDecimal)item["PRICE"].FindControl("txtQuotedPriceEdit")).Text + ',';
            //        discount += ((UserControlDecimal)item["DISCOUNT"].FindControl("txtDiscountEdit")).Text + ',';
            //        unitid += ((RadDropDownList)item["UNIT"].FindControl("ddlUnit")).SelectedValue + ','; 
            //        delivery += ((UserControlDecimal)item["DELTIME"].FindControl("txtDeliveryEdit")).Text + ',';
            //        quotationlineid += item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString() + ',';
            //        quotationid = item.GetDataKeyValue("FLDQUOTATIONID").ToString();
            //    }

            //    PhoenixPurchaseQuotationLine.UpdateQuotationLineBulk(
            //           PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            //           quotationlineid,
            //           new Guid(quotationid),
            //           quantity,
            //           quotedprice,
            //           discount,
            //           unitid,
            //           delivery);

            ucStatus.Show("Quotation line item details updated successfully.");
        }
        catch(Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
        
    }

    protected void rgvLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if(e.CommandName == RadGrid.BatchEditCommandName)
        {
            GridBatchEditingEventArgument argument = e.CommandArgument as GridBatchEditingEventArgument;
            Hashtable oldValues = argument.OldValues;
            Hashtable newValues = argument.NewValues;
        }
    }
}
