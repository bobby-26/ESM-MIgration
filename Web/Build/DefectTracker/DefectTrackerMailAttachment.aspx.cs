using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerMailAttachment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Message", "MAILEDIT");
        MenuAttachment.AccessRights = this.ViewState;
        MenuAttachment.MenuList = toolbar.Show();

        PhoenixToolbar toolbaredit = new PhoenixToolbar();
        toolbaredit.AddButton("Save", "SAVE");
        MenuBugAttachment.AccessRights = this.ViewState;
        MenuBugAttachment.MenuList = toolbaredit.Show();

        ViewState["SESSIONID"] = Request.QueryString["sessionid"].ToString();
        ViewState["MAILID"] = Request.QueryString["mailid"].ToString();

        BindData();
    }

    protected void MenuAttachment_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("MAILEDIT"))
        {
            if (Request.QueryString["sentitem"] != null)
                Response.Redirect("DefectTrackerMailReply.aspx?sentitem=1&mailid=" + ViewState["MAILID"].ToString() + "&sessionid=" + ViewState["SESSIONID"].ToString(), false);
            else
                Response.Redirect("DefectTrackerMailReply.aspx?mailid=" + ViewState["MAILID"].ToString() + "&sessionid=" + ViewState["SESSIONID"].ToString(), false);
        }
    }

    protected void MenuBugAttachment_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
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
            BindData();
        }
    }

    private void BindData()
    {
        DataTable dt = new DataTable();

        dt = PhoenixMailManager.MailOutAttachments(General.GetNullableGuid(ViewState["SESSIONID"].ToString()));

        gvAttachment.DataSource = dt;
        gvAttachment.DataBind();
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
}
