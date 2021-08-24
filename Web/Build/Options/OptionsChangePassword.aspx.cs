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
using Telerik.Web.UI;

public partial class OptionsChangePassword : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();        
        toolbar.AddButton("Change", "SAVE",ToolBarDirection.Right);
        //toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        txtUserName.Text = PhoenixSecurityContext.CurrentSecurityContext.UserName;
        txtUserName.Enabled = false;
        MenuSecurityChangePassword.MenuList = toolbar.Show();

    }

    protected void SecurityChangePassword_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixUser.ChangePassword(txtUserName.Text, txtCurrentPassword.Text, txtNewPassword.Text, txtConfirmNewPassword.Text);
                ucStatus.Text = "Password changed successfully";
                PhoenixSecurityContext.CurrentSecurityContext.PasswordChanged = 1;

                if (Request.QueryString["FROM"] != null && Request.QueryString["FROM"].ToString().ToUpper() == "DASHBOARD")
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:top.location.reload();", true);
            }
            if (CommandName.ToUpper().Equals("NEW"))
                Reset();
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    private void Reset()
    {
        txtCurrentPassword.Text = "";
        txtNewPassword.Text = "";
        txtConfirmNewPassword.Text = "";
    }

}
