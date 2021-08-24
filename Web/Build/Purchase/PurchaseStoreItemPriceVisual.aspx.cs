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


public partial class Purchase_PurchaseStoreItemPriceVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar.Show();

        //NameValueCollection filter = filterPurchase.CurrentPurchaseStoreItemAnalysisPriceFilter;

        NameValueCollection filter;

        if (filterPurchase.CurrentPurchaseStoreItemAnalysisPriceFilter != null)
        {
            filter = filterPurchase.CurrentPurchaseStoreItemAnalysisPriceFilter;
        }
        else
        {
            filter = filterPurchase.DefaultPurchaseStoreItemAnalysisPriceFilter;
        }

      //  pageTitle.Text = "Store Item Visual";
        IDictionary<string, string> Params = new Dictionary<string, string>();

        if (filter != null)
        {
            string ItemNo = General.GetNullableString(filter.Get("txtPartNumber"));
            string ItemName = General.GetNullableString(filter.Get("txtPartName"));
            string storeTypeId = General.GetNullableString(filter.Get("txtStoreTypeId"));

            Params.Add(new KeyValuePair<string, string>("ItemNo", ItemNo));
            Params.Add(new KeyValuePair<string, string>("ItemName", ItemName));
            Params.Add(new KeyValuePair<string, string>("StoreId", storeTypeId));
            Params.Add(new KeyValuePair<string, string>("Year", null));
            Params.Add(new KeyValuePair<string, string>("Port", null));
            Params.Add(new KeyValuePair<string, string>("Vessel", null));
        }
        else
        {
            Params.Add(new KeyValuePair<string, string>("ItemNo", null));
            Params.Add(new KeyValuePair<string, string>("ItemName", null));
            Params.Add(new KeyValuePair<string, string>("StoreId", null));
            Params.Add(new KeyValuePair<string, string>("Year", null));
            Params.Add(new KeyValuePair<string, string>("Port", null));
            Params.Add(new KeyValuePair<string, string>("Vessel", null));
        }
        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);
        string result = PhoenixPurchaseStoreItemPriceAnalysis.GetStoreItemPriceVisualParams();
        Page.ClientScript.RegisterStartupScript(this.GetType(),
            "createChart", String.Format("createChart({0},{1});", result, jsonParams), true);
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            Response.Redirect("../Purchase/PurchaseStoreItemPriceAnalysis.aspx");
        }

    }

    [WebMethod]
    public static string purchaseyearchart(string ItemNo, string ItemName, string StoreId)
    {

        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNO", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemNo));
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNAME", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemName));
        ParameterList.Add(DataAccess.GetDBParameter("@STORETYPEID", SqlDbType.VarChar, DbConstant.VARCHAR_20, ParameterDirection.Input, StoreId));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASESTOREITEMYEARWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        return result;
    }

    [WebMethod]
    public static string purchaseportchart(string ItemNo, string ItemName, string StoreId, string  Year)
    {
        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNO", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemNo));
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNAME", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemName));
        ParameterList.Add(DataAccess.GetDBParameter("@STORETYPEID", SqlDbType.VarChar, DbConstant.VARCHAR_20, ParameterDirection.Input, StoreId));
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Year));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASESTOREITEMPORTWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;

    }

    [WebMethod]
    public static string purchasevesselchart(string ItemNo, string ItemName, string StoreId, string Year, string Port)
    {

        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNO", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemNo));
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNAME", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemName));
        ParameterList.Add(DataAccess.GetDBParameter("@STORETYPEID", SqlDbType.VarChar, DbConstant.VARCHAR_20, ParameterDirection.Input, StoreId));
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@PORT", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Port));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEPURCHASESTOREITEMVESSELWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;

    }

    [WebMethod]
    public static string purchaseportdetailchart(string ItemNo, string ItemName, string StoreId, string Year, string Port, string Vessel)
    {

        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNO", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemNo));
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNAME", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemName));
        ParameterList.Add(DataAccess.GetDBParameter("@STORETYPEID", SqlDbType.VarChar, DbConstant.VARCHAR_20, ParameterDirection.Input, StoreId));
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@PORT", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Port));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSEL", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, Vessel));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASESTOREITEMPORTWISEQUANTITY", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        return result;
    }

}
