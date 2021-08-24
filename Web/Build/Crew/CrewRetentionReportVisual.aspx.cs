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
public partial class Crew_CrewRetentionReportVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar.Show();

        // read filter parameter
        // convert them to dictionary
        NameValueCollection filter = Filter.CurrentCrewRetentionFilter;
        IDictionary<string, string> Params = new Dictionary<string, string>();
        if (filter != null)
        {
            string FromDate = General.GetNullableString(filter.Get("txtFrom"));
            string ToDate = General.GetNullableString(filter.Get("txtTo"));
            string Year = General.GetNullableString(filter.Get("ddlYear"));
            string RankId = General.GetNullableString(filter.Get("ucRank"));
            string Principal = General.GetNullableString(filter.Get("ucPrincipal"));
            string VesselTypeId = General.GetNullableString(filter.Get("ucVesselType"));

            Params.Add(new KeyValuePair<string, string>("Year", Year));
            Params.Add(new KeyValuePair<string, string>("FromDate", FromDate));
            Params.Add(new KeyValuePair<string, string>("ToDate", ToDate));
            Params.Add(new KeyValuePair<string, string>("Principal", Principal));
            Params.Add(new KeyValuePair<string, string>("RankId", RankId));
            Params.Add(new KeyValuePair<string, string>("VesselTypeId", VesselTypeId));
            Params.Add(new KeyValuePair<string, string>("OwnerId", null));
            Params.Add(new KeyValuePair<string, string>("RankGroupId", null));
        }
        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);

        string result = PhoenixCrewRetentionBI.GetCrewRetentionVisualParams();
        string url = Page.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
        Page.ClientScript.RegisterStartupScript(this.GetType(),
            "createChart", String.Format("createChart({0},{1});", result, jsonParams), true);
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            Response.Redirect("../Crew/CrewRetentionReport.aspx");
        }
    }
    [WebMethod]
    public static string CrewRetentionAvgLengthbyRankGroup(string Year,
                                            string FromDate,
                                            string ToDate,
                                            string VesselTypeId,
                                            string OwnerId,
                                            string RankGroupId,
                                            string RankId)
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselTypeId));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPAL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, OwnerId));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, RankGroupId));
            ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, RankId));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWRETENTIONAVGLENBYRANKGROUP", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    [WebMethod]
    public static string CrewRetentionAvgLengthbyRank(string Year,
                                                        string FromDate,
                                                        string ToDate,
                                                        string VesselTypeId,
                                                        string OwnerId,
                                                        string RankGroupId,
                                                        string RankId
                                                        )
    {
        try
        {
            DataSet ds = new DataSet();
            List<SqlParameter> ParameterList = new List<SqlParameter>();
            ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
            ParameterList.Add(DataAccess.GetDBParameter("@FROMDATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, FromDate));
            ParameterList.Add(DataAccess.GetDBParameter("@TODATE", SqlDbType.DateTime, DbConstant.DATETIME, ParameterDirection.Input, ToDate));
            ParameterList.Add(DataAccess.GetDBParameter("@VESSELTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VesselTypeId));
            ParameterList.Add(DataAccess.GetDBParameter("@PRINCIPAL", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, OwnerId));
            ParameterList.Add(DataAccess.GetDBParameter("@RANKGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, RankGroupId));
            ParameterList.Add(DataAccess.GetDBParameter("@RANK", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, RankId));

            ds = DataAccess.ExecSPReturnDataSet("PRCHARTCREWRETENTIONAVGLENBYRANK", ParameterList);

            return General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}