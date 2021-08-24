<%@ WebHandler Language="C#" Class="OptionsAdministration" %>

using System;
using System.Web;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.PhoenixAdministrationAssetReports;
using System.Data;
using System.IO;
using OfficeOpenXml;

public class OptionsAdministration : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    HttpRequest request = null;
    HttpResponse response = null;
    HttpServerUtility server = null;
    public void ProcessRequest(HttpContext context)
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
        if (strFunctionName.ToUpper() == "ADMINISTRATIONASSETINSERT")
        {
            response.Write(AdministrationAsset(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "ADMINISTRATIONASSETUPDATE")
        {
            response.Write(AdministrationAssetUpdate(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "ADMINZONEREGISTERSEARCH")
        {
            response.Write(AdministrationZoneRegister());
        }
        else if (strFunctionName.ToUpper() == "ADMINASSETSEARCH")
        {
            response.Write(AdminAssetSearch(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "ADMINHARDWAREASSETTYPESEARCH")
        {
            response.Write(AdministrationCategoryRegister());
        }
        else if (strFunctionName.ToUpper() == "ADMINSOFTWAREASSETTYPESEARCH")
        {
            response.Write(AdministrationSoftwareCategoryRegister());
        }
        else if (strFunctionName.ToUpper() == "ADMINASSETUSERMAPPINGSEARCH")
        {
            response.Write(AdminAssetUserSearch(input));
        }
    }
    public string AdministrationAsset(string input, int userCode)
    {
        Guid Id = Guid.Empty;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoenixAdministrationAssetReports.AdministrationAssetInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string AdministrationAssetUpdate(string input, int userCode)
    {
        Guid Id = Guid.Empty;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoenixAdministrationAssetReports.AdministrationAssetUpdate(input, userCode);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string AdministrationZoneRegister()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixAdministrationAssetReports.ZoneRegisterReport();
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string AdministrationCategoryRegister()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixAdministrationAssetReports.AdministrationCategoryRegister(1);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string AdministrationSoftwareCategoryRegister()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixAdministrationAssetReports.AdministrationCategoryRegister(2);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string AdminAssetSearch(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixAdministrationAssetReports.AdminAssetSearch(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string AdminAssetUserSearch(string input)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixAdministrationAssetReports.AdminAssetUserSearch(input);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}