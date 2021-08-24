using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class DashboardMachinaryEarthFaultAnalysis : PhoenixBasePage
{
	public string dateList = "";
    string conditionList = "";
    string vesselStatus = "";

    string aeload1 = "";
    string aeload2 = "";
    string aeload3 = "";
    string aeload4 = "";

    string aeophr1 = "";
    string aeophr2 = "";
    string aeophr3 = "";
    string aeophr4 = "";

    string aemax1 = "";
    string aemax2 = "";
    string aemax3 = "";
    string aemax4 = "";

    string earthfault400 = "";
    string earthfault230and110 = "";

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

            dt = PhoenixMachinaryPerformence.ElectricalandEarthFaultAnalysis(Int32.Parse(nvc.Get("vesselId"))
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

                    aeload1 = aeload1 + "'" + dt.Rows[i]["FLDGENERATORLOADAE1"].ToString() + "'" + ",";
                    aeload2 = aeload2 + "'" + dt.Rows[i]["FLDGENERATORLOADAE2"].ToString() + "'" + ",";
                    aeload3 = aeload3 + "'" + dt.Rows[i]["FLDGENERATORLOADAE3"].ToString() + "'" + ",";
                    aeload4 = aeload4 + "'" + dt.Rows[i]["FLDGENERATORLOADAE4"].ToString() + "'" + ",";
                            
                    aeophr1 = aeophr1 + "'" + dt.Rows[i]["FLDGENERATORLOADAE1OPHRS"].ToString() + "'" + ",";
                    aeophr2 = aeophr2 + "'" + dt.Rows[i]["FLDGENERATORLOADAE2OPHRS"].ToString() + "'" + ",";
                    aeophr3 = aeophr3 + "'" + dt.Rows[i]["FLDGENERATORLOADAE3OPHRS"].ToString() + "'" + ",";
                    aeophr4 = aeophr4 + "'" + dt.Rows[i]["FLDGENERATORLOADAE4OPHRS"].ToString() + "'" + ",";

                    aemax1 = aemax1 + "'" + dt.Rows[i]["FLDPOWERKWAE1"].ToString() + "'" + ",";
                    aemax2 = aemax2 + "'" + dt.Rows[i]["FLDPOWERKWAE2"].ToString() + "'" + ",";
                    aemax3 = aemax3 + "'" + dt.Rows[i]["FLDPOWERKWAE3"].ToString() + "'" + ",";
                    aemax4 = aemax4 + "'" + dt.Rows[i]["FLDPOWERKWAE4"].ToString() + "'" + ",";

                    earthfault400 = earthfault400 + "'" + dt.Rows[i]["FLDEARTHFAULTMONITOR400"].ToString() + "'" + ",";
                    earthfault230and110 = earthfault230and110 + "'" + dt.Rows[i]["FLDEARTHFAULTMONITOR230OR110"].ToString() + "'" + ",";

                    i = i + 1;
				}

                aeload1 = "[" + aeload1.TrimEnd(',') + "],";
                aeload2 = "[" + aeload2.TrimEnd(',') + "],";
                aeload3 = "[" + aeload3.TrimEnd(',') + "],";
                aeload4 = "[" + aeload4.TrimEnd(',') + "],";

                aeophr1 = "[" + aeophr1.TrimEnd(',') + "],";
                aeophr2 = "[" + aeophr2.TrimEnd(',') + "],";
                aeophr3 = "[" + aeophr3.TrimEnd(',') + "],";
                aeophr4 = "[" + aeophr4.TrimEnd(',') + "],";

                aemax1 = "[" + aemax1.TrimEnd(',') + "],";
                aemax2 = "[" + aemax2.TrimEnd(',') + "],";
                aemax3 = "[" + aemax3.TrimEnd(',') + "],";
                aemax4 = "[" + aemax4.TrimEnd(',') + "]";

                earthfault400 = "[" + earthfault400.TrimEnd(',') + "],";
                earthfault230and110 = "[" + earthfault230and110.TrimEnd(',') + "]";


                vesselStatus = "[" + vesselStatus.TrimEnd(',') + "],";
                conditionList = "[" + conditionList.TrimEnd(',') + "],";


                seriesdata = "[" + aeload1 + aeophr1 + aeload2 + aeophr2 + aeload3 + aeophr3 + aeload4 + aeophr4 + vesselStatus + conditionList + aemax1 + aemax2 + aemax3 + aemax4 + "]";
                seriesdata1 = "[" + vesselStatus + conditionList + earthfault400 + earthfault230and110 + "]";
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
