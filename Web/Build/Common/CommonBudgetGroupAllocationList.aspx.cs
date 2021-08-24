using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CommonBudgetGroupAllocationList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["SelectedYear"] != null)
                {
                    ucVessel.SelectedVessel = Filter.CurrentBudgetAllocationVesselFilter == null ? "" : Filter.CurrentBudgetAllocationVesselFilter;
                    ucFinancialYear.SelectedQuick = Request.QueryString["SelectedYear"].ToString();
                    ucAccess.SelectedHard = "274";
                }
            }
            base.Render(writer);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        ucBudgetGroup.HardTypeCode = ((int)PhoenixHardTypeCode.BUDGETGROUP).ToString();
        ucAccess.HardTypeCode = ((int)PhoenixHardTypeCode.BUDGETALLOCATIONACCESS).ToString();

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if ((Request.QueryString["budgetgroupid"] != null) && (Request.QueryString["budgetgroupid"] != ""))
            {
                ucVessel.SelectedVessel = Filter.CurrentBudgetAllocationVesselFilter != null ? Filter.CurrentBudgetAllocationVesselFilter : "";
                ucFinancialYear.SelectedQuick = Request.QueryString["finyear"].ToString() == "Dummy" ? "" : Request.QueryString["finyear"].ToString();
                ucBudgetGroup.SelectedHard = Request.QueryString["budgetgroupid"].ToString();
                ViewState["BUDGETGROUPID"] = Request.QueryString["budgetgroupid"].ToString();

                ucVessel.Enabled = false;
                ucFinancialYear.Enabled = false;
                ucBudgetGroup.Enabled = false;
            }


            if (Request.QueryString["vesselaccountid"] != null)
                ViewState["VESSELACCOUNTID"] = Request.QueryString["vesselaccountid"].ToString();

            if (Request.QueryString["vesselbudgetallocationid"] != null)
                ViewState["VESSELBUDGETALLOCATIONID"] = Request.QueryString["vesselbudgetallocationid"].ToString();


            if ((Request.QueryString["BudgetGroupAllocationId"] != null) && (Request.QueryString["BudgetGroupAllocationId"] != ""))
            {
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                ViewState["BudgetGroupAllocationId"] = Request.QueryString["BudgetGroupAllocationId"].ToString();
                BudgetGroupAllocationEdit(Int32.Parse(Request.QueryString["BudgetGroupAllocationId"].ToString()));
            }
            else
            {                
               
                //toolbar.AddButton("New", "NEW");
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            MenuBudgetGroupAllocationList.AccessRights = this.ViewState;
            MenuBudgetGroupAllocationList.MenuList = toolbar.Show();
        }
    }

    protected void BudgetGroupAllocationList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            String scriptClosePopup = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");

            String scriptKeepPopupOpen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'yes');");

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidBudgetGroupAllocation())
                {
                    PhoenixCommonBudgetGroupAllocation.InsertBudgetGroupAllocation(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , Int16.Parse(ucBudgetGroup.SelectedHard)
                        , Int16.Parse(ucFinancialYear.SelectedQuick)
                                                //, Decimal.Parse(txtBudgetAmount.TextWithLiterals)
                        , Decimal.Parse(txtBudgetAmount.Text)
                        , General.GetNullableInteger(ucAccess.SelectedHard)
                        , Int16.Parse(ucVessel.SelectedVessel)
                        , Int16.Parse(ucApportionmentMethod.SelectedHard)
                        , int.Parse(ViewState["VESSELACCOUNTID"].ToString())
                        , General.GetNullableDecimal(ucAllowance.Text)
                        );

                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookmarkScript", scriptClosePopup, true);
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            if (CommandName.ToUpper().Equals("NEW"))
                Reset();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidBudgetGroupAllocation()
    {
        Int32 result;

        ucError.HeaderMessage = "Please provide the following required information";
        
        if (!Int32.TryParse(ucVessel.SelectedVessel, out result))
            ucError.ErrorMessage = "Vessel is required.";

        if (!Int32.TryParse(ucFinancialYear.SelectedQuick, out result))
            ucError.ErrorMessage = "Financial Year is required.";

        if (!Int32.TryParse(ucBudgetGroup.SelectedHard, out result))
            ucError.ErrorMessage = "Budget Group is required.";

        if (!Int32.TryParse(ucApportionmentMethod.SelectedHard, out result))
            ucError.ErrorMessage = "Apportionment Method is required.";

        if (txtBudgetAmount.Text.Equals(""))
            ucError.ErrorMessage = "Budget Amount is required.";

        return (!ucError.IsError);
    }

    private void Reset()
    {
        ViewState["BudgetGroupAllocationId"] = null;

        ucBudgetGroup.SelectedHard = "";
        txtBudgetAmount.Text = "";
        ucApportionmentMethod.SelectedHard = "";
    }

    private void BudgetGroupAllocationEdit(int budgetgroupallocationid)
    {
        DataSet ds = PhoenixCommonBudgetGroupAllocation.EditBudgetGroupAllocation(budgetgroupallocationid,General.GetNullableGuid(ViewState["VESSELBUDGETALLOCATIONID"].ToString()),General.GetNullableInteger(ViewState["BUDGETGROUPID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
            ucFinancialYear.SelectedQuick = dr["FLDFINANCIALYEAR"].ToString();
            ucBudgetGroup.SelectedHard = dr["FLDBUDGETGROUPID"].ToString();
            ucAccess.SelectedHard = dr["FLDACCESS"].ToString();
            txtBudgetAmount.Text = dr["FLDBUDGETAMOUNT"].ToString();
            ucApportionmentMethod.SelectedHard = dr["FLDAPPORTIONMENTMETHOD"].ToString();
            ucAllowance.Text = dr["FLDALLOWANCE"].ToString();
        }
    }
}
