using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionIncidentPurchaseFormLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        SessionUtil.PageAccessRights(this.ViewState);
        SessionUtil.PageFieldViewPermission(this.ViewState);
        try
        {
            if (Filter.CurrentInspectionPurchaseStockType != null && Filter.CurrentInspectionPurchaseStockType == "STORE")
                txtItemNumber.MaskText = "##.##.##";
            else if (Filter.CurrentInspectionPurchaseStockType != null && Filter.CurrentInspectionPurchaseStockType == "SPARE")
                txtItemNumber.MaskText = "###.##.##.###";

            MenuLineItemGeneral.Title = "Line Item ( " + PhoenixInspectionAuditPurchaseForm.FormNumber + " )";

            if (!IsPostBack)
            {                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["NEW"] = "false";
                if (!Filter.CurrentInspectionPurchaseVesselSendDateSelection.ToUpper().Equals("") && !(Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null))
                {
                    MenuLineItemGeneral.Visible = false;
                    lblPrice.Visible = false;
                    txtPrice.Visible = false;
                }

                if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SPARE"))
                {
                    cmdShowComponent.OnClientClick = "return showPickList('spnPickComponent', 'codehelp1', '', 'Common/CommonPickListComponent.aspx?vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&framename=ifMoreInfo', true);";
                    cmdShowItem.Attributes.Add("onclick", "return showPickList('spnPickItem', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListSpareItemByComponent.aspx?mode=custom&vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&txtnumber=" + txtItemNumber.Text + "&txtname=" + txtPartName.Text + "&txtMakerRef=" + txtMakerReference.Text + "&COMPONENTID=" + txtComponentID.Text + "', true); ");
                }
                else
                {
                    string stockclass = "407";
                    if (Filter.CurrentInspectionPurchaseStockClass != "")
                        stockclass = Filter.CurrentInspectionPurchaseStockClass;
                    
                    cmdShowComponent.OnClientClick = "return showPickList('spnPickComponent', 'codehelp1', '', '../Common/CommonPickListStoreType.aspx?vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&framename=ifMoreInfo', true);";
                    cmdShowItem.Attributes.Add("onclick", "return showPickList('spnPickItem', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListStoreItem.aspx?storetype=" + stockclass + "&mode=custom&vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&txtnumber=" + txtItemNumber.Text + "&txtname=" + txtPartName.Text + "', true);");
                    lblmakerRef.Text = "Product Code";
                    trComponent.Visible = false;
                }
                ucReciptstatus.QuickTypeCode = ((int)PhoenixQuickTypeCode.RECEIPTSTATUS).ToString();

                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                }
                FieldSetViewState();
                cmdShowComponent.Attributes.Add("onclick", "return showPickList('spnPickComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx', true);");
            }

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionIncidentPurchaseFormLineItem.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvLineItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "Print");
            txtComponentID.Attributes.Add("style", "visibility:hidden");
            string orderid1 = Request.QueryString["orderid"];

            int lockforvesselyn = 0;
            PhoenixInspectionIncident.IncidentVesselUnlockCheck(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableGuid(Filter.CurrentIncidentID)
                , ref lockforvesselyn);

            if (lockforvesselyn == 0)
            {
                if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SERVICE"))
                {
                    if (Filter.CurrentSelectedIncidentMenu == null)
                        toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','', '" + Session["sitepath"] + "/Inspection/InspectionPurchaseOrderLineItemServiceSelection.aspx?orderid=" + ViewState["orderid"] + "');return false;", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
                }
                else
                {
                    if (Filter.CurrentSelectedIncidentMenu == null)
                        toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','', '" + Session["sitepath"] + "/Inspection/InspectionPurchaseOrderLineItemSelection.aspx?orderid=" + ViewState["orderid"] + "');return false;", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
                }
                if (Filter.CurrentSelectedIncidentMenu == null)
                    toolbargrid.AddFontAwesomeButton("../Inspection/InspectionIncidentPurchaseFormLineItem.aspx", "Delete", "<i class=\"fas fa-trash\"></i>", "BulkDelete");
            }
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Report','', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=3&reportcode=REQUISTIONFORM&orderid=" + ViewState["orderid"] + "');return false;", "Requisition Form", "<i class=\"fas fa-file-alt\"></i>", "REQUISTION");
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
            MenuLineItemGeneral.AccessRights = this.ViewState;
            if (Filter.CurrentSelectedIncidentMenu == null)
                MenuLineItemGeneral.MenuList = toolbarmain.Show();
            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataEdit(string orderid, string orderlineid)
    {
        if (orderlineid != null && orderlineid != "")
        {
            DataSet lineitemdataset = PhoenixInspectionAuditPurchaseForm.EditOrderLine(new Guid(orderlineid));
            if (lineitemdataset.Tables[0].Rows.Count > 0)
            {
                DataRow dr = lineitemdataset.Tables[0].Rows[0];

                ucUnit.ItemType = Filter.CurrentInspectionPurchaseStockType.ToString();
                ucUnit.ItemId = dr["FLDPARTID"].ToString();
                ucUnit.VesslId = Filter.CurrentInspectionPurchaseVesselSelection;

                txtItemNumber.Text = dr["FLDPARTNUMBER"].ToString();
                txtServiceNumber.Text = dr["FLDPARTNUMBER"].ToString();
                txtPartName.Text = dr["FLDNAME"].ToString();
                //MenuLineItemGeneral.Title = "Line Item ( " + dr["FLDNAME"].ToString() + " )"; PhoenixInspectionAuditPurchaseForm.FormNumber
                
                txtComponentID.Text = dr["FLDFORCOMPID"].ToString();
                txtComponent.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
                txtExtraNumber.Text = dr["FLDEXTRANO"].ToString();
                txtMakerReference.Text = dr["FLDMAKERREF"].ToString();
                txtStatus.Text = dr["FLDSTATUSNAME"].ToString();
                txtOrderQty.Text = String.Format("{0:##,##0}", dr["FLDORDEREDQUANTITY"]);
                txtPrice.Text = String.Format("{0:##,###,##0.00}", dr["FLDPRICE"]);
                txtRecivedQty.Text = String.Format("{0:##,##0}", dr["FLDRECEIVEDQUANTITY"]);
                txtRequestedQty.Text = String.Format("{0:##,##0}", dr["FLDREQUESTEDQUANTITY"]);
                txtCanceledQty.Text = String.Format("{0:##,##0}", dr["FLDCANCELLEDQUANTITY"]);

                if (dr["FLDINCLUDEONFORM"].ToString() == "1")
                    chkIncludeOnForm.Checked = true;
                if (dr["FLDBUDGETED"].ToString() == "1")
                    chkBudgetedPurchase.Checked = true;
                if (dr["FLDCURRENCYID"] != null && dr["FLDCURRENCYID"].ToString().Trim() != "")
                    ucCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
                else
                    ucCurrency.SelectedCurrency = PhoenixInspectionAuditPurchaseForm.DefaultCurrency;

                if (dr["FLDUNITID"] != null && dr["FLDUNITID"].ToString().Trim() != "")
                    ucUnit.SelectedUnit = dr["FLDUNITID"].ToString();

                ucReciptstatus.SelectedQuick = dr["FLDRECEIPTSTATUSID"].ToString();
                txtDrawingNo.Text = dr["FLDDRAWINGNUMBER"].ToString();
                txtPosition.Text = dr["FLDPOSITION"].ToString();
            }
        }
        else
            ViewState["NEW"] = true;

    }
    private void BindData(string stockitemcode)
    {
        DataSet stockitemdataset = new DataSet();
        stockitemdataset = PhoenixInspectionAuditPurchaseForm.GetStockItem(new Guid(stockitemcode), new Guid(ViewState["orderid"].ToString()));
        if (stockitemdataset.Tables[0].Rows.Count > 0)
        {
            DataRow dr = stockitemdataset.Tables[0].Rows[0];
            ViewState["StockId"] = dr["FLDSTOCKITEMID"].ToString();
            txtItemNumber.Text = dr["FLDNUMBER"].ToString();
            txtPartName.Text = dr["FLDNAME"].ToString();
            txtExtraNumber.Text = dr["FLDEXTRANUMBER"].ToString();
            txtRequestedQty.Text = String.Format("{0:####0}", dr["FLDWANTED"]);
            txtMakerReference.Text = dr["FLDMAKERREFERENCE"].ToString();
            ucUnit.SelectedUnit = dr["FLDUNITID"].ToString();
            txtComponentID.Text = dr["FLDCOMPONENTID"].ToString();
            txtComponent.Text = dr["FLDCOMPONENTNUMBER"].ToString();
            txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
        }
    }
    protected void MenuLineItemGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["NEW"].ToString().ToUpper() != "TRUE")
                {
                    ucError.ErrorMessage = "Sorry, You can not make any more changes here.";
                    ucError.Visible = true;
                    return;
                }
                if (!IsValidForm())
                {
                    ucError.Visible = true;
                    return;
                }
                if (Convert.ToString(ViewState["orderlineid"]) == "")
                {
                    InsertOrderLineItem();
                    ViewState["NEW"] = "false";
                    if (ViewState["orderlineid"] != null)
                    {
                        BindDataEdit(ViewState["orderid"].ToString(), ViewState["orderlineid"].ToString());
                    }
                    Rebind();
                }
                else
                {
                    ucError.ErrorMessage = "Sorry,You can not make any more changes here.";
                    ucError.Visible = true;
                    return;
                }
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                ClearTextBox();
                ViewState["SaveStatus"] = "New";
                ViewState["orderlineid"] = null;
                ViewState["NEW"] = "true";
            }
            if (CommandName.ToUpper().Equals("ADDLIST"))
            {
                Response.Redirect("PurchaseOrderLineItemSelection.aspx?orderid=" + ViewState["orderid"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ClearTextBox()
    {
        txtItemNumber.Text = "";
        txtPartName.Text = "";
        txtComponentID.Text = "";
        txtComponent.Text = "";
        txtComponentName.Text = "";
        txtExtraNumber.Text = "";
        txtMakerReference.Text = "";
        txtStatus.Text = "";
        txtOrderQty.Text = "";
        txtPrice.Text = "";
        txtRecivedQty.Text = "";
        txtRequestedQty.Text = "";
        txtCanceledQty.Text = "";
        txtDrawingNo.Text = "";
        txtPosition.Text = "";
        txtServiceNumber.Text = "";
        chkIncludeOnForm.Checked = false;
        chkBudgetedPurchase.Checked = false;
        ucUnit.PurchaseUnitList = PhoenixRegistersUnit.ListPurchaseUnit(null, null
                                                                           , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        NameValueCollection nvc = Filter.CurrentPickListSelection;
        if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SERVICE"))
        {
            txtItemNumber.Text = nvc.Get("lblWorkOrderNumber").ToString();
            txtServiceNumber.Text = nvc.Get("lblWorkOrderNumber").ToString();
            txtPartName.Text = nvc.Get("lblTitle").ToString();
            txtComponentID.Text = nvc.Get("lblComponentId").ToString();
            txtComponent.Text = nvc.Get("lblComponentNumber").ToString();
            txtComponentName.Text = nvc.Get("lblComponent").ToString();
            ViewState["StockId"] = nvc.Get("lblWorkOrderId").ToString();
            txtRequestedQty.Text = "1";
            txtOrderQty.Text = "1";
            ucUnit.ItemId = ViewState["StockId"].ToString();
            ucUnit.ItemType = Filter.CurrentInspectionPurchaseStockType.ToString();
            ucUnit.VesslId = Filter.CurrentInspectionPurchaseVesselSelection;
            ViewState["SaveStatus"] = "New";
        }
        else
        {
            string stockitemcode = nvc.Get("lblStockItemId").ToString();
            ViewState["SaveStatus"] = "New";
            ucUnit.PurchaseUnitList = PhoenixRegistersUnit.ListPurchaseUnit(stockitemcode, Filter.CurrentInspectionPurchaseStockType.ToString()
                                                                            , Filter.CurrentInspectionPurchaseVesselSelection);
            BindData(stockitemcode);
        }

    }
    private void InsertOrderLineItem()
    {
        int chkbudget = chkBudgetedPurchase.Checked == true ? 1 : 0;
        int chkforms = chkIncludeOnForm.Checked == true ? 1 : 0;

        string stockitemguid = ViewState["StockId"] != null ? ViewState["StockId"].ToString() : "";

        PhoenixInspectionAuditPurchaseForm.InsertOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtComponentID.Text,
            General.GetNullableInteger(ucUnit.SelectedUnit), General.GetNullableInteger(ucCurrency.SelectedCurrency),
            stockitemguid, new Guid(ViewState["orderid"].ToString()),
            txtItemNumber.Text, txtPartName.Text,
            txtMakerReference.Text, General.GetNullableDecimal(txtPrice.Text),
            General.GetNullableDecimal(txtRequestedQty.Text), General.GetNullableDecimal(txtRecivedQty.Text),
            General.GetNullableDecimal(txtCanceledQty.Text), PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMSTATUS), "ACT"),
            chkforms, chkbudget, General.GetNullableInteger(ucReciptstatus.SelectedQuick), General.GetNullableDecimal(txtOrderQty.Text), txtDrawingNo.Text);

    }
    private void UpdateOrderLineItem()
    {
        int chkbudget = chkBudgetedPurchase.Checked == true ? 1 : 0;
        int chkforms = chkIncludeOnForm.Checked == true ? 1 : 0;

        PhoenixInspectionAuditPurchaseForm.UpdateOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderlineid"].ToString()),
            txtComponentID.Text, General.GetNullableInteger(ucUnit.SelectedUnit),
            General.GetNullableInteger(ucCurrency.SelectedCurrency), General.GetNullableDecimal(txtPrice.Text),
            General.GetNullableDecimal(txtRequestedQty.Text), General.GetNullableDecimal(txtOrderQty.Text)
            , General.GetNullableDecimal(txtRecivedQty.Text), General.GetNullableInteger(txtCanceledQty.Text),
            PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMSTATUS), "ACT"),
            chkforms, chkbudget,
           General.GetNullableInteger(ucReciptstatus.SelectedQuick), txtMakerReference.Text, txtDrawingNo.Text, txtItemNumber.Text, txtPartName.Text);
    }
    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtPartName.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Part name is required.";
        if (ViewState["orderid"] == null)
            ucError.ErrorMessage = "Order number is required.";
        if (ucUnit.SelectedUnit.ToUpper().Trim() == "DUMMY" || ucUnit.SelectedUnit.Trim() == "")
            ucError.ErrorMessage = "Unit is Required.";
        string a = ucUnit.SelectedUnit;
        if (txtRequestedQty.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Requested Quantity is required.";

        if (Filter.CurrentInspectionPurchaseStockType.Equals("SPARE"))
        {
            if (txtComponentID.Text.Trim().Equals(""))
                ucError.ErrorMessage = "Component is required.";
        }

        return (!ucError.IsError);
    }
    protected void txtCanceledQty_TextChanged(object sender, EventArgs e)
    {
        if (txtOrderQty.Text.Trim().Equals(""))
            txtOrderQty.Text = "0";
        if (decimal.Parse(txtOrderQty.Text) < decimal.Parse(txtCanceledQty.Text))
            txtCanceledQty.Text = "0";
    }
    private void FieldSetViewState()
    {
        lblPrice.Visible = IsVisible("lblPrice");
        txtPrice.Visible = IsVisible("txtPrice");
    }
    public bool IsVisible(string command)
    {
        NameValueCollection nvc = null;
        if (ViewState["FIELDVIEWPERMISSION"] == null)
            return true;
        else
        {
            nvc = (NameValueCollection)ViewState["FIELDVIEWPERMISSION"];
            return (nvc[command] == "0") ? false : true;
        }
    }

    protected void MenuOrderLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("BULKDELETE"))
            {
                BulkDelete();
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BulkDelete()
    {
        string selectedlines = ",";
        foreach (GridDataItem gvr in gvLineItem.Items)
        {
            if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked.Equals(true))
            {
                selectedlines = selectedlines + ((RadLabel)(gvr.FindControl("lblLineid"))).Text + ",";
            }
        }
        if (selectedlines.Length > 1)
        {
            PhoenixInspectionAuditPurchaseForm.BulkDeleteOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, selectedlines);
            ViewState["orderlineid"] = null;
            Rebind();
            if (ViewState["orderlineid"] == null)
            {
                ClearTextBox();
            }
        }
        else
        {
            ucError.ErrorMessage = "Please select items for Delete.";
            ucError.Visible = true;
            return;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns;
        string[] alCaptions;

        if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SPARE") || Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SERVICE"))
        {
            alColumns = new string[13] {"FLDSERIALNO", "FLDPARTNUMBER", "FLDMAKERREFERENCE", "FLDNAME", "FLDROBQUANTITY", "FLDREQUESTEDQUANTITY",
                                 "FLDUNITNAME", "FLDSPLITFORMNO", "FLDORDEREDQUANTITY", "FLDSUBACCOUNT", "FLDOWNERACCOUNT", "FLDQUICKNAME", "FLDCHKREMARKS" };
            alCaptions = new string[13] {"S. No.", "Number", "Maker Reference", "Name", "ROB", "Requested Qty",
                                 "Unit", "From Order", "Order Qty", "Budget Code", "Owner Budget Code", "Receipt Status", "Received Remarks" };
        }
        else
        {
            alColumns = new string[13] {"FLDSERIALNO", "FLDPARTNUMBER", "FLDMAKERREFERENCE", "FLDNAME", "FLDROBQUANTITY", "FLDREQUESTEDQUANTITY",
                                 "FLDUNITNAME", "FLDSPLITFORMNO", "FLDORDEREDQUANTITY", "FLDSUBACCOUNT", "FLDOWNERACCOUNT", "FLDQUICKNAME", "FLDCHKREMARKS" };
            alCaptions = new string[13] {"S. No.", "Number", "Product Code", "Name", "ROB", "Requested Qty",
                                 "Unit", "From Order", "Order Qty", "Budget Code", "Owner Budget Code", "Receipt Status", "Received Remarks" };
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixInspectionAuditPurchaseForm.OrderLineSearch(
                                                                        General.GetNullableGuid(ViewState["orderid"].ToString()),
                                                                        sortexpression,
                                                                        sortdirection,
                                                                        (int)ViewState["PAGENUMBER"],
                                                                        iRowCount,
                                                                        ref iRowCount,
                                                                        ref iTotalPageCount);

        gvLineItem.DataSource = ds;
        gvLineItem.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["orderlineid"] == null && ViewState["NEW"] != null && ViewState["NEW"].ToString().ToUpper() != "TRUE")
            {
                ViewState["orderlineid"] = ds.Tables[0].Rows[0]["FLDORDERLINEID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                gvLineItem.SelectedIndexes.Clear();
                BindDataEdit(ViewState["orderid"].ToString(), ViewState["orderlineid"].ToString());
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ViewState["SaveStatus"] = "New";
            ViewState["NEW"] = "true";
        }
        ViewState["ROWCOUNT"] = iRowCount;
        General.SetPrintOptions("gvLineItem", "Order Form Line Items", alCaptions, alColumns, ds);
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns;
        string[] alCaptions;

        if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SPARE") || Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SERVICE"))
        {
            alColumns = new string[13] {"FLDSERIALNO", "FLDPARTNUMBER", "FLDMAKERREFERENCE", "FLDNAME", "FLDROBQUANTITY", "FLDREQUESTEDQUANTITY",
                                 "FLDUNITNAME", "FLDSPLITFORMNO", "FLDORDEREDQUANTITY", "FLDSUBACCOUNT", "FLDOWNERACCOUNT", "FLDQUICKNAME", "FLDCHKREMARKS" };
            alCaptions = new string[13] {"S. No.", "Number", "Maker Reference", "Name", "ROB", "Requested Qty",
                                 "Unit", "From Order", "Order Qty", "Budget Code", "Owner Budget Code", "Receipt Status", "Received Remarks" };
        }
        else
        {
            alColumns = new string[13] {"FLDSERIALNO", "FLDPARTNUMBER", "FLDMAKERREFERENCE", "FLDNAME", "FLDROBQUANTITY", "FLDREQUESTEDQUANTITY",
                                 "FLDUNITNAME", "FLDSPLITFORMNO", "FLDORDEREDQUANTITY", "FLDSUBACCOUNT", "FLDOWNERACCOUNT", "FLDQUICKNAME", "FLDCHKREMARKS" };
            alCaptions = new string[13] {"S. No.", "Number", "Product Code", "Name", "ROB", "Requested Qty",
                                 "Unit", "From Order", "Order Qty", "Budget Code", "Owner Budget Code", "Receipt Status", "Received Remarks" };
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixInspectionAuditPurchaseForm.OrderLineSearch(
                                                                         General.GetNullableGuid(ViewState["orderid"].ToString()),
                                                                         sortexpression,
                                                                         sortdirection,
                                                                         (int)ViewState["PAGENUMBER"],
                                                                         iRowCount,
                                                                         ref iRowCount,
                                                                         ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OrderFormLineItems.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Order Form Line Items</center></h3></td>");
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
    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "RowClick")
            {
                ViewState["orderlineid"] = ((RadLabel)e.Item.FindControl("lblLineid")).Text;
                ViewState["DTKEY"] = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                Rebind();
                BindDataEdit(ViewState["orderid"].ToString(), ViewState["orderlineid"].ToString());
                e.Item.Selected = true;
            }
            if (e.CommandName.ToUpper().Equals("SELECTITEM"))
            {
                ViewState["orderlineid"] = ((RadLabel)e.Item.FindControl("lblLineid")).Text;
                ViewState["DTKEY"] = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                Rebind();
                BindDataEdit(ViewState["orderid"].ToString(), ViewState["orderlineid"].ToString());
                e.Item.Selected = true;
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionAuditPurchaseForm.DeleteOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(((RadLabel)e.Item.FindControl("lblLineid")).Text));
                ViewState["orderlineid"] = null;
                Rebind();
                if (ViewState["orderlineid"] != null)
                    BindDataEdit(ViewState["orderid"].ToString(), ViewState["orderlineid"].ToString());
                else
                    ClearTextBox();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void UpdateOrderQuantity(string orderlineid, string orderquantity, string budgetid, string ownerbudgetid)
    {
        try
        {
            PhoenixInspectionAuditPurchaseForm.UpdateOrderQuantity(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(orderlineid), General.GetNullableDecimal(orderquantity), General.GetNullableInteger(budgetid), General.GetNullableGuid(ownerbudgetid));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLineItem_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton dbdelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (dbdelete != null)
            {
                dbdelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (Filter.CurrentAuditMenu != null && Filter.CurrentAuditMenu.ToString() == "log")
                {
                    dbdelete.Visible = false;
                }
                if (!SessionUtil.CanAccess(this.ViewState, dbdelete.CommandName)) dbdelete.Visible = false;
            }
            LinkButton db = (LinkButton)e.Item.FindControl("cmdEdit");

            if (db != null)
            {
                if (Filter.CurrentAuditMenu != null && Filter.CurrentAuditMenu.ToString() == "log")
                {
                    db.Visible = false;
                }
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }


            RadLabel lblcomponentName = (RadLabel)e.Item.FindControl("lblComponentName");

            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblVesselid");
            RadLabel lblcomponentid = (RadLabel)e.Item.FindControl("lblComponentId");
            ImageButton img = (ImageButton)e.Item.FindControl("imgComponentDetails");
            if (img != null) img.Attributes.Add("onclick", "javascript:openNewWindow('Component', '', '" + Session["sitepath"] + "/Purchase/PurchaseFormItemComponentDetails.aspx?COMPONENTID=" + lblcomponentid.Text + "&VESSELID=" + lblvesselid.Text + "'); return false;");

            if (Filter.CurrentInspectionPurchaseStockType.Equals("STORE"))
            {
                lblcomponentName.Visible = false;
                if (img != null) img.Visible = false;
            }
            lblcomponentid = (RadLabel)e.Item.FindControl("lblPartId");
            LinkButton imgDetail = (LinkButton)e.Item.FindControl("cmdDetail");
            RadLabel lblisitemdetails = (RadLabel)e.Item.FindControl("lblIsItemDetails");
            if (imgDetail != null)
            {
                if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SERVICE"))
                    imgDetail.Attributes.Add("onclick", "javascript:openNewWindow('Component', '', '" + Session["sitepath"] + "/Purchase/PurchaseServiceDetail.aspx?WORKORDERID=" + lblcomponentid.Text + "&VESSELID=" + lblvesselid.Text + "&STOCKTYPE=" + Filter.CurrentInspectionPurchaseStockType.ToString() + "'); return false;");
                else
                    imgDetail.Attributes.Add("onclick", "javascript:openNewWindow('Component', '', '" + Session["sitepath"] + "/Purchase/PurchaseSpareItemDetail.aspx?SPAREITEMID=" + lblcomponentid.Text + "&VESSELID=" + lblvesselid.Text + "&STOCKTYPE=" + Filter.CurrentInspectionPurchaseStockType.ToString() + "'); return false;");

                if (lblisitemdetails.Text.ToUpper().Equals("1"))
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-file-pdf\"></i></span>";
                    imgDetail.Controls.Add(html);
                }
                else
                {
                    imgDetail.Visible = false;
                }

                //imgDetail.ImageUrl = lblisitemdetails.Text.ToUpper().Equals("1") ? Session["images"] + "/part-detail.png" : Session["images"] + "/spacer.gif";
            }

            RadLabel lblIsdefault = (RadLabel)e.Item.FindControl("lblIsFormNotes");
            ImageButton imgFlag = (ImageButton)e.Item.FindControl("imgFlag");
            //imgFlag.ImageUrl = !lblIsdefault.Text.ToUpper().Equals("") ? Session["images"] + "/detail-flag.png" : Session["images"] + "/spacer.gif";

            RadLabel lbl = (RadLabel)e.Item.FindControl("lblLineid");
            RadLabel lb2 = (RadLabel)e.Item.FindControl("lblChkRemarks");
            ImageButton recimg = (ImageButton)e.Item.FindControl("imgReceiptRemarks");
            recimg.Attributes.Add("onclick", "javascript:showMoreInformation(ev, 'PurchaseFormItemReceiptRemarks.aspx?orderlineid=" + lbl.Text + "&View=Y'); return false;");
            //recimg.ImageUrl = lb2.Text.ToUpper().Equals("1") ? Session["images"] + "/te_view.png" : Session["images"] + "/spacer.gif";

            if (lb2.Text.ToUpper().Equals("1"))
            {
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses\"></i></span>";
                recimg.Controls.Add(html);
            }
            else
            {
                recimg.Visible = false;
            }

            if (!Filter.CurrentInspectionPurchaseVesselSendDateSelection.ToUpper().Equals("") && !(Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null))
            {
                db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Enabled = false;
                db = (LinkButton)e.Item.FindControl("cmdEdit");
                if (db != null) db.Enabled = false;
            }

            RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkSelect");
            if (cb != null) cb.Attributes.Add("onclick", "e.cancelBubble = true; if(this.checked == false) { this.checked = false; } else {this.checked = true; }");

            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            LinkButton ib1 = (LinkButton)e.Item.FindControl("btnShowBudgetEdit");
            if (ib1 != null)
            {
                if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SPARE"))
                    ib1.Attributes.Add("onclick", "return showPickList('spnPickListBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&iframignore=true&vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.ToString() + "&framename=ifMoreInfo', true); ");
                else if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SERVICE"))
                    ib1.Attributes.Add("onclick", "return showPickList('spnPickListBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?budgetgroup=107&hardtypecode=30&iframignore=true&vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.ToString() + "&framename=ifMoreInfo', true); ");
                else
                    ib1.Attributes.Add("onclick", "return showPickList('spnPickListBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?budgetgroup=105&hardtypecode=30&iframignore=true&vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.ToString() + "&framename=ifMoreInfo', true); ");
            }

            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            LinkButton ib2 = (LinkButton)e.Item.FindControl("btnShowOwnerBudgetEdit");
            if (ib2 != null)
            {
                if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SPARE"))
                    ib2.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + tb1.Text + "&framename=ifMoreInfo', true); ");
                else
                    ib2.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + tb1.Text + "&framename=ifMoreInfo', true); ");
            }
        }
    }
    private void SetRowSelection()
    {
        if (ViewState["orderlineid"] != null)
        {
            gvLineItem.SelectedIndexes.Clear();
            for (int i = 0; i < gvLineItem.Items.Count; i++)
            {
                if (gvLineItem.MasterTableView.Items[i].GetDataKeyValue("FLDORDERLINEID").ToString().Equals(ViewState["orderlineid"].ToString()))
                {
                    gvLineItem.MasterTableView.Items[i].Selected = true;
                    PhoenixInspectionAuditPurchaseForm.OrderLinePartNumber = ((LinkButton)gvLineItem.Items[i].FindControl("lnkStockItemName")).Text;
                    ViewState["DTKEY"] = ((RadLabel)gvLineItem.Items[i].FindControl("lbldtkey")).Text;
                }
            }
        }
    }
    protected void CheckBoxClicked(object sender, EventArgs e)
    {
        RadCheckBox cb = (RadCheckBox)sender;
    }
    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvLineItem.SelectedIndexes.Clear();
        gvLineItem.EditIndexes.Clear();
        gvLineItem.DataSource = null;
        gvLineItem.Rebind();
    }

    protected void gvLineItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        UpdateOrderQuantity(
            ((RadLabel)e.Item.FindControl("lblLineid")).Text,
            ((UserControlDecimal)e.Item.FindControl("txtOrderQtyEdit")).Text.Replace("_", "0"),
            ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text,
            ((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text
         );
        Rebind();
    }
}
