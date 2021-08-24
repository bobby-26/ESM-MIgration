using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class VesselAccounts_VesselAccountsPhoneCardIssueFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO");
        MenuOfficeFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {

            txtRefNo.Focus();
        }
    }
    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtRefNo", txtRefNo.Text.Trim());
            criteria.Add("txtCardNo", txtCardNo.Text.Trim());
            criteria.Add("ddlEmployee", ddlEmployee.SelectedEmployee);
            criteria.Add("txtFromDate", txtFromDate.Text);
            criteria.Add("txtToDate", txtToDate.Text);
            criteria.Add("ddlStatus", ddlStatus.SelectedPhoneCard);
            Filter.CurrentVesselPhoneCardIssueFilter = criteria;
        }
        Response.Redirect("VesselAccountsPhoneCardIssueApproval.aspx", false);
    }
}

