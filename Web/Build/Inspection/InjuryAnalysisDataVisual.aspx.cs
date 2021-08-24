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


public partial class Inspection_InjuryAnalysisDataVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Visual", "SHOWVISUAL", ToolBarDirection.Right);
        toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar.Show();
        MenuReportsFilter.SelectedMenuIndex = 0;

        NameValueCollection Filter = InspectionFilter.CurrentInspectionInjuryAnalysisFilter;

        string Year = General.GetNullableString(Filter.Get("ddlYear").ToString());
        string Quarter = General.GetNullableString(Filter.Get("lstQuarter").ToString());
        string Month = General.GetNullableString(Filter.Get("lstMonth").ToString());
        string Fleet = General.GetNullableString(Filter.Get("ucFleet").ToString());
        string Vessel = General.GetNullableString(Filter.Get("ucVessel").ToString());
        string Rank = General.GetNullableString(Filter.Get("ucRank").ToString());

        IDictionary<string, string> Params = new Dictionary<string, string>();
      
        Params.Add(new KeyValuePair<string, string>("Year", Year));
        Params.Add(new KeyValuePair<string, string>("Fleet", Fleet));
        Params.Add(new KeyValuePair<string, string>("Vessel", Vessel));
        Params.Add(new KeyValuePair<string, string>("Rank", Rank));
        Params.Add(new KeyValuePair<string, string>("Quarter", Quarter));
        Params.Add(new KeyValuePair<string, string>("Month", Month));

        Params.Add(new KeyValuePair<string, string>("SimsYN", null));
        Params.Add(new KeyValuePair<string, string>("DayNight", null));
        Params.Add(new KeyValuePair<string, string>("AgeRangeId", null));
        Params.Add(new KeyValuePair<string, string>("VesselTypeId", null));
        Params.Add(new KeyValuePair<string, string>("TypeofInjuryId", null));
        Params.Add(new KeyValuePair<string, string>("RankGroupId", null));
        Params.Add(new KeyValuePair<string, string>("PartoftheBodyId", null));
        Params.Add(new KeyValuePair<string, string>("LocationId", null));
        Params.Add(new KeyValuePair<string, string>("ConCategoryId", null));
        
        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);

        string result = PhoenixInspectionInjuryAnalysis.GetInjuryAnalysisDataVisualParams();
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
            Response.Redirect("../Inspection/InjuryAnalysis.aspx");
        }

        if (CommandName.ToUpper().Equals("SHOWVISUAL"))
        {
            Response.Redirect("../Inspection/InjuryAnalysisInjuryTypesVisual.aspx");
        }
    }

   
    [WebMethod]
    public static string QualityChartInjuryAnalysisYearwise(string Year,string Quarter,string Month,string Fleet,string Vessel,string Rank)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINJURYANALYSISYEARRWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;

    }

    [WebMethod]
    public static string QualityChartInjuryAnalysisQuarterwise(string Year, string Quarter, string Month, string Fleet, string Vessel, string Rank)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINJURYANALYSISQUARTERWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartInjuryAnalysisMonthwise(string Year, string Quarter, string Month, string Fleet, string Vessel, string Rank)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINJURYANALYSISMONTHWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartInjuryAnalysisConCategory(string Year, string Quarter, string Month, string Fleet, string Vessel, string Rank)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINJURYANALYSISCONCATEGORY", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartInjuryAnalysisTypeofInjury(string Year, string Quarter, string Month, string Fleet, string Vessel, string Rank,string ConCategoryId,string VesselTypeId)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));
        ParameterList.Add(DataAccess.GetDBParameter("@CONCATEGORY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ConCategoryId));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselTypeId));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINJURYANALYSISTYPEOFINJURY", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartInjuryAnalysisPartoftheBody(string Year, string Quarter, string Month, string Fleet, string Vessel, string Rank, string ConCategoryId, string VesselTypeId,string TypeofInjuryId)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));
        ParameterList.Add(DataAccess.GetDBParameter("@TYPEOFINJURY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, TypeofInjuryId));
        ParameterList.Add(DataAccess.GetDBParameter("@CONCATEGORY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ConCategoryId));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselTypeId));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINJURYANALYSISBODYPART", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartInjuryAnalysisLocation(string Year, string Quarter, string Month, string Fleet, string Vessel, string Rank, string ConCategoryId, string VesselTypeId, string TypeofInjuryId,string PartoftheBodyId)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));
        ParameterList.Add(DataAccess.GetDBParameter("@TYPEOFINJURY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, TypeofInjuryId));
        ParameterList.Add(DataAccess.GetDBParameter("@PARTOFTHEBODY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PartoftheBodyId));
        ParameterList.Add(DataAccess.GetDBParameter("@CONCATEGORY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ConCategoryId));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselTypeId));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINJURYANALYSISLOCATION", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartInjuryAnalysisRankGroup(string Year, string Quarter, string Month, string Fleet, string Vessel, string Rank, string ConCategoryId, string VesselTypeId, string TypeofInjuryId, string PartoftheBodyId,string LocationId)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));
        ParameterList.Add(DataAccess.GetDBParameter("@TYPEOFINJURY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, TypeofInjuryId));
        ParameterList.Add(DataAccess.GetDBParameter("@PARTOFTHEBODY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PartoftheBodyId));
        ParameterList.Add(DataAccess.GetDBParameter("@LOCATION", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, LocationId));
        ParameterList.Add(DataAccess.GetDBParameter("@CONCATEGORY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ConCategoryId));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselTypeId));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINJURYANALYSISRANKGROUP", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartInjuryAnalysisRank(string Year, string Quarter, string Month, string Fleet, string Vessel, string Rank, string ConCategoryId, string VesselTypeId, string TypeofInjuryId,string RankGroupId, string PartoftheBodyId, string LocationId)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));
        ParameterList.Add(DataAccess.GetDBParameter("@TYPEOFINJURY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, TypeofInjuryId));
        ParameterList.Add(DataAccess.GetDBParameter("@PARTOFTHEBODY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PartoftheBodyId));
        ParameterList.Add(DataAccess.GetDBParameter("@LOCATION", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, LocationId));
        ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, RankGroupId));
        ParameterList.Add(DataAccess.GetDBParameter("@CONCATEGORY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ConCategoryId));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselTypeId));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINJURYANALYSISRANK", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartInjuryAnalysisAgewiseCount(string Year, string Quarter, string Month, string Fleet, string Vessel, string Rank, string ConCategoryId, string VesselTypeId, string TypeofInjuryId, string RankGroupId, string PartoftheBodyId, string LocationId)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));
        ParameterList.Add(DataAccess.GetDBParameter("@TYPEOFINJURY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, TypeofInjuryId));
        ParameterList.Add(DataAccess.GetDBParameter("@PARTOFTHEBODY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PartoftheBodyId));
        ParameterList.Add(DataAccess.GetDBParameter("@LOCATION", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, LocationId));
        ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, RankGroupId));
        ParameterList.Add(DataAccess.GetDBParameter("@CONCATEGORY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ConCategoryId));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselTypeId));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINJURYANALYSISAGETRANGE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartInjuryAnalysisDayNight(string Year, string Quarter, string Month, string Fleet, string Vessel, string Rank,string AgeRangeId, string ConCategoryId, string VesselTypeId, string TypeofInjuryId, string RankGroupId, string PartoftheBodyId, string LocationId)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));
        ParameterList.Add(DataAccess.GetDBParameter("@TYPEOFINJURY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, TypeofInjuryId));
        ParameterList.Add(DataAccess.GetDBParameter("@PARTOFTHEBODY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PartoftheBodyId));
        ParameterList.Add(DataAccess.GetDBParameter("@LOCATION", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, LocationId));
        ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, RankGroupId));
        ParameterList.Add(DataAccess.GetDBParameter("@AGERANGEID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, AgeRangeId));
        ParameterList.Add(DataAccess.GetDBParameter("@CONCATEGORY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ConCategoryId));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselTypeId));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINJURYANALYSISDAYNIGHT", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartInjuryAnalysisSimsyn(string Year, string Quarter, string Month, string Fleet, string Vessel, string Rank, string DayNight,string AgeRangeId, string ConCategoryId, string VesselTypeId, string TypeofInjuryId, string RankGroupId, string PartoftheBodyId, string LocationId)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));
        ParameterList.Add(DataAccess.GetDBParameter("@TYPEOFINJURY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, TypeofInjuryId));
        ParameterList.Add(DataAccess.GetDBParameter("@PARTOFTHEBODY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PartoftheBodyId));
        ParameterList.Add(DataAccess.GetDBParameter("@LOCATION", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, LocationId));
        ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, RankGroupId));
        ParameterList.Add(DataAccess.GetDBParameter("@AGERANGEID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, AgeRangeId));
        ParameterList.Add(DataAccess.GetDBParameter("@DAYNIGHT", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, DayNight));
        ParameterList.Add(DataAccess.GetDBParameter("@CONCATEGORY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ConCategoryId));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselTypeId));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINJURYANALYSISSIMSYN", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }

    [WebMethod]
    public static string QualityChartInjuryAnalysisServiceYears(string Year, string Quarter, string Month, string Fleet, string Vessel, string Rank,string SimsYN, string DayNight, string AgeRangeId, string ConCategoryId, string VesselTypeId, string TypeofInjuryId, string RankGroupId, string PartoftheBodyId, string LocationId)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();

        
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEET", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Rank));
        ParameterList.Add(DataAccess.GetDBParameter("@TYPEOFINJURY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, TypeofInjuryId));
        ParameterList.Add(DataAccess.GetDBParameter("@PARTOFTHEBODY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PartoftheBodyId));
        ParameterList.Add(DataAccess.GetDBParameter("@LOCATION", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, LocationId));
        ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, RankGroupId));
        ParameterList.Add(DataAccess.GetDBParameter("@AGERANGEID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, AgeRangeId));
        ParameterList.Add(DataAccess.GetDBParameter("@DAYNIGHT", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, DayNight));
        ParameterList.Add(DataAccess.GetDBParameter("@SIMSYN", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, SimsYN));
        ParameterList.Add(DataAccess.GetDBParameter("@CONCATEGORY", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ConCategoryId));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselTypeId));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTQUALITYINJURYANALYSISSERVICERANGE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[1]);
        return result;
    }
}
