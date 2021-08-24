<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;

public class Handler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        if (strFunctionName.ToUpper() == "BANKACCOUNTCORRECTION")
        {
            Guid? id = General.GetNullableGuid(request.QueryString["id"].ToString());
            response.Write(EmployeeBankAccountBulkUpdate(input, userCode, id));
        }
        else if (strFunctionName.ToUpper() == "STOCKCHECKOFPROVISIONITEMSEARCH")
        {
            int vesselid = int.Parse(request.QueryString["vesselid"].ToString());
            string closingdate = request.QueryString["closingdate"].ToString();
            response.Write(StockCheckProvisionItemSearch(input, vesselid, closingdate, userCode));
        }
        else if (strFunctionName.ToUpper() == "STOCKCHECKOFPROVISIONITEMUPDATE")
        {
            int vesselid = int.Parse(request.QueryString["vesselid"].ToString());
            string closingdate = request.QueryString["closingdate"].ToString();
            response.Write(StockCheckProvisionItemUpdate(userCode, input, vesselid, closingdate));
        }    
    }


    public string EmployeeBankAccountBulkUpdate(string input, int userCode, Guid? id)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixVesselAccountsEmployeeBankAccount.EmployeeBankBulkUpdate(userCode, id, input);

            return "Saved";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }


    }
 
    public string StockCheckProvisionItemSearch(string input, int vesselid, string closingdate, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixVesselAccountsProvision.StockCheckProvisionItemExcelList(input, vesselid, General.GetNullableDateTime(closingdate), userCode);

            return "Saved";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }


    }

    public string StockCheckProvisionItemUpdate(int userCode, string input, int vesselid, string closingdate)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixVesselAccountsProvision.UpdateBulkProvisionConsumptionXml(userCode, input, vesselid, DateTime.Parse(closingdate));

            return "Saved";
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