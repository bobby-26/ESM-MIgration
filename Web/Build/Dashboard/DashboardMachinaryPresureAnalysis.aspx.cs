using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class DashboardMachinaryPresureAnalysis : PhoenixBasePage
{
	public string dateList = "";
    string conditionList = "";
	string vesselStatus = "";
    string seawaterlist = "";
    string scavairList = "";
    string fueloilinletList = "";
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
		NameValueCollection nvc = FilterDashboard.CurrentMachinaryPerformenceChart;
		try
		{
            string ss = nvc.Get("vesselId").ToString();

            dt = PhoenixMachinaryPerformence.PresureAnalysis(Int32.Parse(nvc.Get("vesselId"))
																					, General.GetNullableDateTime(nvc.Get("FromDate")) 
																					, General.GetNullableDateTime(nvc.Get("ToDate"))
																					, General.GetNullableString(nvc.Get("vslStatus"))
                                                                                    , General.GetNullableInteger(nvc.Get("weather"))
                                                                                    , General.GetNullableInteger(nvc.Get("badWeather"))
                                                                                    , General.GetNullableInteger(nvc.Get("vslCondition")));
			if (dt.Rows.Count > 0)
			{
				int i = 0;
				while (i < dt.Rows.Count)
				{
                    conditionList = conditionList + "'" + dt.Rows[i]["FLDCONDITION"].ToString() + "'" + ",";
                    dateList = dateList + "'" + dt.Rows[i]["FLDDATE"].ToString() + "'" + ",";
					vesselStatus = vesselStatus + "'" + dt.Rows[i]["FLDVESSELSTATUS"] + "'" + ",";

                    seawaterlist = seawaterlist + "'" + dt.Rows[i]["FLDSWPRESS"].ToString() + "'" + ",";
                    scavairList = scavairList + "'" + dt.Rows[i]["FLDSCAVAIRPRESS"].ToString() + "'" + ",";
                    fueloilinletList = fueloilinletList + "'" + dt.Rows[i]["FLDFUELOILPRESS"].ToString() + "'" + ",";

                    i = i + 1;
				}

                seawaterlist = "[" + seawaterlist.TrimEnd(',') + "],";
                scavairList = "[" + scavairList.TrimEnd(',') + "],";
                fueloilinletList = "[" + fueloilinletList.TrimEnd(',') + "],";
                vesselStatus = "[" + vesselStatus.TrimEnd(',') + "],";
                conditionList = "[" + conditionList.TrimEnd(',') + "]";
    			
                seriesdata = "[" + seawaterlist + scavairList + fueloilinletList + vesselStatus+ conditionList + "]";
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
		seriesdata = "[[],[],[],[],[],[],[]]";
		dateList = "[[]]";
	}
}
