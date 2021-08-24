using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class AccountsInvoiceMoreInfo : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            Menutab.Title = "Invoice More Info";
            Menutab.AccessRights = this.ViewState;
            Menutab.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["invoiceCode"] = Request.QueryString["invoiceCode"];
                BindData();
            }
        }
        
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void BindData()
    {
        DataSet ds = PhoenixAccountsInvoice.InvoiceListMoreInfo(new Guid(ViewState["invoiceCode"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lblPOList.Text = dr["FLDPOLIST"].ToString();
            lblvesselList.Text = dr["FLDVESSELLIST"].ToString();
            if (dr["FLDSUPPLIERCURRENCYMISMATCH"].ToString() == "2" || dr["FLDSUPPLIERCURRENCYMISMATCH"].ToString() == "3")
                lblMismatch.Text = "Yes";
            else
                lblMismatch.Text = "No";

            if (dr["FLDSUPPLIERCURRENCYMISMATCH"].ToString() == "1" || dr["FLDSUPPLIERCURRENCYMISMATCH"].ToString() == "3")
                lblCurrencyMismatch.Text = "Yes";
            else
                lblCurrencyMismatch.Text = "No";

            lblPurchaserName.Text = dr["FLDPURCHASERNAME"].ToString();
            lblPurchaserUserName.Text = dr["FLDPURCHASERUSERNAME"].ToString();
            lblSuperintendentName.Text = dr["FLDPURCHASESUPDTNAME"].ToString();
            lblSuperintendentUserName.Text = dr["FLDPURCHASESUPDTUSERNAME"].ToString();
            lblPOCount.Text = dr["FLDPOCOUNT"].ToString();
        }
    }
}
