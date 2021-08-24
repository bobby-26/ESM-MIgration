using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaBatchAdmissionSemester : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../PreSea/PreSeaBatchAdmissionSemester.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvPreSeaSemester')", "Print Grid", "icon_print.png", "PRINT");
                MenuPreSeaSemester.AccessRights = this.ViewState;
                MenuPreSeaSemester.MenuList = toolbar.Show();

                PhoenixToolbar Maintoolbar = new PhoenixToolbar();
                Maintoolbar.AddButton("Course", "COURSE");
                Maintoolbar.AddButton("Batch", "BATCH");
                Maintoolbar.AddButton("Eligibility", "ELIGIBILITY");
                Maintoolbar.AddButton("Batch Contact", "CONTACT");
                Maintoolbar.AddButton("Fees", "FEES");
                Maintoolbar.AddButton("Semester", "SEMESTER");
                Maintoolbar.AddButton("Subjects", "SUBJECTS");
                Maintoolbar.AddButton("Exam", "EXAM");

                MenuCourseMaster.AccessRights = this.ViewState;
                MenuCourseMaster.MenuList = Maintoolbar.Show();

                MenuCourseMaster.SelectedMenuIndex = 5;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ucBatch.SelectedBatch = Filter.CurrentPreSeaCourseMasterBatchSelection;
                ucBatch.Enabled = false;
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

        ds = PhoenixPreSeaBatchAdmissionSemester.PreSeaBatchAdmissionSemesterSearch(null
            , General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterSelection)
            , General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterBatchSelection)
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=PreSeaBatchAdmissionSemester.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pre-Sea Semester</h3></td>");
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


        DataSet ds = PhoenixPreSeaBatchAdmissionSemester.PreSeaBatchAdmissionSemesterSearch(null
          , General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterSelection)
          , General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterBatchSelection)
          , sortexpression
          , sortdirection
          , (int)ViewState["PAGENUMBER"]
          , General.ShowRecords(null)
          , ref iRowCount
          , ref iTotalPageCount
          );

        General.SetPrintOptions("gvPreSeaSemester", "Pre-Sea Semester", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPreSeaSemester.DataSource = ds;
            gvPreSeaSemester.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPreSeaSemester);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void PreSeaSemester_TabStripCommand(object sender, EventArgs e)
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
                gvPreSeaSemester.EditIndex = -1;
                gvPreSeaSemester.SelectedIndex = -1;
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

    protected void CourseMaster_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("COURSE"))
            {
                Response.Redirect("../PreSea/PreSeaCourseMaster.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("ELIGIBILITY"))
            {
                Response.Redirect("../PreSea/PreSeaBatchEligiblityDetails.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../PreSea/PreSeaBatch.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("CONTACT"))
            {
                Response.Redirect("../PreSea/PreSeaBatchAdmissionContact.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SUBJECTS"))
            {
                Response.Redirect("../PreSea/PreSeaBatchAdmissionSubjects.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SEMESTER"))
            {
                Response.Redirect("../PreSea/PreSeaBatchAdmissionSemester.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("FEES"))
            {
                Response.Redirect("../PreSea/PreSeaBatchFees.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("EXAM"))
            {
                Response.Redirect("../PreSea/PreSeaBatchAdmissionExam.aspx");
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

    private void InsertPreSeaSemester(int courseid,int BatchId, string semestername, string semestercode, string noofweeks, string ordersequence)
    {
       PhoenixPreSeaBatchAdmissionSemester.InsertPreSeaBatchAdmissionSemester(PhoenixSecurityContext.CurrentSecurityContext.UserCode, courseid, BatchId,
            semestername, semestercode, Convert.ToInt16(noofweeks), General.GetNullableInteger(ordersequence));
    }

    private void UpdatePreSeaSemester(int courseid, int BatchId, int semesterid, string semestername, string semestercode, string noofweeks, string ordersequence)
    {
       PhoenixPreSeaBatchAdmissionSemester.UpdatePreSeaBatchAdmissionSemester(PhoenixSecurityContext.CurrentSecurityContext.UserCode, courseid, BatchId,
            semesterid, semestername, semestercode, Convert.ToInt16(noofweeks), General.GetNullableInteger(ordersequence));
    }

    private void deletePreSeaSemester(int semesterid)
    {
        PhoenixPreSeaBatchAdmissionSemester.DeletePreSeaBatchAdmissionSemester(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            semesterid);
    }

    private bool IsValidPreSeaSemester(string courseid, string BatchId, string semestername, string semestercode, string noofweeks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(courseid) == null)
        {
            ucError.ErrorMessage = "Course Name is required.";
        }
        if (General.GetNullableInteger(BatchId) == null)
        {
            ucError.ErrorMessage = "Batch Name is required";
        }
        if (semestername.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Semester Name is required.";
        }
        if (semestercode.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Semester Code is required.";
        }
        if (noofweeks.Trim().Equals(""))
        {
            ucError.ErrorMessage = "No of weeks is required.";
        }

        return (!ucError.IsError);
    }

    protected void gvPreSeaSemester_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }
    protected void gvPreSeaSemester_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPreSeaSemester(
                     Filter.CurrentPreSeaCourseMasterSelection,
                     Filter.CurrentPreSeaCourseMasterBatchSelection,
                     ((TextBox)_gridView.FooterRow.FindControl("txtSemesterNameAdd")).Text,
                     ((TextBox)_gridView.FooterRow.FindControl("txtSemesterCodeAdd")).Text,
                     ((TextBox)_gridView.FooterRow.FindControl("txtTotalWeeksAdd")).Text
                     )
                    )
                {
                    ucError.Visible = true;
                    return;
                }
                InsertPreSeaSemester(
                     int.Parse(Filter.CurrentPreSeaCourseMasterSelection),
                     int.Parse(Filter.CurrentPreSeaCourseMasterBatchSelection),
                     ((TextBox)_gridView.FooterRow.FindControl("txtSemesterNameAdd")).Text,
                     ((TextBox)_gridView.FooterRow.FindControl("txtSemesterCodeAdd")).Text,
                     ((TextBox)_gridView.FooterRow.FindControl("txtTotalWeeksAdd")).Text,
                     ((TextBox)_gridView.FooterRow.FindControl("txtOrderSequenceAdd")).Text
                 );
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidPreSeaSemester(
                    Filter.CurrentPreSeaCourseMasterSelection,
                    Filter.CurrentPreSeaCourseMasterBatchSelection,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSemesterNameEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSemesterCodeEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTotalWeeksEdit")).Text
                    )
                   )
                {
                    ucError.Visible = true;
                    return;
                }
                UpdatePreSeaSemester(
                    int.Parse(Filter.CurrentPreSeaCourseMasterSelection),
                    int.Parse(Filter.CurrentPreSeaCourseMasterBatchSelection),
                    int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSemesterIdEdit")).Text),
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSemesterNameEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSemesterCodeEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtTotalWeeksEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOrderSequenceEdit")).Text
                 );
                _gridView.EditIndex = -1;
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                deletePreSeaSemester(int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSemesterId")).Text));
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
    protected void gvPreSeaSemester_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void gvPreSeaSemester_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gvPreSeaSemester_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
    }
    protected void gvPreSeaSemester_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvPreSeaSemester.EditIndex = -1;
        gvPreSeaSemester.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvPreSeaSemester.EditIndex = -1;
        gvPreSeaSemester.SelectedIndex = -1;
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
        gvPreSeaSemester.SelectedIndex = -1;
        gvPreSeaSemester.EditIndex = -1;
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

