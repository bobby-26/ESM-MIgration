<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Data;

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
        int Emoloyeeid = int.Parse(request.QueryString["employeeid"].ToString());
        input = server.UrlDecode((new System.IO.StreamReader(request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString();

        response.ClearContent();
        response.Buffer = true;
        response.ContentType = "text/xml";





        context.Session["CURRENTDATABASE"] = null;
        if (strFunctionName.ToUpper() == "SUPDTASSESSMENTUPDATE")
        {

            response.Write(SupdtAssessmentUpdate(input
                , int.Parse(request.QueryString["rowusercode"])
                , int.Parse(request.QueryString["EmployeeId"])
                , int.Parse(request.QueryString["SignOnOffId"])
                , General.GetNullableGuid(request.QueryString["AssessementId"])
                , General.GetNullableDateTime(request.QueryString["AssessementDate"])));
        }
        else if (strFunctionName.ToUpper() == "CREWASSESSMENTUPDATE")
        {

            response.Write(CrewAssessmentUpdate(input
                , int.Parse(request.QueryString["rowusercode"])
                , int.Parse(request.QueryString["EmployeeId"])
                , int.Parse(request.QueryString["SignOnOffId"])
                , General.GetNullableGuid(request.QueryString["AssessementId"])
                , General.GetNullableDateTime(request.QueryString["AssessementDate"])));
        }
        else if (strFunctionName.ToUpper() == "POPULATELICENCEDATA")
        {
            response.Write(POPULATELICENCEDATA(Emoloyeeid));
        }
        else if (strFunctionName.ToUpper() == "POPULATEGMDSSDATA")
        {
            response.Write(POPULATEGMDSSDATA(Emoloyeeid));
        }
        else if (strFunctionName.ToUpper() == "POPULATEENDROSMENTDATA")
        {
            response.Write(POPULATEENDROSMENTDATA(Emoloyeeid));
        }
        else if (strFunctionName.ToUpper() == "POPULATEGMDSSENDROSMENTDATA")
        {
            response.Write(POPULATEGMDSSENDROSMENTDATA(Emoloyeeid));
        }
        else if (strFunctionName.ToUpper() == "POPULATECOURSEDATA")
        {
            response.Write(POPULATECOURSEDATA(Emoloyeeid));
        }
        else if (strFunctionName.ToUpper() == "POPULATESTCWCOURSEDATA")
        {
            response.Write(POPULATESTCWCOURSEDATA(Emoloyeeid));
        }
        else if (strFunctionName.ToUpper() == "POPULATEOTHERTYPECOURSEDATA")
        {
            response.Write(POPULATEOTHERTYPECOURSEDATA(Emoloyeeid));
        }
        else if (strFunctionName.ToUpper() == "POPULATEVESSELTYPECOURSEDATA")
        {
            response.Write(POPULATEVESSELTYPECOURSEDATA(Emoloyeeid));
        }
        else if (strFunctionName.ToUpper() == "POPULATEFAMILYDETAILS")
        {
            response.Write(POPULATEFAMILYDETAILS(Emoloyeeid));
        }
        else if (strFunctionName.ToUpper() == "POPULATECOMPANYEXPERIENCE")
        {
            response.Write(POPULATECOMPANYEXPERIENCE(Emoloyeeid));
        }
        else if (strFunctionName.ToUpper() == "POPULATEVESSELTYPE")
        {
            response.Write(POPULATEVESSELTYPE(Emoloyeeid));
        }

    }
    private string SupdtAssessmentUpdate(string input, int RowUserCode, int employeeId, int SignOnOffId, Guid? AssessmentId, DateTime? AssessmentDate)
    {
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixCrewEmployeeSuptAssessment.SupdtAssessmentUpdate(RowUserCode, input, employeeId, SignOnOffId, AssessmentId, AssessmentDate);

            return "Successful";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string CrewAssessmentUpdate(string input, int RowUserCode, int employeeId, int SignOnOffId, Guid? AssessmentId, DateTime? AssessmentDate)
    {
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixCrewEmployeeSuptAssessment.CrewAssessmentUpdate(RowUserCode, input, employeeId, SignOnOffId, AssessmentId, AssessmentDate);

            return "Successful";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string POPULATELICENCEDATA(int Emoloyeeid)
    {
        try
        {
            DataSet ds = PhoenixCrewPersonal.EmpLicenceDetails(Emoloyeeid);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string POPULATEGMDSSDATA(int Emoloyeeid)
    {
        try
        {
            DataSet ds = PhoenixCrewPersonal.EmpGmdssDetails(Emoloyeeid);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string POPULATEENDROSMENTDATA(int Emoloyeeid)
    {
        try
        {
            DataSet ds = PhoenixCrewPersonal.EmpEndorsement(Emoloyeeid);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string POPULATEGMDSSENDROSMENTDATA(int Emoloyeeid)
    {
        try
        {
            DataSet ds = PhoenixCrewPersonal.EmpGmdssEndorsement(Emoloyeeid);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string POPULATECOURSEDATA(int Emoloyeeid)
    {
        try
        {
            DataSet ds = PhoenixCrewPersonal.EmpCourseCerificates(Emoloyeeid);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string POPULATESTCWCOURSEDATA(int Emoloyeeid)
    {
        try
        {
            DataSet ds = PhoenixCrewPersonal.EmpSTCWCourseCerificates(Emoloyeeid);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string POPULATEVESSELTYPECOURSEDATA(int Emoloyeeid)
    {
        try
        {
            DataSet ds = PhoenixCrewPersonal.EmpVesselTypeCourseCerificates(Emoloyeeid);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string POPULATEOTHERTYPECOURSEDATA(int Emoloyeeid)
    {
        try
        {
            DataSet ds = PhoenixCrewPersonal.EmpOtherTypeCourseCerificates(Emoloyeeid);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string POPULATEFAMILYDETAILS(int Emoloyeeid)
    {
        try
        {
            DataSet ds = PhoenixCrewPersonal.EmpFamilyDetails(Emoloyeeid);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

	public string POPULATEVESSELTYPE(int Emoloyeeid)
    {
        try
        {
            DataSet ds = PhoenixCrewPersonal.EmpVesseltypesearch(Emoloyeeid);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }



    public string POPULATECOMPANYEXPERIENCE(int Emoloyeeid)
    {
        try
        {
            DataSet ds = PhoenixCrewPersonal.EmpComapanyExperience(Emoloyeeid);
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