using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsRemittanceRequest : PhoenixBasePage
{
    public int iCompanyid;
    public int ishowall;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            txtSupplierId.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
           
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            toolbarmain.AddButton("Next", "NEXT", ToolBarDirection.Right);

            iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            if (!IsPostBack)
            {
                ViewState["Remittenceid"] = "";
                if ((Request.QueryString["REMITTENCEID"] != null) && (Request.QueryString["REMITTENCEID"] != ""))
                {
                    ViewState["Remittenceid"] = Request.QueryString["REMITTENCEID"].ToString();
                    BindHeader(ViewState["Remittenceid"].ToString());
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
            //PhoenixToolbar toolbarmain1 = new PhoenixToolbar();
            if (ViewState["Remittenceid"] != null)
            {                
                MenuOrderFormMain.Title = "Remittance      (" + PhoenixAccountsVoucher.VoucherNumber + ") ";                
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
        DataSet ds = PhoenixAccountsRemittance.Editremittance(remittanceid);     


        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtCurrencyId.Text = dr["FLDCURRENCY"].ToString();
            txtCurrencyCode.Text = dr["FLDCURRENCYCODE"].ToString();
           
            txtSupplierCode.Text = dr["FLDSUPPLIERCODE"].ToString();
            txtSupplierName.Text = dr["FLDSUPPLIERNAME"].ToString();
            txtSupplierId.Text = dr["FLDSUPPLIERID"].ToString();
            ddlPaymentmode.SelectedHard = dr["FLDPAYMENTMODE"].ToString();
            ddlBankChargebasis.SelectedHard = dr["FLDBANKCHARGEBASIS"].ToString();
            txtAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDREMITTANCEAMOUNT"]));
            txtBeneficiaryBankName.Text = dr["FLDBANKNAME"].ToString();
            txtBenficiaryBankSWIFTCode.Text = dr["FLDSWIFTCODE"].ToString();
            txtIntermediaryBankSWIFTCode.Text = dr["FLDISWIFTCODE"].ToString();
            txtBeneficiaryName.Text = dr["FLDBENEFICIARYNAME"].ToString();
            txtBenficiaryBankCode.Text = dr["FLDBANKCODE"].ToString();
            txtIntermediaryBankName.Text = dr["FLDIBANKNAME"].ToString();
            txtAccountNumber.Text = dr["FLDACCOUNTNUMBER"].ToString();
            txtBenficiaryBranchCode.Text = dr["FLDBRANCHCODE"].ToString();
            txtIntermediaryBankAccountNumber.Text = dr["FLDIACCOUNTNUMBER"].ToString();
            txtIBANNumber.Text = dr["FLDIBANNUMBER"].ToString();
            txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
            txtAccountVoucherNumber.Text = dr["FLDACCOUNTVOUCHERNUMBER"].ToString();

            ddlPaymentmode.Enabled = false;
            ddlBankAccount.Enabled = false;
            ddlBankChargebasis.Enabled = false;


            if (dr["FLDISBANKACCOUNTACTIVE"].ToString() == "0")
            {
                ishowall = 1;
                ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyid, 1);
            }
            else
            {
                ishowall = 0;
                ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyid, 0);
            }
            ddlBankAccount.SelectedBankAccount = dr["FLDSUBACCOUNTID"].ToString();

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
                if (ViewState["Remittenceid"] == null)
                {
                    if (!IsValidRemittance(ddlBankAccount.SelectedBankAccount, txtSupplierId.Text, ddlPaymentmode.SelectedHard, ddlBankChargebasis.SelectedHard))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    string iRemittenceNumber = "";
                    PhoenixAccountsRemittance.InsertRemittance(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                            General.GetNullableInteger(txtAccountId.Text),
                                                                            General.GetNullableInteger(txtCurrencyId.Text),
                                                                            46, // AWAITING APPROVAL
                                                                            ref iRemittenceNumber,
                                                                            txtSubAccountCode.Text,
                                                                            int.Parse(ddlBankAccount.SelectedBankAccount),
                                                                            int.Parse(txtSupplierId.Text),
                                                                            int.Parse(ddlPaymentmode.SelectedHard),
                                                                            General.GetNullableInteger(ddlBankChargebasis.SelectedHard),
                                                                            PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                       );
                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                    Session["New"] = "Y";
                }
                else
                {
                    if (!IsValidRemittance(ddlBankAccount.SelectedBankAccount, txtSupplierId.Text, ddlPaymentmode.SelectedHard, ddlBankChargebasis.SelectedHard))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixAccountsRemittance.UpdateRemittance(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                General.GetNullableInteger(txtAccountId.Text),
                                                                General.GetNullableInteger(ddlBankAccount.SelectedBankAccount),
                                                                txtSubAccountCode.Text,
                                                                General.GetNullableInteger(txtCurrencyId.Text),
                                                                ViewState["Remittenceid"].ToString(),
                                                                int.Parse(ddlPaymentmode.SelectedHard),
                                                                General.GetNullableInteger(ddlBankChargebasis.SelectedHard));


                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                }
            }
            if (CommandName.ToUpper().Equals("NEXT"))
            {
                int index = 0;
                index = Convert.ToInt32(ViewState["Indexno"].ToString());
                index += 1;
                ViewState["Indexno"] = index;
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("gvindexno", ViewState["Indexno"].ToString());
                Filter.CurrentRemittencegvindex = criteria;
                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    void Reset()
    {
        txtSupplierCode.Text = "";
        txtSupplierName.Text = "";
        txtSupplierId.Text = "";
        ddlBankAccount.SelectedBankAccount = null;
        ddlBankAccount.Enabled = true;
        txtCurrencyCode.Text = "";
        ddlBankAccount.SelectedBankAccount = "";
        txtSupplierCode.Text = "";
        txtSupplierName.Text = "";
        txtSupplierId.Text = "";
        ddlPaymentmode.SelectedHard = "";
        ddlBankChargebasis.SelectedHard = "";
        txtAmount.Text = "";
        txtBeneficiaryBankName.Text = "";
        txtBenficiaryBankSWIFTCode.Text = "";
        txtIntermediaryBankSWIFTCode.Text = "";
        txtBeneficiaryName.Text = "";
        txtBenficiaryBankCode.Text = "";
        txtIntermediaryBankName.Text = "";
        txtAccountNumber.Text = "";
        txtBenficiaryBranchCode.Text = "";
        txtIntermediaryBankAccountNumber.Text = "";
        txtIBANNumber.Text = "";
        ViewState["Remittenceid"] = null;
    }

    public void SetDefault(object sender, EventArgs e)
    {
        ddlBankAccount.SelectedBankAccount = "";
        ddlBankChargebasis.SelectedHard = "";

        if (ddlPaymentmode.SelectedHard == "655" || ddlPaymentmode.SelectedHard == "650" || ddlPaymentmode.SelectedHard == "652" || ddlPaymentmode.SelectedHard == "654")
            ddlBankChargebasis.SelectedHard = "658";
        else
        {
            if (ViewState["Remittenceid"].ToString() != "")
            {
                DataSet ds = PhoenixAccountsRemittance.Editremittance(ViewState["Remittenceid"].ToString());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ddlBankChargebasis.SelectedHard = dr["FLDSUPPLIERBANKCHARGEBASIS"].ToString();
                }
            }
            else
                ddlBankChargebasis.SelectedHard = "";
        }

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

    private bool IsValidRemittance(string accountcode, string strSupplierId, string strPaymentmode, string strBankchargebasis)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int result;
        if (strSupplierId.Trim().Equals(""))
        {
            if (int.TryParse(strSupplierId, out result) == false)
                ucError.ErrorMessage = "Please select supplier.";
        }
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

        if ((strPaymentmode.Trim().Equals("650")) || (strPaymentmode.Trim().Equals("651")) || (strPaymentmode.Trim().Equals("652")) || (strPaymentmode.Trim().Equals("653")) || (strPaymentmode.Trim().Equals("654")))
        {
            if (strBankchargebasis.Trim().Equals("Dummy"))
            {
                if (int.TryParse(strBankchargebasis, out result) == false)
                    ucError.ErrorMessage = "Please select bank charge basis.";
            }

            if (strPaymentmode.Trim().Equals("650"))
            {
                if (strBankchargebasis == "656" || strBankchargebasis == "657")
                    ucError.ErrorMessage = "Please select bank charge basis as OUR for Payment mode Cheque.";
            }
        }
        return (!ucError.IsError);
    }

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
}
