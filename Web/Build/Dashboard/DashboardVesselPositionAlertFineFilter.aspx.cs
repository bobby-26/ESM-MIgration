using System;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class DashboardVesselPositionAlertFineFilter : PhoenixBasePage
{
    public string BfValue = "";
    protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
            if (!string.IsNullOrEmpty(Request.QueryString["vesselname"]))
                ViewState["vesselname"] = Request.QueryString["vesselname"];
            txtVessel.Text = ViewState["vesselname"].ToString();
             if (!string.IsNullOrEmpty(Request.QueryString["vesselid"]))
                ViewState["vesselid"] = Request.QueryString["vesselid"];
            string ss = ViewState["vesselid"].ToString();
            fromDate.Text = DateTime.Parse(Request.QueryString["fromdate"].ToString()).ToString("dd/MM/yyyy");
            ToDateInput.Text = DateTime.Parse(Request.QueryString["todate"].ToString()).ToString("dd/MM/yyyy");
            ViewState["measurename"] = Request.QueryString["measurename"].ToString();
            FilterSet();
        }
        BfValue = b4.Checked == true ? "4" : "5";

    }
    protected void lstVslStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        FilterSet();
    }
    protected void fromDateInput_TextChanged(object sender, EventArgs e)
    {
        FilterSet();
    }
    protected void ToDateInput_TextChanged(object sender, EventArgs e)
    {
        FilterSet();
    }
    protected void lstVslCondition_SelectedIndexChanged(object sender, EventArgs e)
    {
        FilterSet();
    }
    protected void lstWeather_SelectedIndexChanged(object sender, EventArgs e)
    {
        FilterSet();
    }
    protected void b4_CheckedChanged(object sender, EventArgs e)
    {
        FilterSet();
    }
    protected void b5_CheckedChanged(object sender, EventArgs e)
    {
        FilterSet();
    }
    protected void FilterSet()
    {
        NameValueCollection nvc = new NameValueCollection();

        string badWeather;
        badWeather = b4.Checked == true ? "4" : "5";
        nvc.Add("vslStatus", lstVslStatus.SelectedValue);
        nvc.Add("vesselId", ViewState["vesselid"].ToString());
        nvc.Add("FromDate", fromDate.Text);
        nvc.Add("ToDate", ToDateInput.Text);
        nvc.Add("vslCondition", lstVslCondition.SelectedValue);
        nvc.Add("weather", lstWeather.SelectedValue);
        nvc.Add("badWeather", badWeather);
        nvc.Add("measurename", ViewState["measurename"].ToString());
        FilterDashboard.CurrentMachinaryPerformenceChart = nvc;
    }
}
