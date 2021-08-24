using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AccountsAllotmentRemittanceRejectionFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO");
        toolbar.AddButton("Cancel", "CANCEL");

        MenuRemittanceRejectionFilter.AccessRights = this.ViewState;
        MenuRemittanceRejectionFilter.MenuList = toolbar.Show();

    }

    protected void MenuRemittanceRejectionFilter_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        NameValueCollection criteria = new NameValueCollection();

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            //criteria.Clear();
            criteria.Add("txtRemittanceNumberSearch", txtRemittanceNumber.Text.Trim());
            criteria.Add("txtName", txtName.Text.Trim());
            criteria.Add("txtFileNumber", txtFileNumber.Text.Trim());
            criteria.Add("txtFromdateSearch", txtFromdateSearch.Text);
            criteria.Add("txtTodateSearch", txtTodateSearch.Text);
            criteria.Add("chkActiveYN", chkActiveYN.Checked == true ? "1" : "0");
            criteria.Add("ddlRejectionReason", ddlRejectionReason.SelectedQuick);
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);


            Filter.CurrentAllotmentRemittanceRejectionFilter = criteria;
        }
        else if (dce.CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
        }
        Response.Redirect("../Accounts/AccountsAllotmentRemittanceRejection.aspx", false);
    }
}