using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.Spreadsheet;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Web.UI;
using System.Collections.Specialized;
using SouthNests.Phoenix.Elog;
using System.Linq;
using SouthNests.Phoenix.Reports;
using System.Web;

public partial class Log_ElecticLogMainEngineLogV2 : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    public string newval = "";
    public string param = "";
    DateTime date = DateTime.Now;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        RadScriptManager1.RegisterPostBackControl(lnkPdf);
        RadScriptManager1.RegisterPostBackControl(lnkPdfall);
        if (IsPostBack == false)
        {
            ViewState["RowCount"] = 0;
            ViewState["engine2yn"] = 0;
            ViewState["Groupfunction"] = "";
            // LoadLogSelect();



            lblvesselname.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
            if (Request.QueryString["date"] != null && General.GetNullableDateTime(Request.QueryString["date"].ToString()) != null)
            {
                lbldate.Text = Request.QueryString["date"];
                txtdate.SelectedDate = General.GetNullableDateTime(Request.QueryString["date"].ToString());
                txtdate.MaxDate = DateTime.Now;
            }
            else
            {
                txtdate.SelectedDate = DateTime.Now;
                txtdate.MaxDate = DateTime.Now;
                lbldate.Text = txtdate.SelectedDate.Value.ToString("dd-MM-yyyy");
            }
            LoadExcelTemplate(lbldate.Text);
            //BindLogList();
            LoadValidation();
            Loadhide();
            hidemenu();
            Accessrights();
        }

        //if (SouthNests.Phoenix.Framework.Filter.CurrentEngineLogFilterCriteria != null)
        //{
        //    //LoadExcelTemplate(lbldate.Text);
        //    //string script = string.Format("refreshScroll()");
        //    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scroll", script, true);
        //    SouthNests.Phoenix.Framework.Filter.CurrentEngineLogFilterCriteria = null;
        //    Response.Redirect("../Log/ElecticLogMainEngineLogV2.aspx?date=" + lbldate.Text);
        //}

        //if (SouthNests.Phoenix.Framework.Filter.CheifEngineerSignatureFilterCriteria != null)
        //{
        //    //LoadExcelTemplate(lbldate.Text);
        //    //NameValueCollection nvc = SouthNests.Phoenix.Framework.Filter.CheifEngineerSignatureFilterCriteria;
        //    //hdnislock.Value = nvc["islock"];
        //    //string script = string.Format("refreshScroll()");
        //    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scroll", script, true);
        //    SouthNests.Phoenix.Framework.Filter.CheifEngineerSignatureFilterCriteria = null;
        //    Response.Redirect("../Log/ElecticLogMainEngineLogV2.aspx?date=" + lbldate.Text);
        //}

        SaveExcelToDB();

    }
    private string GetRank()
    {
        DataSet ds = PhoenixElog.GetSeaFarerRankName(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            return row["FLDRANKCODE"].ToString();
        }
        return string.Empty;
    }
    private string getfunctionname()
    {
        string rank = GetRank();
        string groupname = "";
        DataSet ds = PhoenixElog.GetGrouprankFunctionCode(rank);
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["FLDFUNCTIONCODE"].ToString() == "OOW" || dr["FLDFUNCTIONCODE"].ToString() == "CE" || dr["FLDFUNCTIONCODE"].ToString() == "DE" || dr["FLDFUNCTIONCODE"].ToString() == "MST")
                    groupname = dr["FLDFUNCTIONCODE"].ToString();
            }
        }
        return groupname;
    }
    private void Accessrights()
    {
        string groupname = getfunctionname();
        ViewState["Groupfunction"] = groupname;
        if (groupname == "OOW" || groupname == "CE" || groupname == "DE")
        {
            hdnallowentry.Value = "1";
            hdnallowWatchsign.Value = "1";
        }
        else
        {
            hdnallowentry.Value = "0";
            hdnallowWatchsign.Value = "0";
        }

        if (PhoenixSecurityContext.CurrentSecurityContext.UserCode == 1)
        {
            hdnallowentry.Value = "1";
        }
        if (groupname == "CE")
            hdnallowdaysign.Value = "1";
        else
            hdnallowdaysign.Value = "0";

    }
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        //LoadExcelTemplate(lbldate.Text);
        //  SaveExcelToDB();
    }
    private void LoadValidation()
    {

        //int iRowCount = 0;
        //int iTotalPageCount = 0;

        //DataSet ds = PhoenixEngineLogAttributes.EngineParameterConfigSearch(null, 0, 1,
        //    1000, ref iRowCount, ref iTotalPageCount, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        DataSet ds = PhoenixEngineLogAttributes.EngineLogParameterList();
        string json = "";
        // dataset
        // for
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                json = json + (i > 0 ? "," : "") + "{\"field\": \"" + dr["FLDENGINELOGPARAMETER"].ToString() + "\", \"min\": \"" + dr["FLDMINVAL"].ToString() + "\", \"max\":\"" + dr["FLDMAXVAL"].ToString() + "\", \"minalert\":\"" + dr["FLDMINALERT"].ToString() + "\", \"maxalert\":\"" + dr["FLDMAXALERT"].ToString() + "\"}";

            }
        }
        json = "[" + json + "]";
        hdnValidateStatus.Value = json;

    }

    private void SaveExcelToDB()
    {
        if (IsCallback && Request.Params["__CALLBACKID"] == RadSpreadsheet1.UniqueID)
        {
            ViewState["excel"] = Request.Params["__CALLBACKPARAM"];
            SaveExcelTemplate(ViewState["excel"].ToString(), "meparam", 1);
            GetMeParamData2DataTable((string)ViewState["excel"]);
        }

        if (IsCallback && Request.Params["__CALLBACKID"] == RadSpreadsheet2.UniqueID)
        {
            ViewState["excel"] = Request.Params["__CALLBACKPARAM"];
            SaveExcelTemplate(ViewState["excel"].ToString(), "metemp", 2);
            GetMETempData2DataTable((string)ViewState["excel"]);

        }


        if (IsCallback && Request.Params["__CALLBACKID"] == RadSpreadsheet3.UniqueID)
        {
            ViewState["excel"] = Request.Params["__CALLBACKPARAM"];
            SaveExcelTemplate(ViewState["excel"].ToString(), "turbocharger", 3);
            GetMETurboChargerData2DataTable((string)ViewState["excel"]);
        }

        if (IsCallback && Request.Params["__CALLBACKID"] == RadSpreadsheet9.UniqueID)
        {
            ViewState["excel"] = Request.Params["__CALLBACKPARAM"];
            SaveExcelTemplate(ViewState["excel"].ToString(), "meparam2", 4);
            GetMeParamData2DataTable2((string)ViewState["excel"]);
        }

        if (IsCallback && Request.Params["__CALLBACKID"] == RadSpreadsheet10.UniqueID)
        {
            ViewState["excel"] = Request.Params["__CALLBACKPARAM"];
            SaveExcelTemplate(ViewState["excel"].ToString(), "metemp2", 5);
            GetMETempData2DataTable2((string)ViewState["excel"]);
        }


        if (IsCallback && Request.Params["__CALLBACKID"] == RadSpreadsheet11.UniqueID)
        {
            ViewState["excel"] = Request.Params["__CALLBACKPARAM"];
            SaveExcelTemplate(ViewState["excel"].ToString(), "turbocharger2", 6);
            GetMETurboChargerData2DataTable2((string)ViewState["excel"]);
        }

        if (IsCallback && Request.Params["__CALLBACKID"] == RadSpreadsheet4.UniqueID)
        {
            ViewState["excel"] = Request.Params["__CALLBACKPARAM"];
            SaveExcelTemplate(ViewState["excel"].ToString(), "generator", 7);
            GetGeneratorData2DataTable((string)ViewState["excel"]);
        }

        if (IsCallback && Request.Params["__CALLBACKID"] == RadSpreadsheet5.UniqueID)
        {
            ViewState["excel"] = Request.Params["__CALLBACKPARAM"];
            SaveExcelTemplate(ViewState["excel"].ToString(), "misc", 8);
            GetMISCData2DataTable((string)ViewState["excel"]);
            GetHeatData2DataTable((string)ViewState["excel"]);
        }

        if (IsCallback && Request.Params["__CALLBACKID"] == RadSpreadsheet6.UniqueID)
        {
            ViewState["excel"] = Request.Params["__CALLBACKPARAM"];
            SaveExcelTemplate(ViewState["excel"].ToString(), "aircond", 9);
            GetAIRData2DataTable((string)ViewState["excel"]);
        }

        if (IsCallback && Request.Params["__CALLBACKID"] == RadSpreadsheet7.UniqueID)
        {
            ViewState["excel"] = Request.Params["__CALLBACKPARAM"];

            SaveExcelTemplate(ViewState["excel"].ToString(), "noonreport", 10);
            GetNoonReportData2DataTable((string)ViewState["excel"]);
        }

        if (IsCallback && Request.Params["__CALLBACKID"] == RadSpreadsheet8.UniqueID)
        {
            ViewState["excel"] = Request.Params["__CALLBACKPARAM"];
            SaveExcelTemplate(ViewState["excel"].ToString(), "remarks", 11);
            GetMiscRemarksData2DataTable((string)ViewState["excel"]);
            GetNoonRemarksData2DataTable((string)ViewState["excel"]);
        }
    }

    private void Loadhide()
    {
        DataSet ds = PhoenixEngineLogAttributes.EngineLogAttributesActiveList(1, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        string json = "";
        // dataset
        // for
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                json = json + (i > 0 ? "," : "") + "{\"field\": \"" + dr["FLDSHORTCODE"].ToString() + "\", \"Value\": \"" + dr["FLDQUANTITY"].ToString() + "\"}";

                if (dr["FLDSHORTCODE"].ToString() == "MAE" && General.GetNullableInteger(dr["FLDQUANTITY"].ToString()) > 1)
                {
                    ViewState["engine2yn"] = 1;
                }
            }
        }
        json = "[" + json + "]";
        hdnmachinery.Value = json;
    }
    private void hidemenu()
    {
        if (ViewState["engine2yn"].ToString() == "0")
        {
            trme2head.Visible = false;
            trme2param.Visible = false;
            trme2temp.Visible = false;
            trme2turbocharger.Visible = false;
            trempty.Visible = false;
            trme1head.Visible = false;
            meparam2.Visible = false;
            metemp2.Visible = false;
            turbocharger2.Visible = false;
            RadSpreadsheet9.Attributes.Add("style", "display:none;");
            RadSpreadsheet10.Attributes.Add("style", "display:none;");
            RadSpreadsheet11.Attributes.Add("style", "display:none;");
        }
    }
    private void SaveExcelTemplate(string json, string logName, int sortOrder)
    {
        Telerik.Web.Spreadsheet.Workbook workbook = new Telerik.Web.Spreadsheet.Workbook();
        PhoenixEngineLog.MainEngineLogInsert(usercode, vesselId, sortOrder, json, txtdate.SelectedDate.Value, logName);
    }

    private void PdfDownload(string date)
    {
        if (ViewState["Groupfunction"].ToString() == "MST")
            return;
        if (string.IsNullOrWhiteSpace(date))
        {
            ucError.ErrorMessage = "Please select a valid date";
            ucError.Visible = true;
            return;
        }

        string Tmpfilelocation = string.Empty; string[] reportfile = new string[0];
        string filename = "";
        DataSet ds;
        NameValueCollection nvc = new NameValueCollection();
        nvc.Add("applicationcode", "24");
        nvc.Add("reportcode", "ENGINELOG3");
        nvc.Add("CRITERIA", "");
        Session["PHOENIXREPORTPARAMETERS"] = nvc;

        ds = PhoenixEngineLogParameter.EngineLogMEReport(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableDateTime(date));

        Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
        filename = "ENGINELOG_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".pdf";
        Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);

        PhoenixSsrsReportsCommon.getVersion();
        PhoenixSsrsReportsCommon.getLogo();
        PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
    }

    private void LoadExcelTemplate(string date)
    {
        int me2exists = 0;
        Telerik.Web.Spreadsheet.Workbook workbook;
        DataSet ds = null;
        if (string.IsNullOrWhiteSpace(date) == false)
        {
            ds = PhoenixEngineLog.MainEngineLogSearch(usercode, vesselId, DateTime.Parse(date));
        }
        hdnislock.Value = "0";
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            hdnislock.Value = ds.Tables[0].Rows[0]["FLDISLOCKED"].ToString();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string exceljson = row["FLDEXCELJSON"].ToString();
                if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "meparam")
                {
                    workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(exceljson);
                    RadSpreadsheet1.Sheets.Add(workbook.Sheets[0]);
                   
                }

                if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "metemp")
                {
                    workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(exceljson);
                    RadSpreadsheet2.Sheets.Add(workbook.Sheets[0]);
                    foreach (Worksheet item in workbook.Sheets)
                    {
                        Row rows = item.Rows[3];
                        List<Cell> cells = rows.Cells.GetRange(2, 18);
                        foreach (Cell cell in cells)
                        {
                            cell.Link = "#rotate";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "turbocharger")
                {
                    workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(exceljson);
                    RadSpreadsheet3.Sheets.Add(workbook.Sheets[0]);
                }

                if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "meparam2" && ViewState["engine2yn"].ToString() == "1")
                {
                    me2exists = 1;
                    workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(exceljson);
                    RadSpreadsheet9.Sheets.Add(workbook.Sheets[0]);
                }

                if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "metemp2" && ViewState["engine2yn"].ToString() == "1")
                {
                    me2exists = 1;
                    workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(exceljson);
                    RadSpreadsheet10.Sheets.Add(workbook.Sheets[0]);
                    foreach (Worksheet item in workbook.Sheets)
                    {
                        Row rows = item.Rows[3];
                        List<Cell> cells = rows.Cells.GetRange(2, 18);
                        foreach (Cell cell in cells)
                        {
                            cell.Link = "#rotate";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "turbocharger2" && ViewState["engine2yn"].ToString() == "1")
                {
                    me2exists = 1;
                    workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(exceljson);
                    RadSpreadsheet11.Sheets.Add(workbook.Sheets[0]);
                }

                if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "generator")
                {
                    workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(exceljson);
                    RadSpreadsheet4.Sheets.Add(workbook.Sheets[0]);
                }

                if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "misc")
                {
                    workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(exceljson);
                    RadSpreadsheet5.Sheets.Add(workbook.Sheets[0]);
                }

                if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "aircond")
                {
                    workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(exceljson);
                    RadSpreadsheet6.Sheets.Add(workbook.Sheets[0]);
                }

                if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "noonreport")
                {
                    workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(exceljson);
                    RadSpreadsheet7.Sheets.Add(workbook.Sheets[0]);
                    foreach (Worksheet item in workbook.Sheets)
                    {
                        //Row rowhead = item.Rows[26];
                        //List<Cell> cellshead = rowhead.Cells.GetRange(21, 24);
                        //foreach (Cell cell in cellshead)
                        //{
                        //    cell.Link = "#rotate";

                        //}

                        Row rows = item.Rows[27];
                        List<Cell> cells = rows.Cells.GetRange(5, 20);
                        foreach (Cell cell in cells)
                        {
                            cell.Link = "#rotate";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(exceljson) && (String)row["FLDLOGNAME"] == "remarks")
                {
                    workbook = Telerik.Web.Spreadsheet.Workbook.FromJson(exceljson);
                    RadSpreadsheet8.Sheets.Add(workbook.Sheets[0]);
                }
            }
            //return;
        }
        string file = Server.MapPath("~/Template/Log/tables_englog_2.3.xlsx");
        SpreadsheetDocumentProvider provider = new SpreadsheetDocumentProvider(file);
        workbook = new Telerik.Web.Spreadsheet.Workbook();
        workbook.Sheets = provider.GetSheets();
        if (ds == null || (ds.Tables[0].Rows.Count <= 0))
        {
            foreach (Worksheet item in workbook.Sheets)
            {
                if (item.Name == "metemp")
                {
                    Row row = item.Rows[3];
                    List<Cell> cells = row.Cells.GetRange(2, 18);
                    foreach (Cell cell in cells)
                    {
                        cell.Link = "#rotate";
                    }
                }
                if (item.Name == "metemp2" && ViewState["engine2yn"].ToString() == "1")
                {
                    Row row = item.Rows[3];
                    List<Cell> cells = row.Cells.GetRange(2, 18);
                    foreach (Cell cell in cells)
                    {
                        cell.Link = "#rotate";
                    }
                }
                if (item.Name == "noonreport")
                {

                    //Row rowhead = item.Rows[26];
                    //List<Cell> cellshead = rowhead.Cells.GetRange(21, 24);
                    //foreach (Cell cell in cellshead)
                    //{
                    //    cell.Link = "#rotate";

                    //}

                    Row rows = item.Rows[27];
                    List<Cell> cells = rows.Cells.GetRange(5, 20);
                    foreach (Cell cell in cells)
                    {
                        cell.Link = "#rotate";
                    }
                }

            }
            RadSpreadsheet1.Sheets.Add(workbook.Sheets[0]);
            RadSpreadsheet2.Sheets.Add(workbook.Sheets[1]);
            RadSpreadsheet3.Sheets.Add(workbook.Sheets[2]);
            RadSpreadsheet4.Sheets.Add(workbook.Sheets[3]);
            RadSpreadsheet5.Sheets.Add(workbook.Sheets[4]);
            RadSpreadsheet6.Sheets.Add(workbook.Sheets[5]);
            RadSpreadsheet7.Sheets.Add(workbook.Sheets[6]);
            RadSpreadsheet8.Sheets.Add(workbook.Sheets[7]);

            if (ViewState["engine2yn"].ToString() == "1")
            {
                RadSpreadsheet9.Sheets.Add(workbook.Sheets[8]);
                RadSpreadsheet10.Sheets.Add(workbook.Sheets[9]);
                RadSpreadsheet11.Sheets.Add(workbook.Sheets[10]);
            }
        }
        if (ds != null && (ds.Tables[0].Rows.Count > 0) && ViewState["engine2yn"].ToString() == "1" && me2exists == 0)
        {
            foreach (Worksheet item in workbook.Sheets)
            {
                if (item.Name == "metemp2")
                {
                    Row row = item.Rows[3];
                    List<Cell> cells = row.Cells.GetRange(2, 18);
                    foreach (Cell cell in cells)
                    {
                        cell.Link = "#rotate";
                    }
                }
            }
            RadSpreadsheet9.Sheets.Add(workbook.Sheets[8]);
            RadSpreadsheet10.Sheets.Add(workbook.Sheets[9]);
            RadSpreadsheet11.Sheets.Add(workbook.Sheets[10]);
        }
    }

    private void GetMeParamData2DataTable(string json)
    {
        // get the spreadsheet
        Dictionary<int, string> rowAddress = new Dictionary<int, string>(); // config based on the templates
        rowAddress.Add(1, "FLDCELLINDEX");
        rowAddress.Add(2, "FLDWATCH");
        // DEFAULT TAG BASED ON THE SHEET
        rowAddress.Add(3, "FLDHOUR");
        rowAddress.Add(4, "FLDMINUTE");
        rowAddress.Add(6, "FLDCOUNTER");
        rowAddress.Add(7, "FLDRPM");
        rowAddress.Add(9, "FLDGOVERNOR");
        rowAddress.Add(10, "FLDCONTROLLOCATION");
        rowAddress.Add(12, "FLDFLOWMETER");
        rowAddress.Add(13, "FLDCONSUMPTION");
        rowAddress.Add(15, "FLDCYLOILLEVEL");
        rowAddress.Add(16, "FLDCYLOILCONS");
        rowAddress.Add(17, "FLDCMESUMP");
        string[] columns = rowAddress.Values.ToArray();

        DataTable meParamDt = CreateMEParamDataTable(columns);
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        LoadWatchTime(meParamDt, workbook);
        foreach (var rowAdd in rowAddress)
        {
            if (rowAdd.Key == 1 || rowAdd.Key == 2) continue;

            Row row = worksheet.Rows[rowAdd.Key];
            List<Cell> cells = row.Cells.GetRange(2, 6);
            foreach (Cell cell in cells)
            {
                string columnName = rowAdd.Value;
                string searchExpr = string.Format("FLDCELLINDEX = {0}", cell.Index);
                DataRow[] dtrows = meParamDt.Select(searchExpr);
                DataRow dtrow = dtrows[0];
                dtrow[columnName] = cell.Value;
            }
        }

        DataTable completedTable = meParamDt;
        InsertMeParam(completedTable, 1);
    }

    private void GetMeParamData2DataTable2(string json)
    {
        // get the spreadsheet
        Dictionary<int, string> rowAddress = new Dictionary<int, string>(); // config based on the templates
        rowAddress.Add(1, "FLDCELLINDEX");
        rowAddress.Add(2, "FLDWATCH");
        // DEFAULT TAG BASED ON THE SHEET
        rowAddress.Add(3, "FLDHOUR");
        rowAddress.Add(4, "FLDMINUTE");
        rowAddress.Add(6, "FLDCOUNTER");
        rowAddress.Add(7, "FLDRPM");
        rowAddress.Add(9, "FLDGOVERNOR");
        rowAddress.Add(10, "FLDCONTROLLOCATION");
        rowAddress.Add(12, "FLDFLOWMETER");
        rowAddress.Add(13, "FLDCONSUMPTION");
        rowAddress.Add(15, "FLDCYLOILLEVEL");
        rowAddress.Add(16, "FLDCYLOILCONS");
        rowAddress.Add(17, "FLDCMESUMP");
        string[] columns = rowAddress.Values.ToArray();

        DataTable meParamDt = CreateMEParamDataTable(columns);
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        LoadWatchTime(meParamDt, workbook);
        foreach (var rowAdd in rowAddress)
        {
            if (rowAdd.Key == 1 || rowAdd.Key == 2) continue;

            Row row = worksheet.Rows[rowAdd.Key];
            List<Cell> cells = row.Cells.GetRange(2, 6);
            foreach (Cell cell in cells)
            {
                string columnName = rowAdd.Value;
                string searchExpr = string.Format("FLDCELLINDEX = {0}", cell.Index);
                DataRow[] dtrows = meParamDt.Select(searchExpr);
                DataRow dtrow = dtrows[0];
                dtrow[columnName] = cell.Value;
            }
        }

        DataTable completedTable = meParamDt;
        InsertMeParam(completedTable, 2);
    }

    private void GetMETempData2DataTable(string json)
    {
        // get the spreadsheet
        Dictionary<int, string> rowAddress = new Dictionary<int, string>(); // config based on the templates
        rowAddress.Add(1, "FLDCELL");
        rowAddress.Add(2, "FLDWATCH");
        rowAddress.Add(4, "FLDOUT");
        rowAddress.Add(5, "FLDUNIT1");
        rowAddress.Add(6, "FLDUNIT2");
        rowAddress.Add(7, "FLDUNIT3");
        rowAddress.Add(8, "FLDUNIT4");
        rowAddress.Add(9, "FLDUNIT5");
        rowAddress.Add(10, "FLDUNIT6");
        rowAddress.Add(11, "FLDUNIT7");
        rowAddress.Add(12, "FLDUNIT8");
        rowAddress.Add(13, "FLDUNIT9");
        rowAddress.Add(14, "FLDUNIT10");
        rowAddress.Add(15, "FLDUNIT11");
        rowAddress.Add(16, "FLDUNIT12");
        rowAddress.Add(17, "FLDUNIT13");
        rowAddress.Add(18, "FLDUNIT14");
        rowAddress.Add(19, "FLDUNIT15");
        rowAddress.Add(20, "FLDUNIT16");
        string[] columns = rowAddress.Values.ToArray();

        DataTable Dt = CreateMETempDataTable(columns);
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        LoadMETempWatch(Dt, workbook);

        foreach (var rowAdd in rowAddress)
        {
            if (rowAdd.Key == 1 || rowAdd.Key == 2) continue;

            Row row = worksheet.Rows[rowAdd.Key];
            List<Cell> cells = row.Cells.GetRange(2, 18);
            foreach (Cell cell in cells)
            {
                string columnName = rowAdd.Value;

                string searchExpr = string.Format("FLDCELL ='{0}'", cell.Index);
                DataRow[] dtrows = new DataRow[100];
                dtrows = Dt.Select(searchExpr);
                DataRow dtrow = dtrows[0];
                dtrow[columnName] = cell.Value;
            }
        }

        DataTable completedTable = Dt;
        InsertTemparature(completedTable, 1);
    }
    private void GetMETempData2DataTable2(string json)
    {
        // get the spreadsheet
        Dictionary<int, string> rowAddress = new Dictionary<int, string>(); // config based on the templates
        rowAddress.Add(1, "FLDCELL");
        rowAddress.Add(2, "FLDWATCH");
        rowAddress.Add(4, "FLDOUT");
        rowAddress.Add(5, "FLDUNIT1");
        rowAddress.Add(6, "FLDUNIT2");
        rowAddress.Add(7, "FLDUNIT3");
        rowAddress.Add(8, "FLDUNIT4");
        rowAddress.Add(9, "FLDUNIT5");
        rowAddress.Add(10, "FLDUNIT6");
        rowAddress.Add(11, "FLDUNIT7");
        rowAddress.Add(12, "FLDUNIT8");
        rowAddress.Add(13, "FLDUNIT9");
        rowAddress.Add(14, "FLDUNIT10");
        rowAddress.Add(15, "FLDUNIT11");
        rowAddress.Add(16, "FLDUNIT12");
        rowAddress.Add(17, "FLDUNIT13");
        rowAddress.Add(18, "FLDUNIT14");
        rowAddress.Add(19, "FLDUNIT15");
        rowAddress.Add(20, "FLDUNIT16");
        string[] columns = rowAddress.Values.ToArray();

        DataTable Dt = CreateMETempDataTable(columns);
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        LoadMETempWatch(Dt, workbook);

        foreach (var rowAdd in rowAddress)
        {
            if (rowAdd.Key == 1 || rowAdd.Key == 2) continue;

            Row row = worksheet.Rows[rowAdd.Key];
            List<Cell> cells = row.Cells.GetRange(2, 18);
            foreach (Cell cell in cells)
            {
                string columnName = rowAdd.Value;

                string searchExpr = string.Format("FLDCELL ='{0}'", cell.Index);
                DataRow[] dtrows = new DataRow[100];
                dtrows = Dt.Select(searchExpr);
                DataRow dtrow = dtrows[0];
                dtrow[columnName] = cell.Value;
            }
        }

        DataTable completedTable = Dt;
        InsertTemparature(completedTable, 2);
    }

    private void GetMETurboChargerData2DataTable(string json)
    {
        // get the spreadsheet
        Dictionary<int, string> rowAddress = new Dictionary<int, string>(); // config based on the templates
        rowAddress.Add(1, "FLDCELLINDEX");
        rowAddress.Add(2, "FLDWATCH");
        rowAddress.Add(3, "FLDRPMX1000");
        rowAddress.Add(5, "FLDEXHAUSTGASINLET");
        rowAddress.Add(6, "FLDEXHAUSTGASOUTLET1");
        rowAddress.Add(7, "FLDAIRCOOLERAIRIN");
        rowAddress.Add(8, "FLDAIRCOOLERAIROUT");
        rowAddress.Add(9, "FLDCOOLINGWATERIN");
        rowAddress.Add(10, "FLDCOOLINGWATEROUT");
        rowAddress.Add(11, "FLDLUBERICATINGOIL");
        rowAddress.Add(13, "FLDSCAVENGEAIR");
        rowAddress.Add(15, "FLDAIRCOOLERIN");
        rowAddress.Add(16, "FLDASFIN");
        rowAddress.Add(17, "FLDTURBINEIN");
        string[] columns = rowAddress.Values.ToArray();

        DataTable Dt = CreateMEParamDataTable(columns);
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        LoadTurboWatchTime(Dt, workbook);

        foreach (var rowAdd in rowAddress)
        {
            if (rowAdd.Key == 1 || rowAdd.Key == 2) continue;

            Row row = worksheet.Rows[rowAdd.Key];
            List<Cell> cells = row.Cells.GetRange(3, 24);
            foreach (Cell cell in cells)
            {
                string columnName = rowAdd.Value;

                string searchExpr = string.Format("FLDCELLINDEX ='{0}'", cell.Index);
                DataRow[] dtrows = new DataRow[100];
                dtrows = Dt.Select(searchExpr);
                DataRow dtrow = dtrows[0];
                dtrow[columnName] = cell.Value;
            }
        }

        DataTable completedTable = Dt;
        InsertTurboCharger(completedTable, 1);
    }

    private void GetMETurboChargerData2DataTable2(string json)
    {
        // get the spreadsheet
        Dictionary<int, string> rowAddress = new Dictionary<int, string>(); // config based on the templates
        rowAddress.Add(1, "FLDCELLINDEX");
        rowAddress.Add(2, "FLDWATCH");
        rowAddress.Add(3, "FLDRPMX1000");
        rowAddress.Add(5, "FLDEXHAUSTGASINLET");
        rowAddress.Add(6, "FLDEXHAUSTGASOUTLET1");
        rowAddress.Add(7, "FLDAIRCOOLERAIRIN");
        rowAddress.Add(8, "FLDAIRCOOLERAIROUT");
        rowAddress.Add(9, "FLDCOOLINGWATERIN");
        rowAddress.Add(10, "FLDCOOLINGWATEROUT");
        rowAddress.Add(11, "FLDLUBERICATINGOIL");
        rowAddress.Add(13, "FLDSCAVENGEAIR");
        rowAddress.Add(15, "FLDAIRCOOLERIN");
        rowAddress.Add(16, "FLDASFIN");
        rowAddress.Add(17, "FLDTURBINEIN");
        string[] columns = rowAddress.Values.ToArray();

        DataTable Dt = CreateMEParamDataTable(columns);
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        LoadTurboWatchTime(Dt, workbook);

        foreach (var rowAdd in rowAddress)
        {
            if (rowAdd.Key == 1 || rowAdd.Key == 2) continue;

            Row row = worksheet.Rows[rowAdd.Key];
            List<Cell> cells = row.Cells.GetRange(3, 24);
            foreach (Cell cell in cells)
            {
                string columnName = rowAdd.Value;

                string searchExpr = string.Format("FLDCELLINDEX ='{0}'", cell.Index);
                DataRow[] dtrows = new DataRow[100];
                dtrows = Dt.Select(searchExpr);
                DataRow dtrow = dtrows[0];
                dtrow[columnName] = cell.Value;
            }
        }

        DataTable completedTable = Dt;
        InsertTurboCharger(completedTable, 2);
    }

    private void GetGeneratorData2DataTable(string json)
    {
        // get the spreadsheet
        Dictionary<int, string> rowAddress = new Dictionary<int, string>(); // config based on the templates
        rowAddress.Add(1, "FLDCELLINDEX");
        rowAddress.Add(2, "FLDWATCH");
        // DEFAULT TAG BASED ON THE SHEET
        rowAddress.Add(3, "FLDRUNNINGHOURS");
        rowAddress.Add(5, "FLDEXHAUSTGASMAX");
        rowAddress.Add(6, "FLDEXHAUSTGASMIN");
        rowAddress.Add(7, "FLDCOOLINGWATERIN");
        rowAddress.Add(8, "FLDCOOLINGWATEROUT");
        rowAddress.Add(9, "FLDLUBEOILIN");
        rowAddress.Add(10, "FLDLUBEOILOUT");
        rowAddress.Add(11, "FLDTEMPBOOSTAIR");
        rowAddress.Add(12, "FLDPEDESTALBEARING");
        rowAddress.Add(14, "FLDLUBEOIL");
        rowAddress.Add(15, "FLDCOOLINGWATER");
        rowAddress.Add(16, "FLDPRESBOOSTAIR");
        rowAddress.Add(17, "FLDFUELOIL");
        rowAddress.Add(19, "FLDCURRENTAMPS");
        rowAddress.Add(20, "FLDPOWERKW");
        rowAddress.Add(21, "FLDDOSERVTKLEVEL");
        string[] columns = rowAddress.Values.ToArray();

        DataTable meParamDt = CreateMEParamDataTable(columns);
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        LoadTurboWatchTime(meParamDt, workbook);
        foreach (var rowAdd in rowAddress)
        {
            if (rowAdd.Key == 1 || rowAdd.Key == 2) continue;

            Row row = worksheet.Rows[rowAdd.Key];
            List<Cell> cells = row.Cells.GetRange(3, 24);
            foreach (Cell cell in cells)
            {
                string columnName = rowAdd.Value;
                string searchExpr = string.Format("FLDCELLINDEX = '{0}'", cell.Index);
                DataRow[] dtrows = meParamDt.Select(searchExpr);
                DataRow dtrow = dtrows[0];
                dtrow[columnName] = cell.Value;
            }
        }

        DataTable completedTable = meParamDt;
        InsertGenerator(completedTable);
    }

    private void GetMiscData2DataTable(string json)
    {
        // get the spreadsheet
        Dictionary<int, string> rowAddress = new Dictionary<int, string>(); // config based on the templates
        rowAddress.Add(1, "FLDCELLINDEX");
        rowAddress.Add(2, "FLDWATCH");
        // DEFAULT TAG BASED ON THE SHEET
        rowAddress.Add(3, "FLDRUNNINGHOURS");
        rowAddress.Add(5, "FLDEXHAUSTGASMAX");
        rowAddress.Add(6, "FLDEXHAUSTGASMIN");
        rowAddress.Add(7, "FLDCOOLINGWATERIN");
        rowAddress.Add(8, "FLDCOOLINGWATEROUT");
        rowAddress.Add(9, "FLDLUBEOILIN");
        rowAddress.Add(10, "FLDLUBEOILOUT");
        rowAddress.Add(11, "FLDTEMPBOOSTAIR");
        rowAddress.Add(12, "FLDPEDESTALBEARING");
        rowAddress.Add(14, "FLDLUBEOIL");
        rowAddress.Add(15, "FLDCOOLINGWATER");
        rowAddress.Add(16, "FLDPRESBOOSTAIR");
        rowAddress.Add(17, "FLDFUELOIL");
        rowAddress.Add(19, "FLDCURRENTAMPS");
        rowAddress.Add(20, "FLDPOWERKW");
        rowAddress.Add(21, "FLDDOSERVTKLEVEL");
        string[] columns = rowAddress.Values.ToArray();

        DataTable meParamDt = CreateMEParamDataTable(columns);
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        LoadWatchTime(meParamDt, workbook);
        foreach (var rowAdd in rowAddress)
        {
            if (rowAdd.Key == 1 || rowAdd.Key == 2) continue;

            Row row = worksheet.Rows[rowAdd.Key];
            List<Cell> cells = row.Cells.GetRange(2, 6);
            foreach (Cell cell in cells)
            {
                string columnName = rowAdd.Value;
                string searchExpr = string.Format("FLDCELLINDEX = '{0}'", cell.Index);
                DataRow[] dtrows = meParamDt.Select(searchExpr);
                DataRow dtrow = dtrows[0];
                dtrow[columnName] = cell.Value;
            }
        }

        DataTable completedTable = meParamDt;
        InsertMisc(completedTable);
    }

    private void GetNoonReportData2DataTable(string json)
    {
        Dictionary<int, string> rowAddress = new Dictionary<int, string>(); // config based on the templates
        rowAddress.Add(1, "FLDCELLINDEX");
        // DEFAULT TAG BASED ON THE SHEET
        rowAddress.Add(30, "FLDHEAVYF");
        rowAddress.Add(31, "FLDLOWSULPHURF");
        rowAddress.Add(32, "FLDVLSRESIDUAL05MAX");
        rowAddress.Add(33, "FLDULSRESIDUAL01MAX");
        rowAddress.Add(34, "FLDMARINED");
        rowAddress.Add(35, "FLDMARINEG");
        rowAddress.Add(36, "FLDVLSDGASOIL05MAX");
        rowAddress.Add(37, "FLDULSDGASOIL01MAX");
        string[] columns = rowAddress.Values.ToArray();



        DataTable meParamDt = CreateMEParamDataTable(columns);
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        string VAR = Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 27).Cells.FirstOrDefault(c => c.Index == 4).Value);

        LoadNooonWatchTime(meParamDt, workbook);
        foreach (var rowAdd in rowAddress)
        {
            if (rowAdd.Key == 1) continue;

            Row row = worksheet.Rows[rowAdd.Key];
            List<Cell> cells = row.Cells.GetRange(4, 21);
            foreach (Cell cell in cells)
            {
                string columnName = rowAdd.Value;
                string searchExpr = string.Format("FLDCELLINDEX = '{0}'", cell.Index);
                DataRow[] dtrows = meParamDt.Select(searchExpr);
                DataRow dtrow = dtrows[0];
                dtrow[columnName] = cell.Value;
            }
        }

        DataTable completedTable = meParamDt;
        InsertNoonReportCons(completedTable);

        PhoenixEngineLogParameter.EngineLogNoonReportPerformanceInsert(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                        , General.GetNullableDateTime(lbldate.Text)
                                                        , General.GetNullableInteger("1")
                                                        , General.GetNullableString("-")
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 2).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 3).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 4).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 5).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 2).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 3).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 4).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 5).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 7).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 8).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 9).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 10).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 11).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 12).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 13).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 14).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 13).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 14).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 15).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 16).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 17).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 18).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 16).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 17).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 18).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 20).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 21).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 20).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 21).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 2).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 3).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 4).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 5).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 6).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 7).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 2).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 3).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 4).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 5).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 6).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 7).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 8).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 9).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 10).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 11).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 12).Cells.FirstOrDefault(c => c.Index == 8).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 10).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 10).Cells.FirstOrDefault(c => c.Index == 15).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 10).Cells.FirstOrDefault(c => c.Index == 17).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 10).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 11).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 11).Cells.FirstOrDefault(c => c.Index == 15).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 11).Cells.FirstOrDefault(c => c.Index == 17).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 11).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 12).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 12).Cells.FirstOrDefault(c => c.Index == 15).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 12).Cells.FirstOrDefault(c => c.Index == 17).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 12).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 13).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 13).Cells.FirstOrDefault(c => c.Index == 15).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 13).Cells.FirstOrDefault(c => c.Index == 17).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 13).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 14).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 14).Cells.FirstOrDefault(c => c.Index == 15).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 14).Cells.FirstOrDefault(c => c.Index == 17).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 14).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 15).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 15).Cells.FirstOrDefault(c => c.Index == 15).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 15).Cells.FirstOrDefault(c => c.Index == 17).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 15).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 18).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 18).Cells.FirstOrDefault(c => c.Index == 15).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 18).Cells.FirstOrDefault(c => c.Index == 17).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 18).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 19).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 19).Cells.FirstOrDefault(c => c.Index == 15).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 19).Cells.FirstOrDefault(c => c.Index == 17).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 19).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 20).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 20).Cells.FirstOrDefault(c => c.Index == 15).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 20).Cells.FirstOrDefault(c => c.Index == 17).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 20).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 21).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 21).Cells.FirstOrDefault(c => c.Index == 15).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 21).Cells.FirstOrDefault(c => c.Index == 17).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 21).Cells.FirstOrDefault(c => c.Index == 18).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 22).Cells.FirstOrDefault(c => c.Index == 14).Value))
                                                       );
    }

    private void GetMISCData2DataTable(string json)
    {
        // get the spreadsheet
        Dictionary<int, string> rowAddress = new Dictionary<int, string>(); // config based on the templates
        rowAddress.Add(1, "FLDCELLINDEX");
        rowAddress.Add(2, "FLDWATCH");
        // DEFAULT TAG BASED ON THE SHEET
        rowAddress.Add(3, "FLDFOSEAWATER");
        rowAddress.Add(4, "FLDENGINEROON");
        rowAddress.Add(5, "FLDFOHOSETTLINGTANK");
        rowAddress.Add(6, "FLDFOHOSERVICETANK");
        rowAddress.Add(7, "FLDTHRUSTEDBEARING");
        rowAddress.Add(8, "FLDSTERNTUBEOIL");
        rowAddress.Add(10, "FLDAIRBOTTEL1");
        rowAddress.Add(11, "FLDAIRBOTTLE2");
        rowAddress.Add(13, "FLDHOSETTLINGTANK");
        rowAddress.Add(14, "FLDHOSERVICETANK");
        rowAddress.Add(16, "FLDHOPURIFIER1");
        rowAddress.Add(17, "FLDHOPURIFIER2");
        rowAddress.Add(18, "FLDDOPURIFIER1");
        rowAddress.Add(19, "FLDDOPURIFIER2");
        rowAddress.Add(20, "FLDLOPURIFIER1");
        rowAddress.Add(21, "FLDLOPURIFIER2");
        string[] columns = rowAddress.Values.ToArray();

        DataTable meParamDt = CreateMEParamDataTable(columns);
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        LoadWatchTime(meParamDt, workbook);
        foreach (var rowAdd in rowAddress)
        {
            if (rowAdd.Key == 1 || rowAdd.Key == 2) continue;

            Row row = worksheet.Rows[rowAdd.Key];
            List<Cell> cells = row.Cells.GetRange(2, 6);
            foreach (Cell cell in cells)
            {
                string columnName = rowAdd.Value;
                string searchExpr = string.Format("FLDCELLINDEX = {0}", cell.Index);
                DataRow[] dtrows = meParamDt.Select(searchExpr);
                DataRow dtrow = dtrows[0];
                dtrow[columnName] = cell.Value;
            }
        }

        DataTable completedTable = meParamDt;
        InsertMisc(completedTable);
    }

    private void GetAIRData2DataTable(string json)
    {
        GetAIRCONDData2DataTable(json);
        GetREFRiDGEData2DataTable(json);
    }
    private void GetAIRCONDData2DataTable(string json)
    {
        // get the spreadsheet
        Dictionary<int, string> rowAddress = new Dictionary<int, string>(); // config based on the templates
        rowAddress.Add(1, "FLDCELLINDEX");
        rowAddress.Add(2, "FLDWATCH");
        // DEFAULT TAG BASED ON THE SHEET
        rowAddress.Add(4, "FLDACSUCTION");
        rowAddress.Add(5, "FLDACDISCHARGE");
        rowAddress.Add(6, "FLDACLUBEOIL");
        rowAddress.Add(7, "FLDACCOOLINGWATER");
        rowAddress.Add(9, "FLDPIN");
        rowAddress.Add(10, "FLDPOUT");
        rowAddress.Add(11, "FLDSIN");
        rowAddress.Add(12, "FLDSOUT");
        rowAddress.Add(16, "FLDOPERATINGHOURS");
        rowAddress.Add(18, "FLDHEATEXCHANGERFWIN");
        rowAddress.Add(19, "FLDHEATEXCHANGERFWOUT");
        rowAddress.Add(20, "FLDHEATEXCHANGERSWIN");
        rowAddress.Add(21, "FLDHEATEXCHANGERSWOUT");
        rowAddress.Add(22, "FLDSHELL");
        rowAddress.Add(24, "FLDVACUUM");
        rowAddress.Add(25, "FLDSALINITY");
        rowAddress.Add(26, "FLDDISTWATERFLOWMETER");
        rowAddress.Add(27, "FLDQUANTITYPROCEDURE");
        string[] columns = rowAddress.Values.ToArray();

        DataTable meParamDt = CreateMEParamDataTable(columns);
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        LoadAirWatchTime(meParamDt, workbook);
        foreach (var rowAdd in rowAddress)
        {
            if (rowAdd.Key == 1 || rowAdd.Key == 2) continue;

            Row row = worksheet.Rows[rowAdd.Key];
            List<Cell> cells = row.Cells.GetRange(3, 6);
            foreach (Cell cell in cells)
            {
                string columnName = rowAdd.Value;
                string searchExpr = string.Format("FLDCELLINDEX = {0}", cell.Index);
                DataRow[] dtrows = meParamDt.Select(searchExpr);
                DataRow dtrow = dtrows[0];
                dtrow[columnName] = cell.Value;
            }
        }

        DataTable completedTable = meParamDt;
        InsertAircond(completedTable);
    }
    private void GetREFRiDGEData2DataTable(string json)
   {
        // get the spreadsheet
        Dictionary<int, string> rowAddress = new Dictionary<int, string>(); // config based on the templates
        rowAddress.Add(1, "FLDCELLINDEX");
        rowAddress.Add(2, "FLDWATCH");
        // DEFAULT TAG BASED ON THE SHEE
        rowAddress.Add(4, "FLDREFRIGNSUCTION");
        rowAddress.Add(5, "FLDREFRIGDISCHARGE");
        rowAddress.Add(6, "FLDREFRIGLUBEOIL");
        rowAddress.Add(7, "FLDCOMPRESSORNO");
        rowAddress.Add(9, "FLDMEAT");
        rowAddress.Add(10, "FLDFISH");
        rowAddress.Add(11, "FLDVEGETABLELOBBY");
        rowAddress.Add(16, "FLDOILFIRINGHOURS");
        rowAddress.Add(17, "FLDFEEDWATER");
        rowAddress.Add(18, "FLDBACKPRESS");
        rowAddress.Add(22, "FLDMSWPUMP");
        rowAddress.Add(23, "FLDAUXSWPUMP");
        rowAddress.Add(24, "FLDEJECTORPUMP");
        rowAddress.Add(25, "FLDMAINLOPUMP");
        rowAddress.Add(26, "FLDBOILERPUMP");
        string[] columns = rowAddress.Values.ToArray();

        DataTable meParamDt = CreateMEParamDataTable(columns);
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        LoadrefWatchTime(meParamDt, workbook);
        foreach (var rowAdd in rowAddress)
        {
            if (rowAdd.Key == 1 || rowAdd.Key == 2) continue;

            Row row = worksheet.Rows[rowAdd.Key];
            List<Cell> cells = row.Cells.GetRange(13, 6);
            foreach (Cell cell in cells)
            {
                string columnName = rowAdd.Value;
                string searchExpr = string.Format("FLDCELLINDEX = {0}", cell.Index);
                DataRow[] dtrows = meParamDt.Select(searchExpr);
                    DataRow dtrow = dtrows[0];
                    dtrow[columnName] = cell.Value;
            }
        }

        DataTable completedTable = meParamDt;
        InsertRefridge(completedTable);
    }

    private void GetHeatData2DataTable(string json)
    {
        // get the spreadsheet
        Dictionary<int, string> rowAddress = new Dictionary<int, string>(); // config based on the templates
        rowAddress.Add(1, "FLDCELLINDEX");
        rowAddress.Add(2, "FLDWATCH");
        // DEFAULT TAG BASED ON THE SHEET
        rowAddress.Add(3, "FLDSWINLET");
        rowAddress.Add(4, "FLDJCSWOUT");
        rowAddress.Add(5, "FLDJCFWIN");
        rowAddress.Add(6, "FLDJCFWOUT");
        rowAddress.Add(7, "FLDLOCSWIN");
        rowAddress.Add(8, "FLDLOCSWOUT");
        rowAddress.Add(9, "FLDLOCFWIN");
        rowAddress.Add(10, "FLDLOCFWOUT");
        rowAddress.Add(11, "FLDFVCWSWIN");
        rowAddress.Add(12, "FLDFVCWSWOUT");
        rowAddress.Add(13, "FLDFVCWFWIN");
        rowAddress.Add(14, "FLDFVCWFWOUT");
        rowAddress.Add(15, "FLDAUXENGLOCSWIN");
        rowAddress.Add(16, "FLDAUXENGLOCSWOUT");
        rowAddress.Add(17, "FLDAUXENGLOCLOIN");
        rowAddress.Add(18, "FLDAUXENGLOCLOOUT");
        rowAddress.Add(19, "FLDFVCW");
        rowAddress.Add(21, "FLDHESEAWATER");
        rowAddress.Add(25, "FLDHEJACKETWATER");
        rowAddress.Add(26, "FLDBEARINGHEAD");
        rowAddress.Add(27, "FLDCAMSHAFTLO");
        rowAddress.Add(28, "FLDHTWATER");
        rowAddress.Add(29, "FLDSCAVENGEAIR1");
        rowAddress.Add(30, "FLDSCAVENGEAIR2");
        rowAddress.Add(31, "FLDFUELOIL");
        rowAddress.Add(32, "FLDPISTONCOOLING");
        rowAddress.Add(33, "FLDCONTROLAIR");
        rowAddress.Add(34, "FLDSERVICEAIR");
        string[] columns = rowAddress.Values.ToArray();

        DataTable meParamDt = CreateMEParamDataTable(columns);
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        LoadHeatTime(meParamDt, workbook);
        foreach (var rowAdd in rowAddress)
        {
            if (rowAdd.Key == 1 || rowAdd.Key == 2) continue;

            Row row = worksheet.Rows[rowAdd.Key];
            List<Cell> cells = row.Cells.GetRange(12, 6);
            foreach (Cell cell in cells)
            {
                string columnName = rowAdd.Value;
                string searchExpr = string.Format("FLDCELLINDEX = {0}", cell.Index);
                DataRow[] dtrows = meParamDt.Select(searchExpr);
                DataRow dtrow = dtrows[0];
                dtrow[columnName] = cell.Value;
            }
        }

        DataTable completedTable = meParamDt;
        InsertHeat(completedTable);
    }

    private void GetMiscRemarksData2DataTable(string json)
    {
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        PhoenixEngineLogParameter.EngineLogMiscRemarksInsert(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
        , General.GetNullableDateTime(lbldate.Text)
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 2).Cells.FirstOrDefault(c => c.Index == 0).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 4).Cells.FirstOrDefault(c => c.Index == 0).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 7).Cells.FirstOrDefault(c => c.Index == 0).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 9).Cells.FirstOrDefault(c => c.Index == 0).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 12).Cells.FirstOrDefault(c => c.Index == 0).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 14).Cells.FirstOrDefault(c => c.Index == 0).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 17).Cells.FirstOrDefault(c => c.Index == 0).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 19).Cells.FirstOrDefault(c => c.Index == 0).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 22).Cells.FirstOrDefault(c => c.Index == 0).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 24).Cells.FirstOrDefault(c => c.Index == 0).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 27).Cells.FirstOrDefault(c => c.Index == 0).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 29).Cells.FirstOrDefault(c => c.Index == 0).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 4).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 9).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 14).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 19).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 24).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 29).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 5).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 10).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 15).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 20).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 25).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 30).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 6).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 11).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 16).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 21).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 26).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 31).Cells.FirstOrDefault(c => c.Index == 3).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 2).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 3).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 4).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 5).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 6).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 7).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 8).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 9).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 10).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 11).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 12).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 13).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 14).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 15).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 16).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 17).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 18).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 19).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 20).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 21).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 22).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 23).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 24).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 25).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 26).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 27).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 28).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 29).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 30).Cells.FirstOrDefault(c => c.Index == 1).Value))
        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 31).Cells.FirstOrDefault(c => c.Index == 1).Value))
        );
    }

    private void GetNoonRemarksData2DataTable(string json)
    {
        Workbook workbook = Workbook.FromJson(json);
        Worksheet worksheet = workbook.Sheets[0];

        PhoenixEngineLogParameter.EngineLogNoonReportRemarksInsert(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                        , General.GetNullableDateTime(lbldate.Text)
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 36).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 37).Cells.FirstOrDefault(c => c.Index == 3).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 34).Cells.FirstOrDefault(c => c.Index == 1).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 35).Cells.FirstOrDefault(c => c.Index == 1).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 36).Cells.FirstOrDefault(c => c.Index == 1).Value))
                                                        , General.GetNullableString(Convert.ToString(worksheet.Rows.FirstOrDefault(r => r.Index == 37).Cells.FirstOrDefault(c => c.Index == 1).Value))
                                                       );
    }

    private void InsertMeParam(DataTable dt, int engineid)
    {
        foreach (DataRow row in dt.Rows)
        {
            PhoenixEngineLogParameter.EngineLogParameterInsert(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                      , General.GetNullableDateTime(lbldate.Text)
                                                      , General.GetNullableInteger(row["FLDCELLINDEX"].ToString())
                                                      , engineid
                                                      , General.GetNullableString(row["FLDWATCH"].ToString())
                                                      , General.GetNullableString(row["FLDHOUR"].ToString())
                                                      , General.GetNullableString(row["FLDMINUTE"].ToString())
                                                      , General.GetNullableString(row["FLDCOUNTER"].ToString())
                                                      , General.GetNullableString(row["FLDRPM"].ToString())
                                                      , General.GetNullableString(row["FLDGOVERNOR"].ToString())
                                                      , General.GetNullableString(row["FLDCONTROLLOCATION"].ToString())
                                                      , General.GetNullableString(row["FLDFLOWMETER"].ToString())
                                                      , General.GetNullableString(row["FLDCONSUMPTION"].ToString())
                                                      , General.GetNullableString(row["FLDCYLOILLEVEL"].ToString())
                                                      , General.GetNullableString(row["FLDCYLOILCONS"].ToString())
                                                      , General.GetNullableString(row["FLDCMESUMP"].ToString())
                                                      );

        }
    }

    private void InsertTemparature(DataTable dt, int engineid)
    {
        foreach (DataRow row in dt.Rows)
        {
            PhoenixEngineLogParameter.EngineLogTemparatureInsert(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                      , General.GetNullableDateTime(lbldate.Text)
                                                      , General.GetNullableInteger(row["FLDCELL"].ToString())
                                                      , engineid
                                                      , General.GetNullableString(row["FLDWATCH"].ToString())
                                                      , General.GetNullableString(row["FLDOUT"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT1"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT3"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT2"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT4"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT5"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT6"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT7"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT8"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT9"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT10"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT11"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT12"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT13"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT14"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT15"].ToString())
                                                      , General.GetNullableString(row["FLDUNIT16"].ToString())
                                                      );

        }
    }

    private void InsertTurboCharger(DataTable dt, int engineid)
    {
        foreach (DataRow row in dt.Rows)
        {
            PhoenixEngineLogParameter.EngineLogTurboChargerInsert(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                      , General.GetNullableDateTime(lbldate.Text)
                                                      , General.GetNullableInteger(row["FLDCELLINDEX"].ToString())
                                                      , engineid
                                                      , General.GetNullableString(row["FLDWATCH"].ToString())
                                                       , General.GetNullableString(row["FLDRPMX1000"].ToString())
                                                        , General.GetNullableString(row["FLDEXHAUSTGASINLET"].ToString())
                                                        , General.GetNullableString(row["FLDEXHAUSTGASOUTLET1"].ToString())
                                                        , General.GetNullableString(row["FLDEXHAUSTGASOUTLET1"].ToString())
                                                        , General.GetNullableString(row["FLDAIRCOOLERAIRIN"].ToString())
                                                        , General.GetNullableString(row["FLDAIRCOOLERAIROUT"].ToString())
                                                        , General.GetNullableString(row["FLDCOOLINGWATERIN"].ToString())
                                                        , General.GetNullableString(row["FLDCOOLINGWATEROUT"].ToString())
                                                        , General.GetNullableString(row["FLDLUBERICATINGOIL"].ToString())
                                                        , General.GetNullableString(row["FLDSCAVENGEAIR"].ToString())
                                                        , General.GetNullableString(row["FLDAIRCOOLERIN"].ToString())
                                                        , General.GetNullableString(row["FLDAIRCOOLERIN"].ToString())
                                                        , General.GetNullableString(null)
                                                        , General.GetNullableString(row["FLDASFIN"].ToString())
                                                        , General.GetNullableString(row["FLDASFIN"].ToString())
                                                        , General.GetNullableString(row["FLDTURBINEIN"].ToString())
                                                        , General.GetNullableString(row["FLDTURBINEIN"].ToString())
                                                        );

        }
    }

    private void InsertGenerator(DataTable dt)
    {
        foreach (DataRow row in dt.Rows)
        {
            PhoenixEngineLogParameter.EngineLogGeneratorInsert(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                      , General.GetNullableDateTime(lbldate.Text)
                                                      , General.GetNullableInteger(row["FLDCELLINDEX"].ToString())
                                                      , General.GetNullableString(row["FLDWATCH"].ToString())
                                                      , General.GetNullableString(row["FLDRUNNINGHOURS"].ToString())
                                                        , General.GetNullableString(row["FLDEXHAUSTGASMAX"].ToString())
                                                        , General.GetNullableString(row["FLDEXHAUSTGASMIN"].ToString())
                                                        , General.GetNullableString(row["FLDCOOLINGWATERIN"].ToString())
                                                        , General.GetNullableString(row["FLDCOOLINGWATEROUT"].ToString())
                                                        , General.GetNullableString(row["FLDLUBEOILIN"].ToString())
                                                        , General.GetNullableString(row["FLDLUBEOILOUT"].ToString())
                                                        , General.GetNullableString(row["FLDTEMPBOOSTAIR"].ToString())
                                                        , General.GetNullableString(row["FLDPEDESTALBEARING"].ToString())
                                                        , General.GetNullableString(row["FLDLUBEOIL"].ToString())
                                                        , General.GetNullableString(row["FLDCOOLINGWATER"].ToString())
                                                        , General.GetNullableString(row["FLDPRESBOOSTAIR"].ToString())
                                                        , General.GetNullableString(row["FLDFUELOIL"].ToString())
                                                        , General.GetNullableString(row["FLDCURRENTAMPS"].ToString())
                                                        , General.GetNullableString(row["FLDPOWERKW"].ToString())
                                                        , General.GetNullableString(row["FLDDOSERVTKLEVEL"].ToString())
                                                      );

        }
    }

    private void InsertMisc(DataTable dt)
    {
        foreach (DataRow row in dt.Rows)
        {
            PhoenixEngineLogParameter.EngineLogMiscInsert(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                      , General.GetNullableDateTime(lbldate.Text)
                                                      , General.GetNullableInteger(row["FLDCELLINDEX"].ToString())
                                                      , General.GetNullableString(row["FLDWATCH"].ToString())
                                                      , General.GetNullableString(row["FLDFOSEAWATER"].ToString())
                                                        , General.GetNullableString(row["FLDENGINEROON"].ToString())
                                                        , General.GetNullableString(row["FLDFOHOSETTLINGTANK"].ToString())
                                                        , General.GetNullableString(row["FLDFOHOSERVICETANK"].ToString())
                                                        , General.GetNullableString(row["FLDTHRUSTEDBEARING"].ToString())
                                                        , General.GetNullableString(row["FLDSTERNTUBEOIL"].ToString())
                                                        , General.GetNullableString(row["FLDAIRBOTTEL1"].ToString())
                                                        , General.GetNullableString(row["FLDAIRBOTTLE2"].ToString())
                                                        , General.GetNullableString(row["FLDHOSETTLINGTANK"].ToString())
                                                        , General.GetNullableString(row["FLDHOSERVICETANK"].ToString())
                                                        , General.GetNullableString(row["FLDHOPURIFIER1"].ToString())
                                                        , General.GetNullableString(row["FLDHOPURIFIER2"].ToString())
                                                        , General.GetNullableString(row["FLDDOPURIFIER1"].ToString())
                                                        , General.GetNullableString(row["FLDDOPURIFIER2"].ToString())
                                                        , General.GetNullableString(row["FLDLOPURIFIER1"].ToString())
                                                        , General.GetNullableString(row["FLDLOPURIFIER2"].ToString())
                                                      );

        }
    }

    private void InsertAir(DataTable dt)
    {
        foreach (DataRow row in dt.Rows)
        {
            PhoenixEngineLogParameter.EngineLogMisc2Insert(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                      , General.GetNullableDateTime(lbldate.Text)
                                                      , General.GetNullableInteger(row["FLDCELLINDEX"].ToString())
                                                      , General.GetNullableString(row["FLDWATCH"].ToString())
                                                      , General.GetNullableString(row["FLDACSUCTION"].ToString())
                                                        , General.GetNullableString(row["FLDACDISCHARGE"].ToString())
                                                        , General.GetNullableString(row["FLDACLUBEOIL"].ToString())
                                                        , General.GetNullableString(row["FLDACCOOLINGWATER"].ToString())
                                                        , General.GetNullableString(row["FLDPIN"].ToString())
                                                        , General.GetNullableString(row["FLDPOUT"].ToString())
                                                        , General.GetNullableString(row["FLDSIN"].ToString())
                                                        , General.GetNullableString(row["FLDSOUT"].ToString())
                                                        , General.GetNullableString(row["FLDREFRIGNSUCTION"].ToString())
                                                        , General.GetNullableString(row["FLDREFRIGDISCHARGE"].ToString())
                                                        , General.GetNullableString(row["FLDREFRIGLUBEOIL"].ToString())
                                                        , General.GetNullableString(row["FLDCOMPRESSORNO"].ToString())
                                                        , General.GetNullableString(row["FLDMEAT"].ToString())
                                                        , General.GetNullableString(row["FLDFISH"].ToString())
                                                        , General.GetNullableString(row["FLDVEGETABLELOBBY"].ToString())
                                                        , General.GetNullableString(row["FLDOPERATINGHOURS"].ToString())
                                                        , General.GetNullableString(row["FLDHEATEXCHANGERFWIN"].ToString())
                                                        , General.GetNullableString(row["FLDHEATEXCHANGERFWOUT"].ToString())
                                                        , General.GetNullableString(row["FLDHEATEXCHANGERSWIN"].ToString())
                                                        , General.GetNullableString(row["FLDHEATEXCHANGERSWOUT"].ToString())
                                                        , General.GetNullableString(row["FLDSHELL"].ToString())
                                                        , General.GetNullableString(row["FLDVACUUM"].ToString())
                                                        , General.GetNullableString(row["FLDSALINITY"].ToString())
                                                        , General.GetNullableString(row["FLDDISTWATERFLOWMETER"].ToString())
                                                        , General.GetNullableString(row["FLDQUANTITYPROCEDURE"].ToString())
                                                        , General.GetNullableString(row["FLDOILFIRINGHOURS"].ToString())
                                                        , General.GetNullableString(row["FLDFEEDWATER"].ToString())
                                                        , General.GetNullableString(row["FLDBACKPRESS"].ToString())
                                                        , General.GetNullableString(row["FLDMSWPUMP"].ToString())
                                                        , General.GetNullableString(row["FLDAUXSWPUMP"].ToString())
                                                        , General.GetNullableString(row["FLDEJECTORPUMP"].ToString())
                                                        , General.GetNullableString(row["FLDMAINLOPUMP"].ToString())
                                                        , General.GetNullableString(row["FLDBOILERPUMP"].ToString())
                                                      );

        }
    }
    private void InsertAircond(DataTable dt)
    {
        foreach (DataRow row in dt.Rows)
        {
            PhoenixEngineLogParameter.EngineLogair2Insert(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                      , General.GetNullableDateTime(lbldate.Text)
                                                      , General.GetNullableInteger(row["FLDCELLINDEX"].ToString())
                                                      , General.GetNullableString(row["FLDWATCH"].ToString())
                                                      , General.GetNullableString(row["FLDACSUCTION"].ToString())
                                                        , General.GetNullableString(row["FLDACDISCHARGE"].ToString())
                                                        , General.GetNullableString(row["FLDACLUBEOIL"].ToString())
                                                        , General.GetNullableString(row["FLDACCOOLINGWATER"].ToString())
                                                        , General.GetNullableString(row["FLDPIN"].ToString())
                                                        , General.GetNullableString(row["FLDPOUT"].ToString())
                                                        , General.GetNullableString(row["FLDSIN"].ToString())
                                                        , General.GetNullableString(row["FLDSOUT"].ToString())
                                                        , General.GetNullableString(row["FLDOPERATINGHOURS"].ToString())
                                                        , General.GetNullableString(row["FLDHEATEXCHANGERFWIN"].ToString())
                                                        , General.GetNullableString(row["FLDHEATEXCHANGERFWOUT"].ToString())
                                                        , General.GetNullableString(row["FLDHEATEXCHANGERSWIN"].ToString())
                                                        , General.GetNullableString(row["FLDHEATEXCHANGERSWOUT"].ToString())
                                                        , General.GetNullableString(row["FLDSHELL"].ToString())
                                                        , General.GetNullableString(row["FLDVACUUM"].ToString())
                                                        , General.GetNullableString(row["FLDSALINITY"].ToString())
                                                        , General.GetNullableString(row["FLDDISTWATERFLOWMETER"].ToString())
                                                        , General.GetNullableString(row["FLDQUANTITYPROCEDURE"].ToString())
                                                      );

        }
    }
    private void InsertRefridge(DataTable dt)
    {
        foreach (DataRow row in dt.Rows)
        {
            PhoenixEngineLogParameter.EngineLogrefrideg2Insert(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                      , General.GetNullableDateTime(lbldate.Text)
                                                      , General.GetNullableInteger(row["FLDCELLINDEX"].ToString())
                                                      , General.GetNullableString(row["FLDWATCH"].ToString())
                                                        , General.GetNullableString(row["FLDREFRIGNSUCTION"].ToString())
                                                        , General.GetNullableString(row["FLDREFRIGDISCHARGE"].ToString())
                                                        , General.GetNullableString(row["FLDREFRIGLUBEOIL"].ToString())
                                                        , General.GetNullableString(row["FLDCOMPRESSORNO"].ToString())
                                                        , General.GetNullableString(row["FLDMEAT"].ToString())
                                                        , General.GetNullableString(row["FLDFISH"].ToString())
                                                        , General.GetNullableString(row["FLDVEGETABLELOBBY"].ToString())
                                                        , General.GetNullableString(row["FLDOILFIRINGHOURS"].ToString())
                                                        , General.GetNullableString(row["FLDFEEDWATER"].ToString())
                                                        , General.GetNullableString(row["FLDBACKPRESS"].ToString())
                                                        , General.GetNullableString(row["FLDMSWPUMP"].ToString())
                                                        , General.GetNullableString(row["FLDAUXSWPUMP"].ToString())
                                                        , General.GetNullableString(row["FLDEJECTORPUMP"].ToString())
                                                        , General.GetNullableString(row["FLDMAINLOPUMP"].ToString())
                                                        , General.GetNullableString(row["FLDBOILERPUMP"].ToString())
                                                      );

        }
    }
    private void InsertHeat(DataTable dt)
    {
        foreach (DataRow row in dt.Rows)
        {
            PhoenixEngineLogParameter.EngineLogMiscHeatInsert(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                      , General.GetNullableDateTime(lbldate.Text)
                                                      , General.GetNullableInteger(row["FLDCELLINDEX"].ToString())
                                                      , General.GetNullableString(row["FLDWATCH"].ToString())
                                                      , General.GetNullableString(row["FLDSWINLET"].ToString())
                                                        , General.GetNullableString(row["FLDJCSWOUT"].ToString())
                                                        , General.GetNullableString(row["FLDJCFWIN"].ToString())
                                                        , General.GetNullableString(row["FLDJCFWOUT"].ToString())
                                                        , General.GetNullableString(row["FLDLOCSWIN"].ToString())
                                                        , General.GetNullableString(row["FLDLOCSWOUT"].ToString())
                                                        , General.GetNullableString(row["FLDLOCFWIN"].ToString())
                                                        , General.GetNullableString(row["FLDLOCFWOUT"].ToString())
                                                        , General.GetNullableString(row["FLDFVCWSWIN"].ToString())
                                                        , General.GetNullableString(row["FLDFVCWSWOUT"].ToString())
                                                        , General.GetNullableString(row["FLDFVCWFWIN"].ToString())
                                                        , General.GetNullableString(row["FLDFVCWFWOUT"].ToString())
                                                        , General.GetNullableString(row["FLDAUXENGLOCSWIN"].ToString())
                                                        , General.GetNullableString(row["FLDAUXENGLOCSWOUT"].ToString())
                                                        , General.GetNullableString(row["FLDAUXENGLOCLOIN"].ToString())
                                                        , General.GetNullableString(row["FLDAUXENGLOCLOOUT"].ToString())
                                                        , General.GetNullableString(row["FLDFVCW"].ToString())
                                                        , General.GetNullableString(row["FLDHESEAWATER"].ToString())
                                                        , General.GetNullableString(row["FLDHEJACKETWATER"].ToString())
                                                        , General.GetNullableString(row["FLDBEARINGHEAD"].ToString())
                                                        , General.GetNullableString(row["FLDCAMSHAFTLO"].ToString())
                                                        , General.GetNullableString(row["FLDHTWATER"].ToString())
                                                        , General.GetNullableString(row["FLDSCAVENGEAIR1"].ToString())
                                                        , General.GetNullableString(row["FLDSCAVENGEAIR2"].ToString())
                                                        , General.GetNullableString(row["FLDFUELOIL"].ToString())
                                                        , General.GetNullableString(row["FLDPISTONCOOLING"].ToString())
                                                        , General.GetNullableString(row["FLDCONTROLAIR"].ToString())
                                                        , General.GetNullableString(row["FLDSERVICEAIR"].ToString())
                                                      );

        }
    }

    private void InsertNoonReportCons(DataTable dt)
    {
        foreach (DataRow row in dt.Rows)
        {
            PhoenixEngineLogParameter.EngineLogNoonReportConsInsert(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                      , General.GetNullableDateTime(lbldate.Text)
                                                      , General.GetNullableInteger(row["FLDCELLINDEX"].ToString())
                                                      , General.GetNullableString(row["FLDHEAVYF"].ToString())
                                                      , General.GetNullableString(row["FLDLOWSULPHURF"].ToString())
                                                        , General.GetNullableString(row["FLDVLSRESIDUAL05MAX"].ToString())
                                                        , General.GetNullableString(row["FLDULSRESIDUAL01MAX"].ToString())
                                                        , General.GetNullableString(row["FLDMARINED"].ToString())
                                                        , General.GetNullableString(row["FLDMARINEG"].ToString())
                                                        , General.GetNullableString(row["FLDVLSDGASOIL05MAX"].ToString())
                                                        , General.GetNullableString(row["FLDULSDGASOIL01MAX"].ToString())
                                                      );

        }
    }

    private DataTable CreateMEParamDataTable(string[] columns)
    {
        DataTable dt = new DataTable();
        foreach (var item in columns)
        {
            DataColumn column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = item;
            dt.Columns.Add(column);
        }
        return dt;
    }

    private DataTable CreateMETempDataTable(string[] columns)
    {
        DataTable dt = new DataTable();
        foreach (var item in columns)
        {
            DataColumn column = new DataColumn();
            column.DataType = typeof(string);

            column.ColumnName = item;
            dt.Columns.Add(column);
        }
        return dt;
    }

    private void LoadWatchTime(DataTable dt, Workbook workbook)
    {
        Worksheet worksheet = workbook.Sheets[0];
        int[] rows = new int[] { 1 }; // range of cells
        for (int i = 0; i < rows.Length; i++)
        {
            Row row = worksheet.Rows[rows[i]];
            List<Cell> cells = row.Cells.GetRange(2, 6);
            foreach (Cell cell in cells)
            {

                DataRow datarow = dt.NewRow();
                datarow["FLDWATCH"] = (string)cell.Value;
                datarow["FLDCELLINDEX"] = cell.Index;
                dt.Rows.Add(datarow);
            }
        }
    }

    private void LoadAirWatchTime(DataTable dt, Workbook workbook)
    {
        Worksheet worksheet = workbook.Sheets[0];
        int[] rows = new int[] { 1 }; // range of cells
        for (int i = 0; i < rows.Length; i++)
        {
            Row row = worksheet.Rows[2];
            List<Cell> cells = row.Cells.GetRange(3, 6);
            foreach (Cell cell in cells)
            {

                DataRow datarow = dt.NewRow();
                datarow["FLDWATCH"] = (string)cell.Value;
                datarow["FLDCELLINDEX"] = cell.Index;
                dt.Rows.Add(datarow);
            }
        }
    }

    private void LoadrefWatchTime(DataTable dt, Workbook workbook)
    {
        Worksheet worksheet = workbook.Sheets[0];
        int[] rows = new int[] { 1 }; // range of cells
        for (int i = 0; i < rows.Length; i++)
        {
            Row row = worksheet.Rows[2];
            List<Cell> cells = row.Cells.GetRange(13, 6);
            foreach (Cell cell in cells)
            {

                DataRow datarow = dt.NewRow();
                datarow["FLDWATCH"] = (string)cell.Value;
                datarow["FLDCELLINDEX"] = cell.Index;
                dt.Rows.Add(datarow);
            }
        }
    }


    private void LoadHeatTime(DataTable dt, Workbook workbook)
    {
        Worksheet worksheet = workbook.Sheets[0];
        int[] rows = new int[] { 1 }; // range of cells
        for (int i = 0; i < rows.Length; i++)
        {
            Row row = worksheet.Rows[rows[i]];
            List<Cell> cells = row.Cells.GetRange(12, 6);
            foreach (Cell cell in cells)
            {

                DataRow datarow = dt.NewRow();
                datarow["FLDWATCH"] = (string)cell.Value;
                datarow["FLDCELLINDEX"] = cell.Index;
                dt.Rows.Add(datarow);
            }
        }
    }

    private void LoadMETempWatch(DataTable dt, Workbook workbook)
    {
        Worksheet worksheet = workbook.Sheets[0];
        int[] rows = new int[] { 2 }; // range of cells
        for (int i = 0; i < rows.Length; i++)
        {
            Row row = worksheet.Rows[rows[i]];
            List<Cell> cells = row.Cells.GetRange(2, 18);
            foreach (Cell cell in cells)
            {

                DataRow datarow = dt.NewRow();
                if (cell.Index == 2 || cell.Index == 5 || cell.Index == 11 || cell.Index == 14 || cell.Index == 17 || cell.Index == 8)
                {
                    datarow["FLDWATCH"] = (string)cell.Value;
                }
                datarow["FLDCELL"] = cell.Index;
                dt.Rows.Add(datarow);
            }
        }
    }

    private void LoadTurboWatchTime(DataTable dt, Workbook workbook)
    {
        Worksheet worksheet = workbook.Sheets[0];
        //int[] rows = new int[] { 2 }; // range of cells
        //for (int i = 0; i < rows.Length; i++)
        //{
        Row row = worksheet.Rows[2];
        List<Cell> cells = row.Cells.GetRange(3, 24);
        foreach (Cell cell in cells)
        {

            DataRow datarow = dt.NewRow();
            datarow["FLDWATCH"] = (string)cell.Value;
            datarow["FLDCELLINDEX"] = cell.Index;
            dt.Rows.Add(datarow);
        }
        //}
    }

    private void LoadNooonWatchTime(DataTable dt, Workbook workbook)
    {
        Worksheet worksheet = workbook.Sheets[0];
        //int[] rows = new int[] { 2 }; // range of cells
        //for (int i = 0; i < rows.Length; i++)
        //{
        Row row = worksheet.Rows[2];
        List<Cell> cells = row.Cells.GetRange(4, 21);
        foreach (Cell cell in cells)
        {

            DataRow datarow = dt.NewRow();
            datarow["FLDCELLINDEX"] = cell.Index;
            dt.Rows.Add(datarow);
        }
        //}
    }

    private DataSet GetLogList()
    {
        DataSet ds = PhoenixEngineLog.MainEngineLogList(usercode, vesselId);
        return ds;
    }

    private void LoadLogSelect()
    {
        string[] logs = new string[] { "Daily Engine Log", "Monthly Summary Log" };
        ddlLogSelect.DataSource = logs;
        ddlLogSelect.DataBind();
        ddlLogSelect.Items[0].Selected = true;
    }

    protected void btnPrev_Click(object sender, EventArgs e)
    {
        if (txtdate.SelectedDate.HasValue)
        {
            txtdate.SelectedDate = txtdate.SelectedDate.Value.AddDays(-1);
            lbldate.Text = txtdate.SelectedDate.Value.ToString("dd/MM/yyyy");
            //LoadExcelTemplate(lbldate.Text);
            Response.Redirect("../Log/ElecticLogMainEngineLogV2.aspx?date=" + lbldate.Text);
        }
    }

    protected void btnNxt_Click(object sender, EventArgs e)
    {
        if (ValidateFutureDate() && txtdate.SelectedDate.HasValue && txtdate.SelectedDate.Value.Date != DateTime.Now.Date)
        {
            txtdate.SelectedDate = txtdate.SelectedDate.Value.AddDays(1);
            lbldate.Text = txtdate.SelectedDate.Value.ToString("dd/MM/yyyy");
            //LoadExcelTemplate(lbldate.Text);
            Response.Redirect("../Log/ElecticLogMainEngineLogV2.aspx?date=" + lbldate.Text);
        }
        else
        {
            if (ucError.Visible == false)
            {
                txtdate.SelectedDate = DateTime.Now;
                lbldate.Text = txtdate.SelectedDate.Value.ToString("dd/MM/yyyy");
                ucError.ErrorMessage = "Please select a valid date or date selected cannot be future date";
                ucError.Visible = true;
            }
        }
    }


    protected void txtdate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        if (ValidateFutureDate())
        {
            lbldate.Text = txtdate.SelectedDate.Value.ToString("dd/MM/yyyy");
            //LoadExcelTemplate(lbldate.Text);
            Response.Redirect("../Log/ElecticLogMainEngineLogV2.aspx?date=" + lbldate.Text);
        }
        else
        {
            txtdate.SelectedDate = DateTime.Now;
            ucError.ErrorMessage = "Please select a valid date or date selected cannot be future date";
            ucError.Visible = true;
        }
    }

    private bool ValidateFutureDate()
    {
        if (txtdate.SelectedDate.HasValue && txtdate.SelectedDate.Value <= DateTime.Now)
        {
            return true;
        }
        return false;
    }

    #region Event Handling
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
			Response.Redirect("../Log/ElecticLogMainEngineLogV2.aspx?date=" + lbldate.Text);
            //LoadExcelTemplate(lbldate.Text);
            //string script = string.Format("refreshScroll()");
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "scroll", script, true);
            //BindLogList();

            //if (SouthNests.Phoenix.Framework.Filter.AmendFilterCriteria != null)
            //{
            //    NameValueCollection nvc = SouthNests.Phoenix.Framework.Filter.AmendFilterCriteria;
            //    string script = string.Format("Amended('{0}','{1}','{2}')", nvc["address"], nvc["value"], nvc["sheet"]);
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "signature", script, true);
            //    SouthNests.Phoenix.Framework.Filter.AmendFilterCriteria = null;
            //}


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

    protected void ddlLogSelect_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlLogSelect.SelectedItem.Index != 0)
        {
            Response.Redirect("../Log/ElectricLogEngineLogSummary.aspx");
        }
    }

    //protected void lnkPdf_Click(object sender, EventArgs e)
    //{
    //    string status = string.Empty;
    //    for (int i = 0; i < gvLogListRepeater.Items.Count; i++)
    //    {
    //        RadCheckBox chk = (RadCheckBox)gvLogListRepeater.Items[i].FindControl("chkDate");
    //        Label lblStatus = (Label)gvLogListRepeater.Items[i].FindControl("lblStatus");

    //        if (chk.Checked.HasValue && chk.Checked.Value && lblStatus != null && lblStatus.Text.ToLower() == "closed")
    //        {
    //            status = lblStatus.Text;
    //            Label lblDate = (Label)gvLogListRepeater.Items[i].FindControl("lblDate");
    //            string date = lblDate.Text;
    //            PdfDownload(date);
    //        }
    //    }

    //    LoadExcelTemplate(lbldate.Text); // while reloading it requires to load excel sheet too

    //    if (string.IsNullOrEmpty(status) || status.ToLower() == "open")
    //    {
    //        ucError.ErrorMessage = "Export to pdf only allowed for closed date";
    //        ucError.Visible = true;
    //    }
    //}
    protected void lnkPdf_Click(object sender, EventArgs e)
    {

        if (RadSpreadsheet1.Sheets.Count > 0)
        {
            Worksheet sheet = RadSpreadsheet1.Sheets[0];
            string str = sheet.Rows[2].Cells[5].Value.ToString();
        }
        PdfDownload(txtdate.SelectedDate.Value.ToString("dd/MM/yyyy"));

        //LoadExcelTemplate(lbldate.Text); // while reloading it requires to load excel sheet too

    }

    private void BindLogList()
    {
        DataSet ds = GetLogList();
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            gvLogListRepeater.DataSource = ds;
            RadLabel lblRecords = (RadLabel)gvLogListRepeater.FindControl("lblRecords");
            ViewState["RowCount"] = ds.Tables[0].Rows.Count.ToString();
            gvLogListRepeater.DataBind();
        }
    }


    protected void gvLogListRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RadLabel lblRecords = (RadLabel)e.Item.FindControl("lblRecords");
        RadCheckBox chkDate = (RadCheckBox)e.Item.FindControl("chkDate");
        DataRowView dr = (DataRowView)e.Item.DataItem;
        string status = string.Empty;
        if (dr != null)
        {
            status = Convert.ToString(dr["FLDSTATUS"]);
        }

        if (lblRecords != null && ViewState["RowCount"] != null)
        {
            lblRecords.Text = ViewState["RowCount"].ToString();
        }

        if (chkDate != null && string.IsNullOrWhiteSpace(status) == false && status.ToLower() == "open")
        {
            chkDate.Enabled = false;
        }
    }

    //protected void lnkHistory_Click(object sender, EventArgs e)
    //{
    //    bool isCheckBoxChecked = false;
    //    for (int i = 0; i < gvLogListRepeater.Items.Count; i++)
    //    {
    //        RadCheckBox chk = (RadCheckBox)gvLogListRepeater.Items[i].FindControl("chkDate");
    //        if (chk.Checked.HasValue && chk.Checked.Value)
    //        {
    //            isCheckBoxChecked = true;
    //            Label lblDate = (Label)gvLogListRepeater.Items[i].FindControl("lblDate");
    //            string date = lblDate.Text;
    //            string script = string.Format("openHistory('{0}')", date);
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "History", script, true);
    //            LoadExcelTemplate(lbldate.Text);
    //        }
    //    }

    //    if (isCheckBoxChecked == false)
    //    {
    //        ucError.ErrorMessage = "Please select a date and click history button";
    //        ucError.Visible = true;
    //    }

    //}
    protected void lnkHistory_Click(object sender, EventArgs e)
    {
        string date = txtdate.SelectedDate.Value.ToString("dd/MM/yyyy");
        string script = string.Format("openHistory('{0}')", date);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "History", script, true);
       // LoadExcelTemplate(lbldate.Text);
    }

    protected void lnkconfiguration_Click(object sender, EventArgs e)
    {
        string script = string.Format("openConfig()");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Configuration", script, true);
      //  LoadExcelTemplate(lbldate.Text);
    }
}