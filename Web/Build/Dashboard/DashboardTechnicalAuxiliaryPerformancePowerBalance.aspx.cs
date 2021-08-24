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

public partial class DashboardTechnicalAuxiliaryPerformancePowerBalance : PhoenixBasePage
{
    public string dateseries = "";
    public string pumpIndexData = "";
    public string pmaxData = "";
    public string exhaustTempData = "";
    public string cfwData = ""; 

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

        AePowerBalance();
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

    protected void AePowerBalance()
    {

        string strPMAX1 = "";
        string strPMAX2 = "";
        string strPMAX3 = "";
        string strPMAX4 = "";
        string strPMAX5 = "";
        string strPMAX6 = "";
        string strPMAX7 = "";
        string strPMAX8 = "";
        string strPMAX9 = "";

        string strPINDEX1 = "";
        string strPINDEX2 = "";
        string strPINDEX3 = "";
        string strPINDEX4 = "";
        string strPINDEX5 = "";
        string strPINDEX6 = "";
        string strPINDEX7 = "";
        string strPINDEX8 = "";
        string strPINDEX9 = "";

        string strEXTEMP1 = "";
        string strEXTEMP2 = "";
        string strEXTEMP3 = "";
        string strEXTEMP4 = "";
        string strEXTEMP5 = "";
        string strEXTEMP6 = "";
        string strEXTEMP7 = "";
        string strEXTEMP8 = "";
        string strEXTEMP9 = "";

        string strCOOLINGFW1 = "";
        string strCOOLINGFW2 = "";
        string strCOOLINGFW3 = "";
        string strCOOLINGFW4 = "";             
        string strCOOLINGFW5 = "";              //  field not there in table so same field used need to include 
        string strCOOLINGFW6 = "";
        string strCOOLINGFW7 = "";
        string strCOOLINGFW8 = "";
        string strCOOLINGFW9 = "";
        
        int vesselid = int.Parse(ViewState["vesselid"].ToString());
        int AeNo = int.Parse(ViewState["AeNo"].ToString());

        DataSet ds;
        ds = PhoenixDashboardAEPerformance.AePowerBalance(vesselid, AeNo);

        if (ds.Tables[0].Rows.Count > 0)
        {
            int i = 0;
            while (i < ds.Tables[0].Rows.Count)
            {
                strPMAX1 = strPMAX1 + "" + ds.Tables[0].Rows[i]["FLDPMAX1"].ToString() + "" + ",";
                strPMAX2 = strPMAX2 + "" + ds.Tables[0].Rows[i]["FLDPMAX2"].ToString() + "" + ",";
                strPMAX3 = strPMAX3 + "" + ds.Tables[0].Rows[i]["FLDPMAX3"].ToString() + "" + ",";
                strPMAX4 = strPMAX4 + "" + ds.Tables[0].Rows[i]["FLDPMAX4"].ToString() + "" + ",";
                strPMAX5 = strPMAX5 + "" + ds.Tables[0].Rows[i]["FLDPMAX5"].ToString() + "" + ",";
                strPMAX6 = strPMAX6 + "" + ds.Tables[0].Rows[i]["FLDPMAX6"].ToString() + "" + ",";
                strPMAX7 = strPMAX7 + "" + ds.Tables[0].Rows[i]["FLDPMAX7"].ToString() + "" + ",";
                strPMAX8 = strPMAX8 + "" + ds.Tables[0].Rows[i]["FLDPMAX8"].ToString() + "" + ",";
                strPMAX9 = strPMAX9 + "" + ds.Tables[0].Rows[i]["FLDPMAX9"].ToString() + "" + ",";

                i = i + 1;
            }

            int j = 0;
            while (j < ds.Tables[1].Rows.Count)
            {
                strPINDEX1 = strPINDEX1 + "" + ds.Tables[1].Rows[j]["FLDPINDEX1"].ToString() + "" + ",";
                strPINDEX2 = strPINDEX2 + "" + ds.Tables[1].Rows[j]["FLDPINDEX2"].ToString() + "" + ",";
                strPINDEX3 = strPINDEX3 + "" + ds.Tables[1].Rows[j]["FLDPINDEX3"].ToString() + "" + ",";
                strPINDEX4 = strPINDEX4 + "" + ds.Tables[1].Rows[j]["FLDPINDEX4"].ToString() + "" + ",";
                strPINDEX5 = strPINDEX5 + "" + ds.Tables[1].Rows[j]["FLDPINDEX5"].ToString() + "" + ",";
                strPINDEX6 = strPINDEX6 + "" + ds.Tables[1].Rows[j]["FLDPINDEX6"].ToString() + "" + ",";
                strPINDEX7 = strPINDEX7 + "" + ds.Tables[1].Rows[j]["FLDPINDEX7"].ToString() + "" + ",";
                strPINDEX8 = strPINDEX8 + "" + ds.Tables[1].Rows[j]["FLDPINDEX8"].ToString() + "" + ",";
                strPINDEX9 = strPINDEX9 + "" + ds.Tables[1].Rows[j]["FLDPINDEX9"].ToString() + "" + ",";

                j = j + 1;
            }

            int k = 0;
            while (k < ds.Tables[2].Rows.Count)
            {
                strEXTEMP1 = strEXTEMP1 + "" + ds.Tables[2].Rows[k]["FLDEXTEMP1"].ToString() + "" + ",";
                strEXTEMP2 = strEXTEMP2 + "" + ds.Tables[2].Rows[k]["FLDEXTEMP2"].ToString() + "" + ",";
                strEXTEMP3 = strEXTEMP3 + "" + ds.Tables[2].Rows[k]["FLDEXTEMP3"].ToString() + "" + ",";
                strEXTEMP4 = strEXTEMP4 + "" + ds.Tables[2].Rows[k]["FLDEXTEMP4"].ToString() + "" + ",";
                strEXTEMP5 = strEXTEMP5 + "" + ds.Tables[2].Rows[k]["FLDEXTEMP5"].ToString() + "" + ",";
                strEXTEMP6 = strEXTEMP6 + "" + ds.Tables[2].Rows[k]["FLDEXTEMP6"].ToString() + "" + ",";
                strEXTEMP7 = strEXTEMP7 + "" + ds.Tables[2].Rows[k]["FLDEXTEMP7"].ToString() + "" + ",";
                strEXTEMP8 = strEXTEMP8 + "" + ds.Tables[2].Rows[k]["FLDEXTEMP8"].ToString() + "" + ",";
                strEXTEMP9 = strEXTEMP9 + "" + ds.Tables[2].Rows[k]["FLDEXTEMP9"].ToString() + "" + ",";

                k = k + 1;
            }

            int l = 0;
            while (l < ds.Tables[3].Rows.Count)
            {
                strCOOLINGFW1 = strCOOLINGFW1 + "" + ds.Tables[3].Rows[l]["FLDCOOLINGFW1"].ToString() + "" + ",";
                strCOOLINGFW2 = strCOOLINGFW2 + "" + ds.Tables[3].Rows[l]["FLDCOOLINGFW2"].ToString() + "" + ",";
                strCOOLINGFW3 = strCOOLINGFW3 + "" + ds.Tables[3].Rows[l]["FLDCOOLINGFW3"].ToString() + "" + ",";
                strCOOLINGFW4 = strCOOLINGFW4 + "" + ds.Tables[3].Rows[l]["FLDCOOLINGFW4"].ToString() + "" + ",";
                strCOOLINGFW5 = strCOOLINGFW5 + "" + ds.Tables[3].Rows[l]["FLDCOOLINGFW4"].ToString() + "" + ",";             //  field not there in table so same field used need to include 
                strCOOLINGFW6 = strCOOLINGFW6 + "" + ds.Tables[3].Rows[l]["FLDCOOLINGFW6"].ToString() + "" + ",";
                strCOOLINGFW7 = strCOOLINGFW7 + "" + ds.Tables[3].Rows[l]["FLDCOOLINGFW7"].ToString() + "" + ",";
                strCOOLINGFW8 = strCOOLINGFW8 + "" + ds.Tables[3].Rows[l]["FLDCOOLINGFW8"].ToString() + "" + ",";
                strCOOLINGFW9 = strCOOLINGFW9 + "" + ds.Tables[3].Rows[l]["FLDCOOLINGFW9"].ToString() + "" + ",";

                l = l + 1;
            }

            strPMAX1 = "[" + strPMAX1.TrimEnd(',') + "],";
            strPMAX2 = "[" + strPMAX2.TrimEnd(',') + "],";
            strPMAX3 = "[" + strPMAX3.TrimEnd(',') + "],";
            strPMAX4 = "[" + strPMAX4.TrimEnd(',') + "],";
            strPMAX5 = "[" + strPMAX5.TrimEnd(',') + "],";
            strPMAX6 = "[" + strPMAX6.TrimEnd(',') + "],";
            strPMAX7 = "[" + strPMAX7.TrimEnd(',') + "],";
            strPMAX8 = "[" + strPMAX8.TrimEnd(',') + "],";
            strPMAX9 = "[" + strPMAX9.TrimEnd(',') + "]";

            strPINDEX1 = "[" + strPINDEX1.TrimEnd(',') + "],";
            strPINDEX2 = "[" + strPINDEX2.TrimEnd(',') + "],";
            strPINDEX3 = "[" + strPINDEX3.TrimEnd(',') + "],";
            strPINDEX4 = "[" + strPINDEX4.TrimEnd(',') + "],";
            strPINDEX5 = "[" + strPINDEX5.TrimEnd(',') + "],";
            strPINDEX6 = "[" + strPINDEX6.TrimEnd(',') + "],";
            strPINDEX7 = "[" + strPINDEX7.TrimEnd(',') + "],";
            strPINDEX8 = "[" + strPINDEX8.TrimEnd(',') + "],";
            strPINDEX9 = "[" + strPINDEX9.TrimEnd(',') + "]";

            strEXTEMP1 = "[" + strEXTEMP1.TrimEnd(',') + "],";
            strEXTEMP2 = "[" + strEXTEMP2.TrimEnd(',') + "],";
            strEXTEMP3 = "[" + strEXTEMP3.TrimEnd(',') + "],";
            strEXTEMP4 = "[" + strEXTEMP4.TrimEnd(',') + "],";
            strEXTEMP5 = "[" + strEXTEMP5.TrimEnd(',') + "],";
            strEXTEMP6 = "[" + strEXTEMP6.TrimEnd(',') + "],";
            strEXTEMP7 = "[" + strEXTEMP7.TrimEnd(',') + "],";
            strEXTEMP8 = "[" + strEXTEMP8.TrimEnd(',') + "],";
            strEXTEMP9 = "[" + strEXTEMP9.TrimEnd(',') + "]";

            strCOOLINGFW1 = "[" + strCOOLINGFW1.TrimEnd(',') + "],";
            strCOOLINGFW2 = "[" + strCOOLINGFW2.TrimEnd(',') + "],";
            strCOOLINGFW3 = "[" + strCOOLINGFW3.TrimEnd(',') + "],";
            strCOOLINGFW4 = "[" + strCOOLINGFW4.TrimEnd(',') + "],";
            strCOOLINGFW5 = "[" + strCOOLINGFW5.TrimEnd(',') + "],";
            strCOOLINGFW6 = "[" + strCOOLINGFW6.TrimEnd(',') + "],";
            strCOOLINGFW7 = "[" + strCOOLINGFW7.TrimEnd(',') + "],";
            strCOOLINGFW8 = "[" + strCOOLINGFW8.TrimEnd(',') + "],";
            strCOOLINGFW9 = "[" + strCOOLINGFW9.TrimEnd(',') + "]";

            pmaxData = "[" + strPMAX1 + strPMAX2 + strPMAX3 + strPMAX4 + strPMAX5 + strPMAX6 + strPMAX7 + strPMAX8 + strPMAX9 + "]";
            pumpIndexData = "[" + strPINDEX1 + strPINDEX2 + strPINDEX3 + strPINDEX4 + strPINDEX5 + strPINDEX6 + strPINDEX7 + strPINDEX8 + strPINDEX9 + "]";
            exhaustTempData = "[" + strEXTEMP1 + strEXTEMP2 + strEXTEMP3 + strEXTEMP4 + strEXTEMP5 + strEXTEMP6 + strEXTEMP7 + strEXTEMP8 + strEXTEMP9 + "]";
            cfwData = "[" + strCOOLINGFW1 + strCOOLINGFW2 + strCOOLINGFW3 + strCOOLINGFW4 + strCOOLINGFW5 + strCOOLINGFW6 + strCOOLINGFW7 + strCOOLINGFW8 + strCOOLINGFW9 + "]";

            dateseries = ChartDateRange(vesselid, AeNo);
        }


        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    int i = 0;
        //    while (i < ds.Tables[0].Rows.Count)
        //    {
        //        strPMAX = strPMAX + "" + ds.Tables[0].Rows[i]["FLDPMAX"].ToString() + "" + ",";
        //        strPINDEX = strPINDEX + "" + ds.Tables[0].Rows[i]["FLDPINDEX"].ToString() + "" + ",";
        //        strEXTEMP = strEXTEMP + "" + ds.Tables[0].Rows[i]["FLDEXTEMP"].ToString() + "" + ",";
        //        strCOOLINGFW = strCOOLINGFW + "" + ds.Tables[0].Rows[i]["FLDCOOLINGFW"].ToString() + "" + ",";

        //        i = i + 1;
        //    }

        //    strPMAX = "[" + strPMAX.TrimEnd(',') + "]";
        //    strPINDEX = "[" + strPINDEX.TrimEnd(',') + "]";
        //    strEXTEMP = "[" + strEXTEMP.TrimEnd(',') + "]";
        //    strCOOLINGFW = "[" + strCOOLINGFW.TrimEnd(',') + "]";


        //    pumpIndexData = "[" + strPINDEX + "]";
        //    pmaxData = "[" + strPMAX +  "]";
        //    exhaustTempData = "[" + strEXTEMP + "]";
        //    cfwData = "[" + strCOOLINGFW + "]";

        //    dateseries = ChartDateRange(vesselid, AeNo);
        //}

    }

}
