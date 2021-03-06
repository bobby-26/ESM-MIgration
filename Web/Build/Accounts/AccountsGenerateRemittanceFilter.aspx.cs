using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;



public partial class Accounts_AccountsGenerateRemittanceFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);


        txtMakerId.Attributes.Add("style", "visibility:hidden");

        txtMakerCode.Attributes.Add("onkeydown", "return false;");
        txtMakerName.Attributes.Add("onkeydown", "return false;");

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.Title = "Generate Remittance Filter";
        MenuOfficeFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            txtVoucherNumberSearch.Focus();

            ddlVoucherStatus.Enabled = false;
            ddlVoucherStatus.SelectedHard = "48";
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
            criteria.Add("txtVoucherNumberSearch", txtVoucherNumberSearch.Text.Trim());
            criteria.Add("txtMakerId", txtMakerId.Text);
            criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
            criteria.Add("txtVoucherFromdateSearch", txtVoucherFromdateSearch.Text);
            criteria.Add("txtVoucherTodateSearch", txtVoucherTodateSearch.Text);
            if (chkShowRemittancenotGenerated.Checked == true)
            {
                criteria.Add("chkShowRemittancenotGenerated", "0");
            }
            else
            {
                criteria.Add("chkShowRemittancenotGenerated", "1");
            }
            criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            criteria.Add("ddlVoucherStatus", ddlVoucherStatus.SelectedHard);
            criteria.Add("txtInvoiceNumber", txtInvoiceNumber.Text.Trim());
            criteria.Add("txtPurchaseInvoiceVoucherNumber", txtPurchaseInvoiceVoucherNumber.Text.Trim());
            criteria.Add("ddlSource", ddlSource.SelectedHard);
            criteria.Add("ddlType", ddlType.SelectedHard);
            criteria.Add("ddlAllocationStatus", ddlAllocationStatus.SelectedValue);
            if (chkReportNotTaken.Checked == true)
            {
                criteria.Add("chkReportNotTaken", "1");
            }
            else
            {
                criteria.Add("chkReportNotTaken", "null");
            }
            criteria.Add("txtuserid", txtuserid.Text);

            Filter.CurrentGenerateRemittenceFilterSelection = criteria;

            Response.Redirect("../Accounts/AccountsRemittanceGenerate.aspx", false);
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Response.Redirect("../Accounts/AccountsRemittanceGenerate.aspx", false);
        }
    }
    protected void txtVoucherNumberSearch_TextChanged(object sender, EventArgs e)
    {
        string test = txtVoucherNumberSearch.Text;
        //string yourString = test.Replace("PMV", ",PMV");
        string yourString = test.Replace(Environment.NewLine, ",");
        txtVoucherNumberSearch.Text = yourString.ToString();
    }
}
