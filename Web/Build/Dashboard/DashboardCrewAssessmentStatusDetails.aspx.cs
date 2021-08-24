using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;

public partial class Dashboard_DashboardCrewAssessmentStatusDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        ViewState["INTSTATUS"] = null;
        if (!IsPostBack)
        {
            if (Request.QueryString["intstatus"] != null)
                ViewState["INTSTATUS"] = Request.QueryString["intstatus"].ToString();
          
        }
    }

    protected void gvdocumentlist_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataTable ds = PhoenixDashboardCrew.CrewAssessmentStatusList(General.GetNullableString(ViewState["INTSTATUS"].ToString()));

        gvdocumentlist.DataSource = ds;
    }
}