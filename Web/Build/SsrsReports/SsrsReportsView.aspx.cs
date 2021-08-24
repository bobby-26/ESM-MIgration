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
using System.Collections.Generic;
using System.Web.UI;
using Telerik.Web.UI;

public partial class SSRSReports_SsrsReportsView : PhoenixBasePage
{
    string ifr = string.Empty;

    public enum ExportFileFormat
    {
        PDF
        , Excel
        , Word
    }
    string _reportfile = "";
    string _filename = "";
    protected void Page_Init(object sender, EventArgs e)
    {
        //SessionUtil.PageAccessRights(this.ViewState);

        var dir = HttpContext.Current.Server.MapPath("../Attachments/Temp");  // folder location

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
        //string reporturl = GetReportURL(_reportfile);
        //SessionUtil.ReportPageAccessRights(this.ViewState, reporturl);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        DataTable dt = PhoenixSsrsReportsCommon.GetReportCommand(int.Parse(Request.QueryString["applicationcode"].ToString()), Request.QueryString["reportcode"].ToString(), null);
        foreach (DataRow dr in dt.Rows)
        {
            if (Request.QueryString["reportcode"].ToString() == "VESSELVARIANCE" || Request.QueryString["reportcode"].ToString() == "VESSELTRAILBALANCE" || Request.QueryString["reportcode"].ToString() == "VESSELSUMMARYBALANCE" || Request.QueryString["reportcode"].ToString() == "STATEMENTOFOWNERACCOUNTSUMMARY")
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.UserType != "OWNER")
                {
                    nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
                    if (nvc["type"].ToString() == dr["FLDCOMMAND"].ToString())
                        toolbarmain.AddButton(dr["FLDCOMMANDCAPTION"].ToString(), dr["FLDCOMMAND"].ToString(),ToolBarDirection.Right);
                }
            }
            else
            {
                toolbarmain.AddButton(dr["FLDCOMMANDCAPTION"].ToString(), dr["FLDCOMMAND"].ToString(),ToolBarDirection.Right);
            }
        }

        List<MenuLink> m = new List<MenuLink>();
        m = toolbarmain.Show();


        OrderExportToPDF.AccessRights = this.ViewState;
        if (m.Count > 0)
        {
            OrderExportToPDF.MenuList = toolbarmain.Show();
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Request.QueryString["ifr"] != null)
                ifr = "0";
            BindReport();
        }
    }
    protected void OrderExportToPDF_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        DataTable dt = null;
        try
        {
            if (CommandName.ToUpper().Equals("PRINT"))
            {
                return;
            }
            else if (CommandName.ToUpper().Equals("SENDMAIL"))
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

            else if (CommandName.ToUpper().Equals("TRAVELCLAIMPOST"))
            {
                PhoenixReportsAccount.GenerateAndSaveDebitnotePdf(Request.QueryString["VisitId"].ToString(), Request.QueryString["TravelClaimId"].ToString());
                Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=TRAVELCLAIMPOST&showmenu=0&VisitId=" + Request.QueryString["VisitId"].ToString() + "&TravelClaimId=" + Request.QueryString["TravelClaimId"].ToString());
                return;
            }
            else if (CommandName.ToUpper().Equals("TRAVELCLAIMBACK"))
            {
                Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=CLAIMDEBITNOTEVIEW&showmenu=0&VisitId=" + Request.QueryString["VisitId"].ToString() + "&TravelClaimId=" + Request.QueryString["TravelClaimId"].ToString());
                return;
            }
            else if (CommandName.ToUpper().Equals("TRAVELCLAIMPOSTING"))
            {
                PhoenixReportsAccount.TravelClaimPosting(new Guid(Request.QueryString["VisitId"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                //PhoenixReportsAccount.GenerateAndSaveDebitnotePdfConvert(Request.QueryString["VisitId"].ToString(), Request.QueryString["TravelClaimId"].ToString());
                Response.Redirect("../Reports/ReportsView.aspx?applicationcode=5&reportcode=TRAVELCLAIMPOST&showmenu=0&VisitId=" + Request.QueryString["VisitId"].ToString() + "&TravelClaimId=" + Request.QueryString["TravelClaimId"].ToString());
                return;
            }
            else if (CommandName.ToUpper().Equals("MONTHLY") || CommandName.ToUpper().Equals("YEARLY") || CommandName.ToUpper().Equals("ACCUMULATED") || CommandName.ToUpper().Equals("VESSELTRAILBALANCE") || CommandName.ToUpper().Equals("VESSELSUMMARYBALANCE") || CommandName.ToUpper().Equals("STATEMENTOFOWNERACCOUNTSUMMARY") || CommandName.ToUpper().Equals("VESSELTRAILBALANCEYTD") || CommandName.ToUpper().Equals("VESSELTRAILBALANCEYTDOWNER"))
            {
                NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
                DataSet ds = PhoenixReportsAccount.SOAGenerationOwnerReportVerification(int.Parse(nvc["ownerid"].ToString()), new Guid(nvc["debitnoteid"].ToString()), nvc["subreportcode"].ToString());
                if (ds.Tables.Count > 0)
                {
                    ucConfirm.ErrorMessage = ds.Tables[0].Rows[0]["FLDMSG"].ToString();
                    ucConfirm.Visible = true;
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('verified'," + (ifr == "0" ? "null" : "'filterandsearch'") + ",null);", true);
                return;
            }
            dt = PhoenixSsrsReportsCommon.GetReportCommand(int.Parse(Request.QueryString["applicationcode"].ToString()), Request.QueryString["reportcode"].ToString(), CommandName.ToUpper());
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
    private bool CheckSentMail()
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        return PhoenixSsrsReportsCommon.CheckSentMail(nvc);
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
    public void SentMailUpdate()
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        PhoenixSsrsReportsCommon.UpdateSentMail(nvc);
    }
    public void SendMail()
    {
        DataTable dt = null;
        dt = GetReportData();
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        ExportSSRSReport(ds, ExportFileFormat.PDF);

        SendMail(dt);
        ucConfirm.ErrorMessage = "Email sent";
        ucConfirm.Visible = true;
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
                //ReportViewer1.LocalReport.SubreportProcessing += new

                //             SubreportProcessingEventHandler(SubreportProcessingEventHandler);

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;

                byte[] bytes = ReportViewer1.LocalReport.Render(
                                Format.ToString(), null, out mimeType, out encoding,
                                out extension,
                                out streamids, out warnings);

                if (!Directory.Exists(Server.MapPath("~/Attachments/Temp/" + nvc["Attachment"].ToString())))
                {
                    System.IO.File.Delete(Server.MapPath("~/Attachments/Temp/" + nvc["Attachment"].ToString()));
                }

                FileStream fs = new FileStream(Server.MapPath("~/Attachments/Temp/" + nvc["Attachment"].ToString()), FileMode.Create);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    private void SendMail(DataTable dt)
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        PhoenixSsrsReportsCommon.SendMailToCustomer(nvc, dt, _filename);
    }
    public void Purchasesendmailupdate(string reason, string remarks, int? senddateupdate)
    {
        if (ViewState["QUOTATIONID"] != null)
            PhoenixReportsPurchase.Purchasesendmailupdate(new Guid(ViewState["QUOTATIONID"].ToString()), General.GetNullableString(reason), General.GetNullableString(remarks), senddateupdate);
    }
    public void SendWorkGearBulkRequestMail()
    {
        try
        {
            DataTable dt = GetReportData();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ExportSSRSReport(ds, ExportFileFormat.PDF);
            //PhoenixSSRSReportClass.ExportReport(_reportfile, _filename, dt);
            SendMail(dt);
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
    public void BindReport()
    {
        try
        {
            NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
            PhoenixReportClass rpt = new PhoenixReportClass();
            nvc.Remove("CRITERIA");
            nvc.Add("CRITERIA", "");
            DataTable dt = GetReportData();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            Session["SUBREPORT"] = ds;
            PhoenixSsrsReportsCommon.LoadSsrsReport(nvc);
           
            //{

            rpt.ResourceName = PhoenixSsrsReportsCommon.GetSsrsMainReport(int.Parse(nvc["applicationcode"].ToString()), nvc["reportcode"].ToString(), nvc["CRITERIA"]);
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.ReportPath = rpt.ResourceName;
            ReportViewer1.LocalReport.EnableExternalImages = true;

            if (_filename.LastIndexOf("\\") > 0)
                ReportViewer1.LocalReport.DisplayName = _filename.Substring(_filename.LastIndexOf("\\") + 1).Replace(".pdf", "");

            if (nvc["LogoPath"] != null && nvc["LogoPath"].ToString() != "")
            {
                //string imagePath = new Uri(Server.MapPath(nvc["LogoPath"].ToString())).AbsoluteUri;
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
            //else
            //{
            //    ucError.ErrorMessage = "No Records Found";
            //    ucError.Visible = true;
            //}
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
            System.Collections.Generic.List<string> rpt = new List<string>();
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
    private DataTable GetReportData()
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        return PhoenixSsrsReportsCommon.GetReportData(nvc, ref _reportfile, ref _filename);
    }

    protected void ReportViewer_OnLoad(object sender, EventArgs e)
    {
        NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
        string exportOptionWord = "Word";
        RenderingExtension extensionwrd = ReportViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(exportOptionWord, StringComparison.CurrentCultureIgnoreCase));
        string exportOptionExcel = "Excel";
        RenderingExtension extensionexl = ReportViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(exportOptionExcel, StringComparison.CurrentCultureIgnoreCase));
        string exportOptionPdf = "PDF";
        RenderingExtension extensionpdf = ReportViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(exportOptionPdf, StringComparison.CurrentCultureIgnoreCase));

        if (nvc["showword"] == null || nvc["showword"].ToUpper().Equals("YES"))
        {
            System.Reflection.FieldInfo fieldInfo = extensionwrd.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(extensionwrd, true);
        }
        else
        {
            System.Reflection.FieldInfo fieldInfo = extensionwrd.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(extensionwrd, false);

        }
        if (nvc["showexcel"] == null || nvc["showexcel"].ToUpper().Equals("YES"))
        {
            System.Reflection.FieldInfo fieldInfo = extensionexl.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(extensionexl, true);
        }
        else
        {
            System.Reflection.FieldInfo fieldInfo = extensionexl.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(extensionexl, false);
        }
        if (nvc["showpdf"] == null || nvc["showpdf"].ToUpper().Equals("YES"))
        {
            System.Reflection.FieldInfo fieldInfo = extensionpdf.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(extensionpdf, true);
        }
        else
        {
            System.Reflection.FieldInfo fieldInfo = extensionpdf.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(extensionpdf, false);
        }
    }
    public void PurchaseCustomersendmailupdate()
    {
        if (ViewState["QUOTATIONID"] != null)
        {
            //PhoenixReportsPurchase.PurchaseCustomersendmailupdate(General.GetNullableGuid(ViewState["QUOTATIONID"].ToString()));
        }
    }
    protected void ucConfirmSent_OnClick(object sender, EventArgs e)
    {
        if (ucConfirmSent.confirmboxvalue == 1)
        {
            SendMail();
        }
    }

}