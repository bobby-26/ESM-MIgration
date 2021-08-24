using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
public partial class CrewBankAccountList : PhoenixBasePage
{
    string id = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ucAccountType.HardTypeCode = ((int)PhoenixHardTypeCode.CREWACCOUNTTYPE).ToString();
            ucRemittanceType.QuickTypeCode = ((int)PhoenixQuickTypeCode.REMITTANCETYPE).ToString();
            ucModeOfPayment.HardTypeCode = ((int)PhoenixHardTypeCode.MODEOFPAYMENT).ToString();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("Registered Bank Account", "REGISTEREDBANKACCOUNT", ToolBarDirection.Right);
            MenuCrewBankAccountList.AccessRights = this.ViewState;
            MenuCrewBankAccountList.MenuList = toolbar.Show();
            id = Request.QueryString["id"];
            if (!IsPostBack)
            {

                //ViewState["ACCOUNTNOPATTERN"] = null;
                ViewState["ACCOUNTNODIGITS"] = null;
                ViewState["ACCOUNTNOALLOWCHARYN"] = null;
                if (!string.IsNullOrEmpty(id))
                {
                    EditCrewBankAccount(new Guid(id));
                }
                else
                    ResetCrewBankAccount();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void EditCrewBankAccount(Guid crewbankaccountid)
    {
        DataTable dt = PhoenixCrewBankAccount.EditCrewBankAccount(crewbankaccountid);

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ucModeOfPayment.SelectedHard = dr["FLDMODEOFPAYMENT"].ToString();
            ucIntermediateBankCountry.SelectedCountry = dr["FLDINTERMEDIATEBANKCOUNTRY"].ToString();
            ucAccountType.SelectedHard = dr["FLDACCOUNTTYPE"].ToString();
            ucRemittanceType.SelectedQuick = dr["FLDTYPEOFREMITTANCE"].ToString();
            txtAccountName.Text = dr["FLDACCOUNTNAME"].ToString();
            txtIBANNumber.Text = dr["FLDIBANNUMBER"].ToString();
            txtIntermediateBank.Text = dr["FLDINTERMEDIATEBANK"].ToString();
            txtIntermediateBankAddres.Text = dr["FLDINTERMEDIATEBANKADDRESS"].ToString();
            txtIntermediateBankSwiftCode.Text = dr["FLDINTERMEDIATEBANKSWIFTCODE"].ToString();
            txtSeafarerBank.Text = dr["FLDBANKNAME"].ToString();
            ucBank.BankList = PhoenixRegistersBank.ListBank(null, null);
            if (dt.Rows[0]["FLDBANKID"].ToString() == "")
            {
                ucBank.SelectedBankText = dr["FLDBANKNAME"].ToString();
            }
            else
            {
                ucBank.SelectedValue = dt.Rows[0]["FLDBANKID"].ToString();
            }
            Bankchange(dt.Rows[0]["FLDBANKID"].ToString());

            txtAccountNumber.Text = dr["FLDACCOUNTNUMBER"].ToString();
            txtSeafarerBankBranch.Text = dr["FLDBANKBRANCH"].ToString();
            txtSeafarerBankSortCode.Text = dr["FLDBANKSORTCODE"].ToString();
            txtSeafarerBankSwiftCode.Text = dr["FLDBANKSWIFTCODE"].ToString();
            txtPaymentPercentage.Text = dr["FLDPAYMENTPERCENTAGE"].ToString();
            ucAddress.Address1 = dr["FLDADDRESS1"].ToString();
            ucAddress.Address2 = dr["FLDADDRESS2"].ToString();
            ucAddress.Address3 = dr["FLDADDRESS3"].ToString();
            ucAddress.Address4 = dr["FLDADDRESS4"].ToString();
            ucAddress.Country = dr["FLDCOUNTRYCODE"].ToString();
            ucAddress.State = dt.Rows[0]["FLDSTATECODE"].ToString();
            ucAddress.City = dt.Rows[0]["FLDCITYID"].ToString();
            ucAddress.PostalCode = dt.Rows[0]["FLDZIPCODE"].ToString();
            ddlaccountopenby.SelectedValue = dt.Rows[0]["FLDACCOUNTOPENEDBY"].ToString();
            ucBankCurrency.SelectedCurrency = dt.Rows[0]["FLDBANKCURRENCYID"].ToString();
            ucIBankCurrency.SelectedCurrency = dt.Rows[0]["FLDINTERMEDIATEBANKCURRENCYID"].ToString();
            txtBankIFSCCode.Text = dt.Rows[0]["FLDBANKIFSCCODE"].ToString();
            txtIbankIFSCCode.Text = dt.Rows[0]["FLDINTERMEDIATEBANKIFSCCODE"].ToString();
            txtIbankAccountName.Text = dt.Rows[0]["FLDIBANKACCOUNTNAME"].ToString();
            txtIbankAccountNo.Text = dt.Rows[0]["FLDIBANKACCOUNTNUMBER"].ToString();
            ucDefaultAccountType.SelectedQuick= dt.Rows[0]["FLDDEFAULTACCOUNTTYPE"].ToString();
            txtAllotmentAmount.Text= dt.Rows[0]["FLDALLOTMENTAMOUNT"].ToString();
            chkRemittingAgent.Checked = dt.Rows[0]["FLDISREMITTINGAGENTREQ"].ToString()=="1"?true:false;
            ucRemittingAgent.SelectedAddress = dt.Rows[0]["FLDREMITTINGAGENTID"].ToString();
            ucDefaultAllotmentCurrency.SelectedCurrency = dt.Rows[0]["FLDDEFAULTALLOTMENTCURRENCY"].ToString();


            if (dt.Rows[0]["FLDACTIVEYN"].ToString() == "0")
                chkInActiveYN.Checked = true;
            txtVerifiedDate.Text = dt.Rows[0]["FLDVERIFIEDDATE"].ToString();
            txtVerifiedBy.Text = dt.Rows[0]["FLDVERIFIEDBYNAME"].ToString();

        }
    }

    protected void ResetCrewBankAccount()
    {
        ucModeOfPayment.SelectedHard = "";
        ucIntermediateBankCountry.SelectedCountry = "";
        ucAccountType.SelectedHard = "";
        ucRemittanceType.SelectedQuick = "";
        ucBank.SelectedValue = "0";
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
        ucAddress.Address1 = "";
        ucAddress.Address2 = "";
        ucAddress.Address3 = "";
        ucAddress.Address4 = "";
        ucAddress.Country = "0";
        ucAddress.City = "0";
        ucAddress.State = "0";
        ucAddress.PostalCode = "";
        txtBankIFSCCode.Text = "";
        ucBankCurrency.SelectedCurrency = "";
        txtVerifiedDate.Text = "";
    }
    protected void UcBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bankchange(ucBank.SelectedBank);
    }
    public void Bankchange(string bank)
    {
        if (General.GetNullableInteger(bank) != null && General.GetNullableInteger(bank) != 0)
        {
            DataSet ds = PhoenixRegistersBank.ListBank(General.GetNullableInteger(bank), null);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtSeafarerBankSortCode.Text = ds.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();
                txtSeafarerBank.Text = ds.Tables[0].Rows[0]["FLDNAME"].ToString();
                ViewState["ACCOUNTNODIGITS"] = ds.Tables[0].Rows[0]["FLDSHORTCODE"].ToString();
                ViewState["ACCOUNTNOALLOWCHARYN"] = ds.Tables[0].Rows[0]["FLDALPHANUMERICYN"].ToString();
                lblDigits.Text = ds.Tables[0].Rows[0]["FLDACCOUNTNODIGIT"].ToString() + " Digits";
                lblswiftcodedigits.Text = ds.Tables[0].Rows[0]["FLDSWIFTCODENODIGIT"].ToString() + " Digits";
            }
        }
        else { lblDigits.Text = ""; lblswiftcodedigits.Text = ""; }

    }
    protected void CrewBankAccountList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string Script = "";
            Script += "<script language='JavaScript' id='BookMarkScript'>";
            Script += "fnReloadList('codehelp1', 'ifMoreInfo');";
            Script += "</script>";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidCrewBankAccount())
                {

                    if (!string.IsNullOrEmpty(id))
                    {
                        PhoenixCrewBankAccount.UpdateCrewBankAccount(
                              new Guid(id)
                            , Int32.Parse(Filter.CurrentCrewSelection.ToString())
                            , Int32.Parse(ucAccountType.SelectedHard)
                            , General.GetNullableString(txtAccountName.Text)
                            , General.GetNullableString(txtAccountNumber.Text.Trim())
                            , General.GetNullableString(txtSeafarerBank.Text)
                            , General.GetNullableString(txtSeafarerBankSortCode.Text)
                            , General.GetNullableString(txtSeafarerBankBranch.Text)
                            , General.GetNullableString(ucAddress.Address1)
                            , General.GetNullableString(ucAddress.Address2)
                            , General.GetNullableString(ucAddress.Address3)
                            , General.GetNullableString(ucAddress.Address4)
                            , General.GetNullableInteger(ucAddress.City)
                            , General.GetNullableInteger(ucAddress.State)
                            , General.GetNullableString(ucAddress.PostalCode)
                            , General.GetNullableString(txtSeafarerBankSwiftCode.Text)
                            , General.GetNullableInteger(ucAddress.Country)
                            , General.GetNullableString(txtIntermediateBank.Text)
                            , General.GetNullableString(txtIntermediateBankAddres.Text)
                            , General.GetNullableString(txtIntermediateBankSwiftCode.Text)
                            , General.GetNullableInteger(ucIntermediateBankCountry.SelectedCountry)
                            , General.GetNullableDecimal(txtPaymentPercentage.Text)
                            , General.GetNullableInteger(ucRemittanceType.SelectedQuick)
                            , General.GetNullableString(txtIBANNumber.Text)
                            , General.GetNullableInteger(ucModeOfPayment.SelectedHard)
                            , General.GetNullableString(txtIbankAccountNo.Text)
                            , General.GetNullableString(txtIbankAccountName.Text)
                            , General.GetNullableString(txtBankIFSCCode.Text)
                            , General.GetNullableString(txtIbankIFSCCode.Text)
                            , General.GetNullableInteger(ucBankCurrency.SelectedCurrency)
                            , General.GetNullableInteger(ucIBankCurrency.SelectedCurrency)
                            , General.GetNullableInteger(chkInActiveYN.Checked == true ? "0" : "1")
                            , General.GetNullableDateTime(txtVerifiedDate.Text)
                            , General.GetNullableInteger(ddlaccountopenby.SelectedValue)
                            , General.GetNullableInteger(ucBank.SelectedBank)
                            ,General.GetNullableInteger(ucDefaultAccountType.SelectedQuick)
                            ,General.GetNullableDecimal(txtAllotmentAmount.Text)
                            ,General.GetNullableInteger(chkRemittingAgent.Checked==true?"1":"0")
                            ,General.GetNullableInteger(ucRemittingAgent.SelectedAddress)
                            ,General.GetNullableInteger(ucDefaultAllotmentCurrency.SelectedCurrency));
                        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
                    }
                    else
                    {
                        PhoenixCrewBankAccount.InsertCrewBankAccount(Int32.Parse(Filter.CurrentCrewSelection.ToString())
                            , Int32.Parse(ucAccountType.SelectedHard)
                            , General.GetNullableString(txtAccountName.Text)
                            , General.GetNullableString(txtAccountNumber.Text.Trim())
                            , General.GetNullableString(ucBank.SelectedBankText)
                            , General.GetNullableString(txtSeafarerBankSortCode.Text)
                            , General.GetNullableString(txtSeafarerBankBranch.Text)
                            , General.GetNullableString(ucAddress.Address1)
                            , General.GetNullableString(ucAddress.Address2)
                            , General.GetNullableString(ucAddress.Address3)
                            , General.GetNullableString(ucAddress.Address4)
                            , General.GetNullableInteger(ucAddress.City)
                            , General.GetNullableInteger(ucAddress.State)
                            , General.GetNullableString(ucAddress.PostalCode)
                            , General.GetNullableString(txtSeafarerBankSwiftCode.Text)
                            , General.GetNullableInteger(ucAddress.Country)
                            , General.GetNullableString(txtIntermediateBank.Text)
                            , General.GetNullableString(txtIntermediateBankAddres.Text)
                            , General.GetNullableString(txtIntermediateBankSwiftCode.Text)
                            , General.GetNullableInteger(ucIntermediateBankCountry.SelectedCountry)
                            , General.GetNullableDecimal(txtPaymentPercentage.Text)
                            , General.GetNullableInteger(ucRemittanceType.SelectedQuick)
                            , General.GetNullableString(txtIBANNumber.Text)
                            , General.GetNullableInteger(ucModeOfPayment.SelectedHard)
                            , General.GetNullableString(txtIbankAccountNo.Text)
                            , General.GetNullableString(txtIbankAccountName.Text)
                            , General.GetNullableString(txtBankIFSCCode.Text)
                            , General.GetNullableString(txtIbankIFSCCode.Text)
                            , General.GetNullableInteger(ucBankCurrency.SelectedCurrency)
                            , General.GetNullableInteger(ucIBankCurrency.SelectedCurrency)
                            , General.GetNullableInteger(chkInActiveYN.Checked == true ? "0" : "1")
                            , General.GetNullableDateTime(txtVerifiedDate.Text)
                            , General.GetNullableInteger(ddlaccountopenby.SelectedValue)
                            , General.GetNullableInteger(ucBank.SelectedBank)
                            , General.GetNullableInteger(ucDefaultAccountType.SelectedQuick)
                            , General.GetNullableDecimal(txtAllotmentAmount.Text)
                            , General.GetNullableInteger(chkRemittingAgent.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(ucRemittingAgent.SelectedAddress)
                            , General.GetNullableInteger(ucDefaultAllotmentCurrency.SelectedCurrency));
                        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
                        ResetCrewBankAccount();
                    }

                    if (!string.IsNullOrEmpty(id))
                    {
                        EditCrewBankAccount(new Guid(id));
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
        Int32 resultInt;
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";
        if (ucBank.SelectedBank == "0")
            ucError.ErrorMessage = "Bank Name is required.";
        if ((!Int32.TryParse(ucAccountType.SelectedHard, out resultInt)))
            ucError.ErrorMessage = "Account Type is required.";

        if (txtAccountName.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Beneficiary  is required.";
        if (Regex.Replace(txtAccountNumber.Text, "[^a-zA-Z0-9]+", "").Trim().Equals(""))
            ucError.ErrorMessage = "Account No. is required.";
        else if (!System.Text.RegularExpressions.Regex.IsMatch(txtAccountNumber.Text, "^[0-9]*$") && ViewState["ACCOUNTNOALLOWCHARYN"].ToString() == "0")
        {
            ucError.ErrorMessage = "Account No. accepts only numbers.";
        }
        if (ddlaccountopenby.SelectedValue == "")
            ucError.ErrorMessage = "Bank Account Opened by is required.";
        //if (txtSeafarerBank.Text.Trim().Equals(""))
        //    ucError.ErrorMessage = "Seafarer Bank is required.";
        if (txtSeafarerBankSwiftCode.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Swift Code is required.";
        if (txtBankIFSCCode.Text.Trim().Equals(""))
            ucError.ErrorMessage = "IFSC Code is required.";
        if (!General.GetNullableInteger(ucBankCurrency.SelectedCurrency).HasValue)
            ucError.ErrorMessage = "Bank Currency is required.";
        if (string.IsNullOrEmpty(ucAddress.Address1))
            ucError.ErrorMessage = "Address1 is required.";
        if (!General.GetNullableInteger(ucAddress.Country).HasValue)
            ucError.ErrorMessage = "Country is required.";
        if (!General.GetNullableInteger(ucAddress.City).HasValue)
            ucError.ErrorMessage = "City is required.";
        if (General.GetNullableDateTime(txtVerifiedDate.Text) == null)
            ucError.ErrorMessage = "Verified Date Required";
        else if (DateTime.TryParse(txtVerifiedDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
            ucError.ErrorMessage = "Verified Date cannot be future date";
        return (!ucError.IsError);
    }
}
