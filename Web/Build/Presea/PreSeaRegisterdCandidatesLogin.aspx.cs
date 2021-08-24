using System;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Data;

public partial class PreSeaRegisterdCandidatesLogin : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["ReturnUrl"] != null && !Request.QueryString["ReturnUrl"].ToString().Contains("OptionsForgotPassword"))
        {
            Response.Redirect("Default.aspx");
        }

        phoenixLogin.Focus();
    }

    protected void phoenixLogin_Authenticate(object sender, AuthenticateEventArgs ae)
    {
        try
        {
            if (General.GetNullableDateTime(phoenixLogin.Password.ToString()) == null)
            {
                ae.Authenticated = false;
                LoginFailiure();
                return;
            }
            DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantLogin(phoenixLogin.UserName, DateTime.Parse(phoenixLogin.Password));
            if (dt.Rows.Count > 0)
            {


                Filter.CurrentPreSeaNewApplicantSelection = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                Response.Redirect("~/Presea/PreSeaApplicationForm.aspx");
                //string Script = "";
                //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                //Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                //Script += "</script>" + "\n";
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            else
            {
                ae.Authenticated = false;
                LoginFailiure();
                return;
            }         
        }
        catch (Exception ex)
        {
            phoenixLogin.FailureText = ex.Message;
            ae.Authenticated = false;
            return;
        }
    }

    protected void lnkRegistration_Click(object sender, EventArgs e)
    {
        Filter.CurrentPreSeaNewApplicantSelection = null;
        Response.Redirect("../PreSea/PreSeaOnlineApplication.aspx");
    }
    private void LoginFailiure()
    {
        phoenixLogin.FailureText = "Invalid emailid or password (DOB)";        
    }
    protected void lnkEnquiry_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Presea/PreSeaEnquiryQueryAboutCourse.aspx");
    }
}
