using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Collections.Generic;
using SouthNests.Phoenix.Dashboard;

public partial class DashboardCommonSyncStatus : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
       
    }
  
    protected void gvVesselSynchronizationStatus_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ds = DataAccess.ExecSPReturnDataSet("PRALERTDATASYNCHRONIZERVESSELS", ParameterList);

            gvVesselSynchronizationStatus.DataSource = ds;
         
            DataSet dsedit = PhoenixCommonDashboard.DashboardVesselAdminEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            if (dsedit.Tables[0].Rows.Count > 0)
                ViewState["FLDDTKEY"] = dsedit.Tables[0].Rows[0]["FLDDTKEY"].ToString();
            else
                ViewState["FLDDTKEY"] = "";
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

