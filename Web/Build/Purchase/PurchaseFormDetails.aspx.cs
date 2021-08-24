using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Purchase_PurchaseFormDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        SessionUtil.PageFieldViewPermission(this.ViewState);   
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            
            if (!IsPostBack)
            {
                VesselConfiguration();

                ViewState["title"] = "";
                toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
                if (Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null)
                {
                    lblImportedDate.Text = "Imported";
                }
                else
                {
                    lblImportedDate.Text = "Exported";
                }

                if (Request.QueryString["VesselId"] != null)
                {
                    ViewState["VesselId"] = Request.QueryString["VesselId"].ToString();
                    Filter.CurrentPurchaseVesselSelection = Convert.ToInt32(ViewState["VesselId"]);
                }
                else
                {
                    ViewState["VesselId"] = "";
                }
                //txtAddressName.Attributes.Add("readonly", "readonly");
                ucMultiVendor.Attributes.Add("style", "display:none");
                //txtAddressPurposeId.Attributes.Add("style", "display:none");
            
                UCPriority.QuickTypeCode =((int)PhoenixQuickTypeCode.ORDERPRIORITY).ToString();
                UCDeliveryTerms.QuickTypeCode =((int) PhoenixQuickTypeCode.DELIVERYTERM).ToString();
                UCPaymentTerms.QuickTypeCode = ((int)PhoenixQuickTypeCode.PAYMENTTERM).ToString();
                ddlComponentClass.QuickTypeCode = ((int)PhoenixQuickTypeCode.COMPONENTCLASS).ToString();
                ddlStockClassType.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();

                ucBudgetCode.Enabled = SessionUtil.CanAccess(this.ViewState, "BUDGETCODE");
                imgClear1.Visible = SessionUtil.CanAccess(this.ViewState, "IMGBUDGETCODE");
                ucOwnerBudgetCode.Enabled = SessionUtil.CanAccess(this.ViewState, "OWNERBUDGETCODE");
                imgClearOwnerBudget.Visible = SessionUtil.CanAccess(this.ViewState, "IMGOWNERBUDGETCODE");
                

                if ((Request.QueryString["orderid"] != null) && (Request.QueryString["orderid"] != ""))
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    BindVesselAccount();
                    BindOwner();
                    BindData(Request.QueryString["orderid"].ToString());
                }

                if (ddlVesselAccount.SelectedValue == "Dummy")
                {
                    ucOwnerBudgetCode.Enabled = false;
                }

                if (Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null)
                {
                    string dtkey = ViewState["DTKEY"] != null ? ViewState["DTKEY"].ToString() : "";

                    if (ViewState["DTKEY"] != null)
                    {
                        if (Session["launchedfrom"] == null || Session["launchedfrom"].ToString().ToUpper() == "PURCHASE")
                            toolbarmain.AddLinkButton("javascript:openNewWindow('FormHistory','','"+Session["sitepath"]+"/Common/CommonPhoenixAuditChanges.aspx?dtkey=" + dtkey + "&shortcode=PUR-FORM" + "')", "Form History", "HISTORY", ToolBarDirection.Right);
                    }
                }
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                {
                    ucReason4Requisition.CssClass = "input_mandatory";
                }
                else
                {
                    ucReason4Requisition.CssClass = "input";
                }
                

               
                
                toolbarmain.AddButton("Convert", "CONVERT", ToolBarDirection.Right);
                toolbarmain.AddButton("Attachment", "ATTACHMENT", ToolBarDirection.Right);
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuFormGeneral.Title = "General      (" + ViewState["title"].ToString() + ")     ";
                MenuFormGeneral.AccessRights = this.ViewState;
                MenuFormGeneral.MenuList = toolbarmain.Show();
                //MenuFormGeneral.SetTrigger(pnlFormGeneral);

                FieldSetViewState();
            }         

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixCommonPurchase.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    private void BindOwner()
    {
        //ucOwnerBudgetCode.OwnerId = hdnPrincipalId.Value;
        //ucOwnerBudgetCode.VesselId = Filter.CurrentPurchaseVesselSelection.ToString();
        //ucOwnerBudgetCode.BudgetId = ucBudgetCode.SelectedBudgetCode;
        //ucOwnerBudgetCode.bind();
        //ucOwnerBudgetCode.DataBind();
    }
    private void BindData(string  orderid)
    {
        DataSet ds = new DataSet();
        ds = PhoenixPurchaseOrderForm.EditOrderForm(new Guid(orderid), Filter.CurrentPurchaseVesselSelection);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtFormNumber.Text = dr["FLDFORMNO"].ToString();
            ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
            txtFromTitle.Text = dr["FLDTITLE"].ToString();
            ucMultiPortAddress.SelectedValue = dr["FLDDELIVERYPLACEID"].ToString();
            ucMultiPortAddress.Text = dr["FLDDELIVERYPLACE"].ToString();
            ucMultiDeliveryAddress.SelectedValue = dr["FLDDELIVERYID"].ToString();
            ucMultiDeliveryAddress.Text = dr["FLDDELIVERYADDRESS"].ToString();
            txtCreatedDate.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"].ToString());
            if(General.GetNullableInteger(dr["FLDBUDGETCODEID"].ToString())!=null)
            {
                ucBudgetCode.SelectedValue = int.Parse(dr["FLDBUDGETCODEID"].ToString());
            }
            
            ucOwnerBudgetCode.SelectedValue= dr["FLDOWNERBUDGETID"].ToString();
            txtBugetDate.Text = General.GetDateTimeToString(dr["FLDBUDGETDATE"].ToString());
            txtEstimeted.Text = String.Format("{0:##,###,###.00}", dr["FLDESTIMATEDTOTAL"]);
            txtFinalTotal.Text = String.Format("{0:##,###,###.00}", dr["FLDACTUALTOTAL"]);
            txtlLastDeliveryDate.Text =  General.GetDateTimeToString(dr["FLDLATESTDELIVERYDATE"].ToString());
            txtPartPaid.Text = String.Format("{0:##,###,###.00}", dr["FLDPARTPAYMENT"]);
            if (Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null)
                txtRecivedDate.Text = General.GetDateTimeToString(dr["FLDEXPORTIMPORTDATE"].ToString());
            else
                txtRecivedDate.Text = General.GetDateTimeToString(Filter.CurrentPurchaseVesselSendDateSelection.ToString());
            ucMultiVendor.Text = dr["FLDVENDORID"].ToString();
            ucMultiVendor.SelectedValue= dr["FLDVENDORID"].ToString();
            txtVenderDelveryDate.Text = General.GetDateTimeToString(dr["FLDVENDORDELIVERYDATE"].ToString());
            txtVenderEsmeted.Text = String.Format("{0:##,###,###.00}", dr["FLDVENDORADVISEDTOTAL"]);
            txtApproveDate.Text = General.GetDateTimeToString(dr["FLDPURCHASEAPPROVEDATE"].ToString());
            txtConfirmDate.Text = General.GetDateTimeToString(dr["FLDCONFIRMATIONDATE"].ToString());
            txtOrderDate.Text = General.GetDateTimeToString(dr["FLDORDEREDDATE"].ToString());
            txtStatus.Text = dr["FLDFORMSTATUSNAME"].ToString();
            ucMultiAgentAddress.Text = dr["FLDFORWARDERNAME"].ToString();
            ucMultiAgentAddress.SelectedValue = dr["FLDFORWARDER"].ToString();
            txtInvoiceNo.Text = dr["FLDINVOICENUMBER"].ToString();
            txtInvoiceStatus.Text = dr["FLDINVOICESTATUSNAME"].ToString();
            txtInvoiceAmount.Text = String.Format("{0:##,###,###.00}", dr["FLDINVOICEAMOUNT"]);

            if (Filter.CurrentPurchaseStockType.ToString().Equals("SPARE") || Filter.CurrentPurchaseStockType.ToString().Equals("SERVICE"))
            {
                ddlComponentClass.SelectedQuick = dr["FLDSTOCKCLASSID"].ToString();
                ddlComponentClass.Visible = true;
                ddlStockClassType.Visible  = false; 
            }
            else
            {
                ddlStockClassType.SelectedHard = dr["FLDSTOCKCLASSID"].ToString();
                ddlComponentClass.Visible = false;
                ddlStockClassType.Visible = true;
                lblComponentClass.Text = "Store Type";
            }
            txtType.Text = dr["FLDFORMTYPENAME"].ToString();
            lblFormType.Text = dr["FLDFORMTYPE"].ToString();
            ucMultiVendor.Text = dr["FLDVENDORNAME"].ToString();
            if (dr["FLDCURRENCYID"] != null && dr["FLDCURRENCYID"].ToString().Trim() != "")
                ucCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            else
                ucCurrency.SelectedCurrency =PhoenixPurchaseOrderForm.DefaultCurrency; 
            if (dr["FLDORDERPRIORITYID"] != null && dr["FLDORDERPRIORITYID"].ToString().Trim() != "")
                UCPriority.SelectedQuick = dr["FLDORDERPRIORITYID"].ToString();
            else
                UCPriority.SelectedQuick = PhoenixPurchaseOrderForm.DefaultPriority;  
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
            ucOwnerBudgetCode.SelectedValue= dr["FLDOWNERBUDGETID"].ToString();
            if (dr["FLDBILLTOCOMPANYID"] != null && dr["FLDBILLTOCOMPANYID"].ToString().Trim() != "")
                ucPayCompany.SelectedCompany = dr["FLDBILLTOCOMPANYID"].ToString();
            else
                ucPayCompany.SelectedCompany = PhoenixPurchaseOrderForm.DefaultBillToCompany;
            ddlStockType.SelectedValue = dr["FLDSTOCKTYPE"].ToString();
            ddlStockType.Enabled = false;
            lblBillToCompanyName.Text = dr["FLDLIABILITYCOMPANYNAME"].ToString();
  
            ViewState["VENDORID"] = dr["FLDVENDORID"].ToString();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                cmdPicPartPaid.Enabled = false;

            cmdvendorAddress.Attributes.Add("onclick", "openNewWindow('Address', '', '"+Session["sitepath"]+"/Purchase/PurchaseFormAddress.aspx?addresscode=" + ucMultiVendor.SelectedValue + "');return false;");
            cmdDeliveryAddress.Attributes.Add("onclick", "openNewWindow('Address', '', '" + Session["sitepath"] + "/Purchase/PurchaseFormAddress.aspx?addresscode=" + ucMultiDeliveryAddress.SelectedValue + "');return false;");
            cmdForwarderAddress.Attributes.Add("onclick", "openNewWindow('Address', '', '" + Session["sitepath"] + "/Purchase/PurchaseFormAddress.aspx?addresscode=" + ucMultiAgentAddress.SelectedValue + "');return false;");

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
                //imgPPClip.Visible = false;
            }
            else
            {
                if (General.GetNullableGuid(dr["FLDINVOICECODE"].ToString()) != null)
                {
                    //imgPPClip.Attributes["onclick"] = "javascript:Openpopup('NATD','','../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode="
                    //    + dr["FLDINVOICECODE"].ToString() + "&qfrom=PURCHASE'); return false;";
                    txtFinalTotal.Text = String.Format("{0:##,###,###.00}", dr["FLDPOPAYABLEAMOUNT"]);
                    txtInvoiceCurrency.Text = dr["FLDINVOICECURRENCY"].ToString();
                    lblFinalTotal.Text = "PO Payable Amount";

                    short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
                    //imgPPClip.Visible = (showcreditnotedisc == 1) ? true : false;
                }
                else
                {
                    //imgPPClip.Visible = false;
                }
            }
            if (General.GetNullableInteger(dr["FLDACCOUNTID"].ToString()) != null)
                ddlVesselAccount.SelectedValue = dr["FLDACCOUNTID"].ToString();
            ViewState["title"] = dr["FLDFORMNO"].ToString();
            hdnPrincipalId.Value = dr["FLDPRINCIPALID"].ToString();
            
        }
        DataSet ds1 = new DataSet();

        ds1 = PhoenixPurchaseOrderForm.DeliveryContactsEdit(new Guid(orderid));
    }

    protected void BindVesselAccount()
    {
        ddlVesselAccount.DataSource = PhoenixPurchaseApprovedVesselAccount.OrderFormVesselAccountList(Filter.CurrentPurchaseVesselSelection, ViewState["orderid"] != null ? General.GetNullableGuid(ViewState["orderid"].ToString()) : null);
        ddlVesselAccount.DataBind();

        ddlVesselAccount.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ddlVeselAccount_TextChanged(object sender, EventArgs e)
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
                    ucOwnerBudgetCode.Enabled = true;
                    Filter.CurrentSelectedESMBudgetCode = null;
                }

            }
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
                ucStatus.Text = "Requisition Updated";
                String script = String.Format("javascript:parent.fnReloadList('Form');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                BindData(Request.QueryString["orderid"].ToString());
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
            }
            if (CommandName.ToUpper().Equals("CONVERT"))
            {
                if (ViewState["orderid"].ToString() == "0")
                    Response.Redirect("../Purchase/PurchaseFormConverter.aspx");
                else
                    Response.Redirect("../Purchase/PurchaseFormConverter.aspx?orderid=" + ViewState["orderid"].ToString());
            }
            if(CommandName.ToUpper().Equals("CLOSE"))
            {
                //string Script = "";
                //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                //Script += " fnReloadList();";
                //Script += "</script>" + "\n";
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

                String script = String.Format("javascript:parent.fnReloadList('Form');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                Response.Redirect("../Purchase/PurchaseAttachment.aspx?orderid=" + ViewState["orderid"].ToString() + "&DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.PURCHASE + "&pageno=" + ViewState["PAGENUMBER"]);
            }
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
            , General.GetNullableInteger(UCPriority.SelectedQuick), General.GetNullableInteger(ucCurrency.SelectedCurrency)
            , General.GetNullableInteger(ucMultiVendor.SelectedValue)
            , General.GetNullableInteger(ucBudgetCode.SelectedBudgetCode.ToString())//General.GetNullableInteger(txtBudgetId.Text)
            , General.GetNullableInteger(ucMultiPortAddress.SelectedValue)//General.GetNullableInteger(txtDeliveryLocationId.Text)
            , txtFromTitle.Text
            , General.GetNullableDateTime(txtApproveDate.Text), General.GetNullableDateTime(txtOrderDate.Text)
            , General.GetNullableDateTime(txtConfirmDate.Text), General.GetNullableDateTime(txtRecivedDate.Text)
            , General.GetNullableDateTime(txtBugetDate.Text)
            , General.GetNullableInteger(ucMultiDeliveryAddress.SelectedValue)//,General.GetNullableInteger(txtDeliveryAddressId.Text)
            , General.GetNullableDecimal(txtEstimeted.Text), General.GetNullableDecimal(txtPartPaid.Text)
            , General.GetNullableDecimal(txtFinalTotal.Text), General.GetNullableDecimal(txtVenderEsmeted.Text)
            , General.GetNullableDateTime(txtlLastDeliveryDate.Text), General.GetNullableInteger(ucPayCompany.SelectedCompany)
            , General.GetNullableDateTime(txtVenderDelveryDate.Text), ddlStockType.SelectedItem.Value
            , General.GetNullableInteger((ddlStockType.SelectedItem.Value.Equals("STORE")) ?ddlStockClassType.SelectedHard:ddlComponentClass.SelectedQuick)
            , General.GetNullableGuid(ucOwnerBudgetCode.SelectedValue)//General.GetNullableGuid(txtOwnerBudgetId.Text)
            , General.GetNullableInteger(ucMultiAgentAddress.SelectedValue)//General.GetNullableInteger(txtForwarderId.Text)
            , General.GetNullableInteger(ucReason4Requisition.SelectedQuick)
            , General.GetNullableInteger(ucPOStatus.SelectedQuick));       

        
    }

    private void UpdateDeliveryAddress()
    {
        PhoenixPurchaseOrderForm.UpdateDeliveryContacts(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["orderid"].ToString())
                                                                , Filter.CurrentPurchaseVesselSelection
                                                                , null);
                                                                //, General.GetNullableInteger(txtAddressPurposeId.Text));
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

        //if (General.GetNullableDateTime(txtConfirmDate.Text)!=null)
        //{
        //    if (General.GetNullableDateTime(txtConfirmDate.Text) != null || (General.GetNullableDateTime(txtConfirmDate.Text) < General.GetNullableDateTime(txtOrderDate.Text)))
        //    ucError.ErrorMessage = "Confirmation date cannot be less than order date.";
        //}

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
        {
            if (General.GetNullableInteger(ucReason4Requisition.SelectedQuick) == null)
                ucError.ErrorMessage = "Reason for Requisition is required.";
        }

        return (!ucError.IsError);
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

    protected void cmdClear_Click(object sender, ImageClickEventArgs e)
    {
        ucMultiPortAddress.Text = "";
    }

    protected void cmdClearBudget_Click(object sender, ImageClickEventArgs e)
    {
        ucBudgetCode.SelectedValue = 0;
    }

    protected void cmdClearAddress_Click(object sender, ImageClickEventArgs e)
    {
        ucMultiDeliveryAddress.Text = "";
    }

    protected void cmdClearOwnerBudget_Click(object sender, ImageClickEventArgs e)
    {
        ucOwnerBudgetCode.SelectedValue = "";
    }

    protected void ucBudgetCode_TextChangedEvent(object sender, EventArgs e)
    {
        ucOwnerBudgetCode.OwnerId = hdnPrincipalId.Value;
        ucOwnerBudgetCode.VesselId = Filter.CurrentPurchaseVesselSelection.ToString();
        ucOwnerBudgetCode.BudgetId = ucBudgetCode.SelectedBudgetCode;
        ucOwnerBudgetCode.bind();
        ucOwnerBudgetCode.DataBind();
    }

    protected void cmdPicPartPaid_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Purchase/PurchaseOrderFormPartPaid.aspx?orderid=" + ViewState["orderid"].ToString() + "&vendorid=" + ViewState["VENDORID"].ToString());
    }
}
