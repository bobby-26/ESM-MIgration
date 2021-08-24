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


public partial class Purchase_PurchaseSpareItemDeliveryAnalysisVisual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar.Show();
        NameValueCollection filter;

        if (filterPurchase.CurrentPurchaseSpareItemDeliveryAnalysisFilter != null)
        {
            filter = filterPurchase.CurrentPurchaseSpareItemDeliveryAnalysisFilter;
        } else
        {
            filter = filterPurchase.DefaultPurchaseSpareItemDeliveryAnalysisFilter;
        }

      //  pageTitle.Text = "Spare Item Delivery Duration Visual";
        IDictionary<string, string> Params = new Dictionary<string, string>();

        if (filter != null)
        {
            string ItemNo   = General.GetNullableString(filter.Get("txtPartNumber"));
            string ItemName = General.GetNullableString(filter.Get("txtPartName"));
            string componentId = General.GetNullableString(filter.Get("txtComponentID"));

            Params.Add(new KeyValuePair<string, string>("ItemNo", ItemNo));
            Params.Add(new KeyValuePair<string, string>("ItemName", ItemName));
            Params.Add(new KeyValuePair<string, string>("ComponentId", componentId));
            Params.Add(new KeyValuePair<string, string>("VendorId", null));
            Params.Add(new KeyValuePair<string, string>("PortId", null));
        }
        else
        {
            Params.Add(new KeyValuePair<string, string>("ItemNo", null));
            Params.Add(new KeyValuePair<string, string>("ItemName", null));
            Params.Add(new KeyValuePair<string, string>("ComponentId", null));
            Params.Add(new KeyValuePair<string, string>("VendorId", null));
            Params.Add(new KeyValuePair<string, string>("PortId", null));
        }
        string jsonParams = JsonConvert.SerializeObject(Params, Formatting.Indented);
        string result = PhoenixPurchaseAnalytics.GetSpareItemDeliveryVisualParams();
        Page.ClientScript.RegisterStartupScript(this.GetType(),
            "createChart", String.Format("createChart({0},{1});", result, jsonParams), true);
    }

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            Response.Redirect("../Purchase/PurchaseSpareItemDeliveryAnalysis.aspx");
        }

    }

    [WebMethod]
    public static string vendorwiseshortdelivery(string ItemNo, string ItemName, string ComponentId)
    {

        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNUMBER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemNo));
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNAME", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemName));
        ParameterList.Add(DataAccess.GetDBParameter("@COMPONENTID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ComponentId));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASESPAREITEMDELIVERYVENDOR", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);
        return result;
    }

    [WebMethod]
    public static string portwiseshortdelivery(string ItemNo, string ItemName, string ComponentId, string VendorId)
    {
        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNUMBER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemNo));
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNAME", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemName));
        ParameterList.Add(DataAccess.GetDBParameter("@COMPONENTID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ComponentId));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, VendorId));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASESPAREITEMDELIVERYPORT", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;

    }

    [WebMethod]
    public static string averagedeliveryduration(string ItemNo, string ItemName, string ComponentId, string VendorId, string PortId)
    {

        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNUMBER", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemNo));
        ParameterList.Add(DataAccess.GetDBParameter("@ITEMNAME", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ItemName));
        ParameterList.Add(DataAccess.GetDBParameter("@COMPONENTID", SqlDbType.VarChar, DbConstant.VARCHAR_MAX, ParameterDirection.Input, ComponentId));
        ParameterList.Add(DataAccess.GetDBParameter("@VENDORID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, VendorId));
        ParameterList.Add(DataAccess.GetDBParameter("@PORTID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, PortId));
        ds = DataAccess.ExecSPReturnDataSet("PRCHARTPURCHASESPAREITEMDELIVERYAVERGAE", ParameterList);
        string result = General.DataTableToJSONWithJSONNet(ds.Tables[0]);

        return result;

    }


}