using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;

public partial class PreSeaBatchSemesterExamResults : PhoenixBasePage
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
            MenuBatchPlanner.SelectedMenuIndex = 7;

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../PreSea/PreSeaBatchSemesterExamResults.aspx", "Export to Excel", "icon_xls.png", "Excel");

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
                ViewState["PLANID"] = "";
                ViewState["SUBJECTID"] = "";
                ViewState["YEAR"] = "";
                if (Session["BATCHMANAGECOURSE"] != null && Session["BATCHMANAGECOURSE"].ToString() != "")
                {
                    ddlCourse.SelectedCourse = Session["BATCHMANAGECOURSE"].ToString();
                    ddlCourse.Enabled = false;
                    ddlSemester.Course = Session["BATCHMANAGECOURSE"].ToString();
                }
                ucBatch.SelectedBatch = Filter.CurrentPreSeaBatchManagerSelection;
                ucBatch.Enabled = false;
                
                //if (Request.QueryString["PLANID"]!= null && Request.QueryString["PLANID"].ToString() != "")
                //{
                //    ViewState["PLANID"] = Request.QueryString["PLANID"].ToString();
                //    ddlPlan.SelectedValue = ViewState["PLANID"].ToString();
                //}
                if (Request.QueryString["SEMESTERPLANID"] != null && Request.QueryString["SEMESTERPLANID"].ToString() != "")
                {
                    ViewState["SEMESTERPLANID"] = Request.QueryString["SEMESTERPLANID"].ToString();
                }
                if (Request.QueryString["SUBJECTID"] != null && Request.QueryString["SUBJECTID"].ToString() != "")
                {
                    ViewState["SUBJECTID"] = Request.QueryString["SUBJECTID"].ToString();
                }
                if (Request.QueryString["YEAR"] != null && Request.QueryString["YEAR"].ToString() != "")
                {
                    ViewState["YEAR"] = Request.QueryString["YEAR"].ToString();
                    ddlYear.QuickList = PhoenixRegistersQuick.ListQuick(1, 55);
                    ddlYear.DataBind();

                    ddlYear.SelectedQuick = Request.QueryString["YEAR"].ToString();

                }
               // BindPlan();
                
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
    //protected void BindPlan()
    //{
    //    ddlPlan.Items.Clear();
        
    //    //ListItem li = new ListItem("--Select--", "");
    //    //ddlPlan.Items.Add(li);

    //    DataTable dt = PhoenixPreSeaWeeklyPlanner.ListSemesterPlan(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                , General.GetNullableInteger(Session["BATCHMANAGECOURSE"].ToString())
    //                , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection));
    //    if (dt.Rows.Count > 0)
    //    {
    //        ddlPlan.DataTextField = "FLDPLANNAME";
    //        ddlPlan.DataValueField = "FLDPLANID";
    //        ddlPlan.DataSource = dt;
    //        ddlPlan.DataBind();
    //    }
    //    else
    //    {
    //        ListItem li = new ListItem("--Select--", "");
    //        ddlPlan.Items.Add(li);
    //    }
    //}

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
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDTRBATCHROLLNUMBER", "FLDNAME","FLDSUBJECTNAME", "FLDMARKS", "FLDPASSFAIL" };
            string[] alCaptions = { "Batch Roll No", "Name","Subject", "Mark", "Pass/Fail" };

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

            ds = PhoenixPreSeaBatchExamResults.SearchPreSeaSemesterExamTrainee(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableInteger(Session["BATCHMANAGECOURSE"].ToString())
                                                                , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                                                                , General.GetNullableInteger(ddlYear.SelectedQuick)
                                                                , General.GetNullableInteger(ddlSemester.SelectedSemester)
                                                                , General.GetNullableInteger(ViewState["SEMESTERPLANID"].ToString())
                                                                , sortexpression, sortdirection
                                                                , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                                , ref iRowCount, ref iTotalPageCount);



            //     General.ShowExcel("Time Slot Details", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            Response.AddHeader("Content-Disposition", "attachment; filename=SemesterResult.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/sims1.png" + "' /></td>");
            Response.Write("<td><h3>Semester Results</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td> <b> Course </b> </td> <td>" + ddlCourse.SelectedName + "</td>");
            Response.Write("<td></td><td> <b> Batch </b></td><td>" + ucBatch.SelectedName + "</td></tr>");
            Response.Write("<tr><td> <b> Year </b> </td> <td>" + ddlYear.SelectedText + "</td>");
            Response.Write("<td></td><td> <b> Semester </b></td><td>" + ddlSemester.SelectedSemester + "</td></tr>");
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
            string[] alColumns = { "FLDTRBATCHROLLNUMBER", "FLDNAME", "FLDMARKS", "FLDPASSFAIL" };
            string[] alCaptions = {"Batch Roll No", "Name", "Marks","Pass/Fail" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = new DataSet();

            ds = PhoenixPreSeaBatchExamResults.SearchPreSeaSemesterExamTrainee(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , General.GetNullableInteger(Session["BATCHMANAGECOURSE"].ToString())
                                                        , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                                                        , General.GetNullableInteger(ddlYear.SelectedQuick)
                                                        , General.GetNullableInteger(ddlSemester.SelectedSemester)
                                                        , General.GetNullableInteger(ViewState["SEMESTERPLANID"].ToString())
                                                        , sortexpression, sortdirection
                                                        , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                        , ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvPreSea", "Exam Results", alCaptions, alColumns, ds);
           
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


    protected void gvPreSea_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            //int WeeklyPlanId ;
            GridView _gridView = (GridView)sender;
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                if (_gridView.EditIndex > -1)
                    _gridView.UpdateRow(_gridView.EditIndex, false);
                _gridView.EditIndex = nCurrentRow;
                //_gridView.SelectedIndex = de.NewEditIndex;

                BindData();
                SetPageNavigator();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (IsValidResult(Session["BATCHMANAGECOURSE"].ToString()
                                        , Filter.CurrentPreSeaBatchManagerSelection
                                        , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectIdEdit")).Text
                                        , ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtMarkEdit")).Text))
                {
                    string TraineeExamid;

                    TraineeExamid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTraineeExamidEdit")).Text;
                    if (TraineeExamid != null && TraineeExamid !="")
                    {
                        PhoenixPreSeaBatchExamResults.UpdatePreSeaSemesterExamTrainee(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                 , General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblTraineeExamidEdit")).Text)
                                                                 , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSemesterPlanIdEdit")).Text)
                                                                 , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectIdEdit")).Text)
                                                                 , General.GetNullableDecimal(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtMarkEdit")).Text)
                                                                 , null
                                                                 , null
                                                                 , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeidEdit")).Text)
                                                                 , 1
                                                                 );
                        _gridView.EditIndex = -1;
                        ucStatus.Text = "Mark is updated successfully";
                        BindData();
                        SetPageNavigator();
                      

                    }
                    else
                    {
                        PhoenixPreSeaBatchExamResults.InsertPreSeaSemesterExamTrainee(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableInteger(Session["BATCHMANAGECOURSE"].ToString())
                                                                , General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                                                                , General.GetNullableInteger(ddlYear.SelectedQuick)
                                                                , General.GetNullableInteger(ddlSemester.SelectedSemester)
                                                                , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSemesterPlanIdEdit")).Text)
                                                                , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubjectIdEdit")).Text)
                                                                , General.GetNullableDecimal(((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtMarkEdit")).Text)
                                                                , null
                                                                , null
                                                                , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeidEdit")).Text)
                                                                , 1
                                                                );
                        _gridView.EditIndex = -1;
                        ucStatus.Text = "Mark is updated successfully";
                        BindData();
                        SetPageNavigator();
                       

                    }
                    //ucStatus.Text = "Mark is updated successfully";                    
                    //   BindData();
                    //    SetPageNavigator();
                    //    _gridView.EditIndex = -1;

                }
                else
                {
                    ucError.Visible = true;
                    return;

                }
            }         
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
               

            }
            else if (e.CommandName.ToUpper().Equals("CANCEL"))
            {
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
        SetPageNavigator();
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

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvPreSea_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvPreSea_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
        SetPageNavigator();
    }

    protected void gvPreSea_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        //ucStatus.Text = "Mark is updated successfully";
        BindData();
        SetPageNavigator();
    }
    private bool IsValidResult(string courseid, string batchid, string subjectid, string marks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(courseid) == null)
            ucError.ErrorMessage = "Course is Required.";

        if (General.GetNullableInteger(batchid) == null)
            ucError.ErrorMessage = "Batch is Required.";

        if (General.GetNullableInteger(ddlYear.SelectedQuick) == null)
            ucError.ErrorMessage = "Year is Required";

        if (General.GetNullableInteger(ddlSemester.SelectedSemester) == null)
            ucError.ErrorMessage = "Semester is Required";

        if (General.GetNullableInteger(subjectid) == null)
            ucError.ErrorMessage = "Subject is required.";

        if (General.GetNullableDecimal(marks) == null)
            ucError.ErrorMessage = "Marks is required.";

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
            SetPageNavigator();
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
    private void SetPageNavigator()
    {
        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
        {
            return true;
        }

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
        try
        {
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvPreSea.SelectedIndex = -1;
            gvPreSea.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



}

