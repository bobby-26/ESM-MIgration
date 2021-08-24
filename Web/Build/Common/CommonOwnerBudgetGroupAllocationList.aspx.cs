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


public partial class CommonOwnerBudgetGroupAllocationList : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    try
    //    {
    //        if (!IsPostBack)
    //        {
    //            if (Request.QueryString["finyear"] != null)
    //            {
    //                ucVessel.SelectedVessel = Filter.CurrentBudgetAllocationVesselFilter == null ? "" : Filter.CurrentBudgetAllocationVesselFilter;
                   
    //                ucFinancialYear.SelectedQuick = Request.QueryString["finyear"].ToString();
    //                ucAccess.SelectedHard = "274";
    //            }
    //        }
    //        base.Render(writer);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //        return;
    //    }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        
        ucAccess.HardTypeCode = ((int)PhoenixHardTypeCode.BUDGETALLOCATIONACCESS).ToString();
        

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if ((Request.QueryString["budgetgroupid"] != null) && (Request.QueryString["budgetgroupid"] != ""))
            {
                ucVessel.SelectedVessel = Filter.CurrentBudgetAllocationVesselFilter != null ? Filter.CurrentBudgetAllocationVesselFilter : "";
                ViewState["Vessel"] = Filter.CurrentBudgetAllocationVesselFilter == null ? "" : Filter.CurrentBudgetAllocationVesselFilter;
                ucFinancialYear.SelectedQuick = Request.QueryString["finyear"].ToString() == "Dummy" ? "" : Request.QueryString["finyear"].ToString();
                txtBudgetGroup.Text = Request.QueryString["budgetgroupcode"].ToString();
                ViewState["budgetgroupid"] = Request.QueryString["budgetgroupid"];
                //ViewState["VESSEL"] = "";
                ucVessel.Enabled = false;
                ucFinancialYear.Enabled = false;
                txtBudgetGroup.Enabled = false;
               
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
                
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            }
            MenuBudgetGroupAllocationList.AccessRights = this.ViewState;
            MenuBudgetGroupAllocationList.MenuList = toolbar.Show();
            BudgetAmountDetails();
        }
    }
    public void BudgetAmountDetails()
    {
        try
        {
            decimal immediateparentamount = 0;
            decimal allocatedamount = 0;
            decimal availableamount = 0;
            
            DataSet ds = PhoenixCommonBudgetGroupAllocation.AvailableBudgetAmount(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , General.GetNullableGuid(ViewState["budgetgroupid"].ToString())
                            , General.GetNullableInteger(Request.QueryString["finyear"].ToString())
                            , General.GetNullableInteger(ViewState["Vessel"].ToString())
                            , General.GetNullableInteger(ViewState["VESSELACCOUNTID"].ToString())
                            , ref immediateparentamount
                            , ref allocatedamount
                            , ref availableamount);
            decimal t = immediateparentamount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
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

                    PhoenixCommonBudgetGroupAllocation.InsertOwnerBudgetGroupAllocation(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ViewState["budgetgroupid"].ToString())
                        , Int16.Parse(ucFinancialYear.SelectedQuick)
                                                //, Decimal.Parse(txtBudgetAmount.TextWithLiterals)
                        , Decimal.Parse(txtBudgetAmount.Text)
                        , General.GetNullableInteger(ucAccess.SelectedHard)
                        , Int16.Parse(ucVessel.SelectedVessel)
                        , Int16.Parse(ucApportionmentMethod.SelectedHard)
                        , int.Parse(ViewState["VESSELACCOUNTID"].ToString())
                        , General.GetNullableDecimal(ucAllowance.Text)
                        );

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookmarkScript", scriptClosePopup, true);
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

        //if (!Guid.TryParse(ViewState["budgetgroupid"].ToString(), out gresult))
        //    ucError.ErrorMessage = "Budget Group is required.";

        if (!Int32.TryParse(ucApportionmentMethod.SelectedHard, out result))
            ucError.ErrorMessage = "Apportionment Method is required.";

        if (txtBudgetAmount.Text.Equals(""))
            ucError.ErrorMessage = "Budget Amount is required.";

        return (!ucError.IsError);
    }

    private void Reset()
    {
        ViewState["BudgetGroupAllocationId"] = null;

        //ucBudgetGroup.SelectedHard = "";
        txtBudgetAmount.Text = "";
        ucApportionmentMethod.SelectedHard = "";
    }

    private void BudgetGroupAllocationEdit(int budgetgroupallocationid)
    {
        DataSet ds = PhoenixCommonBudgetGroupAllocation.EditOwnerBudgetGroupAllocation(budgetgroupallocationid,General.GetNullableGuid(ViewState["VESSELBUDGETALLOCATIONID"].ToString()),General.GetNullableGuid(ViewState["budgetgroupid"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
            ucFinancialYear.SelectedQuick = dr["FLDFINANCIALYEAR"].ToString();
            ViewState["VESSEL"] = dr["FLDVESSELID"].ToString();
            ucAccess.SelectedHard = dr["FLDACCESS"].ToString();
            txtBudgetAmount.Text = dr["FLDBUDGETAMOUNT"].ToString();
            ucApportionmentMethod.SelectedHard = dr["FLDAPPORTIONMENTMETHOD"].ToString();
            ucAllowance.Text = dr["FLDALLOWANCE"].ToString();
        }
    }
}
