using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;

public partial class Inspection_ShippingSPIKPIMappingAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        Tabstripspiaddmenu.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            DataTable dt = PheonixDashboardSKSPI.SPIList();

            Radcombospilist.DataSource = dt;
            Radcombospilist.DataBind();

            DataTable dt1 = PheonixDashboardSKKPI.KPIList();
            radcobkpi.DataSource = dt1;
            radcobkpi.DataBind();

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
                Guid? spiid = General.GetNullableGuid(Radcombospilist.Value);
                Guid? kpiid = General.GetNullableGuid(radcobkpi.Value);

                if (!IsValidShippingSPIKPIDetails(spiid, kpiid))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKKPI.SPIKPIInsert(rowusercode, spiid, kpiid);


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

    private bool IsValidShippingSPIKPIDetails(Guid? spi, Guid? kpi)
    {
        ucError.HeaderMessage = "Provide the following required information";
        if (spi == null)
        {
            ucError.ErrorMessage = "SPI.";
        }
        if (kpi == null)
        {
            ucError.ErrorMessage = "KPI .";
        }
        


        return (!ucError.IsError);
    }


}