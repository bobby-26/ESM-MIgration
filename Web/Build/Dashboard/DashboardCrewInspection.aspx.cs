using System;
using System.Collections.Specialized;
using System.Web;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;

public partial class DashboardCrewInspection : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
                SelectedOption();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SelectedOption()
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();

        criteria.Add("Url", HttpContext.Current.Request.Url.AbsolutePath);
        if (Request.QueryString["APP"] != null)
            criteria.Add("App", Request.QueryString["APP"].ToString());
        if (Request.QueryString["OPT"] != null)
            criteria.Add("Option", Request.QueryString["OPT"].ToString());

        PhoenixDashboardOption.DashboardLastSelected(criteria);
    }
}
