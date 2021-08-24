using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Xml;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using System.Collections.Generic;


public partial class VesselAccountsPortageBillPaySlip :  PhoenixBasePage
{
    string empid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            empid = Request.QueryString["empid"];
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageLink("javascript:cmdPrint_Click()", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsPortageBillPaySlip.aspx?" + Request.QueryString.ToString(), "Download pdf", "download_1.png", "EXCEL");
            MenuPBExcel.AccessRights = this.ViewState;
            MenuPBExcel.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                PhoenixVesselAccountsPortageBill.UpdateSignonoffidEarningdeduction(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                     , int.Parse(Request.QueryString["empid"].ToString()), DateTime.Parse(Request.QueryString["pbd"])
                                                                                     , int.Parse(Request.QueryString["sgid"]));
                EditContractDetails();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void EditContractDetails()
    {
        try
        {
            DataTable dt = PhoenixVesselAccountsPortageBill.EditContract(int.Parse(empid), General.GetNullableDateTime(Request.QueryString["payd"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtSeamanBook.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
                txtNationality.Text = dt.Rows[0]["FLDNATIONALITYNAME"].ToString();
                txtVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
                txtPort.Text = dt.Rows[0]["FLDSEAPORTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtDate.Text = dt.Rows[0]["FLDPAYDATE"].ToString();
                txtContractPeriod.Text = dt.Rows[0]["FLDCONTRACTTENURE"].ToString();
                txtPlusMinusPeriod.Text = dt.Rows[0]["FLDPLUSORMINUSPERIOD"].ToString();
                txtFileNo.Text = dt.Rows[0]["FLDFILENO"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {

        try
        {
            DataTable dt = PhoenixVesselAccountsPortageBill.ListVesselPortagePaySlip(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , int.Parse(Request.QueryString["empid"].ToString()), DateTime.Parse(Request.QueryString["pbd"])
                , int.Parse(Request.QueryString["sgid"]));
            if (dt.Rows.Count > 0)
            {
                txtStartDate.Text = dt.Rows[0]["FLDSTARTDATE"].ToString();
                txtEndDate.Text = dt.Rows[0]["FLDENDDATE"].ToString();
                gvPB.DataSource = dt;
                gvPB.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvPB);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    decimal ear = 0, ded = 0, bf = 0;
    protected void gvPB_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ear = 0; ded = 0;
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            decimal.TryParse(drv["FLDTOTEAMOUNT"].ToString(), out ear);
            decimal.TryParse(drv["FLDTOTDAMOUNT"].ToString(), out ded);
            decimal.TryParse(drv["FLDBFAMOUNT"].ToString(), out bf);
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = ear.ToString();
            e.Row.Cells[3].Text = ded.ToString();
            GridViewRow footer = e.Row;
            int cellcnt = footer.Cells.Count;

            GridViewRow newrow = new GridViewRow(footer.RowIndex + 1, -1, footer.RowType, footer.RowState);
            TableCell cell = null;
            for (int i = 0; i <= cellcnt - 1; i++)
            {
                cell = new TableCell();
                cell.ApplyStyle(((GridView)sender).Columns[i].FooterStyle);
                newrow.Cells.Add(cell);
            }
            newrow.Cells[2].Text = "Net Salary";
            newrow.Cells[3].Text = (ear - ded).ToString();
            Table tbl = (Table)((GridView)sender).Controls[0];
            tbl.Rows.Add(newrow);

            GridViewRow newbfrow = new GridViewRow(footer.RowIndex + 1, -1, footer.RowType, footer.RowState);
            TableCell cellbf = null;
            for (int i = 0; i <= cellcnt - 1; i++)
            {
                cellbf = new TableCell();
                cellbf.ApplyStyle(((GridView)sender).Columns[i].FooterStyle);
                newbfrow.Cells.Add(cellbf);
            }
            newbfrow.Cells[2].Text = "Balance Brought Forward";
            newbfrow.Cells[3].Text = bf.ToString();
            Table tblbf = (Table)((GridView)sender).Controls[0];
            tblbf.Rows.Add(newbfrow);

            GridViewRow newfbrow = new GridViewRow(footer.RowIndex + 1, -1, footer.RowType, footer.RowState);
            TableCell cellfb = null;
            for (int i = 0; i <= cellcnt - 1; i++)
            {
                cellfb = new TableCell();
                cellfb.ApplyStyle(((GridView)sender).Columns[i].FooterStyle);
                newfbrow.Cells.Add(cellfb);
            }
            newfbrow.Cells[2].Text = "Final Balance";
            newfbrow.Cells[3].Text = ((ear - ded) + bf).ToString();
            Table tblfb = (Table)((GridView)sender).Controls[0];
            tblbf.Rows.Add(newfbrow);
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPBExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ConvertToPdf(HtmlTableDataContent(General.GetNullableInteger(Request.QueryString["empid"].ToString()), DateTime.Parse(txtEndDate.Text), int.Parse(Request.QueryString["sgid"]), General.GetNullableDateTime(Request.QueryString["payd"].ToString()), txtFileNo.Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    string companyname = string.Empty; string agentDesc = string.Empty;
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
                    // string path = HttpContext.Current.Request.MapPath("~/Attachments/VesselAccounts/");
                    // string month = DateTime.Parse(txtEndDate.Text).ToString("m");
                    string year = DateTime.Parse(txtEndDate.Text).ToString("y");
                    string filefullpath = year.Replace(" ", "_") + "_Payslip_for_" + txtFileNo.Text + ".pdf";
                    PdfWriter.GetInstance(document, ms);
                    document.Open();
                    string imageURL = "http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png";


                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 530f;
                    table.LockedWidth = true;

                    //PdfPTable table2 = new PdfPTable(2);
                    //float[] WIDTHS = new float[] { 8f, 85f };
                    //table2.SetWidths(WIDTHS);
                    PdfPTable table2 = new PdfPTable(1);
                    float[] WIDTHS = new float[] { 530f };
                    table2.SetWidths(WIDTHS);
                    ////logo
                    //PdfPCell cell2 = new PdfPCell(iTextSharp.text.Image.GetInstance(@imageURL));
                    //cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //table2.AddCell(cell2);

                    //title
                    //PdfPCell cell2 = new PdfPCell(new iTextSharp.text.Phrase(companyname, new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, iTextSharp.text.Font.BOLD)));
                    //cell2.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    //cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //table2.AddCell(cell2);
                    //cell2 = new PdfPCell(new iTextSharp.text.Phrase(agentDesc, new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, iTextSharp.text.Font.BOLD)));
                    //cell2.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    //cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //table2.AddCell(cell2);

                    //PdfPCell cell = new PdfPCell(table2);
                    //cell.Border = iTextSharp.text.Rectangle.BOX;
                    //table.AddCell(cell);
                    document.Add(table);
                    StyleSheet styles = new StyleSheet();
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
                    // HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private string HtmlTableDataContent(int? emp, DateTime todate, int signonoffid, DateTime? paydate, string fileno)
    {

        System.IO.StringWriter stringwriter = new System.IO.StringWriter();

        int employeeid = emp ?? default(int);
        DataTable dt = PhoenixVesselAccountsPortageBill.EditContract(employeeid, paydate, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        StringBuilder sbHtmlContent = new StringBuilder();
        if (dt.Rows.Count > 0)
        {
            string month = todate.ToString("m");
            string year = todate.ToString("y");
            companyname = dt.Rows[0]["FLDCOMPANY"].ToString() == "" ? dt.Rows[0]["FLDCONTRACTINGCOMPANY"].ToString() : dt.Rows[0]["FLDCOMPANY"].ToString();
            agentDesc = dt.Rows[0]["FLDAGENTDESC"].ToString();
            //sbHtmlContent.Append("<div align='left' >");
            //sbHtmlContent.Append("<img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
            //sbHtmlContent.Append("</div>");

            //sbHtmlContent.Append("<tr>");
            //          sbHtmlContent.Append("<td colspan='2' align='center'>");
            //          sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:12px;\">");
            //          sbHtmlContent.Append("<font><b>" + dt.Rows[0]["FLDCONTRACTINGCOMPANY"].ToString() + "</b></font>");
            //          sbHtmlContent.Append("</td>");
            //          sbHtmlContent.Append("</tr>");

            //  sbHtmlContent.Append("<div  style='border: solid; '>");
            sbHtmlContent.Append("<table  width=\"100 %\"border='1'style=\" border-collapse:collapse;\">");
            sbHtmlContent.Append("<tr>");//colspan='2'
            sbHtmlContent.Append("<td>");
            sbHtmlContent.Append("<table border='0' ID ='tbl1'>");
            sbHtmlContent.Append("<tr>");//colspan='2'
            sbHtmlContent.Append("<td  align='center'>");
            sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:10px;\">");
            sbHtmlContent.Append("<font><b>"  + companyname + "</b></font>");
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("</tr>");
            sbHtmlContent.Append("<tr>");//colspan='2'
            sbHtmlContent.Append("<td align='center'>");
            sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:10px;\">");
            sbHtmlContent.Append("<font><b>" + agentDesc + "</b></font>");
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("</tr>");
             sbHtmlContent.Append("</table>");
            sbHtmlContent.Append("<tr>");//colspan='2'
            sbHtmlContent.Append("<td >");  

            sbHtmlContent.Append("<table   width=\"100 %\"border='0' style=\" border-collapse:collapse;\" >");
            sbHtmlContent.Append("<tr>");//colspan='2'
            sbHtmlContent.Append("<td align='center'>");
            sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:10px;\">");
            sbHtmlContent.Append("<font><b>" + "Payslip for the month of " + year + "</b></font>");
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("</tr>");   sbHtmlContent.Append("</table>");

             sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("</tr>");
            sbHtmlContent.Append("<tr>");//colspan='2'
            sbHtmlContent.Append("<td >");

            sbHtmlContent.Append("<table   width=\"100 %\"border='0' style=\" border-collapse:collapse;\" >");
            sbHtmlContent.Append("<tr>");
            sbHtmlContent.Append("<td>");
            sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:8px;\">");
            sbHtmlContent.Append("FILE NO              " + ": " + fileno);
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("</tr>");

            sbHtmlContent.Append("<tr>");
            sbHtmlContent.Append("<td>");
            sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:8px;\">");
            sbHtmlContent.Append("NAME                  " + ": " + dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString());
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("</tr>");

            sbHtmlContent.Append("<tr>");
            sbHtmlContent.Append("<td>");
            sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:8px;\">");
            sbHtmlContent.Append("RANK                  " + ": " + dt.Rows[0]["FLDRANKNAME"].ToString());
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("</tr>");

            sbHtmlContent.Append("<tr>");
            sbHtmlContent.Append("<td>");
            sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:8px;\">");
            sbHtmlContent.Append("VESSEL              " + ": " + dt.Rows[0]["FLDVESSELNAME"].ToString());
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("</tr>");

            DataTable dt1 = PhoenixVesselAccountsPortageBill.ListVesselPortagePaySlip(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , employeeid, todate
                    , signonoffid);
            String periodstartdate = dt1.Rows[0]["FLDSTARTDATE"].ToString().Substring(0, dt1.Rows[0]["FLDSTARTDATE"].ToString().Length - 8);
            String periodenddate = dt1.Rows[0]["FLDENDDATE"].ToString().Substring(0, dt1.Rows[0]["FLDENDDATE"].ToString().Length - 8);

            sbHtmlContent.Append("<td>");
            sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:8px;\">");
            sbHtmlContent.Append("PERIOD OF        " + ": " + General.GetDateTimeToString(dt1.Rows[0]["FLDSTARTDATE"].ToString()) + "  to  " + General.GetDateTimeToString(dt1.Rows[0]["FLDENDDATE"].ToString()));
            sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("</tr>");
           sbHtmlContent.Append("</table>");
             sbHtmlContent.Append("</td>");
            sbHtmlContent.Append("</tr>"); 
            if (dt1.Rows.Count > 0)
            {
                decimal? netamount;
                decimal? finalbalance;
                //string payslip;
                //string payslipamount;
                //string paysliptotal;
                //payslip = "";
                int j = 0;
                // payslipamount = "";
                sbHtmlContent.Append("<tr>");
                sbHtmlContent.Append("<td>");
                sbHtmlContent.Append("<table frame=\"box\" rules=\"none\" ID='tbl2' style=\"border:2px\"  border=\"0.5\">");
                sbHtmlContent.Append("<tr>");
                sbHtmlContent.Append("<th align='center'>");
                sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:8px;  \">");
                sbHtmlContent.Append("<b>Earnings</b>");
                sbHtmlContent.Append("</th>");

                sbHtmlContent.Append("<th align='center'>");
                sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:8px; \">");
                sbHtmlContent.Append("<b>Amount(USD)</b>");
                sbHtmlContent.Append("</th>");


                sbHtmlContent.Append("<th align='center'>");
                sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:8px; \">");
                sbHtmlContent.Append("<b>Deductions</b>");
                sbHtmlContent.Append("</th>");
                sbHtmlContent.Append("<th align='center'>");
                sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:8px; \">");
                sbHtmlContent.Append("<b>Amount(USD)</b>");
                sbHtmlContent.Append("</th>");
                sbHtmlContent.Append("</tr>");

                for (j = 0; j < dt1.Rows.Count; j++)
                {
                    sbHtmlContent.Append("<tr>");
                    sbHtmlContent.Append("<td>");
                    sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:8px;color:Black;\">");
                    sbHtmlContent.Append(dt1.Rows[j]["FLDECOMPONENTNAME"].ToString());
                    sbHtmlContent.Append("</td>");
                    sbHtmlContent.Append("<td align='right'>");
                    sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:8px;\">");
                    sbHtmlContent.Append(dt1.Rows[j]["FLDEAMOUNT"].ToString());
                    sbHtmlContent.Append("</td>");

                    sbHtmlContent.Append("<td>");
                    sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:8px;\">");
                    sbHtmlContent.Append(dt1.Rows[j]["FLDDCOMPONENTNAME"].ToString());
                    sbHtmlContent.Append("</td>");
                    sbHtmlContent.Append("<td align='right'>");
                    sbHtmlContent.Append("<font style=\"font-family:Arial;font-size:8px;\">");
                    sbHtmlContent.Append(dt1.Rows[j]["FLDDAMOUNT"].ToString());
                    sbHtmlContent.Append("</td>");
                    sbHtmlContent.Append("</tr>");
                }
                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                int y = DateTime.Parse(dt1.Rows[0]["FLDENDDATE"].ToString()).Year;
                int m = DateTime.Parse(dt1.Rows[0]["FLDENDDATE"].ToString()).Month;
                string strMonthNameyear = mfi.GetMonthName(m).ToString();

                netamount = (General.GetNullableDecimal(dt1.Rows[0]["FLDTOTEAMOUNT"].ToString()) - General.GetNullableDecimal(dt1.Rows[0]["FLDTOTDAMOUNT"].ToString() == "" ? "0" : dt1.Rows[0]["FLDTOTDAMOUNT"].ToString()));
                finalbalance = (General.GetNullableDecimal(dt1.Rows[0]["FLDTOTEAMOUNT"].ToString()) - General.GetNullableDecimal(dt1.Rows[0]["FLDTOTDAMOUNT"].ToString() == "" ? "0" : dt1.Rows[0]["FLDTOTDAMOUNT"].ToString())) + General.GetNullableDecimal(dt1.Rows[0]["FLDBFAMOUNT"].ToString());

                sbHtmlContent.Append("<tr>");
                sbHtmlContent.Append("<td>");
                sbHtmlContent.Append("<font style=\"font-family:Calibri  (Body);font-size:8px;\">");
                sbHtmlContent.Append("<b>Total Earnings</b>");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("<td align='right' >");
                sbHtmlContent.Append("<font style=\"font-family:Calibri  (Body);font-size:8px;\">");
                sbHtmlContent.Append("<b>" + dt1.Rows[0]["FLDTOTEAMOUNT"].ToString() + "</b>");
                sbHtmlContent.Append("</td>");

                sbHtmlContent.Append("<td>");
                sbHtmlContent.Append("<font style=\"font-family:Calibri  (Body);font-size:8px;\">");
                sbHtmlContent.Append("<b>Total Deductions</b>");
                sbHtmlContent.Append("</td>");


                sbHtmlContent.Append("<td align='right'>");
                sbHtmlContent.Append("<font style=\"font-family:Calibri  (Body);font-size:8px;\" >");
                sbHtmlContent.Append("<b>" + dt1.Rows[0]["FLDTOTDAMOUNT"].ToString() + "</b>");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("</tr>");

                sbHtmlContent.Append("<tr>");
                sbHtmlContent.Append("<td>");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("<td>");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("<td>");
                sbHtmlContent.Append("<font style=\"font-family:Calibri  (Body);font-size:8px;\">");
                sbHtmlContent.Append("<b>Net Salary</b>");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("<td align='right' >");
                sbHtmlContent.Append("<font style=\"font-family:Calibri  (Body);font-size:8px;\">");
                sbHtmlContent.Append("<b>" + netamount + "</b>");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("</tr>");

                sbHtmlContent.Append("<tr>");
                sbHtmlContent.Append("<td >");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("<td >");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("<td >");
                sbHtmlContent.Append("<font style=\"font-family:Calibri  (Body);font-size:8px;\">");
                sbHtmlContent.Append("<b>Balance Brought Forward</b>");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("<td align='right' >");
                sbHtmlContent.Append("<font style=\"font-family:Calibri  (Body);font-size:8px;\">");
                sbHtmlContent.Append("<b>" + dt1.Rows[0]["FLDBFAMOUNT"].ToString() + "</b>");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("</tr>");

                sbHtmlContent.Append("<tr>");
                sbHtmlContent.Append("<td>");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("<td>");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("<td>");
                sbHtmlContent.Append("<font style=\"font-family:Calibri  (Body);font-size:8px;\">");
                sbHtmlContent.Append("<b>Final Balance</b>");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("<td align='right'>");
                sbHtmlContent.Append("<font style=\"font-family:Calibri  (Body);font-size:8px;\">");
                sbHtmlContent.Append("<b>" + finalbalance + "</b>");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("</tr>");
                sbHtmlContent.Append("</table>");
                //sbHtmlContent.Append("<br>");
                sbHtmlContent.Append("</td>");
                sbHtmlContent.Append("</tr>");
                //sbHtmlContent.Append("<table ID='tbl3'>");
                sbHtmlContent.Append("<tr>");
                sbHtmlContent.Append("<td>");
                sbHtmlContent.Append("<font style=\"font-family:Calibri  (Body);font-size:8px;\">");
                sbHtmlContent.Append("<I>This is a system generated pay slip. Hence, signature is not needed</I>");
                sbHtmlContent.Append("</td>"); sbHtmlContent.Append("</tr>");
             //   sbHtmlContent.Append("</table>"); sbHtmlContent.Append("</td>"); sbHtmlContent.Append("</tr>");
                sbHtmlContent.Append("</table>");
                // sbHtmlContent.Append("</div>");

            }
        }
        return sbHtmlContent.ToString();
    }

}
