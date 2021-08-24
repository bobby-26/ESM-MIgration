using System;
using System.Data;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsCreditPurchaseMoreInfo : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);                   
            ViewState["ORDERID"] = null;
            if (Request.QueryString["orderid"] != null && !string.IsNullOrEmpty(Request.QueryString["orderid"].ToString()))
            {
                ViewState["ORDERID"] = Request.QueryString["orderid"];
            }
            setMoreInfo();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void setMoreInfo()
    {
        DataTable dt = PhoenixAccountsVesselAccounting.CreditPurchaseMoreInfoSearch(new Guid(ViewState["ORDERID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            txtEmail.Text = dt.Rows[0]["FLDEMAIL"].ToString();
            txtExchangeRate.Text = dt.Rows[0]["FLDEXCHANGERATE"].ToString();
            txtTotalAmount.Text = dt.Rows[0]["FLDTOTALAMOUNTUSD"].ToString();
        }
    }
}
