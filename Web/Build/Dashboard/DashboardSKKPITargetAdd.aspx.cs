using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;

public partial class Dashboard_DashboardSKKPITargetAdd : PhoenixBasePage
{
        

    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        Tabkpi.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            
            radcbyear.SelectedYear = DateTime.Now.Year;

            DataTable dt = PheonixDashboardSKKPI.KPIList();
            radcobkpi.DataSource = dt;
            radcobkpi.DataBind();

            DataTable dt1 = PheonixDashboardSKKPI.ObjectiveOwnerList();
            Radcombodesignationlist.DataSource = dt1;
            Radcombodesignationlist.DataBind();

        }

    }


    protected void KPI_TabStripMenuCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());
                Guid? kpiid = General.GetNullableGuid(radcobkpi.Value);
                Decimal? KPIminimum = General.GetNullableDecimal(radtbkpiminimumentry.Text);
                Decimal? KPItarget = General.GetNullableDecimal(radtbkpitargetvalueentry.Text);
                int? objectiveowner = General.GetNullableInteger(Radcombodesignationlist.Value);
                string referencenumber = General.GetNullableString(radtbkpirefno.Text);
                if (!IsValidShippingKPITargetDetails(year, kpiid,  KPIminimum, KPItarget))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixDashboardSKKPITarget.KPITargetInsert(rowusercode,kpiid, KPIminimum, KPItarget,year,objectiveowner,referencenumber);


                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }

        }



        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    private bool IsValidShippingKPITargetDetails(int?year, Guid? kpiid, Decimal? KPIminimum, Decimal? KPItarget)
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (year == null)
        {
            ucError.ErrorMessage = "Year.";
        }
        if (kpiid == null)
        {
            ucError.ErrorMessage = "KPI Name.";
        }
        if (KPIminimum == null)
        {
            ucError.ErrorMessage = "KPI Minimum Value.";
        }
        if (KPItarget == null)
        {
            ucError.ErrorMessage = "KPI Target Value.";
        }

       

        return (!ucError.IsError);
    }
}