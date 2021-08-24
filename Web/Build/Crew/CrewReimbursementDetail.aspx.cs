using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
public partial class CrewReimbursementDetail : PhoenixBasePage
{
    string rembid = string.Empty;
    public string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            rembid = Request.QueryString["rembid"];
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuDetail.AccessRights = this.ViewState;
            MenuDetail.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["READONLY"] = "false";
                ViewState["reimid"] = Request.QueryString["rembid"];
                if (Request.QueryString["readonly"] != null)
                    ViewState["READONLY"] = "true";
                if (ViewState["READONLY"].ToString() == "true")
                    MenuDetail.Visible = false;
                ddlPaymentModeAdd.HardList = PhoenixRegistersHard.ListHard(1, 142);
                ddlBankAccount.EmployeeId = "0";
                SetReimbursement();
            }
            if (!string.IsNullOrEmpty(rembid))
            {
                tblSefarer.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeListByFileNo(txtFileNo.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtVessel.Text = dt.Rows[0]["FLDPRESENTVESSELNAME"].ToString();
                ViewState["EMPID"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                lblLastVessel.Text = dt.Rows[0]["FLDLASTVESSELNAME"].ToString();
                lblPresentVessel.Text = dt.Rows[0]["FLDPRESENTVESSELNAME"].ToString();
                lblNextVessel.Text = dt.Rows[0]["FLDNEXTVESSEL"].ToString();
                ddlBankAccount.EmployeeId = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                ddlBankAccount.BankAccountList = PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(0, General.GetNullableInteger(dt.Rows[0]["FLDEMPLOYEEID"].ToString()));
                bankdetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ImgBtnValidFileno_Click(object sender, System.EventArgs e)
    {
        if (IsValidFileNoCheck())
            SetEmployeePrimaryDetails();
        else
        {
            ucError.Visible = true;
            return;
        }
    }
    protected void MenuDetail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidateReimbursement(ddlEarDed.SelectedValue, ddlPurpose.SelectedHard, ddlCurrency.SelectedCurrency, txtAmount.Text
                    , txtApprovedAmount.Text, txtUnApprovedReason.Text, txtDesc.Text, ucDate.Text, txtNoofInstallment.Text
                    , ddlPaymentModeAdd.SelectedHard, txtUnApprovedAmount.Text, ucPaymentcurrency.SelectedCurrency, txtExchangeRate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["reimid"] != null && ViewState["reimid"].ToString() != string.Empty)
                {
                    PhoenixCrewReimbursement.UpdateCrewReimbursement(new Guid(ViewState["reimid"].ToString()), int.Parse(ddlPurpose.SelectedHard), int.Parse(ddlCurrency.SelectedCurrency), decimal.Parse(txtAmount.Text)
                        , General.GetNullableDecimal(txtApprovedAmount.Text), txtUnApprovedReason.Text, null, General.GetNullableInteger(ddlBudgetCode.SelectedBudgetCode)
                        , int.Parse(ddlEarDed.SelectedValue), txtDesc.Text
                        , General.GetNullableInteger(ddlVessel.SelectedVessel), General.GetNullableInteger(ddlVesselChargeable.SelectedVessel), DateTime.Parse(ucDate.Text), txtApprovalRemark.Text, byte.Parse(txtNoofInstallment.Text), byte.Parse(chk_CurrentContract.Checked ? "1" : "0"), int.Parse(ddlPaymentModeAdd.SelectedHard)
                        , General.GetNullableGuid(ddlBankAccount.SelectedBankAccount), int.Parse(ucPaymentcurrency.SelectedCurrency), decimal.Parse(txtExchangeRate.Text));
                    ucStatus.Text = "Reimbursement Details Updated";
                }
                else
                {
                    ViewState["reimid"] = PhoenixCrewReimbursement.InsertCrewReimbursement(int.Parse(ViewState["EMPID"].ToString()), int.Parse(ddlPurpose.SelectedHard), int.Parse(ddlCurrency.SelectedCurrency), decimal.Parse(txtAmount.Text)
                    , General.GetNullableDecimal(txtApprovedAmount.Text), txtUnApprovedReason.Text, null, General.GetNullableInteger(ddlBudgetCode.SelectedBudgetCode)
                    , int.Parse(ddlEarDed.SelectedValue), txtDesc.Text, General.GetNullableInteger(ddlVessel.SelectedVessel)
                    , General.GetNullableInteger(ddlVesselChargeable.SelectedVessel), DateTime.Parse(ucDate.Text)
                    , txtApprovalRemark.Text, byte.Parse(txtNoofInstallment.Text)
                    , byte.Parse(chk_CurrentContract.Checked ? "1" : "0"), int.Parse(ddlPaymentModeAdd.SelectedHard)
                    , General.GetNullableGuid(ddlBankAccount.SelectedBankAccount), int.Parse(ucPaymentcurrency.SelectedCurrency), decimal.Parse(txtExchangeRate.Text));
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('payment',null,'keepopen');", true);
                SetReimbursement();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetReimbursement()
    {
        rembid = (string)ViewState["reimid"];
        if (General.GetNullableGuid(rembid).HasValue)
        {
            DataTable dt = PhoenixCrewReimbursement.EditReimbursement(new Guid(rembid));
            if (dt.Rows.Count > 0)
            {
                tblSefarer.Visible = false;
                DataRow dr = dt.Rows[0];
                txtId.Text = dr["FLDREFERENCENO"].ToString();
                ViewState["EMPID"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                ucDate.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dr["FLDSUBMISSIONDATE"].ToString()));
                txtEmployee.Text = dr["FLDEMPLOYEENAME"].ToString();
                txtEmpFileNo.Text = dr["FLDFILENO"].ToString();
                txtAmount.Text = dr["FLDAMOUNT"].ToString();
                ddlCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
                ddlEarDed.SelectedValue = dr["FLDEARNINGDEDUCTION"].ToString();
                if (General.GetNullableInteger(dr["FLDEARNINGDEDUCTION"].ToString()).HasValue && General.GetNullableInteger(dr["FLDEARNINGDEDUCTION"].ToString()).Value < 0)
                {
                    ddlPurpose.HardTypeCode = "1";

                }
                ddlPurpose.SelectedHard = dr["FLDHARDCODE"].ToString();
                txtApprovedAmount.Text = dr["FLDAPPROVEDAMOUNT"].ToString();
                txtUnApprovedAmount.Text = dr["FLDUNAPPROVEDAMOUNT"].ToString();
                txtExchangeRate.Text = dr["FLDEXCHANGERATE"].ToString();
                txtClaimAmount.Text = dr["FLDUSDAMOUNT"].ToString();
                txtDesc.Text = dr["FLDDESCRIPTION"].ToString();
                txtVessel.Text = dr["FLDVESSELNAME"].ToString();
                txtStatus.Text = dr["FLDSTATUS"].ToString();
                ddlBudgetCode.SelectedBudgetCode = dr["FLDBUDGETCODE"].ToString();
                txtBudgetCode.Text = dr["FLDSUBACCOUNT"].ToString();
                ddlVessel.SelectedVessel = dr["FLDACCOUNTOF"].ToString();
                ddlVesselChargeable.SelectedVessel = dr["FLDCHARGEABLEVESSEL"].ToString();
                txtNoofInstallment.Text = dr["FLDNOOFINSTALLMENT"].ToString();
                chk_CurrentContract.Checked = dr["FLDCURRENTCONTRACTYN"].ToString() == "1" ? true : false;
                ddlBankAccount.EmployeeId = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                ddlBankAccount.BankAccountList = PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(0, General.GetNullableInteger(dt.Rows[0]["FLDEMPLOYEEID"].ToString()));
                ddlPaymentModeAdd.ShortNameFilter = dr["FLDPAYMENTMODELISTCODE"].ToString();
                ddlPaymentModeAdd.SelectedHard = dr["FLDPAYMENTMODE"].ToString();

                //  ddlBankAccount.CssClass = dr["FLDPAYMENTMODE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 142, "ALT") ? "input_mandatory" : "input";
                if (dr["FLDPAYMENTMODE"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 142, "ALT"))
                {

                    ddlBankAccount.Enabled = true;
                    ddlBankAccount.CssClass = "input_mandatory";
                    ddlBankAccount.EmployeeId = ViewState["EMPID"].ToString();
                    ddlBankAccount.BankAccountList = PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(0, General.GetNullableInteger(ViewState["EMPID"].ToString()));

                }
                else
                {
                    ddlBankAccount.SelectedBankAccount = "";
                    ddlBankAccount.Enabled = false;
                    ddlBankAccount.CssClass = "input";
                }
                ViewState["BUDGETCODE"] = dr["FLDBUDGETCODE"].ToString();
                txtUnApprovedReason.Text = dr["FLDUNAPPROVEDREMARKS"].ToString();
                lblLastVessel.Text = dr["FLDLASTVESSEL"].ToString();
                lblPresentVessel.Text = dr["FLDPRESENTVESSEL"].ToString();
                lblNextVessel.Text = dr["FLDNEXTVESSEL"].ToString();
                txtCreatedDate.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"].ToString());
                txtCreatedBy.Text = dr["FLDCREATEDBY"].ToString();
                txtApprovedDate.Text = General.GetDateTimeToString(dr["FLDAPPROVEDDATE"].ToString());
                txtApprovedBy.Text = dr["FLDAPPROVEDBY"].ToString();
                txtApprovalRemark.Text = dr["FLDAPPROVALREMARKS"].ToString();
                // txtPaymentCurrency.Text = dt.Rows[0]["FLDREIMBURCURRENCYCODE"].ToString();
                // ddlBankAccount.SelectedBankAccount = dt.Rows[0]["FLDBANKACCOUNTID"].ToString();
                lblApprovedClaimAmount.Text = "Approved Amount(" + dt.Rows[0]["FLDREIMBURCURRENCYCODE"].ToString() + ")";
                lblunApprovedClaimAmount.Text = "Unapproved Amount(" + dt.Rows[0]["FLDREIMBURCURRENCYCODE"].ToString() + ")";
                lblApprovedamount.Text = "Approved Amount(" + dt.Rows[0]["FLDCURRENCYCODE"].ToString() + ")";
                lblUnapproved.Text = "Unapproved Amount(" + dt.Rows[0]["FLDCURRENCYCODE"].ToString() + ")";
                // bankdetails();
                ucPaymentcurrency.CurrencyList = PhoenixVesselAccountsCurrencyConfiguration.ListCurrencyConfiguration(General.GetNullableInteger(dr["FLDACCOUNTOF"].ToString()), 1);

                if (dt.Rows[0]["FLDEXCHANGERATE"].ToString() != "" && txtApprovedAmount.Text != "")
                {
                    txtClaimAmount.Text = (decimal.Parse(txtApprovedAmount.Text) / decimal.Parse(dt.Rows[0]["FLDEXCHANGERATE"].ToString())).ToString();
                    if (txtUnApprovedAmount.Text != "")
                    {
                        txtunClaimAmount.Text = (decimal.Parse(txtUnApprovedAmount.Text) / decimal.Parse(dt.Rows[0]["FLDEXCHANGERATE"].ToString())).ToString();
                    }
                }
                ucPaymentcurrency.SelectedCurrency = dt.Rows[0]["FLDREIMBURCURRENCYID"].ToString();

            }
        }
    }
    private bool IsValidateReimbursement(string earded, string purpose, string curreny, string amount, string approvedamount, string unapprovedremarks, string desc
        , string date, string noofinstallment, string paymentmode, string UnApprovedAmount, string reimbuscurrency, string ExchangeRate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableInteger(earded).HasValue)
            ucError.ErrorMessage = "Reimbursement/ Recovery is required";

        if (!General.GetNullableInteger(purpose).HasValue)
            ucError.ErrorMessage = "Purpose is required";

        if (desc.Trim() == string.Empty)
            ucError.ErrorMessage = "Description is required";

        if (!General.GetNullableDecimal(curreny).HasValue)
            ucError.ErrorMessage = "Currency is required";
        if (!General.GetNullableDecimal(ExchangeRate).HasValue)
            ucError.ErrorMessage = "Exchage Rate is required";
        if (!General.GetNullableInteger(reimbuscurrency).HasValue)
            ucError.ErrorMessage = "Vessel Currecncy is required";

        if (!General.GetNullableDecimal(amount).HasValue)
            ucError.ErrorMessage = "Amount is required.";

        if (General.GetNullableDecimal(approvedamount).HasValue && General.GetNullableDecimal(amount).HasValue
                && General.GetNullableDecimal(approvedamount).Value > General.GetNullableDecimal(amount).Value)
            ucError.ErrorMessage = "Approved Amount should be less than Amount";

        if (General.GetNullableDecimal(approvedamount).HasValue && General.GetNullableDecimal(amount).HasValue
                && General.GetNullableDecimal(amount).Value - General.GetNullableDecimal(approvedamount).Value > 0 && unapprovedremarks.Trim() == string.Empty)
            ucError.ErrorMessage = "Reason for Unapproved Amount is required.";

        if ((General.GetNullableDecimal(approvedamount).HasValue && General.GetNullableDecimal(amount).HasValue)
            && (General.GetNullableDecimal(amount).Value > General.GetNullableDecimal(approvedamount).Value)
            && !General.GetNullableDecimal(UnApprovedAmount).HasValue)
            ucError.ErrorMessage = "UnApproved Amount is Required";

        if ((General.GetNullableDecimal(approvedamount).HasValue && General.GetNullableDecimal(amount).HasValue && General.GetNullableDecimal(UnApprovedAmount).HasValue)
           && (General.GetNullableDecimal(amount).Value != (General.GetNullableDecimal(approvedamount).Value + General.GetNullableDecimal(UnApprovedAmount).Value)))
            ucError.ErrorMessage = "Approved Amount and UnApproved Amount Should be equal to Claim Amount";

        if (string.IsNullOrEmpty(rembid) && (ViewState["EMPID"] == null || ViewState["EMPID"].ToString() == string.Empty))
        {
            ucError.ErrorMessage = "Please the enter file number and verify by clicking search icon next to the Seafarer File Number textbox.";
        }
        if (!General.GetNullableDateTime(date).HasValue)
            ucError.ErrorMessage = "Claim Submission Date is required.";

        if (!General.GetNullableInteger(ddlBudgetCode.SelectedBudgetCode).HasValue)
            ucError.ErrorMessage = "Budget Code is required.";

        if (!General.GetNullableInteger(ddlVessel.SelectedVessel).HasValue)
            //&& General.GetNullableInteger(earded).HasValue && General.GetNullableInteger(earded).Value > 0)
            ucError.ErrorMessage = "Account of is required.";

        if (!General.GetNullableInteger(ddlVesselChargeable.SelectedVessel).HasValue)
            //&& General.GetNullableInteger(earded).HasValue && General.GetNullableInteger(earded).Value > 0)
            ucError.ErrorMessage = "Chargeable is required.";

        if (General.GetNullableInteger(txtApprovedAmount.Text).HasValue && txtApprovalRemark.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Approval Remarks is required.";

        if (General.GetNullableByte(noofinstallment) == null)
            ucError.ErrorMessage = "No of Installment is required.";


        if (General.GetNullableInteger(paymentmode) == null)
            ucError.ErrorMessage = "Payment Mode is required.";
        if (General.GetNullableInteger(paymentmode) != null && paymentmode == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 142, "ALT"))
        {
            if (string.IsNullOrEmpty(ddlBankAccount.SelectedBankAccount))
                ucError.ErrorMessage = "Bank Account is required.";
        }
        return (!ucError.IsError);
    }
    private bool IsValidFileNoCheck()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(txtFileNo.Text.Trim()))
            ucError.ErrorMessage = "File Number is required";

        return (!ucError.IsError);
    }
    protected void ddlEarDed_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableInteger(ddlEarDed.SelectedValue).HasValue && General.GetNullableInteger(ddlEarDed.SelectedValue).Value < 0)
            {

                //ddlPurpose.HardList = PhoenixRegistersHard.ListHard(1, 129, 0, "CRR,ADL,TFR");
                ddlPurpose.HardList = PhoenixRegistersReimbursementRecovery.ListReimbursementRecovery(1, 0, 1, null);
                ddlVessel.CssClass = "input_mandatory";
            }
            else
            {
                ddlPurpose.HardList = PhoenixRegistersReimbursementRecovery.ListReimbursementRecovery(0, 0, 1, null);
                ddlVessel.CssClass = "input_mandatory";
            }
            if (ddlEarDed.SelectedValue == "-1" || ddlEarDed.SelectedValue == "-3" || ddlEarDed.SelectedValue == "3" || ddlEarDed.SelectedValue == "1")
            {
                txtNoofInstallment.Text = "1";
                txtNoofInstallment.ReadOnly = true;
                txtNoofInstallment.CssClass = "readonlytextbox";
            }
            else
            {
                txtNoofInstallment.Text = "1";
                txtNoofInstallment.ReadOnly = false;
                txtNoofInstallment.CssClass = "input_mandatory";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void txtApprovedAmount_TextChanged(object sender, EventArgs e)
    {
        txtApprovalRemark.CssClass = txtApprovedAmount.Text != string.Empty ? "input_mandatory" : "input";
    }

    public void setPurpose(string Purpose)
    {
        DataTable dt = PhoenixRegistersReimbursementRecovery.EditReimbursementRecovery(int.Parse(Purpose));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ddlBudgetCode.SelectedBudgetCode = dr["FLDBUDGETCODE"].ToString();
            txtBudgetCode.Text = dr["FLDSUBACCOUNT"].ToString();
            if (dr["FLDPAYMENTMODELISTCODE"].ToString() != "")
                ddlPaymentModeAdd.HardList = PhoenixRegistersHard.ListHard(1, 142, 0, dr["FLDPAYMENTMODELISTCODE"].ToString());
            else
                ddlPaymentModeAdd.HardList = PhoenixRegistersHard.ListHard(1, 142);
            //ddlPaymentModeAdd.DataBind();
        }
    }

    protected void ddlPurpose_TextChangedEvent(object sender, EventArgs e)
    {
        setPurpose(ddlPurpose.SelectedHard);
    }
    protected void ucPaymentcurrency_TextChangedEvent(object sender, EventArgs e)
    {
        VesselCurrency(ddlVessel.SelectedVessel);
    }
    protected void ddlVessel_TextChangedEvent(object sender, EventArgs e)
    {

        if (ddlVessel.SelectedVessel != "Dummy")
        {
            ucPaymentcurrency.CurrencyList = PhoenixVesselAccountsCurrencyConfiguration.ListCurrencyConfiguration(General.GetNullableInteger(ddlVessel.SelectedVessel), 1);
            DataTable dt = PhoenixCrewReimbursement.editaccountofvesselcurrency(int.Parse(ddlVessel.SelectedVessel), int.Parse(ddlCurrency.SelectedCurrency), General.GetNullableInteger(ucPaymentcurrency.SelectedCurrency));
            if (dt.Rows.Count > 0)
            {
                ucPaymentcurrency.SelectedCurrency = dt.Rows[0]["FLDVESSELCURRENCY"].ToString();
                txtExchangeRate.Text = dt.Rows[0]["FLDEXCHAGERATE"].ToString();
                if (dt.Rows[0]["FLDEXCHAGERATE"].ToString() != "" && txtApprovedAmount.Text != "")
                {
                    txtClaimAmount.Text = (decimal.Parse(txtApprovedAmount.Text) / decimal.Parse(dt.Rows[0]["FLDEXCHAGERATE"].ToString())).ToString();
                    if (txtUnApprovedAmount.Text != "")
                    {
                        txtunClaimAmount.Text = (decimal.Parse(txtUnApprovedAmount.Text) / decimal.Parse(dt.Rows[0]["FLDEXCHAGERATE"].ToString())).ToString();
                    }
                }
                lblApprovedClaimAmount.Text = "Approved Amount(" + dt.Rows[0]["FLDVESSELCURRENCYCODE"].ToString() + ")";
                lblunApprovedClaimAmount.Text = "Unapproved Amount(" + dt.Rows[0]["FLDVESSELCURRENCYCODE"].ToString() + ")";
            }
        }
    }
    protected void ddlCurrency_TextChangedEvent(object sender, EventArgs e)
    {
        lblApprovedamount.Text = "Approved Amount(" + ddlCurrency.SelectedCurrencyText + ")";
        lblUnapproved.Text = "Unapproved Amount(" + ddlCurrency.SelectedCurrencyText + ")";
        VesselCurrency(ddlVessel.SelectedVessel);
    }
    private void VesselCurrency(string Vessel)
    {

        if (Vessel != "Dummy" && Vessel != "")
        {
            // ucPaymentcurrency.CurrencyList = PhoenixVesselAccountsCurrencyConfiguration.ListCurrencyConfiguration(General.GetNullableInteger(Vessel), 1);
            DataTable dt = PhoenixCrewReimbursement.editaccountofvesselcurrency(int.Parse(Vessel), int.Parse(ddlCurrency.SelectedCurrency), General.GetNullableInteger(ucPaymentcurrency.SelectedCurrency));
            if (dt.Rows.Count > 0)
            {
                txtExchangeRate.Text = dt.Rows[0]["FLDEXCHAGERATE"].ToString();
                if (dt.Rows[0]["FLDEXCHAGERATE"].ToString() != "" && txtApprovedAmount.Text != "")
                {
                    txtClaimAmount.Text = (decimal.Parse(txtApprovedAmount.Text) / decimal.Parse(dt.Rows[0]["FLDEXCHAGERATE"].ToString())).ToString();
                    if (txtUnApprovedAmount.Text != "")
                    {
                        txtunClaimAmount.Text = (decimal.Parse(txtUnApprovedAmount.Text) / decimal.Parse(dt.Rows[0]["FLDEXCHAGERATE"].ToString())).ToString();
                    }
                }
                //lblPaymentcurrencyid.Text = dt.Rows[0]["FLDVESSELCURRENCY"].ToString();
                //txtPaymentCurrency.Text = dt.Rows[0]["FLDVESSELCURRENCYCODE"].ToString();
                lblApprovedClaimAmount.Text = "Approved Amount(" + dt.Rows[0]["FLDVESSELCURRENCYCODE"].ToString() + ")";
                lblunApprovedClaimAmount.Text = "Unapproved Amount(" + dt.Rows[0]["FLDVESSELCURRENCYCODE"].ToString() + ")";
            }
            else
            {
                ClearPaymentCurrency();
            }
        }
        else
        {
            ClearPaymentCurrency();
        }
    }
    private void ClearPaymentCurrency()
    {
        //lblPaymentcurrencyid.Text = "";
        //txtPaymentCurrency.Text = "";
        //ucPaymentcurrency.SelectedCurrency = "Dummy";
        txtExchangeRate.Text = "";
        lblApprovedClaimAmount.Text = "Approved Amount";
        lblunApprovedClaimAmount.Text = "Unapproved Amount";
    }
    protected void ddlPaymentModeAdd_TextChangedEvent(object sender, EventArgs e)
    {
        bankdetails();
    }
    private void bankdetails()
    {
        if (ddlPaymentModeAdd.SelectedHard == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 142, "ALT"))
        {

            ddlBankAccount.Enabled = true;
            ddlBankAccount.CssClass = "input_mandatory";
            ddlBankAccount.EmployeeId = ViewState["EMPID"].ToString();
            ddlBankAccount.BankAccountList = PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(0, General.GetNullableInteger(ViewState["EMPID"].ToString()));

        }
        else
        {
            ddlBankAccount.SelectedBankAccount = "";
            ddlBankAccount.Enabled = false;
            ddlBankAccount.CssClass = "input";
        }

    }
    protected void cmdCalculateWage_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            VesselCurrency(ddlVessel.SelectedVessel);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}