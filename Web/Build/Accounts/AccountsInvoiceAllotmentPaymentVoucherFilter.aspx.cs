using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Windows.Forms;
using Telerik.Web.UI;

public partial class AccountsInvoiceAllotmentPaymentVoucherFilter : PhoenixBasePage
{
    private const int WM_PASTE = 0x0302;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
      
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();


        txtVoucherNumberSearch.Focus();

    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            //criteria.Clear();
            criteria.Add("txtVoucherNumberSearch", txtVoucherNumberSearch.Text.Trim());
            criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
            criteria.Add("txtVoucherFromdateSearch", CalendarExtender1.Text);
            criteria.Add("txtVoucherTodateSearch", CalendarExtender2.Text);
            criteria.Add("txtname", txtName.Text);


            Filter.CurrentInvoiceAllotmentPaymentVoucherSelection = criteria;
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
        }
        Response.Redirect("../Accounts/AccountsAllotmentRemittanceGenerate.aspx", false);
    }

    protected void txtVoucherNumberSearch_TextChanged(object sender, EventArgs e)
    {
        string test = txtVoucherNumberSearch.Text;
        //string yourString = test.Replace("PMV", ",PMV");
        string yourString = test.Replace(Environment.NewLine, ",");
        txtVoucherNumberSearch.Text = yourString.ToString();
    }
}



