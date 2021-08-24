using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.Collections.Specialized;
using System.Web;
using System.Text;
using System.IO;
using OfficeOpenXml;
using Telerik.Web.UI;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using OfficeOpenXml.Style;
using System.Drawing;
using SouthNests.Phoenix.Registers;

public partial class VesselPositionIMODCSReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionIMODCSReport.aspx", "Export Daily Report Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionIMODCSReport.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionIMODCSReport.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionIMODCSReport.aspx", "Send Daily Report Mail", "<i class=\"fa-envelope\"></i>", "MAIL");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionIMODCSReport.aspx", "ABS Report Excel", "<i class=\"fas fa-file-excel\"></i>", "ABS");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionIMODCSReport.aspx", "ABS Report Summary Excel", "<i class=\"fas fa-file-excel\"></i>", "ABSSUMMARY");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionIMODCSReport.aspx", "Anual Total Excel", "<i class=\"fas fa-file-excel\"></i>", "ANUALTOTAL");

        MenuDayToDayReportTab.AccessRights = this.ViewState;
        MenuDayToDayReportTab.MenuList = toolbar.Show();
        //MenuDayToDayReportTab.SetTrigger(pnlDayToDayReport);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Daily Report", "DAILYREPORT");
        toolbarmain.AddButton("History", "HISTORY");
        MainTab.AccessRights = this.ViewState;
        MainTab.MenuList = toolbarmain.Show();
        MainTab.SelectedMenuIndex = 0;

        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        if (!IsPostBack)
        {
            try
            {
                ddlVessel.bind();
                ddlVessel.DataBind();                
                BindVesselFleetList();

                txtFrom.Text = Convert.ToDateTime(DateTime.Now.AddYears(-1).Year.ToString() + "-01-01").ToString();
                txtTo.Text = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-01-01").ToString();

                NameValueCollection nvc = Filter.CurrentIMODCSReportFilter;
                if (nvc != null && !IsPostBack)
                {
                    txtFrom.Text = (nvc.Get("txtReportFrom") == null) ? "" : nvc.Get("txtReportFrom").ToString();
                    txtTo.Text = (nvc.Get("txtReportTo") == null) ? "" : nvc.Get("txtReportTo").ToString();
                    ddlVessel.SelectedVessel = (nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString();
                    ddlFleet.SelectedValue = (nvc.Get("ddlFleet") == null) ? "" : nvc.Get("ddlFleet").ToString();
                }
                BindMPAVEBPVessel();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["PAGENUMBERBDN"] = 1;

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
            gvDayToDayReport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            //gvConsumption.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        
    }

    private void BindMPAVEBPVessel()
    {
        try
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ddlVessel.Enabled = false;
            }
            else
            {
                ddlVessel.Enabled = true;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void DayToDayReportTab_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
           else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                if (General.GetNullableInteger(ddlVessel.SelectedVessel) == null)
                {
                    ucError.ErrorMessage = "Vessel is required.";
                    ucError.Visible = true;
                }
                else
                {
                    if (ViewState["ROWCOUNT"] != null && General.GetNullableInteger(ViewState["ROWCOUNT"].ToString()) > 0)
                    {
                        string Filepath = SaveExcel(2);
                        var fileInfo = new System.IO.FileInfo(Filepath);
                        Response.ContentType = "application/octet-stream";
                        Response.AddHeader("Content-Disposition", String.Format("attachment;filename=\"{0}\"", Path.GetFileName(Filepath)));
                        Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                        Response.WriteFile(Filepath);
                        Response.End();
                    }
                }
            }
            else if(CommandName.ToUpper().Equals("SEARCH"))
            {
                Rebind();
            }

            else if (CommandName.ToUpper().Equals("RESET"))
            {
                ClearFilter();
            }
            else if(CommandName.ToUpper().Equals("MAIL"))
            {
                if (General.GetNullableInteger(ddlVessel.SelectedVessel) == null)
                {
                    ucError.ErrorMessage = "Vessel is required.";
                    ucError.Visible = true;
                }
                else
                {
                    string Filepath = SaveExcel(1);
                    mailsend(Filepath);
                }
            }
            else if (CommandName.ToUpper().Equals("ABS"))
            {
                if (General.GetNullableInteger(ddlVessel.SelectedVessel) != null)
                    ABSExport();
            }
            else if (CommandName.ToUpper().Equals("ABSSUMMARY"))
            {
                if (General.GetNullableInteger(ddlVessel.SelectedVessel) != null)
                    ABSExportSummary();
            }
            else if (CommandName.ToUpper().Equals("ANUALTOTAL"))
            {
                if (General.GetNullableInteger(ddlVessel.SelectedVessel) != null)
                    ExportAnualTotal();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ABSExport()
    {
        DataSet abs = PhoenixVesselPositionDayToDayReport.IMODCSDailyReportABSSearch(General.GetNullableInteger(ddlVessel.SelectedVessel)
            , General.GetNullableDateTime(txtFrom.Text)
            , General.GetNullableDateTime(txtTo.Text)
            , General.GetNullableInteger(ddlFleet.SelectedValue));

        DataSet BDN = PhoenixVesselPositionDayToDayReport.BunkerDeliveryABSList(
            General.GetNullableDateTime(txtFrom.Text)
           , General.GetNullableDateTime(txtTo.Text)
           , General.GetNullableInteger(ddlVessel.SelectedVessel)
           , General.GetNullableInteger(ddlFleet.SelectedValue));

        DataTable dt = abs.Tables[0];
        DataTable DTBDN = BDN.Tables[0];
        if (dt.Rows.Count > 0)
        {
            string path = HttpContext.Current.Server.MapPath("~\\Template\\VesselPosition" + @"\\IMO_DCS_Daily_Report.xlsx");

            var file = new FileInfo(path);

            string vesselname = ddlVessel.SelectedVesselName;

            if (file.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    var wb = excelPackage.Workbook.Worksheets["IMO DCS Data"];
                    var wbBDN = excelPackage.Workbook.Worksheets["IMO DCS BDN Summary"];
                    //var wb = excelPackage.Workbook;


                   
                    if (dt.Rows.Count > 0)
                    {
                        int iRow = 2;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            wb.Cells[iRow, 1].Value = dr["FLDFROMDATE"].ToString();
                            wb.Cells[iRow, 2].Value = dr["FLDDATEUTC"].ToString();
                            wb.Cells[iRow, 3].Value = dr["FLDHOURSUNDERWAY"].ToString();
                            wb.Cells[iRow, 4].Value = dr["FLDDISTANCETRAVELLED"].ToString();
                            wb.Cells[iRow, 5].Value = dr["FLDHFOCONS"].ToString();
                            wb.Cells[iRow, 6].Value = dr["FLDLSFOCONS"].ToString();
                            wb.Cells[iRow, 7].Value = dr["FLDMDOCONS"].ToString();
                            iRow = iRow + 1;
                        }
                    }
                    if (DTBDN.Rows.Count > 0)
                    {
                        int iRow = 2;

                        for (int i = 0; i < DTBDN.Rows.Count; i++)
                        {
                            DataRow dr = DTBDN.Rows[i];
                            wbBDN.Cells[iRow, 1].Value = dr["FLDTYPE"].ToString();
                            wbBDN.Cells[iRow, 2].Value = dr["FLDDATE"].ToString();
                            wbBDN.Cells[iRow, 3].Value = dr["FLDHFO"].ToString();
                            wbBDN.Cells[iRow, 4].Value = dr["FLDLFO"].ToString();
                            wbBDN.Cells[iRow, 5].Value = dr["FLDMDOMGO"].ToString();
                            iRow = iRow + 1;
                        }
                    }
                    //excelPackage.Workbook.Properties.Title = "Attempts";
                    this.Response.ClearHeaders();
                    this.Response.ClearContent();
                    this.Response.Clear();
                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}", "IMO_DCS_Daily_Report_"+ vesselname.Replace(" ", "_") + ".xlsx"));
                    this.Response.BinaryWrite(excelPackage.GetAsByteArray());
                    this.Response.Flush();
                    this.Response.Close();
                }
            }
        }
    }
    private void ABSExportSummary()
    {
        DataSet abs = PhoenixVesselPositionDayToDayReport.IMODCSDailyReportABSSummary(General.GetNullableInteger(ddlVessel.SelectedVessel)
            , General.GetNullableDateTime(txtFrom.Text)
            , General.GetNullableDateTime(txtTo.Text)
            , General.GetNullableInteger(ddlFleet.SelectedValue));



        DataTable dt = abs.Tables[0];

        if (dt.Rows.Count > 0)
        {
            string path = HttpContext.Current.Server.MapPath("~\\Template\\VesselPosition" + @"\\IMO_DCS_Daily_Report_Summary.xlsx");

            var file = new FileInfo(path);

            string vesselname = ddlVessel.SelectedVesselName;

            if (file.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    var wb = excelPackage.Workbook.Worksheets["IMO DCS Data Summary"];
                    //var wb = excelPackage.Workbook;



                    if (dt.Rows.Count > 0)
                    {
                        int iRow = 2;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            wb.Cells[iRow, 1].Value = dr["FLDFROMDATE"].ToString();
                            wb.Cells[iRow, 2].Value = dr["FLDDATEUTC"].ToString();
                            wb.Cells[iRow, 3].Value = dr["FLDHOURSUNDERWAY"].ToString();
                            wb.Cells[iRow, 4].Value = dr["FLDDISTANCETRAVELLED"].ToString();
                            wb.Cells[iRow, 5].Value = dr["FLDHFOCONS"].ToString();
                            wb.Cells[iRow, 6].Value = dr["FLDLSFOCONS"].ToString();
                            wb.Cells[iRow, 7].Value = dr["FLDMDOCONS"].ToString();
                            iRow = iRow + 1;
                        }
                    }
 
                    //excelPackage.Workbook.Properties.Title = "Attempts";
                    this.Response.ClearHeaders();
                    this.Response.ClearContent();
                    this.Response.Clear();
                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}", "IMO_DCS_Daily_Report_Summary_" + vesselname.Replace(" ","_") + ".xlsx"));
                    this.Response.BinaryWrite(excelPackage.GetAsByteArray());
                    this.Response.Flush();
                    this.Response.Close();
                }
            }
        }
    }
    private void ExportAnualTotal()
    {
        DataSet abs = PhoenixVesselPositionDayToDayReport.IMODCSDailyReportABSSummary(General.GetNullableInteger(ddlVessel.SelectedVessel)
            , General.GetNullableDateTime(txtFrom.Text)
            , General.GetNullableDateTime(txtTo.Text)
            , General.GetNullableInteger(ddlFleet.SelectedValue));



        DataTable dt = abs.Tables[0];

        if (dt.Rows.Count > 0)
        {
            string path = HttpContext.Current.Server.MapPath("~\\Template\\VesselPosition" + @"\\IMODCS_Annual_Total.xlsx");

            var file = new FileInfo(path);

            string vesselname = ddlVessel.SelectedVesselName;

            if (file.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    var wb = excelPackage.Workbook.Worksheets["Annual Total"];
                    //var wb = excelPackage.Workbook;



                    if (dt.Rows.Count > 0)
                    {
                        int iRow = 3;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            wb.Cells[iRow, 1].Value = dr["FLDMETHODUSED"].ToString();
                            wb.Cells[iRow, 7].Value = dr["FLDHFOCONS"].ToString();
                            wb.Cells[iRow, 8].Value = dr["FLDLSFOCONS"].ToString();
                            wb.Cells[iRow, 9].Value = dr["FLDMDOCONS"].ToString();

                            wb.Cells[iRow, 10].Value = dr["FLDHOURSUNDERWAY"].ToString();
                            wb.Cells[iRow, 11].Value = dr["FLDDISTANCETRAVELLED"].ToString();
                            wb.Cells[iRow, 12].Value = dr["FLDAEPOWEROUTPUT"].ToString();

                            wb.Cells[iRow, 13].Value = dr["FLDMEPOWEROUTPUT"].ToString();
                            wb.Cells[iRow, 14].Value = dr["FLDICECLASS"].ToString();
                            wb.Cells[iRow, 15].Value = dr["FLDEEDI"].ToString();
                            wb.Cells[iRow, 16].Value = dr["FLDDWTSUMMER"].ToString();
                            wb.Cells[iRow, 17].Value = dr["FLDREGISTEREDNT"].ToString();
                            wb.Cells[iRow, 18].Value = dr["FLDREGISTEREDGT"].ToString();

                            wb.Cells[iRow, 19].Value = dr["FLDVESSELTYPE"].ToString();
                            wb.Cells[iRow, 20].Value = dr["FLDIMONUMBER"].ToString();
                            wb.Cells[iRow, 21].Value = dr["FLDTO"].ToString();
                            wb.Cells[iRow, 22].Value = dr["FLDFROM"].ToString();
                            iRow = iRow + 1;
                        }
                    }

                    //excelPackage.Workbook.Properties.Title = "Attempts";
                    this.Response.ClearHeaders();
                    this.Response.ClearContent();
                    this.Response.Clear();
                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}", "IMODCS_Annual_Total" + vesselname.Replace(" ", "_") + ".xlsx"));
                    this.Response.BinaryWrite(excelPackage.GetAsByteArray());
                    this.Response.Flush();
                    this.Response.Close();
                }
            }
        }
    }
    private void mailsend(string Filepath)
    {
        DataSet ds = PhoenixVesselPositionEUMRVSummaryReport.EUMRVVerifierDetail(General.GetNullableInteger(ddlVessel.SelectedVessel.ToString()) == null ? 0 : int.Parse(ddlVessel.SelectedVessel));

        if (ds.Tables[0].Rows.Count > 0 && General.GetNullableString(ds.Tables[0].Rows[0]["FLDMPATOMAIL"].ToString()) != null)
        {
            DataTable dt = ds.Tables[0];
            string tomail = dt.Rows[0]["FLDMPATOMAIL"].ToString();
            string ccmail = dt.Rows[0]["FLDMPACCMAIL"].ToString();
            //string dataformat = dt.Rows[0]["FLDDATAFORMAT"].ToString();
            string subject = dt.Rows[0]["FLDIMONUMBER"].ToString() + "_" + dt.Rows[0]["FLDVESSELNAME"].ToString();

            StringBuilder emailbody = new StringBuilder();
            emailbody.AppendLine("Dear Sir,");
            emailbody.AppendLine();
            emailbody.AppendLine("Attached please find data as required by the Recognized Organisation");
            emailbody.AppendLine();
            emailbody.AppendLine("Thank you");

            string[] strarrfilenames = new string[1];

            strarrfilenames[0] = Filepath;

            PhoenixMail.SendMail(tomail.ToString().Replace(";", ",").Replace(",,", ",").TrimEnd(','),
            ccmail.Replace(";", ",").Replace(",,", ",").TrimEnd(','),
            "",
            subject,
            emailbody.ToString(), false,
            System.Net.Mail.MailPriority.Normal,
            "",
            strarrfilenames,
            null);

            ucStatus.Text = "Mail Sent.";
        }
        else
        {
            ucError.ErrorMessage = "Mail Id Not Configured.";
            ucError.Visible = true;
        }
    }
    private string SaveExcel(int flag)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet dsVEBP = PhoenixVesselPositionDayToDayReport.IMODCSDailyReportSearch(General.GetNullableInteger(ddlVessel.SelectedVessel),
           null, null,
            1,
           (int)ViewState["ROWCOUNT"],
            ref iRowCount,
            ref iTotalPageCount
            , General.GetNullableDateTime(txtFrom.Text)
            , General.GetNullableDateTime(txtTo.Text)
            , General.GetNullableInteger(ddlFleet.SelectedValue));
        DataRow dr = dsVEBP.Tables[0].Rows[0];
        string[] alColumns = { "FLDVOYAGENUMBER", "FLDMDATE", "FLDTIME", "FLDREPORTTYPE", "FLDBALASTLADEN", "FLDCARGOONBOARD", "FLDSEAPORTNAME", "FLDSEAPORTCODE", "FLDDISTANCETRAVELLED", "FLDHOURSUNDERWAY", "FLDHFOCONS", "FLDLSFOCONS", "FLDVLSFORMCONS", "FLDULSFORMCONS", "FLDMDOCONS", "FLDMGOCONS", "FLDVLSFODMCONS", "FLDULSFOFMCONS", "FLDHFOROB", "FLDLSFOROB", "FLDVLSFORMROB", "FLDULSFORMROB", "FLDMDOROB", "FLDMGOROB", "FLDVLSFODMROB", "FLDULSFOFMROB", "FLDHFOBUNKER", "FLDLSFOBUNKER", "FLDVLSFORMBUNKER", "FLDULSFORMBUNKER", "FLDMDOBUNKER", "FLDMGOBUNKER", "FLDVLSFODMBUNKER", "FLDULSFOFMBUNKER", "FLDHFODEBUNKER", "FLDLSFODEBUNKER", "FLDVLSFORMDEBUNKER", "FLDULSFORMDEBUNKER", "FLDMDODEBUNKER", "FLDMGODEBUNKER", "FLDVLSFODMDEBUNKER", "FLDULSFOFMDEBUNKER", "FLDBDNYN" };
        string[] alCaptions = { "Voy No", "Date", "Time", "Report Type", "Laden/Ballast", "Cargo (MT)", "Port", "UNLOCODE", "Distance Travelled", "Hours Underway", "HFO","LFO", "VLSFO", "ULSFO","MDO", "MGO","VLSMGO", "ULSMGO", "HFO", "LFO", "VLSFO", "ULSFO", "MDO", "MGO", "VLSMGO", "ULSMGO", "HFO", "LFO", "VLSFO", "ULSFO", "MDO", "MGO", "VLSMGO", "ULSMGO", "HFO", "LFO", "VLSFO", "ULSFO", "MDO", "MGO", "VLSMGO", "ULSMGO", "BDN Attached"};
        string vesselname = "";
         vesselname = General.GetNullableInteger(ddlVessel.SelectedVessel) != null ? "IMO DCS_" + ddlVessel.SelectedVesselName : "IMODCS";
        string path = Server.MapPath("~/Attachments/TEMP/" + vesselname + "_" + DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd") + "_DailyReport.xlsx");
        if (File.Exists(path))
            File.Delete(path);

        FileInfo fiTemplate = new FileInfo(path);
        using (ExcelPackage pck = new ExcelPackage(fiTemplate))
        {
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("IMO DCS");
            ws.DefaultRowHeight = 18;
            int nRow = 7;
            int nCol = 1;
            //Data column headings
            ws.Cells[1, 1].Value = "IMO No";
            ws.Cells[1, 2].Value = dr["FLDIMONUMBER"].ToString();
            ws.Cells[1, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[1, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[1, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[1, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            ws.Cells[1, 3].Value = "Vessel Name";
            ws.Cells[1, 4].Value = dr["FLDVESSELNAME"].ToString();
            ws.Cells[1, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[1, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[1, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[1, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells[1, 5].Value = "Ship Type";
            ws.Cells[1, 6].Value = dr["FLDVESSELTYPE"].ToString();
            ws.Cells[1, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[1, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[1, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[1, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells[1, 7].Value = "Ice Class";
            ws.Cells[1, 8].Value = dr["FLDICECLASS"].ToString();
            ws.Cells[1, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[1, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[1, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[1, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells[2, 1].Value = "GRT";
            ws.Cells[2, 2].Value = dr["FLDREGISTEREDGT"].ToString();
            ws.Cells[2, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            ws.Cells[2, 3].Value = "NRT";
            ws.Cells[2, 4].Value = dr["FLDREGISTEREDNT"].ToString();
            ws.Cells[2, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            ws.Cells[2, 5].Value = "DWT";
            ws.Cells[2, 6].Value = dr["FLDDWTSUMMER"].ToString();
            ws.Cells[2, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[2, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            ws.Cells[3, 1].Value = "Power Output (kW)";

            ws.Cells[3, 3].Value = "Main Engine";
            ws.Cells[3, 4].Value = dr["FLDMEPOWEROUTPUT"].ToString();
            ws.Cells[3, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[3, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[3, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[3, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[3, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            ws.Cells[3, 5].Value = "Aux Engines";
            ws.Cells[3, 6].Value = dr["FLDAEPOWEROUTPUT"].ToString();
            ws.Cells[3, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[3, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[3, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[3, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[3, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            ws.Cells[3, 7].Value = "EEDI";
            ws.Cells[3, 8].Value = dr["FLDEEDI"].ToString();
            ws.Cells[3, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[3, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[3, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[3, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[3, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            ws.Cells[4, 1].Value = "Method used to measure FOC";
            ws.Cells[4, 4].Value = dr["FLDMETHODUSED"].ToString();
            ws.Cells[4, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[4, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[4, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[4, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[4, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            ws.Cells[6, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[6, 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[6, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[6, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells[6, 2].Value = "Report Info (UTC)";
            ws.Cells["B6:D6"].Merge = true;
            ws.Cells["B6:D6"].Style.Font.Bold = true;
            ws.Cells["B6:D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["B6:D6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["B6:D6"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["B6:D6"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["B6:D6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells[6, 5].Value = "Loading Condition";
            ws.Cells["E6:F6"].Merge = true;
            ws.Cells["E6:F6"].Style.Font.Bold = true;
            ws.Cells["E6:F6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["E6:F6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["E6:F6"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["E6:F6"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["E6:F6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells[6, 7].Value = "Port Info";
            ws.Cells["G6:H6"].Merge = true;
            ws.Cells["G6:H6"].Style.Font.Bold = true;
            ws.Cells["G6:H6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["G6:H6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["G6:H6"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["G6:H6"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["G6:H6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells[6, 9].Value = "From Last Report";
            ws.Cells["I6:J6"].Merge = true;
            ws.Cells["I6:J6"].Style.Font.Bold = true;
            ws.Cells["I6:J6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["I6:J6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["I6:J6"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["I6:J6"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["I6:J6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells[6, 11].Value = "Fuel Oil Consumption (MT)";
            ws.Cells["K6:R6"].Merge = true;
            ws.Cells["K6:R6"].Style.Font.Bold = true;
            ws.Cells["K6:R6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["K6:R6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["K6:R6"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["K6:R6"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["K6:R6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells[6, 19].Value = "ROB (MT)";
            ws.Cells["S6:Z6"].Merge = true;
            ws.Cells["S6:Z6"].Style.Font.Bold = true;
            ws.Cells["S6:Z6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["S6:Z6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["S6:Z6"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["S6:Z6"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["S6:Z6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells[6, 27].Value = "Bunkered (MT)";
            ws.Cells["AA6:AH6"].Merge = true;
            ws.Cells["AA6:AH6"].Style.Font.Bold = true;
            ws.Cells["AA6:AH6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["AA6:AH6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["AA6:AH6"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["AA6:AH6"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["AA6:AH6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells[6, 35].Value = "De-Bunkered (MT)";
            ws.Cells["AI6:AP6"].Merge = true;
            ws.Cells["AI6:AP6"].Style.Font.Bold = true;
            ws.Cells["AI6:AP6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["AI6:AP6"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["AI6:AP6"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["AI6:AP6"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["AI6:AP6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


            ws.Cells[6, 43].Style.Font.Bold = true;
            ws.Cells[6, 43].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[6, 43].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[6, 43].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[6, 43].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            
            for (int i = 0; i < alCaptions.Length; i++)
            {
                ws.Cells[nRow, (nCol + i)].Value = alCaptions[i];
                ws.Cells[nRow, (nCol + i)].Style.Font.Bold = true;
                ws.Cells[nRow, (nCol + i)].AutoFitColumns();
                ws.Cells[nRow, (nCol + i)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[nRow, (nCol + i)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[nRow, (nCol + i)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[nRow, (nCol + i)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }
            // Data rows
            for (int i = 0; i < dsVEBP.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < alColumns.Length; j++)
                {
                    if (dsVEBP.Tables[0].Columns[alColumns[j]].ColumnName.ToUpper().Equals("FLDMDATE"))
                        ws.Cells[nRow + (i + 1), (nCol + j)].Style.Numberformat.Format = "dd/MM/yyyy";
                    ws.Cells[nRow + (i + 1), (nCol + j)].Value = dsVEBP.Tables[0].Rows[i][alColumns[j]];
                    //ws.Cells[nRow + (i + 1), (nCol + j)].AutoFitColumns();
                    ws.Cells[nRow+ (i + 1), (nCol + j)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[nRow+ (i + 1), (nCol + j)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[nRow+ (i + 1), (nCol + j)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[nRow + (i + 1), (nCol + j)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
            }
            ws.Column(7).Width = 30;
            ws.Column(1).Width = 20;
            ws.Column(2).Width = 12;
            pck.SaveAs(fiTemplate);
          
        }
        return path;
    }
      private void ClearFilter()
    {
        ViewState["PAGENUMBER"] = 1;
        ddlVessel.SelectedVessel = "";
        txtFrom.Text = "";
        txtTo.Text = "";
        Rebind();
    }

    protected void ddlVessel_TextChangedEvent(object sender,EventArgs e)
    {
        Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
            Rebind();
    }

    protected void gvDayToDayReport_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDayToDayReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDayToDayReport.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVOYAGENUMBER", "FLDMDATE", "FLDTIME", "FLDREPORTTYPE", "FLDBALASTLADEN", "FLDCARGOONBOARD", "FLDSEAPORTNAME", "FLDSEAPORTCODE", "FLDDISTANCETRAVELLED", "FLDHOURSUNDERWAY", "FLDHFOCONS", "FLDLSFOCONS", "FLDVLSFORMCONS", "FLDULSFORMCONS", "FLDMDOCONS", "FLDMGOCONS", "FLDVLSFODMCONS", "FLDULSFOFMCONS", "FLDHFOROB", "FLDLSFOROB", "FLDVLSFORMROB", "FLDULSFORMROB", "FLDMDOROB", "FLDMGOROB", "FLDVLSFODMROB", "FLDULSFOFMROB", "FLDHFOBUNKER", "FLDLSFOBUNKER", "FLDVLSFORMBUNKER", "FLDULSFORMBUNKER", "FLDMDOBUNKER", "FLDMGOBUNKER", "FLDVLSFODMBUNKER", "FLDULSFOFMBUNKER", "FLDHFODEBUNKER", "FLDLSFODEBUNKER", "FLDVLSFORMDEBUNKER", "FLDULSFORMDEBUNKER", "FLDMDODEBUNKER", "FLDMGODEBUNKER", "FLDVLSFODMDEBUNKER", "FLDULSFOFMDEBUNKER", "FLDBDNYN" };
        string[] alCaptions = { "Voy No", "Date", "Time", "Report Type", "Laden/Ballast", "Cargo (MT)", "Port", "UNLOCODE", "Distance Travelled", "Hours Underway", "HFO", "LFO", "VLSFO", "ULSFO", "MDO", "MGO", "VLSMGO", "ULMGO", "HFO", "LFO", "VLSFO", "ULSFO", "MDO", "MGO", "VLSMGO", "ULMGO", "HFO", "LFO", "VLSFO", "ULSFO", "MDO", "MGO", "VLSMGO", "ULMGO", "HFO", "LFO", "VLSFO", "ULSFO", "MDO", "MGO", "VLSMGO", "ULMGO", "BDN Attached" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselPositionDayToDayReport.IMODCSDailyReportSearch(General.GetNullableInteger(ddlVessel.SelectedVessel),
            sortexpression, sortdirection,
             int.Parse(ViewState["PAGENUMBER"].ToString()),
             gvDayToDayReport.PageSize,
             ref iRowCount,
             ref iTotalPageCount
             , General.GetNullableDateTime(txtFrom.Text)
             , General.GetNullableDateTime(txtTo.Text)
             , General.GetNullableInteger(ddlFleet.SelectedValue));

        General.SetPrintOptions("gvDayToDayReport", "Daily Report", alCaptions, alColumns, ds);

        gvDayToDayReport.DataSource = ds;
        gvDayToDayReport.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
        criteria.Add("ddlFleet", ddlFleet.SelectedValue);
        criteria.Add("txtReportFrom", txtFrom.Text);
        criteria.Add("txtReportTo", txtTo.Text);

        Filter.CurrentIMODCSReportFilter = criteria;

        gvDayToDayReport.SelectedIndexes.Clear();
        gvDayToDayReport.EditIndexes.Clear();
        gvDayToDayReport.DataSource = null;
        gvDayToDayReport.Rebind();
    }
    
    private void BindVesselFleetList()
    {
        DataSet ds = PhoenixRegistersFleet.ListFleet();

        ddlFleet.DataSource = ds;
        ddlFleet.DataTextField = "FLDFLEETDESCRIPTION";
        ddlFleet.DataValueField = "FLDFLEETID";
        ddlFleet.DataBind();
        ddlFleet.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void MainTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("HISTORY"))
        {
            Response.Redirect("../VesselPosition/VesselPositionReportingDodumentsHistory.aspx");
        }
    }
}
