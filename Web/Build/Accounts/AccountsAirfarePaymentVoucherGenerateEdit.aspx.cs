using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class AccountsAirfarePaymentVoucherGenerateEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("History", "HISTORY");
            toolbar.AddButton("Save", "SAVE");
            MenuPVGenerate.AccessRights = this.ViewState;
            MenuPVGenerate.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["AGENTINVOICEID"] = Request.QueryString["AGENTINVOICEID"].ToString();
                ViewState["VesselId"] = "";
                ViewState["OWNERID"] = "";
                ViewState["BudgetCode"] = "Dummy";
                TravelAgentEdit();
            }
            BindOwnerBudgetCode();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuPVGenerate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("HISTORY"))
            {
                //Response.Redirect("../Accounts/AccountsAirfarePaymentVoucherGenerateHistory.aspx?AGENTINVOICEID=" + ViewState["AGENTINVOICEID"].ToString());
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                "BookMarkScript", "Openpopup('History','','../Accounts/AccountsAirfarePaymentVoucherGenerateHistory.aspx?AGENTINVOICEID=" + ViewState["AGENTINVOICEID"].ToString()+"');", true);
            }
            else if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(ddlAccount.SelectedValue
                    ,ucBudgetCode.SelectedBudgetCode,ucOwnerBudgetCode.SelectedValue,txtChargedAmount.Text,txtPayableAmount.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsAirfarePaymentVoucherGenerate.TravelAgentPVUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["AGENTINVOICEID"].ToString())
                    , int.Parse(ddlAccount.SelectedValue)
                    , int.Parse(ucBudgetCode.SelectedBudgetCode)
                    , new Guid(ucOwnerBudgetCode.SelectedValue)
                    , int.Parse(chkWriteOff.Checked == true ? "1" : "0")
                    , Convert.ToDecimal(txtChargedAmount.Text)
                    , Convert.ToDecimal(txtPayableAmount.Text)
                    , txtRemarks.Text
                    );
                ucStatus.Text = "Updated";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private void TravelAgentEdit()
    {
        try
        {
            DataSet ds = PhoenixAccountsAirfarePaymentVoucherGenerate.TravelAgentPVEdit(new Guid(ViewState["AGENTINVOICEID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if (General.GetNullableInteger(dr["FLDVESSELID"].ToString()) == null)
                    ViewState["VesselId"] = "0";
                else
                    ViewState["VesselId"] = dr["FLDVESSELID"].ToString();

                ViewState["OWNERID"] = dr["FLDPRINCIPALID"].ToString();
                ddlAccount.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(null, 1);
                ddlAccount.DataBind();
                ddlAccount.SelectedValue = dr["FLDVESSELACCOUNTID"].ToString();
                ucBudgetCode.SelectedBudgetCode = dr["FLDBUDGETID"].ToString();
                ViewState["BudgetCode"] = dr["FLDBUDGETID"].ToString();
                if (dr["FLDOWNERBUDGETCODEID"].ToString() != "")
                {
                    ucOwnerBudgetCode.SelectedValue = dr["FLDOWNERBUDGETCODEID"].ToString();
                    ucOwnerBudgetCode.Text = dr["FLDOWNERBUDGETCODE"].ToString();
                }
                chkWriteOff.Checked = dr["FLDWRITEOFFYN"].ToString() == "1" ? true : false;
                txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
                txtChargedAmount.Text = dr["FLDCHARGEABLEAMOUNT"].ToString();
                txtPayableAmount.Text = dr["FLDPAYABLEAMOUNT"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                lblPayingCompanyCode.Text = dr["FLDAGENTCODE"].ToString();

                trPayingCompany.Visible = dr["FLDDIRECTBILLINGYN"].ToString().Equals("1") ? false : true;

                if (dr["FLDWRITEOFFYN"].ToString() == "1")
                {
                    ddlAccount.Enabled = false;
                    ucBudgetCode.Enabled = false;
                    ucOwnerBudgetCode.Enabled = false;
                    ddlAccount.CssClass = "readonlytextbox";
                    ucBudgetCode.CssClass = "readonlytextbox";
                    ucOwnerBudgetCode.CssClass = "readonlytextbox";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidData(string accountCode, string budgetCode, string ownerBudgetCode, string chargedAmount, string payableAmount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!chkWriteOff.Checked)
        {
            if (General.GetNullableInteger(accountCode) == null)
                ucError.ErrorMessage = "Account Code is required.";
            if (General.GetNullableInteger(budgetCode) == null)
                ucError.ErrorMessage = "Budget Code is required.";
            if (General.GetNullableGuid(ownerBudgetCode) == null)
                ucError.ErrorMessage = "Owner Budget Code is required.";
        }
        if (General.GetNullableDecimal(chargedAmount) == null)
            ucError.ErrorMessage = "Charged Amount is required.";
        if (General.GetNullableDecimal(payableAmount) == null)
            ucError.ErrorMessage = "Payable Amount is required.";

        return (!ucError.IsError);

    }

    protected void ucBudgetCode_Changed(object sender, EventArgs e)
    {
        ucOwnerBudgetCode.SelectedValue = "";
        ucOwnerBudgetCode.Text = "";


        ViewState["BudgetCode"] = ucBudgetCode.SelectedBudgetCode;

        if(General.GetNullableInteger(ViewState["OWNERID"].ToString())!=null && General.GetNullableInteger(ucBudgetCode.SelectedBudgetCode)!=null)
            BindOwnerBudgetCode();
    }

    private void BindOwnerBudgetCode()
    {
        if (General.GetNullableInteger(ViewState["OWNERID"].ToString()) != null && General.GetNullableInteger(ViewState["BudgetCode"].ToString()) != null)
        {
            ucOwnerBudgetCode.VesselId = ViewState["VesselId"].ToString();
            ucOwnerBudgetCode.BudgetId = ViewState["BudgetCode"].ToString();
            ucOwnerBudgetCode.OwnerId = ViewState["OWNERID"].ToString();

            //DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(ViewState["VesselId"].ToString()));
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ViewState["OWNERID"] = ds.Tables[0].Rows[0]["FLDPRINCIPAL"].ToString();
               
            //}
            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? iownerid = 0;
            DataSet ds1 = PhoenixCommonRegisters.InternalBillingOwnerBudgetCodeSearch(null
                                                                                     , null
                                                                                     , General.GetNullableInteger(ViewState["OWNERID"].ToString())
                                                                                     , null
                                                                                     , General.GetNullableInteger(ViewState["BudgetCode"].ToString())
                                                                                     , null, null
                                                                                     , 1
                                                                                     , General.ShowRecords(null)
                                                                                     , ref iRowCount
                                                                                     , ref iTotalPageCount
                                                                                     , ref iownerid);

            if (ds1.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds1.Tables[0].Rows[0];
                ucOwnerBudgetCode.SelectedValue = dr["FLDOWNERBUDGETCODEID"].ToString();
                ucOwnerBudgetCode.Text = dr["FLDOWNERBUDGETCODE"].ToString();
            }
        }
    }

    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccount.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", ""));
    }

    protected void chkWriteOff_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkWriteOff.Checked == true)
        {
            ddlAccount.Enabled = false;
            ucBudgetCode.Enabled = false;
            ucOwnerBudgetCode.Enabled = false;
            ddlAccount.CssClass = "readonlytextbox";
            ucBudgetCode.CssClass = "readonlytextbox";
            ucOwnerBudgetCode.CssClass = "readonlytextbox";
        }
        else if (chkWriteOff.Checked == false)
        {
            ddlAccount.Enabled = true;
            ucBudgetCode.Enabled = true;
            ucOwnerBudgetCode.Enabled = true;
            ddlAccount.CssClass = "input_mandatory";
            ucBudgetCode.CssClass = "dropdown_mandatory";
            ucOwnerBudgetCode.CssClass = "input_mandatory";
        }
    }
    protected void ddlAccountDetails_TextChanged(object sender, EventArgs agrs)
    {
        DataSet ds;
        if (General.GetNullableInteger(ddlAccount.SelectedValue) != null)
        {
            ds = PhoenixRegistersAccount.EditAccount(int.Parse(ddlAccount.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["OWNERID"] = ds.Tables[0].Rows[0]["FLDPRINCIPALID"].ToString();
            }
            if (General.GetNullableInteger(ViewState["BudgetCode"].ToString()) != null)
                BindOwnerBudgetCode();
        }
    }
}
