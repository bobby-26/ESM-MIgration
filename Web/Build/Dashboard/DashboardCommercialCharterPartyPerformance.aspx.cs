using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class Dashboard_DashboardCommercialCharterPartyPerformance : PhoenixBasePage
{
	public string dateList = "";
	string conditionList = "";
	string CPwindlist = "";
	string vslWindList = "";
	string CpSOGlist = "";
	string vsSOGlist = "";
	string CpHFOlist = "";
	string vslHFOlist = "";
	string CpDOlist = "";
	string vslDOlist = "";
	string voyageNo = "";
	string vesselStatus = "";
	public string seriesdata = "";
	public string responseMsg = "'success'";
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			BindData();
		}
	}
	protected void BindData()
	{
		DataTable dt;
		NameValueCollection nvc = FilterDashboard.CurrentCommercialPerformanceChart;
		try
		{
			dt = PhoenixDashboardCommercialPerformance.DashboardCharterPartyPerformance(Int32.Parse(nvc.Get("vesselId")), General.GetNullableGuid(nvc.Get("voyageid"))
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
					conditionList = conditionList + "'" + dt.Rows[i]["FLDCONDITION"].ToString() + "'" + ",";
					CPwindlist = CPwindlist + dt.Rows[i]["FLDCPWIND"].ToString() + ",";
					vslWindList = vslWindList + dt.Rows[i]["FLDACTUALWIND"].ToString() + ",";
					CpSOGlist = CpSOGlist + dt.Rows[i]["FLDCPSOG"].ToString() + ",";
					vsSOGlist = vsSOGlist + dt.Rows[i]["FLDVSLSOG"].ToString() + ",";
					CpHFOlist = CpHFOlist + dt.Rows[i]["FLDCPHFO"].ToString() + ",";
					vslHFOlist = vslHFOlist + dt.Rows[i]["FLDVSLHFO"].ToString() + ",";
					CpDOlist = CpDOlist + dt.Rows[i]["FLDCPDO"].ToString() + ",";
					vslDOlist = vslDOlist + dt.Rows[i]["FLDVLSDO"].ToString() + ",";
					dateList = dateList + "'" + dt.Rows[i]["FLDDATE"].ToString() + "'" + ",";
					voyageNo = voyageNo + "'" + dt.Rows[i]["FLDVOYAGENO"].ToString() + "'" + ",";
					vesselStatus = vesselStatus + "'" + dt.Rows[i]["FLDVESSELSTATUS"] + "'" + ",";
				i = i + 1;
				}
				conditionList = "[" + conditionList.TrimEnd(',') + "],";
				CPwindlist = "[" + CPwindlist.TrimEnd(',') + "],";
				vslWindList = "[" + vslWindList.TrimEnd(',') + "],";
				CpSOGlist = "[" + CpSOGlist.TrimEnd(',') + "],";
				vsSOGlist = "[" + vsSOGlist.TrimEnd(',') + "],";
				CpHFOlist = "[" + CpHFOlist.TrimEnd(',') + "],";
				vslHFOlist = "[" + vslHFOlist.TrimEnd(',') + "],";
				CpDOlist = "[" + CpDOlist.TrimEnd(',') + "],";
				vslDOlist = "[" + vslDOlist.TrimEnd(',') + "],";
				voyageNo = "[" + voyageNo.TrimEnd(',') + "],";
				vesselStatus = "[" + vesselStatus.TrimEnd(',') + "]";

				seriesdata = "[" + conditionList + CPwindlist + vslWindList + CpSOGlist + vsSOGlist + CpHFOlist + vslHFOlist + CpDOlist + vslDOlist + voyageNo + "[],[],"+ vesselStatus +"]";
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
			responseMsg = "alert('"+ex.Message+"')";
		}

	}
	protected void EmptyArray()
	{
		seriesdata = "[[],[],[],[],[],[],[],[],[],[],[]]";
		dateList = "[[]]";
	}
}
