using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Purchase;


public partial class PurchaseDeliveryFormsGeneral : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
       
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
                     
            if (Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null)
            {                
                lblImportedDate.Text  = "Imported";               
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
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            if (!IsPostBack)
            {
                UCPeority.QuickTypeCode =((int)PhoenixQuickTypeCode.ORDERPRIORITY).ToString();
                UCDeliveryTerms.QuickTypeCode =((int) PhoenixQuickTypeCode.DELIVERYTERM).ToString();
                UCPaymentTerms.QuickTypeCode = ((int)PhoenixQuickTypeCode.PAYMENTTERM).ToString();
                Title1.Text = "General      (" + PhoenixPurchaseOrderForm.FormNumber + ")     ";


                if ((Request.QueryString["orderid"] != null) && (Request.QueryString["orderid"] != ""))
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    BindData(Request.QueryString["orderid"].ToString());
                }
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
        ds = PhoenixPurchaseOrderForm.EditOrderForm(new Guid(orderid));
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
           // txtlastvenderDeliveryDate.Text = General.GetDateTimeToString(dr["FLDLATESTDELIVERYDATEFORVENDOR"].ToString());
            txtlLastDeliveryDate.Text =  General.GetDateTimeToString(dr["FLDLATESTDELIVERYDATE"].ToString());
            txtPartPaid.Text = String.Format("{0:##,###,###.00}", dr["FLDPARTPAYMENT"]);
            if (Filter.CurrentVesselConfiguration.Equals("0") || Filter.CurrentVesselConfiguration == null)
                txtRecivedDate.Text = General.GetDateTimeToString(dr["FLDRECEIVEDDATE"].ToString());
            else
                txtRecivedDate.Text = General.GetDateTimeToString(Filter.CurrentPurchaseVesselSendDateSelection.ToString());
            txtVendor.Text = dr["FLDVENDORID"].ToString();
            txtVendorNumber.Text = dr["FLDVENDORCODE"].ToString();
            txtVenderDelveryDate.Text = General.GetDateTimeToString(dr["FLDVENDORDELIVERYDATE"].ToString());
            txtVenderEsmeted.Text = String.Format("{0:##,###,###.00}", dr["FLDVENDORADVISEDTOTAL"]);
            txtApproveDate.Text = General.GetDateTimeToString(dr["FLDPURCHASEAPPROVEDATE"].ToString());
            txtConfirmDate.Text = General.GetDateTimeToString(dr["FLDCONFIRMATIONDATE"].ToString());
            txtOrderDate.Text = General.GetDateTimeToString(dr["FLDORDEREDDATE"].ToString());
            txtStatus.Text = dr["FLDFORMSTATUSNAME"].ToString();
            txtStockClass.Text = dr["FLDSTOCKCLASS"].ToString();
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
            if (dr["FLDBILLTOCOMPANYID"] != null && dr["FLDBILLTOCOMPANYID"].ToString().Trim() != "")
                ucPayCompany.SelectedCompany = dr["FLDBILLTOCOMPANYID"].ToString();
            else
                ucPayCompany.SelectedCompany = "2";
           ddlStockType.SelectedValue = dr["FLDSTOCKTYPE"].ToString();
            if (dr["FLDCHKITEMS"].ToString().Equals("1"))
                ddlStockType.Enabled = false; 
          }


    }

    protected void MenuFormGeneral_TabStripCommand(object sender, EventArgs e)
    {
       
        
    }

    private void SendToOffice()
    {
        PhoenixPurchaseOrderForm.OrderFormSendToOffice(PhoenixSecurityContext.CurrentSecurityContext.UserCode,new Guid(ViewState["orderid"].ToString()));
        ucConfirm.ErrorMessage = "Requisition is sent";
        ucConfirm.Visible = true;
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

   
    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";  
        if (txtFormNumber.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Form Number is required. Please Select ";
        if(txtFromTitle.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Form Title is required.";
        if (!txtConfirmDate.Text.Trim().Equals(""))
        {
            if (txtOrderDate.Text.Trim().Equals("") || (General.GetNullableDateTime(txtConfirmDate.Text) < General.GetNullableDateTime(txtOrderDate.Text)))
            ucError.ErrorMessage = "Confirmation date cannot be less than order date.";
        }
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    private void EnabelField()
    {
       ucPayCompany.Enabled = false; 
    }
}
