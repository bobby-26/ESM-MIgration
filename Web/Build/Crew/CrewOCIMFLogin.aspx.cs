using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Net;
using System.Security.Authentication;
using Telerik.Web.UI;
public partial class CrewOCIMFLogin : PhoenixBasePage
{
    public const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
    public const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        ServicePointManager.SecurityProtocol = Tls12;
        if (Request.QueryString["vslid"] != null && Request.QueryString["vslid"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(Request.QueryString["vslid"].ToString()));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FLDIMONUMBER"].ToString() == string.Empty)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Kindly update the IMO number in vessel master";
                    ucError.Visible = true;
                }
            }
        }
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        try
        {
            string token = "";
            string uniqueid = "";

            if (!IsValidPasswrd(txtAccountId.Text, txtUserName.Text, txtPassword.Text))
            {
                ucError.Visible = true;
                return;
            }

            DataTable dt = PhoenixCrewOCIMF.OCIMFAccountFetch(txtAccountId.Text, txtUserName.Text, txtPassword.Text);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["FLDTHUMBPRINTCODE"].ToString() == string.Empty)  // secret code based authentication or session based authentication
                {
                    token = PhoenixCrewSireServiceOperations.StartWebSession(txtAccountId.Text, txtUserName.Text, txtPassword.Text);
                }
                else
                {
                    uniqueid = dt.Rows[0]["FLDTHUMBPRINTCODE"].ToString();
                }

                Response.Redirect("CrewOcimf.aspx?vslid=" + Request.QueryString["vslid"] + "&tokenid=" + token + "&uniqueid=" + uniqueid, true);
                
            }
            else
            {
                ucError.ErrorMessage = "Login Failed, Please check the credentials";
                ucError.Visible = true;

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidPasswrd(string accountid, string username, string password)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (accountid.Trim().Equals(""))
            ucError.ErrorMessage = "Account Id is required.";

        if (username.Trim().Equals(""))
            ucError.ErrorMessage = "Username is required.";

        if (password.Trim().Equals(""))
            ucError.ErrorMessage = "Password is required.";

        return (!ucError.IsError);
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
