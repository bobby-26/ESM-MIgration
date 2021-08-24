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
using Telerik.Web.UI;

public partial class Inspection_IncidentNearMissAnalysisVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Visual", "SHOWVISUAL", ToolBarDirection.Right);
        toolbar.AddButton("Show Report", "SHOWREPORT" ,ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar.Show();
        MenuReportsFilter.SelectedMenuIndex = 0;

        NameValueCollection Filter = InspectionFilter.CurrentInspectionIncidentNearMissFilter;

        string Year = General.GetNullableString(Filter.Get("ddlYear").ToString());
        string Quarter = General.GetNullableString(Filter.Get("Quarter").ToString());
        string Fleet = General.GetNullableString(Filter.Get("ucFleet").ToString());
        string Vessel = General.GetNullableString(Filter.Get("ucVessel").ToString());
        string VesselType = General.GetNullableString(Filter.Get("ucVesselType").ToString());
        string Principal = General.GetNullableString(Filter.Get("ucPrincipal").ToString());
        string EventType = General.GetNullableString(Filter.Get("ddlEventType").ToString());

        //string Month = General.GetNullableString(Filter.Get("Month").ToString());


        IDictionary<string, string> Params = new Dictionary<string, string>();
        Params.Add(new KeyValuePair<string, string>("Year", Year));
        Params.Add(new KeyValuePair<string, string>("Quarter", Quarter));
        Params.Add(new KeyValuePair<string, string>("Fleet", Fleet));
        Params.Add(new KeyValuePair<string, string>("Vessel", Vessel));
        Params.Add(new KeyValuePair<string, string>("VesselType", VesselType));
        Params.Add(new KeyValuePair<string, string>("Principal", Principal));
        Params.Add(new KeyValuePair<string, string>("EventType", EventType));
        Params.Add(new KeyValuePair<string, string>("Month", null));
        Params.Add(new KeyValuePair<string, string>("ConsCategory", null));

        //Params.Add(new KeyValuePair<string, string>("Month", null));

        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);

        string result = PhoenixInspectionIncidentNearMissAnalysis.GetIncidentNearMissVisualParams();
        string url = Page.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
        Page.ClientScript.RegisterStartupScript(this.GetType(),
            "createChart", String.Format("createChart({0}, {1});", result, jsonParams), true);
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            Response.Redirect("../Inspection/IncidentNearMissAnalysis.aspx");
        }
    }

    [WebMethod]
    public static string QualityChartIncidentAnalysisYearwise(string Year, string Vessel, string VesselType, string Fleet, string Principal, string Quarter, string EventType)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEARLIST", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPELIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNERLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTERLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@EVENTTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, EventType));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINCIDENTBYYEAR", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartIncidentAnalysisQuarterwise(int? Year, string Vessel, string VesselType, string Fleet, string Principal, string Quarter, string EventType)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEARLIST", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPELIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNERLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTERLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@EVENTTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, EventType));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINCIDENTBYQUARTER", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartIncidentAnalysisMonthwise(int? Year, string Vessel, string VesselType, string Fleet, string Principal, string Quarter, string EventType)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEARLIST", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPELIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNERLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTERLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@EVENTTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, EventType));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINCIDENTBYMONTH", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartIncidentAnalysisbyConCategory(int? Year, string Vessel, string VesselType, string Fleet, string Principal, string Quarter, string Month, string EventType)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEARLIST", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPELIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNERLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTERLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTHLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@EVENTTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, EventType));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINCIDENTBYCONCAT", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartIncidentAnalysisbyEventCategory(int? Year, string Vessel, string VesselType, string Fleet, string Principal, string Quarter, string Month, string ConsCategory, string EventType)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEARLIST", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPELIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNERLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTERLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTHLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@CONSCATID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ConsCategory));
        ParameterList.Add(DataAccess.GetDBParameter("@EVENTTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, EventType));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINCIDENTBYEVENTCAT", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartIncidentAnalysisbyTrend(int? Year,string Vessel,string VesselType,string Fleet,string Principal,string Quarter,string Month,string ConsCategory,string EventType)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEARLIST", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPELIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNERLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTERLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTHLIST", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@CONSCATID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ConsCategory));
        ParameterList.Add(DataAccess.GetDBParameter("@EVENTTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, EventType));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINCIDENTBYTREND", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }
}
