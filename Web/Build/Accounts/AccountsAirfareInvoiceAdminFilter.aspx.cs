using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsAirfareInvoiceAdminFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            txtVendorId.Attributes.Add("style", "visibility:hidden");
            txtAccountId.Attributes.Add("style", "visibility:hidden");
            imgShowAccount.Attributes.Add("onclick", "return showPickList('spnPickListCreditAccount', 'codehelp1', '', '../Common/CommonPickListCompanyAccount.aspx', true); ");
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
            criteria.Add("txtrequisitionno", txtRequestNo.Text.Trim());
            criteria.Add("txtpassengername", txtPassengerName.Text.Trim());
            criteria.Add("txtticketno", txtTicketNo.Text);
            criteria.Add("txtdeparturefrom", ucDepartureFrom.Text == null ? string.Empty : ucDepartureFrom.Text);
            criteria.Add("txtdepartureto", ucDepartureTO.Text == null ? string.Empty : ucDepartureTO.Text);
            criteria.Add("txtarrivalfrom", ucArrivalFrom.Text == null ? string.Empty : ucArrivalFrom.Text);
            criteria.Add("txtarrivalto", ucArrivalTO.Text == null ? string.Empty : ucArrivalTO.Text);
            criteria.Add("txtinvoiceno", txtInvoiceNo.Text);
            criteria.Add("txtSupplierId", txtVendorId.Text);
            criteria.Add("txtAccountId", txtAccountId.Text);

            Filter.CurrentAirfareInvoiceAdminFilter = criteria;

        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {

        }
        if (Request.QueryString["Source"] != null && Request.QueryString["Source"].ToString() == "Register")
            Response.Redirect("../Accounts/AccountsAirfareInvoiceRegisterMaster.aspx", false);
        else
            Response.Redirect("../Accounts/AccountsAirfareInvoiceAdminMaster.aspx", false);
    }

}
