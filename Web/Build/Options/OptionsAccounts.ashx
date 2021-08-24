<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.IO;
using OfficeOpenXml;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.Text;
using System.Collections.Generic;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Export2XL;

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

        if (strFunctionName.ToUpper() == "INVOICEACCURALS")
        {
            InvoiceAccuralsExport2XL(input);
        }
        if (strFunctionName.ToUpper() == "EXCHANGERATEUPDATE")
        {
            response.Write(ExchanngeRateUpdate(input, userCode));
        }
               

    }


    protected void InvoiceAccuralsExport2XL(string input)
    {
        string finyear = request.QueryString["finyear"].ToString();

        PhoenixAccountsFinancialYearSetup2XL.Export2XLInvoiceAccurals(int.Parse(finyear));

    }
    
    public string ExchanngeRateUpdate(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixAccountsVoucherList.Export2XLExchangeRateUpdate(input, userCode);
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
