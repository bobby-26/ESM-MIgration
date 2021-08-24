using System;
using System.IO;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
//using Ionic.Zip;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;
using SouthNests.Phoenix.Common;

public partial class VesselAccountsSendMailToVesselForTrash :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Sent", "SEND");
        MenuMailRead.AccessRights = this.ViewState;
        MenuMailRead.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            lblFileName.Text = "db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".zip";
            txtFormDetails.Content = MailContent();
            DataSet ds = PhoenixRegistersVesselCommunicationDetails.EditCommunicationDetails(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtTo.Text = dr["FLDEMAIL"].ToString();
                
                txtCc.Text = "";
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

    protected void MenuMailRead_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SEND"))
            {
                DataTable dtStartDate = PhoenixVesselAccountsCorrections.VesselStartDateSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

                if (dtStartDate.Rows.Count > 0)
                {
                    DataRow drStartDate = dtStartDate.Rows[0];

                    DataTable dt = PhoenixVesselAccountsCorrections.VesselTrashSendMailTrackSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, drStartDate["FLDHARDNAME"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        ucError.ErrorMessage = "Already Mail has sent to Vessel";
                        ucError.Visible = true;
                        return;
                    }
                    else
                    {
                        string strHtml;
                        strHtml = txtFormDetails.Content.Replace("&lt;", "<");
                        strHtml = strHtml.Replace("&gt;", ">");
                        strHtml = strHtml.Replace("&quot;", "'");
                        CreateZipFile();
                        SendMail(txtTo.Text, txtCc.Text, null, txtSubject.Text, strHtml, true, System.Net.Mail.MailPriority.Normal, "db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".zip");

                        PhoenixVesselAccountsCorrections.InsertVesselTrashSendMailTrack(PhoenixSecurityContext.CurrentSecurityContext.UserCode, drStartDate["FLDHARDNAME"].ToString(), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

                        ucStatus.Text = "Mail Sent to Vessel..";
                    }
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
            DataTable dt = PhoenixVesselAccountsCorrections.VesselStartDateSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                // Create File1 (Sql File)
                string file1 = @"" + HttpContext.Current.Request.MapPath("~/Attachments/VesselAccounts/") + "db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".sql";
                //string file1 = @"\\30.30.30.2\Prereq\db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".sql";

                if (File.Exists(file1))
                {
                    File.Delete(file1);
                }

                FileStream fs1 = null;
                if (!File.Exists(file1))
                {
                    using (fs1 = File.Create(file1))
                    {

                    }
                }

                if (File.Exists(file1))
                {
                    using (StreamWriter sw = new StreamWriter(file1))
                    {
                        sw.Write(SqlContent());
                    }
                }

                // Create File2 (bat File)
                string file2 = @"" + HttpContext.Current.Request.MapPath("~/Attachments/VesselAccounts/") + "db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".bat";
                //string file2 = @"\\30.30.30.2\Prereq\db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".bat";

                if (File.Exists(file2))
                {
                    File.Delete(file2);
                }

                FileStream fs2 = null;
                if (!File.Exists(file2))
                {
                    using (fs2 = File.Create(file2))
                    {

                    }
                }

                if (File.Exists(file2))
                {
                    using (StreamWriter sw = new StreamWriter(file2))
                    {
                        sw.Write(BatContent());
                    }
                }

                string allfiles, zipfilename;

                allfiles = @"\VesselAccounts\db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                zipfilename = @"\VesselAccounts\db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".zip";

                //allfiles = @"\\30.30.30.2\Prereq\db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                //zipfilename = @"\\30.30.30.2\Prereq\db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".zip";

                PhoenixVesselAccountsCorrections.VesselZipPatchFiles(PhoenixSecurityContext.CurrentSecurityContext.UserCode, allfiles, zipfilename);

                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".zip");
                Response.ContentType = "application/zip";


                Response.TransmitFile(HttpContext.Current.Request.MapPath("~/Attachments/VesselAccounts/db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".zip"));

                //Response.TransmitFile(@"\\30.30.30.2\Prereq\db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".zip");

                ////Zip the above created two files

                ////Download Zip Files
                //using (ZipFile zip = new ZipFile())
                //{
                //    string[] files = { "db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".sql", "db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".bat" };

                //    for (int i = 0; i < files.Length; i++)
                //    {
                //        string filePath = Server.MapPath("~/Attachments/VesselAccounts/" + files[i]);
                //        zip.AddFile(filePath, "");
                //    }
                //    Response.Clear();
                //    Response.AddHeader("Content-Disposition", "attachment; filename=DownloadedFile.zip");
                //    Response.ContentType = "application/zip";
                //    zip.Save(Response.OutputStream);
                //}

                ////Zip the above created two files and then save

                //using (ZipFile zip = new ZipFile())
                //{
                //    string[] files = { "db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".sql", "db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".bat" };

                //    for (int i = 0; i < files.Length; i++)
                //    {
                //        string filePath = Server.MapPath("~/Attachments/VesselAccounts/" + files[i]);
                //        zip.AddFile(filePath, "");
                //        zip.Save(HttpContext.Current.Request.MapPath("~/Attachments/VesselAccounts/") + "db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".zip");
                //    }
                //}

                if (File.Exists(file1))
                {
                    File.Delete(file1);
                }

                if (File.Exists(file2))
                {
                    File.Delete(file2);
                }
                Response.End();
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
        try
        {
            DataTable dt = PhoenixVesselAccountsCorrections.VesselStartDateSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                // Create File1 (Sql File)
                string file1 = @"" + HttpContext.Current.Request.MapPath("~/Attachments/VesselAccounts/") + "db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".sql";

                if (File.Exists(file1))
                {
                    File.Delete(file1);
                }

                FileStream fs1 = null;
                if (!File.Exists(file1))
                {
                    using (fs1 = File.Create(file1))
                    {

                    }
                }

                if (File.Exists(file1))
                {
                    using (StreamWriter sw = new StreamWriter(file1))
                    {
                        sw.Write(SqlContent());
                    }
                }

                // Create File2 (bat File)
                string file2 = @"" + HttpContext.Current.Request.MapPath("~/Attachments/VesselAccounts/") + "db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".bat";

                if (File.Exists(file2))
                {
                    File.Delete(file2);
                }

                FileStream fs2 = null;
                if (!File.Exists(file2))
                {
                    using (fs2 = File.Create(file2))
                    {

                    }
                }

                if (File.Exists(file2))
                {
                    using (StreamWriter sw = new StreamWriter(file2))
                    {
                        sw.Write(BatContent());
                    }
                }

                string allfiles, zipfilename;

                allfiles = @"\VesselAccounts\db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                zipfilename = @"\VesselAccounts\db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".zip";

                PhoenixVesselAccountsCorrections.VesselZipPatchFiles(PhoenixSecurityContext.CurrentSecurityContext.UserCode, allfiles, zipfilename);

                ////Zip the above created two files and then save

                //using (ZipFile zip = new ZipFile())
                //{
                //    string[] files = { "db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".sql", "db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".bat" };

                //    for (int i = 0; i < files.Length; i++)
                //    {
                //        string filePath = Server.MapPath("~/Attachments/VesselAccounts/" + files[i]);
                //        zip.AddFile(filePath, "");
                //        zip.Save(HttpContext.Current.Request.MapPath("~/Attachments/VesselAccounts/") + "db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".zip");
                //    }
                //}

                if (File.Exists(file1))
                {
                    File.Delete(file1);
                }

                if (File.Exists(file2))
                {
                    File.Delete(file2);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public static void SendMail(string to, string cc, string bcc, string mailsubject, string mailbody, bool htmlbody, MailPriority priority, string filename)
    {
        try
        {
            string sourcedirectory = HttpContext.Current.Request.MapPath("~/Attachments/VesselAccounts/" + filename);
            if (File.Exists(sourcedirectory))
                Send(to, cc, bcc, mailsubject, mailbody, htmlbody, priority, sourcedirectory);
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

            Attachment attachment = new Attachment(filename);
            attachment.ContentDisposition.Inline = true;
            objmessage.Attachments.Add(attachment);

            //if (fileentries != null)
            //{
            //    foreach (string filename in fileentries)
            //    {
            //        Attachment attachment = new Attachment(filename);
            //        attachment.ContentDisposition.Inline = true;
            //        objmessage.Attachments.Add(attachment);
            //    }
            //}

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

    private string SqlContent()
    {
        string StrSql;
        StrSql =  "";
        DataTable dt = PhoenixVesselAccountsCorrections.VesselStartDateSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            StrSql = @"IF OBJECT_ID('dbo.PRVESSELACCOUNTCLEAR', 'P') IS NOT NULL DROP PROC dbo.PRVESSELACCOUNTCLEAR
                                GO
                                CREATE PROCEDURE PRVESSELACCOUNTCLEAR
                                (
	                                @ROWUSERCODE	INT
	                                ,@VESSELID		SMALLINT
                                )
                                --WITH ENCRYPTION
                                AS
                                BEGIN
	                                SET NOCOUNT ON

	                                DELETE FROM TBLVESSELTRANSACTIONSLOG
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELTRANSACTIONSLOG
	                                FROM	TBLVESSELTRANSACTIONSLOG L
		                                INNER JOIN TBLVESSELEARNINGDEDUCTION D WITH (NOLOCK)
			                                ON D.FLDDTKEY	=	L.FLDLOGID
	                                WHERE L.FLDVESSELID = @VESSELID
	                                AND	D.FLDVESSELID	=	@VESSELID
	                                AND FLDEARNINGDEDUCTIONID NOT IN (SELECT FLDEARNINGDEDUCTIONID FROM TBLREIMBURSEMENTPAYMENT WITH (NOLOCK) WHERE FLDEARNINGDEDUCTIONID IS NOT NULL)
	                                --------------------------------------


	                                DELETE FROM TBLVESSELEARNINGDEDUCTION
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELEARNINGDEDUCTION
	                                WHERE FLDVESSELID = @VESSELID
	                                AND	FLDEARNINGDEDUCTIONID NOT IN (SELECT  FLDEARNINGDEDUCTIONID FROM TBLREIMBURSEMENTPAYMENT WITH  (NOLOCK) WHERE FLDEARNINGDEDUCTIONID IS NOT NULL)
	                                ----------------------------------------

	                                DELETE FROM TBLVESSELPORTAGEBILL
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELPORTAGEBILL
	                                WHERE FLDVESSELID = @VESSELID
	                                -----------------------------------------

	                                DELETE FROM TBLVESSELCAPTAINCASH
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELCAPTAINCASH
	                                WHERE FLDVESSELID = @VESSELID

	                                -----------------------------------------


	                                DELETE FROM TBLVESSELCAPTAINPETTYCASH
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELCAPTAINPETTYCASH
	                                WHERE FLDVESSELID = @VESSELID

	                                -----------------------------------------


	                                DELETE FROM TBLVESSELCAPTAINCASHBALANCE
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELCAPTAINCASHBALANCE
	                                WHERE FLDVESSELID = @VESSELID

	                                -----------------------------------------

	                                DELETE FROM TBLVESSELSTOREITEMDISPOSITION
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELSTOREITEMDISPOSITION
	                                WHERE FLDVESSELID = @VESSELID
	                                -----------------------------------------
                                	
	                                DELETE FROM TBLVESSELTRANSACTIONSLOG
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELTRANSACTIONSLOG
	                                FROM	TBLVESSELTRANSACTIONSLOG L
		                                INNER JOIN TBLVESSELSTOREISSUEITEM D WITH (NOLOCK)
			                                ON D.FLDDTKEY	=	L.FLDLOGID
	                                WHERE L.FLDVESSELID = @VESSELID
	                                AND	D.FLDVESSELID	=	@VESSELID	

	                                DELETE FROM TBLVESSELSTOREISSUEITEM
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELSTOREISSUEITEM
	                                WHERE FLDVESSELID = @VESSELID
	                                -----------------------------------------

	                                DELETE FROM TBLVESSELSTOREISSUE
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELSTOREISSUE
	                                WHERE FLDVESSELID = @VESSELID

	                                -----------------------------------------

	                                DELETE FROM TBLVESSELRADIOLOG
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELRADIOLOG
	                                WHERE FLDVESSELID = @VESSELID

	                                -----------------------------------------

	                                DELETE FROM TBLVESSELPHONECARDREQUISITIONLINEITEM
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELPHONECARDREQUISITIONLINEITEM
	                                WHERE FLDVESSELID = @VESSELID

	                                -----------------------------------------

	                                DELETE FROM TBLVESSELPHONECARDREQUISITION
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELPHONECARDREQUISITION
	                                WHERE FLDVESSELID = @VESSELID

	                                -----------------------------------------

	                                DELETE FROM TBLVESSELORDERFORM
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELORDERFORM
	                                WHERE FLDVESSELID = @VESSELID

	                                -----------------------------------------

	                                DELETE FROM TBLVESSELORDERLINEITEM
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELORDERLINEITEM
	                                WHERE FLDVESSELID = @VESSELID

	                                -----------------------------------------

	                                DELETE FROM TBLVESSELCAPTAINCASHCALCULATION
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELCAPTAINCASHCALCULATION
	                                WHERE FLDVESSELID = @VESSELID

	                                -----------------------------------------


	                                DELETE FROM TBLVESSELCAPTAINCASHBOW
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELCAPTAINCASHBOW
	                                WHERE FLDVESSELID = @VESSELID

	                                -----------------------------------------

	                                DELETE FROM TBLVESSELPROVISION
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITVESSELPROVISION
	                                WHERE FLDVESSELID = @VESSELID
	                                -----------------------------------------

	                                UPDATE SL
	                                SET FLDQUANTITY = 0.0
	                                OUTPUT 1, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  INSERTED.* INTO TBLAUDITSTOREITEMLOCATION
	                                FROM TBLSTOREITEMLOCATION SL
		                                INNER JOIN TBLSTOREITEM S
			                                ON S.FLDSTOREITEMID = SL.FLDSTOREITEMID
	                                WHERE SL.FLDVESSELID = @VESSELID
	                                AND S.FLDVESSELID = @VESSELID 
	                                AND FLDSTORECLASS IN (411,412,633)


	                                -----------------------------------------

	                                UPDATE TBLSTOREITEM
	                                SET FLDTOTALPRICE = 0.0
	                                OUTPUT 1, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  INSERTED.* INTO TBLAUDITSTOREITEM
	                                WHERE FLDVESSELID = @VESSELID
	                                AND FLDSTORECLASS IN (411,412,633)

	                                -----------------------------------------

	                                DELETE H
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITSTOREITEMDISPOSITIONHEADER
	                                FROM TBLSTOREITEMDISPOSITIONHEADER H
		                                INNER JOIN TBLSTOREITEMDISPOSITION D
			                                ON H.FLDSTOREITEMDISPOSITIONHEADERID = D.FLDSTOREITEMDISPOSITIONHEADERID
		                                INNER JOIN TBLSTOREITEM S
			                                ON S.FLDSTOREITEMID = D.FLDSTOREITEMID
	                                WHERE H.FLDVESSELID = @VESSELID
	                                AND FLDSTORECLASS IN (411,412,633)

	                                -----------------------------------------

	                                DELETE D
	                                OUTPUT 2, @ROWUSERCODE, GETUTCDATE(), OBJECT_NAME(@@PROCID),NULL,  DELETED.* INTO TBLAUDITSTOREITEMDISPOSITION
	                                FROM TBLSTOREITEMDISPOSITION D
		                                INNER JOIN TBLSTOREITEM S
			                                ON S.FLDSTOREITEMID = D.FLDSTOREITEMID
	                                WHERE D.FLDVESSELID = @VESSELID
	                                AND FLDSTORECLASS IN (411,412,633)

	                                -----------------------------------------

                                END
                                GO

                                DECLARE @VESSELID SMALLINT
                                SELECT @VESSELID = FLDINSTALLCODE FROM TBLCONFIGURATION WHERE  FLDCONFIGURATIONCODE = 1 

                                EXEC PRVESSELACCOUNTCLEAR 1, @VESSELID
                                GO

                                SELECT * FROM TBLHARD (NOLOCK) WHERE FLDHARDTYPECODE=165 AND FLDSHORTNAME='" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + @"'
                                GO
                                IF NOT EXISTS (SELECT '1' FROM TBLHARD WHERE FLDHARDCODE = " + dr["FLDHARDCODE"].ToString() + @")
                                BEGIN 
                                 INSERT INTO TBLHARD
                                 (
                                  FLDHARDTYPECODE
                                  ,FLDHARDCODE
                                  ,FLDHARDNAME
                                  ,FLDSHORTNAME
                                  ,FLDACTIVEYN
                                  ,FLDCREATEDDATE
                                  ,FLDCREATEDBY
                                  ,FLDMODIFIEDDATE
                                  ,FLDMODIFIEDBY  
                                 )
                                 VALUES
                                 (
                                  165
                                  ," + dr["FLDHARDCODE"].ToString() + @"
                                  ,'" + dr["FLDHARDNAME"].ToString() + @"'
                                  ,'" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + @"'
                                  ,1
                                  ,GETUTCDATE()
                                  ,1
                                  ,GETUTCDATE()
                                  ,1
                                 )
                                END
                                ELSE
                                BEGIN
                                 UPDATE TBLHARD
                                 SET FLDHARDNAME = '" + dr["FLDHARDNAME"].ToString() + @"'
                                 WHERE FLDHARDCODE = " + dr["FLDHARDCODE"].ToString() + @"
                                 AND FLDHARDTYPECODE = 165
                                END
                                GO
                                SELECT * FROM TBLHARD (NOLOCK) WHERE FLDHARDTYPECODE=165 AND FLDSHORTNAME='" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + @"'
                                GO";
            return StrSql;
        }
        else
        {
            return StrSql;
        }
        
    }

    private string BatContent()
    {
        string StrBat;
        StrBat = "";
        DataTable dt = PhoenixVesselAccountsCorrections.VesselStartDateSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            StrBat = @"@echo off

                        pushd %~dp0

                        set path=%path%;C:\Program Files\Microsoft SQL Server\90\Tools\binn\;

                        for %%a in (c:\phoenix\database\*.mdf) do  set  dbname=%%~na

                        echo %dbname%

                        echo Started applying patch....

                        SQLCMD -E -S .\SQLEXPRESS -d %dbname%  -i db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + @".sql -o db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + @".txt > nul

                        if %ERRORLEVEL% EQU 0 goto Success else goto failed

                        :failed

                        echo Patch executed in database %dbname% >> db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + @".txt
                        echo Patch failed... >> db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + @".txt
                        echo Patch failed... Please send the db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + @".txt file to sep@southnests.com.
                        goto end

                        :success

                        echo Patch executed in database %dbname% >> db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + @".TXT
                        echo Patch succeeded... >> db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + @".txt
                        echo Patch succeeded... 

                        :end
                        pause";
            return StrBat;
        }
        else
        {
            return StrBat;
        }

    }

    private string MailContent()
    {
        string StrMailText;
        StrMailText = "";
        DataTable dt = PhoenixVesselAccountsCorrections.VesselStartDateSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            StringBuilder sbemailbody = new StringBuilder();
            sbemailbody.AppendLine();
            sbemailbody.Append("Good Day Captain,<br><br>");

            sbemailbody.AppendLine("Please refer the below mail..<br><br>");

            sbemailbody.AppendLine("Accounts team requested to trash and Reconnect Vessel Accounting data for your vessel..<br>");
            sbemailbody.AppendLine("Please follow the below steps, it will delete all vessel accounting records..<br><br>");

            sbemailbody.AppendLine("For Windows XP PC - Please follow the below steps<br><br>");

            sbemailbody.AppendLine("In the PC where Phoenix is installed.<br><br>");

            sbemailbody.AppendLine(@"a. Copy the attached zip file to C:\folder.<br>");
            sbemailbody.AppendLine("b. Right click on the zip file and click on ''Extract to db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "'' menu in the context menu.<br>");
            sbemailbody.AppendLine(@"c. Go to folder C:\db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "<br>");
            sbemailbody.AppendLine("d. Double click on db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".bat<br><br>");

            sbemailbody.AppendLine("For Windows 7 PC - Please follow the below steps<br><br>");

            sbemailbody.AppendLine("In the PC where Phoenix is installed.<br><br>");

            sbemailbody.AppendLine(@"a. Copy the attached zip file to C:\folder.<br>");
            sbemailbody.AppendLine("b. Right click on the zip file and click on ''Extract to db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "'' menu in the context menu.<br>");
            sbemailbody.AppendLine(@"c. Go to folder C:\db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "<br>");
            sbemailbody.AppendLine("d. Right click on db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".bat, Select ''Run as adminstrator''<br>");
            sbemailbody.AppendLine("e. If prompt for confirmation, Click yes<br>");
            sbemailbody.AppendLine("f. Send the log file db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".log to sep@southnests.com<br><br>");

            sbemailbody.AppendLine("After follow the above steps:<br>");

            sbemailbody.AppendLine("Send the log file db_vactrashandstartdate" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + ".log to sep@southnests.com<br><br>");

            sbemailbody.AppendLine("And send an export from Phoenix. We would like to check all the vessel accounting data has deleted or not.<br><br>");

            sbemailbody.AppendLine("Steps:<br><br>");

            sbemailbody.AppendLine("a. Login to Phoenix<br>");
            sbemailbody.AppendLine("b. Click on Options >> Data Synchronizer >> Data Synchronizer<br>");
            sbemailbody.AppendLine("c. Click on Export button in the Action column.<br><br>");

            sbemailbody.AppendLine("After Importing Vessel data in office,<br>");
            sbemailbody.AppendLine("We will put the provision and bonded store ROB.<br><br>");

            sbemailbody.AppendLine("And we will send an export..<br><br>");
            sbemailbody.AppendLine("After importing office export  file, you can you vessel accounting..<br><br>");

            sbemailbody.AppendLine("Best Regards<br><br>");
            sbemailbody.AppendLine("Developers(Phoenix Support)");
            sbemailbody.AppendLine();

            return sbemailbody.ToString();
        }
        else
        {
            return StrMailText;
        }

    }

    protected void btnInsertPic_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.Files.Count > 0 && txtFormDetails.ActiveMode == AjaxControlToolkit.HTMLEditor.ActiveModeType.Design)
            {
                Guid gFileName = PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.PURCHASE, null, ".jpg,.png,.gif");
                DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["dtkey"].ToString()));
                DataRow[] dr = dt.Select("FLDDTKEY = '" + gFileName.ToString() + "'");
                if (dr.Length > 0)
                    txtFormDetails.Content = txtFormDetails.Content + "<img src=\"" + HttpContext.Current.Session["sitepath"] + "/attachments/" + dr[0]["FLDFILEPATH"].ToString() + "\" />";
            }
            else
            {
                ucError.Text = Request.Files.Count > 0 ? "You are not in design mode" : "No Picture selected.";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public static bool TrustAllCertificatesCallback(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors)
    {
        return true;
    }

}
