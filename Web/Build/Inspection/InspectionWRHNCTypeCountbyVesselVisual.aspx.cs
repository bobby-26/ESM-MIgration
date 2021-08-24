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

public partial class Inspection_InspectionWRHNCTypeCountbyVesselVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT");
        MenuReportsFilter.MenuList = toolbar.Show();

        NameValueCollection Filter = InspectionFilter.CurrentWRHNcTypeCountbyVesselFilter;

        string FromDate = General.GetNullableString(Filter.Get("ucFromDate"));
        string ToDate = General.GetNullableString(Filter.Get("ucToDate"));
        string Fleet = General.GetNullableString(Filter.Get("ucFleet").ToString());
        string Vessel = General.GetNullableString(Filter.Get("ucVessel").ToString());

        IDictionary<string, string> Params = new Dictionary<string, string>();
        Params.Add(new KeyValuePair<string, string>("Fleet", Fleet));
        Params.Add(new KeyValuePair<string, string>("Vessel", Vessel));
        Params.Add(new KeyValuePair<string, string>("FromDate", FromDate));
        Params.Add(new KeyValuePair<string, string>("ToDate", ToDate));

        Params.Add(new KeyValuePair<string, string>("Year", null));
        Params.Add(new KeyValuePair<string, string>("Quarter", null));
        Params.Add(new KeyValuePair<string, string>("Month", null));

        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);

        string result = PhoenixInspectionWRHNCTypeCountbyVessel.GetWRHNCTypeCountbyVesselVisualParams();
        string url = Page.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
        Page.ClientScript.RegisterStartupScript(this.GetType(),
            "createChart", String.Format("createChart({0}, {1});", result, jsonParams), true);
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            Response.Redirect("../Inspection/InspectionWRHNCTypeCountbyVessel.aspx");
        }
    }

    #region NcTypeCountbyVessel

    [WebMethod]
    public static string NCCountVesselChartbyYear(string FromDate, string ToDate, string Fleet, string Vessel)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTVESSELBYYEAR", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
            return result; 
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string NCCountVesselChartbyQuarter(string FromDate, string ToDate, string Fleet, string Vessel,string Year)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTVESSELBYQUARTER", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string NCCountVesselChartbyMonth(string FromDate, string ToDate, string Fleet, string Vessel,string Year,string Quarter)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTVESSELBYMONTH", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string NCCountVesselChartbyFleet(string FromDate, string ToDate, string Fleet, string Vessel,string Year,string Quarter,string Month)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Month));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTVESSELBYFLEET", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string NCTypeCountChartbyVessel(string FromDate, string ToDate, string Fleet, string Vessel,string Year,string Quarter,string Month)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Month));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTBYVESSEL", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    #endregion NcTypeCountbyVessel
}
