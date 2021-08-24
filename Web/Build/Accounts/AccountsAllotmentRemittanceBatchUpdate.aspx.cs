using SouthNests.Phoenix.Framework;
using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceBatchUpdate : PhoenixBasePage
{
    public int iCompanyid;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Update", "BATCHUPDATE",ToolBarDirection.Right);
            MenuBatchUpdate.AccessRights = this.ViewState;
            MenuBatchUpdate.MenuList = toolbarmain.Show();
            iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            if (!IsPostBack)
            {
                ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyid);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuBatchUpdate_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("BATCHUPDATE"))
            {

                if (!IsValidRemittance(ddlBankAccount.SelectedBankAccount, ddlPaymentmode.SelectedHard, ddlBankChargebasis.SelectedHard))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsAllotmentRemittance.BatchRemittanceUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(txtAccountId.Text), General.GetNullableDateTime(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text), int.Parse(ddlPaymentmode.SelectedHard), int.Parse(ddlBankAccount.SelectedBankAccount), General.GetNullableInteger(ddlBankChargebasis.SelectedHard), Convert.ToInt16(chkDRCurrency.Checked == true ? 1 : 0), General.GetNullableInteger(txtCurrencyId.Text), txtSubAccountCode.Text);

                //String scriptinsert = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

                //String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);

                string Script = "";
                Script += "<script language=JavaScript id='BookMarkScript1'>" + "\n";
                Script += "fnReloadList();";
                Script += "</script>" + "\n";

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    //protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    //{

    //}
    protected void ddlBankAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBankAccount.SelectedBankAccount.ToUpper() != "DUMMY")
        {
            DataSet ds = PhoenixRegistersAccount.ListBankAccount(Convert.ToInt32(ddlBankAccount.SelectedBankAccount.ToString()), null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID,0);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtCurrencyId.Text = dr["FLDBANKCURRENCYID"].ToString();
                //txtCurrencyCode.Text = dr["FLDCURRENCYCODE"].ToString();
                txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
                txtSubAccountCode.Text = dr["FLDSUBACCOUNT"].ToString();
            }
        }
    }

    private bool IsValidRemittance(string accountcode, string strPaymentmode, string strBankchargebasis)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int result;
        if (strPaymentmode.Trim().Equals("Dummy") || strPaymentmode.Trim().Equals(""))
        {
            if (int.TryParse(strPaymentmode, out result) == false)
                ucError.ErrorMessage = "Please select payment mode.";
        }
        if (accountcode.Trim().Equals("Dummy") || accountcode.Trim().Equals(""))
        {
            if (int.TryParse(accountcode, out result) == false)
                ucError.ErrorMessage = "Please select account code.";
        }
        if (strBankchargebasis.Trim().Equals("Dummy") || strBankchargebasis.Trim().Equals(""))
        {
            if (int.TryParse(strBankchargebasis, out result) == false)
                ucError.ErrorMessage = "Please select bank charge basis.";
        }

        //if (strPaymentmode.Trim().Equals("650"))
        //{
        //    if (strBankchargebasis == "656" || strBankchargebasis == "657")
        //        ucError.ErrorMessage = "Please select bank charge basis as OUR for Payment mode Cheque.";
        //}
        return (!ucError.IsError);
    }
}
