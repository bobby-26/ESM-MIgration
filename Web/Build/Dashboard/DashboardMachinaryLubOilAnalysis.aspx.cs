using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class DashboardMachinaryLubOilAnalysis : PhoenixBasePage
{
	public string dateList = "";
    string conditionList = "";
    string vesselStatus = "";

    string fullspeed = "";
    string poweroutput = "";
    string cocons = "";
    string corob = "";
    string cotbncons = "";
    string cotbnrob = "";
    string consratelimit = "";
    string feedrate = "";
    string expectedcons = "";
    string ecatransit = "";
    string sulphurcontent = "";
    string copreviousrob = "";
    string cotblpreviousrob = "";

    string crankcaseoilprerob = "";
    string crankcaseoilprecons = "";
    string crankcaseoilrob = "";
    string crankcasecapacity = "";

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

            dt = PhoenixMachinaryPerformence.LubeOilAnalysis(Int32.Parse(nvc.Get("vesselId"))
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

                    fullspeed = fullspeed + "'" + dt.Rows[i]["FLDFULLSPEED"].ToString() + "'" + ",";
                    poweroutput = poweroutput + "'" + dt.Rows[i]["FLDPOWEROUTPUT"].ToString() + "'" + ",";
                    cocons = cocons + "'" + dt.Rows[i]["FLDMECYLINDEROILCONS"].ToString() + "'" + ",";
                    corob = corob + "'" + dt.Rows[i]["FLDMECYLINDEROILROB"].ToString() + "'" + ",";
                    cotbncons = cotbncons + "'" + dt.Rows[i]["FLDMECYLINDEROILTBNCONS"].ToString() + "'" + ",";
                    cotbnrob = cotbnrob + "'" + dt.Rows[i]["FLDMECYLINDEROILTBNROB"].ToString() + "'" + ",";
                    consratelimit = consratelimit + "'0'" + ",";

                    //feedrate1 = dt.Rows[i]["FLDPOWEROUTPUT"].ToString() != "0" ? (950 * (decimal.Parse(dt.Rows[i]["FLDMECYLINDEROILCONS"].ToString()) + decimal.Parse(dt.Rows[i]["FLDMECYLINDEROILTBNCONS"].ToString()))) / (24 * decimal.Parse(dt.Rows[i]["FLDPOWEROUTPUT"].ToString())) : 0;

                    //feedrate = feedrate + "'" + feedrate1.ToString("0.00") + "'" + ",";

                    ecatransit = ecatransit + "'" + dt.Rows[i]["FLDECATRANSIT"].ToString() + "'" + ",";
                    sulphurcontent = sulphurcontent + "'" + dt.Rows[i]["FLDSULPHURPERCENT"].ToString() + "'" + ",";
                    copreviousrob = copreviousrob + "'" + dt.Rows[i]["FLDMECYLINDEROILPROB"].ToString() + "'" + ",";
                    cotblpreviousrob = cotblpreviousrob + "'" + dt.Rows[i]["FLDMECYLINDEROILTBNPROB"].ToString() + "'" + ",";


                    crankcaseoilprerob = crankcaseoilprerob + "'" + dt.Rows[i]["FLDMECRANKCASEOILPROB"].ToString() + "'" + ",";
                    crankcaseoilprecons = crankcaseoilprecons + "'" + dt.Rows[i]["FLDMECRANKCASEOILCONS"].ToString() + "'" + ",";
                    crankcaseoilrob = crankcaseoilrob + "'" + dt.Rows[i]["FLDMECRANKCASEOILROB"].ToString() + "'" + ",";
                    crankcasecapacity = crankcasecapacity + "'" + dt.Rows[i]["FLDLUBRICATECAPACITY"].ToString() + "'" + ",";

                    i = i + 1;
				}

                vesselStatus = "[" + vesselStatus.TrimEnd(',') + "],";
                conditionList = "[" + conditionList.TrimEnd(',') + "],";

                fullspeed = "[" + fullspeed.TrimEnd(',') + "],";
                poweroutput = "[" + poweroutput.TrimEnd(',') + "],";
                cocons = "[" + cocons.TrimEnd(',') + "],";
                corob = "[" + corob.TrimEnd(',') + "],";
                cotbncons = "[" + cotbncons.TrimEnd(',') + "],";
                cotbnrob = "[" + cotbnrob.TrimEnd(',') + "],";
                consratelimit = "[" + consratelimit.TrimEnd(',') + "],";
                feedrate = "[],";
                //feedrate = "[" + feedrate.TrimEnd(',') + "],";
                expectedcons = "[],";
                ecatransit = "[" + ecatransit.TrimEnd(',') + "],";
                sulphurcontent = "[" + sulphurcontent.TrimEnd(',') + "],";
                copreviousrob = "[" + copreviousrob.TrimEnd(',') + "],";
                cotblpreviousrob = "[" + cotblpreviousrob.TrimEnd(',') + "]";

                crankcaseoilprerob = "[" + crankcaseoilprerob.TrimEnd(',') + "],";
                crankcaseoilprecons = "[" + crankcaseoilprecons.TrimEnd(',') + "],";
                crankcaseoilrob = "[" + crankcaseoilrob.TrimEnd(',') + "],";
                crankcasecapacity = "[" + crankcasecapacity.TrimEnd(',') + "]";

                seriesdata = "[" + vesselStatus + conditionList + fullspeed + poweroutput + cocons + corob + cotbncons+ cotbnrob+ consratelimit+ feedrate+ expectedcons+ ecatransit + sulphurcontent + copreviousrob + cotblpreviousrob + "]";
                seriesdata1 = "[" + vesselStatus + conditionList + crankcaseoilprerob + crankcaseoilprecons + crankcaseoilrob + crankcasecapacity + "]";
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
