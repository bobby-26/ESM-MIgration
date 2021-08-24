using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class AccountsCompanyAccountMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar ptAccountMasterTabs = new PhoenixToolbar();
        ptAccountMasterTabs.AddButton("Accounts", "ACCOUNTS");
        ptAccountMasterTabs.AddButton("Sub Account", "SUBACCOUNT");

        cmdHiddenSubmit.Attributes.Add("style", "Display:None;");

        MenuAccountMaster.AccessRights = this.ViewState;
        MenuAccountMaster.MenuList = ptAccountMasterTabs.Show();
        //   MenuAccountMaster.SetTrigger(pnlAccountMaster);
        MenuAccountMaster.SelectedMenuIndex = 0;   //Accounts
        ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCompanyAccount.aspx";
        if (!IsPostBack)
        {
            ViewState["USAGE"] = null;
            Session["ACCOUNTID"] = "0";


        }
        EditAccount();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    private void EditAccount()
    {
        DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(Session["ACCOUNTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["USAGE"] = dr["FLDACCOUNTUSAGE"].ToString();
            ViewState["ACCOUNTLEVEL"] = dr["FLDACCOUNTLEVEL"].ToString();
        }
    }

    protected void AccountMaster_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ACCOUNTS"))
        {
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCompanyAccount.aspx";
        }
        if (CommandName.ToUpper().Equals("SUBACCOUNT"))
        {
            if (ViewState["USAGE"] != null && ViewState["ACCOUNTLEVEL"] != null)
            {
                if (ViewState["ACCOUNTLEVEL"].ToString() == "4" && (ViewState["USAGE"].ToString() == "81"))
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsUsageVessel.aspx";
                    MenuAccountMaster.SelectedMenuIndex = 1;
                }

                else if (ViewState["ACCOUNTLEVEL"].ToString() == "4" && ViewState["USAGE"].ToString() == "80")
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsUsageSupplier.aspx";
                    MenuAccountMaster.SelectedMenuIndex = 1;
                }
                else if (ViewState["ACCOUNTLEVEL"].ToString() == "4" && ViewState["USAGE"].ToString() == "79")
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsUsageSubAccount.aspx";
                    MenuAccountMaster.SelectedMenuIndex = 1;
                }
                else if (ViewState["ACCOUNTLEVEL"].ToString() == "4" && ViewState["USAGE"].ToString() == "77")
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsUsageCustomer.aspx";
                    MenuAccountMaster.SelectedMenuIndex = 1;

                }
                else if (ViewState["ACCOUNTLEVEL"].ToString() == "4" && ViewState["USAGE"].ToString() == "335")
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsUsageBank.aspx";
                    MenuAccountMaster.SelectedMenuIndex = 1;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCompanyAccount.aspx";
                    MenuAccountMaster.SelectedMenuIndex = 0;

                    ucError.ErrorMessage = "Please select bank (or) Subaccount";
                    ucError.Visible = true;
                    return;
                }
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsCompanyAccount.aspx";
                MenuAccountMaster.SelectedMenuIndex = 0;

                ucError.ErrorMessage = "Please select bank (or) Subaccount";
                ucError.Visible = true;
                return;
            }
        }
    }
}
