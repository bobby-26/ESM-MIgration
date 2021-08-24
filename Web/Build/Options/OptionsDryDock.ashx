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
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Export2XL;

public class Handler : IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    HttpRequest request = null;
    HttpResponse response = null;
    HttpServerUtility server = null;

    public void ProcessRequest (HttpContext context) {

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

        if (strFunctionName.ToUpper() == "DRYDOCKQUOTATIONUPDATE")
        {
            response.Write(DryDockQuotationUpdate(input, userCode));
        }
        if (strFunctionName.ToUpper() == "DRYDOCKPROJECTUPDATE")
        {
            response.Write(DryDockProjectUpdate(input, userCode));
        }
        if (strFunctionName.ToUpper() == "DRYDOCKPROJECTRESPONSIBILITYUPDATE")
        {
            response.Write(DryDockProjectResponsibilityUpdate(input,userCode));
        }
        if (strFunctionName.ToUpper() == "DRYDOCKEXPORT2XL")
        {
            response.Write(DryDockExport2XL(input));
        }
        if (strFunctionName.ToUpper() == "DRYDOCKMAKEREXPORT2XL")
        {
            DryDockMakerExport2XL(input);
        }
        if (strFunctionName.ToUpper() == "DRYDOCKCOMPAREUPDATE")
        {
            response.Write(DryDockCompareUpdate(input, userCode));
        }
        if (strFunctionName.ToUpper() == "DRYDOCKADDITIONALJOBINSERT")
        {
            response.Write(DryDockAdditionalJobInsert(input , userCode));
        }
    }
    private string DryDockProjectUpdate(string input,int usercode)
    {
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixDryDockService.DryDockProjectUpdate(input, usercode);

            return "<Ok/>";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private string DryDockProjectResponsibilityUpdate(string input,int usercode)
    {
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixDryDockService.DryDockProjectResponsibilityUpdate(input,usercode);

            return "<Ok/>";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private string DryDockAdditionalJobInsert(string input,int usercode)
    {
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            DataSet ds;
            DataTable dt;

            ds = PhoenixDryDockService.DryDockAdditionalJobInsert(input,usercode);
            dt = ds.Tables[0];
            string ST = dt.Rows[0][0].ToString();
            return dt.Rows[0][0].ToString();

        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private string DryDockQuotationUpdate(string input,int usercode)
    {
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixDryDockService.DryDockQuotationUpdate(input, usercode);

            return "<Ok/>";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    protected string DryDockExport2XL(string input)
    {
        string exportoption = request.QueryString["exportoption"].ToString();
        try {



            if (exportoption.ToUpper().Equals("PROJECT"))
            {
                string projectid = request.QueryString["projectid"].ToString();
                string vslid = request.QueryString["vslid"].ToString();
                PhoenixDryDock2XL.Export2XLDryDockProject(General.GetNullableGuid(projectid), int.Parse(vslid));
            }

            if (exportoption.ToUpper().Equals("QUOTATION"))
            {
                string orderid = request.QueryString["orderid"].ToString();
                string quotationid = request.QueryString["quotationid"].ToString();
                string vslid = request.QueryString["vslid"].ToString();
                PhoenixDryDock2XL.Export2XLDryDockQuotation(
                    General.GetNullableGuid(orderid),
                    General.GetNullableGuid(quotationid), int.Parse(vslid),null);
            }
            if (exportoption.ToUpper().Equals("INCREMENTALQUOTATION"))
            {
                string orderid = request.QueryString["orderid"].ToString();
                string quotationid = request.QueryString["quotationid"].ToString();
                string vslid = request.QueryString["vslid"].ToString();
                PhoenixDryDock2XL.Export2XLDryDockQuotationIncremental(
                    General.GetNullableGuid(orderid),
                    General.GetNullableGuid(quotationid), int.Parse(vslid) , null);
            }
            if (exportoption.ToUpper().Equals("JOBSTATUS"))
            {
                string orderid = request.QueryString["orderid"].ToString();
                string quotationid = request.QueryString["quotationid"].ToString();
                string vslid = request.QueryString["vslid"].ToString();
                PhoenixDryDock2XL.Export2XLDryDockJobStatus(
                    General.GetNullableGuid(orderid),
                    General.GetNullableGuid(quotationid), int.Parse(vslid));
            }
            if (exportoption.ToUpper().Equals("COMPARE"))
            {
                string orderid = request.QueryString["orderid"].ToString();
                string quotationid = request.QueryString["quotationid"].ToString();
                string vslid = request.QueryString["vslid"].ToString();
                PhoenixDryDock2XL.Export2XLDryDockCompare(
                    General.GetNullableGuid(orderid), int.Parse(vslid), quotationid);
            }

            if (exportoption.ToUpper().Equals("ESTIMATE"))
            {
                string projectid = request.QueryString["projectid"].ToString();
                string vslid = request.QueryString["vslid"].ToString();
                PhoenixDryDock2XL.Export2XLDryDockEstimate(General.GetNullableGuid(projectid), int.Parse(vslid));
            }
            return "<Ok/>";
        }
        catch (Exception ex)
        {
            response.ContentType = "text/html";
            string Help = "";
            if (ex.Message == "A worksheet with this name already exists in the workbook")
            {
                    Help = ". A Job with Same name exists in Project . Please change the Job in Repair Job / Adhoc Job.";
                    return  Help;
            }
            return ex.Message ;
        }
    }

    protected void DryDockMakerExport2XL(string input)
    {
        string exportoption = request.QueryString["exportoption"].ToString();

        if (exportoption.ToUpper().Equals("QUOTATION"))
        {
            string orderid = request.QueryString["orderid"].ToString();
            string quotationid = request.QueryString["quotationid"].ToString();
            string vslid = request.QueryString["vslid"].ToString();
            PhoenixDryDock2XL.Export2XLDryDockMakerQuotation(
                General.GetNullableGuid(orderid),
                General.GetNullableGuid(quotationid), int.Parse(vslid) , null);
        }
        if (exportoption.ToUpper().Equals("INCREMENTALQUOTATION"))
        {
            string orderid = request.QueryString["orderid"].ToString();
            string quotationid = request.QueryString["quotationid"].ToString();
            string vslid = request.QueryString["vslid"].ToString();
            PhoenixDryDock2XL.Export2XLDryDockMakerQuotationIncremental(
                General.GetNullableGuid(orderid),
                General.GetNullableGuid(quotationid), int.Parse(vslid),null);
        }
    }
    private string DryDockCompareUpdate(string input,int usercode)
    {
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixDryDockService.DryDockQuotationCompareUpdate(input,usercode);

            return "<Ok/>";
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
