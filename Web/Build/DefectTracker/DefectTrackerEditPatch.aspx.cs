using System;
using System.Data;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTracker_DefectTrackerEditPatch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbaredit = new PhoenixToolbar();
            toolbaredit.AddButton("Add/Edit", "ADDEDIT");
            toolbaredit.AddButton("Mail", "MAILEDIT");
            toolbaredit.AddButton("Reminder", "REMINDER");
            toolbaredit.AddButton("Escalation", "ESCALATION");
            MenuPatch.AccessRights = this.ViewState;
            MenuPatch.MenuList = toolbaredit.Show();


            PhoenixToolbar toolbarsave = new PhoenixToolbar();
            toolbarsave.AddButton("Save", "SAVE");
            MenuPatchSave.AccessRights = this.ViewState;
            MenuPatchSave.MenuList = toolbarsave.Show();

            cblCc.DataSource = PhoenixPatchTracker.PatchEscalationMailList();
            cblCc.DataBind();

            if (Request.QueryString["projectdtkey"] != null)
            {
                ViewState["PATCHDTKEY"] = Request.QueryString["projectdtkey"].ToString();
                if (Request.QueryString["sessionid"] == null)
                    ViewState["SESSIONID"] = Guid.NewGuid().ToString();
                BindData();
                BindAttachmentData();
            }
        }
    }
    protected void MenuPatch_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("ADDEDIT"))
        {
            Response.Redirect("DefectTrackerPatchProjectAddEdit.aspx?projectdtkey=" + ViewState["PATCHDTKEY"].ToString());
        }

        if (dce.CommandName.ToUpper().Equals("REMINDER"))
        {
            Response.Redirect("DefectTrackerTrackerReminderEscalation.aspx?type=1&projectdtkey=" + ViewState["PATCHDTKEY"].ToString());
        }

        if (dce.CommandName.ToUpper().Equals("ESCALATION"))
        {
            Response.Redirect("DefectTrackerTrackerReminderEscalation.aspx?type=2&projectdtkey=" + ViewState["PATCHDTKEY"].ToString());
        }
    }


    protected void MenuSavePatch_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string departmentcc = General.ReadCheckBoxList(cblCc);    

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            PhoenixDefectTracker.PatchMailSave(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                new Guid(ViewState["PATCHDTKEY"].ToString()),
                                                txtSubject.Text, txtCc.Text, txtMessage.Text, departmentcc
                                               );
        }
    }

    private void BindData()
    {
        DataTable dt = PhoenixDefectTracker.PatchAttachmentsSearch(General.GetNullableGuid(ViewState["PATCHDTKEY"].ToString()));
        if (dt.Rows.Count > 0)
        {
            txtCc.Text = dt.Rows[0]["FLDMAILCC"].ToString();
            txtSubject.Text = dt.Rows[0]["FLDSUBJECT"].ToString();
            txtMessage.Text = dt.Rows[0]["FLDBODY"].ToString();
            General.BindCheckBoxList(cblCc, dt.Rows[0]["FLDDEPARTMENTMAILCC"].ToString());
        }
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
    private void BindAttachmentData()
    {
        DataTable dt = new DataTable();

        dt = PhoenixMailManager.MailOutAttachments(General.GetNullableGuid(ViewState["SESSIONID"].ToString()));

        gvAttachment.DataSource = dt;
        gvAttachment.DataBind();
    }
}
