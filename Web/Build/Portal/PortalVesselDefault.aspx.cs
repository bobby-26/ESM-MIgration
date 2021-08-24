using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class PortalVesselDefault : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {                       
       
    }

    protected void phoenixLogin_Authenticate(object sender, AuthenticateEventArgs ae)
    {
        string usertype = "";
        try
        {
            Filter.IsPortal = true;
            Filter.PortalAccessedBy = "VESSEL";
            PhoenixUser.VesselLoginAudit(phoenixLogin.UserName, phoenixLogin.Password, Request.ServerVariables["REMOTE_ADDR"].ToString(), ref usertype);

            if (!PhoenixUser.PortalVesselLogin(phoenixLogin.UserName, phoenixLogin.Password))
            {
                ae.Authenticated = false;
                return;
            }

            PhoenixUser.SetUserContext(PhoenixSecurityContext.CurrentSecurityContext.UserCode, usertype);
            FormsAuthentication.RedirectFromLoginPage(phoenixLogin.UserName, false);
        }
        catch (Exception ex)
        {
            phoenixLogin.FailureText = ex.Message;
            ae.Authenticated = false;
            return;
        }
    }

}
