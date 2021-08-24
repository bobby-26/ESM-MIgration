using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsAdvancePaymentVoucherFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();        
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            txtVoucherNumberSearch.Focus();
          //  ucPaymentStatus.HardTypeCode = "119";
        }
        ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=183,138,334,131', true); ");
        txtVendorId.Attributes.Add("style", "display:none");
    }
    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        NameValueCollection criteria = new NameValueCollection();

        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();
            criteria.Add("txtVoucherNumberSearch", txtVoucherNumberSearch.Text.Trim());
            criteria.Add("txtMakerId", txtMakerId.Text.Trim());
            criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
            criteria.Add("txtVoucherFromdateSearch", txtVoucherFromdateSearch.Text);
            criteria.Add("txtVoucherTodateSearch", txtVoucherTodateSearch.Text);
            criteria.Add("txtSupplierCode", txtVendorId.Text.Trim());
            Filter.CurrentAdvancePaymentVoucherSelection = criteria;
            Response.Redirect("../Accounts/AccountsAdvancePaymentVoucher.aspx", false);
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentAdvancePaymentVoucherSelection = criteria;
            Response.Redirect("../Accounts/AccountsAdvancePaymentVoucher.aspx", false);
        }        
    }   
}  
