using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.Text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using Telerik.Web.UI;

public partial class VesselPositionArrivalReportMRVSummary : PhoenixBasePage
{
    DataSet dsDeatil = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "ARRIVALREPORT");
            toolbar.AddButton("Noon to EOSP", "EOSP");
            toolbar.AddButton("Passage Summary", "SUMMARY");
            toolbar.AddButton("MRV Summary  ", "MRVSUMMARY");
            DepartureSummary.AccessRights = this.ViewState;
            DepartureSummary.MenuList = toolbar.Show();

            DepartureSummary.SelectedMenuIndex = 3;

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Export PDF", "PDF",ToolBarDirection.Right);
            MRVSummary.AccessRights = this.ViewState;
            MRVSummary.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
               
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void DepartureSummary_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EOSP"))
            {
                Response.Redirect("../VesselPosition/VesselPositionArrivalReportEdit.aspx", true);
            }
            if (CommandName.ToUpper().Equals("SUMMARY"))
            {
                Response.Redirect("../VesselPosition/VesselPositionArrivalReportPassageSummary.aspx");
            }
            if (CommandName.ToUpper().Equals("ARRIVALREPORT"))
            {
                if (Filter.CurrentNoonReportLaunchFrom != null && Filter.CurrentNoonReportLaunchFrom == "ST")
                    Response.Redirect("VesselPositionReports.aspx", false);
                else
                    Response.Redirect("VesselPositionArrivalReport.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MRVSummary_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("PDF"))
            {
                ConvertToPdf(PrepareHtmlDoc());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void BindData()
    {
        DataSet ds = PhoenixVesselPositionArrivalReport.ArrivalMRVSummaryReport(General.GetNullableGuid(Session["VESSELARRIVALID"].ToString()));
        dsDeatil = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtVessel.Text = dr["FLDVESSELNAME"].ToString();
            txtVoyage.Text = dr["FLDVOYAGENUMBER"].ToString();
            chkBallast.Checked = dr["FLDBALLASTYN"].ToString() == "1" ? false : true;
            chkLaden.Checked = dr["FLDBALLASTYN"].ToString() == "1" ? true : false;
            chkEUPortYN.Checked = dr["FLDFROMEUPORTYN"].ToString() == "1" ? true : false;
            txtFromPort.Text = dr["FLDDEPARTUREPORTNAME"].ToString();
            txtToPort.Text = dr["FLDARRIVALPORTNAME"].ToString();
            chkToPort.Checked = dr["FLDTOEUPORTYN"].ToString() == "1" ? true : false;
            txdistanceTravelled.Text = dr["FLDTOTALDISTANCE"].ToString();
            txtTimespentatsea.Text = dr["FLDTIMESPENTATSEA"].ToString();
            txtTotalCargoTransported.Text = dr["FLDTOTALCARGO"].ToString();
            txtTotalTransportwork.Text = dr["FLDTRANSPORTWORK"].ToString();
            txtCO2Emitted.Text = dr["FLDTOTALCONSUMPTION"].ToString();
            
        }
    }

    protected void gvConsumption_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow gv = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell Detail = new TableCell();
            TableCell FOConsumption = new TableCell();
            TableCell Aggco2emision = new TableCell();
            TableCell AvgFuelCons = new TableCell();
            TableCell Avgco2emision = new TableCell();


            Detail.ColumnSpan = 4;
            FOConsumption.ColumnSpan = 2;
            Aggco2emision.ColumnSpan = 1;
            AvgFuelCons.ColumnSpan = 2;
            Avgco2emision.ColumnSpan = 2;

            Detail.Text = "";
            FOConsumption.Text = "FO Cons (MT)";
            Aggco2emision.Text = "Aggregated CO2";
            AvgFuelCons.Text = "Average Fuel Consumption";
            Avgco2emision.Text = "Average CO₂ Emissions";

            Detail.Attributes.Add("style", "text-align:center");
            FOConsumption.Attributes.Add("style", "text-align:center");
            Aggco2emision.Attributes.Add("style", "text-align:center");
            AvgFuelCons.Attributes.Add("style", "text-align:center");
            Avgco2emision.Attributes.Add("style", "text-align:center");

            gv.Cells.Add(Detail);
            gv.Cells.Add(FOConsumption);
            gv.Cells.Add(Aggco2emision);
            gv.Cells.Add(AvgFuelCons);
            gv.Cells.Add(Avgco2emision);

            gvConsumption.Controls[0].Controls.AddAt(0, gv);
        }
    }
    private string PrepareHtmlDoc()
    {
        StringBuilder DsHtmlcontent = new StringBuilder();
        if (dsDeatil.Tables[0].Rows.Count > 0)
        {
            DataRow dr1 = dsDeatil.Tables[0].Rows[0];

            string ballastladen = dr1["FLDBALLASTYN"].ToString() == "1" ? "Laden" : "Ballast";
            string fromeuport = dr1["FLDFROMEUPORTYN"].ToString() == "1" ? "Yes" : "No";
            string toeuport = dr1["FLDTOEUPORTYN"].ToString() == "1" ? "Yes" : "No";

            DsHtmlcontent.Append("<html><table><tr><td align=\"center\"><b>ARRIVAL REPORT MRV SUMMARY</b></td></tr></table><br />");

            DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='3' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\" >Vessel</td><td colspan=\"4\">" + dr1["FLDVESSELNAME"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Voyage No</td><td colspan=\"4\">" + dr1["FLDVOYAGENUMBER"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Ballsat / Laden </td><td colspan=\"4\">" + ballastladen + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"4\"></td></td><td colspan=\"2\">EU Port?</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">From </td><td colspan=\"2\">" + dr1["FLDDEPARTUREPORTNAME"] + "</td><td colspan=\"2\">" + fromeuport + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">To </td><td colspan=\"2\">" + dr1["FLDARRIVALPORTNAME"].ToString() + "</td><td colspan=\"2\">" + toeuport + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Total Distance Travelled</td><td colspan=\"4\">" + dr1["FLDTOTALDISTANCE"].ToString() + "   nm</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Time Spent At Sea </td><td colspan=\"4\">" + dr1["FLDTIMESPENTATSEA"].ToString() + "   hr</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Total Cargo Transported</td><td colspan=\"4\">" + dr1["FLDTOTALCARGO"].ToString() + "   MT</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Total Transport Work </td><td colspan=\"4\">" + dr1["FLDTRANSPORTWORK"].ToString() + "   T-nm</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Total Aggregated CO2 Emitted </td><td colspan=\"4\">" + dr1["FLDTOTALCONSUMPTION"].ToString() + "   T-CO2</td></tr>");
            DsHtmlcontent.Append("</table>");


            if (dsDeatil.Tables[1].Rows.Count > 0)
            {
                DataTable t1 = new DataTable();
                t1 = dsDeatil.Tables[1];
                DsHtmlcontent.Append("<br />");

                DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
                DsHtmlcontent.Append("<font size='9px' face='Helvetica'>");
                DsHtmlcontent.Append("<table ID='tbl1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='4' cellspacing='0'>");
                DsHtmlcontent.Append("<font color='white' size=14px ><tr><td height='9' align='center'><b>Consumption</b></td></tr></font>");
                DsHtmlcontent.Append("</table>");

                DsHtmlcontent.Append("<table ID=\"tbl2\" border ='0.5'  opacity='0.5' cellpadding=\"3\" cellspacing='0' style='border:red 1px solid'>");
                DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' colspan='5'></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' colspan='2' align=\"center\"><b>FO Cons (MT)</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' colspan='1' align=\"center\"><b>Agg CO2</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' colspan='2' align=\"center\"><b>Average Fuel Consumption</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' colspan='2' align=\"center\"><b>Average CO₂ Emissions</b></td></tr>");

                DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' colspan='2'><b>Fuel Type</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Emission Factor</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>ROB @ SBE</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>ROB @ FWE</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Total</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Cargo Heating</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Emitted(MT CO2)</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Distance (kg/nm)</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Transport Work (g/t-nm)</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Distance (kg CO2/nm)</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Transport Work (g CO2/t-nm)</b></td>");
                DsHtmlcontent.Append("</tr>");

                if (t1.Rows.Count > 0)
                {
                    foreach (DataRow dr in t1.Rows)
                    {
                        DsHtmlcontent.Append("<tr>");//colspan='2'
                        DsHtmlcontent.Append("<td colspan='2' >" + dr["FLDOILTYPENAME"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCF"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDROBSBE"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDROBFWE"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDTOTALCONSUMPTIONQTY"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCARGOHEATING"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCO2EMISSION"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGFUELCONSDISTANCE"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGFUELCONSTRANSWORK"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGCO2DISTANCE"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGCO2TRANSPORTWORK"].ToString() + "</td>");
                        DsHtmlcontent.Append("</tr>");//colspan='2'
                    }
                }

                DsHtmlcontent.Append("</table>");
            }

            DsHtmlcontent.Append("</html>");
        }

        return DsHtmlcontent.ToString();
    }

    public void ConvertToPdf(string HTMLString)
    {
        try
        {
            if (HTMLString != "")
            {
                using (var ms = new MemoryStream())
                {
                    iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(595f, 842f));
                    document.SetMargins(36f, 36f, 36f, 0f);
                    document.SetPageSize(iTextSharp.text.PageSize.LEGAL.Rotate());
                    string filefullpath = "MRVSummary" + ".pdf";
                    PdfWriter.GetInstance(document, ms);
                    document.Open();

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
                    Response.Flush();
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

    protected void gvConsumption_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionArrivalReport.ArrivalMRVSummaryReport(General.GetNullableGuid(Session["VESSELARRIVALID"].ToString()));
        gvConsumption.DataSource = ds.Tables[1];
    }
    protected void gvConsumptionRebind()
    {
        gvConsumption.SelectedIndexes.Clear();
        gvConsumption.EditIndexes.Clear();
        gvConsumption.DataSource = null;
        gvConsumption.Rebind();
    }
}

