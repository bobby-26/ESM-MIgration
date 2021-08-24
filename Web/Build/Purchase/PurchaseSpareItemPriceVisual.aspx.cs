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


public partial class Purchase_PurchaseSpareItemPriceVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar.Show();
        NameValueCollection filter;

        if (filterPurchase.CurrentPurchaseSpareItemAnalysisPriceFilter != null)
        {
            filter = filterPurchase.CurrentPurchaseSpareItemAnalysisPriceFilter;
        }
        else
        {
            filter = filterPurchase.DefaulPurchaseSpareItemAnalysisPriceFilter;
        }

        
        // convert them to dictionary
        IDictionary<string, string> Params = new Dictionary<string, string>();

        if (filter != null)
        {
            string Component = General.GetNullableString(filter.Get("txtComponentID"));
            string ItemNo = General.GetNullableString(filter.Get("txtPartNumber"));
            string ItemName = General.GetNullableString(filter.Get("txtPartName"));

            Params.Add(new KeyValuePair<string, string>("Component", Component));
            Params.Add(new KeyValuePair<string, string>("ItemNo", ItemNo));
            Params.Add(new KeyValuePair<string, string>("ItemName", ItemName));
            Params.Add(new KeyValuePair<string, string>("Year", null));
            Params.Add(new KeyValuePair<string, string>("Port", null));
            Params.Add(new KeyValuePair<string, string>("Vessel", null));
        }
        else
        {
            Params.Add(new KeyValuePair<string, string>("Component", null));
            Params.Add(new KeyValuePair<string, string>("ItemNo", null));
            Params.Add(new KeyValuePair<string, string>("ItemName", null));
            Params.Add(new KeyValuePair<string, string>("Year", null));
            Params.Add(new KeyValuePair<string, string>("Port", null));
            Params.Add(new KeyValuePair<string, string>("Vessel", null));
        }
        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);
        string result = PhoenixPurchaseSpareItemPriceAnalysis.GetSpareItemPriceVisualParams();
        Page.ClientScript.RegisterStartupScript(this.GetType(),
            "createChart", String.Format("createChart({0},{1});", result, jsonParams), true);
    }


    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            Response.Redirect("../Purchase/PurchaseSpareItemPriceAnalysis.aspx");
        }

    }

    [WebMethod]
    public static string purchaseyearchart(string Component, string ItemNo, string ItemName)
    {

        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNO", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemNo));
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNAME", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemName));
        ParameterList.Add(DataAccess.GetDBParameter("@COMPONENTID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Component));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASESPAREITEMYEARWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        return result;
    }

    [WebMethod]
    public static string purchaseportchart(string Component, string ItemNo, string ItemName, string Year)
    {
        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNO", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemNo));
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNAME", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemName));
        ParameterList.Add(DataAccess.GetDBParameter("@COMPONENTID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Component));
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASESPAREITEMPORTWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;

    }

    [WebMethod]
    public static string purchasevesselchart(string Component, string ItemNo, string ItemName, string Year, string Port)
    {

        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@PORTID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Port));
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNO", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemNo));
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNAME", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemName));
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));
        ParameterList.Add(DataAccess.GetDBParameter("@COMPONENTID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Component));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASEPURCHASESPAREITEMVESSELWISE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;

    }

    [WebMethod]
    public static string purchaseportdetailchart(string Component, string ItemNo, string ItemName, string Year, string Port, string Vessel)
    {

        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@COMPONENTID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Component));
        ParameterList.Add(DataAccess.GetDBParameter("@PORTID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Port));
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNO", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemNo));
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNAME", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemName));
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Vessel));
        ParameterList.Add(DataAccess.GetDBParameter("@YEAR", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, Year));

        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASESPAREITEMPORTWISEQUANTITY", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        return result;
    }

}