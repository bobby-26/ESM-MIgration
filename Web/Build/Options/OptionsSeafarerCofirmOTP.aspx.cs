using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Data;
using Telerik.Web.UI;
using System.Web;
using SouthNests.Phoenix.Portal;

public partial class OptionsSeafarerCofirmOTP : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;
            if (Request.QueryString["email"] != null)
                txtemail.Text = Request.QueryString["email"].ToString();


        }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
       DataTable dt=  PheonixSeafarerUserLogin.SeafarerOTPValidate(txtemail.Text,txtotp.Text);
        try
        {
            if (dt.Rows.Count > 0)
            {
                ucStatus.Text = "OTP verified";
                ucStatus.Visible = true;
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}