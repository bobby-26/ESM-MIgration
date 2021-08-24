using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreDocumentCheckListMail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {

                if (Request.QueryString["Crewplanid"] != null)
                    ViewState["CREWPLANID"] = Request.QueryString["Crewplanid"].ToString();

              

                PrepareMail(ViewState["CREWPLANID"].ToString());

            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Send", "SEND",ToolBarDirection.Right);

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
    private void PrepareMail(string CrewplanId)
    {
        string emailbodytext = "";
        emailbodytext = PrepareEmailBodyText(CrewplanId);
        edtBody.Content = emailbodytext.ToString();
        txtSubject.Text = "Document Checklist";

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
        PhoenixCrewOffshoreCheckList.InsertMailsentDetail(General.GetNullableGuid(ViewState["CREWPLANID"].ToString()));
        //PhoenixCrewAppraisal.insertmailhistory(int.Parse(ViewState["EMPLOYEEID"].ToString()), new Guid(ViewState["APPRID"].ToString()), txtTo.Text, txtCc.Text);
    }
    protected string PrepareEmailBodyText(string CrewplanId)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataTable dt = PhoenixCrewOffshoreCheckList.EmployeeList(General.GetNullableGuid(CrewplanId));
        DataSet dsUser = PhoenixUser.UserEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        DataRow dr = dt.Rows[0];
        DataRow drUser = dsUser.Tables[0].Rows[0];

        DataSet ds = PhoenixCrewAppraisal.selectedcrewmailid(General.GetNullableInteger(dr["FLDEMPLOYEEID"].ToString()));
        ViewState["USERMAIL"] = drUser["FLDEMAIL"].ToString();
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtTo.Text = ds.Tables[0].Rows[0]["FLDEMAIL"].ToString().Trim();
            txtCc.Text = ViewState["USERMAIL"].ToString();
        }

        string username = drUser["FLDFIRSTNAME"].ToString() + " " + drUser["FLDMIDDLENAME"].ToString() + " " + drUser["FLDLASTNAME"].ToString();

        


        string employeename = dr["FLDFIRSTNAME"].ToString() + " " + dr["FLDMIDDLENAME"].ToString() + " " + dr["FLDLASTNAME"].ToString();

        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("Dear Mr." + employeename + ",");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("You are being assigned to join our vessel " + dr["FLDVESSELNAME"].ToString() + " and below URL will navigate to list of certificates / documents that are required to be carried in Original");
        sbemailbody.AppendLine("with you at the time of joining.");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Kindly on the link and confirm for each document by updating YES / NO in the ‘Holding Original’ Column.If No please give details of");
        sbemailbody.AppendLine("where the original document is. For example: In Labuan Office or with previous employer, etc.");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        string url = Session["sitepath"] + "/" + "Options/OptionsOffshoreCrewDocumentCheckList.aspx";
        sbemailbody.AppendLine("<a href =");
        sbemailbody.AppendLine("\"");
        sbemailbody.AppendLine(url + "?Crewplanid=" + CrewplanId);
        sbemailbody.AppendLine("\"");
        sbemailbody.AppendLine("target=");
        sbemailbody.AppendLine("\"");
        sbemailbody.AppendLine("_blank");
        sbemailbody.AppendLine("\"");

        sbemailbody.AppendLine(">");
        sbemailbody.AppendLine(url + "?Crewplanid=" + CrewplanId);
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("</a>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append("Would request you to kindly Update all required information and click Submit on that screen.");
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
