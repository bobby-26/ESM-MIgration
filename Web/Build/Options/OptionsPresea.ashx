<%@ WebHandler Language="C#" Class="OptionsPresea" %>

using System;
using System.Web;
using SouthNests.Phoenix.PreSea;
using System.Text;
using System.Data;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;

public class OptionsPresea : IHttpHandler
{
    HttpRequest request = null;
    HttpResponse response = null;
    HttpServerUtility server = null;
    public void ProcessRequest (HttpContext context) {

        request = context.Request;
        response = context.Response;
        server = context.Server;
        string input = "";
        string strFunctionName = request.QueryString["methodname"].ToString();
        input = server.UrlDecode((new System.IO.StreamReader(request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString();

        response.ClearContent();
        response.Buffer = true;
        response.ContentType = "text/xml";

        if (strFunctionName.ToUpper() == "BATCHINSERTUPDATE")
        {
            response.Write(BATCHINSERTUPDATE(input, int.Parse(request.QueryString["rowusercode"])));
        }
        else if (strFunctionName.ToUpper() == "PRESEAENTRANCEEXAMINSERTUPDATE")
        {
            response.Write(PRESEAENTRANCEEXAMINSERTUPDATE(input, int.Parse(request.QueryString["rowusercode"])));
        }

        else if (strFunctionName.ToUpper() == "PRESEATRAINEEINSERTUPDATE")
        {
            response.Write(PRESEATRAINEEINSERTUPDATE(input, int.Parse(request.QueryString["rowusercode"])));
        }

        else if (strFunctionName.ToUpper() == "PRESEAEMPLOYEEADDRESSINSERTUPDATE")
        {
            response.Write(PRESEAEMPLOYEEADDRESSINSERTUPDATE(input, int.Parse(request.QueryString["rowusercode"])));
        }

        else if (strFunctionName.ToUpper() == "PRESEAINTERVIEWSUMMARYINSERTUPDATE")
        {
            response.Write(PRESEAINTERVIEWSUMMARYINSERTUPDATE(input, int.Parse(request.QueryString["rowusercode"])));
        }

        else if (strFunctionName.ToUpper() == "PRESEAINTERVIEWACADEMICMARKS")
        {
            response.Write(PRESEAINTERVIEWACADEMICMARKS(input, int.Parse(request.QueryString["rowusercode"])));
        }

        else if (strFunctionName.ToUpper() == "PRESEAINTERVIEWRITTENEXAMMARKS")
        {
            response.Write(PRESEAINTERVIEWRITTENEXAMMARKS(input, int.Parse(request.QueryString["rowusercode"])));
        }

        else if (strFunctionName.ToUpper() == "PRESEAINTERVIEPERSONALITYEXAMMARKS")
        {
            response.Write(PRESEAINTERVIEPERSONALITYEXAMMARKS(input, int.Parse(request.QueryString["rowusercode"])));
        }
        else if (strFunctionName.ToUpper() == "POPULATECITY")
        {
            response.Write(POPULATECITY());
        }
        else if (strFunctionName.ToUpper() == "POPULATESTATE")
        {
            int countryId=0;
            if (request.QueryString["countryId"] != null)
                countryId = General.GetNullableInteger(request.QueryString["countryId"].ToString()).Value;
            response.Write(POPULATESTATE(countryId));
        }
        else if (strFunctionName.ToUpper() == "POPULATECOUNTRY")
        {
            response.Write(POPULATECOUNTRY());
        }
        else if (strFunctionName.ToUpper() == "POPULATESCORECSRDFIELDS")
        {
            int scorecardId=0;
            if (request.QueryString["scorecardId"] != null)
                scorecardId = General.GetNullableInteger(request.QueryString["scorecardId"].ToString()).Value;
            response.Write(POPULATESCORECSRDFIELDS(scorecardId));
        }
        else if (strFunctionName.ToUpper() == "INTERVIEWSCORECARDINSERTUPDATE")
        {
             response.Write(INTERVIEWSCORECARDINSERTUPDATE(input,int.Parse(request.QueryString["rowusercode"])));
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

    private string BATCHINSERTUPDATE(string input, int RowUserCode)
    {
        string batchId = null;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixPreSeaBatch.PreSeaBathPopulate(RowUserCode, input, ref batchId);

            return ("<BATCHID>" + batchId + "</BATCHID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private string PRESEAENTRANCEEXAMINSERTUPDATE(string input, int RowUserCode)
    {
        string Id = null;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixPreSeaEntranceExam.PreSeaEntranceExamInsertUpdate(RowUserCode, input,ref Id);

            return ("<ID>" + Id + "</ID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private string PRESEATRAINEEINSERTUPDATE(string input, int RowUserCode)
    {
        string empId = null;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixPreSeaTraineePersonal.PreSeaTraineeInsertUpdate(RowUserCode, input,ref empId);
            return ("<EMPID>" + empId + "</EMPID>");
            //return "Successful";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string PRESEAEMPLOYEEADDRESSINSERTUPDATE(string input, int RowUserCode)
    {
        string addressId = null;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixPreSeaTraineePersonal.PreSeaPersonalAddressInsertUpdate(RowUserCode, input, ref addressId);
            return ("<EMPID>" + addressId + "</EMPID>");
            //return "Successful";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string PRESEAINTERVIEWSUMMARYINSERTUPDATE(string input, int RowUserCode)
    {
        string interviewId = null;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixPreSeaEntranceExam.PreSeaInterviewSummaryInsertUpdate(RowUserCode, input,ref interviewId);

            return ("<INTERVIEWID>" + interviewId + "</INTERVIEWID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string PRESEAINTERVIEWACADEMICMARKS(string input, int RowUserCode)
    {
        string interviewdataId = null;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixPreSeaEntranceExam.PreSeaAcademicScoreCardInsertUpdate(RowUserCode, input,ref interviewdataId);

            return ("<INTERVIEWDATAID>" + interviewdataId + "</INTERVIEWDATAID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private string PRESEAINTERVIEWRITTENEXAMMARKS(string input, int RowUserCode)
    {
        string interviewdataId = null;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixPreSeaEntranceExam.PreSeaWrittenScoreCardInsertUpdate(RowUserCode, input,ref interviewdataId);

            return ("<INTERVIEWDATAID>" + interviewdataId + "</INTERVIEWDATAID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string PRESEAINTERVIEPERSONALITYEXAMMARKS(string input, int RowUserCode)
    {
        string interviewdataId = null;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixPreSeaEntranceExam.PreSeaPersonalityScoreCardInsertUpdate(RowUserCode, input,ref interviewdataId);

            return ("<INTERVIEWDATAID>" + interviewdataId + "</INTERVIEWDATAID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string POPULATECITY()
    {
        try
        {
            DataSet ds = PhoenixRegistersCity.ListCity(null, null);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string POPULATESTATE(int countryId)
    {
        try
        {
            DataSet ds = PhoenixRegistersState.ListState(1,countryId);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string POPULATECOUNTRY()
    {
        try
        {
            DataSet ds = PhoenixRegistersCountry.ListCountry(1);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string POPULATESCORECSRDFIELDS(int scorecardId)
    {
        try
        {
            DataSet ds = PhoenixPreSeaScoreCardTemplate.ListPreSeaScoreCardFields(scorecardId);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
   private string INTERVIEWSCORECARDINSERTUPDATE(string input, int RowUserCode)
    {
        string interviewdataId = null;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixPreSeaEntranceExam.PreSeaInterviewScoreCardInsertUpdate(RowUserCode, input,ref interviewdataId);

            return ("<INTERVIEWDATAID>" + interviewdataId + "</INTERVIEWDATAID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
         
}