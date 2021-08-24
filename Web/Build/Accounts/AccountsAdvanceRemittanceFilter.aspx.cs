using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class AccountsAdvanceRemittanceFilter : PhoenixBasePage
{
    public int iCompanyCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            MenuOfficeFilterMain.AccessRights = this.ViewState;
            MenuOfficeFilterMain.MenuList = toolbar.Show();
            txtRemittenceNumberSearch.Focus();
            iCompanyCode = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        NameValueCollection criteria = new NameValueCollection();

        if (dce.CommandName.ToUpper().Equals("GO"))
        {          
            criteria.Clear();
            criteria.Add("txtRemittenceNumberSearch", txtRemittenceNumberSearch.Text.Trim());
            criteria.Add("ddlAccountCode", ddlBankAccount.SelectedBankAccount);
            criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
            criteria.Add("txtVoucherFromdateSearch", txtRemittenceFromdateSearch.Text);
            criteria.Add("txtVoucherTodateSearch", txtRemittenceTodateSearch.Text);

            Filter.CurrentRemittenceSelection = criteria;
        }
        Response.Redirect("../Accounts/AccountsAdvanceRemittance.aspx", false);       
    }
}



