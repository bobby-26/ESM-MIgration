using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Collections.Specialized;


public partial class PreSeaBatchWeeklyPlanner : PhoenixBasePage
{
    DataSet dsGrid = new DataSet();

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

            if (!IsPostBack)
            {
                ViewState["BATCH"] = "";
                ViewState["INTERVIEWID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["TYPE"] = "";
               

                if (Request.QueryString["TYPE"].ToString() != "" && Request.QueryString["TYPE"] != null)
                    ViewState["TYPE"] = Request.QueryString["TYPE"].ToString();

                if (Request.QueryString["TYPE"].ToString() != "" && Request.QueryString["TYPE"].ToString() == "1")
                {
                    MenuBatchPlanner.SelectedMenuIndex = 1;
                    WeeklyPlannerTitle.Text = "Weekly Planner";
                }
                else if (Request.QueryString["TYPE"].ToString() != "" && Request.QueryString["TYPE"].ToString() == "2")
                {
                    ddlSemester.Visible = false;
                    ddlSection.Visible= false;
                    lblSection.Visible = false;
                    lblSemester.Visible = false;
                    MenuBatchPlanner.SelectedMenuIndex = 2;
                    WeeklyPlannerTitle.Text = "Buddy Planner";
                }
                else if (Request.QueryString["TYPE"].ToString() != "" && Request.QueryString["TYPE"].ToString() == "3")
                {
                    MenuBatchPlanner.SelectedMenuIndex = 3;
                    WeeklyPlannerTitle.Text = "Mentor Planner";
                }
                if (Session["BATCHMANAGECOURSE"] != null && Session["BATCHMANAGECOURSE"].ToString() != "")
                {
                    ddlCourse.SelectedCourse = Session["BATCHMANAGECOURSE"].ToString();
                    ddlCourse.Enabled = false;
                }
                ddlSemester.Course = Session["BATCHMANAGECOURSE"].ToString();
                ucBatch.SelectedBatch = Filter.CurrentPreSeaBatchManagerSelection;
                ucBatch.Enabled = false;
                BindTimeSlot();
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../PreSea/PreSeaBatchWeeklyPlanner.aspx", "Export to Excel", "icon_xls.png", "Excel");
            MenuPreSea.AccessRights = this.ViewState;
            MenuPreSea.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarPlannerList = new PhoenixToolbar();
            toolbarPlannerList.AddImageButton("../PreSea/PreSeaBatchWeeklyPlanner.aspx?", "Export to Excel", "icon_xls.png", "WeekExcel");
            MenuPreSeaWeekPlanner.AccessRights = this.ViewState;
            MenuPreSeaWeekPlanner.MenuList = toolbarPlannerList.Show();

            PhoenixToolbar menuweeklyplanner = new PhoenixToolbar();
            menuweeklyplanner.AddButton("Copy Previous Week Plan", "COPY");
            MenuWeeklyPlanner.MenuList = menuweeklyplanner.Show();
            BindData();
            BindWeeklyPlan();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindTimeSlot()
    {
        ddlTimeSlot.Items.Clear();

        DataTable dt = PhoenixPreSeaBatch.ListPreSeaTimeSlot(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
            , General.GetNullableInteger(ddlSemester.SelectedSemester));
        if (dt.Rows.Count > 0)
        {
            ddlTimeSlot.DataTextField = "FLDTIMESLOT";
            ddlTimeSlot.DataValueField = "FLDTIMESLOTID";
            ddlTimeSlot.DataSource = dt;
            ddlTimeSlot.DataBind();
        }
        else
        {
            ListItem li = new ListItem("--Select--", "");
            ddlTimeSlot.Items.Add(li);
        }
    }
    protected void ddlSemester_Changed(object sender, EventArgs e)
    {
        BindTimeSlot();
    }
    protected void ddlTimeSlot_Changed(object sender, EventArgs e)
    {
        BindData();
    }
    protected void txtDate_changed(object sender, EventArgs e)
    {
        GridView gv = gvPreSea;
        DropDownList ddlClassRoomAdd = (DropDownList)gv.FooterRow.FindControl("ddlClassRoomAdd");

        if (ddlClassRoomAdd != null && txtDate.Text != "")
        {
            ddlClassRoomAdd.Items.Clear();
            ListItem li = new ListItem("--Select--", "");
            ddlClassRoomAdd.Items.Add(li);

            DataTable dt = PhoenixPreSeaWeeklyPlanner.ListAvailableClassRoom
                (General.GetNullableDateTime(txtDate.Text), General.GetNullableInteger(ddlTimeSlot.SelectedValue));
            ddlClassRoomAdd.DataTextField = "FLDROOMNAME";
            ddlClassRoomAdd.DataValueField = "FLDROOMID";
            ddlClassRoomAdd.DataSource = dt;
            ddlClassRoomAdd.DataBind();
            BindData();
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
    
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;

            string[] alColumns = { "FLDSUBJECTNAME", "FLDSTAFFNAME", "FLDROOMNAME", "FLDPRACTICALNAME" };
            string[] alCaptions = { "Subject", "Staff", "Class Room", "Practical" };

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

            ds = PhoenixPreSeaWeeklyPlanner.ListWeeklyTimeTableDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                   , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                   , General.GetNullableInteger(ddlSemester.SelectedSemester)
                   , General.GetNullableInteger(ddlSection.SelectedValue)
                   , General.GetNullableDateTime(txtDate.Text)
                   , General.GetNullableInteger(ddlTimeSlot.SelectedValue)
                   , General.GetNullableInteger(ViewState["TYPE"].ToString())
                   );

            //     General.ShowExcel("Time Slot Details", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            Response.AddHeader("Content-Disposition", "attachment; filename=TimeSlotDetails.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/sims1.png" + "' /></td>");
            Response.Write("<td><h3>Time Slot Details</h3></td>");
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

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSUBJECTNAME", "FLDSTAFFNAME", "FLDROOMNAME", "FLDPRACTICALNAME" };
            string[] alCaptions = { "Subject", "Staff", "Class Room", "Practical" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = new DataSet();

            ds = PhoenixPreSeaWeeklyPlanner.ListWeeklyTimeTableDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                , General.GetNullableInteger(ddlSemester.SelectedSemester)
                , General.GetNullableInteger(ddlSection.SelectedValue)
                , General.GetNullableDateTime(txtDate.Text)
                , General.GetNullableInteger(ddlTimeSlot.SelectedValue)
                , General.GetNullableInteger(ViewState["TYPE"].ToString())
                );

            General.SetPrintOptions("gvPreSea", "Time slot details", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPreSea.DataSource = ds;
                gvPreSea.DataBind();

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
                                        , ddlSection.SelectedValue
                                        , txtDate.Text
                                        , ddlTimeSlot.SelectedValue
                                        , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectIdEdit")).Text))
                {
                    string subjectId;

                    subjectId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectIdEdit")).Text;
                    if (subjectId != null)

                        PhoenixPreSeaWeeklyPlanner.UpdateWeeklyTimeTableDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblWeeklyPlanIdEdit")).Text)
                                                                , General.GetNullableInteger(Session["BATCHMANAGECOURSE"].ToString())
                                                                , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                                                                , General.GetNullableInteger(ddlSemester.SelectedSemester)
                                                                , General.GetNullableInteger(ddlSection.SelectedValue)
                                                                , General.GetNullableInteger(ddlTimeSlot.SelectedValue)
                                                                , General.GetNullableDateTime(txtDate.Text)
                                                                , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectIdEdit")).Text)
                                                                , General.GetNullableInteger(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFacultyIdEdit")).Text)
                                                                , General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlClassRoomEdit")).SelectedValue)
                                                                , General.GetNullableInteger(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlPracticalEdit")).SelectedValue)
                                                                , null
                                                                , 0
                                                                , General.GetNullableInteger(ViewState["TYPE"].ToString()));

                    _gridView.EditIndex = -1;
                    BindData();
                    BindWeeklyPlan();

                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            else if (e.CommandName.ToUpper().Equals("PLANADD"))
            {
                if (IsValidWeeklyPlan(Session["BATCHMANAGECOURSE"].ToString()
                                        , Filter.CurrentPreSeaBatchManagerSelection
                                        , ddlSemester.SelectedSemester
                                        , ddlSection.SelectedValue
                                        , txtDate.Text
                                        , ddlTimeSlot.SelectedValue
                                        , ((TextBox)_gridView.FooterRow.FindControl("txtsubjectId")).Text))
                {
                    PhoenixPreSeaWeeklyPlanner.InsertWeeklyTimeTableDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableInteger(Session["BATCHMANAGECOURSE"].ToString())
                                                            , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                                                            , General.GetNullableInteger(ddlSemester.SelectedSemester)
                                                            , General.GetNullableInteger(ddlSection.SelectedValue)
                                                            , General.GetNullableInteger(ddlTimeSlot.SelectedValue)
                                                            , General.GetNullableDateTime(txtDate.Text)
                                                            , General.GetNullableInteger(((TextBox)_gridView.FooterRow.FindControl("txtsubjectId")).Text)
                                                            , General.GetNullableInteger(((TextBox)_gridView.FooterRow.FindControl("txtFacultyAddId")).Text)
                                                            , General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlClassRoomAdd")).SelectedValue)
                                                            , General.GetNullableInteger(((DropDownList)_gridView.FooterRow.FindControl("ddlPracticalAdd")).SelectedValue)
                                                            , null
                                                            , 0
                                                            , General.GetNullableInteger(ViewState["TYPE"].ToString())
                                                            , 0
                                                            );


                    BindData();
                    BindWeeklyPlan();
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            else if (e.CommandName.ToUpper().Equals("PLANDELETE"))
            {
                PhoenixPreSeaWeeklyPlanner.DeleteWeeklyTimeTableDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblWeeklyPlanId")).Text));
                BindData();
                BindWeeklyPlan();

            }
            else if (e.CommandName.ToUpper().Equals("PLANCANCEL"))
            {
                _gridView.EditIndex = -1;
                BindData();
                BindWeeklyPlan();
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
        ViewState["EXAMVENUEID"] = String.Empty;
        BindData();
    }

    protected void gvPreSea_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = sender as GridView;
        gvPreSea.SelectedIndex = e.NewSelectedIndex;
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
                ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
                if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

                Label lblIsCompletedYN = (Label)e.Row.FindControl("lblIsCompletedYN");
                //if (lblIsCompletedYN!= null && lblIsCompletedYN.Text.ToUpper() == "YES") 
                //{
                //    if (del != null) del.Visible = false;
                //    if (eb != null) eb.Visible = false;
                //    //sb.Visible = false;
                //    //cb.Visible = false;
                //}
                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                {
                    ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    }

                }
                
                TextBox txtFacultyEdit = (TextBox)e.Row.FindControl("txtFacultyEdit");
                TextBox txtFacultyDesignationEdit = (TextBox)e.Row.FindControl("txtFacultyDesignationEdit");
                TextBox txtFacultyIdEdit = (TextBox)e.Row.FindControl("txtFacultyIdEdit");
                TextBox txtFacultyEmailEdit = (TextBox)e.Row.FindControl("txtFacultyEmailEdit");
                if (txtFacultyDesignationEdit != null)
                    txtFacultyDesignationEdit.Attributes.Add("style", "visibility:hidden");
                if (txtFacultyIdEdit != null)
                    txtFacultyIdEdit.Attributes.Add("style", "visibility:hidden");
                if (txtFacultyEmailEdit != null)
                    txtFacultyEmailEdit.Attributes.Add("style", "visibility:hidden");

                ImageButton imgFacultyEdit = (ImageButton)e.Row.FindControl("imgFacultyEdit");
                if (imgFacultyEdit != null)
                {
                    DataTable dt = new DataTable();
                    dt = PhoenixPreSeaBatchPlanner.ListPreSeaTrainingDepartmentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                    string DepartmentList = dt.Rows[0]["FLDDEPARTMENTIDLIST"].ToString();

                    imgFacultyEdit.Attributes.Add("onclick", "return showPickList('spnFacultyEdit', 'codehelp1', '', '../Common/CommonPickListPreSeaUser.aspx?departmentlist=" + DepartmentList + "', true);");
                    if (!SessionUtil.CanAccess(this.ViewState, imgFacultyEdit.CommandName)) imgFacultyEdit.Visible = false;

                }

                Label lblClassRoomIdEdit = (Label)e.Row.FindControl("lblClassRoomIdEdit");

                DropDownList ddlClassRoomEdit = (DropDownList)e.Row.FindControl("ddlClassRoomEdit");
                if (ddlClassRoomEdit != null)
                {
                    ddlClassRoomEdit.Items.Clear();
                    ListItem li = new ListItem("--Select--", "");
                    ddlClassRoomEdit.Items.Add(li);

                    DataTable dt = PhoenixPreSeaWeeklyPlanner.ListAvailableClassRoom
                        (General.GetNullableDateTime(txtDate.Text), General.GetNullableInteger(ddlTimeSlot.SelectedValue));

                    ddlClassRoomEdit.DataSource = dt;
                    ddlClassRoomEdit.DataBind();
                    if (lblClassRoomIdEdit != null)
                    {
                        ddlClassRoomEdit.SelectedValue = lblClassRoomIdEdit.Text;
                    }
                }

                Label lblPracticalIdEdit = (Label)e.Row.FindControl("lblPracticalIdEdit");
                DropDownList ddlPracticalEdit = (DropDownList)e.Row.FindControl("ddlPracticalEdit");

                if (ddlPracticalEdit != null)
                {
                    ddlPracticalEdit.SelectedValue = lblPracticalIdEdit.Text;
                }
                ImageButton bp = (ImageButton)e.Row.FindControl("cmdPlan");
                if (bp != null) bp.Visible = SessionUtil.CanAccess(this.ViewState, bp.CommandName);
                Label lblStaffId = (Label)e.Row.FindControl("lblStaffId");
                Label lblDate = (Label)e.Row.FindControl("lblDate");

                if (bp != null && lblStaffId != null && lblDate != null)
                {
                    bp.Attributes.Add("onclick", "javascript:return Openpopup('PreSea','','PreSeaWeeklyPlannerIndividual.aspx?type=1&staffid=" + lblStaffId.Text + "&date=" + lblDate.Text + "'); return false;");
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
                Label lblIsCompletedYNAdd = (Label)e.Row.FindControl("lblIsCompletedYNAdd");

                if (lblIsCompletedYNAdd.Text.ToUpper() == "YES")
                {
                    db.Visible = false;
                }
                DropDownList ddlClassRoomAdd = (DropDownList)e.Row.FindControl("ddlClassRoomAdd");
                if (ddlClassRoomAdd != null)
                {
                    ddlClassRoomAdd.Items.Clear();
                    ListItem li = new ListItem("--Select--", "");
                    ddlClassRoomAdd.Items.Add(li);
                    ddlClassRoomAdd.DataValueField = "FLDROOMID";
                    ddlClassRoomAdd.DataTextField = "FLDROOMNAME";
                    DataTable dt = PhoenixPreSeaWeeklyPlanner.ListAvailableClassRoom
                        (General.GetNullableDateTime(txtDate.Text), General.GetNullableInteger(ddlTimeSlot.SelectedValue));

                    ddlClassRoomAdd.DataSource = dt;
                    ddlClassRoomAdd.DataBind();
                }
                TextBox txtSubjectNameAdd = (TextBox)e.Row.FindControl("txtSubjectNameAdd");
                TextBox txtsubjectId = (TextBox)e.Row.FindControl("txtsubjectId");
                TextBox txtSubjectType = (TextBox)e.Row.FindControl("txtSubjectType");

                if (txtsubjectId != null)
                    txtsubjectId.Attributes.Add("style", "visibility:hidden");
                if (txtSubjectType != null)
                    txtSubjectType.Attributes.Add("style", "visibility:hidden");

                TextBox txtFacultyAdd = (TextBox)e.Row.FindControl("txtFacultyAdd");
                TextBox txtFacultyAddDesignation = (TextBox)e.Row.FindControl("txtFacultyAddDesignation");
                TextBox txtFacultyAddId = (TextBox)e.Row.FindControl("txtFacultyAddId");
                TextBox txtFacultyAddEmail = (TextBox)e.Row.FindControl("txtFacultyAddEmail");
                if (txtFacultyAddDesignation != null)
                    txtFacultyAddDesignation.Attributes.Add("style", "visibility:hidden");
                if (txtFacultyAddId.Text != null)
                    txtFacultyAddId.Attributes.Add("style", "visibility:hidden");
                if (txtFacultyAddEmail != null)
                    txtFacultyAddEmail.Attributes.Add("style", "visibility:hidden");


                ImageButton imgSubjectAdd = (ImageButton)e.Row.FindControl("imgSubjectAdd");
                if (imgSubjectAdd != null)
                {
                    imgSubjectAdd.Attributes.Add("onclick", "return showPickList('spnSubjectAdd', 'codehelp1', '', '../Common/CommonPickListPreSeaSubjects.aspx?SEMESTERID=" + ddlSemester.SelectedSemester + "&COURSEID=" + Session["BATCHMANAGECOURSE"].ToString() + "', true);");
                    imgSubjectAdd.Visible = SessionUtil.CanAccess(this.ViewState, imgSubjectAdd.CommandName);

                }

                ImageButton imgFacultyAdd = (ImageButton)e.Row.FindControl("imgFacultyAdd");
                if (imgFacultyAdd != null)
                {
                    DataTable dt = new DataTable();
                    dt = PhoenixPreSeaBatchPlanner.ListPreSeaTrainingDepartmentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                    string DepartmentList = dt.Rows[0]["FLDDEPARTMENTIDLIST"].ToString();

                    imgFacultyAdd.Attributes.Add("onclick", "return showPickList('spnFacultyAdd', 'codehelp1', '', '../Common/CommonPickListPreSeaUser.aspx?departmentlist=" + DepartmentList + "', true);");
                    imgFacultyAdd.Visible = SessionUtil.CanAccess(this.ViewState, imgFacultyAdd.CommandName);

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
    private bool IsValidWeeklyPlan(string courseid, string batchid, string semesterid, string sectionid, string date, string timeslotid, string subjectid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(courseid) == null)
            ucError.ErrorMessage = "Course is Required.";

        if (General.GetNullableInteger(batchid) == null)
            ucError.ErrorMessage = "Batch is Required.";

        if (General.GetNullableInteger(semesterid) == null)
            ucError.ErrorMessage = "Semester is Required.";

        if (General.GetNullableInteger(sectionid) == null)
            ucError.ErrorMessage = "Section is Required.";

        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Date is required.";

        if (General.GetNullableInteger(timeslotid) == null)
            ucError.ErrorMessage = "Time slot is required.";

        if (General.GetNullableInteger(subjectid) == null)
            ucError.ErrorMessage = "Subject is required.";

        return (!ucError.IsError);
    }

    private bool IsValidWeeklyPlan(string courseid, string batchid, string semesterid, string sectionid, string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(courseid) == null)
            ucError.ErrorMessage = "Course is Required.";

        if (General.GetNullableInteger(batchid) == null)
            ucError.ErrorMessage = "Batch is Required.";

        if (General.GetNullableInteger(semesterid) == null)
            ucError.ErrorMessage = "Semester is Required.";

        if (General.GetNullableInteger(sectionid) == null)
            ucError.ErrorMessage = "Section is Required.";

        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Date is required.";
        
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

        }
    }

    private void EditTimeSlotData(string Date, string TimeSlotId)
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixPreSeaWeeklyPlanner.ListWeeklyTimeTableDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                , General.GetNullableInteger(ddlSemester.SelectedSemester)
                , General.GetNullableInteger(ddlSection.SelectedValue)
                , General.GetNullableDateTime(Date)
                , General.GetNullableInteger(TimeSlotId)
                , General.GetNullableInteger(ViewState["TYPE"].ToString())
                );

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtDate.Text = Date;
                ddlTimeSlot.SelectedValue = TimeSlotId;
                gvPreSea.DataSource = ds;
                gvPreSea.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvPreSea);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    /*Time Table Calender*/

    private void BindWeeklyPlan()
    {

        try
        {
            //DataSet dsGrid = new DataSet();
            gvPreseaWeeklyPlanner.Columns.Clear();

            dsGrid = PhoenixPreSeaWeeklyPlanner.ListWeeklyTimeTable(General.GetNullableInteger(Session["BATCHMANAGECOURSE"].ToString())
                                    , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                                    , General.GetNullableInteger(ddlSemester.SelectedSemester)
                                    , General.GetNullableInteger(ddlSection.SelectedValue)
                                    , General.GetNullableDateTime(txtDate.Text)
                                    , General.GetNullableInteger(ViewState["TYPE"].ToString()));

            if (dsGrid.Tables.Count > 0 && dsGrid.Tables[0].Rows.Count > 0)
            {
                DataTable dt = dsGrid.Tables[0];
                DataTable dt2 = dsGrid.Tables[1];

                if (dt.Rows.Count > 0)
                {
                    BoundField field1 = new BoundField();
                    field1.HtmlEncode = false;
                    field1.HeaderText = "Time";
                    field1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    field1.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    field1.ItemStyle.Wrap = false;
                    gvPreseaWeeklyPlanner.Columns.Add(field1);

                    for (int i = 0; i < dt.Columns.Count - 1; i++)
                    {
                        //if (i != 1)
                        //{
                        BoundField field = new BoundField();
                        field.HtmlEncode = false;
                        field.HeaderText = dt.Rows[0][i].ToString();
                        field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                        field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                        field.ItemStyle.Wrap = false;
                        gvPreseaWeeklyPlanner.Columns.Add(field);
                        //}
                    }

                    gvPreseaWeeklyPlanner.DataSource = dsGrid.Tables[2];
                    gvPreseaWeeklyPlanner.DataBind();
                    //MenuPreSeaWeekPlanner.MenuList = toolbarPlannerList.Show();
                }
                else
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i != 1)
                        {
                            BoundField field = new BoundField();
                            field.HtmlEncode = false;
                            field.HeaderText = dt.Rows[0][i].ToString();
                            field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

                            field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            field.ItemStyle.Wrap = false;
                            gvPreseaWeeklyPlanner.Columns.Add(field);
                        }
                    }

                    ShowNoRecordsFound(dsGrid.Tables[0], gvPreseaWeeklyPlanner);
                }
            }
            else
            {
                ShowNoRecordsFound(dsGrid.Tables[0], gvPreseaWeeklyPlanner);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreseaWeeklyPlanner_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        try
        {
            GridView gv = (GridView)sender;

            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow && gv.DataSource.GetType().Equals(typeof(DataTable)))
            {
                DataTable timeslots = (DataTable)gv.DataSource;
                DataTable header = dsGrid.Tables[0];
                DataTable Days = dsGrid.Tables[1];
                DataTable time = dsGrid.Tables[2];
                DataTable data = dsGrid.Tables[3];
                if (timeslots.Rows.Count > 0)
                {
                    string holidays = header.Rows[0]["FLDHOLIDAYS"].ToString();

                    Literal ltrlHRs = new Literal();
                    Literal lblTimeSlotId = new Literal();
                    Literal lblBrkSlot = new Literal();
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[0].VerticalAlign = VerticalAlign.Middle;
                    e.Row.Cells[0].Style.Add("font-weight", "bold");
                    e.Row.Cells[0].Style.Add("vertical-align", "middle");
                    ltrlHRs.Text = time.Rows[e.Row.RowIndex]["FLDTIMESLOT"].ToString() + "&nbsp;&nbsp;";
                    lblTimeSlotId.Text = time.Rows[e.Row.RowIndex]["FLDTIMESLOTID"].ToString();
                    lblTimeSlotId.Visible = false;

                    e.Row.Cells[0].Controls.Add(ltrlHRs);
                    e.Row.Cells[0].Controls.Add(lblTimeSlotId);

                    if (time.Rows[e.Row.RowIndex]["FLDISBRKYN"].ToString() != "" && time.Rows[e.Row.RowIndex]["FLDISBRKYN"].ToString() == "1")
                    {
                        for (int i = 1; i < e.Row.Cells.Count; i++)
                        {
                            lblBrkSlot.Text = "BREAK " + "<br/>";
                            e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;
                            e.Row.Cells[i].Style.Add("font-weight", "bold");
                            e.Row.Cells[i].Controls.Add(lblBrkSlot);
                        }
                            
                        //e.Row.Cells[1].ColumnSpan = 6;
                        //e.Row.Cells.RemoveAt(2);
                        //e.Row.Cells.RemoveAt(3);
                        //e.Row.Cells.RemoveAt(4);
                        //e.Row.Cells.RemoveAt(5);
                        //e.Row.Cells[1].Controls.Add(lblBrkSlot);
                        //e.Row.Cells[2].Controls.Add(lblBrkSlot);
                        //e.Row.Cells[3].Controls.Add(lblBrkSlot);
                        //e.Row.Cells[4].Controls.Add(lblBrkSlot);
                        //e.Row.Cells[5].Controls.Add(lblBrkSlot);
                        //e.Row.Cells[6].Controls.Add(lblBrkSlot);
                    }
                    else
                    {
                        for (int i = 0; i < e.Row.Cells.Count; i++)
                        {
                            if (i > 0)
                            {

                                Literal ltrl = new Literal();
                               // Literal ltrstaffname = new Literal();
                                LinkButton lnkEdit = new LinkButton();
                                Label lblIsBreak = new Label();

                                ImageButton ibEdit = new ImageButton();
                                ibEdit.ID = "ibEditSlot" + i.ToString();
                                ibEdit.ImageUrl = Session["images"] + "te_edit.png";
                                ibEdit.CommandName = "EDITSLOT";
                                ibEdit.CommandArgument = Days.Rows[i - 1]["FLDCOLUMNDATE"].ToString() + "|" + timeslots.Rows[e.Row.RowIndex]["FLDROWID"].ToString();
                                ibEdit.ToolTip = "Edit";
                                //ibEdit.ImageAlign = ImageAlign.AbsMiddle;
                                //ibEdit.Style.Add("margin", "5px");

                                ImageButton ibdel = new ImageButton();
                                ibdel.ID = "ibdelHours" + i.ToString();
                                ibdel.ImageUrl = Session["images"] + "/te_del.png";
                                ibdel.CommandName = "DELHOURS";
                                ibdel.ToolTip = "Delete";
                                ibdel.ImageAlign = ImageAlign.AbsMiddle;
                                ibdel.Style.Add("margin", "5px");

                                ltrl.ID = "ltrlDay" + i.ToString();
                                lnkEdit.ID = "lnkEdit" + i.ToString();
                                lblIsBreak.ID = "lblIsBreak" + i.ToString();

                                if (!holidays.Contains(Convert.ToDateTime(Days.Rows[i - 1]["FLDCOLUMNDATE"]).ToShortDateString()))
                                {

                                    DataRow[] dr = data.Select("FLDCOLUMNID = '" + Days.Rows[i - 1]["FLDCOLUMNDATE"].ToString() + "' AND FLDROWID = '" + timeslots.Rows[e.Row.RowIndex]["FLDROWID"].ToString() + "'");
                                    if (dr != null && dr.Length == 1)
                                    {
                                        if (!holidays.Contains(dr[0]["FLDCOLUMNID"].ToString()))
                                        {
                                            lnkEdit.Text = "Edit";
                                            lnkEdit.CommandName = "EDITSLOT";
                                            lnkEdit.CommandArgument = dr[0]["FLDCOLUMNID"].ToString() + "|" + dr[0]["FLDROWID"].ToString();
                                            //lnkEdit.Attributes.Add("onclick", "Openpopup('EditTimeTable', '', 'PreSeaWeeklyPlannerChange.aspx?Day=" + dr[0]["FLDDAYID"].ToString() + "&Hour=" + dr[0]["FLDHOURID"].ToString() + "'); return false;");

                                            if (!String.IsNullOrEmpty(dr[0]["FLDSUBJECTID"].ToString()))
                                                ltrl.Text = dr[0]["FLDSUBJECTNAME"].ToString();
                                            else
                                                ltrl.Text = "Break";

                                            //if (!String.IsNullOrEmpty(dr[0]["FLDSTAFFNAME"].ToString()))
                                            //    ltrstaffname.Text = dr[0]["FLDSTAFFNAME"].ToString();
                                            //lblIsBreak.Text = dr[0]["FLDISBREAKHOURS"].ToString();
                                            //lblIsBreak.Visible = false;

                                            ltrl.Text += "<br/>";
                                            e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;
                                            e.Row.Cells[i].Style.Add("font-weight", "bold");

                                            e.Row.Cells[i].Controls.Add(ltrl);
                                            ibdel.CommandArgument = dr[0]["FLDWEEKPLANID"].ToString();

                                            //if (dr[0]["FLDCOMPLETEDYN"].ToString().Equals("0"))
                                            //{                                                
                                            e.Row.Cells[i].Controls.Add(lnkEdit);
                                            //}                                          
                                            e.Row.Cells[i].Controls.Add(lblIsBreak);
                                           // e.Row.Cells[i].Controls.Add(ltrstaffname);
                                        }

                                    }
                                    else if (dr != null && dr.Length > 1)
                                    {
                                        if (!holidays.Contains(dr[0]["FLDCOLUMNID"].ToString()))
                                        {
                                            lnkEdit.Text = "Edit";
                                            lnkEdit.CommandName = "EDITSLOT";
                                            lnkEdit.CommandArgument = dr[0]["FLDCOLUMNID"].ToString() + "|" + dr[0]["FLDROWID"].ToString();

                                            foreach (DataRow row in dr)
                                            {
                                                if (!String.IsNullOrEmpty(row["FLDSUBJECTID"].ToString()))
                                                    ltrl.Text = ltrl.Text + "<br/>" + row["FLDSUBJECTNAME"].ToString();
                                                else
                                                    ltrl.Text = "Break";
                                            }
                                            ltrl.Text += "<br/>";
                                            e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;
                                            e.Row.Cells[i].Style.Add("font-weight", "bold");

                                            e.Row.Cells[i].Controls.Add(ltrl);
                                            ibdel.CommandArgument = dr[0]["FLDWEEKPLANID"].ToString();
                                            //if (dr[0]["FLDCOMPLETEDYN"].ToString().Equals("0"))
                                            //{
                                            e.Row.Cells[i].Controls.Add(lnkEdit);
                                            //}                                      
                                            e.Row.Cells[i].Controls.Add(lblIsBreak);
                                        }
                                    }

                                }
                                else if (holidays.Contains(Convert.ToDateTime(Days.Rows[i - 1]["FLDCOLUMNDATE"]).ToShortDateString()))
                                {
                                    DataTable dt = PhoenixPreSeaHoliday.ListHoliday(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableDateTime(Convert.ToDateTime(Days.Rows[i - 1]["FLDCOLUMNDATE"]).ToString()));
                                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;
                                    e.Row.Cells[i].Style.Add("font-weight", "bold");
                                    if (dt.Rows.Count > 0)
                                        e.Row.Cells[i].Text = "HOLIDAY" + "<br/>" + dt.Rows[0]["FLDREASON"].ToString();
                                    else
                                        e.Row.Cells[i].Text = "HOLIDAY";
                                }
                            }
                        }
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

    protected void gvPreseaWeeklyPlanner_DataBound(object sender, EventArgs e)
    {
        try
        {

            foreach (GridViewRow gvRow in gvPreseaWeeklyPlanner.Rows)
            {

                string CellText = ""; bool AllCellSame = false;
                for (int cellCount = 1; cellCount < 7; cellCount++)
                {
                    Label lblIsBreak = (Label)gvRow.Cells[cellCount].FindControl("lblIsBreak" + cellCount.ToString());
                    Literal ltrl = (Literal)gvRow.Cells[cellCount].FindControl("ltrlDay" + cellCount.ToString());

                    if (lblIsBreak != null && lblIsBreak.Text.ToUpper() == "YES")
                    {
                        if (ltrl != null && cellCount == 1)
                            CellText = ltrl.Text;
                        else if (ltrl != null && cellCount > 1)
                        {
                            if (CellText == ltrl.Text)
                            {
                                gvRow.Cells[cellCount].Visible = false;
                                AllCellSame = true;
                            }
                            else
                            {
                                AllCellSame = false;
                            }
                        }
                        else
                        {
                            AllCellSame = false;
                        }

                    }

                }
                if (AllCellSame)
                {
                    gvRow.Cells[1].ColumnSpan = 6;
                    gvRow.Style.Add("background-color", "#CCCCCC");
                    foreach (TableCell cl in gvRow.Cells)
                    {
                        if (cl.Text.Contains("HOLIDAY"))
                            cl.Visible = false;
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

    protected void gvPreseaWeeklyPlanner_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "DELETE")
            {
                string arg = e.CommandArgument.ToString();
                string[] args = new string[3];
                args = arg.Split('|');

            }
            else if (e.CommandName.ToString().ToUpper() == "EDITSLOT")
            {
                string arg = e.CommandArgument.ToString();
                string[] args = new string[3];
                args = arg.Split('|');

                EditTimeSlotData(args[0], args[1]);
            }
            BindWeeklyPlan();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreseaWeeklyPlanner_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPreSeaWeekPlanner_TabStripCommand(object sender, EventArgs e)
    {
        //DataSet dsGrid = new DataSet();
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("WEEKEXCEL"))
            {
                BindWeeklyPlan();

                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=WeeklyPlanner.xls");
                Response.Charset = "";
                //if (dsGrid.Tables.Count > 3 && dsGrid.Tables[3].Rows.Count > 0)
                //{
                string Header = "<table>";
                Header += "<tr><td colspan='2' rowspan='5'><img width='99' height='99' src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/sims.png" + "' /></td>";
                Header += "<td colspan='6' align='center'  style='font-weight:bold;'>Samundra Institute of Maritime Studies, Lonavla</td></tr>";
                Header += "<tr><td colspan='6'  align='center'  style='font-weight:bold;'>";
                //Header += dsGrid.Tables[3].Rows[0]["FLDBATCH"].ToString();
                Header += "</td></tr>";
                Header += "<tr><td colspan='6'  align='center'  style='font-weight:bold;'>";
                //Header += dsGrid.Tables[3].Rows[0]["FLDSEMESTERID"].ToString();
                Header += "</td></tr>";
                Header += "<tr><td colspan='6'  align='center'  style='font-weight:bold;'>";
                //Header += dsGrid.Tables[3].Rows[0]["FLDROOMSHORTNAME"].ToString();
                Header += "</td></tr>";
                Header += "</table>";
                Response.Write(Header);
                //}
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();

                HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                Table table = new Table();
                table.CellPadding = 3;
                table.CellSpacing = 2;
                table.GridLines = GridLines.Both;
                int count = 0;
                if (gvPreseaWeeklyPlanner.HeaderRow != null)
                {
                    GridViewRow HdrRow = gvPreseaWeeklyPlanner.HeaderRow;

                    TableCell HdrCell;

                    count = HdrRow.Cells.Count;
                    HdrRow.HorizontalAlign = HorizontalAlign.Center;
                    for (int i = 2; i < count + 6; i++) // 6 COLUMNS FOR SIGN
                    {
                        HdrCell = new TableCell();
                        HdrCell.Text = "SIGN";
                        HdrCell.Font.Bold = true;
                        HdrCell.Attributes["style"] = "text-align: center;align:center;vertical-align:middle;";
                        HdrRow.Cells.AddAt(i, HdrCell);
                        i = i + 1;
                    }
                    PrepareControlForExport(HdrRow);
                    table.Rows.Add(HdrRow);
                }
                if (gvPreseaWeeklyPlanner.Rows.Count > 0)
                {
                    GridViewRow firstrow = gvPreseaWeeklyPlanner.Rows[0];
                    if (firstrow.Cells.Count > 1) // check whether it is norcords found (dummy row)
                    {
                        foreach (GridViewRow row in gvPreseaWeeklyPlanner.Rows)
                        {
                            TableCell cell;
                            for (int i = 2; i < count + 6; i++)
                            {
                                cell = new TableCell();
                                cell.Text = "";
                                cell.Font.Bold = true;
                                cell.Attributes["style"] = "vertical-align:middle;";
                                row.Cells.AddAt(i, cell);
                                i = i + 1;
                            }
                            PrepareControlForExport(row);
                            table.Rows.Add(row);
                        }


                    }
                }

                //Response.ClearContent();
                //Response.ContentType = "application/ms-excel";
                //Response.AddHeader("content-disposition", "attachment;filename=WeeklyPlanner.xls");
                //Response.Charset = "";
                // System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                //stringwriter.Write("<table><tr><td colspan=\"" + (gvPreseaWeeklyPlanner.Columns.Count + 1) + "\"></td></tr></table>");
                //stringwriter.Write("<table><tr><td colspan=\"" + (gvPreseaWeeklyPlanner.Columns.Count - 6) + "\"></td></tr></table>");
                //<td colspan=\"3\"><b>" + PhoenixSecurityContext.CurrentSecurityContext.VesselName + "<b/></td>" +

                //"<td>Period : </td><td>" + DateTime.Parse(txtFromDate.Text).ToString("dd-MMMM-yyyy") + "</td><td>To : </td><td>" + DateTime.Parse(txtClosginDate.Text).ToString("dd-MMMM-yyyy") + "</td>" +
                //"<td colspan=\"" + (gvPreseaWeeklyPlanner.Columns.Count - 6) + "\"></td></tr></table>");
                //stringwriter.Write("<table><tr><td colspan=\"" + (gvPreseaWeeklyPlanner.Columns.Count + 1) + "\"></td></tr></table>");
                //HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                //gvPreseaWeeklyPlanner.RenderControl(htmlwriter);
                //Response.Write(stringwriter.ToString());
                //Response.End();

                table.RenderControl(htmlwriter);
                Response.Write(stringwriter.ToString());
                Response.End();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void PrepareGridViewForExport(Control gridView)
    {
        for (int i = 0; i < gridView.Controls.Count; i++)
        {
            //Get the control
            Control currentControl = gridView.Controls[i];
            if (currentControl is LinkButton)
            {
                gridView.Controls.Remove(currentControl);
                //gridView.Controls.AddAt(i, new LiteralControl((currentControl as LinkButton).Text));
            }
            else if (currentControl is ImageButton)
            {
                gridView.Controls.Remove(currentControl);
                //gridView.Controls.AddAt(i, new LiteralControl((currentControl as ImageButton).AlternateText));
            }
            else if (currentControl is HyperLink)
            {
                gridView.Controls.Remove(currentControl);
                //gridView.Controls.AddAt(i, new LiteralControl((currentControl as HyperLink).Text));
            }
            else if (currentControl is DropDownList)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as DropDownList).SelectedItem.Text));
            }
            else if (currentControl is CheckBox)
            {
                gridView.Controls.Remove(currentControl);
                gridView.Controls.AddAt(i, new LiteralControl((currentControl as CheckBox).Checked ? "True" : "False"));
            }
            if (currentControl.HasControls())
            {
                // if there is any child controls, call this method to prepare for export
                PrepareGridViewForExport(currentControl);
            }
        }
    }
    private void PrepareControlForExport(Control control)
    {
        Control current = new Control();
        string cellid = "";
        for (int i = 0; i < control.Controls.Count; i++)
        {
            current = control.Controls[i];
            if (current.HasControls())
            {
                PrepareControlForExport(current);
            }

            if (current is LinkButton)
            {
                cellid = current.ID.Replace("lnkEdit", "");
                control.Controls.Remove(current);
            }
            else if (current is Literal)
            {
                Literal ltr = (Literal)current;
                ltr.Text = ltr.Text.Replace("{0}", "<br>");
            }
            //else if (current is ImageButton)
            //{
            //    control.Controls.Remove(current);
            //}

            Control ibdel = control.FindControl("ibdelHours" + cellid);
            if (ibdel != null)
                control.Controls.Remove(ibdel);

            Control ibcpy = control.FindControl("ibCpyHours" + cellid);
            if (ibcpy != null)
                control.Controls.Remove(ibcpy);

            Control ibPlan = control.FindControl("ibPlanWeek" + cellid);
            if (ibPlan != null)
                control.Controls.Remove(ibPlan);
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
    protected void imgCopy_Click(object sender, ImageClickEventArgs e)
    {
        if (string.IsNullOrEmpty(txtDate.Text))
        {
            ucError.Text = "Date is required";
            ucError.Visible = true;
        }

        PhoenixPreSeaWeeklyPlanner.InsertWeeklyTimeTableDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                           , General.GetNullableInteger(Session["BATCHMANAGECOURSE"].ToString())
                                                           , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                                                           , General.GetNullableInteger(ddlSemester.SelectedSemester)
                                                           , General.GetNullableInteger(ddlSection.SelectedValue)
                                                           , General.GetNullableInteger(ddlTimeSlot.SelectedValue)
                                                           , General.GetNullableDateTime(txtDate.Text)
                                                           , null
                                                           , null
                                                           , null
                                                           , null
                                                           , null
                                                           , 0
                                                           , null
                                                           , 1
                                                           );
        BindData();
        BindWeeklyPlan();
    }

    protected void MenuWeeklyPlanner_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("COPY"))
            {
                if (IsValidWeeklyPlan(ddlCourse.SelectedCourse,ucBatch.SelectedBatch,ddlSemester.SelectedSemester,ddlSection.SelectedValue,txtDate.Text))
                {
                    PhoenixPreSeaWeeklyPlanner.CopyWeeklyTimeTable(General.GetNullableInteger(ddlCourse.SelectedCourse)
                                                                  , General.GetNullableInteger(ucBatch.SelectedBatch)
                                                                  , General.GetNullableInteger(ddlSemester.SelectedSemester)
                                                                  , General.GetNullableInteger(ddlSection.SelectedValue)
                                                                  , General.GetNullableDateTime(txtDate.Text));
                    BindData();
                    BindWeeklyPlan();
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
