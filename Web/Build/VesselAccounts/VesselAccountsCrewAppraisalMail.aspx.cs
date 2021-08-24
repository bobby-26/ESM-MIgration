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
using Telerik.Web.UI;

public partial class VesselAccountsCrewAppraisalMail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbarmain.AddButton("Send", "SEND",ToolBarDirection.Right);
           
            MenuMailRead.AccessRights = this.ViewState;
            MenuMailRead.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["MailType"] = string.IsNullOrEmpty(Request.QueryString["MailType"]) ? "" : Request.QueryString["MailType"];
                ViewState["EMPLOYEEID"] = "";
                ViewState["USERMAIL"] = "";

                if (Request.QueryString["employeeid"] != null)
                    ViewState["EMPLOYEEID"] = Request.QueryString["employeeid"].ToString();
                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                
                PrepareMail(ViewState["EMPLOYEEID"].ToString(), ViewState["VESSELID"].ToString());
                if ((string.IsNullOrEmpty(ViewState["MailType"].ToString()) ? "" : ViewState["MailType"].ToString()).Trim().ToUpper().Equals("FEEDBACK"))
                    txtSubject.Text = "To fill Sign Off FeedBack.";
                else
                    txtSubject.Text = "To fill seafarer comments for appraisal.";
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
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    private void PrepareMail(string employeeid, string vesselid)
    {
        string emailbodytext = "";
        emailbodytext = PrepareEmailBodyText(employeeid, string.IsNullOrEmpty(ViewState["MailType"].ToString()) ? "" : ViewState["MailType"].ToString());
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

    protected string PrepareEmailBodyText(string employeeid, string MailType)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()));
        DataSet dsUser = PhoenixUser.UserEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        DataRow drUser = dsUser.Tables[0].Rows[0];
        string username = drUser["FLDFIRSTNAME"].ToString() + " " + drUser["FLDMIDDLENAME"].ToString() + " " + drUser["FLDLASTNAME"].ToString();

        ViewState["USERMAIL"] = drUser["FLDEMAIL"].ToString();

        DataRow dr = dt.Rows[0];
        string employeename = dr["FLDFIRSTNAME"].ToString() + " " + dr["FLDMIDDLENAME"].ToString() + " " + dr["FLDLASTNAME"].ToString();
        if (MailType.ToUpper().Equals("FEEDBACK"))
        {
            sbemailbody.AppendLine("Dear " + employeename + "");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("Greetings!");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("We trust you had a good and safe trip on our vessel and the journey back home was comfortable.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("We appreciate your kind assistance and wish to place on records the sincere gratitude for the professionalism,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("understanding and support displayed by your good self throughout your last tenure.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("We would take this opportunity to request you for any honest feedback on your experiences onboard and with the");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("organization to ensure your tenures with us in future are pleasurable and satisfying.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Thanks and best regards,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(username);
            sbemailbody.AppendLine("<br/>");
        }
        else
        {
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("Good Day,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Please note,the appraisal for " + dr["FLDRANKPOSTEDNAME"].ToString() + " - " + employeename + " has been completed .");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("Would request you to kindly go through and fill in the seafarer comments and click save on that screen.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Thanks and best regards,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(username);
            sbemailbody.AppendLine("<br/>");
        }
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
