using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;


public partial class AccountsAirfareCreditNoteLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        txtBudgetId.Attributes.Add("style", "visibility:hidden");
        txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");
        txtAccountId.Attributes.Add("style", "visibility:hidden;");

        if (!IsPostBack)
        {
            Reset();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            if (Request.QueryString["AIRFARECREDITNOTEID"] != null && Request.QueryString["AIRFARECREDITNOTEID"] != string.Empty)
            {
                ViewState["AIRFARECREDITNOTEID"] = Request.QueryString["AIRFARECREDITNOTEID"];
            }


            if (Request.QueryString["AIRFARELINEITEMID"] != null && Request.QueryString["AIRFARELINEITEMID"] != string.Empty)
            {
                ViewState["AIRFARELINEITEMID"] = Request.QueryString["AIRFARELINEITEMID"];
                VoucherLineEdit(new Guid(ViewState["AIRFARELINEITEMID"].ToString()));
            }
            ViewState["VoucherDate"] = DateTime.Now;
            //btnShowBudget.Attributes.Add("onclick", "return showSubAccountPickList('spnPickListExpenseAccount', 'codehelp1', '','../Common/CommonPickListSubAccount.aspx', true);");
            ttlVoucher.Text = "Row Number (" + txtRowNumber.Text + ")";
        }

        toolbar.AddButton("New", "NEW");
        toolbar.AddButton("Save", "SAVE");

        MenuVoucherLineItem.AccessRights = this.ViewState;
        MenuVoucherLineItem.MenuList = toolbar.Show();
        MenuVoucherLineItem.SetTrigger(pnlVoucher);
    }

    
    protected void Voucher_SetExchangeRate(object sender, EventArgs e)
    {
        decimal dTransactionExchangerate = 0;
        if (ddlCurrencyCode.SelectedCurrency.ToUpper() != "DUMMY")
        {
            DataSet dsInvoice = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(ddlCurrencyCode.SelectedCurrency), DateTime.Parse(ViewState["VoucherDate"].ToString()));
            if (dsInvoice.Tables[0].Rows.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                dTransactionExchangerate = decimal.Parse(drInvoice["FLDEXCHANGERATE"].ToString());
                dTransactionExchangerate = Decimal.Round(dTransactionExchangerate, 6);
            }
        }

        if (ddlCurrencyCode.SelectedCurrency.ToUpper() != "DUMMY")
        {
            DataSet ds = PhoenixRegistersCompany.EditCompany(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                DataSet dsbasecurrency = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(dr["FLDBASECURRENCY"].ToString()), DateTime.Parse(ViewState["VoucherDate"].ToString()));
                DataSet dsReportcurrency = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(dr["FLDREPORTINGCURRENCY"].ToString()), DateTime.Parse(ViewState["VoucherDate"].ToString()));

                DataRow drbasecurrency = dsbasecurrency.Tables[0].Rows[0];
                DataRow drreportcurrency = dsReportcurrency.Tables[0].Rows[0];
                if (Session["VOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "Rate"] != null)
                {
                    System.Collections.Hashtable ht = new System.Collections.Hashtable();
                    ht = (System.Collections.Hashtable)Session["VOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "Rate"];
                    txtExchangeRate.Text = ht["EXCHANGERATE"].ToString();
                    txt2ndExchangeRate.Text = ht["2ndEXCHANGERATE"].ToString();
                }
                else
                {
                    txtExchangeRate.Text = string.Format(String.Format("{0:#####.00000000000000000}", dTransactionExchangerate / decimal.Parse(drbasecurrency["FLDEXCHANGERATE"].ToString())));
                    txt2ndExchangeRate.Text = string.Format(String.Format("{0:#####.00000000000000000}", dTransactionExchangerate / decimal.Parse(drreportcurrency["FLDEXCHANGERATE"].ToString())));
                    System.Collections.Hashtable htInitialRate = new System.Collections.Hashtable();
                    htInitialRate["INITIALEXCHANGERATE"] = decimal.Parse(txtExchangeRate.Text) < 1 ? '0' + txtExchangeRate.Text : txtExchangeRate.Text; ;
                    htInitialRate["INITIAL2ndEXCHANGERATE"] = decimal.Parse(txt2ndExchangeRate.Text) < 1 ? '0' + txtExchangeRate.Text : txtExchangeRate.Text;
                    Session["VOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "INITIALRate"] = htInitialRate;
                }
            }
        }
        else
        {
            txtExchangeRate.Text = "";
            txt2ndExchangeRate.Text = "";
        }
    }

    protected void Voucher_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidLineItem())
            {
                ucError.Visible = true;
                return;
            }
            if (ViewState["AIRFARELINEITEMID"] == null)
            {
                try
                {

                    PhoenixAccountsAirfareCreditNote.AirfareCreditNoteLineItemInsert(
                                        new Guid(ViewState["AIRFARECREDITNOTEID"].ToString())
                                        , decimal.Parse(txtPrimeAmoutEdit.Text)
                                        , General.GetNullableString(txtLongDescription.Text)
                                        , int.Parse(txtAccountId.Text)
                                        , null
                                        , int.Parse(ddlCurrencyCode.SelectedCurrency)
                                        , decimal.Parse(txtExchangeRate.Text)
                                        , decimal.Parse(txt2ndExchangeRate.Text)
                                        , General.GetNullableString(txtChequeno.Text)
                                        , General.GetNullableGuid(txtBudgetId.Text)
                                        , null
                                        , General.GetNullableString(txtBudgetCode.Text)
                                        , General.GetNullableString(txtBudgetName.Text)
                        );
                    String script = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

                    ucStatus.Text = "Voucher Line Item information added";
                    Reset();
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            else
            {
                try
                {
                    PhoenixAccountsAirfareCreditNote.AirfareCreditNoteLineItemUpdate(
                                        new Guid(ViewState["AIRFARELINEITEMID"].ToString())
                                        , decimal.Parse(txtPrimeAmoutEdit.Text)
                                        , General.GetNullableString(txtLongDescription.Text)
                                        , int.Parse(txtAccountId.Text)
                                        , null
                                        , int.Parse(ddlCurrencyCode.SelectedCurrency)
                                        , decimal.Parse(txtExchangeRate.Text)
                                        , decimal.Parse(txt2ndExchangeRate.Text)
                                        , General.GetNullableString(txtChequeno.Text)
                                        , General.GetNullableGuid(txtBudgetId.Text)
                                        , null
                                        , General.GetNullableString(txtBudgetCode.Text)
                                        , General.GetNullableString(txtBudgetName.Text)
                        );
                                      
                    String script = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

                    ucStatus.Text = "Voucher Line Item information updated";
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
        }
        else if (dce.CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
        }
    }

    protected void Reset()
    {
        ViewState["AIRFARELINEITEMID"] = null;
        txtRowNumber.Text = "";
        txtAccountId.Text = "";
        txtAccountUsage.Text = "";
        txtAccountSource.Text = "";
        txtAccountCode.Text = "";
        txtAccountDescription.Text = "";
        txtBudgetId.Text = "";
        ddlCurrencyCode.SelectedCurrency = "";
        txtExchangeRate.Text = "";
        txt2ndExchangeRate.Text = "";
        
        txtPrimeAmoutEdit.Text = "";
        txtChequeno.Text = "";
        txtLongDescription.Text = "";
        txtUpdatedBy.Text = "";
        txtUpdatedDate.Text = "";
        txtBudgetCode.Text = "";
        txtBudgetName.Text = "";
        txtBudgetId.Text = "";
        txtBudgetgroupId.Text = "";
        if (Session["SelectedAccountId"] != null)
            Session.Remove("SelectedAccountId");
        if (Session["sAccountCode"] != null)
            Session.Remove("sAccountCode");
        if (Session["sAccountCodeDescription"] != null)
            Session.Remove("sAccountCodeDescription");
        txtExchangeRate.Text = "";
        txt2ndExchangeRate.Text = "";
        ddlCurrencyCode.SelectedCurrency = "";

        ViewState["ISACCOUNTACTIVE"] = "1";
    }

    protected void EditVoucherLineItem(object sender, CommandEventArgs e)
    {
        string[] strValues = new string[2];
        strValues = e.CommandArgument.ToString().Split('^');
        VoucherLineEdit(new Guid(strValues[0]));
    }

    protected void VoucherLineEdit(Guid gLineId)
    {
        ViewState["AIRFARELINEITEMID"] = gLineId.ToString();
        DataSet ds = PhoenixAccountsAirfareCreditNote.AirfareCreditNoteLineItemEdit(gLineId);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtRowNumber.Text = dr["FLDVOUCHERLINEITEMNO"].ToString();
                txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
                txtAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
                txtAccountDescription.Text = dr["FLDACCOUNTDESCRIPTION"].ToString();
                txtBudgetId.Text = dr["FLDSUBACCOUNTMAPID"].ToString();
                txtBudgetCode.Text = dr["FLDSUBACCOUNT"].ToString();
                txtBudgetName.Text = dr["FLDSUBACCOUNTDESCRIPTION"].ToString(); 
                txtAccountSource.Text = dr["FLDACCOUNTSOURCENAME"].ToString();
                txtAccountUsage.Text = dr["FLDACCOUNTUSAGENAME"].ToString();
                ddlCurrencyCode.SelectedCurrency = dr["FLDCURRENCYCODE"].ToString();
                txtExchangeRate.Text = string.Format(String.Format("{0:#####.00000000000000000}", decimal.Parse(dr["FLDBASEEXCHANGERATE"].ToString())));
                txt2ndExchangeRate.Text = string.Format(String.Format("{0:#####.00000000000000000}", decimal.Parse(dr["FLDREPORTEXCHANGERATE"].ToString())));
                txtPrimeAmoutEdit.Text = string.Format(String.Format("{0:#####.00}", dr["FLDTRANSACTIONAMOUNT"]));
                txtChequeno.Text = dr["FLDREFERENCEDOCUMENTNO"].ToString();
                txtLongDescription.Text = dr["FLDDESCRIPTION"].ToString();
                txtUpdatedBy.Text = dr["FLDUPDATEDBYUSERNAME"].ToString();
                txtUpdatedDate.Text = dr["FLDMODIFIEDDATE"].ToString();
                ViewState["ISACCOUNTACTIVE"] = dr["FLDACCOUNTACTIVEYN"].ToString();
                ViewState["COMPANYID"] = dr["FLDCOMPANYID"].ToString();
                Session["SelectedAccountId"] = dr["FLDACCOUNTID"].ToString();
                ViewState["VoucherDate"] = dr["FLDVOUCHERDATE"].ToString();
                txtVoucherNumber.Text = dr["FLDCNREGISTERNO"].ToString();

                txtPrimeAmoutEdit.Focus();
            }

        }

    }

    public bool IsValidLineItem()
    {
        if (txtAccountId.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Account is required.";
        else
        {
            DataSet dsaccount = PhoenixRegistersAccount.EditAccount(Convert.ToInt32(txtAccountId.Text));
            if (dsaccount.Tables[0].Rows.Count > 0)
            {
                if (dsaccount.Tables[0].Rows[0]["FLDACCOUNTUSAGE"].ToString() != "78" && dsaccount.Tables[0].Rows[0]["FLDACCOUNTUSAGE"].ToString() != "460")
                {
                    if (txtBudgetId.Text.Trim().Equals(""))
                        ucError.ErrorMessage = "Sub account is required.";
                }
            }
        }
        if (ddlCurrencyCode.SelectedCurrency.ToUpper() == "DUMMY")
            ucError.ErrorMessage = "Currency is required.";
        if (txtExchangeRate.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Base exchange rate is required.";
        if (txt2ndExchangeRate.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Report exchange rate is required.";
        if (txtPrimeAmoutEdit.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Prime amount is required.";

        ucError.HeaderMessage = "Please provide the following required information";
        return (!ucError.IsError);
    }

    protected void txtExchangeRate_TextChanged(object sender, EventArgs e)
    {
        System.Collections.Hashtable ht = new System.Collections.Hashtable();
        System.Collections.Hashtable htInitialRate = new System.Collections.Hashtable();
        if (Session["VOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "INITIALRate"] != null)
        {
            htInitialRate = (System.Collections.Hashtable)Session["VOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "INITIALRate"];
            if ((txtExchangeRate.Text != "") && (txtExchangeRate.Text != htInitialRate["INITIALEXCHANGERATE"].ToString()))
            {
                ht["EXCHANGERATE"] = txtExchangeRate.Text;
                ht["2ndEXCHANGERATE"] = txt2ndExchangeRate.Text;
                Session["VOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "Rate"] = ht;
            }
        }
    }

    protected void txt2ndExchangeRate_TextChanged(object sender, EventArgs e)
    {
        Session["VOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency] = ddlCurrencyCode.SelectedCurrency;
        System.Collections.Hashtable ht = new System.Collections.Hashtable();
        System.Collections.Hashtable htInitialRate = new System.Collections.Hashtable();
        if (Session["VOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "INITIALRate"] != null)
        {
            htInitialRate = (System.Collections.Hashtable)Session["VOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "INITIALRate"];
            if ((txt2ndExchangeRate.Text != "") && (txt2ndExchangeRate.Text != htInitialRate["INITIAL2ndEXCHANGERATE"].ToString()))
            {

                ht["EXCHANGERATE"] = txtExchangeRate.Text;
                ht["2ndEXCHANGERATE"] = txt2ndExchangeRate.Text;
                Session["VOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "Rate"] = ht;
            }
        }
    }

    protected void txtAccountCode_changed(object sender, EventArgs e)
    {
        DataSet ds = PhoenixRegistersAccount.EditCompanyAccount(Convert.ToInt32(txtAccountId.Text.ToString()), int.Parse(ViewState["COMPANYID"].ToString()));
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            if (dr["FLDACCOUNTUSAGE"].ToString() == "460")
            {
                ddlCurrencyCode.SelectedCurrency = dr["FLDBANKCURRENCYID"].ToString();
                ddlCurrencyCode.Enabled = false;
                Voucher_SetExchangeRate(ddlCurrencyCode, e);
            }
            else
            {
                ddlCurrencyCode.Enabled = true;
            }

        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}

