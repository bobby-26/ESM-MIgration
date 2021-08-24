using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class Dashboard_DashboardCommercialFuelConsumption : PhoenixBasePage
{
	public string seriesData = "";
	public string dateList = "";
	string Meconsumtion = "";
	string MeDoConsumtion = "";
	string AeConsumtion = "";
	string IGG = "";
	string cargoEngine = "";
	string cargoHeating = "";
	string tankCleaning = "";
	string othersConsumption = "";
	string vslStatus = "";
	string vslCondition = "";
	string actualWind = "";
	string Voyageno = "";
	string cpfoc = "";

	string ActualHours = "";
	string MeFoActual = "";
	string MeDoActual = "";
	string AeActual = "";
	string AeDOActual = "";
	string IggActual = "";
	string IggDOActual = "";
	string cargoEngineActual = "";
	string cargoEngineDOActual = "";
	string cargoHeatActual = "";
	string cargoHeatDOActual = "";
	string tankActual = "";
	string tankDOActual = "";
	string otherActual = "";
	string otherDOActual = "";

	public string responseMsg = "'success'";

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			BindData();
		}
	}
	protected void BindData()
	{

		//int vesselid = int.Parse(ViewState["vesselid"].ToString());
		//Guid vayageid = new Guid();
		DataTable dt;
		NameValueCollection nvc = FilterDashboard.CurrentCommercialPerformanceChart;
		try
		{
			dt = PhoenixDashboardCommercialPerformance.DashboardFuelComsumption(Int32.Parse(nvc.Get("vesselId")), General.GetNullableGuid(nvc.Get("voyageid"))
																					, General.GetNullableDateTime(nvc.Get("FromDate"))
																					, General.GetNullableDateTime(nvc.Get("ToDate"))
																					, Int32.Parse(nvc.Get("vslCondition"))
																					, Int32.Parse(nvc.Get("weather"))
																					, General.GetNullableString(nvc.Get("vslStatus"))
																					, Int32.Parse(nvc.Get("badWeather"))
																					, General.GetNullableGuid(nvc.Get("arrivalId")));
			if (dt.Rows.Count > 0)
			{
				int i = 0;
				while (i < dt.Rows.Count)
				{
					Meconsumtion = Meconsumtion + dt.Rows[i]["FLDMEHFOCONS"].ToString() + ",";
					AeConsumtion = AeConsumtion + dt.Rows[i]["FLDAEHFOCONS"].ToString() + ",";
					IGG = IGG + dt.Rows[i]["FLDIGGHFOCONS"].ToString() + ",";
					cargoEngine = cargoEngine + dt.Rows[i]["FLDCARGOENGHFOCONS"].ToString() + ",";
					cargoHeating = cargoHeating + dt.Rows[i]["FLDCTHGHFOCONS"].ToString() + ",";
					tankCleaning = tankCleaning + dt.Rows[i]["FLDTKCLNGHFOCONS"].ToString() + ",";
					othersConsumption = othersConsumption + dt.Rows[i]["FLDOTHHFOCONS"].ToString() + ",";
					vslStatus = vslStatus + "'" + dt.Rows[i]["FLDVESSELSTATUS"].ToString() + "'" + ",";
					vslCondition = vslCondition + "'" + dt.Rows[i]["FLDCONDITION"].ToString() + "'" + ",";
					dateList = dateList + "'" + dt.Rows[i]["FLDDATE"].ToString() + "'" + ",";
					actualWind = actualWind + dt.Rows[i]["FLDACTUALWIND"].ToString() + ",";
					Voyageno = Voyageno + "'" + dt.Rows[i]["FLDVOYAGENO"].ToString() + "'" + ",";
					cpfoc = cpfoc + dt.Rows[i]["FLDCPFOC"].ToString() + ",";
					MeDoConsumtion = MeDoConsumtion + dt.Rows[i]["FLDMEMDOCONS"].ToString() + ",";

					MeFoActual = MeFoActual + dt.Rows[i]["FLDMAINENGINEFOCONSUMPTION"].ToString() + ",";
					AeActual = AeActual + dt.Rows[i]["FLDAUXILLARYFOCONSUMPTION"].ToString() + ",";
					IggActual = IggActual + dt.Rows[i]["FLDIGGFOCONSUMPTION"].ToString() + ",";
					cargoEngineActual = cargoEngineActual + dt.Rows[i]["FLDCARGOENGINEFOCONSUMPTION"].ToString() + ",";
					cargoHeatActual = cargoHeatActual + dt.Rows[i]["FLDCARGOHEATINGFOCONSUMPTION"].ToString() + ",";
					tankActual = tankActual + dt.Rows[i]["FLDTANKCLEANINGFOCONSUMPTION"].ToString() + ",";
					otherActual = otherActual + dt.Rows[i]["FLDOTHERFOCONSUMPTION"].ToString() + ",";
					ActualHours = ActualHours + dt.Rows[i]["FLDRUNHOUR"].ToString() + ",";
					MeDoActual = MeDoActual + dt.Rows[i]["FLDMAINENGINEDOCONSUMPTION"].ToString() + ",";

					AeDOActual = AeDOActual + dt.Rows[i]["FLDAEMDO"].ToString() + ",";
					IggDOActual = IggDOActual + dt.Rows[i]["FLDIGGMDO"].ToString() + ",";
					cargoEngineDOActual = cargoEngineDOActual + dt.Rows[i]["FLDCARGOENGMDO"].ToString() + ",";
					cargoHeatDOActual = cargoHeatDOActual + dt.Rows[i]["FLDCTHGMDO"].ToString() + ",";
					tankDOActual = tankDOActual + dt.Rows[i]["FLDTKCLNGMDO"].ToString() + ",";
					otherDOActual = otherDOActual + dt.Rows[i]["FLDOTHMDO"].ToString() + ",";
					

					i = i + 1;
				}
				Meconsumtion = "[" + Meconsumtion.TrimEnd(',') + "],";
				AeConsumtion = "[" + AeConsumtion.TrimEnd(',') + "],";
				IGG = "[" + IGG.TrimEnd(',') + "],";
				cargoEngine = "[" + cargoEngine.TrimEnd(',') + "],";
				cargoHeating = "[" + cargoHeating.TrimEnd(',') + "],";
				tankCleaning = "[" + tankCleaning.TrimEnd(',') + "],";
				othersConsumption = "[" + othersConsumption.TrimEnd(',') + "],";
				vslStatus = "[" + vslStatus.TrimEnd(',') + "],";
				vslCondition = "[" + vslCondition.TrimEnd(',') + "],[],[],[],[],[],[],[],";
				actualWind = "[" + actualWind.TrimEnd(',') + "],";
				Voyageno = "[" + Voyageno.TrimEnd(',') + "],";
				cpfoc = "[" + cpfoc.TrimEnd(',') + "],";
				MeDoConsumtion = "[" + MeDoConsumtion.TrimEnd(',') + "],";

				MeFoActual = "[" + MeFoActual.TrimEnd(',') + "],";
				AeActual = "[" + AeActual.TrimEnd(',') + "],";
				IggActual = "[" + IggActual.TrimEnd(',') + "],";
				cargoEngineActual = "[" + cargoEngineActual.TrimEnd(',') + "],";
				cargoHeatActual = "[" + cargoHeatActual.TrimEnd(',') + "],";
				tankActual = "[" + tankActual.TrimEnd(',') + "],";
				otherActual = "[" + otherActual.TrimEnd(',') + "],";
				ActualHours = "[" + ActualHours.TrimEnd(',') + "],[],";
				MeDoActual = "[" + MeDoActual.TrimEnd(',') + "],[],[],[],[],[],[],[],[],[],[],";

				AeDOActual = "[" + AeDOActual.TrimEnd(',') + "],";
				IggDOActual = "[" + IggDOActual.TrimEnd(',') + "],";
				cargoEngineDOActual = "[" + cargoEngineDOActual.TrimEnd(',') + "],";
				cargoHeatDOActual = "[" + cargoHeatDOActual.TrimEnd(',') + "],";
				tankDOActual = "[" + tankDOActual.TrimEnd(',') + "],";
				otherDOActual = "[" + otherDOActual.TrimEnd(',') + "],[],[],[],[],[],[],[],[],[],[],[],[]";

				seriesData = "[" + Meconsumtion + AeConsumtion + IGG + cargoEngine + cargoHeating + tankCleaning + othersConsumption + vslStatus + vslCondition + actualWind + Voyageno + cpfoc + MeDoConsumtion + MeFoActual + AeActual + IggActual + cargoEngineActual + cargoHeatActual + tankActual + otherActual + ActualHours + MeDoActual + AeDOActual + IggDOActual + cargoEngineDOActual + cargoHeatDOActual + tankDOActual + otherDOActual+"]";
				dateList = "[" + dateList.TrimEnd(',') + "]";
			}
			else
			{
				EmptyArray();
			}
		}

		catch (Exception ex)
		{
			EmptyArray();
			responseMsg = "alert('" + ex.Message + "')";
		}

	}
	protected void EmptyArray()
	{
		seriesData = "[[]]";
		dateList = "[[]]";
	}
}
