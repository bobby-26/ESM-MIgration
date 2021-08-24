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

public partial class Inspection_InspectionWRHNCTypeCountbyNOWVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT");
        MenuReportsFilter.MenuList = toolbar.Show();

        NameValueCollection Filter = InspectionFilter.CurrentWRHNcTypeCountbyNOWFilter;

        string FromDate = General.GetNullableString(Filter.Get("ucFromDate"));
        string ToDate = General.GetNullableString(Filter.Get("ucToDate"));
        string Fleet = General.GetNullableString(Filter.Get("ucFleet").ToString());
        string Vessel = General.GetNullableString(Filter.Get("ucVessel").ToString());
        string NOW = General.GetNullableString(Filter.Get("chkboxlstNOW"));

        IDictionary<string, string> Params = new Dictionary<string, string>();
        Params.Add(new KeyValuePair<string, string>("Fleet", Fleet));
        Params.Add(new KeyValuePair<string, string>("Vessel", Vessel));
        Params.Add(new KeyValuePair<string, string>("FromDate", FromDate));
        Params.Add(new KeyValuePair<string, string>("ToDate", ToDate));
        Params.Add(new KeyValuePair<string, string>("NatureOfWork", NOW));

        Params.Add(new KeyValuePair<string, string>("Year", null));
        Params.Add(new KeyValuePair<string, string>("Quarter", null));
        Params.Add(new KeyValuePair<string, string>("Month", null));

        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);

        string result = PhoenixInspectionWRHNCTypeCountbyNOW.GetWRHNCTypeCountbyNOWVisualParams();
        string url = Page.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
        Page.ClientScript.RegisterStartupScript(this.GetType(),
            "createChart", String.Format("createChart({0}, {1});", result, jsonParams), true);
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            Response.Redirect("../Inspection/InspectionWRHNCTypeCountbyNOW.aspx");
        }
    }

    #region NcTypeCountbyNOW
    [WebMethod]
    public static string NcCountNOWChartbyYear(string FromDate,string ToDate,string Fleet,string Vessel,string NatureOfWork)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@NATUREOFWORK", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, NatureOfWork));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTNOWBYYEAR", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string NcCountNOWChartbyQuarter(string FromDate, string ToDate, string Fleet, string Vessel, string NatureOfWork,string Year)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@NATUREOFWORK", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, NatureOfWork));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTNOWBYQUARTER", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
                 
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string NcCountNOWChartbyMonth(string FromDate, string ToDate, string Fleet, string Vessel, string NatureOfWork, string Year,string Quarter)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@NATUREOFWORK", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, NatureOfWork));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTNOWBYMONTH", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string NcCountNOWChartbyFleet(string FromDate, string ToDate, string Fleet, string Vessel, string NatureOfWork, string Year, string Quarter,string Month)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@NATUREOFWORK", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, NatureOfWork));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Month));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTNOWBYFLEET", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string NcCountNOWChartbyVessel(string FromDate, string ToDate, string Fleet, string Vessel, string NatureOfWork, string Year, string Quarter, string Month)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@NATUREOFWORK", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, NatureOfWork));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Month));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTNOWBYVESSEL", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string NcTypeCountChartbyNOW(string FromDate, string ToDate, string Fleet, string Vessel, string NatureOfWork, string Year, string Quarter, string Month)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@NATUREOFWORK", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, NatureOfWork));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Month));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTBYNOW", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion NcTypeCountbyNOW
}
