using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class AccountsOffSettingEntriesAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            txtBudgetId.Attributes.Add("style", "visibility:hidden");
            txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");
            txtAccountId.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Contra Voucher", "CREATEVOUCHER");
            PhoenixToolbar toolbarsubmenu = new PhoenixToolbar();
            toolbarsubmenu.AddButton("Save", "SAVE");
            MenuOffSettingAdd.AccessRights = this.ViewState;
            MenuOffSettingAdd.MenuList = toolbarmain.Show();
            MenuContraVoucher.AccessRights = this.ViewState;
            MenuContraVoucher.MenuList = toolbarsubmenu.Show();

            if (!IsPostBack)
            {

                if (Request.QueryString["VOUCHERLINEITEMID"] != null && Request.QueryString["VOUCHERLINEITEMID"] != string.Empty)
                    ViewState["VOUCHERLINEITEMID"] = Request.QueryString["VOUCHERLINEITEMID"];
            }
            btnShowBudget.Attributes.Add("onclick", "return showSubAccountPickList('spnPickListExpenseAccount', 'codehelp1', '','../Common/CommonPickListSubAccount.aspx', true);");
            if (Request.QueryString["VoucherID"] != null && Request.QueryString["VoucherID"] != string.Empty)
                ViewState["VoucherID"] = Request.QueryString["VoucherID"];
            if (Request.QueryString["CurrencyID"] != null && Request.QueryString["CurrencyID"] != string.Empty)
            {
                ViewState["CurrencyID"] = Request.QueryString["CurrencyID"];
                ddlCurrencyCode.SelectedCurrency = ViewState["CurrencyID"].ToString();              
            }
            if (Request.QueryString["BaseRate"] != null && Request.QueryString["BaseRate"] != string.Empty)
            {
                if (txtExchangeRate.Text == "")
                    txtExchangeRate.Text = Request.QueryString["BaseRate"];
            }
            if (Request.QueryString["ReportRate"] != null && Request.QueryString["ReportRate"] != string.Empty)
            {
                if (txt2ndExchangeRate.Text == "")
                    txt2ndExchangeRate.Text = Request.QueryString["ReportRate"];
            }
            if (Request.QueryString["CurrencyName"] != null && Request.QueryString["CurrencyName"] != string.Empty)
                ViewState["CurrencyName"] = Request.QueryString["CurrencyName"];
            if (ViewState["VOUCHERLINEITEMNO"] == null)
                ViewState["VOUCHERLINEITEMNO"] = 10;
            VoucherEdit();         
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void VoucherEdit()
    {
        if (ViewState["VoucherID"] != null && ViewState["VoucherID"].ToString() != string.Empty)
        {

            DataSet ds = PhoenixAccountsVoucher.VoucherEdit(int.Parse(ViewState["VoucherID"].ToString()));
            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];                
                ViewState["VoucherDate"] = dr["FLDVOUCHERDATE"].ToString();              
            }
        }
    }
    protected void MenuOffSettingAdd_TabStripCommand(object sender, EventArgs e)  
    {
         DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
         try
         {
             if (dce.CommandName.ToUpper().Equals("CREATEVOUCHER"))
             {
                 Response.Redirect("../Accounts/AccountsOffSettingEntriesCreateVoucher.aspx?VOUCHERLINEITEMID=" + ViewState["VOUCHERLINEITEMID"].ToString());
             }
         }
         catch (Exception ex)
         {
             ucError.ErrorMessage = ex.Message;
             ucError.Visible = true;
             return;
         }
    }
    protected void MenuContraVoucher_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {            
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidOffSetAdd())
                {
                    ucError.Visible = true;
                    return;
                }
               
                    DataTable dtVoucher = new DataTable();
                    if (ViewState["VOUCHERLINEITEMID"] != null)
                    {
                        if (Session["VoucherData" + ViewState["VOUCHERLINEITEMID"].ToString()] != null)
                            dtVoucher = (DataTable)Session["VoucherData" + ViewState["VOUCHERLINEITEMID"].ToString()];
                        else
                        {
                            dtVoucher.Columns.Add("FLDVOUCHERLINEITEMID", System.Type.GetType("System.Guid"));
                            dtVoucher.Columns.Add("FLDVOUCHERID", System.Type.GetType("System.Int32"));
                            dtVoucher.Columns.Add("FLDVOUCHERLINEITEMNO", System.Type.GetType("System.Int32"));
                            dtVoucher.Columns.Add("FLDACCOUNTID", System.Type.GetType("System.Int32"));
                            dtVoucher.Columns.Add("FLDBUDGETID", System.Type.GetType("System.Int32"));
                            dtVoucher.Columns.Add("FLDCURRENCYCODE", System.Type.GetType("System.Int32"));
                            dtVoucher.Columns.Add("FLDBASEEXCHANGERATE", System.Type.GetType("System.Decimal"));
                            dtVoucher.Columns.Add("FLDREPORTEXCHANGERATE", System.Type.GetType("System.Decimal"));
                            dtVoucher.Columns.Add("FLDAMOUNT", System.Type.GetType("System.Decimal"));
                            dtVoucher.Columns.Add("FLDCOSTCENTER", System.Type.GetType("System.String"));
                            dtVoucher.Columns.Add("FLDPROFITCENTER", System.Type.GetType("System.String"));
                            dtVoucher.Columns.Add("FLDACTIVEYN", System.Type.GetType("System.Int32"));
                            dtVoucher.Columns.Add("FLDLOCKEDYN", System.Type.GetType("System.Int32"));
                            dtVoucher.Columns.Add("FLDREMITTANCENO", System.Type.GetType("System.String"));
                            dtVoucher.Columns.Add("FLDCREATEDBY", System.Type.GetType("System.Int32"));
                            dtVoucher.Columns.Add("FLDCREATEDDATE", System.Type.GetType("System.DateTime"));
                            dtVoucher.Columns.Add("FLDUPDATEDBY", System.Type.GetType("System.Int32"));
                            dtVoucher.Columns.Add("FLDUPDATEDDATE", System.Type.GetType("System.DateTime"));
                            dtVoucher.Columns.Add("FLDREFERENCEDOCUMENTNO", System.Type.GetType("System.String"));
                            dtVoucher.Columns.Add("FLDLONGDESCRIPTION", System.Type.GetType("System.String"));
                            dtVoucher.Columns.Add("FLDACCOUNTCODE", System.Type.GetType("System.String"));
                            dtVoucher.Columns.Add("FLDACCOUNTDESCRIPTION", System.Type.GetType("System.String"));
                            dtVoucher.Columns.Add("FLDACCOUNTUSAGENAME", System.Type.GetType("System.String"));
                            dtVoucher.Columns.Add("FLDACCOUNTSOURCENAME", System.Type.GetType("System.String"));
                            dtVoucher.Columns.Add("FLDSUBACCOUNTMAPID", System.Type.GetType("System.Guid"));
                            dtVoucher.Columns.Add("FLDBUDGETCODE", System.Type.GetType("System.String"));
                            dtVoucher.Columns.Add("FLDBUDGETDESCRIPTION", System.Type.GetType("System.String"));
                            dtVoucher.Columns.Add("FLDACCOUNTACTIVEYN", System.Type.GetType("System.String"));
                            dtVoucher.Columns.Add("FLDVOUCHERNUMBER", System.Type.GetType("System.String"));
                            dtVoucher.Columns.Add("FLDVOUCHERID", System.Type.GetType("System.String"));
                            dtVoucher.Columns.Add("FLDCURRENCYNAME", System.Type.GetType("System.String")); 
                        }
                        DataRow drVoucher = dtVoucher.NewRow();
                        drVoucher[0] = new Guid(ViewState["VOUCHERLINEITEMID"].ToString());
                        drVoucher[1] = Convert.ToInt32(ViewState["VoucherID"]);
                        drVoucher[2] = Convert.ToInt32(ViewState["VOUCHERLINEITEMNO"]) + 10;
                        drVoucher[3] = Convert.ToInt32(txtAccountId.Text);
                        drVoucher[5] = Convert.ToInt32(ddlCurrencyCode.SelectedCurrency);
                        drVoucher[6] = Convert.ToDecimal(txtExchangeRate.Text);
                        drVoucher[7] = Convert.ToDecimal(txt2ndExchangeRate.Text);
                        drVoucher[8] = Convert.ToDecimal(txtAmount.Text);
                        drVoucher[11] = 1;
                        drVoucher[12] = 0;
                        drVoucher[14] = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                        drVoucher[15] = DateTime.Now.ToShortDateString();
                        drVoucher[16] = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                        drVoucher[17] = DateTime.Now.ToShortDateString();
                        drVoucher[19] = txtLongDescription.Text;
                        drVoucher[20] = txtAccountCode.Text;
                        drVoucher[21] = txtAccountDescription.Text;
                        drVoucher[22] = txtAccountUsage.Text;
                        drVoucher[23] = txtAccountSource.Text;
                        drVoucher[24] = new Guid(txtBudgetId.Text);
                        drVoucher[25] = txtBudgetCode.Text;
                        drVoucher[30] = ViewState["CurrencyName"].ToString();
                        dtVoucher.Rows.Add(drVoucher);
                        Session["VoucherData" + ViewState["VOUCHERLINEITEMID"].ToString()] = dtVoucher;
                        ucStatus.Text = "Voucher Line Item is saved";

                        if (Session["VoucherBalance" + ViewState["VOUCHERLINEITEMID"].ToString()] == null)
                            Session["VoucherBalance" + ViewState["VOUCHERLINEITEMID"].ToString()] = Convert.ToDecimal(txtAmount.Text);
                        else
                            Session["VoucherBalance" + ViewState["VOUCHERLINEITEMID"].ToString()] = (decimal)Session["VoucherBalance" + ViewState["VOUCHERLINEITEMID"].ToString()] + Convert.ToDecimal(txtAmount.Text);

                        //PhoenixAccountsOffSettingEntries.ContraVoucherLineItemAmountUpdate((Guid)General.GetNullableGuid(ViewState["VOUCHERLINEITEMID"].ToString())
                        //                                                          , (decimal)Session["VoucherBalance" + ViewState["VOUCHERLINEITEMID"].ToString()]
                        //                                                          , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                        txtAccountId.Text = txtAmount.Text = txtLongDescription.Text = txtAccountCode.Text = txtAccountDescription.Text = txtAccountUsage.Text = txtBudgetId.Text = txtBudgetCode.Text = txtBudgetName.Text = txtBudgetgroupId.Text = txtAccountSource.Text = "";
                    }
                    //Response.Redirect("../Accounts/AccountsOffSettingEntriesCreateVoucher.aspx?VOUCHERLINEITEMID=" + ViewState["VOUCHERLINEITEMID"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private bool IsValidOffSetAdd()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (txtLongDescription.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Long description is required";            
        if (txtAccountCode.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Account code is required";
        if (txtAccountDescription.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Account description is required";        
        if (ddlCurrencyCode.SelectedCurrency.ToUpper() == "DUMMY")
            ucError.ErrorMessage = "Currency is required.";        
        if (txtAmount.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is Required";
        if (txtExchangeRate.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Base rate is required";
        if (txt2ndExchangeRate.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Report rate is required";
        if (txtBudgetCode.Text.Trim().Equals("") || txtBudgetId.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Sub account is required";

        return (!ucError.IsError);
    }  
    protected void txtAccountCode_changed(object sender, EventArgs e)
    {
        Session["sAccountCode"] = txtAccountCode.Text;
        Session["sAccountCodeDescription"] = txtAccountDescription.Text;
        Session["sSubAccountId"] = txtBudgetCode.Text;
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
    protected void txtBaseRate_TextChanged(object sender, EventArgs e) 
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
    protected void txtReportRate_TextChanged(object sender, EventArgs e)
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
}
