using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Dashboard;
using System.Collections.Specialized;

public partial class DashboardVesselPositionAlertsMonthly : PhoenixBasePage
{
    public string dateList = "";
	public string seriesdata = "";
    public string responseMsg = "'success'";
    string data = "";
    string alertyn = "";
    string measure = "";
    string unit = "";
    string vesselstatus = "";
    string vesselcondition = "";
    string meloadkw = "";
    string swtemprature = "";
    string MELoaddiv1000 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
            
        }
    }
    protected void BindData()
    {

        DataSet ds;
		DataTable dt = new DataTable();
        NameValueCollection nvc = FilterDashboard.CurrentMachinaryPerformenceChart;
        try
        {
			ds = PhoenixDashboardPerformance.DashboardVesselPositionAlertMonthly(Int32.Parse(nvc.Get("vesselId"))
                                                                                    , General.GetNullableDateTime(nvc.Get("FromDate"))
                                                                                    , General.GetNullableDateTime(nvc.Get("ToDate"))
                                                                                    , General.GetNullableString(nvc.Get("vslStatus"))
                                                                                    , General.GetNullableInteger(nvc.Get("weather"))
                                                                                    , General.GetNullableInteger(nvc.Get("badWeather"))
                                                                                    , General.GetNullableInteger(nvc.Get("vslCondition"))
                                                                                    ,General.GetNullableString(nvc.Get("measurename")));
			dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                int i = 0;
                while (i < dt.Rows.Count)
                {
                    data = data + dt.Rows[i]["FLDVALUE"].ToString() + ",";
                    alertyn = alertyn + dt.Rows[i]["FLDALERTYN"].ToString() + ",";
					dateList = dateList + "'" + dt.Rows[i]["FLDDATE"].ToString() + "'" + ",";
                    measure = measure + "'" + dt.Rows[i]["MEASURENAME"].ToString() + "'" + ",";
                    unit = unit + "'" + dt.Rows[i]["FLDUNIT"].ToString() + "'" + ",";
                    vesselstatus = vesselstatus + "'" + dt.Rows[i]["FLDVESSELSTATUS"].ToString() + "'" + ",";
                    vesselcondition = vesselcondition + "'" + dt.Rows[i]["FLDCONDITION"].ToString() + "'" + ",";
                    meloadkw = meloadkw + "'" + dt.Rows[i]["FLDLOADKW"].ToString() + "'" + ",";
                    swtemprature = swtemprature + "'" + dt.Rows[i]["FLDSWTEMP"].ToString() + "'" + ",";
                    MELoaddiv1000 = MELoaddiv1000 + "'" + (decimal.Parse( dt.Rows[i]["FLDLOADKW"].ToString())/100).ToString("0.00") + "'" + ",";

                    i = i + 1;
                }
               
                data = "[" + data.TrimEnd(',') + "],";
                alertyn = "[" + alertyn.TrimEnd(',') + "],";
                measure = "[" + measure.TrimEnd(',') + "],";
                unit = "[" + unit.TrimEnd(',') + "],";
                vesselstatus = "[" + vesselstatus.TrimEnd(',') + "],";
                vesselcondition = "[" + vesselcondition.TrimEnd(',') + "],";
                meloadkw = "[" + meloadkw.TrimEnd(',') + "],";
                swtemprature = "[" + swtemprature.TrimEnd(',') + "],";
                MELoaddiv1000 = "[" + MELoaddiv1000.TrimEnd(',') + "]";

                dateList = "[" + dateList.TrimEnd(',') + "]";

                

                seriesdata = "[" + data + alertyn + dateList + "," + measure + unit+ vesselstatus + vesselcondition + meloadkw + swtemprature + MELoaddiv1000 + "]";

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
        seriesdata = "[[],[],[],[],[],[],[]]";
        dateList = "[[]]";
    }

}
