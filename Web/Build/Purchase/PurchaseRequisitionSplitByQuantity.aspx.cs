using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PurchaseRequisitionSplitByQuantity : PhoenixBasePage
{
    DataTable dtSplit = new DataTable();
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

                menuSaveDetails.Title = "Split Lineitem [ " + PhoenixPurchaseOrderForm.FormNumber +" ]";
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

                rdoFormType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE));
                rdoFormType.DataBind();
                
                rdoFormType.SelectedValue = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE), "RQ");
                rdoFormType.Enabled = false;
                string formtype = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE), "RQ");

                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
                {
                    imgOrder.Attributes.Add("onclick", "top.openNewWindow('spnPickListOrder', 'codehelp1', '" + Session["sitepath"] + "/Common/CommonPickListOrder.aspx?ispopup=spnPickListOrder,Split&mode=custom&VESSELID=" + Filter.CurrentPurchaseVesselSelection + "&STOCKTYPE=SPARE'); ");
                }
                else if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
                {
                    imgOrder.Attributes.Add("onclick", "top.openNewWindow('spnPickListOrder', 'codehelp1', '" + Session["sitepath"] + "/Common/CommonPickListOrder.aspx?ispopup=spnPickListOrder,Split&mode=custom&VESSELID=" + Filter.CurrentPurchaseVesselSelection + "&STOCKTYPE=SERVICE'); ");
                }
                else
                {
                    imgOrder.Attributes.Add("onclick", "top.openNewWindow('spnPickListOrder', 'codehelp1', '" + Session["sitepath"] + "/Common/CommonPickListOrder.aspx?ispopup=spnPickListOrder,Split&mode=custom&VESSELID=" + Filter.CurrentPurchaseVesselSelection + "&STOCKTYPE=STORE'); ");
                }
                CreateTableStructure();
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
                Script += " fnReloadList();";
                Script += "</script>" + "\n";
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
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

        rgvLine.MasterTableView.Columns.Clear();

        GridBoundColumn serialno = new GridBoundColumn();
        rgvLine.MasterTableView.Columns.Add(serialno);
        serialno.HeaderText = "S.No";
        serialno.UniqueName = "FLDSERIALNO";
        serialno.ReadOnly = true;
        serialno.DataField = "FLDSERIALNO";
        serialno.DataType = typeof(System.Int32);
        serialno.HeaderStyle.Width = Unit.Parse("40px");
        serialno.ItemStyle.Width = Unit.Parse("40px");

        
        GridBoundColumn partNumber = new GridBoundColumn();
        rgvLine.MasterTableView.Columns.Add(partNumber);
        partNumber.HeaderText = "Part Number";
        partNumber.UniqueName = "FLDPARTNUMBER";
        partNumber.ReadOnly = true;
        partNumber.DataField = "FLDPARTNUMBER";
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

        

        GridBoundColumn unit = new GridBoundColumn();
        rgvLine.MasterTableView.Columns.Add(unit);
        unit.HeaderText = "Unit";
        unit.UniqueName = "FLDUNITNAME";
        unit.ReadOnly = true;
        unit.DataField = "FLDUNITNAME";
        unit.DataType = typeof(System.String);

        GridNumericColumn quantity = new GridNumericColumn();
        rgvLine.MasterTableView.Columns.Add(quantity);
        quantity.HeaderText = "Quantity";
        quantity.UniqueName = "FLDORDEREDQUANTITY";
        quantity.DataField = "FLDORDEREDQUANTITY";
        quantity.ReadOnly = true;
        quantity.DataType = typeof(System.Decimal);
        quantity.DataFormatString = "{0:n0}";
        quantity.DecimalDigits = 2;
        quantity.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

        GridNumericColumn splitQuantity = new GridNumericColumn();
        rgvLine.MasterTableView.Columns.Add(splitQuantity);
        splitQuantity.HeaderText = "Split Quantity";
        splitQuantity.UniqueName = "FLDSPLITQUANTITY";
        //splitQuantity.DataField = "FLDORDEREDQUANTITY";
        splitQuantity.DataType = typeof(System.Decimal);
        splitQuantity.DataFormatString = "{0:n0}";
        splitQuantity.DecimalDigits = 2;
        splitQuantity.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

        // GridNumericColumn quotedQty = new GridNumericColumn();
        // rgvLine.MasterTableView.Columns.Add(quotedQty);
        // quotedQty.HeaderText = "Quoted Qty";
        // quotedQty.UniqueName = "FLDQUANTITY";
        //// quotedQty.ReadOnly = true;
        // quotedQty.DataField = "FLDQUANTITY";
        // quotedQty.DataType = typeof(System.Decimal);
        // quotedQty.DataFormatString = "{0:n0}";
        // quotedQty.DecimalDigits = 2;
        // quotedQty.ItemStyle.HorizontalAlign = HorizontalAlign.Right;


        //GridBoundColumn price = new GridBoundColumn();
        //rgvLine.MasterTableView.Columns.Add(price);
        //price.HeaderText = "Price";
        //price.UniqueName = "FLDQUOTEDPRICE";
        //price.ReadOnly = true;
        //price.DataField = "FLDQUOTEDPRICE";
        //price.DataType = typeof(System.Decimal);
        ////price.DecimalDigits = 3;
        //price.DataFormatString = "{0:0.000}";
        //price.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

        //GridNumericColumn discount = new GridNumericColumn();
        //rgvLine.MasterTableView.Columns.Add(discount);
        //discount.HeaderText = "Discount";
        //discount.UniqueName = "FLDDISCOUNT";
        // discount.ReadOnly = true;
        //discount.DataField = "FLDDISCOUNT";
        //discount.DataType = typeof(System.Decimal);
        //discount.DecimalDigits = 2;
        //discount.DataFormatString = "{0:n0}";
        //discount.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

        //GridNumericColumn deltime = new GridNumericColumn();
        //rgvLine.MasterTableView.Columns.Add(deltime);
        //deltime.HeaderText = "Del.Time";
        //deltime.UniqueName = "FLDDELIVERYTIME";
        //deltime.ReadOnly = true;
        //deltime.DataField = "FLDDELIVERYTIME";
        //deltime.DataType = typeof(System.Int32);
        //deltime.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

        DataSet ds = PhoenixPurchaseOrderLine.OrderLineSearch(
            General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection,
            sortexpression, sortdirection, rgvLine.CurrentPageIndex + 1,
            rgvLine.PageSize, ref iRowCount, ref iTotalPageCount);
        //else
        //{
        //    ds = PhoenixPurchaseOrderLine.OrderLineSearch(new Guid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection,
        //    sortexpression, sortdirection, 1,
        //    iRowCount,
        //    ref iRowCount,
        //    ref iTotalPageCount);
        //}

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
            item["FLDSPLITQUANTITY"].Text = "0";

            DataRow[] dr = dtSplit.Select("FLDORDERLINEID = '" + item.GetDataKeyValue("FLDORDERLINEID").ToString() + "'");
            if (dr.Length > 0)
                item["FLDSPLITQUANTITY"].Text = dr[0]["FLDSPLITQUANTITY"].ToString();
        }
    }

    protected void rgvLine_PreRender(object sender, EventArgs e)
    {
        GridHeaderItem headerItem = rgvLine.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
        if (headerItem != null && Filter.CurrentPurchaseStockType == "STORE")
        {
            headerItem["FLDMAKERREFERENCE"].Text = "Product Code";
        }
        GridTableView masterTable = ((RadGrid)sender).MasterTableView;

        //GridNumericColumnEditor editor = masterTable.GetBatchColumnEditor("FLDQUANTITY") as GridNumericColumnEditor;
        //RadNumericTextBox numBox = editor.NumericTextBox;
        //numBox.IncrementSettings.InterceptArrowKeys = false;
        //numBox.IncrementSettings.InterceptMouseWheel = false;

        
        GridNumericColumnEditor editor1 = masterTable.GetBatchColumnEditor("FLDSPLITQUANTITY") as GridNumericColumnEditor;
        RadNumericTextBox numBox1 = editor1.NumericTextBox;
        numBox1.IncrementSettings.InterceptArrowKeys = false;
        numBox1.IncrementSettings.InterceptMouseWheel = false;

        //GridNumericColumnEditor editor2 = masterTable.GetBatchColumnEditor("FLDQUOTEDPRICE") as GridNumericColumnEditor;
        //RadNumericTextBox numBox2 = editor2.NumericTextBox;
        //numBox2.IncrementSettings.InterceptArrowKeys = false;
        //numBox2.IncrementSettings.InterceptMouseWheel = false;

        //GridNumericColumnEditor editor3 = masterTable.GetBatchColumnEditor("FLDDELIVERYTIME") as GridNumericColumnEditor;
        //RadNumericTextBox numBox3 = editor3.NumericTextBox;
        //numBox3.IncrementSettings.InterceptArrowKeys = false;
        //numBox3.IncrementSettings.InterceptMouseWheel = false;

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

    protected void rgvLine_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        
        foreach (GridBatchEditingCommand command in e.Commands)
        {
            Hashtable newValues = command.NewValues;
            Hashtable oldValues = command.OldValues;
            CreateTableStructure();
            if(General.GetNullableDecimal(newValues["FLDSPLITQUANTITY"].ToString())>0)
            {
                DataRow dr = dtSplit.NewRow();
                dr["FLDVESSELID"] = newValues["FLDVESSELID"].ToString();
                dr["FLDORDERLINEID"] = newValues["FLDORDERLINEID"].ToString();
                dr["FLDSPLITQUANTITY"] = newValues["FLDSPLITQUANTITY"].ToString();
                dtSplit.Rows.Add(dr);
            }
        }

        DataSet ds = new DataSet();
        ds.Tables.Add(dtSplit);
        string xml = ds.GetXml();
        if (dtSplit.Rows.Count > 0)
        {
            PhoenixPurchaseOrderLine.SplitItemsByQuantity(int.Parse(dtSplit.Rows[0]["FLDVESSELID"].ToString()), new Guid(ViewState["orderid"].ToString())
                , xml
                , 61
                , General.GetNullableInteger(rblCreation.SelectedValue)
                , General.GetNullableGuid(txtOrderId.Text)
                , General.GetNullableGuid(ViewState["quotationid"].ToString()));
        }

        //PhoenixPurchaseQuotationLine.UpdateQuotationLineBulk(
        //           PhoenixSecurityContext.CurrentSecurityContext.UserCode,
        //           quotationlineid,
        //           new Guid(quotationid),
        //           quantity,
        //           quotedprice,
        //           discount,
        //           unitid,
        //           delivery);



        ucStatus.Show("Items splitted successfully");
    }

    protected void CreateTableStructure()
    {
        if (dtSplit.Columns.Count == 0)
        {
            dtSplit.Columns.Add("FLDVESSELID");
            dtSplit.Columns.Add("FLDORDERLINEID");
            dtSplit.Columns.Add("FLDSPLITQUANTITY");
        }
        
    }
    protected void rblCreation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblCreation.SelectedValue == "2")
        {
            spnPickListOrder.Visible = true;
            lblordernumber.Visible = true;
        }
        else
        {
            spnPickListOrder.Visible = false;
            lblordernumber.Visible = false;

        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            txtOrderNumber.Text = nvc.Get("lblOrderNumber").ToString();
            txtOrderName.Text = nvc.Get("lblTitle").ToString();
            txtOrderId.Text = nvc.Get("lblOrderId").ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void rgvLine_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.PageCommandName)
        {
        }
    }
}
