using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaCourseExam : PhoenixBasePage
{
    string courceid;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            courceid = Filter.CurrentPreSeaCourseMasterSelection;
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../PreSea/PreSeaCourseExam.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvPreSeaExam')", "Print Grid", "icon_print.png", "PRINT");
                toolbar.AddImageButton("../PreSea/PreSeaCourseExam.aspx", "Find", "search.png", "FIND");
                MenuPreSeaExam.AccessRights = this.ViewState;
                MenuPreSeaExam.MenuList = toolbar.Show();


                PhoenixToolbar Maintoolbar = new PhoenixToolbar();
                Maintoolbar.AddButton("Course", "COURSE");
                Maintoolbar.AddButton("Eligibility", "ELIGIBILITY");
                Maintoolbar.AddButton("Batch", "BATCH");
                Maintoolbar.AddButton("Course Contact", "COURSECONTACT");
                Maintoolbar.AddButton("Fees", "FEES");      
                Maintoolbar.AddButton("Semester", "SEMESTER");                          
                Maintoolbar.AddButton("Subjects", "SUBJECTS");
                Maintoolbar.AddButton("Exam", "EXAM");

                MenuCourseMaster.AccessRights = this.ViewState;
                MenuCourseMaster.MenuList = Maintoolbar.Show();

                MenuCourseMaster.SelectedMenuIndex = 7;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();


        string[] alColumns = { "FLDPRESEACOURSENAME", "FLDEXAMNAME", "FLDSEMESTERNAME", "FLDSUBJECTNAME", "FLDSUBJECTTYPE", "FLDMAXMARKS", "FLDPASSMARKS", "FLDISACTIVEYN" };
        string[] alCaptions = { "Course Name", "Exam Name", "Semester", "Subject", "Type", "Max. marks", "Pass Marks", "Active YN" };
            
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPreSeaCourseExam.PreSeaCourseExamSearch(General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterSelection)
           , General.GetNullableInteger(ucExam.SelectedExam)           
           , sortexpression
           , sortdirection
           , (int)ViewState["PAGENUMBER"]
           , General.ShowRecords(null)
           , ref iRowCount
           , ref iTotalPageCount
           );

        Response.AddHeader("Content-Disposition", "attachment; filename=PreSeaCourseExam.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pre Sea Course Exam</h3></td>");
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
                Response.Redirect("../PreSea/PreSeaCourseEligibiltyDetails.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../PreSea/PreSeaBatch.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("COURSECONTACT"))
            {
                Response.Redirect("../PreSea/PreSeaCourseContact.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SUBJECTS"))
            {
                Response.Redirect("../PreSea/PreSeaCourseSubjects.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SEMESTER"))
            {
                Response.Redirect("../PreSea/PreSeaSemester.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("FEES"))
            {
                Response.Redirect("../PreSea/PreSeaCourseFees.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("EXAM"))
            {
                Response.Redirect("../PreSea/PreSeaCourseExam.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDPRESEACOURSENAME", "FLDEXAMNAME", "FLDSEMESTERNAME", "FLDSUBJECTNAME", "FLDSUBJECTTYPE", "FLDMAXMARKS", "FLDPASSMARKS", "FLDISACTIVEYN" };
        string[] alCaptions = { "Course Name", "Exam Name","Semester","Subject","Type","Max. marks","Pass Marks","Active YN"};

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixPreSeaCourseExam.PreSeaCourseExamSearch(General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterSelection)
                  , General.GetNullableInteger(ucExam.SelectedExam)
                  , sortexpression
                  , sortdirection
                  , (int)ViewState["PAGENUMBER"]
                  , General.ShowRecords(null)
                  , ref iRowCount
                  , ref iTotalPageCount
                  );

      
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPreSeaExam.DataSource = ds;
            gvPreSeaExam.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPreSeaExam);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        General.SetPrintOptions("gvPreSeaExam", "Presea Course Exam", alCaptions, alColumns, ds);
    }

    protected void PreSeaExam_TabStripCommand(object sender, EventArgs e)
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
                gvPreSeaExam.EditIndex = -1;
                gvPreSeaExam.SelectedIndex = -1;
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

    private void InsertPreSeaExam(int courseid, int examid, int semesterid, int subjectid, int? type,decimal maxmarks,decimal passmarks,int activeyn)
    {
        PhoenixPreSeaCourseExam.InsertPreSeaCourseExam(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , courseid
            , examid
            , semesterid
            , subjectid
            , type
            , maxmarks
            , passmarks
            , activeyn);
    }

    private void UpdatePreSeaExam(int courseexamid,int examid, int semesterid, int subjectid, int? type, decimal maxmarks, decimal passmarks, int activeyn)
    {
        PhoenixPreSeaCourseExam.UpdatePreSeaCourseExam(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , courseexamid
            , examid
            , semesterid
            , subjectid
            , type
            , maxmarks
            , passmarks
            , activeyn);
    }

    private void DeletePreSeaExam(int courseexamid)
    {
        PhoenixPreSeaCourseExam.DeletePreSeaCourseExam(PhoenixSecurityContext.CurrentSecurityContext.UserCode, courseexamid);
    }

    private bool IsValidPreSeaCourseExam(string courseid, string examid, string semesterid,string sujectid,string maxmark,string passmark)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(courseid) == null)
        {
            ucError.ErrorMessage = "Course Name is required.";
        }
        if (General.GetNullableInteger(examid) == null)
        {
            ucError.ErrorMessage = "Exam is required.";
        }
        if (General.GetNullableInteger(semesterid) == null)
        {
            ucError.ErrorMessage = "Semester is required.";
        }
        if (General.GetNullableInteger(sujectid) == null)
        {
            ucError.ErrorMessage = "Subject is required.";
        }
        if (General.GetNullableDecimal(maxmark) == null)
        {
            ucError.ErrorMessage = "Max mark is required.";
        }
        if (General.GetNullableDecimal(passmark) == null)
        {
            ucError.ErrorMessage = "Pass mark is required.";
        }
        else if (General.GetNullableDecimal(maxmark) < General.GetNullableDecimal(passmark))
        {
            ucError.ErrorMessage = "Pass mark should be less than or equal to max mark.";
        }

        return (!ucError.IsError);
    }

    protected void gvPreSeaExam_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }

    protected void gvPreSeaExam_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string examid = ucExam.SelectedExam;
                string semesterid = ((UserControlPreSeaSemester)_gridView.FooterRow.FindControl("ucSemesterAdd")).SelectedSemester;
                string subjectid = ((UserControlPreSeaSubject)_gridView.FooterRow.FindControl("ucSubjectAdd")).SelectedSubject;
                string maxmark    = ((UserControlNumber)_gridView.FooterRow.FindControl("txtMaxMarkAdd")).Text;
                string passmark = ((UserControlNumber)_gridView.FooterRow.FindControl("txtPassMarkAdd")).Text;
                string activeyn = (((CheckBox)_gridView.FooterRow.FindControl("chkAciveAdd")).Checked ? "1" : "0");

                if (!IsValidPreSeaCourseExam(courceid
                    , examid
                    , semesterid
                    , subjectid
                    , maxmark
                    , passmark
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertPreSeaExam(int.Parse(courceid)
                    , int.Parse(examid)
                    , int.Parse(semesterid)
                    , int.Parse(subjectid)
                    , null
                    , decimal.Parse(maxmark)
                    , decimal.Parse(passmark)
                    , int.Parse(activeyn));

                BindData();
                SetPageNavigator();
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string courseexamid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseExamIdEdit")).Text;
                string examid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblExamIdEdit")).Text;
                string semesterid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectIdEdit")).Text;
                string subjectid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectIdEdit")).Text;
                string maxmark = ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtMaxMarkEdit")).Text;
                string passmark = ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtPassMarkEdit")).Text;
                string activeyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkAciveEdit")).Checked ? "1" : "0");

                if (!IsValidPreSeaCourseExam(courceid
                     , examid
                     , semesterid
                     , subjectid
                     , maxmark
                     , passmark
                     ))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdatePreSeaExam(int.Parse(courseexamid)
                    , int.Parse(examid)
                    , int.Parse(semesterid)
                    , int.Parse(subjectid)
                    , null
                    , decimal.Parse(maxmark)
                    , decimal.Parse(passmark)
                    , int.Parse(activeyn));

                _gridView.EditIndex = -1;
                BindData();                
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeletePreSeaExam(int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseExamId")).Text));
                _gridView.EditIndex = -1;
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

    protected void gvPreSeaExam_RowDataBound(object sender, GridViewRowEventArgs e)
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

            UserControlPreSeaSemester ucSem = (UserControlPreSeaSemester)e.Row.FindControl("ucSemesterAdd");
            if (ucSem != null)
            {
                DataSet ds = PhoenixPreSeaSemester.ListPreSeaSemester(General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterSelection), null);
                ucSem.Course = Filter.CurrentPreSeaCourseMasterSelection;
                ucSem.SemesterList = ds;
                ucSem.DataBind();
            }           
        }
    }

    protected void gvPreSeaExam_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gvPreSeaExam_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
    }

    protected void gvPreSeaExam_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }
    protected void ddlCourseSemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    ListItem li = new ListItem("--Select--", "DUMMY");

        //    GridView _gridView = gvPreSeaExam;

        //    DropDownList ddlCourseSemester = (DropDownList)_gridView.FooterRow.FindControl("ddlCourseSemesterAdd");
        //    DropDownList ddlCourseSubject = (DropDownList)_gridView.FooterRow.FindControl("ddlSubjectsAdd");

        //    ddlCourseSubject.Items.Clear();
        //    ddlCourseSubject.Items.Add(li);
        //    string semesterid = ddlCourseSemester.SelectedValue;

        //    DataSet dsSub = PhoenixPreSeaCourseSubjects.ListPreSeaCourseSubject(General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterSelection)
        //        , General.GetNullableInteger(semesterid));
        //    DataTable dt = dsSub.Tables[0];

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        DataRow dr = dt.Rows[i];
        //        li = new ListItem(dr["FLDSUBJECTNAME"].ToString(), dr["FLDSUBJECTID"].ToString());
        //        ddlCourseSubject.Items.Add(li);
        //    }
        //    ddlCourseSubject.DataBind();            
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvPreSeaExam.EditIndex = -1;
        gvPreSeaExam.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvPreSeaExam.EditIndex = -1;
        gvPreSeaExam.SelectedIndex = -1;
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
        gvPreSeaExam.SelectedIndex = -1;
        gvPreSeaExam.EditIndex = -1;
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

    protected void ucSemesterAdd_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridView gv = gvPreSeaExam;

            UserControlPreSeaSemester ucSem = (UserControlPreSeaSemester)gv.FooterRow.FindControl("ucSemesterAdd");
            UserControlPreSeaSubject  ucSub = (UserControlPreSeaSubject)gv.FooterRow.FindControl("ucSubjectAdd");

            string sem = ucSem.SelectedSemester;
            ucSub.Enabled = true;

            if (General.GetNullableInteger(sem) != null)
            {
                DataSet ds = PhoenixPreSeaCourseSubjects.ListPreSeaCourseSubject(General.GetNullableInteger(Filter.CurrentPreSeaCourseMasterSelection),General.GetNullableInteger(sem));
                ucSub.SubjectList = ds;
                ucSub.DataBind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

