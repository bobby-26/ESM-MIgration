using System;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using System.IO;

public partial class DefectTrackerIssueFlag : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Header.DataBind();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Edit", "BUGEDIT");
            toolbar.AddButton("Comments", "COMMENTS");
            toolbar.AddButton("Attachment", "ATTACHMENT");
            toolbar.AddButton("Analysis", "ANALYSIS");
            toolbar.AddButton("Track", "TRACK");

            MenuIssueTrack.AccessRights = this.ViewState;
            MenuIssueTrack.MenuList = toolbar.Show();

            PhoenixToolbar toolbaredit = new PhoenixToolbar();
            toolbaredit.AddButton("Save", "SAVE");

            MenuBugDiscussion.AccessRights = this.ViewState;
            MenuBugDiscussion.MenuList = toolbaredit.Show();

            if (!IsPostBack)
            {
                ViewState["SERVERLIST"] = "";
                string dtkey = "";
                if (Request.QueryString["dtkey"] != null)
                {
                    dtkey = Request.QueryString["dtkey"].ToString();
                    ViewState["BUGDTKEY"] = dtkey;
                }
                BugEdit(ViewState["BUGDTKEY"].ToString());
                BindMilestone();
                BindFlag();
                ddlissueflag.Enabled = false;
                BindDeploymentServer();
                BugMilestoneEdit(ViewState["BUGID"].ToString());
            }

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BugMilestoneEdit(string bugdtkey)
    {
        DataTable dt = PhoenixDefectTracker.BugMilestoneEdit(int.Parse(bugdtkey));        

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ddlmilestone.SelectedValue = dr["FLDMILESTONE"].ToString();
            if (dr["FLDISSUEFLAG"].ToString() != "")
            {
                ddlissueflag.SelectedValue = dr["FLDISSUEFLAG"].ToString();
                ddlissueflag.Enabled = true;
                txtNotesDescription.CssClass = "gridinput_mandatory";
                ddlissueflag.CssClass = "dropdown_mandatory";
            }
            txtNotesDescription.Text = dr["FLDCOMMENT"].ToString();
        }
    }

    private void BugEdit(string id)
    {
        DataTable dt = PhoenixDefectTracker.BugEdit(General.GetNullableGuid(id));
        DataRow dr = dt.Rows[0];

        ViewState["BUGID"] = dr["FLDBUGID"].ToString();
        ucTitle.Text = "Road Map - [" + dr["FLDBUGID"].ToString() + "] ";

    }

    private void BindFlag()
    {
        DataTable dt = PhoenixDefectTracker.FlagMilestone(null);
        if (dt.Rows.Count > 0)
        {
            ddlissueflag.DataSource = dt;
            ddlissueflag.DataValueField = "FLDISSUEFLAGCODE";
            ddlissueflag.DataTextField = "FLDISSUEFLAG";
            ddlissueflag.DataBind();
        }
        ddlissueflag.Items.Insert(0, new ListItem("--Select--", ""));
    }

    private void BindMilestone()
    {
        DataTable dt = PhoenixDefectTracker.TargetMilestone(null);
        if (dt.Rows.Count > 0)
        {
            ddlmilestone.DataSource = dt;
            ddlmilestone.DataValueField = "FLDMILESTONECODE";
            ddlmilestone.DataTextField = "FLDMILESTONE";
            ddlmilestone.DataBind();
        }
        ddlmilestone.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void BindDeploymentServer()
    {
        DataTable dt = new DataTable();
        dt = PhoenixDefectTracker.DeploymentServerList();

        if (dt.Rows.Count > 0)
        {
            chklstDeploymentServer.DataSource = dt;
            chklstDeploymentServer.DataValueField = "FLDSERVERID";
            chklstDeploymentServer.DataValueField = "FLDSERVERNAME";
            chklstDeploymentServer.DataBind();
        }
    }

    protected void chklstDeploymentServer_TextChanged(object sender, EventArgs e)
    {
        string serverlist = "";

        foreach (ListItem item in chklstDeploymentServer.Items)
        {
            if (item.Selected)
            {
                if (serverlist == "")
                    serverlist = serverlist + (item.Text);
                else
                    serverlist = serverlist + "," + (item.Text);
            }
        }
        ViewState["SERVERLIST"] = serverlist;
    }

    private void BindData()
    {
        DataTable dt = PhoenixDefectTracker.BugMilestoneList(new Guid(ViewState["BUGDTKEY"].ToString()));
        if (dt.Rows.Count > 0)
        {

            gvIssueTrack.DataSource = dt;
            gvIssueTrack.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvIssueTrack);
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
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    protected void MenuIssueTrack_TabStripCommand(object sender, EventArgs e)
    {
        try
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
            if (dce.CommandName.ToUpper().Equals("ANALYSIS"))
            {
                Response.Redirect("DefectTrackerBugAnalysis.aspx?dtkey=" + ViewState["BUGDTKEY"].ToString(), false);
            }
            if (dce.CommandName.ToUpper().Equals("TRACK"))
            {
                Response.Redirect("DefectTrackerIssueTrack.aspx?dtkey=" + ViewState["BUGDTKEY"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvIssueTrack_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                string script = drv["FLDSCRIPTNAME"].ToString();
                string bugid = drv["FLDBUGID"].ToString();

                ImageButton eb = (ImageButton)e.Row.FindControl("cmdAttachment");

                if ((drv["FLDSCRIPTNAME"].ToString() == "") || (drv["FLDSCRIPTNAME"].ToString() == null))
                {
                    eb.ImageUrl = Session["images"] + "/no-attachment.png";
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                }
                else
                {
                    eb.ImageUrl = Session["images"] + "/attachment.png";
                    eb.Attributes.Add("onclick", "Openpopup('filter', '','DefectTrackerIssueScriptAttachment.aspx?bugid=" + bugid + "&script=" + script + "'); return false;");

                }

            }

        }
        catch (Exception er)
        {
            ucError.ErrorMessage = er.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuBugDiscussion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                string filename = "";
                string path = "";
                string orginalfilename = "";
                if (ddlissueflag.SelectedValue == "12")
                {
                    HttpFileCollection postedFiles = Request.Files;
                    if (Request.Files["filePatchAttachment"].ContentLength > 0)
                    {
                        string origpath = HttpContext.Current.Request.MapPath("~/");

                        for (int i = 0; i < postedFiles.Count; i++)
                        {
                            HttpPostedFile postedFile = postedFiles[i];
                            path = origpath + "Attachments\\Scripts";

                            if (postedFile.ContentLength > 0)
                            {
                                orginalfilename = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1);

                                if (postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.')) != ".zip")
                                {
                                    ucError.ErrorMessage = "Upload file with .zip extension.";
                                    ucError.Visible = true;
                                    break;
                                }
                                if (File.Exists(path + "\\" + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1)))
                                {
                                    ucError.ErrorMessage = "The file already exists. Cannot upload file with same name.";
                                    ucError.Visible = true;
                                    break;
                                }
                                string modulename = ucModule.ModuleName;
                                filename = "Phoenix1.0.Script." + modulename + "." + DateTime.Now.ToShortDateString().Replace("/", "") + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + ".zip";
                                postedFile.SaveAs(path + "\\" + filename);

                            }
                        }
                    }
                }
                if (!IsCommentValid(ddlmilestone.SelectedValue, ddlissueflag.SelectedValue, txtNotesDescription.Text, txtsubject.Text, txtcreatedby.Text, ucModule.SelectedValue, ViewState["SERVERLIST"].ToString(), filename))
                {
                    ucError.Visible = true;
                    return;
                }
                ViewState["COMMENT"] = txtNotesDescription.Text;
                PhoenixDefectTracker.BugMilestone(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ddlmilestone.SelectedValue, ddlissueflag.SelectedValue, new Guid(ViewState["BUGDTKEY"].ToString()), txtNotesDescription.Text,
                                     txtsubject.Text, filename, path, ucModule.SelectedValue, orginalfilename, ViewState["SERVERLIST"].ToString(), txtcreatedby.Text);
                txtNotesDescription.Text = "";
                txtcreatedby.Text = "";
                txtsubject.Text = "";
                ucModule.SelectedValue = "";
                chklstDeploymentServer.ClearSelection();
                ddlmilestone.SelectedValue = "";
                ddlissueflag.SelectedValue = "";
                BindData();
                BugMilestoneEdit(ViewState["BUGID"].ToString());
                ucStatus.Text = "Added";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsCommentValid(string milestone, string issueflag, string comment, string subject, string createdby, string module, string deployon, string filename)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((milestone.Trim() == "") && (issueflag.Trim() == ""))
            ucError.ErrorMessage = "Milestone is required";

        if (((milestone.Trim() == "2") || (milestone.Trim() == "3")) && (comment.Trim() == ""))
            ucError.ErrorMessage = "Comment is required";

        if ((ddlmilestone.SelectedValue == "2") || (ddlmilestone.SelectedValue == "3"))
        {
            if (ddlissueflag.SelectedValue == "")
                ucError.ErrorMessage = "Issue Flag is required";
        }

        if ((ddlissueflag.SelectedValue != "") && (ddlmilestone.SelectedValue == ""))
            ucError.ErrorMessage = "Milestone is required";

        if (ddlmilestone.SelectedValue != "")
        {
            if (ddlissueflag.SelectedValue != "")
            {
                if ((int.Parse(ddlissueflag.SelectedValue) >= 10) && ((ddlmilestone.SelectedValue == "1") || (int.Parse(ddlmilestone.SelectedValue) > 3)))
                    ucError.ErrorMessage = "Milestone is not valid";
            }
        }

        if ((issueflag.Trim() == "12"))
        {
            if (subject == "")
                ucError.ErrorMessage = "Subject is required";

            if (createdby == "")
                ucError.ErrorMessage = "Createdby is required";

            if (module == "Dummy")
                ucError.ErrorMessage = "Module is required";

            if (deployon == "")
                ucError.ErrorMessage = "Database name is required";

            if (filename == "")
                ucError.ErrorMessage = "Script File is required";
        }

        return (!ucError.IsError);
    }

    protected void ddlmilestone_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlmilestone.SelectedValue != "")
            {
                if ((ddlmilestone.SelectedValue == "1") || (int.Parse(ddlmilestone.SelectedValue) > 3))
                {
                    ddlissueflag.SelectedValue = "";
                    txtNotesDescription.Text = "";
                    ddlissueflag.Enabled = false;
                    if (ddlissueflag.SelectedValue == "")
                    {
                        tblscriptadd.Visible = false;
                    }
                }
                else
                {
                    ddlissueflag.Enabled = true;
                }
            }
            else
            {
                ddlissueflag.SelectedValue = "";
                ddlissueflag.Enabled = false;
                tblscriptadd.Visible = false;

            }

            if ((ddlmilestone.SelectedValue == "2") || (ddlmilestone.SelectedValue == "3"))
            {
                txtNotesDescription.CssClass = "gridinput_mandatory";
                ddlissueflag.CssClass = "dropdown_mandatory";
            }
            else
            {
                txtNotesDescription.CssClass = "gridinput";
                ddlissueflag.CssClass = "input";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlissueflag_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtNotesDescription.Text = "";
            if (ddlissueflag.SelectedValue == "12")
            {
                tblscriptadd.Visible = true;
                ddlissueflag.CssClass = "dropdown_mandatory";

            }
            else
            {
                tblscriptadd.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
