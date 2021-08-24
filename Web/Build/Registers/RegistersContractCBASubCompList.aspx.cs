using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersContractCBASubCompList : PhoenixBasePage
{
    string SubComponentId = string.Empty;
    string MainComponentId = string.Empty, Unionid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuContract.AccessRights = this.ViewState;
            MenuContract.MenuList = toolbar.Show();
            Unionid = Unionid == string.Empty ? Request.QueryString["UnionId"].ToString() : Unionid;
            SubComponentId = Request.QueryString["compid"] == null ? string.Empty : Request.QueryString["compid"].ToString();
            MainComponentId = Request.QueryString["maincomponent"] == null ? string.Empty : Request.QueryString["maincomponent"].ToString();
            if (!IsPostBack)
            {
             
                if (!string.IsNullOrEmpty(SubComponentId))
                {
                    EditContract(new Guid(SubComponentId));
                }
                if (MainComponentId != string.Empty)
                {
                    DataTable dt = PhoenixRegistersContract.EditCBAContract(new Guid(MainComponentId));
                    if (dt.Rows.Count > 0)
                    {
                        txtMainCompName.Text = dt.Rows[0]["FLDCOMPONENTNAME"].ToString();
                        txtMainCompCode.Text = dt.Rows[0]["FLDSHORTCODE"].ToString();
                    }
                }

                if (Request.QueryString["UnionId"] != null || Unionid != string.Empty)
                {

                    DataTable dt = PhoenixRegistersContract.ListCBARevision(int.Parse(Unionid));
                    if (dt.Rows.Count > 0)
                        txtUnion.Text = dt.Rows[0]["FLDUNIONNAME"].ToString();
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
                string company = ddlCompany.SelectedCompany;
                string supplier = ddlSupplier.SelectedAddress;
                string supplierpaybasis = ddlSupplierBasis.SelectedHard;
                string calunitbasis = ddlCalUnitBasis.SelectedHard;
                string caltimebasis = ddlCalTimeBasis.SelectedHard;
                string currency = ddlCurrency.SelectedCurrency;

                if (!IsValidSubComponent(shortcode, name, company, supplier, supplierpaybasis, calunitbasis, caltimebasis, currency))
                {
                    ucError.Visible = true;
                    return;
                }

                if (SubComponentId != string.Empty)
                {
                    PhoenixRegistersContract.UpdateCBAContract(new Guid(SubComponentId), shortcode, name, null
                              , null, null, null
                              , General.GetNullableInteger(company), General.GetNullableInteger(supplier), General.GetNullableInteger(supplierpaybasis)
                              , int.Parse(calunitbasis), int.Parse(caltimebasis), null, string.Empty, int.Parse(currency), General.GetNullableInteger(ddlBudget.SelectedBudgetCode), General.GetNullableInteger(ucSortOrder.Text),
                       General.GetNullableInteger(ddlChargingBudget.SelectedBudgetCode));

                }
                else
                {
                    PhoenixRegistersContract.InsertCBAContract(int.Parse(Unionid), shortcode, name, null, null, null, null
                                                , int.Parse(company), int.Parse(supplier), int.Parse(supplierpaybasis), int.Parse(calunitbasis), int.Parse(caltimebasis)
                                                , null, string.Empty, int.Parse(currency), new Guid(MainComponentId)
                                                , (byte?)General.GetNullableInteger(Request.QueryString["rev"] == null ? null : Request.QueryString["rev"].ToString()), General.GetNullableInteger(ddlBudget.SelectedBudgetCode)
                        , General.GetNullableInteger(ucSortOrder.Text), General.GetNullableInteger(ddlChargingBudget.SelectedBudgetCode));
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "javascript:fnReloadList('codehelp2');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidSubComponent(string shortcode, string name, string company, string supplier, string supplierpay, string calunitbasis, string caltimebasis, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        int resultInt;
        if (!int.TryParse(Unionid, out resultInt))
            ucError.ErrorMessage = "Union is required.";
        else if (string.IsNullOrEmpty(MainComponentId))
            ucError.ErrorMessage = "Main Component is required";
        else
        {
            if (string.IsNullOrEmpty(shortcode))
                ucError.ErrorMessage = "Code is required.";

            if (string.IsNullOrEmpty(name))
                ucError.ErrorMessage = "Name is required.";

            if (!int.TryParse(company, out resultInt))
                ucError.ErrorMessage = "Company Accruing is required.";

            if (!int.TryParse(supplier, out resultInt))
                ucError.ErrorMessage = "Supplier Payable is required.";

            if (!int.TryParse(supplierpay, out resultInt))
                ucError.ErrorMessage = "Supplier Payable Basis is required.";

            if (!int.TryParse(calunitbasis, out resultInt))
                ucError.ErrorMessage = "Calculation Unit Basis is required.";

            if (!int.TryParse(caltimebasis, out resultInt))
                ucError.ErrorMessage = "Calculation Time Basis is required.";

            if (!int.TryParse(currency, out resultInt))
                ucError.ErrorMessage = "Currency is required.";
        }
        return (!ucError.IsError);
    }
    private void EditContract(Guid SubComponentId)
    {
        try
        {
            DataTable dt = PhoenixRegistersContract.EditCBAContract(SubComponentId);
            if (dt.Rows.Count > 0)
            {
                Unionid = dt.Rows[0]["FLDADDRESSCODE"].ToString();
                txtShortCode.Text = dt.Rows[0]["FLDSHORTCODE"].ToString();
                txtComponentName.Text = dt.Rows[0]["FLDCOMPONENTNAME"].ToString();
                ddlCompany.SelectedCompany = dt.Rows[0]["FLDACCRUALCOMPANY"].ToString();
                ddlSupplier.SelectedAddress = dt.Rows[0]["FLDSUPPLIERPAY"].ToString();
                ddlSupplierBasis.SelectedHard = dt.Rows[0]["FLDSUPPLIERPAYBASIS"].ToString();
                ddlCalUnitBasis.SelectedHard = dt.Rows[0]["FLDCALUNITBASIS"].ToString();
                ddlCalTimeBasis.SelectedHard = dt.Rows[0]["FLDCALTIMEBASIS"].ToString();
                ddlBudget.SelectedBudgetCode = dt.Rows[0]["FLDBUDGETID"].ToString();
                ucSortOrder.Text = dt.Rows[0]["FLDSORTORDER"].ToString();
                ddlChargingBudget.SelectedBudgetCode = dt.Rows[0]["FLDCHARGINGBUDGETID"].ToString();
                ddlCurrency.SelectedCurrency = dt.Rows[0]["FLDCURRENCYID"].ToString();
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
