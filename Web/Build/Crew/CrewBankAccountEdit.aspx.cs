using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
public partial class CrewBankAccountEdit : PhoenixBasePage
{
    string id = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("Registered Bank Account", "REGISTEREDBANKACCOUNT", ToolBarDirection.Right);
        MenuCrewBankAccountList.AccessRights = this.ViewState;
        MenuCrewBankAccountList.MenuList = toolbar.Show();

        id = Request.QueryString["id"];
        if (!IsPostBack)
        {

            ViewState["ACCOUNTNOPATTERN"] = null;
            ViewState["EMPLOYEEID"] = null;
            if (!string.IsNullOrEmpty(id))
            {
                EditCrewBankAccount(new Guid(id));
            }
            else
                ResetCrewBankAccount();
        }
    }

    protected void EditCrewBankAccount(Guid crewbankaccountid)
    {
        DataTable dt = PhoenixCrewBankAccount.EditCrewBankAccount(crewbankaccountid);

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                txtVerifiedDate.CssClass = "input_mandatory";
                txtVerifiedDate.ReadOnly = false;
            }
            else
            {
                txtVerifiedDate.CssClass = "readonlytextbox";
                txtVerifiedDate.ReadOnly = true;
            }
            txtModeOfPayment.Text = dr["FLDMODEOFPAYMENTNAME"].ToString();
            txtIntermediateBankCountry.Text = dr["FLDINTERMEDIATEBANKCOUNTRYNAME"].ToString();
            txtRemittanceType.Text = dr["FLDTYPEOFREMITTANCENAME"].ToString();
            txtType.Text = dr["FLDACCOUNTTYPEDESC"].ToString();
            txtAccountName.Text = dr["FLDACCOUNTNAME"].ToString();
            txtIBANNumber.Text = dr["FLDIBANNUMBER"].ToString();
            txtIntermediateBank.Text = dr["FLDINTERMEDIATEBANK"].ToString();
            txtIntermediateBankAddres.Text = dr["FLDINTERMEDIATEBANKADDRESS"].ToString();
            txtIntermediateBankSwiftCode.Text = dr["FLDINTERMEDIATEBANKSWIFTCODE"].ToString();
            txtSeafarerBank.Text = dr["FLDBANKNAME"].ToString();
            ViewState["FLDACTIVEYN"] = dr["FLDACTIVEYN"].ToString();
            if (dr["FLDBANKID"].ToString() != "")
            {
                ViewState["ACCOUNTNOPATTERN"] = dr["FLDACCOUNTNOPATTERN"].ToString();
                Bankchange(dr["FLDBANKID"].ToString());
            }

            txtAccountNumber.Text = dr["FLDACCOUNTNUMBER"].ToString();
            txtSeafarerBankBranch.Text = dr["FLDBANKBRANCH"].ToString();
            txtSeafarerBankSortCode.Text = dr["FLDBANKSORTCODE"].ToString();
            txtSeafarerBankSwiftCode.Text = dr["FLDBANKSWIFTCODE"].ToString();
            txtPaymentPercentage.Text = dr["FLDPAYMENTPERCENTAGE"].ToString();

            txtAddressLine1.Text = dr["FLDADDRESS1"].ToString();
            txtAddressLine2.Text = dr["FLDADDRESS2"].ToString();
            txtAddressLine3.Text = dr["FLDADDRESS3"].ToString();
            txtAddressLine4.Text = dr["FLDADDRESS4"].ToString();
            txtCountry.Text = dr["FLDBANKCOUNTRYNAME"].ToString();
            txtstate.Text = dr["FLDSTATENAME"].ToString();
            txtCity.Text = dr["FLDCITYNAME"].ToString();
            txtPinCode.Text = dr["FLDZIPCODE"].ToString();
            txtCurrency.Text = dr["FLDBANKCURRENCYCODE"].ToString();
            ViewState["EMPLOYEEID"] = dr["FLDEMPLOYEEID"].ToString();
            txtIBankCurrency.Text = dr["FLDINTERMEDIATEBANKCURRENCYCODE"].ToString();
            txtaccountopenby.Text = dr["FLDACCONTOPENEDBYDESC"].ToString();
            txtBankIFSCCode.Text = dr["FLDBANKIFSCCODE"].ToString();
            txtIbankIFSCCode.Text = dr["FLDINTERMEDIATEBANKIFSCCODE"].ToString();
            txtIbankAccountName.Text = dr["FLDIBANKACCOUNTNAME"].ToString();
            txtIbankAccountNo.Text = dr["FLDIBANKACCOUNTNUMBER"].ToString();
            ucDefaultAccountType.SelectedQuick = dt.Rows[0]["FLDDEFAULTACCOUNTTYPE"].ToString();
            txtAllotmentAmount.Text = dt.Rows[0]["FLDALLOTMENTAMOUNT"].ToString();
            chkRemittingAgent.Checked = dt.Rows[0]["FLDISREMITTINGAGENTREQ"].ToString() == "1" ? true : false;
            ucRemittingAgent.SelectedAddress = dt.Rows[0]["FLDREMITTINGAGENTID"].ToString();
            ucDefaultAllotmentCurrency.SelectedCurrency = dt.Rows[0]["FLDDEFAULTALLOTMENTCURRENCY"].ToString();
            if (dr["FLDACTIVEYN"].ToString() == "0")
                chkInActiveYN.Checked = true;
            txtVerifiedDate.Text = dr["FLDVERIFIEDDATE"].ToString();
            txtVerifiedBy.Text = dr["FLDVERIFIEDBYNAME"].ToString();
            txtInActiveRemarks.Text = dr["FLDINACTIVEREMARKS"].ToString();

        }
    }

    protected void ResetCrewBankAccount()
    {
        txtAccountName.Text = "";
        txtAccountNumber.Text = "";
        txtIBANNumber.Text = "";
        txtIntermediateBank.Text = "";
        txtIntermediateBankAddres.Text = "";
        txtIntermediateBankSwiftCode.Text = "";
        txtSeafarerBank.Text = "";
        txtSeafarerBankBranch.Text = "";
        txtSeafarerBankSortCode.Text = "";
        txtSeafarerBankSwiftCode.Text = "";
        txtPaymentPercentage.Text = "";
        txtBankIFSCCode.Text = "";
        txtVerifiedDate.Text = "";
    }
    protected void UcBank_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void Bankchange(string bank)
    {
        {
            DataSet ds = PhoenixRegistersBank.ListBank(General.GetNullableInteger(bank), null);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["ACCOUNTNOPATTERN"] = ds.Tables[0].Rows[0]["FLDACCOUNTNOPATTERN"].ToString();

                txtSeafarerBankSortCode.Text = ds.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();
                txtSeafarerBank.Text = ds.Tables[0].Rows[0]["FLDNAME"].ToString();

            }

        }
    }
    protected void CrewBankAccountList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            string Script = "";
            Script += "<script language='JavaScript' id='BookMarkScript'>";
            Script += "fnReloadList();";
            Script += "</script>";
            String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
            String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidCrewBankAccount())
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        PheonixAccountCrewBankAccount.UpdateBankdetail(new Guid(id)
                            , DateTime.Parse(txtVerifiedDate.Text), int.Parse(ViewState["EMPLOYEEID"].ToString())
                            , General.GetNullableString(txtInActiveRemarks.Text)
                            , General.GetNullableInteger(chkInActiveYN.Checked == true ? "0" : "1")
                            , General.GetNullableInteger(ucDefaultAccountType.SelectedQuick)
                            , General.GetNullableDecimal(txtAllotmentAmount.Text)
                            , General.GetNullableInteger(chkRemittingAgent.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(ucRemittingAgent.SelectedAddress)
                            , General.GetNullableInteger(ucDefaultAllotmentCurrency.SelectedCurrency));

                        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
                    }

                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            if (CommandName.ToUpper().Equals("REGISTEREDBANKACCOUNT"))
            {
                Response.Redirect("../Crew/CrewRegisteredBankAccount.aspx", false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCrewBankAccount()
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";
        if (ViewState["FLDACTIVEYN"].ToString() != (chkInActiveYN.Checked == true ? "0" : "1"))
        {
            if (txtInActiveRemarks.Text.Trim().Equals(""))
                ucError.ErrorMessage = "Remark is required.";
        }
        if (General.GetNullableDateTime(txtVerifiedDate.Text) == null)
            ucError.ErrorMessage = "Verified Date Required";
        else if (DateTime.TryParse(txtVerifiedDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
            ucError.ErrorMessage = "Verified Date cannot be future date";

        return (!ucError.IsError);

    }
}
