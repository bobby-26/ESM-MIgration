<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
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

        string strFunctionName = request.QueryString["methodname"].ToString();
        response.ClearContent();
        response.Buffer = true;
        response.ContentType = "text/xml";

        if (strFunctionName.ToUpper() == "ECOINSIGHTSEARCH")
        {
            response.Write(ECOInsightInterfaceDataSearch());
        }
        if (strFunctionName.ToUpper() == "ECOINSIGHTBUNKERDATASEARCH")
        {
            response.Write(ECOInsightInterfaceBunkerDataSearch());
        }
        if (strFunctionName.ToUpper() == "INSERTECOINSIGHTINTERFACEDATA")
        {
            response.Write(InsertECOInsightInterfaceData());
        }
        if (strFunctionName.ToUpper() == "UPDATEECOINSIGHTINTERFACEDATA")
        {
            response.Write(UpdateECOInsightInterfaceData());
        }
        if (strFunctionName.ToUpper() == "UPDATEECOINSIGHTINTERFACEEXPORT")
        {
            response.Write(UpdateECOInsightInterfaceExport());
        }
        if (strFunctionName.ToUpper() == "UPDATELATESTREPORT")
        {
            response.Write(UpdateLatestReport());
        }
        /// Daily Mail
        if (strFunctionName.ToUpper() == "DAILYMAILSEARCH")
        {
            response.Write(DailyMailDataSearch());
        }
        if (strFunctionName.ToUpper() == "DAILYMAILBUNKERDATASEARCH")
        {
            response.Write(DailyMailBunkerDataSearch());
        }
        if (strFunctionName.ToUpper() == "INSERTDAILYMAILDATA")
        {
            response.Write(InsertDailyMailData());
        }
        if (strFunctionName.ToUpper() == "UPDATEDAILYMAILDATA")
        {
            response.Write(UpdateDailyMailData());
        }
        if (strFunctionName.ToUpper() == "UPDATEDAILYMAILEXPORT")
        {
            response.Write(UpdateDailyMailExport());
        }
        if (strFunctionName.ToUpper() == "UPDATEDAILYMAILSEND")
        {
            response.Write(dailymailsend());
        }
    }

    private string dailymailsend()
    {
        string status = "No Data";
        string csvHeader =  DailyMailDataSearch();
        if (csvHeader != "")
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("../Attachments/TEMP/DailyReport.csv");
                string tomail = request.QueryString["tomail"];
                string ccmail = "";
                if (request.QueryString["ccmail"] != null)
                    ccmail = request.QueryString["ccmail"];

                System.IO.File.WriteAllText(path, csvHeader);

                string subject = "Daily Report";

                StringBuilder emailbody = new StringBuilder();
                emailbody.AppendLine("Dear Sir,");
                emailbody.AppendLine();
                emailbody.AppendLine("Please find the attached Daily Report data as CSV file.");
                emailbody.AppendLine();
                emailbody.AppendLine("Thank you");

                string[] strarrfilenames = new string[1];


                strarrfilenames[0] = path;

                PhoenixMail.SendMail(tomail.ToString().Replace(";", ",").Replace(",,", ",").TrimEnd(','),
                ccmail.Replace(";", ",").Replace(",,", ",").TrimEnd(','),
                "",
                subject,
                emailbody.ToString(), false,
                System.Net.Mail.MailPriority.Normal,
                "",
                strarrfilenames,
                null);

                status = "Mail Sent";
            }
            catch (Exception ex)
            {
                status = "Unable to send. Detail:" +ex.Message.ToString();
                return ex.Message;
            }
        }
        return status;
    }
    private string ECOInsightInterfaceDataSearch()
    {
        Guid Id = Guid.Empty;

        try
        {
            DataSet ds =  PhoenixVesselPositionECOInsightDataUpload.ECOInsightInterfaceDataSearch(General.GetNullableInteger(request.QueryString["VesselId"].ToString()));

            return  GetCSVData(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string ECOInsightInterfaceBunkerDataSearch()
    {
        Guid Id = Guid.Empty;

        try
        {
            DataSet ds =  PhoenixVesselPositionECOInsightDataUpload.ECOInsightInterfaceBunkerDataSearch(General.GetNullableInteger(request.QueryString["VesselId"].ToString()));

            return  GetCSVBunkerData(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string InsertECOInsightInterfaceData()
    {
        Guid Id = Guid.Empty;

        try
        {
            PhoenixVesselPositionECOInsightDataUpload.InsertECOInsightInterfaceData(General.GetNullableInteger(request.QueryString["VesselId"].ToString()),General.GetNullableDateTime(request.QueryString["Date"].ToString()),General.GetNullableInteger(request.QueryString["usercode"].ToString()));

            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string UpdateECOInsightInterfaceData()
    {
        Guid Id = Guid.Empty;

        try
        {
            PhoenixVesselPositionECOInsightDataUpload.UpdateECOInsightInterfaceData(General.GetNullableInteger(request.QueryString["VesselId"].ToString()), General.GetNullableDateTime(request.QueryString["Date"].ToString()), General.GetNullableInteger(request.QueryString["usercode"].ToString()));

            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string UpdateECOInsightInterfaceExport()
    {
        Guid Id = Guid.Empty;

        try
        {
            PhoenixVesselPositionECOInsightDataUpload.UpdateECOInsightInterfaceExport(General.GetNullableInteger(request.QueryString["VesselId"].ToString()), General.GetNullableInteger(request.QueryString["usercode"].ToString()));

            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string GetCSVData(DataTable dt)
    {
        string csvdata = "";

        if (dt.Rows.Count > 0)
        {
            csvdata = "IMO,Date_UTC,Date_Local,Time_UTC,Time_Local,Reporting_Time,Voyage_From,Voyage_To,Voyage_Number,ETA,Voyage_Type,Voyage_Leg,Voyage_Leg_Type,Latitude_Degree,Latitude_Minutes,Latitude_North_South,Longitude_Degree,Longitude_Minutes,Longitude_East_West,Position,Course,Wind_Dir,Wind_Force_Bft,Sea_state_Dir,Current_Dir,Current_Speed,Temperature_Ambient,Temperature_Water,Draft_Actual_Fore,Draft_Actual_Aft,Draft_Displacement_Actual,Cargo_Mt,Crew,Fresh_Water_Bunkered,Fresh_Water_Consumption_Drinking,Fresh_Water_Consumption_Technical,Fresh_Water_Consumption_Washing,Fresh_Water_Produced,Fresh_Water_ROB,Duration_Fresh_Water,HFO_HS_ROB,HFO_LS_ROB,MDO_MGO_HS_ROB,MDO_MGO_LS_ROB,ME_Cylinder_Oil_ROB,ME_System_Oil_ROB,AE_System_Oil_ROB,Sludge_ROB,ME_Fuel_BDN,ME_Fuel_BDN_2,ME_Fuel_BDN_3,ME_Fuel_BDN_4,AE_Fuel_BDN,AE_Fuel_BDN_2,AE_Fuel_BDN_3,AE_Fuel_BDN_4,Remarks,Entry_Made_By_1,Entry_Made_By_2,Event,Time_Since_Previous_Report,Time_Elapsed_Sailing,Time_Elapsed_Maneuvering,Time_Elapsed_Waiting,Time_Elapsed_Loading_Unloading,Distance,Distance_To_Go,Average_Propeller_Speed,Intended_Speed_Next_24Hrs,Speed_Projected_From_Charter_Party,Nominal_Slip,ME_Projected_Consumption,ME_Consumption,ME_Consumption_BDN_2,ME_Consumption_BDN_3,ME_Consumption_BDN_4,ME_Cylinder_Oil_Consumption,ME_System_Oil_Consumption,ME_1_Running_Hours,ME_1_Work,AE_Projected_Consumption,AE_Consumption,AE_Consumption_BDN_2,AE_Consumption_BDN_3,AE_Consumption_BDN_4,AE_System_Oil_Consumption,AE_1_Running_Hours,AE_1_Work,AE_2_Running_Hours,AE_2_Work,AE_3_Running_Hours,AE_3_Work,AE_4_Running_Hours,AE_4_Work,Boiler_Consumption,Boiler_Consumption_BDN_2,Boiler_Consumption_BDN_3,Boiler_Consumption_BDN_4,Mode,ME_1_Speed_RPM,ME_1_Charge_Air_Pressure,ME_1_TC_Speed,ME_1_Exh_Temp_Before_TC,ME_1_Exh_Temp_After_TC,ME_1_SFOC,AE_1_Load,AE_1_SFOC,AE_2_Load,AE_2_SFOC,AE_3_Load,AE_3_SFOC,AE_4_Load,AE_4_SFOC";
            foreach (DataRow dr in dt.Rows)
            {
                csvdata = csvdata + Environment.NewLine + dr["IMO"] + "," + dr["Date_UTC"] + "," + dr["Date_Local"] + "," + dr["Time_UTC"] + "," + dr["Time_Local"] + "," + dr["Reporting_Time"] + "," + dr["Voyage_From"] + "," + dr["Voyage_To"] + "," + dr["Voyage_Number"] + "," + dr["ETA"] + "," + dr["Voyage_Type"] + "," + dr["Voyage_Leg"] + "," + dr["Voyage_Leg_Type"] + "," + dr["Latitude_Degree"] + "," + dr["Latitude_Minutes"] + "," + dr["Latitude_North_South"] + "," + dr["Longitude_Degree"] + "," + dr["Longitude_Minutes"] + "," + dr["Longitude_East_West"] + "," + dr["Position"] + "," + dr["Course"] + "," + dr["Wind_Dir"] + "," + dr["Wind_Force_Bft"] + "," + dr["Sea_state_Dir"] + "," + dr["Current_Dir"] + "," + dr["Current_Speed"] + "," + dr["Temperature_Ambient"] + "," + dr["Temperature_Water"] + "," + dr["Draft_Actual_Fore"] + "," + dr["Draft_Actual_Aft"] + "," + dr["Draft_Displacement_Actual"] + "," + dr["Cargo_Mt"] + "," + dr["Crew"] + "," + dr["Fresh_Water_Bunkered"] + "," + dr["Fresh_Water_Consumption_Drinking"] + "," + dr["Fresh_Water_Consumption_Technical"] + "," + dr["Fresh_Water_Consumption_Washing"] + "," + dr["Fresh_Water_Produced"] + "," + dr["Fresh_Water_ROB"] + "," + dr["Duration_Fresh_Water"] + "," + dr["HFO_HS_ROB"] + "," + dr["HFO_LS_ROB"] + "," + dr["MDO_MGO_HS_ROB"] + "," + dr["MDO_MGO_LS_ROB"] + "," + dr["ME_Cylinder_Oil_ROB"] + "," + dr["ME_System_Oil_ROB"] + "," + dr["AE_System_Oil_ROB"] + "," + dr["Sludge_ROB"] + "," + dr["ME_Fuel_BDN"] + "," + dr["ME_Fuel_BDN_2"] + "," + dr["ME_Fuel_BDN_3"] + "," + dr["ME_Fuel_BDN_4"] + "," + dr["AE_Fuel_BDN"] + "," + dr["AE_Fuel_BDN_2"] + "," + dr["AE_Fuel_BDN_3"] + "," + dr["AE_Fuel_BDN_4"] + "," + dr["Remarks"] + "," + dr["Entry_Made_By_1"] + "," + dr["Entry_Made_By_2"] + "," + dr["Event"] + "," + dr["Time_Since_Previous_Report"] + "," + dr["Time_Elapsed_Sailing"] + "," + dr["Time_Elapsed_Maneuvering"] + "," + dr["Time_Elapsed_Waiting"] + "," + dr["Time_Elapsed_Loading_Unloading"] + "," + dr["Distance"] + "," + dr["Distance_To_Go"] + "," + dr["Average_Propeller_Speed"] + "," + dr["Intended_Speed_Next_24Hrs"] + "," + dr["Speed_Projected_From_Charter_Party"] + "," + dr["Nominal_Slip"] + "," + dr["ME_Projected_Consumption"] + "," + dr["ME_Consumption"] + "," + dr["ME_Consumption_BDN_2"] + "," + dr["ME_Consumption_BDN_3"] + "," + dr["ME_Consumption_BDN_4"] + "," + dr["ME_Cylinder_Oil_Consumption"] + "," + dr["ME_System_Oil_Consumption"] + "," + dr["ME_1_Running_Hours"] + "," + dr["ME_1_Work"] + "," + dr["AE_Projected_Consumption"] + "," + dr["AE_Consumption"] + "," + dr["AE_Consumption_BDN_2"] + "," + dr["AE_Consumption_BDN_3"] + "," + dr["AE_Consumption_BDN_4"] + "," + dr["AE_System_Oil_Consumption"] + "," + dr["AE_1_Running_Hours"] + "," + dr["AE_1_Work"] + "," + dr["AE_2_Running_Hours"] + "," + dr["AE_2_Work"] + "," + dr["AE_3_Running_Hours"] + "," + dr["AE_3_Work"] + "," + dr["AE_4_Running_Hours"] + "," + dr["AE_4_Work"] + "," + dr["Boiler_Consumption"] + "," + dr["Boiler_Consumption_BDN_2"] + "," + dr["Boiler_Consumption_BDN_3"] + "," + dr["Boiler_Consumption_BDN_4"] + "," + dr["Mode"] + "," + dr["ME_1_Speed_RPM"] + "," + dr["ME_1_Charge_Air_Pressure"] + "," + dr["ME_1_TC_Speed"] + "," + dr["ME_1_Exh_Temp_Before_TC"] + "," + dr["ME_1_Exh_Temp_After_TC"] + "," + dr["ME_1_SFOC"] + "," + dr["AE_1_Load"] + "," + dr["AE_1_SFOC"] + "," + dr["AE_2_Load"] + "," + dr["AE_2_SFOC"] + "," + dr["AE_3_Load"] + "," + dr["AE_3_SFOC"] + "," + dr["AE_4_Load"] + "," + dr["AE_4_SFOC"];
            }
        }
        return csvdata;
    }
    private string GetCSVBunkerData(DataTable dt)
    {
        string csvdata = "";

        if (dt.Rows.Count > 0)
        {
            csvdata = "BDN_Number,IMO,Fuel_type,Bunker_delivery_date,Mass,Sulphur_Content,Density_At_15dg";
            foreach (DataRow dr in dt.Rows)
            {
                csvdata = csvdata + Environment.NewLine + dr["BDN_Number"] + "," + dr["IMO"] + "," + dr["Fuel_type"] + "," + dr["Bunker_delivery_date"] + "," + dr["Mass"] + "," + dr["Sulphur_Content"] + "," + dr["Density_At_15dg"];
            }
        }
        return csvdata;
    }
    private string UpdateLatestReport()
    {
        Guid Id = Guid.Empty;

        try
        {
            PhoenixVesselPositionECOInsightDataUpload.UpdateLatestReport(1);

            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    /// <summary>
    /// /////////////////
    private string DailyMailDataSearch()
    {
        try
        {
            DataSet ds =  PhoenixVesselPositionECOInsightDataUpload.DailyMailDataSearch(General.GetNullableInteger(request.QueryString["VesselId"]),General.GetNullableDateTime(request.QueryString["fromdate"]));

            return  GetCSVData(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string  DailyMailBunkerDataSearch()
    {
        Guid Id = Guid.Empty;

        try
        {
            DataSet ds =  PhoenixVesselPositionECOInsightDataUpload.DailyMailBunkerDataSearch(General.GetNullableInteger(request.QueryString["VesselId"]),General.GetNullableDateTime(request.QueryString["fromdate"]));

            return  GetCSVBunkerData(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string InsertDailyMailData()
    {
        Guid Id = Guid.Empty;

        try
        {
            PhoenixVesselPositionECOInsightDataUpload.InsertDailyMailData(General.GetNullableInteger(request.QueryString["VesselId"]),General.GetNullableDateTime(request.QueryString["Date"]),General.GetNullableInteger(request.QueryString["usercode"]));

            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string UpdateDailyMailData()
    {
        Guid Id = Guid.Empty;

        try
        {
            PhoenixVesselPositionECOInsightDataUpload.UpdateDailyMailData(General.GetNullableInteger(request.QueryString["VesselId"]), General.GetNullableDateTime(request.QueryString["Date"]), General.GetNullableInteger(request.QueryString["usercode"]),General.GetNullableDateTime(request.QueryString["toDate"]));

            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    private string UpdateDailyMailExport()
    {
        Guid Id = Guid.Empty;

        try
        {
            PhoenixVesselPositionECOInsightDataUpload.UpdateDailyMailExport(General.GetNullableInteger(request.QueryString["VesselId"].ToString()), General.GetNullableInteger(request.QueryString["usercode"]),General.GetNullableDateTime(request.QueryString["fromdate"]));

            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    /// </summary>
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}