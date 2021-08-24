using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
using System.Web.Services;
using System.Web.Script.Serialization;

public partial class DashboardTechnicalAuxiliaryPerformanceSFOC : PhoenixBasePage
{
	public string vesselName = "-";
	public string AeModel = "-";
	public string AeMaker = "-";
	public string TypeofTC = "-";
	public string AENo = "-";

	public string dateseries = "";
	public string SeriesData = "";
	public string SeriesData1 = "";
	public string SeriesData2 = "";

	public string shoptestSFOC = "";

	public string overhauldata = "";
	public string cwEngineData ="";

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

		AeSFOC();
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

	protected void AeSFOC()
	{
		string strSFOC = "";
		string strActualSFOC = "";
		string strCaloric = "";
		string strActualLoad = "";
		string strFoConsumption = "";
		string strLoadPerc = "";
		string strFOInletBar = "";
		string strFoInletC = "";

		string strLOPiston = "";
		string strLOFOInjector = "";
		string strLOFOPump = "";
		string strLOSUCValve = "";
		string strLOEXHValve = "";
		string strLOTCWater = "";
		string strLOTCRENEW = "";

		string cwIntlet = "";
		string barometric = "";
		string engine = "";

		int vesselid = int.Parse(ViewState["vesselid"].ToString());
		int AeNo = int.Parse(ViewState["AeNo"].ToString());


		DataSet ds;
		ds = PhoenixDashboardAEPerformance.AeEngineSFOCChart(vesselid, AeNo);
		if (ds.Tables.Count > 0)
		{
			shoptestSFOC = ds.Tables[0].Rows[0]["SFOC"].ToString();
			if (ds.Tables[1].Rows.Count > 0)
			{
				DataTable dt;
				dt = ds.Tables[1];
				int i = 0;
				while (i < dt.Rows.Count)
				{
					strSFOC = strSFOC + "" + dt.Rows[i]["FLDFOSFOCAMP"].ToString() + "" + ",";
					strActualSFOC = strActualSFOC + "" + dt.Rows[i]["FLDFOSFOCISO"].ToString() + "" + ",";
					strCaloric = strCaloric + "" + dt.Rows[i]["FLDFOCALORIFIC"].ToString() + "" + ",";

					strActualLoad = strActualLoad + "" + dt.Rows[i]["FLDSBACTUALLOAD"].ToString() + "" + ",";
					strFoConsumption = strFoConsumption + "" + dt.Rows[i]["FLDFOCONSUMPTION"].ToString() + "" + ",";
					strLoadPerc = strLoadPerc + "" + dt.Rows[i]["FLDSBLOAD"].ToString() + "" + ",";

					strFoInletC = strFoInletC + "" + dt.Rows[i]["FLDFOINLETC"].ToString() + "" + ",";
					strFOInletBar = strFOInletBar + "" + dt.Rows[i]["FLDFOINLETBAR"].ToString() + "" + ",";

					strLOPiston = strLOPiston + "" + dt.Rows[i]["FLDLOPISTON"].ToString() + "" + ",";
					strLOFOInjector = strLOFOInjector + "" + dt.Rows[i]["FLDLOFOINJECTOR"].ToString() + "" + ",";
					strLOFOPump = strLOFOPump + "" + dt.Rows[i]["FLDLOFOPUMP"].ToString() + "" + ",";
					strLOSUCValve = strLOSUCValve + "" + dt.Rows[i]["FLDLOSUCVALVE"].ToString() + "" + ",";
					strLOEXHValve = strLOEXHValve + "" + dt.Rows[i]["FLDLOEXHVALVE"].ToString() + "" + ",";
					strLOTCWater = strLOTCWater + "" + dt.Rows[i]["FLDLOTCWATER"].ToString() + "" + ",";
					strLOTCRENEW = strLOTCRENEW + "" + dt.Rows[i]["FLDLOTCRENEW"].ToString() + "" + ",";

					cwIntlet = cwIntlet + "" + dt.Rows[i]["FLDACCWWATER"].ToString() + "" + ",";
					barometric = barometric + "" + dt.Rows[i]["FLDACBPRESSURE"].ToString() + "" + ",";
					engine = engine + "" + dt.Rows[i]["FLDACENGINEROOM"].ToString() + "" + ",";

					i = i + 1;
				}

				strSFOC = "[" + strSFOC.TrimEnd(',') + "],";
				strActualSFOC = "[" + strActualSFOC.TrimEnd(',') + "],";
				strCaloric = "[" + strCaloric.TrimEnd(',') + "]";

				strActualLoad = "[" + strActualLoad.TrimEnd(',') + "],";
				strFoConsumption = "[" + strFoConsumption.TrimEnd(',') + "],";
				strLoadPerc = "[" + strLoadPerc.TrimEnd(',') + "]";

				strFoInletC = "[" + strFoInletC.TrimEnd(',') + "],";
				strFOInletBar = "[" + strFOInletBar.TrimEnd(',') + "]";

				strLOPiston = "[" + strLOPiston.TrimEnd(',') + "],";
				strLOFOInjector = "[" + strLOFOInjector.TrimEnd(',') + "],";
				strLOFOPump = "[" + strLOFOPump.TrimEnd(',') + "],";
				strLOSUCValve = "[" + strLOSUCValve.TrimEnd(',') + "],";
				strLOEXHValve = "[" + strLOEXHValve.TrimEnd(',') + "],";
				strLOTCWater = "[" + strLOTCWater.TrimEnd(',') + "],";
				strLOTCRENEW = "[" + strLOTCRENEW.TrimEnd(',') + "]";

				cwIntlet = "[" + cwIntlet.TrimEnd(',') + "],";
				barometric = "[" + barometric.TrimEnd(',') + "],";
				engine = "[" + engine.TrimEnd(',') + "]";

				SeriesData = "[" + strActualSFOC + strSFOC + strCaloric + "]";
				SeriesData1 = "[" + strActualLoad + strFoConsumption + strLoadPerc + "]";
				SeriesData2 = "[" + strFoInletC + strFOInletBar + "]";

				overhauldata = "[" + strLOPiston + strLOFOInjector + strLOFOPump + strLOSUCValve + strLOEXHValve + strLOTCWater + strLOTCRENEW +"]";

				cwEngineData = "[" + cwIntlet + barometric + engine + "]";

				dateseries = ChartDateRange(vesselid, AeNo);

			}
		}

	}
}
	

