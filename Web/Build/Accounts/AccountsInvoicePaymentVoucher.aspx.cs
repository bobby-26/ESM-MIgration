using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsInvoicePaymentVoucher : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        MenuVoucher.AccessRights = this.ViewState;
        MenuVoucher.Title = "Payment Voucher   (" + PhoenixAccountsVoucher.VoucherNumber + ")     ";
        MenuVoucher.MenuList = toolbar.Show();
        txtVendorId.Attributes.Add("style", "visibility:hidden");
        txtVendorId.Attributes.Add("onkeydown", "return false;");

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
            if (ViewState["VOUCHERID"] != null)
            {
                // ttlVoucher.Text = "Payment Voucher   (" + PhoenixAccountsVoucher.VoucherNumber + ")     ";
            }
        }
    }

    protected void Voucher_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("Title"))
        {

        }
        if (CommandName.ToUpper().Equals("SAVE"))
        {

            try
            {
                PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                ucStatus.Text = "Voucher information added";
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            ucStatus.Text = "Payment Voucher information added";
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void Reset()
    {
        if (ViewState["VOUCHERID"] != null)
        {
            ViewState["VOUCHERID"] = null;
            txtVoucherNumber.Text = "";
            txtVendorId.Text = "";
            txtVendorCode.Text = "";
            txtVendorName.Text = "";
            ddlCurrency.SelectedCurrency = "";
            //ttlVoucher.Text = "Invoice Payment Voucher";
        }
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
                    PhoenixAccountsVoucher.VoucherNumber = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();
                    txtVoucherNumber.Text = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();
                    txtVoucherDate.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"].ToString());
                    txtVendorId.Text = dr["FLDSUPPLIERCODE"].ToString();
                    txtVendorCode.Text = dr["FLDCODE"].ToString();
                    txtVendorName.Text = dr["FLDNAME"].ToString();
                    ddlCurrency.SelectedCurrency = dr["FLDCURRENCY"].ToString();
                    txtStatus.Text = dr["FLDPAYMENTVOUCHERSTATUS"].ToString();
                    // ttlVoucher.Text = "Payment Voucher   (" + dr["FLDPAYMENTVOUCHERNUMBER"].ToString() + ")     ";
                    if (dr["FLDSUBTYPE"].ToString() == "1328")
                        lblSupplier.Text = "Employee";
                    if (dr["FLDTYPE"].ToString() == "1565")
                    {
                        lblSupplier.Text = "Employee ID/Name";
                        lblCurrency.Text = "Payable Currecny";
                        txtAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDAMOUNT"])); ;
                        txtPurpose.Text = dr["FLDPURPOSE"].ToString();
                        txtVisitDate.Text = dr["FLDDATE"].ToString();
                        lblPayableAmount.Visible = true;
                        txtAmount.Visible = true;
                        lblPurpose.Visible = true;
                        lblVisitDate.Visible = true;
                        txtPurpose.Visible = true;
                        txtVisitDate.Visible = true;
                    }
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
