using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersContractESM : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersContractESM.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersCity.AccessRights = this.ViewState;
            MenuRegistersCity.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["Rank"] = string.Empty;
                ViewState["COMPONENTID"] = string.Empty;
                ViewState["RATINGS"] = string.Empty;
                ViewState["OFFICERS"] = string.Empty;
                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBERRANKWISE"] = 1;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindDataSub();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RegistersCity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDHARDNAME", "FLDSHORTCODE", "FLDCOMPONENTNAME","FLDWAGECOMPONENTNAME", "FLDCALCULATIONBASISNAME", "FLDPAYABLEBASISNAME", "FLDBUDGETCODE", "FLDCHARGINGBUDGETCODE", "FLDSHOWTOOWNER" };
                string[] alCaptions = { "Wage Component", "Code", "Component","Global Wage Component" ,"Calculation Basis", "Payable Basis", "Posting Budget Code", "Charging Budget Code", "Show to Owner" };
                DataTable dt = PhoenixRegistersContract.ESMContractSearch(General.GetNullableInteger(ucWageComponents.SelectedHard), 1, 1000, ref iRowCount, ref iTotalPageCount);
                General.ShowExcel("Standard Wage Components", dt, alColumns, alCaptions, null, string.Empty);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCrew.SelectedIndexes.Clear();
        gvCrew.EditIndexes.Clear();
        gvCrew.DataSource = null;
        gvCrew.Rebind();
    }
    protected void RebindSub()
    {
        gvCR.SelectedIndexes.Clear();
        gvCR.EditIndexes.Clear();
        gvCR.DataSource = null;
        gvCR.Rebind();
    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string shortcode = ((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text;
                string compname = ((RadTextBox)e.Item.FindControl("txtComponentNameAdd")).Text;
                string calbasis = ((UserControlHard)e.Item.FindControl("ddlCalBasisAdd")).SelectedHard;
                string paybasis = ((UserControlHard)e.Item.FindControl("ddlPayBasisAdd")).SelectedHard;
                string budgetcode = ((UserControlBudgetCode)e.Item.FindControl("ddlBudgetAdd")).SelectedBudgetCode;
                string Cbudgetcode = ((UserControlBudgetCode)e.Item.FindControl("ddlChargingBudgetAdd")).SelectedBudgetCode;
                string GWComponent = ((UserControlGlobalWageComponent)e.Item.FindControl("ucGlobalWageComponentAdd")).SelectedComponent;
                string isofferlettercheck = ((RadRadioButtonList)e.Item.FindControl("rblIsCheckOfferLetterAdd")).SelectedValue;
                RadComboBox ddlcontract = (RadComboBox)e.Item.FindControl("ddlContactAdd");
                if (!IsValidCrewContract(shortcode, compname, ucWageComponents.SelectedHard, calbasis, paybasis, ddlcontract.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContract.InsertESMContract(shortcode.Trim(), compname.Trim(), int.Parse(ucWageComponents.SelectedHard), int.Parse(calbasis)
                , int.Parse(paybasis), General.GetNullableInteger(budgetcode), General.GetNullableInteger(Cbudgetcode)
                , General.GetNullableInteger(ddlcontract.SelectedValue),General.GetNullableGuid(GWComponent),General.GetNullableInteger(isofferlettercheck));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string componentid = ((RadLabel)e.Item.FindControl("lblComponentIdEdit")).Text;
                string shortcode = ((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text;
                string compname = ((RadTextBox)e.Item.FindControl("txtComponentNameEdit")).Text;
                string calbasis = ((UserControlHard)e.Item.FindControl("ddlCalBasisEdit")).SelectedHard;
                string paybasis = ((UserControlHard)e.Item.FindControl("ddlPayBasisEdit")).SelectedHard;
                string budgetcode = ((UserControlBudgetCode)e.Item.FindControl("ddlBudgetEdit")).SelectedBudgetCode;
                string cbudgetcode = ((UserControlBudgetCode)e.Item.FindControl("ddlChargingBudgetEdit")).SelectedBudgetCode;
                string GWComponent = ((UserControlGlobalWageComponent)e.Item.FindControl("ucGlobalWageComponentEdit")).SelectedComponent;
                string isofferlettercheck = ((RadRadioButtonList)e.Item.FindControl("rblIsCheckOfferLetterEdit")).SelectedValue;
                // CheckBox ShowtoOwner = ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkShowtoOwnerEdit"));
                RadComboBox ddlcontract = (RadComboBox)e.Item.FindControl("ddlContactEdit");

                if (!IsValidCrewContract(shortcode, compname, ucWageComponents.SelectedHard, calbasis, paybasis, ddlcontract.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContract.UpdateESMContract(new Guid(componentid), shortcode.Trim(), compname.Trim(), int.Parse(calbasis), int.Parse(paybasis)
                    , General.GetNullableInteger(budgetcode), General.GetNullableInteger(cbudgetcode), General.GetNullableInteger(ddlcontract.SelectedValue),General.GetNullableGuid(GWComponent)
                    , General.GetNullableInteger(isofferlettercheck));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string componentid = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
                PhoenixRegistersContract.DeleteESMContract(new Guid(componentid));
                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCR_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindDataSub();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDHARDNAME", "FLDSHORTCODE", "FLDCOMPONENTNAME", "FLDWAGECOMPONENTNAME", "FLDCALCULATIONBASISNAME", "FLDPAYABLEBASISNAME", "FLDBUDGETCODE", "FLDCHARGINGBUDGETCODE", "FLDSHOWTOOWNER" };
        string[] alCaptions = { "Wage Component", "Code", "Component", "Global Wage Component", "Calculation Basis", "Payable Basis", "Posting Budget Code", "Charging Budget Code", "Show to Owner" };
        DataTable dt = PhoenixRegistersContract.ESMContractSearch(General.GetNullableInteger(ucWageComponents.SelectedHard)
            , 1, 1000, ref iRowCount, ref iTotalPageCount);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvCrew", "Standard Wage Components", alCaptions, alColumns, ds);
        gvCrew.DataSource = dt;
        gvCrew.VirtualItemCount = dt.Rows.Count;
    }
    private void BindDataSub()
    {
        int iRowCountRank = 0;
        int iTotalPageCountRank = 0;
        Guid? g = null;
        if (ViewState["COMPONENTID"].ToString() != string.Empty)
            g = new Guid(ViewState["COMPONENTID"].ToString());
        DataTable dt1 = PhoenixRegistersContract.ESMSubContractRankWiseSearch(General.GetNullableInteger(ucWageComponents.SelectedHard)
            , General.GetNullableInteger(ucRank.SelectedRank), 1, 1000, ref iRowCountRank, ref iTotalPageCountRank);
        gvCR.DataSource = dt1;
        gvCR.VirtualItemCount = dt1.Rows.Count;
    }
    protected void ucWageComponents_Changed(object sender, EventArgs e)
    {
        ViewState["COMPONENTID"] = string.Empty;
        ucRank.SelectedRank = "0";
        Rebind();
        RebindSub();
    }
    protected void gvCrew_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadRadioButtonList IsCheckOfferLetterEdit = (RadRadioButtonList)e.Item.FindControl("rblIsCheckOfferLetterEdit");
            if (IsCheckOfferLetterEdit != null)
                IsCheckOfferLetterEdit.SelectedValue = drv["FLDISCHECKOFFERLETTER"].ToString();
            UserControlHard calbasis = e.Item.FindControl("ddlCalBasisEdit") as UserControlHard;
            if (calbasis != null) calbasis.SelectedHard = drv["FLDCALCULATIONBASIS"].ToString();
            UserControlHard paybasis = e.Item.FindControl("ddlPayBasisEdit") as UserControlHard;
            if (paybasis != null) paybasis.SelectedHard = drv["FLDPAYABLEBASIS"].ToString();
            UserControlBudgetCode bud = (UserControlBudgetCode)e.Item.FindControl("ddlBudgetEdit");
            if (bud != null) bud.SelectedBudgetCode = drv["FLDBUDGETID"].ToString();
            UserControlBudgetCode cbud = (UserControlBudgetCode)e.Item.FindControl("ddlChargingBudgetEdit");
            if (cbud != null) cbud.SelectedBudgetCode = drv["FLDCHARGINGBUDGETID"].ToString();
            UserControlGlobalWageComponent Gwc = (UserControlGlobalWageComponent)e.Item.FindControl("ucGlobalWageComponentEdit");
            if (Gwc != null) Gwc.SelectedComponent = drv["FLDWAGECOMPONENTID"].ToString();
            RadComboBox ddlcontract = (RadComboBox)e.Item.FindControl("ddlContactEdit");
            if (ddlcontract != null) ddlcontract.SelectedValue = drv["FLDSHOWTOOWNER"].ToString();
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    protected void gvCR_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                if (drv["FLDSUBCOMPONENTID"].ToString() == string.Empty)
                    db.Visible = false;
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            UserControlCurrency currency = e.Item.FindControl("ddlCurrencyEdit") as UserControlCurrency;
            if (currency != null) currency.SelectedCurrency = drv["FLDCURRENCYID"].ToString();
            UserControlHard cal = e.Item.FindControl("ddlCalculationEdit") as UserControlHard;
            if (cal != null) cal.SelectedHard = drv["FLDCALCULATION"].ToString();
            UserControlContractCBA contract = e.Item.FindControl("ddlContractEdit") as UserControlContractCBA;
            if (contract != null)
            {
                string rank = ((Label)e.Item.FindControl("lblRank")).Text;
                contract.ContractList = PhoenixRegistersContract.ListCBAContract(General.GetNullableInteger(ViewState[IsOfficer(int.Parse(rank)) ? "OFFICERS" : "RATINGS"].ToString()), null, null);
                contract.SelectedContract = drv["FLDMAINCOMPONENTID"].ToString();
            }

            LinkButton History = (LinkButton)e.Item.FindControl("cmdHistory");
            if (History != null)
            {
                if (drv["FLDSUBCOMPONENTID"].ToString() == string.Empty)
                    History.Visible = false;
                History.Attributes.Add("onclick", "openNewWindow('filter', '', '" + Session["sitepath"] + "/Registers/RegistersContractESMHistory.aspx?&subComponentId=" + drv["FLDSUBCOMPONENTID"].ToString() + "');return false;");
            }
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    protected void gvCR_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                Guid? g = null;
                string componentid = ((RadLabel)e.Item.FindControl("lblComponentIdRankEdit")).Text;
                string subcomponentid = ((RadLabel)e.Item.FindControl("lblSubComponentIdEdit")).Text;
                string rank = ucRank.SelectedRank;
                string currency = ((UserControlCurrency)e.Item.FindControl("ddlCurrencyEdit")).SelectedCurrency;
                string amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text;

                string calc = string.Empty;//((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlCalculationEdit")).SelectedHard;
                string cont = string.Empty;//((UserControlContractCBA)_gridView.Rows[nCurrentRow].FindControl("ddlContractEdit")).SelectedContract;
                if (cont != string.Empty)
                    g = new Guid(cont);
                if (!IsValidESMSub(rank, currency, amount, calc, cont))
                {
                    ucError.Visible = true;
                    return;
                }
                if (!subcomponentid.ToString().Equals(""))
                    PhoenixRegistersContract.UpdateESMSubContract(new Guid(subcomponentid), int.Parse(rank), int.Parse(currency), decimal.Parse(amount)
                        , General.GetNullableInteger(calc), g);
                else
                    PhoenixRegistersContract.InsertESMSubContract(new Guid(componentid), int.Parse(rank), int.Parse(currency), decimal.Parse(amount)
                        , General.GetNullableInteger(calc), g);
                RebindSub();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string subcomponentid = ((RadLabel)e.Item.FindControl("lblSubComponentId")).Text;
                PhoenixRegistersContract.DeleteESMSubContract(new Guid(subcomponentid));
                RebindSub();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidCrewContract(string shortcode, string name, string vesselid, string calbasis, string paybasis, string contract)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(shortcode))
            ucError.ErrorMessage = "Code is required.";
        if (string.IsNullOrEmpty(name))
            ucError.ErrorMessage = "Component is required.";
        if (!General.GetNullableInteger(vesselid).HasValue)
            ucError.ErrorMessage = "Wage Components is required.";
        if (!General.GetNullableInteger(calbasis).HasValue)
            ucError.ErrorMessage = "Calculation Basis is required";
        if (!General.GetNullableInteger(paybasis).HasValue)
            ucError.ErrorMessage = "Payable Basis is required";
        if (!General.GetNullableInteger(contract).HasValue)
            ucError.ErrorMessage = "Contract Setting is required";
        return (!ucError.IsError);
    }
    private bool IsValidESMSub(string rank, string currency, string amount, string calculation, string componentid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        decimal resutlDec;

        if (!General.GetNullableInteger(rank).HasValue)
            ucError.ErrorMessage = "Rank is required.";

        if (!General.GetNullableInteger(currency).HasValue)
            ucError.ErrorMessage = "Currency is required.";

        if (!decimal.TryParse(amount, out resutlDec))
            ucError.ErrorMessage = "Amount is required.";
        else if (decimal.Parse(amount) < 0)
            ucError.ErrorMessage = "Amount is positive integer field";

        if (string.IsNullOrEmpty(componentid) && General.GetNullableInteger(calculation).HasValue)
            ucError.ErrorMessage = "CBA Contract is required.";

        if (!string.IsNullOrEmpty(componentid) && !General.GetNullableInteger(calculation).HasValue)
            ucError.ErrorMessage = "Calculation for CBA Contract is required.";

        return (!ucError.IsError);
    }
    protected bool IsOfficer(int RankId)
    {
        bool result = false;
        DataSet ds = PhoenixRegistersRank.EditRank(RankId);
        if (ds.Tables[0].Rows[0]["FLDOFFICECREW"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 50, "OFF"))
            result = true;
        return result;
    }
    protected void ddlRankAdd_TextChangedEvent(object sender, EventArgs e)
    {
        UserControlRank rank = sender as UserControlRank;
        ViewState["Rank"] = rank.SelectedRank;
        RebindSub();
    }
}
