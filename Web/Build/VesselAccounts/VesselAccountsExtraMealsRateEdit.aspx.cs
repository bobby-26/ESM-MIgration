using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Generic;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class VesselAccountsExtraMealsRateEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);

        MenuDefaultRate.AccessRights = this.ViewState;
        MenuDefaultRate.MenuList = toolbar.Show();


    }
    protected void Rebind()
    {
        gvExtraMealsDefaultRate.SelectedIndexes.Clear();
        gvExtraMealsDefaultRate.EditIndexes.Clear();
        gvExtraMealsDefaultRate.DataSource = null;
        gvExtraMealsDefaultRate.Rebind();
    }
    private void BindData()
    {
        DataTable dt = PhoenixVesselAccountsExtraMeals.SearchExtraMealsDefaultRate(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        gvExtraMealsDefaultRate.DataSource = dt;
        gvExtraMealsDefaultRate.VirtualItemCount = dt.Rows.Count;
    }
    protected void gvExtraMealsDefaultRate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void MenuDefaultRate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string actualvictualingrate;
                actualvictualingrate = chkVictualRate.Checked == true ? "1" : "0";
                UpdateExtraMealsDefaultRate(ddlAccountType.SelectedValue.ToString(), txtDefaultRate.Text, actualvictualingrate);
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void UpdateExtraMealsDefaultRate(string accunttype, string defaultrate, string victualtingrate)
    {
        if (!IsValidDefaultExtraMeals(accunttype, defaultrate, victualtingrate))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixVesselAccountsExtraMeals.UpdateExtraMealsDefaultRate(PhoenixSecurityContext.CurrentSecurityContext.VesselID
            , Int16.Parse(accunttype), General.GetNullableDecimal((defaultrate)), Int32.Parse(victualtingrate));
    }
    protected void chkVictualRate_OnCheckedChanged(object sender, EventArgs args)
    {
        if (chkVictualRate.Checked == true)
        {
            txtDefaultRate.CssClass = "readonlytextbox";
            txtDefaultRate.ReadOnly = true;
            txtDefaultRate.Text = "";
        }
        else
        {
            txtDefaultRate.CssClass = "input";
            txtDefaultRate.ReadOnly = false;
        }

    }
    private bool IsValidDefaultExtraMeals(string accounttype, string defaultrate, string victualtingrate)
    {
        Int16 resultInt;
        Decimal resultDecimal;
        ucError.HeaderMessage = "Please provide the following required information";

        if (Int16.TryParse(accounttype, out resultInt))
        {
            if (Int32.Parse(accounttype) == 0)
                ucError.ErrorMessage = "Account type  is required.";
        }
        else
        {
            ucError.ErrorMessage = "Account type  is required.";
        }
        if (!decimal.TryParse(defaultrate, out resultDecimal) && txtDefaultRate.CssClass == "readonlytextbox")
            ucError.ErrorMessage = "valid date is required.";
        if (General.GetNullableDecimal(defaultrate) != null && victualtingrate != "0")
        {
            if (Decimal.TryParse(defaultrate, out resultDecimal))
            {
                if ((resultDecimal < 0) || (resultDecimal > Decimal.Parse("99.99")))
                {
                    ucError.ErrorMessage = "Rate should be in between 0.00 to 99.99.";
                }
            }

        }

        return (!ucError.IsError);
    }
   
    protected void ddlAccountType_OnSelectedIndexChanged(object sender, EventArgs args)
    {

    }
}
