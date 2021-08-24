using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class Dashboard_DashboardV2Map : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
		ViewState["VESSELNAME"] = string.Empty;
		if (Request.QueryString["name"] != null)
		{
			ViewState["VESSELNAME"] = Request.QueryString["name"];
		}
		LoadMap();
	}
	private void LoadMap()
	{
		DataSet ds = PhoenixCommonDashboard.DashboardVesselSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, null, ViewState["VESSELNAME"].ToString());
		RadMap1.DataSource = ds;
		RadMap1.DataBind();
	}
	private static string TOOLTIP_TEMPLATE = @"
            <div class=""leftCol"">
                <div class=""vessel"">{0}</div>
                <div>Date: {1}</div>
                <div>Course: {3}</div>
                <div>Wind Direction / Force: {4} / {5}</div>
                <div>Speed: {6}</div>
                <div>ETA: {7}</div>
                <div class=""location"">Location: {2}</div>
            </div>
            ";
	protected void RadMap1_ItemDataBound(object sender, Telerik.Web.UI.Map.MapItemDataBoundEventArgs e)
	{
		MapMarker marker = e.Item as MapMarker;
		if (marker != null)
		{
			DataRowView item = e.DataItem as DataRowView;
			string vessel = item.Row["FLDVESSELNAME"] as string;
			string imo = item.Row["FLDIMONUMBER"] as string;
			string lat = item.Row["FLDLATITUDE"] as string;
			string log = item.Row["FLDLONGITUDE"] as string;
			string date = item.Row["FLDNOONREPORTDATE"].ToString();
			string course = item.Row["FLDCOURSE"].ToString();
			string windforce = item.Row["FLDWINDFORCE"].ToString();
			string winddirection = item.Row["FLDWINDDIRECTION"] as string;
			string eta = item.Row["FLDETA"].ToString();
			string logspeed = item.Row["FLDLOGSPEED"].ToString();
			marker.TooltipSettings.Content = String.Format(TOOLTIP_TEMPLATE, vessel + " (" + imo + ")", General.GetDateTimeToString(date), lat + " , " + log, course, winddirection, windforce, logspeed, General.GetDateTimeToString(eta));
		}
	}
}