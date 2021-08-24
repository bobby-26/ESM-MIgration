using System;
using System.Data;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsAirfareInvoiceRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            txtVendorId.Attributes.Add("style", "Visibility:hidden");

            if (!IsPostBack)
            {
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
                {
                    ViewState["ID"] = Request.QueryString["ID"];
                    AirfareInvoiceAdminEdit();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true; ;
        }

    }
    protected void AirfareInvoiceAdminEdit()
    {
        if (General.GetNullableInteger(ViewState["ID"].ToString()) != null)
        {
            DataSet ds = PhoenixAccountsAirfareInvoiceAdmin.AirfareInvoiceRegisterEdit(General.GetNullableInteger(ViewState["ID"].ToString()));


            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtRequestNo.Text = dr["FLDREQUISITIONNO"].ToString();
                txtPassengerName.Text = dr["FLDPASSENGERNAME"].ToString();
                txtTicketNo.Text = dr["FLDTICKETNO"].ToString();
                txtOrigin.Text = dr["FLDORIGIN"].ToString();
                ucDepartureDate.Text = dr["FLDDEPARTUREDATE"].ToString();
                ucArrivalDate.Text = dr["FLDARRIVALDATE"].ToString();
                txtDestination.Text = dr["FLDDESTINATION"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                txtInvoiceNumber.Text = dr["FLDINVOICENO"].ToString();
                txtVendorCode.Text = dr["FLDSUPPLIERCODE"].ToString();
                txtVenderName.Text = dr["FLDSUPPLIERNAME"].ToString();
                txtVendorId.Text = dr["FLDSUPPLIERID"].ToString();
                txtChargedAmount.Text = dr["FLDCHARGEDAMOUNT"].ToString();
                txtBillTo.Text = dr["FLDBILLTOCOMPANY"].ToString();
                txtAccountsVouchers.Text = dr["FLDACCOUNTSVOUCHERS"].ToString();
            }
        }
    }
}
