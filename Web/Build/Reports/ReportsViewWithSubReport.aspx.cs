using System;
using SouthNests.Phoenix.Framework;

public partial class ReportsViewWithSubReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (PhoenixGeneralSettings.CurrentGeneralSetting == null || PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
        {
            Response.Redirect("../SSRSReports/SsrsReportsViewWithSubReport.aspx?" + Request.QueryString.ToString().Replace("&&", "&"));
            
        }
        else
        {
            Response.Redirect("../Reports/CrystalReportsViewWithSubReport.aspx?" + Request.QueryString.ToString().Replace("&&", "&"));
        }

    }
}