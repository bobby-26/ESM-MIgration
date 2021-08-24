using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;

public partial class CrewOffshorePendingCourses : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");            

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["coursetype"] = "";
                if (Request.QueryString["coursetype"] != null && Request.QueryString["coursetype"].ToString() != "")
                    ViewState["coursetype"] = Request.QueryString["coursetype"].ToString();
                else
                    ViewState["coursetype"] = PhoenixCommonRegisters.GetHardCode(1, 103, "4");                
            }

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Pending Courses", "PENDINGCOURSE");
            toolbarsub.AddButton("Completed Courses", "COMPLETEDCOURSE");
            CrewQuery.AccessRights = this.ViewState;
            CrewQuery.MenuList = toolbarsub.Show();
            CrewQuery.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshorePendingCourses.aspx?coursetype=" + ViewState["coursetype"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvPendingCourse')", "Print Grid", "icon_print.png", "PRINT");
            //toolbar.AddImageButton("../CrewOffshore/CrewOffshorePendingCourses.aspx", "Find", "search.png", "FIND");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbar.Show();

            PhoenixToolbar toolbarcourse = new PhoenixToolbar();
            toolbarcourse.AddButton("CBT", "CBT");
            toolbarcourse.AddButton("HSEQA Training", "HSEQA");
            toolbarcourse.AddButton("Seagull", "SEAGULL");
            CourseRequest.AccessRights = this.ViewState;
            CourseRequest.MenuList = toolbarcourse.Show();
            if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
                CourseRequest.SelectedMenuIndex = 0;
            else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
                CourseRequest.SelectedMenuIndex = 1;
            else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "8"))
                CourseRequest.SelectedMenuIndex = 2;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("COMPLETEDCOURSE"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCompletedCourses.aspx", true);
        }
    }

    protected void CourseRequest_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        string coursetype = "";

        if (dce.CommandName.ToUpper().Equals("CBT"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
            Response.Redirect("../CrewOffshore/CrewOffshorePendingCourses.aspx?coursetype=" + coursetype, true);
        }
        else if (dce.CommandName.ToUpper().Equals("HSEQA"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../CrewOffshore/CrewOffshorePendingCourses.aspx?coursetype=" + coursetype, true);
        }
        else if (dce.CommandName.ToUpper().Equals("SEAGULL"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "8");
            Response.Redirect("../CrewOffshore/CrewOffshorePendingCourses.aspx?coursetype=" + coursetype, true);
        }
    }

    protected void CrewQueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDVESSELNAME", "FLDNAME", "FLDFILENO", "FLDRANKNAME", "FLDCOURSE", "FLDEXAMNAME", "FLDTOBEDONEBY", "FLDSCORE", "FLDEXAMRESULT", "FLDDATEATTENDED" };
                string[] alCaptions = { "Vessel", "Employee Name", "File No", "Rank", "Course", "Exam", "To be done by", "Score %", "Result", "Date Attended" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataTable dt = PhoenixCrewOffshoreTrainingNeeds.CourseRequestSearch(null,null,null,null
                                                                                       , sortexpression, sortdirection
                                                                                       , 1, iRowCount
                                                                                       , ref iRowCount, ref iTotalPageCount
                                                                                       , 0
                                                                                       , General.GetNullableInteger(ViewState["coursetype"].ToString()));
                General.ShowExcel("Pending Courses", dt, alColumns, alCaptions, sortdirection, sortexpression);
            }
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
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

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDNAME", "FLDFILENO", "FLDRANKNAME", "FLDCOURSE", "FLDEXAMNAME", "FLDTOBEDONEBY", "FLDSCORE", "FLDEXAMRESULT", "FLDDATEATTENDED" };
        string[] alCaptions = { "Vessel", "Employee Name", "File No", "Rank", "Course", "Exam", "To be done by", "Score %", "Result", "Date Attended" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = PhoenixCrewOffshoreTrainingNeeds.CourseRequestSearch(null, null, null, null
                                                                       , sortexpression, sortdirection
                                                                       , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                                       , ref iRowCount, ref iTotalPageCount, 0
                                                                       , General.GetNullableInteger(ViewState["coursetype"].ToString()));

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvPendingCourse", "Pending Courses", alCaptions, alColumns, ds);
            if (dt.Rows.Count > 0)
            {
                gvPendingCourse.DataSource = dt;
                gvPendingCourse.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvPendingCourse);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPendingCourse_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvPendingCourse_RowDataBound(object sender, GridViewRowEventArgs e)
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
            LinkButton lnkEmployeeName = (LinkButton)e.Row.FindControl("lnkEmployeeName");
            Label lblEmployeeid = (Label)e.Row.FindControl("lblEmployeeid");
            if (lnkEmployeeName != null)
            {
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:Openpopup('chml','','../CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
            }

            ImageButton cmdExamReq = (ImageButton)e.Row.FindControl("cmdExamReq");
            if (cmdExamReq != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdExamReq.CommandName))
                    cmdExamReq.Visible = false;

                cmdExamReq.Attributes.Add("onclick", "javascript:parent.Openpopup('chml','','../CrewOffshore/CrewOffshoreInitiateExamRequest.aspx?courserequestid=" + drv["FLDCOURSEREQUESTID"].ToString() + "&examid=" + drv["FLDEXAMID"].ToString()
                     + "&courseid=" + drv["FLDCOURSEID"].ToString() + "'); return true;");
            }
        }
    }

    protected void gvPendingCourse_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = sender as GridView;
        //Filter.CurrentVesselCrewSelection = ((Label)_gridView.Rows[e.NewEditIndex].FindControl("lblEmployeeid")).Text;
        //Response.Redirect("..\\VesselAccounts\\VesselAccountsEmployeeGeneral.aspx", false);
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
            gvPendingCourse.SelectedIndex = -1;
            gvPendingCourse.EditIndex = -1;
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }
}
