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

public partial class Inspection_InspectionWRHNCTypeCountbyCorrectiveActionVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT");
        MenuReportsFilter.MenuList = toolbar.Show();

        NameValueCollection Filter = InspectionFilter.CurrentWRHNcTypeCountbyCorrectiveActionFilter;

        string FromDate = General.GetNullableString(Filter.Get("ucFromDate"));
        string ToDate = General.GetNullableString(Filter.Get("ucToDate"));
        string Fleet = General.GetNullableString(Filter.Get("ucFleet").ToString());
        string Vessel = General.GetNullableString(Filter.Get("ucVessel").ToString());
        string CorAction = General.GetNullableString(Filter.Get("chkboxlstCorrectiveAction"));

        IDictionary<string, string> Params = new Dictionary<string, string>();
        Params.Add(new KeyValuePair<string, string>("Fleet", Fleet));
        Params.Add(new KeyValuePair<string, string>("Vessel", Vessel));
        Params.Add(new KeyValuePair<string, string>("FromDate", FromDate));
        Params.Add(new KeyValuePair<string, string>("ToDate", ToDate));
        Params.Add(new KeyValuePair<string, string>("CorrectiveAction", CorAction));

        Params.Add(new KeyValuePair<string, string>("Year", null));
        Params.Add(new KeyValuePair<string, string>("Quarter", null));
        Params.Add(new KeyValuePair<string, string>("Month", null));

        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);

        string result = PhoenixInspectionWRHNCTypeCountbyCorrectiveAction.GetWRHNCTypeCountbyCorrectiveActionVisualParams();
        string url = Page.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
        Page.ClientScript.RegisterStartupScript(this.GetType(),
            "createChart", String.Format("createChart({0}, {1});", result, jsonParams), true);
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            Response.Redirect("../Inspection/InspectionWRHNCTypeCountbyCorrectiveAction.aspx");
        }
    }


    #region NcTypeCountbyCorrectiveAction

    [WebMethod]
    public static string NCCountCorrectiveActionChartbyYear(string FromDate,string ToDate,string Fleet,string Vessel,string CorrectiveAction)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@CORRECTIVEACTION", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, CorrectiveAction));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTACTIONBYYEAR", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public static string NCCountCorrectiveActionChartbyQuarter(string FromDate, string ToDate, string Fleet, string Vessel, string CorrectiveAction,string Year)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@CORRECTIVEACTION", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, CorrectiveAction));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTACTIONBYQUARTER", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
         
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public static string NCCountCorrectiveActionChartbyMonth(string FromDate, string ToDate, string Fleet, string Vessel, string CorrectiveAction,string Year,string Quarter)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@CORRECTIVEACTION", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, CorrectiveAction));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTACTIONBYMONTH", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
        
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public static string NCCountCorrectiveActionChartbyFleet(string FromDate, string ToDate, string Fleet, string Vessel, string CorrectiveAction, string Year, string Quarter,string Month)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@CORRECTIVEACTION", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, CorrectiveAction));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Month));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTACTIONBYFLEET", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
        
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public static string NCCountCorrectiveActionChartbyVessel(string FromDate, string ToDate, string Fleet, string Vessel, string CorrectiveAction, string Year, string Quarter, string Month)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@CORRECTIVEACTION", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, CorrectiveAction));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Month));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTACTIONBYVESSEL", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
   
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [WebMethod]
    public static string NCTypeCountChartbyCorrectiveAction(string FromDate, string ToDate, string Fleet, string Vessel, string CorrectiveAction, string Year, string Quarter, string Month)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@CORRECTIVEACTION", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, CorrectiveAction));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Month));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTBYACTION", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
            return result;
        
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion NcTypeCountbyCorrectiveAction
}
