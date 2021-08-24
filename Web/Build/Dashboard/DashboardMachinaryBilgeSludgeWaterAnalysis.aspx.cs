using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class DashboardMachinaryBilgeSludgeWaterAnalysis : PhoenixBasePage
{
	public string dateList = "";
    string conditionList = "";
    string vesselStatus = "";

    string bilge = "";
    string sludge = "";
    string temp = "";
    string bilgeCapacity = "";
    string SludgeCapacity = "";

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

            dt = PhoenixMachinaryPerformence.BilgeSludgeWaterAnalysis(Int32.Parse(nvc.Get("vesselId"))
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

                    bilge = bilge + "'" + dt.Rows[i]["FLDBILGETANKROB"].ToString() + "'" + ",";
                    sludge = sludge + "'" + dt.Rows[i]["FLDMEFOAUTOBWFILTERHR"].ToString() + "'" + ",";
                    temp = sludge + "'0'" + ",";
                    bilgeCapacity = bilgeCapacity + "'" + dt.Rows[i]["FLDBIGECAPACITY"].ToString() + "'" + ",";
                    SludgeCapacity = SludgeCapacity + "'" + dt.Rows[i]["FLDSLUDGECAPACITY"].ToString() + "'" + ",";

                    i = i + 1;
				}

                temp = "[" + temp.TrimEnd(',') + "],";
                vesselStatus = "[" + vesselStatus.TrimEnd(',') + "],";
                conditionList = "[" + conditionList.TrimEnd(',') + "],";
                bilge = "[" + bilge.TrimEnd(',') + "],";
                sludge = "[" + sludge.TrimEnd(',') + "],";

                bilgeCapacity = "[" + bilgeCapacity.TrimEnd(',') + "],";
                SludgeCapacity = "[" + SludgeCapacity.TrimEnd(',') + "]";

                seriesdata = "[" + temp + vesselStatus + conditionList + bilge + sludge + bilgeCapacity + SludgeCapacity + "]";
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
