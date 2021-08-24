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

public partial class Inspection_InspectionWRHNCTypeCountbyOwnerVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT");
        MenuReportsFilter.MenuList = toolbar.Show();

        NameValueCollection Filter = InspectionFilter.CurrentWRHNcTypeCountbyOwnerFilter;

        string FromDate = General.GetNullableString(Filter.Get("ucFromDate"));
        string ToDate = General.GetNullableString(Filter.Get("ucToDate"));
        string Fleet = General.GetNullableString(Filter.Get("ucFleet").ToString());
        string Vessel = General.GetNullableString(Filter.Get("ucVessel").ToString());
        string Owner = General.GetNullableString(Filter.Get("ucPrincipal").ToString());

        IDictionary<string, string> Params = new Dictionary<string, string>();
        Params.Add(new KeyValuePair<string, string>("Fleet", Fleet));
        Params.Add(new KeyValuePair<string, string>("Vessel", Vessel));
        Params.Add(new KeyValuePair<string, string>("FromDate", FromDate));
        Params.Add(new KeyValuePair<string, string>("ToDate", ToDate));
        Params.Add(new KeyValuePair<string, string>("Owner", Owner));

        Params.Add(new KeyValuePair<string, string>("Year", null));
        Params.Add(new KeyValuePair<string, string>("Quarter", null));
        Params.Add(new KeyValuePair<string, string>("Month", null));

        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);

        string result = PhoenixInspectionWRHNCTypeCountbyOwner.GetWRHNCTypeCountbyOwnerVisualParams();
        string url = Page.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
        Page.ClientScript.RegisterStartupScript(this.GetType(),
            "createChart", String.Format("createChart({0}, {1});", result, jsonParams), true);
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            Response.Redirect("../Inspection/InspectionWRHNCTypeCountbyOwner.aspx");
        }
    }

    #region NCTypeCountbyOwner

    [WebMethod]
    public static string NCCountOwnerChartbyYear(string FromDate, string ToDate, string Fleet, string Owner, string Vessel)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Owner));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTOWNERBYYEAR", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string NCCountOwnerChartbyQuarter(string FromDate, string ToDate, string Fleet, string Owner, string Vessel,string Year)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Owner));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTOWNERBYQUARTER", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string NCCountOwnerChartbyMonth(string FromDate, string ToDate, string Fleet, string Owner, string Vessel, string Year, string Quarter, string Month)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Owner));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));


            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTOWNERBYMONTH", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string NCCountOwnerChartbyFleet(string FromDate, string ToDate, string Fleet, string Owner, string Vessel, string Year, string Quarter, string Month)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Owner));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Month));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTOWNERBYFLEET", ParameterList);

            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string NCCountOwnerChartbyVessel(string FromDate, string ToDate, string Fleet, string Owner, string Vessel, string Year, string Quarter, string Month)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Owner));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Month));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTOWNERBYVESSEL", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string NCTypeCountChartbyOwner(string FromDate, string ToDate, string Fleet, string Owner, string Vessel, string Year,string Quarter,string Month)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fleet));
            ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Owner));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Month));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWRHNCTYPECOUNTBYOWNER", ParameterList);
            string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion NCTypeCountbyOwner
}
