using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceRequest : PhoenixBasePage
{
    public int iCompanyid;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            
            iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            if (!IsPostBack)
            {
                ViewState["REMITTANCEID"] = "";
                if ((Request.QueryString["REMITTANCEID"] != null) && (Request.QueryString["REMITTANCEID"] != ""))
                {
                    ViewState["REMITTANCEID"] = Request.QueryString["REMITTANCEID"].ToString();
                    BindHeader(ViewState["REMITTANCEID"].ToString());
                }
                else
                {
                    ViewState["MODE"] = "ADD";
                    ddlBankAccount.Enabled = true;
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["Indexno"] = Request.QueryString["gvindex"];
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarmain.AddButton("Verified", "VERIFIED", ToolBarDirection.Right);
            //toolbarmain.AddButton("Submit for MD Approval", "SUBMITFORMDAPPROVAL");
            if (ViewState["REMITTANCEID"] != null)
            {
                MenuOrderFormMain.Title = "Remittance      (" + PhoenixAccountsVoucher.VoucherNumber + ")     ";
            }
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindHeader(string remittanceid)
    {
        DataSet ds = PhoenixAccountsAllotmentRemittance.Editremittance(remittanceid);
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtCurrencyId.Text = dr["FLDCURRENCY"].ToString();
            txtCurrencyCode.Text = dr["FLDPAYMENTVOUCHERCURRENCY"].ToString();
          
            txtEmployeeCode.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtEmployeeName.Text = dr["FLDSUPPLIERNAME"].ToString();
            lblEmployeeId.Text = dr["FLDSUPPLIERID"].ToString();
            ddlPaymentmode.SelectedHard = dr["FLDPAYMENTMODE"].ToString();
            ddlBankChargebasis.SelectedHard = dr["FLDBANKCHARGEBASIS"].ToString();
            txtPaymentVoucherAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDPAYMENTVOUCHERAMOUNT"]));

            txtBeneficiaryName.Text = dr["FLDACCOUNTNAME"].ToString();
            txtAccountNumber.Text = dr["FLDACCOUNTNUMBER"].ToString();
            txtBankAddress.Text = dr["FLDBANKADDRESS"].ToString();
            txtBeneficiaryBankName.Text = dr["FLDBANKNAME"].ToString();
            txtIFSCCode.Text = dr["FLDBANKIFSCCODE"].ToString();
            txtBankCurrency.Text = dr["FLDEMPLOYEECURRENCY"].ToString();
            txtAccountNumber.Text = dr["FLDACCOUNTNUMBER"].ToString();
            chkDRCurrency.Checked = dr["FLDREMITINDRCURRENCY"].ToString() == "1" ? true : false;
            txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
            txtRemittanceCurrency.Text = dr["FLDREMITTANCECURRENCY"].ToString();
            txtRemittanceAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDREMITTANCEAMOUNT"]));
            txtVoucherNumber.Text = dr["FLDVOUCHERNUMBER"].ToString();

            if (dr["FLDISBANKACCOUNTACTIVE"].ToString() == "0")
            {               
                ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyid, 1);
            }
            else
            {               
                ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyid, 0);
            }
            ddlBankAccount.SelectedBankAccount = dr["FLDSUBACCOUNTID"].ToString();

            ddlPaymentmode.Enabled = false;
            ddlBankAccount.Enabled = false;
            ddlBankChargebasis.Enabled = false;
            if (dr["FLDCURRENCYCONVERTERYN"].ToString().Equals("1"))
            {
                imgCurrencyConverter.Visible = true;
                imgCurrencyConverter.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Accounts/AccountsAllotmentRemittanceCurrencyConverter.aspx?REMITTANCEID=" + ViewState["REMITTANCEID"].ToString() + "&ACCESSEDFROM=0');return false;");
            }
            if (dr["FLDREMITTANCESTATUS"].ToString() != "678")
            {
                ddlPaymentmode.Enabled = true;
                ddlBankAccount.Enabled = true;
                ddlBankChargebasis.Enabled = true;
            }
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["REMITTANCEID"] == null)
                {
                    if (!IsValidRemittance(ddlBankAccount.SelectedBankAccount, ddlPaymentmode.SelectedHard, ddlBankChargebasis.SelectedHard))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    string iRemittenceNumber = "";
                    PhoenixAccountsAllotmentRemittance.InsertRemittance(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                            General.GetNullableInteger(txtAccountId.Text),
                                                                            General.GetNullableInteger(txtCurrencyId.Text),
                                                                            46, // AWAITING APPROVAL
                                                                            ref iRemittenceNumber,
                                                                            txtSubAccountCode.Text,
                                                                            int.Parse(ddlBankAccount.SelectedBankAccount),
                                                                            int.Parse(lblEmployeeId.Text),
                                                                            int.Parse(ddlPaymentmode.SelectedHard),
                                                                            General.GetNullableInteger(ddlBankChargebasis.SelectedHard),
                                                                            PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                       );
                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                    //Session["New"] = "Y";
                }
                else
                {
                    if (!IsValidRemittance(ddlBankAccount.SelectedBankAccount, ddlPaymentmode.SelectedHard, ddlBankChargebasis.SelectedHard))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixAccountsAllotmentRemittance.UpdateRemittance(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                General.GetNullableInteger(txtAccountId.Text),
                                                                General.GetNullableInteger(ddlBankAccount.SelectedBankAccount),
                                                                txtSubAccountCode.Text,
                                                                General.GetNullableInteger(txtCurrencyId.Text),
                                                                ViewState["REMITTANCEID"].ToString(),
                                                                int.Parse(ddlPaymentmode.SelectedHard),
                                                                General.GetNullableInteger(ddlBankChargebasis.SelectedHard)
                                                                , Convert.ToInt16(chkDRCurrency.Checked == true ? 1 : 0));


                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                    //Session["New"] = "Y";
                }
            }
            if (CommandName.ToUpper().Equals("VERIFIED"))
            {
                if (ViewState["REMITTANCEID"] != null)
                {
                    PhoenixAccountsAllotmentRemittance.ApproveRemittance(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["REMITTANCEID"].ToString());
                    ucStatus.Text = "Remittance details verified.";

                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                }
            }
            //if (dce.CommandName.ToUpper().Equals("SUBMITFORMDAPPROVAL"))
            //{
            //    PhoenixAccountsAllotmentRemittance.PrepareRemittanceInstruction(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            //    String scriptinsert = String.Format("javascript:fnReloadList('codehelp1');");
            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    void Reset()
    {
        txtEmployeeCode.Text = "";
        txtEmployeeName.Text = "";
        lblEmployeeId.Text = "";
        ddlBankAccount.SelectedBankAccount = null;
        ddlBankAccount.Enabled = true;
        txtCurrencyCode.Text = "";
        ddlBankAccount.SelectedBankAccount = "";
        ddlPaymentmode.SelectedHard = "";
        ddlBankChargebasis.SelectedHard = "";

        txtBeneficiaryName.Text = "";
        txtAccountNumber.Text = "";
        txtBankAddress.Text = "";
        txtBeneficiaryBankName.Text = "";
        txtIFSCCode.Text = "";
        txtBankCurrency.Text = "";
        txtAccountNumber.Text = "";
        ViewState["REMITTANCEID"] = null;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRemittance(string accountcode, string strPaymentmode, string strBankchargebasis)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int result;

        if (strPaymentmode.Trim().Equals("Dummy"))
        {
            if (int.TryParse(strPaymentmode, out result) == false)
                ucError.ErrorMessage = "Please select payment mode.";
        }
        if (accountcode.Trim().Equals("Dummy"))
        {
            if (int.TryParse(accountcode, out result) == false)
                ucError.ErrorMessage = "Please select account code.";
        }
        if (strBankchargebasis.Trim().Equals("Dummy"))
        {
            if (int.TryParse(strBankchargebasis, out result) == false)
                ucError.ErrorMessage = "Please select bank charge basis.";
        }

        //if ((strPaymentmode.Trim().Equals("650")) || (strPaymentmode.Trim().Equals("651")) || (strPaymentmode.Trim().Equals("652")) || (strPaymentmode.Trim().Equals("653")) || (strPaymentmode.Trim().Equals("654")))
        //{
        //    if (strPaymentmode.Trim().Equals("650"))
        //    {
        //        if (strBankchargebasis == "656" || strBankchargebasis == "657")
        //            ucError.ErrorMessage = "Please select bank charge basis as OUR for Payment mode Cheque.";
        //    }
        //}
        return (!ucError.IsError);
    }

    protected void ddlBankAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBankAccount.SelectedBankAccount.ToUpper() != "DUMMY")
        {
            DataSet ds = PhoenixRegistersAccount.ListBankAccount(Convert.ToInt32(ddlBankAccount.SelectedBankAccount.ToString()), null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

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
}
