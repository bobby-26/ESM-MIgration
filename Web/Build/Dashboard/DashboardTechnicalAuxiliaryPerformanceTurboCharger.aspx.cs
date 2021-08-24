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

public partial class DashboardTechnicalAuxiliaryPerformanceTurboCharger : PhoenixBasePage
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

        AeTurboCharger();
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

    protected void AeTurboCharger()
    {
        string strTCBEFORETC = "";
        string strTCAFTERTC = "";
        string strTCAIRAE = "";
        string strTCAIRAEBAR = "";
        string strTCAIRDIFFP = "";


        int vesselid = int.Parse(ViewState["vesselid"].ToString());
        int AeNo = int.Parse(ViewState["AeNo"].ToString());

        DataSet ds;
        ds = PhoenixDashboardAEPerformance.AeTurboCharger(vesselid, AeNo);

        if (ds.Tables[0].Rows.Count > 0)
        {
            int i = 0;
            while (i < ds.Tables[0].Rows.Count)
            {
                strTCBEFORETC = strTCBEFORETC + "" + ds.Tables[0].Rows[i]["FLDTCBEFORETC"].ToString() + "" + ",";
                strTCAFTERTC = strTCAFTERTC + "" + ds.Tables[0].Rows[i]["FLDTCAFTERTC"].ToString() + "" + ",";
                strTCAIRAE = strTCAIRAE + "" + ds.Tables[0].Rows[i]["FLDTCAIRAE"].ToString() + "" + ",";
                strTCAIRAEBAR = strTCAIRAEBAR + "" + ds.Tables[0].Rows[i]["FLDTCAIRAEBAR"].ToString() + "" + ",";
                strTCAIRDIFFP = strTCAIRDIFFP + "" + ds.Tables[0].Rows[i]["FLDTCAIRDIFFP"].ToString() + "" + ",";

                i = i + 1;
            }

            strTCBEFORETC = "[" + strTCBEFORETC.TrimEnd(',') + "],";
            strTCAFTERTC = "[" + strTCAFTERTC.TrimEnd(',') + "],";
            strTCAIRAE = "[" + strTCAIRAE.TrimEnd(',') + "],";
            strTCAIRAEBAR = "[" + strTCAIRAEBAR.TrimEnd(',') + "],";
            strTCAIRDIFFP = "[" + strTCAIRDIFFP.TrimEnd(',') + "]";
            
            seriesData = "[" + strTCBEFORETC + strTCAFTERTC + strTCAIRAE + strTCAIRAEBAR + strTCAIRDIFFP +  "]";

            dateseries = ChartDateRange(vesselid, AeNo);
        }

    }


}
