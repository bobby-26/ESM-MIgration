using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Collections.Specialized;

public partial class PreSeaBatchSemesterPlanner : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar MainToolbar = new PhoenixToolbar();

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
            MenuBatchPlanner.SelectedMenuIndex = 6;

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../PreSea/PreSeaBatchSemesterPlanner.aspx", "Export to Excel", "icon_xls.png", "Excel");
            MenuPreSea.AccessRights = this.ViewState;
            MenuPreSea.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["BATCH"] = "";                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["SEMESTERPLANID"] = "";
                if (Session["BATCHMANAGECOURSE"] != null && Session["BATCHMANAGECOURSE"].ToString() != "")
                {
                    ddlCourse.SelectedCourse = Session["BATCHMANAGECOURSE"].ToString();
                    ddlCourse.Enabled = false;
                    ddlSemester.Course = Session["BATCHMANAGECOURSE"].ToString();
                }               
            }

            BindData();
            //BindWeeklyPlan();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }   
   
    protected void BatchPlanner_TabStripCommand(object sender, EventArgs e)
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
    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        try
        {
            int iRowCount = 0;

            string[] alColumns = { "FLDSEMESTERID", "FLDSUBJECTNAME", "FLDDATE", "FLDFROMTIME", "FLDTOTIME", "FLDINVIGILATORSNAME" };
            string[] alCaptions = { "Semester", "Subject", "Date", "Start Time", "End Time", "Invigilators" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = new DataSet();

            ds = PhoenixPreSeaWeeklyPlanner.ListSemesterTimeTable(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                  , General.GetNullableInteger(Session["BATCHMANAGECOURSE"].ToString())
                  , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                  , General.GetNullableInteger(ddlYear.SelectedQuick)
                  , General.GetNullableInteger(ddlSemester.SelectedSemester)
                  );


            //     General.ShowExcel("Time Slot Details", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            Response.AddHeader("Content-Disposition", "attachment; filename=SemesterPlan.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/sims1.png" + "' /></td>");
            Response.Write("<td><h3>Semester Plan</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td> <b> Course </b></td> <td>" + ddlCourse.SelectedName + "</td>");
            Response.Write("<td></td>");
            Response.Write("<td> <b> Year </b> </td> <td>" + ddlYear.SelectedText + "</td>");
            Response.Write("<td></td><td><b> Semester <b> </td><td>" + ddlSemester.SelectedSemester + "</td></tr>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSEMESTERID", "FLDSUBJECTNAME", "FLDDATE", "FLDFROMTIME", "FLDTOTIME", "FLDINVIGILATORSNAME" };
            string[] alCaptions = { "Semester", "Subject", "Date","Start Time","End Time","Invigilators" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = new DataSet();

            ds = PhoenixPreSeaWeeklyPlanner.ListSemesterTimeTable(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableInteger(Session["BATCHMANAGECOURSE"].ToString())
                , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                , General.GetNullableInteger(ddlYear.SelectedQuick)
                , General.GetNullableInteger(ddlSemester.SelectedSemester)                
                );

            General.SetPrintOptions("gvPreSea", "Semester Plan Details", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPreSea.DataSource = ds;
                gvPreSea.DataBind();
                if (String.IsNullOrEmpty(ViewState["SEMESTERPLANID"].ToString()) || ViewState["SEMESTERPLANID"].ToString() == "")
                {
                    ViewState["SEMESTERPLANID"] = ds.Tables[0].Rows[0]["FLDSEMESTERPLANID"].ToString();
                    gvPreSea.SelectedIndex = 0;
                }
                if (Filter.CurrentPreSeaSemesterPlanId == null)
                {
                    gvPreSea.SelectedIndex = 0;
                    Filter.CurrentPreSeaSemesterPlanId = ((Label)gvPreSea.Rows[0].FindControl("lblSemesterPlanId")).Text;
                }
                //SetRowSelection();

            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvPreSea);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSea_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;

    }
    protected void gvPreSea_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
    }


    protected void gvPreSea_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            //int WeeklyPlanId ;
            GridView _gridView = (GridView)sender;

            if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                string semesterplanid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSemesterPlanId")).Text;
                string planid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPlanId")).Text;
                string subjectid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectId")).Text;
                ViewState["SEMESTERPLANID"] = semesterplanid;

                Response.Redirect("../PreSea/PreSeaBatchSemesterExamResults.aspx?PLANID="+planid+"&SEMESTERPLANID="+ semesterplanid+"&SUBJECTID="+subjectid+"&YEAR="+ddlYear.SelectedQuick);
            }
            if (e.CommandName.ToUpper().Equals("PLANEDIT"))
            {
                if (_gridView.EditIndex > -1)
                    _gridView.UpdateRow(_gridView.EditIndex, false);
                _gridView.EditIndex = nCurrentRow;
                //_gridView.SelectedIndex = de.NewEditIndex;

                BindData();

            }
            else if (e.CommandName.ToUpper().Equals("PLANUPDATE"))
            {
                if (IsValidWeeklyPlan(Session["BATCHMANAGECOURSE"].ToString()
                                        , Filter.CurrentPreSeaBatchManagerSelection
                                        , ddlSemester.SelectedSemester
                                        , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectIdEdit")).Text
                                        , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDate")).Text
                                        , ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtStartTimeEdit")).Text
                                        , ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtEndTimeEdit")).Text
                                        ))
                {
                    string subjectId;
                    string semesterplanid="";
                    semesterplanid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSemesterPlanIdEdit")).Text;
                    subjectId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectIdEdit")).Text;
                    if (subjectId != null && semesterplanid != "")
                    {

                        PhoenixPreSeaWeeklyPlanner.UpdateSemesterPlanDate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableInteger(ddlSemester.SelectedSemester)
                                                                , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSemesterPlanIdEdit")).Text)
                                                                , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDate")).Text)
                                                                , General.GetNullableDecimal(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtStartTimeEdit")).Text)
                                                                , General.GetNullableDecimal(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtEndTimeEdit")).Text)
                                                                );

                        _gridView.EditIndex = -1;
                        BindData();
                    }
                    else
                    {
                        PhoenixPreSeaWeeklyPlanner.InsertSemesterPlanDate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , General.GetNullableInteger(ddlYear.SelectedQuick)
                                                             , General.GetNullableInteger(ddlSemester.SelectedSemester)
                                                             , General.GetNullableInteger(Session["BATCHMANAGECOURSE"].ToString())
                                                             , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                                                             , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectIdEdit")).Text)
                                                             , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDate")).Text)
                                                             , General.GetNullableDecimal(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtStartTimeEdit")).Text)
                                                             , General.GetNullableDecimal(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtEndTimeEdit")).Text)
                                                             );
                        _gridView.EditIndex = -1;
                        BindData();
                    }
                 

                }
                else
                {
                    ucError.Visible = true;
                    return;

                }
            }
            else if (e.CommandName.ToUpper().Equals("PLANADD"))
            { 
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("PLANDELETE"))
            {
                PhoenixPreSeaWeeklyPlanner.DeleteWeeklyTimeTableDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblWeeklyPlanId")).Text));
                BindData();
               

            }
            else if (e.CommandName.ToUpper().Equals("PLANCANCEL"))
            {
                _gridView.EditIndex = -1;
                BindData();
                
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSea_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["STORETYPEID"] = null;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        ViewState["SEMESTERPLANID"] = String.Empty;
        BindData();
    }

    protected void gvPreSea_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = sender as GridView;
        gvPreSea.SelectedIndex = e.NewSelectedIndex;
        string SemesterPlanId = ((Label)_gridView.Rows[e.NewSelectedIndex].FindControl("lblSemesterPlanId")).Text;
        ViewState["SEMESTERPLANID"] = SemesterPlanId;
        Filter.CurrentPreSeaSemesterPlanId = SemesterPlanId;
        BindData();
    }

    protected void gvPreSea_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        //ucStatus.Text = "Mark is updated successfully";
        BindData();
        
    }

    protected void gvPreSea_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
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
                //ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
                //if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                

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
                    if (General.GetNullableInteger(drv["FLDSEMESTERPLANID"].ToString()) != null)
                    {
                        bi.Visible = true;
                        HtmlImage img = (HtmlImage)e.Row.FindControl("imgInvList");
                        bi.Attributes.Add("onclick", "javascript:return Openpopup('PreSea','','PreSeaSemesterExamInvigilator.aspx?semesterplanid=" + drv["FLDSEMESTERPLANID"].ToString() + "'); return false;");


                        if (img != null)
                        {
                            img.Visible = true;
                            img.Attributes.Add("onclick", "showMoreInformation(ev, 'PreSeaMoreInfoSemesterExamInvigilator.aspx?semesterplanid=" + drv["FLDSEMESTERPLANID"].ToString() + "')");
                        }
                    }
                    else
                        bi.Visible = false;
                }                               
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
        BindData();
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
    private bool IsValidWeeklyPlan(string courseid, string batchid,string planid,string subjectid,string date, string starttime,string endtime)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(courseid) == null)
            ucError.ErrorMessage = "Course is Required.";

        if (General.GetNullableInteger(batchid) == null)
            ucError.ErrorMessage = "Batch is Required.";

        if (General.GetNullableInteger(planid) == null)
            ucError.ErrorMessage = "Plan is Required.";

        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Date is required.";

        if (General.GetNullableInteger(subjectid) == null)
            ucError.ErrorMessage = "Subject is required.";

        if (!string.IsNullOrEmpty(starttime) && decimal.Parse(starttime) > 24)
            ucError.ErrorMessage = "Start time is not valid time.";
        if (!string.IsNullOrEmpty(endtime) &&  decimal.Parse(endtime) > 24)
            ucError.ErrorMessage = "End time is not valid time.";

        if (starttime.Split('.').Length == 2)
        {
            string time = starttime.Split('.')[1].Substring(0, starttime.Split('.')[1].Length);
            if (int.Parse(time) >= 60)
                ucError.ErrorMessage = "Start time is not valid time.";
        }
        if (endtime.Split('.').Length == 2)
        {
            string time = endtime.Split('.')[1].Substring(0, endtime.Split('.')[1].Length);
            if (int.Parse(time) >= 60)
                ucError.ErrorMessage = "End time is not valid time.";
        }
        if (!string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime) && (decimal.Parse(starttime) - decimal.Parse(endtime)) > 0)
            ucError.ErrorMessage = "Start time should be earlier than end time";            

        return (!ucError.IsError);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
    private void SetRowSelection()
    {
        gvPreSea.SelectedIndex = -1;
        for (int i = 0; i < gvPreSea.Rows.Count; i++)
        {
            if (gvPreSea.DataKeys[i].Value.ToString().Equals(Filter.CurrentPreSeaSemesterPlanId))
            {
                gvPreSea.SelectedIndex = i;

            }
        }
    }
    public bool IsValidBatchPlanDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information";
       
        if (General.GetNullableInteger(ddlCourse.SelectedCourse) == null)
            ucError.ErrorMessage = "Course is required";
        //if (General.GetNullableInteger(ucBatch.SelectedBatch) == null)
        //    ucError.ErrorMessage = "Batch is required";
        return (!ucError.IsError);

    }


}

