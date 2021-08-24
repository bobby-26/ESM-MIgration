using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class DashboardMachinaryMainEngineAnalysis : PhoenixBasePage
{
	public string dateList = "";
    string conditionList = "";
    string vesselStatus = "";

    string avgRPMList = "";
    string GOVSettingList = "";
    string PowerOutputList = "";
    string MEFocMTPerday = "";
    string log = "";
    string windforce = "";
    string MEMCRRPM = "";
    string PwroutNCR = "";
    string MEFocMTNCR = "";
    string NCRBHP = "";

    string turbocharger1 = "";
    string turbocharger2 = "";
    string MainEngineRPM = "";

    public string seriesdata = "";
    public string seriesdata1 = "";
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

            dt = PhoenixMachinaryPerformence.MainEngineAnalysis(Int32.Parse(nvc.Get("vesselId"))
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

                    avgRPMList = avgRPMList + "'" + dt.Rows[i]["FLDMERPM"].ToString() + "'" + ",";
                    GOVSettingList = GOVSettingList + "'" + dt.Rows[i]["FLDGOVERNORSETTING"].ToString() + "'" + ",";
                    PowerOutputList = PowerOutputList + "'" + dt.Rows[i]["FLDBHP"].ToString() + "'" + ",";
                    MEFocMTPerday = MEFocMTPerday + "'" + dt.Rows[i]["FLDMEFOCPERDAY"].ToString() + "'" + ",";
                    log = log + "'" + dt.Rows[i]["FLDLOGSPEED"].ToString() + "'" + ",";
                    windforce = windforce + "'" + dt.Rows[i]["FLDACTUALWIND"].ToString() + "'" + ",";
                    MEMCRRPM = MEMCRRPM + "'" + dt.Rows[i]["FLDNCRRPM"].ToString() + "'" + ",";
                    PwroutNCR = PwroutNCR + "'" + dt.Rows[i]["FLDNCRBHP"].ToString() + "'" + ",";
                    MEFocMTNCR = MEFocMTNCR + "'" + dt.Rows[i]["FLDNCRFOCONS"].ToString() + "'" + ",";
                    NCRBHP = NCRBHP + "'" + dt.Rows[i]["FLDNCRBHP"].ToString() + "'" + ",";

                    turbocharger1 = turbocharger1 + "'" + dt.Rows[i]["FLDTURBOCHARGER1"].ToString() + "'" + ",";
                    turbocharger2 = turbocharger2 + "'" + dt.Rows[i]["FLDTURBOCHARGER2"].ToString() + "'" + ",";
                    MainEngineRPM = MainEngineRPM + "'" + dt.Rows[i]["FLDMAINENGINERPM"].ToString() + "'" + ",";
                    
                    
                    i = i + 1;
				}

                avgRPMList = "[" + avgRPMList.TrimEnd(',') + "],";
                GOVSettingList = "[" + GOVSettingList.TrimEnd(',') + "],";
                PowerOutputList = "[" + PowerOutputList.TrimEnd(',') + "],";

                MEFocMTPerday = "[" + MEFocMTPerday.TrimEnd(',') + "],";
                vesselStatus = "[" + vesselStatus.TrimEnd(',') + "],";
                conditionList = "[" + conditionList.TrimEnd(',') + "],";
                MEMCRRPM = "[" + MEMCRRPM.TrimEnd(',') + "],";
                PwroutNCR = "[" + PwroutNCR.TrimEnd(',') + "],";
                MEFocMTNCR = "[" + MEFocMTNCR.TrimEnd(',') + "],";
                windforce = "[" + windforce.TrimEnd(',') + "],";
                log = "[" + log.TrimEnd(',') + "],";
                NCRBHP = "[" + NCRBHP.TrimEnd(',') + "]";

                turbocharger1 = "[" + turbocharger1.TrimEnd(',') + "],";
                turbocharger2 = "[" + turbocharger2.TrimEnd(',') + "],";
                MainEngineRPM = "[" + MainEngineRPM.TrimEnd(',') + "],";

                seriesdata = "[" + avgRPMList + GOVSettingList + PowerOutputList + MEFocMTPerday + vesselStatus + conditionList + MEMCRRPM + PwroutNCR + MEFocMTNCR + windforce + log + NCRBHP +"]";
                seriesdata1 = "[" + MainEngineRPM + turbocharger1 + turbocharger2 + vesselStatus + conditionList + "]";
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
