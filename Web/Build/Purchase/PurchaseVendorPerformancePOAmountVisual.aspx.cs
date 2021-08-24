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
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class Purchase_PurchaseVendorPerformancePOAmountVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
        toolbar.AddButton("Visual", "SHOWVISUAL",ToolBarDirection.Right);
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

        string result = PhoenixPurchaseVendorPerformance.GetVendorPerformancePOAmountVisualParams();
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
            Response.Redirect("../Purchase/PurchaseVendorPerformance.aspx");
        }

        if (CommandName.ToUpper().Equals("SHOWVISUAL"))
        {
            Response.Redirect("../Purchase/PurchaseVendorPerformanceDataVisual.aspx");
        }
    }

    [WebMethod]
    public static string PurchaseAnalysisPOAmountYearWise(string Year, string VendorID, String Fleet, string Vessel, int? StockType)
    {

        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEPOAMOUNTBYYEAR", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;

    }

    [WebMethod]
    public static string PurchaseAnalysisPOAmountMonthWise(string Year, string VendorID, String Fleet, string Vessel, int? StockType)
    {

        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEPOAMOUNTMONTHWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;
    }

    [WebMethod]
    public static string PurchaseAnalysisPOAmountVendorWise(string Year, string Month, string VendorID, String Fleet, string Vessel, int? StockType)
    {

        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("MONTHID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEPOAMOUNTVENDORWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;
    }

    [WebMethod]
    public static string PurchaseAnalysisPOAmountFleetWise(string Year, string Month, string VendorID, String Fleet, string Vessel, int? StockType)
    {

        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("MONTHID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEFLEETWISEPOAMOUNT", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;
    }

    [WebMethod]
    public static string PurchaseAnalysisPOAmountVesselWise(string Year, string Month, string VendorID, String Fleet, string Vessel, int? StockType)
    {

        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("MONTHID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEVESSELWISEPOAMOUNT", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;
    }

    [WebMethod]
    public static string PurchaseAnalysisPOAmountVendorList(string Year, string Month, string VendorID, String Fleet, string Vessel, int? StockType)
    {

        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, VendorID));
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Fleet));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTHID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, StockType));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEPOAMOUNTVENDORLIST", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;


    }
}
