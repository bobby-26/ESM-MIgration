using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text;
using System.Net;
using System.Collections;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;


public partial class AccountsSoaCheckingPdfReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["debitnotereference"] = Request.QueryString["debitnotereference"].ToString();
                ViewState["debitnotereferenceid"] = Request.QueryString["debitnotereferenceid"].ToString();
                ViewState["accountid"] = Request.QueryString["accountid"].ToString();
                ViewState["Ownerid"] = Request.QueryString["Ownerid"].ToString();
                ViewState["dtkey"] = Request.QueryString["dtkey"].ToString();
                ViewState["description"] = Request.QueryString["description"].ToString();
                ViewState["year"] = Request.QueryString["year"].ToString();
                ViewState["month"] = Request.QueryString["month"].ToString();
                ViewState["pdfGen"] = Request.QueryString["pdfGen"].ToString();
                ViewState["Type"] = Request.QueryString["Type"].ToString();
                ViewState["VOUCHERDTKEY"] = "";
                ViewState["Generated"] = null;
            }

            if (ViewState["pdfGen"] != null)
            {
                if (ViewState["pdfGen"].ToString() == "1")
                {
                    Bind();
                    PhoenixAccountsSoaChecking.SoaCheckingReportHistoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                General.GetNullableGuid(ViewState["debitnotereferenceid"].ToString()),
                                                                                0,
                                                                                General.GetNullableInteger(ViewState["accountid"].ToString()),
                                                                                General.GetNullableGuid(ViewState["dtkey"].ToString()));
                }
            }
            DataSet ds = PhoenixAccountsSoaChecking.SoaCheckingReportHistoryEdit(General.GetNullableInteger(ViewState["Type"].ToString()), General.GetNullableGuid(ViewState["debitnotereferenceid"].ToString()));
            string src, filepath;
            filepath = HttpContext.Current.Request.MapPath("~/Attachments/Accounts/" + ViewState["dtkey"].ToString() + ".pdf");
            // src = Session["sitepath"] + "/attachments/Accounts/" + ViewState["dtkey"].ToString() + ".pdf";
            src = "..common/download.aspx?dtkey=" + ViewState["dtkey"].ToString();
            if (File.Exists(filepath) && ds.Tables[0].Rows.Count > 0)
            {
                ifMoreInfo.Attributes["src"] = src;
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Please Generate Pdf First..";
                ifMoreInfo.Attributes["src"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public class lstCol
    {
        public string FilePath { get; set; }
        public string Flag { get; set; }
    }

    public class lstPdfFiles1
    {
        public string FilePath { get; set; }
        public string Flag { get; set; }
    }

    private void Bind()
    {
        FontFactory.RegisterDirectories();
        Font fontNormal = new Font(FontFactory.GetFont("Arial", 12, Font.NORMAL));

        List<lstCol> lstPdfFiles = new List<lstCol>();

        List<lstPdfFiles1> lstPdf = new List<lstPdfFiles1>();

        string[] alColumns = { "FLDVOUCHERDATE", "FLDVOUCHERROW", "FLDLONGDESCRIPTION", "FLDDEBIT", "FLDCREDIT", "FLDAMOUNT" };
        string[] alCaptions = { "DATE", "REF. NO.", "PARTICULARS", "DEBIT", "CREDIT", "BALANCE" };

        DataTable DtBL = PhoenixAccountsSoaChecking.SoaCheckingBudgetList(int.Parse(ViewState["accountid"].ToString()), ViewState["debitnotereference"].ToString());

        if (DtBL.Rows.Count > 0)
        {
            foreach (DataRow DrBL in DtBL.Rows) // Loop over the rows.
            {
                DataTable DtVL = PhoenixAccountsSoaChecking.SoaCheckingVouchersList(ViewState["debitnotereference"].ToString(), int.Parse(ViewState["Ownerid"].ToString()), General.GetNullableString(DrBL["FLDBUDGETID"].ToString()), int.Parse(ViewState["accountid"].ToString()));
                ViewState["VOUCHERDTKEY"] = "";
                string Gid = Guid.NewGuid().ToString();

                if (DtVL.Rows.Count > 0)
                {
                    ConvertToPdf(HtmlTableDataContent(alColumns, alCaptions, DtVL), HttpContext.Current.Request.MapPath("~/Attachments/Accounts/" + Gid + ".pdf"));

                    AddHeaderContentInPdf(HtmlTableHeaderContent(alColumns, alCaptions, DtVL), HttpContext.Current.Request.MapPath("~/Attachments/Accounts/") + Gid + "");

                    lstPdfFiles.Add(new lstCol { FilePath = HttpContext.Current.Request.MapPath("~/Attachments/Accounts/") + Gid + ".pdf", Flag = "0" });
                    lstPdfFiles.Add(new lstCol { FilePath = HttpContext.Current.Request.MapPath("~/Attachments/Accounts/") + Gid + "_Html.pdf", Flag = "1" });

                    FileInfo fi;
                    string strFilePath;
                    foreach (DataRow DrVL in DtVL.Rows) // Loop over the rows.
                    {

                        // Voucher Level Attachments.. 
                        if (ViewState["VOUCHERDTKEY"].ToString() != DrVL["FLDVOUCHERDTKEY"].ToString())
                        {
                            DataTable DtVLFA = PhoenixCommonFileAttachment.AttachmentList(new Guid(DrVL["FLDVOUCHERDTKEY"].ToString()), "");
                            foreach (DataRow DrVLFA in DtVLFA.Rows) // Loop over the rows.
                            {
                                strFilePath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrVLFA["FLDFILEPATH"].ToString();
                                fi = new FileInfo(strFilePath);
                                if (fi.Extension.ToUpper() == ".PDF")
                                    lstPdfFiles.Add(new lstCol { FilePath = strFilePath, Flag = "2" });
                            }
                        }
                        ViewState["VOUCHERDTKEY"] = DrVL["FLDVOUCHERDTKEY"].ToString();

                        // Voucher Line Item Level Attachments..
                        DataTable DtFA = PhoenixCommonFileAttachment.AttachmentList(new Guid(DrVL["FLDDTKEY"].ToString()), "");
                        foreach (DataRow DrFA in DtFA.Rows) // Loop over the rows.
                        {
                            strFilePath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrFA["FLDFILEPATH"].ToString();
                            fi = new FileInfo(strFilePath);
                            if (fi.Extension.ToUpper() == ".PDF")
                                lstPdfFiles.Add(new lstCol { FilePath = strFilePath, Flag = "2" });
                        }
                    }
                }
            }
        }


        for (int index = 0; index < lstPdfFiles.Count; index++)
        {
            lstCol List = lstPdfFiles[index];
            if (List.Flag == "0")
            {
                if (File.Exists(List.FilePath))
                {
                    File.Delete(List.FilePath);
                }
            }

            if (List.Flag == "1")
                lstPdf.Add(new lstPdfFiles1 { FilePath = List.FilePath.Replace("_Html", "_PageNo"), Flag = "0" });
            else
                lstPdf.Add(new lstPdfFiles1 { FilePath = List.FilePath, Flag = "1" });
        }

        AddPageNoInPdf(lstPdfFiles);

        CombineAndSavePdf(HttpContext.Current.Request.MapPath("~/Attachments/Accounts/" + ViewState["dtkey"].ToString() + ".pdf"), lstPdf);

        for (int index = 0; index < lstPdf.Count; index++)
        {
            lstPdfFiles1 List = lstPdf[index];
            if (List.Flag != "1")
            {
                if (File.Exists(List.FilePath))
                    File.Delete(List.FilePath);
            }
        }

        for (int index = 0; index < lstPdfFiles.Count; index++)
        {
            lstCol List = lstPdfFiles[index];
            if (List.Flag != "2")
            {
                if (File.Exists(List.FilePath))
                    File.Delete(List.FilePath);
            }
        }
    }

    private string HtmlTableHeaderContent(string[] alColumns, string[] alCaptions, DataTable dt)
    {
        string StrBudgetDescription = "", StrFromDate = "", StrToDate = "", StrMonth = "";
        string Strcompany = "", Strcompanyadd1 = "", Strcompanyadd2 = "", Strcompanycountry = "", Strcompanypostal = "";

        if (ViewState["year"].ToString() != "" && ViewState["month"].ToString() != "")
        {
            StrMonth = ViewState["month"].ToString();
            if (StrMonth.Length == 1)
                StrMonth = "0" + StrMonth;

            StrFromDate = "01/" + StrMonth + "/" + ViewState["year"].ToString();
            StrToDate = DateTime.DaysInMonth(int.Parse(ViewState["year"].ToString()), int.Parse(ViewState["month"].ToString())) + "/" + StrMonth + "/" + ViewState["year"].ToString();
        }

        if (dt.Rows.Count > 0)
        {
            DataRow dfrow = dt.Rows[0];
            StrBudgetDescription = dfrow["FLDOWNERBUDGETCODE"].ToString() + " " + dfrow["FLDDESCRIPTION"].ToString();
            Strcompany = dfrow["FLDCOMPANY"].ToString();
            Strcompanyadd1 = dfrow["COMPANYADDR1"].ToString();
            Strcompanyadd2 = dfrow["COMPANYADDR2"].ToString();
            Strcompanycountry = dfrow["COMPANYCOUNTRY"].ToString();
            Strcompanypostal = dfrow["COMPANYPOSTAL"].ToString();
        }

        StringBuilder sbHtmlContent = new StringBuilder();
        sbHtmlContent.Append("<table width='100%'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='7' valign='top'>");
        sbHtmlContent.Append("<table width='100%'>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td width='25%' valign='top' bgcolor='#FAFAD2' border='1' style=\"font-family:times new roman;font-size:10px;\">");

        sbHtmlContent.Append("&nbsp;" + ViewState["description"].ToString());

        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("&nbsp;<b>" + StrBudgetDescription + "</b>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td align='center' valign='middle'>");
        sbHtmlContent.Append("<b>");
        sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:12px;\">");
        sbHtmlContent.Append("STATEMENT OF ACCOUNT");
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("</b>");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append(ViewState["debitnotereference"].ToString());
        sbHtmlContent.Append("</font>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td width='25%' bgcolor='#FAFAD2' border='1' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("&nbsp;PREPARED BY:");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("&nbsp;" + Strcompany + "");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("&nbsp;" + Strcompanyadd1 + "");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("&nbsp;" + Strcompanyadd2 + "");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("&nbsp;" + Strcompanycountry + " " + Strcompanypostal + "");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("SUMMARY SHEET PERIOD FROM:                   " + StrFromDate);
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("   To   " + StrToDate);
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("CURRENCY : USD");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td align='center' bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
        sbHtmlContent.Append("DATE");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td align='center' bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
        sbHtmlContent.Append("REF. NO.");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
        sbHtmlContent.Append("PARTICULARS");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td align='right' bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
        sbHtmlContent.Append("DEBIT");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td align='right' bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
        sbHtmlContent.Append("CREDIT");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td align='right' bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
        sbHtmlContent.Append("BALANCE");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        return sbHtmlContent.ToString();
    }


    private string HtmlTableDataContent(string[] alColumns, string[] alCaptions, DataTable dt)
    {
        StringBuilder sbHtmlContent = new StringBuilder();

        decimal TotalAmt = 0, DebitAmt = 0, CreditAmt = 0;

        sbHtmlContent.Append("<table width='100%'>");

        foreach (DataRow dr in dt.Rows)
        {
            sbHtmlContent.Append("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {

                if (alColumns[i] == "FLDDEBIT")
                {
                    DebitAmt = DebitAmt + Convert.ToDecimal(dr[alColumns[i]]);

                    sbHtmlContent.Append("<td align='right' border='1' style=\"font-family:times new roman;font-size:9px;\">");
                    sbHtmlContent.Append(String.Format("{0:#,##0.00}", dr[alColumns[i]]));
                    sbHtmlContent.Append("</td>");
                }
                else if (alColumns[i] == "FLDCREDIT")
                {
                    CreditAmt = CreditAmt + Convert.ToDecimal(dr[alColumns[i]]);

                    sbHtmlContent.Append("<td align='right' border='1' style=\"font-family:times new roman;font-size:9px;\">");
                    sbHtmlContent.Append(String.Format("{0:#,##0.00}", dr[alColumns[i]]));
                    sbHtmlContent.Append("</td>");
                }
                else if (alColumns[i] == "FLDAMOUNT")
                {
                    TotalAmt = DebitAmt - CreditAmt;
                    sbHtmlContent.Append("<td align='right' border='1' style=\"font-family:times new roman;font-size:9px;\">");
                    sbHtmlContent.Append((TotalAmt < 0) ? "(" + String.Format("{0:#,##0.00}", -TotalAmt) + ")" : String.Format("{0:#,##0.00}", TotalAmt));
                    sbHtmlContent.Append("</td>");
                }
                else if (alColumns[i] == "FLDVOUCHERDATE")
                {
                    sbHtmlContent.Append("<td align='center' border='1' style=\"font-family:times new roman;font-size:9px;\">");
                    sbHtmlContent.Append(dr[alColumns[i]]);
                    sbHtmlContent.Append("</td>");
                }
                else if (alColumns[i] == "FLDLONGDESCRIPTION")
                {
                    sbHtmlContent.Append("<td align='left' border='1' colspan='2' style=\"font-family:times new roman;font-size:9px;\">");
                    sbHtmlContent.Append(dr[alColumns[i]]);
                    sbHtmlContent.Append("</td>");
                }
                else
                {
                    sbHtmlContent.Append("<td align='left' border='1' style=\"font-family:times new roman;font-size:9px;\">");
                    sbHtmlContent.Append(dr[alColumns[i]]);
                    sbHtmlContent.Append("</td>");
                }
            }
            sbHtmlContent.Append("</tr>");
        }

        sbHtmlContent.Append("<td colspan='5' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("&nbsp;&nbsp;AMOUNT DUE IN LAST FIGURE IN THE COLUMN");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td align='left' bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
        sbHtmlContent.Append("TOTAL");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("<td align='right' border='1' style=\"font-family:times new roman;font-size:9px;\">");
        sbHtmlContent.Append((TotalAmt < 0) ? "(" + String.Format("{0:#,##0.00}", -TotalAmt) + ")" : String.Format("{0:#,##0.00}", TotalAmt));
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='7' style=\"font-family:times new roman;font-size:10px;\">");
        sbHtmlContent.Append("&nbsp;&nbsp;NOTE:");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("<tr>");
        sbHtmlContent.Append("<td colspan='7' style=\"font-family:times new roman;font-size:8px;\">");
        sbHtmlContent.Append("&nbsp;&nbsp;&nbsp;For small sundry charges such as photocopying costs and postage and for regular monthly charges which are per tariff of agreement we will not prepare");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("&nbsp;&nbsp;&nbsp;seperate invoice as details are provides in the narrative section of the attached statement.");
        sbHtmlContent.Append("</td>");
        sbHtmlContent.Append("</tr>");
        sbHtmlContent.Append("</table>");

        return sbHtmlContent.ToString();
    }


    //HTMLString = Pass your Html , fileLocation = File Store Location
    public void ConvertToPdf(string HTMLString, string fileLocation)
    {
        //Document document = new Document(PageSize.A4, 50, 50, 25, 25);
        Document document = new Document(new Rectangle(842f, 595f));
        document.SetMargins(36f, 36f, 208f, 0f);

        PdfWriter.GetInstance(document, new FileStream(fileLocation, FileMode.Create));
        document.Open();

        StyleSheet styles = new StyleSheet();

        ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);
        //List<IElement> htmlarraylist = HTMLWorker.ParseToList(new StringReader(HTMLString), null);

        for (int k = 0; k < htmlarraylist.Count; k++)
        {
            document.Add((IElement)htmlarraylist[k]);
        }

        document.Close();
    }

    public void AddHeaderContentInPdf(string HTMLString, string PdfFilePath)
    {
        int TotalPageCount;
        string inputfilefullpath = PdfFilePath + ".pdf"; // Input.pdf it has to be there in the location
        string outputfilefullpath = PdfFilePath + "_Html.pdf"; // Output.pdf will create in the location

        PdfReader reader = new PdfReader(inputfilefullpath);
        TotalPageCount = reader.NumberOfPages;
        Rectangle size = reader.GetPageSizeWithRotation(TotalPageCount);
        Document document = new Document(new Rectangle(842f, 595f));
        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputfilefullpath, FileMode.Create, FileAccess.Write)); //Ouput.pdf it will create..

        document.Open();

        for (int iCurtentpageNum = 1; iCurtentpageNum <= TotalPageCount; iCurtentpageNum++)
        {
            PdfContentByte cb = writer.DirectContent;
            document.NewPage();

            StyleSheet styles = new StyleSheet();

            ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);
            //List<IElement> htmlarraylist = HTMLWorker.ParseToList(new StringReader(HTMLString), null);

            for (int k = 0; k < htmlarraylist.Count; k++)
            {
                document.Add((IElement)htmlarraylist[k]);
            }

            PdfImportedPage page = writer.GetImportedPage(reader, iCurtentpageNum);
            cb.AddTemplate(page, 0, 0);
        }

        document.Close();
    }
    private void AddPageNoInPdf(List<lstCol> lstPdfFiles)
    {
        int TPageCount = 0;
        for (int index = 0; index < lstPdfFiles.Count; index++)
        {
            lstCol List = lstPdfFiles[index];
            string file = List.FilePath;
            if (List.Flag == "1")
            {
                PdfReader reader = new PdfReader(file);
                TPageCount = TPageCount + reader.NumberOfPages;
            }
        }

        int TotalPageCount;
        int PageNo = 1;
        for (int index = 0; index < lstPdfFiles.Count; index++)
        {
            lstCol List = lstPdfFiles[index];
            string ifile = List.FilePath;
            if (List.Flag == "1")
            {
                string ofile = ifile.Replace("_Html", "_PageNo");
                PdfReader reader = new PdfReader(ifile);
                TotalPageCount = reader.NumberOfPages;
                Rectangle size = reader.GetPageSizeWithRotation(TotalPageCount);
                Document document = new Document(new Rectangle(842f, 595f));
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(ofile, FileMode.Create, FileAccess.Write)); //Ouput.pdf it will create..

                document.Open();

                for (int iCurtentpageNum = 1; iCurtentpageNum <= TotalPageCount; iCurtentpageNum++)
                {
                    PdfContentByte cb = writer.DirectContent;
                    document.NewPage();

                    StyleSheet styles = new StyleSheet();

                    ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader("<table width='100%'><tr><td></td><td></td><td align='right' border='0' style=\"font-family:times new roman;font-size:10px;\"><br/><br/><br/><br/><br/><br/><br/><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Page No: " + PageNo + " of " + TPageCount + "</td> </tr></table>"), styles);
                    PageNo = PageNo + 1;

                    for (int k = 0; k < htmlarraylist.Count; k++)
                    {
                        document.Add((IElement)htmlarraylist[k]);
                    }

                    PdfImportedPage page = writer.GetImportedPage(reader, iCurtentpageNum);
                    cb.AddTemplate(page, 0, 0);
                }

                document.Close();
            }
        }
    }
    private void CombineAndSavePdf(string savePath, List<lstPdfFiles1> lstPdfFiles)
    {
        using (Stream outputPdfStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            Document document = new Document();
            PdfSmartCopy copy = new PdfSmartCopy(document, outputPdfStream);
            document.Open();
            PdfReader reader;
            int totalPageCnt;
            PdfStamper stamper;
            string[] fieldNames;
            for (int index = 0; index < lstPdfFiles.Count; index++)
            {
                lstPdfFiles1 List = lstPdfFiles[index];
                string file = List.FilePath;
                if (File.Exists(file))
                {
                    reader = new PdfReader(file);
                    totalPageCnt = reader.NumberOfPages;
                    for (int pageCnt = 0; pageCnt < totalPageCnt;)
                    {
                        //have to create a new reader for each page or PdfStamper will throw error
                        reader = new PdfReader(file);
                        stamper = new PdfStamper(reader, outputPdfStream);
                        fieldNames = new string[stamper.AcroFields.Fields.Keys.Count];
                        stamper.AcroFields.Fields.Keys.CopyTo(fieldNames, 0);
                        foreach (string name in fieldNames)
                        {
                            stamper.AcroFields.RenameField(name, name + "_file" + pageCnt.ToString());
                        }
                        copy.AddPage(copy.GetImportedPage(reader, ++pageCnt));
                    }
                    copy.FreeReader(reader);
                }
            }
            document.Close();
        }
    }
}
