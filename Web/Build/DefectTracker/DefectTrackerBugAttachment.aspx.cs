using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTracker_DefectTrackerBugAttachment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Edit", "BUGEDIT");
        toolbar.AddButton("Comments", "COMMENTS");
        toolbar.AddButton("Analysis", "ANALYSIS");
        toolbar.AddButton("Track", "TRACK");
        
        string username = PhoenixSecurityContext.CurrentSecurityContext.UserName;
        if (username.Contains("808"))
            toolbar.AddButton("Road Map", "ROADMAP");

        MenuAttachment.AccessRights = this.ViewState;
        MenuAttachment.MenuList = toolbar.Show();

        ViewState["BUGDTKEY"] = Request.QueryString["dtkey"].ToString();

        if (!IsPostBack)
        {
            BugEdit(ViewState["BUGDTKEY"].ToString());
        }


        BindData();
    }

    private void BugEdit(string id)
    {
        DataTable dt = PhoenixDefectTracker.BugEdit(General.GetNullableGuid(id));
        DataRow dr = dt.Rows[0];
        ViewState["BUGID"] = dr["FLDBUGID"].ToString();
        ucTitle.Text = "Attachments - [" + dr["FLDBUGID"].ToString() + "]";

            PhoenixToolbar toolbaredit = new PhoenixToolbar();
            toolbaredit.AddButton("Save", "SAVE");
            MenuBugAttachment.AccessRights = this.ViewState;
            MenuBugAttachment.MenuList = toolbaredit.Show();
    }

    protected void MenuAttachment_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("COMMENTS"))
        {
            Response.Redirect("DefectTrackerBugComment.aspx?dtkey=" + ViewState["BUGDTKEY"].ToString(), false);
        }
        if (dce.CommandName.ToUpper().Equals("BUGEDIT"))
        {
            Response.Redirect("DefectTrackerBugEdit.aspx?dtkey=" + ViewState["BUGDTKEY"].ToString(), false);
        }
        if (dce.CommandName.ToUpper().Equals("ANALYSIS"))
        {
            Response.Redirect("DefectTrackerBugAnalysis.aspx?dtkey=" + ViewState["BUGDTKEY"].ToString(), false);
        }
        if (dce.CommandName.ToUpper().Equals("TRACK"))
        {
            Response.Redirect("DefectTrackerIssueTrack.aspx?dtkey=" + ViewState["BUGDTKEY"].ToString(), false);
        }
        if (dce.CommandName.ToUpper().Equals("ROADMAP"))
        {
            Response.Redirect("DefectTrackerIssueFlag.aspx?dtkey=" + ViewState["BUGDTKEY"].ToString(), false);
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
                path = path + "Attachments\\SEP";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                for (int i = 0; i < postedFiles.Count; i++)
                {
                    HttpPostedFile postedFile = postedFiles[i];
                    if (postedFile.ContentLength > 0)
                    {
                        string dtkey = Guid.NewGuid().ToString();

                        string filepath = path;
                        postedFile.SaveAs(path + "\\" + ViewState["BUGID"].ToString() + "_" + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\')+1));
                        PhoenixDefectTracker.BugAttachmentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            General.GetNullableGuid(ViewState["BUGDTKEY"].ToString()), ViewState["BUGID"].ToString() + "_" + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1), filepath);
                    }
                }
            }
            BindData();
        }
    }

    private void BindData()
    {
        DataTable dt = new DataTable();

        dt = PhoenixDefectTracker.BugAttachments(General.GetNullableGuid(ViewState["BUGDTKEY"].ToString()));

        gvAttachment.DataSource = dt;
        gvAttachment.DataBind();
    }

    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFileName = (Label)e.Row.FindControl("lblFileName");
            HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");
            lnk.NavigateUrl = Session["sitepath"] + "/attachments/SEP/" + lblFileName.Text;
        }
    }
}
