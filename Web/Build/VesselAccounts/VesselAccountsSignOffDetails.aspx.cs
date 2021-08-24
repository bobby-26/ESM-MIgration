using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsSignOffDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState); PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("BOW", "BOW", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuSignOffBow.AccessRights = this.ViewState;
        MenuSignOffBow.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {

            if (!string.IsNullOrEmpty(Request.QueryString["SIGNONID"]))
                ViewState["SIGNONID"] = Request.QueryString["SIGNONID"];

            txtVesselName.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
            txtEmpName.Text = Request.QueryString["empname"].ToString();

            ddlReason.SignOffReasonList = PhoenixRegistersreasonssignoff.Listreasonssignoff();
            ddlReason.DataBind();
            ddlBankAccount.EmployeeId = Request.QueryString["EMPID"].ToString();
            ddlBankAccount.BankAccountList = PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(Request.QueryString["EMPID"]));
            ddlBankAccount.DataBind();

            EditSignOffdetails();
            ViewState["BNDSUBSIDY"] = "0";
            DataTable dt = PhoenixVesselAccountsBondSubsidy.FetchVesselBondSubsidy(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0 && General.GetNullableDecimal(dt.Rows[0]["FLDBONDSUBSIDYAMOUNT"].ToString()).HasValue
                && General.GetNullableDecimal(dt.Rows[0]["FLDBONDSUBSIDYAMOUNT"].ToString()).Value > 0)
            {
                ViewState["BNDSUBSIDY"] = "1";
            }
        }

    }
    protected void EditSignOffdetails()
    {
        int signonid = int.Parse(ViewState["SIGNONID"].ToString());
        DataTable dt = PhoenixVesselAccountsEmployee.EditSignOffDetails(signonid);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtSignOffDate.Text = dr["FLDSIGNOFFDATE"].ToString();
            if (dr["FLDSIGNOFFSEAPORTID"].ToString() != string.Empty)
            {
                ddlSeaPort.SelectedValue = dr["FLDSIGNOFFSEAPORTID"].ToString();
                ddlSeaPort.Text = dr["FLDSIGNOFFSEAPORTNAME"].ToString();
            }
            if (dr["FLDSIGNOFFREASONID"].ToString() != string.Empty)
                ddlReason.SelectedSignOffReason = dr["FLDSIGNOFFREASONID"].ToString();
            txtBalance.Text = dr["FLDCURRENTBALANCE"].ToString();
            if (dr["FLDEARNINGDEDUCTIONID"].ToString() != string.Empty)
            {
                txtAmountAllot.Text = dr["FLDALLOTMENT"].ToString();
                txtAmountAllot.Enabled = false;
                txtAmountAllot.CssClass = "readonlytextbox";
                ddlBankAccount.SelectedBankAccount = dr["FLDBANKACCOUNTID"].ToString();
                ddlBankAccount.Enabled = false;
                cmdDelete.Visible = true;
                cmdDelete.CommandArgument = dr["FLDEARNINGDEDUCTIONID"].ToString();
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete sign-off Allotment ?'); return false;");
            }
            else
            {
                ddlBankAccount.SelectedBankAccount = dr["FLDBANKACCOUNTID"].ToString();
                ddlBankAccount.Enabled = true;
                txtAmountAllot.Text = string.Empty;
                txtAmountAllot.Enabled = true;
                txtAmountAllot.CssClass = "input";
                cmdDelete.Visible = false;
            }
            if (dr["FLDSIGNOFFDATE"].ToString() != string.Empty)
            {
                cmdSignOffDateDelete.Visible = true;
                cmdSignOffDateDelete.CommandArgument = Request.QueryString["EMPID"].ToString();
                cmdSignOffDateDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete sign-off Date ?'); return false;");
            }
            else
            {
                cmdSignOffDateDelete.Visible = false;
            }
            string paidwages = dr["FLDCANCELWAGES"].ToString();
            txtRoundOff.Text = dr["FLDCURRENTBALANCE"].ToString();
            txtRemarks.Text = dr["FLDSIGNOFFREMARKS"].ToString();
            if (!String.IsNullOrEmpty(paidwages))
            {
                string[] wages = paidwages.Trim().Split(',');
                for (int i = 0; i < wages.Length; i++)
                {
                    if (wages[i] == "1" && i == 1)
                        chbxlstPaidWages.Items[0].Selected = true;
                    if (wages[i] == "1" && i == 2)
                        chbxlstPaidWages.Items[1].Selected = true;
                }
            }
            else
            {
                foreach (ButtonListItem item in chbxlstPaidWages.Items)
                {
                    item.Selected = true;
                }
            }
        }
    }
    protected void MenuSignOffBow_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            int? signonid = null;

            if (ViewState["SIGNONID"] != null)
                signonid = int.Parse(ViewState["SIGNONID"].ToString());

            if (CommandName.ToUpper().Equals("BOW"))
            {
                PhoenixVesselAccountsReimbursement.IncludeReimbursementRecovery(PhoenixSecurityContext.CurrentSecurityContext.VesselID, signonid);
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    PhoenixVesselAccountsBOW.CrewAppraisalCheck(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , signonid, General.GetNullableDateTime(txtSignOffDate.Text));
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=7&showexcel=no&showword=no&reportcode=SIGNOFFBOW&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&employee=" + Request.QueryString["EMPID"] + "&date=" + (string.IsNullOrEmpty(txtSignOffDate.Text) ? DateTime.Now.ToString() : txtSignOffDate.Text) + "&signonoffid=" + signonid);
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string paidwages = "1,";
                foreach (ButtonListItem item in chbxlstPaidWages.Items)
                {
                    if (item.Selected == true)
                        paidwages += "1,";
                    else
                        paidwages += "0,";
                }

                decimal currentbalance = String.IsNullOrEmpty(General.GetNullableDecimal(txtBalance.Text).ToString()) ? 0 : decimal.Parse(txtBalance.Text);
                int signonid1 = int.Parse(ViewState["SIGNONID"].ToString());
                if (!IsValidSignOff(txtSignOffDate.Text, ddlSeaPort.SelectedValue, ddlReason.SelectedSignOffReason, txtAmountCash.Text.Trim(), txtAmountAllot.Text.Trim(), ddlBankAccount.SelectedBankAccount, currentbalance, txtRoundOff.Text.Trim()))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsEmployee.UpdateVesselSignOffDetails(signonid1
                                                                   , DateTime.Parse(txtSignOffDate.Text.Trim())
                                                                   , int.Parse(ddlSeaPort.SelectedValue)
                                                                   , int.Parse(ddlReason.SelectedSignOffReason)
                                                                   , txtRemarks.Text.Trim()
                                                                   , paidwages.TrimEnd(',')
                                                                   , 1
                                                                   , General.GetNullableDecimal(txtAmountCash.Text.Trim())
                                                                   , (!txtAmountAllot.Enabled ? null : General.GetNullableDecimal(txtAmountAllot.Text.Trim()))
                                                                   , (!txtAmountAllot.Enabled ? null : General.GetNullableGuid(ddlBankAccount.SelectedBankAccount))
                                                                   , byte.Parse(chkRecoveryYn.Checked == true ? "1" : "0"));
                string Script = "";
                Script += "<script language='JavaScript' id='BookMarkScript1'>";
                Script += "fnReloadList();";
                Script += "</script>";

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript1", Script, false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void chbxlstPaidWages_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            RadCheckBoxList chk = (RadCheckBoxList)sender;
            string paidwages = "1,";
            foreach (ButtonListItem item in chk.Items)
            {
                if (item.Selected == true)
                    paidwages += "1,";
                else
                    paidwages += "0,";
            }

            int signonid = int.Parse(ViewState["SIGNONID"].ToString());

            PhoenixVesselAccountsEmployee.UpdateVesselCancelSignOffAllowence(signonid, paidwages.TrimEnd(','), 1, General.GetNullableDateTime(txtSignOffDate.Text));
            EditSignOffdetails();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo','keepopen');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidSignOff(string date, string SeaPort, string reason, string cashamt, string allotamt, string bank, decimal currbalance, string roundoff)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Sign Off Date is required.";
        }
        if (General.GetNullableInteger(SeaPort) == null)
        {
            ucError.ErrorMessage = "Sign Off Port is required.";
        }
        if (!General.GetNullableInteger(reason).HasValue)
        {
            ucError.ErrorMessage = "Sign Off Reason is required.";
        }
        if (General.GetNullableDecimal(cashamt).HasValue && General.GetNullableDecimal(cashamt).Value < 0)
        {
            ucError.ErrorMessage = "Pay as cash advance not allow negative values";
        }
        if (General.GetNullableDecimal(allotamt).HasValue && General.GetNullableDecimal(allotamt).Value < 0)
        {
            ucError.ErrorMessage = "Pay as Allotment not allow negative values";
        }
        else if (!String.IsNullOrEmpty(allotamt))
        {
            if (string.IsNullOrEmpty(bank))
            {
                ucError.ErrorMessage = "Bank is required for Allotment";
            }
        }
        if (General.GetNullableDecimal(roundoff).HasValue)
        {
            decimal round = String.IsNullOrEmpty(roundoff) ? 0 : decimal.Parse(roundoff);
            if (round != 0)
            {
                if (round > 1 || round < -1)
                    ucError.ErrorMessage = "Round Off Amount should be +/- 1.00";
            }
        }
        return (!ucError.IsError);
    }

    private bool IsValidSignOffDateUpdate(string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Sign Off Date is required.";
        }
        return (!ucError.IsError);
    }

    protected void txtCashAmount_TextChanged(object sender, EventArgs e)
    {
        decimal paid = String.IsNullOrEmpty(General.GetNullableDecimal(txtAmountCash.Text).ToString()) ? 0 : decimal.Parse(txtAmountCash.Text);
        paid += String.IsNullOrEmpty(General.GetNullableDecimal(txtAmountAllot.Text).ToString()) ? 0 : decimal.Parse(txtAmountAllot.Text);
        decimal bal = String.IsNullOrEmpty(General.GetNullableDecimal(txtBalance.Text).ToString()) ? 0 : decimal.Parse(txtBalance.Text);
        txtRoundOff.Text = (bal - paid).ToString();

    }

    protected void txtAllotAmount_TextChanged(object sender, EventArgs e)
    {

        decimal paid = String.IsNullOrEmpty(General.GetNullableDecimal(txtAmountCash.Text).ToString()) ? 0 : decimal.Parse(txtAmountCash.Text);
        paid += String.IsNullOrEmpty(General.GetNullableDecimal(txtAmountAllot.Text).ToString()) ? 0 : decimal.Parse(txtAmountAllot.Text);
        decimal bal = String.IsNullOrEmpty(General.GetNullableDecimal(txtBalance.Text).ToString()) ? 0 : decimal.Parse(txtBalance.Text);
        txtRoundOff.Text = (bal - paid).ToString();
    }

    protected void cmdDelete_Click(object sender, EventArgs e)
    {

        try
        {
            PhoenixVesselAccountsEarningDeduction.VesselEarningDeductionDelete(new Guid(cmdDelete.CommandArgument));
            EditSignOffdetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSignOffDateDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixVesselAccountsEarningDeduction.UpdateVesselAccountSignOffDate(PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(cmdSignOffDateDelete.CommandArgument), int.Parse(ViewState["SIGNONID"].ToString()));
            EditSignOffdetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdCalculateWage_Click(object sender, EventArgs e)
    {
        try
        {
            int signonid = int.Parse(ViewState["SIGNONID"].ToString());
            string paidwages = "1,";
            foreach (ButtonListItem item in chbxlstPaidWages.Items)
            {
                if (item.Selected == true)
                    paidwages += "1,";
                else
                    paidwages += "0,";
            }
            if (ViewState["BNDSUBSIDY"].ToString() == "1")
            {
                PhoenixVesselAccountsBondSubsidy.UpdateBondSubsidy(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            }
            PhoenixVesselAccountsEmployee.UpdateVesselSignOffDate(signonid
                , General.GetNullableDateTime(String.IsNullOrEmpty(txtSignOffDate.Text) ? "" : txtSignOffDate.Text.Trim())
                                                               , paidwages.TrimEnd(','));
            EditSignOffdetails();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo','keepopen');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
