using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web;
using SouthNests.Phoenix.Portal;
using SouthNests.Phoenix.Common;

public partial class PortalSeafarerDefault : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


    }

    protected void phoenixLogin_Authenticate(object sender, EventArgs ae)
    {
        string usertype = "";
        try
        {
            Filter.IsPortal = true;
            Filter.PortalAccessedBy = "SEAFARER";

            string UserName = txtUserName.Value;
            string Password = txtPassword.Value;
            
            PhoenixUser.LoginAudit(UserName, Password, Request.ServerVariables["REMOTE_ADDR"].ToString(), ref usertype);

            if (!PhoenixUser.PortalLogin(UserName, Password))
            {               
                return;
            }
            PhoenixUser.SetUserContext(PhoenixSecurityContext.CurrentSecurityContext.UserCode, usertype);
            //OTP Generation
            DataTable dt = PheonixSeafarerUserLogin.GetUserDetails(UserName);
            if (dt.Rows.Count > 0)
            {

                //string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                //string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
                string numbers = "1234567890";

                string characters = numbers;
               // characters += alphabets + small_alphabets + numbers;

                int length = 6;
                string otp = string.Empty;
                for (int i = 0; i < length; i++)
                {
                    string character = string.Empty;
                    do
                    {
                        int index = new Random().Next(0, characters.Length);
                        character = characters.ToCharArray()[index].ToString();
                    } while (otp.IndexOf(character) != -1);
                    otp += character;

                }
                ViewState["otp"] = "";
                ViewState["otp"] = otp;               
            }
            //end OTP generation
            string strEmailTxt = @"<pre> 
            Your OTP for login phoenix system is                     
            OTP     :<OTP>
                        
            Regards</pre>" + HttpContext.Current.Session["companyname"].ToString() +
            
            @"< pre >" + GetCompanyURL() +
            @"</pre>";
            Guid? otpid = null;
            PhoenixUser.PortalLoginOTPInsert(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString()), ViewState["otp"].ToString(), ref otpid);
            PhoenixMail.SendMailFromPortal(UserName, null, null, "OTP for login"
                   , strEmailTxt.Replace("<OTP>", ViewState["otp"].ToString())
                   , true, System.Net.Mail.MailPriority.Normal, string.Empty, "1");
            Response.Redirect("../Portal/PortalSeafarerOTPVerify.aspx?ID="+ otpid.ToString() + "&UN="+ UserName);
            //FormsAuthentication.RedirectFromLoginPage(UserName, false);
        }
        catch (Exception ex)
        {
            spnmsg.InnerText = ex.Message;
            divMessage.Visible = true;
            return;
        }
    }
    protected string GetCompanyURL()
    {
        DataTable dt = PhoenixCommonAdministration.GetCompanyURL(2);

        if (dt.Rows.Count > 0)
        {
            string CompanyURL = dt.Rows[0]["FLDURL"].ToString();
            return CompanyURL;
        }
        else
        {
            return null;
        }
    }
}