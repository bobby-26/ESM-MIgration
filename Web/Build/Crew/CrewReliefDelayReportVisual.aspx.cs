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
using Telerik.Web.UI;
public partial class Crew_CrewReliefDelayReportVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar.Show();

        IDictionary<string, string> Params = new Dictionary<string, string>();
        // read filter parameter

        if (Filter.CurrentCrewReliefDelayFilter != null)
        {
            NameValueCollection filter = Filter.CurrentCrewReliefDelayFilter;
            string FileNo = General.GetNullableString((filter.Get("txtFileNumber")));
            string Year = General.GetNullableString(filter.Get("ddlYear"));
            string Quarter = General.GetNullableString(filter.Get("ddlQuarter"));
            string Month = General.GetNullableString(filter.Get("ddlMonth"));
            string Principal = General.GetNullableString(filter.Get("ucPrincipal"));
            string Vessel = General.GetNullableString((filter.Get("ucVessel")));
            string Rank = General.GetNullableString(filter.Get("ucRank"));
            string Zone = General.GetNullableString(filter.Get("ucZone"));
            string Pool = General.GetNullableString(filter.Get("ucPool"));
            string NoofDelayed = General.GetNullableString(filter.Get("NoofDelayed"));
            string VesselType = General.GetNullableString(filter.Get("VesselType"));
            string ReasonId = General.GetNullableString(filter.Get("//"));


            // convert them to dictionary
            Params.Add(new KeyValuePair<string, string>("Fileno", FileNo));
            Params.Add(new KeyValuePair<string, string>("Year", Year));
            Params.Add(new KeyValuePair<string, string>("Month", Month));
            Params.Add(new KeyValuePair<string, string>("Quarter", Quarter));
            Params.Add(new KeyValuePair<string, string>("Principal", Principal));
            Params.Add(new KeyValuePair<string, string>("Vessel", Vessel));
            Params.Add(new KeyValuePair<string, string>("Zone", Zone));
            Params.Add(new KeyValuePair<string, string>("Pool", Pool));
            Params.Add(new KeyValuePair<string, string>("Rank", Rank));
           
            Params.Add(new KeyValuePair<string, string>("NoofDelayed", NoofDelayed));
            Params.Add(new KeyValuePair<string, string>("VesselType", VesselType));
            Params.Add(new KeyValuePair<string, string>("ReasonId", ReasonId));
        }
        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);

        string result = PhoenixCrewReliefDelayBI.GetCrewReliefDelayVisualParams();
        string url = Page.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
        Page.ClientScript.RegisterStartupScript(this.GetType(),
            "createChart", String.Format("createChart({0},{1});", result, jsonParams), true);
    }

    // tabstrip methods
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            Response.Redirect("../Crew/CrewReliefDelayReport.aspx");
        }
    }
    // web methods
    [WebMethod]
    public static string CrewReliefDelayedRankGroup(string Fileno, 
                                                    string Year,
                                                    string Month,
                                                    string Quarter,
                                                    string Principal,
                                                    string Vessel,
                                                    string Zone,
                                                    string Pool,
                                                    string Rank)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Rank));
            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWRELIEFOFFICERSRANKGROUP", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewReliefDelayedbyYear(string Fileno, string Year, string Month, string Quarter, string Principal, string Vessel, string Zone, string Pool, string Rank, string RankGroupId)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUPID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, RankGroupId));
            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWRELIEFOFFICERSDELAYED", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewReliefDelayedRank(string Fileno, string Year, string Month, string Quarter, string Principal, string Vessel, string Zone, string Pool, string Rank)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Rank));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWRELIEFOFFICERSRANK", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewReliefDelayedNoofDays(string Fileno, string Year, string Month, string Quarter, string Principal, string Vessel, string VesselType, string Zone, string Pool, string Rank)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, VesselType));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Pool));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWRELIEFOFFICERSNOOFDAYSDELAYED", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewReliefDelayedbyQuarter(string Fileno, string Year, string Month, string Quarter, string Principal, string Vessel, string Zone, string Pool, string Rank)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Rank));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWRELIEFOFFICERSDELAYEDQUARTER", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string CrewReliefDelayedbyMonth(string Fileno, string Year, string Month, string Quarter, string Principal, string Vessel, string Zone, string Pool, string Rank)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Rank));
            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWRELIEFOFFICERSDELAYEDMONTH", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string CrewReliefDelayedEmpDetails(string Fileno, string Year, string Month, string Quarter, string Principal, string Vessel, string VesselType, string Zone, string Pool, string Rank, string ReasonId, string NoofDelayed)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, VesselType));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@REASONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, ReasonId));
            ParameterList.Add(DataAccess.GetDBParameter("@DELAYEDID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, NoofDelayed));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWRELIEFEMPDETAIL", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewReliefDelayedbyReason(string Fileno, string Year, string Month, string Quarter, string Principal, string Vessel, string VesselType, string Zone, string Pool, string Rank, string NoofDelayed)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, VesselType));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@DELAYEDID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, NoofDelayed));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWRELIEFDELAYBYREASON", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewReliefMostDelayedbyVesselType(string Fileno, string Year, string Month, string Quarter, string Principal, string Vessel, string Zone, string Pool, string Rank, string NoofDelayed)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@DELAYEDID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, NoofDelayed));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWRELIEFBYVESSELTYPE", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
}