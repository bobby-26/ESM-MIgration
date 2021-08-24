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
public partial class Crew_CrewContractReportVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar.Show();

        // read filter parameter

        NameValueCollection filter = Filter.CurrentCrewContractFilter;
        // convert them to dictionary
        IDictionary<string, string> Params = new Dictionary<string, string>();

        if (filter != null)
        {
            string Principal = General.GetNullableString(filter.Get("ucPrincipal"));
            string Vessel = General.GetNullableString((filter.Get("ucVessel")));
            string FileNo = General.GetNullableString((filter.Get("txtFileNumber")));
            string Rank = General.GetNullableString(filter.Get("ucRank"));
            string Zone = General.GetNullableString(filter.Get("ucZone"));
            string Year = General.GetNullableString(filter.Get("ddlYear"));
            string Pool = General.GetNullableString(filter.Get("ucPool"));
            string Reason = General.GetNullableString(filter.Get("ddlReason"));

            Params.Add(new KeyValuePair<string, string>("Fileno", FileNo));
            Params.Add(new KeyValuePair<string, string>("Year", Year));
            Params.Add(new KeyValuePair<string, string>("Principal", Principal));
            Params.Add(new KeyValuePair<string, string>("Vessel", Vessel));
            Params.Add(new KeyValuePair<string, string>("Zone", Zone));
            Params.Add(new KeyValuePair<string, string>("Pool", Pool));
            Params.Add(new KeyValuePair<string, string>("Rank", Rank));
            Params.Add(new KeyValuePair<string, string>("RankGroupId", null));
            Params.Add(new KeyValuePair<string, string>("Reason", Reason));
        }

        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);

        string result = PhoenixCrewContractBI.GetCrewContractVisualParams();
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
            Response.Redirect("../Crew/CrewContractReport.aspx");
        }

    }

    [WebMethod]
    public static string CrewContractRankGroup(string Fileno, string Year, string Principal, string Vessel, string Zone, string Pool, string Rank, string RankGroupId, string Reason)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@REASON", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Reason));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWCONTRACTRANKGROUP", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string CrewContractRank(string Fileno, string Year, string Principal, string Vessel, string Zone, string Pool, string Rank, string RankGroupId, string Reason)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUPID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, RankGroupId));
            ParameterList.Add(DataAccess.GetDBParameter("@REASON", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Reason));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWCONTRACTRANK", ParameterList);

           return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string CrewContarctbyZone(string Fileno, string Year, string Principal, string Vessel, string Zone, string Pool, string Rank, string RankGroupId, string Reason)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUPID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, RankGroupId));
            ParameterList.Add(DataAccess.GetDBParameter("@REASON", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Reason));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWCONTRACTBYZONE", ParameterList);

           return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string CrewCompletedContractsbyPrincipal(string Fileno, string Year, string Principal, string Vessel, string Zone, string Pool, string Rank, string RankGroupId, string Reason)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUPID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, RankGroupId));
            ParameterList.Add(DataAccess.GetDBParameter("@REASON", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Reason));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWCOMPLETEDCONTRACTBYPRINCIPAL", ParameterList);

           return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string CrewShortContractsbyPrincipal(string Fileno, string Year, string Principal, string Vessel, string Zone, string Pool, string Rank, string RankGroupId, string Reason)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUPID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, RankGroupId));
            ParameterList.Add(DataAccess.GetDBParameter("@REASON", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Reason));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWSHORTCONTRACTBYPRINCIPAL", ParameterList);

           return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string CrewContractsEarlyReliefbyPrincipal(string Fileno, string Year, string Principal, string Vessel, string Zone, string Pool, string Rank, string RankGroupId, string Reason)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUPID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, RankGroupId));
            ParameterList.Add(DataAccess.GetDBParameter("@REASON", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Reason));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWCONTRACTEARLYRELIEFBYPRINCIPAL", ParameterList);

           return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string CrewContractsEarlyReliefbyReason(string Fileno, string Year, string Principal, string Vessel, string Zone, string Pool, string Rank, string RankGroupId, string Reason)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUPID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, RankGroupId));
            ParameterList.Add(DataAccess.GetDBParameter("@REASON", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Reason));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWCONTRACTEARLYRELIEF", ParameterList);

           return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string CrewContractsMinMonthbyPrincipal(string Fileno, string Year, string Principal, string Vessel, string Zone, string Pool, string Rank, string RankGroupId, string Reason)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUPID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, RankGroupId));
            ParameterList.Add(DataAccess.GetDBParameter("@REASON", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Reason));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTBELOWFOURMONTHCONTRACTBYPRINCIPAL", ParameterList);

           return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string CrewContarctIssuedWithOutPlusMinusbyPrincipal(string Fileno, string Year, string Principal, string Vessel, string Zone, string Pool, string Rank, string RankGroupId, string Reason)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUPID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, RankGroupId));
            ParameterList.Add(DataAccess.GetDBParameter("@REASON", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Reason));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTWITHOUTPLUMINUSBYPRINCIPAL", ParameterList);

           return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string CrewContarctbyPrincipal(string Fileno, string Year, string Principal, string Vessel, string Zone, string Pool, string Rank, string RankGroupId, string Reason)
    {
        try
        {
            DataSet ds = new DataSet();

            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@FILENO", SqlDbType.VarChar, DbConstant.VARCHAR_100, ParameterDirection.Input, Fileno));
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPALLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Principal));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Vessel));
            ParameterList.Add(DataAccess.GetDBParameter("@ZONELIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Zone));
            ParameterList.Add(DataAccess.GetDBParameter("@POOLLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Pool));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKLIST", SqlDbType.VarChar, DbConstant.VARCHAR_500, ParameterDirection.Input, Rank));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUPID", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, RankGroupId));
            ParameterList.Add(DataAccess.GetDBParameter("@REASON", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, Reason));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWCONTRACTBYPRINCIPAL", ParameterList);

           return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}