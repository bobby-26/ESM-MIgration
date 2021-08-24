using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;

public partial class PurchaseContractGeneral : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddButton("New", "NEW",ToolBarDirection.Right);        
        
        try
        {
            MenuFormGeneral.Title = "General  (" + PhoenixPurchaseContract.VendorName + ")";
            MenuFormGeneral.MenuList = toolbarmain.Show();
           // MenuFormGeneral.SetTrigger(pnlFormGeneral);
            txtVendorId.Attributes.Add("style", "display:none;");
            if (!IsPostBack)
            {
                UCDeliveryTerms.QuickTypeCode =((int)PhoenixQuickTypeCode.DELIVERYTERM).ToString();
                UCPaymentTerms.QuickTypeCode = ((int)PhoenixQuickTypeCode.PAYMENTTERM).ToString(); 
                
                if (Request.QueryString["contractid"] != null)
                {
                    ViewState["contractid"] = Request.QueryString["contractid"].ToString();
                    BindData(Request.QueryString["contractid"].ToString());
                    ViewState["SaveStatus"] = "Edit";
                }
                else
                {
                    ViewState["SaveStatus"] = "New";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData(string  contractid)
    {
        DataSet ds = PhoenixPurchaseContract.EditContract(new Guid(contractid));

        if (ds.Tables[0].Rows.Count > 0)
        {        
            DataRow dr = ds.Tables[0].Rows[0];

            txtContractNumber.Text = dr["FLDCONTRACTNO"].ToString(); 
            txtVendorId.Text = dr["FLDVENDORID"].ToString();
            txtVendorCode.Text = dr["FLDVENDORCODE"].ToString();
            txtVenderName.Text = dr["FLDVENDORNAME"].ToString();
            txtContractDescription.Text = dr["FLDCONTRACTDESCRIPTION"].ToString();
            txtConractComments.Text = dr["FLDCOMMENTS"].ToString() ;
            txtContractDate.Text = General.GetDateTimeToString(dr["FLDCONTRACTDATE"].ToString());            
            txtContractExpireDate.Text = General.GetDateTimeToString(dr["FLDEXPIRYDATE"].ToString());
            txtContractNotes.Text = dr["FLDNOTES"].ToString();
            UCDeliveryTerms.SelectedQuick = dr["FLDDELIVERYTERMSID"].ToString();
            UCPaymentTerms.SelectedQuick = dr["FLDPAYMENTTERMSID"].ToString();
            chkPackage.Checked = dr["FLDPACKAGEYESNO"].ToString() == "1" ? true : false;
        }
    }

    protected void MenuFormGeneral_TabStripCommand(object sender, EventArgs e)
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
                if (ViewState["SaveStatus"].ToString().Equals("New"))
                {
                    InsertContract();
                }
                else if (ViewState["SaveStatus"].ToString().Equals("Edit"))
                {
                    UpdateContract();
                }
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);               
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                ClearText();
                ViewState["SaveStatus"] = "New";
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearText()
    {
        txtContractNumber.Text = "";
        txtVendorId.Text = "";
        txtVendorCode.Text = "";
        txtVenderName.Text = "";
        txtContractDescription.Text = "";
        txtConractComments.Text = "";
        txtContractDate.Text = "";
        txtContractExpireDate.Text = "";
        txtContractNotes.Text = "";
        UCDeliveryTerms.SelectedQuick = "";
        UCPaymentTerms.SelectedQuick = "";     
    }   

    private void InsertContract()
    {
        if (!IsValidForm())
        {
            ucError.Visible = true;
            return;
        }
        PhoenixPurchaseContract.InsertContract(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(txtVendorId.Text)
            , txtContractNumber.Text, txtContractDescription.Text
            , txtConractComments.Text, General.GetNullableInteger(UCDeliveryTerms.SelectedQuick)
            , General.GetNullableInteger(UCPaymentTerms.SelectedQuick), General.GetNullableDateTime(txtContractDate.Text)
            , General.GetNullableDateTime(txtContractExpireDate.Text)
            , General.GetNullableDecimal(""), txtContractNotes.Text
            , chkPackage.Checked == true ? 1 : 0);
    }

    private void UpdateContract()
    {
        if (!IsValidForm())
        {
            ucError.Visible = true;
            return;
        }

        PhoenixPurchaseContract.UpdateContract(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["contractid"].ToString())
            , int.Parse(txtVendorId.Text), txtContractNumber.Text
            , txtContractDescription.Text, txtConractComments.Text
            , General.GetNullableInteger(UCDeliveryTerms.SelectedQuick),
            General.GetNullableInteger(UCPaymentTerms.SelectedQuick),
            General.GetNullableDateTime(txtContractDate.Text), General.GetNullableDateTime(txtContractExpireDate.Text)
            , General.GetNullableDecimal(""), txtContractNotes.Text
            , chkPackage.Checked == true ? 1 : 0);
    }

    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";   

        if (txtContractNumber.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Contract number is required. ";

        if (txtVendorId.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Vendor is required, please select Vendor";

        if (General.GetNullableDateTime(txtContractDate.Text) != null && General.GetNullableDateTime(txtContractExpireDate.Text) != null)
        {
            if (General.GetNullableDateTime(txtContractExpireDate.Text) < General.GetNullableDateTime(txtContractDate.Text))
                ucError.ErrorMessage = "'Expiry Date' cannot be less than 'Contract Date'";
        }
      
        return (!ucError.IsError);
    }
}
