using System;
using System.Data;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Framework;

public partial class PlannedMaintenanceManualsToolTip : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindMappedComponent();
    }
    private void BindMappedComponent()
    {
        DataTable dt = PhoenixPlannedMaintenanceManuals.ListPMSManualsComponentMapping(int.Parse(Request.QueryString["vslid"]), Request.QueryString["path"]);
        if (dt.Rows.Count > 0)
        {

            lstMappedComponent.DataSource = dt;
            lstMappedComponent.DataBind();
        }
        else
        {
            Response.Write("No Records Found");
        }
    }
}
