using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Collections.Specialized;

public partial class PreSeaBatchWeeklyPlannerActual : PhoenixBasePage
{
    DataSet dsGrid = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar MainToolbar = new PhoenixToolbar();

            MainToolbar.AddButton("Batch", "BATCHACTUAL");
            MainToolbar.AddButton("Weekly Planner Actual", "WEEKLYPLANACTUAL");
            MainToolbar.AddButton("Buddy Planner", "BUDDYPLANACTUAL");
            MainToolbar.AddButton("Mentoring Planner", "MENTORPLANACTUAL");
            //MainToolbar.AddButton("Exam", "EXAM");


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
                    WeeklyPlannerTitle.Text = "Weekly Planner Actual";
                }
                else if (Request.QueryString["TYPE"].ToString() != "" && Request.QueryString["TYPE"].ToString() == "2")
                {
                    MenuBatchPlanner.SelectedMenuIndex = 2;
                    WeeklyPlannerTitle.Text = "Buddy Planner Actual";
                }
                else if (Request.QueryString["TYPE"].ToString() != "" && Request.QueryString["TYPE"].ToString() == "3")
                {
                    MenuBatchPlanner.SelectedMenuIndex = 3;
                    WeeklyPlannerTitle.Text = "Mentor Planner Actual";
                }

                if (Filter.CurrentPreSeaActualCourseSelection != null && Filter.CurrentPreSeaActualCourseSelection != "")
                {
                    ddlCourse.SelectedCourse = Filter.CurrentPreSeaActualCourseSelection.ToString();
                    ddlCourse.Enabled = false;
                    ddlSemester.Course = Filter.CurrentPreSeaActualCourseSelection.ToString();

                }
                ucBatch.SelectedBatch = Filter.CurrentPreSeaActualBatchSelection;
                ucBatch.Enabled = false;
                BindTimeSlot(); 

            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../PreSea/PreSeaBatchWeeklyPlannerActual.aspx?TYPE=" + ViewState["TYPE"].ToString(), "Export to Excel", "icon_xls.png", "Excel");

            MenuPreSea.AccessRights = this.ViewState;
            MenuPreSea.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarPlannerList = new PhoenixToolbar();
            toolbarPlannerList.AddImageButton("../PreSea/PreSeaBatchWeeklyPlannerActual.aspx?TYPE=" + ViewState["TYPE"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            MenuPreSeaWeekPlanner.AccessRights = this.ViewState;
            //MenuPreSeaWeekPlanner.MenuList = toolbarPlannerList.Show();
            BindData();
            BindWeeklyPlan();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }  
    protected void txtDate_changed(object sender, EventArgs e)
    {
        //GridView gv = gvPreSea;
        //DropDownList ddlClassRoomAdd = (DropDownList)gv.FooterRow.FindControl("ddlClassRoomAdd");

        //if (ddlClassRoomAdd != null && txtDate.Text != "")
        //{
        //    ddlClassRoomAdd.Items.Clear();
        //    ListItem li = new ListItem("--Select--", "");
        //    ddlClassRoomAdd.Items.Add(li);

        //    DataTable dt = PhoenixPreSeaWeeklyPlanner.ListAvailableClassRoom
        //        (General.GetNullableDateTime(txtDate.Text), General.GetNullableInteger(ddlTimeSlot.SelectedValue));
        //    ddlClassRoomAdd.DataTextField = "FLDROOMNAME";
        //    ddlClassRoomAdd.DataValueField = "FLDROOMID";
        //    ddlClassRoomAdd.DataSource = dt;
        //    ddlClassRoomAdd.DataBind();
        //    BindData();
        //}

    }
    protected void BindTimeSlot()
    {
        ddlTimeSlot.Items.Clear();

        DataTable dt = PhoenixPreSeaBatch.ListPreSeaTimeSlot(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(Filter.CurrentPreSeaActualBatchSelection)
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
    //protected void ddlSemester_Changed(object sender, EventArgs e)
    //{
      
    //}
    protected void ddlTimeSlot_Changed(object sender, EventArgs e)
    {
        BindData();
    }
    protected void BatchPlanner_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("BATCHACTUAL"))
            {
                Response.Redirect("../PreSea/PreSeaBatchManagerActual.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("BUDDYPLANACTUAL"))
            {
                Response.Redirect("../PreSea/PreSeaMentorPlannerActual.aspx?type=2");
            }
            else if (dce.CommandName.ToUpper().Equals("MENTORPLANACTUAL"))
            {
                Response.Redirect("../PreSea/PreSeaMentorPlannerActual.aspx?type=1");
            }
            else
            {
                if (String.IsNullOrEmpty(Filter.CurrentPreSeaActualBatchSelection))
                {
                    ucError.ErrorMessage = "Please select batch to view/modify plan details.";
                    ucError.Visible = true;
                    return;
                }

                if (dce.CommandName.ToUpper().Equals("WEEKLYPLANACTUAL"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchWeeklyPlannerActual.aspx?TYPE=1");
                }
                else if (dce.CommandName.ToUpper().Equals("EXAM"))
                {
                    Response.Redirect("../PreSea/PreSeaBatchExamSchedule.aspx");
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

            string[] alColumns = { "FLDSUBJECTNAME", "FLDSTAFFNAME", "FLDROOMNAME", "FLDPRACTICALNAME", "FLDCOMPLETED" };
            string[] alCaptions = { "Subject", "Staff", "Class Room", "Practical", "Completed" };

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
            Response.AddHeader("Content-Disposition", "attachment; filename=Time Slot Details.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/sims1.png" + "' /></td>");
            Response.Write("<td><h3>Time Slot Details - Actual</h3></td>");
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
    protected void gvPreSea_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;

    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSUBJECTNAME", "FLDSTAFFNAME", "FLDROOMNAME", "FLDPRACTICALNAME", "FLDCOMPLETED" };
            string[] alCaptions = { "Subject", "Staff", "Class Room", "Practical", "Completed" };


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
                , General.GetNullableInteger(Filter.CurrentPreSeaActualBatchSelection)
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
                PhoenixPreSeaWeeklyPlanner.UpdateWeeklyTimeTableActualDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblWeeklyPlanIdEdit")).Text)
                        , General.GetNullableInteger(Filter.CurrentPreSeaActualCourseSelection)
                        , General.GetNullableInteger(Filter.CurrentPreSeaActualBatchSelection)
                        , General.GetNullableInteger(ddlSemester.SelectedSemester)
                        , General.GetNullableInteger(ddlSection.SelectedValue)
                        , General.GetNullableInteger(ddlTimeSlot.SelectedValue)
                        , General.GetNullableByte(((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkIsCompletedYN")).Checked ? "1" : "0")
                        , General.GetNullableInteger(ViewState["TYPE"].ToString()));
                _gridView.EditIndex = -1;
                BindData();
                BindWeeklyPlan();
            }
            else if (e.CommandName.ToUpper().Equals("PLANADD"))
            {
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
                //ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
                //if (db != null)
                //{
                //    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                //        db.Visible = false;
                //}
                //Label lblIsCompletedYNAdd = (Label)e.Row.FindControl("lblIsCompletedYNAdd");

                //if (lblIsCompletedYNAdd.Text.ToUpper() == "YES")
                //{
                //    db.Visible = false;
                //}
                //DropDownList ddlClassRoomAdd = (DropDownList)e.Row.FindControl("ddlClassRoomAdd");
                //if (ddlClassRoomAdd != null)
                //{
                //    ddlClassRoomAdd.Items.Clear();
                //    ListItem li = new ListItem("--Select--", "");
                //    ddlClassRoomAdd.Items.Add(li);
                //    ddlClassRoomAdd.DataValueField = "FLDROOMID";
                //    ddlClassRoomAdd.DataTextField = "FLDROOMNAME";
                //    DataTable dt = PhoenixPreSeaWeeklyPlanner.ListAvailableClassRoom
                //        (General.GetNullableDateTime(txtDate.Text), General.GetNullableInteger(ddlTimeSlot.SelectedValue));

                //    ddlClassRoomAdd.DataSource = dt;
                //    ddlClassRoomAdd.DataBind();
                //}
                //TextBox txtSubjectNameAdd = (TextBox)e.Row.FindControl("txtSubjectNameAdd");
                //TextBox txtsubjectId = (TextBox)e.Row.FindControl("txtsubjectId");
                //TextBox txtSubjectType = (TextBox)e.Row.FindControl("txtSubjectType");

                //if (txtsubjectId != null)
                //    txtsubjectId.Attributes.Add("style", "visibility:hidden");
                //if (txtSubjectType != null)
                //    txtSubjectType.Attributes.Add("style", "visibility:hidden");

                //TextBox txtFacultyAdd = (TextBox)e.Row.FindControl("txtFacultyAdd");
                //TextBox txtFacultyAddDesignation = (TextBox)e.Row.FindControl("txtFacultyAddDesignation");
                //TextBox txtFacultyAddId = (TextBox)e.Row.FindControl("txtFacultyAddId");
                //TextBox txtFacultyAddEmail = (TextBox)e.Row.FindControl("txtFacultyAddEmail");
                //if (txtFacultyAddDesignation != null)
                //    txtFacultyAddDesignation.Attributes.Add("style", "visibility:hidden");
                //if (txtFacultyAddId.Text != null)
                //    txtFacultyAddId.Attributes.Add("style", "visibility:hidden");
                //if (txtFacultyAddEmail != null)
                //    txtFacultyAddEmail.Attributes.Add("style", "visibility:hidden");


                //ImageButton imgSubjectAdd = (ImageButton)e.Row.FindControl("imgSubjectAdd");
                //if (imgSubjectAdd != null)
                //{
                //    imgSubjectAdd.Attributes.Add("onclick", "return showPickList('spnSubjectAdd', 'codehelp1', '', '../Common/CommonPickListPreSeaSubjects.aspx', true);");
                //    imgSubjectAdd.Visible = SessionUtil.CanAccess(this.ViewState, imgSubjectAdd.CommandName);

                //}

                //ImageButton imgFacultyAdd = (ImageButton)e.Row.FindControl("imgFacultyAdd");
                //if (imgFacultyAdd != null)
                //{
                //    DataTable dt = new DataTable();
                //    dt = PhoenixPreSeaBatchPlanner.ListPreSeaTrainingDepartmentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                //    string DepartmentList = dt.Rows[0]["FLDDEPARTMENTIDLIST"].ToString();

                //    imgFacultyAdd.Attributes.Add("onclick", "return showPickList('spnFacultyAdd', 'codehelp1', '', '../Common/CommonPickListPreSeaUser.aspx?departmentlist=" + DepartmentList + "', true);");
                //    imgFacultyAdd.Visible = SessionUtil.CanAccess(this.ViewState, imgFacultyAdd.CommandName);

                //}
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
            //BindData();
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

            dsGrid = PhoenixPreSeaWeeklyPlanner.ListWeeklyTimeTable(General.GetNullableInteger(Filter.CurrentPreSeaActualCourseSelection)
                                    , General.GetNullableInteger(Filter.CurrentPreSeaActualBatchSelection)
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
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
                        lblBrkSlot.Text = "BREAK " + "<br/>";
                        e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                        e.Row.Cells[1].Style.Add("font-weight", "bold");                      
                        e.Row.Cells[1].Controls.Add(lblBrkSlot);
                        
                    }
                    else
                    {
                        for (int i = 0; i < e.Row.Cells.Count; i++)
                        {
                            if (i > 0)
                            {

                                Literal ltrl = new Literal();
                                LinkButton lnkEdit = new LinkButton();
                                CheckBox chkActual = new CheckBox();
                                Label lblweekplanid = new Label();
                                Label lblTimeSlot = new Label();
                               
                                ltrl.ID = "ltrlDay" + i.ToString();
                                lnkEdit.ID = "lnkEdit" + i.ToString();
                                lblweekplanid.Visible = false;
                                lblTimeSlot.Visible = false;
                                if (!holidays.Contains(Convert.ToDateTime(Days.Rows[i - 1]["FLDCOLUMNDATE"]).ToShortDateString()))
                                {

                                    DataRow[] dr = data.Select("FLDCOLUMNID = '" + Days.Rows[i - 1]["FLDCOLUMNDATE"].ToString() + "' AND FLDROWID = '" + timeslots.Rows[e.Row.RowIndex]["FLDROWID"].ToString() + "'");
                                    /*Thoery Data*/
                                    if (dr != null && dr.Length == 1)
                                    {
                                        if (!holidays.Contains(dr[0]["FLDCOLUMNID"].ToString()))
                                        {
                                            lnkEdit.Text = "Edit";
                                            lnkEdit.CommandName = "EDITSLOT";
                                            lnkEdit.CommandArgument = dr[0]["FLDCOLUMNID"].ToString() + "|" + dr[0]["FLDROWID"].ToString();

                                            lblTimeSlot.Text = dr[0]["FLDROWID"].ToString();
                                            lblweekplanid.Text = dr[0]["FLDWEEKPLANID"].ToString();
                                            //chkActual.ID = lblweekplanid.Text + "|" + lblTimeSlot.Text;
                                            //chkActual.AutoPostBack = true;
                                            //chkActual.CheckedChanged += new EventHandler(chkActual_CheckedChanged); 
                                            
                                            if (!String.IsNullOrEmpty(dr[0]["FLDSUBJECTID"].ToString()))
                                                ltrl.Text = dr[0]["FLDSUBJECTNAME"].ToString();  

                                            ltrl.Text += "<br/>";
                                            e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                                            e.Row.Cells[i].Style.Add("font-weight", "bold");

                                            e.Row.Cells[i].Controls.Add(ltrl);
                                            e.Row.Cells[i].Controls.Add(lnkEdit);
                                            //if (dr[0]["FLDCOMPLETEDYN"].ToString().Equals("1"))
                                            //{
                                            //    chkActual.Checked = true;
                                            //}
                                           
                                            //e.Row.Cells[i].Controls.Add(chkActual);
                                        }

                                    }
                                    /*Practical Data*/
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
                                            }
                                            ltrl.Text += "<br/>";
                                            e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                                            e.Row.Cells[i].Style.Add("font-weight", "bold");

                                            e.Row.Cells[i].Controls.Add(ltrl);

                                            //if (dr[0]["FLDCOMPLETEDYN"].ToString().Equals("1"))
                                            //{
                                            //    chkActual.Checked = true;
                                               
                                            //}                                         
                                            lblweekplanid.Text = dr[0]["FLDCOMPLETEDYN"].ToString();
                                            e.Row.Cells[i].Controls.Add(lnkEdit); 
                                            //e.Row.Cells[i].Controls.Add(chkActual);                                       
                                        }
                                    }

                                }
                                else if (holidays.Contains(Convert.ToDateTime(Days.Rows[i - 1]["FLDCOLUMNDATE"]).ToShortDateString()))
                                {
                                    DataTable dt = PhoenixPreSeaHoliday.ListHoliday(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableDateTime(Convert.ToDateTime(Days.Rows[i - 1]["FLDCOLUMNDATE"]).ToString()));
                                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
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

                //int week = Convert.ToInt32(args[0]);
                //PhoenixPreSeaWeeklyPlanner.DeleteTimeSlotsForWholeWeek(PhoenixSecurityContext.CurrentSecurityContext.UserCode, week, args[1], args[2]);

                //ClearEntryScreen();
                //ClearPracticalDetails();
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
    private void EditTimeSlotData(string Date, string TimeSlotId)
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixPreSeaWeeklyPlanner.ListWeeklyTimeTableDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableInteger(Filter.CurrentPreSeaActualBatchSelection)
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


    protected void MenuPreSeaWeekPlanner_TabStripCommand(object sender, EventArgs e)
    {
        //DataSet dsGrid = new DataSet();
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                BindWeeklyPlan();

                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=PlannerList.xls");
                Response.Charset = "";
                //if (dsGrid.Tables.Count > 3 && dsGrid.Tables[3].Rows.Count > 0)
                //{
                string Header = "<table>";
                Header += "<tr><td colspan='2' rowspan='5'><img width='99' height='99' src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/sims.png" + "' /></td>";
                Header += "<td colspan='6' align='center'  style='font-weight:bold;'>Samundra Institute of Maritime Studies, Lonavla</td></tr>";
                Header += "<tr><td colspan='6'  align='center'  style='font-weight:bold;'>";
                Header += dsGrid.Tables[3].Rows[0]["FLDBATCH"].ToString();
                Header += "</td></tr>";
                Header += "<tr><td colspan='6'  align='center'  style='font-weight:bold;'>";
                Header += dsGrid.Tables[3].Rows[0]["FLDSEMESTERNAME"].ToString();
                Header += "</td></tr>";
                Header += "<tr><td colspan='6'  align='center'  style='font-weight:bold;'>";
                Header += dsGrid.Tables[3].Rows[0]["FLDSECTIONNAME"].ToString();
                Header += "</td></tr>";
                Header += "<tr><td colspan='6'  align='center'  style='font-weight:bold;'>";
                Header += dsGrid.Tables[3].Rows[0]["FLDWEEKPERIOD"].ToString();
                Header += "</td></tr><tr><td colspan='8'>&nbsp;</td></tr>";
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
                    //PrepareControlForExport(HdrRow);
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
                            // PrepareControlForExport(row);
                            table.Rows.Add(row);
                        }

                        TableRow tr = new TableRow();
                        table.Rows.Add(tr);

                        TableRow FooterRow = new TableRow();
                        TableCell Ftrcell = new TableCell();
                        Ftrcell.ColumnSpan = 13;
                        Ftrcell.Text = dsGrid.Tables[4].Rows[0][0].ToString();
                        FooterRow.Cells.Add(Ftrcell);

                        table.Rows.Add(FooterRow);

                        TableRow trow = new TableRow();
                        table.Rows.Add(trow);
                        table.Rows.Add(trow);
                        //Add PrePared by
                        TableRow FooterDet = new TableRow();
                        TableCell Ftrpreparedbycell = new TableCell();
                        Ftrpreparedbycell.ColumnSpan = 2;
                        Ftrpreparedbycell.Font.Bold = true;
                        Ftrpreparedbycell.Text = "Prepared By: " + dsGrid.Tables[5].Rows[0]["FLDPREPAREDBY"].ToString();
                        FooterDet.Cells.Add(Ftrpreparedbycell);
                        TableCell tc = new TableCell();
                        tc.ColumnSpan = 2;
                        FooterDet.Cells.Add(tc);
                        TableCell FtrTDC = new TableCell();
                        FtrTDC.ColumnSpan = 2;
                        FtrTDC.Font.Bold = true;
                        FtrTDC.Text = dsGrid.Tables[5].Rows[0]["FLDTDCINCHARGE"].ToString();
                        FooterDet.Cells.Add(FtrTDC);
                        TableCell tcell = new TableCell();
                        tcell.ColumnSpan = 2;
                        FooterDet.Cells.Add(tcell);
                        TableCell FtrApproved = new TableCell();
                        FtrApproved.ColumnSpan = 5;
                        FtrApproved.Font.Bold = true;
                        FtrApproved.Text = "Check & Approved By: " + dsGrid.Tables[5].Rows[0]["FLDCOURSEINCHARGE"].ToString();
                        FooterDet.Cells.Add(FtrApproved);
                        table.Rows.Add(FooterDet);

                        //second row
                        TableRow FooterDet1 = new TableRow();
                        TableCell Ftrpreparedby = new TableCell();
                        Ftrpreparedby.ColumnSpan = 2;
                        FooterDet1.Cells.Add(Ftrpreparedby);
                        TableCell tc1 = new TableCell();
                        tc1.ColumnSpan = 2;
                        FooterDet1.Cells.Add(tc1);
                        TableCell FtrTDC1 = new TableCell();
                        FtrTDC1.ColumnSpan = 2;
                        FtrTDC1.Font.Bold = true;
                        FtrTDC1.Text = "TDC in-charge";
                        FooterDet1.Cells.Add(FtrTDC1);
                        TableCell cell2 = new TableCell();
                        cell2.ColumnSpan = 2;
                        FooterDet1.Cells.Add(cell2);
                        TableCell FtrApproved1 = new TableCell();
                        FtrApproved1.ColumnSpan = 5;
                        FtrApproved1.Font.Bold = true;
                        FtrApproved1.Text = "Course in-charge & Acting Dean,Marine Engineering";
                        FooterDet1.Cells.Add(FtrApproved1);
                        table.Rows.Add(FooterDet1);

                    }
                }
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


    protected void chkActual_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;

        string arg = chk.ID;
        string[] args = new string[2];
        args = arg.Split('|');

        if (General.GetNullableInteger(args[0]).HasValue && General.GetNullableInteger(args[1]).HasValue)
        {
            PhoenixPreSeaWeeklyPlanner.UpdateWeeklyTimeTableActualDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableInteger(args[0].ToString())
                        , General.GetNullableInteger(Filter.CurrentPreSeaActualCourseSelection)
                        , General.GetNullableInteger(Filter.CurrentPreSeaActualBatchSelection)
                        , General.GetNullableInteger(ddlSemester.SelectedSemester)
                        , General.GetNullableInteger(ddlSection.SelectedValue)
                        , General.GetNullableInteger(args[1].ToString())
                        , General.GetNullableByte(chk.Checked ? "1" : "0")
                        , General.GetNullableInteger(ViewState["TYPE"].ToString()));

        }
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
    protected void ddlSemester_TextChangedEvent(object sender, EventArgs e)
    {
        BindTimeSlot();
    }
}
