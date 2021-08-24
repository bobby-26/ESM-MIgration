using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.IO;
using System.Net;
using Telerik.Web.UI;
public partial class CrewTravelExceedTicketCancellation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["TravelId"] = Request.QueryString["TravelId"];
        ViewState["TicketNo"] = Request.QueryString["TicketNo"];
        btnProceed.Attributes.Add("onclick", "javascript:this.onclick=function(){ return false;};");
    }
    protected void cmdProceed_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidSelect(rblCancellation.SelectedValue))
            {
                ucError.Visible = true;
                return;
            }
            Guid iDtKey = new Guid();

            PhoenixCrewTravelQuoteLine.TicketCancellationVoucherInsert(new Guid(Request.QueryString["AGENTINVOICEID"]),
                                                                            new Guid(ViewState["TravelId"].ToString()),
                                                                           ViewState["TicketNo"].ToString(),
                                                                           PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                           Int32.Parse(rblCancellation.SelectedValue),
                                                                           ref iDtKey);
            
            string script = "javascript:fnReloadList('codehelp2');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidSelect(string option)
    {
        if (option.Trim().Equals(""))
            ucError.ErrorMessage = "Please Select Any one option";

        return (!ucError.IsError);
    }

    private void CreateReport(Guid agentInvoiceId, Guid dtkey)
    {
        DataSet ds = PhoenixCrewTravelInvoice.TravelPostReport(agentInvoiceId);
        if (ds.Tables[0].Rows.Count > 0)
        {
            AddAttachment(ds, dtkey);
        }
    }
    private string AddAttachment(DataSet ds, Guid dtkey)
    {
        FontFactory.RegisterDirectories();
        Font fontNormal = new Font(FontFactory.GetFont("Arial", 12, Font.NORMAL));

        //string path = HttpContext.Current.Request.MapPath("~/Attachments/Accounts/");

        string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts/";
        
        string filefullpath = path + dtkey + ".pdf";

        ConvertToPdf(HtmlTableDataContent(ds), filefullpath);
        return filefullpath;
    }

    private string HtmlTableDataContent(DataSet ds)
    {
        StringBuilder sbHtmlContent = new StringBuilder();

        sbHtmlContent.Append("<table ID='tbl1' width='100%' border='1'>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td width='10%'>");
        sbHtmlContent.Append("<b>BILL  TO</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='60%'>");
        sbHtmlContent.Append("");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='10%'>");
        sbHtmlContent.Append("<b>INVOICE NO</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='20%'>");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDREVENUEVOUCHERNUMBER"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='2'>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDCOMPANYNAME"].ToString());
        sbHtmlContent.Append("<br/>");
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDCOMPANYADDRESS"].ToString());
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDADDRESS2"].ToString());
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDADDRESS3"].ToString());
        sbHtmlContent.Append(ds.Tables[0].Rows[0]["FLDADDRESS4"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<b>DATE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        //sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDINVOICEDATE"].ToString());
        sbHtmlContent.Append(General.GetDateTimeToString(ds.Tables[1].Rows[0]["FLDINVOICEDATE"].ToString()));
        //sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDINVOICEDATE"].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(ds.Tables[1].Rows[0]["FLDINVOICEDATE"].ToString()) : ds.Tables[1].Rows[0]["FLDINVOICEDATE"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='2'>");
        sbHtmlContent.Append("");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<b>ORDER</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTARGETCOPURCHASEINVOICENO"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        sbHtmlContent.Append("<table ID='tbl2' width='100%' border='1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<b>S.NO.</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<b>DESCRIPTION</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<b>FLIGHT DATE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<b>BASE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<b>TAX</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("<b>TOTAL FARE (USD)</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");

        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append("1");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDDESCRIPTION"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td>");
        //sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDDEPARTUREDATE"].ToString());
        sbHtmlContent.Append(General.GetDateTimeToString(ds.Tables[1].Rows[0]["FLDDEPARTUREDATE"].ToString()));
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td align='right'>");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDBASE"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td align='right'>");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTALTAX"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td align='right'>");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTAL"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        sbHtmlContent.Append("<table ID='tbl3' width='100%' border='1'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td width='50%'>");
        sbHtmlContent.Append("");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='25%'>");
        sbHtmlContent.Append("<b>BASE FARE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='10%'>");
        sbHtmlContent.Append("<b>USD</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='15%' align='right'>");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDBASE"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");


        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td width='50%'>");
        sbHtmlContent.Append("");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='25%'>");
        sbHtmlContent.Append("<b>TAXES AND SURCHARGES</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='10%'>");
        sbHtmlContent.Append("<b>USD</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='15%' align='right'>");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTALTAX"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");


        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td width='50%'>");
        sbHtmlContent.Append("");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='25%'>");
        sbHtmlContent.Append("<b>AMOUNT PAYABLE</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='10%'>");
        sbHtmlContent.Append("<b>USD</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='15%' align='right'>");
        sbHtmlContent.Append(ds.Tables[1].Rows[0]["FLDTOTAL"].ToString());
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        return sbHtmlContent.ToString();
    }

    public void ConvertToPdf(string HTMLString, string fileLocation)
    {      
        Document document = new Document(new Rectangle(842f, 595f));
        document.SetMargins(36f, 36f, 208f, 0f);

        PdfWriter.GetInstance(document, new FileStream(fileLocation, FileMode.Create));
        document.Open();

        StyleSheet styles = new StyleSheet();

        ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);
    
        for (int k = 0; k < htmlarraylist.Count; k++)
        {
            document.Add((IElement)htmlarraylist[k]);
        }

        document.Close();
    }
}
