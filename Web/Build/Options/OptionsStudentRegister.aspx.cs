using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
public partial class Options_OptionsStudentRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Login", "LOGIN");
        toolbar.AddButton("Register", "SAVE");
        MenuSecurityChangePassword.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            lblManagement.Text = HttpContext.Current.Session["companyname"].ToString();
        }
    }
    protected void SecurityChangePassword_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDetails(txtUserName.Text, txtNewPassword.Text, txtConfirmNewPassword.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (!IsValidPassword(txtNewPassword.Text, txtConfirmNewPassword.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixStudentUser.StudentRegister(txtUserName.Text, txtNewPassword.Text, txtConfirmNewPassword.Text);
                ucStatus.Text = "Registered successfully";

                //Response.Redirect("~/Portal/PortalStudentDefault.aspx");
            }
            if (dce.CommandName.ToUpper().Equals("LOGIN"))
            {
                Response.Redirect("~/Portal/PortalStudentDefault.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidDetails(string rollnumber, string password, string confirmpassword)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(rollnumber))
            ucError.ErrorMessage = "Roll Number is required";

        if (string.IsNullOrEmpty(password))
            ucError.ErrorMessage = "Password is required";

        if (string.IsNullOrEmpty(confirmpassword))
            ucError.ErrorMessage = "Confirm Passowrd is required";

        return (!ucError.IsError);
    }

    public bool IsValidPassword(string password, string newpassword)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (password != newpassword)
        {
            ucError.ErrorMessage = "Password and Confirm Password do not match";
        }
        return (!ucError.IsError);

    }
}
