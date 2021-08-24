<%@ WebHandler Language="C#" Class="OptionsInventory" %>

using System;
using System.Web;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using System.Data;

public class OptionsInventory : IHttpHandler, System.Web.SessionState.IRequiresSessionState 
{
    HttpRequest request = null;
    HttpResponse response = null;
    HttpServerUtility server = null;    
    public void ProcessRequest (HttpContext context) 
    {
        request = context.Request;
        response = context.Response;
        server = context.Server;

        string input = "";
        string strFunctionName = request.QueryString["methodname"].ToString();
        int userCode = int.Parse(request.QueryString["usercode"].ToString());

        input = server.UrlDecode((new System.IO.StreamReader(request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString();

        response.ClearContent();
        response.Buffer = true;
        response.ContentType = "text/xml";

        context.Session["CURRENTDATABASE"] = null;

        if (strFunctionName.ToUpper() == "INVENTORYSPARELOCATIONQTYUPDATE")
        {
            response.Write(INVENTORYSPARELOCATIONQTYUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "INVENTORYSTORELOCATIONQTYUPDATE")
        {
            response.Write(INVENTORYSTORELOCATIONQTYUPDATE(input, userCode));
        }             
        else if (strFunctionName.ToUpper() == "INVENTORYSPARELOCATIONCONTROLSEARCH")
        {
            response.Write(INVENTORYSPARELOCATIONCONTROLSEARCH(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "INVENTORYSTORELOCATIONCONTROLSEARCH")
        {
            response.Write(INVENTORYSTORELOCATIONCONTROLSEARCH(input, userCode));
        }         
    }

    public string INVENTORYSPARELOCATIONQTYUPDATE(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInventorySpareItemControl.SpareItemLocationQtyUpdate(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
            
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string INVENTORYSTORELOCATIONQTYUPDATE(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInventoryStoreItemControl.StoreItemLocationQtyUpdate(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
            
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }    

    public string INVENTORYSPARELOCATIONCONTROLSEARCH(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInventorySpareItemControl.SpareItemLocationQtySearch(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
            
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string INVENTORYSTORELOCATIONCONTROLSEARCH(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInventoryStoreItemControl.StoreItemLocationQtySearch(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);

        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }         
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}