using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;


public partial class PurchaseCommittedBudget : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        Menu.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ucBudgetGroup.bind();
            BindVesselAccount();
        }
        EditBudgetAllocation();
    }
    protected void BindVesselAccount()
    {
        ddlAccountDetails.Visible = true;
        ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(null, 1);
        ddlAccountDetails.DataBind();
        ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

    }
    protected void EditBudgetAllocation()
    {
        DataSet ds = PhoenixPurchaseCommittedCosts.VesselBudgetAllocation(General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                                                                        , General.GetNullableInteger(ucFinancialYear.SelectedQuick)
                                                                        , General.GetNullableInteger(ucBudgetGroup.SelectedHard)
                                                                        );

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
            ucEffectiveDate.Text = General.GetDateTimeToString(dr["FLDEFFECTIVEDATE"].ToString());
            txtTotalAmount.Text = dr["FLDTOTALBUDGETAMOUNT"].ToString();
            txtBudgetGroupAmount.Text = dr["FLDTOTALBUDGETGROUPAMOUNT"].ToString();
        }
    }
    protected void gvBudget_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixPurchaseCommittedCosts.CommittedBudget(
                                        General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                                        , General.GetNullableInteger(ucFinancialYear.SelectedQuick)
                                        , General.GetNullableInteger(ucBudgetGroup.SelectedHard)
                                                                            );

            gvBudget.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudget_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;
            LinkButton lblCommittedAmount = (LinkButton)item.FindControl("lblCommittedAmount");
            LinkButton lblRevokedAmount = (LinkButton)item.FindControl("lblRevokedAmount");
            LinkButton lblChargedAmount = (LinkButton)item.FindControl("lblChargedAmount");

            string lblPeriod = drv["FLDPERIOD"].ToString();
            string lblVesselId = drv["FLDVESSELID"].ToString();
            string lblAccountId = drv["FLDACCOUNTID"].ToString();
            string lblYear = drv["FLDYEAR"].ToString();
            string lblBudgetGroupId = drv["FLDBUDGETGROUPID"].ToString();
            string lblPeriodName = drv["FLDMONTHNAME"].ToString();

            if (lblCommittedAmount != null)
            {
                lblCommittedAmount.Attributes.Add("onclick"
                    , "openNewWindow('codehelp1', '', 'Purchase/PurchaseCommittedCostBreakUp.aspx?vesselid=" + lblVesselId+ "&accountid=" + lblAccountId
                    + "&year=" + lblYear + "&month=" + lblPeriod
                    + "&budgetgroupid=" + lblBudgetGroupId + "');return false;");
            }

            if (lblRevokedAmount != null)
            {
                lblRevokedAmount.Attributes.Add("onclick"
                    , "openNewWindow('codehelp1', '', 'Purchase/PurchaseCommittedCostBreakUpRevoked.aspx?vesselid=" + lblVesselId+ "&accountid=" + lblAccountId
                    + "&year=" + lblYear + "&month=" + lblPeriod
                    + "&budgetgroupid=" + lblBudgetGroupId+ "');return false;");
            }

            if (lblChargedAmount != null)
            {
                lblChargedAmount.Attributes.Add("onclick"
                    , "openNewWindow('codehelp1', '', 'Purchase/PurchaseCommittedCostBreakUpCharged.aspx?vesselid=" + lblVesselId+ "&accountid=" + lblAccountId
                    + "&year=" + lblYear+ "&month=" + lblPeriod
                    + "&budgetgroupid=" + lblBudgetGroupId+ "');return false;");
            }

        }
    }

    protected void ucFinancialYear_TextChangedEvent(object sender, EventArgs e)
    {
        gvBudget.Rebind();
    }

    protected void ucBudgetGroup_TextChangedEvent(object sender, EventArgs e)
    {
        gvBudget.Rebind();
    }
}
