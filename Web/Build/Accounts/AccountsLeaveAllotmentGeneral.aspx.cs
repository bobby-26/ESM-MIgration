using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsLeaveAllotmentGeneral : PhoenixBasePage
{
    private string empid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Leave Allotment Requests", "LEAVEALLOTMENT");
        toolbarmain.AddButton("Balance Breakdown by Contracts", "BALANCEBREAKDOWN");
        LeaveAllotment.AccessRights = this.ViewState;
        LeaveAllotment.MenuList = toolbarmain.Show();
        LeaveAllotment.SelectedMenuIndex = 0;
        txtHidden.Style.Add("display", "none");
        cmdHiddenSubmit.Style.Add("display", "none");
        ifMoreInfo.Attributes["src"] = "../Accounts/AccountsLeaveBalance.aspx";
    }

    protected void LeaveAllotment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("LEAVEALLOTMENT"))
        {
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsLeaveBalance.aspx";
        }
        else if (CommandName.ToUpper().Equals("BALANCEBREAKDOWN"))
        {
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsLeaveBalanceBreakDown.aspx";
            LeaveAllotment.SelectedMenuIndex = 1;
        }
    }
    protected void cmdHiddenSubmit_OnClick(object sender, EventArgs e)
    {
        LeaveAllotment.SelectedMenuIndex = 1;
        ifMoreInfo.Attributes["src"] = "../Accounts/AccountsLeaveBalanceBreakDown.aspx?empid=" + txtHidden.Text;
    }
}
