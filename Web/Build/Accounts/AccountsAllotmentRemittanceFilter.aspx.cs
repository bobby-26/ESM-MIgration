using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceFilter : PhoenixBasePage
{
    public int iCompanyCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState); PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();
        txtRemittenceNumberSearch.Focus();
        if (!IsPostBack)
        {

            iCompanyCode = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            ucRemittanceStatus.HardTypeCode = ((int)PhoenixHardTypeCode.REMITTANCESTATUS).ToString();

        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();
            criteria.Add("txtRemittenceNumberSearch", txtRemittenceNumberSearch.Text.Trim());
            criteria.Add("txtName", txtName.Text.Trim());
            criteria.Add("txtFileNumber", txtFileNumber.Text.Trim());
            criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
            criteria.Add("txtVoucherFromdateSearch", txtRemittenceFromdateSearch.Text);
            criteria.Add("txtVoucherTodateSearch", txtRemittenceTodateSearch.Text);
            criteria.Add("ucRemittanceStatus", ucRemittanceStatus.SelectedHard);
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
            criteria.Add("ddlPaymentmode", ddlPaymentmode.SelectedHard);
            criteria.Add("ddlPrincipal", ucPrincipal.SelectedAddress);
            criteria.Add("ddlBankAccount", ddlBankAccount.SelectedBankAccount);

            Filter.CurrentAllotmentRemittenceSelection = criteria;
        }

        Response.Redirect("../Accounts/AccountsAllotmentRemittanceMaster.aspx", false);
    }
}



