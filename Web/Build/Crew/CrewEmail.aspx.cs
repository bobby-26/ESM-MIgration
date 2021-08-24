using System;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Reports;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Web;

public partial class CrewEmail : PhoenixBasePage
{
    string empid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Send", "SEND",ToolBarDirection.Right);
            if (Request.QueryString["course"] == null || Request.QueryString["pdformoffshore"] != null)
            {
                toolbarmain.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            }

            EmailMenu.AccessRights = this.ViewState;
            EmailMenu.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                btnView.Attributes.Add("style", "display:none;");

                ViewState["empid"] = "";
                Session["AttachFiles"] = null;
                ViewState["mailsessionid"] = System.Guid.NewGuid().ToString();
                if (Request.QueryString["t"] != null)
                {
                    MedicalRequestMail();
                    string att = Session["medreqatt"].ToString();
                    FileInfo f = new FileInfo(att);
                    lstAttachments.Items.Clear();
                    lstAttachments.Items.Add(new RadListBoxItem(f.Name, f.Name));
                }
                else if (Request.QueryString["csvsgid"] != null)
                {
                    AppraisalMail();
                }
                else if (Request.QueryString["course"] != null)
                {
                    empid = Request.QueryString["empid"];
                    CourseMail();

                }
                else if (Request.QueryString["itf"] != null)
                {
                    CrewChangeMail();                 
                }
                else if (Request.QueryString["bday"] != null)
                {
                    BirthdayEmail();
                }
                else if (Request.QueryString["wgrequest"] != null)
                {
                    WorkGearRequestMail();
                    string att = Session["wgrequest"].ToString();
                    FileInfo f = new FileInfo(att);
                    lstAttachments.Items.Clear();
                    lstAttachments.Items.Add(new RadListBoxItem(f.Name, f.Name));
                }
                else if (Request.QueryString["wgbulkrequest"] != null)
                {
                    WorkGearBulkRequestMail();
                    string att = Session["wgbulkrequest"].ToString();
                    FileInfo f = new FileInfo(att);
                    lstAttachments.Items.Clear();
                    lstAttachments.Items.Add(new RadListBoxItem(f.Name, f.Name));
                }
                else if (Request.QueryString["pdformoffshore"] != null)
                {
                    PDFormOffshore();
                }
                else if (Request.QueryString["Assessment"] != null)
                {
                    EmployeeAssessmentMail();
                }
                else if (Request.QueryString["LicEmail"] != null)
                {
                    LicenceEmailNew();
                }
                else
                {
                    LicenceEmail();
                }

                lnkAttachment.Attributes.Add("onclick", "javascript:openNewWindow('Codehelp1', '', '" + Session["sitepath"] + "/Options/OptionsAttachment.aspx?mailsessionid=" + ViewState["mailsessionid"] + "');");
                
            }
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }

    protected void EmailMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEND"))
            {
                if (!IsValidateEmail())
                {
                    ucError.Visible = true;
                    return;
                }

                SendMail();
                if (Request.QueryString["t"] != null)
                {
                    PhoenixCrewMedical.UpdateMedicalRequestMail(new Guid(Request["reqid"].ToString()));                 
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=MEDICALSLIP&reqid=" + Request.QueryString["reqid"], false);
                }
                else if (Request.QueryString["csvsgid"] != null)
                {
                    PhoenixCrewSignOnOff.UpdateSignOffEmail(Request.QueryString["csvsgid"]);
                    Session["AppraisalMail"] = null;
                    Response.Redirect("CrewAppraisalMail.aspx?vslid=" + Request.QueryString["vslid"], false);
                }
                else if (Request.QueryString["course"] != null)
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('NAFA', 'ifMoreInfo', null);", true);
                }
                else if (Request.QueryString["itf"] != null)
                {
                    return;                    
                }
                else if (Request.QueryString["bday"] != null)
                {
                    Response.Redirect("../Crew/CrewQueryBirthday.aspx", false);
                }
                else if (Request.QueryString["wgrequest"] != null)
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=WORKGEARINDIVIDUALREQUEST&orderid=" + Request.QueryString["orderid"] + "&showmenu=0&showword=no&showexcel=no&emailyn=2", false);
                }
                else if (Request.QueryString["wgbulkrequest"] != null)
                {
                    PhoenixCrewWorkingGearQuotation.UpdatePOSent(General.GetNullableGuid(Request.QueryString["Neededid"].ToString()), General.GetNullableInteger(Request.QueryString["Agentid"].ToString()));
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=WORKINGGEARREQUEST&Neededid=" + Request.QueryString["Neededid"].ToString() + "&Agentid=" + Request.QueryString["Agentid"].ToString() + "&showmenu=0&showword=no&showexcel=no&emailyn=2", false);
                }
                else if (Request.QueryString["pdformoffshore"] != null)
                {
                    Response.Redirect("CrewReportsOffshoreShellPPRegistration.aspx?" + Request.QueryString.ToString());
                }
                else if (Request.QueryString["Assessment"] != null)
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('NAFA', 'ifMoreInfo', null);", true);
                }
                else if (Request.QueryString["LicEmail"] != null)
                {
                    Response.Redirect("../Crew/CrewLicenceRequestList.aspx", false);
                }
                else
                {
                    Response.Redirect("../Crew/CrewLicenceProcess.aspx", false);
                }
            }
            else if (CommandName.ToUpper().Equals("CANCEL"))
            {
                if (Request.QueryString["t"] != null)
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=MEDICALSLIP&showword=0&showexcel=0&reqid=" + Request.QueryString["reqid"], false);
                }
                else if (Request.QueryString["csvsgid"] != null)
                {
                    Response.Redirect("CrewAppraisalMail.aspx?vslid=" + Request.QueryString["vslid"], false);
                }
                else if (Request.QueryString["itf"] != null)
                {
                    Response.Redirect("CrewReportsCrewChange.aspx?redir=yes");
                }
                else if (Request.QueryString["bday"] != null)
                {
                    Response.Redirect("../Crew/CrewQueryBirthday.aspx", false);
                }
                else if (Request.QueryString["wgrequest"] != null)
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=WORKGEARINDIVIDUALREQUEST&orderid=" + Request.QueryString["orderid"] + "&showmenu=0&showword=no&showexcel=no&emailyn=2", false);
                }
                else if (Request.QueryString["wgbulkrequest"] != null)
                {

                    Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=WORKINGGEARREQUEST&orderid=" + Request.QueryString["orderid"] + "&showmenu=0&showword=no&showexcel=no&emailyn=2", false);

                }
                else if (Request.QueryString["pdformoffshore"] != null)
                {
                    Response.Redirect("CrewReportsOffshoreShellPPRegistration.aspx?" + Request.QueryString.ToString());
                }
                else if (Request.QueryString["Assessment"] != null)
                {

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('NAFA', 'ifMoreInfo', null);", true);
                }
                else if (Request.QueryString["LicEmail"] != null)
                {
                    Response.Redirect("../Crew/CrewLicenceRequestList.aspx", false);
                }
                else
                {
                    Response.Redirect("../Crew/CrewLicenceProcess.aspx", false);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SendMail()
    {

        try
        {
            if (Request.QueryString["t"] != null)
            {
                string[] str = { "" };
                str[0] = Session["medreqatt"].ToString();
                PhoenixMail.SendMail(txtTO.Text.Trim(), txtCC.Text.Trim(), txtBCC.Text.Trim(), txtSubject.Text.Trim(), edtBody.Content, true, (MailPriority)Convert.ToInt32(ddlPriority.SelectedValue.ToString()), "", str, null);
            }
            else if (Request.QueryString["wgrequest"] != null)
            {
                string[] str = { "" };
                str[0] = Session["wgrequest"].ToString();
                PhoenixMail.SendMail(txtTO.Text.Trim(), txtCC.Text.Trim(), txtBCC.Text.Trim(), txtSubject.Text.Trim(), edtBody.Content, true, (MailPriority)Convert.ToInt32(ddlPriority.SelectedValue.ToString()), "", str, null);
            }
            else if (Request.QueryString["wgbulkrequest"] != null)
            {
                string[] str = { "" };
                str[0] = Session["wgbulkrequest"].ToString();
                PhoenixMail.SendMail(txtTO.Text.Trim(), txtCC.Text.Trim(), txtBCC.Text.Trim(), txtSubject.Text.Trim(), edtBody.Content, true, (MailPriority)Convert.ToInt32(ddlPriority.SelectedValue.ToString()), "", str, null);
            }
            else if (Request.QueryString["itf"] != null)
            {
                PhoenixMail.SendMail(txtTO.Text.Trim(), txtCC.Text.Trim(), txtBCC.Text.Trim(), txtSubject.Text.Trim(), edtBody.Content, true, (MailPriority)Convert.ToInt32(ddlPriority.SelectedValue.ToString()), ViewState["mailsessionid"].ToString());
            }
            else if (Request.QueryString["pdformoffshore"] != null)
            {
                PhoenixMail.SendMail(txtTO.Text.Trim(), txtCC.Text.Trim(), txtBCC.Text.Trim(), txtSubject.Text.Trim(), edtBody.Content, true, (MailPriority)Convert.ToInt32(ddlPriority.SelectedValue.ToString()), ViewState["mailsessionid"].ToString(), General.GetNullableInteger(ViewState["empid"].ToString()));
            }
            else
                PhoenixMail.SendMail(txtTO.Text.Trim(), txtCC.Text.Trim(), txtBCC.Text.Trim(), txtSubject.Text.Trim(), edtBody.Content, true, (MailPriority)Convert.ToInt32(ddlPriority.SelectedValue.ToString()), ViewState["mailsessionid"].ToString());
        }
        catch (Exception ex)
        {
            throw ex.InnerException;
        }
    }

    protected void OpenAttachment(object sender, CommandEventArgs e)
    {
        try
        {
            string s = e.CommandArgument.ToString();          
            string frameScript = "<script language='javascript'>" + "window.open('EmailAttachments/" + ViewState["mailsessionid"].ToString() + "/" + e.CommandArgument.ToString().Replace(",", "").Trim() + "');</script>";
            Response.Write(frameScript);
        }
        catch (Exception ex)
        {
            StringBuilder sbError = new StringBuilder();
            throw ex;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Session["AttachFiles"] != null)
        {
            lstAttachments.Items.Clear();
            DataTable dt = (DataTable)Session["AttachFiles"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAttachments.Items.Add(new RadListBoxItem(dt.Rows[i]["Text"].ToString(), dt.Rows[i]["Value"].ToString()));
                lstAttachments.Attributes.Add("ondblclick", "javascript:Openfile(this);");
            }
            Session["AttachFiles"] = null;
        }
    }

    private bool IsValidateEmail()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.IsvalidEmail(txtTO.Text))
        {
            ucError.ErrorMessage = "Please enter valid To E-Mail Address";
        }
        if (txtCC.Text.Trim() != string.Empty && !General.IsvalidEmail(txtTO.Text))
        {
            ucError.ErrorMessage = "Please enter valid Cc E-Mail Address";
        }
        if (txtBCC.Text.Trim() != string.Empty && !General.IsvalidEmail(txtBCC.Text))
        {
            ucError.ErrorMessage = "Please enter valid Bcc E-Mail Address";
        }
        return (!ucError.IsError);
    }

    private void BirthdayEmail()
    {
        DataTable dt = PhoenixCrewReports.BirthdayMessage(int.Parse(Request.QueryString["vesselid"].ToString()),
             int.Parse(Request.QueryString["empid"].ToString()), int.Parse(Request.QueryString["rankid"].ToString()),
             int.Parse(Request.QueryString["supy"].ToString()));

        StringBuilder sbemailbody = new StringBuilder();
        DataRow dr = dt.Rows[0];
        txtSubject.Text = "Happy Birthday!!!";
        edtBody.Content = dr["FLDBODYOFMAIL"].ToString();
        txtTO.Text = dr["FLDTOADDRESS"].ToString();
        txtCC.Text = dr["FLDCCADDRESS"].ToString();
    }

    private void LicenceEmail()
    {
        try
        {
            DataTable dt = PhoenixCrewLicenceRequest.HongKongCoveringLetterReport(Request.QueryString["processid"]);
            StringBuilder sbemailbody = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                sbemailbody.AppendLine("K/Attn: " + " " + dr["FLDATTENTION"].ToString());
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("<br/>Pls find attached " + Request.QueryString["sub"] + " application/s for following officers along with required documents, License Verification & photo:.");
                sbemailbody.AppendLine("<br/><table cellspacing='1' cellpadding='1' width='50%'>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sbemailbody.AppendLine("<tr><td>" + dt.Rows[i]["FLDSRNO"].ToString() + "</td><td>" + dt.Rows[i]["FLDRANK"].ToString() + "</td>");
                    sbemailbody.AppendLine("<td>" + dt.Rows[i]["FLDNAME"].ToString() + "</td><td>" + dt.Rows[i]["FLDDOCUMENTNAME"].ToString().TrimEnd('+') + "</td></tr>");
                }
                sbemailbody.AppendLine("</table>");
                sbemailbody.AppendLine("<br/>Attached herewith is the remittance detail toward above " + Request.QueryString["sub"] + " application.");
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("<br/>Request you to kindly issue " + Request.QueryString["sub"] + " documents to the above officer & forward the CRA to our email ID crewmumbai@executiveship.com.");
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("<br/>Kindly confirm if all attached dox are in order or do you need any addl dox for issuance of CRA & full term " + Request.QueryString["sub"] + " document.");
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("<br/>Many thanks in advance for your kind co-operation in this matter.");
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("<br/>Best Regards<br/>");
                sbemailbody.AppendLine(dr["FLDAUTHORIZEDREPRESENTATIVE"].ToString() + " --Personnel Officer");
                sbemailbody.AppendLine("<br/>For Executive Ship Management Pte. Ltd. ");
                sbemailbody.AppendLine("<br/>(As Agents only)");
                sbemailbody.AppendLine("<br/>Sai Commercial Annexee,");
                sbemailbody.AppendLine("<br/>2nd Floor, BKS Devshi Marg,");
                sbemailbody.AppendLine("<br/>Govandi Station Road,");
                sbemailbody.AppendLine("<br/>Govandi East, Mumbai - 400088.");
                sbemailbody.AppendLine("<br/>Tel : 91 22 67551700 / 67551777");
                sbemailbody.AppendLine("<br/>Fax : 91 22 67551771");
                sbemailbody.AppendLine("<br/>Email : crewlicense@executiveship.com");
                sbemailbody.AppendLine("<br/>");
                txtTO.Text = dt.Rows[0]["FLDEMAIL"].ToString();
                txtSubject.Text = Request.QueryString["sub"] + " application";
                edtBody.Content = sbemailbody.ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void LicenceEmailNew()
    {
        try
        {
            DataTable dt = PhoenixCrewLicenceRequest.HongKongCoveringLetterReportNew(new Guid(Request.QueryString["processid"]));
            if (dt.Rows.Count > 0)
            {
                StringBuilder sbemailbody = new StringBuilder();
                DataRow dr = dt.Rows[0];
                sbemailbody.AppendLine("K/Attn: " + " " + dr["FLDATTENTION"].ToString());
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("<br/>Pls find attached " + Request.QueryString["sub"] + " application/s for following officers along with required documents, License Verification & photo:.");
                sbemailbody.AppendLine("<br/><table cellspacing='1' cellpadding='1' width='50%'>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sbemailbody.AppendLine("<tr><td>" + dt.Rows[i]["FLDSRNO"].ToString() + "</td><td>" + dt.Rows[i]["FLDRANK"].ToString() + "</td>");
                    sbemailbody.AppendLine("<td>" + dt.Rows[i]["FLDNAME"].ToString() + "</td><td>" + dt.Rows[i]["FLDDOCUMENTNAME"].ToString().TrimEnd('+') + "</td></tr>");
                }
                sbemailbody.AppendLine("</table>");
                sbemailbody.AppendLine("<br/>Attached herewith is the remittance detail toward above " + Request.QueryString["sub"] + " application.");
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("<br/>Request you to kindly issue " + Request.QueryString["sub"] + " documents to the above officer & forward the CRA to our email ID crewmumbai@executiveship.com.");
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("<br/>Kindly confirm if all attached dox are in order or do you need any addl dox for issuance of CRA & full term " + Request.QueryString["sub"] + " document.");
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("<br/>Many thanks in advance for your kind co-operation in this matter.");
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("<br/>Best Regards<br/>");
                sbemailbody.AppendLine(dr["FLDAUTHORIZEDREPRESENTATIVE"].ToString() + " --Personnel Officer");
                sbemailbody.AppendLine("<br/>For Executive Ship Management Pte. Ltd. ");
                sbemailbody.AppendLine("<br/>(As Agents only)");
                sbemailbody.AppendLine("<br/>Sai Commercial Annexee,");
                sbemailbody.AppendLine("<br/>2nd Floor, BKS Devshi Marg,");
                sbemailbody.AppendLine("<br/>Govandi Station Road,");
                sbemailbody.AppendLine("<br/>Govandi East, Mumbai - 400088.");
                sbemailbody.AppendLine("<br/>Tel : 91 22 67551700 / 67551777");
                sbemailbody.AppendLine("<br/>Fax : 91 22 67551771");
                sbemailbody.AppendLine("<br/>Email : crewlicense@executiveship.com");
                sbemailbody.AppendLine("<br/>");
                txtTO.Text = dt.Rows[0]["FLDEMAIL"].ToString();
                txtSubject.Text = Request.QueryString["sub"] + " application";
                edtBody.Content = sbemailbody.ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void MedicalRequestMail()
    {
        DataTable dt = PhoenixCrewMedical.ReportMedicalSlip(new Guid(Request.QueryString["reqid"].ToString())).Tables[0];
        StringBuilder sbemailbody = new StringBuilder();

        DataRow dr = dt.Rows[0];
        sbemailbody.AppendLine("Date : " + DateTime.Now.ToString("dd/MM/yyyy"));
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>Dear Dr.:" + " " + dr["FLDDOCTORNAME"].ToString());
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>please note,the subject officer would be at your clinic at '" + string.Format("{0:hh:mm tt}", General.GetNullableDateTime(dr["FLDAPPOINTMENTDATE"].ToString())) + "' on '" + string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dr["FLDAPPOINTMENTDATE"].ToString())) + "' for the pre joining medicals.Kindly forward us the medicals earliest.");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>Thank you,");
        sbemailbody.AppendLine("<br/>" + dr["FLDSENTBY"].ToString() + "");
        sbemailbody.AppendLine("<br/>");
        txtTO.Text = dt.Rows[0]["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(',');
        txtCC.Text = dt.Rows[0]["FLDZONEEMAIL"].ToString().Replace(";", ",").TrimEnd(',');
        txtSubject.Text = "Medical request for " + "     " + dt.Rows[0]["FLDEMPLOYEENAME"].ToString() + " - " + dt.Rows[0]["FLDRANK"].ToString();
        edtBody.Content = sbemailbody.ToString();
    }
    private void AppraisalMail()
    {

        PhoenixSecurityContext psc = PhoenixSecurityContext.CurrentSecurityContext;
        string username = string.Empty;
        if (psc.FirstName.ToUpper().Equals(psc.LastName.ToUpper()))
        {
            username = psc.FirstName + " " + psc.MiddleName;
        }
        else
        {
            username = psc.FirstName + " " + psc.MiddleName + " " + psc.LastName;
        }
        DataTable dt = PhoenixRegistersVesselCommunicationDetails.EditCommunicationDetails(int.Parse(Request.QueryString["vslid"])).Tables[0];
        StringBuilder sbemailbody = new StringBuilder();

        sbemailbody.AppendLine("Date : " + DateTime.Now.ToString("dd/MM/yyyy"));
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>Good Day ,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>Would request you to kindly complete the appraisals for the below mentioned seafarers signed off from ");
        sbemailbody.AppendLine("<br/>your good vessel.");// on the DATE at PORT");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>" + Session["AppraisalMail"].ToString());
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>SIGNATURE OF THE PO");
        sbemailbody.AppendLine("<br/>" + username);
        sbemailbody.AppendLine("<br/>");
        txtTO.Text = dt.Rows.Count > 0 ? dt.Rows[0]["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(',') : string.Empty;
        txtSubject.Text = "APPRAISAL REMINDER MAIL TO MASTER";
        edtBody.Content = sbemailbody.ToString();
    }

    private void CourseMail()
    {
        DataTable dt = PhoenixNewApplicantCourse.CrewCourseEmail(Convert.ToInt32(Request.QueryString["empid"]), General.GetNullableString(Request.QueryString["csvcourseid"].ToString()));
        StringBuilder sbemailbody = new StringBuilder();
        if (dt.Rows.Count >= 1)
        {
            DataRow dr = dt.Rows[0];
            sbemailbody.AppendLine("Date : " + DateTime.Now.ToString("dd/MM/yyyy"));
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>Employee No:" + " " + dr["FLDEMPLOYEECODE"].ToString());
            sbemailbody.AppendLine("<br/>Rank:" + " " + dr["FLDRANKNAME"].ToString());
            sbemailbody.AppendLine("<br/>Name" + " " + dr["FLDEMPLOYEENAME"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>Please verify the below documents,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            foreach (DataRow dr1 in dt.Rows)
            {
                sbemailbody.AppendLine("<br/>Course:" + " " + dr1["FLDCOURSE"].ToString());
                sbemailbody.AppendLine("<br/>Nationality:" + " " + dr1["FLDCOUNTRYNAME"].ToString());
                sbemailbody.AppendLine("<br/>Certificate No:" + " " + dr1["FLDCOURSENUMBER"].ToString());
                sbemailbody.AppendLine("<br/>Place of Issue:" + " " + dr1["FLDPLACEOFISSUE"].ToString());
                sbemailbody.AppendLine("<br/>Date of Issue:" + " " + dr1["FLDDATEOFISSUE"].ToString());
                sbemailbody.AppendLine("<br/>Date of expiry:" + " " + dr1["FLDDATEOFEXPIRY"].ToString());
                sbemailbody.AppendLine("<br/>Institution:" + " " + dr1["FLDNAME"].ToString());
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("<hr/>");

            }
            sbemailbody.AppendLine("<br/>Thank you,");
            sbemailbody.AppendLine("<br/>" + dr["FLDSENTBY"].ToString() + "");
            sbemailbody.AppendLine("<br/>");
            txtTO.Text = dt.Rows[0]["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(',');
            txtSubject.Text = "Course(s) for verification";
            edtBody.Content = sbemailbody.ToString();
            lblnote.Visible = true;
            DataTable dt1 = PhoenixCrewCourseCertificate.ListCrewCourseFileAttachment(Convert.ToInt32(Request.QueryString["empid"])
                , General.GetNullableString(Request.QueryString["csvcourseid"].ToString()));
            if (dt1.Rows.Count > 0)
            {
                string dirpath = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString()).ToString();
                if (!Directory.Exists(dirpath))
                    Directory.CreateDirectory(dirpath);
                string filepath = string.Empty;
                lstAttachments.Items.Clear();
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    filepath = Server.MapPath("~/Attachments/" + dt1.Rows[i]["FLDFILEPATH"].ToString()).ToString();
                    if (File.Exists(filepath))
                    {
                        FileInfo fi = new FileInfo(filepath);
                        fi.CopyTo(dirpath + "/" + dt1.Rows[i]["FLDFILENAME"].ToString(), true);
                        lstAttachments.Items.Add(new RadListBoxItem(dt1.Rows[i]["FLDABBREVIATION"].ToString() + "_" + dt1.Rows[i]["FLDFILENAME"].ToString(), dt1.Rows[i]["FLDFILEPATH"].ToString()));
                        lstAttachments.Attributes.Add("ondblclick", "javascript:Openfile(this);");


                    }

                }
            }
        }
    }

    private void CrewChangeMail()
    {
        int rowcount = 0;
        int totalrowcount = 0;
        DataSet ds = PhoenixCrewReportsCrewChange.CrewChangeList(int.Parse(Request.QueryString["vesselid"].ToString()), DateTime.Parse(Request.QueryString["fromdate"].ToString()),
                                                                    DateTime.Parse(Request.QueryString["todate"].ToString()), null, null, 1, 1, ref rowcount, ref totalrowcount);
        DataTable dt = ds.Tables[2];
        StringBuilder sbemailbody = new StringBuilder();
        if (dt.Rows.Count >= 1)
        {
            DataRow dr = dt.Rows[0];
            sbemailbody.AppendLine("To:" + dt.Rows[0]["FLDNAME"].ToString() + "<br/>");
            sbemailbody.AppendLine("From:" + dt.Rows[0]["FLDFROM"] + "<br/>");            
            sbemailbody.AppendLine("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + "<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Dear" + dt.Rows[0]["FLDATTENTION"] + "<br/>");
            sbemailbody.AppendLine("Please Note the crew change conducted on the following vessels From &nbsp;" + dt.Rows[0]["FLDFROMDATE"] + "&nbsp;To&nbsp;" + dt.Rows[0]["FLDTODATE"] + ":<br/>");
            string[] vesselname = dt.Rows[0]["FLDVESSELS"].ToString().Split(',');
            int count = 1;
            foreach (string vssl in vesselname)
            {
                sbemailbody.AppendLine(count + ". " + vssl);
                sbemailbody.AppendLine("<br/>");
                count++;
            }
            sbemailbody.AppendLine("Please Find Attached Documents:");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Kindly acknowledge receipt of this email with attachments.<br/>");
            sbemailbody.AppendLine("Thank You & Best Regards,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(dt.Rows[0]["FLDUSERNAME"].ToString() + "<br/>");
            sbemailbody.AppendLine(dt.Rows[0]["FLDCOMPANYNAME"].ToString());
            sbemailbody.AppendLine("<br/>(As Agents For Owners)<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(dt.Rows[0]["FLDADDRESS1"].ToString() + "<br/>");
            sbemailbody.AppendLine(dt.Rows[0]["FLDADDRESS2"].ToString() + "<br/>");
            sbemailbody.AppendLine(dt.Rows[0]["FLDADDRESS3"].ToString() + "<br/>");
            sbemailbody.AppendLine("Tel:" + dt.Rows[0]["FLDPHONE"].ToString() + "<br/>");
            sbemailbody.AppendLine("Fax:" + dt.Rows[0]["FLDFAX"].ToString() + "<br/>");
            sbemailbody.AppendLine("E-Mail:" + dt.Rows[0]["FLDEMAIL"].ToString() + "<br/>");

            txtTO.Text = dt.Rows[0]["FLDEMAIL1"].ToString();
            txtSubject.Text = "JSU Submission";
            edtBody.Content = sbemailbody.ToString();
        }

    }

    private void WorkGearRequestMail()
    {
        DataSet ds = PhoenixCrewWorkGearIndividualRequest.ReportWorkGearIndividualRequest(new Guid(Request.QueryString["orderid"].ToString()));
        StringBuilder sbemailbody = new StringBuilder();

        DataRow dr = ds.Tables[0].Rows[0];
        DataRow dr2 = ds.Tables[1].Rows[0];
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Date : " + DateTime.Now.ToString("dd/MM/yyyy"));
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Attention:" + " " + dr["FLDSUPPLIERNAME"].ToString());
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Request Ref No.:" + " " + dr["FLDREFERENCENO"].ToString());
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Phone No:" + " " + dr["FLDPHONE"].ToString());
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("E-Mail:" + " " + dr["FLDEMAIL"].ToString());
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Request Date:" + " " + dr["FLDREQUESTDATE"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Kindly issue the items as per the attached request to the seafarer");
        sbemailbody.AppendLine("<br/>Thank you,");
        sbemailbody.AppendLine("<br/>" + dr["FLDSENTBY"].ToString() + "");
        sbemailbody.AppendLine("<br/>");
        txtTO.Text = dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(',');
        txtSubject.Text = "Work Gear request for " + "     " + dr2["FLDEMPLOYEENAME"].ToString() + " - " + dr2["FLDRANKNAME"].ToString();
        edtBody.Content = sbemailbody.ToString();
    }

    private void WorkGearBulkRequestMail()
    {
        DataSet ds = new DataSet();

         ds= PhoenixCrewWorkingGearOrderForm.ReportWorkingGearRequest(General.GetNullableGuid(Request.QueryString["Neededid"].ToString()), General.GetNullableInteger(Request.QueryString["Agentid"].ToString()));
        DataTable dt = ds.Tables[0];
        StringBuilder sbemailbody = new StringBuilder();

        DataRow dr = dt.Rows[0];
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Date : " + DateTime.Now.ToString("dd/MM/yyyy"));
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Attention:" + " " + dr["FLDSUPPLIERNAME"].ToString());
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Request Ref No.:" + " " + dr["FLDREFERENCENO"].ToString());
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Phone No:" + " " + dr["FLDPHONE"].ToString());
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("E-Mail:" + " " + dr["FLDEMAIL"].ToString());
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Request Date:" + " " + dr["FLDREQUESTDATE"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Kindly send the items as per the attached request.");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>Thank you,");
        sbemailbody.AppendLine("<br/>" + dr["FLDSENTBY"].ToString() + "");
        sbemailbody.AppendLine("<br/>");
        txtTO.Text = dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(',');
        txtSubject.Text = "Working Gear Request";
        edtBody.Content = sbemailbody.ToString();
    }
    private void PDFormOffshore()
    {
        try
        {
            string Tmpfilelocation = string.Empty;
            DataSet ds = PhoenixCrewReports.OffshoreShellPPRegistrationReport(General.GetNullableString(Request.QueryString["fileno"]), General.GetNullableInteger(Request.QueryString["vesselid"]));
            string[] rptfile = new string[1];
            rptfile[0] = System.Web.HttpContext.Current.Server.MapPath("../Reports/ReportsCrewOffshoreShellPPRegistration.rpt");

            Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
            Tmpfilelocation = Tmpfilelocation + "Attachments/Crew/PDFORM/" + Convert.ToString(Guid.NewGuid()) + ".pdf";
            PhoenixReportClass.ExportReport(rptfile, Tmpfilelocation, ds);


            string pdffile = string.Empty;
            Int64 empid = 0;
            DataTable sourceFiles = PhoenixCrewReports.ListOffshoreShellPPRegistrationDocument(Request.QueryString["fileno"], Tmpfilelocation, ref pdffile, ref empid);
            ViewState["empid"] = empid;
            DataTable Tmpfiles = new DataTable();
            Tmpfiles.Clear();
            Tmpfiles.Columns.Add("NAME");
            Tmpfiles.Rows.Add(Tmpfilelocation);

            string dirpath = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString());
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);

            if (sourceFiles.Rows.Count > 0)
            {
                FileStream fs = new FileStream(dirpath + "/" + pdffile + ".pdf", FileMode.Create);
                int f = 0;
                // step 1: we create a new document
                Document document = new Document();

                // step 2: we create a writer that listens to the document		
                PdfWriter writer = PdfWriter.GetInstance(document, fs);

                //step 3: open the document
                document.Open();
                while (f < sourceFiles.Rows.Count)
                {
                    if ((sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".pdf") && File.Exists(sourceFiles.Rows[f][0].ToString()))
                    {
                        PdfReader reader = new PdfReader(sourceFiles.Rows[f][0].ToString());
                        // we retrieve the total number of pages			
                        int n = reader.NumberOfPages;
                        if (n > 0)
                        {
                            PdfContentByte cb = writer.DirectContent;
                            PdfImportedPage page;
                            int rotation;
                            int i = 0;
                            // step 4: we add content	
                            while (i < n) // loop thorugh total no of pages
                            {
                                i++;

                                document.SetPageSize(reader.GetPageSizeWithRotation(i));
                                document.NewPage();
                                page = writer.GetImportedPage(reader, i);
                                rotation = reader.GetPageRotation(i);
                                if (rotation == 90 || rotation == 270)
                                {
                                    cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                                }
                                else
                                {
                                    cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                                }

                            }
                        }
                    }
                    else if (((sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".jpg") ||
                            (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".jpeg") ||
                            (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".gif") ||
                            (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".png") ||
                            (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".tiff") ||
                            (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".tiff") ||
                            (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".jif") ||
                            (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".jfif") ||
                            (sourceFiles.Rows[f][0].ToString().ToLower()).Contains(".bmp")) && File.Exists(sourceFiles.Rows[f][0].ToString()))
                    {
                        document.NewPage();
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(sourceFiles.Rows[f][0].ToString());
                        img.ScaleToFit((PageSize.A4.Width - 20), (PageSize.A4.Height - 20));
                        document.Add(img);
                    }
                    f++;

                }
                // step 5: we close the document	

                document.Close();
                fs.Close();

                if (Tmpfiles.Rows.Count > 0)
                {
                    int i = 0;
                    while (i < Tmpfiles.Rows.Count)
                    {
                        File.Delete(Tmpfiles.Rows[i][0].ToString());
                        i++;
                    }

                }

                lstAttachments.Items.Clear();

                PhoenixCrewReports.ZipOffshoreShellPPRegistration(Request.QueryString["fileno"], ViewState["mailsessionid"].ToString(), pdffile);

                if (File.Exists(dirpath + "/" + pdffile + ".zip"))
                {
                    lstAttachments.Items.Add(new RadListBoxItem(pdffile + ".zip", "EmailAttachments\\" + ViewState["mailsessionid"].ToString() + "\\" + pdffile + ".zip"));
                    lstAttachments.Attributes.Add("ondblclick", "javascript:Openfile(this);");
                }
                File.Delete(dirpath + "/" + pdffile + ".pdf");
            }
            else
            {
                ucError.ErrorMessage = "No attachments found.";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EmployeeAssessmentMail()
    {
        string AssessmantFile = "";
        PhoenixCrew2XL.Export2XLSupdtAssessment(Convert.ToInt32(Request.QueryString["empid"])
                                                        , int.Parse(Request.QueryString["SignonOffId"])
                                                        , General.GetNullableGuid(Request.QueryString["AssessmentId"])
                                                        , Request.QueryString["Rank"], ref AssessmantFile);

        DataTable dt = PhoenixCrewEmployeeSuptAssessment.EmployeeSuptAssessmentEmail(Convert.ToInt32(Request.QueryString["empid"]));
        StringBuilder sbemailbody = new StringBuilder();
        if (dt.Rows.Count >= 1)
        {
            DataRow dr = dt.Rows[0];
            sbemailbody.AppendLine("Date : " + DateTime.Now.ToString("dd/MM/yyyy"));
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>Employee No:" + " " + dr["FLDEMPLOYEECODE"].ToString());
            sbemailbody.AppendLine("<br/>Rank:" + " " + dr["FLDRANKNAME"].ToString());
            sbemailbody.AppendLine("<br/>Name" + " " + dr["FLDEMPLOYEENAME"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");

            sbemailbody.AppendLine("<br/>Thank you,");
            sbemailbody.AppendLine("<br/>" + dr["FLDSENTBY"].ToString() + "");
            sbemailbody.AppendLine("<br/>");
            txtTO.Text = dt.Rows[0]["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(',');
            txtSubject.Text = "Seafarer Assessment";
            edtBody.Content = sbemailbody.ToString();
            lblnote.Visible = true;

            string dirpath = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString()).ToString();
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);
            string filepath = string.Empty;
            lstAttachments.Items.Clear();
            filepath = Server.MapPath("~/Attachments/Inspection/" + AssessmantFile + ".xlsm");
            if (File.Exists(filepath))
            {
                FileInfo fi = new FileInfo(filepath);
                fi.MoveTo(dirpath + "/" + ViewState["mailsessionid"].ToString() + ".xlsm");
                lstAttachments.Items.Add(new RadListBoxItem("OC28.xlsm", "EmailAttachments/" + ViewState["mailsessionid"].ToString() + "/" + ViewState["mailsessionid"].ToString() + ".xlsm"));
                lstAttachments.Attributes.Add("ondblclick", "javascript:Openfile(this);");
            }
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        
        String scriptpopup = String.Format("javascript:openNewWindow('','','" + Session["sitepath"] + "/attachments/" + lstAttachments.SelectedValue.ToString() + " ');");

        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);


    }

    protected void btnAttDel_Click(object sender, EventArgs e)
    {
        ShowCheckedItems(lstAttachments);
    }

    private void ShowCheckedItems(RadListBox listBox)
    {
        StringBuilder sb = new StringBuilder();
        IList<RadListBoxItem> collection = listBox.SelectedItems;

        string sessionid = ViewState["mailsessionid"].ToString();
        
        foreach (RadListBoxItem item in collection)
        {
            MessageDelete(sessionid, item.Text);
            listBox.Delete(item);
        }
    }
    public static void MessageDelete(string sessionid, string filename)
    {
        try
        {
            string destPath = HttpContext.Current.Server.MapPath("~/Attachments/EmailAttachments/" + sessionid + "/" + filename);
            System.IO.File.Delete(destPath);            
        }
        catch (Exception ex)
        {
            StringBuilder sbError = new StringBuilder();
            throw ex;
        }
    }


}
