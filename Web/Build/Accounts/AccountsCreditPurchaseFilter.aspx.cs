using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class AccountsCreditPurchaseFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //  if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
            MenuOfficeFilterMain.AccessRights = this.ViewState;
            MenuOfficeFilterMain.MenuList = toolbar.Show();
        }
        txtVendorId.Attributes.Add("style", "visibility:hidden");
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
       
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            NameValueCollection criteria = new NameValueCollection();

            if (CommandName.ToUpper().Equals("GO"))
            {
                criteria.Clear();
                criteria.Add("txtOrderNo", txtOrderNo.Text.Trim());
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("FromDate", FromDate.Text);
                criteria.Add("ToDate", ToDate.Text);
                criteria.Add("txtVendorId", txtVendorId.Text);
                criteria.Add("ucCurrency", ucCurrency.SelectedCurrency);
                Filter.CurrentCreditPurchaseFilter = criteria;
                if (Request.QueryString["committedyn"].ToString() == "0")
                    Response.Redirect("../Accounts/AccountsCreditPurchaseOfBondProvisions.aspx", false);
                if (Request.QueryString["committedyn"].ToString() == "1")
                    Response.Redirect("../Accounts/AccountsCreditPurchaseOfBondProvisionsCommitted.aspx", false);
            }
            else if (CommandName.ToUpper().Equals("CANCEL"))
            {
                criteria.Clear();
                Filter.CurrentCreditPurchaseFilter = null;
                if (Request.QueryString["committedyn"].ToString() == "0")
                    Response.Redirect("../Accounts/AccountsCreditPurchaseOfBondProvisions.aspx", false);
                if (Request.QueryString["committedyn"].ToString() == "1")
                    Response.Redirect("../Accounts/AccountsCreditPurchaseOfBondProvisionsCommitted.aspx", false);
            }
       

    }
}
