using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;


public partial class Portal_PortalStudentDefault : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

       
    }

    protected void phoenixLogin_Authenticate(object sender, AuthenticateEventArgs ae)
    {
        //string usertype = "";
        try
        {
            Filter.IsStudentPortal = true;
            Filter.PortalAccessedBy = "STUDENT";
            //PhoenixUser.PreSeaLoginAudit(phoenixLogin.UserName, phoenixLogin.Password, Request.ServerVariables["REMOTE_ADDR"].ToString(), ref usertype);

            if (!PhoenixStudentUser.StudentPortalLogin(phoenixLogin.UserName, phoenixLogin.Password))
            {
                ae.Authenticated = false;
                return;
            }

            //PhoenixUser.SetUserContext(PhoenixSecurityContext.CurrentSecurityContext.UserCode, usertype);
            FormsAuthentication.RedirectFromLoginPage(phoenixLogin.UserName, false);
        }
        catch (Exception ex)
        {
            phoenixLogin.FailureText = ex.Message;
            ae.Authenticated = false;
            return;
        }
    }

    protected void lnkOptions_Click(object sender, EventArgs e)
    {
        if (((LinkButton)sender).CommandName.Equals("REGISTER"))
            Response.Redirect("~/Options/OptionsStudentRegister.aspx");
    }
}
