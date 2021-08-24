using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class DashboardMachinaryTemperatureAnalysis : PhoenixBasePage
{
	public string dateList = "";
    string conditionList = "";
    string vesselStatus = "";

    string engineroomtemp = "";
    string seawatertemp = "";
    string scaveairtemp = "";
    string fuelinlettemp = "";

    string exhasttempmax = "";
    string exhasttempmin = "";

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

            dt = PhoenixMachinaryPerformence.TemperatureAnalysis(Int32.Parse(nvc.Get("vesselId"))
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

                    engineroomtemp = engineroomtemp + "'" + dt.Rows[i]["FLDERTEMP"].ToString() + "'" + ",";
                    seawatertemp = seawatertemp + "'" + dt.Rows[i]["FLDSWTEMP"].ToString() + "'" + ",";
                    scaveairtemp = scaveairtemp + "'" + dt.Rows[i]["FLDSCAVAIRTEMP"].ToString() + "'" + ",";
                    fuelinlettemp = fuelinlettemp + "'" + dt.Rows[i]["FLDFOINLETTEMP"].ToString() + "'" + ",";

                    exhasttempmax = exhasttempmax + "'" + dt.Rows[i]["FLDMAXEXHTEMP"].ToString() + "'" + ",";
                    exhasttempmin = exhasttempmin + "'" + dt.Rows[i]["FLDMINEXHTEMP"].ToString() + "'" + ",";
                   
                    
                    
                    i = i + 1;
				}

                engineroomtemp = "[" + engineroomtemp.TrimEnd(',') + "],";
                seawatertemp = "[" + seawatertemp.TrimEnd(',') + "],";
                scaveairtemp = "[" + scaveairtemp.TrimEnd(',') + "],";
                fuelinlettemp = "[" + fuelinlettemp.TrimEnd(',') + "],";

                exhasttempmax = "[" + exhasttempmax.TrimEnd(',') + "],";
                exhasttempmin = "[" + exhasttempmin.TrimEnd(',') + "],";

                vesselStatus = "[" + vesselStatus.TrimEnd(',') + "],";
                conditionList = "[" + conditionList.TrimEnd(',') + "]";

                seriesdata = "[" + engineroomtemp + seawatertemp + scaveairtemp + fuelinlettemp + vesselStatus + conditionList + "]";
                seriesdata1 = "[" + exhasttempmax + exhasttempmin + vesselStatus + conditionList + "]";
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
