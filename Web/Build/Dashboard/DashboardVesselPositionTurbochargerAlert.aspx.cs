using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class DashboardVesselPositionTurbochargerAlert : PhoenixBasePage
{
	public string dateList = "";
    string conditionList = "";
    string vesselStatus = "";

    string tc1tempOUT = "";
    string tc1tempmIN = "";

    string tc2tempOUT = "";
    string tc2tempmIN = "";

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
        DataSet ds;
		DataTable dt;
		NameValueCollection nvc = FilterDashboard.CurrentMachinaryPerformenceChart;
		try
		{
            string ss = nvc.Get("vesselId").ToString();

            ds = PhoenixDashboardPerformance.TurbochargerAlert(Int32.Parse(nvc.Get("vesselId"))
																					, General.GetNullableDateTime(nvc.Get("FromDate")) 
																					, General.GetNullableDateTime(nvc.Get("ToDate"))
																					, General.GetNullableString(nvc.Get("vslStatus"))
                                                                                    , General.GetNullableInteger(nvc.Get("weather"))
                                                                                    , General.GetNullableInteger(nvc.Get("badWeather"))
                                                                                    , General.GetNullableInteger(nvc.Get("vslCondition")));
            dt = ds.Tables[0];
			if (dt.Rows.Count > 0)
			{
				int i = 0;
				while (i < dt.Rows.Count)
				{
                    conditionList = conditionList + "'" + dt.Rows[i]["FLDCONDITION"].ToString() + "'" + ",";
                    dateList = dateList + "'" + dt.Rows[i]["FLDDATE"].ToString() + "'" + ",";
					vesselStatus = vesselStatus + "'" + dt.Rows[i]["FLDVESSELSTATUS"] + "'" + ",";

                    tc1tempOUT = tc1tempOUT + "'" + dt.Rows[i]["FLDTC1TEMPOUT"].ToString() + "'" + ",";
                    tc1tempmIN = tc1tempmIN + "'" + dt.Rows[i]["FLDTC1TEMPIN"].ToString() + "'" + ",";

                    tc2tempOUT = tc2tempOUT + "'" + dt.Rows[i]["FLDTC2TEMPOUT"].ToString() + "'" + ",";
                    tc2tempmIN = tc2tempmIN + "'" + dt.Rows[i]["FLDTC2TEMPIN"].ToString() + "'" + ",";



                    i = i + 1;
				}

                tc1tempOUT = "[" + tc1tempOUT.TrimEnd(',') + "],";
                tc1tempmIN = "[" + tc1tempmIN.TrimEnd(',') + "],";
                tc2tempOUT = "[" + tc2tempOUT.TrimEnd(',') + "],";
                tc2tempmIN = "[" + tc2tempmIN.TrimEnd(',') + "],";

                vesselStatus = "[" + vesselStatus.TrimEnd(',') + "],";
                conditionList = "[" + conditionList.TrimEnd(',') + "],";

                seriesdata = "[" + tc1tempmIN + tc1tempOUT + vesselStatus + conditionList + tc2tempmIN + tc2tempOUT + "]";
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
