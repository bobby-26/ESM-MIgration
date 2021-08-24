using Newtonsoft.Json;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class PurchaseVendorperformanceDataVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
        toolbar.AddButton("PO Amount Visual", "SHOWVISUAL",ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar.Show();

        NameValueCollection Filter = filterPurchase.CurrentVendorPerformanceFilter;

        string Year = General.GetNullableString(Filter.Get("ddlYear").ToString());
        string Fleet = General.GetNullableString(Filter.Get("ucFleet").ToString());
        string StockType = General.GetNullableString(Filter.Get("ddlType").ToString());
        string StockTypeName = General.GetNullableString(Filter.Get("ddlTypeName").ToString());
        string Vessel = General.GetNullableString(Filter.Get("ucVessel").ToString());
        string VendorID = General.GetNullableString(Filter.Get("txtVendorID").ToString());

        string VendorName = General.GetNullableString(Filter.Get("txtVendorName").ToString());
        string VendorCode = General.GetNullableString(Filter.Get("txtVendorCode").ToString());

        string Quarter = General.GetNullableString(Filter.Get("Quarter").ToString());
        string Month = General.GetNullableString(Filter.Get("Month").ToString());

        //if (StockType == "" || StockType == null)
        //{
        //    ucTitle.Text = "Vendor Performance";
        //}
        //else
        //{
        //    ucTitle.Text = "Vendor Performance - " + StockTypeName;
        //}

        IDictionary<string, string> Params = new Dictionary<string, string>();
        Params.Add(new KeyValuePair<string, string>("Year", Year));
        Params.Add(new KeyValuePair<string, string>("VendorID", VendorID));
        Params.Add(new KeyValuePair<string, string>("Fleet", Fleet));
        Params.Add(new KeyValuePair<string, string>("Vessel", Vessel));
        Params.Add(new KeyValuePair<string, string>("StockType", StockType));
        Params.Add(new KeyValuePair<string, string>("Quarter", null));
        Params.Add(new KeyValuePair<string, string>("Month", null));
        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);

        string result = PhoenixPurchaseVendorPerformance.GetVendorPerformanceDataVisualParams();
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
            Response.Redirect("../Purchase/PurchaseVendorPerformance.aspx");
        }

        if (CommandName.ToUpper().Equals("SHOWVISUAL"))
        {
            Response.Redirect("../Purchase/PurchaseVendorPerformancePOAmountVisual.aspx");
        }
    }

    [WebMethod]
    public static string PurchaseAnalysisYearWise(string Year, string VendorID, String Fleet, string Vessel, int? StockType)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEYEARWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        return result;
    }

    [WebMethod]
    public static string PurchaseAnalysisQuarterWise(string Year, string VendorID, String Fleet, string Vessel, int? StockType)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEQUARTERWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        return result;
    }

    [WebMethod]
    public static string PurchaseAnalysisMonthWise(string Year, string VendorID, String Fleet, string Vessel, int? StockType, string Quarter)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEMONTHWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        return result;
    }

    [WebMethod]
    public static string PurchaseAnalysisTopVendorComparison(string Year, string VendorID, String Fleet, string Vessel, int? StockType, string Quarter, string Month)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_5, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.Int, DbConstant.SMALLINT, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORCOUNT", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, 10));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASESTATUSCOUNTFORVENDOR", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        return result;
    }

    [WebMethod]
    public static string PurchaseAnalysisTopVendorAvgDeliveryDuration(string Year, string VendorID, String Fleet, string Vessel, int? StockType, string Quarter, string Month)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_5, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.Int, DbConstant.SMALLINT, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORCOUNT", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, null));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASETOPVENDORDELIVERYDURATION", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        return result;
    }

    [WebMethod]
    public static string PurchaseAnalysisAvgDaysToReceivedQuotation(string Year, string VendorID, String Fleet, string Vessel, int? StockType, string Quarter, string Month)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_5, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.Int, DbConstant.SMALLINT, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));


        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEAVGTIMERECEIVEQUOTATION", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        return result;
    }


    [WebMethod]
    public static string PurchaseAnalysisAvgTimetoPOIssuedChart(string Year, string VendorID, String Fleet, string Vessel, int? StockType, string Quarter, string Month)
    {

        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_5, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.Int, DbConstant.SMALLINT, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEAVGTIMEPOISSUED", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;

    }

    [WebMethod]
    public static string PurchaseAnalysisAvgTimetogoodsReceiveChart(string Year, string VendorID, String Fleet, string Vessel, int? StockType, string Quarter, string Month)
    {

        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_5, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.Int, DbConstant.SMALLINT, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEAVGGOODSRECEIVEDDUTRATION", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;

    }

    [WebMethod]
    public static string PurchaseAnalysisAvgTimetoQuoteTable(string Year, string VendorID, String Fleet, string Vessel, int? StockType, string Quarter, string Month)
    {

        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_5, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.Int, DbConstant.SMALLINT, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORCOUNT", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, 40));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEVENDORAVGTIMETOQUOTE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;

    }


}
