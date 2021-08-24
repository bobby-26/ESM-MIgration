using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionPurchaseFormGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        SessionUtil.PageFieldViewPermission(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        ViewState["DIRECTOBJ"] = Request.QueryString["DIRECTOBJ"];
        try
        {
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarmain.AddButton("New", "NEWFORM", ToolBarDirection.Right);
            MenuFormGeneral.AccessRights = this.ViewState;
            if (Filter.CurrentInspectionMenu == null)
            {                
                MenuFormGeneral.MenuList = toolbarmain.Show();                
            }
            else if (Filter.CurrentInspectionMenu == "directobs")
            {
                if (ViewState["DIRECTOBJ"] != null)
                {
                    DataSet ds = PhoenixInspectionObservation.EditDirectObservation(new Guid(ViewState["DIRECTOBJ"].ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        if (dr["FLDSTATUS"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 146, "CLD"))
                            MenuFormGeneral.MenuList = toolbarmain.Show();
                    }
                }
            }
            //MenuFormGeneral.SetTrigger(pnlFormGeneral);
            if (Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null)
            {
                lblImportedDate.Text = "Imported";
            }
            else
            {
                lblImportedDate.Text = "Exported";
                EnabelField();
            }
            
            txtVendor.Attributes.Add("style", "visibility:hidden");
            txtDeliveryAddressId.Attributes.Add("style", "visibility:hidden");
            txtBudgetId.Attributes.Add("style", "visibility:hidden");
            txtDeliveryLocationId.Attributes.Add("style", "visibility:hidden");
            txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");
            txtOwnerBudgetgroupId.Attributes.Add("style", "visibility:hidden");
            txtOwnerBudgetId.Attributes.Add("style", "visibility:hidden");
            txtOwnerBudgetName.Attributes.Add("style", "visibility:hidden");
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            txtForwarderId.Attributes.Add("style", "visibility:hidden");
            if (!IsPostBack)
            {                
                UCPeority.QuickTypeCode = ((int)PhoenixQuickTypeCode.ORDERPRIORITY).ToString();
                UCDeliveryTerms.QuickTypeCode = ((int)PhoenixQuickTypeCode.DELIVERYTERM).ToString();
                UCPaymentTerms.QuickTypeCode = ((int)PhoenixQuickTypeCode.PAYMENTTERM).ToString();
                ddlComponentClass.QuickTypeCode = ((int)PhoenixQuickTypeCode.COMPONENTCLASS).ToString();
                ddlStockClassType.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
                //Title1.Text = "General      (" + PhoenixInspectionPurchaseForm.FormNumber + ")     ";

                if ((Request.QueryString["orderid"] != null) && (Request.QueryString["orderid"] != ""))
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    BindData(Request.QueryString["orderid"].ToString());
                }
                if ((Request.QueryString["reffrom"] != null) && Request.QueryString["reffrom"] != "" && Request.QueryString["reffrom"].ToString() == "directobs")
                {
                    ViewState["REFFROM"] = Request.QueryString["reffrom"].ToString();                    
                }
                cmdShowMaker.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131,132&framename=ifMoreInfo',true);");
                cmdShowLocation.Attributes.Add("onclick", "return showPickList('spnDLocation', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDeliveryLocation.aspx',true);");
                cmdShowAddress.Attributes.Add("onclick", "return showPickList('spnDAddress', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=141&framename=ifMoreInfo',true);");
                btnPickForwarder.Attributes.Add("onclick", "return showPickList('spnPickListForwarder', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=135&framename=ifMoreInfo',true);");
            }
            FieldSetViewState();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData(string orderid)
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditOrderForm(new Guid(orderid));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtFormNumber.Text = dr["FLDFORMNO"].ToString();
            txtFromTitle.Text = dr["FLDTITLE"].ToString();
            txtDeliveryAddressId.Text = dr["FLDDELIVERYID"].ToString();
            txtDeliveryAddressCode.Text = dr["FLDDELIVERYADDRESSCODE"].ToString();
            txtDeliveryAddressName.Text = dr["FLDDELIVERYADDRESS"].ToString();
            txtDeliveryLocationId.Text = dr["FLDDELIVERYPLACEID"].ToString();
            txtDeliveryLocationName.Text = dr["FLDDELIVERYPLACE"].ToString();
            txtDeliveryLocationCode.Text = dr["FLDDELIVERYCODE"].ToString();
            txtCreatedDate.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"].ToString());
            txtBudgetId.Text = dr["FLDBUDGETCODEID"].ToString();
            txtBudgetCode.Text = dr["FLDBUDGETCODE"].ToString();
            txtBudgetName.Text = dr["FLDBUDGETNAME"].ToString();
            txtBugetDate.Text = General.GetDateTimeToString(dr["FLDBUDGETDATE"].ToString());
            txtEstimeted.Text = String.Format("{0:##,###,###.00}", dr["FLDESTIMATEDTOTAL"]);
            txtFinalTotal.Text = String.Format("{0:##,###,###.00}", dr["FLDACTUALTOTAL"]);
            txtlLastDeliveryDate.Text = General.GetDateTimeToString(dr["FLDLATESTDELIVERYDATE"].ToString());
            txtPartPaid.Text = String.Format("{0:##,###,###.00}", dr["FLDPARTPAYMENT"]);
            if (Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null)
                txtRecivedDate.Text = General.GetDateTimeToString(dr["FLDEXPORTIMPORTDATE"].ToString());
            else
                txtRecivedDate.Text = Filter.CurrentInspectionPurchaseVesselSendDateSelection != null ? General.GetDateTimeToString(Filter.CurrentInspectionPurchaseVesselSendDateSelection.ToString()) : "";                 
            txtVendor.Text = dr["FLDVENDORID"].ToString();
            txtVendorNumber.Text = dr["FLDVENDORCODE"].ToString();
            txtVenderDelveryDate.Text = General.GetDateTimeToString(dr["FLDVENDORDELIVERYDATE"].ToString());
            txtVenderEsmeted.Text = String.Format("{0:##,###,###.00}", dr["FLDVENDORADVISEDTOTAL"]);
            txtApproveDate.Text = General.GetDateTimeToString(dr["FLDPURCHASEAPPROVEDATE"].ToString());
            txtConfirmDate.Text = General.GetDateTimeToString(dr["FLDCONFIRMATIONDATE"].ToString());
            txtOrderDate.Text = General.GetDateTimeToString(dr["FLDORDEREDDATE"].ToString());
            txtStatus.Text = dr["FLDFORMSTATUSNAME"].ToString();

            txtForwarderCode.Text = dr["FLDFORWARDERCODE"].ToString();
            txtForwarderName.Text = dr["FLDFORWARDERNAME"].ToString();
            txtForwarderId.Text = dr["FLDFORWARDER"].ToString();

            if (Filter.CurrentInspectionPurchaseStockType.ToString().Equals("SPARE") || Filter.CurrentInspectionPurchaseStockType.ToString().Equals("SERVICE"))
            {
                ddlComponentClass.SelectedQuick = dr["FLDSTOCKCLASSID"].ToString();
                ddlComponentClass.Visible = true;
                ddlStockClassType.Visible = false;
            }
            else
            {
                ddlStockClassType.SelectedHard = dr["FLDSTOCKCLASSID"].ToString();
                ddlComponentClass.Visible = false;
                ddlStockClassType.Visible = true;
                lblComponentClass.Text = "Store Type";
            }
            txtType.Text = dr["FLDFORMTYPENAME"].ToString();
            //lblFormType.Text = dr["FLDFORMTYPE"].ToString();
            txtVenderName.Text = dr["FLDVENDORNAME"].ToString();
            if (dr["FLDCURRENCYID"] != null && dr["FLDCURRENCYID"].ToString().Trim() != "")
                ucCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            else
                ucCurrency.SelectedCurrency = PhoenixInspectionAuditPurchaseForm.DefaultCurrency;
            if (dr["FLDORDERPRIORITYID"] != null && dr["FLDORDERPRIORITYID"].ToString().Trim() != "")
                UCPeority.SelectedQuick = dr["FLDORDERPRIORITYID"].ToString();
            else
                UCPeority.SelectedQuick = PhoenixInspectionAuditPurchaseForm.DefaultPriority;
            if (dr["FLDDELIVERYTERMID"] != null && dr["FLDDELIVERYTERMID"].ToString().Trim() != "")
                UCDeliveryTerms.SelectedQuick = dr["FLDDELIVERYTERMID"].ToString();
            if (dr["FLDPAYMENTTERMID"] != null && dr["FLDPAYMENTTERMID"].ToString().Trim() != "")
                UCPaymentTerms.SelectedQuick = dr["FLDPAYMENTTERMID"].ToString();
            txtFormCreatedBy.Text = dr["FLDCREATEDUSERNAME"].ToString();
            txtPOorderedBy.Text = dr["FLDPURCHASEORDERSENDUSERNAME"].ToString();
            txtReqApprovedBy.Text = dr["FLDREQUISITIONAPPROVEUSERNAME"].ToString();
            txtPurchaseAppovedBy.Text = dr["FLDPURCHASEAPPROVEUSERNAME"].ToString();
            txtAccumulatedBudget.Text = dr["FLDACCUMETEDBUDGET"].ToString();
            txtAccumulatedTotal.Text = dr["FLDACCUMLATEDTOTAL"].ToString();
            txtOwnerBudgetId.Text = dr["FLDOWNERBUDGETID"].ToString();
            txtOwnerBudgetCode.Text = dr["FLDOWNERACCOUNT"].ToString();
            if (dr["FLDBILLTOCOMPANYID"] != null && dr["FLDBILLTOCOMPANYID"].ToString().Trim() != "")
                ucPayCompany.SelectedCompany = dr["FLDBILLTOCOMPANYID"].ToString();
            else
                ucPayCompany.SelectedCompany = PhoenixInspectionAuditPurchaseForm.DefaultBillToCompany;
            ddlStockType.SelectedValue = dr["FLDSTOCKTYPE"].ToString();
            //if (dr["FLDCHKITEMS"].ToString().Equals("1"))
            ddlStockType.Enabled = false;
            if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SPARE"))
            {
                btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&budgetdate=" + txtBugetDate.Text + "&framename=ifMoreInfo', true); ");
            }
            else if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SERVICE"))
            {
                btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?budgetgroup=107&hardtypecode=30&vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&budgetdate=" + txtBugetDate.Text + "&framename=ifMoreInfo', true); ");
            }
            else
            {
                btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudget.aspx?budgetgroup=105&hardtypecode=30&vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&budgetdate=" + txtBugetDate.Text + "&framename=ifMoreInfo', true); ");
            }
            cmdPicPartPaid.Attributes.Add("onclick", "return showPickList('spnPicPartPaid', 'codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionPurchaseOrderPartPaid.aspx?orderid=" + ViewState["orderid"].ToString() + "', true); ");
            if (dr["FLDBUDGETCODEID"] != null && dr["FLDBUDGETCODEID"].ToString().Trim() != "")
                btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudget.aspx?vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&budgetdate=" + txtBugetDate.Text + "&budgetid=" + dr["FLDBUDGETCODEID"].ToString() + "&framename=ifMoreInfo', true); ");//.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&budgetdate=" + txtBugetDate.Text + "', true); ");
            else
                btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudget.aspx?vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&budgetdate=" + txtBugetDate.Text + "&framename=ifMoreInfo', true); ");//.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + Filter.CurrentInspectionPurchaseVesselSelection + "&budgetdate=" + txtBugetDate.Text + "', true); ");

            cmdvendorAddress.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionPurchaseFormAddress.aspx?addresscode=" + txtVendor.Text + "');return false;");
            cmdDeliveryAddress.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionPurchaseFormAddress.aspx?addresscode=" + txtDeliveryAddressId.Text + "');return false;");
            cmdForwarderAddress.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionPurchaseFormAddress.aspx?addresscode=" + txtForwarderId.Text + "');return false;");
        }
    }

    protected void MenuFormGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("NEWFORM"))
            {
                if (ViewState["REFFROM"] != null && ViewState["REFFROM"].ToString() == "directobs")
                    Response.Redirect("../Inspection/InspectionPurchaseFormType.aspx?DIRECTOBJ=" + ViewState["DIRECTOBJ"] + "&REFFROM=" + ViewState["REFFROM"], false);
                else
                    Response.Redirect("../Inspection/InspectionPurchaseFormType.aspx?DIRECTOBJ=" + ViewState["DIRECTOBJ"], false);
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidForm())
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateOrderForm();
                InsertOrderFormHistory();
                ucStatus.Text = "Requisition Updated";
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                if (!IsValidForm())
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateOrderForm();
                //ApprovedOrderForm();
                // ucStatus.Text = "Requisition Updated";
            }

            BindData(ViewState["orderid"].ToString());
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void InsertOrderFormHistory()
    {
        PhoenixInspectionAuditPurchaseForm.InsertOrderFormHistory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentInspectionPurchaseVesselSelection);
    }

    //private void ApprovedOrderForm()
    //{

    //    if (lblFormType.Text == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE), "RQ"))
    //    {
    //        PhoenixPurchaseOrderForm.ApproveOrderForm(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString())
    //            , General.GetNullableDateTime(DateTime.Now.ToString()), "REQUISITION");
    //        ucConfirm.ErrorMessage = "Requisition is approved";
    //        ucConfirm.Visible = true;
    //    }
    //}

    private void UpdateOrderForm()
    {

        PhoenixInspectionAuditPurchaseForm.UpdateOrderForm(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString())
            , General.GetNullableInteger(UCDeliveryTerms.SelectedQuick), General.GetNullableInteger(UCPaymentTerms.SelectedQuick)
            , General.GetNullableInteger(UCPeority.SelectedQuick), General.GetNullableInteger(ucCurrency.SelectedCurrency)
            , General.GetNullableInteger(txtVendor.Text), General.GetNullableInteger(txtBudgetId.Text)
            , General.GetNullableInteger(txtDeliveryLocationId.Text), txtFromTitle.Text
            , General.GetNullableDateTime(txtApproveDate.Text), General.GetNullableDateTime(txtOrderDate.Text)
            , General.GetNullableDateTime(txtConfirmDate.Text), General.GetNullableDateTime(txtRecivedDate.Text)
            , General.GetNullableDateTime(txtBugetDate.Text), General.GetNullableInteger(txtDeliveryAddressId.Text)
            , General.GetNullableDecimal(txtEstimeted.Text), General.GetNullableDecimal(txtPartPaid.Text)
            , General.GetNullableDecimal(txtFinalTotal.Text), General.GetNullableDecimal(txtVenderEsmeted.Text)
            , General.GetNullableDateTime(txtlLastDeliveryDate.Text), General.GetNullableInteger(ucPayCompany.SelectedCompany)
            , General.GetNullableDateTime(txtVenderDelveryDate.Text), ddlStockType.SelectedItem.Value
            , General.GetNullableInteger((ddlStockType.SelectedItem.Value.Equals("STORE")) ? ddlStockClassType.SelectedHard : ddlComponentClass.SelectedQuick), General.GetNullableGuid(txtOwnerBudgetId.Text)
            , General.GetNullableInteger(txtForwarderId.Text));


    }
    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (txtFormNumber.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Form Number is required. Please Select ";
        if (txtFromTitle.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Form Title is required.";
        if (General.GetNullableDateTime(txtConfirmDate.Text) != null)
        {
            if (General.GetNullableDateTime(txtOrderDate.Text) == null || (General.GetNullableDateTime(txtConfirmDate.Text) < General.GetNullableDateTime(txtOrderDate.Text)))
                ucError.ErrorMessage = "Confirmation date cannot be less than order date.";
        }
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    private void EnabelField()
    {
        btnShowBudget.Visible = false;
        ucPayCompany.Enabled = false;
    }
    private void FieldSetViewState()
    {
        txtVenderEsmeted.Visible = IsVisible("txtVenderEsmeted");
        lblVenderEsmeted.Visible = IsVisible("lblVenderEsmeted");
        txtFinalTotal.Visible = IsVisible("txtFinalTotal");
        lblFinalTotal.Visible = IsVisible("lblFinalTotal");
        lblPartPaid.Visible = IsVisible("lblPartPaid");
        txtPartPaid.Visible = IsVisible("txtPartPaid");
        //cmdPicPartPaid.Visible = IsVisible("cmdPicPartPaid");
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
}
