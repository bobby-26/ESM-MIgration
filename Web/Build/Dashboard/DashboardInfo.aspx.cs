using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;

public partial class Dashboard_DashboardInfo : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        
        if (!IsPostBack)
        {
            ViewState["ID"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                ViewState["ID"] = Request.QueryString["id"];
            }
            Edit();
        }
    }
    private void Edit()
    {
        string id = ViewState["ID"].ToString();
        DataTable dt = PhoenixDashboardOption.EditDashboardInfoEPSS(new Guid(id));
        if (dt.Rows.Count > 0)
        {
            lblInfo.Text = dt.Rows[0]["FLDINFORMATION"].ToString();     
        }        
    }   
}