using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.DefectTracker;
using SouthNests.Phoenix.Framework;

public partial class DefectTrackerBugEdit : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Comments", "COMMENTS");
            toolbar.AddButton("Attachment", "ATTACHMENT");
            toolbar.AddButton("Analysis", "ANALYSIS");
            toolbar.AddButton("Track", "TRACK");

            string username = PhoenixSecurityContext.CurrentSecurityContext.UserName;
            if (username.Contains("808"))
                toolbar.AddButton("Road Map", "ROADMAP");
            
            MenuBugComment.AccessRights = this.ViewState;
            MenuBugComment.MenuList = toolbar.Show();

            PhoenixToolbar toolbarbugedit = new PhoenixToolbar();
            toolbarbugedit.AddButton("New", "NEW");
            toolbarbugedit.AddButton("Save", "SAVE");
            toolbarbugedit.AddButton("Approve", "APPROVE");
            toolbarbugedit.AddButton("Reply By Mail", "REPLYBYMAIL");

            MenuBugEdit.AccessRights = this.ViewState;
            MenuBugEdit.MenuList = toolbarbugedit.Show();  

            if (!IsPostBack)
            {
                string id = Request.QueryString["dtkey"].ToString();
                ViewState["BUGDTKEY"] = id;
                BugEdit(id);

            }

            BindDefaultVessel();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindProjectList()
    {
        DataTable dt = PhoenixDefectTracker.ProjectList(null);
        if (dt.Rows.Count > 0)
        {
            ddlProject.DataSource = dt;
            ddlProject.DataValueField = "FLDPROJECTID";
            ddlProject.DataTextField = "FLDPROJECTNAME";
            ddlProject.DataBind();
        }
    }

    private void BindDefaultVessel()
    {
        DataTable dt = PhoenixDefectTracker.DefaultVessel();

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            int nVessel = int.Parse(dr["FLDINSTALLCODE"].ToString());

            if (nVessel > 0)
            {
                ddlVesselCode.SelectedValue = dr["FLDINSTALLCODE"].ToString();
                ddlVesselCode.Enabled = false;
            }
        }
    }

    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindVesselList();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindVesselList()
    {
        if (ddlProject.SelectedValue.Trim() != string.Empty)
        {
            DataTable dt = PhoenixDefectTracker.vessellist(int.Parse(ddlProject.SelectedValue));
            if (dt.Rows.Count > 0)
            {
                ddlVesselCode.DataSource = dt;
                ddlVesselCode.DataValueField = "FLDVESSELID";
                ddlVesselCode.DataTextField = "FLDVESSELNAME";
                ddlVesselCode.DataBind();
                ddlVesselCode.Items.Insert(0, new ListItem("--SELECT--", ""));
            }
        }
    }

    private void BugEdit(string id)
    {

        DataTable dt = PhoenixDefectTracker.BugEdit(General.GetNullableGuid(id));
        DataRow dr = dt.Rows[0];
        ucTitle.Text = "Edit - [" + dr["FLDBUGID"].ToString() + "] - " + dr["FLDREPORTEDBY"].ToString() + " (" + dr["FLDOPENDATE"].ToString() + ")";
        ViewState["BUGDTKEY"] = dr["FLDDTKEY"].ToString();
        lblCreatedOn.Text = dr["FLDOPENDATE"].ToString();
        lblCompletedOn.Text = dr["FLDCOMPLETEDDATE"].ToString();
        lblClosedOn.Text = dr["FLDCLOSEDDATE"].ToString();
        ddlSEPBugStatus.BugDTKey = new Guid(id);
        ddlSEPBugStatus.BugId = int.Parse(dr["FLDBUGID"].ToString());
        ddlSEPBugStatus.SelectedValue = dr["FLDSTATUS"].ToString();
        ddlBugPriority.SelectedValue = dr["FLDPRIORITY"].ToString();
        ddlBugSeverity.SelectedValue = dr["FLDSEVERITY"].ToString();
        ddlBugType.SelectedValue = dr["FLDTYPE"].ToString();
        ddlModuleList.SelectedValue = dr["FLDMODULEID"].ToString();
        ddlVesselCode.SelectedValue = General.GetNullableString(dr["FLDVESSELCODE"].ToString());
        txtSubject.Text = dr["FLDSUBJECT"].ToString();
        txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
        txtReportedBy.Text = dr["FLDREPORTEDBYNAME"].ToString();
        ddlProject.SelectedValue = dr["FLDPROJECTID"].ToString();
        BindProjectList();
        if (ddlVesselCode.SelectedValue != string.Empty)
        {
            BindVesselList();
        }
        else
        {
            DataTable ddt = PhoenixDefectTracker.vessellist(int.Parse(ddlProject.SelectedValue));
            if (ddt.Rows.Count > 0)
            {
                ddlVesselCode.DataSource = ddt;
                ddlVesselCode.DataValueField = "FLDVESSELID";
                ddlVesselCode.DataTextField = "FLDVESSELNAME";
                ddlVesselCode.DataBind();
                ddlVesselCode.Items.Insert(0, new ListItem("--SELECT--", ""));
            }
        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

    }

    protected void MenuBugComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("COMMENTS"))
            {
                Response.Redirect("DefectTrackerBugComment.aspx?dtkey=" + ViewState["BUGDTKEY"].ToString(), false);
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuBugEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (!ValidateBugId())
            {
                ucError.Visible = true;
                return;
            }
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                Response.Redirect("DefectTrackerBugAdd.aspx?projectname=" + ddlProject.SelectedValue, false);
            }

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                Guid dtkey = Guid.NewGuid();

                PhoenixDefectTracker.BugSave(-1,
                            int.Parse(ddlModuleList.SelectedValue),
                            56
                            , txtSubject.Text
                            , txtDescription.Text
                            , ddlSEPBugStatus.SelectedValue
                            , ddlBugType.SelectedValue
                            , ddlBugPriority.SelectedValue
                            , ddlBugSeverity.SelectedValue, PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , "1.0", "Phoenix", null, null, null, "sep@southnests.com", General.GetNullableInteger(ddlVesselCode.SelectedValue)
                            , txtReportedBy.Text, General.GetNullableGuid(ViewState["BUGDTKEY"].ToString()), int.Parse(ddlProject.SelectedValue), ref dtkey);

                ucStatus.Text = "Issue changes saved";
                ddlSEPBugStatus.BugList = PhoenixDefectTracker.GetNextStatus(new Guid(ViewState["BUGDTKEY"].ToString()));
                BugEdit(ViewState["BUGDTKEY"].ToString());

                if (Request.QueryString["norefresh"] == null)
                {
                    String script = "javascript:fnReloadList('code1');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }

            }
            if (dce.CommandName.ToUpper().Equals("APPROVE"))
            {
                PhoenixDefectTracker.BugApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["BUGDTKEY"].ToString()));
                BugEdit(ViewState["BUGDTKEY"].ToString());
            }
            if (dce.CommandName.ToUpper().Equals("REPLYBYMAIL"))
            {
                PhoenixDefectTracker.BugToReply(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["BUGDTKEY"].ToString()));
                BugEdit(ViewState["BUGDTKEY"].ToString());
                ucStatus.Text = "Issue logged as Mail. Use Mail tracker to reply";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool ValidateBugId()
    {
        if (General.GetNullableInteger(ddlModuleList.SelectedValue) == null)
            ucError.ErrorMessage = "Module is required";

        if (General.GetNullableInteger(ddlSEPBugStatus.SelectedValue) == null)
            ucError.ErrorMessage = "Status is required";

        if (General.GetNullableInteger(ddlBugType.SelectedValue) == null)
            ucError.ErrorMessage = "Type is required";

        if (General.GetNullableInteger(ddlBugSeverity.SelectedValue) == null)
            ucError.ErrorMessage = "Severity is required";

        if (General.GetNullableInteger(ddlBugPriority.SelectedValue) == null)
            ucError.ErrorMessage = "Priority is required";

        return !ucError.IsError;
    }



    protected void gvDefectAssign_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = -1;
        BindData();

    }

    protected void gvDefectAssign_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvDefectAssign.EditIndex = -1;
        gvDefectAssign.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    private bool IsValidAssignment(GridView _gridView)
    {
        if (General.GetNullableString(((UserControlSEPTeamMembers)_gridView.FooterRow.FindControl("ucDeveloperNameAdd")).SelectedTeamMemberName) == null)
            ucError.ErrorMessage = "Assigned To is required";

        if (General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("ucStartDateFooter")).Text) == null)
            ucError.ErrorMessage = "Start date is required";

        if (General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("ucFinishDateFooter")).Text) == null)
            ucError.ErrorMessage = "Expected Finish Date is required";

        return !ucError.IsError;
    }

    protected void gvDefectAssign_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidAssignment(_gridView))
                {
                    ucError.Visible = true;
                    return;
                }
                TextBox ucStartTimeAdd = (TextBox)_gridView.FooterRow.FindControl("ucStartTimeAdd");
                TextBox ucFinishTimeAdd = (TextBox)_gridView.FooterRow.FindControl("ucFinishTimeAdd");

                string timeofstart = (ucStartTimeAdd.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : ucStartTimeAdd.Text;
                string timeoffinish = (ucFinishTimeAdd.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : ucFinishTimeAdd.Text;

                PhoenixDefectTracker.BugAssign(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["BUGDTKEY"].ToString()),
                    ((UserControlSEPTeamMembers)_gridView.FooterRow.FindControl("ucDeveloperNameAdd")).SelectedTeamMemberName,
                    General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("ucStartDateFooter")).Text + " " + timeofstart),
                    General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("ucFinishDateFooter")).Text + " " + timeoffinish),
                    ((TextBox)_gridView.FooterRow.FindControl("txtRemarksAdd")).Text,
                    null, General.GetNullableDecimal(((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtPlannedEffortAdd")).Text), null
                    );
                BindData();
                BugEdit(ViewState["BUGDTKEY"].ToString());
                ddlSEPBugStatus.BugList = PhoenixDefectTracker.GetNextStatus(new Guid(ViewState["BUGDTKEY"].ToString()));
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                TextBox ucActualFinishTimeEdit = (TextBox)_gridView.Rows[nCurrentRow].FindControl("ucActualFinishTimeEdit");
                string timeofactualfinish = (ucActualFinishTimeEdit.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : ucActualFinishTimeEdit.Text;

                TextBox ucDeployTimeEdit = (TextBox)_gridView.Rows[nCurrentRow].FindControl("ucDeployTimeEdit");
                string timeofdeploy = (ucDeployTimeEdit.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : ucDeployTimeEdit.Text;

                TextBox ucActualStartTimeEdit = (TextBox)_gridView.Rows[nCurrentRow].FindControl("ucActualStartTimeEdit");
                string timeofstart = (ucActualStartTimeEdit.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : ucActualStartTimeEdit.Text;

                UserControlDate ucActualFinishDateEdit = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucActualFinishDateEdit"));

                PhoenixDefectTracker.BugAssignUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAssignedTo")).Text,
                                                    General.GetNullableDateTime(ucActualFinishDateEdit.Text + " " + timeofactualfinish),
                                                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text,
                                                    new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text),
                                                    General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDeployDateEdit")).Text + " " + timeofdeploy), 0,
                                                    General.GetNullableDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtActualEffort")).Text),
                                                    General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucActualStartDateEdit")).Text + " " + timeofstart)
                                                    );

                _gridView.EditIndex = -1;
                _gridView.SelectedIndex = -1;
                BindData();
                ddlSEPBugStatus.BugList = PhoenixDefectTracker.GetNextStatus(new Guid(ViewState["BUGDTKEY"].ToString()));
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixDefectTracker.BugAssignDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKeyDisplay")).Text));

                BindData();
                BugEdit(ViewState["BUGDTKEY"].ToString());
                ddlSEPBugStatus.BugList = PhoenixDefectTracker.GetNextStatus(new Guid(ViewState["BUGDTKEY"].ToString()));

            }
            BugEdit(ViewState["BUGDTKEY"].ToString());
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDefectAssign_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
    }

    protected void gvDefectAssign_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
    }

    protected void gvDefectAssign_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            _gridView.EditIndex = -1;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDefectAssign_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            TextBox ucActualStartTimeEdit = (TextBox)e.Row.FindControl("ucActualStartTimeEdit");
            TextBox ucActualFinishTimeEdit = (TextBox)e.Row.FindControl("ucActualFinishTimeEdit");
            TextBox ucAssignedTimeEdit = (TextBox)e.Row.FindControl("ucAssignedTimeEdit");
            TextBox ucStartTimeEdit = (TextBox)e.Row.FindControl("ucStartTimeEdit");
            TextBox ucFinishTimeEdit = (TextBox)e.Row.FindControl("ucFinishTimeEdit");
            TextBox ucDeployTimeEdit = (TextBox)e.Row.FindControl("ucDeployTimeEdit");
            UserControlSEPTeamMembers ucDeveloperNameEdit = (UserControlSEPTeamMembers)e.Row.FindControl("ucDeveloperNameEdit");

            if (ucDeveloperNameEdit != null)
                ucDeveloperNameEdit.SelectedTeamMember = drv["FLDDEVELOPERID"].ToString();

            if (ucActualStartTimeEdit != null)
                ucActualStartTimeEdit.Text = String.Format("{0:HH:mm}", drv["FLDACTUALSTARTDATE"]);

            if (ucActualFinishTimeEdit != null)
                ucActualFinishTimeEdit.Text = String.Format("{0:HH:mm}", drv["FLDACTUALFINISHDATE"]);

            if (ucAssignedTimeEdit != null)
                ucAssignedTimeEdit.Text = String.Format("{0:HH:mm}", drv["FLDASSIGNEDDATE"]);

            if (ucStartTimeEdit != null)
                ucStartTimeEdit.Text = String.Format("{0:HH:mm}", drv["FLDSTARTDATE"]);

            if (ucFinishTimeEdit != null)
                ucFinishTimeEdit.Text = String.Format("{0:HH:mm}", drv["FLDFINISHDATE"]);

            if (ucDeployTimeEdit != null)
                ucDeployTimeEdit.Text = String.Format("{0:HH:mm}", drv["FLDDEPLOYEDDATE"]);

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    private void BindData()
    {
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataTable dt = PhoenixDefectTracker.DefectAssignment(new Guid(ViewState["BUGDTKEY"].ToString()));
        if (dt.Rows.Count > 0)
        {
            gvDefectAssign.DataSource = dt;
            gvDefectAssign.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvDefectAssign);
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
}
