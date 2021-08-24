<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Data;
using System.Data.SqlClient;
using System.Xml;


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
        input = (new System.IO.StreamReader(request.InputStream, Encoding.UTF8)).ReadToEnd().ToString();
        input = server.UrlDecode(server.UrlEncode(input));
        response.ClearContent();
        response.Buffer = true;
        response.ContentType = "text/xml";

        context.Session["CURRENTDATABASE"] = null;

        if (strFunctionName.ToUpper() == "JOBLIST")
        {
            response.Write(Joblist());
        }
        else if (strFunctionName.ToUpper() == "JOBSTATUS")
        {
            response.Write(Jobstatuslist());
        }
        else if (strFunctionName.ToUpper() == "COMPONENTSTATUSLIST")
        {
            response.Write(ComponentStatuslist(userCode));
        }
        else if (strFunctionName.ToUpper() == "RESPONSIBILITY")
        {
            response.Write(Responsibilitylist());
        }
        else if (strFunctionName.ToUpper() == "CALENDARFREQUENCY")
        {
            response.Write(CALENDARlist(7, userCode));
        }
        else if (strFunctionName.ToUpper() == "HOURFREQUENCY")
        {
            response.Write(CALENDARlist(111, userCode));
        }
        else if (strFunctionName.ToUpper() == "MAINTENANCETYPE")
        {
            response.Write(QuickListlist(32));
        }
        else if (strFunctionName.ToUpper() == "MAINTENANCECLASS")
        {
            response.Write(QuickListlist(30));
        }
        else if (strFunctionName.ToUpper() == "HISTORYTEMPLATE")
        {
            response.Write(HistoryTemplateListlist());
        }
        else if (strFunctionName.ToUpper() == "UNITLIST")
        {
            response.Write(UNITLIST(input, userCode));
        }
        else if (strFunctionName.ToUpper() == "GETVESSEL")
        {
            response.Write(GETVESSEL(input, userCode));
        }
        if (strFunctionName.ToUpper() == "VENDORLIST")
        {
            response.Write(VendorList());
        }
        else if (strFunctionName.ToUpper() == "MAKERLIST")
        {
            response.Write(MAKERLIST());
        }
        else if (strFunctionName.ToUpper() == "PLANNINGMETHOD")
        {
            response.Write(PLANNINGMETHOD(5,userCode));

        }
        else if (strFunctionName.ToUpper() == "MAINTENANCECLAIM")
        {
            response.Write(MAINTENANCECLAIM(29));

        }
        else if (strFunctionName.ToUpper() == "MAINTENANCEFORM")
        {
            response.Write(HistoryTemplatelist());

        }
        else if (strFunctionName.ToUpper() == "INSERTSPARE")
        {
            response.Write(InsertSpare(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "INSERTCOMPONENT")
        {
            response.Write(InsertComponent(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "INSERTCOMPONENTCOUNTER")
        {
            response.Write(InsertComponentCounter(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "INSERTCOMPONENTSPARE")
        {
            response.Write(InsertComponentSpare(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "INSERTCOMPONENTJOB")
        {
            response.Write(InsertComponentJob(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "INSERTCOMPONENTJOBMAINTANANCEFORM")
        {
            response.Write(InsertComponentCoponentJobMaintananceForm(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "POPULATESPARE")
        {
            response.Write(PopulateSpare(input));

        }
        else if (strFunctionName.ToUpper() == "POPULATECOMPONENT")
        {
            response.Write(PopulateComponent(input));

        }
        else if (strFunctionName.ToUpper() == "POPULATECOMPONENTCOUNTER")
        {
            response.Write(PopulateComponentCounter(input));

        }
        else if (strFunctionName.ToUpper() == "POPULATECOMPONENTSPARE")
        {
            response.Write(PopulateComponentSpare(input));

        }
        else if (strFunctionName.ToUpper() == "POPULATECOMPONENTJOB")
        {
            response.Write(PopulateComponentJob(input));

        }
        else if (strFunctionName.ToUpper() == "POPULATECOMPONENTMAINTANANCEFORM")
        {
            response.Write(PopulateComponentJobMaintananceForm(input));

        }
        else if (strFunctionName.ToUpper() == "MIGRATECOMPONENT")
        {
            response.Write(MigrateComponent(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "MIGRATECOMPONENTJOB")
        {
            response.Write(MigrateComponentJob(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "MIGRATECOMJOBMAINTENANCEFORM")
        {
            response.Write(MigrateComponentJobMaintenanceForm(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "MIGRATESPARE")
        {
            response.Write(MigrateSpare(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "MIGRATECOMPONENTSPARE")
        {
            response.Write(MigrateComponentSpare(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "MIGRATECOMPONENTCOUNTER")
        {
            response.Write(MigrateComponentCounter(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "TRANSFERCOMPONENTMIGRATION")
        {
            response.Write(TransferComponentMigration(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "TRANSFERCOMPONENTJOBMIGRATION")
        {
            response.Write(TransferComponentJobMigration(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "TRANSFERSPAREITEMMIGRATION")
        {
            response.Write(TransferSpareItemMigration(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "TRANSFERSPAREITEMCOMPONENTMIGRATION")
        {
            response.Write(TransferSpareItemComponentMigration(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "TRANSFERCOMPONENTCOUNTERMIGRATION")
        {
            response.Write(TransferComponentCounterMigration(input, userCode));

        }
        else if (strFunctionName.ToUpper() == "TRANSFERMAINTENANCEFORMMIGRATION")
        {
            response.Write(TransferMaintenanceFormMigration(input, userCode));

        }
    }
    private string VendorList( )
    {

        string s=string.Empty;
        try
        {
            string addresstype = "130,131";
            DataSet ds = PhoenixRegistersAddress.ListAddress(addresstype);
            s = ds.GetXml().Replace("\r\n", "");
            return s;
        }
        catch (Exception ex)
        { 
            return ex.Message;
        }
    }
    
    private string MAKERLIST()
    {

        string s=string.Empty;
        try
        {
            string addresstype = "130,131";
            DataSet ds = PhoenixRegistersAddress.ListAddress(addresstype);
            s = ds.GetXml().Replace("\r\n", "");
            return s;
        }
        catch (Exception ex)
        { 
            return ex.Message;
        }
    }

    private string PLANNINGMETHOD(int code, int usercode)
    {
        try
        {
            DataSet ds = PhoneixPlannedMaintananceMigration.ListHard(usercode,code, 0, "", null);
            string s = ds.GetXml().Replace("\r\n", "");
            return s;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
        private string Jobstatuslist()
        {
            try
            {
                DataSet ds = PhoenixRegistersHard.ListHard(1, 13, 0, "NON,AVA,IUE,REP");
                string s = ds.GetXml().Replace("\r\n", "");
                return s;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string MAINTENANCECLAIM(int code)
         {
             try
             {
                 DataSet ds = PhoenixRegistersQuick.ListQuick(1, code);
                 string s = ds.GetXml().Replace("\r\n", "");
                 return s;
             }
             catch (Exception ex)
             {
                 return ex.Message;
             }
        }
    private string Joblist()
    {
        try
        {
            DataSet ds = PhoenixPlannedMaintenanceJob.ListJob();
            string s = ds.GetXml().Replace("\r\n","");
            return s;
        }
        catch(Exception ex) {
            return ex.Message;
        }
    }
    private string ComponentStatuslist(int usercode)
    {
        try
        {
            DataSet ds = PhoneixPlannedMaintananceMigration.ListHard(usercode,13, 0, "", null);
            string s = ds.GetXml().Replace("\r\n", "");
            return s;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string Responsibilitylist()
    {
        try
        {
            DataSet ds = SouthNests.Phoenix.Registers.PhoenixRegistersDiscipline.ListDiscipline();
            string s = ds.GetXml().Replace("\r\n", "");
            return s;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string CALENDARlist(int code,int userCode)
    {
        try
        {
            DataSet ds = PhoneixPlannedMaintananceMigration.ListHard(userCode, code, 0, "", null);
            string s = ds.GetXml().Replace("\r\n", "");
            return s;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string QuickListlist(int code)
    {
        try
        {
            DataSet ds = SouthNests.Phoenix.Registers.PhoenixRegistersQuick.ListQuick(1, code);
            string s = ds.GetXml().Replace("\r\n", "");
            return s;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private string HistoryTemplateListlist()
    {
        try
        {

            DataSet ds = PhoneixPlannedMaintananceMigration.ListHistoryTemplate();

            string s = ds.GetXml().Replace("\r\n", "");
            return s;
        }

        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private string HistoryTemplatelist()
    {
        try
        {
            DataSet ds = PhoneixPlannedMaintananceMigration.ListPlannedMaintenaceFormList();
            string s = ds.GetXml().Replace("\r\n", "");
            return s;
        }

        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    
    public string UNITLIST(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixRegistersUnit.ListUnit();
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string GETVESSEL(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.PMSDBGetVessel(input);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string InsertSpare(string input, int userCode)
    {

        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.InsertSpare(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string InsertComponent(string input, int userCode)
    {

        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.InsertComponent(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string InsertComponentSpare(string input, int userCode)
    {

        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.InsertComponentSpare(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string InsertComponentCounter(string input, int userCode)
    {

        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.InsertComponentCounter(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string InsertComponentCoponentJobMaintananceForm(string input, int userCode)
    {

        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.InsertComponentCoponentJobMaintananceForm(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string InsertComponentJob(string input, int userCode)
    {

        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.InsertComponentJob(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string PopulateComponent(string input)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.PopulateComponent(input);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string PopulateComponentJob(string input)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.PopulateComponentJob(input);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string PopulateSpare(string input)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.PopulateSpare(input);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string PopulateComponentCounter(string input)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.PopulateComponentCounter(input);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string PopulateComponentSpare(string input)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.PopulateComponentSpare(input);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string PopulateComponentJobMaintananceForm(string input)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.PopulateComponentJobMaintananceForm(input);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string MigrateComponent(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.MigrateComponent(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string MigrateComponentJob(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.MigrateComponentJob(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string MigrateComponentJobMaintenanceForm(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.MigrateComponentJobMaintenanceForm(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string MigrateSpare(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.MigrateSpare(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string MigrateComponentSpare(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.MigrateComponentSpare(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string MigrateComponentCounter(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.MigrateComponentCounter(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string TransferComponentMigration(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.TransferComponentMigration(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string TransferComponentJobMigration(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.TransferComponentJobMigration(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string TransferSpareItemMigration(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.TransferSpareItemMigration(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string TransferSpareItemComponentMigration(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.TransferSpareItemComponentMigration(input, userCode);
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
    public string TransferComponentCounterMigration(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.TransferComponentCounterMigration(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string TransferMaintenanceFormMigration(string input, int userCode)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoneixPlannedMaintananceMigration.TransferMaintenanceFormMigration(input, userCode);
            string S = ds.GetXml().Replace("\r\n", "");
            return (S);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}