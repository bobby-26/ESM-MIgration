using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class CrewOffshoreTrainingCourseRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            if (!IsPostBack)
            {
                ViewState["trainingneedid"] = "";                
                ViewState["employeeid"] = "";
                ViewState["vesselid"] = "";
                ViewState["tobedoneby"] = "";
                ViewState["category"] = "";
                ViewState["subcategory"] = "";
                ViewState["courselist"] = "";

                if (Request.QueryString["trainingneedid"] != null && Request.QueryString["trainingneedid"].ToString() != "")
                    ViewState["trainingneedid"] = Request.QueryString["trainingneedid"].ToString();

                EditTrainingNeed();

                //PhoenixToolbar toolbar = new PhoenixToolbar();                
                //toolbar.AddButton("Initiate Course Request", "COURSEREQUEST");
                //CrewMenu.AccessRights = this.ViewState;
                //CrewMenu.MenuList = toolbar.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                BindCourses();
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

    protected void EditTrainingNeed()
    {
        if (ViewState["trainingneedid"] != null && ViewState["trainingneedid"].ToString() != "")
        {
            DataTable dt = PhoenixCrewOffshoreTrainingNeeds.EditTrainingNeed(new Guid(ViewState["trainingneedid"].ToString()));
            if (dt.Rows.Count > 0)
            {
                ViewState["employeeid"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                ViewState["vesselid"] = dt.Rows[0]["FLDVESSELID"].ToString();
                ViewState["tobedoneby"] = dt.Rows[0]["FLDTOBEDONEBY"].ToString();
                ViewState["category"] = dt.Rows[0]["FLDCATEGORY"].ToString();
                ViewState["subcategory"] = dt.Rows[0]["FLDSUBCATEGORY"].ToString();
            }
        }
    }

    protected void BindCourses()
    {
        cblCourseList.DataSource = PhoenixCrewOffshoreTrainingNeeds.ListTrainingCourse(General.GetNullableInteger(ViewState["category"].ToString())
            , General.GetNullableInteger(ViewState["subcategory"].ToString()), General.GetNullableGuid(ViewState["trainingneedid"].ToString()));
        cblCourseList.DataTextField = "FLDCOURSE";
        cblCourseList.DataValueField = "FLDDOCUMENTID";
        cblCourseList.DataBind();

        //if (cblCourseList.Items.Count == 0)
        //{
        //    lblNil.Visible = true;
        //    cblCourseList.Visible = false;
        //}

    }

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("COURSEREQUEST"))
            {
                ViewState["courselist"] = General.ReadCheckBoxList(cblCourseList);

                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                
                foreach(ListItem course in cblCourseList.Items)
                {
                    if (course.Selected)
                    {
                        PhoenixCrewOffshoreTrainingNeeds.InsertCourseRequest(General.GetNullableGuid(ViewState["trainingneedid"].ToString())
                        , General.GetNullableInteger(ViewState["vesselid"].ToString())
                        , General.GetNullableInteger(ViewState["employeeid"].ToString())
                        , General.GetNullableInteger(course.Value)
                        , General.GetNullableInteger(ViewState["tobedoneby"].ToString())
                        );
                    }
                }
                ucStatus.Text = "Course request initiated successfully.";
                BindCourses();
                BindData();
                SetPageNavigator();
                ViewState["courselist"] = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ViewState["courselist"].ToString()) == null)
            ucError.ErrorMessage = "Please select atlease one course.";

        return (!ucError.IsError);
    }

    public void BindData()
    {
        try
        {
            string[] alColumns = { "FLDCOURSE", "FLDCOMPLETEDYNNAME", "FLDCOMPLETIONDATE" };
            string[] alCaptions = { "Course", "Completed Y/N", "Completion Date" };

            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixCrewOffshoreTrainingNeeds.CourseRequestSearch(
                                                        General.GetNullableGuid(ViewState["trainingneedid"].ToString())
                                                        , General.GetNullableInteger(ViewState["vesselid"].ToString())
                                                        , General.GetNullableInteger(ViewState["employeeid"].ToString())
                                                        , null
                                                        , sortexpression, sortdirection
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , General.ShowRecords(null)
                                                        , ref iRowCount
                                                        , ref iTotalPageCount
                                                        );

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvCourseReq", "Course Request", alCaptions, alColumns, ds);

            if (dt.Rows.Count > 0)
            {
                gvCourseReq.DataSource = dt;
                gvCourseReq.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvCourseReq);
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

    protected void ShowExcel()
    {
        string[] alColumns = { "FLDCOURSE", "FLDCOMPLETEDYNNAME", "FLDCOMPLETIONDATE" };
        string[] alCaptions = { "Course", "Completed Y/N", "Completion Date" };

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.CourseRequestSearch(
                                                        General.GetNullableGuid(ViewState["trainingneedid"].ToString())
                                                        , General.GetNullableInteger(ViewState["vesselid"].ToString())
                                                        , General.GetNullableInteger(ViewState["employeeid"].ToString())
                                                        , null
                                                        , sortexpression, sortdirection
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , iRowCount
                                                        , ref iRowCount
                                                        , ref iTotalPageCount
                                                        );
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.ShowExcel("Course Request", dt, alColumns, alCaptions, sortdirection, sortexpression);       
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
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
    }
    protected void cmdGo_Click(object sender, EventArgs e)
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvCourseReq.SelectedIndex = -1;
        gvCourseReq.EditIndex = -1;
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
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
