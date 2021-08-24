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
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreAppraisalMail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                if (Request.QueryString["employeeid"] != null)
                    ViewState["EMPLOYEEID"] = Request.QueryString["employeeid"].ToString();
                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
                if (Request.QueryString["apprid"] != null)
                    ViewState["APPRID"] = Request.QueryString["apprid"].ToString();
                if (Request.QueryString["rankid"] != null)
                    ViewState["RANKID"] = Request.QueryString["rankid"].ToString();



                PrepareMail(ViewState["EMPLOYEEID"].ToString(), ViewState["VESSELID"].ToString());
                txtSubject.Text = "To fill seafarer comments for appraisal.";
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Send", "SEND", ToolBarDirection.Right);

            MenuMailRead.AccessRights = this.ViewState;
            MenuMailRead.MenuList = toolbarmain.Show();
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
        emailbodytext = PrepareEmailBodyText(employeeid);
        edtBody.Content = emailbodytext.ToString();

        DataSet ds = PhoenixCrewAppraisal.selectedcrewmailid(General.GetNullableInteger(employeeid));

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
        PhoenixCrewAppraisal.insertmailhistory(int.Parse(ViewState["EMPLOYEEID"].ToString()), new Guid(ViewState["APPRID"].ToString()), txtTo.Text, txtCc.Text);
    }
    protected string PrepareEmailBodyText(string employeeid)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()));
        DataSet dsUser = PhoenixUser.UserEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        DataSet ds = PhoenixCrewAppraisal.GetApraisalDataForEmailBody(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["APPRID"].ToString()), "CFC");
        DataRow dr1 = ds.Tables[0].Rows[0];

        DataRow drUser = dsUser.Tables[0].Rows[0];

        string username = drUser["FLDFIRSTNAME"].ToString() + " " + drUser["FLDMIDDLENAME"].ToString() + " " + drUser["FLDLASTNAME"].ToString();

        ViewState["USERMAIL"] = drUser["FLDEMAIL"].ToString();

        DataRow dr = dt.Rows[0];
        string employeename = dr["FLDFIRSTNAME"].ToString() + " " + dr["FLDMIDDLENAME"].ToString() + " " + dr["FLDLASTNAME"].ToString();

        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("Good Day,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Please note,the appraisal for " + dr["FLDRANKPOSTEDNAME"].ToString() + " - " + employeename + " has been completed .");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("Please click on the link below and key in the relevant fields indicated.");
        sbemailbody.AppendLine("If the link is wrapped, please copy and paste it on the address bar of your browser");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");

        string url = Session["sitepath"] + "/" + dr1["URL"].ToString();
        sbemailbody.AppendLine("<a href =");
        sbemailbody.AppendLine("\"");
        sbemailbody.AppendLine(url + "?aprid=" + ViewState["APPRID"].ToString());
        sbemailbody.AppendLine("\"");
        sbemailbody.AppendLine("target=");
        sbemailbody.AppendLine("\"");
        sbemailbody.AppendLine("_blank");
        sbemailbody.AppendLine("\"");

        sbemailbody.AppendLine(">");
        sbemailbody.Append(url + "?aprid=" + ViewState["APPRID"].ToString());
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("</a>");
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
