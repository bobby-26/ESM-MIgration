using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class AccountsAirfarePaymentVoucherSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        //if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            toolbar.AddButton("Cancel", "CANCEL");
            MenuFilterMain.AccessRights = this.ViewState;
            MenuFilterMain.MenuList = toolbar.Show();
            txtInvoiceNumber.Focus();
        }
    }

    protected void MenuFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        NameValueCollection criteria = new NameValueCollection();

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();
            criteria.Add("txtInvoiceNumber", txtInvoiceNumber.Text.Trim());
            criteria.Add("txtVessel", txtVessel.Text.Trim());
            criteria.Add("txtRangeFrom", txtRangeFrom.Text);
            criteria.Add("txtRangeTo", txtRangeTo.Text);
            criteria.Add("txtFromDate", txtFromDate.Text);
            criteria.Add("txtToDate", txtToDate.Text);
            Filter.CurrentInvoiceSelection = criteria;
        }
        else if (dce.CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentInvoiceSelection = criteria;
        }
        Session["New"] = "Y";
        string script = "javascript:fnReloadList('codehelp1');";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }
}
