using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;

public partial class AccountsAllotmentRequestDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Request", "REQUEST");
        toolbar.AddButton("LineItem", "LINEITEM");
        MenuAllotment.AccessRights = this.ViewState;
        MenuAllotment.MenuList = toolbar.Show();

        MenuAllotment.SelectedMenuIndex = 1;
        try
        {

            if (!IsPostBack)
            {
                ViewState["CHECKTYPE"] = "1";

                if (!IsPostBack)
                {
                    ViewState["ALLOTMENTID"] = null;
                    ViewState["ALLOTMENTSTATUS"] = null;

                    ViewState["PBVALIDYN"] = "0";
                    ViewState["BANKVALIDYN"] = "0";
                    ViewState["REIMRECVALIDYN"] = "0";
                    ViewState["SIDELETTERVALIDYN"] = "0";
                    ViewState["ALLOTMENTTYPE"] = "";

                    if (Request.QueryString["ALLOTMENTID"] != null)
                    {
                        ViewState["ALLOTMENTID"] = Request.QueryString["ALLOTMENTID"].ToString();
                        DataTable dt = PhoenixAccountsAllotmentRequest.AllotmentRequestEdit(new Guid(ViewState["ALLOTMENTID"].ToString()));
                        if (dt.Rows.Count > 0)
                        {
                            ViewState["ALLOTMENTTYPE"] = dt.Rows[0]["FLDALLOTMENTTYPE"].ToString();
                        }
                    }
                PhoenixToolbar toolbarsub = new PhoenixToolbar();
                toolbarsub.AddButton("Checked", "CHECKEDBYACCOUNTS");
                if (ViewState["ALLOTMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 239, "MAL") || ViewState["ALLOTMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 239, "SPA") || ViewState["ALLOTMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 239, "SOF"))
                   toolbarsub.AddButton("Portage Bill", "PORTAGEBILL");
                MenuChecking.AccessRights = this.ViewState;
                MenuChecking.MenuList = toolbarsub.Show();

                BindValidations();
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = PhoenixAccountsAllotmentRequestSystemChecking.AllotmentRequestSystemCheckDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ucTitle.Text = dr["FLDREQUESTNUMBER"].ToString();
                txtFileNo.Text = dr["FLDFILENO"].ToString();
                txtRank.Text = dr["FLDRANKNAME"].ToString();
                txtName.Text = dr["FLDEMPLOYEENAME"].ToString();
                txtAllotmentType.Text = dr["FLDALLOTMENTTYPENAME"].ToString();
                txtMonthAndYear.Text = dr["FLDMONTH"].ToString() + "-" + dr["FLDYEAR"].ToString();
                txtBeneficiary.Text = dr["FLDACCOUNTNAME"].ToString();
                txtBeneficiaryBank.Text = dr["FLDBANKNAME"].ToString();
                txtAccountNo.Text = dr["FLDACCOUNTNUMBER"].ToString();
                txtBankAddress.Text = dr["FLDADDRESS1"].ToString();
                txtIFSCCode.Text = dr["FLDBANKIFSCCODE"].ToString();

                ViewState["REIMRECVALIDYN"] = dr["FLDREIMRECVALIDYN"].ToString();
                ViewState["ALLOTMENTSTATUS"] = dr["FLDREQUESTSTATUS"].ToString();

                if (ViewState["ALLOTMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 239, "MAL") || ViewState["ALLOTMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 239, "SPA") || ViewState["ALLOTMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 239, "SOF"))
                {
                    ViewState["PBVALIDYN"] = dr["FLDPBVALIDYN"].ToString();
                    ViewState["BANKVALIDYN"] = dr["FLDBANKVALIDYN"].ToString();
                    ViewState["SIDELETTERVALIDYN"] = dr["FLDSIDELETTERVALIDYN"].ToString();
                }
                txtSignoffReason.Text = dr["FLDSIGNOFFREASON"].ToString();

                if (ViewState["ALLOTMENTSTATUS"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 238, "ACC"))
                    MenuChecking.Visible = false;

                SetValidation();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindValidations()
    {
        if (ViewState["ALLOTMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 239, "MAL") || ViewState["ALLOTMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 239, "SPA") || ViewState["ALLOTMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 239, "SOF"))
        {
            rblAllotmentCheck.Items.Clear();

            ListItem liPB = new ListItem("Balance in Portage Bill", "1");
            ListItem liBank = new ListItem("Banking Details Verification", "2");
            ListItem liORR = new ListItem("Pending Reimbursement/Recoveries", "3");
            ListItem liSLT = new ListItem("Side Letter", "4");

            rblAllotmentCheck.Items.Add(liPB);
            rblAllotmentCheck.Items.Add(liBank);
            rblAllotmentCheck.Items.Add(liORR);
            rblAllotmentCheck.Items.Add(liSLT);

            rblAllotmentCheck.SelectedIndex = 0;
        }
        if (ViewState["ALLOTMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 239, " ") || ViewState["ALLOTMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 239, "ORR"))
        {
            rblAllotmentCheck.Items.Clear();
            ListItem liORR = new ListItem("Pending Reimbursement/Recoveries", "3");
            rblAllotmentCheck.Items.Add(liORR);

            rblAllotmentCheck.SelectedIndex = 0;
        }
        if (ViewState["ALLOTMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 239, "ORR"))
        {
            ListItem liBank = new ListItem("Banking Details Verification", "2");
            rblAllotmentCheck.Items.Add(liBank);
        }

        SetLink();
    }

    protected void MenuAllotment_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("REQUEST"))
            {
                Response.Redirect("AccountsAllotmentRequest.aspx?ALLOTMENTID=" + ViewState["ALLOTMENTID"].ToString() + "&ALLOTMENTTYPE=" + ViewState["ALLOTMENTTYPE"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuChecking_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper() == "CHECKEDBYACCOUNTS")
            {
                PhoenixAccountsAllotmentRequestSystemChecking.AllotmentRequestCheckedStatusChange(General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()));
                ucStatus.Visible = true;
                ucStatus.Text = "Status Changed";
                MenuChecking.Visible = false;
            }
            if (dce.CommandName.ToUpper() == "PORTAGEBILL")
            {
                Response.Redirect("../Accounts/AccountsAllotmentPortageBillingDetails.aspx?ALLOTMENTID=" + ViewState["ALLOTMENTID"].ToString()+"&ALLOTMENTTYPE=" + ViewState["ALLOTMENTTYPE"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            SetValidation();
        }

    }
    protected void rblAllotmentCheck_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetLink();
    }

    private void SetValidation()
    {
        foreach (ListItem item in rblAllotmentCheck.Items)
        {
            if (item.Value == "1")
            {
                if (ViewState["PBVALIDYN"].ToString() == "0")
                {
                    item.Attributes.Add("style", "color: red");
                }
            }
            if (item.Value == "2")
            {
                if (ViewState["BANKVALIDYN"].ToString() == "0")
                    item.Attributes.Add("style", "color: red");
            }
            if (item.Value == "3")
            {
                if (ViewState["REIMRECVALIDYN"].ToString() == "0")
                    item.Attributes.Add("style", "color: red");
            }
            if (item.Value == "4")
            {
                if (ViewState["SIDELETTERVALIDYN"].ToString() == "0")
                    item.Attributes.Add("style", "color: red");
            }
        }
    }
    private void SetLink()
    {
        foreach (ListItem item in rblAllotmentCheck.Items)
        {
            if (item.Selected == true)
            {
                if (item.Value == "1")
                {

                    ViewState["CHECKTYPE"] = "1";
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAllotmentRequestPBBalance.aspx?ALLOTMENTID=" + ViewState["ALLOTMENTID"].ToString();
                    break;
                }
                if (item.Value == "2")
                {
                    ViewState["CHECKTYPE"] = "2";

                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAllotmentRequestBankVerification.aspx?ALLOTMENTID=" + ViewState["ALLOTMENTID"].ToString();
                    break;
                }
                if (item.Value == "3")
                {
                    ViewState["CHECKTYPE"] = "3";
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAllotmentRequesPendingReimbRecoveries.aspx?ALLOTMENTID=" + ViewState["ALLOTMENTID"].ToString();
                    break;
                }
                if (item.Value == "4")
                {
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsAllotmentRequestSideLetter.aspx?ALLOTMENTID=" + ViewState["ALLOTMENTID"].ToString();
                }
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            SetValidation();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
