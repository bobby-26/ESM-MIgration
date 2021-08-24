using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreDebriefingEmail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Send", "SEND",ToolBarDirection.Right);

            MenuMailRead.AccessRights = this.ViewState;
            MenuMailRead.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                if (Request.QueryString["Signonoffid"] != null)
                    ViewState["FLDSIGNONOFFID"] = Request.QueryString["Signonoffid"].ToString();
                


                PrepareMail(ViewState["FLDSIGNONOFFID"].ToString());
                txtSubject.Text = "De - Briefing Request";
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
    private void PrepareMail(string Signonoffid)
    {
        string emailbodytext = "";
        string emailid = "";
        string ccemailid = "";
        emailbodytext = PrepareEmailBodyText(Signonoffid,ref emailid,ref ccemailid);
        edtBody.Content = emailbodytext.ToString();

        if (emailid!=null|| emailid!= "")
        {
            txtTo.Text = emailid.Trim();
            txtCc.Text = ccemailid.Trim();
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
        PhoenixCrewDeBriefing.EmailCountUpdate(General.GetNullableInteger(ViewState["FLDSIGNONOFFID"].ToString()));
    }
    protected string PrepareEmailBodyText(string signonoffid,ref string employeeemailid,ref string ccemailid)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataTable dt = PhoenixCrewDeBriefing.EmployeeList(General.GetNullableInteger(signonoffid));
        DataSet dsUser = PhoenixUser.UserEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        //DataSet ds = PhoenixCrewAppraisal.GetApraisalDataForEmailBody(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["APPRID"].ToString()), "CFC");
        //DataRow dr1 = ds.Tables[0].Rows[0];

        DataRow drUser = dsUser.Tables[0].Rows[0];

        string username = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
        employeeemailid = dt.Rows[0]["FLDEMAIL"].ToString();
        ccemailid = drUser["FLDEMAIL"].ToString();

            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Dear Mr." + dt.Rows[0]["FLDNAME"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("We trust you had a safe trip home and look forward to having you back onboard our vessels in the near future.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("Kindly click the below link to submit the following:");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("1) De-Briefing report for your last tenure on board.");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("2) Your date of availability for joining, so that we can plan your next tour of duty.");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("3) Due to operational/ technical constraints you may have missed out to enter your comments on your appraisal report, hence request for your comments so that the appraisal can be updated with those comments.");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("If the link is wrapped, please copy and paste it on the address bar of your browser");
            sbemailbody.AppendLine();
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");

            string url = Session["sitepath"] + "/Options/OptionsOffshoreCrewDeBriefing.aspx";
            sbemailbody.AppendLine("<a href =");
            sbemailbody.AppendLine("\"");
            sbemailbody.AppendLine(url + "?signonoffid=" + signonoffid);
            sbemailbody.AppendLine("\"");
            sbemailbody.AppendLine("target=");
            sbemailbody.AppendLine("\"");
            sbemailbody.AppendLine("_blank");
            sbemailbody.AppendLine("\"");

            sbemailbody.AppendLine(">");
            sbemailbody.Append(url + "?signonoffid=" + signonoffid);
            sbemailbody.AppendLine("</a>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("Note :");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("This is an automated message.");
            sbemailbody.AppendLine("<br/>");
            
            sbemailbody.AppendLine("Would request you to kindly go through and fill, then click Submit on that screen.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Thanks and best regards,");
            sbemailbody.AppendLine("<br/>");
            //sbemailbody.AppendLine(username);
            //sbemailbody.AppendLine("<br/>");
        
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
