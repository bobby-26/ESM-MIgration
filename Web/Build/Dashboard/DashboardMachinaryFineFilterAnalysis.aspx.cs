using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class DashboardMachinaryFineFilterAnalysis : PhoenixBasePage
{
	public string dateList = "";
    string conditionList = "";
    string vesselStatus = "";

    string mefocounter = "";
    string mefohrsop = "";
    string melocounter = "";
    string melohrsop = "";
    string fullspeed = "";
    string reducespeed = "";
    string FOavgOperation = "";
    string LOavgOperation = "";

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

            dt = PhoenixMachinaryPerformence.FineFilterAnalysis(Int32.Parse(nvc.Get("vesselId"))
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


                    mefocounter = mefocounter + "'" + dt.Rows[i]["FLDMEFOAUTOBACKWASHFILTERCOUNTER"].ToString() + "'" + ",";
                    mefohrsop = mefohrsop + "'" + dt.Rows[i]["FLDMEFOAUTOBWFILTERHR"].ToString() + "'" + ",";
                    melocounter = melocounter + "'" + dt.Rows[i]["FLDMELOAUTOBACKWASHFILTERCOUNTER"].ToString() + "'" + ",";
                    melohrsop = melohrsop + "'" + dt.Rows[i]["FLDMELOAUTOBWFILTERHR"].ToString() + "'" + ",";
                    fullspeed = fullspeed + "'" + dt.Rows[i]["FLDFULLSPEEDHR"].ToString() + "'" + ",";
                    reducespeed = reducespeed + "'" + dt.Rows[i]["FLDREDUCEDSPEEDHR"].ToString() + "'" + ",";
                    FOavgOperation = FOavgOperation + "'" + dt.Rows[i]["FLDAVGMEFOOPERATION"].ToString() + "'" + ",";
                    LOavgOperation = LOavgOperation + "'" + dt.Rows[i]["FLDAVGMELOOPERATION"].ToString() + "'" + ",";

                    i = i + 1;
				}

                mefocounter = "[" + mefocounter.TrimEnd(',') + "],";
                mefohrsop = "[" + mefohrsop.TrimEnd(',') + "],";
                melocounter = "[" + melocounter.TrimEnd(',') + "],";
                melohrsop = "[" + melohrsop.TrimEnd(',') + "],";
                fullspeed = "[" + fullspeed.TrimEnd(',') + "],";
                reducespeed = "[" + reducespeed.TrimEnd(',') + "],";

                FOavgOperation = "[" + FOavgOperation.TrimEnd(',') + "]";
                LOavgOperation = "[" + LOavgOperation.TrimEnd(',') + "]";

                vesselStatus = "[" + vesselStatus.TrimEnd(',') + "],";
                conditionList = "[" + conditionList.TrimEnd(',') + "],";


                seriesdata = "[" + mefocounter + mefohrsop + vesselStatus + conditionList + fullspeed + reducespeed + FOavgOperation + "]";
                seriesdata1 = "[" + melocounter + melohrsop + vesselStatus + conditionList + fullspeed + reducespeed + LOavgOperation+ "]";
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
