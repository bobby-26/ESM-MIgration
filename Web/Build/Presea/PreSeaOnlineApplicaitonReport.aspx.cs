using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Reports;
using System.Collections.Specialized;


public partial class PreSeaOnlineApplicaitonReport : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("SUMBIT", "SUBMIT");
                MenuPreSeaOnlineRegistration.AccessRights = this.ViewState;
                MenuPreSeaOnlineRegistration.MenuList = toolbar.Show();
                ViewState["PAGEURL"] = "../Reports/ReportsViewWithSubReport.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=10&reportcode=ONLINEREGISTRATION&applicantid=" + Filter.CurrentPreSeaNewApplicantSelection + "&showmenu=0&showword=NO&showexcel=NO&showPDF=Yes";
            }          
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPreSeaOnline_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SUBMIT"))
            {              
                //send mail
                ucConfirm.Visible = true;
                ucConfirm.Text = "Are you sure to submit the application?";
               
                
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public  void SendMailToCandidate()
    {
        string[] reportfile = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        string filename = "";
        NameValueCollection nvc = new NameValueCollection();
        nvc["applicantid"] = Filter.CurrentPreSeaNewApplicantSelection;
        DataTable dt = new DataTable();

        DataSet ds = PhoenixReportsPreSea.NewApplicantOnlineRegistration(nvc, ref reportfile, ref filename);
        reportfile[0] = reportfile[0].Replace("\\Presea\\", "\\Reports\\");
        PhoenixReportClass.ExportReport(reportfile, filename, ds); 

        dt = ds.Tables[0];
        string[] strarrfilenames = new string[1];
        strarrfilenames[0] = filename;// attachment;

        StringBuilder sbemailbody = new StringBuilder();

        DataRow dr = dt.Rows[0];
        sbemailbody.AppendLine(" <div style='border:double;font-family: Calibri; font-size: 13;width:70%;' >"); //<img  src='http://" + Request.Url.Authority + Session["images"] + "/sims.png" + "' height='75px' width='75px'/
        sbemailbody.AppendLine("<center><table width='90%' style='margin-left:50px;'><tr><td align='left'></td><td align='center' style='font-family:Arial Narrow;font-size:19;color:Gray;width: 90%'><b><u><h2>SAMUNDRA INSTITUTE OF MARITIME STUDIES</h2></u></b></td></tr>");
        sbemailbody.AppendLine("<tr><td colspan='2'></td></tr>");
        sbemailbody.AppendLine("<tr><td colspan='2'></td></tr>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<tr><td width='5%'></td><td align='center' style='font-family: Calibri; font-size: 15;text-align:center;'><b><u>ONLINE APPLICATION REGISTRATION ACKNOWLEDGEMENT</u></b></td></tr>");
        sbemailbody.AppendLine("<tr><td colspan='2'></td></tr>");
        sbemailbody.AppendLine("<tr> <td style='text-align:left;font-size: 13' colspan='2'>");
        sbemailbody.AppendLine("&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  Your name has been registered for " + dt.Rows[0]["FLDBATCHNAME"].ToString() + " course, kindly login with your user id and"); 
        sbemailbody.AppendLine("<br />password to complete the online application process.</td></tr>");
        sbemailbody.AppendLine("</table></center>");        
        sbemailbody.AppendLine("<br/>");       
        sbemailbody.AppendLine("<table width='90%' style='margin-left:50px;text-align:left;font-size: 13'><tr><td>User Name : " + dt.Rows[0]["FLDPERSONALMAIL"].ToString() + "</td></tr>");
        sbemailbody.AppendLine("<tr><td>Password  : " + dt.Rows[0]["FLDDATEOFBIRTH"].ToString() + "</td></tr></table>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<center>=====================================================================================================</center>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<table width='90%' style='margin-left:50px;font-family: Calibri; font-size: 13;text-align:left;'><tr><td style='font-size: 14'><b><u>Applicants Particulars:</b></u></td></tr>");
        sbemailbody.AppendLine("<tr><td>Roll No         :</td></tr>");
        sbemailbody.AppendLine("<tr><td>Name            :" + dt.Rows[0]["FLDCANDIDATENAME"].ToString() + "</td></tr>");
        sbemailbody.AppendLine("<tr><td>Contact No.     :" + dt.Rows[0]["FLDCONTACTNO"].ToString() + "</td></tr>");
        sbemailbody.AppendLine("<tr><td>Exam Center     :" + dt.Rows[0]["FLDEXAMVENUENAMELIST"].ToString()+"</td></tr>");
        sbemailbody.AppendLine("</table><br/><br/>");
        sbemailbody.AppendLine("<center>=====================================================================================================</center>");
        sbemailbody.AppendLine("<br/> <br/>");
        sbemailbody.AppendLine("<center><b><u>IMPORTANT NOTE:</u></b></center>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<table width='90%' style='margin-left:50px;font-family: Calibri; font-size: 13;text-align:left;'><tr><td>1.&nbsp;&nbsp; Candidate must take the printout of application form once it is completed and submit it to");
        sbemailbody.AppendLine("<br/>    &nbsp; &nbsp; &nbsp; Admission Team on the date of entrance exam.</td></tr>");
        sbemailbody.AppendLine("<tr><td>2.&nbsp;&nbsp; Incomplete application form will not be entertained.</td></tr>");
        sbemailbody.AppendLine("<tr><td>3.&nbsp;&nbsp; For further queries please contact our Admission Team at below mentioned address.</td></tr></table>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<center> --------------------- XXX ------------------- </center>");
        sbemailbody.AppendLine("<center>SAMUNDRA INSTITUTE OF MARITIME STUDIES");
        sbemailbody.AppendLine("<br/>Takwe-Khurd, Mumbai-Pune Highway (NH-4), Lonavla – 410 405");
        sbemailbody.AppendLine("<br/>Dist: Pune, Maharashtra ");
        sbemailbody.AppendLine("<br/>Tel:  02114–399500, Fax No. 399600; Email: <a href='admission.sims@samundra.com' target='_blank'>admission.sims@samundra.com</a>; Website: <a href='http://www.samundra.com' target='_blank'>www.samundra.com</a></center>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<center>=====================================================================================================</center>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("</div>");
        string emailbody = sbemailbody.ToString();


        PhoenixMail.SendMail(dt.Rows[0]["FLDPERSONALMAIL"].ToString(), "admission.sims@samundra.com",
            "", //admission.sims@samundra.com
            "ONLINE APPLICATION REGISTRATION ACKNOWLEDGEMENT - " + dt.Rows[0]["FLDCANDIDATENAME"].ToString(),
            emailbody, true,
            System.Net.Mail.MailPriority.Normal,
            "",
            strarrfilenames,
            null);       
        Session["SUBMITCLICKED"] = "1";
        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        
 
    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
       
            try
            {
                UserControlConfirmMessage ucSendMail = sender as UserControlConfirmMessage;
                if (ucSendMail.confirmboxvalue == 1)
                {
                    PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantPersonalDeclarationUpadte(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                            , Convert.ToInt32(Filter.CurrentPreSeaNewApplicantSelection)
                                                                                            , General.GetNullableByte("1")); //checked
                    SendMailToCandidate();
                }
                    
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
       
     
    }

}
