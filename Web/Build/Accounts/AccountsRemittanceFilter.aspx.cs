using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsRemittanceFilter : PhoenixBasePage
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
            txtRemittenceNumberSearch.Focus();
            iCompanyCode = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            ucRemittanceStatus.HardTypeCode = ((int)PhoenixHardTypeCode.REMITTANCESTATUS).ToString();
            txtVendorId.Attributes.Add("style", "display:none");
            ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyCode, 1);
            if (Request.QueryString["source"] != null)
            {
                ViewState["Source"] = Request.QueryString["source"];
                lblCaption.Text = "Cash Out Filter";
            }

            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx', true); ");
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
            criteria.Add("ddlAccountCode", ddlBankAccount.SelectedBankAccount);
            criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
            criteria.Add("txtVoucherFromdateSearch", txtRemittenceFromdateSearch.Text);
            criteria.Add("txtVoucherTodateSearch", txtRemittenceTodateSearch.Text);
            criteria.Add("ucRemittanceStatus", ucRemittanceStatus.SelectedHard);
            criteria.Add("txtSupplierCode", txtVendorId.Text.Trim());
            criteria.Add("chkIsZeroAmount", chkZeroAmount.Checked == true ? "1" : "0");
            criteria.Add("ddlPaymentmode", ddlPaymentmode.SelectedHard);
            criteria.Add("txtbatchno", txtbatchno.Text.Trim());
            criteria.Add("txtvouchernumber", txtvouchernumber.Text.Trim());

            Filter.CurrentRemittenceRequestSelection = criteria;
        }

        if (ViewState["Source"] != null)
        {
            if (ViewState["Source"].ToString() == "cashoutrequest")
                Response.Redirect("../Accounts/AccountsCashOutRequest.aspx", false);
        }
        else
            Response.Redirect("../Accounts/AccountsRemittanceMaster.aspx", false);
    }
}



