using System;
using System.IO;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using Telerik.Web.UI;
public partial class CrewTravelVisaEmail : PhoenixBasePage
{
    string travelvisaid = "";
    string attachmentyn = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Send", "SEND", ToolBarDirection.Right);
        MenuMailRead.AccessRights = this.ViewState;
        MenuMailRead.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            confirm.Attributes.Add("style", "display:none");

            RemoveEditorToolBarIcons(); // remove the unwanted icons in editor

            if (Request.QueryString["travelvisaid"] != null)
            {
                ViewState["TRAVELVISAID"] = null;
                ViewState["FILENAME"] = null;

                travelvisaid = Request.QueryString["travelvisaid"];
                ViewState["TRAVELVISAID"] = Request.QueryString["travelvisaid"];
                VisaRequestMail(travelvisaid);
            }
        }
    }

    protected void RemoveEditorToolBarIcons()
    {
        edtBody.EnsureToolsFileLoaded();
        RemoveButton("ImageManager");
        RemoveButton("DocumentManager");
        RemoveButton("FlashManager");
        RemoveButton("MediaManager");
        RemoveButton("TemplateManager");
        RemoveButton("XhtmlValidator");
        RemoveButton("InsertSnippet");
        RemoveButton("ModuleManager");
        RemoveButton("ImageMapDialog");
        RemoveButton("AboutDialog");
        RemoveButton("InsertFormElement");

        edtBody.EnsureToolsFileLoaded();
        edtBody.Modules.Remove("RadEditorHtmlInspector");
        edtBody.Modules.Remove("RadEditorNodeInspector");
        edtBody.Modules.Remove("RadEditorDomInspector");
        edtBody.Modules.Remove("RadEditorStatistics");

    }

    protected void RemoveButton(string name)
    {
        foreach (EditorToolGroup group in edtBody.Tools)
        {
            EditorTool tool = group.FindTool(name);
            if (tool != null)
            {
                group.Tools.Remove(tool);
            }
        }

    }


    protected void lknfilename_OnClick(object sender, EventArgs e)
    {
        try
        {
            DownloadZipFile();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void VisaRequestMail(string travelvisaid)
    {
        DataTable dtLI = PhoenixCrewTravelVisa.ListCrewTravelVisaLineItem(new Guid(travelvisaid));
        DataTable dtAgent = PhoenixCrewTravelVisa.EditCrewTravelVisaAgentDetail(new Guid(travelvisaid));

        DataRow dr1 = dtAgent.Rows[0];

        StringBuilder sbemailbody = new StringBuilder();
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Dear Agent,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Please arrange to process " + dr1["FLDCOUNTRYNAME"].ToString() + " Visa for the below mentioned candidates.");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("REMARKS : " + dr1["FLDREMARKS"].ToString());

        if (dtLI.Rows.Count > 0)
        {
            foreach (DataRow drLI in dtLI.Rows)
            {
                DataTable dtDOC = PhoenixCrewTravelVisa.ListCrewTravelVisaLIDoc(new Guid(travelvisaid)
                , new Guid(drLI["FLDVISALINEITEMID"].ToString()));

                if (dtDOC.Rows.Count > 0)
                {
                    DataRow drDoc = dtDOC.Rows[0];

                    sbemailbody.AppendLine("<br/>");
                    sbemailbody.AppendLine("<br/>");
                    sbemailbody.AppendLine("<br/>Employee Details(" + drLI[0].ToString() + ")");
                    sbemailbody.AppendLine("<br/>");
                    sbemailbody.AppendLine("<br/>Employee No:" + " " + drDoc["FLDEMPLOYEECODE"].ToString());
                    sbemailbody.AppendLine("<br/>Rank       :" + " " + drDoc["FLDRANKNAME"].ToString());
                    sbemailbody.AppendLine("<br/>Name       :" + " " + drDoc["FLDEMPLOYEENAME"].ToString());
                    sbemailbody.AppendLine("<br/>Date of Birth - Place :" + " " + drDoc["FLDDATEOFBIRTH"].ToString() + " - " + drDoc["FLDPLACEOFBIRTH"].ToString());
                    sbemailbody.AppendLine("<br/>Nationality:" + " " + drDoc["FLDNATIONALITY"].ToString());
                    sbemailbody.AppendLine("<br/>Passport No:" + " " + drDoc["FLDPASSPORTNO"].ToString());
                    sbemailbody.AppendLine("<br/>Issued - Valid untill - Place :" + " " + drDoc["FLDDATEOFISSUE"].ToString() + " - " + drDoc["FLDDATEOFEXPIRY"].ToString() + " - " + drDoc["FLDPLACEOFISSUE"].ToString());
                    sbemailbody.AppendLine("<br/>Seaman Book No:" + " " + drDoc["FLDSEAMANBOOKNO"].ToString());
                    sbemailbody.AppendLine("<br/>Issued - Valid untill - Place :" + " " + drDoc["FLDSDATEOFISSUE"].ToString() + " - " + drDoc["FLDSDATEOFEXPIRY"].ToString() + " - " + drDoc["FLDSPLACEOFISSUE"].ToString());
                    sbemailbody.AppendLine("<br/>");
                }

                DataTable dtAtt = PhoenixCrewTravelVisa.ListCrewTravelVisaLIAttachmentDetails(new Guid(travelvisaid)
                    , new Guid(drLI["FLDVISALINEITEMID"].ToString()));

                if (dtAtt.Rows.Count > 0)
                {
                    sbemailbody.AppendLine("<br/>");
                    sbemailbody.AppendLine("Attachment Details");
                    sbemailbody.AppendLine("<br/>");
                    sbemailbody.AppendLine("Document             -              Path");
                    sbemailbody.AppendLine("<br/>");
                    sbemailbody.AppendLine("===============================================");
                    sbemailbody.AppendLine("<br/>");
                    sbemailbody.AppendLine("<br/>");
                    foreach (DataRow drAtt in dtAtt.Rows)
                    {
                        sbemailbody.AppendLine(drAtt["FLDDOCUMENTTYPE"].ToString() + "  -  " + drAtt["FLDFILEPATH"].ToString());
                        sbemailbody.AppendLine("<br/>");
                    }

                    sbemailbody.AppendLine("<br/>");
                    sbemailbody.AppendLine("<br/>");
                    sbemailbody.AppendLine("<hr/>");
                }
            }

            sbemailbody.AppendLine("<br/>Thank you,");
            if (dtAgent.Rows.Count > 0)
            {
                DataRow dr = dtAgent.Rows[0];
                sbemailbody.AppendLine("<br/>" + dr["FLDUSERNAME"].ToString() + "");

                sbemailbody.AppendLine("<br/>");
                txtTo.Text = dr["FLDAGENTMAIL"].ToString().Replace(";", ",").TrimEnd(',');
                txtCc.Text = dr["FLDUSERMAIL"].ToString().Replace(";", ",").TrimEnd(',');

                lblFileName.Text = dr["FLDREFERENCENO"].ToString();
                txtSubject.Text = dr["FLDREFERENCENO"].ToString() + " - " + dr["FLDVESSELNAME"].ToString() + '/' + dr["FLDCOUNTRYNAME"].ToString() + " VISA REQUEST.";
                edtBody.Content = sbemailbody.ToString();
            }
        }
    }
    protected void MenuMailRead_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEND"))
            {

                attachmentyn = "0";
                CreateZipFile();
                if (attachmentyn == "1")
                {
                    SendMail(txtTo.Text.Trim(), txtCc.Text.Trim().TrimEnd(','), null, txtSubject.Text.Trim(), edtBody.Content.ToString(), true, System.Net.Mail.MailPriority.Normal, ViewState["FILENAME"].ToString() + ".zip");
                    PhoenixCrewTravelVisa.UpdateCrewTravelVisaAppliedDate(General.GetNullableGuid(ViewState["TRAVELVISAID"].ToString()));
                    ucStatus.Text = "Mail Sent to Agent..";
                }
                if (attachmentyn == "0")
                {

                    RadWindowManager1.RadConfirm("There is no attachments for the documents. Kindly confirm if you still need to send mail without attachments", "confirm", 300, 125, null, "Attachment Confirmation");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void DownloadZipFile()
    {
        try
        {
            string zipfilenameout = "";
            attachmentyn = "0";

            DataTable dt1 = PhoenixCrewTravelVisa.TravelVisaZipDocumentFiles(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["TRAVELVISAID"].ToString()), ref zipfilenameout, ref attachmentyn);
            ViewState["FILENAME"] = zipfilenameout;

            string filename = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + zipfilenameout + ".zip";
            
            if (attachmentyn == "1")
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + zipfilenameout + ".zip");
                Response.ContentType = "application/zip";
                Response.TransmitFile(filename);            
                Response.End();
            }
            else
            {
                ucError.ErrorMessage = "No Attachment found";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void CreateZipFile()
    {
        string zipfilenameout = "";
        DataTable dt1 = PhoenixCrewTravelVisa.TravelVisaZipDocumentFiles(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableGuid(ViewState["TRAVELVISAID"].ToString())
            , ref zipfilenameout
            , ref attachmentyn);

        ViewState["FILENAME"] = zipfilenameout;
    }

    public static void SendMail(string to, string cc, string bcc, string mailsubject, string mailbody, bool htmlbody, MailPriority priority, string filename)
    {
        try
        {
            if (filename != null)
            {
                string sourcedirectory = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + filename;

                if (File.Exists(sourcedirectory))
                    Send(to, cc, bcc, mailsubject, mailbody, htmlbody, priority, sourcedirectory);
            }
            else
            {
                Send(to, cc, bcc, mailsubject, mailbody, htmlbody, priority, null);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private static void Send(string to, string cc, string bcc, string mailsubject, string mailbody, bool htmlbody, MailPriority priority, string filename)
    {
        try
        {
            MailMessage objmessage = new MailMessage();
            objmessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            objmessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["FromMail"].ToString());

            if (!IsvalidEmail(to))
                throw new Exception("You have not specified a valid To address.");
            objmessage.To.Add(to);

            if (IsvalidEmail(cc))
                objmessage.CC.Add(cc);

            if (IsvalidEmail(bcc))
                objmessage.Bcc.Add(bcc);

            objmessage.IsBodyHtml = htmlbody;
            objmessage.Subject = mailsubject;
            objmessage.Body = mailbody;
            objmessage.Priority = priority;

            if (filename != null)
            {
                Attachment attachment = new Attachment(filename);
                attachment.ContentDisposition.Inline = true;
                objmessage.Attachments.Add(attachment);
            }
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("smtpport"));
            smtp.Host = ConfigurationManager.AppSettings.Get("smtpipaddress").ToString();
            ServicePointManager.ServerCertificateValidationCallback = TrustAllCertificatesCallback;
            smtp.EnableSsl = true;
            string mailuser = ConfigurationManager.AppSettings.Get("mailuser").ToString();
            string mailpassword = ConfigurationManager.AppSettings.Get("mailpassword").ToString();
            smtp.Credentials = new System.Net.NetworkCredential(mailuser, mailpassword);
            smtp.Send(objmessage);
            objmessage.Dispose();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private static bool IsvalidEmail(string email)
    {
        if (email == null)
            return false;
        if (email.Trim().Equals(""))
            return false;

        email = email.Replace(';', ',');
        email = email.Replace(" ", "");
        string[] mailids = email.Split(new char[] { ',' });

        foreach (string id in mailids)
        {
            string regex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(regex);
            if (!re.IsMatch(id))
                return (false);
        }
        return (true);
    }
    public static bool TrustAllCertificatesCallback(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors)
    {
        return true;
    }



    protected void confirm_Click(object sender, EventArgs e)
    {
        SendMail(txtTo.Text.Trim(), txtCc.Text.Trim().TrimEnd(','), null, txtSubject.Text.Trim(), edtBody.Content.ToString(), true, System.Net.Mail.MailPriority.Normal, null);
        PhoenixCrewTravelVisa.UpdateCrewTravelVisaAppliedDate(General.GetNullableGuid(ViewState["TRAVELVISAID"].ToString()));
        ucStatus.Text = "Mail Sent to Agent..";

    }
}
