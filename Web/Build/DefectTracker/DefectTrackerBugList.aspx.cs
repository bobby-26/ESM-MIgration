using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using System.Data;

public partial class DefectTrackerBugList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            if (!IsPostBack)
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
                criteria.Add("SORTEXPRESSION", ViewState["SORTEXPRESSION"] == null ? "" : ViewState["SORTEXPRESSION"].ToString());
                criteria.Add("SORTDIRECTION", ViewState["SORTDIRECTION"] == null ? "" : ViewState["SORTDIRECTION"].ToString());
                BindProjectList();
                Filter.CurrentBugStatusFilter = criteria;
                BindVesselList();
                BindDefaultVessel();
                BindMilestone();
                BindFlag();
                ddlProject.SelectedValue = "24";
                string usercode = PhoenixSecurityContext.CurrentSecurityContext.UserName;
                if (usercode.Contains("808"))
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

            ViewState["PROJECTNAME"] = ddlProject.SelectedValue;
            lblsep.Text = "PROJECT - " + ddlProject.SelectedItem.Text.ToUpper();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbarbuglist = new PhoenixToolbar();
            toolbarbuglist.AddImageLink("javascript:Openpopup('Filter','','DefectTrackerBugAdd.aspx?projectname=" + ViewState["PROJECTNAME"].ToString() + "')", "Add", "add.png", "ADDDEFECT");
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerBugList.aspx", "Search", "search.png", "SEARCH");
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerBugList.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerBugList.aspx", "Clear Filter", "clear-filter.png", "RESET");
            MenuDefectTracker.AccessRights = this.ViewState;
            MenuDefectTracker.MenuList = toolbarbuglist.Show();

            if (Filter.CurrentBugStatusFilter != null)
            {
                NameValueCollection nvc = Filter.CurrentBugStatusFilter;

                ViewState["PAGENUMBER"] = General.GetNullableInteger(nvc.Get("PAGENUMBER").ToString());
                ViewState["SORTEXPRESSION"] = nvc.Get("SORTEXPRESSION").ToString();
                ViewState["SORTDIRECTION"] = General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString());
            }
            else
            {
                NameValueCollection criteria = new NameValueCollection();

                criteria.Clear();
                criteria.Add("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
                criteria.Add("SORTEXPRESSION", ViewState["SORTEXPRESSION"] == null ? "" : ViewState["SORTEXPRESSION"].ToString());
                criteria.Add("SORTDIRECTION", ViewState["SORTDIRECTION"] == null ? "" : ViewState["SORTDIRECTION"].ToString());

                Filter.CurrentBugStatusFilter = criteria;
            }

            SEPStatus.ParentCode = "23";
            ucSEPType.ParentCode = "9";
            SEPSeverity.ParentCode = "19";
            SEPPriority.ParentCode = "15";

            BindData();
            SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    private void BindVesselList()
    {
        if (ddlProject.SelectedValue.Trim() != string.Empty)
        {
            DataTable dt = PhoenixDefectTracker.vessellist(int.Parse(ddlProject.SelectedValue));
            if (dt.Rows.Count > 0)
            {
                ddlvesselcode.DataSource = dt;
                ddlvesselcode.DataValueField = "FLDVESSELID";
                ddlvesselcode.DataTextField = "FLDVESSELNAME";
                ddlvesselcode.DataBind();
                ddlvesselcode.Items.Insert(0, new ListItem("--SELECT--", ""));
            }
        }
    }

    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ucStatus.Text = ddlProject.SelectedItem.Text + " Issue Tracker";
            lblsep.Text = "PROJECT - " + ddlProject.SelectedItem.Text.ToUpper();
            BindVesselList();
            BindData();
            ViewState["PROJECTNAME"] = ddlProject.SelectedValue;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdThisUser_Click(object sender, EventArgs e)
    {
        txtReportedBy.Text = PhoenixSecurityContext.CurrentSecurityContext.UserName.ToString();
        BindData();
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
                ddlvesselcode.SelectedValue = dr["FLDINSTALLCODE"].ToString();
                ddlvesselcode.Enabled = false;
            }
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
            string usercode = PhoenixSecurityContext.CurrentSecurityContext.UserName;
            if (!usercode.Contains("808"))
                ShowExcel();
            else
                ShowDevExcel();
        }

        if (dce.CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        NameValueCollection nvc = Filter.CurrentBugStatusFilter;

        try
        {
            ViewState["PAGENUMBER"] = int.Parse(nvc.Get("PAGENUMBER"));
            ViewState["SORTEXPRESSION"] = General.GetNullableInteger(nvc.Get("SORTEXPRESSION").ToString());
            ViewState["SORTDIRECTION"] = General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString());
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearFilter()
    {
        ucSEPModule.SelectedValue = "";
        SEPStatus.SelectedValue = "";
        SEPSeverity.SelectedValue = "";
        ucSEPType.SelectedValue = "";
        SEPPriority.SelectedValue = "";
        SEPSeverity.SelectedValue = "";
        txtDescriptionSearch.Text = "";
        txtIDSearch.Text = "";
        txtSubjectSearch.Text = "";
        ucFromDate.Text = "";
        ucToDate.Text = "";
        txtnopage.Text = "";
        ViewState["PAGENUMBER"] = 1;
        ddlmilestone.SelectedValue = "";
        ddlissueflag.SelectedValue = "";
        NameValueCollection criteria = new NameValueCollection();

        criteria.Clear();
        criteria.Add("PAGENUMBER", "1");
        criteria.Add("SORTEXPRESSION", "");
        criteria.Add("SORTDIRECTION", "");

        Filter.CurrentBugStatusFilter = criteria;
        BindData();
        SetPageNavigator();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        SetPageNavigator();
    }
    protected void Filter_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        ViewState["SORTEXPRESSION"] = null;
        ViewState["SORTDIRECTION"] = null;
        BindData();
        SetPageNavigator();
        Filter.CurrentBugStatusFilter.Set("PAGENUMBER", ViewState["PAGENUMBER"].ToString());
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDBUGID", "FLDTYPE", "FLDPROJECT", "FLDMODULE", "FLDSUBJECT", "FLDPRIORITY", "FLDREPORTEDBY" };
        string[] alCaptions = { "Bug Id", "Type", "Project", "Module", "Subject", "Priority", "Reported By" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if ((int)ViewState["PAGENUMBER"] > 0)
            Filter.CurrentBugStatusFilter.Set("PAGENUMBER", ViewState["PAGENUMBER"].ToString());

        NameValueCollection nvc = Filter.CurrentBugStatusFilter;

        DataSet ds = PhoenixDefectTracker.BugList(
            General.GetNullableString(txtReportedBy.Text),
            General.GetNullableString((ucDeveloperName.SelectedTeamMemberName == "--Select--") ? "" : (ucDeveloperName.SelectedTeamMemberName)),
            General.GetNullableString(ucSEPModule.SelectedValue),
            General.GetNullableString(SEPStatus.SelectedValue),
            General.GetNullableString(SEPPriority.SelectedValue),
            General.GetNullableString(SEPSeverity.SelectedValue),
            General.GetNullableInteger(ddlvesselcode.SelectedValue),
            General.GetNullableString(ucSEPType.SelectedValue),
            General.GetNullableString(txtIDSearch.Text),
            General.GetNullableString(txtSubjectSearch.Text),
            General.GetNullableString(txtDescriptionSearch.Text),
            General.GetNullableDateTime(ucFromDate.Text),
            General.GetNullableDateTime(ucToDate.Text),
            ddlProject.SelectedValue,
            General.GetNullableInteger(ddlmilestone.SelectedValue),
            General.GetNullableInteger(ddlissueflag.SelectedValue),
            General.GetNullableString(nvc.Get("SORTEXPRESSION").ToString()),
            General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString()),
            int.Parse(nvc.Get("PAGENUMBER").ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            SEPBugSearchGrid.DataSource = ds;
            SEPBugSearchGrid.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, SEPBugSearchGrid);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        string[] alColumns = { "FLDBUGID", "FLDOPENDATE", "FLDType", "FLDMODULENAME", "FLDSUBJECT", "FLDDESCRIPTION", "FLDPRIORITY", "FLDSEVERITYNAME", "FLDSTATUSNAME", "FLDREPORTEDBY" };
        string[] alCaptions = { "ID", "Logged On", "Type", "Module", "Subject", "Description", "Priority", "Severity", "Status", "Reported By" };


        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixDefectTracker.BugList(
            General.GetNullableString(txtReportedBy.Text),
            General.GetNullableString(ucDeveloperName.SelectedTeamMemberName),
            General.GetNullableString(ucSEPModule.SelectedValue),
            General.GetNullableString(SEPStatus.SelectedValue),
            General.GetNullableString(SEPPriority.SelectedValue),
            General.GetNullableString(SEPSeverity.SelectedValue),
            General.GetNullableInteger(ddlvesselcode.SelectedValue),
            General.GetNullableString(ucSEPType.SelectedValue),
            General.GetNullableString(txtIDSearch.Text),
            General.GetNullableString(txtSubjectSearch.Text),
            General.GetNullableString(txtDescriptionSearch.Text),
            General.GetNullableDateTime(ucFromDate.Text),
            General.GetNullableDateTime(ucToDate.Text),
            ddlProject.SelectedValue,
            General.GetNullableInteger(ddlmilestone.SelectedValue),
            General.GetNullableInteger(ddlissueflag.SelectedValue),
            sortexpression,
            sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        DataTable dt = ds.Tables[0];

        Response.AddHeader("Content-Disposition", "attachment; filename=SEPIssueList.xls");
        Response.ContentType = "application/vnd.msexcel";
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

    protected void ShowDevExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        string usercode = PhoenixSecurityContext.CurrentSecurityContext.UserName;

        string[] alColumns = { "FLDBUGID", "FLDOPENDATE", "FLDType", "FLDMODULENAME", "FLDSUBJECT", "FLDMILESTONE", "FLDISSUEFLAG", "FLDDESCRIPTION", "FLDPRIORITY", "FLDSEVERITYNAME", "FLDSTATUSNAME", "FLDREPORTEDBY" };
        string[] alCaptions = { "ID", "Logged On", "Type", "Module", "Subject", "Milestone", "IssueFlag", "Description", "Priority", "Severity", "Status", "Reported By" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixDefectTracker.BugList(
            General.GetNullableString(txtReportedBy.Text),
            General.GetNullableString(ucDeveloperName.SelectedTeamMemberName),
            General.GetNullableString(ucSEPModule.SelectedValue),
            General.GetNullableString(SEPStatus.SelectedValue),
            General.GetNullableString(SEPPriority.SelectedValue),
            General.GetNullableString(SEPSeverity.SelectedValue),
            General.GetNullableInteger(ddlvesselcode.SelectedValue),
            General.GetNullableString(ucSEPType.SelectedValue),
            General.GetNullableString(txtIDSearch.Text),
            General.GetNullableString(txtSubjectSearch.Text),
            General.GetNullableString(txtDescriptionSearch.Text),
            General.GetNullableDateTime(ucFromDate.Text),
            General.GetNullableDateTime(ucToDate.Text),
            ddlProject.SelectedValue,
            General.GetNullableInteger(ddlmilestone.SelectedValue),
            General.GetNullableInteger(ddlissueflag.SelectedValue),
            sortexpression,
            sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        DataTable dt = ds.Tables[0];

        Response.AddHeader("Content-Disposition", "attachment; filename=SEPIssueList.xls");
        Response.ContentType = "application/vnd.msexcel";
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

    protected void SEPBugSearchGrid_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton anl = (ImageButton)e.Row.FindControl("cmdAnalysis");
            if (anl != null) anl.Visible = SessionUtil.CanAccess(this.ViewState, anl.CommandName);
            if (anl != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                anl.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugAnalysis.aspx?dtkey=" + lbl.Text + "'); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            if (eb != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                eb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugEdit.aspx?dtkey=" + lbl.Text + "'); return false;");
            }

            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAttachment");
            if (ab != null) ab.Visible = SessionUtil.CanAccess(this.ViewState, ab.CommandName);
            if (ab != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                ab.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugAttachment.aspx?dtkey=" + lbl.Text + "'); return false;");
            }

            ImageButton cmb = (ImageButton)e.Row.FindControl("cmdComments");
            if (cmb != null) cmb.Visible = SessionUtil.CanAccess(this.ViewState, cmb.CommandName);
            if (cmb != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                cmb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugComment.aspx?dtkey=" + lbl.Text + "'); return false;");
            }

            LinkButton lb = (LinkButton)e.Row.FindControl("lnkSubject");
            if (lb != null)
            {
                Label lbl = (Label)e.Row.FindControl("lblUniqueID");
                if (SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                    lb.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugEdit.aspx?dtkey=" + lbl.Text + "'); return false;");
            }

            LinkButton lbE = (LinkButton)e.Row.FindControl("lnkBugId");
            if (lbE != null)
            {
                Label lblE = (Label)e.Row.FindControl("lblUniqueID");
                if (SessionUtil.CanAccess(this.ViewState, lb.CommandName))
                    lbE.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerBugEdit.aspx?dtkey=" + lblE.Text + "'); return false;");
            }

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucDescription");
            lb.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
            lb.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");

            DataRowView drv = (DataRowView)e.Row.DataItem;
            ImageButton iab = (ImageButton)e.Row.FindControl("cmdAttachment");
            ImageButton inab = (ImageButton)e.Row.FindControl("cmdNoAttachment");
            if (iab != null) iab.Visible = true;
            if (inab != null) inab.Visible = false;
            int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());
            if (n == 0)
            {
                if (iab != null) iab.Visible = false;
                if (inab != null) inab.Visible = true;
            }
        }

    }
    protected void SEPBugSearchGrid_Sorting(object sender, GridViewSortEventArgs se)
    {
        SEPBugSearchGrid.EditIndex = -1;
        SEPBugSearchGrid.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        SEPBugSearchGrid.EditIndex = -1;
        SEPBugSearchGrid.SelectedIndex = -1;
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
        SEPBugSearchGrid.SelectedIndex = -1;
        SEPBugSearchGrid.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        Filter.CurrentBugStatusFilter.Set("PAGENUMBER", ViewState["PAGENUMBER"].ToString());

        BindData();
        SetPageNavigator();
    }
    protected void SEPBugSearchGrid_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();

    }
    protected void SEPBugSearchGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            Label lbldtkey = (Label)_gridView.Rows[nCurrentRow].FindControl("lblUniqueID");
            if (e.CommandName.ToUpper().Equals("DEFECTCLOSE"))
            {

                PhoenixDefectTracker.BulkDefectClosure(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(lbldtkey.Text));

                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
