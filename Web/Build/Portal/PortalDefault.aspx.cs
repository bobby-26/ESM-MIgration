using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class PortalDefault : System.Web.UI.Page
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
            Filter.PortalAccessedBy = "AGENT";

            string UserName = txtUserName.Value;
            string Password = txtPassword.Value;

            PhoenixUser.LoginAudit(UserName, Password, Request.ServerVariables["REMOTE_ADDR"].ToString(), ref usertype);

            if (!PhoenixUser.PortalLogin(UserName, Password))
            {              
                return;
            }

            PhoenixUser.SetUserContext(PhoenixSecurityContext.CurrentSecurityContext.UserCode, usertype);
            FormsAuthentication.RedirectFromLoginPage(UserName, false);
        }
        catch (Exception ex)
        {
            spnmsg.InnerText = ex.Message;
            divMessage.Visible = true;
            return;
        }
    }

}
