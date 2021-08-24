using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Common;

public partial class PreSeaBankAccountList : PhoenixBasePage
{
    string id = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        ucAccountType.HardTypeCode = ((int)PhoenixHardTypeCode.CREWACCOUNTTYPE).ToString();
        ucRemittanceType.QuickTypeCode = ((int)PhoenixQuickTypeCode.REMITTANCETYPE).ToString();
        ucModeOfPayment.HardTypeCode = ((int)PhoenixHardTypeCode.MODEOFPAYMENT).ToString();
        id = Request.QueryString["id"];
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE");
        MenuPreSeaBankAccountList.AccessRights = this.ViewState;
        MenuPreSeaBankAccountList.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(id))
            {
                EditPreSeaBankAccount(new Guid(id));
            }
            else
                ResetPreSeaBankAccount();
        }
    }

    protected void EditPreSeaBankAccount(Guid preseabankaccountid)
    {
        DataTable dt = PhoenixPreSeaBankAccount.EditPreSeaBankAccount(preseabankaccountid);

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            ucModeOfPayment.SelectedHard = dr["FLDMODEOFPAYMENT"].ToString();
            ucIntermediateBankCountry.SelectedCountry = dr["FLDINTERMEDIATEBANKCOUNTRY"].ToString();
            ucAccountType.SelectedHard = dr["FLDACCOUNTTYPE"].ToString();
            ucRemittanceType.SelectedQuick = dr["FLDTYPEOFREMITTANCE"].ToString();
            txtAccountName.Text = dr["FLDACCOUNTNAME"].ToString();
            txtAccountNumber.Text = dr["FLDACCOUNTNUMBER"].ToString();
            txtIBANNumber.Text = dr["FLDIBANNUMBER"].ToString();
            txtIntermediateBank.Text = dr["FLDINTERMEDIATEBANK"].ToString();
            txtIntermediateBankAddres.Text = dr["FLDINTERMEDIATEBANKADDRESS"].ToString();
            txtIntermediateBankSwiftCode.Text = dr["FLDINTERMEDIATEBANKSWIFTCODE"].ToString();
            txtSeafarerBank.Text = dr["FLDBANKNAME"].ToString();
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
        }
    }

    protected void ResetPreSeaBankAccount()
    {
        ucModeOfPayment.SelectedHard = "";
        ucIntermediateBankCountry.SelectedCountry = "";
        ucAccountType.SelectedHard = "";
        ucRemittanceType.SelectedQuick = "";
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
    }

    protected void PreSeaBankAccountList_TabStripCommand(object sender, EventArgs e)
    {
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (IsValidPreSeaBankAccount())
            {
                if (!string.IsNullOrEmpty(id))
                {
                    PhoenixPreSeaBankAccount.UpdatePreSeaBankAccount(
                          new Guid(id)
                        , Int32.Parse(Filter.CurrentPreSeaNewApplicantSelection.ToString())
                        , Int32.Parse(ucAccountType.SelectedHard)
                        , txtAccountName.Text
                        , txtAccountNumber.Text
                        , txtSeafarerBank.Text
                        , txtSeafarerBankSortCode.Text
                        , txtSeafarerBankSwiftCode.Text
                        , ucAddress.Address1
                        , ucAddress.Address2
                        , ucAddress.Address3
                        , ucAddress.Address4
                        , General.GetNullableInteger(ucAddress.City)
                        , General.GetNullableInteger(ucAddress.State)
                        , ucAddress.PostalCode
                        , txtSeafarerBankSwiftCode.Text
                        , General.GetNullableInteger(ucAddress.Country)
                        , txtIntermediateBank.Text
                        , txtIntermediateBankAddres.Text
                        , txtIntermediateBankSwiftCode.Text
                        , General.GetNullableInteger(ucIntermediateBankCountry.SelectedCountry)
                        , General.GetNullableDecimal(txtPaymentPercentage.Text)
                        , General.GetNullableInteger(ucRemittanceType.SelectedQuick)
                        , txtIBANNumber.Text
                        , General.GetNullableInteger(ucModeOfPayment.SelectedHard));

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                }
                else
                {
                    PhoenixPreSeaBankAccount.InsertPreSeaBankAccount(Int32.Parse(Filter.CurrentPreSeaNewApplicantSelection.ToString())
                        , Int32.Parse(ucAccountType.SelectedHard)
                        , txtAccountName.Text
                        , txtAccountNumber.Text
                        , txtSeafarerBank.Text
                        , txtSeafarerBankSortCode.Text
                        , txtSeafarerBankSwiftCode.Text
                        , ucAddress.Address1
                        , ucAddress.Address2
                        , ucAddress.Address3
                        , ucAddress.Address4
                        , General.GetNullableInteger(ucAddress.City)
                        , General.GetNullableInteger(ucAddress.State)
                        , ucAddress.PostalCode
                        , txtSeafarerBankSwiftCode.Text
                        , General.GetNullableInteger(ucAddress.Country)
                        , txtIntermediateBank.Text
                        , txtIntermediateBankAddres.Text
                        , txtIntermediateBankSwiftCode.Text
                        , General.GetNullableInteger(ucIntermediateBankCountry.SelectedCountry)
                        , General.GetNullableDecimal(txtPaymentPercentage.Text)
                        , General.GetNullableInteger(ucRemittanceType.SelectedQuick)
                        , txtIBANNumber.Text
                        , General.GetNullableInteger(ucModeOfPayment.SelectedHard));

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupopen, true);
                    ResetPreSeaBankAccount();
                }
            }
            else
            {
                ucError.Visible = true;
                return;
            }
        }
    }

    private bool IsValidPreSeaBankAccount()
    {
        Int32 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if ((!Int32.TryParse(ucAccountType.SelectedHard, out resultInt)))
            ucError.ErrorMessage = "Account Type is required.";

        if (txtAccountName.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Beneficiary Name is required.";
        if (txtAccountNumber.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Account Number is required.";
        if (txtSeafarerBank.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Seafarer Bank is required.";
        if (string.IsNullOrEmpty(ucAddress.Address1))
            ucError.ErrorMessage = "Address1 is required.";
        if (!General.GetNullableInteger(ucAddress.Country).HasValue)
            ucError.ErrorMessage = "Country is required.";
        if (!General.GetNullableInteger(ucAddress.City).HasValue)
            ucError.ErrorMessage = "City is required.";
        return (!ucError.IsError);
    }
}
