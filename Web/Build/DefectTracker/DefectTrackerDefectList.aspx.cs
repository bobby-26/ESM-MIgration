using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DefectTracker;
using SouthNests.Phoenix.Framework;

public partial class DefectTracker_DefectTrackerDefectList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvDefectList.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvDefectList.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarbuglist = new PhoenixToolbar();
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerDefectList.aspx", "Search", "search.png", "SEARCH");
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerDefectList.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerDefectList.aspx", "Clear Filter", "clear-filter.png", "RESET");
            MenuDefectTracker.AccessRights = this.ViewState;
            MenuDefectTracker.MenuList = toolbarbuglist.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
            BindData();
            SetPageNavigator();
            SEPStatus.ParentCode = "23";
            SEPSeverity.ParentCode = "19";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    protected void gvDefectList_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDefectList, "Select$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixDefectTracker.DefectList
          ( 
            General.GetNullableString(txtIDSearch.Text),
            General .GetNullableString (SEPSeverity .SelectedValue ),
            General .GetNullableString(ucSEPModule .SelectedValue ),
            General.GetNullableString(txtReportedBy.Text),
            General .GetNullableString(SEPStatus.SelectedValue ),
            General.GetNullableDateTime(ucFromOpenDate.Text),
            General.GetNullableDateTime(ucToOpenDate.Text),
            General.GetNullableDateTime(ucFromFixedDate.Text),
            General.GetNullableDateTime(ucToFixedDate.Text),
            General.GetNullableDateTime(ucFromClosedDate.Text),
            General.GetNullableDateTime(ucToClosedDate.Text),
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount
          );

        if (dt.Rows.Count > 0)
        {
            gvDefectList.DataSource = dt;
            gvDefectList.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvDefectList);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDBUGID", "FLDSEVERITYNAME", "FLDMODULENAME", "FLDUSERNAME", "FLDSTATUSNAME", "FLDOPENDATE", "FLDFIXEDDATE", "FLDREOPENDATE", "FLDCLOSEDDATE" };
        string[] alCaptions = { "ID", "Severity", "Module", "Reported By", "Status", "Opened Date", "Fixed Date", "Reopened Date", "Closed Date" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixDefectTracker.DefectList
          (
            General.GetNullableString(txtIDSearch.Text),
            General.GetNullableString(SEPSeverity.SelectedValue),
            General.GetNullableString(ucSEPModule.SelectedValue),
            General.GetNullableString(txtReportedBy.Text),
            General.GetNullableString(SEPStatus.SelectedValue),
            General.GetNullableDateTime(ucFromOpenDate.Text),
            General.GetNullableDateTime(ucToOpenDate.Text),
            General.GetNullableDateTime(ucFromFixedDate.Text),
            General.GetNullableDateTime(ucToFixedDate.Text),
            General.GetNullableDateTime(ucFromClosedDate.Text),
            General.GetNullableDateTime(ucToClosedDate.Text),
            (int)ViewState["PAGENUMBER"],
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount
          );

        string XlsPath = Server.MapPath(@"~/Attachments/SEPDefectList.xls");
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


        //Response.AddHeader("Content-Disposition", "attachment; filename=SEPDefectList.xls");
        //Response.ContentType = "application/vnd.msexcel";    
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
            ShowExcel();
        }

        if (dce.CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }

    private void ClearFilter()
    {
        ucSEPModule.SelectedValue = "";
        SEPStatus.SelectedValue = "";
        SEPSeverity.SelectedValue = "";
        txtIDSearch.Text = "";
        txtReportedBy.Text = "";
        ucFromOpenDate.Text = "";
        ucToOpenDate.Text = "";
        ucFromFixedDate.Text = "";
        ucToFixedDate.Text = "";
        ucFromClosedDate.Text = "";
        ucToClosedDate.Text = "";
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

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvDefectList.EditIndex = -1;
        gvDefectList.SelectedIndex = -1;
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
        gvDefectList.SelectedIndex = -1;
        gvDefectList.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }
}

