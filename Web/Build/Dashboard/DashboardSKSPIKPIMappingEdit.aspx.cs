using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;

public partial class Inspection_ShippingSPIKPIMappingEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);

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

            ViewState["SPI2KPILINKID"] = General.GetNullableGuid(Request.QueryString["shippingspikpilinkid"]);
            Guid? shippingspikpilinkid = General.GetNullableGuid(ViewState["SPI2KPILINKID"].ToString());

            DataTable dt2 = PheonixDashboardSKKPI.SPIKPIEditList(shippingspikpilinkid);
            if (dt2.Rows.Count > 0)
            {

                Radcombospilist.Value = dt2.Rows[0]["FLDSPIID"].ToString();
                radcobkpi.Value = dt2.Rows[0]["FLDKPIID"].ToString();

            }
        }
    }
        protected void piaddmenu_TabStripCommand(object sender, EventArgs e)
        {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? spiid = General.GetNullableGuid(Radcombospilist.Value);
                Guid? kpiid = General.GetNullableGuid(radcobkpi.Value);
                Guid? shippingspikpilinkid = General.GetNullableGuid(ViewState["SPI2KPILINKID"].ToString());
                if (!IsValidShippingSPIKPIDetails(spiid, kpiid))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKKPI.SPIKPIUpdate(rowusercode, spiid, kpiid, shippingspikpilinkid);


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


