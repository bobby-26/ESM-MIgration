using System;
using System.Data;
using System.Collections.Specialized;
using SouthNests.Phoenix.Reports;
using Microsoft.Reporting.WebForms;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;

using System.Collections.Generic;
using Telerik.Web.UI;

public partial class SSRSReports_SsrsReportsViewWithSubReport : PhoenixBasePage
{
    string ifr = string.Empty;

    public enum ExportFileFormat
    {
        PDF
         , Excel
         , Word
    }

    string[] _reportfile = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
    string _filename = "";
    protected void Page_Init(object sender, EventArgs e)
    {
        //SessionUtil.PageAccessRights(this.ViewState);

        var dir = HttpContext.Current.Server.MapPath("../Attachments/reffromTemp");  // folder location

        if (!Directory.Exists(dir))  // if it doesn't exist, create
            Directory.CreateDirectory(dir);


        string[] reportparameters = Request.QueryString.AllKeys;
        NameValueCollection nvc = new NameValueCollection();

        //if (Request.QueryString["showmenu"] != null)
        //    Title1.ShowMenu = "false";

        if (!IsPostBack)
        {
            foreach (string s in reportparameters)
                nvc.Add(s, Request.QueryString[s]);

            Session["PHOENIXREPORTPARAMETERS"] = nvc;
        }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            DataTable dt = PhoenixSsrsReportsCommon.GetReportCommand(int.Parse(Request.QueryString["applicationcode"].ToString()), Request.QueryString["reportcode"].ToString(), null);
            if (Request.QueryString["medicalinvoiceyn"] == "1")
            {
                DataRow dr = dt.Rows[1];
                toolbarmain.AddButton(dr["FLDCOMMANDCAPTION"].ToString(), dr["FLDCOMMAND"].ToString(),ToolBarDirection.Right);
            }
            else if (Request.QueryString["emailyn"] == "2")
            {
                DataRow dr = dt.Rows[0];
                toolbarmain.AddButton(dr["FLDCOMMANDCAPTION"].ToString(), dr["FLDCOMMAND"].ToString(), ToolBarDirection.Right);
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
            else if (Request.QueryString["emailyn"] != "1")
            {
                if (Request.QueryString["placeorder"] != "no")
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Request.QueryString["reportcode"].ToString() == "STATEMENTOFOWNERACCOUNTSUMMARY" || (Request.QueryString["reportcode"].ToString() == "STATEMENTOFACCOUNTSUMMARYESMBUDGET") || (Request.QueryString["reportcode"].ToString() == "STATEMENTOFOWNERACCOUNTWITHOUTBUDGET"))
                        {
                            if (PhoenixSecurityContext.CurrentSecurityContext.UserType != "OWNER")
                            {                                
                                toolbarmain.AddButton(dr["FLDCOMMANDCAPTION"].ToString(), dr["FLDCOMMAND"].ToString(), ToolBarDirection.Right);
                            }
                        }
                        else
                            toolbarmain.AddButton(dr["FLDCOMMANDCAPTION"].ToString(), dr["FLDCOMMAND"].ToString(), ToolBarDirection.Right);
                    }
                }
            }

            List<MenuLink> m = new List<MenuLink>();
            m = toolbarmain.Show();
            
           
            OrderExportToPDF.AccessRights = this.ViewState; 
            //if (m.Count>0)
            //{
                    OrderExportToPDF.MenuList = toolbarmain.Show();
            //}

    }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {

            if (Request.QueryString["ifr"] != null)
                ifr = "0";

            ReportViewer1.LocalReport.SubreportProcessing += new

                             SubreportProcessingEventHandler(SubreportProcessingEventHandler);

            this.ReportViewer1.LocalReport.Refresh();



            BindReport();
        }
    }
    void SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        DataSet ds = (DataSet)Session["SUBREPORT"];
        SetReportDataSource(e.ReportPath, nvc, ds, e.DataSources);
    }
    public void BindReport()
    {
        try
        {
            NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
            PhoenixReportClass rpt = new PhoenixReportClass();
            nvc.Remove("CRITERIA");
            nvc.Add("CRITERIA", "");
            DataSet ds = GetReportAndSubReportData();
            Session["SUBREPORT"] = ds;

            PhoenixSsrsReportsCommon.LoadSsrsReport(nvc);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //if (_reportfile.Count(s => s != "") > 0)
            //{

            rpt.ResourceName = PhoenixSsrsReportsCommon.GetSsrsMainReport(int.Parse(nvc["applicationcode"].ToString()), nvc["reportcode"].ToString(), nvc["CRITERIA"]);
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = rpt.ResourceName;
            ReportViewer1.LocalReport.EnableExternalImages = true;

            if (_filename.LastIndexOf("\\") > 0)
                ReportViewer1.LocalReport.DisplayName = _filename.Substring(_filename.LastIndexOf("\\") + 1).Replace(".pdf", "");

            if (nvc["LogoPath"] != null && nvc["LogoPath"].ToString() != "")
            {
                string imagePath = new Uri(Server.MapPath("~/css/Theme1/images/" + Path.GetFileName(nvc["LogoPath"].ToString()))).AbsoluteUri;
                ReportParameter LogoPath = new ReportParameter("imagepath", imagePath);
                ReportViewer1.LocalReport.SetParameters(LogoPath);
            }
            if (nvc["Version"] != null && nvc["Version"].ToString() != "")
            {

                string strVersion = nvc["Version"].ToString();
                ReportParameter Version = new ReportParameter("Version", strVersion);
                ReportViewer1.LocalReport.SetParameters(Version);

            }

            if (ds.Tables[0].Rows.Count == 0)
            {
                ds.Tables[0].Rows.Add();
            }

            ReportViewer1.LocalReport.DataSources.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                SetReportDataSource(rpt.ResourceName.Substring(rpt.ResourceName.LastIndexOf("\\") + 1).Replace(".rdlc", ""), nvc, ds, ReportViewer1.LocalReport.DataSources);
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetReportDataSource(string ReportName, NameValueCollection nvc, DataSet ds, ReportDataSourceCollection e)
    {
        if (ds.Tables.Count > 0)
        {
            List<string> rpt = new List<string>();
            foreach (string s in nvc.Keys)
            {
                if (s.Contains(ReportName + ".rdlc"))
                {
                    rpt.Add(nvc[s] + "," + s.Substring(s.IndexOf(".rdlc")).Replace(".rdlc", ""));
                }
            }

            for (int i = 0; i < rpt.Count; i++)
            {
                int index = 0;
                int.TryParse(rpt[i].Split(',')[1], out index);
                if (ds.Tables[index].Rows.Count == 0)
                {
                    ds.Tables[index].Rows.Add();
                }
                e.Add(new ReportDataSource(rpt[i].Split(',')[0], ds.Tables[index]));
            }
        }
    }
    protected void OrderExportToPDF_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        //DataSet ds = null;
        //Guid? quotationid = null;
        try
        {

            if (CommandName.ToUpper().Equals("SENDMAIL"))
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

            else if ((CommandName.ToUpper().Equals("LICAPPBACK")) || (CommandName.ToUpper().Equals("COVLETTERBACK")))
            {
                Response.Redirect("../Crew/CrewLicenceProcess.aspx");
                return;
            }
            else if ((CommandName.ToUpper().Equals("LICAPPBACKNEW")) || (CommandName.ToUpper().Equals("COVLETTERBACKNEW")))
            {
                Response.Redirect("../Crew/CrewLicenceRequestList.aspx");
                return;
            }
            else if (CommandName.ToUpper().Equals("LICENCEAPPSERVICE"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPPSERVICE&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"]);
                return;
            }
            else if (CommandName.ToUpper().Equals("LICENCEAPPSERVICENEW"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPPSERVICENEW&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"]);
                return;
            }
            else if (CommandName.ToUpper().Equals("LICENCEAPPCOC"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPPCOC&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"]);
                return;
            }
            else if (CommandName.ToUpper().Equals("LICENCEAPPCOCNEW"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPPCOCNEW&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"]);
                return;
            }
            else if (CommandName.ToUpper().Equals("LICENCEAPPCOCSERVICE"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPPCOCSERVICE&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"]);
                return;
            }
            else if (CommandName.ToUpper().Equals("LICENCEAPPCOCSERVICENEW"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPPCOCSERVICENEW&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"]);
                return;
            }
            else if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPP&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"] + "&showword=" + Request.QueryString["showword"] + "&showexcel=" + Request.QueryString["showexcel"]);
                return;
            }
            else if (CommandName.ToUpper().Equals("BACKNEW"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=LICENCEAPPNEW&flagid=" + Request.QueryString["flagid"] + "&processid=" + Request.QueryString["processid"] + "&refno=" + Request.QueryString["strRefNo"] + "&showword=" + Request.QueryString["showword"] + "&showexcel=" + Request.QueryString["showexcel"]);
                return;
            }
            else if (CommandName.ToUpper().Equals("OWNERPROPOSALSEASERVICE"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=OWNERPROPOSALSEASERVICE&employeeid=" + Request.QueryString["employeeid"] );
                return;
            }
            else if (CommandName.ToUpper().Equals("OWNERPROPOSAL"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=OWNERPROPOSAL&employeeid=" + Request.QueryString["employeeid"] );
                return;
            }
            else if (CommandName.ToUpper().Equals("CREWSUPTEVAL"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=CREWSUPTEVAL&suptevalid=" + Request.QueryString["suptevalid"]);
                return;
            }
            else if (CommandName.ToUpper().Equals("INVOICE"))
            {
                Response.Redirect("../Accounts/AccountsInvoiceLineItemDetails.aspx?" + GetQueryString(), false);
                return;
            }
            else if (CommandName.ToUpper().Equals("CONTRACTBACK"))
            {
                if (HttpContext.Current.Request.QueryString["empid"] != null && HttpContext.Current.Request.QueryString["lf"]==null)
                    Response.Redirect("../Crew/CrewContract.aspx?empid=" + Request.QueryString["empid"] + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + Request.QueryString["date"] + "&planid=" + Request.QueryString["planid"], false);
                else if (HttpContext.Current.Request.QueryString["empid"] != null && HttpContext.Current.Request.QueryString["lf"]!=null)
                    Response.Redirect("../CrewOffshore/CrewOffshoreAppointmentLetter.aspx?employeeid=" + Request.QueryString["empid"] + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + Request.QueryString["date"] + "&crewplanid=" + Request.QueryString["planid"] + "&appointmentletterid=" + Request.QueryString["appointmentletterid"] + "&popup=1&lf=" + Request.QueryString["lf"], false);

                //CrewOffshoreAppointmentLetter.aspx? employeeid = 19403 & crewplanid = 262acfdb - dcc7 - ea11 - 98e2 - 8cec4b9f0c4d & appointmentletterid = 746d5425 - b0ca - ea11 - 98e2 - 8cec4b9f0c4d & popup = 1
                return;
            }
            else if (CommandName.ToUpper().Equals("INVOICEPAYMENTVOUCHERBACK"))
            {
                Response.Redirect("../Accounts/AccountsInvoicePaymentVoucherLineItemDetails.aspx?" + GetQueryString(), false);
                return;
            }
            else if (CommandName.ToUpper().Equals("CTMPAYMENTVOUCHERBACK"))
            {
                Response.Redirect("../Accounts/AccountsCtmPaymentVoucherLineItemDetails.aspx?" + GetQueryString(), false);
                return;
            }
            else if (CommandName.ToUpper().Equals("PARTICIPANTSFEEDBACK"))
            {

                Response.Redirect("../Crew/CrewCourseFeedBack.aspx?empid=" + Request.QueryString["employeeid"] + "&batchid=" + Request.QueryString["batchid"], false);
                return;
            }
            else if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];

                if (nvc["applicationcode"].ToString().Equals("9"))  //Inspection
                {
                    PhoenixReportsInspection.AuditReportGenerationUpdate(new Guid(nvc["reviewscheduleid"].ToString()));
                    ucStatus.Text = "Report is confirmed.";
                }
                return;
            }
            else if (CommandName.ToUpper().Equals("STATEMENTOFOWNERACCOUNTSUMMARY") || CommandName.ToUpper().Equals("STATEMENTOFACCOUNTSUMMARYESMBUDGET") || CommandName.ToUpper().Equals("STATEMENTOFOWNERACCOUNTWITHOUTBUDGET"))
            {
                NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
                DataSet ds1 = PhoenixReportsAccount.SOAGenerationOwnerReportVerification(int.Parse(nvc["ownerid"].ToString()), new Guid(nvc["debitnoteid"].ToString()), nvc["subreportcode"].ToString());
                if (ds1.Tables.Count > 0)
                {
                    ucConfirm.ErrorMessage = ds1.Tables[0].Rows[0]["FLDMSG"].ToString();
                    ucConfirm.Visible = true;
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('verified'," + (ifr == "0" ? "null" : "'filterandsearch'") + ",null);", true);
                return;
            }
            DataTable dt = PhoenixSsrsReportsCommon.GetReportCommand(int.Parse(Request.QueryString["applicationcode"].ToString()), Request.QueryString["reportcode"].ToString(), null);

            if (dt.Rows[0]["FLDURL"].ToString().Contains("?"))
                Response.Redirect(dt.Rows[0]["FLDURL"].ToString() + "&" + GetQueryString());
            else
                Response.Redirect(dt.Rows[0]["FLDURL"].ToString() + "?" + GetQueryString());
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
                ucError.ErrorMessage = ex.Message + ex.InnerException;
            else
                ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
            if (ex.InnerException != null)
                ucError.ErrorMessage = ex.Message + ex.InnerException;
            else
                ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void Purchasesendmailupdate(string reason, string remarks, int? senddateupdate)
    {
        if (ViewState["QUOTATIONID"] != null)
            PhoenixReportsPurchase.Purchasesendmailupdate(new Guid(ViewState["QUOTATIONID"].ToString()), General.GetNullableString(reason), General.GetNullableString(remarks), senddateupdate);
    }

    private bool CheckSentMail()
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        return PhoenixSsrsReportsCommon.CheckSentMail(nvc);
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
            ExportSSRSReport(ds, ExportFileFormat.PDF);

            SendMail(ds);
            ucStatus.Text = "Purchase Order Email sent";
            ucStatus.Visible = true;
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
                ucError.ErrorMessage = ex.Message + ex.InnerException;
            else
                ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void ExportSSRSReport(DataSet ds, ExportFileFormat Format)
    {

        try
        {
            NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
            PhoenixReportClass rpt = new PhoenixReportClass();
            //ArchangelReportClass rpt = new ArchangelReportClass();
            nvc.Remove("CRITERIA");
            nvc.Add("CRITERIA", "");
            ds = GetReportAndSubReportData();
            Session["SUBREPORT"] = ds;

            PhoenixSsrsReportsCommon.LoadSsrsReport(nvc);
            if (ds.Tables[0].Rows.Count > 0)
            {

                rpt.ResourceName = PhoenixSsrsReportsCommon.GetSsrsMainReport(int.Parse(nvc["applicationcode"].ToString()), nvc["reportcode"].ToString(), nvc["CRITERIA"]);
                ReportViewer1.Visible = true;
                ReportViewer1.LocalReport.ReportPath = rpt.ResourceName;
                ReportViewer1.LocalReport.EnableExternalImages = true;

                if (nvc["LogoPath"] != null && nvc["LogoPath"].ToString() != "")
                {
                    string imagePath = new Uri(Server.MapPath("~/css/Theme1/images/" + Path.GetFileName(nvc["LogoPath"].ToString()))).AbsoluteUri;
                    ReportParameter LogoPath = new ReportParameter("imagepath", imagePath);
                    ReportViewer1.LocalReport.SetParameters(LogoPath);

                }
                if (nvc["Version"] != null && nvc["Version"].ToString() != "")
                {

                    string strVersion = nvc["Version"].ToString();
                    ReportParameter Version = new ReportParameter("Version", strVersion);
                    ReportViewer1.LocalReport.SetParameters(Version);

                }

                ReportViewer1.LocalReport.DataSources.Clear();
                SetReportDataSource(rpt.ResourceName.Substring(rpt.ResourceName.LastIndexOf("\\") + 1).Replace(".rdlc", ""), nvc, ds, ReportViewer1.LocalReport.DataSources);
                ReportViewer1.LocalReport.SubreportProcessing += new

                             SubreportProcessingEventHandler(SubreportProcessingEventHandler);

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = ReportViewer1.LocalReport.Render(
                                Format.ToString(), null, out mimeType, out encoding,
                                out extension,
                                out streamids, out warnings);

                if (Directory.Exists(_filename))
                {
                    System.IO.File.Delete(_filename);
                }

                FileStream fs = new FileStream( _filename, FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    private void SendMail(DataSet ds)
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        PhoenixSsrsReportsCommon.SendMail(nvc, ds, _filename);
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
        return PhoenixSsrsReportsCommon.GetReportAndSubReportData(nvc, ref _reportfile, ref _filename);

    }
    protected void ReportViewer_OnLoad(object sender, EventArgs e)
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        if (nvc["showword"] == null || nvc["showword"].ToUpper().Equals("YES"))
        {
            string exportOptionWord = "Word";
            RenderingExtension extension = ReportViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(exportOptionWord, StringComparison.CurrentCultureIgnoreCase));

            System.Reflection.FieldInfo fieldInfo = extension.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(extension, true);
        }
        else
        {
            string exportOptionWord = "Word";
            RenderingExtension extension = ReportViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(exportOptionWord, StringComparison.CurrentCultureIgnoreCase));

            System.Reflection.FieldInfo fieldInfo = extension.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(extension, false);

        }
        if (nvc["showexcel"] == null || nvc["showexcel"].ToUpper().Equals("YES"))
        {
            string exportOptionExcel = "Excel";
            RenderingExtension extension = ReportViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(exportOptionExcel, StringComparison.CurrentCultureIgnoreCase));

            System.Reflection.FieldInfo fieldInfo = extension.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(extension, true);
        }
        else
        {
            string exportOptionExcel = "Excel";
            RenderingExtension extension = ReportViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(exportOptionExcel, StringComparison.CurrentCultureIgnoreCase));

            System.Reflection.FieldInfo fieldInfo = extension.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(extension, false);
        }
        if (nvc["showpdf"] == null || nvc["showpdf"].ToUpper().Equals("YES"))
        {
            string exportOptionPdf = "PDF";
            RenderingExtension extension = ReportViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(exportOptionPdf, StringComparison.CurrentCultureIgnoreCase));

            System.Reflection.FieldInfo fieldInfo = extension.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(extension, true);
        }
        else
        {
            string exportOptionPdf = "PDF";
            RenderingExtension extension = ReportViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(exportOptionPdf, StringComparison.CurrentCultureIgnoreCase));

            System.Reflection.FieldInfo fieldInfo = extension.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(extension, false);
        }

    }
    protected void ucConfirmSent_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ucConfirmSent.confirmboxvalue == 1)
            {
                SendMail();
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            if (ex.InnerException != null)
                ucError.ErrorMessage = ex.Message + ex.InnerException;
            else
                ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SendWorkGearMail()
    {
        try
        {
            DataSet ds = GetReportAndSubReportData();
            ExportSSRSReport(ds, ExportFileFormat.PDF);
            //PhoenixReportClass.ExportReport(_reportfile, _filename, ds);
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
            PhoenixSsrsReportsCommon.CheckSentMail(nvc);
        }
        else
            PhoenixSsrsReportsCommon.UpdateSentMail(nvc);
    }
}