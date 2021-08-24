using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;
public partial class RegistersAccountMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["USAGE"] = null;
          //  Session["ACCOUNTID"] = "0";

        }
        PhoenixToolbar ptAccountMasterTabs = new PhoenixToolbar();
        ptAccountMasterTabs.AddButton("Accounts", "ACCOUNTS", ToolBarDirection.Left);
        ptAccountMasterTabs.AddButton("Sub Account", "SUBACCOUNT", ToolBarDirection.Left);
        ptAccountMasterTabs.AddButton("History", "HISTORY", ToolBarDirection.Left);
        MenuAccountMaster.AccessRights = this.ViewState;
        MenuAccountMaster.MenuList = ptAccountMasterTabs.Show();
        //MenuAccountMaster.SetTrigger(pnlAccountMaster);
        MenuAccountMaster.SelectedMenuIndex = 0;   //Accounts
        ifMoreInfo.Attributes["src"] = "../Registers/RegistersAccount.aspx";

        EditAccount();
    }

    private void EditAccount()
    {
        int? ACCOUNTID=General.GetNullableInteger(Convert.ToString(Session["ACCOUNTID"]));
        if (General.GetNullableInteger(Convert.ToString(Session["ACCOUNTID"]))==null)
        {
            Session["ACCOUNTID"] = 0;
        }

        DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(Convert.ToString(Session["ACCOUNTID"])));


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
            ifMoreInfo.Attributes["src"] = "../Registers/RegistersAccount.aspx";
        }
        if (CommandName.ToUpper().Equals("SUBACCOUNT"))
        {
            MenuAccountMaster.SelectedMenuIndex = 1;
            if (ViewState["USAGE"] != null && ViewState["ACCOUNTLEVEL"] != null)
            {
                if (ViewState["ACCOUNTLEVEL"].ToString() == "4" && ViewState["USAGE"].ToString() == "81")
                {
                    ifMoreInfo.Attributes["src"] = "../Registers/RegistersUsageVessel.aspx";
                }
                else if (ViewState["ACCOUNTLEVEL"].ToString() == "4" && ViewState["USAGE"].ToString() == "80")
                {
                    ifMoreInfo.Attributes["src"] = "../Registers/RegistersUsageSupplier.aspx";
                }
                else if (ViewState["ACCOUNTLEVEL"].ToString() == "4" && ViewState["USAGE"].ToString() == "77")
                {
                    ifMoreInfo.Attributes["src"] = "../Registers/RegistersUsageCustomer.aspx";
                }
                else if (ViewState["ACCOUNTLEVEL"].ToString() == "4" && ViewState["USAGE"].ToString() == "750")
                {
                    ifMoreInfo.Attributes["src"] = "../Registers/RegistersAccountUsageVesselCosts.aspx";
                }
                else if (ViewState["ACCOUNTLEVEL"].ToString() == "4" && ViewState["USAGE"].ToString() == "1230")
                {
                    ifMoreInfo.Attributes["src"] = "../Registers/RegistersAccountUsageEmployee.aspx";
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "../Registers/RegistersAccount.aspx";
                    MenuAccountMaster.SelectedMenuIndex = 0;

                    ucError.HeaderMessage = "Navigation error";
                    ucError.ErrorMessage = "Please select Account Code that having usage as  'Supplier'or 'Customer' or 'Employee' or 'Vessel' or 'Vessel Costs'";
                    ucError.Visible = true;
                    return;
                }
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Registers/RegistersAccount.aspx";
                MenuAccountMaster.SelectedMenuIndex = 0;
                ucError.HeaderMessage = "Navigation error";
                ucError.ErrorMessage = "Please select a Ledger and navigate to Sub Account";
                ucError.Visible = true;
                return;
            }
        }
        if (CommandName.ToUpper().Equals("HISTORY"))
        {
            Response.Redirect("../Registers/RegistersAccountHistory.aspx");
        }
    }
}