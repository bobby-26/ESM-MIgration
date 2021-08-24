using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.Data;
using System.Collections.Specialized;
using SouthNests.Phoenix.Dashboard;

public partial class DashboardVesselPositionAlertAuxillaryLoaddetails : PhoenixBasePage
{
    public string dateList = "";
	public string seriesdata = "";
    public string seriesdata1 = "";
    public string responseMsg = "'success'";
    //ae1
    string powerauxen1 = "";
    string powerandrunhouraux1 = "";
    string runhouraux1 = "";
    string loadauxgenaux1 = "";
    string loadandrunhouauxgenaux1 = "";

    //ae2
    string powerauxen2 = "";
    string powerandrunhouraux2 = "";
    string runhouraux2 = "";
    string loadauxgenaux2 = "";
    string loadandrunhouauxgenaux2 = "";

    //ae3
    string powerauxen3 = "";
    string powerandrunhouraux3 = "";
    string runhouraux3 = "";
    string loadauxgenaux3 = "";
    string loadandrunhouauxgenaux3 = "";

    //ae4
    string powerauxen4 = "";
    string powerandrunhouraux4 = "";
    string runhouraux4 = "";
    string loadauxgenaux4 = "";
    string loadandrunhouauxgenaux4 = "";

    string aeFoc = "";
	string blrFoc = "";
    string auxLoad = "";
    string auxLoadCapacity = "";
    string auxLoadpercent = "";
    string totalHoursOperation = "";

    string difference = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();            
        }
    }
    protected void BindData()
    {


		//int vesselid= int.Parse(ViewState["vesselid"].ToString());
        NameValueCollection nvc = FilterDashboard.CurrentMachinaryPerformenceChart;
        DataSet ds;
		DataTable dt = new DataTable();
        try
        {
            ds = PhoenixDashboardPerformance.AuxiliaryEngjineAlert(Int32.Parse(nvc.Get("vesselId"))
                                                                                    , General.GetNullableDateTime(nvc.Get("FromDate"))
                                                                                    , General.GetNullableDateTime(nvc.Get("ToDate"))
                                                                                    , General.GetNullableString(nvc.Get("vslStatus"))
                                                                                    , General.GetNullableInteger(nvc.Get("weather"))
                                                                                    , General.GetNullableInteger(nvc.Get("badWeather"))
                                                                                    , General.GetNullableInteger(nvc.Get("vslCondition"))
                                                                                    );

            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                int i = 0;
                while (i < dt.Rows.Count)
                {

                    //AUXILLARY ENGINE 01
                    powerauxen1 = powerauxen1 + dt.Rows[i]["FLDPOWERAUXILLARYENGINE01"].ToString() + ",";
                    powerandrunhouraux1 = powerandrunhouraux1 + dt.Rows[i]["FLDPOWERANDRUNHRSAUXILLARYENGINE01"].ToString() + ",";
                    runhouraux1 = runhouraux1 + dt.Rows[i]["FLDRUNHRSAUXENGINE01"].ToString() + ",";
                    loadauxgenaux1 = loadauxgenaux1 + dt.Rows[i]["FLDLOADAUXENGINEGENERATOR01"].ToString() + ",";
                    loadandrunhouauxgenaux1 = loadandrunhouauxgenaux1 + dt.Rows[i]["FLDLOADANDRUNHRSAUXILLARYENGINE01"].ToString() + ",";
                    //AUXILLARY ENGINE 02
                    powerauxen2 = powerauxen2 + dt.Rows[i]["FLDPOWERAUXILLARYENGINE02"].ToString() + ",";
                    powerandrunhouraux2 = powerandrunhouraux2 + dt.Rows[i]["FLDPOWERANDRUNHRSAUXILLARYENGINE02"].ToString() + ",";
                    runhouraux2 = runhouraux2 + dt.Rows[i]["FLDRUNHRSAUXENGINE02"].ToString() + ",";
                    loadauxgenaux2 = loadauxgenaux2 + dt.Rows[i]["FLDLOADAUXENGINEGENERATOR02"].ToString() + ",";
                    loadandrunhouauxgenaux2 = loadandrunhouauxgenaux2 + dt.Rows[i]["FLDLOADANDRUNHRSAUXILLARYENGINE02"].ToString() + ",";
                    //AUXILLARY ENGINE 02
                    powerauxen3 = powerauxen3 + dt.Rows[i]["FLDPOWERAUXILLARYENGINE03"].ToString() + ",";
                    powerandrunhouraux3 = powerandrunhouraux3 + dt.Rows[i]["FLDPOWERANDRUNHRSAUXILLARYENGINE03"].ToString() + ",";
                    runhouraux3 = runhouraux3 + dt.Rows[i]["FLDRUNHRSAUXENGINE03"].ToString() + ",";
                    loadauxgenaux3 = loadauxgenaux3 + dt.Rows[i]["FLDLOADAUXENGINEGENERATOR03"].ToString() + ",";
                    loadandrunhouauxgenaux3 = loadandrunhouauxgenaux3 + dt.Rows[i]["FLDLOADANDRUNHRSAUXILLARYENGINE03"].ToString() + ",";
                    //AUXILLARY ENGINE 02
                    powerauxen4 = powerauxen4 + dt.Rows[i]["FLDPOWERAUXILLARYENGINE04"].ToString() + ",";
                    powerandrunhouraux4 = powerandrunhouraux4 + dt.Rows[i]["FLDPOWERANDRUNHRSAUXILLARYENGINE04"].ToString() + ",";
                    runhouraux4 = runhouraux4 + dt.Rows[i]["FLDRUNHRSAUXENGINE04"].ToString() + ", ";
                    loadauxgenaux4 = loadauxgenaux4 + dt.Rows[i]["FLDLOADAUXENGINEGENERATOR04"].ToString() + ",";
                    loadandrunhouauxgenaux4 = loadandrunhouauxgenaux4 + dt.Rows[i]["FLDLOADANDRUNHRSAUXILLARYENGINE04"].ToString() + ",";


                    aeFoc = aeFoc + dt.Rows[i]["FLDAUXFOC"].ToString() + ",";
					blrFoc = blrFoc + dt.Rows[i]["FLDAUXBOILER"].ToString() + ",";
                    auxLoadpercent = auxLoadpercent + dt.Rows[i]["FLDLOADPERCENTAGE"].ToString() + ",";
                    dateList = dateList + "'" + dt.Rows[i]["FLDDATE"].ToString() + "'" + ",";

                    auxLoad = auxLoad + dt.Rows[i]["FLDTOTALAUXLOAD"].ToString() + ",";
                    auxLoadCapacity = auxLoadCapacity + dt.Rows[i]["FLDTOTALAUXCAPACITY"].ToString() + ",";

                    totalHoursOperation = totalHoursOperation + (decimal.Parse(dt.Rows[i]["FLDRUNHRSAUXENGINE01"].ToString())
                                            + decimal.Parse(dt.Rows[i]["FLDRUNHRSAUXENGINE02"].ToString())
                                            + decimal.Parse(dt.Rows[i]["FLDRUNHRSAUXENGINE03"].ToString())
                                            + decimal.Parse(dt.Rows[i]["FLDRUNHRSAUXENGINE04"].ToString())
                                            ).ToString() + ",";

                    difference = difference + ( decimal.Parse(dt.Rows[i]["FLDTOTALAUXCAPACITY"].ToString()) !=0?(decimal.Parse(dt.Rows[i]["FLDTOTALAUXCAPACITY"].ToString()) - decimal.Parse(dt.Rows[i]["FLDTOTALAUXLOAD"].ToString())).ToString(): dt.Rows[i]["FLDTOTALAUXCAPACITY"].ToString()) + ",";

                    i = i + 1;
                }
                //AUXILLARY ENGINE 01
                auxLoad = "[" + auxLoad.TrimEnd(',') + "],";
                auxLoadCapacity = "[" + auxLoadCapacity.TrimEnd(',') + "],";

                powerauxen1 = "[" + powerauxen1.TrimEnd(',') + "],";
                powerandrunhouraux1 = "[" + powerandrunhouraux1.TrimEnd(',') + "],";
                runhouraux1 = "[" + runhouraux1.TrimEnd(',') + "],";
                loadauxgenaux1 = "[" + loadauxgenaux1.TrimEnd(',') + "],";
                loadandrunhouauxgenaux1 = "[" + loadandrunhouauxgenaux1.TrimEnd(',') + "],";
                //AUXILLARY ENGINE 02
                powerauxen2 = "[" + powerauxen2.TrimEnd(',') + "],";
                powerandrunhouraux2 = "[" + powerandrunhouraux2.TrimEnd(',') + "],";
                runhouraux2 = "[" + runhouraux2.TrimEnd(',') + "],";
                loadauxgenaux2 = "[" + loadauxgenaux2.TrimEnd(',') + "],";
                loadandrunhouauxgenaux2 = "[" + loadandrunhouauxgenaux2.TrimEnd(',') + "],";
                //AUXILLARY ENGINE 03
                powerauxen3 = "[" + powerauxen3.TrimEnd(',') + "],";
                powerandrunhouraux3 = "[" + powerandrunhouraux3.TrimEnd(',') + "],";
                runhouraux3 = "[" + runhouraux3.TrimEnd(',') + "],";
                loadauxgenaux3 = "[" + loadauxgenaux3.TrimEnd(',') + "],";
                loadandrunhouauxgenaux3 = "[" + loadandrunhouauxgenaux3.TrimEnd(',') + "],";
                //AUXILLARY ENGINE 04
                powerauxen4 = "[" + powerauxen4.TrimEnd(',') + "],";
                powerandrunhouraux4 = "[" + powerandrunhouraux4.TrimEnd(',') + "],";
                runhouraux4 = "[" + runhouraux4.TrimEnd(',') + "],";
                loadauxgenaux4 = "[" + loadauxgenaux4.TrimEnd(',') + "],";
                loadandrunhouauxgenaux4 = "[" + loadandrunhouauxgenaux4.TrimEnd(',') + "],";
                totalHoursOperation = "[" + totalHoursOperation.TrimEnd(',') + "],";
                difference = "[" + difference.TrimEnd(',') + "]";

                aeFoc = "[" + aeFoc.TrimEnd(',') + "],";
                blrFoc = "[" + blrFoc.TrimEnd(',') + "]";
                auxLoadpercent = "[" + auxLoadpercent.TrimEnd(',') + "],";
                


                dateList = "[" + dateList.TrimEnd(',') + "]";

				//DocRate = "[" + DocRate.TrimEnd(',') + "]";

				seriesdata = "[" + aeFoc + blrFoc + "]";
                seriesdata1 = "[" + auxLoadCapacity + auxLoad + auxLoadpercent + powerauxen1 + powerandrunhouraux1 + runhouraux1 + loadauxgenaux1 + loadandrunhouauxgenaux1 + powerauxen2 + powerandrunhouraux2 + runhouraux2 + loadauxgenaux2 + loadandrunhouauxgenaux2 + powerauxen3 + powerandrunhouraux3 + runhouraux3 + loadauxgenaux3 + loadandrunhouauxgenaux3 + powerauxen4 + powerandrunhouraux4 + runhouraux4 + loadauxgenaux4 + loadandrunhouauxgenaux4 + totalHoursOperation + difference + "]";

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
