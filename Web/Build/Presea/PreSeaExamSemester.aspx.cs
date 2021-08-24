using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;


public partial class PreSeaExamSemester : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../PreSea/PreSeaExamSemester.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvPreSeaSemesterExam')", "Print Grid", "icon_print.png", "PRINT");
                toolbar.AddImageButton("../PreSea/PreSeaExamSemester.aspx", "Find", "search.png", "FIND");
                MenuPreSeaSemesterExam.AccessRights = this.ViewState;
                MenuPreSeaSemesterExam.MenuList = toolbar.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDPRESEACOURSENAME", "FLDSEMESTERNAME", "FLDEXAMNAME", "FLDEXAMDATE" };
        string[] alCaptions = { "Course Name", "Semester Name", "Exam Name", "Exam Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPreSeaSemesterExam.PreSeaSemesterExamSearch(General.GetNullableInteger(ddlSemester.SelectedSemester)
          , General.GetNullableInteger(ddlCourse.SelectedCourse)
          , General.GetNullableInteger(ddlExam.SelectedExam)
          , sortexpression
          , sortdirection
          , (int)ViewState["PAGENUMBER"]
          , General.ShowRecords(null)
          , ref iRowCount
          , ref iTotalPageCount
          );

        Response.AddHeader("Content-Disposition", "attachment; filename=DesignationinvoiceStatus.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Country Register</h3></td>");
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
        foreach (DataRow dr in ds.Tables[0].Rows)
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

        string[] alColumns = { "FLDPRESEACOURSENAME", "FLDSEMESTERNAME", "FLDSEMESTERCODE", "FLDNOOFWEEKS", "FLDORDERSEQUENCE" };
        string[] alCaptions = { "Course Name", "Semester Name", "Semester Code", "No. Of Weeks", "Order Sequence" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixPreSeaSemesterExam.PreSeaSemesterExamSearch(General.GetNullableInteger(ddlSemester.SelectedSemester)
          , General.GetNullableInteger(ddlCourse.SelectedCourse)
          , null
          , sortexpression
          , sortdirection
          , (int)ViewState["PAGENUMBER"]
          , General.ShowRecords(null)
          , ref iRowCount
          , ref iTotalPageCount
          );

        General.SetPrintOptions("gvPreSeaSemesterExam", "Presea Semester Exam", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPreSeaSemesterExam.DataSource = ds;
            gvPreSeaSemesterExam.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPreSeaSemesterExam);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void PreSeaSemesterExam_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvPreSeaSemesterExam.EditIndex = -1;
                gvPreSeaSemesterExam.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
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

    private void InsertPreSeaSemesterExam(int semesterid, int courseid, int examid, DateTime? examdate)
    {
        PhoenixPreSeaSemesterExam.InsertPreSeaSemesterExam(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            semesterid, courseid, examid, examdate);
    }

    private void UpdatePreSeaSemesterExam(int semesterexamid, int semesterid, int courseid, int examid, DateTime? examdate)
    {
        PhoenixPreSeaSemesterExam.UpdatePreSeaSemesterExam(PhoenixSecurityContext.CurrentSecurityContext.UserCode, semesterexamid,
            semesterid, courseid, examid, examdate);
    }

    private void deletePreSeaSemesterExam(int semesterexamid)
    {
        PhoenixPreSeaSemesterExam.DeletePreSeaSemesterExam(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            semesterexamid);
    }

    private bool IsValidPreSeaSemesterExam(string semesterid, string courseid, string examid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(semesterid) == null)
        {
            ucError.ErrorMessage = "Semester name is required.";
        }
        if (General.GetNullableInteger(courseid) == null)
        {
            ucError.ErrorMessage = "Course Name is required.";
        }
        if (General.GetNullableInteger(examid) == null)
        {
            ucError.ErrorMessage = "Exam Code is required.";
        }

        return (!ucError.IsError);
    }

    protected void gvPreSeaSemesterExam_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }
    protected void gvPreSeaSemesterExam_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPreSeaSemesterExam(
                     ((UserControlPreSeaSemester)_gridView.FooterRow.FindControl("ddlSemesterAdd")).SelectedSemester,
                     ((UserControlPreSeaCourse)_gridView.FooterRow.FindControl("ddlCourseAdd")).SelectedCourse,
                     ((UserControlPreSeaExam)_gridView.FooterRow.FindControl("ddlExamAdd")).SelectedExam
                     )
                    )
                {
                    ucError.Visible = true;
                    return;
                }
                InsertPreSeaSemesterExam(
                     int.Parse(((UserControlPreSeaSemester)_gridView.FooterRow.FindControl("ddlSemesterAdd")).SelectedSemester),
                     int.Parse(((UserControlPreSeaCourse)_gridView.FooterRow.FindControl("ddlCourseAdd")).SelectedCourse),
                     int.Parse(((UserControlPreSeaExam)_gridView.FooterRow.FindControl("ddlExamAdd")).SelectedExam),
                     General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("ucExamDateAdd")).Text)
                 );
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPreSeaSemesterExam(
                    ((UserControlPreSeaSemester)_gridView.Rows[nCurrentRow].FindControl("ddlSemesterEdit")).SelectedSemester,
                    ((UserControlPreSeaCourse)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit")).SelectedCourse,
                    ((UserControlPreSeaExam)_gridView.Rows[nCurrentRow].FindControl("ddlExamEdit")).SelectedExam
                    )
                   )
                {
                    ucError.Visible = true;
                    return;
                }
                UpdatePreSeaSemesterExam(
                    int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSemesterExamIdEdit")).Text),
                    int.Parse(((UserControlPreSeaSemester)_gridView.Rows[nCurrentRow].FindControl("ddlSemesterEdit")).SelectedSemester),
                    int.Parse(((UserControlPreSeaCourse)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit")).SelectedCourse),
                    int.Parse(((UserControlPreSeaExam)_gridView.Rows[nCurrentRow].FindControl("ddlExamEdit")).SelectedExam),
                   General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucExamDateEdit")).Text)
                 );
                _gridView.EditIndex = -1;
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                deletePreSeaSemesterExam(int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSemesterExamId")).Text));
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPreSeaSemesterExam_RowDataBound(object sender, GridViewRowEventArgs e)
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

            UserControlPreSeaCourse ucCourse = (UserControlPreSeaCourse)e.Row.FindControl("ddlCourseEdit");
            DataRowView drvCourse = (DataRowView)e.Row.DataItem;
            if (ucCourse != null)
            {
                ucCourse.SelectedCourse = drvCourse["FLDCOURSEID"].ToString();
            }

            UserControlPreSeaSemester ucSemester = (UserControlPreSeaSemester)e.Row.FindControl("ddlSemesterEdit");
            DataRowView drvSemester = (DataRowView)e.Row.DataItem;
            if (ucSemester != null)
            {
                ucSemester.SelectedSemester = drvSemester["FLDSEMESTERID"].ToString();
            }

            UserControlPreSeaExam ucExam = (UserControlPreSeaExam)e.Row.FindControl("ddlExamEdit");
            DataRowView drvExam = (DataRowView)e.Row.DataItem;
            if (ucExam != null)
            {
                ucExam.SelectedExam = drvExam["FLDEXAMID"].ToString();
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
    protected void gvPreSeaSemesterExam_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gvPreSeaSemesterExam_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
    }
    protected void gvPreSeaSemesterExam_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvPreSeaSemesterExam.EditIndex = -1;
        gvPreSeaSemesterExam.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvPreSeaSemesterExam.EditIndex = -1;
        gvPreSeaSemesterExam.SelectedIndex = -1;
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
        gvPreSeaSemesterExam.SelectedIndex = -1;
        gvPreSeaSemesterExam.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
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
}

