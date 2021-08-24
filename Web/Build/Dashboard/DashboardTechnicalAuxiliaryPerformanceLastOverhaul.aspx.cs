using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
using System.Web.Services;
using System.Web.Script.Serialization;


public partial class DashboardTechnicalAuxiliaryPerformanceLastOverhaul : PhoenixBasePage
{
    public string dateseries = "";
    public string overhaulData = "";
	public string seriesData = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["vesselid"] != null)
                ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
            else
                ViewState["vesselid"] = "0";

            if (Request.QueryString["AeNo"] != null)
                ViewState["AeNo"] = Request.QueryString["AeNo"].ToString();
            else
                ViewState["AeNo"] = "1";
        }

        AeLastOverhaul();
    }

    protected static string ChartDateRange(int vesselid, int AeNo)
    {
        string daterange = "";
        DataTable dt = new DataTable();
        dt = PhoenixDashboardAEPerformance.AeChartDateRange(vesselid, AeNo);
        if (dt.Rows.Count > 0)
        {
            int i = 0;
            while (i < dt.Rows.Count)
            {
                daterange = daterange + "'" + dt.Rows[i]["FLDREPORTDATE"].ToString() + "'" + ",";
                i = i + 1;
            }
            daterange = "[" + daterange.TrimEnd(',') + "]";

        }
        return daterange.ToString();
    }

    protected void AeLastOverhaul()
    {
        string strLOPiston = "";
        string strLOFOInjector = "";
        string strLOFOPump = "";
        string strLOSUCValve = "";
        string strLOEXHValve = "";
        string strLOTCWater = "";
        string strLOTCRENEW = "";
		string strChartData = "";

        int vesselid = int.Parse(ViewState["vesselid"].ToString());
        int AeNo = int.Parse(ViewState["AeNo"].ToString());

        DataSet ds;
        ds = PhoenixDashboardAEPerformance.AeLastOverhaul(vesselid, AeNo);
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            int i = 0;
            while (i < ds.Tables[0].Rows.Count)
            {
                strLOPiston = strLOPiston + "" + ds.Tables[0].Rows[i]["FLDLOPISTON"].ToString() + "" + ",";
                strLOFOInjector = strLOFOInjector + "" + ds.Tables[0].Rows[i]["FLDLOFOINJECTOR"].ToString() + "" + ",";
                strLOFOPump = strLOFOPump + "" + ds.Tables[0].Rows[i]["FLDLOFOPUMP"].ToString() + "" + ",";
                strLOSUCValve = strLOSUCValve + "" + ds.Tables[0].Rows[i]["FLDLOSUCVALVE"].ToString() + "" + ",";
                strLOEXHValve = strLOEXHValve + "" + ds.Tables[0].Rows[i]["FLDLOEXHVALVE"].ToString() + "" + ",";
                strLOTCWater = strLOTCWater + "" + ds.Tables[0].Rows[i]["FLDLOTCWATER"].ToString() + "" + ",";
                strLOTCRENEW = strLOTCRENEW + "" + ds.Tables[0].Rows[i]["FLDLOTCRENEW"].ToString() + "" + ",";
				strChartData = strChartData + "" + ds.Tables[0].Rows[i]["FLDCHARTDATA"].ToString() + "" + ",";

				i = i + 1;
            }

            strLOPiston = "[" + strLOPiston.TrimEnd(',') + "],";
            strLOFOInjector = "[" + strLOFOInjector.TrimEnd(',') + "],";
            strLOFOPump = "[" + strLOFOPump.TrimEnd(',') + "],";
            strLOSUCValve = "[" + strLOSUCValve.TrimEnd(',') + "],";
            strLOEXHValve = "[" + strLOEXHValve.TrimEnd(',') + "],";
            strLOTCWater = "[" + strLOTCWater.TrimEnd(',') + "],";
            strLOTCRENEW = "[" + strLOTCRENEW.TrimEnd(',') + "]";
			

			seriesData = strChartData = "[" + strChartData.TrimEnd(',') + "]";
			overhaulData = "[" + strLOPiston + strLOFOInjector + strLOFOPump + strLOSUCValve + strLOEXHValve + strLOTCWater + strLOTCRENEW + "]";

            dateseries = ChartDateRange(vesselid, AeNo);
        }


    }


}
