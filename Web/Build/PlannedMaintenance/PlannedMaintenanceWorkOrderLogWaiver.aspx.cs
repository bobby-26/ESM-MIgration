using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderLogWaiver : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {

            ViewState["WorkorderId"] = null;
            if (Request.QueryString["WORKORDERID"] != null)
            {
                ViewState["WorkorderId"] = Request.QueryString["WORKORDERID"].ToString();
            }

        }
    }
   
    protected void gvWaiver_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt;
        if (Request.QueryString["vesselid"] != null)
            dt = PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderLogWaiverList(new Guid(ViewState["WorkorderId"].ToString()), null, int.Parse(Request.QueryString["vesselid"].ToString()));
        else
            dt = PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderLogWaiverList(new Guid(ViewState["WorkorderId"].ToString()), null);
        gvWaiver.DataSource = dt;
    }
}
