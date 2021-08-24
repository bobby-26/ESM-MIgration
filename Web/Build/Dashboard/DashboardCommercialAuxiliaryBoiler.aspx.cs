using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
public partial class Dashboard_DashboardCommercialAuxiliaryBoiler : PhoenixBasePage
{
	public string seriesData ="";
	public string dateList = "";

	string BoilerFuel = "";
	string BoilerWater = "";
	string vslspeed = "";
	string vslstatus = "";
	string vslCondition = "";
	string fullSpeed = "";
	string fcCargoHaeating = "";
	string fcTankcleaning = "";
	string fcInerting = "";
	string fcCargoPump = "";
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
			dt = PhoenixDashboardCommercialPerformance.DashboardAuxiliaryboiler(Int32.Parse(nvc.Get("vesselId")), General.GetNullableGuid(nvc.Get("voyageid"))
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
				BoilerFuel = BoilerFuel + dt.Rows[i]["FLDBOILERFOCONSUMPTION"].ToString() + ",";
				BoilerWater = BoilerWater + dt.Rows[i]["FLDBOILERWATERCONSUMPTION"].ToString() + ",";
				vslspeed = vslspeed + dt.Rows[i]["FLDSHIPSPEED"].ToString() + ",";
				vslstatus = vslstatus + "'" + dt.Rows[i]["FLDVESSELSTATUS"].ToString() + "'" + ",";
				vslCondition = vslCondition + "'" + dt.Rows[i]["FLDCONDITION"].ToString() + "'" + ",";
				fullSpeed = fullSpeed + dt.Rows[i]["FLDFULLSPEED"].ToString() + ",";
				fcCargoHaeating = fcCargoHaeating + dt.Rows[i]["FLDCARGOHEATINGCONSUMPTION"].ToString() + ",";
				fcTankcleaning = fcTankcleaning + dt.Rows[i]["FLDTANKCLEANINGCONSUMPTION"].ToString() + ",";
				fcInerting = fcInerting + dt.Rows[i]["FLDINERTINGCONSUMPTION"].ToString() + ",";
				fcCargoPump = fcCargoPump + dt.Rows[i]["FLDCARGOENGINECONSUMPTION"].ToString() + ",";

				dateList = dateList + "'" + dt.Rows[i]["FLDDATE"].ToString() + "'" + ",";


				i = i + 1;
			}
			BoilerFuel = "[" + BoilerFuel.TrimEnd(',') + "],";
			BoilerWater = "[" + BoilerWater.TrimEnd(',') + "],";
			vslspeed = "[" + vslspeed.TrimEnd(',') + "],";
			vslstatus = "[" + vslstatus.TrimEnd(',') + "],";
			vslCondition = "[" + vslCondition.TrimEnd(',') + "],";
			fullSpeed = "[" + fullSpeed.TrimEnd(',') + "],";
			fcCargoHaeating = "[" + fcCargoHaeating.TrimEnd(',') + "],";
			fcTankcleaning = "[" + fcTankcleaning.TrimEnd(',') + "],";
			fcInerting = "[" + fcInerting.TrimEnd(',') + "],";
			fcCargoPump = "[" + fcCargoPump.TrimEnd(',') + "]";

			seriesData = "[" + BoilerFuel + BoilerWater + vslspeed + vslstatus + vslCondition  + fullSpeed + fcCargoHaeating + fcTankcleaning + fcInerting + fcCargoPump + "]";
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
