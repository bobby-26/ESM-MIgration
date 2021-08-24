using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTracker_DefectTrackerMailRead : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Reply", "REPLY");
        toolbar.AddButton("Log Defect", "DEFECT");
        MenuMailRead.AccessRights = this.ViewState;
        MenuMailRead.MenuList = toolbar.Show();

        Guid mailid = new Guid(Request.QueryString["mailid"].ToString());
        if (!IsPostBack)
        {
            if (Request.QueryString["sentitem"] != null)
                ShowSentMail(mailid);
            else
                ShowMail(mailid);
        }
    }

    protected void MenuMailRead_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("REPLY"))
            {
                string modulelist = ucSEPModule.SelectedValue;

                if (Request.QueryString["sentitem"] != null)
                    Response.Redirect("DefectTrackerMailReply.aspx?sentitem=1&mailid=" + ViewState["MAILID"].ToString() + "&modulelist=" + modulelist + "&phoenixit=" + ViewState["ESMIT"].ToString(), false);
                else
                    Response.Redirect("DefectTrackerMailReply.aspx?mailid=" + ViewState["MAILID"].ToString() + "&modulelist=" + modulelist + "&phoenixit=" + ViewState["ESMIT"].ToString(), false);
            }

            if (dce.CommandName.ToUpper().Equals("DEFECT"))
            {
                Guid dtkey = Guid.NewGuid();

                PhoenixDefectTracker.BugSave(-1
                , 1112   // Module - General
                , 15
                , txtSubject.Text
                , txtMessage.Text
                , "24" // Bug Status - New
                , "92" // Help Desk
                , "16" // Not Critical
                , "20", PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , "1.0", "Phoenix", null, null, null, "sep@southnests.com", null
                , PhoenixSecurityContext.CurrentSecurityContext.UserName
                , null, 24, ref dtkey);


                PhoenixMailManager.MailAttachments2Bug(
                    General.GetNullableGuid(ViewState["MAILID"].ToString()),
                    dtkey);

                Response.Redirect("DefectTrackerBugEdit.aspx?norefresh=yes&dtkey=" + dtkey.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void ShowMail(Guid mailid)
    {
        DataTable dt = PhoenixMailManager.MailToRead(mailid);
        DataRow dr = dt.Rows[0];

        txtFrom.Text = dr["FLDFROM"].ToString();
        txtCc.Text = dr["FLDCC"].ToString();
        txtTo.Text = dr["FLDTO"].ToString();
        txtMessage.Text = dr["FLDBODY"].ToString();
        txtSubject.Text = dr["FLDSUBJECT"].ToString();
        ViewState["MAILID"] = dr["FLDMAILID"].ToString();
        ucSEPModule.SelectedValue = dr["FLDMODULELIST"].ToString();
        ViewState["ESMIT"] = General.GetNullableInteger(dr["FLDPHOENIXIT"].ToString());

        string selectedmodules = dr["FLDMODULELIST"].ToString();

        if (ViewState["ESMIT"] == null)
            ViewState["ESMIT"] = 0;

        if (selectedmodules.Contains(",1128") == true)
        {         
                lblesmit.Visible = true;
                chkESMIT.Visible = true;
                if (ViewState["ESMIT"].ToString() == "1")
                    chkESMIT.Checked = true;         
        }
        else
        {
            lblesmit.Visible = false;
            chkESMIT.Visible = false;
            chkESMIT.Checked = false;
        }

        BindAttachmentData();
    }

    private void ShowSentMail(Guid mailid)
    {
        DataTable dt = PhoenixMailManager.MailSentToRead(mailid);
        DataRow dr = dt.Rows[0];

        txtFrom.Text = dr["FLDFROM"].ToString();
        txtCc.Text = dr["FLDCC"].ToString();
        txtTo.Text = dr["FLDTO"].ToString();
        txtMessage.Text = dr["FLDBODY"].ToString();
        txtSubject.Text = dr["FLDSUBJECT"].ToString();
        ViewState["MAILID"] = dr["FLDMAILOUTID"].ToString();
        ucSEPModule.SelectedValue = dr["FLDMODULELIST"].ToString();
        string attachments = dr["FLDATTACHMENTS"].ToString();
        ViewState["ESMIT"] = General.GetNullableInteger(dr["FLDPHOENIXIT"].ToString());
        BindAttachmentData();
        
        if (ViewState["ESMIT"] == null)
            ViewState["ESMIT"] = 0;

        string selectedmodules = dr["FLDMODULELIST"].ToString();
        if (selectedmodules.Contains(",1128") == true)
        {
            lblesmit.Visible = true;
            chkESMIT.Visible = true;
            if (ViewState["ESMIT"].ToString() == "1")               
                chkESMIT.Checked = true;
        }
        else
        {
            lblesmit.Visible = false;
            chkESMIT.Visible = false;
            chkESMIT.Checked = false;
        }
    }


    protected void ucSEPModule_TextChanged(object sender, EventArgs e)
    {
        string selectedmodules = ucSEPModule.SelectedValue;

        if (selectedmodules.Contains(",1128") == true)
        {
            lblesmit.Visible = true;
            chkESMIT.Visible = true;
            if (chkESMIT.Checked == true)
                ViewState["ESMIT"] = "1";
            else
                ViewState["ESMIT"] = "0";
        }
        else
        {
            lblesmit.Visible = false ;
            chkESMIT.Visible = false ;
            chkESMIT.Checked = false ;
            ViewState["ESMIT"] = "0";
        }

        if (Request.QueryString["sentitem"] != null)
            PhoenixMailManager.MailSentModuleUpdate(General.GetNullableGuid(ViewState["MAILID"].ToString()), selectedmodules);
        else
            PhoenixMailManager.MailModuleUpdate(General.GetNullableGuid(ViewState["MAILID"].ToString()), selectedmodules,General.GetNullableInteger(ViewState["ESMIT"].ToString()));

        BindAttachmentData();
    }

    private void BindAttachmentData()
    {
        DataTable dt = new DataTable();

        if (Request.QueryString["sentitem"] == null)
            dt = PhoenixMailManager.MailToReadAttachments(General.GetNullableGuid(ViewState["MAILID"].ToString()));
        else
            dt = PhoenixMailManager.MailToSendAttachments(General.GetNullableGuid(ViewState["MAILID"].ToString()));

        gvAttachment.DataSource = dt;
        gvAttachment.DataBind();
    }

    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFileName = (Label)e.Row.FindControl("lblFileName");
            HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");
            lnk.NavigateUrl = "DefectTrackerMailAttachment.ashx?path=" + HttpUtility.UrlEncode(lblFileName.Text);
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
                Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey");
                PhoenixMailManager.MailOutDelete(General.GetNullableGuid(lbl.Text));
                BindAttachmentData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
