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

public partial class VesselPositionIMODCSReportBDNSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionIMODCSReportBDNSummary.aspx", "Export BDN Summary Excel", "<i class=\"fas fa-file-excel\"></i>", "BDNEXCEL");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionIMODCSReportBDNSummary.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionIMODCSReportBDNSummary.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionIMODCSReportBDNSummary.aspx", "Send BDN Summary Mail", "<i class=\"fa-envelope\"></i>", "BDN");

        MenuDayToDayReportTab.AccessRights = this.ViewState;
        MenuDayToDayReportTab.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Daily Report", "DAILYREPORT");
        toolbarmain.AddButton("BDN Summary", "BDNSUMMARY");
        MainTab.AccessRights = this.ViewState;
        MainTab.MenuList = toolbarmain.Show();
        MainTab.SelectedMenuIndex = 1;

        //MenuDayToDayReportTab.SetTrigger(pnlDayToDayReport);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        if (!IsPostBack)
        {
            try
            {
                ddlVessel.bind();
                ddlVessel.DataBind();
                BindMPAVEBPVessel();
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
            //gvDayToDayReport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvConsumption.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        
    }
    private void BindMPAVEBPVessel()
    {
        try
        {
            //DataSet ds = PhoenixVesselPositionDayToDayReport.ListMPAVEBPVsessel();
            //ddlVessel.DataSource=ds.Tables[0];
            //ddlVessel.DataValueField = "FLDVESSELID";
            //ddlVessel.DataTextField = "FLDVESSELNAME";
            //ddlVessel.DataBind();
            //ddlVessel.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //        if (ds.Tables[0].Rows[i]["FLDVESSELID"].ToString() == PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                //        {
                ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ddlVessel.Enabled = false;
                //        }
                //}
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
                RebindgvConsumption();
            }
            else if(CommandName.ToUpper().Equals("SEARCH"))
            {
                RebindgvConsumption();
            }

            else if (CommandName.ToUpper().Equals("RESET"))
            {
                ClearFilter();
            }
            else if(CommandName.ToUpper().Equals("BDN"))
            {
                if (General.GetNullableInteger(ddlVessel.SelectedVessel) == null)
                {
                    ucError.ErrorMessage = "Vessel is required.";
                    ucError.Visible = true;
                }
                else
                {
                    string Filepath = bunker();
                    mailsend(Filepath);
                }
            }
            else if(CommandName.ToUpper().Equals("BDNEXCEL"))
            {
                if (General.GetNullableInteger(ddlVessel.SelectedVessel) == null)
                {
                    ucError.ErrorMessage = "Vessel is required.";
                    ucError.Visible = true;
                }
                else
                {
                    string Filepath = bunker();
                    var fileInfo = new System.IO.FileInfo(Filepath);
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", String.Format("attachment;filename=\"{0}\"", Path.GetFileName(Filepath)));
                    Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    Response.WriteFile(Filepath);
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    private  string bunker()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        decimal mdomgototal = 0;
        decimal lfototal = 0;
        decimal hfototal = 0;

        DataSet ds = PhoenixVesselPositionDayToDayReport.BunkerDeliveryList(
             General.GetNullableDateTime(txtFrom.Text)
           , General.GetNullableDateTime(txtTo.Text)
           , General.GetNullableInteger(ddlVessel.SelectedVessel)
           , 1
           , 10000
           , ref iRowCount
           , ref iTotalPageCount
           , General.GetNullableInteger(ddlFleet.SelectedValue));

        if (ds.Tables[0].Rows.Count>0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                mdomgototal = mdomgototal + Convert.ToDecimal(General.GetNullableDecimal(dr["FLDMDOMGO"].ToString()) != null ? dr["FLDMDOMGO"].ToString() : "0");
                lfototal = lfototal + Convert.ToDecimal(General.GetNullableDecimal(dr["FLDLFO"].ToString()) != null ? dr["FLDLFO"].ToString() : "0");
                hfototal = hfototal + Convert.ToDecimal(General.GetNullableDecimal(dr["FLDHFO"].ToString()) != null ? dr["FLDHFO"].ToString() : "0");
            }
        }
        
            DataSet dstank = PhoenixVesselPositionDayToDayReport.TankSoundingLogList(
             General.GetNullableDateTime(txtFrom.Text)
           , General.GetNullableDateTime(txtTo.Text)
           , General.GetNullableInteger(ddlVessel.SelectedVessel)
           , General.GetNullableInteger(ddlFleet.SelectedValue));

        decimal mdomgofirst = 0;
        decimal lfofirst = 0;
        decimal hfofirst = 0;

        decimal mdomgoLast = 0;
        decimal lfolast = 0;
        decimal hfolast = 0;

        decimal mdomgoDiff = 0;
        decimal lfoDiff = 0;
        decimal hfoDiff = 0;

        //if (dstank.Tables[0].Rows.Count > 0)
        //{
        //    for (int i = 0; i < dstank.Tables[0].Rows.Count;i++)
        //    {
        //        DataRow dr = dstank.Tables[0].Rows[i];
        //        if (i == 0)
        //        {
        //            mdomgofirst = Convert.ToDecimal(General.GetNullableDecimal(dr["FLDMDOMGO"].ToString()) != null ? Convert.ToDecimal(dr["FLDMDOMGO"].ToString()) : 0);
        //            lfofirst = Convert.ToDecimal(General.GetNullableDecimal(dr["FLDLFO"].ToString()) != null ? Convert.ToDecimal(dr["FLDLFO"].ToString()) : 0);
        //            hfofirst = Convert.ToDecimal(General.GetNullableDecimal(dr["FLDHFO"].ToString()) != null ? Convert.ToDecimal(dr["FLDHFO"].ToString()) : 0);

        //        }
        //        if (i == dstank.Tables[0].Rows.Count - 1)
        //        {
        //            mdomgoLast = Convert.ToDecimal(General.GetNullableDecimal(dr["FLDMDOMGO"].ToString()) != null ? Convert.ToDecimal(dr["FLDMDOMGO"].ToString()) : 0);
        //            lfolast = Convert.ToDecimal(General.GetNullableDecimal(dr["FLDLFO"].ToString()) != null ? Convert.ToDecimal(dr["FLDLFO"].ToString()) : 0);
        //            hfolast = Convert.ToDecimal(General.GetNullableDecimal(dr["FLDHFO"].ToString()) != null ? Convert.ToDecimal(dr["FLDHFO"].ToString()) : 0);

        //        }
        //    }
        //}
        mdomgoDiff = mdomgofirst- mdomgoLast;
        lfoDiff = lfofirst- lfolast;
        hfoDiff = hfofirst- hfolast;

        string[] alColumns = { "FLDDATE", "FLDMDOMGO", "FLDLFO", "FLDHFO", "FLDLPGP", "FLDLPGB", "LNG", "FLDOTHERS", "FLDDESCRIPTION" };
        string[] alCaptions = { "Date of Operations (dd/mm/yyyy)", "DO/GO", "LFO", "HFO", "LPG(P)", "LPG(B)", "LNG", "Others(CF)", "Descriptions"};

        string vesselname = General.GetNullableInteger(ddlVessel.SelectedVessel) != null ? "IMO DCS_" + ddlVessel.SelectedVesselName : "IMODCS";

        string path = Server.MapPath("~/Attachments/TEMP/" + vesselname + "_" + DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd") + "_BDNSummary.xlsx");
        if (File.Exists(path))
            File.Delete(path);

        FileInfo fiTemplate = new FileInfo(path);
        using (ExcelPackage pck = new ExcelPackage(fiTemplate))
        {
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("BDN SUMMARY");
            ws.DefaultRowHeight = 18;
            int nRow = 3;
            int nCol = 1;
            //Data column headings

            ws.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.Gray);

            ws.Cells[1, 2].Value = "Fuel Oil Type/Mass(MT)";
            ws.Cells[1, 2].Style.Font.Bold = true;
            ws.Cells["B1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["B1:H1"].Merge = true;
            ws.Cells["B1:H1"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["B1:H1"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["B1:H1"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["B1:H1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells["B1:H1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["B1:H1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

            ws.Cells[1, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[1, 9].Style.Fill.BackgroundColor.SetColor(Color.Gray);

            for (int i = 0; i < alCaptions.Length; i++)
            {
                ws.Cells[nRow-1, (nCol + i)].Value = alCaptions[i];
                ws.Cells[nRow-1, (nCol + i)].Style.Font.Bold = true;
                ws.Cells[nRow-1, (nCol + i)].AutoFitColumns();
                ws.Cells[nRow-1, (nCol + i)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[nRow-1, (nCol + i)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[nRow-1, (nCol + i)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[nRow-1, (nCol + i)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[nRow-1, (nCol + i)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[nRow-1, (nCol + i)].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            }

            ws.Cells[nRow, 1].Value = "① BDN";
            ws.Cells[nRow, 1].Style.Font.Bold = true;
            ws.Cells["A" + (nRow).ToString() + ":i" + (nRow).ToString() + ""].Merge = true;
            ws.Cells["A" + (nRow).ToString() + ":i" + (nRow).ToString() + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["A" + (nRow).ToString() + ":i" + (nRow).ToString() + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["A" + (nRow).ToString() + ":i" + (nRow).ToString() + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["A" + (nRow).ToString() + ":i" + (nRow).ToString() + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["A" + (nRow).ToString() + ":i" + (nRow).ToString() + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells["A" + (nRow).ToString() + ":i" + (nRow).ToString() + ""].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A" + (nRow).ToString() + ":i" + (nRow).ToString() + ""].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

            // Data rows
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < alColumns.Length; j++)
                {
                    if (ds.Tables[0].Columns[alColumns[j]].ColumnName.ToUpper().Equals("FLDDATE") || ds.Tables[0].Columns[alColumns[j]].ColumnName.ToUpper().Equals("FLDATE"))
                        ws.Cells[nRow + (i + 1), (nCol + j)].Style.Numberformat.Format = "dd/MM/yyyy";
                    ws.Cells[nRow + (i + 1), (nCol + j)].Value = ds.Tables[0].Rows[i][alColumns[j]];
                    //ws.Cells[nRow + (i + 1), (nCol + j)].AutoFitColumns();
                    ws.Cells[nRow + (i + 1), (nCol + j)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[nRow + (i + 1), (nCol + j)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[nRow + (i + 1), (nCol + j)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[nRow + (i + 1), (nCol + j)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
            }
            int dscount1 = ds.Tables[0].Rows.Count;
            int dscount2 = dstank.Tables[0].Rows.Count;


            for (int i = 1; i < alColumns.Length+1; i++)
            {
              
                if(i==1)
                    ws.Cells[dscount1 + 4, i].Value = "① Annual Supply Amount";
                else if(i == 9)
                    ws.Cells[dscount1 + 4, i].Value = "";
                else 
                    ws.Cells[dscount1 + 4, i].Value = 0;
                //ws.Cells[dscount1 + 3, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //ws.Cells[dscount1 + 3, i].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                ws.Cells[dscount1 + 4, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[dscount1 + 4, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[dscount1 + 4, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[dscount1 + 4, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }

            ws.Cells[(dscount1  + 4), 2].Value = mdomgototal;
            ws.Cells[(dscount1  + 4), 3].Value = lfototal;
            ws.Cells[(dscount1  + 4), 4].Value = hfototal;

            ws.Cells[dscount1 + 5, 1].Value = "② Correction for the tank oil remainings";
            ws.Cells[dscount1 + 5, 1].Style.Font.Bold = true;
            ws.Cells["A"+ (dscount1 + 5).ToString() + ":i"+ (dscount1 + 5).ToString() + ""].Merge = true;
            ws.Cells["A" + (dscount1 + 5).ToString() + ":i" + (dscount1 + 5).ToString() + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["A"+ (dscount1 + 5).ToString() + ":i"+ (dscount1 + 5).ToString() + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["A"+ (dscount1 + 5).ToString() + ":i"+ (dscount1 + 5).ToString() + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["A"+ (dscount1 + 5).ToString() + ":i"+ (dscount1 + 5).ToString() + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["A" + (dscount1 + 5).ToString() + ":i" + (dscount1 + 5).ToString() + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells["A" + (dscount1 + 5).ToString() + ":i" + (dscount1 + 5).ToString() + ""].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A" + (dscount1 + 5).ToString() + ":i" + (dscount1 + 5).ToString() + ""].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


            //for (int i = dscount1 + 6; i < (dscount1 + dscount2 + 6); i++)
            //{
            //    for (int j = 0; j < alColumns.Length; j++)
            //    {
            //        string sss = alColumns[j].ToString();
            //        if (dstank.Tables[0].Columns[alColumns[j]].ColumnName.ToUpper().Equals("FLDDATE"))
            //            ws.Cells[i, (nCol + j)].Style.Numberformat.Format = "dd/MM/yyyy";
            //        ws.Cells[i , (nCol + j)].Value = dstank.Tables[0].Rows[i - (dscount1 + 6)][alColumns[j]];
            //        ws.Cells[i , (nCol + j)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            //        ws.Cells[i , (nCol + j)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            //        ws.Cells[i , (nCol + j)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //        ws.Cells[i , (nCol + j)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            //    }
            //}

            dscount2 = 0; // Reset to zero for removing the 

            for (int i = 1; i < alColumns.Length + 1; i++)
            {

                if (i == 1)
                    ws.Cells[dscount1 + dscount2 + 6, i].Value = "② Correction for the tank oil remainings";
                else if (i == 9)
                    ws.Cells[dscount1 + dscount2 + 6, i].Value = "";
                else
                    ws.Cells[dscount1 + dscount2 + 6, i].Value = 0;
                //ws.Cells[dscount1 + 3, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //ws.Cells[dscount1 + 3, i].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                ws.Cells[dscount1 + dscount2 + 6, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[dscount1 + dscount2 + 6, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[dscount1 + dscount2 + 6, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[dscount1 + dscount2 + 6, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }

            ws.Cells[(dscount1 + dscount2 + 6), 2].Value = mdomgoDiff;
            ws.Cells[(dscount1 + dscount2 + 6), 3].Value = lfoDiff;
            ws.Cells[(dscount1 + dscount2 + 6), 4].Value = hfoDiff;

            ws.Cells[(dscount1 + dscount2 + 7), 1].Value = "③ Other corrections";
            ws.Cells[(dscount1 + dscount2 + 7), 1].Style.Font.Bold = true;
            ws.Cells["A" + (dscount1 + dscount2 + 7).ToString() + ":i" + (dscount1 + dscount2 + 7).ToString() + ""].Merge = true;
            ws.Cells["A" + (dscount1 + dscount2 + 7).ToString() + ":i" + (dscount1 + dscount2 + 7).ToString() + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["A" + (dscount1 + dscount2 + 7).ToString() + ":i" + (dscount1 + dscount2 + 7).ToString() + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["A" + (dscount1 + dscount2 + 7).ToString() + ":i" + (dscount1 + dscount2 + 7).ToString() + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["A" + (dscount1 + dscount2 + 7).ToString() + ":i" + (dscount1 + dscount2 + 7).ToString() + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["A" + (dscount1 + dscount2 + 7).ToString() + ":i" + (dscount1 + dscount2 + 7).ToString() + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells["A" + (dscount1 + dscount2 + 7).ToString() + ":i" + (dscount1 + dscount2 + 7).ToString() + ""].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A" + (dscount1 + dscount2 + 7).ToString() + ":i" + (dscount1 + dscount2 + 7).ToString() + ""].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

            for (int i = 1; i < alColumns.Length + 1; i++)
            {

                if (i == 1)
                    ws.Cells[dscount1 + dscount2 + 8, i].Value = "③ Annual other corrections";
                else if (i == 9)
                    ws.Cells[dscount1 + dscount2 + 8, i].Value = "";
                else
                    ws.Cells[dscount1 + dscount2 + 8, i].Value = 0;
                //ws.Cells[dscount1 + 3, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //ws.Cells[dscount1 + 3, i].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                ws.Cells[dscount1 + dscount2 + 8, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[dscount1 + dscount2 + 8, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[dscount1 + dscount2 + 8, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[dscount1 + dscount2 + 8, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }

            ws.Cells[(dscount1 + dscount2 + 9), 1].Value = "Annual Fuel Consumption";
            ws.Cells[(dscount1 + dscount2 + 9), 1].Style.Font.Bold = true;
            ws.Cells["A" + (dscount1 + dscount2 + 9).ToString() + ":i" + (dscount1 + dscount2 + 9).ToString() + ""].Merge = true;
            ws.Cells["A" + (dscount1 + dscount2 + 9).ToString() + ":i" + (dscount1 + dscount2 + 9).ToString() + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["A" + (dscount1 + dscount2 + 9).ToString() + ":i" + (dscount1 + dscount2 + 9).ToString() + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells["A" + (dscount1 + dscount2 + 9).ToString() + ":i" + (dscount1 + dscount2 + 9).ToString() + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells["A" + (dscount1 + dscount2 + 9).ToString() + ":i" + (dscount1 + dscount2 + 9).ToString() + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells["A" + (dscount1 + dscount2 + 9).ToString() + ":i" + (dscount1 + dscount2 + 9).ToString() + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells["A" + (dscount1 + dscount2 + 9).ToString() + ":i" + (dscount1 + dscount2 + 9).ToString() + ""].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A" + (dscount1 + dscount2 + 9).ToString() + ":i" + (dscount1 + dscount2 + 9).ToString() + ""].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

            for (int i = 1; i < alColumns.Length + 1; i++)
            {

                if (i == 1)
                    ws.Cells[dscount1 + dscount2 + 10, i].Value = "Annual Fuel Consumption ①+②+③";
                else if (i == 9)
                    ws.Cells[dscount1 + dscount2 + 10, i].Value = "";
                else
                    ws.Cells[dscount1 + dscount2 + 10, i].Value = 0;
                //ws.Cells[dscount1 + 3, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //ws.Cells[dscount1 + 3, i].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                ws.Cells[dscount1 + dscount2 + 10, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[dscount1 + dscount2 + 10, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[dscount1 + dscount2 + 10, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[dscount1 + dscount2 + 10, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }

            ws.Cells[(dscount1 + dscount2 + 10), 2].Value = mdomgototal + mdomgoDiff;
            ws.Cells[(dscount1 + dscount2 + 10), 3].Value = lfototal + lfoDiff;
            ws.Cells[(dscount1 + dscount2 + 10), 4].Value = hfototal + hfoDiff;


            pck.SaveAs(fiTemplate);

            return path;
        }
    }
    private void ClearFilter()
    {
        ViewState["PAGENUMBER"] = 1;
        ddlVessel.SelectedVessel = "";
        txtFrom.Text = "";
        txtTo.Text = "";
        //Rebind();
    }

    protected void ddlVessel_TextChangedEvent(object sender,EventArgs e)
    {
        RebindgvConsumption();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
            RebindgvConsumption();
    }

    //protected void gvDayToDayReport_RowCommand(object sender, GridCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;
    //        if (e.CommandName == "Page")
    //        {
    //            ViewState["PAGENUMBER"] = null;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void gvDayToDayReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDayToDayReport.CurrentPageIndex + 1;
    //    int iRowCount = 0;
    //    int iTotalPageCount = 0;

    //    string[] alColumns = { "FLDVESSELNAME", "FLDFROMDATE", "FLDTODATE", "FLDIMONUMBER", "FLDVESSELTYPE", "FLDREGISTEREDGT", "FLDREGISTEREDNT", "FLDDWTSUMMER", "FLDEEDI", "FLDICECLASS","FLDMEPOWEROUTPUT","FLDAEPOWEROUTPUT",
    //        "FLDDISTANCETRAVELLED","FLDHOURSUNDERWAY","FLDMDOMGOCONS","FLDLFOCONS","FLDHFOCONS","FLDLPGPROPANE","FLDLPGBUTANE","FLDLNG","FLDMETHANOL","FLDETHANOL","FLDOTHERS","FLDFUELCONSMEASUREMETHOD","FLDREMARKS"};
    //    string[] alCaptions = { "Vessel", "Start Date", "End Date", "IMO number", "Ship type", "Gross tonnage (GT)", "Net tonnage (NT)", "Deadweight tonnage (DWT)", "EEDI(gCO2/t.nm)", "Ice class", "Main propulsion power",
    //        "Auxiliary engine(s)", "Distance travelled (nm)", "Hours underway (h)", "Diesel/Gas Oil", "LFO", "HFO", "LPG (Propane)", "LPG (Butane)", "LNG", "Methanol", "Ethanol", "Other", "Method of measure cons", "Remarks" };

    //    string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
    //    int? sortdirection = null;
    //    if (ViewState["SORTDIRECTION"] != null)
    //        sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

    //    DataSet ds = PhoenixVesselPositionDayToDayReport.MPAVEBPSearch(General.GetNullableInteger(ddlVessel.SelectedVessel),
    //        sortexpression, sortdirection,
    //         int.Parse(ViewState["PAGENUMBER"].ToString()),
    //         gvDayToDayReport.PageSize,
    //         ref iRowCount,
    //         ref iTotalPageCount
    //         , General.GetNullableDateTime(txtFrom.Text)
    //         , General.GetNullableDateTime(txtTo.Text)
    //         , General.GetNullableInteger(ddlFleet.SelectedValue));

    //    General.SetPrintOptions("gvDayToDayReport", "MPA VEBP", alCaptions, alColumns, ds);

    //    gvDayToDayReport.DataSource = ds;
    //    gvDayToDayReport.VirtualItemCount = iRowCount;

    //    ViewState["ROWCOUNT"] = iRowCount;
    //    ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    //}
    //protected void Rebind()
    //{
    //    gvDayToDayReport.SelectedIndexes.Clear();
    //    gvDayToDayReport.EditIndexes.Clear();
    //    gvDayToDayReport.DataSource = null;
    //    gvDayToDayReport.Rebind();
    //}

    protected void RebindgvConsumption()
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
        criteria.Add("ddlFleet", ddlFleet.SelectedValue);
        criteria.Add("txtReportFrom", txtFrom.Text);
        criteria.Add("txtReportTo", txtTo.Text);

        Filter.CurrentIMODCSReportFilter = criteria;

        gvConsumption.SelectedIndexes.Clear();
        gvConsumption.EditIndexes.Clear();
        gvConsumption.DataSource = null;
        gvConsumption.Rebind();
    }
    protected void gvConsumption_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBERBDN"] = ViewState["PAGENUMBERBDN"] != null ? ViewState["PAGENUMBERBDN"] : gvConsumption.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = PhoenixVesselPositionDayToDayReport.BunkerDeliveryList(
              General.GetNullableDateTime(txtFrom.Text)
            , General.GetNullableDateTime(txtTo.Text)
            , General.GetNullableInteger(ddlVessel.SelectedVessel)
            , int.Parse(ViewState["PAGENUMBERBDN"].ToString())
            , gvConsumption.PageSize
            , ref iRowCount
            , ref iTotalPageCount
            , General.GetNullableInteger(ddlFleet.SelectedValue));

        gvConsumption.DataSource = ds;
        gvConsumption.VirtualItemCount = iRowCount;
    }

    protected void gvConsumption_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBERBDN"] = null;
        }
    }
    private string BDNHtml()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        //DataSet ds = PhoenixVesselPositionDayToDayReport.BunkerDeliveryList(
        //     General.GetNullableDateTime(txtFrom.Text)
        //   , General.GetNullableDateTime(txtTo.Text)
        //   , General.GetNullableInteger(ddlVessel.SelectedVessel)
        //   , 1
        //   , 10000
        //   , ref iRowCount
        //   , ref iTotalPageCount);

        StringBuilder DsHtmlcontent = new StringBuilder();
        //DsHtmlcontent.Append("<table border='1' cellpadding='3' cellspacing='0'>");

        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>Date of Operations</th>");
        //DsHtmlcontent.Append("<th colspan='7' bgcolor='#f1f1f1'>Fuel Oil Type/Mass(MT)</th>");
        //DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>Descriptions</th>");
        //DsHtmlcontent.Append("</tr>");

        //DsHtmlcontent.Append("<tr>");       
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>DO/GO</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>LFO</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>HFO</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>LPG(P)</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>LPG(B)</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>LNG</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Others(CF)</th>");
        //DsHtmlcontent.Append("</tr>");

        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<td colspan='9' bgcolor='#d3d3d3' align='center'>(1).BDN</td>");
        //DsHtmlcontent.Append("</tr>");

        //decimal mdomgo = 0;
        //decimal hfo = 0;
        //decimal lfo = 0;
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    DataTable dt = ds.Tables[0];
        //    for(int i=0;i< dt.Rows.Count;i++)
        //    {
        //        DataRow dr = dt.Rows[i];
        //        DsHtmlcontent.Append("<tr>");
        //        DsHtmlcontent.Append("<td style='text-align: center; mso-number-format:\\@' > " + Convert.ToDateTime(dr["FLDDATE"].ToString()).ToString("yyyy-MM-dd") + "</td>");
        //        DsHtmlcontent.Append("<td>" + dr["FLDMDOMGO"].ToString() + "</td>");
        //        DsHtmlcontent.Append("<td>" + dr["FLDLFO"].ToString() + "</td>");
        //        DsHtmlcontent.Append("<td>" + dr["FLDHFO"].ToString() + "</td>");
        //        DsHtmlcontent.Append("<td></td>");
        //        DsHtmlcontent.Append("<td></td>");
        //        DsHtmlcontent.Append("<td></td>");
        //        DsHtmlcontent.Append("<td></td>");
        //        DsHtmlcontent.Append("<td></td>");
        //        DsHtmlcontent.Append("</tr>");

        //        mdomgo = mdomgo + Convert.ToDecimal(General.GetNullableDecimal(dr["FLDMDOMGO"].ToString()) != null ? Convert.ToDecimal(dr["FLDMDOMGO"].ToString()) : 0);
        //        hfo = hfo + Convert.ToDecimal(General.GetNullableDecimal(dr["FLDHFO"].ToString()) != null ? Convert.ToDecimal(dr["FLDHFO"].ToString()) : 0);
        //        lfo = lfo + Convert.ToDecimal( General.GetNullableDecimal(dr["FLDLFO"].ToString()) != null ? Convert.ToDecimal(dr["FLDLFO"].ToString()) : 0);

        //    }
        //}
        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>(1).Annual Supply Amount</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>" + mdomgo.ToString() + "</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>" + lfo.ToString() + "</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>" + hfo.ToString() + "</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'></td>");
        //DsHtmlcontent.Append("</tr>");

        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<td colspan='9' bgcolor='#d3d3d3' align='center'>(2).Correction for the tank oil remainings</td>");
        //DsHtmlcontent.Append("</tr>");

        //DataSet dstank = PhoenixVesselPositionDayToDayReport.TankSoundingLogList(
        //     General.GetNullableDateTime(txtFrom.Text)
        //   , General.GetNullableDateTime(txtTo.Text)
        //   , General.GetNullableInteger(ddlVessel.SelectedVessel));

        //decimal mdomgofirst = 0;
        //decimal lfofirst = 0;
        //decimal hfofirst = 0;


        //decimal mdomgoLast = 0;
        //decimal lfolast = 0;
        //decimal hfolast = 0;

        //decimal mdomgoDiff = 0;
        //decimal lfoDiff = 0;
        //decimal hfoDiff = 0;

        //if (dstank.Tables[0].Rows.Count > 0)
        //{
        //    DataTable dt = dstank.Tables[0];
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        DataRow dr = dt.Rows[i];
        //        DsHtmlcontent.Append("<tr>");
        //        DsHtmlcontent.Append("<td style='text-align: center; mso-number-format:\\@;' >" + Convert.ToDateTime(dr["FLDDATE"].ToString()).ToString("yyyy-MM-dd") + "</td>");
        //        DsHtmlcontent.Append("<td>" + dr["FLDMDOMGO"].ToString() + "</td>");
        //        DsHtmlcontent.Append("<td>" + dr["FLDLFO"].ToString() + "</td>");
        //        DsHtmlcontent.Append("<td>" + dr["FLDHFO"].ToString() + "</td>");
        //        DsHtmlcontent.Append("<td></td>");
        //        DsHtmlcontent.Append("<td></td>");
        //        DsHtmlcontent.Append("<td></td>");
        //        DsHtmlcontent.Append("<td></td>");
        //        DsHtmlcontent.Append("<td></td>");

        //        if (i == 0)
        //        {
        //            mdomgofirst = Convert.ToDecimal(General.GetNullableDecimal(dr["FLDMDOMGO"].ToString()) != null ? Convert.ToDecimal(dr["FLDMDOMGO"].ToString()) : 0);
        //            lfofirst = Convert.ToDecimal(General.GetNullableDecimal(dr["FLDLFO"].ToString()) != null ? Convert.ToDecimal(dr["FLDLFO"].ToString()) : 0);
        //            hfofirst = Convert.ToDecimal(General.GetNullableDecimal(dr["FLDHFO"].ToString()) != null ? Convert.ToDecimal(dr["FLDHFO"].ToString()) : 0);

        //        }
        //        if (i == dt.Rows.Count-1)
        //        {
        //            mdomgoLast = Convert.ToDecimal(General.GetNullableDecimal(dr["FLDMDOMGO"].ToString()) != null ? Convert.ToDecimal(dr["FLDMDOMGO"].ToString()) : 0);
        //            lfolast = Convert.ToDecimal(General.GetNullableDecimal(dr["FLDLFO"].ToString()) != null ? Convert.ToDecimal(dr["FLDLFO"].ToString()) : 0);
        //            hfolast = Convert.ToDecimal(General.GetNullableDecimal(dr["FLDHFO"].ToString()) != null ? Convert.ToDecimal(dr["FLDHFO"].ToString()) : 0);

        //        }
        //    }
        //}

        //mdomgoDiff = mdomgofirst - mdomgoLast;
        //lfoDiff = lfofirst - lfolast;
        //hfoDiff = hfofirst - hfolast;

        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>(2).Correction for the <br> tank oil remainings</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>" + (mdomgoDiff).ToString() + "</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>" + (lfoDiff).ToString() + "</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>" + (hfoDiff).ToString() + "</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>The difference in the amount of the remaining tank oil <br> at the beginning/end of the data collection period.</td>");
        //DsHtmlcontent.Append("</tr>");

        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<td colspan='9' bgcolor='#d3d3d3' align='center'>(3).Other correction</td>");
        //DsHtmlcontent.Append("</tr>");

        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>(3).Annual other corrections</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'></td>");
        //DsHtmlcontent.Append("</tr>");

        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<td colspan='9' bgcolor='#d3d3d3' align='center'>Annual Fuel Consumption</td>");
        //DsHtmlcontent.Append("</tr>");

        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>Annual Fuel Consumption <br> ((1)+(2)+(3)) </ td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>" + (mdomgo + mdomgoDiff).ToString() + "</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>" + (lfo + lfoDiff).ToString() + "</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>" + (hfo + hfoDiff).ToString() + "</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>0</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'></td>");
        //DsHtmlcontent.Append("</tr>");

        //DsHtmlcontent.Append("</table>");

        DataSet dsVEBP = PhoenixVesselPositionDayToDayReport.MPAVEBPSearch(General.GetNullableInteger(ddlVessel.SelectedVessel),
            null, null,
             1,
            (int)ViewState["ROWCOUNT"],
             ref iRowCount,
             ref iTotalPageCount
             , General.GetNullableDateTime(txtFrom.Text)
             , General.GetNullableDateTime(txtTo.Text)
             , General.GetNullableInteger(ddlFleet.SelectedValue));

        //DsHtmlcontent.AppendLine("<br/><br/>");
        //DsHtmlcontent.Append("<table border='1'>");

        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>Date from</th>");
        //DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>Date to*</th>");
        //DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>Distance Travelled (nm)</th>");
        //DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>Hours Underway</th>");
        //DsHtmlcontent.Append("<th colspan='7' bgcolor='#f1f1f1'>Fuel Consumption (Metric tons)</th>");
        //DsHtmlcontent.Append("</tr>");

        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>DO/GO</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>LFO</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>HFO</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>LPG(P)</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>LPG(B)</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>LNG</th>");
        //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Others(CF)</th>");
        //DsHtmlcontent.Append("</tr>");

        //decimal totaldistance = 0;
        //decimal totalhours = 0;
        //decimal mdomgototal = 0;
        //decimal lfototal = 0;
        //decimal hfototal = 0;

        //if (dsVEBP.Tables[0].Rows.Count > 0)
        //{
        //    DataTable dt = new DataTable();
        //    dt = dsVEBP.Tables[0];
        //    for (int i = 0; i < dt.Rows.Count;i++)
        //    {
        //        DataRow dr = dt.Rows[i];
        //        DsHtmlcontent.Append("<tr>");
        //        DsHtmlcontent.Append("<td style='text-align: center; mso-number-format:\\@;'>" + Convert.ToDateTime(dr["FLDFROMDATE"].ToString()).ToString("yyyy-MM-dd") + "</td>");
        //        DsHtmlcontent.Append("<td style='text-align: center; mso-number-format:\\@;'>" + Convert.ToDateTime(dr["FLDTODATE"].ToString()).ToString("yyyy-MM-dd") + "</td>");
        //        DsHtmlcontent.Append("<td>" + dr["FLDDISTANCETRAVELLED"].ToString() + "</td>");
        //        DsHtmlcontent.Append("<td>" + dr["FLDHOURSUNDERWAY"].ToString() + "</td>");
        //        DsHtmlcontent.Append("<td>" + dr["FLDMDOMGOCONS"].ToString() + "</td>");
        //        DsHtmlcontent.Append("<td>" + dr["FLDLFOCONS"].ToString() + "</td>");
        //        DsHtmlcontent.Append("<td>" + dr["FLDHFOCONS"].ToString() + "</td>");
        //        DsHtmlcontent.Append("<td></td>");
        //        DsHtmlcontent.Append("<td></td>");
        //        DsHtmlcontent.Append("<td></td>");
        //        DsHtmlcontent.Append("<td></td>");
        //        DsHtmlcontent.Append("</tr>");

        //        totaldistance = totaldistance + Convert.ToDecimal(General.GetNullableDecimal(dr["FLDDISTANCETRAVELLED"].ToString()) != null ? Convert.ToDecimal(dr["FLDDISTANCETRAVELLED"].ToString()) : 0);
        //        totalhours = totalhours + Convert.ToDecimal(General.GetNullableDecimal(dr["FLDHOURSUNDERWAY"].ToString()) != null ? Convert.ToDecimal(dr["FLDHOURSUNDERWAY"].ToString()) : 0);
        //        mdomgototal = mdomgototal + Convert.ToDecimal(General.GetNullableDecimal(dr["FLDMDOMGOCONS"].ToString()) != null ? Convert.ToDecimal(dr["FLDMDOMGOCONS"].ToString()) : 0);
        //        lfototal = lfototal + Convert.ToDecimal(General.GetNullableDecimal(dr["FLDLFOCONS"].ToString()) != null ? Convert.ToDecimal(dr["FLDLFOCONS"].ToString()) : 0);
        //        hfototal = hfototal + Convert.ToDecimal(General.GetNullableDecimal(dr["FLDHFOCONS"].ToString()) != null ? Convert.ToDecimal(dr["FLDHFOCONS"].ToString()) : 0);
        //    }
        //}
        //DsHtmlcontent.Append("<tr>");
        //DsHtmlcontent.Append("<td colspan='2' bgcolor='#ededed''>Annual Total</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>" + totaldistance + "</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>" + totalhours + "</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>" + mdomgototal + "</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>" + lfototal + "</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'>" + hfototal + "</td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'></td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'></td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'></td>");
        //DsHtmlcontent.Append("<td bgcolor='#ededed'></td>");
        //DsHtmlcontent.Append("</tr>");

        //DsHtmlcontent.Append("</table>");



        //DsHtmlcontent.AppendLine("<br/><br/>");

        DsHtmlcontent.Append("<table border='1'>");

        DsHtmlcontent.Append("<tr>");
        DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>Method used to muasure fuel oil consumption</th>");
        DsHtmlcontent.Append("<th colspan='9' bgcolor='#f1f1f1'>Fuel oil consumption (t)</th>");
        DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>Hours Underway</th>");
        DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>Distance Travelled (nm)</th>");
        DsHtmlcontent.Append("<th colspan='2' bgcolor='#f1f1f1'>Power output (rated power) (kW)8</th>");
        DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>Ice Class7 (if applicable)</th>");
        DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>EEDI (if applicable)6 (gCO2/t.nm)</th>");
        DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>DWT5</th>");
        DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>NT4</th>");
        DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>Gross tonnage3</th>");
        DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>Ship type2</th>");
        DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>IMO number1</th>");
        DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>Date to (dd/mm/yyyy)</th>");
        DsHtmlcontent.Append("<th rowspan='2' bgcolor='#f1f1f1'>Date from (dd/mm/yyyy)</th>");
        DsHtmlcontent.Append("</tr>");

        DsHtmlcontent.Append("<tr>");

        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Others(Cf)</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Ethanol (Cf: 1.913)</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Methanol (Cf: 1.375)</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>LNG (Cf: 2.750)</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>LPG(Butane) (Cf: 3.030) </ th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>LPG(Propane)</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>HFO (Cf : 3.114)</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>LFO (Cf: 3.151)</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Diesel/Gas Oil (Cf : 3.206)</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Auxiliary Engine(s)</th>");
        DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Main Propulsion Power</th>");
        DsHtmlcontent.Append("</tr>");

        if (dsVEBP.Tables[0].Rows.Count > 0)
        {
            DataTable dt = new DataTable();
            dt = dsVEBP.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                DsHtmlcontent.Append("<tr>");
                DsHtmlcontent.Append("<td>"+ dr["FLDFUELCONSMEASUREMETHOD"].ToString() + "</td>");
                DsHtmlcontent.Append("<td></td>");
                DsHtmlcontent.Append("<td></td>");
                DsHtmlcontent.Append("<td></td>");
                DsHtmlcontent.Append("<td></td>");
                DsHtmlcontent.Append("<td></td>");
                DsHtmlcontent.Append("<td></td>");
                DsHtmlcontent.Append("<td>" + dr["FLDHFOCONS"].ToString() + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDLFOCONS"].ToString() + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDMDOMGOCONS"].ToString() + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDHOURSUNDERWAY"].ToString() + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDDISTANCETRAVELLED"].ToString() + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDAEPOWEROUTPUT"].ToString() + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDMEPOWEROUTPUT"].ToString() + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDICECLASS"].ToString() + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDEEDI"].ToString() + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDDWTSUMMER"].ToString() + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDREGISTEREDNT"].ToString() + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDREGISTEREDGT"].ToString() + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDVESSELTYPE"].ToString() + "</td>");
                DsHtmlcontent.Append("<td>" + dr["FLDIMONUMBER"].ToString() + "</td>");
                DsHtmlcontent.Append("<td style='text-align: center; mso-number-format:\\@;'>" + Convert.ToDateTime(dr["FLDTODATE"].ToString()).ToString("dd/MM/yyyy") + "</td>");
                DsHtmlcontent.Append("<td style='text-align: center; mso-number-format:\\@;'>" + Convert.ToDateTime(dr["FLDFROMDATE"].ToString()).ToString("dd/MM/yyyy") + "</td>");                
                DsHtmlcontent.Append("</tr>");
            }
        }

        DsHtmlcontent.Append("</table>");

        return DsHtmlcontent.ToString();
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

        if (CommandName.ToUpper().Equals("DAILYREPORT"))
        {
            Response.Redirect("../VesselPosition/VesselPositionIMODCSReport.aspx");
        }
    }
}
