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

public partial class DashboardTechnicalAuxiliaryPerformanceLubeOilSystem : PhoenixBasePage
{
    public string dateseries = "";
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
        AeLubeOilSystem();
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


    protected void AeLubeOilSystem()
    {
        string strLOAEINLETC = "";
        string strLOAEINLETBAR = "";
        string strLOFILTER = "";
        string strLOINLETTC = "";
        string strLOCONSUMPTION = "";

        int vesselid = int.Parse(ViewState["vesselid"].ToString());
        int AeNo = int.Parse(ViewState["AeNo"].ToString());

        DataSet ds;
        ds = PhoenixDashboardAEPerformance.AeLubeOilSystem(vesselid, AeNo);

        if (ds.Tables[0].Rows.Count > 0)
        {
            int i = 0;
            while (i < ds.Tables[0].Rows.Count)
            {
                strLOAEINLETC = strLOAEINLETC + "" + ds.Tables[0].Rows[i]["FLDLOAEINLETC"].ToString() + "" + ",";
                strLOAEINLETBAR = strLOAEINLETBAR + "" + ds.Tables[0].Rows[i]["FLDLOAEINLETBAR"].ToString() + "" + ",";
                strLOFILTER = strLOFILTER + "" + ds.Tables[0].Rows[i]["FLDLOFILTER"].ToString() + "" + ",";
                strLOINLETTC = strLOINLETTC + "" + ds.Tables[0].Rows[i]["FLDLOINLETTC"].ToString() + "" + ",";
                strLOCONSUMPTION = strLOCONSUMPTION + "" + ds.Tables[0].Rows[i]["FLDLOCONSUMPTION"].ToString() + "" + ",";
        
                i = i + 1;
            }

            strLOAEINLETC = "[" + strLOAEINLETC.TrimEnd(',') + "],";
            strLOAEINLETBAR = "[" + strLOAEINLETBAR.TrimEnd(',') + "],";
            strLOFILTER = "[" + strLOFILTER.TrimEnd(',') + "],";
            strLOINLETTC = "[" + strLOINLETTC.TrimEnd(',') + "],";
            strLOCONSUMPTION = "[" + strLOCONSUMPTION.TrimEnd(',') + "]";

            seriesData = "[" + strLOAEINLETC + strLOAEINLETBAR + strLOFILTER + strLOINLETTC + strLOCONSUMPTION + "]";

            dateseries = ChartDateRange(vesselid, AeNo);
        }
        
    }


}
