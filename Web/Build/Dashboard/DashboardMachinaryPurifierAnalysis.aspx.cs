using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class DashboardMachinaryPurifierAnalysis : PhoenixBasePage
{
	public string dateList = "";
    string conditionList = "";
    string vesselStatus = "";

    string hfopurifier1 = "";
    string hfopurifier2 = "";
    string dopurifier = "";
    string MELOpurifier = "";
    string AELOpurifier = "";

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

            dt = PhoenixMachinaryPerformence.PurifierAnalysis(Int32.Parse(nvc.Get("vesselId"))
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

                    hfopurifier1 = hfopurifier1 + "'" + dt.Rows[i]["FLDHFOPFR1HRS"].ToString() + "'" + ",";
                    hfopurifier2 = hfopurifier2 + "'" + dt.Rows[i]["FLDHFOPFR2HRS"].ToString() + "'" + ",";
                    dopurifier = dopurifier + "'" + dt.Rows[i]["FLDDOPFRHRS"].ToString() + "'" + ",";
                    MELOpurifier = MELOpurifier + "'" + dt.Rows[i]["FLDMELOPFRHRS"].ToString() + "'" + ",";
                    AELOpurifier = AELOpurifier + "'" + dt.Rows[i]["FLDAELOPFRHRS"].ToString() + "'" + ",";
                    
                    
                    i = i + 1;
				}

                hfopurifier1 = "[" + hfopurifier1.TrimEnd(',') + "],";
                hfopurifier2 = "[" + hfopurifier2.TrimEnd(',') + "],";
                dopurifier = "[" + dopurifier.TrimEnd(',') + "],";
                MELOpurifier = "[" + MELOpurifier.TrimEnd(',') + "],";
                AELOpurifier = "[" + AELOpurifier.TrimEnd(',') + "],";

                vesselStatus = "[" + vesselStatus.TrimEnd(',') + "],";
                conditionList = "[" + conditionList.TrimEnd(',') + "]";

                seriesdata = "[" + hfopurifier1 + hfopurifier2 + dopurifier + MELOpurifier + AELOpurifier + vesselStatus + conditionList +"]";
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
