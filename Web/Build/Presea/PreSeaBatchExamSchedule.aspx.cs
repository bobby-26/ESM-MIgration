using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaBatchExamSchedule : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar MainToolbar = new PhoenixToolbar();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            PhoenixToolbar toolbarsub = new PhoenixToolbar();

            MainToolbar.AddButton("Batch", "BATCH");
            MainToolbar.AddButton("Weekly Planner", "WEEKLYPLAN");
            MainToolbar.AddButton("Buddy Planner", "BUDDYPLAN");
            MainToolbar.AddButton("Mentor Planner", "MENTORPLAN");
            MainToolbar.AddButton("Exam Planner", "EXAMPLAN");
            MainToolbar.AddButton("Exam Results", "EXAMRESULTS");
            MainToolbar.AddButton("Semester Planner", "SEMESTERPLAN");
            MainToolbar.AddButton("Semester Results", "SEMEXAMRESULTS");
            MenuBatchPlanner.AccessRights = this.ViewState;
            MenuBatchPlanner.MenuList = MainToolbar.Show();
            MenuBatchPlanner.SelectedMenuIndex = 4;

            toolbar.AddImageButton("../PreSea/PreSeaBatchExamSchedule.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvPreSeaExam')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../PreSea/PreSeaBatchExamSchedule.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../PreSea/PreSeaBatchExamSchedule.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuPreSeaExam.AccessRights = this.ViewState;
            MenuPreSeaExam.MenuList = toolbar.Show();

            toolbarsub.AddImageButton("../PreSea/PreSeaBatchExamSchedule.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarsub.AddImageLink("javascript:CallPrint('gvExamDetails')", "Print Grid", "icon_print.png", "PRINT");
            MenuPreSeaExamSub.AccessRights = this.ViewState;
            MenuPreSeaExamSub.MenuList = toolbarsub.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SEMESTERID"] = "";
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["EXAMSCHEDULEID"] = "";
                ViewState["EXAMNAME"] = "";
                ViewState["ISSEMESTEREXAM"] = "";

                if (Session["BATCHMANAGECOURSE"] != null)
                {
                    ddlCourse.SelectedCourse = Session["BATCHMANAGECOURSE"].ToString();
                    ddlCourse.Enabled = false;
                    ddlSemester.Course = Session["BATCHMANAGECOURSE"].ToString();
                }                   
                ucBatch.SelectedBatch = Filter.CurrentPreSeaBatchManagerSelection;
                ucBatch.Enabled = false;
            }
            BindData();
            BindExamDetails();

            SetPageNavigator();
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

        string[] alColumns = { "FLDROWNUMBER", "FLDEXAM", "FLDEXAMNAME", "FLDSEMESTERID", "FLDFROMDATE", "FLDTODATE", "FLDSECTIONNAME" };
        string[] alCaptions = { "S.No.", "Exam Name", "Exam Type", "Semester", "From Date", "To Date", "Section" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixPreSeaBatchExamSchedule.PreSeaBatchExamScheduleSearch(General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                 , General.GetNullableInteger(ucExam.SelectedExam)
                 , General.GetNullableInteger(ddlSemester.SelectedSemester)
                 , General.GetNullableDateTime(txtFromDate.Text)
                 , General.GetNullableDateTime(txtToDate.Text)
                 , General.GetNullableInteger(ucSection.SelectedValue)
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
            //if (!IsPostBack)
            //{
                if (General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString()) == null)
                    ViewState["EXAMSCHEDULEID"] = ds.Tables[0].Rows[0]["FLDEXAMSCHEDULEID"].ToString();

                ViewState["EXAMNAME"] = ds.Tables[0].Rows[0]["FLDEXAM"].ToString() + " - " + ds.Tables[0].Rows[0]["FLDSECTIONNAME"].ToString();
                ViewState["SEMESTERID"] = ds.Tables[0].Rows[0]["FLDSEMESTERID"].ToString();
                ViewState["ISSEMESTEREXAM"] = ds.Tables[0].Rows[0]["FLDISSEMESTEREXAM"].ToString();
            //}

            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPreSeaExam);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        General.SetPrintOptions("gvPreSeaExam", "Presea Batch Exam Schedule", alCaptions, alColumns, ds);
    }
   
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();


        string[] alColumns = { "FLDROWNUMBER", "FLDEXAM", "FLDEXAMNAME", "FLDSEMESTERID", "FLDFROMDATE", "FLDTODATE", "FLDSECTIONNAME" };
        string[] alCaptions = { "S.No.","Exam Name","Exam Type", "Semester", "From Date", "To Date","Section" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPreSeaBatchExamSchedule.PreSeaBatchExamScheduleSearch(General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
           , General.GetNullableInteger(ucExam.SelectedExam)
           , General.GetNullableInteger(ddlSemester.SelectedSemester)
           , General.GetNullableDateTime(txtFromDate.Text)
           , General.GetNullableDateTime(txtToDate.Text)
           , General.GetNullableInteger(ucSection.SelectedValue)
           , sortexpression
           , sortdirection
           , (int)ViewState["PAGENUMBER"]
           , General.ShowRecords(null)
           , ref iRowCount
           , ref iTotalPageCount
           );

        Response.AddHeader("Content-Disposition", "attachment; filename=BatchExamSchedule.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pre Sea Batch Exam</h3></td>");
        Response.Write("<tr><td>Batch: </td><td>" + ds.Tables[0].Rows[0]["FLDBATCH"].ToString() + "</td>");        
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
    protected void ShowExcelExam()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();


        string[] alColumns = { "FLDROWNUMBER", "FLDEXAMDATE", "FLDSTARTTIME", "FLDENDTIME", "FLDSUBJECTNAME", "FLDMAXMARKS", "FLDPASSMARKS", "FLDSUBTYPENAME", "FLDINVIGILATORSNAME", "FLDACTIVEYNNAME" };
        string[] alCaptions = { "S.No", "Exam Date", "Start Time", "Enad Time", "Subject", "Max Score", "Pass Score", "Type", "Invigilators", "Active YN" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

         ds = PhoenixPreSeaBatchExam.PreSeaBatchExamSearch(General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString())
            , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
            , null
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=BatchExamSchedule.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pre Sea Batch Exam</h3></td>");
        Response.Write("<tr><td>" + ViewState["EXAMNAME"].ToString() + "</td></tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        // Response.Write("<tr><td>"+ + "</td></tr>");        
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

    protected void  BatchPlanner_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("MENTORPLAN"))
            {
                Response.Redirect("../PreSea/PreSeaMentorPlanner.aspx?type=1");
            }
            if (dce.CommandName.ToUpper().Equals("BUDDYPLAN"))
            {
                Response.Redirect("../PreSea/PreSeaMentorPlanner.aspx?type=2");
            }
            if (dce.CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../PreSea/PreSeaBatchManager.aspx");
            }

            else
            {
                if (String.IsNullOrEmpty(Filter.CurrentPreSeaBatchManagerSelection))
                {
                    ucError.ErrorMessage = "Please select batch to view/modify plan details.";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    if (dce.CommandName.ToUpper().Equals("WEEKLYPLAN"))
                    {
                        Response.Redirect("../PreSea/PreSeaBatchWeeklyPlanner.aspx?TYPE=1");
                    }
                    else if (dce.CommandName.ToUpper().Equals("EXAMRESULTS"))
                    {
                        Response.Redirect("../PreSea/PreSeaBatchInternalExamResults.aspx");
                    }
                    else if (dce.CommandName.ToUpper().Equals("EXAMPLAN"))
                    {
                        Response.Redirect("../PreSea/PreSeaBatchExamSchedule.aspx");
                    }
                    else if (dce.CommandName.ToUpper().Equals("SEMESTERPLAN"))
                    {
                        Response.Redirect("../PreSea/PreSeaBatchSemesterPlanner.aspx");
                    }
                    else if (dce.CommandName.ToUpper().Equals("SEMEXAMRESULTS"))
                    {
                        Response.Redirect("../PreSea/PreSeaBatchSemesterExamResults.aspx");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
                ViewState["PAGENUMBER"] = 1;
                ViewState["EXAMSCHEDULEID"] = "";
                BindData();                
                SetPageNavigator();
                BindExamDetails();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                clearfilters();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void MenuPreSeaExamSub_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                 ShowExcelExam();
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

    protected void gvPreSeaExam_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string batchid = Filter.CurrentPreSeaBatchManagerSelection;
                string examtypeid = ((UserControlPreSeaExam)_gridView.FooterRow.FindControl("ucExamTypeAdd")).SelectedExam;
                string semesterid = ((UserControls_UserControlPreSeaCourseSemester)_gridView.FooterRow.FindControl("ucSemesterAdd")).SelectedSemester;
                string fromdate = ((UserControlDate)_gridView.FooterRow.FindControl("txtFromDateAdd")).Text;
                string todate = ((UserControlDate)_gridView.FooterRow.FindControl("txtToDateAdd")).Text;
                string semexamyn = "0";
                string sectionid = ((DropDownList)_gridView.FooterRow.FindControl("ucSectionAdd")).SelectedValue;

                if (!IsValidPreSeaBatchExamSchedule(batchid
                    , examtypeid
                    , semesterid
                    , fromdate
                    , todate
                    , semexamyn
                    , sectionid
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertPreSeaExamSchedule(int.Parse(batchid)
                    , int.Parse(examtypeid)
                    , int.Parse(semesterid)
                    , General.GetNullableDateTime(fromdate)
                    , General.GetNullableDateTime(todate)
                    , 0
                    , General.GetNullableInteger(sectionid)
                    );

                clearfilters();
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string scheduleid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblScheduleIdEdit")).Text;
                string batchid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchIdEdit")).Text;
                string examtypeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblExamTypeIdEdit")).Text;
                string semesterid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSemesterIdEdit")).Text;
                string fromdate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtFromDateEdit")).Text;
                string todate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtToDateEdit")).Text;
                string semesterexamyn = "0";
                string sectionid = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ucSectionEdit")).SelectedValue;

                if (!IsValidPreSeaBatchExamSchedule(batchid
                    , examtypeid
                    , semesterid
                    , fromdate
                    , todate
                    , semesterexamyn
                    , sectionid
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdatePreSeaExamSchedule(General.GetNullableGuid(scheduleid)
                    , General.GetNullableDateTime(fromdate)
                    , General.GetNullableDateTime(todate)
                    , General.GetNullableInteger(sectionid)
                    );

                _gridView.EditIndex = -1;
                BindData();
                BindExamDetails();
                SetPageNavigator();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string scheduleid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblScheduleId")).Text;
                
                DeletePreSeaExamSchedule(General.GetNullableGuid(scheduleid));

                _gridView.EditIndex = -1;

                BindData();
                BindExamDetails();
                SetPageNavigator();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void clearfilters()
    {
        ucExam.SelectedExam = "";
        ddlSemester.SelectedSemester = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
        ucSection.SelectedValue = "";

        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    private void InsertPreSeaExamSchedule(int batchid, int examid, int semesterid, DateTime? fromdate, DateTime? todate,int semexamyn,int? sectionid)
    {
        PhoenixPreSeaBatchExamSchedule.InsertPreSeaBatchExamSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , batchid
            , examid
            , semesterid
            , null
            , fromdate
            , todate
            , 0
            , sectionid
            );
    }

    private void UpdatePreSeaExamSchedule(Guid? scheduleid, DateTime? fromdate, DateTime? todate,int? sectionid)
    {
        PhoenixPreSeaBatchExamSchedule.UpdatePreSeaBatchExamSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , scheduleid
            , fromdate
            , todate
            , sectionid
            );
    }

    private void DeletePreSeaExamSchedule(Guid? scheduleid)
    {
        PhoenixPreSeaBatchExamSchedule.DeletePreSeaBatchExamSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , scheduleid);
    }

    private bool IsValidPreSeaBatchExamSchedule(string batchid, string examid, string semesterid, string fromdate, string todate,string semesterexamyn, string sectionid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(batchid) == null)
        {
            ucError.ErrorMessage = "Batch Name is required.";
        }
        if (General.GetNullableInteger(examid) == null)
        {
            ucError.ErrorMessage = "Exam is required.";
        }
        if (General.GetNullableInteger(semesterid) == null)
        {
            ucError.ErrorMessage = "Semester is required.";
        }
        if (General.GetNullableDateTime(fromdate) == null)
        {
            ucError.ErrorMessage = "Fromdate is required.";
        }
        if (General.GetNullableDateTime(todate) == null)
        {
            ucError.ErrorMessage = "Todate is required.";
        }
        if (General.GetNullableDateTime(fromdate) != null && General.GetNullableDateTime(todate) != null)
        {
            DateTime resultdate;
            if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(todate)) > 0)
                ucError.ErrorMessage = "From date should be greater or equal to to date";
        }
        if (General.GetNullableInteger(semesterexamyn) == 0)
        {
            if(General.GetNullableInteger(sectionid) == null)
                ucError.ErrorMessage = "Section is required.";
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

    protected void gvPreSeaExam_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
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
                DataRowView drv = (DataRowView)e.Row.DataItem;

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

                DropDownList ucSection = (DropDownList)e.Row.FindControl("ucSectionEdit");
                if (ucSection != null)
                {
                    if (drv["FLDISSEMESTEREXAM"].ToString() == "1")
                    {
                        ucSection.Visible = false;
                        Label lblSectionNameEdit = (Label)e.Row.FindControl("lblSectionEdit");
                        if (lblSectionNameEdit != null)
                            lblSectionNameEdit.Visible = true;                       
                    }
                    else
                    {
                        Label lblSectionEdit = (Label)e.Row.FindControl("lblSectionEdit");
                        ucSection.SelectedValue = lblSectionEdit.Text;
                        //ucSection.SectionList = PhoenixPreSeaTrainee.ListPreSeaTraineeSection(General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection));
                        //ucSection.DataBind();
                        //ucSection.SelectedSection = drv["FLDSECTIONID"].ToString();
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

                DataTable dt = new DataTable();
                DataSet ds = new DataSet();

                UserControlPreSeaExam ucExamTypeAdd = (UserControlPreSeaExam)e.Row.FindControl("ucExamTypeAdd");
                ucExamTypeAdd.ExamList = PhoenixPreSeaExam.ListPreSeaExam(null);
                ucExamTypeAdd.DataBind();

                UserControls_UserControlPreSeaCourseSemester ucSemester = (UserControls_UserControlPreSeaCourseSemester)e.Row.FindControl("ucSemesterAdd");
                if (ucSemester != null)
                {
                    ucSemester.Course = Session["BATCHMANAGECOURSE"].ToString();
                    ucSemester.bind();
                }
                //dt = PhoenixPreSeaBatchManager.ListBatchSemesters(int.Parse(Filter.CurrentPreSeaBatchManagerSelection), null);
                //ds.Tables.Add(dt.Copy());
                //ucSemester.SemesterList = ds;
                //ucSemester.Batch = Filter.CurrentPreSeaBatchManagerSelection;

                DropDownList ucSection = (DropDownList)e.Row.FindControl("ucSectionAdd");
                //ucSection.SectionList = PhoenixPreSeaTrainee.ListPreSeaTraineeSection(General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection));
                //ucSection.DataBind();
                //ucSection.Batch = Filter.CurrentPreSeaBatchManagerSelection;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
    
    protected void gvPreSeaExam_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = sender as GridView;
        gvPreSeaExam.SelectedIndex = e.NewSelectedIndex;

        string batchid = Filter.CurrentPreSeaBatchManagerSelection;
        string scheduleid = ((Label)_gridView.Rows[e.NewSelectedIndex].FindControl("lblScheduleId")).Text;
        string courseid = ((Label)_gridView.Rows[e.NewSelectedIndex].FindControl("lblPreSeaCourseId")).Text;
        string examname = ((LinkButton)_gridView.Rows[e.NewSelectedIndex].FindControl("lnkExamName")).Text;
        string semesterid = ((Label)_gridView.Rows[e.NewSelectedIndex].FindControl("lblSemesterId")).Text;
        string sectionname = ((Label)_gridView.Rows[e.NewSelectedIndex].FindControl("lblSectionName")).Text;
        string IsSemesterExam = "0";
        examname = examname + " - " + sectionname;

        ViewState["EXAMSCHEDULEID"] = scheduleid;
        ViewState["EXAMNAME"] = examname;
        ViewState["SEMESTERID"] = semesterid;
        ViewState["ISSEMESTEREXAM"] = IsSemesterExam;

        BindExamDetails();
    }
    protected void chkSemExamAdd_Changed(object sender, EventArgs e)
    {
        GridView gv = gvPreSeaExam;
        //CheckBox chkSemExamYN = (CheckBox)gv.FooterRow.FindControl("chkSemExamAdd");
        //UserControlPreSeaSection ucSection = (UserControlPreSeaSection)gv.FooterRow.FindControl("ucSectionAdd");
        //if (chkSemExamYN.Checked == true)
        //{
        //    ucSection.SelectedSection = "";
        //    ucSection.Enabled = false;
        //}
        //else
        //{
        //    ucSection.Enabled = true;
        //}
    }

    private void SetRowSelection()
    {
        gvPreSeaExam.SelectedIndex = -1;
        for (int i = 0; i < gvPreSeaExam.Rows.Count; i++)
        {
            if (gvPreSeaExam.DataKeys[i].Value.ToString().Equals(ViewState["EXAMSCHEDULEID"].ToString()))
            {
                gvPreSeaExam.SelectedIndex = i;

            }
        }
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

    //----------  exam details ---------------------

    private void BindExamDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDEXAMDATE", "FLDSTARTTIME", "FLDENDTIME", "FLDSUBJECTNAME", "FLDMAXMARKS", "FLDPASSMARKS", "FLDSUBTYPENAME", "FLDINVIGILATORSNAME", "FLDACTIVEYNNAME" };
        string[] alCaptions = { "S.No", "Exam Date", "Start Time", "Enad Time", "Subject", "Max Score", "Pass Score", "Type", "Invigilators", "Active YN" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixPreSeaBatchExam.PreSeaBatchExamSearch(General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString())
            , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
            , null
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount
            );


        if (ds.Tables[0].Rows.Count > 0)
        {
            gvExamDetails.DataSource = ds;
            gvExamDetails.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvExamDetails);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        General.SetPrintOptions("gvExamDetails", "Presea Batch Exam", alCaptions, alColumns, ds);
    }

    protected void gvExamDetails_RowDataBound(object sender, GridViewRowEventArgs e)
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
            DataRowView drv = (DataRowView)e.Row.DataItem;

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
            ImageButton bi = (ImageButton)e.Row.FindControl("cmdInvigilator");
            if (bi != null) bi.Visible = SessionUtil.CanAccess(this.ViewState, bi.CommandName);

            if (bi != null)
            {
                bi.Attributes.Add("onclick", "javascript:return Openpopup('PreSea','','PreSeaBatchExamInvigilator.aspx?batchexamid=" + drv["FLDBATCHEXAMID"].ToString() + "'); return false;");
            }

            HtmlImage img = (HtmlImage)e.Row.FindControl("imgInvList");
            img.Attributes.Add("onclick", "showMoreInformation(ev, 'PreSeaMoreInfoExamInvigilator.aspx?batchexamid=" + drv["FLDBATCHEXAMID"].ToString() + "')");

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            //DropDownList ddlSubject = (DropDownList)e.Row.FindControl("ddlBatchSubjectAdd");
            TextBox txtSubjectNameAdd = (TextBox)e.Row.FindControl("txtSubjectNameAdd");
            TextBox txtsubjectId = (TextBox)e.Row.FindControl("txtsubjectId");
            TextBox txtSubjectType = (TextBox)e.Row.FindControl("txtSubjectType");

            if (txtsubjectId != null)
                txtsubjectId.Attributes.Add("style", "visibility:hidden");
            if (txtSubjectType != null)
                txtSubjectType.Attributes.Add("style", "visibility:hidden");
            
            ImageButton imgSubjectAdd = (ImageButton)e.Row.FindControl("imgSubjectAdd");
            if (imgSubjectAdd != null)
            {
                imgSubjectAdd.Attributes.Add("onclick", "return showPickList('spnSubjectAdd', 'codehelp1', '', '../Common/CommonPickListPreSeaSubjects.aspx?SEMESTERID="+ddlSemester.SelectedSemester+"&COURSEID="+ Session["BATCHMANAGECOURSE"].ToString()+"', true);");
                imgSubjectAdd.Visible = SessionUtil.CanAccess(this.ViewState, imgSubjectAdd.CommandName);

            }

            //if (ddlSubject != null)
            //{

            //    ddlSubject.DataSource = PhoenixPreSeaBatchManager.BatchSubjectTreeList(PhoenixSecurityContext.CurrentSecurityContext.UserCode,General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
            //        , General.GetNullableInteger(ViewState["SEMESTERID"].ToString()));
            //    ddlSubject.DataTextField = "FLDSUBJECTNAME";
            //    ddlSubject.DataValueField = "FLDSUBJECTID";
            //    ddlSubject.DataBind();
            //    ddlSubject.Items.Insert(0, new ListItem("--Select--", "Dummy"));
            //}

            //get the marks automatically for semester
            if (ViewState["ISSEMESTEREXAM"].ToString() == "1")
            {
                UserControlNumber txtmax = (UserControlNumber)e.Row.FindControl("txtMaxMarkAdd");
                if (txtmax != null && ViewState["ISSEMESTEREXAM"].ToString() == "1")
                {
                    txtmax.CssClass = "readonlytextbox";
                }
                UserControlNumber txtpass = (UserControlNumber)e.Row.FindControl("txtPassMarkAdd");
                if (txtpass != null && ViewState["ISSEMESTEREXAM"].ToString() == "1")
                {
                    txtpass.CssClass = "readonlytextbox";
                }
            }
        }
    }

    protected void gvExamDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {


                string examdate = ((UserControlDate)_gridView.FooterRow.FindControl("txtExamDateAdd")).Text;
                string starttime = ((TextBox)_gridView.FooterRow.FindControl("txtStartTimeAdd")).Text;
                string endtime = ((TextBox)_gridView.FooterRow.FindControl("txtEndTimeAdd")).Text;
                string subjectid = ((TextBox)_gridView.FooterRow.FindControl("txtsubjectId")).Text;
                string maxmark = ((UserControlNumber)_gridView.FooterRow.FindControl("txtMaxMarkAdd")).Text;
                string passmark = ((UserControlNumber)_gridView.FooterRow.FindControl("txtPassMarkAdd")).Text;
                string activeyn = (((CheckBox)_gridView.FooterRow.FindControl("chkAciveAdd")).Checked ? "1" : "0");
                string scheduleid = ViewState["EXAMSCHEDULEID"].ToString();

                string examstarttime = (starttime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : starttime;
                string examendtime = (endtime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : endtime;


                if (!IsValidExamDetails(examdate
                    , examstarttime
                    , examendtime
                    , subjectid
                    , maxmark
                    , passmark
                    , scheduleid
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertPreSeaExam(new Guid(ViewState["EXAMSCHEDULEID"].ToString())
                , int.Parse(subjectid)
                , General.GetNullableDecimal(maxmark)
                , General.GetNullableDecimal(passmark)
                , int.Parse(activeyn)
                , DateTime.Parse(examdate)
                , DateTime.Parse(examdate + " " + starttime)
                , DateTime.Parse(examdate + " " + examendtime));

                BindData();
                BindExamDetails();
                SetPageNavigator();
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                string batchexamid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchExamIdEdit")).Text;
                string examdate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtExamDateEdit")).Text;
                string starttime = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtStartTimeEdit")).Text;
                string endtime = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEndTimeEdit")).Text;
                string subjectid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectIdEdit")).Text;
                string maxmark = ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtMaxMarkEdit")).Text;
                string passmark = ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtPassMarkEdit")).Text;
                string activeyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkAciveEdit")).Checked ? "1" : "0");
                string scheduleid = ViewState["EXAMSCHEDULEID"].ToString();
                string examstarttime = (starttime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : starttime;
                string examendtime = (endtime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : endtime;

                if (!IsValidExamDetails(examdate
                  , examstarttime
                  , examendtime
                  , subjectid
                  , maxmark
                  , passmark
                  , scheduleid
                  ))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdatePreSeaExam(General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString())
                    , int.Parse(batchexamid)
                    , decimal.Parse(maxmark)
                    , decimal.Parse(passmark)
                    , int.Parse(activeyn)
                    , DateTime.Parse(examdate)
                    , DateTime.Parse(examdate + " " + starttime)
                    , DateTime.Parse(examdate + " " + examendtime));

                _gridView.EditIndex = -1;
                BindData();
                BindExamDetails();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeletePreSeaExam(int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchExamId")).Text));
                _gridView.EditIndex = -1;
                BindData();
                BindExamDetails();
                SetPageNavigator();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvExamDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;    
        _gridView.SelectedIndex = e.NewEditIndex;

        BindExamDetails();
    }

    protected void gvExamDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }

    protected void gvExamDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void gvExamDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    private bool IsValidExamDetails(string examdate, string starttime, string endtime, string sujectid, string maxmark, string passmark, string scheduleid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(examdate) == null)
            ucError.ErrorMessage = "Exam date is required.";
        if (General.GetNullableString(starttime) == null)
            ucError.ErrorMessage = "Start time is required";
        else if (General.GetNullableString(endtime) == null)
            ucError.ErrorMessage = "End time is required";
        else
        {
            DateTime resultdate;
            if (General.GetNullableDateTime(examdate + " " + starttime) == null)
                ucError.ErrorMessage = "Start time is not valid time.";
            else if (General.GetNullableDateTime(examdate + " " + starttime) == null)
                ucError.ErrorMessage = "Start time is not valid time.";
            if (DateTime.TryParse(examdate + " " + starttime, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(examdate + " " + endtime)) > 0)
                ucError.ErrorMessage = "Start time should be earlier than end time";
        }
        if (General.GetNullableInteger(sujectid) == null)
        {
            ucError.ErrorMessage = "Subject is required.";
        }
        if (General.GetNullableDecimal(maxmark) == null && ViewState["ISSEMESTEREXAM"].ToString() != "1")
        {
            ucError.ErrorMessage = "Max mark is required.";
        }
        if (General.GetNullableDecimal(passmark) == null && ViewState["ISSEMESTEREXAM"].ToString() != "1")
        {
            ucError.ErrorMessage = "Pass mark is required.";
        }
        else if ((General.GetNullableDecimal(maxmark) < General.GetNullableDecimal(passmark)) && ViewState["ISSEMESTEREXAM"].ToString() != "1")
        {
            ucError.ErrorMessage = "Pass mark should be less than or equal to max mark.";
        }
        if (General.GetNullableGuid(scheduleid) == null)
        {
            ucError.ErrorMessage = "Please Select the Exam.";
        }

        return (!ucError.IsError);
    }

    private void InsertPreSeaExam(Guid scheduleid, int subjectid, decimal? maxmarks, decimal? passmarks, int activeyn, DateTime examdate, DateTime starttime, DateTime endtime)
    {
        PhoenixPreSeaBatchExam.InsertPreSeaBatchExam(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , scheduleid
            , subjectid
            , null
            , maxmarks
            , passmarks
            , activeyn
            , examdate
            , starttime
            , endtime
            );
    }

    private void UpdatePreSeaExam(Guid? scheduleid, int batchexamid, decimal maxmarks, decimal passmarks, int activeyn, DateTime examdate, DateTime starttime, DateTime endtime)
    {
        PhoenixPreSeaBatchExam.UpdatePreSeaBatchExam(PhoenixSecurityContext.CurrentSecurityContext.UserCode
          , scheduleid
          , batchexamid
          , maxmarks
          , passmarks
          , activeyn
          , examdate
          , starttime
          , endtime
          );
    }

    private void DeletePreSeaExam(int batchidexamid)
    {
        PhoenixPreSeaBatchExam.DeletePreSeaBatchExam(PhoenixSecurityContext.CurrentSecurityContext.UserCode, batchidexamid);
    }


    //----------------------------------------------
}

