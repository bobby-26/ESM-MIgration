using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
//using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Framework;

public partial class AccountsAirfareManualBillingExport2PDF : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = PhoenixIntegrationAccounts.TravelPostReport(new Guid(Request.QueryString["AgentInvoiceId"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                convertToPDF(ds);
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                      "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    public void convertToPDF(DataSet ds)
    {
        StringBuilder sbHtmlContent = new StringBuilder();

        sbHtmlContent.Append("<div align='left'>");
        sbHtmlContent.Append("<table ID='tbl1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<img src='http://" + Request.Url.Authority + Session["images"] + "/UnisonTravel_Logo.png" + "' />");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td colspan='8'>");
        sbHtmlContent.Append("<b>");
        sbHtmlContent.Append("<font style=\"font-family:Calibri (Body);font-size:20px;\">");
        sbHtmlContent.Append("Unison Universals Travel Limited");
        sbHtmlContent.Append("</b>");
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("</div>");
        sbHtmlContent.Append("<br/>");

        sbHtmlContent.Append("<table width='100%'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='7' valign='top'>");
        sbHtmlContent.Append("<table width='100%'>");
        sbHtmlContent.Append("<tr>");

        sbHtmlContent.Append("<td width='40%' valign='top' bgcolor='#FAFAD2' border='1' style=\"font-family:times new roman;font-size:10px;\">");

        sbHtmlContent.Append("<b>");
        sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:12px;\">");
        sbHtmlContent.Append("BILL TO");
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("</b>");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDCOMPANYNAME"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDCOMPANYADDRESS"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDADDRESS2"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDADDRESS3"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDADDRESS4"].ToString());
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("<td width=\"20%\" align='center' valign='middle'>");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("<td width='40%' bgcolor='#FAFAD2' border='1' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>INVOICE NO:</b>");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDREVENUEVOUCHERNUMBER"].ToString());
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<b>DATE:</b>");
        sbHtmlContent.Append(General.GetDateTimeToString(ds.Tables[1].Rows[0]["FLDINVOICEDATE"].ToString()));
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<b>ORDER:</b>");
        //sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTARGETCOPURCHASEINVOICENO"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("REF :" + ds.Tables[1].Rows[0]["FLDTARGETCOPURCHASEINVOICENO"].ToString());
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<br />");

        sbHtmlContent.Append("<table ID='tbl2' width='100%' border='1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td width=\"5%\" style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>S.NO.</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td colspan='2' width='35%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>DESCRIPTION</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='20%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>FLIGHT DATE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='20%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>BASE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='20%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>TAX</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='20%' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>TOTAL FARE (USD)</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td valign='top' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("1");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' colspan='2' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDDESCRIPTION"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(General.GetDateTimeToString(ds.Tables[1].Rows[0]["FLDDEPARTUREDATE"].ToString()));
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDBASE"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTALTAX"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td valign='top' align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTAL"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        sbHtmlContent.Append("<table ID='tbl4' width='100%' border='1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td  style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>Our Banking Details:</b>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("The Hongkong and Shanghai Bank");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("Swift Code: HSBCHKHK");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("Account Number: 801-042771-838");
        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("<td width='50px' rowspan='3'  style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<table ID='tbl3' width='100%' border='0'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='2' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>BASE FARE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>USD</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDBASE"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='2' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>TAXES AND SURCHARGES</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>USD</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTALTAX"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");


        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='2' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>AMOUNT PAYABLE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("<b>USD</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td  align='right' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTAL"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        sbHtmlContent.Append("</td>");

        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("<footer>");
        sbHtmlContent.Append("<p align=\"center\">");
        sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("Suite B, 11th Floor, Hang Seng Causeway Bay Building, 28 Yee Wo Street, Causeway Bay, Hong Kong");
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append("E-mail: travel@unisonuniversals.com");
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("</p>");
        sbHtmlContent.Append("</footer>");

        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=AirfareBilling.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter sw = new StringWriter(sbHtmlContent);
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        StringReader sr = new StringReader(sw.ToString());

        Document pdfDoc = new Document(new Rectangle(595f, 842f));
        pdfDoc.SetMargins(36f, 36f, 36f, 0f);
        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();
        Response.Write(pdfDoc);
        Response.End();
    }
}
