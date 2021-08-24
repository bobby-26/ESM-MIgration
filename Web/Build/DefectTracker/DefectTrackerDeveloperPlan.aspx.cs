using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DefectTracker;
using SouthNests.Phoenix.Framework;

public partial class DefectTracker_DefectTrackerDeveloperPlan : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvDeveloperPlan.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvDeveloperPlan.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarbuglist = new PhoenixToolbar();
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerDeveloperPlan.aspx", "Search", "search.png", "SEARCH");
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerDeveloperPlan.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerDeveloperPlan.aspx", "Export to Word", "word.png", "HTMLREPORT");
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerDeveloperPlan.aspx", "Clear Filter", "clear-filter.png", "RESET");
            MenuDefectTracker.AccessRights = this.ViewState;
            MenuDefectTracker.MenuList = toolbarbuglist.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindMilestone();
                BindFlag();
                string username = PhoenixSecurityContext.CurrentSecurityContext.UserName;
                if (username.Contains("808"))
                {
                    lblissueflag.Visible = true;
                    lblmilestone.Visible = true;
                    ddlmilestone.Visible = true;
                    ddlissueflag.Visible = true;
                }
                else
                {
                    lblissueflag.Visible = false;
                    lblmilestone.Visible = false;
                    ddlissueflag.Visible = false;
                    ddlmilestone.Visible = false;
                }
            }
            BindData();
            SetPageNavigator();
            SEPStatus.ParentCode = "23";
            SEPSeverity.ParentCode = "19";
            SEPPriority.ParentCode = "15";
            ucSEPType.ParentCode = "9";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    protected void gvDeveloperPlann_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();

    }
    protected void gvDeveloperPlann_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string count = "1";
            count = (DataBinder.Eval(e.Row.DataItem, "COUNTS").ToString() == null || DataBinder.Eval(e.Row.DataItem, "COUNTS").ToString() == "") ? "1" : DataBinder.Eval(e.Row.DataItem, "COUNTS").ToString();
            if ((Int32.Parse(count) >= 2) || DataBinder.Eval(e.Row.DataItem, "FLDSTATUS").ToString() == "30")
                e.Row.ForeColor = System.Drawing.Color.Red;

            LinkButton lbE = (LinkButton)e.Row.FindControl("lnkBugId");
            if (lbE != null)
            {
                Label lblE = (Label)e.Row.FindControl("lblUniqueID");
                if (SessionUtil.CanAccess(this.ViewState, lbE.CommandName))
                    lbE.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugEdit.aspx?dtkey=" + lblE.Text + "'); return false;");
            }
            Label lbtn = (Label)e.Row.FindControl("lblSubject");
            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("uclblSubject");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
        }
    }

    protected void gvDeveloperPlan_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDeveloperPlan, "Select$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuDefectTracker_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            BindData();
            SetPageNavigator();
        }

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            string username = PhoenixSecurityContext.CurrentSecurityContext.UserName;
            if (username.Contains("808"))
                ShowDevExcel();
            else
                ShowExcel();

        }

        if (dce.CommandName.ToUpper().Equals("HTMLREPORT"))
        {
            GenerateHtmlReport();
        }

        if (dce.CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }
    protected void Filter_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        ViewState["SORTEXPRESSION"] = null;
        ViewState["SORTDIRECTION"] = null;
        BindData();
        SetPageNavigator();
    }

    protected void ShowDevExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDBUGID", "FLDOPENDATE", "FLDMODULENAME", "FLDSUBJECT", "FLDMILESTONE", "FLDISSUEFLAG", "FLDSTATUSNAME", "FLDASSIGNEDTO", "FLDSTARTDATE", "FLDFINISHDATE", "FLDACTUALFINISHDATE", "FLDUSERNAME", "FLDPLANNEDEFFORT", "FLDACTUALEFFORT" };
        string[] alCaptions = { "Bug Id", "Logged On", "Module Name", "Subject", "Milestone", "Issue Flag", "Status", "Assigned To", "Start Date", "Finish Date", "Actual Finish", "Reported By", "Planned Effort", "Actual Effort" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixDefectTracker.DeveloperPlan
          (
            General.GetNullableString(txtIDSearch.Text),
            General.GetNullableString(SEPSeverity.SelectedValue),
            General.GetNullableString(ucSEPModule.SelectedValue),
            General.GetNullableString((ucDeveloperName.SelectedTeamMemberName == "--Select--") ? "" : (ucDeveloperName.SelectedTeamMemberName)),
            General.GetNullableString(SEPStatus.SelectedValue),
            General.GetNullableString(SEPPriority.SelectedValue),
            General.GetNullableString(ucSEPType.SelectedValue),
            General.GetNullableDateTime(ucFromOpenDate.Text),
            General.GetNullableDateTime(ucToOpenDate.Text),
            General.GetNullableDateTime(ucFromStartDate.Text),
            General.GetNullableDateTime(ucToStartDate.Text),
            General.GetNullableDateTime(ucFromFinishDate.Text),
            General.GetNullableDateTime(ucToFinishDate.Text),
            General.GetNullableDateTime(ucFromActualFinishDate.Text),
            General.GetNullableDateTime(ucToActualFinishDate.Text),
            General.GetNullableString(txtReportedby.Text),
            General.GetNullableString(ddlmilestone.SelectedValue),
            General.GetNullableString(ddlissueflag.SelectedValue),
            (int)ViewState["PAGENUMBER"],
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount
          );

        string XlsPath = Server.MapPath(@"~/Attachments/DeveloperPlan.xls");
        string attachment = string.Empty;
        if (XlsPath.IndexOf("\\") != -1)
        {
            string[] strFileName = XlsPath.Split(new char[] { '\\' });
            attachment = "attachment; filename=" + strFileName[strFileName.Length - 1];
        }
        else
            attachment = "attachment; filename=" + XlsPath;

        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/vnd.ms-excel";

        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Issue List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDBUGID", "FLDOPENDATE", "FLDMODULENAME", "FLDSUBJECT", "FLDSTATUSNAME", "FLDASSIGNEDTO", "FLDSTARTDATE", "FLDFINISHDATE", "FLDACTUALFINISHDATE", "FLDUSERNAME", "FLDPLANNEDEFFORT", "FLDACTUALEFFORT" };
        string[] alCaptions = { "Bug Id", "Logged On", "Module Name", "Subject", "Status", "Assigned To", "Start Date", "Finish Date", "Actual Finish", "Reported By", "Planned Effort", "Actual Effort" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixDefectTracker.DeveloperPlan
          (
            General.GetNullableString(txtIDSearch.Text),
            General.GetNullableString(SEPSeverity.SelectedValue),
            General.GetNullableString(ucSEPModule.SelectedValue),
            General.GetNullableString((ucDeveloperName.SelectedTeamMemberName == "--Select--") ? "" : (ucDeveloperName.SelectedTeamMemberName)),
            General.GetNullableString(SEPStatus.SelectedValue),
            General.GetNullableString(SEPPriority.SelectedValue),
            General.GetNullableString(ucSEPType.SelectedValue),
            General.GetNullableDateTime(ucFromOpenDate.Text),
            General.GetNullableDateTime(ucToOpenDate.Text),
            General.GetNullableDateTime(ucFromStartDate.Text),
            General.GetNullableDateTime(ucToStartDate.Text),
            General.GetNullableDateTime(ucFromFinishDate.Text),
            General.GetNullableDateTime(ucToFinishDate.Text),
            General.GetNullableDateTime(ucFromActualFinishDate.Text),
            General.GetNullableDateTime(ucToActualFinishDate.Text),
            General.GetNullableString(txtReportedby.Text),
            General.GetNullableString(ddlmilestone.SelectedValue),
            General.GetNullableString(ddlissueflag.SelectedValue),
            (int)ViewState["PAGENUMBER"],
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount
          );

        string XlsPath = Server.MapPath(@"~/Attachments/DeveloperPlan.xls");
        string attachment = string.Empty;
        if (XlsPath.IndexOf("\\") != -1)
        {
            string[] strFileName = XlsPath.Split(new char[] { '\\' });
            attachment = "attachment; filename=" + strFileName[strFileName.Length - 1];
        }
        else
            attachment = "attachment; filename=" + XlsPath;

        Response.ClearContent();
        Response.AddHeader("content-disposition", attachment);
        Response.ContentType = "application/vnd.ms-excel";

        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Issue List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixDefectTracker.DeveloperPlan
          (
            General.GetNullableString(txtIDSearch.Text),
            General.GetNullableString(SEPSeverity.SelectedValue),
            General.GetNullableString(ucSEPModule.SelectedValue),
            General.GetNullableString((ucDeveloperName.SelectedTeamMemberName == "--Select--") ? "" : (ucDeveloperName.SelectedTeamMemberName)),
            General.GetNullableString(SEPStatus.SelectedValue),
            General.GetNullableString(SEPPriority.SelectedValue),
            General.GetNullableString(ucSEPType.SelectedValue),
            General.GetNullableDateTime(ucFromOpenDate.Text),
            General.GetNullableDateTime(ucToOpenDate.Text),
            General.GetNullableDateTime(ucFromStartDate.Text),
            General.GetNullableDateTime(ucToStartDate.Text),
            General.GetNullableDateTime(ucFromFinishDate.Text),
            General.GetNullableDateTime(ucToFinishDate.Text),
            General.GetNullableDateTime(ucFromActualFinishDate.Text),
            General.GetNullableDateTime(ucToActualFinishDate.Text),
            General.GetNullableString(txtReportedby.Text),
            General.GetNullableString(ddlmilestone.SelectedValue),
            General.GetNullableString(ddlissueflag.SelectedValue),
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount
          );

        if (dt.Rows.Count > 0)
        {
            gvDeveloperPlan.DataSource = dt;
            gvDeveloperPlan.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvDeveloperPlan);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    private void ClearFilter()
    {
        ucSEPModule.SelectedValue = "";
        SEPStatus.SelectedValue = "";
        SEPSeverity.SelectedValue = "";
        ucSEPType.SelectedValue = "";
        txtIDSearch.Text = "";
        ucDeveloperName.SelectedTeamMember = "";
        ucFromOpenDate.Text = "";
        ucToOpenDate.Text = "";
        ucFromStartDate.Text = "";
        ucToStartDate.Text = "";
        ucFromFinishDate.Text = "";
        ucToFinishDate.Text = "";
        ucFromActualFinishDate.Text = "";
        ucToActualFinishDate.Text = "";
        ddlmilestone.SelectedValue = "";
        ddlissueflag.SelectedValue = "";
        BindData();
        SetPageNavigator();
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
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvDeveloperPlan.EditIndex = -1;
        gvDeveloperPlan.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvDeveloperPlan.SelectedIndex = -1;
        gvDeveloperPlan.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }
    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }
    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }
    private void GenerateHtmlReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string actualfromdate = null;
        string actualtodate = null;

        string[] alColumns = { "FLDBUGID", "FLDTYPE", "FLDMODULENAME", "FLDSUBJECT", "FLDDESCRIPTION", "FLDPRIORITY", "FLDSEVERITYNAME", "FLDSTATUSNAME", "FLDREPORTEDBY" };
        string[] alCaptions = { "ID", "Type", "Module", "Subject", "Description", "Priority", "Severity", "Status", "Reported By" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixDefectTracker.BugListByModule(
                                                            General.GetNullableString(txtIDSearch.Text),
                                                            ((ucDeveloperName.SelectedTeamMemberName == "--Select--") ? "" : (ucDeveloperName.SelectedTeamMemberName)),
                                                            General.GetNullableString(ucSEPModule.SelectedValue),
                                                            General.GetNullableString(SEPStatus.SelectedValue),
                                                            General.GetNullableString(SEPPriority.SelectedValue),
                                                            General.GetNullableString(SEPSeverity.SelectedValue),
                                                            General.GetNullableString(ucSEPType.SelectedValue),
                                                            General.GetNullableDateTime(ucFromOpenDate.Text),
                                                            General.GetNullableDateTime(ucToOpenDate.Text),
                                                            General.GetNullableDateTime(ucFromStartDate.Text),
                                                            General.GetNullableDateTime(ucToStartDate.Text),
                                                            General.GetNullableDateTime(ucFromFinishDate.Text),
                                                            General.GetNullableDateTime(ucToFinishDate.Text),
                                                            General.GetNullableDateTime(ucFromActualFinishDate.Text),
                                                            General.GetNullableDateTime(ucToActualFinishDate.Text),
                                                            1,
                                                            10000,
                                                            ref iRowCount,
                                                            ref iTotalPageCount,
                                                            ref actualfromdate,
                                                            ref actualtodate
                                                          );

        DataTable dt = ds.Tables[0];
        DataTable dt3 = ds.Tables[1];

        Response.Write("<table width='100%' >");
        Response.Write("<tr>");
        Response.Write("<td  align='center'>");
        Response.Write("<b><h1> Phoenix Release note</h1></b>");
        Response.Write("</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td>");
        Response.Write("Report from " + actualfromdate + " to " + actualtodate);
        Response.Write("</td>");
        Response.Write("<td>");
        Response.Write("</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td>");
        string module = "";
        string type = "";
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<table>");
            if (module != dr["FLDMODULENAME"].ToString())
            {
                type = "";
                Response.Write("<tr>");
                Response.Write("<td colspan='2'>");
                Response.Write("<b><u><h2>" + dr["FLDMODULENAME"].ToString() + "</h2></u></b>");
                Response.Write("</td>");
                Response.Write("</tr>");
                module = dr["FLDMODULENAME"].ToString();
            }
            if (type != dr["FLDTYPE"].ToString())
            {
                Response.Write("<tr>");
                Response.Write("<td colspan='2'>");
                Response.Write("<b><u><h3>" + dr["FLDTYPE"].ToString() + "</h3></u></b>");
                Response.Write("</td>");
                Response.Write("</tr>");
                type = dr["FLDTYPE"].ToString();
            }
            Response.Write("</table>");
            Response.Write("<table>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("<b>Issue ID</b>");
            Response.Write("</td>");
            Response.Write("<td>");
            Response.Write(dr["FLDBUGID"].ToString());
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td colspan='2'>");
            Response.Write("<b>Posted by:</b> " + dr["FLDREPORTEDBY"].ToString() + " On " + dr["FLDOPENDATE"].ToString() + " <b>Severity:</b> " + dr["FLDSEVERITYNAME"] + " <b>Status:</b>" + dr["FLDSTATUSNAME"]);
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td colspan='2'>");
            Response.Write("<b>Subject</b> ");
            Response.Write(dr["FLDSUBJECT"].ToString());
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Write("<hr/>");
        }
        Response.Write("<b><u>Clarification accepted</u></b>");

        foreach (DataRow dr2 in dt3.Rows)
        {
            Response.Write("<table>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("<b>Issue ID</b>");
            Response.Write("</td>");
            Response.Write("<td>");
            Response.Write(dr2["FLDBUGID"].ToString());
            Response.Write("</td>");
            Response.Write("<td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td colspan='2'>");
            Response.Write("<b>Posted by:</b> " + dr2["FLDREPORTEDBY"].ToString() + " On " + dr2["FLDOPENDATE"].ToString() + " <b>Severity:</b> " + dr2["FLDSEVERITYNAME"] + " <b>Status:</b>" + dr2["FLDSTATUSNAME"]);
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td colspan='2'>");
            Response.Write("<b>Subject</b>");
            Response.Write(dr2["FLDSUBJECT"].ToString());
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Write("<hr/>");
        }
        Response.Write("</td>");
        Response.Write("</tr>");
        Response.Write(" </table>");
        Response.ContentType = "application/msword";
        Response.AddHeader("Content-Disposition", "attachment; filename=SEPReleaseNotes.doc");
        Response.End();
    }
}
