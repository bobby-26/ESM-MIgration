using System;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Reports;

public partial class CrystalReportsView : System.Web.UI.Page
{
    string _reportfile = "";
    string _filename = "";


    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {

            //if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            //{
            //    Response.Redirect("../SSRSReports/SsrsReportsView.aspx?" + Request.QueryString.ToString());
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
            string reporturl = GetReportURL(_reportfile);

            SessionUtil.ReportPageAccessRights(this.ViewState, reporturl);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            if (Request.QueryString["showword"] == null || Request.QueryString["showword"].ToUpper().Equals("YES"))
                toolbarmain.AddButton("Convert to Word", "WORD");
            if (Request.QueryString["showexcel"] == null || Request.QueryString["showexcel"].ToUpper().Equals("YES"))
                toolbarmain.AddButton("Convert to Excel", "EXCEL");
            if (Request.QueryString["showpdf"] == null || Request.QueryString["showpdf"].ToUpper().Equals("YES"))
                toolbarmain.AddButton("Convert to PDF", "PDF");

            if (Request.QueryString["showprint"] != null && Request.QueryString["showprint"].ToUpper().Equals("YES"))
                toolbarmain.AddButton("Print", "PRINT");

            DataTable dt = PhoenixReportsCommon.GetReportCommand(int.Parse(Request.QueryString["applicationcode"].ToString()), Request.QueryString["reportcode"].ToString(), null);
            foreach (DataRow dr in dt.Rows)
            {
                if (Request.QueryString["reportcode"].ToString() == "VESSELVARIANCE" || Request.QueryString["reportcode"].ToString() == "VESSELTRAILBALANCE" || Request.QueryString["reportcode"].ToString() == "VESSELSUMMARYBALANCE" || Request.QueryString["reportcode"].ToString() == "STATEMENTOFOWNERACCOUNTSUMMARY")
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserType != "OWNER")
                    {
                        nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
                        if (nvc["type"].ToString() == dr["FLDCOMMAND"].ToString())
                            toolbarmain.AddButton(dr["FLDCOMMANDCAPTION"].ToString(), dr["FLDCOMMAND"].ToString());
                    }
                }
                else
                {
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
    protected void Page_Load(object sender, EventArgs e)
    {  
    }

    public void BindReport()
    {
        try
        {
            PhoenixReportClass rpt = new PhoenixReportClass();
            DataTable dt = GetReportData();


            rpt.ResourceName = _reportfile;
            rpt.SetDataSource(dt);
            rpt.Site = this.Site;

            CrystalReportViewer1.Visible = true;
            CrystalReportViewer1.ReportSource = rpt;

            CrystalReportViewer1.DisplayToolbar = true;
            CrystalReportViewer1.HasPrintButton = false;
            CrystalReportViewer1.HasExportButton = false;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
            CrystalReportViewer1.DisplayGroupTree = false;

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
        DataTable dt = null;
        try
        {
            if (dce.CommandName.ToUpper().Equals("PDF"))
            {
                NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
                dt = GetReportData();

                PhoenixReportClass.ExportReport(_reportfile, _filename, dt);

                Response.Redirect("ReportsDownload.aspx?filename=" + _filename + "&type=pdf");

                return;
            }
            else if (dce.CommandName.ToUpper().Equals("WORD"))
            {
                dt = GetReportData();

                PhoenixReportClass.ExportReportDoc(_reportfile, ref _filename, dt);

                Response.Redirect("ReportsDownload.aspx?filename=" + _filename + "&type=word");

                return;
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                dt = GetReportData();

                PhoenixReportClass.ExportReportExcel(_reportfile, ref _filename, dt);

                Response.Redirect("ReportsDownload.aspx?filename=" + _filename + "&type=excel");

                return;
            }
            else if (dce.CommandName.ToUpper().Equals("PRINT"))
            {
                PhoenixReportClass.PrintToPrinter(_reportfile, dt);
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("SENDMAIL"))
            {
                NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];

                if (Request.QueryString["applicationcode"].ToString().Equals("4") && Request.QueryString["reportcode"].ToString().Equals("WORKINGGEARREQUEST"))  //Crew
                {
                    SendWorkGearBulkRequestMail();
                }

                if (CheckSentMail())
                {
                    ucConfirmSent.HeaderMessage = "Please Confirm";
                    ucConfirmSent.ErrorMessage = "Email already sent, Do you want to send again?";
                    ucConfirmSent.Visible = true;
                }
                else
                {
                    SentMailUpdate();
                    SendMail();
                }
                return;
            }
            else if ((dce.CommandName.ToUpper().Equals("MONTHLYOVERTIMESUMMARY")))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsWRHOTDetail.aspx");
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("TRAVELCLAIMPOST"))
            {
                PhoenixReportsAccount.GenerateAndSaveDebitnotePdf(Request.QueryString["VisitId"].ToString(), Request.QueryString["TravelClaimId"].ToString());
                Response.Redirect("../Reports/CrystalReportsView.aspx?applicationcode=5&reportcode=TRAVELCLAIMPOST&showmenu=0&VisitId=" + Request.QueryString["VisitId"].ToString() + "&TravelClaimId=" + Request.QueryString["TravelClaimId"].ToString());
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("TRAVELCLAIMBACK"))
            {
                Response.Redirect("../Reports/CrystalReportsView.aspx?applicationcode=5&reportcode=CLAIMDEBITNOTEVIEW&showmenu=0&VisitId=" + Request.QueryString["VisitId"].ToString() + "&TravelClaimId=" + Request.QueryString["TravelClaimId"].ToString());
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("BANKRECONCILATION"))
            {
                PhoenixReportsAccount.BankStatementReconValidationUpdate(new Guid(Request.QueryString["Uploadid"].ToString()));
                Response.Redirect("../Reports/CrystalReportsView.aspx?applicationcode=5&reportcode=BANKRECONCILATION&showmenu=0&uploadid=" + Request.QueryString["Uploadid"].ToString());
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("TRAVELCLAIMPOSTING"))
            {
                PhoenixReportsAccount.TravelClaimPosting(new Guid(Request.QueryString["VisitId"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                Response.Redirect("../Reports/CrystalReportsView.aspx?applicationcode=5&reportcode=TRAVELCLAIMPOST&showmenu=0&VisitId=" + Request.QueryString["VisitId"].ToString() + "&TravelClaimId=" + Request.QueryString["TravelClaimId"].ToString());
                return;
            }
            else if (dce.CommandName.ToUpper().Equals("MONTHLY") || dce.CommandName.ToUpper().Equals("YEARLY") || dce.CommandName.ToUpper().Equals("ACCUMULATED") || dce.CommandName.ToUpper().Equals("VESSELTRAILBALANCE") || dce.CommandName.ToUpper().Equals("VESSELSUMMARYBALANCE") || dce.CommandName.ToUpper().Equals("STATEMENTOFOWNERACCOUNTSUMMARY") || dce.CommandName.ToUpper().Equals("VESSELTRAILBALANCEYTD") || dce.CommandName.ToUpper().Equals("VESSELTRAILBALANCEYTDOWNER"))
            {
                NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
                DataSet ds = PhoenixReportsAccount.SOAGenerationOwnerReportVerification(int.Parse(nvc["ownerid"].ToString()), new Guid(nvc["debitnoteid"].ToString()), nvc["subreportcode"].ToString());
                if (ds.Tables.Count > 0)
                {
                    ucConfirm.ErrorMessage = ds.Tables[0].Rows[0]["FLDMSG"].ToString();
                    ucConfirm.Visible = true;
                }                
                return;
            }
            dt = PhoenixReportsCommon.GetReportCommand(int.Parse(Request.QueryString["applicationcode"].ToString()), Request.QueryString["reportcode"].ToString(), dce.CommandName.ToUpper());
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

    public void SendWorkGearBulkRequestMail()
    {
        try
        {
            DataTable dt = GetReportData();
            PhoenixReportClass.ExportReport(_reportfile, _filename, dt);
            SendMail(dt);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SendMail(DataTable dt)
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        PhoenixReportsCommon.SendMail(nvc, dt, _filename);
    }

    private bool CheckSentMail()
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        return PhoenixReportsCommon.CheckSentMail(nvc);
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

    private DataTable GetReportData()
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        return PhoenixReportsCommon.GetReportData(nvc, ref _reportfile, ref _filename);
    }

    protected void ucConfirmSent_OnClick(object sender, EventArgs e)
    {
        if (ucConfirmSent.confirmboxvalue == 1)
        {
            SendMail();
        }
    }
    public void SendMail()
    {
        DataTable dt = null;
        dt = GetReportData();
        PhoenixReportClass.ExportReport(_reportfile, _filename, dt);
        SendMail(dt);
        ucConfirm.ErrorMessage = "Email sent";
        ucConfirm.Visible = true;
    }

    public void SentMailUpdate()
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        PhoenixReportsCommon.UpdateSentMail(nvc);
    }
}