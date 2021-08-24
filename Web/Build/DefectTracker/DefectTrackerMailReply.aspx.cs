using System;
using System.Collections.Specialized;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.DefectTracker;
using System.IO;
using System.Web.UI;

public partial class DefectTracker_DefectTrackerMailReply : PhoenixBasePage
{
    Guid? incidentid;

    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvSEPIncidentList.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvSEPIncidentList.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Send", "SEND");
        MenuMailReply.AccessRights = this.ViewState;
        MenuMailReply.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            // Used for Attachment addition to message.

            if (Request.QueryString["sessionid"] == null)
                ViewState["SESSIONID"] = Guid.NewGuid().ToString();
            else
                ViewState["SESSIONID"] = Request.QueryString["sessionid"].ToString();

            ViewState["MODULELIST"] = "";
            if (Request.QueryString["modulelist"] != null)
                ViewState["MODULELIST"] = Request.QueryString["modulelist"].ToString();

            ViewState["PHOENIXIT"] = "";
            
            if (Request.QueryString["phoenixit"] != null)
                ViewState["PHOENIXIT"] = Request.QueryString["phoenixit"].ToString();
            else
                ViewState["PHOENIXIT"] = "";

            Guid mailid = new Guid(Request.QueryString["mailid"].ToString());

            if (Request.QueryString["sentitem"] != null)
                ShowSentMail(mailid);
            else
                ShowMail(mailid);
        }
        bind();
        BindAttachmentData();
    }

    private void bind()
    {

        if (ViewState["MODULELIST"] == null)
        {
            ViewState["MODULELIST"] = ",1128,";
        }

        DataTable dt = PhoenixDefectTracker.IncidentList(General.GetNullableString(ViewState["MODULELIST"].ToString()), null, null);

        if (dt.Rows.Count > 0)
        {
            gvSEPIncidentList.DataSource = dt;
            gvSEPIncidentList.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvSEPIncidentList);
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO INCIDENT RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    protected void ddl_TextChanged(object sender, EventArgs e)
    {
        string s = ucSearchPatch.SelectedValue.ToString();
        DataTable dt = PhoenixDefectTracker.SelectPatch(new Guid(s));
        DataRow dr = dt.Rows[0];

        string filename = dr["FLDFILENAME"].ToString();
        string createdBy = dr["FLDPATCHCREATEDBY"].ToString();
        filename = filename.Substring(0, filename.Length - ".zip".Length);
        string mailbody = "";

        mailbody = mailbody + "\nGood Day Captain,\n\n Kindly do the following steps to apply the patch,\n\n In the PC where Phoenix is installed.\n\n ";
        mailbody += "\tFor Windows-XP  machine,\n\n";
        mailbody += "\ta.\t Copy the attached zip file \"" + filename + ".zip\" to C:\\ folder.\n";
        mailbody += "\tb.\t Right click on the zip file and click on Extract to \"" + filename + "\\\" \n";
        mailbody += "\tc.\t Go to folder C:\\" + filename + "\n";
        mailbody += "\td.\t Double click on \"phnxPatch.bat\". \n";
        mailbody += "\te.\t Right on the file " + filename + ".log\" and select Send to -> Compressed(Zip) folder. Now Send the zip to sep@southnests.com \n\n";

        mailbody += "\tFor Windows-7  machine,\n\n";
        mailbody += "\ta.\t Copy the attached zip file \"" + filename + "\" to C:\\ folder.\n";
        mailbody += "\tb.\t Right click on the zip file and click on Extract to \"" + filename + "\\\" \n";
        mailbody += "\tc.\t Go to folder C:\\" + filename + "\n";
        mailbody += "\td.\t Right click on \"phnxPatch.bat\" and Select Run as Administrator. If prompt for confirmation, Click yes \n";
        mailbody += "\te.\t Right on the file " + filename + ".log\" and select Send to -> Compressed(Zip) folder. Now Send the zip to sep@southnests.com \n\n";
        mailbody += "Note :   If you are using Windows-7 , do not double click on \"phnxPatch.bat\". In order to run batch file you must run as Administrator.\n\n";
        mailbody += "Best Regards \n";
        mailbody += createdBy + "(Phoenix Support)\n";
        mailbody += "--------------------------------\n";
        txtMessage.Text = mailbody + ViewState["ORIGINALBODY"].ToString();
        PhoenixMailManager.MailAttachmentInsert(General.GetNullableGuid(ViewState["SESSIONID"].ToString()), ViewState["MAILID"].ToString(), dr["FLDFILEPATH"].ToString() + dr["FLDFILENAME"]);
        BindAttachmentData();
    }

    private void ShowMail(Guid mailid)
    {
        DataTable dt = PhoenixMailManager.MailToReply(mailid);
        DataRow dr = dt.Rows[0];

        txtFrom.Text = dr["FLDMAILUSERNAME"].ToString();
        if (dr["FLDFROM"].ToString().ToUpper().Contains("SEP"))
            txtTo.Text = dr["FLDTO"].ToString();
        else
            txtTo.Text = dr["FLDFROM"].ToString() + "," + dr["FLDTO"].ToString();

        txtCc.Text = dr["FLDCC"].ToString();
        txtSubject.Text = "RE: " + dr["FLDSUBJECT"].ToString();

        string mailheader = "\n\n------------------------------- \n";
        mailheader = mailheader + "Date: " + dr["FLDRECEIVEDON"].ToString() + "\n";
        mailheader = mailheader + "From: " + txtFrom.Text + "\n";
        mailheader = mailheader + "To: " + txtTo.Text + "\n";
        mailheader = mailheader + "Cc: " + txtCc.Text + "\n";
        mailheader = mailheader + "Subject: " + dr["FLDSUBJECT"].ToString() + "\n\n";

        txtMessage.Text = mailheader + dr["FLDBODY"].ToString();

        ViewState["ORIGINALBODY"] = dr["FLDBODY"].ToString();
        ViewState["MESSAGEID"] = dr["FLDMESSAGEID"].ToString();
        ViewState["MAILID"] = dr["FLDMAILID"].ToString();
        ViewState["CALLNUMBER"] = dr["FLDCALLNUMBER"].ToString();        
    }

    private void ShowSentMail(Guid mailid)
    {
        DataTable dt = PhoenixMailManager.MailSentToReply(mailid);
        DataRow dr = dt.Rows[0];

        txtFrom.Text = dr["FLDMAILUSERNAME"].ToString();

        if (dr["FLDFROM"].ToString().ToUpper().Contains("SEP"))
            txtTo.Text = dr["FLDTO"].ToString();
        else
        {
            txtTo.Text = dr["FLDFROM"].ToString() + "," + dr["FLDTO"].ToString();
            txtTo.Text = txtTo.Text.TrimStart(',', ' ');
        }

        txtCc.Text = dr["FLDCC"].ToString();
        
        string mailheader = "\n\n------------------------------- \n";
        mailheader = mailheader + "Date: " + dr["FLDSENTON"].ToString() + "\n";
        mailheader = mailheader + "From: " + txtFrom.Text + "\n";
        mailheader = mailheader + "To: " + txtTo.Text + "\n";
        mailheader = mailheader + "Cc: " + txtCc.Text + "\n";
        mailheader = mailheader + "Subject: " + dr["FLDSUBJECT"].ToString() + "\n\n";

        txtMessage.Text = mailheader + dr["FLDBODY"].ToString();

        ViewState["ORIGINALBODY"] = dr["FLDBODY"].ToString();
        txtSubject.Text = "RE: " + dr["FLDSUBJECT"].ToString();
        ViewState["MESSAGEID"] = dr["FLDMESSAGEID"].ToString();
        ViewState["MAILID"] = dr["FLDMAILOUTID"].ToString();
        ViewState["CALLNUMBER"] = dr["FLDCALLNUMBER"].ToString();
        
    }
    protected void cmdUpload_Click(object sender, EventArgs e)
    {
        HttpFileCollection postedFiles = Request.Files;

        if (Request.Files["txtBugAttachment"].ContentLength > 0)
        {
            string path = HttpContext.Current.Request.MapPath("~/");
            path = path + "Attachments\\SEP\\" + ViewState["MAILID"].ToString();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            for (int i = 0; i < postedFiles.Count; i++)
            {
                HttpPostedFile postedFile = postedFiles[i];
                if (postedFile.ContentLength > 0)
                {
                    string filepath = path;
                    postedFile.SaveAs(path + "\\" + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1));
                    string filename = path + "\\" + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1);
                    PhoenixMailManager.MailAttachmentInsert(General.GetNullableGuid(ViewState["SESSIONID"].ToString()), ViewState["MAILID"].ToString(), filename);
                }
            }
        }
        BindAttachmentData();
    }
    protected void MenuMailReply_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SEND"))
            {
                PhoenixMailManager.MailToSend(General.GetNullableGuid(ViewState["SESSIONID"].ToString()), txtFrom.Text,
                    ViewState["MESSAGEID"].ToString(), txtTo.Text, txtCc.Text,
                    txtSubject.Text, txtMessage.Text, "", ViewState["CALLNUMBER"].ToString(), ViewState["MODULELIST"].ToString(), ViewState["PHOENIXIT"].ToString());
                ucStatus.Text = "MAIL HAS BEEN SENT";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvSEPIncidentList_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            Label incident = (Label)e.Row.FindControl("lblincidentid");
            incidentid = General.GetNullableGuid(((Label)e.Row.FindControl("lblincidentid")).Text);

            e.Row.ToolTip = (e.Row.DataItem as DataRowView)["FLDBODY"].ToString();
        }
    }

    protected void Incident_Changed(object sender, EventArgs e)
    {
        foreach (GridViewRow r in gvSEPIncidentList.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                RadioButton rdbt = (RadioButton)r.FindControl("rbsepincident");
                if (rdbt.Checked)
                {
                    Label incident = (Label)r.FindControl("lblincidentid");
                    incidentid = General.GetNullableGuid(((Label)r.FindControl("lblincidentid")).Text);
                    if (ViewState["Oldincidentid"] == null)
                    {
                        ViewState["Oldincidentid"] = incidentid;
                        Guid? dtkey = General.GetNullableGuid(incidentid.ToString());

                        if (dtkey == null)
                        {
                            txtMessage.Text = ViewState["ORIGINALBODY"].ToString();
                            return;
                        }

                        DataTable dt = PhoenixMailManager.MailIncidentEdit(txtFrom.Text, dtkey);
                        if (dt.Rows.Count > 0)
                            txtMessage.Text = dt.Rows[0]["FLDBODY"].ToString().Trim() + "\n\n" + PhoenixSecurityContext.CurrentSecurityContext.FirstName + "(Phoenix Support)" + "\n\n\n\n ---------------  \n\n\n" + ViewState["ORIGINALBODY"].ToString();

                        if ((dt.Rows[0]["FLDFILENAMES"].ToString() != null) && (dt.Rows[0]["FLDFILENAMES"].ToString() != ""))
                        {
                            int n = PhoenixMailManager.MailOutIncidentAttachment(General.GetNullableGuid(ViewState["SESSIONID"].ToString()), ViewState["MESSAGEID"].ToString(), txtFrom.Text, dtkey);
                            BindAttachmentData();
                        }
                        else
                            gvAttachment.Visible = false;
                    }
                    else if (incidentid.ToString() != ViewState["Oldincidentid"].ToString())
                    {
                        ViewState["Oldincidentid"] = incidentid;
                        Guid? dtkey = General.GetNullableGuid(incidentid.ToString());

                        if (dtkey == null)
                        {
                            txtMessage.Text = ViewState["ORIGINALBODY"].ToString();
                            return;
                        }

                        DataTable dt = PhoenixMailManager.MailIncidentEdit(txtFrom.Text, dtkey);
                        if (dt.Rows.Count > 0)
                            txtMessage.Text = dt.Rows[0]["FLDBODY"].ToString().Trim() + "\n\n" + PhoenixSecurityContext.CurrentSecurityContext.FirstName + "(Phoenix Support)" + "\n\n\n\n ---------------  \n\n\n" + ViewState["ORIGINALBODY"].ToString();

                        if ((dt.Rows[0]["FLDFILENAMES"].ToString() != null) && (dt.Rows[0]["FLDFILENAMES"].ToString() != ""))
                        {
                            int n = PhoenixMailManager.MailOutIncidentAttachment(General.GetNullableGuid(ViewState["SESSIONID"].ToString()), ViewState["MESSAGEID"].ToString(), txtFrom.Text, dtkey);
                            BindAttachmentData();
                        }
                        else
                            gvAttachment.Visible = false;
                    }
                    else
                    {                                             
                    }
                }               
            }            
        }     
    }

    private void BindAttachmentData()
    {
        DataTable dt = new DataTable();
        dt = PhoenixMailManager.MailOutAttachments(General.GetNullableGuid(ViewState["SESSIONID"].ToString()));

        if (dt.Rows.Count > 0)
        {
            gvAttachment.Visible = true;
            gvAttachment.DataSource = dt;
            gvAttachment.DataBind();
        }
        else
            gvAttachment.Visible = false;
    }

    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFileName = (Label)e.Row.FindControl("lblFileName");
            HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");
            lnk.NavigateUrl = "DefectTrackerMailAttachment.ashx?path=" + lblFileName.Text;
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
    protected void gvSEPIncidentList_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvSEPIncidentList, "Select$" + e.Row.RowIndex.ToString(), false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
