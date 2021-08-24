using System;
using SouthNests.Phoenix.Framework;

public partial class ReportsView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
        {
            Response.Redirect("../SSRSReports/SsrsReportsView.aspx?" + Request.QueryString.ToString()); 
        }
        else
        {
            Response.Redirect("../Reports/CrystalReportsView.aspx?" + Request.QueryString.ToString());
        }
    }
}