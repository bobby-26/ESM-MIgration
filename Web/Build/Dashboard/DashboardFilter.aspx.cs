using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;

public partial class DashboardFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["filter"] != null)
                ShowScreen(Request.QueryString["filter"].ToString());
        }
    }

    private void ShowScreen(string FilterType)
    {
        if (FilterType == "1")
            Response.Redirect("DashboardVesselFilter.aspx");
        else if (FilterType == "2")
            Response.Redirect("DashboardRankFilter.aspx");
    }
}



