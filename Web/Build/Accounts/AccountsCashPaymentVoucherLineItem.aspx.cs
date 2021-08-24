using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class AccountsCashPaymentVoucherLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        cmdHiddenPick.Attributes.Add("style", "Display:None");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        txtBudgetId.Attributes.Add("style", "Display:None");
        txtBudgetgroupId.Attributes.Add("style", "Display:None");
        txtAccountId.Attributes.Add("style", "Display:None;");
        //txtAccountSource.ReadOnly = true;
        //txtAccountUsage.ReadOnly = true;
        if (!IsPostBack)
        {
            Reset();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["PRINCIPALID"] = null;
            if (Request.QueryString["voucherlineitemcode"] != null && Request.QueryString["voucherlineitemcode"] != string.Empty)
            {
                ViewState["VOUCHERLINEITEMCODE"] = Request.QueryString["voucherlineitemcode"];
                VoucherLineEdit(new Guid(ViewState["VOUCHERLINEITEMCODE"].ToString()));
            }
            if (Request.QueryString["lastAddedlineitemid"] != null && Request.QueryString["lastAddedlineitemid"] != string.Empty)
            {
                ViewState["lastAddedlineitemid"] = Request.QueryString["lastAddedlineitemid"];
            }
            //btnShowBudget.Attributes.Add("onclick", "return showSubAccountPickList('spnPickListExpenseAccount', 'codehelp1', '','../Common/CommonPickListSubAccount.aspx', true);");
            //ttlVoucher.Text = "Row Number (" + txtRowNumber.Text + ")";
            ViewState["VOUCHERID"] = Request.QueryString["qvouchercode"];
            VoucherEdit();
        }
        if (ViewState["ISACCOUNTACTIVE"] != null)
        {
            if (ViewState["ISACCOUNTACTIVE"].ToString() == "1")
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
                toolbar.AddButton("Add", "ADD", ToolBarDirection.Right);

            }
            else
            { toolbar.AddButton("New", "NEW"); }
        }
        else
        {
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
            toolbar.AddButton("Add", "ADD", ToolBarDirection.Right);
        }
        MenuVoucherLineItem.AccessRights = this.ViewState;
        MenuVoucherLineItem.Title = "Row Number (" + txtRowNumber.Text + ")";
        MenuVoucherLineItem.MenuList = toolbar.Show();
       
    }

    protected void VoucherEdit()
    {
        if (ViewState["VOUCHERID"] != null && ViewState["VOUCHERID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixAccountsVoucher.VoucherEdit(int.Parse(ViewState["VOUCHERID"].ToString()));
            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtVoucherNumber.Text = dr["FLDVOUCHERNUMBER"].ToString();
                ViewState["VoucherDate"] = dr["FLDVOUCHERDATE"].ToString();
                ViewState["Defaultcurrency"] = dr["FLDDEFAULTCURRENCY"].ToString();
                if (int.Parse(dr["FLDISPERIODLOCKED"].ToString()) == 1)
                    MenuVoucherLineItem.Visible = false;
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
                if (dsInvoice.Tables[0].Rows.Count > 0)
                {
                    DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                    dTransactionExchangerate = decimal.Parse(drInvoice["FLDEXCHANGERATE"].ToString());
                }
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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidLineItem())
            {
                string errormessage = "";
                errormessage = ucError.ErrorMessage;
                errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                return;
            }
            if (ViewState["VOUCHERLINEITEMCODE"] == null)
            {
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
                                            chkIncludeYNEdit.Checked == true ? 1 : 0,
                                            chkShowInSummaryBalance.Checked == true ? 1 : 0,
                                            General.GetNullableGuid(txtownerbudgetedit.Text)
                                            , txtRemarks.Text);

                    ucStatus.Text = "Voucher Line Item information added";


                    Reset();
                }
                catch (Exception ex)
                {
                    string errormessage = "";
                    errormessage = ex.Message;
                    errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                    RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                    return;
                }
            }
            else
            {
                try
                {
                    DataSet ds = new DataSet();
                    ds = PhoenixAccountsVoucher.AllocatedVoucherLineItemEdit(General.GetNullableGuid(ViewState["VOUCHERLINEITEMCODE"].ToString()));
                    DataRow dr = ds.Tables[0].Rows[0];

                    if (!IsValidAmount(dr["FLDALLOCATEDAMOUNT"].ToString(), txtPrimeAmoutEdit.Text))
                    {
                        string errormessage = "";
                        errormessage = ucError.ErrorMessage;
                        errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                        RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                        return;
                    }
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
                                            General.GetNullableGuid(txtBudgetId.Text),
                                            chkIncludeYNEdit.Checked == true ? 1 : 0,
                                            General.GetNullableGuid(txtownerbudgetedit.Text),
                                            chkShowInSummaryBalance.Checked == true ? 1 : 0,
                                            txtRemarks.Text);


                    ucStatus.Text = "Voucher Line Item information updated";
                }
                catch (Exception ex)
                {
                    string errormessage = "";
                    errormessage = ex.Message;
                    errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                    RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                    return;
                }
            }
        }

        else if (CommandName.ToUpper().Equals("ADD"))
        {
            if (!IsValidLineItem())
            {
                string errormessage = "";
                errormessage = ucError.ErrorMessage;
                errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
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
                                        chkIncludeYNEdit.Checked == true ? 1 : 0,
                                            chkShowInSummaryBalance.Checked == true ? 1 : 0,
                                            General.GetNullableGuid(txtownerbudgetedit.Text),
                                            txtRemarks.Text
                                        );

                ucStatus.Text = "Voucher Line Item information added";
                Reset();
            }
            catch (Exception ex)
            {
                string errormessage = "";
                errormessage = ex.Message;
                errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                return;
            }
        }
        else if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
            ddlCurrencyCode.SelectedCurrency = ViewState["Defaultcurrency"].ToString();
            Voucher_SetExchangeRate(ddlCurrencyCode, e);

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
        txtBudgetId.Text = "";
        if (ViewState["lastAddedlineitemid"] != null)
        {
            DataSet ds1 = PhoenixAccountsVoucher.VoucherLineItemEdit((Guid)General.GetNullableGuid(ViewState["lastAddedlineitemid"].ToString()));
            if (ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds1.Tables[0].Rows[0];
                    ddlCurrencyCode.SelectedCurrency = dr["FLDCURRENCYCODE"].ToString();
                    txtExchangeRate.Text = string.Format(String.Format("{0:#####.00000000000000000}", decimal.Parse(dr["FLDBASEEXCHANGERATE"].ToString())));
                    txt2ndExchangeRate.Text = string.Format(String.Format("{0:#####.00000000000000000}", decimal.Parse(dr["FLDREPORTEXCHANGERATE"].ToString())));
                }
            }
        }
        else
        {
            ddlCurrencyCode.SelectedCurrency = "";
            txtExchangeRate.Text = "";
            txt2ndExchangeRate.Text = "";
        }
        txtPrimeAmoutEdit.Text = "";
        txtChequeno.Text = "";
        txtLongDescription.Text = "";
        txtUpdatedBy.Text = "";
        txtUpdatedDate.Text = "";
        txtBudgetCode.Text = "";
        txtBudgetName.Text = "";
        txtBudgetId.Text = "";
        txtBudgetgroupId.Text = "";
        ddlCurrencyCode.Enabled = true;
        imgShowAccount.Attributes.Add("onclick", "return showAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccount.aspx',true);");
        imgShowBudget.Enabled = true;
        imgShowBudget.Attributes.Add("onclick", "return showSubAccountPickList('spnPickListExpenseAccount', 'codehelp1', '','" + Session["sitepath"] + "/Common/CommonPickListSubAccount.aspx', true);");
        if (Session["SelectedAccountId"] != null)
            Session.Remove("SelectedAccountId");
        if (Session["sAccountCode"] != null)
            Session.Remove("sAccountCode");
        if (Session["sAccountCodeDescription"] != null)
            Session.Remove("sAccountCodeDescription");
        chkIncludeYNEdit.Enabled = false;
        chkIncludeYNEdit.Checked = false;

        txtAccountCode1.Text = "";
        txtOwnerBudgetNameEdit.Text = "";
        txtownerbudgetedit.Text = "";
        TextBox1.Text = "";
        txtExchangeRate.Text = "";
        txt2ndExchangeRate.Text = "";
        ddlCurrencyCode.SelectedCurrency = "";
        lblbudgetid.Text = "";

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        toolbar.AddButton("Add", "ADD", ToolBarDirection.Right);
        MenuVoucherLineItem.AccessRights = this.ViewState;
        MenuVoucherLineItem.MenuList = toolbar.Show();
        // MenuVoucherLineItem.SetTrigger(pnlVoucher);
        ViewState["ISACCOUNTACTIVE"] = "1";
        chkShowInSummaryBalance.Checked = false;
        txtRemarks.Text = "";

        imgShowAccount.Enabled = true;
         txtAccountCode.ReadOnly = false;
          txtAccountDescription.ReadOnly = false;
        txtBudgetName.ReadOnly = false;
        txtBudgetName.ReadOnly = false;
        txtBudgetCode.ReadOnly = false;
        imgShowBudget.Enabled = true;
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
        DataSet ds = PhoenixAccountsVoucher.VoucherLineItemEdit(gLineId);
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
                lblvesselid.Text = dr["FLDVESSELID"].ToString();
                lblaccountusage.Text = dr["FLDACCOUNTUSAGE"].ToString();
                lblbudgetid.Text = dr["FLDBUDGETID"].ToString();
                txtAccountCode1.Text = dr["FLDOWNERACCOUNT"].ToString();
                chkIncludeYNEdit.Checked = dr["FLDNOTINCULDEINOWNERREPORT"].ToString().Equals("1") ? true : false;
                ViewState["ISACCOUNTACTIVE"] = dr["FLDACCOUNTACTIVEYN"].ToString();
                Session["SelectedAccountId"] = dr["FLDACCOUNTID"].ToString();
                txtPrimeAmoutEdit.Focus();
                chkShowInSummaryBalance.Checked = dr["FLDSHOWINSUMMARYBALANCE"].ToString().Equals("1") ? true : false;
                txtownerbudgetedit.Text = dr["FLDOWNERBUDGETID"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                ViewState["PRINCIPALID"] = dr["FLDOWNERACCOUNTID"].ToString();
                txtSOAreference.Text = dr["FLDDEBITNOTEREFERENCE"].ToString();
                if (dr["FLDACCOUNTUSAGE"].ToString() == "460")
                {
                    ddlCurrencyCode.Enabled = false;
                    imgShowAccount.Attributes.Add("onclick", "return false;");

                }
            }
            if (txtRowNumber.Text == "10") // Automatic inserted row not allow to change the selected currency, account, subaccount
            {
                ddlCurrencyCode.Enabled = false;
                imgShowAccount.Enabled = false;
                txtAccountCode.ReadOnly = true;
                txtAccountDescription.ReadOnly = true;
                txtBudgetCode.ReadOnly = true;
                txtBudgetName.ReadOnly = true;
                //imgShowAccount.Attributes.Add("onclick", "return false;");
                imgShowBudget.Enabled = false;
            }
            else
            {
                imgShowAccount.Enabled = true;
                txtAccountCode.ReadOnly = false;
                txtAccountDescription.ReadOnly = false;
                txtBudgetName.ReadOnly = false;
                txtBudgetName.ReadOnly = false;
                imgShowBudget.Enabled = true;
            }

            if (lblaccountusage.Text != "81")
            {
                chkIncludeYNEdit.Enabled = false;
                txtAccountCode1.Enabled = false;
                imgShowAccount1.Enabled = false;
            }
            else
            {
                chkIncludeYNEdit.Enabled = true;
                txtAccountCode1.Enabled = true;
                imgShowAccount1.Enabled = true;
            }
            imgShowAccount1.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudget.aspx?&vesselid=" + null + "&Ownerid=" + ViewState["PRINCIPALID"] + "&budgetid=" + lblbudgetid.Text + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
            if ((lblvesselid.Text != null) && lblvesselid.Text != string.Empty)
                imgShowAccount1.Enabled = true;
            else
                imgShowAccount1.Enabled = false;
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
        int IsNotIncluded,
        int ShowInSummaryBalance,
        Guid? Ownerbudgetid,
        string strRemarks
    )
    {
        PhoenixAccountsVoucher.VoucherLineItemInsert(
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
            IsNotIncluded,
            ShowInSummaryBalance,
            Ownerbudgetid,
             strRemarks);
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
        , string strLongDescription
        , Guid? gSubAccountMapId
        , int IsNotIncluded
        , Guid? Ownerbudgetid
        , int ShowInSummaryBalance
        , string strRemarks
    )
    {
        PhoenixAccountsVoucher.VoucherLineItemUpdate(
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
        , gSubAccountMapId
        , IsNotIncluded
        , Ownerbudgetid
        , ShowInSummaryBalance
         , strRemarks
);

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
                if (dsaccount.Tables[0].Rows[0]["FLDACCOUNTUSAGE"].ToString() != "78" && dsaccount.Tables[0].Rows[0]["FLDACCOUNTUSAGE"].ToString() != "460" && dsaccount.Tables[0].Rows[0]["FLDACCOUNTUSAGE"].ToString() != "336")
                {
                    //if (txtBudgetId.Text.Trim().Equals(""))
                    if (General.GetNullableString(txtBudgetCode.Text) == null)
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


    protected void txtAccountCode_changed(object sender, EventArgs e)
    {
        DataSet ds = PhoenixRegistersAccount.EditCompanyAccount(Convert.ToInt32(txtAccountId.Text.ToString()), PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        DataTable dt = ds.Tables[0];
        DataSet dsprincipal = PhoenixRegistersAccount.EditAccount(Convert.ToInt32(txtAccountId.Text));
        if (dsprincipal.Tables[0].Rows.Count > 0)
        {
            DataRow drprincipal = dsprincipal.Tables[0].Rows[0];
            ViewState["PRINCIPALID"] = drprincipal["FLDPRINCIPALID"];
        }

        if (dt.Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            if (dr["FLDACCOUNTUSAGE"].ToString() == "460" || dr["FLDACCOUNTUSAGE"].ToString() == "335")
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

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        if (txtAccountUsage.Text == "Vessel")
        {
            chkIncludeYNEdit.Enabled = true;
            txtAccountCode1.Enabled = true;
            imgShowAccount1.Enabled = true;
        }
        else
        {
            chkIncludeYNEdit.Enabled = false;
            txtAccountCode1.Enabled = false;
            imgShowAccount1.Enabled = false;
        }

        DataSet dsBudget = PhoenixCommonRegisters.SubAccountSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
              , General.GetNullableInteger(txtAccountId.Text)
              , General.GetNullableInteger(txtAccountUsage.Text)
              , General.GetNullableString(txtBudgetCode.Text)
              , General.GetNullableString(txtBudgetName.Text)
              , null, null
              , 1, General.ShowRecords(null),
              ref iRowCount,
              ref iTotalPageCount
              );

        if (dsBudget.Tables.Count > 0)
        {
            if (dsBudget.Tables[0].Rows.Count > 0)
            {
                lblbudgetid.Text = dsBudget.Tables[0].Rows[0]["FLDBUDGETID"].ToString();
                Filter.CurrentSelectedESMBudgetCode = null;
                txtBudgetId.Text = dsBudget.Tables[0].Rows[0]["FLDSUBACCOUNTMAPID"].ToString();

                if (txtBudgetId.Text != "" && txtAccountUsage.Text == "Vessel")
                {
                    int iIncludeSOA = 0;
                    int iVesselId = 0;
                    //DataSet ds = PhoenixAccountsVoucher.OwnerBudgetIncludeSOA(txtBudgetCode.Text, General.GetNullableInteger(txtAccountId.Text), ref iIncludeSOA, ref iVesselId);
                    DataSet ds = PhoenixAccountsVoucher.OwnerBudgetIncludeSOA(txtBudgetCode.Text, General.GetNullableInteger(txtAccountId.Text), ref iIncludeSOA, ref iVesselId, General.GetNullableGuid(txtownerbudgetedit.Text));

                    if (iIncludeSOA == 0)
                        chkIncludeYNEdit.Checked = true;
                    else
                        chkIncludeYNEdit.Checked = false;

                    imgShowAccount1.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudget.aspx?&vesselid=" + null + "&Ownerid=" + ViewState["PRINCIPALID"] + "&budgetid=" + lblbudgetid.Text + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                    if ((iVesselId.ToString() != null) && iVesselId.ToString() != string.Empty)
                        imgShowAccount1.Enabled = true;
                    else
                        imgShowAccount1.Enabled = false;

                }
            }
        }
    }

    public bool IsValidAmount(string allocatedamount, string newAmount)
    {
        if (Convert.ToDecimal(allocatedamount) != 0 && Math.Abs(Convert.ToDecimal(newAmount)) < Convert.ToDecimal(allocatedamount))
            ucError.ErrorMessage = "Amount is lesser than the allocated amount";

        ucError.HeaderMessage = "Please provide the following required information";
        return (!ucError.IsError);
    }
    protected void txtBudgetId_Changed(object sender, EventArgs e)
    {
        txtAccountCode1.Text = "";
        txtownerbudgetedit.Text = "";
        int iRowCount = 0;
        int iTotalPageCount = 0;

        txtAccountCode1.Text = "";

        DataSet dsBudget = PhoenixCommonRegisters.SubAccountSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
               , General.GetNullableInteger(txtAccountId.Text)
               , General.GetNullableInteger(txtAccountUsage.Text)
               , General.GetNullableString(txtBudgetCode.Text)
               , null
               , null, null
               , 1, General.ShowRecords(null),
               ref iRowCount,
               ref iTotalPageCount
               );

        if (dsBudget.Tables[0].Rows.Count > 0)
            lblbudgetid.Text = dsBudget.Tables[0].Rows[0]["FLDBUDGETID"].ToString();

        imgShowAccount1.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?&vesselid=" + null + "&Ownerid=" + ViewState["PRINCIPALID"] + "&budgetid=" + lblbudgetid.Text + "', true); ");
    }
}
