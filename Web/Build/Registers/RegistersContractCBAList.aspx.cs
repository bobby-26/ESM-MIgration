using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersContractCBAList : PhoenixBasePage
{
    string ComponentId = string.Empty, Unionid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ComponentId = Request.QueryString["compid"] == null ? string.Empty : Request.QueryString["compid"].ToString();
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuContract.AccessRights = this.ViewState;
            MenuContract.MenuList = toolbar.Show();
            Unionid = Unionid == string.Empty ? Request.QueryString["UnionId"].ToString() : Unionid;
            if (!IsPostBack)
            {
               
                cblVesselType.DataSource = PhoenixRegistersVesselType.ListVesselType(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                
                cblVesselType.DataBind();
                if (!string.IsNullOrEmpty(ComponentId))
                {
                    EditContract(new Guid(ComponentId));
                }
                if (Request.QueryString["UnionId"] != null || Unionid != string.Empty)
                {
                    DataTable dt = PhoenixRegistersContract.ListCBARevision(int.Parse(Unionid));
                    if (dt.Rows.Count > 0)
                    {
                        txtUnion.Text = dt.Rows[0]["FLDUNIONNAME"].ToString();
                    }
                }
            }
            txtShortCode.Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Contract_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string shortcode = txtShortCode.Text.Trim();
                string name = txtComponentName.Text.Trim();
                string payextorg = rblPayExtOrg.YesNoOption.ToLower() == "yes" ? "1" : "0";
                string deduction = rblIncContDed.YesNoOption.ToLower() == "yes" ? "1" : "0";
                string earnings = rblIncContEar.YesNoOption.ToLower() == "yes" ? "1" : "0";
                string amtdeductiondiff = rblAmtDiffDed.YesNoOption.ToLower() == "yes" ? "1" : "0";
                string company = ddlCompany.SelectedCompany;
                string supplier = ddlSupplier.SelectedAddress;
                string supplierpaybasis = ddlSupplierBasis.SelectedHard;
                string calunitbasis = ddlCalUnitBasis.SelectedHard;
                string caltimebasis = ddlCalTimeBasis.SelectedHard;
                string onboardpayded = ddlOnbPayDed.SelectedHard;
                string currency = ddlCurrency.SelectedCurrency;
                string strvesseltype = "";

                foreach (RadListBoxItem item in cblVesselType.Items)
                {
                    strvesseltype += item.Checked==true ? item.Value + "," : string.Empty;

                }
                strvesseltype = strvesseltype.TrimEnd(',');

                if (!IsValidContract(shortcode, name, payextorg, deduction, earnings, calunitbasis, caltimebasis, currency, earnings, deduction))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ComponentId != string.Empty)
                {
                    PhoenixRegistersContract.UpdateCBAContract(new Guid(ComponentId), shortcode, name, byte.Parse(payextorg)
                       , byte.Parse(deduction), byte.Parse(earnings), byte.Parse(amtdeductiondiff)
                       , General.GetNullableInteger(company), General.GetNullableInteger(supplier), General.GetNullableInteger(supplierpaybasis)
                       , int.Parse(calunitbasis), int.Parse(caltimebasis), General.GetNullableInteger(onboardpayded), strvesseltype, int.Parse(currency),
                       General.GetNullableInteger(ddlBudget.SelectedBudgetCode), General.GetNullableInteger(ucSortOrder.Text),
                       General.GetNullableInteger(ddlChargingBudget.SelectedBudgetCode),General.GetNullableGuid(ucGlobalWageComponent.SelectedComponent));
                }
                else
                {
                    PhoenixRegistersContract.InsertCBAContract(int.Parse(Unionid), shortcode, name, byte.Parse(payextorg)
                        , byte.Parse(deduction), byte.Parse(earnings), byte.Parse(amtdeductiondiff)
                        , General.GetNullableInteger(company), General.GetNullableInteger(supplier), General.GetNullableInteger(supplierpaybasis)
                        , int.Parse(calunitbasis), int.Parse(caltimebasis), General.GetNullableInteger(onboardpayded), strvesseltype, int.Parse(currency),null
                        , (byte?)General.GetNullableInteger(Request.QueryString["rev"] == null ? null : Request.QueryString["rev"].ToString()), General.GetNullableInteger(ddlBudget.SelectedBudgetCode)
                        , General.GetNullableInteger(ucSortOrder.Text), General.GetNullableInteger(ddlChargingBudget.SelectedBudgetCode), General.GetNullableGuid(ucGlobalWageComponent.SelectedComponent));
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "javascript:fnReloadList('codehelp1');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditContract(Guid ComponentId)
    {
        try
        {
            DataTable dt = PhoenixRegistersContract.EditCBAContract(ComponentId);
            if (dt.Rows.Count > 0)
            {
                Unionid = dt.Rows[0]["FLDADDRESSCODE"].ToString();
                txtShortCode.Text = dt.Rows[0]["FLDSHORTCODE"].ToString();
                txtComponentName.Text = dt.Rows[0]["FLDCOMPONENTNAME"].ToString();
                rblPayExtOrg.YesNoOption = dt.Rows[0]["FLDPAYABLEEXTORG"].ToString() == "1" ? "YES" : "NO";
                rblIncContEar.YesNoOption = dt.Rows[0]["FLDINCLUDECONTEAR"].ToString() == "1" ? "YES" : "NO";
                rblIncContDed.YesNoOption = dt.Rows[0]["FLDINCLUDECONTDED"].ToString() == "1" ? "YES" : "NO";
                rblAmtDiffDed.YesNoOption = dt.Rows[0]["FLDAMTDIFFDED"].ToString() == "1" ? "YES" : "NO";
                ddlCompany.SelectedCompany = dt.Rows[0]["FLDACCRUALCOMPANY"].ToString();
                ddlSupplier.SelectedAddress = dt.Rows[0]["FLDSUPPLIERPAY"].ToString();
                ddlSupplierBasis.SelectedHard = dt.Rows[0]["FLDSUPPLIERPAYBASIS"].ToString();
                ddlCalUnitBasis.SelectedHard = dt.Rows[0]["FLDCALUNITBASIS"].ToString();
                ddlCalTimeBasis.SelectedHard = dt.Rows[0]["FLDCALTIMEBASIS"].ToString();
                ddlOnbPayDed.SelectedHard = dt.Rows[0]["FLDONBARDPAYDED"].ToString();
                ddlBudget.SelectedBudgetCode = dt.Rows[0]["FLDBUDGETID"].ToString();

                ucSortOrder.Text = dt.Rows[0]["FLDSORTORDER"].ToString();
                ddlChargingBudget.SelectedBudgetCode = dt.Rows[0]["FLDCHARGINGBUDGETID"].ToString();
                ucGlobalWageComponent.SelectedComponent= dt.Rows[0]["FLDWAGECOMPONENTID"].ToString();
                string strlist = "," + dt.Rows[0]["FLDVESSELTYPE"].ToString() + ",";
                foreach (RadListBoxItem item in cblVesselType.Items)
                {

                    item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
                }
                 ddlCurrency.SelectedCurrency = dt.Rows[0]["FLDCURRENCYID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidContract(string shortcode, string name, string extorganization, string inclcontded, string inclcontear, string calunitbasis, string caltimebasis, string currency, string contractualearnings, string contractualdeductions)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        int resultInt;
        if (!int.TryParse(Unionid, out resultInt))
            ucError.ErrorMessage = "Union is required.";

        if (string.IsNullOrEmpty(shortcode))
            ucError.ErrorMessage = "Code is required.";

        if (string.IsNullOrEmpty(name))
            ucError.ErrorMessage = "Name is required.";

        if (string.IsNullOrEmpty(extorganization))
            ucError.ErrorMessage = "Payable to External Organizations is required.";

        if (string.IsNullOrEmpty(inclcontded))
            ucError.ErrorMessage = "Included in Contractual Deductions is required.";

        if (string.IsNullOrEmpty(inclcontear))
            ucError.ErrorMessage = "Included in Contractual Earnings is required.";

        if (!int.TryParse(calunitbasis, out resultInt))
            ucError.ErrorMessage = "Calculation Unit Basis is required.";

        if (!int.TryParse(caltimebasis, out resultInt))
            ucError.ErrorMessage = "Calculation Time Basis is required.";

        if (!int.TryParse(currency, out resultInt))
            ucError.ErrorMessage = "Currency is required.";

        if ((contractualearnings == "1") || contractualdeductions == "1")
        {
            if (ddlOnbPayDed.SelectedHard == "Dummy")
            {
                ucError.ErrorMessage = "Onboard Payable / Deduction is required.";
            }
        }

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
