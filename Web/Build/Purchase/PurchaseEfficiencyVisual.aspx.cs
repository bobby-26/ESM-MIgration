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


public partial class Purchase_PurchaseEfficiencyVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
        toolbar.AddButton("Visual", "SHOWVISUAL",ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar.Show();

        NameValueCollection Filter = filterPurchase.CurrentPurchaseEfficiencyFilter;

        string Year = Filter.Get("ddlYear") != null ? General.GetNullableString(Filter["ddlYear"].ToString()) : string.Empty;
        string StockType = Filter.Get("ddlType") != null ? General.GetNullableString(Filter["ddlType"].ToString()) : string.Empty;
        string StockTypeName = Filter.Get("ddlTypeName") != null ? General.GetNullableString(Filter["ddlTypeName"].ToString()) : string.Empty;
        string Group = Filter.Get("chkGroupList") != null ? General.GetNullableString(Filter["chkGroupList"].ToString()) : string.Empty;
        string Location = Filter.Get("lstPurchaseLocation") != null ? General.GetNullableString(Filter["lstPurchaseLocation"].ToString()) : string.Empty;
        string Quarter = Filter.Get("lstQuarter") != null ? General.GetNullableString(Filter["lstQuarter"].ToString()) : string.Empty;
        string Month = Filter.Get("lstMonth") != null ? General.GetNullableString(Filter["lstMonth"].ToString()) : string.Empty;



       IDictionary<string, string> Params = new Dictionary<string, string>();
        Params.Add(new KeyValuePair<string, string>("Year", Year));
        Params.Add(new KeyValuePair<string, string>("PurchaseGroup", Group));
        Params.Add(new KeyValuePair<string, string>("PurchaseLocation", Location));
        Params.Add(new KeyValuePair<string, string>("StockType", StockType));
        Params.Add(new KeyValuePair<string, string>("Quarter", Quarter));
        Params.Add(new KeyValuePair<string, string>("Month", Month));

        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);

        string result = PhoenixPurchaseEfficiency.GetPurchaseEfficiencyVisualParams();
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
            Response.Redirect("../Purchase/PurchaseEfficiency.aspx");
        }
    }

    //ChartMethods
    [WebMethod]
    public static string PurchasePerformanceRFQCreatedPlace(string Year, string Quarter, string Month, string StockType, string PurchaseGroup, string PurchaseLocation)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_10, ParameterDirection.Input, StockType));
        ParameterList.Add(DataAccess.GetDBParameter("@PURCHASEGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseGroup));
        ParameterList.Add(DataAccess.GetDBParameter("@LOCATION", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseLocation));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASERPERFORMANCERFQCOUNTBYYEAR", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;
    }
    [WebMethod]
    public static string PurchasePerformanceQuarterWise(string Year, string Quarter, string Month, string StockType, string PurchaseGroup, string PurchaseLocation)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_10, ParameterDirection.Input, StockType));
        ParameterList.Add(DataAccess.GetDBParameter("@POCREATEDPLACE", SqlDbType.VarChar, DbConstant.VARCHAR_20, ParameterDirection.Input, null));
        ParameterList.Add(DataAccess.GetDBParameter("@PURCHASEGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseGroup));
        ParameterList.Add(DataAccess.GetDBParameter("@LOCATION", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseLocation));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASERPERFORMANCEQUARTERWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;
    }
    [WebMethod]
    public static string PurchasePerformanceMonthWise(string Year, string Quarter, string Month, string StockType, string PurchaseGroup, string PurchaseLocation)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_10, ParameterDirection.Input, StockType));
        ParameterList.Add(DataAccess.GetDBParameter("@POCREATEDPLACE", SqlDbType.VarChar, DbConstant.VARCHAR_20, ParameterDirection.Input, null));
        ParameterList.Add(DataAccess.GetDBParameter("@PURCHASEGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseGroup));
        ParameterList.Add(DataAccess.GetDBParameter("@LOCATION", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseLocation));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASERPERFORMANCEMONTHWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;
    }
    [WebMethod]
    public static string PurchasePerformanceLocation(string Year, string Quarter, string Month, string StockType, string PurchaseGroup, string PurchaseLocation)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_10, ParameterDirection.Input, StockType));
        ParameterList.Add(DataAccess.GetDBParameter("@POCREATEDPLACE", SqlDbType.VarChar, DbConstant.VARCHAR_20, ParameterDirection.Input, null));
        ParameterList.Add(DataAccess.GetDBParameter("@PURCHASEGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseGroup));
        ParameterList.Add(DataAccess.GetDBParameter("@LOCATION", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseLocation));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASERPERFORMANCELOCATION", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;
    }
    [WebMethod]
    public static string PurchasePerformanceGroup(string Year, string Quarter, string Month, string StockType, string PurchaseGroup, string PurchaseLocation)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_10, ParameterDirection.Input, StockType));
        ParameterList.Add(DataAccess.GetDBParameter("@POCREATEDPLACE", SqlDbType.VarChar, DbConstant.VARCHAR_20, ParameterDirection.Input, null));
        ParameterList.Add(DataAccess.GetDBParameter("@PURCHASEGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseGroup));
        ParameterList.Add(DataAccess.GetDBParameter("@LOCATION", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseLocation));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASERPERFORMANCEPURCHASEGROUP", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;
    }
    [WebMethod]
    public static string PurchasePerformancePurchaser(string Year, string Quarter, string Month, string StockType, string PurchaseGroup, string PurchaseLocation)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_10, ParameterDirection.Input, StockType));
        ParameterList.Add(DataAccess.GetDBParameter("@POCREATEDPLACE", SqlDbType.VarChar, DbConstant.VARCHAR_20, ParameterDirection.Input, null));
        ParameterList.Add(DataAccess.GetDBParameter("@PURCHASEGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseGroup));
        ParameterList.Add(DataAccess.GetDBParameter("@LOCATION", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseLocation));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASERPERFORMANCEPURCHASERWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;
    }
    [WebMethod]
    public static string PurchasePerformancePOAmountRange(string Year, string Quarter, string Month, string StockType, string PurchaseGroup, string PurchaseLocation)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_10, ParameterDirection.Input, StockType));
        ParameterList.Add(DataAccess.GetDBParameter("@POCREATEDPLACE", SqlDbType.VarChar, DbConstant.VARCHAR_20, ParameterDirection.Input, null));
        ParameterList.Add(DataAccess.GetDBParameter("@PURCHASEGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseGroup));
        ParameterList.Add(DataAccess.GetDBParameter("@PURCHASER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, null));
        ParameterList.Add(DataAccess.GetDBParameter("@LOCATION", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseLocation));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASERPERFORMANCEPOAMOUNTRANGE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;
    }
    [WebMethod]
    public static string PurchasePerformanceVendorRank(string Year, string Quarter, string Month, string StockType, string PurchaseGroup, string PurchaseLocation)
    {
        DataSet ds = new DataSet();
        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@QUARTER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Quarter));
        ParameterList.Add(DataAccess.GetDBParameter("@MONTH", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Month));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_10, ParameterDirection.Input, StockType));
        ParameterList.Add(DataAccess.GetDBParameter("@POCREATEDPLACE", SqlDbType.VarChar, DbConstant.VARCHAR_20, ParameterDirection.Input, null));
        ParameterList.Add(DataAccess.GetDBParameter("@PURCHASEGROUP", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseGroup));
        ParameterList.Add(DataAccess.GetDBParameter("@PURCHASER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, null));
        ParameterList.Add(DataAccess.GetDBParameter("@POAMOUNTRANGEID", SqlDbType.SmallInt, DbConstant.SMALLINT, ParameterDirection.Input, null));
        ParameterList.Add(DataAccess.GetDBParameter("@LOCATION", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, PurchaseLocation));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASERPERFORMANCEVENDORRANK", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;
    }

    ////ChartMethods

}
