using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class Dashboard_DashboadCommercialFuelEfficiency : PhoenixBasePage
{
	public string seriesData = "";
	public string dateList = "";

	string vslSpeed = "";
	string FoCkg = "";
	string EEOI = "";
	string vslStatus = "";
	string vslCondition = "";
	string Actualwind = "";
	string voyageNo = "";
	string distanceObserved = "";
	string fullSpeed = "";
	string cargoCarried = "";
	string actualConsumption = "";
	string fuelconsNm = "";
	public string responseMsg = "'success'";
	string totalhrs = ""; //full spd+red spd
	string totaldistance = ""; //full spd distance + red spd distance

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
		dt = PhoenixDashboardCommercialPerformance.DashboardFuelEfficiency(Int32.Parse(nvc.Get("vesselId")), General.GetNullableGuid(nvc.Get("voyageid"))
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
				while (i<dt.Rows.Count)
				{
					vslSpeed = vslSpeed + dt.Rows[i]["FLDSHIPSPEED"].ToString() + ",";
					FoCkg = FoCkg + dt.Rows[i]["FLDFUELOILCONSUMPTIONPERDAY"].ToString() + ",";
					EEOI = EEOI + dt.Rows[i]["FLDEEOI"].ToString() + ",";
					vslStatus = vslStatus + "'" + dt.Rows[i]["FLDVESSELSTATUS"].ToString() + "'" + ",";
					vslCondition = vslCondition + "'" + dt.Rows[i]["FLDCONDITION"].ToString() + "'" + ",";
					Actualwind = Actualwind + dt.Rows[i]["FLDACTUALWIND"].ToString() + ",";
					voyageNo = voyageNo + "'" + dt.Rows[i]["FLDVOYAGENO"].ToString() + "'" + ",";
					distanceObserved = distanceObserved + dt.Rows[i]["FLDFULLSPEEDOBSERVEDDISTANCE"].ToString() + ",";
					fullSpeed = fullSpeed + dt.Rows[i]["FLDFULLSPEEDHRS"].ToString() + ",";
					cargoCarried = cargoCarried + dt.Rows[i]["FLDCARGOCARIED"].ToString() + ",";
					actualConsumption = actualConsumption + dt.Rows[i]["FLDACTUALFUELCONS"].ToString() + ",";
					fuelconsNm = fuelconsNm + dt.Rows[i]["FLDFUELCONSUMPEDPERNM"].ToString() + ",";
					totalhrs = totalhrs + dt.Rows[i]["FLDTOTALHRS"].ToString()+",";
					totaldistance = totaldistance + dt.Rows[i]["FLDDISTANCEOBSERVED"].ToString() + ",";
					dateList = dateList + "'" + dt.Rows[i]["FLDDATE"].ToString() + "'" + ",";


					i = i + 1;
				}
				vslSpeed = "[" + vslSpeed.TrimEnd(',') + "],";
				FoCkg = "[" + FoCkg.TrimEnd(',') + "],";
				EEOI = "[" + EEOI.TrimEnd(',') + "],";
				vslStatus = "[" + vslStatus.TrimEnd(',') + "],";
				vslCondition = "[" + vslCondition.TrimEnd(',') + "],";
				Actualwind = "[" + Actualwind.TrimEnd(',') + "],";
				voyageNo = "[" + voyageNo.TrimEnd(',') + "],";
				distanceObserved = "[" + distanceObserved.TrimEnd(',') + "],";
				fuelconsNm = "[" + fuelconsNm.TrimEnd(',') + "],[],[],[],[],";
				fullSpeed = "[" + fullSpeed.TrimEnd(',') + "],";
				cargoCarried = "[" + cargoCarried.TrimEnd(',') + "],";
				actualConsumption = "[" + actualConsumption.TrimEnd(',') + "],";

				totalhrs = "[" + totalhrs.TrimEnd(',') + "],";
				totaldistance = "[" + totaldistance.TrimEnd(',') + "],[],[],[],[],[],[]";
				seriesData = "[" + vslSpeed + FoCkg + EEOI + vslStatus + vslCondition + Actualwind + voyageNo + distanceObserved + fuelconsNm + fullSpeed + cargoCarried + actualConsumption + totalhrs+ totaldistance+ "]";
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
