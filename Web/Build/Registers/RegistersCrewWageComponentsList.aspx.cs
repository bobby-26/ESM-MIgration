using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersCrewWageComponentsList : PhoenixBasePage
{
    string ComponentId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        ComponentId = Request.QueryString["compid"] == null ? String.Empty : Request.QueryString["compid"].ToString();

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuCWC.AccessRights = this.ViewState;
        MenuCWC.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            rblShowowneratsignoff.SelectedValue = "0";
            if (!string.IsNullOrEmpty(ComponentId))
            {
                EditContract(new Guid(ComponentId));
            }
        }
    }

    protected void CWC_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            string Script = "";
            Script += "<script language='JavaScript' id='BookMarkScript1'>";
            Script += "fnReloadList();";
            Script += "</script>";
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string shortcode = txtShortCode.Text.Trim();
                string name = txtComponentName.Text.Trim();
                string calbasis = ucCalculationBasis.SelectedHard;
                string paybasis = ucPayableBasis.SelectedHard;
                string includedonboard = rblIncludedOnboard.SelectedValue;
                string earningdeduction = rblEarningDeduction.SelectedValue;
                string activeyn = rblActiveyn.SelectedValue;
                string description = txtDescription.Text.Trim();
                string postingbudgetcode = ucPostingBudget.SelectedBudgetCode;
                string chargingbudgetcode = ucChargingbudget.SelectedBudgetCode;
                string Contract = ddlContact.SelectedValue;

                if (!IsValidCrewContract(shortcode, name, calbasis, paybasis, includedonboard, earningdeduction, activeyn, postingbudgetcode, Contract))
                {
                    ucError.Visible = true;
                    return;
                }

                if (ComponentId != string.Empty)
                {
                    PhoenixRegistersContract.UpdateContractCrew(new Guid(ComponentId), shortcode.Trim(), name.Trim(), null
                                                                    , int.Parse(postingbudgetcode), General.GetNullableInteger(chargingbudgetcode)
                                                                    , int.Parse(calbasis), int.Parse(paybasis), byte.Parse(includedonboard)
                                                                    , description.Trim(), int.Parse(earningdeduction), General.GetNullableInteger(activeyn)
                                                                    , General.GetNullableInteger(Contract)
                                                                    , General.GetNullableInteger(rblShowowneratsignoff.SelectedValue)
                                                                    , General.GetNullableGuid(ucGlobalWageComponent.SelectedComponent)
                                                                   , General.GetNullableInteger(rblIsCheckOfferLetter.SelectedValue));
                    
                }
                else
                {
                    PhoenixRegistersContract.InsertContractCrew(shortcode.Trim(), name.Trim(), null
                                                                , int.Parse(postingbudgetcode), General.GetNullableInteger(chargingbudgetcode)
                                                                , int.Parse(calbasis), int.Parse(paybasis), byte.Parse(includedonboard)
                                                                , description.Trim(), int.Parse(earningdeduction)
                                                                , General.GetNullableInteger(activeyn), General.GetNullableInteger(Contract)
                                                                 , General.GetNullableInteger(rblShowowneratsignoff.SelectedValue)
                                                                 , General.GetNullableGuid(ucGlobalWageComponent.SelectedComponent)
                                                                  , General.GetNullableInteger(rblIsCheckOfferLetter.SelectedValue));
                }
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCrewContract(string shortcode, string name, string calbasis, string paybasis, string includeonboard, string earningdeduction, string activeyn, string postingbudgetcode, string Contract)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(shortcode))
            ucError.ErrorMessage = "Code is required.";

        if (string.IsNullOrEmpty(name))
            ucError.ErrorMessage = "Component is required.";

        if (!General.GetNullableInteger(calbasis).HasValue)
            ucError.ErrorMessage = "Calculation Basis is required";

        if (!General.GetNullableInteger(paybasis).HasValue)
            ucError.ErrorMessage = "Payable Basis is required";

        if (!General.GetNullableInteger(includeonboard).HasValue)
            ucError.ErrorMessage = "Included Onboard is required";

        if (!General.GetNullableInteger(earningdeduction).HasValue)
            ucError.ErrorMessage = "Earning/Deduction is required";

        if (!General.GetNullableInteger(postingbudgetcode).HasValue)
            ucError.ErrorMessage = "Posting Budget Code is required.";

        if (!General.GetNullableInteger(activeyn).HasValue)
            ucError.ErrorMessage = "Active ? is required";
        if (!General.GetNullableInteger(Contract).HasValue)
            ucError.ErrorMessage = "Contract Setting is required";
        return (!ucError.IsError);
    }

    private void EditContract(Guid ComponentId)
    {
        try
        {
            DataTable dt = PhoenixRegistersContract.CrewWageComponentList(ComponentId);
            if (dt.Rows.Count > 0)
            {

                txtShortCode.Text = dt.Rows[0]["FLDSHORTCODE"].ToString();
                txtComponentName.Text = dt.Rows[0]["FLDCOMPONENTNAME"].ToString();
                rblIncludedOnboard.SelectedValue = dt.Rows[0]["FLDINCLUDEDONBOARDYN"].ToString();
                rblEarningDeduction.SelectedValue = dt.Rows[0]["FLDEARNINGDEDUCTION"].ToString();
                rblActiveyn.SelectedValue = dt.Rows[0]["FLDACTIVEYN"].ToString();
                ucCalculationBasis.SelectedHard = dt.Rows[0]["FLDCALCULATIONBASIS"].ToString();
                ucPayableBasis.SelectedHard = dt.Rows[0]["FLDPAYABLEBASIS"].ToString();
                ucChargingbudget.SelectedBudgetCode = dt.Rows[0]["FLDCHARGINGBUDGETID"].ToString();
                ucPostingBudget.SelectedBudgetCode = dt.Rows[0]["FLDBUDGETID"].ToString();
                txtDescription.Text = dt.Rows[0]["FLDDESCRIPTION"].ToString();
                ddlContact.SelectedValue = dt.Rows[0]["FLDSHOWTOOWNER"].ToString();
                rblShowowneratsignoff.SelectedValue = dt.Rows[0]["FLDSHOWTOOWNERSIGNOFF"].ToString();
                ucGlobalWageComponent.SelectedComponent = dt.Rows[0]["FLDWAGECOMPONENTID"].ToString();
                rblIsCheckOfferLetter.SelectedValue = dt.Rows[0]["FLDISCHECKOFFERLETTER"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
