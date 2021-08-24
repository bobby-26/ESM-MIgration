using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PurchaseFormGeneral : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        SessionUtil.PageFieldViewPermission(this.ViewState);   
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        txtComponentID.Attributes.Add("style","display:none");
        try
        {
            
            if (!IsPostBack)
            {
                if (Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null)
                {
                    lblImportedDate.Text = "Imported";
                }
                else
                {
                    lblImportedDate.Text = "Exported";
                    EnabelField();
                }
                
                txtVendorNumber.Attributes.Add("readonly", "readonly");
                txtVenderName.Attributes.Add("readonly", "readonly");
                txtDeliveryLocationCode.Attributes.Add("readonly", "readonly");
                txtDeliveryLocationName.Attributes.Add("readonly", "readonly");
                txtDeliveryAddressCode.Attributes.Add("readonly", "readonly");
                txtDeliveryAddressName.Attributes.Add("readonly", "readonly");
                txtAddressName.Attributes.Add("readonly", "readonly");
                txtBudgetCode.Attributes.Add("readonly", "readonly");
                txtBudgetName.Attributes.Add("readonly", "readonly");
                txtForwarderCode.Attributes.Add("readonly", "readonly");
                txtForwarderName.Attributes.Add("readonly", "readonly");
                txtOwnerBudgetCode.Attributes.Add("readonly", "readonly");
                txtOwnerBudgetName.Attributes.Add("readonly", "readonly");

                txtVendor.Attributes.Add("style", "display:none");
                txtDeliveryAddressId.Attributes.Add("style", "display:none");
                txtBudgetId.Attributes.Add("style", "display:none");
                txtDeliveryLocationId.Attributes.Add("style", "display:none");
                txtBudgetgroupId.Attributes.Add("style", "display:none");
                txtOwnerBudgetgroupId.Attributes.Add("style", "display:none");
                txtOwnerBudgetId.Attributes.Add("style", "display:none");
                txtOwnerBudgetName.Attributes.Add("style", "display:none");
                //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                cmdHiddenPick.Attributes.Add("style", "display:none");
                txtForwarderId.Attributes.Add("style", "display:none");
                txtAddressPurposeId.Attributes.Add("style", "display:none");
            
                UCPeority.QuickTypeCode =((int)PhoenixQuickTypeCode.ORDERPRIORITY).ToString();
                UCDeliveryTerms.QuickTypeCode =((int) PhoenixQuickTypeCode.DELIVERYTERM).ToString();
                UCPaymentTerms.QuickTypeCode = ((int)PhoenixQuickTypeCode.PAYMENTTERM).ToString();
                //ddlComponentClass.QuickTypeCode = ((int)PhoenixQuickTypeCode.COMPONENTCLASS).ToString();
                ddlStockClassType.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();

                //if (General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString()) != null)
                //    ucPayCompany.UserId = PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString();

                if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() == "OWNER")
                    ucPayCompany.Enabled = false;

                btnShowBudget.Visible = SessionUtil.CanAccess(this.ViewState, "BUDGETCODE");
                imgClear1.Visible = SessionUtil.CanAccess(this.ViewState, "IMGBUDGETCODE");
                btnShowOwnerBudget.Visible = SessionUtil.CanAccess(this.ViewState, "OWNERBUDGETCODE");
                imgClearOwnerBudget.Visible = SessionUtil.CanAccess(this.ViewState, "IMGOWNERBUDGETCODE");

                if (!string.IsNullOrEmpty(Request.QueryString["vesselid"]))
                {
                    Filter.CurrentPurchaseVesselSelection = int.Parse(Request.QueryString["vesselid"].ToString());
                }

                if ((Request.QueryString["orderid"] != null) && (Request.QueryString["orderid"] != ""))
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    BindVesselAccount();
                    BindData(Request.QueryString["orderid"].ToString());
                }
                MenuFormGeneral.Title = "General      (" + PhoenixPurchaseOrderForm.FormNumber + ")     ";
                if (Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null)
                {
                    string dtkey = ViewState["DTKEY"] != null ? ViewState["DTKEY"].ToString() : "";

                    if (ViewState["DTKEY"] != null)
                    {
                        if (Session["launchedfrom"] == null || Session["launchedfrom"].ToString().ToUpper() == "PURCHASE")
                            toolbarmain.AddLinkButton("javascript:openNewWindow('codehelp1','','Common/CommonPhoenixAuditChanges.aspx?dtkey=" + dtkey + "&shortcode=PUR-FORM" + "')", "Form History", "HISTORY",ToolBarDirection.Right);
                    }
                }
                int i = 10;
                int i2 = 10;
                DataSet ds = PhoenixPurchaseConfiguration.Searchpurchaseconfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    "POAME", null, null, 1, General.ShowRecords(null), ref i, ref i2);


                if (Session["launchedfrom"] != null && Session["launchedfrom"].ToString().ToUpper() == "PURCHASE")
                {
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["FLDVALUE"].ToString().ToUpper().Equals("ENABLE") && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                        toolbarmain.AddButton("Amend PO", "AMENDPO", ToolBarDirection.Right);

                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                        toolbarmain.AddButton("Send to Office", "SEND", ToolBarDirection.Right);
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                        toolbarmain.AddButton("Convert", "CONVERT", ToolBarDirection.Right);
                    toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    toolbarmain.AddButton("New", "NEWFORM", ToolBarDirection.Right);



                }
                else if (Session["launchedfrom"] != null && Session["launchedfrom"].ToString().ToUpper() == "ACCOUNTS")
                {
                    //toolbarmain.AddButton("New", "NEWFORM");
                    toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    //toolbarmain.AddButton("Convert", "CONVERT");
                }
                else if (Session["launchedfrom"] != null && Session["launchedfrom"].ToString().ToUpper() == "ANALYSIS")
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                        toolbarmain.AddButton("Convert", "CONVERT", ToolBarDirection.Right);
                    toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    toolbarmain.AddButton("New", "NEWFORM", ToolBarDirection.Right);


                }
                else if (Session["launchedfrom"] != null && Session["launchedfrom"].ToString().ToUpper() == "DMR")
                {
                    toolbarmain.AddButton("Form", "FORM");
                }
                else if (Session["launchedfrom"] == null)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                        toolbarmain.AddButton("Send to Office", "SEND", ToolBarDirection.Right);
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                        toolbarmain.AddButton("Convert", "CONVERT", ToolBarDirection.Right);
                    toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    toolbarmain.AddButton("New", "NEWFORM", ToolBarDirection.Right);



                }
                MenuFormGeneral.AccessRights = this.ViewState;
                MenuFormGeneral.MenuList = toolbarmain.Show();
                ucProjectcode.bind(null, null);
                FieldSetViewState();
                
            }         

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData(string  orderid)
    {

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseOrderForm.EditOrderForm(new Guid(orderid), Filter.CurrentPurchaseVesselSelection);
        if (ds.Tables[0].Rows.Count > 0)
        {

            DataRow dr = ds.Tables[0].Rows[0];
            txtFormNumber.Text = dr["FLDFORMNO"].ToString();

            Filter.CurrentPurchaseVesselSelection = int.Parse(dr["FLDVESSELID"].ToString());
            PhoenixPurchaseOrderForm.FormNumber = dr["FLDFORMNO"].ToString();
            Filter.CurrentPurchaseStockType = dr["FLDSTOCKTYPE"].ToString();
            Filter.CurrentPurchaseStockClass = dr["FLDSTOCKCLASSID"].ToString();
            Filter.CurrentPurchaseOrderIdSelection = dr["FLDORDERID"].ToString();
            Filter.CurrentPurchaseOrderComponentIDSelection = dr["FLDCOMPONENTID"].ToString();

            ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
            ViewState["FORMTYPE"] = dr["FLDFORMTYPE"].ToString();
            ViewState["FORMSTATUS"] = dr["FLDFORMSTATUS"].ToString();
            txtFromTitle.Text = dr["FLDTITLE"].ToString();
            txtDeliveryAddressId.Text = dr["FLDDELIVERYID"].ToString();
            txtDeliveryAddressCode.Text = dr["FLDDELIVERYADDRESSCODE"].ToString();
            txtDeliveryAddressName.Text = dr["FLDDELIVERYADDRESS"].ToString();
            txtDeliveryLocationId.Text = dr["FLDDELIVERYPLACEID"].ToString();
            txtDeliveryLocationName.Text = dr["FLDDELIVERYPLACE"].ToString();
            txtDeliveryLocationCode.Text = dr["FLDDELIVERYCODE"].ToString(); 
            txtCreatedDate.Text = dr["FLDCREATEDDATE"].ToString();
            txtBudgetId.Text = dr["FLDBUDGETCODEID"].ToString();
            txtBudgetCode.Text = dr["FLDBUDGETCODE"].ToString();
            txtBudgetName.Text = dr["FLDBUDGETNAME"].ToString();
            txtBugetDate.Text = dr["FLDBUDGETDATE"].ToString();
            txtEstimeted.Text = String.Format("{0:##,###,###.00}", dr["FLDESTIMATEDTOTAL"]);
            txtFinalTotal.Text = String.Format("{0:##,###,###.00}", dr["FLDACTUALTOTAL"]);
            //txtlLastDeliveryDate.Text =  General.GetDateTimeToString(dr["FLDLATESTDELIVERYDATE"].ToString());
            txtlLastDeliveryDate.Text = dr["FLDLATESTDELIVERYDATE"].ToString();
            txtPartPaid.Text = String.Format("{0:##,###,###.00}", dr["FLDPARTPAYMENT"]);
            if (Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null)
                txtRecivedDate.Text = dr["FLDEXPORTIMPORTDATE"].ToString();
            else
                txtRecivedDate.Text = Filter.CurrentPurchaseVesselSendDateSelection.ToString();
            txtVendor.Text = dr["FLDVENDORID"].ToString();
            txtVendorNumber.Text = dr["FLDVENDORCODE"].ToString();
            txtVenderDelveryDate.Text = dr["FLDVENDORDELIVERYDATE"].ToString();
            txtVenderEsmeted.Text = String.Format("{0:##,###,###.00}", dr["FLDVENDORADVISEDTOTAL"]);
            txtApproveDate.Text = dr["FLDPURCHASEAPPROVEDATE"].ToString();
            txtConfirmDate.Text = dr["FLDCONFIRMATIONDATE"].ToString();
            txtOrderDate.Text = dr["FLDORDEREDDATE"].ToString();
            txtStatus.Text = dr["FLDFORMSTATUSNAME"].ToString();
            ucProjectcode.SelectedProjectCode = dr["FLDPROJECTCODE"].ToString();

            if (string.IsNullOrEmpty(dr["FLDPURCHASEAPPROVEDATE"].ToString()))
                ucProjectcode.Enabled = false;

            txtForwarderName.Text = dr["FLDFORWARDERNAME"].ToString();
            txtForwarderId.Text = dr["FLDFORWARDER"].ToString();
            txtInvoiceNo.Text = dr["FLDINVOICENUMBER"].ToString();
            txtInvoiceStatus.Text = dr["FLDINVOICESTATUSNAME"].ToString();
            txtInvoiceAmount.Text = String.Format("{0:##,###,###.00}", dr["FLDINVOICEAMOUNT"]);

            txtDepartment.Text = dr["FLDDEPARTMENT"].ToString();

            if(!string.IsNullOrEmpty(dr["FLDWOGROUPID"].ToString()))
            {
                lnkWorkorder.Visible = true;
                lnkWorkorder.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + dr["FLDWOGROUPID"].ToString() + "');return false;");
            }
            else if(!string.IsNullOrEmpty(dr["FLDDEFECTID"].ToString()))
            {
                lnkWorkorder.Visible = true;
                lnkWorkorder.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'PlannedMaintenance/PlannedMaintenanceWODefectJobDetails.aspx?DefectId="+dr["FLDDEFECTID"].ToString()+"&DefectNo="+dr["FLDDEFECTNO"].ToString()+"');return false;");
            }

            if (Filter.CurrentPurchaseStockType.ToString().Equals("SPARE") || Filter.CurrentPurchaseStockType.ToString().Equals("SERVICE"))
            {
                //ddlComponentClass.SelectedQuick = dr["FLDSTOCKCLASSID"].ToString();
                //ddlComponentClass.Visible = true;

                txtComponentID.Text = dr["FLDCOMPONENTID"].ToString(); 
                txtComponentNo.Text = dr["FLDCOMPONENTNUMBER"].ToString(); 
                txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString(); 
                txtComponentID.Visible = true;
                txtComponentNo.Visible = true;
                txtComponentName.Visible = true;
                IbtnPickListComponent.Visible = true;
                ddlStockClassType.Visible  = false; 
            }
            else
            {
                ddlStockClassType.SelectedHard = dr["FLDSTOCKCLASSID"].ToString();
                //ddlComponentClass.Visible = false;
                txtComponentID.Visible = false;
                txtComponentNo.Visible = false; ;
                txtComponentName.Visible = false; ;
                IbtnPickListComponent.Visible = false; ;
                ddlStockClassType.Visible = true;
                lblComponentClass.Text = "Store Type";
            }
            txtType.Text = dr["FLDFORMTYPENAME"].ToString();
            lblFormType.Text = dr["FLDFORMTYPE"].ToString();
            txtVenderName.Text = dr["FLDVENDORNAME"].ToString();
            if (dr["FLDCURRENCYID"] != null && dr["FLDCURRENCYID"].ToString().Trim() != "")
                ucCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            else
                ucCurrency.SelectedCurrency =PhoenixPurchaseOrderForm.DefaultCurrency; 
            if (dr["FLDORDERPRIORITYID"] != null && dr["FLDORDERPRIORITYID"].ToString().Trim() != "")
                UCPeority.SelectedQuick = dr["FLDORDERPRIORITYID"].ToString();
            else
                UCPeority.SelectedQuick = PhoenixPurchaseOrderForm.DefaultPriority;  
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
                ucPayCompany.SelectedCompany = PhoenixPurchaseOrderForm.DefaultBillToCompany;
            ddlStockType.SelectedValue = dr["FLDSTOCKTYPE"].ToString();
            //if (dr["FLDCHKITEMS"].ToString().Equals("1"))
            ddlStockType.Enabled = false;

            lblBillToCompanyName.Text = dr["FLDLIABILITYCOMPANYNAME"].ToString();
            if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
            {
                btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', 'Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&windowname=detail&framename=ifMoreInfo&POPUP=Y&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + txtOrderDate.Text + "', true); ");
                IbtnPickListComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', 'Common/CommonPickListComponent.aspx?vesselid="+ dr["FLDVESSELID"].ToString() + "&framename=ifMoreInfo', true); ");
            }
            else if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
            {
                btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', 'Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=107&windowname=detail&framename=ifMoreInfo&POPUP=Y&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + txtOrderDate.Text + "', true); ");
                IbtnPickListComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', 'Common/CommonPickListComponent.aspx?vesselid=" + dr["FLDVESSELID"].ToString() + "&framename=ifMoreInfo', true); ");
            }
            else
            {
                btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '','Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=105&windowname=detail&framename=ifMoreInfo&POPUP=Y&hardtypecode=30&vesselid=" + Filter.CurrentPurchaseVesselSelection + "&budgetdate=" + txtOrderDate.Text + "', true); ");
            }

            cmdPicPartPaid.Attributes.Add("onclick", "return showPickList('spnPicPartPaid', 'codehelp1', '', 'Purchase/PurchaseOrderPartPaid.aspx?orderid=" + ViewState["orderid"].ToString() + "&vendorid=" + dr["FLDVENDORID"].ToString() + "', true); ");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                cmdPicPartPaid.Enabled = false;

            if(General.GetNullableInteger(dr["FLDPRINCIPALID"].ToString())!=null)
            {
                if (dr["FLDBUDGETCODEID"] != null && dr["FLDBUDGETCODEID"].ToString().Trim() != "")
                    btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', 'Common/CommonPickListOwnerBudget.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "&windowname=detail&framename=ifMoreInfo&POPUP=Y&budgetdate=" + txtBugetDate.Text + "&budgetid=" + dr["FLDBUDGETCODEID"].ToString() + "&Ownerid=" + dr["FLDPRINCIPALID"].ToString() + "', true); ");
                else
                    btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', 'Common/CommonPickListOwnerBudget.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "&windowname=detail&framename=ifMoreInfo&POPUP=Y&budgetdate=" + txtBugetDate.Text + "&Ownerid=" + dr["FLDPRINCIPALID"].ToString() + "', true); ");
            }
            
            cmdvendorAddress.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'Purchase/PurchaseFormAddress.aspx?addresscode=" + txtVendor.Text + "');return false;");
            cmdDeliveryAddress.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'Purchase/PurchaseFormAddress.aspx?addresscode=" + txtDeliveryAddressId.Text + "');return false;");
            cmdForwarderAddress.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'Purchase/PurchaseFormAddress.aspx?addresscode=" + txtForwarderId.Text + "');return false;");

            cmdAddressPurpose.Attributes.Add("onclick", "return showPickList('spnDeliveryAddress', 'codehelp1', '', 'Common/CommonPickListDeliveryAddress.aspx?addresscode=" + txtDeliveryAddressId.Text + "', true); ");

            ucReason4Requisition.SelectedQuick = dr["FLDREASONFORREQUISITION"].ToString();
            ucPOStatus.SelectedQuick = dr["FLDPOSTATUS"].ToString();
            if (Filter.CurrentVesselConfiguration != null && General.GetNullableInteger(Filter.CurrentVesselConfiguration) > 0)
            {
                txtPartPaid.Text = "";
                txtInvoiceNo.Text = "";
                txtInvoiceAmount.Text = "";
                txtInvoiceStatus.Text = "";
                txtVenderEsmeted.Text = "";
                txtFinalTotal.Text = "";
                imgPPClip.Visible = false;
            }
            else
            {
                if (General.GetNullableGuid(dr["FLDINVOICECODE"].ToString()) != null)
                {
                    imgPPClip.Attributes["onclick"] = "javascript:parent.openNewWindow('NATD','','Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode="
                        + dr["FLDINVOICECODE"].ToString() + "&qfrom=PURCHASE'); return false;";
                    txtFinalTotal.Text = String.Format("{0:##,###,###.00}", dr["FLDPOPAYABLEAMOUNT"]);
                    txtInvoiceCurrency.Text = dr["FLDINVOICECURRENCY"].ToString();
                    lblFinalTotal.Text = "PO Payable Amount";

                    short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
                    imgPPClip.Visible = (showcreditnotedisc == 1) ? true : false;
                }
                else
                {
                    imgPPClip.Visible = false;
                }
            }

            if (General.GetNullableInteger(dr["FLDACCOUNTID"].ToString()) != null)
                ddlVesselAccount.SelectedValue = dr["FLDACCOUNTID"].ToString();

            if (General.GetNullableDateTime(dr["FLDPURCHASEAPPROVEDATE"].ToString()) != null)
                ddlVesselAccount.Enabled = false;

            hdnPrincipalId.Value = dr["FLDPRINCIPALID"].ToString();

            ddlDeliveryto.SelectedHard = dr["FLDDELIVERYTO"].ToString();
            txtReadinessDate.Text= dr["FLDREADINESSDATE"].ToString();
        }
        DataSet ds1 = new DataSet();

        ds1 = PhoenixPurchaseOrderForm.DeliveryContactsEdit(new Guid(orderid));
        if (ds1.Tables[0].Rows.Count > 0)
        {
            txtAddressPurposeId.Text = ds1.Tables[0].Rows[0]["FLDADDRESSCONTACTID"].ToString();
            txtAddressName.Text = ds1.Tables[0].Rows[0]["FLDNAME"].ToString();
        }

        trPay.Visible = PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() == "OWNER" ? false : true;
        trPay1.Visible = PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() == "OWNER" ? false : true;
        trPay2.Visible = PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() == "OWNER" ? false : true;
    }

    protected void MenuFormGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("NEWFORM"))
            {
                Response.Redirect("../Purchase/PurchaseFormType.aspx");
            }          
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidForm())
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateOrderForm();
                UpdateDeliveryAddress();
                InsertOrderFormHistory();
                ucStatus.Show("Requisition Updated");
                String script = String.Format(" closeTelerikWindow('code1','detail','t');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
       
            }
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                if (!IsValidForm())
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateOrderForm();                 
                ApprovedOrderForm();
               // ucStatus.Text = "Requisition Updated";
            }
            if (CommandName.ToUpper().Equals("CONVERT"))
            {
                if (ViewState["orderid"].ToString() == "0")
                    Response.Redirect("../Purchase/PurchaseFormConvert.aspx");
                else
                    Response.Redirect("../Purchase/PurchaseFormConvert.aspx?orderid=" + ViewState["orderid"].ToString());
            }
            if (CommandName.ToUpper().Equals("SEND"))
            {
                if(ViewState["FORMTYPE"]!=null && General.GetNullableInteger(ViewState["FORMTYPE"].ToString())!=null&& ViewState["FORMSTATUS"]!=null && General.GetNullableInteger(ViewState["FORMSTATUS"].ToString())==54)
                {
                    PhoenixPurchaseOrderForm.ConvertOrderForm(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), (int)PhoenixHard.ACTIVE, int.Parse(ViewState["FORMTYPE"].ToString()));
                    PhoenixPurchaseOrderForm.InsertOrderFormHistory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
                    String script = String.Format("javascript:parent.fnReloadList('code1');");
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
                
            }
            else if (CommandName.ToUpper().Equals("AMENDPO"))
            {
                if (General.GetNullableGuid(ViewState["orderid"].ToString()) != null && Filter.CurrentPurchaseVesselSelection > 0)
                    PhoenixPurchaseOrderForm.AmendPO(new Guid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);

                ucStatus.Show("PO is active, You can amend the PO");
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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

    private void UpdateOrderformExtnLocation()
    {
        PhoenixPurchaseOrderForm.UpdateOrderformEXTNLocation(new Guid(ViewState["orderid"].ToString()), General.GetNullableInteger(ddlDeliveryto.SelectedHard));
    }
    private void InsertOrderFormHistory()
    {
        PhoenixPurchaseOrderForm.InsertOrderFormHistory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString()), Filter.CurrentPurchaseVesselSelection);
    }
   
    private void ApprovedOrderForm()
    {
       
        if (lblFormType.Text == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.FORMTYPE), "RQ"))
        {
            PhoenixPurchaseOrderForm.ApproveOrderForm(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["orderid"].ToString())
                , General.GetNullableDateTime(DateTime.Now.ToString()),"REQUISITION");
            ucConfirm.ErrorMessage = "Requisition is approved";
            ucConfirm.Visible = true;
        }       
    }

    private void UpdateOrderForm()
    {
        
        PhoenixPurchaseOrderForm.UpdateOrderForm(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["orderid"].ToString())
            , General.GetNullableInteger(UCDeliveryTerms.SelectedQuick), General.GetNullableInteger(UCPaymentTerms.SelectedQuick)
            , General.GetNullableInteger(UCPeority.SelectedQuick), General.GetNullableInteger(ucCurrency.SelectedCurrency)
            , General.GetNullableInteger(txtVendor.Text), General.GetNullableInteger(txtBudgetId.Text)
            , General.GetNullableInteger(txtDeliveryLocationId.Text), txtFromTitle.Text
            , General.GetNullableDateTime(txtApproveDate.Text), General.GetNullableDateTime(txtOrderDate.Text)
            , General.GetNullableDateTime(txtConfirmDate.Text), General.GetNullableDateTime(txtRecivedDate.Text)
            , General.GetNullableDateTime(txtBugetDate.Text),General.GetNullableInteger(txtDeliveryAddressId.Text)
            ,General.GetNullableDecimal(txtEstimeted.Text), General.GetNullableDecimal(txtPartPaid.Text)
            , General.GetNullableDecimal(txtFinalTotal.Text), General.GetNullableDecimal(txtVenderEsmeted.Text)
            , General.GetNullableDateTime(txtlLastDeliveryDate.Text), General.GetNullableInteger(ucPayCompany.SelectedCompany)
            , General.GetNullableDateTime(txtVenderDelveryDate.Text), ddlStockType.SelectedValue
            ,General.GetNullableInteger((ddlStockType.SelectedValue.Equals("STORE")) ?ddlStockClassType.SelectedHard:"Dummy"), General.GetNullableGuid(txtOwnerBudgetId.Text)
            ,General.GetNullableInteger(txtForwarderId.Text)
            ,General.GetNullableInteger(ucReason4Requisition.SelectedQuick)
            ,General.GetNullableInteger(ucPOStatus.SelectedQuick)
            , General.GetNullableGuid(txtComponentID.Text.Trim())
            , General.GetNullableInteger(ucProjectcode.SelectedProjectCode));       

        
    }

    private void UpdateDeliveryAddress()
    {
        PhoenixPurchaseOrderForm.UpdateDeliveryContacts(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["orderid"].ToString())
                                                                , Filter.CurrentPurchaseVesselSelection
                                                                , General.GetNullableInteger(txtAddressPurposeId.Text));
    }

    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";  
        if (txtFormNumber.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Form Number is required. Please Select ";
        if(txtFromTitle.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Form Title is required.";

        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && General.GetNullableInteger(ucPayCompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Bill To is required.";

        if (General.GetNullableDateTime(txtConfirmDate.Text)!=null)
        {
            if (General.GetNullableDateTime(txtOrderDate.Text)==null || (General.GetNullableDateTime(txtConfirmDate.Text) < General.GetNullableDateTime(txtOrderDate.Text)))
            ucError.ErrorMessage = "Confirmation date cannot be less than order date.";
        }

        if (General.GetNullableInteger(ucReason4Requisition.SelectedQuick) == null)
            ucError.ErrorMessage = "Reason for Requisition is required.";

        return (!ucError.IsError);
    }

    //protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    //{

    //}
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(hdnPrincipalId.Value) != null)
        {
            btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', 'Common/CommonPickListOwnerBudget.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "&windowname=detail&framename=ifMoreInfo&POPUP=Y&budgetdate=" + txtBugetDate.Text + "&budgetid=" + txtBudgetId.Text + "&Ownerid=" + hdnPrincipalId.Value + "', true); ");
            Filter.CurrentSelectedESMBudgetCode = null;
        }
    }
    private void EnabelField()
    {
        btnShowBudget.Visible = false;
        ucPayCompany.Enabled = false;
        btnShowOwnerBudget.Visible = false;
        cmdPicPartPaid.Visible = false;
    }

    private void FieldSetViewState()
    {
        txtVenderEsmeted.Visible = IsVisible("txtVenderEsmeted");
        lblVenderEsmeted.Visible = IsVisible("lblVenderEsmeted");
        txtFinalTotal.Visible = IsVisible("txtFinalTotal");
        lblFinalTotal.Visible = IsVisible("lblFinalTotal");
        lblPartPaid.Visible = IsVisible("lblPartPaid");
        txtPartPaid.Visible = IsVisible("txtPartPaid");
        cmdPicPartPaid.Visible = IsVisible("cmdPicPartPaid");
        
       
    }

    public bool IsVisible(string command)
    {
        NameValueCollection nvc = null;
        if (ViewState["FIELDVIEWPERMISSION"]== null)
            return true;
        else
        {
            nvc = (NameValueCollection)ViewState["FIELDVIEWPERMISSION"];
            return (nvc[command] == "0") ? false : true;
        }
    }

    protected void cmdClear_Click(object sender, EventArgs e)
    {
        txtDeliveryLocationCode.Text = "";
        txtDeliveryLocationName.Text = "";
        txtDeliveryLocationId.Text = "";
    }

    protected void cmdClearBudget_Click(object sender, EventArgs e)
    {
        txtBudgetCode.Text = "";
        txtBudgetName.Text = "";
        txtBudgetId.Text = "";
        txtBudgetgroupId.Text = "";
    }

    protected void cmdClearAddress_Click(object sender, EventArgs e)
    {
        txtDeliveryAddressCode.Text = "";
        txtDeliveryAddressName.Text = "";
        txtDeliveryAddressId.Text = "";
    }

    protected void cmdClearOwnerBudget_Click(object sender, EventArgs e)
    {
        txtOwnerBudgetCode.Text = "";
        txtOwnerBudgetName.Text = "";
        txtOwnerBudgetId.Text = "";
        txtOwnerBudgetgroupId.Text = "";
    }

    protected void cmdClearDeliveryContact_Click(object sender, EventArgs e)
    {
        txtAddressPurposeId.Text = "";
        txtAddressName.Text = "";
    }
    protected void BindVesselAccount()
    {
        ddlVesselAccount.DataSource = PhoenixPurchaseApprovedVesselAccount.OrderFormVesselAccountList(Filter.CurrentPurchaseVesselSelection, ViewState["orderid"] != null ? General.GetNullableGuid(ViewState["orderid"].ToString()) : null);
        ddlVesselAccount.DataBind();

        ddlVesselAccount.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ddlVeselAccount_TextChanged(object sender,EventArgs e)
    {
        try
        {
            if (General.GetNullableInteger(ddlVesselAccount.SelectedValue) != null && ViewState["orderid"] != null)
            {
                PhoenixPurchaseApprovedVesselAccount.OrderFormVesselAccountMap(new Guid(ViewState["orderid"].ToString())
                                                                , Filter.CurrentPurchaseVesselSelection
                                                                , General.GetNullableInteger(ddlVesselAccount.SelectedValue)
                                                                , General.GetNullableString(txtFormNumber.Text)
                                                               );

                if (General.GetNullableInteger(ddlVesselAccount.SelectedValue) != null)
                {
                    DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(ddlVesselAccount.SelectedValue));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        hdnPrincipalId.Value = ds.Tables[0].Rows[0]["FLDPRINCIPALID"].ToString();
                        btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', 'Common/CommonPickListOwnerBudget.aspx?vesselid=" + Filter.CurrentPurchaseVesselSelection + "&windowname=detail&framename=ifMoreInfo&POPUP=Y&budgetdate=" + txtBugetDate.Text + "&budgetid=" + txtBudgetId.Text + "&Ownerid=" + hdnPrincipalId.Value + "', true); ");
                        Filter.CurrentSelectedESMBudgetCode = null;
                    }

                }


            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }

    protected void ddlDeliveryto_TextChangedEvent(object sender, EventArgs e)
    {
        UpdateOrderformExtnLocation();
    }
}
