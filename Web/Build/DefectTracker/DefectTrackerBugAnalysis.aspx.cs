using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;


public partial class DefectTracker_DefectTrackerBugAnalysis : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Edit", "BUGEDIT");
        toolbar.AddButton("Comments", "COMMENTS");
        toolbar.AddButton("Attachment", "ATTACHMENT");
        toolbar.AddButton("Track", "TRACK");

        string username = PhoenixSecurityContext.CurrentSecurityContext.UserName;
        if (username.Contains("808"))
            toolbar.AddButton("Road Map", "ROADMAP");

        MenuAnalysis.AccessRights = this.ViewState;
        MenuAnalysis.MenuList = toolbar.Show();

        PhoenixToolbar toolbaredit = new PhoenixToolbar();
        toolbaredit.AddButton("Save", "SAVE");
        toolbaredit.AddButton("Log Issue", "POST");
        MenuBugAnalysis.MenuList = toolbaredit.Show();

        if (!IsPostBack)
        {
            BugEdit(Request.QueryString["dtkey"].ToString());
            ViewState["COMMENTS"] = "";
            ViewState["BUGID"] = "";
            ViewState["NEWBUGID"] = "";
            BindData();
        }
        else
            BindData();
        //BugEdit(ViewState["BUGDTKEY"].ToString());
    }

    private void BugEdit(string id)
    {
        DataTable dt = PhoenixDefectTracker.BugEdit(General.GetNullableGuid(id));
        DataRow dr = dt.Rows[0];
        ucTitle.Text = "Analysis - [" + dr["FLDBUGID"].ToString() + "]";
        lblBugid.Text = dr["FLDBUGID"].ToString();
        ViewState["BUGDTKEY"] = dr["FLDDTKEY"].ToString();
        ddlBugPriority.SelectedValue = dr["FLDPRIORITYANALYSIS"].ToString();
        ddlBugSeverity.SelectedValue = dr["FLDSEVERITYANALYSIS"].ToString();
        ddlBugType.SelectedValue = dr["FLDTYPEANALYSIS"].ToString();
        ViewState["NEWBUGID"] = dr["FLDANALYSIS"].ToString();
    }

    protected void MenuAnalysis_TabStripCommand(object sender, EventArgs e)
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
        if (dce.CommandName.ToUpper().Equals("ATTACHMENT"))
        {
            Response.Redirect("DefectTrackerBugAttachment.aspx?dtkey=" + ViewState["BUGDTKEY"].ToString(), false);
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


    protected void MenuBugAnalysis_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixDefectTracker.BugAnalysis(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["BUGDTKEY"].ToString())
                    , General.GetNullableString(ddlBugType.SelectedValue)
                    , General.GetNullableString(ddlBugPriority.SelectedValue)
                    , General.GetNullableString(ddlBugSeverity.SelectedValue)
                    , "");
            }
            if (dce.CommandName.ToUpper().Equals("POST"))
            {
                string bugid = ViewState["NEWBUGID"].ToString();
                PhoenixDefectTracker.InsertNewDefect(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["BUGDTKEY"].ToString()), General.GetNullableString(txtNewIssueComment.Text), ref bugid);
                ucStatus.Text = "New issue added";
            }
            BindData();
            BugEdit(ViewState["BUGDTKEY"].ToString());
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        DataTable dt;

        dt = PhoenixDefectTracker.DefectComments(new Guid(ViewState["BUGDTKEY"].ToString()));

        if (dt.Rows.Count > 0)
        {
            gvCommentList.DataSource = dt;
            gvCommentList.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvCommentList);
        }
    }

    protected void gvCommentList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            Label lblComment = (Label)_gridView.Rows[nCurrentRow].FindControl("lblComments");
            if (lblComment != null) txtNewIssueComment.Text = "[Requirement carry forwarded from " + lblBugid.Text + "] \n" + lblComment.Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();
    }
}
