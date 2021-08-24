using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using System.Collections.Specialized;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class Accounts_AccountsCrewBankAccountList : PhoenixBasePage
{
    string id = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            id = Request.QueryString["id"];
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuAccountsBankAccountList.AccessRights = this.ViewState;
                MenuAccountsBankAccountList.MenuList = toolbar.Show();
                ViewState["FLDACTIVEYN"] = null;

                if (!string.IsNullOrEmpty(id))
                {
                    EditAccountsBankAccount(new Guid(id));
                }
                else
                    ResetAccountsBankAccount();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void EditAccountsBankAccount(Guid bankaccountid)
    {
        try
        {
            DataTable dt = PheonixAccountCrewBankAccount.EditAccountsBankAccount(bankaccountid);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtFileno.Text = dr["FLDFILENO"].ToString();
                txtRank.Text = dr["FLDRANKNAME"].ToString();
                txtEmployee.Text = dr["FLDEMPLOYEENAME"].ToString();
                txtAccountType.Text = dr["FLDACCOUNTTYPENAME"].ToString();
                txtEmployeeId.Text = dr["FLDEMPLOYEEID"].ToString();
                txtAccountName.Text = dr["FLDACCOUNTNAME"].ToString();
                txtAccountNumber.Text = dr["FLDACCOUNTNUMBER"].ToString();
                txtBankName.Text = dr["FLDBANKNAME"].ToString();
                txtSeafarerBankSwiftCode.Text = dr["FLDBANKSWIFTCODE"].ToString();
                txtBankIFSCCode.Text = dt.Rows[0]["FLDBANKIFSCCODE"].ToString();
                txtBankCurrency.Text = dt.Rows[0]["FLDCURRENCYNAME"].ToString();

                if (dt.Rows[0]["FLDACTIVEYN"].ToString() == "0")
                    chkInActiveYN.Checked = true;
                ViewState["FLDACTIVEYN"] = dt.Rows[0]["FLDACTIVEYN"].ToString();
                txtInActiveRemarks.Text = dt.Rows[0]["FLDINACTIVEREMARKS"].ToString();
                txtVerifiedDate.Text = dt.Rows[0]["FLDVERIFIEDDATE"].ToString();
                txtVerifiedBy.Text = dt.Rows[0]["FLDVERIFIEDBYNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void ResetAccountsBankAccount()
    {
        txtAccountType.Text = "";
        txtAccountName.Text = "";
        txtAccountNumber.Text = "";
        txtBankName.Text = "";
        txtSeafarerBankSwiftCode.Text = "";
        txtBankIFSCCode.Text = "";
        txtBankCurrency.Text = "";
        txtInActiveRemarks.Text = "";
        txtVerifiedDate.Text = "";
        txtVerifiedBy.Text = "";
    }

    protected void AccountsBankAccountList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
            String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidAccountsBankAccount())
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        PheonixAccountCrewBankAccount.UpdateBankdetail(new Guid(id)
                            , DateTime.Parse(txtVerifiedDate.Text), int.Parse(txtEmployeeId.Text)
                             , General.GetNullableString(txtInActiveRemarks.Text)
                            , General.GetNullableInteger(chkInActiveYN.Checked == true ? "0" : "1")
                         );
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                               "BokMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                       // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private bool IsValidAccountsBankAccount()
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";
        if ((ViewState["FLDACTIVEYN"].ToString() != (chkInActiveYN.Checked == true ? "0" : "1")) || chkInActiveYN.Checked == true)
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

