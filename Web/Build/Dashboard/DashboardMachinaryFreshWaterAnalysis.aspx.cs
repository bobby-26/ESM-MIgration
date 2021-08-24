using System;
using SouthNests.Phoenix.Dashboard;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class DashboardMachinaryFreshWaterAnalysis : PhoenixBasePage
{
	public string dateList = "";
    string conditionList = "";
    string vesselStatus = "";

    string freshwaterPreRob = "";
    string freshwaterProduced = "";
    string freshwatercons = "";
    string freshwaterRob = "";

    string drinkingwaterPreRob = "";
    string drinkingwaterProduced = "";
    string drinkingwatercons = "";
    string drinkingwaterRob = "";

    string boilerwaterPreRob = "";
    string boilerwaterProduced = "";
    string boilerwatercons = "";
    string boilerwaterRob = "";

    string tkclngwaterPreRob = "";
    string tkclngwaterProduced = "";
    string tkclngwatercons = "";
    string tkclngwaterRob = "";

    string totalwaterPreRob = "";
    string totalwaterProduced = "";
    string totalwatercons = "";
    string totalwaterRob = "";


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

            dt = PhoenixMachinaryPerformence.FreshWaterAnalysis(Int32.Parse(nvc.Get("vesselId"))
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

                    freshwaterPreRob = freshwaterPreRob + "'" + dt.Rows[i]["FLDFRESHWATERPROB"].ToString() + "'" + ",";
                    freshwaterProduced = freshwaterProduced + "'" + dt.Rows[i]["FLDFRESHWATERPROD"].ToString() + "'" + ",";
                    freshwatercons = freshwatercons + "'" + dt.Rows[i]["FLDFRESHWATERCONS"].ToString() + "'" + ",";
                    freshwaterRob = freshwaterRob + "'" + dt.Rows[i]["FLDFRESHWATERROB"].ToString() + "'" + ",";

                    drinkingwaterPreRob = drinkingwaterPreRob + "'" + dt.Rows[i]["FLDDRINKINGWATERPROB"].ToString() + "'" + ",";
                    drinkingwaterProduced = drinkingwaterProduced + "'" + dt.Rows[i]["FLDDRINKINGWATERPROD"].ToString() + "'" + ",";
                    drinkingwatercons = drinkingwatercons + "'" + dt.Rows[i]["FLDDRINKINGWATERCONS"].ToString() + "'" + ",";
                    drinkingwaterRob = drinkingwaterRob + "'" + dt.Rows[i]["FLDDRINKINGWATERROB"].ToString() + "'" + ",";

                    boilerwaterPreRob = boilerwaterPreRob + "'" + dt.Rows[i]["FLDBOILERWATERPROB"].ToString() + "'" + ",";
                    boilerwaterProduced = boilerwaterProduced + "'" + dt.Rows[i]["FLDBOILERWATERPROD"].ToString() + "'" + ",";
                    boilerwatercons = boilerwatercons + "'" + dt.Rows[i]["FLDBOILERWATERCONS"].ToString() + "'" + ",";
                    boilerwaterRob = boilerwaterRob + "'" + dt.Rows[i]["FLDBOILERWATERROB"].ToString() + "'" + ",";

                    tkclngwaterPreRob = tkclngwaterPreRob + "'" + dt.Rows[i]["FLDTANKCLEANINGWATERPROB"].ToString() + "'" + ",";
                    tkclngwaterProduced = tkclngwaterProduced + "'" + dt.Rows[i]["FLDTANKCLEANINGWATERPROD"].ToString() + "'" + ",";
                    tkclngwatercons = tkclngwatercons + "'" + dt.Rows[i]["FLDTANKCLEANINGWATERCONS"].ToString() + "'" + ",";
                    tkclngwaterRob = tkclngwaterRob + "'" + dt.Rows[i]["FLDTANKCLEANINGWATERROB"].ToString() + "'" + ",";

                    totalwaterPreRob = totalwaterPreRob + "'0'" + ",";
                    totalwaterProduced = totalwaterProduced + "'0'" + ",";
                    totalwatercons = totalwatercons + "'0'" + ",";
                    totalwaterRob = totalwaterRob + "'0'" + ",";

                    i = i + 1;
				}

                freshwaterPreRob = "[" + freshwaterPreRob.TrimEnd(',') + "],";
                freshwaterProduced = "[" + freshwaterProduced.TrimEnd(',') + "],";
                freshwatercons = "[" + freshwatercons.TrimEnd(',') + "],";
                freshwaterRob = "[" + freshwaterRob.TrimEnd(',') + "],";

                drinkingwaterPreRob = "[" + drinkingwaterPreRob.TrimEnd(',') + "],";
                drinkingwaterProduced = "[" + drinkingwaterProduced.TrimEnd(',') + "],";
                drinkingwatercons = "[" + drinkingwatercons.TrimEnd(',') + "],";
                drinkingwaterRob = "[" + drinkingwaterRob.TrimEnd(',') + "],";

                boilerwaterPreRob = "[" + boilerwaterPreRob.TrimEnd(',') + "],";
                boilerwaterProduced = "[" + boilerwaterProduced.TrimEnd(',') + "],";
                boilerwatercons = "[" + boilerwatercons.TrimEnd(',') + "],";
                boilerwaterRob = "[" + boilerwaterRob.TrimEnd(',') + "],";

                tkclngwaterPreRob = "[" + tkclngwaterPreRob.TrimEnd(',') + "],";
                tkclngwaterProduced = "[" + tkclngwaterProduced.TrimEnd(',') + "],";
                tkclngwatercons = "[" + tkclngwatercons.TrimEnd(',') + "],";
                tkclngwaterRob = "[" + tkclngwaterRob.TrimEnd(',') + "],";

                totalwaterPreRob = "[" + totalwaterPreRob.TrimEnd(',') + "],";
                totalwaterProduced = "[" + totalwaterProduced.TrimEnd(',') + "],";
                totalwatercons = "[" + totalwatercons.TrimEnd(',') + "],";
                totalwaterRob = "[" + totalwaterRob.TrimEnd(',') + "]";

                vesselStatus = "[" + vesselStatus.TrimEnd(',') + "],";
                conditionList = "[" + conditionList.TrimEnd(',') + "],";

                seriesdata = "[" + vesselStatus + conditionList + freshwaterPreRob + freshwaterProduced + freshwatercons+ freshwaterRob+ drinkingwaterPreRob+ drinkingwaterProduced + drinkingwatercons + drinkingwaterRob + boilerwaterPreRob + boilerwaterProduced + boilerwatercons + boilerwaterRob+ tkclngwaterPreRob + tkclngwaterProduced + tkclngwatercons + tkclngwaterRob + totalwaterPreRob + totalwaterProduced + totalwatercons + totalwaterRob + "]";
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
