using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Accounts_AccountDepositFilter : PhoenixBasePage
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
            txtDepositNumber.Focus();
            //ucPaymentStatus.HardTypeCode = "119";         
            
            //if (Request.QueryString["status"].ToString() != "")
            //    ViewState["status"] = int.Parse(Request.QueryString["status"].ToString());

            //ucInvoiceStatus.HardTypeCode = ((int)PhoenixHardTypeCode.INVOICESTATUS).ToString();
        }
        ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx', true); ");
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
            criteria.Add("txtDepositNumber", txtDepositNumber.Text.Trim());            
            criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
            criteria.Add("txtDepositFromdate", txtDepositFromdate.Text);
            criteria.Add("txtDepositTodate", txtDepositTodate.Text);
            criteria.Add("ddlDepositStatus", ddlDepositStatus.SelectedValue);            
            criteria.Add("txtSupplierCode", txtVendorId.Text.Trim());
            Filter.CurrentAdvancePaymentSelection = criteria;

            Response.Redirect("../Accounts/AccountsDepositList.aspx", false);
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentAdvancePaymentSelection = criteria;
            Response.Redirect("../Accounts/AccountsDepositList.aspx", false);
        }
    }
}
