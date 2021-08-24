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

public partial class OptionsChangeSecurityQuestion : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //PhoenixToolbar toolbar = new PhoenixToolbar();        
        //toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);

        if (!IsPostBack)
        {
            txtUserName.Text = PhoenixSecurityContext.CurrentSecurityContext.UserName;
            txtUserName.Enabled = false;
            //MenuSecurityChangePassword.MenuList = toolbar.Show();

            txtAnswer.Text = PhoenixGeneralSettings.CurrentGeneralSetting.SecurityAnswer;
            ucSecurityQuestion.SelectedValue = PhoenixGeneralSettings.CurrentGeneralSetting.SecurityQuestion.ToString();
        }
        
    }

    //protected void SecurityChangePassword_TabStripCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

    //        if (CommandName.ToUpper().Equals("SAVE"))
    //        {
                
    //            ucStatus.Text = "Password changed successfully";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.Text = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void SecurityWizard_NextButtonClick(object sender, WizardEventArgs e)
    {
        try
        {
            if (e.CurrentStepIndex == 0)
            {
                if (!ValidateUser())
                {
                    e.CurrentStep.Active = true;
                    ucError.ErrorMessage = "Invalid Username or Password";
                    ucError.Visible = true;
                }
            }
                
        }
        catch(Exception ex)
        {

            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }
    protected bool ValidateUser()
    {
        try
        {
            if (!PhoenixUser.Login(txtUserName.Text, txtCurrentPassword.Text))
            {
                return false;
            }
            else
                return true;
        }
        catch
        {
            return false;
        }
        
    }

    protected void SecurityWizard_NavigationBarButtonClick(object sender, WizardEventArgs e)
    {
        try
        {
            if (e.CurrentStepIndex == 0)
            {
                if (!ValidateUser())
                {
                    e.CurrentStep.Active = true;
                    ucError.ErrorMessage = "Invalid Username or Password";
                    ucError.Visible = true;
                }
            }

        }
        catch (Exception ex)
        {

            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SecurityWizard_FinishButtonClick(object sender, WizardEventArgs e)
    {
        try
        {
            if (General.GetNullableInteger(ucSecurityQuestion.SelectedValue) == null)
                ucError.ErrorMessage = "Security Question is required. <br/>";
            if (General.GetNullableString(txtAnswer.Text) == null)
                ucError.ErrorMessage = "Answer is required.";

            if (ucError.IsError)
            {
                ucError.Visible = true;
                e.CurrentStep.Active = true;
                return;
            }

            PhoenixGeneralSettings.UpdateSecurityQuestion(int.Parse(ucSecurityQuestion.SelectedValue), txtAnswer.Text);
            DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);

            if (Request.QueryString["FROM"] != null && Request.QueryString["FROM"].ToString().ToUpper() == "DASHBOARD")
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:top.location.reload();", true);
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            e.CurrentStep.Active = true;
        }
    }
}
