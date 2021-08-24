using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;

public partial class PurchaseFormItem : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        SessionUtil.PageAccessRights(this.ViewState);
        SessionUtil.PageFieldViewPermission(this.ViewState); 
        try
        {
            MenuLineItemGeneral.Title = "Line Items ( " + PhoenixPurchaseOrderForm.FormNumber + " )";
            if (Filter.CurrentPurchaseStockType != null && Filter.CurrentPurchaseStockType == "STORE")
                txtItemNumber.MaskText = "##.##.##";
            else if (Filter.CurrentPurchaseStockType != null && Filter.CurrentPurchaseStockType == "SPARE")
                txtItemNumber.MaskText = "###.##.##.###";
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            toolbarmain.AddButton("New", "NEW",ToolBarDirection.Right);
            MenuLineItemGeneral.AccessRights = this.ViewState; 
            MenuLineItemGeneral.MenuList = toolbarmain.Show();           
            //MenuLineItemGeneral.SetTrigger(pnlLineItemEntry);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            txtComponentID.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                
                if (!Filter.CurrentPurchaseVesselSendDateSelection.ToUpper().Equals("") && !(Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null))
                {
                    MenuLineItemGeneral.Visible = false;
                    lblPrice.Visible = false;
                    txtPrice.Visible = false;  
                }

                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
                {                   
                    cmdShowComponent.OnClientClick = "return showPickList('spnPickComponent', 'codehelp1', '', 'Common/CommonPickListComponentPurchase.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "', true);";
                    // cmdShowItem.OnClientClick = "return showPickList('spnPickItem', 'codehelp1', '', '../Common/CommonPickListSpareItemByComponent.aspx?mode=custom&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&txtnumber='+ document.getElementById('" + txtItemNumber.UniqueID + "').value, true);";
                    cmdShowItem.Attributes.Add("onclick", "return showPickList('spnPickItem', 'codehelp1', '', '../Common/CommonPickListSpareItemByComponent.aspx?mode=custom&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&txtnumber=" + txtItemNumber.TextWithLiterals + "&txtname=" + txtPartName.Text + "&txtMakerRef=" + txtMakerReference.Text+ "&COMPONENTID=" + txtComponentID.Text+ "', true);");

                    txtRecivedSpareQty.Visible = true;
                    txtRecivedStoreQty.Visible = false;
                    txtItemNumber.CssClass = "input_mandatory";
                    txtMakerReference.Enabled = false;
                    txtPartName.Enabled = false;
                    txtPartName.CssClass = "readonlytextbox";

                }
                else if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
                {
                    cmdShowComponent.OnClientClick = "return showPickList('spnPickComponent', 'codehelp1', '', '../Common/CommonPickListComponent.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "', true);";
                    cmdShowItem.OnClientClick = "return showPickList('spnPickItem', 'codehelp1', '', '../Common/CommonPickListWorkOrder.aspx?mode=custom', true);";
                    txtServiceNumber.Visible = true;
                    txtItemNumber.Visible = false;

                    txtRecivedSpareQty.Visible = true;
                    txtRecivedStoreQty.Visible = false;
                }
                else
                {
                    string stockclass="407";
                     if (Filter.CurrentPurchaseStockClass != "")
                      stockclass=  Filter.CurrentPurchaseStockClass;

                    //MaskNumber.Mask = "99.99.CC";
                    cmdShowComponent.OnClientClick = "return showPickList('spnPickComponent', 'codehelp1', '', '../Common/CommonPickListStoreType.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "', true);";
                    cmdShowItem.OnClientClick = "return showPickList('spnPickItem', 'codehelp1', '', '../Common/CommonPickListStoreItem.aspx?storetype=" + stockclass + "&mode=custom&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&makerref='+ document.getElementById('" + txtMakerReference.UniqueID + "').value+'&txtnumber='+ document.getElementById('" + txtItemNumber.UniqueID + "').value+'&txtname='+ document.getElementById('" + txtPartName.UniqueID + "').value, true);";
                    lblmakerRef.Text = "Product Code";
                    trComponent.Visible = false;

                    txtRecivedSpareQty.Visible = false;
                    txtRecivedStoreQty.Visible = true;
                }
                ucReciptstatus.QuickTypeCode = ((int)PhoenixQuickTypeCode.RECEIPTSTATUS).ToString();
                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    if (Request.QueryString["orderlineid"] != null)
                    {
                        ViewState["orderlineid"] = Request.QueryString["orderlineid"].ToString();
                        BindDataEdit(Request.QueryString["orderid"].ToString(), ViewState["orderlineid"].ToString());
                        ViewState["SaveStatus"] = "Edit";
                    }
                    else
                    {
                        ViewState["SaveStatus"] = "New";
                        ViewState["orderlineid"] = null;
                        imgJobList.Visible = false;
                    }
                }
                FieldSetViewState();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataEdit(string orderid, string orderlineid)
    {
        DataSet lineitemdataset = new DataSet();
        lineitemdataset = PhoenixPurchaseOrderLine.EditOrderLine(new Guid(orderlineid));
        if (lineitemdataset.Tables[0].Rows.Count > 0)
        {
            DataRow dr = lineitemdataset.Tables[0].Rows[0];

            ucUnit.ItemType = Filter.CurrentPurchaseStockType.ToString();
            ucUnit.ItemId = dr["FLDPARTID"].ToString();
            ucUnit.VesslId = Filter.CurrentPurchaseVesselSelection;

            txtItemNumber.Text = dr["FLDPARTNUMBER"].ToString();
            txtServiceNumber.Text = dr["FLDPARTNUMBER"].ToString();
            txtPartName.Text = dr["FLDNAME"].ToString();
            //Title1.Text = "General ( " + dr["FLDPARTNUMBER"].ToString() + " " + dr["FLDNAME"].ToString() + " )";
            txtComponentID.Text = dr["FLDFORCOMPID"].ToString();
            txtComponent.Text = dr["FLDCOMPONENTNUMBER"].ToString();
            txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
            txtExtraNumber.Text = dr["FLDEXTRANO"].ToString();
            txtMakerReference.Text = dr["FLDMAKERREF"].ToString();
            txtStatus.Text = dr["FLDSTATUSNAME"].ToString();
            txtOrderQty.Text = String.Format("{0:##,##0}", dr["FLDORDEREDQUANTITY"]);

            txtBudgetCode.Text = dr["FLDBUDGETCODE"].ToString();
            txtBudgetName.Text = dr["FLDBUDGETNAME"].ToString();
            txtBudgetId.Text = dr["FLDBUDGETID"].ToString();

            txtOwnerBudgetCode.Text = dr["FLDOWNERACCOUNT"].ToString();
            txtOwnerBudgetId.Text = dr["FLDOWNERBUDGETID"].ToString();
            ViewState["PRINCIPALID"] = dr["FLDPRINCIPALID"].ToString();

            if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
            {
                btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', 'Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + dr["FLDORDERDATE"].ToString() + "', true); ");
            }
            else if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
            {
                btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', 'Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=107&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + dr["FLDORDERDATE"].ToString() + "', true); ");
            }
            else
            {
                btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '','Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=105&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + dr["FLDORDERDATE"].ToString() + "', true); ");
            }

            if (ViewState["PRINCIPALID"] != null)
            {
                btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', 'Common/CommonPickListOwnerBudget.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetid=" + txtBudgetId.Text + "&Ownerid=" + ViewState["PRINCIPALID"] + "', true); ");
                Filter.CurrentSelectedESMBudgetCode = null;
            }

            if (Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null)
            {
                txtPrice.Text = String.Format("{0:##,###,##0.00}", dr["FLDPRICE"]);
            }
            txtLastSuppliedDate.Text = dr["FLDLASTSUPPLIEDDATE"].ToString();
            if (Filter.CurrentPurchaseStockType.ToUpper().Equals("STORE"))
                txtRecivedStoreQty.Text = String.Format("{0:##,##0.000}", dr["FLDRECEIVEDQUANTITY"]);
            else
                txtRecivedSpareQty.Text = String.Format("{0:##,##0}", dr["FLDRECEIVEDQUANTITY"]);

            txtRequestedQty.Text = String.Format("{0:##,##0}", dr["FLDREQUESTEDQUANTITY"]);
            txtCanceledQty.Text = String.Format("{0:##,##0}", dr["FLDCANCELLEDQUANTITY"]);

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                txtRequestedQty.CssClass = "readonlytextbox";
                txtRequestedQty.ReadOnly = "true";
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                txtOrderQty.CssClass = "readonlytextbox";
                txtOrderQty.ReadOnly = "true";
            }

            if (dr["FLDINCLUDEONFORM"].ToString() == "1")
                chkIncludeOnForm.Checked = true;
            if (dr["FLDBUDGETED"].ToString() == "1")
                chkBudgetedPurchase.Checked = true;
            if (dr["FLDCURRENCYID"] != null && dr["FLDCURRENCYID"].ToString().Trim() != "")
                ucCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            else
                ucCurrency.SelectedCurrency = PhoenixPurchaseOrderForm.DefaultCurrency;

            if (dr["FLDUNITID"] != null && dr["FLDUNITID"].ToString().Trim() != "")
                ucUnit.SelectedUnit = dr["FLDUNITID"].ToString();

            ucReciptstatus.SelectedQuick = dr["FLDRECEIPTSTATUSID"].ToString();
            txtDrawingNo.Text = dr["FLDDRAWINGNUMBER"].ToString();
            txtPosition.Text = dr["FLDPOSITION"].ToString();

            if (!dr["FLDPARTID"].ToString().Equals("99999999-9999-9999-9999-999999999999"))
            {
                FreezeControls();
            }

            if (General.GetNullableGuid(dr["FLDFORCOMPID"].ToString()) != null)
            {
                imgJobList.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Purchase/PurchaseComponentJobList.aspx?COMPONENTID=" + dr["FLDFORCOMPID"].ToString() + "'); return false;");
            }
        }
        else
        {
            imgJobList.Visible = false;

            DataSet dsp= PhoenixPurchaseOrderLine.EditOrderLinePrincipal(new Guid(orderlineid), Filter.CurrentPurchaseVesselSelection);
            if(dsp.Tables[0].Rows.Count>0)
            {
                DataRow dr = dsp.Tables[0].Rows[0];
                ViewState["PRINCIPALID"]= dr["FLDPRINCIPALID"].ToString();

                if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
                {
                    btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', 'Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + dr["FLDORDERDATE"].ToString() + "', true); ");
                   
                }
                else if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
                {
                    btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', 'Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=107&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + dr["FLDORDERDATE"].ToString() + "', true); ");
                   
                }
                else
                {
                    btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '','Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=105&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + dr["FLDORDERDATE"].ToString() + "', true); ");
                }
            }
        }
    }
    private void BindData(string stockitemcode)
    {
        DataSet stockitemdataset = new DataSet();
        stockitemdataset = PhoenixPurchaseOrderLine.getStockItem(new Guid(stockitemcode), new Guid(ViewState["orderid"].ToString()));
        if (stockitemdataset.Tables[0].Rows.Count > 0)
        {
            DataRow dr = stockitemdataset.Tables[0].Rows[0];
            ViewState["StockId"] = dr["FLDSTOCKITEMID"].ToString();
            txtItemNumber.Text = dr["FLDNUMBER"].ToString();
            txtPartName.Text = dr["FLDNAME"].ToString();           
            txtExtraNumber.Text = dr["FLDEXTRANUMBER"].ToString();
            txtRequestedQty.Text = String.Format("{0:####0}",dr["FLDWANTED"]);
            txtMakerReference.Text = dr["FLDMAKERREFERENCE"].ToString();
            ucUnit.SelectedUnit = dr["FLDUNITID"].ToString();
            txtComponentID.Text = dr["FLDCOMPONENTID"].ToString();
            txtComponent.Text = dr["FLDCOMPONENTNUMBER"].ToString();
            txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
            FreezeControls();
        }
    }   
    protected void MenuLineItemGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidForm())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["orderlineid"] == null)
                {
                    InsertOrderLineItem();

                    string Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += " closeTelerikWindow('','Filter','true');";
                    Script += "</script>" + "\n";

                    RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
                }
                else
                {
                    UpdateOrderLineItem();

                    string Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += " closeTelerikWindow('','Filter','true');";
                    Script += "</script>" + "\n";
                    RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
                }

            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                ClearTextBox();
                ViewState["SaveStatus"] = "New";
                EnableControls();
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
        txtComponentID.Text ="";
        txtComponent.Text ="";
        txtComponentName.Text ="";
        txtExtraNumber.Text ="";
        txtMakerReference.Text ="";
        txtStatus.Text = "";
        txtOrderQty.Text = "";
        txtPrice.Text = "";
        txtRecivedSpareQty.Text = "";
        txtRecivedStoreQty.Text = "";
        txtRequestedQty.Text = "";      
        txtCanceledQty.Text = "";         
        txtDrawingNo.Text = "";
        txtPosition.Text = "";
        txtServiceNumber.Text = ""; 
        chkIncludeOnForm.Checked = false;
        chkBudgetedPurchase.Checked = false;
        ViewState["orderlineid"] = null;
		ucUnit.SelectedUnit = "";
        txtBudgetCode.Text = "";
        txtBudgetName.Text = "";
        txtBudgetId.Text = "";
        txtOwnerBudgetCode.Text = "";
        txtOwnerBudgetId.Text = "";
        ucUnit.PurchaseUnitList = PhoenixRegistersUnit.ListPurchaseUnit(null,null
                                                                           , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    }
    protected void cmdClearBudget_Click(object sender, EventArgs e)
    {
        txtBudgetCode.Text = "";
        txtBudgetName.Text = "";
        txtBudgetId.Text = "";
        txtBudgetgroupId.Text = "";
    }
    protected void cmdClearOwnerBudget_Click(object sender, EventArgs e)
    {
        txtOwnerBudgetCode.Text = "";
        txtOwnerBudgetName.Text = "";
        txtOwnerBudgetId.Text = "";
        txtOwnerBudgetgroupId.Text = "";
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (ViewState["PRINCIPALID"] != null)
        {
            btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', 'Common/CommonPickListOwnerBudget.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetid=" + txtBudgetId.Text + "&Ownerid=" + ViewState["PRINCIPALID"] + "', true); ");
            Filter.CurrentSelectedESMBudgetCode = null;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        NameValueCollection nvc = Filter.CurrentPickListSelection;
        if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
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
            ucUnit.ItemType = Filter.CurrentPurchaseStockType.ToString();
            ucUnit.VesslId = Filter.CurrentPurchaseVesselSelection ;
            ViewState["SaveStatus"] = "New";
            ViewState["orderlineid"] = null;
            imgJobList.Visible = false;
            FreezeControls();
        }
        else
        {
            string stockitemcode = nvc.Get("lblStockItemId").ToString();
            ViewState["SaveStatus"] = "New";
            ViewState["orderlineid"] = null;
            ucUnit.PurchaseUnitList = PhoenixRegistersUnit.ListPurchaseUnit(stockitemcode, Filter.CurrentPurchaseStockType.ToString()
                                                                            ,Filter.CurrentPurchaseVesselSelection );
            BindData(stockitemcode);
        }
       
    }
    private void InsertOrderLineItem()
    {
        int chkbudget = chkBudgetedPurchase.Checked == true ? 1 : 0;
        int chkforms = chkIncludeOnForm.Checked == true ? 1 : 0;

        string stockitemguid = ViewState["StockId"] != null ? ViewState["StockId"].ToString() : "";

        PhoenixPurchaseOrderLine.InsertOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtComponentID.Text,
            General.GetNullableInteger(ucUnit.SelectedUnit), General.GetNullableInteger(ucCurrency.SelectedCurrency),
            stockitemguid, new Guid(ViewState["orderid"].ToString()),
            Filter.CurrentPurchaseStockType != null && Filter.CurrentPurchaseStockType == "SERVICE" ? txtServiceNumber.Text : txtItemNumber.TextWithLiterals, txtPartName.Text,
            txtMakerReference.Text, General.GetNullableDecimal(txtPrice.Text),
            General.GetNullableDecimal(txtRequestedQty.Text), General.GetNullableDecimal(txtRecivedSpareQty.Visible == true ? txtRecivedSpareQty.Text : txtRecivedStoreQty.Text),
            General.GetNullableDecimal(txtCanceledQty.Text), PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMSTATUS), "ACT"),
            chkforms, chkbudget, General.GetNullableInteger(ucReciptstatus.SelectedQuick), General.GetNullableDecimal(txtOrderQty.Text), General.GetNullableString(txtDrawingNo.Text),
            General.GetNullableInteger(txtBudgetId.Text), General.GetNullableGuid(txtOwnerBudgetId.Text));

    }
    private void UpdateOrderLineItem()
    {
        int chkbudget= chkBudgetedPurchase.Checked==true?1:0;
        int chkforms = chkIncludeOnForm.Checked == true ? 1 : 0;  

        PhoenixPurchaseOrderLine.UpdateOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderlineid"].ToString()), 
            txtComponentID.Text,General.GetNullableInteger(ucUnit.SelectedUnit),
            General.GetNullableInteger(ucCurrency.SelectedCurrency),General.GetNullableDecimal(txtPrice.Text),
            General.GetNullableDecimal(txtRequestedQty.Text), General.GetNullableDecimal(txtOrderQty.Text)
            , General.GetNullableDecimal(txtRecivedSpareQty.Visible == true ? txtRecivedSpareQty.Text : txtRecivedStoreQty.Text), General.GetNullableInteger(txtCanceledQty.Text),
            PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMSTATUS), "ACT"),
            chkforms, chkbudget,
           General.GetNullableInteger(ucReciptstatus.SelectedQuick), txtMakerReference.Text, txtDrawingNo.Text, txtItemNumber.Text, txtPartName.Text,
           General.GetNullableInteger(txtBudgetId.Text), General.GetNullableGuid(txtOwnerBudgetId.Text));
    }
    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (txtItemNumber.Text.Trim().Equals(""))
        //    ucError.ErrorMessage = "Part number is required. Please Select ";

        if (txtPartName.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Part Name is required.";
        if(ViewState["orderid"]==null)
            ucError.ErrorMessage = "Order number is required.";
        if (ucUnit.SelectedUnit.ToUpper().Trim() == "DUMMY" || ucUnit.SelectedUnit.Trim() == "")
            ucError.ErrorMessage = "Unit is required.";
        string a = ucUnit.SelectedUnit;
        if (txtRequestedQty.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Requested Quantity is required.";
       

        if (Filter.CurrentPurchaseStockType.Equals("SPARE"))
        {
            if (txtComponentID.Text.Trim().Equals(""))
                ucError.ErrorMessage = "Component is required.";

            if (ViewState["orderlineid"] == null)
            {
                if (txtItemNumber.Text.Trim().Equals(""))
                    ucError.ErrorMessage = "Number is required.";
            }
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
    public void FreezeControls()
    {
        if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
        {
            txtServiceNumber.Enabled = false;
            txtMakerReference.Enabled = false;
            txtPartName.Enabled = false;
            txtDrawingNo.Enabled = false;
            txtPosition.Enabled = false;
        }
        else if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
        {
            //txtItemNumber.Enabled = false;
            txtMakerReference.Enabled = false;
            txtPartName.Enabled = false;
            txtDrawingNo.Enabled = false;
            txtPosition.Enabled = false;
        }
        else
        {
            //txtItemNumber.Enabled = false;
            txtMakerReference.Enabled = false;
            txtPartName.Enabled = false;
        }
    }
    public void EnableControls()
    {
        //txtServiceNumber.Enabled = true;
        txtServiceNumber.ReadOnly = false;
        //txtItemNumber.Enabled = true;
        txtItemNumber.ReadOnly = false;
        txtMakerReference.Enabled = true;
        txtPartName.Enabled = true;
        txtDrawingNo.Enabled = true;
        txtPosition.Enabled = true;
        txtRequestedQty.CssClass = "input_mandatory";
        txtRequestedQty.ReadOnly = "false";
    }
}
