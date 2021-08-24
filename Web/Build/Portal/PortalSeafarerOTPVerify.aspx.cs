using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Portal_PortalSeafarerOTPVerify : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            ViewState["OTPID"] = "";
            ViewState["USERNAME"] = "";
            if (Request.QueryString["ID"] != null)
                ViewState["OTPID"] = Request.QueryString["ID"].ToString();
            if (Request.QueryString["UN"] != null)
                ViewState["USERNAME"] = Request.QueryString["UN"].ToString();
        }
    }
    protected void phoenixLogin_Authenticate(object sender, EventArgs ae)
    {
        string usertype = "";
        try
        {
            Filter.IsPortal = true;
            Filter.PortalAccessedBy = "SEAFARER";

            string otp = txtUserName.Value;
        

           if (!PhoenixUser.PortalLoginOTPVerifyList(General.GetNullableGuid(ViewState["OTPID"].ToString()), otp))
            {
                spnmsg.InnerText = "Incorrect OTP";
                divMessage.Visible = true;
                return;               
            }
            PhoenixUser.PortalLoginOTPVerify(General.GetNullableGuid(ViewState["OTPID"].ToString()), otp);
            PhoenixUser.SetUserContext(PhoenixSecurityContext.CurrentSecurityContext.UserCode, usertype);
            FormsAuthentication.RedirectFromLoginPage(ViewState["USERNAME"].ToString(), false);
        }
        catch (Exception ex)
        {
            spnmsg.InnerText = ex.Message;
            divMessage.Visible = true;
            return;
        }
    }
}