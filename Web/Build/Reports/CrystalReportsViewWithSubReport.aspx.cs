using System;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Reports;
using System.Web;


public partial class CrystalReportsViewWithSubReport : System.Web.UI.Page
{

    string[] _reportfile = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
    string _filename = "";

    protected void Page_Init(object sender, EventArgs e)
    {
        //SessionUtil.PageAccessRights(this.ViewState);

        try
        {
            //if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            //{
            //    Response.Redirect("../SSRSReports/SsrsReportsViewWithSubReport.aspx?" + Request.QueryString.ToString().Replace("&&","&"));
            //}

            string[] reportparameters = Request.QueryString.AllKeys;
            NameValueCollection nvc = new NameValueCollection();

            if (Request.QueryString["showmenu"] != null)
                Title1.ShowMenu = "false";

            if (!IsPostBack)
            {
                foreach (string s in reportparameters)
                    nvc.Add(s, Request.QueryString[s]);
                Session["PHOENIXREPORTPARAMETERS"] = nvc;
            }

            BindReport();

            string reporturl = GetReportURL(_reportfile[0]);

            SessionUtil.ReportPageAccessRights(this.ViewState, reporturl);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            if (Request.QueryString["showword"] == null || Request.QueryString["showword"].ToUpper().Equals("YES"))
                toolbarmain.AddButton("Convert to Word", "WORD");
            if (Request.QueryString["showexcel"] == null || Request.QueryString["showexcel"].ToUpper().Equals("YES"))
                toolbarmain.AddButton("Convert to Excel", "EXCEL");
            if (Request.QueryString["showpdf"] == null || Request.QueryString["showpdf"].ToUpper().Equals("YES"))
                toolbarmain.AddButton("Convert to PDF", "PDF");

            DataTable dt = PhoenixReportsCommon.GetReportCommand(int.Parse(Request.QueryString["applicationcode"].ToString()), Request.QueryString["reportcode"].ToString(), null);


            if (Request.QueryString["medicalinvoiceyn"] == "1")
            {
                DataRow dr = dt.Rows[1];
                toolbarmain.AddButton(dr["FLDCOMMANDCAPTION"].ToString(), dr["FLDCOMMAND"].ToString());
            }
            else if (Request.QueryString["emailyn"] == "2")
            {
                DataRow dr = dt.Rows[0];
                toolbarmain.AddButton(dr["FLDCOMMANDCAPTION"].ToString(), dr["FLDCOMMAND"].ToString());
            }
            else if (Request.QueryString["reffrom"] == "cl")
            {
                //do nothing
            }
            else if (Request.QueryString["frmowner"] == "yes")
            {
                if (Request.QueryString["stocktype"].ToString().ToUpper().Equals("SPARE") || Request.QueryString["stocktype"].ToString().ToUpper().Equals("SERVICE"))
                    Filter.CurrentPurchaseStockType = "SPARE";
                else
                    Filter.CurrentPurchaseStockType = "";
            }
            //else if (Request.QueryString["reportcode"].ToString() == "STATEMENTOFOWNERACCOUNTSUMMARY" || (Request.QueryString["reportcode"].ToString() == "STATEMENTOFACCOUNTSUMMARYESMBUDGET"))
            //{
            //    if (PhoenixSecurityContext.CurrentSecurityContext.UserType != "OWNER")
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
            //            if (nvc["type"].ToString() == dr["FLDCOMMAND"].ToString())
            //                toolbarmain.AddButton(dr["FLDCOMMANDCAPTION"].ToString(), dr["FLDCOMMAND"].ToString());
            //        }
            //    }
            //}
            else if (Request.QueryString["emailyn"] != "1")
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (Request.QueryString["reportcode"].ToString() == "STATEMENTOFOWNERACCOUNTSUMMARY" || (Request.QueryString["reportcode"].ToString() == "STATEMENTOFACCOUNTSUMMARYESMBUDGET"))
                    {
                        if (PhoenixSecurityContext.CurrentSecurityContext.UserType != "OWNER")
                        {
                            //nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
                            //if (nvc["type"].ToString() == dr["FLDCOMMAND"].ToString())
                                toolbarmain.AddButton(dr["FLDCOMMANDCAPTION"].ToString(), dr["FLDCOMMAND"].ToString());
                        }
                    }
                    else
                        toolbarmain.AddButton(dr["FLDCOMMANDCAPTION"].ToString(), dr["FLDCOMMAND"].ToString());
                }          
                    
            }
           


            OrderExportToPDF.AccessRights = this.ViewState;
            OrderExportToPDF.MenuList = toolbarmain.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
       

    }

    public static string GetReportURL(string reportfile)
    {
        try
        {
            string filename = reportfile.Substring(reportfile.LastIndexOf('\\') + 1, reportfile.Length - reportfile.LastIndexOf('\\') - 1);
            filename = "~/Reports/" + filename;
            return filename;
        }
        catch { return ""; }
    }

    public void BindReport()
    {
        try
        {
            PhoenixReportClass rpt = new PhoenixReportClass();
            DataSet ds = GetReportAndSubReportData();
            if (ds.Tables[0].Rows.Count > 0)
            {
                rpt.ResourceName = _reportfile[0];
                rpt.SetDataSource(ds.Tables[0]);
                rpt.Site = this.Site;
                for (int j = 1; j < ds.Tables.Count; j++)
                    rpt.Subreports[_reportfile[j]].SetDataSource(ds.Tables[j]);

                CrystalReportViewer1.Visible = true;
                CrystalReportViewer1.ReportSource = rpt;

                CrystalReportViewer1.DisplayToolbar = true;
                CrystalReportViewer1.HasPrintButton = false;
                CrystalReportViewer1.HasExportButton = false;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.DisplayGroupTree = false;
            }
            else
            {
                ucError.ErrorMessage = "No Records Found";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void OrderExportToPDF_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        DataSet ds = null;
        try
        {
            if (dce.CommandName.ToUpper().Equals("PDF"))
            {
                NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
                if (nvc["applicationcode"].ToString().Equals("4") && nvc["reportcode"].ToString().Equals("MEDICALSLIP") && nvc["t"] != null)  //Medical
                {

                    //PhoenixReportsCrew.UpdateMedicalStatus(nvc);
                }
                ds = GetReportAndSubReportData();

                PhoenixReportClass.ExportReport(_reportfile, _filename, ds);

                Response.Redirect("ReportsDownload.aspx?filename=" + _filename + "&type=pdf", false);


                return;
            }
            else if (dce.CommandName.ToUpper().Equals("WORD"))
            {
                ds = GetReportAndSubReportData();
                PhoenixReportClass.ExportReportDoc(_reportfile, ref _filename, ds);

                Response.Redirect("ReportsDownload.aspx?filename=" + _filename + "&type=word", false);

                return;
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ds = GetReportAndSubReportData();

                PhoenixReportClass.ExportReportExcel(_reportfile, ref _filename, ds);

                Response.Redirect("ReportsDownload.aspx?filename=" + _filename + "&type=excel", false);

                return;
            }
            else if (dce.CommandName.ToUpper().Equals("SENDMAIL"))
            {
                NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];

                if (nvc["applicationcode"].ToString().Equals("3"))  //Purchase
                {
                    ucPurchaseConfirmSent.HeaderMessage = "Do you want to send Email ?";
                    ucPurchaseConfirmSent.ErrorMessage = "";
                    ucPurchaseConfirmSent.Visible = true;

                    ViewState["NOTSENDORDERPLACED"] = null;
                    ViewState["QUOTATIONID"] = nvc["quotationid"].ToString();
                    if (ViewState["QUOTATIONID"] != null)
                    {
                        DataTable dtPurchasesendmail = PhoenixReportsPurchase.PurchasesendmailEdit(new Guid(ViewState["QUOTATIONID"].ToString()));
                        if (dtPurchasesendmail.Rows.Count > 0)
                        {
                            ucPurchaseConfirmSent.Remark = dtPurchasesendmail.Rows[0]["FLDMAILNOTSENDREMARKS"].ToString();
                            ucPurchaseConfirmSent.Reason = dtPurchasesendmail.Rows[0]["FLDMAILNOTSENDREASON"].ToString();

                            if (dtPurchasesendmail.Rows[0]["FLDMAILNOTSENDREASON"] != null && !string.IsNullOrEmpty(dtPurchasesendmail.Rows[0]["FLDMAILNOTSENDREASON"].ToString()))
                                ViewState["NOTSENDORDERPLACED"] = "true";
                        }
                    }
                    return;
                }
                if (nvc["applicationcode"].ToString().Equals("4") && nvc["reportcode"].ToString().Equals("WORKGEARINDIVIDUALREQUEST"))  //Crew
                {
                    SendWorkGearMail();
                }
                if (nvc["applicationcode"].ToString().Equals("4") && nvc["reportcode"].ToString().Equals("WORKINGGEARREQUEST"))  //Crew
                {
                    SendWorkGearMail();
                }
                if (Request.QueryString["applicationcode"].ToString().Equals("4") && Request.QueryString["reportcode"].ToString().Equals("HOTELBOOKINGORDER"))  //Crew Hotel Booking
                {
                    ucPurchaseConfirmSent.HeaderMessage = "Do you want to send Email ?";
                    ucPurchaseConfirmSent.ErrorMessage = "";
                    ucPurchaseConfirmSent.Visible = true;

                    ViewState["NOTSENDBOOKING"] = null;
                    ViewState["QUOTEID"] = nvc["quoteid"].ToString();
                    if (ViewState["QUOTEID"] != null)
                    {
                        DataTable dtHotelbookingsendmail = PhoenixReportsCrew.CrewHotelBookingsendmailEdit(new Guid(ViewState["QUOTEID"].ToString()));
                        if (dtHotelbookingsendmail.Rows.Count > 0)
                        {
                            ucPurchaseConfirmSent.Remark = dtHotelbookingsendmail.Rows[0]["FLDMAILNOTSENDREMARKS"].ToString();
                            ucPurchaseConfirmSent.Reason = dtHotelbookingsendmail.Rows[0]["FLDMAILNOTSENDREASON"].ToString();

                            if (dtHotelbookingsendmail.Rows[0]["FLDMAILNOTSENDREASON"] != null && !string.IsNullOrEmpty(dtHotelbookingsendmail.Rows[0]["FLDMAILNOTSENDREASON"].ToString()))
                                ViewState["NOTSENDORDERPLACED"] = "true";
                        }
                    }
                    return;
                }
                if (nvc["applicationcode"].ToString().Equals("4") && nvc["reportcode"].ToString().Equals("MEDICALSLIP") && nvc["t"] != null)  //Medical
                {

                    PhoenixReportsCrew.UpdateMedicalStatus(nvc);
                }
                if (CheckSentMail())
                {
                    ucConfirmSent.HeaderMessage = "Please Confirm";
                    ucConfirmSent.ErrorMessage = "Email already sent, Do you want to send again?";
                    ucConfirmSent.Visible = true;
                }
                else
                {
                    SendMail();
                    SentMailUpdate();
                }
                return;
            }
            else if ((dce.CommandName.ToUpper().Equals("LICAPPBACK")) || (dce.CommandName.ToUpper().Equals("COVLETTERBACK")))
            {
                Response.Redirect("../Crew/CrewLicenceProcess.aspx");
                return;
            }
            else if ((dce.CommandName.ToUpper().Equals("LICAPPBACKNEW")) || (dce.CommandName.ToUpper().Equals("COVLETTERBACKNEW")))
            {
                Response.Redirect("../Crew/CrewLicenceRequestList.aspx");
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("LICENCEAPPSERVICE"))
            {
                Response.Redirect("../Reports/CrystalReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPPSERVICE&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"]);
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("LICENCEAPPSERVICENEW"))
            {
                Response.Redirect("../Reports/CrystalReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPPSERVICENEW&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"]);
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("LICENCEAPPCOC"))
            {
                Response.Redirect("../Reports/CrystalReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPPCOC&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"]);
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("LICENCEAPPCOCNEW"))
            {
                Response.Redirect("../Reports/CrystalReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPPCOCNEW&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"]);
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("LICENCEAPPCOCSERVICE"))
            {
                Response.Redirect("../Reports/CrystalReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPPCOCSERVICE&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"]);
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("LICENCEAPPCOCSERVICENEW"))
            {
                Response.Redirect("../Reports/CrystalReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPPCOCSERVICENEW&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"]);
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Reports/CrystalReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPP&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"] + "&showword=" + Request.QueryString["showword"] + "&showexcel=" + Request.QueryString["showexcel"]);
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("BACKNEW"))
            {
                Response.Redirect("../Reports/CrystalReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPPNEW&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"] + "&showword=" + Request.QueryString["showword"] + "&showexcel=" + Request.QueryString["showexcel"]);
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("INVOICE"))
            {
                Response.Redirect("../Accounts/AccountsInvoiceLineItemDetails.aspx?" + GetQueryString(), false);
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("CONTRACTBACK"))
            {
                if (HttpContext.Current.Request.QueryString["empid"] != null)
                    Response.Redirect("../Crew/CrewContract.aspx?empid=" + Request.QueryString["empid"] + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + Request.QueryString["date"] + "&planid=" + Request.QueryString["planid"], false);
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("INVOICEPAYMENTVOUCHERBACK"))
            {
                Response.Redirect("../Accounts/AccountsInvoicePaymentVoucherLineItemDetails.aspx?" + GetQueryString(), false);
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("CTMPAYMENTVOUCHERBACK"))
            {
                Response.Redirect("../Accounts/AccountsCtmPaymentVoucherLineItemDetails.aspx?" + GetQueryString(), false);
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("PARTICIPANTSFEEDBACK"))
            {

                Response.Redirect("../Crew/CrewCourseFeedBack.aspx?empid=" + Request.QueryString["employeeid"] + "&batchid=" + Request.QueryString["batchid"], false);
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("CONFIRM"))
            {
                NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];

                if (nvc["applicationcode"].ToString().Equals("9"))  //Inspection
                {
                    PhoenixReportsInspection.AuditReportGenerationUpdate(new Guid(nvc["reviewscheduleid"].ToString()));
                    ucStatus.Text = "Report is confirmed.";
                }
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("STATEMENTOFOWNERACCOUNTSUMMARY") || dce.CommandName.ToUpper().Equals("STATEMENTOFACCOUNTSUMMARYESMBUDGET"))
            {
                NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
                DataSet ds1 = PhoenixReportsAccount.SOAGenerationOwnerReportVerification(int.Parse(nvc["ownerid"].ToString()), new Guid(nvc["debitnoteid"].ToString()), nvc["subreportcode"].ToString());
                if (ds1.Tables.Count > 0)
                {
                    ucConfirm.ErrorMessage = ds1.Tables[0].Rows[0]["FLDMSG"].ToString();
                    ucConfirm.Visible = true;
                }
                return;
            }


            DataTable dt = PhoenixReportsCommon.GetReportCommand(int.Parse(Request.QueryString["applicationcode"].ToString()), Request.QueryString["reportcode"].ToString(), null);

            if (dt.Rows[0]["FLDURL"].ToString().Contains("?"))
                Response.Redirect(dt.Rows[0]["FLDURL"].ToString() + "&" + GetQueryString());
            else
                Response.Redirect(dt.Rows[0]["FLDURL"].ToString() + "?" + GetQueryString());
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void SendMail(DataSet ds)
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        PhoenixReportsCommon.SendMail(nvc, ds, _filename);
    }


    private string GetQueryString()
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        string buffer = "";
        if (nvc != null)
        {
            foreach (string s in nvc.AllKeys)
            {
                if (buffer.Length == 0)
                    buffer = s + "=" + nvc[s];
                else
                    buffer = buffer + "&" + s + "=" + nvc[s];
            }
        }
        return buffer;
    }

    private DataSet GetReportAndSubReportData()
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        return PhoenixReportsCommon.GetReportAndSubReportData(nvc, ref _reportfile, ref _filename);
    }

    private bool CheckSentMail()
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        return PhoenixReportsCommon.CheckSentMail(nvc);
    }

    protected void ucConfirmSent_OnClick(object sender, EventArgs e)
    {
        if (ucConfirmSent.confirmboxvalue == 1)
        {
            SendMail();
        }
    }

    protected void ucpurchaseConfirmSent_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["reportcode"].ToString().Equals("HOTELBOOKINGORDER"))
            {
                if (ucPurchaseConfirmSent.confirmboxvalue == 1)
                {
                    if (CheckSentMail())
                    {
                        if (ViewState["NOTSENDORDERPLACED"] != null && ViewState["NOTSENDORDERPLACED"].ToString().ToUpper() == "TRUE")
                        {
                            HotelBookingsendmailupdate(null, null, 0); // 0--> not to update send date, bec it's already updated...
                            SendMail();
                        }
                        else
                        {
                            ucPurchaseConfirmSent.Visible = false;
                            ucConfirmSent.HeaderMessage = "Please Confirm";
                            ucConfirmSent.ErrorMessage = "Email already sent, Do you want to send again?";
                            ucConfirmSent.Visible = true;
                        }
                    }
                    else
                    {
                        SentMailUpdate();
                        HotelBookingsendmailupdate(null, null, 1);
                        SendMail();

                    }
                    return;
                }
                else
                {
                    if (!CheckSentMail())
                    {
                        SentMailUpdate();
                        HotelBookingsendmailupdate(ucPurchaseConfirmSent.Reason, ucPurchaseConfirmSent.Remark, 1);
                        BindReport();
                    }
                    else
                    {
                        ucError.ErrorMessage = "Booking already done, you can not do more changes on this.";
                        ucError.Visible = true;
                        return;
                    }
                }

            }
            else
            {
                if (ucPurchaseConfirmSent.confirmboxvalue == 1)
                {
                    if (CheckSentMail())
                    {
                        if (ViewState["NOTSENDORDERPLACED"] != null && ViewState["NOTSENDORDERPLACED"].ToString().ToUpper() == "TRUE")
                        {
                            Purchasesendmailupdate(null, null, 0); // o--> not to update send date, bec it's already updated...
                            SendMail();
                        }
                        else
                        {
                            ucPurchaseConfirmSent.Visible = false;
                            ucConfirmSent.HeaderMessage = "Please Confirm";
                            ucConfirmSent.ErrorMessage = "Email already sent, Do you want to send again?";
                            ucConfirmSent.Visible = true;
                        }
                    }
                    else
                    {
                        SentMailUpdate();
                        Purchasesendmailupdate(null, null, 1);
                        SendMail();
                    }
                    return;
                }
                else
                {
                    if (!CheckSentMail())
                    {
                        SentMailUpdate();
                        Purchasesendmailupdate(ucPurchaseConfirmSent.Reason, ucPurchaseConfirmSent.Remark, 1);
                        BindReport();
                    }
                    else
                    {
                        ucError.ErrorMessage = "Order already Placed, you can not do more changes on this.";
                        ucError.Visible = true;
                        return;
                    }

                }

            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void Purchasesendmailupdate(string reason, string remarks, int? senddateupdate)
    {
        if (ViewState["QUOTATIONID"] != null)
            PhoenixReportsPurchase.Purchasesendmailupdate(new Guid(ViewState["QUOTATIONID"].ToString()), General.GetNullableString(reason), General.GetNullableString(remarks), senddateupdate);
    }


    public void HotelBookingsendmailupdate(string reason, string remarks, int? senddateupdate)
    {
        if (ViewState["QUOTEID"] != null)
            PhoenixReportsCrew.CrewHotelBookingsendmailUpdate(new Guid(ViewState["QUOTEID"].ToString()), General.GetNullableString(reason), General.GetNullableString(remarks), senddateupdate);
    }

    public void SendMail()
    {
        try
        {

            DataSet ds = GetReportAndSubReportData();
            PhoenixReportClass.ExportReport(_reportfile, _filename, ds);
            SendMail(ds);
            ucConfirm.ErrorMessage = "Purchase Order Email sent";
            ucConfirm.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SendWorkGearMail()
    {
        try
        {
            DataSet ds = GetReportAndSubReportData();
            PhoenixReportClass.ExportReport(_reportfile, _filename, ds);
            SendMail(ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SentMailUpdate()
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        if (Request.QueryString["reportcode"].ToString().Equals("HOTELBOOKINGORDER"))
        {
            PhoenixReportsCommon.CheckSentMail(nvc);
        }
        else
            PhoenixReportsCommon.UpdateSentMail(nvc);
    }
}