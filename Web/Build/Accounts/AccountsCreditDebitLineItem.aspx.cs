using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsCreditDebitLineItem : PhoenixBasePage
{
    public string  CreditenoteCompanyid ;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenPick.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbar = new PhoenixToolbar();

            txtBudgetId.Attributes.Add("style", "visibility:hidden");
            txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");
            txtAccountId.Attributes.Add("style", "visibility:hidden;");
            //txtAccountSource.ReadOnly = true;
            //txtAccountUsage.ReadOnly = true;
            if (!IsPostBack)
            {
                Reset();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PRINCIPALID"] = null;
                if (Request.QueryString["creditdebitnotelineitemid"] != null && Request.QueryString["creditdebitnotelineitemid"] != string.Empty)
                {
                    ViewState["CREDITDEBITNOTELINEITEMID"] = Request.QueryString["creditdebitnotelineitemid"];
                    CreditDebitNoteLineEdit(new Guid(ViewState["CREDITDEBITNOTELINEITEMID"].ToString()));
                }
                if (Request.QueryString["lastAddedlineitemid"] != null && Request.QueryString["lastAddedlineitemid"] != string.Empty)
                {
                    ViewState["lastAddedlineitemid"] = Request.QueryString["lastAddedlineitemid"];
                }
                //cmdAttachment.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod=" + PhoenixModule.ACCOUNTS + "');return true;");
                
                //imgShowAccount.Attributes.Add("onclick", "return showAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListAccount.aspx?iframename=true&companyid= + CreditenoteCompanyid +',true);
                //btnShowBudget.Attributes.Add("onclick", "return showSubAccountPickList('spnPickListExpenseAccount', 'codehelp1', '','../Common/CommonPickListSubAccount.aspx', true);");
                // ttlCreditDebitNote.Text = "Row Number (" + txtRowNumber.Text + ")";
                ViewState["creditdebitnoteid"] = Request.QueryString["creditdebitnoteid"];
                CreditDebitEdit();
                imgShowAccount.Attributes.Add("onclick", "return showAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListAccount.aspx?iframename=true&companyid=" + CreditenoteCompanyid + "');return true;");
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

            MenuCreditDebitNoteLineItem.AccessRights = this.ViewState;
            MenuCreditDebitNoteLineItem.Title = "Row Number (" + txtRowNumber.Text + ")";
            MenuCreditDebitNoteLineItem.MenuList = toolbar.Show();
            //MenuCreditDebitNoteLineItem.SetTrigger(pnlCreditDebitNote);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CreditDebitEdit()
    {
        if (ViewState["creditdebitnoteid"] != null && ViewState["creditdebitnoteid"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixAccountsCreditDebitNote.EditCreditDebitNote(new Guid(ViewState["creditdebitnoteid"].ToString()));
            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtCreditDebitNoteNumber.Text = dr["FLDVOUCHERNUMBER"].ToString();
                ViewState["VoucherDate"] = dr["FLDVOUCHERDATE"].ToString();
                CreditenoteCompanyid = dr["FLDCOMPANYID"].ToString ();
                if (int.Parse(dr["FLDISPERIODLOCKED"].ToString()) == 1)
                    MenuCreditDebitNoteLineItem.Visible = false;
            }
        }
    }

    protected void CreditDebitNote_SetExchangeRate(object sender, EventArgs e)
    {
        decimal dTransactionExchangerate = 0;
        if (ddlCurrencyCode.SelectedCurrency.ToUpper() != "DUMMY")
        {
            ViewState["VoucherDate"] = General.GetNullableDateTime(Convert.ToString(ViewState["VoucherDate"])) != null ? ViewState["VoucherDate"] : DateTime.Parse(DateTime.Now.ToShortDateString());
            DataSet dsInvoice = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(ddlCurrencyCode.SelectedCurrency), DateTime.Parse(ViewState["VoucherDate"].ToString()));
            if (dsInvoice.Tables[0].Rows.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                dTransactionExchangerate = decimal.Parse(drInvoice["FLDEXCHANGERATE"].ToString());
            }
        }

        if (ddlCurrencyCode.SelectedCurrency.ToUpper() != "DUMMY")
        {
            DataSet ds = PhoenixRegistersCompany.EditCompany(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            ViewState["VoucherDate"] = General.GetNullableDateTime(Convert.ToString(ViewState["VoucherDate"])) != null ? ViewState["VoucherDate"] : DateTime.Parse(DateTime.Now.ToShortDateString());
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

    protected void CreditDebitNote_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string vtxtExchangeRate = "0";
        string vtxt2ndExchangeRate = "0";

        DataSet DsCnE = PhoenixAccountsCreditDebitNote.EditCreditDebitNote(new Guid(ViewState["creditdebitnoteid"].ToString()));

        if (DsCnE.Tables[0].Rows.Count > 0)
        {
            DataRow DrCnE = DsCnE.Tables[0].Rows[0];

            DataSet ds = PhoenixRegistersCompany.EditCompany(int.Parse(DrCnE["FLDCOMPANYID"].ToString()));
            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                DataSet dsbasecurrency = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(dr["FLDBASECURRENCY"].ToString()), DateTime.Parse(DateTime.Now.ToShortDateString()));
                DataSet dsReportcurrency = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(dr["FLDREPORTINGCURRENCY"].ToString()), DateTime.Parse(DateTime.Now.ToShortDateString()));

                DataRow drbasecurrency = dsbasecurrency.Tables[0].Rows[0];
                DataRow drreportcurrency = dsReportcurrency.Tables[0].Rows[0];

                decimal dTransactionExchangerate = 0;

                DataSet dsInvoice = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(DrCnE["FLDCURRENCYCODE"].ToString()), DateTime.Parse(DateTime.Now.ToShortDateString()));
                if (dsInvoice.Tables.Count > 0)
                {
                    DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                    dTransactionExchangerate = decimal.Parse(drInvoice["FLDEXCHANGERATE"].ToString());
                }


                vtxtExchangeRate = string.Format(String.Format("{0:#####.00000000000000000}", dTransactionExchangerate / decimal.Parse(drbasecurrency["FLDEXCHANGERATE"].ToString())));
                vtxt2ndExchangeRate = string.Format(String.Format("{0:#####.00000000000000000}", dTransactionExchangerate / decimal.Parse(drreportcurrency["FLDEXCHANGERATE"].ToString())));

            }
        }

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidLineItem())
            {
                ucError.Visible = true;
                return;
            }
            if (ViewState["CREDITDEBITNOTELINEITEMID"] == null)
            {
                try
                {
                    InsertCreditDebitNoteLineItem(new Guid(ViewState["creditdebitnoteid"].ToString()),
                                            int.Parse(txtAccountId.Text),
                                            int.Parse(ddlCurrencyCode.SelectedCurrency),
                                            decimal.Parse(txtExchangeRate.Text),
                                            decimal.Parse(txt2ndExchangeRate.Text),
                                            decimal.Parse(vtxtExchangeRate),
                                            decimal.Parse(vtxt2ndExchangeRate),
                                            DateTime.Parse(DateTime.Now.ToShortDateString()),
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
                                            chkIncludeYNEdit.Checked ? 1 : 0,
                                            General.GetNullableGuid(txtownerbudgetedit.Text));

                    ucStatus.Text = "Credit Debit Note Line Item information added";
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
                    UpdateCreditDebitNoteLineItem(
                                            new Guid(ViewState["CREDITDEBITNOTELINEITEMID"].ToString()),
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
                                             chkIncludeYNEdit.Checked ? 1 : 0,
                                         General.GetNullableGuid(txtownerbudgetedit.Text));

                    ucStatus.Text = "Credit Debit Note Line Item information updated";
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
                InsertCreditDebitNoteLineItem(new Guid(ViewState["creditdebitnoteid"].ToString()),
                                        int.Parse(txtAccountId.Text),
                                        int.Parse(ddlCurrencyCode.SelectedCurrency),
                                        decimal.Parse(txtExchangeRate.Text),
                                        decimal.Parse(txt2ndExchangeRate.Text),
                                        decimal.Parse(vtxtExchangeRate),
                                        decimal.Parse(vtxt2ndExchangeRate),
                                        DateTime.Parse(DateTime.Now.ToShortDateString()),
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
                                         chkIncludeYNEdit.Checked ? 1 : 0,
                                         General.GetNullableGuid(txtownerbudgetedit.Text)
                                        );

                ucStatus.Text = "CreditDebitNote Line Item information added";
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
        imgShowAccount.Enabled = true;
        ViewState["CREDITDEBITNOTELINEITEMID"] = null;
        txtRowNumber.Text = "";
        txtAccountId.Text = "";
        txtAccountUsage.Text = "";
        txtAccountSource.Text = "";
        txtAccountCode.Text = "";
        txtAccountDescription.Text = "";
        txtBudgetId.Text = "";
        if (ViewState["lastAddedlineitemid"] != null)
        {
            DataSet ds1 = PhoenixAccountsCreditDebitNoteLineItems.EditCreditDebitNoteLineItems(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (Guid)General.GetNullableGuid(ViewState["lastAddedlineitemid"].ToString()));
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

        MenuCreditDebitNoteLineItem.AccessRights = this.ViewState;
        MenuCreditDebitNoteLineItem.MenuList = toolbar.Show();
        // MenuCreditDebitNoteLineItem.SetTrigger(pnlCreditDebitNote);
        ViewState["ISACCOUNTACTIVE"] = "1";

    }

    protected void EditCreditDebitNoteLineItem(object sender, CommandEventArgs e)
    {
        string[] strValues = new string[2];
        strValues = e.CommandArgument.ToString().Split('^');
        CreditDebitNoteLineEdit(new Guid(strValues[0]));
    }

    protected void CreditDebitNoteLineEdit(Guid gLineId)
    {
        ViewState["CREDITDEBITNOTELINEITEMID"] = gLineId.ToString();
        DataSet ds = PhoenixAccountsCreditDebitNoteLineItems.EditCreditDebitNoteLineItems(PhoenixSecurityContext.CurrentSecurityContext.UserCode, gLineId);
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
                txtBudgetName.Text = dr["FLDBUDGETDESCRIPTION"].ToString(); txtAccountSource.Text = dr["FLDACCOUNTSOURCENAME"].ToString();
                txtAccountUsage.Text = dr["FLDACCOUNTUSAGENAME"].ToString();
                ddlCurrencyCode.SelectedCurrency = dr["FLDCURRENCYCODE"].ToString();
                txtExchangeRate.Text = string.Format(String.Format("{0:#####.00000000000000000}", decimal.Parse(dr["FLDBASEEXCHANGERATE"].ToString())));
                txt2ndExchangeRate.Text = string.Format(String.Format("{0:#####.00000000000000000}", decimal.Parse(dr["FLDREPORTEXCHANGERATE"].ToString())));
                txtPrimeAmoutEdit.Text = string.Format(String.Format("{0:#####.00}", dr["FLDAMOUNT"]));
                txtChequeno.Text = dr["FLDREMITTANCENO"].ToString();
                txtLongDescription.Text = dr["FLDLONGDESCRIPTION"].ToString();
                txtUpdatedBy.Text = dr["FLDUPDATEDBYUSERNAME"].ToString();
                lblvesselid.Text = dr["FLDVESSELID"].ToString();
                lblaccountusage.Text = dr["FLDACCOUNTUSAGE"].ToString();
                lblbudgetid.Text = dr["FLDBUDGETID"].ToString();
                txtAccountCode1.Text = dr["FLDOWNERACCOUNT"].ToString();
                txtUpdatedDate.Text = dr["FLDUPDATEDDATE"].ToString();
                ViewState["ISACCOUNTACTIVE"] = dr["FLDACCOUNTACTIVEYN"].ToString();
                Session["SelectedAccountId"] = dr["FLDACCOUNTID"].ToString();
                txtPrimeAmoutEdit.Focus();

                txtownerbudgetedit.Text = dr["FLDOWNERBUDGETID"].ToString();
                ViewState["PRINCIPALID"] = dr["FLDOWNERACCOUNTID"].ToString();
                if (dr["FLDACCOUNTUSAGE"].ToString() == "460")
                {
                    ddlCurrencyCode.Enabled = false;
                    imgShowAccount.Enabled = false;

                }
            }
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

        if (txtBudgetId.Text != "" && txtAccountUsage.Text == "Vessel")
        {
            int iIncludeSOA = 0;
            int iVesselId = 0;

            DataSet ds1 = PhoenixAccountsVoucher.OwnerBudgetIncludeSOA(txtBudgetCode.Text, General.GetNullableInteger(txtAccountId.Text), ref iIncludeSOA, ref iVesselId);
            if (iIncludeSOA == 0)
                chkIncludeYNEdit.Checked = true;
            else
                chkIncludeYNEdit.Checked = false;

            imgShowAccount1.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?&vesselid=" + null + "&Ownerid=" + ViewState["PRINCIPALID"] + "&budgetid=" + lblbudgetid.Text + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
            if ((iVesselId.ToString() != null) && iVesselId.ToString() != string.Empty)
                imgShowAccount1.Enabled = true;
            else
                imgShowAccount1.Enabled = false;

        }
        imgShowAccount1.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?&vesselid=" + null + "&Ownerid=" + ViewState["PRINCIPALID"] + "&budgetid=" + lblbudgetid.Text + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
        if ((lblvesselid.Text != null) && lblvesselid.Text != string.Empty)
            imgShowAccount1.Enabled = true;
        else
            imgShowAccount1.Enabled = false;
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
                    DataSet ds = PhoenixAccountsVoucher.OwnerBudgetIncludeSOA(txtBudgetCode.Text, General.GetNullableInteger(txtAccountId.Text), ref iIncludeSOA, ref iVesselId);
                    if (iIncludeSOA == 0)
                        chkIncludeYNEdit.Checked = true;
                    else
                        chkIncludeYNEdit.Checked = false;

                    imgShowAccount1.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?&vesselid=" + null + "&Ownerid=" + ViewState["PRINCIPALID"] + "&budgetid=" + lblbudgetid.Text + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                    if ((iVesselId.ToString() != null) && iVesselId.ToString() != string.Empty)
                        imgShowAccount1.Enabled = true;
                    else
                        imgShowAccount1.Enabled = false;

                }
            }
        }
    }

    protected void InsertCreditDebitNoteLineItem
    (
        Guid iCreditDebitNodeId,
        int iAccountId,
        int iCurrencyCode,
        decimal dBaseExchangeRate,
        decimal dReportExchangeRate,
        decimal vBaseExchangeRate,
        decimal vReportExchangeRate,
        DateTime dtVoucherDate,
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
        Guid? OwnerBudgetId

    )
    {
        PhoenixAccountsCreditDebitNoteLineItems.InsertCreditDebitNoteLineItems(
            iCreditDebitNodeId,
            iAccountId,
            iCurrencyCode,
            dBaseExchangeRate,
            dReportExchangeRate,
            vBaseExchangeRate,
            vReportExchangeRate,
            dtVoucherDate,
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
            OwnerBudgetId

            );
        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void UpdateCreditDebitNoteLineItem
    (
         Guid gCreditDebitNoteLineItemId
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
       , Guid? OwnerBudgetId
    )
    {
        PhoenixAccountsCreditDebitNoteLineItems.UpdateCreditDebitNoteGeneralLineItems(
         gCreditDebitNoteLineItemId
        , new Guid(ViewState["creditdebitnoteid"].ToString())
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
        , OwnerBudgetId);

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
    public StateBag ReturnViewState()
    {
        return ViewState;
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

        imgShowAccount1.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?&vesselid=" + null + "&Ownerid=" + ViewState["PRINCIPALID"] + "&budgetid=" + lblbudgetid.Text + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
    }
}
