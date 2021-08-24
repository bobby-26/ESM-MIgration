using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class Inspection_InspectionShippingKPIPIMappingAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        Tabstripspiaddmenu.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            DataTable dt = PheonixDashboardSKKPI.KPIList();

            RadComboKpilist.DataSource = dt;

            RadComboKpilist.DataBind();

            DataTable dt1 = PheonixDashboardSKPI.PIList();
            Radcombopilist.DataSource = dt1;

            Radcombopilist.DataBind();

        }
    }
    protected void piaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? kpiid = General.GetNullableGuid(RadComboKpilist.Value);
                Guid? piid = General.GetNullableGuid(Radcombopilist.Value);

                if (!IsValidShippingKPIPIDetails(kpiid, piid))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKKPI.KPIPIInsert(rowusercode, kpiid, piid);


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

    private bool IsValidShippingKPIPIDetails(Guid? kpi, Guid? pi)
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (kpi == null)
        {
            ucError.ErrorMessage = "KPI.";
        }
        if (pi == null)
        {
            ucError.ErrorMessage = "PI .";
        }



        return (!ucError.IsError);
    }

}