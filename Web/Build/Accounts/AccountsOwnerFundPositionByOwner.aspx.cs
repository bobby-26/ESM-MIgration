using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Accounts_AccountsOwnerFundPositionByOwner : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            DateTime now = DateTime.Now;
            txtFromDate.Text = now.Date.AddMonths(-0).ToShortDateString();
        }

        ucConfirmMessage.Visible = false;

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Owner", "OWNER", ToolBarDirection.Right);
        toolbarmain.AddButton("Summary", "SUMMARY",ToolBarDirection.Right);
     

        MenuMainFilter.AccessRights = this.ViewState;
        MenuMainFilter.MenuList = toolbarmain.Show();
        MenuMainFilter.SelectedMenuIndex = 0;


        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Generate", "GENERATE",ToolBarDirection.Right);
        MenuFinancialYearStatement.AccessRights = this.ViewState;
        MenuFinancialYearStatement.MenuList = toolbar1.Show();

    }

    protected void OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string principalId = (e.Item.FindControl("accountid") as HiddenField).Value;
            Repeater rptOrders = e.Item.FindControl("rptOrders") as Repeater;
            rptOrders.DataSource = PhoenixAccountsOwnerFundPosition.OwnerFundPositionByAccountWise(General.GetNullableDateTime(txtFromDate.Text), General.GetNullableInteger(ucOwner.SelectedAddress), General.GetNullableInteger(principalId), 2, null);
            rptOrders.DataBind();
        }
    }

    protected void rptOrders_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string principalId = (e.Item.FindControl("hfPrincipalId") as HiddenField).Value;
            string accountid = (e.Item.FindControl("accountid") as HiddenField).Value;
            Repeater rptOrders = e.Item.FindControl("rptOrders1") as Repeater;
            rptOrders.DataSource = PhoenixAccountsOwnerFundPosition.OwnerFundPositionByAccountWise(General.GetNullableDateTime(txtFromDate.Text), General.GetNullableInteger(ucOwner.SelectedAddress), General.GetNullableInteger(accountid), 3, General.GetNullableInteger(principalId));
            rptOrders.DataBind();
        }
    }

    protected void MenuMainFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SUMMARY"))
            {
                Response.Redirect("../Accounts/AccountsOwnerFundPosition.aspx", false);
            }

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuFinancialYearStatement_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GENERATE"))
            {

                ControlVisibility();

                rptCustomers.DataSource = PhoenixAccountsOwnerFundPosition.OwnerFundPositionByAccountWise(General.GetNullableDateTime(txtFromDate.Text), General.GetNullableInteger(ucOwner.SelectedAddress), General.GetNullableInteger(null), 1, General.GetNullableInteger(null));
                rptCustomers.DataBind();

            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void ControlVisibility()
    {
        rptCustomers.DataSource = null;
        rptCustomers.DataBind();
    }
}
