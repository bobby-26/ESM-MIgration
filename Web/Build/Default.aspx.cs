using System;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    
    {
        //litTitle.Text = Application["softwarename"].ToString();
        if (Request.QueryString["ReturnUrl"] != null && !Request.QueryString["ReturnUrl"].ToString().Contains("OptionsForgotPassword"))
        {
            Response.Redirect("Default.aspx");
        }

        //phoenixLogin.Focus();
        lnkSupport.HRef = ConfigurationManager.AppSettings.Get("SupportTicketUrl").ToString();

        if (!ConfigurationManager.AppSettings.Get("showloginlinks").ToString().ToUpper().Equals("YES"))
        {
            //litGuidanceText.Text = "<b>Click on the link to report</b>";
            //lblLoginLinks.Text = "Open Report";
            lnkSupport.Visible = false;
            lnkPresea.Visible = false;
            //lnkReportIncident.Visible = false;
            //lnkOpenReport.Visible = false;
            //lnkCrewComplaints.Visible = false;
            lnkNewApplicant.Visible = false;
        }
        else
        {
            //lblLoginLinks.Text = "Sign Up";
            //litGuidanceText.Text = "<b>Choose the application type that will describe your relationship with " + Session["companyshortname"] + "</b>";
            //lnkReportIncident.Visible = false;
            //lnkOpenReport.Visible = false;
        }
        //spnmarquee.InnerText = "Kind Attention!      " + litTitle.Text + " will not be operational from 06/June/2015 00:05 hrs to 07/June/2015 16:00 hrs (Singapore Std. Time) due to Scheduled Migration.";
        //if (!ConfigurationManager.AppSettings.Get("marqueeyn").ToString().ToUpper().Equals("TRUE"))
        //{
        //    mq.Visible = false;
        //}
        
        if (ConfigurationManager.AppSettings.Get("databaseconfig").ToString().ToUpper().Equals("TRUE"))
        {
            txtCompanyCode.Visible = true;
        }
    }

    protected void phoenixLogin_Authenticate(object sender, EventArgs ae)
    {
        try
        {
            string usertype = "";
            string useridentity = "";

            useridentity = (Request.QueryString["UID"] == null) ? Guid.NewGuid().ToString() : Request.QueryString["UID"].ToString();

            string UserName = txtUserName.Value;
            string Password = txtPassword.Value;
            HttpContext.Current.Session["COMPANYCODE"] = txtCompanyCode.Value.ToUpper();

            if (ConfigurationManager.AppSettings.Get("databaseconfig").ToString().ToUpper().Equals("TRUE"))
            {
                if (ConfigurationManager.AppSettings[HttpContext.Current.Session["COMPANYCODE"].ToString()] == null)
                {
                    spnmsg.InnerText = "Invalid Login Credentials.";
                    divMessage.Visible = true;
                    return;
                }
            }

            if (!PhoenixUser.AuthenticateUserIdentity(useridentity, Request.ServerVariables["REMOTE_ADDR"].ToString()))
            {
                spnmsg.InnerText = "You are not authorized to login.";
                divMessage.Visible = true;
                return;
            }

            PhoenixUser.LoginAudit(UserName, Password, Request.ServerVariables["REMOTE_ADDR"].ToString(), ref usertype);

            if (!PhoenixUser.Login(UserName, Password))
            {
                return;
            }

            PhoenixUser.SetUserContext(PhoenixSecurityContext.CurrentSecurityContext.UserCode, usertype);

            string Tokenid = null;

            PhoenixUser.LoginToken(UserName, ref Tokenid);

            Filter.CurrentLoginToken = Tokenid.ToString();

            FormsAuthentication.RedirectFromLoginPage(UserName, false);
        }
        catch (Exception ex)
        {
            spnmsg.InnerText = ex.Message;
            divMessage.Visible = true;
            return;
        }
    }

    protected void lnkOptions_Click(object sender, EventArgs e)
    {
        if (((LinkButton)sender).CommandName.Equals("NEWAPPLICANT"))
            Response.Redirect("~/Options/OptionsNewApplicant.aspx");
        if (((LinkButton)sender).CommandName.Equals("PRESEANEWAPPLICANT"))
            Response.Redirect("~/Presea/PreSeaRegisterdCandidatesLogin.aspx");
    }

    protected void lnkOpenReport_Click(object sender, EventArgs e)
    {
        if (((LinkButton)sender).CommandName.Equals("REPORTNEARMISS"))
            Response.Redirect("~/Options/OptionsDirectIncidentNearmissReporting.aspx?category=1");
        if (((LinkButton)sender).CommandName.Equals("OPENREPORT"))
            Response.Redirect("~/Options/OptionsOpenReporting.aspx?category=2");
        if (((LinkButton)sender).CommandName.Equals("CREWCOMPLAINTS"))
            Response.Redirect("~/Options/OptionsDirectIncidentReporting.aspx?category=3");
    }
}
