using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
public partial class DashboardTechnicalAuxiliaryPerformanceCoolingWaterSystem : PhoenixBasePage
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

        AeCoolingWaterSystem();
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

    protected void AeCoolingWaterSystem()
    {
        string strCWINLETC = "";
        string strCWINLETBAR = "";
    
        int vesselid = int.Parse(ViewState["vesselid"].ToString());
        int AeNo = int.Parse(ViewState["AeNo"].ToString());

        DataSet ds;
        ds = PhoenixDashboardAEPerformance.AeCoolingWaterSystem(vesselid, AeNo);

        if (ds.Tables[0].Rows.Count > 0)
        {
            int i = 0;
            while (i < ds.Tables[0].Rows.Count)
            {
                strCWINLETBAR = strCWINLETBAR + "" + ds.Tables[0].Rows[i]["FLDCWINLETBAR"].ToString() + "" + ",";
                strCWINLETC = strCWINLETC + "" + ds.Tables[0].Rows[i]["FLDCWINLETC"].ToString() + "" + ",";

                i = i + 1;
            }

            strCWINLETBAR = "[" + strCWINLETBAR.TrimEnd(',') + "],";
            strCWINLETC = "[" + strCWINLETC.TrimEnd(',') + "]";
        

            seriesData = "[" + strCWINLETBAR + strCWINLETC + "]";

            dateseries = ChartDateRange(vesselid, AeNo);
        }

    }

}
