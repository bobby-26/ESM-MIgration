using Newtonsoft.Json;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Inspection_InspectionWRHNCTypeCountbyRankVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT");
        MenuReportsFilter.MenuList = toolbar.Show();

        NameValueCollection Filter = InspectionFilter.CurrentWRHNcTypeCountbyRankFilter;

        string FromDate = General.GetNullableString(Filter.Get("ucFromDate"));
        string ToDate = General.GetNullableString(Filter.Get("ucToDate"));
        string Fleet = General.GetNullableString(Filter.Get("ucFleet").ToString());
        string Vessel = General.GetNullableString(Filter.Get("ucVessel").ToString());
        string Rank = General.GetNullableString(Filter.Get("ucRank").ToString());

        IDictionary<string, string> Params = new Dictionary<string, string>();
        Params.Add(new KeyValuePair<string, string>("Fleet", Fleet));
        Params.Add(new KeyValuePair<string, string>("Vessel", Vessel));
        Params.Add(new KeyValuePair<string, string>("FromDate", FromDate));
        Params.Add(new KeyValuePair<string, string>("ToDate", ToDate));
        Params.Add(new KeyValuePair<string, string>("Rank", Rank));

        Params.Add(new KeyValuePair<string, string>("Year", null));
        Params.Add(new KeyValuePair<string, string>("Quarter", null));
        Params.Add(new KeyValuePair<string, string>("Month", null));

        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);

        string result = PhoenixInspectionWRHNCTypeCountbyRank.GetWRHTypeCountVisualParams();
        string url = Page.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
        Page.ClientScript.RegisterStartupScript(this.GetType(),
            "createChart", String.Format("createChart({0}, {1});", result, jsonParams), true);
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            Response.Redirect("../Inspection/InspectionWRHNCTypeCountbyRank.aspx");
        }
    }

    #region NCTypeCountbyRank

    [WebMethod]
    public static string NCCountRankChartbyYear(string FromDate, string ToDate, string Fleet, string Rank, string Vessel)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            //ParameterList.Add(DataAccess.GetDBParameter("@ROWUSERCODE", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, usercode));
            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Vessel));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTRANKBYYEAR", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string NCCountRankChartbyQuarter(string FromDate, string ToDate, string Year, string Fleet, string Rank, string Vessel)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            //ParameterList.Add(DataAccess.GetDBParameter("@ROWUSERCODE", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, usercode));
            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTRANKBYQUARTER", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string NCCountRankChartbyMonth(string FromDate, string ToDate, string Year, string Quarter, string Fleet, string Rank, string Vessel)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            //ParameterList.Add(DataAccess.GetDBParameter("@ROWUSERCODE", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, usercode));
            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTRANKBYMONTH", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string NCCountChartRankbyFleet(string FromDate, string ToDate, string Year, string Quarter, string Month, string Fleet, string Rank, string Vessel)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            //ParameterList.Add(DataAccess.GetDBParameter("@ROWUSERCODE", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, usercode));
            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Month));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTRANKBYFLEET", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string NCCountChartRankbyVessel(string FromDate, string ToDate, string Year, string Quarter, string Month, string Fleet, string Rank, string Vessel)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            //ParameterList.Add(DataAccess.GetDBParameter("@ROWUSERCODE", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, usercode));
            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Month));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTRANKBYVESSEL", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string NCCountChartbyRank(string FromDate, string ToDate, string Year, string Quarter, string Month, string Fleet, string Rank, string Vessel)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            //ParameterList.Add(DataAccess.GetDBParameter("@ROWUSERCODE", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, usercode));
            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Month));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTBYRANK", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion NCTypeCountbyRank
}
