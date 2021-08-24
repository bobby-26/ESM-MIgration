using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class AccountsOffSettingContraVocLineSubAccount : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        txtBudgetId.Attributes.Add("style", "visibility:hidden");
        txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");
        txtAccountId.Attributes.Add("style", "visibility:hidden;");
        txtAccountSource.ReadOnly = true;
        txtAccountUsage.ReadOnly = true;
        Reset();
        if (!IsPostBack)
        {
           
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            if (Request.QueryString["voucherlineitemcode"] != null && Request.QueryString["voucherlineitemcode"] != string.Empty)
            {
                ViewState["VOUCHERLINEITEMCODE"] = Request.QueryString["voucherlineitemcode"];
                VoucherLineEdit(new Guid(ViewState["VOUCHERLINEITEMCODE"].ToString()));
            }
            if (Request.QueryString["offsettinglineitemid"] != null && Request.QueryString["offsettinglineitemid"] != string.Empty)
            {
                ViewState["OFFSETTINGLINEITEMID"] = Request.QueryString["offsettinglineitemid"];              
            }           
            if (Request.QueryString["newvoucherid"] != null && Request.QueryString["newvoucherid"] != string.Empty)
                ViewState["newvoucherid"] = Request.QueryString["newvoucherid"];            
        //    ttlVoucher.Text = "Row Number (" + txtRowNumber.Text + ")";
            ViewState["VOUCHERID"] = Request.QueryString["qvouchercode"];
            VoucherEdit();
        }

        if (Request.QueryString["offsetisposted"] != null && Request.QueryString["offsetisposted"] != string.Empty)
            ViewState["offsetisposted"] = Request.QueryString["offsetisposted"];
        int posted = int.Parse(ViewState["offsetisposted"] != null ? ViewState["offsetisposted"].ToString() : "0");
        if (ViewState["ISACCOUNTACTIVE"] != null)
        {
            if (ViewState["ISACCOUNTACTIVE"].ToString() == "1")
            {
                if (posted != 1)
                {
                    toolbar.AddButton("Post", "POST", ToolBarDirection.Right);
                    toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    toolbar.AddButton("New", "NEW", ToolBarDirection.Right);                    
                
                    MenuVoucherLineItem.AccessRights = this.ViewState;
                    MenuVoucherLineItem.MenuList = toolbar.Show();
                  //  MenuVoucherLineItem.SetTrigger(pnlVoucher);
                }
            }
        }
        else
        {
            if (posted != 1)
            {
                toolbar.AddButton("Post", "POST", ToolBarDirection.Right);
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
                MenuVoucherLineItem.AccessRights = this.ViewState;
                MenuVoucherLineItem.MenuList = toolbar.Show();
              //  MenuVoucherLineItem.SetTrigger(pnlVoucher);
            }
        }         
    }

    protected void VoucherEdit()
    {
        if (ViewState["VOUCHERID"] != null && ViewState["VOUCHERID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixAccountsContraVoucher.ContraVoucherEdit(int.Parse(ViewState["VOUCHERID"].ToString()));           
            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtVoucherNumber.Text = dr["FLDVOUCHERNUMBER"].ToString();
                ViewState["VoucherDate"] = dr["FLDVOUCHERDATE"].ToString();
                if (int.Parse(dr["FLDISPERIODLOCKED"].ToString()) == 1)
                    MenuVoucherLineItem.Visible = false;
                ViewState["FLDOLDVOUCHERID"] = dr["FLDOLDVOUCHERID"].ToString();
                ViewState["FLDOLDVOUCHERLINEITEMID"] = dr["FLDOLDVOUCHERLINEITEMID"].ToString();
                ViewState["FLDCONTRAVOUCHERTYPEID"] = dr["FLDVOUCHERTYPEID"].ToString();
            }
        }
    }

    protected void Voucher_SetExchangeRate(object sender, EventArgs e)
    {
        decimal dTransactionExchangerate = 0;
        if (ddlCurrencyCode.SelectedCurrency.ToUpper() != "DUMMY")
        {
            DataSet dsInvoice = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(ddlCurrencyCode.SelectedCurrency), DateTime.Parse(ViewState["VoucherDate"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                dTransactionExchangerate = decimal.Parse(drInvoice["FLDEXCHANGERATE"].ToString());
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
                if (Session["OFFSETCONTRAVOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "Rate"] != null)
                {
                    System.Collections.Hashtable ht = new System.Collections.Hashtable();
                    ht = (System.Collections.Hashtable)Session["OFFSETCONTRAVOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "Rate"];
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
                    Session["OFFSETCONTRAVOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "INITIALRate"] = htInitialRate;
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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        Guid ivoucherlineitemid = Guid.Empty;
        if (CommandName.ToUpper().Equals("POST"))
        {
            try
            {
                if (ViewState["newvoucherid"] != null)
                    PhoenixAccountsContraVoucher.ContraVoucherPost(int.Parse(ViewState["newvoucherid"].ToString()));
                ucStatus.Text = "Contra voucher is posted";
                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidLineItem())
            {
                ucError.Visible = true;
                return;
            }
            if (ViewState["VOUCHERLINEITEMCODE"] == null)
            {
                try
                {
                    if (ViewState["OFFSETTINGLINEITEMID"] != null)
                    {
                        InsertVoucherLineItem(int.Parse(ViewState["VOUCHERID"].ToString()),
                                                int.Parse(txtAccountId.Text),
                                                int.Parse(ddlCurrencyCode.SelectedCurrency),
                                                decimal.Parse(txtExchangeRate.Text),
                                                decimal.Parse(txt2ndExchangeRate.Text),
                                                decimal.Parse(txtPrimeAmoutEdit.Text),
                                                string.Empty,
                                                string.Empty,
                                                1,
                                                1,
                                                txtChequeno.Text,
                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                string.Empty,
                                                txtLongDescription.Text,
                                                txtBudgetId.Text != "" ? General.GetNullableGuid(txtBudgetId.Text) : Guid.Empty,
                                                ref ivoucherlineitemid,
                                                ViewState["FLDOLDVOUCHERID"] != null ? General.GetNullableInteger(ViewState["FLDOLDVOUCHERID"].ToString()) : null,
                                                ViewState["FLDOLDVOUCHERLINEITEMID"] != null ? General.GetNullableGuid(ViewState["FLDOLDVOUCHERLINEITEMID"].ToString()) : null
                                                );

                        ucStatus.Text = "Voucher Line Item information added";
                    }
                   
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
                    
                    UpdateVoucherLineItem(
                                            new Guid(ViewState["VOUCHERLINEITEMCODE"].ToString()),
                                            int.Parse(txtAccountId.Text),
                                            int.Parse(ddlCurrencyCode.SelectedCurrency),
                                            decimal.Parse(txtExchangeRate.Text),
                                            decimal.Parse(txt2ndExchangeRate.Text),
                                            decimal.Parse(txtPrimeAmoutEdit.Text),
                                            string.Empty,
                                            string.Empty,
                                            1,
                                            1,
                                            txtChequeno.Text,
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                            string.Empty,
                                            txtLongDescription.Text,
                                            General.GetNullableGuid(txtBudgetId.Text));

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
        else if (CommandName.ToUpper().Equals("ADD"))
        {
            if (!IsValidLineItem())
            {
                ucError.Visible = true;
                return;
            }


            try
            {
                InsertVoucherLineItem(int.Parse(ViewState["VOUCHERID"].ToString()),
                                        int.Parse(txtAccountId.Text),
                                        int.Parse(ddlCurrencyCode.SelectedCurrency),
                                        decimal.Parse(txtExchangeRate.Text),
                                        decimal.Parse(txt2ndExchangeRate.Text),
                                        decimal.Parse(txtPrimeAmoutEdit.Text),
                                        string.Empty,
                                        string.Empty,
                                        1,
                                        1,
                                        txtChequeno.Text,
                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        string.Empty,
                                        txtLongDescription.Text,
                                        General.GetNullableGuid(txtBudgetId.Text),
                                        ref ivoucherlineitemid,
                                        ViewState["FLDOLDVOUCHERID"] != null ? General.GetNullableInteger(ViewState["FLDOLDVOUCHERID"].ToString()) : null,
                                        ViewState["FLDOLDVOUCHERLINEITEMID"] != null ? General.GetNullableGuid(ViewState["FLDOLDVOUCHERLINEITEMID"].ToString()) : null
                                        );
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

        else if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
        }
    }

    protected void Reset()
    {
        ViewState["VOUCHERLINEITEMCODE"] = null;
        txtRowNumber.Text = "";
        txtAccountId.Text = "";
        txtAccountUsage.Text = "";
        txtAccountSource.Text = "";
        txtAccountCode.Text = "";
        txtAccountDescription.Text = "";
        txtAccountId.Text = "";        
        txtPrimeAmoutEdit.Text = "";
        txtChequeno.Text = "";
        txtLongDescription.Text = "";
        txtUpdatedBy.Text = "";
        txtUpdatedDate.Text = "";
        txtBudgetCode.Text = "";
        txtBudgetName.Text = "";
        ddlCurrencyCode.SelectedCurrency = "";
        txtExchangeRate.Text = string.Empty;
        txt2ndExchangeRate.Text = string.Empty;
        if (Session["SelectedOffsetAccountId"] != null)
            Session.Remove("SelectedOffsetAccountId");
        if (Session["sOffsetAccountCode"] != null)
            Session.Remove("sOffsetAccountCode");
        if (Session["sOffsetAccountCodeDescription"] != null)
            Session.Remove("sOffsetAccountCodeDescription");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Post", "POST", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        MenuVoucherLineItem.AccessRights = this.ViewState;
        MenuVoucherLineItem.MenuList = toolbar.Show();
     //   MenuVoucherLineItem.SetTrigger(pnlVoucher);

        imgShowAccount.Visible = true;
        imgShowBudget.Visible = true;
        ddlCurrencyCode.Enabled = true;
        txtExchangeRate.Enabled = true;
        txt2ndExchangeRate.Enabled = true;
        txtLongDescription.Enabled = true;

        lblAmountSign.Text = "";
    }


    protected void EditVoucherLineItem(object sender, CommandEventArgs e)
    {
        string[] strValues = new string[2];
        strValues = e.CommandArgument.ToString().Split('^');
        VoucherLineEdit(new Guid(strValues[0]));
    }

    protected void VoucherLineEdit(Guid gLineId)
    {
        ViewState["VOUCHERLINEITEMCODE"] = gLineId.ToString();
        DataSet ds = PhoenixAccountsContraVoucher.ContraVoucherLineItemEdit(gLineId);
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
                txtBudgetCode.Text = dr["FLDBUDGETCODE"].ToString();
                txtBudgetName.Text = dr["FLDBUDGETDESCRIPTION"].ToString();
                //txtSubAccount.Text = dr["FLDBUDGETID"].ToString();
                txtAccountSource.Text = dr["FLDACCOUNTSOURCENAME"].ToString();
                txtAccountUsage.Text = dr["FLDACCOUNTUSAGENAME"].ToString();
                ddlCurrencyCode.SelectedCurrency = dr["FLDCURRENCYCODE"].ToString();
                txtExchangeRate.Text = string.Format(String.Format("{0:#####.00000000000000000}", decimal.Parse(dr["FLDBASEEXCHANGERATE"].ToString())));
                txt2ndExchangeRate.Text = string.Format(String.Format("{0:#####.00000000000000000}", decimal.Parse(dr["FLDREPORTEXCHANGERATE"].ToString())));
                txtPrimeAmoutEdit.Text = string.Format(String.Format("{0:#####.00}", dr["FLDAMOUNT"]));
                txtChequeno.Text = dr["FLDREMITTANCENO"].ToString();
                txtLongDescription.Text = dr["FLDLONGDESCRIPTION"].ToString();
                txtUpdatedBy.Text = dr["FLDUPDATEDBYUSERNAME"].ToString();
                txtUpdatedDate.Text = dr["FLDUPDATEDDATE"].ToString();
                ViewState["ISACCOUNTACTIVE"] = dr["FLDACCOUNTACTIVEYN"].ToString();
                Session["SelectedOffsetAccountId"] = dr["FLDACCOUNTID"].ToString();
                if (!string.IsNullOrEmpty(txtPrimeAmoutEdit.Text) && decimal.Parse(txtPrimeAmoutEdit.Text) < 0)
                {
                    lblAmountSign.Text = "-1";
                }
                else
                    lblAmountSign.Text = "1";
                txtPrimeAmoutEdit.Focus();
                DisableControls(txtRowNumber.Text);
            }
        }
    }

    protected void InsertVoucherLineItem
    (
        int iVoucherId,
        int iAccountId,
        int iCurrencyCode,
        decimal dBaseExchangeRate,
        decimal dReportExchangeRate,
        decimal dAmount,
        string strCostCenter,
        string strProfitCenter,
        int? iActiveYN,
        int? iLockedYN,
        string strRemittanceno,
        int iCreatedBy,
        string strRefDocno,
        string strLongDescription,
        Guid? gSubAccountMapId,
        ref Guid ivoucherlineitemid,
        int? oldvoucherid,
        Guid? oldoffsettinglineitemid
    )
    {
        PhoenixAccountsContraVoucher.ContraVoucherLineItemInsert(
            iVoucherId,
            iAccountId,
            iCurrencyCode,
            dBaseExchangeRate,
            dReportExchangeRate,
            dAmount,
            strCostCenter,
            strProfitCenter,
            iActiveYN,
            iLockedYN,
            strRemittanceno,
            iCreatedBy,
            strRefDocno,
            strLongDescription,
            gSubAccountMapId,
            ref ivoucherlineitemid,
            oldvoucherid,
            oldoffsettinglineitemid);
        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void UpdateVoucherLineItem
    (
         Guid gVoucherLineItemId
        , int iAccountId
        , int iCurrencyCode
        , decimal dBaseExchangeRate
        , decimal dReportExchangeRate
        , decimal dAmount
        , string strCostCenter
        , string strProfitCenter
        , int? iActiveYN
        , int? iLockedYN
        , string strRemittanceno
        , int iUpdatedBy
        , string strRefDocno
        , string strLongDescription,
        Guid? gSubAccountMapId
    )
    {
        PhoenixAccountsContraVoucher.ContraVoucherLineItemUpdate(
         gVoucherLineItemId
        , int.Parse(ViewState["VOUCHERID"].ToString())
        , iAccountId
        , iCurrencyCode
        , dBaseExchangeRate
        , dReportExchangeRate
        , dAmount
        , strCostCenter
        , strProfitCenter
        , iActiveYN
        , iLockedYN
        , strRemittanceno
        , iUpdatedBy
        , strRefDocno
        , strLongDescription
        , gSubAccountMapId);

        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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
                    //if (txtBudgetId.Text.Trim().Equals(""))
                    //    ucError.ErrorMessage = "Sub Account is required.";
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

        if (!string.IsNullOrEmpty(txtRowNumber.Text) && txtRowNumber.Text == "10")
        {
            if ((int.Parse(lblAmountSign.Text) > 0 && decimal.Parse(txtPrimeAmoutEdit.Text) < 0) || (int.Parse(lblAmountSign.Text) < 0 && decimal.Parse(txtPrimeAmoutEdit.Text) > 0))
            {
                ucError.ErrorMessage = "Amount sign should not change";
            }
        }

        ucError.HeaderMessage = "Please provide the following required information";
        return (!ucError.IsError);
    }

    protected void txtAccountCode_changed(object sender, EventArgs e)
    {
        Session["sOffsetAccountCode"] = txtAccountCode.Text;
        Session["sOffsetAccountCodeDescription"] = txtAccountDescription.Text;
        Session["sOffsetSubAccountId"] = txtBudgetCode.Text;
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void txtExchangeRate_TextChanged(object sender, EventArgs e)
    {
        System.Collections.Hashtable ht = new System.Collections.Hashtable();
        System.Collections.Hashtable htInitialRate = new System.Collections.Hashtable();
        if (Session["OFFSETCONTRAVOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "INITIALRate"] != null)
        {
            htInitialRate = (System.Collections.Hashtable)Session["OFFSETCONTRAVOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "INITIALRate"];
            if ((txtExchangeRate.Text != "") && (txtExchangeRate.Text != htInitialRate["INITIALEXCHANGERATE"].ToString()))
            {
                ht["EXCHANGERATE"] = txtExchangeRate.Text;
                ht["2ndEXCHANGERATE"] = txt2ndExchangeRate.Text;
                Session["OFFSETCONTRAVOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "Rate"] = ht;
            }
        }
    }
    protected void txt2ndExchangeRate_TextChanged(object sender, EventArgs e)
    {
        Session["OFFSETCONTRAVOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency] = ddlCurrencyCode.SelectedCurrency;
        System.Collections.Hashtable ht = new System.Collections.Hashtable();
        System.Collections.Hashtable htInitialRate = new System.Collections.Hashtable();
        if (Session["OFFSETCONTRAVOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "INITIALRate"] != null)
        {
            htInitialRate = (System.Collections.Hashtable)Session["OFFSETCONTRAVOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "INITIALRate"];
            if ((txt2ndExchangeRate.Text != "") && (txt2ndExchangeRate.Text != htInitialRate["INITIAL2ndEXCHANGERATE"].ToString()))
            {
                ht["EXCHANGERATE"] = txtExchangeRate.Text;
                ht["2ndEXCHANGERATE"] = txt2ndExchangeRate.Text;
                Session["OFFSETCONTRAVOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "Rate"] = ht;
            }
        }
    }

    private void DisableControls(string row)
    {
        if (row == "10")
        {
            imgShowAccount.Visible = false;
            imgShowBudget.Visible = false;
            ddlCurrencyCode.Enabled = false;
            txtExchangeRate.Enabled = false;
            txt2ndExchangeRate.Enabled = false;
            txtLongDescription.Enabled = false;
        }
        else
        {
            imgShowAccount.Visible = true;
            imgShowBudget.Visible = true;
            ddlCurrencyCode.Enabled = true;
            txtExchangeRate.Enabled = true;
            txt2ndExchangeRate.Enabled = true;
            txtLongDescription.Enabled = true;
        }
    }

}
