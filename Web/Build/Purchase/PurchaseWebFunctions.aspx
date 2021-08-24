<%@ Page Language="C#" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Purchase" %>
<script language="c#" runat="server">
    
    private string strFunctionName;
    
    protected void Page_Load(object o, EventArgs e)
    {
        Session.Timeout = 120;
        //GetParameters(Server.UrlDecode((new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd()));

        strFunctionName = Request.QueryString.ToString();

        if (strFunctionName == "TestMethod")
        {
            Response.Write(Server.UrlDecode((new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString());
        }
        if (strFunctionName == "GetRFQToReceive")
        {
            string input = "";
            input = Server.UrlDecode((new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString();
            GetRFQToReceive(input);
        }
        if (strFunctionName == "GetRFQToSend")
        {
            string input = "";
            input = Server.UrlDecode((new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString();
            GetRFQToSend(input);
        }
        if (strFunctionName == "GetRFQExport")
        {
            string input = "";
            input = Server.UrlDecode((new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString();
            GetRFQExport(input);
        }

        if (strFunctionName == "GetRFQLineItemExport")
        {
            string input = "";
            input = Server.UrlDecode((new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString();
            GetRFQLineItemExport(input);
        }

        if (strFunctionName == "ImportRFQ")
        {
            string input = "";
            input = Server.UrlDecode((new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString();
            GetRFQImport(input);
        }

        if (strFunctionName == "GetRFQLineItemImport")
        {
            string input = "";
            input = Server.UrlDecode((new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString();
            GetRFQLineItemImport(input);
        }

        if (strFunctionName == "UpdateImportRFQLookUp")
        {
            string input = "";
            input = Server.UrlDecode((new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString();
            UpdateImportRFQLookUp(input);
        }

        if (strFunctionName == "UpdateExportRFQLookUp")
        {
            string input = "";
            input = Server.UrlDecode((new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString();
            UpdateExportRFQLookUp(input);
        }
    }

    private void GetRFQToSend(string input)
    {
        DataSet ds = PhoenixPurchaseQuotation.GetRFQToSend();
        Response.Write(ds.GetXml());
    }
    private void GetRFQToReceive(string input)
    {
        DataSet ds = PhoenixPurchaseQuotation.GetRFQToReceive();
        Response.Write(ds.GetXml());
    }
    private void GetRFQExport(string input)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(input);
        DataSet ds = PhoenixPurchaseQuotation.GetRFQExport(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            doc.DocumentElement.SelectSingleNode("FLDORDERID").InnerText,
            doc.DocumentElement.SelectSingleNode("FLDQUOTATIONID").InnerText,
            doc.DocumentElement.SelectSingleNode("FLDVENDORID").InnerText);        
        Response.Write(ds.GetXml());
    }

    private void GetRFQLineItemExport(string input)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(input);
        DataSet ds = PhoenixPurchaseQuotation.GetRFQLineItemExport(doc.DocumentElement.SelectSingleNode("FLDORDERID").InnerText, doc.DocumentElement.SelectSingleNode("FLDQUOTATIONID").InnerText, doc.DocumentElement.SelectSingleNode("FLDVENDORID").InnerText);
        Response.Write(ds.GetXml());
    }
    private void GetRFQImport(string input)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(input);
       
        PhoenixPurchaseQuotation.UpdateQuotationForExcel
            (1,
            doc.DocumentElement.SelectSingleNode("FLDQUOTATIONID").InnerText,
            doc.DocumentElement.SelectSingleNode("FLDORDERID").InnerText,
            doc.DocumentElement.SelectSingleNode("FLDDATE").InnerText,
			doc.DocumentElement.SelectSingleNode("FLDQUOTEDCURRENCY").InnerText,
			doc.DocumentElement.SelectSingleNode("FLDDELIVERYTIME").InnerText,
			doc.DocumentElement.SelectSingleNode("FLDYOURREF").InnerText
            );
    }

    private void GetRFQLineItemImport(string input)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(input);
        PhoenixPurchaseQuotationLine.UpdateQuotationLineForExcel
			(1,
			doc.DocumentElement.SelectSingleNode("FLDQUOTATIONLINEID").InnerText,
			doc.DocumentElement.SelectSingleNode("FLDQUOTATIONID").InnerText,
			doc.DocumentElement.SelectSingleNode("FLDUNITPRICE").InnerText,
            doc.DocumentElement.SelectSingleNode("FLDORDERID").InnerText
			);
    }

    private void UpdateImportRFQLookUp(string input)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(input);
        PhoenixPurchaseQuotation.UpdateImportRFQLookUp(doc.DocumentElement.SelectSingleNode("FLDUNIQUEID").InnerText);
        
    }

    private void UpdateExportRFQLookUp(string input)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(input);
        PhoenixPurchaseQuotation.UpdateExportRFQLookUp(doc.DocumentElement.SelectSingleNode("FLDUNIQUEID").InnerText);

    }
    private void GetParameters(string strQueryString)
    {
        Response.Write(strQueryString);
    }
</script>
