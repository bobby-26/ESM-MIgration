using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.Web;
using System.Xml;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using System.Text;
using System.Collections.Specialized;

public partial class VesselPositionEUVoyageReportingInport : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvConsumption.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                //Page.ClientScript.RegisterForEventValidation(gvConsumption.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("At Sea", "ATSEA");
                toolbarmain.AddButton("In Port", "ARBERTH");
                MenuDepartureReport.AccessRights = this.ViewState;
                MenuDepartureReport.MenuList = toolbarmain.Show();
                MenuDepartureReport.SelectedMenuIndex = 1;
                
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../VesselPosition/VesselPositionEUVoyageReportingInport.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvConsumption')", "Print Grid", "icon_print.png", "PRINT");
                toolbar.AddImageButton("../VesselPosition/VesselPositionEUVoyageReportingInport.aspx", "Search", "search.png", "SEARCH");
                toolbar.AddImageButton("../VesselPosition/VesselPositionEUVoyageReportingInport.aspx", "Clear-Filter", "clear-filter.png", "CLEAR");
                MenuCrewCourseList.AccessRights = this.ViewState;
                MenuCrewCourseList.MenuList = toolbar.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                    UcVessel.Enabled = true;
                else
                    UcVessel.Enabled = false;

                txtArrivalTo.Text = DateTime.Now.ToShortDateString();
                txtDepartureTo.Text = DateTime.Now.ToShortDateString();

                

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
                    txtArrivalFrom.Text = (nvc.Get("txtCommenceFrom") == null) ? "" : nvc.Get("txtCommenceFrom").ToString();
                    txtArrivalTo.Text = (nvc.Get("txtCommenceTo") == null) ? "" : nvc.Get("txtCommenceTo").ToString();
                    txtDepartureFrom.Text = (nvc.Get("txtCompletedfrom") == null) ? "" : nvc.Get("txtCompletedfrom").ToString();
                    txtDepartureTo.Text = (nvc.Get("txtCompletedTo") == null) ? "" : nvc.Get("txtCompletedTo").ToString();
                    ucFleet.SelectedFleet = ((nvc.Get("ddlFleet") == null) || General.GetNullableInteger( nvc.Get("ddlFleet"))==null) ? "" : nvc.Get("ddlFleet").ToString();
                    ucOwner.SelectedAddress = (nvc.Get("ddlOwner") == null) ? "" : nvc.Get("ddlOwner").ToString();
                    chkShowNonEU.Checked = ((nvc.Get("chkShowNonEU") == null) || (nvc.Get("chkShowNonEU") == "0")) ? false : true;
                }
                
            }
            BindData();
           
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
        criteria.Add("txtCommenceFrom", txtArrivalFrom.Text);
        criteria.Add("txtCommenceTo", txtArrivalTo.Text);
        criteria.Add("txtCompletedfrom", txtDepartureFrom.Text);
        criteria.Add("txtCompletedTo", txtDepartureTo.Text);
        criteria.Add("ddlFleet", ucFleet.SelectedFleet);
        criteria.Add("ddlOwner", ucOwner.SelectedAddress);
        criteria.Add("chkShowNonEU", chkShowNonEU.Checked ? "1" : "0");

        Filter.CurrentEUVoyageListFilter = criteria;
    }
    protected void MenuDepartureReport_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("ATSEA"))
        {
            Response.Redirect("../VesselPosition/VesselPositionEUVoyageReporting.aspx");
        }
    }
    protected void CrewCourseList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            SetFilter();
            BindData();
        }
        if (dce.CommandName.ToUpper().Equals("CLEAR"))
        {
            txtArrivalFrom.Text = "";
            txtDepartureFrom.Text = "";
            ucPortMulti.Text = "";
            ucPortMulti.SelectedValue = "";
            txtArrivalTo.Text = DateTime.Now.ToShortDateString();
            txtDepartureTo.Text = DateTime.Now.ToShortDateString();
            ucFleet.SelectedFleet = "";
            ucOwner.SelectedAddress = "";

            SetFilter();
            BindData();
        }
    }
    protected void gvConsumption_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private string ConvertVoyageXML(string departuresummaryid, int vesselid,string vesselname)
    //{
    //    string path = Server.MapPath("~/Attachments/TEMP/" + vesselname + "_Port_Emissions.xml");
    //    try
    //    {

    //        Guid result = new Guid(departuresummaryid);
    //        DataSet dssummary = PhoenixVesselPositionEUMRVSummaryReport.EUMRVInportVoyages(vesselid, result);
    //        XmlDocument doc = new XmlDocument();
    //        doc.LoadXml(dssummary.Tables[0].Rows[0][0].ToString());
    //        // Save the document to a file and auto-indent the output.
    //        if (File.Exists(path))
    //            File.Delete(path);
    //        using (XmlTextWriter writer = new XmlTextWriter(path, null))
    //        {
    //            writer.Formatting = Formatting.Indented;
    //            doc.Save(writer);
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    return path;
    //}
    //private string SaveVoyageEXCEL(string departuresummaryid, int? vesselid, string vesselname)
    //{
    //    string path = Server.MapPath("~/Attachments/TEMP/" + vesselname + "_Port_Emissions.xls");
    //    try
    //    {

    //        if (File.Exists(path))
    //            File.Delete(path);
    //        using (StringWriter sw = new StringWriter())
    //        {
    //            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
    //            {
    //                StreamWriter writer = File.AppendText(path);
    //                writer.WriteLine(PrepareHtmlDocAnnualSummary(General.GetNullableGuid(departuresummaryid), vesselid));
    //                writer.Close();
    //            }
    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    return path;
    //}
    //private string SaveVoyagePDF(string departuresummaryid, int? vesselid, string vesselname)
    //{
    //    string path = Server.MapPath("~/Attachments/TEMP/" + vesselname + "_Port_Emissions.pdf");
    //    try
    //    {
    //        using (var ms = new MemoryStream())
    //        {
    //            iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(595f, 842f));
    //            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
    //            document.SetMargins(36f, 36f, 36f, 0f);
    //            if (File.Exists(path))
    //                File.Delete(path);
    //            PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
    //            document.Open();
    //            StyleSheet styles = new StyleSheet();
    //            styles.LoadStyle(".headertable td", "background-color", "Blue");
    //            ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(PrepareHtmlDocAnnualSummary(General.GetNullableGuid(departuresummaryid), vesselid)), styles);
    //            for (int k = 0; k < htmlarraylist.Count; k++)
    //            {
    //                document.Add((iTextSharp.text.IElement)htmlarraylist[k]);
    //            }
    //            document.Close();
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    return path;
    //}
    //private string PrepareHtmlDocAnnualSummary(Guid? departuresummaryid, int? vesselid)
    //{
    //    StringBuilder DsHtmlcontent = new StringBuilder();
    //    try
    //    {

    //        int iRowCount = 0;
    //        int iTotalPageCount = 0;
    //        // int? vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0 ? General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) : General.GetNullableInteger(UcVessel.SelectedVessel.ToString());

    //        NameValueCollection nvc = Filter.CurrentEUVoyageListFilter;

    //        DataSet dsinport = PhoenixVesselPositionEUMRVSummaryReport.EUMRVDepartureSummarySearch(vesselid
    //                                                                                , null
    //                                                                                 , null
    //                                                                                 , 1
    //                                                                                 , 1000
    //                                                                                 , ref iRowCount
    //                                                                                 , ref iTotalPageCount
    //                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceFrom"] : string.Empty)
    //                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceTo"] : string.Empty)
    //                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedfrom"] : string.Empty)
    //                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedTo"] : string.Empty)
    //                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlPort"] : string.Empty)
    //                                                                                 , departuresummaryid
    //                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlOwner"] : string.Empty)
    //                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty));

    //        DataSet dssummary = PhoenixVesselPositionEUMRVSummaryReport.EUMRVAnnualSummary(vesselid
    //                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceFrom"] : string.Empty)
    //                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCommenceTo"] : string.Empty)
    //                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedfrom"] : string.Empty)
    //                                                                                 , General.GetNullableDateTime(nvc != null ? nvc["txtCompletedTo"] : string.Empty)
    //                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlPort"] : string.Empty)
    //                                                                                 , departuresummaryid
    //                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlOwner"] : string.Empty)
    //                                                                                 , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty));


    //        string fromdate = "", todate = "";
    //        if (departuresummaryid == null)
    //        {
    //            fromdate = General.GetNullableDateTime(txtArrivalFrom.Text) != null ? txtArrivalFrom.Text : (General.GetNullableDateTime(txtDepartureFrom.Text)) != null ? txtDepartureFrom.Text : "";
    //            todate = General.GetNullableDateTime(txtDepartureTo.Text) != null ? txtDepartureTo.Text.ToString() : (General.GetNullableDateTime(txtArrivalTo.Text)) != null ? txtArrivalTo.Text.ToString() : "";
    //        }
    //        else
    //        {
    //            fromdate = General.GetNullableDateTime(dsinport.Tables[0].Rows[0]["FLDARRIVALDATE"].ToString()) != null ? dsinport.Tables[0].Rows[0]["FLDARRIVALDATE"].ToString() : (General.GetNullableDateTime(dsinport.Tables[0].Rows[0]["FLDDEPARTUREDATE"].ToString())) != null ? dsinport.Tables[0].Rows[0]["FLDDEPARTUREDATE"].ToString() : "";
    //            todate = General.GetNullableDateTime(dsinport.Tables[0].Rows[0]["FLDDEPARTUREDATE"].ToString()) != null ? dsinport.Tables[0].Rows[0]["FLDDEPARTUREDATE"].ToString() : (General.GetNullableDateTime(dsinport.Tables[0].Rows[0]["FLDARRIVALDATE"].ToString())) != null ? dsinport.Tables[0].Rows[0]["FLDARRIVALDATE"].ToString() : "";
    //        }

    //        DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
    //        DsHtmlcontent.Append("<font size='9px' face='Helvetica'>");

    //        DsHtmlcontent.Append("<table ID='tbl1' border='1' cellpadding='7' cellspacing='0' >");
    //        DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>Vessel</b></td><td><b>" + dssummary.Tables[1].Rows[0]["FLDVESSELNAME"].ToString() + "</b></td>");
    //        DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>IMO Number</b></td><td><b>" + dssummary.Tables[1].Rows[0]["FLDIMONUMBER"].ToString() + "</b></td>");
    //        DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>From</b></td><td><b>" + fromdate + "</b></td>");
    //        DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>To</b></td><td><b>" + todate + "</b></td></tr>");
    //        DsHtmlcontent.Append("</table>");

    //        if (dsinport.Tables[0].Rows.Count > 0)
    //        {
    //            DataTable t1 = new DataTable();
    //            t1 = dsinport.Tables[0];
    //            DsHtmlcontent.Append("<br />");
    //            DsHtmlcontent.Append("<br />");
    //            DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
    //            DsHtmlcontent.Append("<font size='9px' face='Helvetica'>");
    //            DsHtmlcontent.Append("<table ID='tbl1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
    //            DsHtmlcontent.Append("<font color='white' size=14px><tr><td height='9' align='center'><b>In Port</b></td></tr></font>");
    //            DsHtmlcontent.Append("</table>");

    //            DsHtmlcontent.Append("<table ID=\"tbl1\" border ='1'  opacity='0.5' cellpadding=\"7\" cellspacing='0' style='border:red 1px solid'>");
    //            DsHtmlcontent.Append("<tr><td colspan=\"5\" bgcolor='#f1f1f1'></td>");
    //            DsHtmlcontent.Append("<td colspan=\"4\" bgcolor='#f1f1f1' align=\"center\">Cargo Quantity (MT)</td>");
    //            DsHtmlcontent.Append("<td colspan=\"1\" bgcolor='#f1f1f1'></td></tr>");
    //            DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Voyage No</th>");
    //            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Port</th>");
    //            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Arrival</th>");
    //            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Departure</th>");
    //            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Time In Port</th>");
    //            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>On Arrival</th>");
    //            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Load</th>");
    //            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Discharged</th>");
    //            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>On Departure</th>");
    //            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Agg CO<sub><font size='6px'>2</font></sub> Emitted (T-CO<sub><font size='6px'>2</font></sub>)</th></tr>");

    //            if (t1.Rows.Count > 0)
    //            {
    //                foreach (DataRow dr in t1.Rows)
    //                {
    //                    DsHtmlcontent.Append("<tr>");//colspan='2'
    //                    DsHtmlcontent.Append("<td>" + dr["FLDVOYAGENO"].ToString() + "</td>");
    //                    DsHtmlcontent.Append("<td>" + dr["FLDSEAPORTNAME"].ToString() + "</td>");
    //                    DsHtmlcontent.Append("<td>" + General.GetDateTimeToString(dr["FLDARRIVALDATE"].ToString()) + " " + string.Format("{0:t}", dr["FLDARRIVALDATE"]) + "</td>");
    //                    DsHtmlcontent.Append("<td>" + General.GetDateTimeToString(dr["FLDDEPARTUREDATE"].ToString()) + " " + string.Format("{0:t}", dr["FLDDEPARTUREDATE"]) + "</td>");
    //                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDTOTALTIMESPENTINPORT"].ToString() + "</td>");
    //                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCARGOONARRIVAL"].ToString() + "</td>");
    //                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCARGOLOADED"].ToString() + "</td>");
    //                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCARGODISCHARGED"].ToString() + "</td>");
    //                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCARGOONDEPARTURE"].ToString() + "</td>");
    //                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAGGCO2EMISSION"].ToString() + "</td>");
    //                    DsHtmlcontent.Append("</tr>");//colspan='2'
    //                }
    //            }
    //            DsHtmlcontent.Append("</table>");
    //        }

    //        //if (dssummary.Tables[0].Rows.Count > 0)
    //        //{
    //        //    DataTable t1 = new DataTable();
    //        //    t1 = dssummary.Tables[0];
    //        //    DsHtmlcontent.Append("<br />");

    //        //    DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
    //        //    DsHtmlcontent.Append("<font size='9px' face='Helvetica'>");
    //        //    DsHtmlcontent.Append("<table ID='tbl1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
    //        //    DsHtmlcontent.Append("<font color='white' size=14px ><tr><td height='9' align='center'><b>Fuel Consumption</b></td></tr></font>");
    //        //    DsHtmlcontent.Append("</table>");

    //        //    DsHtmlcontent.Append("<table ID=\"tbl1\" border ='1'  opacity='0.5' cellpadding=\"7\" cellspacing='0' style='border:red 1px solid'>");
    //        //    DsHtmlcontent.Append("<tr><td colspan=\"3\" bgcolor='#f1f1f1'></td>");
    //        //    DsHtmlcontent.Append("<td colspan=\"5\" bgcolor='#f1f1f1' align=\"center\">Fuel Consumption</td>");
    //        //    DsHtmlcontent.Append("<td colspan=\"5\" bgcolor='#f1f1f1' align=\"center\">CO2 Emissions (mT)</td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' colspan='2'>Fuel Type</td>");
    //        //    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Emisson Factor</td>");
    //        //    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>All Voyages</td>");
    //        //    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>At Sea</td>");
    //        //    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>In Port</td>");
    //        //    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Laden Voyages</td>");
    //        //    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Cargo Heating</td>");
    //        //    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>All Voyages</td>");
    //        //    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>At Sea</td>");
    //        //    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>In Port</td>");
    //        //    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Laden Voyages</td>");
    //        //    DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Cargo Heating</td></tr>");

    //        //    if (t1.Rows.Count > 0)
    //        //    {
    //        //        foreach (DataRow dr in t1.Rows)
    //        //        {
    //        //            DsHtmlcontent.Append("<tr>");//colspan='2'
    //        //            DsHtmlcontent.Append("<td colspan='2' >" + dr["FLDOILTYPENAME"].ToString() + "</td>");
    //        //            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCF"].ToString() + "</td>");
    //        //            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDALLVOYAGES"].ToString() + "</td>");
    //        //            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDATSEA"].ToString() + "</td>");
    //        //            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDINPORT"].ToString() + "</td>");
    //        //            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLADENVOYAGE"].ToString() + "</td>");
    //        //            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCARGOHEATING"].ToString() + "</td>");
    //        //            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDALLVOYAGESCO2"].ToString() + "</td>");
    //        //            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDATSEACO2"].ToString() + "</td>");
    //        //            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDINPORTCO2"].ToString() + "</td>");
    //        //            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLADENVOYAGECO2"].ToString() + "</td>");
    //        //            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCARGOHEATINGCO2"].ToString() + "</td>");
    //        //            DsHtmlcontent.Append("</tr>");//colspan='2'
    //        //        }
    //        //    }

    //        //    DsHtmlcontent.Append("<tr>");//colspan='2'
    //        //    DsHtmlcontent.Append("<td><b>Total</b></td>");
    //        //    DsHtmlcontent.Append("<td></td>");
    //        //    DsHtmlcontent.Append("<td align=\"Right\"><b>" + dssummary.Tables[1].Rows[0]["FLDTOTALFUELCONSUMPTION"].ToString() + "</td></td>");
    //        //    DsHtmlcontent.Append("<td align=\"Right\"><b>" + dssummary.Tables[1].Rows[0]["FLDATSEACONSUMPTION"].ToString() + "</td></td>");
    //        //    DsHtmlcontent.Append("<td align=\"Right\"><b>" + dssummary.Tables[1].Rows[0]["FLDINPORTCONSUMPTION"].ToString() + "</td></td>");
    //        //    DsHtmlcontent.Append("<td align=\"Right\"><b>" + dssummary.Tables[1].Rows[0]["FLDLADENVOYAGECONSUMPTION"].ToString() + "</td></td>");
    //        //    DsHtmlcontent.Append("<td align=\"Right\"><b>" + dssummary.Tables[1].Rows[0]["FLDCARGOHEATINGCONSUMPTION"].ToString() + "</td></td>");
    //        //    DsHtmlcontent.Append("<td align=\"Right\"><b>" + dssummary.Tables[1].Rows[0]["FLDTOTALFUELCO2"].ToString() + "</td></td>");
    //        //    DsHtmlcontent.Append("<td align=\"Right\"><b>" + dssummary.Tables[1].Rows[0]["FLDATSEACO2"].ToString() + "</td></td>");
    //        //    DsHtmlcontent.Append("<td align=\"Right\"><b>" + dssummary.Tables[1].Rows[0]["FLDWITHINEUPORTCO2"].ToString() + "</td></td>");
    //        //    DsHtmlcontent.Append("<td align=\"Right\"><b>" + dssummary.Tables[1].Rows[0]["FLDLADENVOYAGECO2"].ToString() + "</td></td>");
    //        //    DsHtmlcontent.Append("<td align=\"Right\"><b>" + dssummary.Tables[1].Rows[0]["FLDCARGOHEATINGCO2"].ToString() + "</td></td>");
    //        //    DsHtmlcontent.Append("</tr>");//colspan='2'
    //        //    DsHtmlcontent.Append("</table>");
    //        //}
    //        //if (dssummary.Tables[1].Rows.Count > 0)
    //        //{
    //        //    DataRow dr1 = dssummary.Tables[1].Rows[0];

    //        //    DsHtmlcontent.Append("<br/>");
    //        //    DsHtmlcontent.Append("<table ID='tbl1' border='1' cellpadding='7' cellspacing='0' >");
    //        //    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><h3>Parameter</h3></th>");
    //        //    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><h3>Value</h3></th>");
    //        //    DsHtmlcontent.Append("<th bgcolor='#f1f1f1'><h3>Unit</h3></th></tr>");

    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total Distance Travelled</td> <td>" + dr1["FLDTOTALDISTANCE"].ToString() + "</td><td>nm</td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total Time Spent at Sea</td><td>" + dr1["FLDTIMEATSEA"].ToString() + "</td><td>hr</td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total Transport Work</td><td>" + dr1["FLDTRANSPORTWORK"].ToString() + "</td><td>T-nm</td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total Fuel Consumption</td><td>" + dr1["FLDTOTALFUELCONSUMPTION"].ToString() + "</td><td>mT</td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total Aggregated CO2 emissions</td><td>" + dr1["FLDTOTALFUELCO2"].ToString() + "</td><td>T-CO<sub><font size='6px'>2</font></sub></td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total aggregated CO2 emissions Between EU ports</td><td>" + dr1["FLDBETWEENEUPORTCO2"].ToString() + "</td><td>T-CO<sub><font size='6px'>2</font></sub></td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total aggregated CO2 emissions To EU ports</td><td>" + dr1["FLDTOEUPORTCO2"].ToString() + "</td><td>T-CO<sub><font size='6px'>2</font></sub></td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total aggregated CO2 emissions From EU ports</td><td>" + dr1["FLDFROMEUPORTCO2"].ToString() + "</td><td>T-CO<sub><font size='6px'>2</font></sub></td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total aggregated CO2 emissions Within EU ports</td><td>" + dr1["FLDWITHINEUPORTCO2"].ToString() + "</td><td>T-CO<sub><font size='6px'>2</font></sub></td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total Fuel Consumption, Laden Voyages</td><td>" + dr1["FLDLADENVOYAGECONSUMPTION"].ToString() + "</td><td>mT</td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total aggregated CO2 emissions, Laden Voyages<td>" + dr1["FLDLADENVOYAGECO2"].ToString() + "</td><td>T-CO<sub><font size='6px'>2</font></sub></td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Total Fuel Consumption for Cargo Heating</td><td>" + dr1["FLDCARGOHEATINGCONSUMPTION"].ToString() + "</td><td>mT</td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Fuel Cons per distance</td><td>" + dr1["FLDFUELCONSUMPTIONPERDISTANCE"].ToString() + "</td><td>T/nm</td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Fuel Cons per Transport work</td><td>" + dr1["FLDFUELCONSUMPTIONPERTRANSPORTWORK"].ToString() + "</td><td>g/nm</td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>CO2 emissions per distance</td><td>" + dr1["FLDCO2EMISSIONPERDISTANCE"].ToString() + "</td><td>kg-CO<sub><font size='6px'>2</font></sub>/nm</td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>CO2 emissions per transport work</td><td>" + dr1["FLDCO2EMISSIONPERTRANSPORTWORK"].ToString() + "</td><td>g-CO<sub><font size='6px'>2</font></sub>/T-nm</td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>Fuel Cons per distance, Laden voyages</td><td>" + dr1["FLDFUELCONSUMPITONPERDISTANCELADENVOOYAGE"].ToString() + "</td><td>T/nm</td></tr>");
    //        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width='70%'>CO2 emissions per distance, Laden Voyages</td><td>" + dr1["FLDCO2EMISSIONPERDISTANCELADENVOOYAGE"].ToString() + "</td><td>g-CO<sub><font size='6px'>2</font></sub>/nm</td></tr>");
    //        //    DsHtmlcontent.Append("</table>");
    //        //    DsHtmlcontent.Append("<br/><br/>");
    //        //    DsHtmlcontent.Append("<table ID='tbl1' border='0.5' bgcolor='#f1f1f1' cellpadding='7' cellspacing='5'>");
    //        //    DsHtmlcontent.Append("</table>");
    //        //}


    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    return DsHtmlcontent.ToString();
    //}
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDVOYAGENO", "FLDTO", "FLDCOMPLETED", "FLDCURRENTSBE", "FLDTIMEINPORT", "FLDCARGOQTYONARRIVAL", "FLDCARGOQTYLOADED", "FLDCARGOQTYDISC", "FLDCARGOQTYONDEPT", "FLDHFOCONSINPORT", "FLDMDOCONSINPORT", "FLDCO2EMISSIONINPORT" };
        string[] alCaptions = { "Vessel", "Voy No.", "Port", "Arrival", "Departure", "Time In Port", "On Arrival", "Loaded", "Discharged", "On Departure", "HFO Cons (MT)", "MDO/MGO Cons (MT)", "Agg CO₂ Emitted (T-CO₂)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentEUVoyageListFilter;

        DataSet ds = PhoenixVesselPositionEUMRVSummaryReport.EUReportingSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? ((nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                                                , sortexpression
                                                                                 , sortdirection
                                                                                 , (int)ViewState["PAGENUMBER"]
                                                                                 , General.ShowRecords(null)
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
        General.SetPrintOptions("gvConsumption", "EU Voyages", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvConsumption.DataSource = ds;
            gvConsumption.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvConsumption);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }
    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDVOYAGENO", "FLDTO", "FLDCOMPLETED", "FLDCURRENTSBE", "FLDTIMEINPORT", "FLDCARGOQTYONARRIVAL", "FLDCARGOQTYLOADED", "FLDCARGOQTYDISC", "FLDCARGOQTYONDEPT", "FLDHFOCONSINPORT", "FLDMDOCONSINPORT", "FLDCO2EMISSIONINPORT" };
        string[] alCaptions = { "Vessel", "Voy No.", "Port", "Arrival", "Departure", "Time In Port", "On Arrival", "Loaded", "Discharged", "On Departure", "HFO Cons (MT)", "MDO/MGO Cons (MT)", "Agg CO₂ Emitted (T-CO₂)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentEUVoyageListFilter;
        DataSet ds = PhoenixVesselPositionEUMRVSummaryReport.EUReportingSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0 ? ((nvc.Get("ddlVessel") == null) ? "" : nvc.Get("ddlVessel").ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                                                , sortexpression
                                                                                 , sortdirection
                                                                                 , 1
                                                                                 , (int)ViewState["ROWCOUNT"] == 0 ? 1 : (int)ViewState["ROWCOUNT"]
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
    protected void gvConsumption_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            Label lblAggCo2EmittedItem = (Label)e.Row.FindControl("lblAggCo2EmittedItem");

            if (drv!=null && drv["FLDEUVOYAGEYN"].ToString() == "1" && drv["FLDTOPORTEUYN"].ToString() =="1")
            {
                e.Row.Font.Bold = true;
            }


            //if (lblAggCo2EmittedItem != null)
            //{
            //    if (drv["FLDAGGCO2ALERTYN"].ToString() == "1")
            //    {
            //        lblAggCo2EmittedItem.BackColor = System.Drawing.Color.Red;
            //        lblAggCo2EmittedItem.ForeColor = System.Drawing.Color.White;
            //    }
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvConsumption_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell Detail = new TableCell();
            TableCell Avgco2emision = new TableCell();
            TableCell Aggco2emision = new TableCell();
            TableCell Action = new TableCell();

            Detail.ColumnSpan = 6;
            Avgco2emision.ColumnSpan = 4;
            Aggco2emision.ColumnSpan =3;
            Action.ColumnSpan = 1;

            Detail.Text = "";
            Avgco2emision.Text = "Cargo Quantity (MT)";
            Aggco2emision.Text = "";
            Action.Text = "";

            Detail.Attributes.Add("style", "text-align:center");
            Avgco2emision.Attributes.Add("style", "text-align:center");
            Aggco2emision.Attributes.Add("style", "text-align:center");
            Action.Attributes.Add("style", "text-align:center");

            gv.Cells.Add(Detail);
            gv.Cells.Add(Avgco2emision);
            gv.Cells.Add(Aggco2emision);
            gv.Cells.Add(Action);

            gvConsumption.Controls[0].Controls.AddAt(0, gv);
        }
    }
    protected void UcVessel_OnTextChangedEvent(object sender, EventArgs e)
    {
        SetFilter();
        BindData();
        SetPageNavigator();
    }
    protected void ucFleet_OnTextChangedEvent(object sender, EventArgs e)
    {
        SetFilter();
        BindData();
        SetPageNavigator();
    }
    protected void ucOwner_OnTextChangedEvent(object sender, EventArgs e)
    {
        SetFilter();
        BindData();
        SetPageNavigator();
    }
    protected void gvConsumption_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvConsumption.EditIndex = -1;
        gvConsumption.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvConsumption.SelectedIndex = -1;
        gvConsumption.EditIndex = -1;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvConsumption.SelectedIndex = -1;
        gvConsumption.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
}
