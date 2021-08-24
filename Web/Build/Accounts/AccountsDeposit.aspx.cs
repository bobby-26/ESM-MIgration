using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class Accounts_AccountsDeposit : PhoenixBasePage
{
    public int iUserCode;

    protected void Page_Load(object sender, EventArgs e)

    {
        SessionUtil.PageAccessRights(this.ViewState);
        txtSupplierCode.Attributes.Add("readonly", "readonly");
        txtSupplierName.Attributes.Add("readonly", "readonly");
        txtSupplierId.Attributes.Add("readonly", "readonly");
        txtSupplierId.Attributes.Add("style", "display:none");
        txtBankID.Attributes.Add("style", "display:none");

        iUserCode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;

        if (!IsPostBack)
        {

            btnPickSupplier.Attributes.Add("onclick", "return showPickList('spnPickListSupplier', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?framename=ifMoreInfo&addresstype=131', true); ");

            ddlLiabilityCompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();

            if (Request.QueryString["depositid"] != string.Empty)
            {
                ViewState["DEPOSITID"] = Request.QueryString["depositid"];
                DepositEdit();
            }
        }

        btnPickBank.Attributes.Add("onclick", "return showPickList('spnPickListBank', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBankInformationAddress.aspx?framename=ifMoreInfo&addresscode=" + txtSupplierId.Text + "&currency=" + General.GetDateTimeToString(ucCurrency.SelectedCurrency) + "', true);");

        PhoenixToolbar toolbar1 = new PhoenixToolbar();

        toolbar1.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar1.AddButton("Approve", "APPROVE", ToolBarDirection.Right);
        toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar1.AddButton("New", "NEW", ToolBarDirection.Right);
        //toolbar1.AddButton("History", "HISTORY");

        MenuDeposit.AccessRights = this.ViewState;
        MenuDeposit.MenuList = toolbar1.Show();

    }

    protected void MenuDeposit_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
        }
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                if (!IsValidDeposit())
                {
                    ucError.Visible = true;
                    return;
                }

                string txtExchangeRate = "0";
                string txt2ndExchangeRate = "0";

                DataSet ds = PhoenixRegistersCompany.EditCompany(int.Parse(ddlLiabilityCompany.SelectedCompany));
                if (ds.Tables.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    DataSet dsbasecurrency = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(dr["FLDBASECURRENCY"].ToString()), DateTime.Parse(DateTime.Now.ToShortDateString()));
                    DataSet dsReportcurrency = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(dr["FLDREPORTINGCURRENCY"].ToString()), DateTime.Parse(DateTime.Now.ToShortDateString()));

                    DataRow drbasecurrency = dsbasecurrency.Tables[0].Rows[0];
                    DataRow drreportcurrency = dsReportcurrency.Tables[0].Rows[0];

                    decimal dTransactionExchangerate = 0;

                    DataSet dsInvoice = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(ucCurrency.SelectedCurrency), DateTime.Parse(DateTime.Now.ToShortDateString()));
                    if (dsInvoice.Tables.Count > 0)
                    {
                        DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                        dTransactionExchangerate = decimal.Parse(drInvoice["FLDEXCHANGERATE"].ToString());
                    }


                    txtExchangeRate = string.Format(String.Format("{0:#####.00000000000000000}", dTransactionExchangerate / decimal.Parse(drbasecurrency["FLDEXCHANGERATE"].ToString())));
                    txt2ndExchangeRate = string.Format(String.Format("{0:#####.00000000000000000}", dTransactionExchangerate / decimal.Parse(drreportcurrency["FLDEXCHANGERATE"].ToString())));

                }

                if (ViewState["DEPOSITID"] == null)
                {
                    PhoenixAccountsDeposit.DepositInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , int.Parse(txtSupplierId.Text)
                        , int.Parse(ucCurrency.SelectedCurrency)
                        , decimal.Parse(txtAmount.Text)
                        , General.GetNullableDateTime(ucDate.Text)
                        , int.Parse(ddlLiabilityCompany.SelectedCompany)
                        , General.GetNullableInteger(txtBankID.Text)
                        , txtRemarks.Text
                        );

                    ucStatus.Text = "Deposit information added";
                    Reset();
                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                    Session["New"] = "Y";
                }
                else
                {
                    PhoenixAccountsDeposit.DepositUpdate(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ViewState["DEPOSITID"].ToString())
                        , int.Parse(txtSupplierId.Text)
                        , int.Parse(ucCurrency.SelectedCurrency)
                        , decimal.Parse(txtAmount.Text)
                        , General.GetNullableDateTime(ucDate.Text)
                        , int.Parse(ddlLiabilityCompany.SelectedCompany)
                        , General.GetNullableInteger(txtBankID.Text)
                        , txtRemarks.Text
                        , txtCancellationRemarks.Text
                        );
                    ucStatus.Text = "Deposit information updated";
                }
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
        if (CommandName.ToUpper().Equals("APPROVE"))
        {
            try
            {

                if (txtBankID.Text.Trim().Equals(""))
                {
                    ucError.ErrorMessage = "Supplier Banking Details is required.";
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["DEPOSITID"] != null)
                {
                    PhoenixAccountsDeposit.DepositApprove(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["DEPOSITID"].ToString())
                        , General.GetNullableInteger(txtBankID.Text)
                        );

                    ucStatus.Text = "Deposit approved";
                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
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
        if (CommandName.ToUpper().Equals("CANCEL"))
        {
            try
            {
                if (ViewState["DEPOSITID"] != null)
                {
                    PhoenixAccountsDeposit.DepositCancel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["DEPOSITID"].ToString()), General.GetNullableString(txtCancellationRemarks.Text));
                    //ucStatus.Text = "Deposit Reposted";
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            String scriptudate = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptudate, true);
        }
        DepositEdit();
    }


    protected void Deposit_SetExchangeRate(object sender, EventArgs e)
    {
        if (ucCurrency.SelectedCurrency.ToUpper() != "DUMMY")
        {
            DataSet dsInvoice = PhoenixRegistersExchangeRate.GetCurrencyExchangeRate(int.Parse(ucCurrency.SelectedCurrency));
            if (dsInvoice.Tables[0].Rows.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                txtExchangeRateEdit.Text = string.Format(String.Format("{0:#####.000000}", drInvoice["FLDEXCHANGERATE"]));
            }
        }
        else
        {
            txtExchangeRateEdit.Text = "";
        }

    }

    protected void Reset()
    {
        ViewState["DEPOSITID"] = null;
        txtDepositNo.Text = "";
        txtDepositType.Text = "";
        txtAmount.Text = "";
        ucCurrency.SelectedCurrency = string.Empty;
        ucDate.Text = "";
        txtRemarks.Text = "";
        txtSupplierCode.Text = "";
        txtSupplierId.Text = "";
        txtSupplierName.Text = "";
        txtStatus.Text = "";
        txtPaymentVoucherNo.Text = "";
        ddlLiabilityCompany.SelectedCompany = "";
        txtBankName.Text = "";
        txtBankID.Text = "";
        txtAccountNo.Text = "";
        txtCancellationRemarks.Text = "";
        txtApprovedBy.Text = "";
        ucApprovedDate.Text = "";
        btnPickBank.Attributes.Add("onclick", "return showPickList('spnPickListBank', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBankInformationAddress.aspx?framename=ifMoreInfo&addresscode=" + txtSupplierId.Text + "&currency=" + General.GetDateTimeToString(ucCurrency.SelectedCurrency) + "', true);");
    }

    protected void DepositEdit()
    {
        if (ViewState["DEPOSITID"] != null)
        {
            DataSet ds = PhoenixAccountsDeposit.DepositEdit(new Guid(ViewState["DEPOSITID"].ToString()));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtDepositNo.Text = dr["FLDDEPOSITNUMBER"].ToString();
                    txtAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDAMOUNT"])); 
                    ucCurrency.SelectedCurrency = dr["FLDCURRENCY"].ToString();
                    ucDate.Text = General.GetDateTimeToString(dr["FLDDATE"].ToString());
                    txtRemarks.Text = dr["FLDREMARKS"].ToString();
                    txtSupplierCode.Text = dr["FLDCODE"].ToString();
                    txtSupplierId.Text = dr["FLDSUPPLIERCODE"].ToString();
                    txtSupplierName.Text = dr["FLDNAME"].ToString();
                    txtStatus.Text = dr["FLDDEPOSITSTATUSNAME"].ToString();
                    txtDepositType.Text = dr["FLDTYPEDESCRIPTION"].ToString();
                    txtPaymentVoucherNo.Text = dr["FLDCREDITNOTENUMBER"].ToString();
                    ddlLiabilityCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();

                    //if (dr["FLDVENDORCREDITNOTENUMBERALREADYEXISTS"].ToString() == "1")
                    //{
                    //    HlinkRefDuplicate.NavigateUrl = "~/Accounts/AccountsCreditDebitNoteDuplicateList.aspx?creditdebitnoteid=" + ViewState["CREDITDEBITNOTEID"].ToString() + "&suppliercode=" + dr["FLDSUPPLIERCODE"].ToString() + "&referenceno=" + dr["FLDREFERENCENO"].ToString();
                    //    HlinkRefDuplicate.Visible = true;
                    //}
                    //imgBankPicklist.Attributes.Add("onclick", "return showPickList('spnPickListBank', 'codehelp1', '', '../Common/CommonPickListBankInformationAddress.aspx?addresscode=" + txtSupplierId.Text + "&currency=" + dr["FLDCURRENCY"] + "', true);");

                    txtBankName.Text = dr["FLDBANKNAME"].ToString();
                    txtBankID.Text = dr["FLDBANKID"].ToString();
                    txtAccountNo.Text = dr["FLDACCOUNTNUMBER"].ToString();
                    txtApprovedBy.Text = dr["FLDAPPROVEDBY"].ToString();
                    ucApprovedDate.Text = dr["FLDAPPROVEDATE"].ToString();
                    txtCancellationRemarks.Text = dr["FLDCANCELREMARKS"].ToString();

                }
            }
        }
    }

    protected bool IsValidDeposit()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtSupplierId.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Supplier Name is required";

        if (txtAmount.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required";

        if (General.GetNullableInteger(ucCurrency.SelectedCurrency) == null)
            ucError.ErrorMessage = "Currency is required";

        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Date is required";

        if (ddlLiabilityCompany.SelectedCompany == "")
            ucError.ErrorMessage = "Liability Company is required";

        //if (txtBankID.Text.Trim().Equals(""))
        //    ucError.ErrorMessage = "Supplier Banking Details is required.";

        if (txtRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Remarks is required";

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    //protected void btnPickSupplier_Click(object sender, ImageClickEventArgs e)
    //{

    //}

    //protected void btnPickBank_Click(object sender, ImageClickEventArgs e)
    //{


    //    //btnPickSupplier.Attributes.Add("onclick", "return showPickList('spnPickListSupplier', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131', true); ");
    //}

}
