<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
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
        if (strFunctionName.ToUpper() == "AEMAINTENANCEREPORTINSERTUPDATE")
        {
            response.Write(AEMaintenaneReportInsert(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "AUXILIARYENGINEPERFORMANCEINSERT")
        {
            response.Write(AuxiliaryengineperformanceInsert(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "AE CR INSPECTION REPORT")
        {
            response.Write(AuxiliaryengineConnectingRodReport(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "MAINENGINEPISTONANDPISTONCALIBRATIONREPORT")
        {
            response.Write(MainEnginePistonAndPistonCalibrationReportInsert(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "MAINENGINERESTUFFINGBOXREPORT")
        {
            response.Write(MainEngineStuffingBoxReportInsert(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "MAINENGINECYLINDERCALIBRATIONINSERTUPDATE")
        {
            response.Write(MainEngineCylinderCalibrationInsert(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "MAINENGINEMAINTENANCEREPORTINSERTUPDATE")
        {
            response.Write(MAINENGINEMAINTENANCEREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "MAINENGINEMAINTENANCEREPORTEOPLINSERTUPDATE")
        {
            response.Write(MAINENGINEMAINTENANCEREPORTEOPLINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "MEINSPECTIONREPORTINSERTUPDATE")
        {
            response.Write(MAINENGINEINSPECTIONREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "AUXILIARYENGINEDECARBONISATIONINSERTUPDATE")
        {
            response.Write(AUXILIARYENGINEDECARBONISATIONREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "MEPERFORMANCEREPORTINSERTUPDATE")
        {
            response.Write(MAINENGINEPERFORMANCEREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "MEPERFORMANCEEOPLREPORTINSERTUPDATE")
        {
            response.Write(MAINENGINEPERFORMANCEEOPLREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "CARGOTANKINSPECTIONREPORTINSERTUPDATE")
        {
            response.Write(CARGOTANKINSPECTIONREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "BALLASTTANKINSPECTIONREPORTINSERTUPDATE")
        {
            response.Write(BALLASTTANKINSPECTIONREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "RHTURBOCHARGERREPORTINSERTUPDATE")
        {
            response.Write(RHTURBOCHARGERRECORDINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "TANKERVALVEMAINTENANCEREPORTINSERTUPDATE")
        {
            response.Write(TANKERVALVEMAINTENANCEREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "CASPACSYSTEMLOGREPORTINSERTUPDATE")
        {
            response.Write(CASPACSYSTEMLOGREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "PISTONRINGGAPMEASURMENTREPORTINSERTUPDATE")
        {
            response.Write(PISTONRINGGAPMEASURMENTREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "VALVEGREASINGMAINTENANCEREPORTINSERTUPDATE")
        {
            response.Write(VALVEGREASINGMAINTENANCEREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "MEEXHAUSTVALVEOVERHAULREPORTINSERTUPDATE")
        {
            response.Write(MEEXHAUSTVALVEOVERHAULREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "POTABLEWATERTESTRECORDINSERTUPDATE")
        {
            response.Write(POTABLEWATERTESTRECORDINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "MEGGERTESTREPORTINSERTUPDATE")
        {
            response.Write(MEGGERTESTREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "VIBRATIONMONITORINGPUMPINSERTUPDATE")
        {
            response.Write(VIBRATIONMONITORINGPUMPINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "VIBRATIONMONITORINGCOMPRESSORINSERTUPDATE")
        {
            response.Write(VIBRATIONMONITORINGCOMPRESSORINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "VIBRATIONMONITORINGPUMPBLOWERWITHBRNGHOUSINGINSERTUPDATE")
        {
            response.Write(VIBRATIONMONITORINGPUMPBLOWERWITHBRNGHOUSINGINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "OZONEDELPETINGSUBSTANCEINSERTUPDATE")
        {
            response.Write(OZONEDELPETINGSUBSTANCEINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "SCAVENGEINSPECTIONREPORTUECINSERTUPDATE")
        {
            response.Write(SCAVENGEINSPECTIONREPORTUECINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "CARGOTANKPASSIVITYTESTREPORTINSERTUPDATE")
        {
            response.Write(CARGOTANKPASSIVITYTESTREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "GASTANKERVALVEMAINTENANCEREPORTINSERTUPDATE")
        {
            response.Write(GASTANKERVALVEMAINTENANCEREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "SHAFTEARTHLOGINSERTUPDATE")
        {
            response.Write(SHAFTEARTHLOGINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "SCAVENGEINSPECTIONREPORTSULZERINSERTUPDATE")
        {
            response.Write(SCAVENGEINSPECTIONREPORTSULZERINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "HOLDCONDITIONREPORTINSERTUPDATE")
        {
            response.Write(HOLDCONDITIONREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "SCAVENGEINSPECTIONREPORTMANBWINSERTUPDATE")
        {
            response.Write(SCAVENGEINSPECTIONREPORTMANBWINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "CRANKWEBDEFLECTIONREPORTINSERTUPDATE")
        {
            response.Write(CRANKWEBDEFLECTIONREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "BEARINGMEASURINGREPORTINSERTUPDATE")
        {
            response.Write(BEARINGMEASURINGREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "WEEKLYSAFETYREPORTINSERTUPDATE")
        {
            response.Write(WEEKLYSAFETYREPORTINSERTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "SIMPLEJOBREPORTUPDATE")
        {
            response.Write(SIMPLEJOBREPORTUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "AEPERFORMANCER2UPDATE")
        {
            response.Write(AEPERFORMANCER2UPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "MONTHLYMEPERFORMANCEUPDATE")
        {
            response.Write(MONTHLYMEPERFORMANCEUPDATE(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "WORKORDERRAUPDATE")
        {
            response.Write(WORKORDERRAUPDATE(input, userCode));
        }

        if (strFunctionName.ToUpper() == "RAEXPORT2XL")
        {
            RAExport2XL(input);
        }
        if(strFunctionName.ToUpper()=="AEPERFORMANCER3UPDATE")
        {
            response.Write(AEPERFORMANCER3UPDATE(input, userCode));
        }

        if(strFunctionName.ToUpper()=="PAINTSTOCKUPDATE")
        {
            response.Write(PAINTSTOCKUPDATE(input, userCode));
        }
        if (strFunctionName.ToUpper() == "HOLDINSPECTIONREPORTINSERTUPDATE")
        {
            response.Write(HOLDINSPECTIONREPORTINSERTUPDATE(input, userCode));
        }
        if (strFunctionName.ToUpper() == "PERIODICALCHECKLISTREPORTINSERTUPDATE")
        {
            response.Write(PERIODICALCHECKLISTREPORTINSERTUPDATE(input, userCode));
        }
    }

    protected void RAExport2XL(string input)
    {
        string exportoption = request.QueryString["exportoption"].ToString();


        if (exportoption.ToUpper().Equals("RA"))
        {
            //string projectid = request.QueryString["projectid"].ToString();
            string WORKORDERID = request.QueryString["workorderid"].ToString();
            string vesselId = request.QueryString["vesselId"] != null ? request.QueryString["vesselId"].ToString() : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            string ReportId = request.QueryString["ReportID"].ToString();
            string PDate = request.QueryString["PostponeDate"].ToString();
            string PId = request.QueryString["Pid"].ToString();
            PhoenixPMS2XL.Export2XLWorkOrderRA(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(ReportId)
                                                                , General.GetNullableInteger(vesselId)
                                                                , General.GetNullableGuid(WORKORDERID)
                                                                ,PDate
                                                                ,General.GetNullableGuid(PId));
        }
    }
    private string AEMaintenaneReportInsert(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoneixPlannedMaintananceReports.AEMaintenanceXML(userCode, input, ref Id);

            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }


    }
    private string AuxiliaryengineperformanceInsert(string input, int userCode)
    {
        Guid g = Guid.Empty;
        string s=string.Empty;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.AuInsert(input, userCode, ref g);
            return ("<GUID>" + g.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string AuxiliaryengineConnectingRodReport(string input, int userCode)
    {
        Guid g = Guid.Empty;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.AuConnectionRod(input, userCode, ref g);
            return ("<GUID>" + g.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string MainEnginePistonAndPistonCalibrationReportInsert(string input, int userCode)
    {
        Guid g = Guid.Empty;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.MePistonCalibrationReportInsert(input, userCode, ref g);
            return ("<GUID>" + g.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string MainEngineStuffingBoxReportInsert(string input, int userCode)
    {
        Guid g = Guid.Empty;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.SbrInsert(input, userCode, ref g);
            return ("<GUID>" + g.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string MainEngineCylinderCalibrationInsert(string input, int userCode)
    {
        Guid Id = Guid.Empty;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);


            PhoneixPlannedMaintananceReports.MECylinderCalibrationXML(userCode, input, ref Id);

            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string MAINENGINEMAINTENANCEREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);


            PhoneixPlannedMaintananceReports.MEMaintenanceReportXML(userCode, input, ref Id);

            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string MAINENGINEMAINTENANCEREPORTEOPLINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);


            PhoneixPlannedMaintananceReports.MEMaintenanceReportEOPLXML(userCode, input, ref Id);

            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string MAINENGINEINSPECTIONREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;
        string s=string.Empty;
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.MEInspectionInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string AUXILIARYENGINEDECARBONISATIONREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.AEDecarbonisationInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string MAINENGINEPERFORMANCEREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.MEPerformanceInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string MAINENGINEPERFORMANCEEOPLREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.MEPerformanceEoplInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string CARGOTANKINSPECTIONREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.CargoTankInspectionInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string BALLASTTANKINSPECTIONREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.BallastTankInspectionInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string RHTURBOCHARGERRECORDINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.RHTurbochargerInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string TANKERVALVEMAINTENANCEREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.TankerValvesMaintenanceInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string CASPACSYSTEMLOGREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.CASPACSystemLogInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string PISTONRINGGAPMEASURMENTREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.MEPistonRingGapMeasurementInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string VALVEGREASINGMAINTENANCEREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.ValveGreasingandMaintenanceInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string MEEXHAUSTVALVEOVERHAULREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.MainEngineExhaustValveOverhaulInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string POTABLEWATERTESTRECORDINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.POTableWaterTestRecordInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string MEGGERTESTREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.MeggerTestReportInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string VIBRATIONMONITORINGPUMPINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.VibrationMonitoringPumpInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string VIBRATIONMONITORINGCOMPRESSORINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.VibrationMonitoringCompressorInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string VIBRATIONMONITORINGPUMPBLOWERWITHBRNGHOUSINGINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.VibrationMonitoringPumpBlowerInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string OZONEDELPETINGSUBSTANCEINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.OzonedepletingInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string SCAVENGEINSPECTIONREPORTUECINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.ScavengeinspectionreportUECInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string CARGOTANKPASSIVITYTESTREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.CargoTankPassivityTestReportInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string GASTANKERVALVEMAINTENANCEREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.GasTankerValveMaintenanceInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string SHAFTEARTHLOGINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.ShaftEarthLogInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string SCAVENGEINSPECTIONREPORTSULZERINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.ScavengeinspectionreportSULZERInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string HOLDCONDITIONREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.HoldConditionReportInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string SCAVENGEINSPECTIONREPORTMANBWINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.ScavengeinspectionreportMANBWInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string CRANKWEBDEFLECTIONREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.CrankwebsDeflectionInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string BEARINGMEASURINGREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.BearingMeasuringReportInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string WEEKLYSAFETYREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.WeeklySafetyInsertUpdate(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string SIMPLEJOBREPORTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.SimpleJobsReportInsertUpdate(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string AEPERFORMANCER2UPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.AEperformanceR2InsertUpdate(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string MONTHLYMEPERFORMANCEUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.MonthlyMEperformInserUpdate(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string WORKORDERRAUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.WorkOrderRAInsertUpdate(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string AEPERFORMANCER3UPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.AEperformanceR3InsertUpdate(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string PAINTSTOCKUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.InventoryPaintStockInsertUpdate(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string HOLDINSPECTIONREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.HoldInspectionInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string PERIODICALCHECKLISTREPORTINSERTUPDATE(string input, int userCode)
    {
        Guid Id = Guid.Empty;

        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);
            PhoneixPlannedMaintananceReports.PeriodicalCheckListInsert(input, userCode, ref Id);
            return ("<GUID>" + Id.ToString() + "</GUID>");
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