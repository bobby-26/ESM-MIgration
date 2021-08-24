using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;

public partial class DashboardCommonVesselAccountingSummary : PhoenixBasePage
{  
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            SelectedOption();
            ViewState["MODULENAME"] = "VESSELACCOUNTING";
        }
    }

    private void SelectedOption()
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();

        criteria.Add("Url", HttpContext.Current.Request.Url.AbsolutePath);
        criteria.Add("APP", "ACCT");
        criteria.Add("Option", "VAC");

        Filter.CurrentDashboardLastSelection = criteria;
        PhoenixDashboardOption.DashboardLastSelected(criteria);
      
    }
    
    protected void gvAccountingSummary_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = PhoenixCommonDashboard.DashboardSummaryAcrossModuleList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["MODULENAME"].ToString());
            DataTable dt = ds.Tables[0];
            gvAccountingSummary.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    
}
