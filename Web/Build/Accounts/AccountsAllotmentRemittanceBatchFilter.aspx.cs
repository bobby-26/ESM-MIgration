using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceBatchFilter : PhoenixBasePage
{
    public int iCompanyCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {

            txtRemittenceBatchNumberSearch.Focus();
            iCompanyCode = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyCode, 1);
            //ucRemittanceStatus.HardTypeCode = ((int)PhoenixHardTypeCode.REMITTANCESTATUS).ToString();           
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
            criteria.Add("txtRemittenceBatchNumberSearch", txtRemittenceBatchNumberSearch.Text.Trim());
            criteria.Add("ddlAccountCode", ddlBankAccount.SelectedBankAccount);
            criteria.Add("ddlPaymentmode", ddlPaymentmode.SelectedHard);
            criteria.Add("txtPaymentFromdateSearch", txtPaymentFromdateSearch.Text);
            criteria.Add("txtPaymentTodateSearch", txtPaymentTodateSearch.Text);
            //criteria.Add("ucRemittanceStatus", ucRemittanceStatus.SelectedHard);
            Filter.CurrentAllotmentRemittenceBatchSelection = criteria;
        }
        Response.Redirect("../Accounts/AccountsAllotmentRemittanceBatchList.aspx", false);
    }
}



