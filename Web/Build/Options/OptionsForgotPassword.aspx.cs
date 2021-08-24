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
public partial class OptionsForgotPassword : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.Visible = false;


            DataSet ds = new DataSet();
            ds = PhoenixGeneralSettings.ConfigurationSettingEdit(1);
            if (General.GetNullableInteger(ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString()) != null && ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString() == "0")
                SecurityWizard.Visible = false;
            else
                pnlEmail.Visible = false;

            ViewState["LOGIN"] = "";

            if (Request.QueryString["loginfrom"] != null && Request.QueryString["loginfrom"].ToString() != "")
            {
                ViewState["LOGIN"] = Request.QueryString["loginfrom"].ToString();
            }

        }

    }


    protected void btnlogin_Click(object sender, EventArgs e)
    {
        if (ViewState["LOGIN"] != null && ViewState["LOGIN"].ToString() == "SEAFARER")
        {
            Response.Redirect("~/Portal/PortalSeafarerDefault.aspx");
        }
        if (ViewState["LOGIN"] != null && ViewState["LOGIN"].ToString() == "AGENT")
        {
            Response.Redirect("~/Portal/PortalDefault.aspx");
        }      
        else
        {
            Response.Redirect("~/Default.aspx");
        }
    }

    protected void cmdSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string link = Session["sitepath"] + "/" + "Portal/PortalSeafarerDefault.aspx";
            if (ViewState["LOGIN"].ToString() == "SEAFARER")
            {
                if (PhoenixUser.ForgotPasswordSeafarer(txtEMail.Text, link))
                {
                    lblMessage.Text = "Your password is sent to your E-Mail address.";
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessage.Text = "Your E-Mail address is not registered with this application.";
                    lblMessage.Visible = true;
                }
            }
            else
            {
                if (PhoenixUser.ForgotPassword(txtEMail.Text))
                {
                    lblMessage.Text = "Your password is sent to your E-Mail address.";
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessage.Text = "Your E-Mail address is not registered with this application.";
                    lblMessage.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Your E-Mail address is not registered with this application." + "<br/>" + ex.Message;
            lblMessage.Visible = true;
        }
    }

    protected void SecurityWizard_NextButtonClick(object sender, WizardEventArgs e)
    {
        if (e.CurrentStepIndex == 0)
        {
            Page.Validate("UserName");
            if (Page.IsValid)
            {
                DataTable dt = new DataTable();
                dt = PhoenixGeneralSettings.GetSecurityQuestionEdit(txtUserName.Text);
                if (dt.Rows.Count > 0)
                {
                    ucSecurityQuestion.SelectedValue = dt.Rows[0]["FLDSECURITYQUESTION"].ToString();
                    hdnAnswer.Text = dt.Rows[0]["FLDSECURITYANSWER"].ToString();
                }
                else
                {
                    RequiredFieldValidator Validator = new RequiredFieldValidator();
                    Validator.ErrorMessage = "* Invalid Username";
                    Validator.ValidationGroup = "UserName";
                    Validator.IsValid = false;
                    Validator.Visible = false;
                    Page.Form.Controls.Add(Validator);
                    e.CurrentStep.Active = true;
                }

            }
            else
            {
                e.CurrentStep.Active = true;
            }
        }
        else if (e.CurrentStepIndex == 1)
        {
            Page.Validate("Answer");

            if (Page.IsValid)
            {
                if (txtAnswer.Text.ToUpper() != hdnAnswer.Text.ToUpper())
                {
                    RequiredFieldValidator validator = new RequiredFieldValidator();
                    validator.ErrorMessage = "* Answer not matching";
                    validator.ValidationGroup = "Answer";
                    validator.IsValid = false;
                    validator.Visible = false;
                    Page.Form.Controls.Add(validator);
                    e.CurrentStep.Active = true;
                }

            }
            else
            {
                e.CurrentStep.Active = true;
            }

        }


    }

    protected void SecurityWizard_NavigationBarButtonClick(object sender, WizardEventArgs e)
    {
        if (e.CurrentStepIndex == 0)
        {
            Page.Validate("UserName");
            if (Page.IsValid)
            {
                DataTable dt = new DataTable();
                dt = PhoenixGeneralSettings.GetSecurityQuestionEdit(txtUserName.Text);
                if (dt.Rows.Count > 0)
                {
                    ucSecurityQuestion.SelectedValue = dt.Rows[0]["FLDSECURITYQUESTION"].ToString();
                    hdnAnswer.Text = dt.Rows[0]["FLDSECURITYANSWER"].ToString();
                }
                else
                {
                    RequiredFieldValidator Validator = new RequiredFieldValidator();
                    Validator.ErrorMessage = "* Invalid Username";
                    Validator.ValidationGroup = "UserName";
                    Validator.IsValid = false;
                    Validator.Visible = false;
                    Page.Form.Controls.Add(Validator);
                    e.CurrentStep.Active = true;
                }

            }
            else
            {
                e.CurrentStep.Active = true;
            }
        }
        else if (e.CurrentStepIndex == 1)
        {


            Page.Validate("Answer");

            if (Page.IsValid)
            {
                if (txtAnswer.Text.ToUpper() != hdnAnswer.Text.ToUpper())
                {
                    RequiredFieldValidator validator = new RequiredFieldValidator();
                    validator.ErrorMessage = "* Answer not matching";
                    validator.ValidationGroup = "Answer";
                    validator.IsValid = false;
                    validator.Visible = false;
                    Page.Form.Controls.Add(validator);
                    e.CurrentStep.Active = true;
                }

            }
            else
            {
                e.CurrentStep.Active = true;
            }

        }
    }

    protected void SecurityWizard_FinishButtonClick(object sender, WizardEventArgs e)
    {
        Page.Validate("Reset");
        if (Page.IsValid)
        {
            try
            {
                PhoenixUser.ResetPassword(txtUserName.Text, txtNewPassword.Text, txtConfirmPassword.Text);
            }
            catch (Exception ex)
            {
                RequiredFieldValidator Validator = new RequiredFieldValidator();
                Validator.ErrorMessage = "* " + ex.Message;
                Validator.ValidationGroup = "Reset";
                Validator.IsValid = false;
                Validator.Visible = false;
                Page.Form.Controls.Add(Validator);
                e.CurrentStep.Active = true;
            }
        }
        else
        {
            e.CurrentStep.Active = true;
        }
    }

}
