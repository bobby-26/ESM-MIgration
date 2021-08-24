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

public partial class Crew_CrewTravelReportVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar.Show();
        IDictionary<string, string> Params = new Dictionary<string, string>();
        // read filter parameter

        NameValueCollection filter = Filter.CurrentCrewTravelFilter;
        if (filter != null)
        {
            string year = General.GetNullableString(filter.Get("ddlYear"));
            string Quarter = General.GetNullableString(filter.Get("ddlQuarter"));
            string Month = General.GetNullableString(filter.Get("ddlMonth"));
            string Fileno = General.GetNullableString(filter.Get("txtFileNumber"));
            string Passportno = General.GetNullableString(filter.Get("txtPassportno"));
            string RequestionNo = General.GetNullableString((filter.Get("txtRequisition")));
            string Agent = null;
            string Zone = General.GetNullableString(filter.Get("ucZone"));
            string Rank = General.GetNullableString(filter.Get("ucRank"));
            string Designation = General.GetNullableString(filter.Get("ddlDesignation"));
            string Vessel = General.GetNullableString((filter.Get("ucVessel")));
            string Origin = General.GetNullableString(filter.Get("ddlOrigin"));
            string Destination = General.GetNullableString(filter.Get("ddlDestination"));
            string OfficeorCrew = General.GetNullableString(filter.Get("ddlOfficeCrew"));
            string travelReason = General.GetNullableString(filter.Get("ddlTravelreason"));
            string ReasonId = General.GetNullableString(filter.Get("ddlTravelreason"));
            string Ticketno = General.GetNullableString(filter.Get("txtTicketno"));
            string AirlineCode = null;

            // convert them to dictionary
            Params.Add(new KeyValuePair<string, string>("Fileno", Fileno));
            Params.Add(new KeyValuePair<string, string>("Passportno", Passportno));
            Params.Add(new KeyValuePair<string, string>("Requisitionno", RequestionNo));
            Params.Add(new KeyValuePair<string, string>("Ticketno", Ticketno));
            Params.Add(new KeyValuePair<string, string>("Vessel", Vessel));
            Params.Add(new KeyValuePair<string, string>("Rank", Rank));
            Params.Add(new KeyValuePair<string, string>("Agent", Agent));
            Params.Add(new KeyValuePair<string, string>("Designation", Designation));
            Params.Add(new KeyValuePair<string, string>("Zone", Zone));
            Params.Add(new KeyValuePair<string, string>("year", year));
            Params.Add(new KeyValuePair<string, string>("Month", Month));
            Params.Add(new KeyValuePair<string, string>("Quarter", Quarter));
            Params.Add(new KeyValuePair<string, string>("Origin", Origin));
            Params.Add(new KeyValuePair<string, string>("Destination", Destination));
            Params.Add(new KeyValuePair<string, string>("OfficeorCrew", OfficeorCrew));
            Params.Add(new KeyValuePair<string, string>("ReasonId", ReasonId));
            Params.Add(new KeyValuePair<string, string>("travelReason", travelReason));
            Params.Add(new KeyValuePair<string, string>("AirlineCode", AirlineCode));

            


        }
        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);

        string result = PhoenixCrewTravelBI.GetCrewTravelVisualParams();
        string url = Page.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
        Page.ClientScript.RegisterStartupScript(this.GetType(),
            "createChart", String.Format("createChart({0},{1});", result, jsonParams), true);
    }

    // 

    // tabstrip methods
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            Response.Redirect("../Crew/CrewTravelReport.aspx");
        }
    }
    [WebMethod]
    public static string CrewTravelbyYear(string Fileno
                                   , string Passportno
                                   , string Ticketno
                                   , string Requisitionno
                                   , string Vessel
                                   , string Rank
                                   , string Agent
                                   , string Designation
                                   , string Zone
                                   , string year
                                   , string Month
                                   , string Quarter
                                   , string Origin
                                   , string Destination
                                   , string OfficeorCrew
                                   , string ReasonId
                                   )
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@PASSPORTNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Passportno));
            ParameterList.Add(DataAccess.GetDBParameter("@TICKETNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Ticketno));
            ParameterList.Add(DataAccess.GetDBParameter("@REQUISITIONO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Requisitionno));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@AGENTLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Agent));
            ParameterList.Add(DataAccess.GetDBParameter("@DESIGNATIONLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Designation));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@ORIGINID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Origin));
            ParameterList.Add(DataAccess.GetDBParameter("@DESTINATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Destination));
            ParameterList.Add(DataAccess.GetDBParameter("@OFFICEORCREW", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, OfficeorCrew));
            ParameterList.Add(DataAccess.GetDBParameter("@REASONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, ReasonId));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWTRAVELBYYEAR", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewTravelbyQuarter(string Fileno
                                            , string Passportno
                                            , string Ticketno
                                            , string Requisitionno
                                            , string Vessel
                                            , string Rank
                                            , string Agent
                                            , string Designation
                                            , string Zone
                                            , string year
                                            , string Month
                                            , string Quarter
                                            , string Origin
                                            , string Destination
                                            , string OfficeorCrew
                                            , string ReasonId
                                            )
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@PASSPORTNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Passportno));
            ParameterList.Add(DataAccess.GetDBParameter("@TICKETNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Ticketno));
            ParameterList.Add(DataAccess.GetDBParameter("@REQUISITIONNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Requisitionno));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@AGENTLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Agent));
            ParameterList.Add(DataAccess.GetDBParameter("@DESIGNATIONLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Designation));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@ORIGINID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Origin));
            ParameterList.Add(DataAccess.GetDBParameter("@DESTINATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Destination));
            ParameterList.Add(DataAccess.GetDBParameter("@OFFICEORCREW", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, OfficeorCrew));
            ParameterList.Add(DataAccess.GetDBParameter("@REASONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, ReasonId));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWTRAVELBYQUARTER", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewTravelbyMonth(string Fileno
                                         , string Passportno
                                         , string Ticketno
                                         , string Requisitionno
                                         , string Vessel
                                         , string Rank
                                         , string Agent
                                         , string Designation
                                         , string Zone
                                         , string year
                                         , string Month
                                         , string Quarter
                                         , string Origin
                                         , string Destination
                                         , string OfficeorCrew
                                         , string ReasonId
                                         )
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@PASSPORTNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Passportno));
            ParameterList.Add(DataAccess.GetDBParameter("@TICKETNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Ticketno));
            ParameterList.Add(DataAccess.GetDBParameter("@REQUISITIONO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Requisitionno));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@AGENTLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Agent));
            ParameterList.Add(DataAccess.GetDBParameter("@DESIGNATIONLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Designation));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@ORIGINID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Origin));
            ParameterList.Add(DataAccess.GetDBParameter("@DESTINATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Destination));
            ParameterList.Add(DataAccess.GetDBParameter("@OFFICEORCREW", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, OfficeorCrew));
            ParameterList.Add(DataAccess.GetDBParameter("@REASONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, ReasonId));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWTRAVELBYMONTH", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewTravelOnTopRoute(string Fileno
                                            , string Passportno
                                            , string Ticketno
                                            , string Requisitionno
                                            , string Vessel
                                            , string Rank
                                            , string Agent
                                            , string Designation
                                            , string Zone
                                            , string year
                                            , string Month
                                            , string Quarter
                                            , string Origin
                                            , string Destination
                                            , string OfficeorCrew
                                            , string ReasonId
                                            )
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@PASSPORTNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Passportno));
            ParameterList.Add(DataAccess.GetDBParameter("@TICKETNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Ticketno));
            ParameterList.Add(DataAccess.GetDBParameter("@REQUISITIONO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Requisitionno));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@AGENTLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Agent));
            ParameterList.Add(DataAccess.GetDBParameter("@DESIGNATIONLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Designation));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@ORIGINID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Origin));
            ParameterList.Add(DataAccess.GetDBParameter("@DESTINATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Destination));
            ParameterList.Add(DataAccess.GetDBParameter("@OFFICEORCREW", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, OfficeorCrew));
            ParameterList.Add(DataAccess.GetDBParameter("@REASONID", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, ReasonId));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWTRAVELTOPROUTE", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewTravelbyAgent(string Fileno
                                          , string Passportno
                                          , string Ticketno
                                          , string Requisitionno
                                          , string Vessel
                                          , string Rank
                                          , string Agent
                                          , string Designation
                                          , string Zone
                                          , string year
                                          , string Month
                                          , string Quarter
                                          , string Origin
                                          , string Destination
                                          , string ReasonId
                                          , string OfficeorCrew
                                          )
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@PASSPORTNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Passportno));
            ParameterList.Add(DataAccess.GetDBParameter("@TICKETNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Ticketno));
            ParameterList.Add(DataAccess.GetDBParameter("@REQUISITIONO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Requisitionno));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@AGENTLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Agent));
            ParameterList.Add(DataAccess.GetDBParameter("@DESIGNATIONLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Designation));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@ORIGINID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Origin));
            ParameterList.Add(DataAccess.GetDBParameter("@DESTINATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Destination));
            ParameterList.Add(DataAccess.GetDBParameter("@OFFICEORCREW", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, OfficeorCrew));
            ParameterList.Add(DataAccess.GetDBParameter("@REASONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, ReasonId));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWTRAVELBYAGENT", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewTravelAgentDistribution(string Fileno
                                                      , string Passportno
                                                      , string Ticketno
                                                      , string Requisitionno
                                                      , string Vessel
                                                      , string Rank
                                                      , string Agent
                                                      , string Designation
                                                      , string Zone
                                                      , string year
                                                      , string Month
                                                      , string Quarter
                                                      , string Origin
                                                      , string Destination
                                                      , string OfficeorCrew
                                                      , string ReasonId
                                                      )
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@PASSPORTNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Passportno));
            ParameterList.Add(DataAccess.GetDBParameter("@TICKETNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Ticketno));
            ParameterList.Add(DataAccess.GetDBParameter("@REQUISITIONO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Requisitionno));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@AGENTLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Agent));
            ParameterList.Add(DataAccess.GetDBParameter("@DESIGNATIONLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Designation));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@ORIGINID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Origin));
            ParameterList.Add(DataAccess.GetDBParameter("@DESTINATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Destination));
            ParameterList.Add(DataAccess.GetDBParameter("@OFFICEORCREW", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, OfficeorCrew));
            ParameterList.Add(DataAccess.GetDBParameter("@REASONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, ReasonId));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWTRAVELAGENTDISTRIBUTION", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }


    [WebMethod]
    public static string CrewTravelProvidertrends(string Fileno
                                                 , string Passportno
                                                 , string Ticketno
                                                 , string Requisitionno
                                                 , string Vessel
                                                 , string Rank
                                                 , string Agent
                                                 , string Designation
                                                 , string Zone
                                                 , string year
                                                 , string Month
                                                 , string Quarter
                                                 , string Origin
                                                 , string Destination
                                                 , string OfficeorCrew
                                                 )
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@PASSPORTNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Passportno));
            ParameterList.Add(DataAccess.GetDBParameter("@TICKETNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Ticketno));
            ParameterList.Add(DataAccess.GetDBParameter("@REQUISITIONO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Requisitionno));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@AGENTLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Agent));
            ParameterList.Add(DataAccess.GetDBParameter("@DESIGNATIONLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Designation));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            //ParameterList.Add(DataAccess.GetDBParameter("@ORIGINID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Origin));
            //ParameterList.Add(DataAccess.GetDBParameter("@DESTINATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Destination));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWTRAVELPROVIDERSTREND", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewTravelAirlineAnalysis(string Fileno
                                                  , string Passportno
                                                  , string Ticketno
                                                  , string Requisitionno
                                                  , string Vessel
                                                  , string Rank
                                                  , string Agent
                                                  , string Designation
                                                  , string Zone
                                                  , string year
                                                  , string Month
                                                  , string Quarter
                                                  , string Origin
                                                  , string Destination
                                                  , string OfficeorCrew
                                                  , string ReasonId
                                                  )
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@PASSPORTNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Passportno));
            ParameterList.Add(DataAccess.GetDBParameter("@TICKETNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Ticketno));
            ParameterList.Add(DataAccess.GetDBParameter("@REQUISITIONO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Requisitionno));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@AGENTLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Agent));
            ParameterList.Add(DataAccess.GetDBParameter("@DESIGNATIONLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Designation));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@ORIGINID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Origin));
            ParameterList.Add(DataAccess.GetDBParameter("@DESTINATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Destination));
            ParameterList.Add(DataAccess.GetDBParameter("@OFFICEORCREW", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, OfficeorCrew));
            ParameterList.Add(DataAccess.GetDBParameter("@REASONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, ReasonId));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWTRAVELAIRLINEANALYSIS", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewTravelTicketCancelledYN(string Fileno
                                                , string Passportno
                                                , string Ticketno
                                                , string Requisitionno
                                                , string Vessel
                                                , string Rank
                                                , string Agent
                                                , string Designation
                                                , string Zone
                                                , string year
                                                , string Month
                                                , string Quarter
                                                , string Origin
                                                , string Destination
                                                , string OfficeorCrew
                                                , string ReasonId
                                                )
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@PASSPORTNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Passportno));
            ParameterList.Add(DataAccess.GetDBParameter("@TICKETNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Ticketno));
            ParameterList.Add(DataAccess.GetDBParameter("@REQUISITIONO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Requisitionno));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@AGENTLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Agent));
            ParameterList.Add(DataAccess.GetDBParameter("@DESIGNATIONLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Designation));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@ORIGINID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Origin));
            ParameterList.Add(DataAccess.GetDBParameter("@DESTINATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Destination));
            ParameterList.Add(DataAccess.GetDBParameter("@OFFICEORCREW", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, OfficeorCrew));
            ParameterList.Add(DataAccess.GetDBParameter("@REASONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, ReasonId));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWTRAVELTICKETCANCELLEDYN", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }



    [WebMethod]
    public static string CrewTravelAirlineExpences(string Fileno
                                                        , string Passportno
                                                        , string Ticketno
                                                        , string Requisitionno
                                                        , string Vessel
                                                        , string Rank
                                                        , string Agent
                                                        , string Designation
                                                        , string Zone
                                                        , string year
                                                        , string Month
                                                        , string Quarter
                                                        , string Origin
                                                        , string Destination
                                                        , string ReasonId
                                                        , string OfficeorCrew
                                                        , string AirlineCode
                                                        )
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@PASSPORTNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Passportno));
            ParameterList.Add(DataAccess.GetDBParameter("@TICKETNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Ticketno));
            ParameterList.Add(DataAccess.GetDBParameter("@REQUISITIONO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Requisitionno));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@AGENTLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Agent));
            ParameterList.Add(DataAccess.GetDBParameter("@DESIGNATIONLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Designation));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@ORIGINID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Origin));
            ParameterList.Add(DataAccess.GetDBParameter("@DESTINATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Destination));
            ParameterList.Add(DataAccess.GetDBParameter("@REASONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, ReasonId));
            ParameterList.Add(DataAccess.GetDBParameter("@OFFICEORCREW", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, OfficeorCrew));
            ParameterList.Add(DataAccess.GetDBParameter("@AIRLINE", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, AirlineCode));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWTRAVELAIRLINEEXPENSES", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewTravelAirlineDetails(string Fileno
                                                    , string Passportno
                                                    , string Ticketno
                                                    , string Requisitionno
                                                    , string Vessel
                                                    , string Rank
                                                    , string Agent
                                                    , string Designation
                                                    , string Zone
                                                    , string year
                                                    , string Month
                                                    , string Quarter
                                                    , string Origin
                                                    , string Destination
                                                    , string ReasonId
                                                    , string OfficeorCrew
                                                    )
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@PASSPORTNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Passportno));
            ParameterList.Add(DataAccess.GetDBParameter("@TICKETNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Ticketno));
            ParameterList.Add(DataAccess.GetDBParameter("@REQUISITIONO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Requisitionno));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@AGENTLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Agent));
            ParameterList.Add(DataAccess.GetDBParameter("@DESIGNATIONLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Designation));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@ORIGINID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Origin));
            ParameterList.Add(DataAccess.GetDBParameter("@DESTINATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Destination));
            ParameterList.Add(DataAccess.GetDBParameter("@REASONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, ReasonId));
            ParameterList.Add(DataAccess.GetDBParameter("@OFFICEORCREW", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, OfficeorCrew));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWTRAVELAIRLINEEXPENSESDETAIL", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewTravelbyZone(string Fileno
                                         , string Passportno
                                         , string Ticketno
                                         , string Requisitionno
                                         , string Vessel
                                         , string Rank
                                         , string Agent
                                         , string Designation
                                         , string Zone
                                         , string year
                                         , string Month
                                         , string Quarter
                                         , string Origin
                                         , string Destination
                                         , string OfficeorCrew
                                         , string ReasonId
                                         )
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@PASSPORTNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Passportno));
            ParameterList.Add(DataAccess.GetDBParameter("@TICKETNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Ticketno));
            ParameterList.Add(DataAccess.GetDBParameter("@REQUISITIONO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Requisitionno));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@AGENTLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Agent));
            ParameterList.Add(DataAccess.GetDBParameter("@DESIGNATIONLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Designation));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@ORIGINID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Origin));
            ParameterList.Add(DataAccess.GetDBParameter("@DESTINATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Destination));
            ParameterList.Add(DataAccess.GetDBParameter("@OFFICEORCREW", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, OfficeorCrew));
            ParameterList.Add(DataAccess.GetDBParameter("@REASONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, ReasonId));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWTRAVELBYZONE", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    [WebMethod]
    public static string CrewTravelbyReason(string Fileno
                                             , string Passportno
                                             , string Ticketno
                                             , string Requisitionno
                                             , string Vessel
                                             , string Rank
                                             , string Agent
                                             , string Designation
                                             , string Zone
                                             , string year
                                             , string Month
                                             , string Quarter
                                             , string Origin
                                             , string Destination
                                             , string OfficeorCrew
                                             , string ReasonId
                                            )
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();

            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@PASSPORTNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Passportno));
            ParameterList.Add(DataAccess.GetDBParameter("@TICKETNO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Ticketno));
            ParameterList.Add(DataAccess.GetDBParameter("@REQUISITIONO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Requisitionno));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@AGENTLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Agent));
            ParameterList.Add(DataAccess.GetDBParameter("@DESIGNATIONLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Designation));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, year));
            ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Month));
            ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Quarter));
            ParameterList.Add(DataAccess.GetDBParameter("@ORIGINID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Origin));
            ParameterList.Add(DataAccess.GetDBParameter("@DESTINATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Destination));
            ParameterList.Add(DataAccess.GetDBParameter("@OFFICEORCREW", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, OfficeorCrew));
            ParameterList.Add(DataAccess.GetDBParameter("@REASONID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, ReasonId));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWTRAVELBYREASON", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }



}