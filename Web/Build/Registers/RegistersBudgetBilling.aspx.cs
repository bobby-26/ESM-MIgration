using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;


public partial class RegistersBudgetBilling : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
       

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);

            MenuBudgetBilling.AccessRights = this.ViewState;
            MenuBudgetBilling.MenuList = toolbar.Show();

            txtBudgetCodeId.Attributes.Add("style", "visibility:hidden");
            txtCreditAccountId.Attributes.Add("style", "visibility:hidden");
            txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");

            BindRankList();

            if (Request.QueryString["BUDGETBILLINGID"] != null && Request.QueryString["BUDGETBILLINGID"] != string.Empty)
            {
                ViewState["BUDGETBILLINGID"] = Request.QueryString["BUDGETBILLINGID"].ToString();
                EditBudgetCode(new Guid(ViewState["BUDGETBILLINGID"].ToString()));
            }
            else
            {
                ViewState["BUDGETBILLINGID"] = null;
                Reset();
            }
            SessionUtil.PageAccessRights(this.ViewState);
          
        }
    }

    protected void EditBudgetCode(Guid BudgetCodeId)
    {
        txtBillingItemName.Enabled = false;

        DataSet ds = PhoenixRegistersBudgetBilling.EditBudgetBilling(BudgetCodeId);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            lblBudgetbillingid.Text = Convert.ToString(BudgetCodeId);
            txtBillingItemName.Text = dr["FLDBILLINGITEMDESCRIPTION"].ToString();
            ucBillingUnit.SelectedHard = dr["FLDBILLINGUNIT"].ToString();
            ucBillingBasis.SelectedHard = dr["FLDBILLINGBASIS"].ToString();
            ucFrequency.SelectedHard = dr["FLDFREQUENCY"].ToString();

            txtBudgetCode.Text = dr["FLDVESSELBUDGETCODE"].ToString();
            txtBudgetCodeDescription.Text = dr["FLDVESSELBUDGETDESCRIPTION"].ToString();
            txtBudgetCodeId.Text = dr["FLDVESSELBUDGETID"].ToString();

            txtCreditAccountCode.Text = dr["FLDCREDITACCOUNTCODE"].ToString();
            txtCreditAccountDescription.Text = dr["FLDCREDITACCOUNTDESCRIPTION"].ToString();
            txtCreditAccountId.Text = dr["FLDACCOUNTID"].ToString();
            General.RadBindComboBoxCheckList(chkRankList, dr["FLDRANKS"].ToString());

            if (dr["FLDFREQUENCY"] != null && dr["FLDFREQUENCY"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 188, "MTY"))
            {
                ucBillingBasis.CssClass = "dropdown_mandatory";
                txtBudgetCodeId.CssClass = "input_mandatory";
                txtBudgetCode.CssClass = "input_mandatory";
                txtBudgetCodeDescription.CssClass = "input_mandatory";

                txtCreditAccountCode.CssClass = "input_mandatory";
                txtCreditAccountDescription.CssClass = "input_mandatory";
                txtCreditAccountId.CssClass = "input_mandatory";
            }
            else
            {
                ucBillingBasis.Enabled = false;
                ucBillingBasis.CssClass = "input";
                txtBudgetCodeId.CssClass = "input";
                txtBudgetCode.CssClass = "input";
                txtBudgetCodeDescription.CssClass = "input";
                txtCreditAccountCode.CssClass = "input";
                txtCreditAccountDescription.CssClass = "input";
                txtCreditAccountId.CssClass = "input";
            }
        }
    }

    protected void BudgetBilling_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
               

                if (IsValidBudgetBilling())
                {
                    if (ViewState["BUDGETBILLINGID"] != null && ViewState["BUDGETBILLINGID"].ToString() != "")
                    {
                        PhoenixRegistersBudgetBilling.UpdateBudgetBilling(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , new Guid(ViewState["BUDGETBILLINGID"].ToString())
                            , txtBillingItemName.Text
                            , General.GetNullableInteger(ucBillingUnit.SelectedHard)
                            , General.GetNullableInteger(ucFrequency.SelectedHard)
                            , General.GetNullableInteger(ucBillingBasis.SelectedHard)
                            , General.GetNullableInteger(txtBudgetCodeId.Text)
                            , General.GetNullableInteger(txtCreditAccountId.Text)
                            , General.GetNullableString(General.RadGetComboboxCheckList(chkRankList))
                         );

                        ucStatus.Text = "Budget Billing Information is updated.";
                        ucStatus.Visible = true;
                        EditBudgetCode(new Guid(ViewState["BUDGETBILLINGID"].ToString()));
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:parent.fnReloadList('code1');", true);
                    }
                    else
                    {
                        PhoenixRegistersBudgetBilling.InsertBudgetBilling(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , txtBillingItemName.Text
                            , General.GetNullableInteger(ucBillingUnit.SelectedHard.ToString())
                            , General.GetNullableInteger(ucFrequency.SelectedHard.ToString())
                            , General.GetNullableInteger(ucBillingBasis.SelectedHard.ToString())
                            , General.GetNullableInteger(txtBudgetCodeId.Text)
                            , General.GetNullableInteger(txtCreditAccountId.Text)
                            , General.GetNullableString(General.RadGetComboboxCheckList(chkRankList))
                           );

                        ucStatus.Text = "Budget Billing Information is saved.";
                        ucStatus.Visible = true;

                        String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucFrequency_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableString(ucFrequency.SelectedHard) == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 188, "MTY"))
        {
            ucBillingBasis.Enabled = true;
            ucBillingBasis.CssClass = "dropdown_mandatory";
            txtBudgetCodeId.CssClass = "input_mandatory";
            txtBudgetCode.CssClass = "input_mandatory";
            txtBudgetCodeDescription.CssClass = "input_mandatory";

            txtCreditAccountCode.CssClass = "input_mandatory";
            txtCreditAccountDescription.CssClass = "input_mandatory";
            txtCreditAccountId.CssClass = "input_mandatory";
        }
        else
        {
            ucBillingBasis.Enabled = false;
            ucBillingBasis.SelectedHard = "";
            ucBillingBasis.CssClass = "input";
            txtBudgetCodeId.CssClass = "input";
            txtBudgetCode.CssClass = "input";
            txtBudgetCodeDescription.CssClass = "input";

            txtCreditAccountCode.CssClass = "input";
            txtCreditAccountDescription.CssClass = "input";
            txtCreditAccountId.CssClass = "input";
        }
        ucBillingBasis_Changed(ucBillingBasis, e);
    }

    protected void ucBillingBasis_Changed(object sender, EventArgs e)
    {
        string allcrew = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 189, "ACR");
        string officer = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 189, "OFO");
        string joiners = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 189, "JIO");
        string notapplicable = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 189, "NON");
        string cadetsortrainers = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 189, "COT");
        string rank = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 189, "RNK");
        if (General.GetNullableString(ucBillingBasis.SelectedHard) == rank)
        {
            chkRankList.CssClass = "dropdown_mandatory";
            chkRankList.Enabled = true;
        }
        else
        {
            chkRankList.CssClass = "input";
            chkRankList.ClearCheckedItems();
            chkRankList.Enabled = false;


        }
        if (General.GetNullableString(ucFrequency.SelectedHard) == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 188, "MTY"))
        {
            if (General.GetNullableString(ucBillingBasis.SelectedHard) == allcrew || General.GetNullableString(ucBillingBasis.SelectedHard) == officer || General.GetNullableString(ucBillingBasis.SelectedHard) == joiners || General.GetNullableString(ucBillingBasis.SelectedHard) == cadetsortrainers || General.GetNullableString(ucBillingBasis.SelectedHard) == rank)
            {
                ucBillingUnit.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 187, 0, "PPN");
            }
            else if (General.GetNullableString(ucBillingBasis.SelectedHard) == notapplicable)
            {
                ucBillingUnit.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 187, 0, "PDY,LUM");
            }
        }
        else if (General.GetNullableString(ucFrequency.SelectedHard) == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 188, "AIN"))
        {
            ucBillingUnit.HardList = null;
        }
    }
    private bool IsValidBudgetBilling()
    {
        string rank = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 189, "RNK");
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableString(ucBillingBasis.SelectedHard) == rank)
        {
            if (chkRankList.Text == "" || chkRankList.Text == null)
                ucError.ErrorMessage = "Rank is required.";

        }
        if (txtBillingItemName.Text.Equals(""))
            ucError.ErrorMessage = "Billing Item Description is required.";

        if (General.GetNullableInteger(ucFrequency.SelectedHard) == null)
            ucError.ErrorMessage = "Frequency is required.";
        else
        {
            if (General.GetNullableString(ucFrequency.SelectedHard) == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 188, "MTY"))
            {
                if (General.GetNullableInteger(ucBillingBasis.SelectedHard) == null)
                    ucError.ErrorMessage = "Billing Basis is required.";

                if (txtBudgetCodeId.Text.Equals(""))
                    ucError.ErrorMessage = "Budget Code is required.";

                if (txtCreditAccountId.Text.Equals(""))
                    ucError.ErrorMessage = "Credit Account is required.";
            }
        }

        if (General.GetNullableInteger(ucBillingUnit.SelectedHard) == null)
            ucError.ErrorMessage = "Billing Unit is required.";

        return (!ucError.IsError);

    }

    private void Reset()
    {
        ViewState["BUDGETBILLINGID"] = null;

        ucBillingBasis.Enabled = true;
        txtBillingItemName.Enabled = true;

        ucBillingBasis.CssClass = "dropdown_mandatory";
        txtBudgetCodeId.CssClass = "input_mandatory";
        txtBudgetCode.CssClass = "input_mandatory";
        txtBudgetCodeDescription.CssClass = "input_mandatory";

        txtCreditAccountCode.CssClass = "input_mandatory";
        txtCreditAccountDescription.CssClass = "input_mandatory";
        txtCreditAccountId.CssClass = "input_mandatory";
        chkRankList.ClearCheckedItems();


        txtBillingItemName.Text = "";
        ucBillingUnit.SelectedHard = "";
        ucBillingBasis.SelectedHard = "";
        ucFrequency.SelectedHard = "";
        txtBudgetCode.Text = "";
        txtBudgetCodeDescription.Text = "";
        txtBudgetCodeId.Text = "";
        txtCreditAccountCode.Text = "";
        txtCreditAccountDescription.Text = "";
        txtCreditAccountId.Text = "";


    }
    private void BindRankList()
    {
        DataSet ds = PhoenixRegistersRank.ListRank();
        chkRankList.DataSource = ds;
        chkRankList.DataTextField = "FLDRANKNAME";
        chkRankList.DataValueField = "FLDRANKID";
        chkRankList.DataBind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        EditBudgetCode(new Guid(ViewState["BUDGETBILLINGID"].ToString()));

    }
}