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

public partial class Accounts_AccountsOwnerFundPositionDataPopulate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            DateTime now = DateTime.Now;
            txtFromDate.Text = now.Date.AddMonths(-0).ToShortDateString();
        }

        ucConfirmMessage.Visible = false;

        //PhoenixToolbar toolbarmain = new PhoenixToolbar();
        //toolbarmain.AddButton("Summary", "SUMMARY");
        //toolbarmain.AddButton("Owner", "OWNER");

        //MenuMainFilter.AccessRights = this.ViewState;
        //MenuMainFilter.MenuList = toolbarmain.Show();
        //MenuMainFilter.SelectedMenuIndex = 1;


        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Populate", "GENERATE");
        MenuFinancialYearStatement.AccessRights = this.ViewState;
        MenuFinancialYearStatement.MenuList = toolbar1.Show();

    }

    protected void MenuFinancialYearStatement_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("GENERATE"))
            {

                PhoenixAccountsOwnerFundPositionPopulate.OwnerFundPositionDataPopulate(
                        General.GetNullableDateTime(txtFromDate.Text), General.GetNullableInteger(ucOwner.SelectedAddress)
                        );

                ucStatus.Text = "Successfully populated";
                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                Session["New"] = "Y";
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

}
