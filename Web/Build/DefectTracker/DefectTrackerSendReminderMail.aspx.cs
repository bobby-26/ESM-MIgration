using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerSendReminderMail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Send", "SEND");
        MenuNewMail.AccessRights = this.ViewState;
        MenuNewMail.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["ORIGINALBODY"] = "";
            if (Request.QueryString["dtkey"] != null)
                ViewState["DTKEY"] = Request.QueryString["dtkey"].ToString();

            ViewState["SESSIONID"] = Guid.NewGuid().ToString();
            ViewState["MESSAGEID"] = Guid.NewGuid().ToString();
            ViewState["MAILID"] = Guid.NewGuid().ToString();
            BindMail();
            BindAttachmentData();
        }
    }

    private void BindMail()
    {
        DataTable dt = PhoenixDefectTracker.GetMailID(new Guid(ViewState["DTKEY"].ToString()));

        if (dt.Rows.Count > 0)
        {
            string filename = dt.Rows[0]["FLDFILENAME"].ToString();
            string attachment = dt.Rows[0]["FLDFILEPATH"].ToString() + "\\" + filename;
            txtCc.Text = dt.Rows[0]["FLDSUPTEMAIL"].ToString() + ",projectlead@southnests.com";
            txtTo.Text = dt.Rows[0]["FLDEMAIL"].ToString();
            txtMessage.Text = dt.Rows[0]["FLDBODY"].ToString();
            filename = filename.Substring(0, filename.Length - ".zip".Length);
            txtSubject.Text = "Reminder for below patch : " + filename;
            PhoenixMailManager.MailAttachmentInsert(General.GetNullableGuid(ViewState["SESSIONID"].ToString()), ViewState["MAILID"].ToString(), attachment);
        }
    }

    protected void MenuNewMail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SEND"))
            {
                PhoenixMailManager.MailToSend(General.GetNullableGuid(ViewState["SESSIONID"].ToString()), "SEP", ViewState["MESSAGEID"].ToString(),
                                                txtTo.Text, txtCc.Text, txtSubject.Text, txtMessage.Text, "", "","",""
                                               );
              }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFileName = (Label)e.Row.FindControl("lblFileName");
            HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");
            lnk.NavigateUrl = "DefectTrackerMailAttachment.ashx?path=" + lblFileName.Text;
            ViewState["ATTACHMENT"] = lblFileName.Text;
        }
    }
    protected void gvAttachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    protected void gvAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixMailManager.MailOutDelete(General.GetNullableGuid(ViewState["SESSIONID"].ToString()));
                BindAttachmentData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindAttachmentData()
    {
        DataTable dt = new DataTable();
        dt = PhoenixMailManager.MailOutAttachments(General.GetNullableGuid(ViewState["SESSIONID"].ToString()));
        gvAttachment.DataSource = dt;
        gvAttachment.DataBind();
    }
}
