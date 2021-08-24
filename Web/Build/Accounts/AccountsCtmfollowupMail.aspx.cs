using System;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class AccountsCtmfollowupMail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
          

            if (!IsPostBack)
            {
                ViewState["USERMAIL"] = "";
                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                if (Request.QueryString["port"] != null)
                    ViewState["PORT"] = Request.QueryString["port"].ToString();
                if (Request.QueryString["date"] != null)
                    ViewState["DATE"] = Request.QueryString["date"].ToString();
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Send", "SEND", ToolBarDirection.Right);
                MenuMailRead.AccessRights = this.ViewState;
                MenuMailRead.MenuList = toolbarmain.Show();

                PrepareMail( ViewState["VESSELID"].ToString());
                    txtSubject.Text = "To Confirm Safe Receipt of CTM Request Dated " + General.GetDateTimeToString(DateTime.Parse(ViewState["DATE"].ToString())) + " at " + ViewState["PORT"].ToString();
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
            //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            //if (dce.CommandName.ToUpper().Equals("SEND"))
            {
                SendMail();
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('Accounts');";
                Script += "</script>" + "\n";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                   "BokMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
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
        ucStatus.Text = "Mail Sent";

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
        sbemailbody.Append("If same is not received, please email"+ ViewState["USERMAIL"]);
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
        if(General.GetNullableString(edtBody.Content.ToString()) == null)
            ucError.ErrorMessage = "Mail content required.";

        return (!ucError.IsError);
    }
}
