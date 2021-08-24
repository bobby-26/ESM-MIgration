using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;

public partial class UserControls_UserControlPMSManual : System.Web.UI.UserControl
{
    private Guid? _componentjobid;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public Guid? ComponentJobId
    {
        get
        {
            return _componentjobid;
        }
        set
        {
            _componentjobid = value;
            BindData();
        }
    }
    private void BindData()
    {
        DataTable dt = PhoenixPlannedMaintenanceComponentJobManual.List(PhoenixSecurityContext.CurrentSecurityContext.VesselID, ComponentJobId);
        lstPMSManual.DataSource = dt;
        lstPMSManual.DataBind();
    }
}