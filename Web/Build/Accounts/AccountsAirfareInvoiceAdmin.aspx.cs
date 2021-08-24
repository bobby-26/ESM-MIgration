using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
public partial class AccountsAirfareInvoiceAdmin : PhoenixBasePage
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
            DataSet ds = PhoenixAccountsAirfareInvoiceAdmin.AirfareInvoiceAdminEdit(General.GetNullableInteger(ViewState["ID"].ToString()));


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
                txtAmountPayable.Text = dr["FLDAMOUNTPAYABLE"].ToString();
                txtChargedAmount.Text = dr["FLDCHARGEDAMOUNT"].ToString();
                txtBillTo.Text = dr["FLDBILLTOCOMPANY"].ToString();
                txtAccountsVouchers.Text = dr["FLDACCOUNTSVOUCHERS"].ToString();
            }
        }
    }
    
}
