using Newtonsoft.Json;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Inspection_InspectionWorkRestHourNCAnalysisVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT");
        MenuReportsFilter.MenuList = toolbar.Show();

        // read filter parameter
        NameValueCollection filter = InspectionFilter.CurrentWRHNcAnalysisFilter;
        string Year = General.GetNullableString(filter.Get("ddlYear"));
        string Quarter = General.GetNullableString(filter.Get("Quarter"));
        string Month = General.GetNullableString(filter.Get("Month"));
        string Fleet = General.GetNullableString(filter.Get("ucFleet"));
        string Principal = General.GetNullableString(filter.Get("ucPrincipal"));
        string Vessel = General.GetNullableString((filter.Get("ucVessel")));
        string VesselType = General.GetNullableString((filter.Get("ucVesselType")));
        string Rank = General.GetNullableString(filter.Get("ucRank"));

        // convert them to dictionary
        IDictionary<string, string> Params = new Dictionary<string, string>();
        Params.Add(new KeyValuePair<string, string>("Year", Year));
        Params.Add(new KeyValuePair<string, string>("Quarter", Quarter));
        Params.Add(new KeyValuePair<string, string>("Month", Month));
        Params.Add(new KeyValuePair<string, string>("Fleet", Fleet));
        Params.Add(new KeyValuePair<string, string>("Principal", Principal));
        Params.Add(new KeyValuePair<string, string>("Vessel", Vessel));
        Params.Add(new KeyValuePair<string, string>("VesselType", VesselType));
        Params.Add(new KeyValuePair<string, string>("Rank", Rank));
        Params.Add(new KeyValuePair<string, string>("RankGroup", null));
        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);

        string result = PhoenixInspectionWorkRestHourNCAnalysis.GetWorkrestHourNCAnalysisVisualParams();
        string url = Page.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
        Page.ClientScript.RegisterStartupScript(this.GetType(),
            "createChart", String.Format("createChart({0},{1});", result, jsonParams), true);
    }

    // 

    // tabstrip methods
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            Response.Redirect("../Inspection/InspectionWorkRestHourNCAnalysis.aspx");
        }

    }

    [WebMethod]
    public static string QualityChartWRHNCAnalysisYearwise(string Year, string Quarter, string Month, string Vessel, string VesselType, string Principal, string Fleet, string Rank)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYWRHANALYSISYEARWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        return result;
    }

    [WebMethod]
    public static string QualityChartWRHNCAnalysisQuarterwise(string Year, string Quarter, string Month, string Vessel, string VesselType, string Principal, string Fleet, string Rank)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYWRHANALYSISQUARTERWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartWRHNCAnalysisMonthwise(string Year, string Quarter, string Month, string Vessel, string VesselType, string Principal, string Fleet, string Rank)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYWRHANALYSISMONTHWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartWRHNCAnalysisFleetwise(string Year, string Quarter, string Month, string Vessel, string VesselType, string Principal, string Fleet, string Rank)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYWRHANALYSISFLEETWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartWRHNCAnalysisVesselTypewise(string Year, string Quarter, string Month, string Vessel, string VesselType, string Principal, string Fleet, string Rank)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYWRHANALYSISVESSELTYPEWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartWRHNCAnalysisOwnerwise(string Year, string Quarter, string Month, string Vessel, string VesselType, string Principal, string Fleet, string Rank)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYWRHANALYSISOWNERWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartWRHNCAnalysisVesselwise(string Year, string Quarter, string Month, string Vessel, string VesselType, string Principal, string Fleet, string Rank)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYWRHANALYSISVESSELWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartWRHNCAnalysisRankGroupwise(string Year, string Quarter, string Month, string Vessel, string VesselType, string Principal, string Fleet, string Rank)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYWRHANALYSISDEPARTMENTWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartWRHNCAnalysisRankwise(string Year, string Quarter, string Month, string Vessel, string VesselType, string Principal, string Fleet, string Rank, string RankGroup)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@OWNER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Principal));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselType));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));
        ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, RankGroup));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYWRHANALYSISRANKWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }
}