using System;
using System.Data;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class AccountsInvoicePaymentVoucherAdmin : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");
            txtVendorId.Attributes.Add("style", "visibility:hidden");
            txtVendorId.Attributes.Add("onkeydown", "return false;");
            txtBankName.Attributes.Add("style", "visibility:hidden");
            txtBankID.Attributes.Add("style", "visibility:hidden");


            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("Remove All Line Items", "REMOVE", ToolBarDirection.Right);

            MenuVoucher.AccessRights = this.ViewState;
            MenuVoucher.Title = "Payment Voucher   (" + PhoenixAccountsVoucher.VoucherNumber + ")     ";
            MenuVoucher.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;


                if (Request.QueryString["VOUCHERID"] != string.Empty)
                {
                    ViewState["VOUCHERID"] = Request.QueryString["VOUCHERID"];
                }
                InvoicePaymentVoucherEdit();
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Voucher_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixAccountsInvoicePaymentVoucherAdmin.PaymentVoucherUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                int.Parse(txtBankID.Text),
                                                                                new Guid(ViewState["VOUCHERID"].ToString()));
                ucStatus.Text = "Voucher information Updated";


            }
            if (CommandName.ToUpper().Equals("REMOVE"))
            {
                PhoenixAccountsInvoicePaymentVoucherAdmin.RemovePaymentVoucherLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                        new Guid(ViewState["VOUCHERID"].ToString()));

                ucStatus.Text = "Payment Voucher Line Item Removed";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        txtBeneficiaryBankName.Text = txtBankName.Text;
        DataSet ds = PhoenixAccountsInvoicePaymentVoucherAdmin.BankDetails(int.Parse(txtBankID.Text));
        txtBeneficiaryName.Text = ds.Tables[0].Rows[0]["FLDBENEFICIARYNAME"].ToString();
    }

    protected void InvoicePaymentVoucherEdit()
    {
        if (ViewState["VOUCHERID"] != null)
        {
            DataSet ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherEdit(ViewState["VOUCHERID"].ToString());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtVoucherNumber.Text = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();
                    txtVoucherDate.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"].ToString());
                    txtVendorId.Text = dr["FLDSUPPLIERCODE"].ToString();
                    txtVendorCode.Text = dr["FLDCODE"].ToString();
                    txtVendorName.Text = dr["FLDNAME"].ToString();
                    ddlCurrency.SelectedCurrency = dr["FLDCURRENCY"].ToString();
                    txtAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDAMOUNT"])); ;

                    imgBankPicklist.Attributes.Add("onclick", "return showPickList('spnPickListBank', 'codehelp1', '', '../Common/CommonPickListBankInformationAddress.aspx?addresscode=" + txtVendorId.Text + "&currency=" + dr["FLDCURRENCY"].ToString() + "', true);");
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[1].Rows[0];
                    txtBeneficiaryBankName.Text = dr["FLDBANKNAME"].ToString();
                    txtBeneficiaryName.Text = dr["FLDBENEFICIARYNAME"].ToString();
                    txtAccountNumber.Text = dr["FLDACCOUNTNUMBER"].ToString();
                    txtBankID.Text = dr["FLDBANKID"].ToString();
                }
            }
        }

        short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;
        lblCurrency.Visible = (showcreditnotedisc == 1) ? true : false;
        ddlCurrency.Visible = (showcreditnotedisc == 1) ? true : false;
    }

    protected bool IsValidVoucher()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlCurrency.SelectedCurrency.ToUpper() == "DUMMY")
            ucError.ErrorMessage = "Currency is required.";
        if (txtVendorId.Text.ToString() == "")
            ucError.ErrorMessage = "Supplier is required.";

        return (!ucError.IsError);
    }
}
