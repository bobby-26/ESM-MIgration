using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.Dashboard;

public partial class Accounts_AccountsPortageBillfollowupMail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["USERMAIL"] = "";
                ViewState["VESSELID"] = "";
                ViewState["Year"] = "";
                ViewState["Month"] = "";

                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

                if (Request.QueryString["Year"] != null)
                    ViewState["Year"] = Request.QueryString["Year"].ToString();

                if (Request.QueryString["Month"] != null)
                    ViewState["Month"] = Request.QueryString["Month"].ToString();
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Send", "SEND", ToolBarDirection.Right);
                MenuMailRead.AccessRights = this.ViewState;
                MenuMailRead.MenuList = toolbarmain.Show();

                PrepareMail(ViewState["VESSELID"].ToString());
                //  txtSubject.Text = "Finalizing of Month End Accounts" + General.GetDateTimeToString(DateTime.Parse(ViewState["DATE"].ToString())) + " at " + ViewState["PORT"].ToString();
                txtSubject.Text = "Finalizing of Month End Accounts";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }
    protected void MenuMailRead_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEND"))

            {
                SendMail();
                ucStatus.Text = "Email Sent";
                //string Script = "";
                //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                //Script += "fnReloadList('Accounts');";
                //Script += "</script>" + "\n";

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //   "BokMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                SendMailHistory();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    private void PrepareMail(string vesselid)
    {
        string emailbodytext = "";
        emailbodytext = PrepareEmailBodyText();
        edtBody.Content = emailbodytext.ToString();

        DataSet ds = PhoenixRegistersVesselCommunicationDetails.EditCommunicationDetails(int.Parse(vesselid));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtTo.Text = dr["FLDEMAIL"].ToString().Trim();
            txtCc.Text = ViewState["USERMAIL"].ToString();
        }
    }

    private void SendMailHistory()
    {

        PhoenixDashboardAccounts.InsertAccountsSendMailHistory(General.GetNullableInteger(ViewState["Month"].ToString())
                                                               ,General.GetNullableInteger(ViewState["Year"].ToString())
                                                               ,General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                               , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    }

    private void SendMail()
    {
        if (!IsValidEmail())
        {
            ucError.Visible = true;
            return;
        }
        else

            PhoenixMail.SendMail(txtTo.Text.Trim()
                , txtCc.Text.Trim()
                , null
                , txtSubject.Text.Trim()
                , edtBody.Content.ToString()
                , true
                , System.Net.Mail.MailPriority.Normal, "");
        ucStatus.Visible = true;
    }

    protected string PrepareEmailBodyText()
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet dsUser = PhoenixUser.UserEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        DataRow drUser = dsUser.Tables[0].Rows[0];
        string username = drUser["FLDFIRSTNAME"].ToString() + " " + drUser["FLDMIDDLENAME"].ToString() + " " + drUser["FLDLASTNAME"].ToString();

        ViewState["USERMAIL"] = drUser["FLDEMAIL"].ToString();
        sbemailbody.AppendLine("Dear Captain");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("The subject CTM Request should have been delivered. Please update the received amount in Phoenix..");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("If same is not received, please email" + ViewState["USERMAIL"]);
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Thanks and best regards,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine(username);
        sbemailbody.AppendLine("<br/>");
        return sbemailbody.ToString();
    }

    private bool IsValidEmail()
    {
        if (!General.IsvalidEmail(txtTo.Text.ToString().Trim()))
            ucError.ErrorMessage = "Valid To mailid required.";
        if (!General.IsvalidEmail(txtCc.Text.ToString().Trim()))
            ucError.ErrorMessage = "Valid To mailid required.";
        if (General.GetNullableString(edtBody.Content.ToString()) == null)
            ucError.ErrorMessage = "Mail content required.";

        return (!ucError.IsError);
    }
}
