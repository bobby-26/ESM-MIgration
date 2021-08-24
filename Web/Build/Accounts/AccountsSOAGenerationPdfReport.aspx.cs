using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Reports;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Text;
using System.Web;


public partial class AccountsSOAGenerationPdfReport : PhoenixBasePage
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
                //ViewState["VOUCHERDTKEY"] = "";
                ViewState["Generated"] = null;
                ViewState["URL"] = Request.QueryString["URL"].ToString();
                ViewState["DIRECTORYNAME"] = null;

                DataSet dsDR = PhoenixAccountsDebitNoteReference.DebitNoteReferenceEdit(new Guid(ViewState["debitnotereferenceid"].ToString()));

                if (dsDR.Tables.Count > 0)
                {
                    if (dsDR.Tables[0].Rows.Count > 0)
                    {
                        DateTime date = DateTime.Parse(dsDR.Tables[0].Rows[0]["FLDLASTDATE"].ToString());
                        ViewState["AsOnDate"] = date.ToShortDateString();
                        ViewState["AccountCode"] = dsDR.Tables[0].Rows[0]["FLDACCOUNTCODE"].ToString();
                        ViewState["month"] = dsDR.Tables[0].Rows[0]["FLDMONTH"].ToString();
                        ViewState["year"] = dsDR.Tables[0].Rows[0]["FLDYEAR"].ToString();
                        ViewState["monthname"] = dsDR.Tables[0].Rows[0]["FLDMONTHNAME"].ToString();
                        ViewState["yearname"] = dsDR.Tables[0].Rows[0]["FLDYEARNAME"].ToString();
                    }
                }
                if (ViewState["accountid"] != null)
                {
                    DataTable dt1 = PhoenixAccountsSOAGeneration.SOAGenerationVesselByAccount(int.Parse(ViewState["accountid"].ToString()));
                    if (dt1.Rows.Count > 0)
                    {
                        ViewState["vesselid"] = dt1.Rows[0]["FLDVESSELID"].ToString();
                        ViewState["VesselAccountID"] = dt1.Rows[0]["FLDVESSELACCOUNTID"].ToString();
                    }
                }

            }

            if (ViewState["pdfGen"] != null)
            {
                //BindExcelReports();
                if (ViewState["pdfGen"].ToString() == "1")
                {
                    Bind();
                    PhoenixAccountsSoaChecking.SoaCheckingReportHistoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                General.GetNullableGuid(ViewState["debitnotereferenceid"].ToString()),
                                                                                0,
                                                                                General.GetNullableInteger(ViewState["accountid"].ToString()),
                                                                                General.GetNullableGuid(ViewState["dtkey"].ToString()));
                    PhoenixAccountsSOAGeneration.SOAGenerationCombinedPDFUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["debitnotereferenceid"].ToString()), ViewState["URL"].ToString(), 0);
                }
            }
            DataSet ds = PhoenixAccountsSoaChecking.SoaCheckingReportHistoryEdit(General.GetNullableInteger(ViewState["Type"].ToString()), General.GetNullableGuid(ViewState["debitnotereferenceid"].ToString()));
            string src, filepath;
            filepath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts/" + ViewState["dtkey"].ToString() + ".pdf";
            src = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts/" + ViewState["dtkey"].ToString() + ".pdf";
            //src = Session["sitepath"] + "/attachments/Accounts/" + ViewState["dtkey"].ToString() + ".pdf";
            if (File.Exists(filepath) && ds.Tables[0].Rows.Count > 0)
            {
                ifMoreInfo.Attributes["src"] = "../accounts/download.aspx?filename=" + ViewState["dtkey"].ToString() + ".pdf&filepath=" + filepath;
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Combined PDF not yet created.";
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
        string referencetype = "";
        string CombineUsingPDFSharp = "";

        //DataSet dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportTypeList(int.Parse(ViewState["Ownerid"].ToString()));

        string[] alColumns = { "FLDVOUCHERDATE", "FLDVOUCHERROW", "FLDLONGDESCRIPTION", "FLDDEBIT", "FLDCREDIT", "FLDAMOUNT" };
        string[] alCaptions = { "DATE", "REF. NO.", "PARTICULARS", "DEBIT", "CREDIT", "BALANCE" };

        NameValueCollection nvc = new NameValueCollection();

        nvc.Add("accountId", ViewState["accountid"].ToString());
        nvc.Add("vessel", "");
        nvc.Add("month", "");
        nvc.Add("year", "");
        nvc.Add("debitnotereference", ViewState["debitnotereference"].ToString());
        nvc.Add("debitnoteid", ViewState["debitnotereferenceid"].ToString());
        nvc.Add("vesselaccountid", ViewState["VesselAccountID"].ToString());


        DataSet ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceEdit(new Guid(ViewState["debitnotereferenceid"].ToString()));
        DataSet dsConfig = PhoenixAccountsDebitNoteReference.SOAConfigEdit();

        if (dsConfig.Tables.Count > 0)
        {
            CombineUsingPDFSharp = dsConfig.Tables[0].Rows[0]["FLDPDFMERGER"].ToString();
        }


        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DateTime date = DateTime.Parse(ds.Tables[0].Rows[0]["FLDLASTDATE"].ToString());
                nvc.Add("AsOnDate", date.ToShortDateString());
                referencetype = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
            }
        }

        DataSet dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportTypeList(int.Parse(ViewState["Ownerid"].ToString()), referencetype);

        nvc.Add("type", "");
        nvc.Add("subreportcode", "");

        if (dt.Tables.Count > 0)
        {
            if (dt.Tables[0].Rows.Count > 0)
            {
                //List<lstCol> TotlstPdfFiles = new List<lstCol>();

                //List<lstPdfFiles1> TotlstPdf = new List<lstPdfFiles1>();

                List<lstCol> lstPdfFiles = new List<lstCol>();
                List<lstPdfFiles1> lstPdf = new List<lstPdfFiles1>();

                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    BindAttachmentList(nvc, dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString(), lstPdfFiles, lstPdf);
                }

                for (int index = 0; index < lstPdfFiles.Count; index++)
                {
                    lstCol List = lstPdfFiles[index];
                    if (List.Flag != "3")
                    {
                        if (List.Flag == "0")
                        {
                            if (File.Exists(List.FilePath))
                            {
                                File.Delete(List.FilePath);
                            }
                        }

                        else if (List.Flag == "1")
                            lstPdf.Add(new lstPdfFiles1 { FilePath = List.FilePath.Replace("_Html", "_PageNo"), Flag = "0" });
                        else
                            lstPdf.Add(new lstPdfFiles1 { FilePath = List.FilePath, Flag = "1" });
                    }
                    else
                        lstPdf.Add(new lstPdfFiles1 { FilePath = List.FilePath, Flag = "3" });

                }

                AddPageNoInPdf(lstPdfFiles);

                if (CombineUsingPDFSharp == "0")
                {
                    CombineAndSavePdf(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts/" + ViewState["dtkey"].ToString() + ".pdf", lstPdf);
                }
                else
                {
                    CombineAndSavePdfusingPDFSharp(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts/" + ViewState["dtkey"].ToString() + ".pdf", lstPdf);
                }

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

                //for (int index = 0; index < lstPdfFiles.Count; index++)
                //{
                //    lstCol List = lstPdfFiles[index];
                //    if (List.Flag != "3")
                //    {
                //        if (File.Exists(List.FilePath))
                //            File.Delete(List.FilePath);
                //    }
                //}

            }
        }
    }

    //private string HtmlTableHeaderContent(string[] alColumns, string[] alCaptions, DataTable dt)
    //{
    //    string StrBudgetDescription = "", StrFromDate = "", StrToDate = "", StrMonth = "";

    //    if (ViewState["year"].ToString() != "" && ViewState["month"].ToString() != "")
    //    {
    //        StrMonth = ViewState["month"].ToString();
    //        if (StrMonth.Length == 1)
    //            StrMonth = "0" + StrMonth;

    //        StrFromDate = "01/" + StrMonth + "/" + ViewState["year"].ToString();
    //        StrToDate = DateTime.DaysInMonth(int.Parse(ViewState["year"].ToString()), int.Parse(ViewState["month"].ToString())) + "/" + StrMonth + "/" + ViewState["year"].ToString();
    //    }

    //    if (dt.Rows.Count > 0)
    //    {
    //        DataRow dfrow = dt.Rows[0];
    //        StrBudgetDescription = dfrow["FLDOWNERBUDGETCODE"].ToString() + " " + dfrow["FLDDESCRIPTION"].ToString();
    //    }

    //    StringBuilder sbHtmlContent = new StringBuilder();
    //    sbHtmlContent.Append("<table width='100%'>");
    //    sbHtmlContent.Append("<tr>");
    //    sbHtmlContent.Append("<td colspan='7' valign='top'>");
    //    sbHtmlContent.Append("<table width='100%'>");
    //    sbHtmlContent.Append("<tr>");
    //    sbHtmlContent.Append("<td width='25%' valign='top' bgcolor='#FAFAD2' border='1' style=\"font-family:times new roman;font-size:10px;\">");

    //    sbHtmlContent.Append("&nbsp;" + ViewState["description"].ToString());

    //    sbHtmlContent.Append("<br />");
    //    sbHtmlContent.Append("<br />");
    //    sbHtmlContent.Append("&nbsp;<b>" + StrBudgetDescription + "</b>");
    //    sbHtmlContent.Append("</td>");
    //    sbHtmlContent.Append("<td align='center' valign='middle'>");
    //    sbHtmlContent.Append("<b>");
    //    sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:12px;\">");
    //    sbHtmlContent.Append("STATEMENT OF ACCOUNT");
    //    sbHtmlContent.Append("</font>");
    //    sbHtmlContent.Append("</b>");
    //    sbHtmlContent.Append("<br />");
    //    sbHtmlContent.Append("<font style=\"font-family:times new roman;font-size:10px;\">");
    //    sbHtmlContent.Append(ViewState["debitnotereference"].ToString());
    //    sbHtmlContent.Append("</font>");
    //    sbHtmlContent.Append("</td>");
    //    sbHtmlContent.Append("<td width='25%' bgcolor='#FAFAD2' border='1' style=\"font-family:times new roman;font-size:10px;\">");
    //    sbHtmlContent.Append("&nbsp;PREPARED BY:");
    //    sbHtmlContent.Append("<br />");
    //    sbHtmlContent.Append("<br />");
    //    sbHtmlContent.Append("&nbsp;EXECUTIVE SHIP MANAGEMENT INC.");
    //    sbHtmlContent.Append("<br />");
    //    sbHtmlContent.Append("&nbsp;78 SHENTON WAY");
    //    sbHtmlContent.Append("<br />");
    //    sbHtmlContent.Append("&nbsp;#21-00/22-00");
    //    sbHtmlContent.Append("<br />");
    //    sbHtmlContent.Append("&nbsp;SINGAPORE 079120");
    //    sbHtmlContent.Append("<br />");
    //    sbHtmlContent.Append("<br />");
    //    sbHtmlContent.Append("<br />");
    //    sbHtmlContent.Append("</td>");
    //    sbHtmlContent.Append("</tr>");
    //    sbHtmlContent.Append("<tr>");
    //    sbHtmlContent.Append("<td style=\"font-family:times new roman;font-size:10px;\">");
    //    sbHtmlContent.Append("SUMMARY SHEET PERIOD FROM:                   " + StrFromDate);
    //    sbHtmlContent.Append("</td>");
    //    sbHtmlContent.Append("<td style=\"font-family:times new roman;font-size:10px;\">");
    //    sbHtmlContent.Append("   To   " + StrToDate);
    //    sbHtmlContent.Append("</td>");
    //    sbHtmlContent.Append("<td style=\"font-family:times new roman;font-size:10px;\">");
    //    sbHtmlContent.Append("CURRENCY : USD");
    //    sbHtmlContent.Append("</td>");
    //    sbHtmlContent.Append("</tr>");
    //    sbHtmlContent.Append("</table>");
    //    sbHtmlContent.Append("</td>");
    //    sbHtmlContent.Append("</tr>");
    //    sbHtmlContent.Append("<tr>");
    //    sbHtmlContent.Append("<td align='center' bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
    //    sbHtmlContent.Append("DATE");
    //    sbHtmlContent.Append("</td>");
    //    sbHtmlContent.Append("<td align='center' bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
    //    sbHtmlContent.Append("REF. NO.");
    //    sbHtmlContent.Append("</td>");
    //    sbHtmlContent.Append("<td bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
    //    sbHtmlContent.Append("PARTICULARS");
    //    sbHtmlContent.Append("</td>");
    //    sbHtmlContent.Append("<td bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
    //    sbHtmlContent.Append("</td>");
    //    sbHtmlContent.Append("<td align='right' bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
    //    sbHtmlContent.Append("DEBIT");
    //    sbHtmlContent.Append("</td>");
    //    sbHtmlContent.Append("<td align='right' bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
    //    sbHtmlContent.Append("CREDIT");
    //    sbHtmlContent.Append("</td>");
    //    sbHtmlContent.Append("<td align='right' bgcolor='#000080' style=\"font-family:times new roman;font-size:10px;color:White;\">");
    //    sbHtmlContent.Append("BALANCE");
    //    sbHtmlContent.Append("</td>");
    //    sbHtmlContent.Append("</tr>");
    //    sbHtmlContent.Append("</table>");

    //    return sbHtmlContent.ToString();
    //}

    private string HtmlTableHeaderContent(string[] alColumns, string[] alCaptions, DataTable dt)
    {
        string StrBudgetDescription = "", StrFromDate = "", StrToDate = "", StrMonth = "";
        string Strcompany = "", Strcompanyadd1 = "", Strcompanyadd2 = "", Strcompanycountry = "", Strcompanypostal = "";
        string strcompanyadd3 = "", strcompanyddr4 = "", strcompanycity = "", strcurrency = "";

        if (ViewState["yearname"].ToString() != "" && ViewState["monthname"].ToString() != "")
        {
            StrMonth = ViewState["monthname"].ToString();
            if (StrMonth.Length == 1)
                StrMonth = "0" + StrMonth;

            StrFromDate = "01/" + StrMonth + "/" + ViewState["yearname"].ToString();
            StrToDate = DateTime.DaysInMonth(int.Parse(ViewState["yearname"].ToString()), int.Parse(ViewState["monthname"].ToString())) + "/" + StrMonth + "/" + ViewState["yearname"].ToString();
        }

        if (dt.Rows.Count > 0)
        {
            DataRow dfrow = dt.Rows[0];
            if (dfrow["FLDOWNERBUDGETCODE"].ToString() == dfrow["FLDDESCRIPTION"].ToString())
            {
                StrBudgetDescription = dfrow["FLDOWNERBUDGETCODE"].ToString();
            }
            else
            {
                StrBudgetDescription = dfrow["FLDOWNERBUDGETCODE"].ToString() + " " + dfrow["FLDDESCRIPTION"].ToString();
            }
            Strcompany = dfrow["FLDCOMPANY"].ToString();
            Strcompanyadd1 = dfrow["COMPANYADDR2"].ToString();
            Strcompanyadd2 = dfrow["COMPANYADDR1"].ToString();
            Strcompanycountry = dfrow["COMPANYCOUNTRY"].ToString();
            Strcompanypostal = dfrow["COMPANYPOSTAL"].ToString();
            strcompanyadd3 = dfrow["FLDCOMPANYADDR3"].ToString();
            strcompanyddr4 = dfrow["FLDCOMPANYADDR4"].ToString();
            strcompanycity = dfrow["FLDCOMPANYCITY"].ToString();
            strcurrency = dfrow["FLDCURRENCYCODE"].ToString();
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
        sbHtmlContent.Append("&nbsp;" + strcompanyadd3 + "");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("&nbsp;" + strcompanyddr4 + "");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("&nbsp;" + strcompanycity + " " + Strcompanycountry + " " + Strcompanypostal + "");
        //sbHtmlContent.Append("<br />");
        //sbHtmlContent.Append("<br />");
        //sbHtmlContent.Append("<br />");
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
        sbHtmlContent.Append("CURRENCY : " + strcurrency);
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



    private void BindAttachmentList(NameValueCollection nvc, string subreportcode, List<lstCol> lstPdfFiles, List<lstPdfFiles1> lstPdf)
    {
        string VVCCYN = "0";
        List<lstCol> lst = new List<lstCol>();
        string _reportfile = "";
        string[] _reportfile1 = new string[20];
        string _filename = "";
        string dtkey = "", filepath = "";
        DataSet dtS = new DataSet();
        DataTable dtTB = new DataTable();
        DataRow dr = dtTB.NewRow();

        string[] alColumns = { "FLDVOUCHERDATE", "FLDVOUCHERROW", "FLDLONGDESCRIPTION", "FLDDEBIT", "FLDCREDIT", "FLDAMOUNT" };
        string[] alCaptions = { "DATE", "REF. NO.", "PARTICULARS", "DEBIT", "CREDIT", "BALANCE" };


        if (subreportcode == "VTBA")
        {
            dtTB = PhoenixReportsAccount.VesselTrailBalance(nvc, ref _reportfile, ref _filename);
            nvc.Remove("reportcode");
            nvc.Add("reportcode", "VESSELTRAILBALANCE");
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {
                _reportfile = HttpContext.Current.Server.MapPath("../SsrsReports/ReportsAccountsVesselTrailBalance.rdlc");
            }
            else
            {
                _reportfile = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsVesselTrailBalance.rpt");
            }
        }
        if (subreportcode == "VTBY")
        {
            dtTB = PhoenixReportsAccount.VesselTrailBalanceYTD(nvc, ref _reportfile, ref _filename);
            nvc.Remove("reportcode");
            nvc.Add("reportcode", "VESSELTRAILBALANCEYTD");
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {
                _reportfile = HttpContext.Current.Server.MapPath("../SsrsReports/ReportsAccountsVesselTrailBalanceYTD.rdlc");
            }
            else
            {
                _reportfile = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsVesselTrailBalanceYTD.rpt");
            }
        }
        if (subreportcode == "VTBO")
        {
            dtTB = PhoenixReportsAccount.VesselTrailBalanceYTDOwnerbudgetcode(nvc, ref _reportfile, ref _filename);
            nvc.Remove("reportcode");
            nvc.Add("reportcode", "VESSELTRAILBALANCEYTDOWNER");
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {
                _reportfile = HttpContext.Current.Server.MapPath("../SsrsReports/ReportsAccountsVesselTrailBalanceYTDOwner.rdlc");
            }
            else
            {
                _reportfile = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsVesselTrailBalanceYTDOwner.rpt");
            }
        }
        if (subreportcode == "ASLO")
        {
            DataTable DtBL = PhoenixAccountsSoaChecking.SoaCheckingBudgetList(int.Parse(ViewState["accountid"].ToString()), ViewState["debitnotereference"].ToString());
            if (DtBL.Rows.Count > 0)
            {
                //string PreVoucherNumber = string.Empty;
                //string CurVoucherNumber = string.Empty;

                foreach (DataRow DrBL in DtBL.Rows) // Loop over the rows.
                {
                    DataTable DtVL = PhoenixAccountsSoaChecking.SoaCheckingVouchersList(ViewState["debitnotereference"].ToString(), int.Parse(ViewState["Ownerid"].ToString()), General.GetNullableString(DrBL["FLDBUDGETID"].ToString()), int.Parse(ViewState["accountid"].ToString()));

                    string Gid = Guid.NewGuid().ToString();

                    if (DtVL.Rows.Count > 0)
                    {
                        ConvertToPdf(HtmlTableDataContent(alColumns, alCaptions, DtVL), HttpContext.Current.Request.MapPath("~/Attachments/TEMP/" + Gid + ".pdf"));

                        AddHeaderContentInPdf(HtmlTableHeaderContent(alColumns, alCaptions, DtVL), HttpContext.Current.Request.MapPath("~/Attachments/TEMP/") + Gid + "");

                        lstPdfFiles.Add(new lstCol { FilePath = HttpContext.Current.Request.MapPath("~/Attachments/TEMP/") + Gid + ".pdf", Flag = "0" });
                        lstPdfFiles.Add(new lstCol { FilePath = HttpContext.Current.Request.MapPath("~/Attachments/TEMP/") + Gid + "_Html.pdf", Flag = "1" });

                        FileInfo fi;
                        string strFilePath, strpath, strdirectoryname;
                        int iRowCount = 0;
                        int iTotalPageCount = 0;
                        foreach (DataRow DrVL in DtVL.Rows) // Loop over the rows.
                        {
                            //CurVoucherNumber = DrVL["FLDVOUCHERNUMBER"].ToString();

                            //DataSet dsVLFA = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearch(new Guid(DrVL["FLDVOUCHERDTKEY"].ToString()), null, null, null, null, 1, 100, ref iRowCount, ref iTotalPageCount);
                            //if (PreVoucherNumber != CurVoucherNumber)
                            //{

                            //    if (dsVLFA.Tables.Count > 0)
                            //    {
                            //        DataTable DtVLFA = dsVLFA.Tables[0];
                            //        foreach (DataRow DrVLFA in DtVLFA.Rows) // Loop over the rows.
                            //        {
                            //            strpath = "";
                            //            strFilePath = "";
                            //            strdirectoryname = "";

                            //            strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrVLFA["FLDFILEPATH"].ToString();
                            //            strFilePath = strpath.Substring(strpath.Length - 40, 40);

                            //            strdirectoryname = strpath.Substring(0, strpath.Length - 40);
                            //            strdirectoryname = strdirectoryname + ViewState["debitnotereference"].ToString();

                            //            ViewState["DIRECTORYNAME"] = strdirectoryname;

                            //            strFilePath = strdirectoryname + "/" + strFilePath;

                            //            if (!Directory.Exists(strdirectoryname))
                            //                strFilePath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrVLFA["FLDFILEPATH"].ToString();

                            //            fi = new FileInfo(strFilePath);
                            //            if (fi.Extension.ToUpper() == ".PDF")
                            //                lstPdfFiles.Add(new lstCol { FilePath = strFilePath, Flag = "2" });
                            //        }
                            //    }
                            //    PreVoucherNumber = CurVoucherNumber;
                            //}

                            DataSet dsFA = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearchForCombinedPDF(new Guid(DrVL["FLDDTKEY"].ToString()), null, null, null, null, 1, 100, ref iRowCount, ref iTotalPageCount);
                            if (dsFA.Tables.Count > 0)
                            {
                                DataTable DtFA = dsFA.Tables[0];
                                foreach (DataRow DrFA in DtFA.Rows) // Loop over the rows.
                                {

                                    strpath = "";
                                    strFilePath = "";
                                    strdirectoryname = "";

                                    strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrFA["FLDFILEPATH"].ToString();
                                    strFilePath = strpath.Substring(strpath.Length - 40, 40);

                                    strFilePath = DrVL["FLDVOUCHERLINEITEMNO"].ToString() + "_" + strFilePath;

                                    strdirectoryname = strpath.Substring(0, strpath.Length - 40);
                                    strdirectoryname = strdirectoryname + ViewState["debitnotereference"].ToString();

                                    ViewState["DIRECTORYNAME"] = strdirectoryname;

                                    strFilePath = strdirectoryname + "/" + strFilePath;

                                    if (!Directory.Exists(strdirectoryname))
                                        strFilePath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrFA["FLDFILEPATH"].ToString();
                                    
                                    fi = new FileInfo(strFilePath);
                                    if (fi.Extension.ToUpper() == ".PDF")
                                        lstPdfFiles.Add(new lstCol { FilePath = strFilePath, Flag = "2" });
                                }
                            }
                        }
                    }
                }
            }
        }
        if (subreportcode == "ASLE")
        {
            DataTable DtBL = PhoenixAccountsSoaChecking.SoaCheckingESMBudgetList(int.Parse(ViewState["accountid"].ToString()), ViewState["debitnotereference"].ToString());

            if (DtBL.Rows.Count > 0)
            {
                //string PreVoucherNumber = string.Empty;
                //string CurVoucherNumber = string.Empty;

                foreach (DataRow DrBL in DtBL.Rows) // Loop over the rows.
                {
                    DataTable DtVL = PhoenixAccountsSoaChecking.SoaCheckingESMVouchersList(ViewState["debitnotereference"].ToString(), int.Parse(ViewState["Ownerid"].ToString()), General.GetNullableString(DrBL["FLDBUDGETID"].ToString()), int.Parse(ViewState["accountid"].ToString()));

                    string Gid = Guid.NewGuid().ToString();

                    if (DtVL.Rows.Count > 0)
                    {
                        ConvertToPdf(HtmlTableDataContent(alColumns, alCaptions, DtVL), HttpContext.Current.Request.MapPath("~/Attachments/TEMP/" + Gid + ".pdf"));

                        AddHeaderContentInPdf(HtmlTableHeaderContentESM(alColumns, alCaptions, DtVL), HttpContext.Current.Request.MapPath("~/Attachments/TEMP/") + Gid + "");

                        lstPdfFiles.Add(new lstCol { FilePath = HttpContext.Current.Request.MapPath("~/Attachments/TEMP/") + Gid + ".pdf", Flag = "0" });
                        lstPdfFiles.Add(new lstCol { FilePath = HttpContext.Current.Request.MapPath("~/Attachments/TEMP/") + Gid + "_Html.pdf", Flag = "1" });

                        FileInfo fi;
                        //string strFilePath;
                        string strFilePath, strpath, strdirectoryname;
                        int iRowCount = 0;
                        int iTotalPageCount = 0;

                        foreach (DataRow DrVL in DtVL.Rows) // Loop over the rows.
                        {
                            //CurVoucherNumber = DrVL["FLDVOUCHERNUMBER"].ToString();

                            //DataSet dsVLFA = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearch(new Guid(DrVL["FLDVOUCHERDTKEY"].ToString()), null, null, null, null, 1, 100, ref iRowCount, ref iTotalPageCount);
                            //if (PreVoucherNumber != CurVoucherNumber)
                            //{

                            //    if (dsVLFA.Tables.Count > 0)
                            //    {
                            //        DataTable DtVLFA = dsVLFA.Tables[0];
                            //        foreach (DataRow DrVLFA in DtVLFA.Rows) // Loop over the rows.
                            //        {
                            //            strpath = "";
                            //            strFilePath = "";
                            //            strdirectoryname = "";

                            //            strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrVLFA["FLDFILEPATH"].ToString();
                            //            strFilePath = strpath.Substring(strpath.Length - 40, 40);

                            //            strdirectoryname = strpath.Substring(0, strpath.Length - 40);
                            //            strdirectoryname = strdirectoryname + ViewState["debitnotereference"].ToString();

                            //            ViewState["DIRECTORYNAME"] = strdirectoryname;

                            //            strFilePath = strdirectoryname + "/" + strFilePath;

                            //            if (!Directory.Exists(strdirectoryname))
                            //                strFilePath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrVLFA["FLDFILEPATH"].ToString();

                            //            fi = new FileInfo(strFilePath);
                            //            if (fi.Extension.ToUpper() == ".PDF")
                            //                lstPdfFiles.Add(new lstCol { FilePath = strFilePath, Flag = "2" });
                            //        }
                            //    }
                            //    PreVoucherNumber = CurVoucherNumber;
                            //}
                            DataSet dsFA = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearchForCombinedPDF(new Guid(DrVL["FLDDTKEY"].ToString()), null, null, null, null, 1, 100, ref iRowCount, ref iTotalPageCount);
                            if (dsFA.Tables.Count > 0)
                            {
                                DataTable DtFA = dsFA.Tables[0];
                                foreach (DataRow DrFA in DtFA.Rows) // Loop over the rows.
                                {

                                    strpath = "";
                                    strFilePath = "";
                                    strdirectoryname = "";

                                    strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrFA["FLDFILEPATH"].ToString();
                                    strFilePath = strpath.Substring(strpath.Length - 40, 40);

                                    strFilePath = DrVL["FLDVOUCHERLINEITEMNO"].ToString() + "_" + strFilePath;

                                    strdirectoryname = strpath.Substring(0, strpath.Length - 40);
                                    strdirectoryname = strdirectoryname + ViewState["debitnotereference"].ToString();

                                    ViewState["DIRECTORYNAME"] = strdirectoryname;

                                    strFilePath = strdirectoryname + "/" + strFilePath;

                                    if (!Directory.Exists(strdirectoryname))
                                        strFilePath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrFA["FLDFILEPATH"].ToString();
                                  
                                    fi = new FileInfo(strFilePath);
                                    if (fi.Extension.ToUpper() == ".PDF")
                                        lstPdfFiles.Add(new lstCol { FilePath = strFilePath, Flag = "2" });
                                }
                            }
                        }
                    }
                }
            }
        }


        if (subreportcode == "SENE")
        {
            dtS = PhoenixReportsAccount.StatementOfOwnerAccountsSummaryesmbudget(nvc, ref _reportfile1, ref _filename);
            nvc.Remove("reportcode");
            nvc.Add("reportcode", "STATEMENTOFACCOUNTSUMMARYESMBUDGET");
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {
                _reportfile1[0] = HttpContext.Current.Server.MapPath("../SsrsReports/ReportStatementofAccountsLineitemEsmBudgetSummary.rdlc");
                _reportfile1[1] = "ReportStatementofAccountsSummaryEsmBudgetHeadder.rdlc";
                _reportfile1[2] = "";
                _reportfile1[3] = "";
                _reportfile1[4] = "";
                _reportfile1[5] = "";
                _reportfile1[6] = "";
                _reportfile1[7] = "";
                _reportfile1[8] = "";
                _reportfile1[9] = "";
                _reportfile1[10] = "";
                _reportfile1[11] = "";
                _reportfile1[12] = "";
                _reportfile1[13] = "";
                _reportfile1[14] = "";
                _reportfile1[15] = "";
                _reportfile1[16] = "";
                _reportfile1[17] = "";
                _reportfile1[18] = "";
                _reportfile1[19] = "";
            }
            else
            {
                _reportfile1[0] = HttpContext.Current.Server.MapPath("../Reports/ReportStatementofAccountsLineitemEsmBudgetSummary.rpt");
                _reportfile1[1] = "ReportStatementofAccountsSummaryEsmBudgetHeadder.rpt";
                _reportfile1[2] = "";
                _reportfile1[3] = "";
                _reportfile1[4] = "";
                _reportfile1[5] = "";
                _reportfile1[6] = "";
                _reportfile1[7] = "";
                _reportfile1[8] = "";
                _reportfile1[9] = "";
                _reportfile1[10] = "";
                _reportfile1[11] = "";
                _reportfile1[12] = "";
                _reportfile1[13] = "";
                _reportfile1[14] = "";
                _reportfile1[15] = "";
                _reportfile1[16] = "";
                _reportfile1[17] = "";
                _reportfile1[18] = "";
                _reportfile1[19] = "";
            }
        }
        if (subreportcode == "SENO")
        {
            dtS = PhoenixReportsAccount.StatementOfOwnerAccountsSummary(nvc, ref _reportfile1, ref _filename);
            nvc.Remove("reportcode");
            nvc.Add("reportcode", "STATEMENTOFOWNERACCOUNTSUMMARY");
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {
                _reportfile1[0] = HttpContext.Current.Server.MapPath("../SsrsReports/ReportStatementofOwnerAccountsLineitemSummary.rdlc");
                _reportfile1[1] = "ReportStatementofOwnerAccountsSummaryHeadder.rdlc";
                _reportfile1[2] = "";
                _reportfile1[3] = "";
                _reportfile1[4] = "";
                _reportfile1[5] = "";
                _reportfile1[6] = "";
                _reportfile1[7] = "";
                _reportfile1[8] = "";
                _reportfile1[9] = "";
                _reportfile1[10] = "";
                _reportfile1[11] = "";
                _reportfile1[12] = "";
                _reportfile1[13] = "";
                _reportfile1[14] = "";
                _reportfile1[15] = "";
                _reportfile1[16] = "";
                _reportfile1[17] = "";
                _reportfile1[18] = "";
                _reportfile1[19] = "";
            }
            else
            {
                _reportfile1[0] = HttpContext.Current.Server.MapPath("../Reports/ReportStatementofOwnerAccountsLineitemSummary.rpt");
                _reportfile1[1] = "ReportStatementofOwnerAccountsSummaryHeadder.rpt";
                _reportfile1[2] = "";
                _reportfile1[3] = "";
                _reportfile1[4] = "";
                _reportfile1[5] = "";
                _reportfile1[6] = "";
                _reportfile1[7] = "";
                _reportfile1[8] = "";
                _reportfile1[9] = "";
                _reportfile1[10] = "";
                _reportfile1[11] = "";
                _reportfile1[12] = "";
                _reportfile1[13] = "";
                _reportfile1[14] = "";
                _reportfile1[15] = "";
                _reportfile1[16] = "";
                _reportfile1[17] = "";
                _reportfile1[18] = "";
                _reportfile1[19] = "";
            }
        }

        if (subreportcode == "SENWB")
        {
            dtS = PhoenixReportsAccount.StatementOfOwnerAccountsSummary(nvc, ref _reportfile1, ref _filename);
            nvc.Remove("reportcode");
            nvc.Add("reportcode", "STATEMENTOFOWNERACCOUNTWITHOUTBUDGET");
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {
                _reportfile1[0] = HttpContext.Current.Server.MapPath("../SsrsReports/ReportStatementofOwnerAccountsWithoutBudgetSummary.rdlc");
                _reportfile1[1] = "ReportStatementofOwnerAccountsWithoutBudgetHeader.rdlc";
                _reportfile1[2] = "";
                _reportfile1[3] = "";
                _reportfile1[4] = "";
                _reportfile1[5] = "";
                _reportfile1[6] = "";
                _reportfile1[7] = "";
                _reportfile1[8] = "";
                _reportfile1[9] = "";
                _reportfile1[10] = "";
                _reportfile1[11] = "";
                _reportfile1[12] = "";
                _reportfile1[13] = "";
                _reportfile1[14] = "";
                _reportfile1[15] = "";
                _reportfile1[16] = "";
                _reportfile1[17] = "";
                _reportfile1[18] = "";
                _reportfile1[19] = "";
            }
            else
            {
                _reportfile1[0] = HttpContext.Current.Server.MapPath("../Reports/ReportStatementofOwnerAccountsWithoutBudgetSummary.rpt");
                _reportfile1[1] = "ReportStatementofOwnerAccountsWithoutBudgetHeader.rpt";
                _reportfile1[2] = "";
                _reportfile1[3] = "";
                _reportfile1[4] = "";
                _reportfile1[5] = "";
                _reportfile1[6] = "";
                _reportfile1[7] = "";
                _reportfile1[8] = "";
                _reportfile1[9] = "";
                _reportfile1[10] = "";
                _reportfile1[11] = "";
                _reportfile1[12] = "";
                _reportfile1[13] = "";
                _reportfile1[14] = "";
                _reportfile1[15] = "";
                _reportfile1[16] = "";
                _reportfile1[17] = "";
                _reportfile1[18] = "";
                _reportfile1[19] = "";
            }
        }

        if (subreportcode == "MVRE")
        {
            nvc.Remove("type");
            nvc.Add("type", "Monthly");
            dtTB = PhoenixReportsAccount.VesselVariance(nvc, ref _reportfile, ref _filename);
            nvc.Remove("reportcode");
            nvc.Add("reportcode", "VESSELVARIANCE");
            dr = dtTB.Rows[0];

            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {
                if (dr["FLDCOMMITTEDCOSTSINCLUDEDYN"].ToString() == "1")
                    _reportfile = HttpContext.Current.Server.MapPath("../SsrsReports/ReportsAccountsVesselMonthlyVariance.rdlc");
                else
                {
                    VVCCYN = "1";
                    _reportfile = HttpContext.Current.Server.MapPath("../SsrsReports/ReportsAccountsVesselVariance.rdlc");
                }
            }
            else
            {
                if (dr["FLDCOMMITTEDCOSTSINCLUDEDYN"].ToString() == "1")
                    _reportfile = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsVesselMonthlyVariance.rpt");
                else
                    _reportfile = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsVesselVariance.rpt");
            }
        }

        if (subreportcode == "YVRE")
        {
            nvc.Remove("type");
            nvc.Add("type", "Yearly");
            dtTB = PhoenixReportsAccount.VesselVariance(nvc, ref _reportfile, ref _filename);
            nvc.Remove("reportcode");
            nvc.Add("reportcode", "VESSELVARIANCE");
            dr = dtTB.Rows[0];
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {
                if (dr["FLDCOMMITTEDCOSTSINCLUDEDYN"].ToString() == "1")
                    _reportfile = HttpContext.Current.Server.MapPath("../SsrsReports/ReportsAccountsVesselYearlyVariance.rdlc");
                else
                {
                    VVCCYN = "1";
                    _reportfile = HttpContext.Current.Server.MapPath("../SsrsReports/ReportsAccountsVesselVariance.rdlc");
                }
            }
            else
            {
                if (dr["FLDCOMMITTEDCOSTSINCLUDEDYN"].ToString() == "1")
                    _reportfile = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsVesselYearlyVariance.rpt");
                else
                    _reportfile = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsVesselVariance.rpt");
            }
        }

        if (subreportcode == "AVRE")
        {
            nvc.Remove("type");
            nvc.Add("type", "Accumulated");
            dtTB = PhoenixReportsAccount.VesselVariance(nvc, ref _reportfile, ref _filename);
            nvc.Remove("reportcode");
            nvc.Add("reportcode", "VESSELVARIANCE");
            dr = dtTB.Rows[0];
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {
                if (dr["FLDCOMMITTEDCOSTSINCLUDEDYN"].ToString() == "1")
                    _reportfile = HttpContext.Current.Server.MapPath("../SsrsReports/ReportsAccountsVesselVarianceNoCommittedCosts.rdlc");
                else
                {
                    VVCCYN = "1";
                    _reportfile = HttpContext.Current.Server.MapPath("../SsrsReports/ReportsAccountsVesselVariance.rdlc");
                }

            }
            else
            {
                if (dr["FLDCOMMITTEDCOSTSINCLUDEDYN"].ToString() == "1")
                    _reportfile = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsVesselVarianceNoCommittedCosts.rpt");
                else
                    _reportfile = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsVesselVariance.rpt");
            }
        }

        if (subreportcode == "FDP")
        {
            dtTB = PhoenixReportsAccount.VesselSummaryBalance(nvc, ref _reportfile, ref _filename);
            nvc.Remove("reportcode");
            nvc.Add("reportcode", "VESSELSUMMARYBALANCE");
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {
                _reportfile = HttpContext.Current.Server.MapPath("../SsrsReports/ReportsAccountsVesselSummaryBalance.rdlc");
            }
            else
            {
                _reportfile = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsVesselSummaryBalance.rpt");
            }
        }
        if (subreportcode == "CCR")
        {
            DataTable dt = PhoenixAccountsVoucher.CommittedCostVoucherDetails(int.Parse(ViewState["VesselAccountID"].ToString()), DateTime.Parse(nvc["AsOnDate"].ToString()), 1);
            if (dt.Rows.Count > 0)
            {
                FileInfo fi;
                string strFilePath;
                int iRowCount = 0;
                int iTotalPageCount = 0;
                foreach (DataRow drt in dt.Rows) // Loop over the rows.
                {
                    DataSet dsFA = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(drt["FLDDTKEY"].ToString()), null, null, null, null, 1, 100, ref iRowCount, ref iTotalPageCount);
                    if (dsFA.Tables.Count > 0)
                    {
                        DataTable DtFA = dsFA.Tables[0];
                        foreach (DataRow DrFA in DtFA.Rows) // Loop over the rows.
                        {
                            //strFilePath = HttpContext.Current.Request.MapPath("~/Attachments/") + DrFA["FLDFILEPATH"].ToString();
                            strFilePath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrFA["FLDFILEPATH"].ToString();
                            fi = new FileInfo(strFilePath);
                            if (fi.Extension.ToUpper() == ".PDF")
                                lstPdfFiles.Add(new lstCol { FilePath = strFilePath, Flag = "2" });
                        }
                    }
                }
            }
        }
        if (subreportcode == "CVP")
        {
            nvc.Remove("subreportcode");
            nvc.Add("subreportcode", "CVP");
            dtTB = PhoenixReportsAccount.SOAGenerationCoverpage(nvc, ref _reportfile, ref _filename);
            nvc.Remove("reportcode");
            nvc.Add("reportcode", "SOAGENERATIONCOVERPAGE");
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {
                _reportfile = HttpContext.Current.Server.MapPath("../SsrsReports/ReportsAccountSOAGenerationCoverPage.rdlc");
            }
            else
            {
                _reportfile = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountSOAGenerationCoverPage.rpt");
            }
        }
        if (subreportcode == "IDX")
        {
            nvc.Remove("subreportcode");
            nvc.Add("subreportcode", "IDX");
            dtTB = PhoenixReportsAccount.SOAGenerationIndex(nvc, ref _reportfile, ref _filename);
            nvc.Remove("reportcode");
            nvc.Add("reportcode", "SOAGENERATIONINDEX");
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {
                _reportfile = HttpContext.Current.Server.MapPath("../SsrsReports/ReportsAccountSOAGenerationIndex.rdlc");
            }
            else
            {
                _reportfile = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountSOAGenerationIndex.rpt");
            }
        }
        if (subreportcode == "SHSL" || subreportcode == "SHPB" || subreportcode == "SHTB" || subreportcode == "SHVR" || subreportcode == "SHCC" || subreportcode == "SHAJ")
        {
            nvc.Remove("subreportcode");
            nvc.Add("subreportcode", subreportcode);
            dtTB = PhoenixReportsAccount.SOAGenerationSectionHeadrer(nvc, ref _reportfile, ref _filename);
            nvc.Remove("reportcode");
            nvc.Add("reportcode", "SOAGENERATIONSECTIONHEADER");
            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {
                _reportfile = HttpContext.Current.Server.MapPath("../SsrsReports/ReportsAccountSOAGenerationSectionHeader.rdlc");
            }
            else
            {
                _reportfile = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountSOAGenerationSectionHeader.rpt");
            }
        }
        if (subreportcode == "PBR" || subreportcode == "CAR" || subreportcode == "AJE")
        {
            DataSet dt = PhoenixAccountsDebitNoteReference.DebitNoteReferenceEdit(new Guid(ViewState["debitnotereferenceid"].ToString()));
            if (dt.Tables.Count > 0)
            {
                FileInfo fi;
                string strFilePath;
                int iRowCount = 0;
                int iTotalPageCount = 0;
                Guid dtMankey = new Guid();

                dtMankey = new Guid(subreportcode == "PBR" ? dt.Tables[0].Rows[0]["FLDPORTAGEBILLDTKEY"].ToString() : (subreportcode == "CAR" ? dt.Tables[0].Rows[0]["FLDCAPTAINCASHDTKEY"].ToString() : dt.Tables[0].Rows[0]["FLDJOURNALENTRYDTKEY"].ToString()));
                DataSet dsFA = PhoenixCommonFileAttachment.AttachmentSearch(dtMankey, null, null, null, null, 1, 100, ref iRowCount, ref iTotalPageCount);
                if (dsFA.Tables.Count > 0)
                {
                    DataTable DtFA = dsFA.Tables[0];
                    foreach (DataRow DrFA in DtFA.Rows) // Loop over the rows.
                    {
                        //strFilePath = HttpContext.Current.Request.MapPath("~/Attachments/") + DrFA["FLDFILEPATH"].ToString();
                        strFilePath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + DrFA["FLDFILEPATH"].ToString();
                        fi = new FileInfo(strFilePath);
                        if (fi.Extension.ToUpper() == ".PDF")
                            lstPdfFiles.Add(new lstCol { FilePath = strFilePath, Flag = "2" });
                    }
                }
            }
        }


        if (subreportcode != "ASLE" && subreportcode != "ASLO" && subreportcode != "CCR" && subreportcode != "PBR" && subreportcode != "CAR" && subreportcode != "AJE")
        {
            dtkey = ViewState["dtkey"].ToString() + "_" + subreportcode;
            filepath = PhoenixModule.ACCOUNTS + "/" + dtkey + ".pdf";
            string filename = Path.GetFileName(_filename);
            string newfullfilename = Path.GetDirectoryName(_filename) + @"\" + dtkey + ".pdf";

            DataSet ds = new DataSet();
            DataTable dtCopy = dtTB.Copy();
            ds.Tables.Add(dtCopy);

            string reportcode = nvc["reportcode"].ToString();

            string[] _reportfile2 = new string[10];
            _reportfile2[0] = _reportfile.ToString();

            if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {

                if (subreportcode == "SENE" || subreportcode == "SENO" || subreportcode == "SENWB")
                    //PhoenixSsrsReportsCommon.GetInterface(_reportfile1, dtS, ExportFileFormat.PDF, "GenerateSSRSPDF", ref _filename);
                    GenerateSSRSPDF(newfullfilename, dtS, reportcode, subreportcode, VVCCYN);
                else
                    //PhoenixSsrsReportsCommon.GetInterface(_reportfile2, ds , ExportFileFormat.PDF, "GenerateSSRSPDF", ref _filename);
                    GenerateSSRSPDF(newfullfilename, ds, reportcode, subreportcode, VVCCYN);

            }
            else
            {
                if (subreportcode == "SENE" || subreportcode == "SENO" || subreportcode == "SENWB")
                    PhoenixReportClass.ExportReport(_reportfile1, newfullfilename, dtS);
                else
                    PhoenixReportClass.ExportReport(_reportfile, newfullfilename, dtTB);
            }
            lstPdfFiles.Add(new lstCol { FilePath = newfullfilename, Flag = "3" });
        }

    }


    private void BindExcelReports()
    {
        FontFactory.RegisterDirectories();
        Font fontNormal = new Font(FontFactory.GetFont("Arial", 12, Font.NORMAL));

        DataSet dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReportAditionalAttachmentList(int.Parse(ViewState["Ownerid"].ToString()));

        string[] alColumns = { "FLDVOUCHERDATE", "FLDVOUCHERROW", "FLDLONGDESCRIPTION", "FLDDEBIT", "FLDCREDIT", "FLDAMOUNT" };
        string[] alCaptions = { "DATE", "REF. NO.", "PARTICULARS", "DEBIT", "CREDIT", "BALANCE" };

        NameValueCollection nvc = new NameValueCollection();

        nvc.Add("accountId", ViewState["accountid"].ToString());
        nvc.Add("vessel", "");
        nvc.Add("month", "");
        nvc.Add("year", "");
        nvc.Add("debitnotereference", ViewState["debitnotereference"].ToString());
        nvc.Add("debitnoteid", ViewState["debitnotereferenceid"].ToString());
        nvc.Add("ownerid", ViewState["Ownerid"].ToString());


        DataSet ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceEdit(new Guid(ViewState["debitnotereferenceid"].ToString()));

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DateTime date = DateTime.Parse(ds.Tables[0].Rows[0]["FLDLASTDATE"].ToString());
                nvc.Add("AsOnDate", date.ToShortDateString());
                nvc.Add("AccountCode", ds.Tables[0].Rows[0]["FLDACCOUNTCODE"].ToString());
                nvc.Remove("month");
                nvc.Add("month", ds.Tables[0].Rows[0]["FLDMONTH"].ToString());
                nvc.Remove("year");
                nvc.Add("year", ds.Tables[0].Rows[0]["FLDYEAR"].ToString());
                //nvc.Add("monthname", ds.Tables[0].Rows[0]["FLDMONTHNAME"].ToString());
                //nvc.Add("yearname", ds.Tables[0].Rows[0]["FLDYEARNAME"].ToString());
            }
        }

        nvc.Add("type", "");

        if (dt.Tables.Count > 0)
        {
            if (dt.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    Guid dtkey = Guid.NewGuid();
                    if (dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString() == "OSRM")
                    {
                        PhoenixAccounts2XL.Export2XLMMSLVoucherDetails(General.GetNullableInteger(nvc["accountId"].ToString()), General.GetNullableString(nvc["AccountCode"].ToString()),
                                                                                    General.GetNullableString(nvc["debitnotereference"].ToString()), General.GetNullableInteger(nvc["ownerid"].ToString()), dtkey);

                    }

                    if (dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString() == "OSRK")
                    {

                        PhoenixAccounts2XL.Export2XLKoyoVoucherDetails(General.GetNullableInteger(nvc["accountId"].ToString()), General.GetNullableString(nvc["AccountCode"].ToString()),
                                                                                  General.GetNullableString(nvc["debitnotereference"].ToString()), General.GetNullableInteger(nvc["ownerid"].ToString()), dtkey);
                    }

                    if (dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString() == "OSRU")
                    {

                        PhoenixAccountsSOAUACCReport2XL.Export2XLSOAUACC(General.GetNullableInteger(nvc["year"].ToString()), General.GetNullableInteger(nvc["ownerid"].ToString()), 0, General.GetNullableInteger(ViewState["vesselid"].ToString()), ViewState["yearname"].ToString(), dtkey);
                    }

                    if (dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString() == "OSRD")
                    {

                        PhoenixAccountsSOADiamondReport2XL.Export2XLDiamondVoucherDetails(General.GetNullableInteger(nvc["year"].ToString()), General.GetNullableInteger(nvc["ownerid"].ToString()), 0, dtkey);

                    }

                    //if (dt.Tables[0].Rows[i]["FLDSUBREPORTCODE"].ToString() == "OSRC")
                    //{
                    //    //string headyear = ucFinancialYear.SelectedText;
                    //    //PhoenixAccountsSOAUACCReport2XL.Export2XLUACC(General.GetNullableInteger(ucFinancialYear.SelectedValue), General.GetNullableInteger(ucOwner.SelectedAddress), 0, General.GetNullableInteger(ddlvessel.SelectedValue), headyear);
                    //}

                    long length = new System.IO.FileInfo(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts/" + dtkey + ".xlsx").Length;

                    PhoenixCommonFileAttachment.InsertAttachment(
                        new Guid(ViewState["dtkey"].ToString()), ViewState["debitnotereference"].ToString() + ".xlsx", "Accounts/" + dtkey.ToString() + ".xlsx",
                         length, null, 0, null, dtkey);
                }

            }
        }
    }

    private string HtmlTableHeaderContentESM(string[] alColumns, string[] alCaptions, DataTable dt)
    {
        string StrBudgetDescription = "", StrFromDate = "", StrToDate = "", StrMonth = "";
        string Strcompany = "", Strcompanyadd1 = "", Strcompanyadd2 = "", Strcompanycountry = "", Strcompanypostal = "";
        string Strcompanyadd3 = "", Strcompanyadd4 = "", Strcompanycity = "", strcurrency = "";

        if (ViewState["yearname"].ToString() != "" && ViewState["monthname"].ToString() != "")
        {
            StrMonth = ViewState["monthname"].ToString();
            if (StrMonth.Length == 1)
                StrMonth = "0" + StrMonth;

            StrFromDate = "01/" + StrMonth + "/" + ViewState["yearname"].ToString();
            StrToDate = DateTime.DaysInMonth(int.Parse(ViewState["yearname"].ToString()), int.Parse(ViewState["monthname"].ToString())) + "/" + StrMonth + "/" + ViewState["yearname"].ToString();
        }

        if (dt.Rows.Count > 0)
        {
            DataRow dfrow = dt.Rows[0];
            StrBudgetDescription = dfrow["FLDESMBUDGETCODE"].ToString() + " " + dfrow["FLDBUDGETCODEDESCRIPTION"].ToString();
            Strcompany = dfrow["FLDCOMPANY"].ToString();
            Strcompanyadd1 = dfrow["COMPANYADDR2"].ToString();
            Strcompanyadd2 = dfrow["COMPANYADDR1"].ToString();
            Strcompanycountry = dfrow["COMPANYCOUNTRY"].ToString();
            Strcompanypostal = dfrow["COMPANYPOSTAL"].ToString();
            Strcompanycity = dfrow["FLDCOMPANYCITY"].ToString();
            Strcompanyadd3 = dfrow["FLDCOMPANYADDR3"].ToString();
            Strcompanyadd4 = dfrow["FLDCOMPANYADDR4"].ToString();
            strcurrency = dfrow["FLDCURRENCYCODE"].ToString();
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
        sbHtmlContent.Append("&nbsp;" + Strcompanyadd3 + "");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("&nbsp;" + Strcompanyadd4 + "");
        sbHtmlContent.Append("<br />");
        sbHtmlContent.Append("&nbsp;" + Strcompanycity + " " + Strcompanycountry + " " + Strcompanypostal + "");
        //sbHtmlContent.Append("<br />");
        //sbHtmlContent.Append("<br />");
        //sbHtmlContent.Append("<br />");
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
        sbHtmlContent.Append("CURRENCY : " + strcurrency);
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

    public void GenerateSSRSPDF(string _filename, DataSet ds, string reportcode, string subreportcode, string VVCCYN)
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];

        nvc.Remove("applicationcode");
        nvc.Add("applicationcode", "5");
        nvc.Remove("reportcode");
        nvc.Add("reportcode", reportcode.ToString());
        if (reportcode.ToString() != "VESSELVARIANCE")
        {
            nvc.Add("CRITERIA", "");
        }
        else
        {
            nvc.Remove("CRITERIA");
            if (VVCCYN == "1")
                nvc.Add("CRITERIA", "0");
            else
                nvc.Add("CRITERIA", subreportcode == "AVRE" ? "1" : subreportcode == "YVRE" ? "3" : "2");
        }
        Session["PHOENIXREPORTPARAMETERS"] = nvc;
        string[] rdlcfilename = new string[11];

        PhoenixSSRSReportClass.ExportSSRSReportforMerging(rdlcfilename, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, "GenerateSSRSPDF", ref _filename);

        //PhoenixSsrsReportsCommon.GetInterfaceforMerging(rdlcfilename, ds, ExportFileFormat.PDF, "GenerateSSRSPDF", ref _filename);
    }

    private void CombineAndSavePdfusingPDFSharp(string savePath, List<lstPdfFiles1> lstPdfFiles)
    {
        //using (Stream outputPdfStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
        //{
        PdfSharp.Pdf.PdfDocument targetDoc = new PdfSharp.Pdf.PdfDocument();
        for (int index = 0; index < lstPdfFiles.Count; index++)
        {
            lstPdfFiles1 List = lstPdfFiles[index];
            string file = List.FilePath;
            if (File.Exists(file))
            {
                using (PdfSharp.Pdf.PdfDocument pdfDoc = PdfSharp.Pdf.IO.PdfReader.Open(file, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import))
                {
                    for (int i = 0; i < pdfDoc.PageCount; i++)
                    {
                        targetDoc.AddPage(pdfDoc.Pages[i]);
                    }
                }
            }
        }
        targetDoc.Save(savePath);

        if (Directory.Exists(ViewState["DIRECTORYNAME"].ToString()))
        {
            Directory.Delete(ViewState["DIRECTORYNAME"].ToString(), true);
        }
        //}
    }


}