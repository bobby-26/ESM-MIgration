<%@ WebHandler Language="C#" Class="OptionsPurchase" %>

using System;
using System.Web;
using System.IO;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.Text;
using System.Collections.Generic;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Framework;

public class OptionsPurchase : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        HttpServerUtility server = context.Server;

        string input = "";
        string strFunctionName = request.QueryString["methodname"].ToString();
        string strStockType = request.QueryString["stocktype"].ToString();
        string strQuotationId = request.QueryString["quotationid"].ToString();
        input = server.UrlDecode((new System.IO.StreamReader(request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString();

        response.ClearContent();
        response.Buffer = true;
        response.ContentType = "text/xml";

        context.Session["CURRENTDATABASE"] = null;

        if (strFunctionName == "QuotationLineItemUpdate")
        {
            response.Write(QuotationLineItemUpdate(input, strStockType, strQuotationId));
        }
    }
    
    private string QuotationLineItemUpdate(string input, string stocktype, string quotationid)
    {
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            if (stocktype == "STORE")
            {
                PhoenixPurchaseQuotationLine.QuotationStoreLineXLUpdate(input, new Guid(quotationid));
                PhoenixPurchaseQuotationLine.FinalizeQuotationStoreForVendorFromXL(1, new Guid(quotationid));
            }
            else
            {
                PhoenixPurchaseQuotationLine.QuotationLineXLUpdate(input, new Guid(quotationid));
                PhoenixPurchaseQuotationLine.FinalizeQuotationForVendorFromXL(1, new Guid(quotationid));
            }

            //PhoenixPurchaseQuotationLine.FinalizeQuotationForVendor(1, new Guid(quotationid));
            PhoenixCommoneProcessing.CloseUserSession(new Guid(quotationid));

            return "You've updated successfully";
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