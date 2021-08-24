using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;


public partial class PortalDocuments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ParameterList.Add(DataAccess.GetDBParameter("@REMOTEADDRESS", SqlDbType.VarChar, 100, ParameterDirection.Input, Request.ServerVariables["REMOTE_ADDR"].ToString()));
        //dt = DataAccess.ExecSPReturnDataTable("PRPHOENIXPORTALIDENTITY", ParameterList);

    }

    protected void phoenixLogin_Authenticate(object sender, AuthenticateEventArgs ae)
    {
        try
        {
            string usertype = "";
            string useridentity = "";

            useridentity = (Request.QueryString["UID"] == null) ? Guid.NewGuid().ToString() : Request.QueryString["UID"].ToString();

            PhoenixUser.LoginAudit(phoenixLogin.UserName, phoenixLogin.Password, Request.ServerVariables["REMOTE_ADDR"].ToString(), ref usertype);

            if (phoenixLogin.UserName.ToUpper() != "SEAFARER")
            {
                ae.Authenticated = false;
                return;
            }
            if (!PhoenixUser.Login(phoenixLogin.UserName, phoenixLogin.Password))
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
