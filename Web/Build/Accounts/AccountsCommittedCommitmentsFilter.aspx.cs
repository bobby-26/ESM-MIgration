using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsCommittedCommitmentsFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
            
            MenuOfficeFilterMain.AccessRights = this.ViewState;
            MenuOfficeFilterMain.MenuList = toolbar.Show();
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
            criteria.Add("ucVesselAccount", ucVesselAccount.SelectedVesselAccount);
            criteria.Add("txtPoNumber", txtPoNumber.Text);
            criteria.Add("ucCommittedDate", ucCommittedDate.Text);
            criteria.Add("chkreversed", chkReversed.Checked == true ? "1" : "0");
            Filter.CommitedCommitmentsFilter = criteria;
            Response.Redirect("../Accounts/AccountsCommitedCommitments.aspx", false);
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            Filter.CommitedCommitmentsFilter = null;
            Response.Redirect("../Accounts/AccountsCommitedCommitments.aspx", false);
        }
    }
}
