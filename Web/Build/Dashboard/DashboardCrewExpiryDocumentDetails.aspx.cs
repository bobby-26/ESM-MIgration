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
public partial class Dashboard_DashboardCrewExpiryDocumentDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = null;
        ViewState["DUEDAYS"] = null;
        if(!IsPostBack)
        {
            if (Request.QueryString["vid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vid"].ToString();
            if (Request.QueryString["due"] != null)
                ViewState["DUEDAYS"] = Request.QueryString["due"].ToString();
        }
    }

    protected void gvdocumentlist_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataTable ds = PhoenixDashboardCrew.CrewExpiryDocumentList(Convert.ToInt32(ViewState["VESSELID"].ToString()), Convert.ToInt32(ViewState["DUEDAYS"].ToString()));

        gvdocumentlist.DataSource = ds;
    }

  
}