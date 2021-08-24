using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class Dashboard_DashboardCommercialVoyageOverview : PhoenixBasePage
{
	public string seriesData = "";
	public string seriesData1 = "";
	public string seriesData2 = "";
	public string dateList = "";

	string vslCondition = "";
	string vslstatus = "";
	string windforce = "";
	string Sog = "";
	string Log = "";
	string cpSpeed = "";
	string trim = "";
	string displacement = "";

	string meLoadfuel = "";
	string meLoadindex = "";
	string slip = "";
	string MeRPm = "";

	string focRate = "";
	string cpFoc = "";
	string cpwind = "";

	string minDisplacement = "";
	string maxDisplacement = "";
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
			dt = PhoenixDashboardCommercialPerformance.DashboardVoyageOverview(Int32.Parse(nvc.Get("vesselId")), General.GetNullableGuid(nvc.Get("voyageid"))
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
					vslCondition = vslCondition + "'" + dt.Rows[i]["FLDCONDITION"].ToString() + "'" + ",";
					windforce = windforce + dt.Rows[i]["FLDACTUALWIND"].ToString() + ",";
					Sog = Sog + dt.Rows[i]["FLDVSLSOG"].ToString() + ",";
					Log = Log + "'" + dt.Rows[i]["FLDVSLLOG"].ToString() + "'" + ",";
					cpSpeed = cpSpeed + dt.Rows[i]["FLDCPSPEED"].ToString() + ",";
					trim = trim + dt.Rows[i]["FLDTRIM"].ToString() + ",";
					displacement = displacement + dt.Rows[i]["FLDDISPLACEMENT"].ToString() + ",";

					meLoadfuel = meLoadfuel + dt.Rows[i]["FLDLOAD"].ToString() + ",";
					meLoadindex = meLoadindex + dt.Rows[i]["FLDMEINDEX"].ToString() + ",";
					slip = slip + dt.Rows[i]["FLDSLIP"].ToString() + ",";
					MeRPm = MeRPm + dt.Rows[i]["FLDMERPM"].ToString() + ",";

					focRate = focRate + dt.Rows[i]["FLDVSLFOCRATE"].ToString() + ",";
					cpFoc = cpFoc + dt.Rows[i]["FLDCPFOCRATE"].ToString() + ",";
					cpwind = cpwind + dt.Rows[i]["FLDCPWIND"].ToString() + ",";
					vslstatus = vslstatus + "'" + dt.Rows[i]["FLDVESSELSTATUS"].ToString()+"'" + ",";
					maxDisplacement = maxDisplacement + dt.Rows[i]["FLDDWTTROPICAL"].ToString() + ",";
					minDisplacement = minDisplacement + dt.Rows[i]["FLDDWTBALLASTCOND"].ToString() + ",";

					dateList = dateList + "'" + dt.Rows[i]["FLDDATE"].ToString() + "'" + ",";


					i = i + 1;
				}
				vslCondition = "[" + vslCondition.TrimEnd(',') + "],";
				windforce = "[" + windforce.TrimEnd(',') + "],";
				Sog = "[" + Sog.TrimEnd(',') + "],";
				Log = "[" + Log.TrimEnd(',') + "],";
				cpSpeed = "[" + cpSpeed.TrimEnd(',') + "],";
				trim = "[" + trim.TrimEnd(',') + "],";
				displacement = "[" + displacement.TrimEnd(',') + "],";

				meLoadfuel = "[" + meLoadfuel.TrimEnd(',') + "],";
				meLoadindex = "[" + meLoadindex.TrimEnd(',') + "],";
				slip = "[" + slip.TrimEnd(',') + "],";
				MeRPm = "[" + MeRPm.TrimEnd(',') + "],";

				focRate = "[" + focRate.TrimEnd(',') + "],";
				cpFoc = "[" + cpFoc.TrimEnd(',') + "],";
				cpwind = "[" + cpwind.TrimEnd(',') + "],";
				vslstatus = "[" + vslstatus.TrimEnd(',') + "],";
				minDisplacement = "[" + minDisplacement.TrimEnd(',') + "],";
				maxDisplacement = "[" + maxDisplacement.TrimEnd(',') + "],";


				seriesData = "[" + vslCondition + windforce + Sog + Log + cpSpeed + trim + displacement + meLoadfuel + meLoadindex + slip + MeRPm + focRate + cpFoc + cpwind + vslstatus + minDisplacement + maxDisplacement +"]";
				dateList = "[" + dateList.TrimEnd(',') + "]";
			}
			else
			{
				EmptyArray();
			}
		}
		catch(Exception ex)
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
