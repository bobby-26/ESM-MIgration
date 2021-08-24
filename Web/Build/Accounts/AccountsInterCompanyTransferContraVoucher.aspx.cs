using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class AccountsInterCompanyTransferContraVoucher : PhoenixBasePage
{
    public int iUserCode;
    public int iCompanyid;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        iUserCode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;        
        
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyid, 0);

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            txtAccountId.Attributes.Add("style", "visibility:hidden;");

            if (Request.QueryString["interlineitemisposted"] != null && Request.QueryString["interlineitemisposted"] != string.Empty)
                ViewState["interlineitemisposted"] = Request.QueryString["interlineitemisposted"].ToString();
            int posted = int.Parse(ViewState["interlineitemisposted"] != null ? ViewState["interlineitemisposted"].ToString() : "0");
            txtVoucherDate.Text = General.GetDateTimeToString(DateTime.Now.ToString());

            if (Request.QueryString["VOUCHERID"] != string.Empty)
            {
                ViewState["VOUCHERID"] = Request.QueryString["VOUCHERID"];
                ViewState["VOUCHERID1"] = ViewState["VOUCHERID"];
            }
            if (Request.QueryString["offsettingvoucherid"] != null && Request.QueryString["offsettingvoucherid"] != string.Empty)
                ViewState["offsettingvoucherid"] = Request.QueryString["offsettingvoucherid"];
            if (Request.QueryString["offsettinglineitemid"] != null && Request.QueryString["offsettinglineitemid"] != string.Empty)
                ViewState["OFFSETTINGLINEITEMID"] = Request.QueryString["offsettinglineitemid"].ToString();

            BindFields();
            LoadVoucherType();
            VoucherEdit();

            if (ViewState["FULLYCONTRAOUTYN"] != null && ViewState["FULLYCONTRAOUTYN"].ToString() == "0")
            {
                             
                toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar1.AddButton("New", "NEW", ToolBarDirection.Right);
                MenuVoucher.AccessRights = this.ViewState;
                MenuVoucher.MenuList = toolbar1.Show();
                //MenuVoucher.SetTrigger(pnlVoucher);
            }
            else if (posted != 1 && ViewState["OFFSETTINGLINEITEMID"] != null)
            {
                toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuVoucher.AccessRights = this.ViewState;
                MenuVoucher.MenuList = toolbar1.Show();
               // MenuVoucher.SetTrigger(pnlVoucher);
            }
            else
            {
                ddlVoucherType.Enabled = false;
            }

            ddlBankAccount.Enabled = false;
            imgShowAccount.Visible = false;
            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"] != string.Empty)
                ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();
            iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
        }        
        for (int i = 0; i < Session.Contents.Count; i++)
        {
            if (Session.Keys[i].ToString().StartsWith("INTERCONTRAVOUCHERCURRENCYID"))
                Session.Remove(Session.Keys[i].ToString());
        }
    }

    private void BindFields()
    {
        try
        {
            if ((Request.QueryString["offsettinglineitemid"] != null) && (Request.QueryString["offsettinglineitemid"] != ""))
            {

                DataSet ds = PhoenixAccountsOffSettingEntries.OffSettingLineItemsList(
                                                   new Guid(ViewState["OFFSETTINGLINEITEMID"].ToString())
                                                   );
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtOffSettingVoucherNumber.Text = dr["FLDVOUCHERNUMBER"].ToString();
                    //txtLongDescription.Text = dr["FLDLONGDESCRIPTION"].ToString();
                    txtReferenceNo.Text = dr["FLDREFERENCEDOCUMENTNO"].ToString();
                    ddlCurrencyCode.SelectedCurrency = dr["FLDCURRENCYCODE"].ToString();
                    txtAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDAMOUNT"]));
                    txtAllocatedVoucherAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDPREVIOUSVOUCHERAMOUNT"]));
                    txtBalanceVoucherAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDBALANCEAMOUNT"]));
                    if (dr["FLDTARGETCOMPANYNAME"] != null && dr["FLDTARGETCOMPANYNAME"].ToString() != string.Empty)
                        txttargetCompany.Text = dr["FLDTARGETCOMPANYNAME"].ToString();
                    else
                        txttargetCompany.Text = "Not defined";
                    ViewState["AMOUNT"] = txtAmount.Text;
                    if (ViewState["AMOUNT"] == null || ViewState["AMOUNT"].ToString() == string.Empty)
                        ViewState["AMOUNT"] = 0;
                    ViewState["FROMDATE"] = dr["FLDVOUCHERCREATEDDATE"].ToString();
                    ViewState["COMPANYID"] = dr["FLDCOMPANYID"].ToString();
                    ViewState["TODATE"] = DateTime.Now.ToString();

                    ViewState["PRIMEAMOUNT"] = dr["FLDPRIMEAMOUNT"].ToString();
                    ViewState["CURRENCYCODE"] = dr["FLDCURRENCYCODE"].ToString();
                    ViewState["BASEEXCHANGERATE"] = dr["FLDBASEEXCHANGERATE"].ToString();
                    ViewState["REPORTEXCHANGERATE"] = dr["FLDREPORTEXCHANGERATE"].ToString();
                    ViewState["SUBACCOUNTMAPID"] = dr["FLDSUBACCOUNTMAPID"].ToString();
                    ViewState["ACCOUNTID"] = dr["FLDACCOUNTID"].ToString();
                    ViewState["FULLYCONTRAOUTYN"] = dr["FLDFULLYCONTRAOUTYN"].ToString();                    
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Approve(object sender, EventArgs e)
    {
        if (sender != null && ((UserControlConfirmMessage)sender).confirmboxvalue == 1)
        {
            ViewState["approvestatus"] = 0;
        }
        else
        {
            ViewState["approvestatus"] = 1;
            return;
        }

    }

    protected void Voucher_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            System.Collections.Hashtable htLineItem = (new System.Collections.Hashtable());
            string accountid, baserate, reportrate, amount, suaacountid, courrencycode;
            accountid = baserate = reportrate = amount = suaacountid = courrencycode = "";
            Guid iVoucherLineItemId = Guid.Empty;

            if (ViewState["PRIMEAMOUNT"] != null && ViewState["BASEEXCHANGERATE"] != null && ViewState["REPORTEXCHANGERATE"] != null)
            {
                amount = ViewState["PRIMEAMOUNT"].ToString();
                courrencycode = ViewState["CURRENCYCODE"].ToString();
                baserate = ViewState["BASEEXCHANGERATE"].ToString();
                reportrate = ViewState["REPORTEXCHANGERATE"].ToString();
                suaacountid = ViewState["SUBACCOUNTMAPID"].ToString();
                accountid = ViewState["ACCOUNTID"].ToString();
            }

            if (CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }
            if (ViewState["approvestatus"] == null)
            {
                ViewState["approvestatus"] = 0;
            }
            if (CommandName.ToUpper().Equals("SAVE") && ViewState["approvestatus"].ToString() == "0")
            {
                int iVoucherId = 0;
                if (!IsValidVoucher())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["VOUCHERID"] == null)
                {
                    try
                    {
                        if (ddlVoucherType.SelectedValue.ToString() == "69")
                        {
                            DataSet ds = PhoenixRegistersAccount.ListBankAccount(Convert.ToInt32(ddlBankAccount.SelectedBankAccount.ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr = ds.Tables[0].Rows[0];
                                ViewState["VoucherNumberNameValuelist"] = ",CURRENCYCODE=" + dr["FLDCURRENCYCHAR"].ToString() + ",ACCOUNT=" + dr["FLDACCOUNTCHAR"].ToString() + ",";
                            }
                            PhoenixAccountsContraVoucher.ContraVoucherInsert(
                                                                     PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                      69,
                                                                      int.Parse(ddlBankAccount.SelectedBankAccount),
                                                                      DateTime.Parse(txtVoucherDate.Text),
                                                                      txtReferenceNumber.Text,
                                                                      chkLocked.Checked == true ? 1 : 0,
                                                                      txtLongDescription.Text,
                                                                      DateTime.Parse(txtVoucherDate.Text),
                                                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                      ref iVoucherId,
                                                                      ViewState["VoucherNumberNameValuelist"].ToString(),
                                                                      ViewState["offsettingvoucherid"] != null ? General.GetNullableInteger(ViewState["offsettingvoucherid"].ToString()) : null,
                                                                      new Guid(ViewState["OFFSETTINGLINEITEMID"].ToString())
                                                                    );
                            PhoenixAccountsContraVoucher.ContraVoucherLineItemInsert(
                                                            iVoucherId,
                                                            int.Parse(accountid),
                                                            int.Parse(courrencycode),
                                                            decimal.Parse(baserate),
                                                            decimal.Parse(reportrate),
                                                            -decimal.Parse(amount),
                                                            string.Empty,
                                                            string.Empty,
                                                            1,
                                                            0,
                                                            string.Empty,
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            string.Empty,
                                                            txtLongDescription.Text,
                                                            General.GetNullableGuid(suaacountid),
                                                            ref iVoucherLineItemId,
                                                            ViewState["offsettingvoucherid"] != null ? General.GetNullableInteger(ViewState["offsettingvoucherid"].ToString()) : null,
                                                            new Guid(ViewState["OFFSETTINGLINEITEMID"].ToString())
                                                            );
                        }
                        else if (ddlVoucherType.SelectedValue.ToString() == "71")
                        {
                            DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(txtAccountId.Text));
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr = ds.Tables[0].Rows[0];
                                ViewState["VoucherNumberNameValuelist"] = ",CURRENCYCODE=" + dr["FLDVOUCHERPREFIXCURRENCYCODE"].ToString() + ",";
                            }
                            PhoenixAccountsContraVoucher.ContraVoucherInsert(
                                                                PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                  71,
                                                                  int.Parse(txtAccountId.Text),
                                                                  DateTime.Parse(txtVoucherDate.Text),
                                                                  txtReferenceNumber.Text,
                                                                  chkLocked.Checked == true ? 1 : 0,
                                                                  txtLongDescription.Text,
                                                                  General.GetNullableDateTime(string.Empty),
                                                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                  ref iVoucherId,
                                                                  ViewState["VoucherNumberNameValuelist"].ToString(),
                                                                  ViewState["offsettingvoucherid"] != null ? General.GetNullableInteger(ViewState["offsettingvoucherid"].ToString()) : null,
                                                                  new Guid(ViewState["OFFSETTINGLINEITEMID"].ToString())
                                                                );
                            PhoenixAccountsContraVoucher.ContraVoucherLineItemInsert(
                                                            iVoucherId,
                                                            int.Parse(accountid),
                                                            int.Parse(courrencycode),
                                                            decimal.Parse(baserate),
                                                            decimal.Parse(reportrate),
                                                            -decimal.Parse(amount),
                                                            string.Empty,
                                                            string.Empty,
                                                            1,
                                                            null,
                                                            string.Empty,
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            string.Empty,
                                                            txtLongDescription.Text,
                                                            General.GetNullableGuid(suaacountid),
                                                            ref iVoucherLineItemId,
                                                            ViewState["offsettingvoucherid"] != null ? General.GetNullableInteger(ViewState["offsettingvoucherid"].ToString()) : null,
                                                            new Guid(ViewState["OFFSETTINGLINEITEMID"].ToString())
                                                            );
                        }
                        else
                        {

                            PhoenixAccountsContraVoucher.ContraVoucherInsert(
                                                             PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                               Convert.ToInt32(ddlVoucherType.SelectedValue.ToString()),
                                                               0,
                                                               DateTime.Parse(txtVoucherDate.Text),
                                                               txtReferenceNumber.Text,
                                                               chkLocked.Checked == true ? 1 : 0,
                                                               txtLongDescription.Text,
                                                               General.GetNullableDateTime(string.Empty),
                                                               PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                               ref iVoucherId,
                                                               string.Empty,
                                                               ViewState["offsettingvoucherid"] != null ? General.GetNullableInteger(ViewState["offsettingvoucherid"].ToString()) : null,
                                                               new Guid(ViewState["OFFSETTINGLINEITEMID"].ToString())
                                                             );
                            PhoenixAccountsContraVoucher.ContraVoucherLineItemInsert(
                                                            iVoucherId,
                                                            int.Parse(accountid),
                                                            int.Parse(courrencycode),
                                                            decimal.Parse(baserate),
                                                            decimal.Parse(reportrate),
                                                            -decimal.Parse(amount),
                                                            string.Empty,
                                                            string.Empty,
                                                            1,
                                                            null,
                                                            string.Empty,
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            string.Empty,
                                                            txtLongDescription.Text,
                                                            General.GetNullableGuid(suaacountid),
                                                            ref iVoucherLineItemId,
                                                            ViewState["offsettingvoucherid"] != null ? General.GetNullableInteger(ViewState["offsettingvoucherid"].ToString()) : null,
                                                            new Guid(ViewState["OFFSETTINGLINEITEMID"].ToString())
                                                            );
                        }
                        ucStatus.Text = "Voucher information added.";
                    }
                    catch (Exception ex)
                    {
                        ucError.HeaderMessage = "";
                        ucError.ErrorMessage = ex.Message;
                        ucError.Visible = true;
                        return;
                    }
                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                    Session["New"] = "Y";
                }
                else
                {
                    int isposted = int.Parse(ViewState["interlineitemisposted"] != null ? ViewState["interlineitemisposted"].ToString() : "0");
                    iVoucherId = int.Parse(ViewState["VOUCHERID"].ToString());

                    if (isposted == 0)
                    {
                        PhoenixAccountsContraVoucher.ContraVoucherUpdate(int.Parse(ViewState["VOUCHERID"].ToString()), DateTime.Parse(txtVoucherDate.Text),
                                                                txtReferenceNumber.Text,
                                                                chkLocked.Checked ? 1 : 0,
                                                                txtLongDescription.Text,
                                                                General.GetNullableDateTime(ucDueDate.Text),
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                              );
                        ucStatus.Text = "Voucher information updated.";
                    }
                    else
                    {
                        ucError.HeaderMessage = "";
                        ucError.ErrorMessage = "Voucher cannot be modified. Voucher already posted.";
                        ucError.Visible = true;
                        return;
                    }
                    if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "INTER")
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                            "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo',"
                            + (("'keepopen'")) + ");", true);
                    else
                    {
                        String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    private void LoadVoucherType()
    {
        try
        {
            string ShortCode = "BP,CP";
            DataSet ds = PhoenixAccountsOffSettingEntries.OffSettingVoucherType(ShortCode);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlVoucherType.DataSource = ds.Tables[0];
                ddlVoucherType.DataBind();
                ddlVoucherType.SelectedValue = "76";                
            }       
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Reset()
    {
        if (ViewState["VOUCHERID"] != null)
        {
            ViewState["VOUCHERID"] = null;
            txtVoucherNumber.Text = "";
            txtVoucherDate.Text = General.GetDateTimeToString(DateTime.Now.ToString());
            txtReferenceNumber.Text = "";
            chkLocked.Checked = false;
            txtUpdatedBy.Text = "";
            txtUpdatedDate.Text = "";
            txtLongDescription.Text = "";
            txtStatus.Text = "";
            ucDueDate.Text = "";
         //   ttlVoucher.Text = "Contra Voucher      ()     ";
            ddlVoucherType.SelectedValue = "76";
            ddlVoucherType.Enabled = true;            
            ViewState["offsetisposted"] = 0;
            ddlBankAccount.SelectedBankAccount = "";
            txtAccountCode.Text = txtAccountDescription.Text = txtAccountId.Text = "";           

            int posted = int.Parse(ViewState["interlineitemisposted"] != null ? ViewState["interlineitemisposted"].ToString() : "0");
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            if (ViewState["FULLYCONTRAOUTYN"] != null && ViewState["FULLYCONTRAOUTYN"].ToString() == "0")
            {
             
                toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar1.AddButton("New", "NEW", ToolBarDirection.Right);
                MenuVoucher.AccessRights = this.ViewState;
                MenuVoucher.MenuList = toolbar1.Show();
               // MenuVoucher.SetTrigger(pnlVoucher);
            }
            else if (posted != 1)
            {
                toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuVoucher.AccessRights = this.ViewState;
                MenuVoucher.MenuList = toolbar1.Show();
               // MenuVoucher.SetTrigger(pnlVoucher);
            }      
        }
    }

    protected void VoucherEdit()
    {
        if (ViewState["VOUCHERID"] != null)
        {
            DataSet ds = PhoenixAccountsContraVoucher.ContraVoucherEdit(int.Parse(ViewState["VOUCHERID"].ToString()));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtVoucherNumber.Text = dr["FLDVOUCHERNUMBER"].ToString();
                    txtVoucherDate.Text = General.GetDateTimeToString(dr["FLDVOUCHERDATE"].ToString());
                    txtReferenceNumber.Text = dr["FLDREFERENCEDOCUMENTNO"].ToString();
                    if (dr["FLDLOCKEDYN"].ToString() == "1")
                        chkLocked.Checked = true;
                    txtUpdatedBy.Text = dr["FLDLASTUPDATEBYUSERNAME"].ToString();
                    txtUpdatedDate.Text = dr["FLDUPDATEDDATE"].ToString();
                    txtLongDescription.Text = dr["FLDLONGDESCRIPTION"].ToString();
                    txtStatus.Text = dr["FLDVOUCHERSTATUSNAME"].ToString();
                    ucDueDate.Text = General.GetDateTimeToString(dr["FLDDUEDATE"].ToString());
                    ViewState["SubVoucherTypeId"] = dr["FLDSUBVOUCHERTYPEID"].ToString();
                    ddlVoucherType.SelectedValue = dr["FLDVOUCHERTYPEID"].ToString();
                    ddlVoucherType.Enabled = false;
                    if (dr["FLDVOUCHERTYPEID"] != null && dr["FLDVOUCHERTYPEID"].ToString() == "69")
                        ddlBankAccount.SelectedBankAccount = dr["FLDACCOUNTID"].ToString();
                    if (dr["FLDVOUCHERTYPEID"] != null && dr["FLDVOUCHERTYPEID"].ToString() == "71")
                    {
                        txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
                        txtAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
                        txtAccountDescription.Text = dr["FLDACCOUNTDESCRIPTION"].ToString();
                    }                    
                    if (int.Parse(dr["FLDISPERIODLOCKED"].ToString()) == 1)
                        AddRemoveSaveButton();
                }
            }
        }
    }

    private void AddRemoveSaveButton()
    {
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        int posted = int.Parse(ViewState["interlineitemisposted"] != null ? ViewState["interlineitemisposted"].ToString() : "0");
        if (posted != 1)
        {
            toolbar1.AddButton("Add", "ADD", ToolBarDirection.Right);
        }
        toolbar1.AddButton("New", "NEW", ToolBarDirection.Right);

        MenuVoucher.AccessRights = this.ViewState;
        MenuVoucher.MenuList = toolbar1.Show();
      //  MenuVoucher.SetTrigger(pnlVoucher);
    }

    protected void ddlBankAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBankAccount.SelectedBankAccount.ToUpper() != "DUMMY")
        {
            DataSet ds = PhoenixRegistersAccount.ListBankAccount(Convert.ToInt32(ddlBankAccount.SelectedBankAccount.ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["VoucherNumberNameValuelist"] = ",CURRENCYCODE=" + dr["FLDCURRENCYCHAR"].ToString() + ",ACCOUNT=" + dr["FLDACCOUNTCHAR"].ToString() + ",";
            }
        }
    }

    protected void ddlVoucherType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlVoucherType.SelectedValue.ToString() == "69")
        {            
            ddlBankAccount.Enabled = true;
            imgShowAccount.Visible = false;
            DataSet dsRate = PhoenixAccountsContraVoucher.ExchangeRateDifference(
                                                                            Convert.ToInt32(ViewState["COMPANYID"].ToString())
                                                                          , General.GetNullableDecimal(ViewState["AMOUNT"].ToString())
                                                                          , General.GetNullableDateTime(ViewState["FROMDATE"].ToString())
                                                                          , General.GetNullableDateTime(ViewState["TODATE"].ToString())
                                                                        );
            if (dsRate.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsRate.Tables[0].Rows[0];
                ViewState["baseamount"] = dr["FLDBASEAMOUNTDIFFERENCE"].ToString();
                ViewState["reportamount"] = dr["FLDREPORTAMOUNTDIFFERENCE"].ToString();
                if (ViewState["baseamount"] != null && ViewState["reportamount"] != null)
                {
                    if (decimal.Parse(ViewState["baseamount"].ToString()) != 0 || decimal.Parse(ViewState["reportamount"].ToString()) != 0)
                    {
                        ucConfirm.HeaderMessage = "Please Confirm";
                        if (decimal.Parse(ViewState["baseamount"].ToString()) != 0)
                            ucConfirm.Text = "Base Amount Difference : " + ViewState["baseamount"].ToString() + " ";
                        if (decimal.Parse(ViewState["reportamount"].ToString()) != 0)
                            ucConfirm.Text = "Report Amount Difference : " + ViewState["reportamount"].ToString();
                        ucConfirm.Visible = true;
                        ViewState["approvestatus"] = 1;
                    }
                }
            }	
        }
        if (ddlVoucherType.SelectedValue.ToString() == "76")
        {            
            ddlBankAccount.Enabled = false;
            imgShowAccount.Visible = false;
        }
        if (ddlVoucherType.SelectedValue.ToString() == "72")
        {            
            ddlBankAccount.Enabled = false;
            imgShowAccount.Visible = false;
        }
        if (ddlVoucherType.SelectedValue.ToString() == "74")
        {         
            ddlBankAccount.Enabled = false;
            imgShowAccount.Visible = false;
        }
        if (ddlVoucherType.SelectedValue.ToString() == "73")
        {         
            ddlBankAccount.Enabled = false;
            imgShowAccount.Visible = false;
        }
        if (ddlVoucherType.SelectedValue.ToString() == "75")
        {            
            ddlBankAccount.Enabled = false;
            imgShowAccount.Visible = false;
        }
        if (ddlVoucherType.SelectedValue.ToString() == "77")
        {         
            ddlBankAccount.Enabled = false;
            imgShowAccount.Visible = false;
        }
        if (ddlVoucherType.SelectedValue.ToString() == "71")
        {            
            txtAccountId.Enabled = txtAccountCode.Enabled = txtAccountDescription.Enabled = true;
            imgShowAccount.Visible = true;
            ddlBankAccount.Enabled = false;
            DataSet dsRate1 = PhoenixAccountsContraVoucher.ExchangeRateDifference(
                                                                            Convert.ToInt32(ViewState["COMPANYID"].ToString())
                                                                          , General.GetNullableDecimal(ViewState["AMOUNT"].ToString())
                                                                          , General.GetNullableDateTime(ViewState["FROMDATE"].ToString())
                                                                          , General.GetNullableDateTime(ViewState["TODATE"].ToString())
                                                                        );
            if (dsRate1.Tables[0].Rows.Count > 0)
            {
                DataRow dr1 = dsRate1.Tables[0].Rows[0];
                ViewState["baseamount"] = dr1["FLDBASEAMOUNTDIFFERENCE"].ToString();
                ViewState["reportamount"] = dr1["FLDREPORTAMOUNTDIFFERENCE"].ToString();
                if (ViewState["baseamount"] != null && ViewState["reportamount"] != null)
                {
                    if (decimal.Parse(ViewState["baseamount"].ToString()) != 0 || decimal.Parse(ViewState["reportamount"].ToString()) != 0)
                    {
                        ucConfirm.HeaderMessage = "Please Confirm";
                        if (decimal.Parse(ViewState["baseamount"].ToString()) != 0)
                            ucConfirm.Text = "Base Amount Difference : " + ViewState["baseamount"].ToString() + " ";
                        if (decimal.Parse(ViewState["reportamount"].ToString()) != 0)
                            ucConfirm.Text = "Report Amount Difference : " + ViewState["reportamount"].ToString();
                        ucConfirm.Visible = true;
                    }
                }
            }	
        }
    }

    protected bool IsValidVoucher()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime dtduedate = new DateTime();

        if (txtVoucherDate.Text == null)
            ucError.ErrorMessage = "Voucher date is required";
        if (txtReferenceNumber.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Reference number is required.";
        if (ucDueDate.Text != null && ucDueDate.Text.Trim().Length > 0)
        {
            dtduedate = DateTime.Parse(ucDueDate.Text);
            if (DateTime.Parse(dtduedate.ToShortDateString()) < DateTime.Parse(DateTime.Now.ToShortDateString()))
            {
                ucError.ErrorMessage = "Due date should be greater than or equal to current date.";
            }
        }
        if (ddlVoucherType.SelectedValue.ToString() == "69")
        {
            if (ddlBankAccount.SelectedBankAccount.ToUpper() == "DUMMY")
                ucError.ErrorMessage = "Bank account is required to create a bank payment voucher";
        }
        if (ddlVoucherType.SelectedValue.ToString() == "71")
        {
            if (txtAccountId.Text.Trim().Equals(""))
                ucError.ErrorMessage = "Cash account is required to create a cash payment voucher";
        }      
        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
