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
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewUserResetPassword : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE");
        MenuSecurityResetPassword.AccessRights = this.ViewState;
        MenuSecurityResetPassword.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["usercode"] != null)
                ViewState["USERCODE"] = Request.QueryString["usercode"].ToString();
            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
            if (Request.QueryString["username"] != null)
                txtUserName.Text = Request.QueryString["username"].ToString();
        }

    }

    protected void SecurityResetPassword_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPasswordReset(txtNewPassword.Text, txtConfirmNewPassword.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewUserMapping.CrewUserResetPassword(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , int.Parse(ViewState["USERCODE"].ToString())
                                                            , txtNewPassword.Text
                                                            , txtConfirmNewPassword.Text
                                                            , int.Parse(ViewState["VESSELID"].ToString()));
                ucStatus.Text = "Password reset";
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidPasswordReset(string newpwd, string confirmpwd)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(newpwd) == null)
            ucError.ErrorMessage = "New Password is required.";

        if (General.GetNullableString(confirmpwd) == null)
            ucError.ErrorMessage = "Confirm Password is required.";

        if (General.GetNullableString(confirmpwd) != General.GetNullableString(newpwd))
            ucError.ErrorMessage = "The password and confirm password not match.";

        return (!ucError.IsError);
    }

}
