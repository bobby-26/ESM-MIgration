<%@ WebHandler Language="C#" Class="OptionsCrewOffshore" %>

using System;
using System.Web;
using System.IO;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.Text;
using System.Collections.Generic;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Export2XL;

public class OptionsCrewOffshore : IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        string rowusercode = request.QueryString["rowusercode"].ToString();
        input = server.UrlDecode((new System.IO.StreamReader(request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString();

        response.ClearContent();
        response.Buffer = true;
        response.ContentType = "text/xml";

        context.Session["CURRENTDATABASE"] = null;

        if (strFunctionName.ToUpper() == "CREWOFFSHORETESTQUESTIONUPDATE")
        {
            response.Write(CrewOffshoreTestQuestionUpdate(input, rowusercode));
        }
    }

    private string CrewOffshoreTestQuestionUpdate(string input, string rowusercode)
    {
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixCrewOffshoreService.CrewOffshoreTestQuestionUpdate(int.Parse(rowusercode), input);

            return "Questions are updated.";
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