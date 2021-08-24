using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;
using System.Text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using Telerik.Web.UI;

public partial class VesselPositionQuarterlyReport : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
            toolbarvoyagetap.AddButton("List", "QUATERLYREPORTLIST");
            toolbarvoyagetap.AddButton("Quarterly Report", "QUATERLYREPORT");
            MenuQuarterlyReportTap.AccessRights = this.ViewState;
            MenuQuarterlyReportTap.MenuList = toolbarvoyagetap.Show();
            MenuQuarterlyReportTap.SelectedMenuIndex = 1;
           
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbarQuarterlyReporttap = new PhoenixToolbar();
            toolbarQuarterlyReporttap.AddButton("Export Excel", "EXCEL", ToolBarDirection.Right);
            toolbarQuarterlyReporttap.AddButton("Export PDF", "PDF", ToolBarDirection.Right);
            if (Request.QueryString["vesselid"] == null)
                toolbarQuarterlyReporttap.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuNewSaveTabStrip.AccessRights = this.ViewState;
            MenuNewSaveTabStrip.MenuList = toolbarQuarterlyReporttap.Show();
            cmdHiddenSubmits.Attributes.Add("style", "display:none;");

            txtCurrentDate.Text = System.DateTime.UtcNow.ToString();
            if (!IsPostBack)
            {
                for (int i = 2005; i <= DateTime.Now.Year; i++)
                {
                    ddlYear.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
                ddlYear.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

                ddlYear.SelectedValue = DateTime.Now.Year.ToString();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["QUATERLYREPORTID"] = "";

                ViewState["MONTH"] = "";
                ViewState["YEAR"] = "";

                if (Request.QueryString["QuarterlyReportID"] != null)
                {
                    ViewState["QUATERLYREPORTID"] = Request.QueryString["QuarterlyReportID"].ToString();
                    Session["QUARTERLYREPORTID"] = ViewState["QUATERLYREPORTID"].ToString();

                    ddlPeriod.Enabled = false;
                    ddlYear.Enabled = false;
                }
                else
                {
                    Session["QUARTERLYREPORTID"] = "";
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
    
    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NEW"))
            {
                ResetQuarterly();
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if ((ViewState["QUATERLYREPORTID"].ToString() == null) || ViewState["QUATERLYREPORTID"].ToString() == "")
                {
                    AddQuarterlyReport();
                }
                else
                {
                    SaveQuarterlyReport();
                }
                BindData();
            }

            if (CommandName.ToUpper().Equals("PDF"))
            {

                ConvertToPdf(PrepareHtmlDoc());
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ConvertToExcel(PrepareHtmlDoc());
            }
        }
        catch (Exception ex)
        {

            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
       
    }

    protected void QuarterlyReportTapp_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("QUATERLYREPORTLIST"))
        {
            if (Request.QueryString["vesselid"] != null)
                Response.Redirect("VesselPositionQuarterlyReportList.aspx?vesselid="+ Request.QueryString["vesselid"], false);
            else
                Response.Redirect("VesselPositionQuarterlyReportList.aspx", false);
        }
    }

    DataTable dt = new DataTable();
    private void BindData()
    {
        string QuaterlyReportId = ViewState["QUATERLYREPORTID"].ToString();
       DataSet ds = new DataSet();
        if (QuaterlyReportId == "")
            ds = PhoenixVesselPositionQuarterlyReport.EditQuarterlyReportCalculation(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), General.GetNullableInteger(ViewState["MONTH"].ToString()), General.GetNullableInteger(ViewState["YEAR"].ToString()));
        else
            ds = PhoenixVesselPositionQuarterlyReport.EditQuarterlyReport(General.GetNullableGuid(QuaterlyReportId));

        dt = ds.Tables[0];

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (QuaterlyReportId == "")
                txtCurrentDate.Text = System.DateTime.Now.ToString(); 
            else
                txtCurrentDate.Text = dt.Rows[0]["FLDDATE"].ToString();  

            txtballastdistancesteamed.Text = dt.Rows[0]["FLDBALLASTDISTANCESTEAMED"].ToString();
            txtloadeddistancesteamed.Text = dt.Rows[0]["FLDLOADEDDISTANCESTEAMED"].ToString();
            txttotaldistancesteamed.Text = dt.Rows[0]["FLDTOTALDISTANCESTEAMED"].ToString();
            txtballaststeamingtime.Text = dt.Rows[0]["FLDBALLASTSTEAMINGTIME"].ToString();
            txtloadedsteamingtime.Text = dt.Rows[0]["FLDLOADEDSTEAMINGTIME"].ToString();
            txttotalsteamingtime.Text = dt.Rows[0]["FLDTOTALSTEAMINGTIME"].ToString();
            txttotalavgspeed.Text = dt.Rows[0]["FLDTOTALAVGSPEED"].ToString();
            txttotalavgrpm.Text = dt.Rows[0]["FLDTOTALAVGRPM"].ToString();
            txtballastavgslip.Text = dt.Rows[0]["FLDBALLASTAVGSLIP"].ToString();
            txtloadedavgslip.Text = dt.Rows[0]["FLDLOADEDAVGSLIP"].ToString();
            txttotalavgslip.Text = dt.Rows[0]["FLDTOTALAVGSLIP"].ToString();
            txtballastavgfoconsumptionperday.Text = dt.Rows[0]["FLDBALLASTAVGFOCONSUMPTIONPERDAY"].ToString();
            txtloadedavgfoconsumptionperday.Text = dt.Rows[0]["FLDLOADEDAVGFOCONSUMPTIONPERDAY"].ToString();
            txttotalavgfoconsumptionperday.Text = dt.Rows[0]["FLDTOTALAVGFOCONSUMPTIONPERDAY"].ToString();
            txtballastmestoppagetime.Text = dt.Rows[0]["FLDBALLASTMESTOPPAGETIME"].ToString();
            txtloadedmestoppagetime.Text = dt.Rows[0]["FLDLOADEDMESTOPPAGETIME"].ToString();
            txttotalmestoppagetime.Text = dt.Rows[0]["FLDTOTALMESTOPPAGETIME"].ToString();
            txtballastavgmefoconsumptionperday.Text = dt.Rows[0]["FLDBALLASTAVGMEFOCONSUMPTIONPERDAY"].ToString();
            txtloadedavgmefoconsumptionperday.Text = dt.Rows[0]["FLDLOADEDAVGMEFOCONSUMPTIONPERDAY"].ToString();
            txttotalavgmefoconsumptionperday.Text = dt.Rows[0]["FLDTOTALAVGMEFOCONSUMPTIONPERDAY"].ToString();
            txtballastavgaedoconsumptionperday.Text = dt.Rows[0]["FLDBALLASTAVGAEDOCONSUMPTIONPERDAY"].ToString();
            txtloadedavgaedoconsumptionperday.Text = dt.Rows[0]["FLDLOADEDAVGAEDOCONSUMPTIONPERDAY"].ToString();
            txttotalavgaedoconsumptionperday.Text = dt.Rows[0]["FLDTOTALAVGAEDOCONSUMPTIONPERDAY"].ToString();
            txtballastavgboilerfoconsumptionperday.Text = dt.Rows[0]["FLDBALLASTAVGBOILERFOCONSUMPTIONPERDAY"].ToString();
            txtloadedavgboilerfoconsumptionperday.Text = dt.Rows[0]["FLDLOADEDAVGBOILERFOCONSUMPTIONPERDAY"].ToString();
            txttotalavgboilerfoconsumptionperday.Text = dt.Rows[0]["FLDTOTALAVGBOILERFOCONSUMPTIONPERDAY"].ToString();
            txtballastavgfoconsumptionperdayfortankcleaning.Text = dt.Rows[0]["FLDBALLASTAVGFOCONSUMPTIONPERDAYFORTANKCLEANING"].ToString();
            txtloadedavgfoconsumptionperdayfortankcleaning.Text = dt.Rows[0]["FLDLOADEDAVGFOCONSUMPTIONPERDAYFORTANKCLEANING"].ToString();
            txttotalavgfoconsumptionperdayfortankcleaning.Text = dt.Rows[0]["FLDTOTALAVGFOCONSUMPTIONPERDAYFORTANKCLEANING"].ToString();
            txttotalavgmecylltbnperday.Text = dt.Rows[0]["FLDTOTALAVGMECYLLTBNPERDAY"].ToString();
            txttotalmecylltbnspecificconsumption.Text = dt.Rows[0]["FLDTOTALMECYLLTBNSPECIFICCONSUMPTION"].ToString();
            txtballastavgspeed.Text = dt.Rows[0]["FLDBALLASTAVGSPEED"].ToString();
            txtloadedavgspeed.Text = dt.Rows[0]["FLDLOADEDAVGSPEED"].ToString();
            txtballastavgrpm.Text = dt.Rows[0]["FLDBALLASTAVGRPM"].ToString();
            txtloadedavgrpm.Text = dt.Rows[0]["FLDLOADEDAVGRPM"].ToString();
            txtavgmeccconsumptionperday.Text = dt.Rows[0]["FLDAVGMECCCONSUMPTIONPERDAY"].ToString();
            txtavgmecylconsumptionperday.Text = dt.Rows[0]["FLDAVGMECYLCONSUMPTIONPERDAY"].ToString();
            txtmecylspecificconsumption.Text = dt.Rows[0]["FLDMECYLSPECIFICCONSUMPTION"].ToString();
            txtavgaeccconsumptionperday.Text = dt.Rows[0]["FLDAVGAECCCONSUMPTIONPERDAY"].ToString();
            txtFromDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDREPORTFROMDATE"].ToString()) + " " + String.Format("{0:HH:mm}", dt.Rows[0]["FLDREPORTFROMDATE"]);
            txtToDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDREPORTTODATE"].ToString()) + " " + String.Format("{0:HH:mm}", dt.Rows[0]["FLDREPORTTODATE"]);
            
            if (QuaterlyReportId != "")
            {
                ddlPeriod.SelectedValue = dt.Rows[0]["FLDPERIOD"].ToString();
                ddlYear.SelectedValue = dt.Rows[0]["FLDYEAR"].ToString();
            }
        }
    }
    private string PrepareHtmlDoc()
    {
        StringBuilder DsHtmlcontent = new StringBuilder();

        if (dt.Rows.Count > 0)
        {
            DataRow dr1 = dt.Rows[0];

            DsHtmlcontent.Append("<html><table><tr><td align=\"center\"><b>QUARTELY REPORT DETAILS</b></td></tr></table><br />");

            DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='3' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Vessel</td><td colspan=\"3\">" + dr1["FLDVESSELNAME"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Selected Period</td><td colspan=\"3\">" + dr1["FLDPERIODMONTH"].ToString() + "-" + dr1["FLDYEAR"].ToString() + "</td></tr>");
            //DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Vessel</td><td colspan=\"3\">HIGH JUPITER</td><tr>");
            //DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Selected Period</td><td colspan=\"3\">Apr-Jun-2017</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >From Date</td><td colspan=\"3\">" + General.GetDateTimeToString(dr1["FLDREPORTFROMDATE"].ToString()) + " " + String.Format("{0:HH:mm}", dr1["FLDREPORTFROMDATE"]) + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >From Date</td><td colspan=\"3\">" + General.GetDateTimeToString(dr1["FLDREPORTTODATE"].ToString()) + " " + String.Format("{0:HH:mm}", dr1["FLDREPORTTODATE"]) + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" ></td> <td>Ballast</td><td>Loaded</td><td>Total</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Distance Steamed</td> <td>" + dr1["FLDBALLASTDISTANCESTEAMED"].ToString() + "</td><td>" + dr1["FLDLOADEDDISTANCESTEAMED"].ToString() + "</td><td>" + dr1["FLDTOTALDISTANCESTEAMED"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Steaming Time</td> <td>" + dr1["FLDBALLASTSTEAMINGTIME"].ToString() + "</td><td>" + dr1["FLDLOADEDSTEAMINGTIME"].ToString() + "</td><td>" + dr1["FLDTOTALSTEAMINGTIME"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Average Speed</td> <td>" + dr1["FLDBALLASTAVGSPEED"].ToString() + "</td><td>" + dr1["FLDLOADEDAVGSPEED"].ToString() + "</td><td>" + dr1["FLDTOTALAVGSPEED"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Average RPM</td> <td>" + dr1["FLDBALLASTAVGRPM"].ToString() + "</td><td>" + dr1["FLDLOADEDAVGRPM"].ToString() + "</td><td>" + dr1["FLDTOTALAVGRPM"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Average Slip</td> <td>" + dr1["FLDBALLASTAVGSLIP"].ToString() + "</td><td>" + dr1["FLDLOADEDAVGSLIP"].ToString() + "</td><td>" + dr1["FLDTOTALAVGSLIP"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Average FO Cons/Day</td> <td>" + dr1["FLDBALLASTAVGFOCONSUMPTIONPERDAY"].ToString() + "</td><td>" + dr1["FLDLOADEDAVGFOCONSUMPTIONPERDAY"].ToString() + "</td><td>" + dr1["FLDTOTALAVGFOCONSUMPTIONPERDAY"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >M/E Stoppage Time</td> <td>" + dr1["FLDBALLASTMESTOPPAGETIME"].ToString() + "</td><td>" + dr1["FLDLOADEDMESTOPPAGETIME"].ToString() + "</td><td>" + dr1["FLDTOTALMESTOPPAGETIME"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Average M/E FO Cons/Day</td> <td>" + dr1["FLDBALLASTAVGMEFOCONSUMPTIONPERDAY"].ToString() + "</td><td>" + dr1["FLDLOADEDAVGMEFOCONSUMPTIONPERDAY"].ToString() + "</td><td>" + dr1["FLDTOTALAVGMEFOCONSUMPTIONPERDAY"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Average A/E DO Cons/Day</td> <td>" + dr1["FLDBALLASTAVGAEDOCONSUMPTIONPERDAY"].ToString() + "</td><td>" + dr1["FLDLOADEDAVGAEDOCONSUMPTIONPERDAY"].ToString() + "</td><td>" + dr1["FLDTOTALAVGAEDOCONSUMPTIONPERDAY"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Average Boiler FO Cons/Day</td> <td>" + dr1["FLDBALLASTAVGBOILERFOCONSUMPTIONPERDAY"].ToString() + "</td><td>" + dr1["FLDLOADEDAVGBOILERFOCONSUMPTIONPERDAY"].ToString() + "</td><td>" + dr1["FLDTOTALAVGBOILERFOCONSUMPTIONPERDAY"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Average FO Cons/Day for Tank Cleaning</td> <td>" + dr1["FLDBALLASTAVGFOCONSUMPTIONPERDAYFORTANKCLEANING"].ToString() + "</td><td>" + dr1["FLDLOADEDAVGFOCONSUMPTIONPERDAYFORTANKCLEANING"].ToString() + "</td><td>" + dr1["FLDTOTALAVGFOCONSUMPTIONPERDAYFORTANKCLEANING"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Average MECC/Day</td> <td>-</td><td>-</td><td>" + dr1["FLDAVGMECCCONSUMPTIONPERDAY"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Average MECYL/Day</td> <td>-</td><td>-</td><td>" + dr1["FLDAVGMECYLCONSUMPTIONPERDAY"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Average MECYLLTBN/Day</td> <td>-</td><td>-</td><td>" + dr1["FLDTOTALAVGMECYLLTBNPERDAY"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >MECYL Specific Cons</td> <td>-</td><td>-</td><td>" + dr1["FLDMECYLSPECIFICCONSUMPTION"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >MECYLLTBN Specific Cons</td> <td>-</td><td>-</td><td>" + dr1["FLDTOTALMECYLLTBNSPECIFICCONSUMPTION"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" >Average AECC Cons/Day</td> <td>-</td><td>-</td><td>" + dr1["FLDAVGAECCCONSUMPTIONPERDAY"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("</table></html>");

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
                    document.SetPageSize(iTextSharp.text.PageSize.A4);
                    string filefullpath = "QuaterlyReport" + ".pdf";
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

   
    public void ConvertToExcel(string HTMLString)
    {
        try
        {
            if (HTMLString != "")
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=QuaterlyReport.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
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
    private void ResetQuarterly()
    {
        ViewState["QUATERLYREPORTID"] = "";

        txtCurrentDate.Text = System.DateTime.Now.ToString();
        txtballastdistancesteamed.Text = "";
        txtloadeddistancesteamed.Text = "";
        txttotaldistancesteamed.Text = "";
        txtballaststeamingtime.Text = "";
        txtloadedsteamingtime.Text = "";
        txttotalsteamingtime.Text = "";
        txttotalavgspeed.Text = "";
        txttotalavgrpm.Text = "";
        txtballastavgslip.Text = "";
        txtloadedavgslip.Text = "";
        txttotalavgslip.Text = "";
        txtballastavgfoconsumptionperday.Text = "";
        txtloadedavgfoconsumptionperday.Text = "";
        txttotalavgfoconsumptionperday.Text = "";
        txtballastmestoppagetime.Text = "";
        txtloadedmestoppagetime.Text = "";
        txttotalmestoppagetime.Text = "";
        txtballastavgmefoconsumptionperday.Text = "";
        txtloadedavgmefoconsumptionperday.Text = "";
        txttotalavgmefoconsumptionperday.Text = "";
        txtballastavgaedoconsumptionperday.Text = "";
        txtloadedavgaedoconsumptionperday.Text = "";
        txttotalavgaedoconsumptionperday.Text = "";
        txtballastavgboilerfoconsumptionperday.Text = "";
        txtloadedavgboilerfoconsumptionperday.Text = "";
        txttotalavgboilerfoconsumptionperday.Text = "";
        txtballastavgfoconsumptionperdayfortankcleaning.Text = "";
        txtloadedavgfoconsumptionperdayfortankcleaning.Text = "";
        txttotalavgfoconsumptionperdayfortankcleaning.Text = "";
        txttotalavgmecylltbnperday.Text = "";
        txttotalmecylltbnspecificconsumption.Text = "";
        txtballastavgspeed.Text = "";
        txtloadedavgspeed.Text = "";
        txtballastavgrpm.Text = "";
        txtloadedavgrpm.Text = "";
        txtavgmeccconsumptionperday.Text = "";
        txtavgmecylconsumptionperday.Text = "";
        txtmecylspecificconsumption.Text = "";
        txtavgaeccconsumptionperday.Text = "";
        ddlPeriod.SelectedValue = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
    }

    private void SaveQuarterlyReport()
    {
        try
        {
            PhoenixVesselPositionQuarterlyReport.UpdateQuarterlyReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                               General.GetNullableGuid(ViewState["QUATERLYREPORTID"].ToString()),
                                                               General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                                                               General.GetNullableDateTime(txtCurrentDate.Text),
                                                               General.GetNullableDecimal(txtballastavgspeed.Text),
                                                                General.GetNullableDecimal(txtloadedavgspeed.Text),
                                                                General.GetNullableDecimal(txtballastavgrpm.Text),
                                                                General.GetNullableDecimal(txtloadedavgrpm.Text),
                                                                General.GetNullableDecimal(txtavgmeccconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtavgmecylconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtmecylspecificconsumption.Text),
                                                                General.GetNullableDecimal(txtavgaeccconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtballastdistancesteamed.Text),
                                                                General.GetNullableDecimal(txtloadeddistancesteamed.Text),
                                                                General.GetNullableDecimal(txttotaldistancesteamed.Text),
                                                                General.GetNullableDecimal(txtballaststeamingtime.Text),
                                                                General.GetNullableDecimal(txtloadedsteamingtime.Text),
                                                                General.GetNullableDecimal(txttotalsteamingtime.Text),
                                                                General.GetNullableDecimal(txttotalavgspeed.Text),
                                                                General.GetNullableDecimal(txttotalavgrpm.Text),
                                                                General.GetNullableDecimal(txtballastavgslip.Text),
                                                                General.GetNullableDecimal(txtloadedavgslip.Text),
                                                                General.GetNullableDecimal(txttotalavgslip.Text),
                                                                General.GetNullableDecimal(txtballastavgfoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtloadedavgfoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txttotalavgfoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtballastmestoppagetime.Text),
                                                                General.GetNullableDecimal(txtloadedmestoppagetime.Text),
                                                                General.GetNullableDecimal(txttotalmestoppagetime.Text),
                                                                General.GetNullableDecimal(txtballastavgmefoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtloadedavgmefoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txttotalavgmefoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtballastavgaedoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtloadedavgaedoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txttotalavgaedoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtballastavgboilerfoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtloadedavgboilerfoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txttotalavgboilerfoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtballastavgfoconsumptionperdayfortankcleaning.Text),
                                                                General.GetNullableDecimal(txtloadedavgfoconsumptionperdayfortankcleaning.Text),
                                                                General.GetNullableDecimal(txttotalavgfoconsumptionperdayfortankcleaning.Text),
                                                                null,
                                                                General.GetNullableDecimal(txttotalavgmecylltbnperday.Text),
                                                                General.GetNullableDecimal(txttotalmecylltbnspecificconsumption.Text),
                                                                General.GetNullableDateTime(txtFromDate.Text),
                                                                General.GetNullableDateTime(txtToDate.Text)
                                                           );
            ucStatus.Text = "Quarterly Report updated ";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;          
        }

    }

    private void AddQuarterlyReport()
    {
        if (!IsValidPeriod(txtFromDate.Text, txtToDate.Text))
        {
            ucError.Visible = true;
            return;
        }
        
        Guid? quarterlyreportid=null;
        PhoenixVesselPositionQuarterlyReport.InsertQuarterlyReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                                                                General.GetNullableDateTime(txtCurrentDate.Text),
                                                                General.GetNullableDecimal(txtballastavgspeed.Text),
                                                                General.GetNullableDecimal(txtloadedavgspeed.Text),
                                                                General.GetNullableDecimal(txtballastavgrpm.Text),
                                                                General.GetNullableDecimal(txtloadedavgrpm.Text),
                                                                General.GetNullableDecimal(txtavgmeccconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtavgmecylconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtmecylspecificconsumption.Text),
                                                                General.GetNullableDecimal(txtavgaeccconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtballastdistancesteamed.Text),
                                                                General.GetNullableDecimal(txtloadeddistancesteamed.Text),
                                                                General.GetNullableDecimal(txttotaldistancesteamed.Text),
                                                                General.GetNullableDecimal(txtballaststeamingtime.Text),
                                                                General.GetNullableDecimal(txtloadedsteamingtime.Text),
                                                                General.GetNullableDecimal(txttotalsteamingtime.Text),
                                                                General.GetNullableDecimal(txttotalavgspeed.Text),
                                                                General.GetNullableDecimal(txttotalavgrpm.Text),
                                                                General.GetNullableDecimal(txtballastavgslip.Text),
                                                                General.GetNullableDecimal(txtloadedavgslip.Text),
                                                                General.GetNullableDecimal(txttotalavgslip.Text),
                                                                General.GetNullableDecimal(txtballastavgfoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtloadedavgfoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txttotalavgfoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtballastmestoppagetime.Text),
                                                                General.GetNullableDecimal(txtloadedmestoppagetime.Text),
                                                                General.GetNullableDecimal(txttotalmestoppagetime.Text),
                                                                General.GetNullableDecimal(txtballastavgmefoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtloadedavgmefoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txttotalavgmefoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtballastavgaedoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtloadedavgaedoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txttotalavgaedoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtballastavgboilerfoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtloadedavgboilerfoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txttotalavgboilerfoconsumptionperday.Text),
                                                                General.GetNullableDecimal(txtballastavgfoconsumptionperdayfortankcleaning.Text),
                                                                General.GetNullableDecimal(txtloadedavgfoconsumptionperdayfortankcleaning.Text),
                                                                General.GetNullableDecimal(txttotalavgfoconsumptionperdayfortankcleaning.Text),
                                                                null,
                                                                General.GetNullableDecimal(txttotalavgmecylltbnperday.Text),
                                                                General.GetNullableDecimal(txttotalmecylltbnspecificconsumption.Text),
                                                                General.GetNullableDateTime(txtFromDate.Text),
                                                                General.GetNullableDateTime(txtToDate.Text),
                                                                ref quarterlyreportid
                                                               );
        ViewState["QUATERLYREPORTID"] = quarterlyreportid;
        Session["QUARTERLYREPORTID"] = ViewState["QUATERLYREPORTID"].ToString();
        ucStatus.Text = "Quarterly Report added ";
        BindData();
    }

    protected void ddlPeriod_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["MONTH"] = ddlPeriod.SelectedValue;
        ViewState["YEAR"] = ddlYear.SelectedValue;
        BindData();
    }

    private bool IsValidPeriod(string fromdate, string todate)
    {
        if (fromdate == "")
        {
            ucError.ErrorMessage = "From Date is required.";
        }
        if (todate == "")
        {
            ucError.ErrorMessage = "To Date is required.";
        }
        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }
}
