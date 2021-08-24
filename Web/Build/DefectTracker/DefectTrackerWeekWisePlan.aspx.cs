using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DefectTracker;
using SouthNests.Phoenix.Framework;

public partial class DefectTrackerWeekWisePlan : PhoenixBasePage
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
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Previous", "PREV");
            toolbar.AddButton("Next", "NEXT");
            MenuWeeklyPlan.AccessRights = this.ViewState;
            MenuWeeklyPlan.MenuList = toolbar.Show();

            PhoenixToolbar toolbarbuglist = new PhoenixToolbar();
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerWeekWisePlan.aspx", "Search", "search.png", "SEARCH");
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerWeekWisePlan.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerWeekWisePlan.aspx", "Clear Filter", "clear-filter.png", "RESET");
            MenuDefectTracker.AccessRights = this.ViewState;
            MenuDefectTracker.MenuList = toolbarbuglist.Show();

            if (!IsPostBack)
            {
                DateTime input = DateTime.Now;
                AssignBeginEnd(input, 0);
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void AssignBeginEnd(DateTime input, int week)
    {
        int mon = DayOfWeek.Monday - input.DayOfWeek;
        int sat = DayOfWeek.Saturday - input.DayOfWeek;
        DateTime monday = input.AddDays((week * 7) + mon);
        DateTime saturday = input.AddDays((week * 7) + sat);
        ucFromFinishDate.Text = monday.ToString();
        ucToFinishDate.Text = saturday.ToString();
    }
    protected void gvDeveloperPlann_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();

    }
    string developername = "";
    protected void gvDeveloperPlann_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
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
            Label lblAssignedTo = (Label)e.Row.FindControl("lblAssignedTo");
            if (lblAssignedTo.Text != developername)
            {
                developername = lblAssignedTo.Text;
                e.Row.Cells[0].Font.Bold = true;
            }
            else
                lblAssignedTo.Visible = false;
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
                //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDeveloperPlan, "Select$" + e.Row.RowIndex.ToString(), false);
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
            BindData();
        }

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

        if (dce.CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }
    protected void ShowExcel()
    {
        string[] alColumns = { "FLDASSIGNEDTO", "FLDBUGID", "FLDOPENDATE", "FLDMODULENAME", "FLDSUBJECT", "FLDSTATUSNAME", "FLDSTARTDATE", "FLDFINISHDATE", "FLDACTUALFINISHDATE", "FLDUSERNAME", "FLDPLANNEDEFFORT", "FLDACTUALEFFORT" };
        string[] alCaptions = { "Assigned To", "Bug Id", "Open Date", "Module Name", "Subject", "Status", "Start Date", "Finish Date", "Actual Finish", "Reported By", "Planned Effort", "Actual Effort" };

        DataTable dt = PhoenixDefectTracker.WeekWisePlan
          (
            General.GetNullableDateTime(ucFromFinishDate.Text),
            General.GetNullableDateTime(ucToFinishDate.Text)
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
        string developername = "";
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                if (alCaptions[i] == "Assigned To")
                {
                    if (developername == dr[alColumns[i]].ToString())
                        Response.Write("");
                    else
                        Response.Write(dr[alColumns[i]]);
                    developername = dr[alColumns[i]].ToString();
                }
                else
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
        DataTable dt = PhoenixDefectTracker.WeekWisePlan
          (
            General.GetNullableDateTime(ucFromFinishDate.Text),
            General.GetNullableDateTime(ucToFinishDate.Text)
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
    }
    private void ClearFilter()
    {
        AssignBeginEnd(DateTime.Now, 0);
        BindData();
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
    protected void MenuWeeklyPlan_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("PREV"))
        {
            AssignBeginEnd(DateTime.Parse(ucFromFinishDate.Text), -1);
        }
        if (dce.CommandName.ToUpper().Equals("NEXT"))
        {
            AssignBeginEnd(DateTime.Parse(ucFromFinishDate.Text), 1);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
}
