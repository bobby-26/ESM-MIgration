using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerBugComment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Edit", "BUGEDIT");
        toolbar.AddButton("Attachment", "ATTACHMENT");
        toolbar.AddButton("Analysis", "ANALYSIS");
        toolbar.AddButton("Track", "TRACK");

        string username = PhoenixSecurityContext.CurrentSecurityContext.UserName;
        if (username.Contains("808"))
            toolbar.AddButton("Road Map", "ROADMAP");

        MenuDiscussion.AccessRights = this.ViewState;
        MenuDiscussion.MenuList = toolbar.Show();

        PhoenixToolbar toolbaredit = new PhoenixToolbar();
        toolbaredit.AddButton("Post", "POSTCOMMENT");
        MenuBugDiscussion.AccessRights = this.ViewState;
        MenuBugDiscussion.MenuList = toolbaredit.Show();

        ViewState["BUGDTKEY"] = Request.QueryString["dtkey"].ToString();
        if (!IsPostBack)
        {
            BugEdit(ViewState["BUGDTKEY"].ToString());
            BindData();
        }
        //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
    }

    private void BugEdit(string id)
    {
        DataTable dt = PhoenixDefectTracker.BugEdit(General.GetNullableGuid(id));
        DataRow dr = dt.Rows[0];
        ucTitle.Text = "Comments - [" + dr["FLDBUGID"].ToString() + "]";
    }

    private void BindData()
    {
        DataTable dt;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        dt = PhoenixDefectTracker.DefectComments(new Guid(ViewState["BUGDTKEY"].ToString()));

        if (dt.Rows.Count > 0)
        {
            repDiscussion.DataSource = dt;
            repDiscussion.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, repDiscussion);
        }
    }

    private void ShowNoRecordsFound(DataTable dt, Repeater rep)
    {
        dt.Rows.Add(dt.NewRow());
        rep.DataSource = dt;
        rep.DataBind();
    }

    protected void MenuDiscussion_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("BUGEDIT"))
        {
            Response.Redirect("DefectTrackerBugEdit.aspx?dtkey=" + ViewState["BUGDTKEY"].ToString(), false);
        }
        if (dce.CommandName.ToUpper().Equals("ATTACHMENT"))
        {
            Response.Redirect("DefectTrackerBugAttachment.aspx?dtkey=" + ViewState["BUGDTKEY"].ToString(), false);
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

    protected void MenuBugDiscussion_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("POSTCOMMENT"))
        {
            if (!IsCommentValid(txtNotesDescription.Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixDefectTracker.BugComment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["BUGDTKEY"].ToString()), txtNotesDescription.Text);
            txtNotesDescription.Text = "";
            BindData();
        }
    }

    private bool IsCommentValid(string strComment)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        return (!ucError.IsError);
    }


    protected void GetRemarks(object sender, EventArgs e)
    {
        BindData();
    }


}
