using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.Text;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Web;
using System.Xml;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using OfficeOpenXml;

public partial class VesselPositionEUVoyageReportingSummary : PhoenixBasePage
{
    DataSet ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("At Sea", "ATSEA");
            toolbarmain.AddButton("In Port", "ARBERTH");
            toolbarmain.AddButton("History", "HISTORY");
            MenuDepartureReport.AccessRights = this.ViewState;
            MenuDepartureReport.MenuList = toolbarmain.Show();
            MenuDepartureReport.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionEUVoyageReportingSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvConsumption')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionEUVoyageReportingSummary.aspx", "Search", "<i class=\"fas fa-search\"></i>", "Search");
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionEUVoyageReportingSummary.aspx", "Clear-Filter", "<i class=\"fas fa-eraser\"></i>", "Clear");
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionEUVoyageReportingSummary.aspx", "Summary Report", "<i class=\"fas fa-file-pdf\"></i>", "PDFAll");
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionEUVoyageReportingSummary.aspx", "Summary Report Excel", "<i class=\"fas fa-file-excel\"></i>", "EXPORTEXCEL");
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionEUVoyageReportingSummary.aspx", "Generate Report", "<i class=\"fas fa-copy\"></i>", "GRREPORT");
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionEUVoyageReportingSummary.aspx", "Download XML", "<i class=\"fas fa-download\"></i>", "DOWNLOAD");
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionEUVoyageReportingSummary.aspx", "Summary Report ABS Excel", "<i class=\"fas fa-file-excel\"></i>", "ABS");
            MenuCrewCourseList.AccessRights = this.ViewState;
            MenuCrewCourseList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
                    {
                        UcVessel.Enabled = false;
                        UcVessel.Enabled = false;
                    }
                }

                txtCommencedTo.Text = DateTime.Now.ToShortDateString();
                txtCompletedTo.Text = DateTime.Now.ToShortDateString();

                if (Filter.CurrentEUVoyageListFilter == null)
                {
                    SetFilter();
                }
                else
                {
                    NameValueCollection nvc = Filter.CurrentEUVoyageListFilter;

                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? ((nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucPortMulti.SelectedValue = (nvc.Get("ddlPort") == null) ? "" : nvc.Get("ddlPort").ToString();
                    ucPortMulti.Text = (nvc.Get("ddlPortName") == null) ? "" : nvc.Get("ddlPortName").ToString();
                    txtCommencedFrom.Text = (nvc.Get("txtCommenceFrom") == null) ? "" : nvc.Get("txtCommenceFrom").ToString();
                    txtCommencedTo.Text = (nvc.Get("txtCommenceTo") == null) ? "" : nvc.Get("txtCommenceTo").ToString();
                    txtCompletedFrom.Text = (nvc.Get("txtCompletedfrom") == null) ? "" : nvc.Get("txtCompletedfrom").ToString();
                    txtCompletedTo.Text = (nvc.Get("txtCompletedTo") == null) ? "" : nvc.Get("txtCompletedTo").ToString();
                    ucFleet.SelectedFleet = ((nvc.Get("ddlFleet") == null) || General.GetNullableInteger(nvc.Get("ddlFleet")) == null) ? "" : nvc.Get("ddlFleet").ToString();
                    ucOwner.SelectedAddress = (nvc.Get("ddlOwner") == null) ? "" : nvc.Get("ddlOwner").ToString();
                    chkShowNonEU.Checked = ((nvc.Get("chkShowNonEU")== null) || (nvc.Get("chkShowNonEU")=="0"))?false:true;

                }
                gvConsumption.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetFilter()
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();

        criteria.Add("ddlVessel", PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? UcVessel.SelectedVessel : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
        criteria.Add("ddlPort", ucPortMulti.SelectedValue);
        criteria.Add("ddlPortName", ucPortMulti.Text);
        criteria.Add("txtCommenceFrom", txtCommencedFrom.Text);
        criteria.Add("txtCommenceTo", txtCommencedTo.Text);
        criteria.Add("txtCompletedfrom", txtCompletedFrom.Text);
        criteria.Add("txtCompletedTo", txtCompletedTo.Text);
        criteria.Add("ddlFleet", ucFleet.SelectedFleet);
        criteria.Add("ddlOwner", ucOwner.SelectedAddress);
        criteria.Add("chkShowNonEU", chkShowNonEU.Checked ? "1" : "0");
        Filter.CurrentEUVoyageListFilter = criteria;
    }
    protected void MenuDepartureReport_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ARBERTH"))
        {
            Response.Redirect("../VesselPosition/VesselPositionEUVoyageReportingSummaryInport.aspx");
        }
        if (CommandName.ToUpper().Equals("HISTORY"))
        {
            Response.Redirect("../VesselPosition/VesselPositionReportingDodumentsHistoryMRV.aspx");
        }
    }
    protected void CrewCourseList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            SetFilter();
            Rebind();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtCommencedFrom.Text = "";
            txtCompletedFrom.Text = "";
            ucPortMulti.Text = "";
            ucPortMulti.SelectedValue = "";
            txtCommencedTo.Text = DateTime.Now.ToShortDateString();
            txtCompletedTo.Text = DateTime.Now.ToShortDateString();
            ucFleet.SelectedFleet = "";
            ucOwner.SelectedAddress = "";

            SetFilter();

            Rebind();
        }
        if (CommandName.ToUpper().Equals("GRREPORT"))
        {
            NameValueCollection nvc = Filter.CurrentEUVoyageListFilter;
            PhoenixVesselPositionEUMRVSummaryReport.GenerateEUReportSummary(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? ((nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));
            Rebind();
        }

        else if (CommandName.ToUpper().Equals("PDFALL"))
        {
            NameValueCollection nvc = Filter.CurrentEUVoyageListFilter;
            if ((nvc.Get("ddlVessel") != null && General.GetNullableInteger(nvc.Get("ddlVessel")) > 0) || PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                ConvertToPdf(PrepareHtmlDocAnnualSummary(null, General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? ((nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())), "EuVoyages");
        }
        else if (CommandName.ToUpper().Equals("EXPORTEXCEL"))
        {
            NameValueCollection nvc = Filter.CurrentEUVoyageListFilter;
            if ((nvc.Get("ddlVessel") != null && General.GetNullableInteger(nvc.Get("ddlVessel")) > 0) || PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                ConvertToExcel(PrepareHtmlDocAnnualSummary(null, General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? ((nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())), "EuVoyages");
        }
        else if (CommandName.ToUpper().Equals("DOWNLOAD"))
        {
            if (General.GetNullableInteger(UcVessel.SelectedVessel) != null && General.GetNullableInteger(UcVessel.SelectedVessel) > 0)
            {
                string Filepath = "";
                NameValueCollection nvc = Filter.CurrentEUVoyageListFilter;
                Filepath = ConvertAnnualSummaryXML(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? ((nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceFrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceTo"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedfrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedTo"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlPort"] : string.Empty)
                                                                                     , UcVessel.SelectedVesselName.ToString()
                                                                                     , General.GetNullableInteger(nvc != null ? nvc["ddlOwner"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty));
                Response.Clear();
                Response.ContentType = "text/xml";
                Response.AppendHeader("Content-Disposition", String.Format("attachment;filename={0}", UcVessel.SelectedVesselName.ToString() + "_emissions.xml"));
                Response.TransmitFile(Filepath);
                Response.End();
            }

        }
        else if(CommandName.ToUpper().Equals("ABS"))
        {
            ABSExport();
        }

    }
    private void ABSExport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        NameValueCollection nvc = Filter.CurrentEUVoyageListFilter;

        int? vesselid = General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? ((nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());

        DataSet dspartA = PhoenixRegistersEUMRVEmisionSource.RegistersEUMRVShipDetails(vesselid);

        DataSet dsatsea = PhoenixVesselPositionEUMRVSummaryReport.EUReportingSummarySearch(vesselid
                                                                            , null
                                                                             , null
                                                                             , (int)ViewState["PAGENUMBER"]
                                                                             , 100000
                                                                             , ref iRowCount
                                                                             , ref iTotalPageCount
                                                                             , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceFrom"] : string.Empty)
                                                                             , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceTo"] : string.Empty)
                                                                             , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedfrom"] : string.Empty)
                                                                             , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedTo"] : string.Empty)
                                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlPort"] : string.Empty)
                                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlOwner"] : string.Empty)
                                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
                                                                             , General.GetNullableInteger(nvc != null ? nvc["chkShowNonEU"] : string.Empty)
                                                                             , 0
                                                                             );

        DataSet dsinport = PhoenixVesselPositionEUMRVSummaryReport.EUReportingSummarySearch(vesselid
                                                                            , null
                                                                             , null
                                                                             , (int)ViewState["PAGENUMBER"]
                                                                             , 100000
                                                                             , ref iRowCount
                                                                             , ref iTotalPageCount
                                                                             , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceFrom"] : string.Empty)
                                                                             , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceTo"] : string.Empty)
                                                                             , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedfrom"] : string.Empty)
                                                                             , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedTo"] : string.Empty)
                                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlPort"] : string.Empty)
                                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlOwner"] : string.Empty)
                                                                             , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
                                                                             , General.GetNullableInteger(nvc != null ? nvc["chkShowNonEU"] : string.Empty)
                                                                             , 1
                                                                             );

        DataTable dt = dsatsea.Tables[0];
        DataTable DTBDN = dsinport.Tables[0];
        if (dt.Rows.Count > 0)
        {
            string path = HttpContext.Current.Server.MapPath("~\\Template\\VesselPosition" + @"\\EUMRV_Report.xlsx");

            var file = new FileInfo(path);

            string vesselname = dspartA.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();

            if (file.Exists)
            {
                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    var wb = excelPackage.Workbook.Worksheets["EU MRV Voyages"];
                    var wbBDN = excelPackage.Workbook.Worksheets["EU MRV Port Stays"];
                    //var wb = excelPackage.Workbook;



                    if (dt.Rows.Count > 0)
                    {
                        int iRow = 2;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            wb.Cells[iRow, 1].Value = dr["FLDFROMPORTCODE"].ToString();
                            wb.Cells[iRow, 2].Value = dr["FLDTOPORTCODE"].ToString();
                            wb.Cells[iRow, 3].Value = dr["FLDCOMMENCEDDATE"].ToString();
                            wb.Cells[iRow, 4].Value = dr["FLDCOMPLETEDDATE"].ToString();
                            wb.Cells[iRow, 5].Value = dr["FLDTIMEATSEA"].ToString();
                            wb.Cells[iRow, 6].Value = dr["FLDDISTANCE"].ToString();
                            wb.Cells[iRow, 7].Value = dr["FLDCARGOQTYONARRIVAL"].ToString();
                            wb.Cells[iRow, 11].Value = dr["FLDHFOCONSATSEA"].ToString();
                            wb.Cells[iRow, 12].Value = dr["FLDLFOCONSATSEA"].ToString();
                            wb.Cells[iRow, 13].Value = dr["FLDMDOCONSATSEA"].ToString();
                            iRow = iRow + 1;
                        }
                    }
                    if (DTBDN.Rows.Count > 0)
                    {
                        int iRow = 2;

                        for (int i = 0; i < DTBDN.Rows.Count; i++)
                        {
                            DataRow dr = DTBDN.Rows[i];
                            wbBDN.Cells[iRow, 1].Value = dr["FLDTOPORTCODE"].ToString();
                            wbBDN.Cells[iRow, 2].Value = dr["FLDCOMPLETED"].ToString();
                            wbBDN.Cells[iRow, 3].Value = dr["FLDCURRENTSBEDATE"].ToString();
                            wbBDN.Cells[iRow, 4].Value = dr["FLDHFOCONSINPORT"].ToString();
                            wbBDN.Cells[iRow, 6].Value = dr["FLDLFOCONSINPORT"].ToString();
                            wbBDN.Cells[iRow, 7].Value = dr["FLDMDOCONSINPORT"].ToString();
                            iRow = iRow + 1;
                        }
                    }
                    //excelPackage.Workbook.Properties.Title = "Attempts";
                    this.Response.ClearHeaders();
                    this.Response.ClearContent();
                    this.Response.Clear();
                    this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    this.Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}", "EUMRV_Report_" + vesselname + ".xlsx"));
                    this.Response.BinaryWrite(excelPackage.GetAsByteArray());
                    this.Response.Flush();
                    this.Response.Close();
                }
            }
        }
    }
    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDVOYAGENO", "FLDFROM", "FLDTO", "FLDBALLASTRLADEN", "FLDCOMMENCED", "FLDCOMPLETED", "FLDREPORTTYPE", "FLDDISTANCE", "FLDTIMEATSEA", "FLDANCHORAGETIME", "FLDCARGOQTYONARRIVAL", "FLDHFOCONSATSEA", "FLDMDOCONSATSEA", "FLDTRANSPORTWORK", "FLDCO2EMISSIONATSEA", "FLDCO2EMISSIONPERDIST", "FLDCO2EMISSIONPERTRANSPORTWORK" };
        string[] alCaptions = { "Vessel", "Voy No.", "From", "To", "Ballast/Laden", "Commenced", "Completed", "Report Type", "Distance (nm)", "Time At Sea","Anchorage Time", "Cargo Qty (MT)", "HFO Cons (MT)", "MDO/MGO (MT)", "Transport Work (T-nm)", "Agg CO2 Emitted (T-CO2)", "Per Dist (Kg CO2/nm)", "Per Transport Work (g CO2/T-nm)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentEUVoyageListFilter;

        int? vesselid = General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? ((nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
        DataSet ds = PhoenixVesselPositionEUMRVSummaryReport.EUReportingSummarySearch(vesselid
                                                                                , sortexpression
                                                                                 , sortdirection
                                                                                 , 1
                                                                                 , (int)ViewState["ROWCOUNT"] == 0? 1: (int)ViewState["ROWCOUNT"]
                                                                                 , ref iRowCount
                                                                                 , ref iTotalPageCount
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceFrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceTo"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedfrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedTo"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlPort"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlOwner"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["chkShowNonEU"] : string.Empty)
                                                                                 , 0
                                                                                 );


        string style = @"<style> TD { mso-number-format:\@; } </style> ";
        Response.AddHeader("Content-Disposition", "attachment; filename=\"EUVoyages.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write(style);
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>EU Voyages</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");

        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void gvConsumption_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                RadLabel lblPerTransportWorkItem = (RadLabel)e.Item.FindControl("lblPerTransportWorkItem");

                RadCheckBox chkEUVoyYN = (RadCheckBox)e.Item.FindControl("chkEUVoyYN");
                if (chkEUVoyYN != null)
                {
                    chkEUVoyYN.Checked = drv["FLDEUVOYAGEYN"].ToString().Equals("1") ? true : false;

                    if (drv["FLDEUVOYAGEYN"].ToString() == "1")
                    {
                        e.Item.Font.Bold = true;
                    }
                }

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //protected void gvConsumption_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
    //        TableCell Detail = new TableCell();
    //        TableCell Avgco2emision = new TableCell();
    //        TableCell Action = new TableCell();

    //        Detail.ColumnSpan = 15;
    //        Avgco2emision.ColumnSpan = 2;
    //        Action.ColumnSpan = 3;

    //        Detail.Text = "";
    //        Avgco2emision.Text = "Average CO₂ Emissions";
    //        Action.Text = "";

    //        Detail.Attributes.Add("style", "text-align:center");
    //        Avgco2emision.Attributes.Add("style", "text-align:center");
    //        Action.Attributes.Add("style", "text-align:center");

    //        gv.Cells.Add(Detail);
    //        gv.Cells.Add(Avgco2emision);
    //        gv.Cells.Add(Action);

    //        gvConsumption.Controls[0].Controls.AddAt(0, gv);
    //    }
    //}
    protected void UcVessel_OnTextChangedEvent(object sender, EventArgs e)
    {
        SetFilter();
        Rebind();

    }
    protected void ucFleet_OnTextChangedEvent(object sender, EventArgs e)
    {
        SetFilter();
        Rebind();
    }
    protected void ucOwner_OnTextChangedEvent(object sender, EventArgs e)
    {
        SetFilter();
        Rebind();

    }
    protected void gvConsumption_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if(e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionEUMRVSummaryReport.DeleteEUReportsUMMARY(General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblVesselid")).Text),
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblID")).Text)
                    );
                Rebind();
            }
            else if (e.CommandName == "Page")
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
    private string PrepareHtmlDocAnnualSummary(Guid? arrivalsummaryid,int? vesselid)
    {
        StringBuilder DsHtmlcontent = new StringBuilder();
        try
        {
            
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet dspartA = PhoenixRegistersEUMRVEmisionSource.RegistersEUMRVShipDetails(vesselid);
            DataSet dscompany = PhoenixVesselPositionEUMRV.EUMRVBasicData(vesselid);
            DataSet dsVerfier = PhoenixRegistersVPRSVerifier.GetVerifier(vesselid);
            DataTable dtpartc1 =  PhoenixRegistersEUMRVDeterminationofdestination.EUMRVMonitoringfuelconsumption(vesselid
                                                                                , null, null, 1, 200, ref iRowCount, ref iTotalPageCount);
            DataTable dtpartc2 = PhoenixRegistersEUMRVDeterminationofdestination.EUMRVMonitoringSearch(
                                                                                null, null, 1, 200, ref iRowCount, ref iTotalPageCount, vesselid);

            
            // int? vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0 ? General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) : General.GetNullableInteger(UcVessel.SelectedVessel.ToString());
            NameValueCollection nvc = Filter.CurrentEUVoyageListFilter;
            DataSet dsatsea = PhoenixVesselPositionEUMRVSummaryReport.EUReportingSummarySearch(vesselid
                                                                                , null
                                                                                 , null
                                                                                 , (int)ViewState["PAGENUMBER"]
                                                                                 , 100000
                                                                                 , ref iRowCount
                                                                                 , ref iTotalPageCount
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceFrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceTo"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedfrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedTo"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlPort"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlOwner"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["chkShowNonEU"] : string.Empty)
                                                                                 , 0
                                                                                 );

            DataSet dsinport = PhoenixVesselPositionEUMRVSummaryReport.EUReportingSummarySearch(vesselid
                                                                                , null
                                                                                 , null
                                                                                 , (int)ViewState["PAGENUMBER"]
                                                                                 , 100000
                                                                                 , ref iRowCount
                                                                                 , ref iTotalPageCount
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceFrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceTo"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedfrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedTo"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlPort"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlOwner"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["chkShowNonEU"] : string.Empty)
                                                                                 , 1
                                                                                 );
            DataSet dssummary = PhoenixVesselPositionEUMRVSummaryReport.EUReportingSummaryConsolidated(vesselid
                                                                                , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceFrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceTo"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedfrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedTo"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlPort"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlOwner"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["chkShowNonEU"] : string.Empty));


            string fromdate = "", todate = "";
            if (arrivalsummaryid == null)
            {
                fromdate = General.GetNullableDateTime(txtCommencedFrom.Text) != null ? txtCommencedFrom.Text : (General.GetNullableDateTime(txtCompletedFrom.Text)) != null ? txtCompletedFrom.Text : "";
                todate = General.GetNullableDateTime(txtCompletedTo.Text) != null ? txtCompletedTo.Text.ToString() : (General.GetNullableDateTime(txtCommencedTo.Text)) != null ? txtCommencedTo.Text.ToString() : "";
            }
            else
            {
                fromdate = General.GetNullableDateTime(dsatsea.Tables[0].Rows[0]["FLDCOMMENCED"].ToString()) != null ? dsatsea.Tables[0].Rows[0]["FLDCOMMENCED"].ToString() : (General.GetNullableDateTime(dsatsea.Tables[0].Rows[0]["FLDCOMPLETED"].ToString())) != null ? dsatsea.Tables[0].Rows[0]["FLDCOMPLETED"].ToString() : "";
                todate = General.GetNullableDateTime(dsatsea.Tables[0].Rows[0]["FLDCOMPLETED"].ToString()) != null ? dsatsea.Tables[0].Rows[0]["FLDCOMPLETED"].ToString() : (General.GetNullableDateTime(dsatsea.Tables[0].Rows[0]["FLDCOMMENCED"].ToString())) != null ? dsatsea.Tables[0].Rows[0]["FLDCOMMENCED"].ToString() : "";
            }

            int rowcount = dsatsea.Tables[0].Rows.Count;
            
            DataTable dtBDN = PhoenixVesselPositionEUMRVSummaryReport.BunkeringList(vesselid
                ,rowcount>0? General.GetNullableDateTime(dsatsea.Tables[0].Rows[rowcount-1]["FLDCOMMENCED"].ToString()):DateTime.Now
                ,General.GetNullableDateTime(dsatsea.Tables[0].Rows[0]["FLDCOMPLETED"].ToString()));

            DataTable dtSTS = PhoenixVesselPositionEUMRVSummaryReport.CargoSTSList(vesselid
                , rowcount > 0 ? General.GetNullableDateTime(dsatsea.Tables[0].Rows[rowcount - 1]["FLDCOMMENCED"].ToString()) : DateTime.Now
                , General.GetNullableDateTime(dsatsea.Tables[0].Rows[0]["FLDCOMPLETED"].ToString()));

            string companyaddress = "";
            string contactperson = "";
            string companyname = "";

            if (dscompany.Tables[1].Rows.Count > 0)
            {
                companyaddress = dscompany.Tables[1].Rows[0]["FLDADDRESS1"].ToString();
                companyaddress = companyaddress != null ? companyaddress + "," : "";
                companyaddress = companyaddress + dscompany.Tables[1].Rows[0]["FLDCITYNAME"].ToString();
                companyaddress = companyaddress != null ? companyaddress + "," : "";
                companyaddress = companyaddress + dscompany.Tables[1].Rows[0]["FLDSTATENAME"].ToString();
                companyaddress = companyaddress != null ? companyaddress + "," : "";
                companyaddress = companyaddress + dscompany.Tables[1].Rows[0]["FLDCONTRY"].ToString();
                //companyaddress = companyaddress != null ? companyaddress + "-" : "";
                //companyaddress = companyaddress + dscompany.Tables[1].Rows[0]["FLDPOSTALCODE"].ToString();
                companyaddress = companyaddress.Replace(",,", ",");

                contactperson = dscompany.Tables[1].Rows[0]["FLDINCHARGE"].ToString();
                contactperson = contactperson != null ? contactperson + "," : "";
                contactperson = contactperson + dscompany.Tables[1].Rows[0]["FLDEMAIL1"].ToString();
                contactperson = contactperson != null ? contactperson + "," : "";
                contactperson = contactperson + dscompany.Tables[1].Rows[0]["FLDPHONE2"].ToString();
                contactperson = contactperson.Replace(",,", ",");

                companyname = dscompany.Tables[1].Rows[0]["FLDCOMPANYNAME"].ToString();
            }

            DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
            DsHtmlcontent.Append("<font size='9px' face='Helvetica'>");

            DsHtmlcontent.Append("<br />");
            DsHtmlcontent.Append("<table ID='tblheader' border ='1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
            DsHtmlcontent.Append("<font color='white' size='14px'><tr><td colspan='2' align='left'><b>Part A Data identifying the ship and the company</b></td></tr></font>");
            DsHtmlcontent.Append("</table>");
            DsHtmlcontent.Append("<br />");

            DsHtmlcontent.Append("<table ID='tbl1' border='1' cellpadding='7' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Vessel Name</b></td><td>" + dspartA.Tables[0].Rows[0]["FLDVESSELNAME"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>IMO Identification Number</b></td><td>" + dspartA.Tables[0].Rows[0]["FLDIMONUMBER"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Port of Registry</b></td><td>" + dspartA.Tables[0].Rows[0]["FLDPORTREGISTERED"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Type of the Ship</b></td><td>" + dspartA.Tables[0].Rows[0]["FLDVESSELTYPE"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Ice Class</b></td><td>" + dspartA.Tables[0].Rows[0]["FLDICECLASS"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>EEDI or EIV</b></td><td>" + dspartA.Tables[0].Rows[0]["FLDTECHNICALEFFICIENCY"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Name of the Ship Owner</b></td><td>" + dspartA.Tables[0].Rows[0]["FLDOWNER"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Address of the Ship Owner</b></td><td>" + dspartA.Tables[0].Rows[0]["FLDOWNERADDRESS"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Name of the Company</b></td><td>" + companyname + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Address of the Company</b></td><td>" + companyaddress + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Contact Person</b></td><td>" + contactperson + "</td></tr>");

            //DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>From</b></td><td><b>" + fromdate + "</b></td>");
            //DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>To</b></td><td><b>" + todate + "</b></td></tr>");
            DsHtmlcontent.Append("</table>");

            DsHtmlcontent.Append("<br />");
            DsHtmlcontent.Append("<table ID='tblheaderVerifier' border ='1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
            DsHtmlcontent.Append("<font color='white' size='14px' ><tr><td colspan='2' align='left'><b>Part B Verification</b></td></tr></font>");
            DsHtmlcontent.Append("</table>");
            DsHtmlcontent.Append("<br />");


            string verifier = "";
            string verifieraddress = "";
            string verifieracc = "";

            if (dsVerfier.Tables[0].Rows.Count > 0)
            {
                 verifier = dsVerfier.Tables[0].Rows[0]["FLDVERIFIERNAME"].ToString() ;
                 verifieraddress =  dsVerfier.Tables[0].Rows[0]["FLDADDRESS"].ToString();
                 verifieracc = dsVerfier.Tables[0].Rows[0]["FLDACCREDITATIONNO"].ToString();
            }

            DsHtmlcontent.Append("<table ID='tblVerifier' border='1' cellpadding='7' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Verifier</b></td><td>" + verifier + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Address</b></td><td>" + verifieraddress + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Accreditation Number</b></td><td>" + verifieracc + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Verifier's statement</b></td><td></td></tr>");
            DsHtmlcontent.Append("</table>");

            DsHtmlcontent.Append("<br />");
            DsHtmlcontent.Append("<table ID='tblheaderPartC' border ='1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
            DsHtmlcontent.Append("<font color='white' size='14px'><tr><td colspan='2' align='left'><b>Part C   Information on the monitoring method used and the related level of uncertainty</b></td></tr></font>");
            DsHtmlcontent.Append("</table>");
            DsHtmlcontent.Append("<br />");

            if(dtpartc1.Rows.Count>0)
            {
                DsHtmlcontent.Append("<table ID='tblVerifier' border='1' cellpadding='7' cellspacing='0' >");
                DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Emission Source</b></td><td bgcolor='#f1f1f1'><b>Methods for fuel consumption</b></td></tr>");
                foreach (DataRow dr in dtpartc1.Rows)
                {
                    DsHtmlcontent.Append("<tr><td>"+ dr["FLDEMISSIONSOURCENAME"].ToString() + "</td><td>"+ dr["FLDMONITORINGMETHODNAME"].ToString() + "</td></tr>");
                }
                DsHtmlcontent.Append("</table>");
            }


            DsHtmlcontent.Append("<br />");
            DsHtmlcontent.Append("<table ID='tblheaderPartC2' border ='1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
            DsHtmlcontent.Append("<font color='white' size='14px'><tr><td colspan='2' align='left'><b>Level of uncertainty associated with fuel monitoring</b></td></tr></font>");
            DsHtmlcontent.Append("</table>");
            DsHtmlcontent.Append("<br />");
            if (dtpartc2.Rows.Count > 0)
            {
                DsHtmlcontent.Append("<table ID='tblVerifier' border='1' cellpadding='7' cellspacing='0' >");
                DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Monitoring Method</b></td><td bgcolor='#f1f1f1'><b>Approach Used</b></td><td bgcolor='#f1f1f1'><b>Value</b></td></tr>");
                foreach (DataRow dr in dtpartc2.Rows)
                {
                    DsHtmlcontent.Append("<tr><td>" + dr["FLDMONITORINGMETHODNAME"].ToString() + "</td><td>" + dr["FLDAPPROACHUSEDNAME"].ToString() + "</td><td>" + dr["FLDVALUE"].ToString() + " %</td></tr>");
                }
                DsHtmlcontent.Append("</table>");
            }

            if (dsatsea.Tables[0].Rows.Count > 0)
            {
                DataTable t1 = new DataTable();
                t1 = dsatsea.Tables[0];


                DsHtmlcontent.Append("<br />");
                DsHtmlcontent.Append("<table ID='tbl1' border ='1' bgcolor='#93a3b9' opacity ='1' cellpadding='7' cellspacing='0'>");
                DsHtmlcontent.Append("<font color='white' size='14px'><tr><td colspan='18' align='center'><b>At Sea</b></td></tr></font>");
                DsHtmlcontent.Append("</table>");

                DsHtmlcontent.Append("<table ID=\"tbl11\" border ='1'  opacity='0.5' cellpadding=\"7\" cellspacing='0' style='border:1px solid'>");
                //DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Vessel</th>");
                DsHtmlcontent.Append("<tr style=\"font-weight:bold;\"><td colspan=\"12\" bgcolor='#f1f1f1'></td>");
                DsHtmlcontent.Append("<td colspan=\"3\" bgcolor='#f1f1f1' align=\"center\">Fuel Consumption (MT)</td>");
                DsHtmlcontent.Append("<td colspan=\"3\" bgcolor='#f1f1f1'></td></tr>");

                DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'><b>Voy No</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>From</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>To</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>Type</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>B/L</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>Commenced</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>Completed</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>Distance (nm)</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>Time At Sea</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>Anchorage Time</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>Cargo Qty (MT)</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>Transport Work (T-nm)</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>HFO</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>LFO</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>MDO/MGO</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>Agg CO<sub><font size='6px'>2</font></sub> Emitted (T-CO<sub><font size='6px'>2</font></sub>)</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>Per Dist (Kg CO<sub><font size='6px'>2</font></sub>/nm)</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>Per Transport Work (g CO<sub><font size='6px'>2</font></sub>/T-nm)</b></th></tr>");

                if (t1.Rows.Count > 0)
                {
                    foreach (DataRow dr in t1.Rows)
                    {
                        if (dr["FLDEUVOYAGEYN"].ToString() == "1")
                            DsHtmlcontent.Append("<tr style=\"font-weight:bold;\">");//colspan='2'
                        else
                            DsHtmlcontent.Append("<tr>");//colspan='2'
                                                 
                        DsHtmlcontent.Append("<td>" + dr["FLDVOYAGENO"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td>" + dr["FLDFROM"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td>" + dr["FLDTO"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td>" + dr["FLDREPORTTYPE"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td>" + dr["FLDBALLASTRLADEN"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td>" + General.GetDateTimeToString(dr["FLDCOMMENCED"].ToString()) + " " + string.Format("{0:t}", dr["FLDCOMMENCED"]) + "</td>");
                        DsHtmlcontent.Append("<td>" + General.GetDateTimeToString(dr["FLDCOMPLETED"].ToString()) + " " + string.Format("{0:t}", dr["FLDCOMPLETED"]) + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDDISTANCE"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDTIMEATSEA"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDANCHORAGETIME"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCARGOQTYONARRIVAL"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDTRANSPORTWORK"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOCONSATSEA"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLFOCONSATSEA"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOCONSATSEA"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCO2EMISSIONATSEA"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCO2EMISSIONPERDIST"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCO2EMISSIONPERTRANSPORTWORK"].ToString() + "</td>");
                        DsHtmlcontent.Append("</tr>");//colspan='2'
                    }
                }
                DsHtmlcontent.Append("</table>");
            }

            if (dsinport.Tables[0].Rows.Count > 0)
            {
                DataTable t1 = new DataTable();
                t1 = dsinport.Tables[0];
                DsHtmlcontent.Append("<br />");
                DsHtmlcontent.Append("<br />");
                DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
                DsHtmlcontent.Append("<font size='9px' face='Helvetica'>");
                DsHtmlcontent.Append("<table ID='tbl1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
                DsHtmlcontent.Append("<font color='white' size=14px><tr><td colspan='13' align='center'><b>In Port</b></td></tr></font>");
                DsHtmlcontent.Append("</table>");

                DsHtmlcontent.Append("<table ID=\"tbl1\" border ='1'  opacity='0.5' cellpadding=\"7\" cellspacing='0' style='border:black 1px solid'>");
                DsHtmlcontent.Append("<tr style=\"font-weight:bold;\"><td colspan=\"5\" bgcolor='#f1f1f1'></td>");
                DsHtmlcontent.Append("<td colspan=\"4\" bgcolor='#f1f1f1' align=\"center\">Cargo Quantity (MT)</td>");
                DsHtmlcontent.Append("<td colspan=\"3\" bgcolor='#f1f1f1' align=\"center\">Fuel Consumption (MT)</td>");
                DsHtmlcontent.Append("<td colspan=\"1\" bgcolor='#f1f1f1'></td></tr>");
                DsHtmlcontent.Append("<tr style=\"font-weight:bold;\"><th bgcolor='#f1f1f1'>Voy No</th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Port</th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Arrival</th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Departure</th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Time In Port</th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>On Arrival</th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Load</th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Discharged</th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>On Departure</th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>HFO</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>LFO</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><b>MDO/MGO</b></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Agg CO<sub><font size='6px'>2</font></sub> Emitted (T-CO<sub><font size='6px'>2</font></sub>)</th></tr>");

                if (t1.Rows.Count > 0)
                {
                    foreach (DataRow dr in t1.Rows)
                    {
                        if (dr["FLDEUVOYAGEYN"].ToString() == "1" && dr["FLDTOPORTEUYN"].ToString() == "1")
                            DsHtmlcontent.Append("<tr style=\"font-weight:bold;\">");//colspan='2'
                        else
                            DsHtmlcontent.Append("<tr>");//colspan='2'

                        DsHtmlcontent.Append("<td>" + dr["FLDVOYAGENO"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td>" + dr["FLDTO"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td>" + General.GetDateTimeToString(dr["FLDCOMPLETED"].ToString()) + " " + string.Format("{0:t}", dr["FLDCOMPLETED"]) + "</td>");
                        DsHtmlcontent.Append("<td>" + General.GetDateTimeToString(dr["FLDCURRENTSBE"].ToString()) + " " + string.Format("{0:t}", dr["FLDCURRENTSBE"]) + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDTIMEINPORT"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCARGOQTYONARRIVAL"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCARGOQTYLOADED"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCARGOQTYDISC"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCARGOQTYONDEPT"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOCONSINPORT"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLFOCONSINPORT"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOCONSINPORT"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCO2EMISSIONINPORT"].ToString() + "</td>");
                        DsHtmlcontent.Append("</tr>");//colspan='2'
                    }
                }
                DsHtmlcontent.Append("</table>");
            }

            if (dssummary.Tables[0].Rows.Count > 0 && arrivalsummaryid == null )
            {
                DataTable t1 = new DataTable();
                t1 = dssummary.Tables[0];
                DsHtmlcontent.Append("<br />");

                DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
                DsHtmlcontent.Append("<font size='9px' face='Helvetica'>");
                DsHtmlcontent.Append("<table ID='tbl1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
                DsHtmlcontent.Append("<font color='white' size='14px'><tr><td colspan='13' align='center'><b>Fuel Consumption</b></td></tr></font>");
                DsHtmlcontent.Append("</table>");

                DsHtmlcontent.Append("<table ID=\"tbl1\" border ='1'  opacity='0.5' cellpadding=\"7\" cellspacing='0' style='border:black 1px solid'>");
                DsHtmlcontent.Append("<tr style=\"font-weight:bold;\"><td colspan=\"3\" bgcolor='#f1f1f1'></td>");
                DsHtmlcontent.Append("<td colspan=\"5\" bgcolor='#f1f1f1' align=\"center\">Fuel Consumption</td>");
                DsHtmlcontent.Append("<td colspan=\"5\" bgcolor='#f1f1f1' align=\"center\">CO2 Emissions (mT)</td></tr>");
                DsHtmlcontent.Append("<tr style=\"font-weight:bold;\"><td bgcolor='#f1f1f1' colspan='2'>Fuel Type</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Emisson Factor</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>All Voyages</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>At Sea</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>In Port</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Laden Voyages</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Cargo Heating</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>All Voyages</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>At Sea</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>In Port</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Laden Voyages</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Cargo Heating</td></tr>");

                if (t1.Rows.Count > 0)
                {
                    DataRow dr = t1.Rows[0];
                    DsHtmlcontent.Append("<tr>");//colspan='2'
                    DsHtmlcontent.Append("<td colspan='2' >HFO</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">3.114</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOALL"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOATSEA"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOINPORT"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOLADEN"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOCGHT"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOALLEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOATSEAEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOINPORTEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOLADENEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOCGHTEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("</tr>");//colspan='2'

                    DsHtmlcontent.Append("<tr>");//colspan='2'
                    DsHtmlcontent.Append("<td colspan='2' >LFO</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">3.151</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLFOALL"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLFOATSEA"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLFOINPORT"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLFOLADEN"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLFOCGHT"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLFOALLEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLFOATSEAEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLFOINPORTEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLFOLADENEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLFOCGHTEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("</tr>");//colspan='2'

                    DsHtmlcontent.Append("<tr>");//colspan='2'
                    DsHtmlcontent.Append("<td colspan='2' >MDO/MGO</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">3.206</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOALL"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOATSEA"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOINPORT"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOLADEN"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOCGHT"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOALLEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOATSEAEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOINPORTEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOLADENEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOCGHTEMISSION"].ToString() + "</td>");
                    DsHtmlcontent.Append("</tr>");//colspan='2'
                }
                DsHtmlcontent.Append("</table>");

                DsHtmlcontent.Append("<br/>");
                DsHtmlcontent.Append("<table ID='tbl1' border='1' cellpadding='7' cellspacing='0' ><tr style=\"font-weight:bold;\">");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><h3>Parameter</h3></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><h3>Value</h3></th>");
                DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><h3>Unit</h3></th></tr>");
                if (t1.Rows.Count > 0)
                {
                    DataRow dr = t1.Rows[0];

                    decimal TotalCO2emission = decimal.Zero;
                    decimal TotalCO2emiBEUPorts = decimal.Zero;
                    decimal TotalCO2emiFEUPorts = decimal.Zero;
                    decimal TotalCO2emiTEUPorts = decimal.Zero;
                    decimal TotalCO2emiWEUPorts = decimal.Zero;
                    decimal TotalFuelConsLaden = decimal.Zero;
                    decimal TotalCO2EmissionLaden = decimal.Zero;
                    decimal TotalFuelConsCGHT = decimal.Zero;
                    decimal FuelConsPerDistance = decimal.Zero;
                    decimal FuelConsPerTransportwork = decimal.Zero;
                    decimal TotalCO2emissionperDist = decimal.Zero;
                    decimal TotalCO2emissionperTransportwork = decimal.Zero;
                    decimal FuelConsPerDistanceLaden = decimal.Zero;
                    decimal CO2emissionPerDistanceLaden = decimal.Zero;


                    TotalCO2emission = (General.GetNullableDecimal(dr["FLDHFOATSEAEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDHFOATSEAEMISSION"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDLFOATSEAEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDLFOATSEAEMISSION"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDMDOATSEAEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDMDOATSEAEMISSION"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDHFOINPORTEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDHFOINPORTEMISSION"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDLFOINPORTEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDLFOINPORTEMISSION"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDMDOINPORTEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDMDOINPORTEMISSION"].ToString()) : decimal.Zero);

                    TotalCO2emiBEUPorts = (General.GetNullableDecimal(dr["FLDHFOBEUPORTSEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDHFOBEUPORTSEMISSION"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDLFOBEUPORTSEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDLFOBEUPORTSEMISSION"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDMDOBEUPORTSEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDMDOBEUPORTSEMISSION"].ToString()) : decimal.Zero);

                    TotalCO2emiFEUPorts = (General.GetNullableDecimal(dr["FLDHFOFEUPORTSEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDHFOFEUPORTSEMISSION"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDLFOFEUPORTSEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDLFOFEUPORTSEMISSION"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDMDOFEUPORTSEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDMDOFEUPORTSEMISSION"].ToString()) : decimal.Zero);

                    TotalCO2emiTEUPorts = (General.GetNullableDecimal(dr["FLDHFOTEUPORTSEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDHFOTEUPORTSEMISSION"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDLFOTEUPORTSEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDLFOTEUPORTSEMISSION"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDMDOTEUPORTSEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDMDOTEUPORTSEMISSION"].ToString()) : decimal.Zero);

                    TotalCO2emiWEUPorts = (General.GetNullableDecimal(dr["FLDHFOWEUPORTSEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDHFOWEUPORTSEMISSION"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDLFOWEUPORTSEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDLFOWEUPORTSEMISSION"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDMDOWEUPORTSEMISSION"].ToString()) != null ? decimal.Parse(dr["FLDMDOWEUPORTSEMISSION"].ToString()) : decimal.Zero);

                    TotalFuelConsLaden = (General.GetNullableDecimal(dr["FLDHFOATSEALADEN"].ToString()) != null ? decimal.Parse(dr["FLDHFOATSEALADEN"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDLFOATSEALADEN"].ToString()) != null ? decimal.Parse(dr["FLDLFOATSEALADEN"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDMDOATSEALADEN"].ToString()) != null ? decimal.Parse(dr["FLDMDOATSEALADEN"].ToString()) : decimal.Zero);

                    TotalCO2EmissionLaden = ((General.GetNullableDecimal(dr["FLDHFOATSEALADEN"].ToString()) != null ? decimal.Parse(dr["FLDHFOATSEALADEN"].ToString()) : decimal.Zero) * decimal.Parse("3.114"))
                                        + ((General.GetNullableDecimal(dr["FLDLFOATSEALADEN"].ToString()) != null ? decimal.Parse(dr["FLDLFOATSEALADEN"].ToString()) : decimal.Zero) * decimal.Parse("3.151"))
                                        +((General.GetNullableDecimal(dr["FLDMDOATSEALADEN"].ToString()) != null ? decimal.Parse(dr["FLDMDOATSEALADEN"].ToString()) : decimal.Zero) * decimal.Parse("3.206"));

                    TotalFuelConsCGHT = (General.GetNullableDecimal(dr["FLDHFOCGHT"].ToString()) != null ? decimal.Parse(dr["FLDHFOCGHT"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDLFOCGHT"].ToString()) != null ? decimal.Parse(dr["FLDLFOCGHT"].ToString()) : decimal.Zero)
                                        + (General.GetNullableDecimal(dr["FLDMDOCGHT"].ToString()) != null ? decimal.Parse(dr["FLDMDOCGHT"].ToString()) : decimal.Zero);

                    if (General.GetNullableDecimal(dr["FLDDISTANCE"].ToString()) != null && General.GetNullableDecimal(dr["FLDDISTANCE"].ToString()) > 0)
                        FuelConsPerDistance = ((General.GetNullableDecimal(dr["FLDTOTALFUELCONS"].ToString()) != null ? decimal.Parse(dr["FLDTOTALFUELCONS"].ToString()) : decimal.Zero) 
                                                / decimal.Parse(dr["FLDDISTANCE"].ToString()))*1000;

                    if (General.GetNullableDecimal(dr["FLDTRANSPORTWORK"].ToString()) != null && General.GetNullableDecimal(dr["FLDTRANSPORTWORK"].ToString()) > 0)
                        FuelConsPerTransportwork = (((General.GetNullableDecimal(dr["FLDTOTALFUELCONS"].ToString()) != null ? decimal.Parse(dr["FLDTOTALFUELCONS"].ToString()) : decimal.Zero)
                                                / (decimal.Parse(dr["FLDTRANSPORTWORK"].ToString())))*1000*1000);

                    if (General.GetNullableDecimal(dr["FLDDISTANCE"].ToString()) != null && General.GetNullableDecimal(dr["FLDDISTANCE"].ToString()) > 0)
                        TotalCO2emissionperDist = ((TotalCO2emission / (decimal.Parse(dr["FLDDISTANCE"].ToString()))) * 1000);

                    if (General.GetNullableDecimal(dr["FLDTRANSPORTWORK"].ToString()) != null && General.GetNullableDecimal(dr["FLDTRANSPORTWORK"].ToString()) > 0)
                        TotalCO2emissionperTransportwork = ((TotalCO2emission/ (decimal.Parse(dr["FLDTRANSPORTWORK"].ToString()))) * 1000 * 1000);

                    if (General.GetNullableDecimal(dr["FLDDISTANCELADEN"].ToString()) != null && General.GetNullableDecimal(dr["FLDDISTANCELADEN"].ToString()) > 0)
                        FuelConsPerDistanceLaden = ((TotalFuelConsLaden / (decimal.Parse(dr["FLDDISTANCELADEN"].ToString()))) * 1000);

                    if (General.GetNullableDecimal(dr["FLDDISTANCELADEN"].ToString()) != null && General.GetNullableDecimal(dr["FLDDISTANCELADEN"].ToString()) > 0)
                        CO2emissionPerDistanceLaden = ((TotalCO2EmissionLaden / (decimal.Parse(dr["FLDDISTANCELADEN"].ToString()))) * 1000);

                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total Distance Travelled</td> <td>" + dr["FLDDISTANCE"].ToString() + "</td><td>nm</td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total Time Spent at Sea</td><td>" + dr["FLDTIMEATSEA"].ToString() + "</td><td>hr</td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total Transport Work</td><td>" + dr["FLDTRANSPORTWORK"].ToString() + "</td><td>T-nm</td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total Fuel Consumption</td><td>" + dr["FLDTOTALFUELCONS"].ToString() + "</td><td>mT</td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total Aggregated CO2 emissions</td><td>" + TotalCO2emission.ToString("#.##") + "</td><td>T-CO<sub><font size='6px'>2</font></sub></td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total aggregated CO2 emissions Between EU ports</td><td>" + TotalCO2emiBEUPorts.ToString("#.##") + "</td><td>T-CO<sub><font size='6px'>2</font></sub></td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total aggregated CO2 emissions To EU ports</td><td>" + TotalCO2emiTEUPorts.ToString("#.##") + "</td><td>T-CO<sub><font size='6px'>2</font></sub></td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total aggregated CO2 emissions From EU ports</td><td>" + TotalCO2emiFEUPorts.ToString("#.##") + "</td><td>T-CO<sub><font size='6px'>2</font></sub></td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total aggregated CO2 emissions Within EU ports</td><td>" + TotalCO2emiWEUPorts.ToString("#.##") + "</td><td>T-CO<sub><font size='6px'>2</font></sub></td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total Fuel Consumption, Laden Voyages</td><td>" + TotalFuelConsLaden.ToString("#.##") + "</td><td>mT</td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total aggregated CO2 emissions, Laden Voyages<td>" + TotalCO2EmissionLaden.ToString("#.##") + "</td><td>T-CO<sub><font size='6px'>2</font></sub></td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total Fuel Consumption for Cargo Heating</td><td>" + TotalFuelConsCGHT.ToString("#.##") + "</td><td>mT</td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Fuel Cons per distance</td><td>" + FuelConsPerDistance.ToString("#.##") + "</td><td>kg/nm</td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Fuel Cons per Transport work</td><td>" + FuelConsPerTransportwork.ToString("#.##") + "</td><td>g/T-nm</td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>CO2 emissions per distance</td><td>" + TotalCO2emissionperDist.ToString("#.##") + "</td><td>kg-CO<sub><font size='6px'>2</font></sub>/nm</td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>CO2 emissions per transport work</td><td>" + TotalCO2emissionperTransportwork.ToString("#.##") + "</td><td>g-CO<sub><font size='6px'>2</font></sub>/T-nm</td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Fuel Cons per distance, Laden voyages</td><td>" + FuelConsPerDistanceLaden.ToString("#.##") + "</td><td>kg/nm</td></tr>");
                    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>CO2 emissions per distance, Laden Voyages</td><td>" + CO2emissionPerDistanceLaden.ToString("#.##") + "</td><td>kg-CO<sub><font size='6px'>2</font></sub>/nm</td></tr>");
                }
                
                DsHtmlcontent.Append("</table>");
                DsHtmlcontent.Append("<br/><br/>");
                DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='5'>");
                DsHtmlcontent.Append("</table>");

            }
            //if (dssummary.Tables[1].Rows.Count > 0 && arrivalsummaryid == null)
            //{
            //    DataRow dr1 = dssummary.Tables[1].Rows[0];

            //    
            //}

            DsHtmlcontent.Append("<br />");
            DsHtmlcontent.Append("<table ID='tblheaderBDN' border ='1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
            DsHtmlcontent.Append("<font color='white' size=14px ><tr><td colspan='5' align='left'><b>Bunkering or De-bunkering</b></td></tr></font>");
            DsHtmlcontent.Append("</table>");
            DsHtmlcontent.Append("<br />");

            DsHtmlcontent.Append("<table ID='tblVerifier' border='1' cellpadding='7' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Date</b></td><td bgcolor='#f1f1f1'><b>Port</b></td><td bgcolor='#f1f1f1'><b>Event</b></td><td bgcolor='#f1f1f1'><b>Fuel Grade</b></td><td bgcolor='#f1f1f1'><b>Qty (mt)</b></td></tr>");
            if (dtBDN.Rows.Count > 0)
            {
                foreach (DataRow dr in dtBDN.Rows)
                {
                    DsHtmlcontent.Append("<tr><td>" + General.GetDateTimeToString(dr["FLDDATE"].ToString()) + "</td><td>" + dr["FLDSEAPORTNAME"].ToString() + "</td><td>" + dr["FLDTYPE"].ToString() + "</td><td>" + dr["FLDCODE"].ToString() + "</td><td>" + dr["FLDQUANTITY"].ToString() + "</td></tr>");
                }
            }
            DsHtmlcontent.Append("</table>");

            DsHtmlcontent.Append("<br />");
            DsHtmlcontent.Append("<table ID='tblheaderSTS' border ='1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
            DsHtmlcontent.Append("<font color='white' size=14px ><tr><td colspan='5' align='left'><b>Ship to Ship Transfer of Cargo<b></td></tr></font>");
            DsHtmlcontent.Append("</table>");
            DsHtmlcontent.Append("<br />");

            DsHtmlcontent.Append("<table ID='tblVerifier' border='1' cellpadding='7' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Port</b></td><td bgcolor='#f1f1f1'><b>Cargo</b></td><td bgcolor='#f1f1f1'><b>Qty (mt)</b></td><td bgcolor='#f1f1f1'><b>Commenced</b></td><td bgcolor='#f1f1f1'><b>Completed</b></td></tr>");
            if (dtSTS.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSTS.Rows)
                {
                    DsHtmlcontent.Append("<tr><td>" + dr["FLDSEAPORTNAME"].ToString() + "</td><td>" + dr["FLDCARGONAME"].ToString() + "</td><td>" + dr["FLDQUANTITY"].ToString() + "</td><td>" + General.GetDateTimeToString(dr["FLDCOMMENCED"].ToString()) + "</td><td>" +General.GetDateTimeToString(dr["FLDCOMPLETED"].ToString()) + "</td></tr>");
                }
            }
            DsHtmlcontent.Append("</table>");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return DsHtmlcontent.ToString();
    }
    public void ConvertToPdf(string HTMLString,string filename)
    {
        try
        {
            if (HTMLString != "")
            {
                using (var ms = new MemoryStream())
                {
                    iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(595f, 842f));
                    document.SetPageSize(PageSize.LEGAL);
                    document.SetPageSize(PageSize.LEGAL.Rotate());
                    document.SetMargins(36f, 36f, 36f, 0f);
                    string filefullpath = filename + ".pdf";
                    PdfWriter.GetInstance(document, ms);
                    document.Open();
                    string imageURL = "http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png";
                    StyleSheet styles = new StyleSheet();
                    styles.LoadStyle(".headertable td", "background-color", "Blue");
                    ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);
                    for (int k = 0; k < htmlarraylist.Count; k++)
                    {
                        document.Add((iTextSharp.text.IElement)htmlarraylist[k]);

                    }
                    document.Close();
                    Response.Buffer = true;
                    var bytes = ms.ToArray();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filefullpath);
                    Response.OutputStream.Write(bytes, 0, bytes.Length);
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
    public void ConvertToExcel(string HTMLString, string filename)
    {
        try
        {
            if (HTMLString != "")
            {
                string style = @"<style> TD { mso-number-format:\@; } </style> ";
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename="+ filename + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                Response.Write(style);
                Response.Output.Write(HTMLString);
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void chkEUVoyYN_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridItem gv in gvConsumption.Items)
        {
            PhoenixVesselPositionEUMRVSummaryReport.EUReportingSummaryUpdate(General.GetNullableGuid(((RadLabel)gv.FindControl("lblID")).Text)
                            , ((RadCheckBox)gv.FindControl("chkEUVoyYN")).Checked == true ? 1 : 0);
        }
        Rebind();
    }

    private string ConvertAnnualSummaryXML(int? vesselid, DateTime? commencedfrom, DateTime? commencedto, DateTime? completdfrom, DateTime? completedto, int? port, string vesselname, int? owner, int? fleet)
    {
        string path = Server.MapPath("~/Attachments/TEMP/" + vesselname + "_emissions.xml");
        try
        {
            DataSet dssummary = PhoenixVesselPositionEUMRVSummaryReport.EUMRVVoyagesAnualSummaryXML(vesselid, commencedfrom, commencedto, completdfrom, completedto, port, owner, fleet,chkShowNonEU.Checked?1:0);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(dssummary.Tables[0].Rows[0][0].ToString());
            // Save the document to a file and auto-indent the output.
            if (File.Exists(path))
                File.Delete(path);
            using (XmlTextWriter writer = new XmlTextWriter(path, null))
            {
                writer.Formatting = Formatting.Indented;
                doc.Save(writer);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return path;
    }

    protected void gvConsumption_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvConsumption.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDVOYAGENO", "FLDFROM", "FLDTO", "FLDBALLASTRLADEN", "FLDCOMMENCED", "FLDCOMPLETED", "FLDREPORTTYPE", "FLDDISTANCE", "FLDTIMEATSEA", "FLDANCHORAGETIME", "FLDCARGOQTYONARRIVAL", "FLDHFOCONSATSEA", "FLDMDOCONSATSEA", "FLDTRANSPORTWORK", "FLDCO2EMISSIONATSEA", "FLDCO2EMISSIONPERDIST", "FLDCO2EMISSIONPERTRANSPORTWORK" };
        string[] alCaptions = { "Vessel", "Voy No.", "From", "To", "Ballast/Laden", "Commenced", "Completed", "Report Type", "Distance (nm)", "Time At Sea", "Anchorage Time", "Cargo Qty (MT)", "HFO Cons (MT)", "MDO/MGO (MT)", "Transport Work (T-nm)", "Agg CO2 Emitted (T-CO2)", "Per Dist (Kg CO2/nm)", "Per Transport Work (g CO2/T-nm)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentEUVoyageListFilter;

        int? vesselid = General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? ((nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
        ds = PhoenixVesselPositionEUMRVSummaryReport.EUReportingSummarySearch(vesselid
                                                                                , sortexpression
                                                                                 , sortdirection
                                                                                 , (int)ViewState["PAGENUMBER"]
                                                                                 , gvConsumption.PageSize
                                                                                 , ref iRowCount
                                                                                 , ref iTotalPageCount
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceFrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceTo"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedfrom"] : string.Empty)
                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedTo"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlPort"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlOwner"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
                                                                                 , General.GetNullableInteger(nvc != null ? nvc["chkShowNonEU"] : string.Empty)
                                                                                 , 0
                                                                                 );
        General.SetPrintOptions("gvConsumption", "EU Voyages", alCaptions, alColumns, ds);

        gvConsumption.DataSource = ds;
        gvConsumption.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvConsumption.SelectedIndexes.Clear();
        gvConsumption.EditIndexes.Clear();
        gvConsumption.DataSource = null;
        gvConsumption.Rebind();
    }
}
