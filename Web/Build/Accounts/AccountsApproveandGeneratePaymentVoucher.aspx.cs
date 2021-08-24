using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;

public partial class AccountsApproveandGeneratePaymentVoucher : PhoenixBasePage
{
    string header = "Please provide the following required information", error = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            txtBudgetId.Attributes.Add("style", "visibility:hidden");
            txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");
            txtAccountId.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Generate PV", "APPROVEANDGENERATEDPV", ToolBarDirection.Right);
            toolbar.AddButton("List", "LIST", ToolBarDirection.Right);
            MenuVoucher.AccessRights = this.ViewState;
            MenuVoucher.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["pagenumber"] == null ? "1" : Request.QueryString["pagenumber"].ToString());
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ddlUnion.AddressList = PhoenixRegistersAddress.ListAddress("135");

                ddlCurrency.CurrencyList = PhoenixRegistersCurrency.ListAllActiveCurrency(1);
                if (Request.QueryString["Union"] != null)
                    ddlUnion.SelectedAddress = Request.QueryString["Union"].ToString();

                if (Request.QueryString["AllotmentIdList"] != null && Request.QueryString["AllotmentIdList"] != string.Empty)
                {
                    ViewState["AllotmentIdList"] = Request.QueryString["AllotmentIdList"].ToString();
                }

                if (Request.QueryString["AMOUNT"] != null && Request.QueryString["AMOUNT"] != string.Empty)
                {
                    txtTotalAmount.Text = Request.QueryString["AMOUNT"].ToString();
                }

                if (Request.QueryString["COUNT"] != null && Request.QueryString["COUNT"] != string.Empty)
                {
                    txtNoofTransaction.Text = Request.QueryString["COUNT"].ToString();
                }
                if (Request.QueryString["CURRENCYID"] != null && Request.QueryString["CURRENCYID"] != string.Empty)
                {
                    ddlCurrency.Enabled = false;
                    ddlCurrency.SelectedCurrency = Request.QueryString["CURRENCYID"].ToString();
                }


                // txtCompany.Text = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
                txtCompany.Text = PhoenixSecurityContext.CurrentSecurityContext.UserDefaultCompanyName;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuVoucher_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("APPROVEANDGENERATEDPV"))
            {

                    string union = ddlUnion.SelectedAddress;
                    string RAbankaccount = ddlAgentBankAccount.SelectedBankAccount;
                    string agentcharges = ddlCurrency.SelectedCurrency;
                    string amount = txtAmount.Text;
                    string ownerbudgetcode = ddlBudgetGroup.SelectedValue;
                    string account = txtAccountId.Text;


                    if (IsValidPVGenerate(union, RAbankaccount, agentcharges, amount, ownerbudgetcode, account, ref header, ref error))
                    {
                        RadWindowManager1.RadAlert(error, 400, 175, header, null);

                        return;
                    }

                    PhoenixAccountsAllotment.AllotmentRAPVGenerate(int.Parse(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString()),
                                                                        (ViewState["AllotmentIdList"].ToString()).Length > 1 ? (ViewState["AllotmentIdList"].ToString()) : null,
                                                                        General.GetNullableString(txtPaymentvoucherremarks.Text),
                                                                        int.Parse(ddlUnion.SelectedAddress),
                                                                        int.Parse(ddlAgentBankAccount.SelectedBankAccount),
                                                                        int.Parse(ddlCurrency.SelectedCurrency),
                                                                        decimal.Parse(txtAmount.Text),
                                                                        int.Parse(txtAccountId.Text),
                                                                        txtAccountCode.Text,
                                                                        General.GetNullableInteger(txtBudgetId.Text),
                                                                        General.GetNullableString(txtBudgetCode.Text),
                                                                        new Guid(ddlBudgetGroup.SelectedValue),
                                                                        ddlBudgetGroup.SelectedText
                                                                        );



            }

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Accounts/AccountsAllotmentList.aspx?pageno=" + ViewState["PAGENUMBER"], false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ddlUnion_Changed(object sender, EventArgs e)
    {
        ddlAgentBankAccount.SelectedBankAccount = "";
        ddlAgentBankAccount.AddressCode = ddlUnion.SelectedAddress;
        ddlAgentBankAccount.AgentBankAccountList = PhoenixVesselAccountsEmployeeBankAccount.ListAgentBankAccount(int.Parse(ddlUnion.SelectedAddress));
        ddlAgentBankAccount.DataBind();
    }
    protected void ddlAgentBankAccount_TextChangedEvent(object o, EventArgs e)
    {
        ddlAgentBankAccount.DataBind();
    }
    protected void txtAccountCode_changed(object sender, EventArgs e)
    {


        DataSet ds = PhoenixRegistersAccount.EditCompanyAccount(Convert.ToInt32(txtAccountId.Text.ToString()), PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        DataTable dt = ds.Tables[0];
        DataSet dsprincipal = PhoenixRegistersAccount.EditAccount(Convert.ToInt32(txtAccountId.Text));
        if (dsprincipal.Tables[0].Rows.Count > 0)
        {
            DataRow drprincipal = dsprincipal.Tables[0].Rows[0];
            ViewState["PRINCIPALID"] = drprincipal["FLDPRINCIPALID"];
        }


    }
    private bool IsValidPVGenerate(string union, string RAbankaccount, string agentcharges, string amount, string ownerbudgetcode, string account, ref string headermessage, ref string errormessage)
    {

        errormessage = "";


        if ((General.GetNullableInteger(union) == null))
            errormessage = errormessage + "Remitting agent is required.</br>";


        if ((General.GetNullableInteger(RAbankaccount) == null))
            errormessage = errormessage + "Remitting agent bank account is required.</br>";

        if ((General.GetNullableInteger(agentcharges) == null))
            errormessage = errormessage + "Agent charges is required.</br>";

        if (string.IsNullOrEmpty(amount))
            errormessage = errormessage + "Amount is required.</br>";

        if ((General.GetNullableGuid(ownerbudgetcode) == null))
            errormessage = errormessage + "Owner budget code is required.</br>";

        if (string.IsNullOrEmpty(account))
            errormessage = errormessage + "account is required.</br>";


        if (!errormessage.Length.Equals(0))
        {
            return true;
        }
        else
            return false;

        //  return (!ucError.IsError);
    }
    //protected void txtBudgetId_Changed(object sender, EventArgs e)
    //{
    //    //  txtAccountCode1.Text = "";
    //    //  txtownerbudgetedit.Text = "";
    //    int iRowCount = 0;
    //    int iTotalPageCount = 0;

    //    //   txtAccountCode1.Text = "";

    //    DataSet dsBudget = PhoenixCommonRegisters.SubAccountSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
    //           , General.GetNullableInteger(txtAccountId.Text)
    //           , General.GetNullableInteger(txtAccountUsage.Text)
    //           , General.GetNullableString(txtBudgetCode.Text)
    //           , null
    //           , null, null
    //           , 1, General.ShowRecords(null),
    //           ref iRowCount,
    //           ref iTotalPageCount
    //           );

    //    //  if (dsBudget.Tables[0].Rows.Count > 0)
    //    //      lblbudgetid.Text = dsBudget.Tables[0].Rows[0]["FLDBUDGETID"].ToString();
    //    //
    //    //  imgShowAccount1.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?&vesselid=" + null + "&Ownerid=" + ViewState["PRINCIPALID"] + "&budgetid=" + lblbudgetid.Text + "', true); ");
    //}
}