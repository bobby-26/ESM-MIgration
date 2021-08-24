using System;
using System.Data;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;

public partial class DashboardTechnicalAuxiliaryPerformanceSwitchboard : PhoenixBasePage
{
	public string dateseries = "";
	public string SeriesData = "";
	protected void Page_Load(object sender, EventArgs e)
    {
		if (!IsPostBack)
		{
			if (Request.QueryString["vesselid"] != null)
				ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
			else
				ViewState["vesselid"] = "0";

			if (Request.QueryString["AeNo"] != null)
				ViewState["AeNo"] = Request.QueryString["AeNo"].ToString();
			else
				ViewState["AeNo"] = "1";
		}

		AeSwitchBoard();
	}

protected static string ChartDateRange(int vesselid, int AeNo)
	{
		string daterange = "";
		DataTable dt = new DataTable();
		dt = PhoenixDashboardAEPerformance.AeChartDateRange(vesselid, AeNo);
		if (dt.Rows.Count > 0)
		{
			int i = 0;
			while (i < dt.Rows.Count)
			{
				daterange = daterange + "'" + dt.Rows[i]["FLDREPORTDATE"].ToString() + "'" + ",";
				i = i + 1;
			}
			daterange = "[" + daterange.TrimEnd(',') + "]";

		}
		return daterange.ToString();
	}
	public void AeSwitchBoard()
	{
		string strload = "";
		string strLoadper = "";
		string strLoadAmp = "";
		string strRPM = "";
		DataSet ds;
		int vesselid = int.Parse(ViewState["vesselid"].ToString());
		int AeNo = int.Parse(ViewState["AeNo"].ToString());

		ds = PhoenixDashboardAEPerformance.AeSwitchBoard(vesselid, AeNo);
		DataTable dt = ds.Tables[0];
		if (dt.Rows.Count > 0)
		{
			int i = 0;
			while (i < dt.Rows.Count)
			{
				strload = strload + "" + dt.Rows[i]["FLDSBRATEDLOAD"].ToString() + "" + ",";
				strLoadper = strLoadper + "" + dt.Rows[i]["FLDSBLOAD"].ToString() + "" + ",";
				strLoadAmp = strLoadAmp + "" + dt.Rows[i]["FLDSBACTUALAMP"].ToString() + "" + ",";
				strRPM = strRPM + "" + dt.Rows[i]["FLDRPM"].ToString() + "" + ",";
				i = i + 1;
			}
			strload = "[" + strload.TrimEnd(',') + "],";
			strLoadper = "[" + strLoadper.TrimEnd(',') + "],";
			strLoadAmp = "[" + strLoadAmp.TrimEnd(',') + "],";
			strRPM = "[" + strRPM.TrimEnd(',') + "]";

			SeriesData = "[" + strload + strLoadper + strLoadAmp + strRPM + "]";

			dateseries = ChartDateRange(vesselid, AeNo);


		}

	}
}
