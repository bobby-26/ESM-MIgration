using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsVesselVisitMoneyChanger : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Reset All", "RESET", ToolBarDirection.Right);
            toolbar.AddButton("Add", "ADD", ToolBarDirection.Right);

            MenuMoneyChanger.AccessRights = this.ViewState;
            MenuMoneyChanger.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ViewState["VisitId"] = Request.QueryString["visitId"];
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuMoneyChanger_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(ddlPaidCurrency.SelectedCurrency, txtPaidAmount.Text, ddlReceivedCurrency.SelectedCurrency, txtReceivedAmount.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsVesselVIistTravelClaimRegister.MoneyChangerInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["VisitId"].ToString())
                    , int.Parse(ddlPaidCurrency.SelectedCurrency)
                    , Convert.ToDecimal(txtPaidAmount.Text)
                    , int.Parse(ddlReceivedCurrency.SelectedCurrency)
                    , Convert.ToDecimal(txtReceivedAmount.Text));

                string script = "javascript:fnReloadList('codehelp1');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (CommandName.ToUpper().Equals("RESET"))
            {
                PhoenixAccountsVesselVIistTravelClaimRegister.MoneyChangerResetAll(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["VisitId"].ToString()));

                string script = "javascript:fnReloadList('codehelp1');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidData(string paidCurrency, string paidAmount, string receivedCurrency, string receivedAmount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (paidCurrency.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Paid curency is required.";
        if (paidAmount.Trim().Equals(""))
            ucError.ErrorMessage = "Paid amount is required.";
        if (receivedCurrency.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Received curency is required.";
        if (receivedAmount.Trim().Equals(""))
            ucError.ErrorMessage = "Received amount is required.";
        return (!ucError.IsError);

    }
}
