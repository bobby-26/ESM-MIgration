using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewFacultyCourseAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewFacultyCourseAdd.aspx?"+ Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCourseList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Crew/CrewFacultyCourseAdd.aspx?" + Request.QueryString, "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Crew/CrewFacultyCourseAdd.aspx?" + Request.QueryString, "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");        
        MenuCourse.AccessRights = this.ViewState;
        MenuCourse.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Add", "SAVE",ToolBarDirection.Right);
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvCourseList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }               
    }
    protected void MenuTitle_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            string facultyId = Request.QueryString["facultyId"].ToString();
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem dataItem in gvCourseList.MasterTableView.Items)
                {
                    RadCheckBox cb = (RadCheckBox)dataItem.FindControl("chkSelect");
                    string courseId = dataItem.GetDataKeyValue("FLDDOCUMENTID").ToString();
                    if (cb.Checked == true)
                    {
                        if (Request.QueryString["from"] != null)
                        {
                            string instituteId = Request.QueryString["instituteId"].ToString();
                            string calendarid = Request.QueryString["calendarId"].ToString();

                            PhoenixCrewInstituteFacultyPlanner.CrewFacultyCalendarInsert(General.GetNullableInteger(facultyId).Value
                                                                                        , General.GetNullableInteger(instituteId)
                                                                                        , General.GetNullableInteger(courseId)
                                                                                        , General.GetNullableGuid(calendarid));
                        }
                        else
                        {
                            PhoenixCrewInstituteFaculty.CrewInstituteCourseContactInsert(General.GetNullableInteger(courseId).Value
                                                                                        , General.GetNullableInteger(facultyId).Value);
                        }
                        cout++;
                       
                    }
                }
                if (cout == 0)
                {
                    ucError.ErrorMessage = "Select one or more course to add";
                    ucError.Visible = true;
                }
                BindData();
                gvCourseList.Rebind();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1','',true);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCourse_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvCourseList.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDABBREVIATION", "FLDCOURSE" };
                string[] alCaptions = { "Course Code", "Course" };

                string sortexpression;
                int? sortdirection = 1;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
                string instituteId = Request.QueryString["instituteId"];

                DataSet ds = PhoenixCrewCourseInitiation.CrewCourseInstituteSearch(General.GetNullableString(txtCourseName.Text)
                                                                       , General.GetNullableString(txtCourseCode.Text)
                                                                       , null
                                                                       , General.GetNullableInteger(instituteId)
                                                                       , null
                                                                       , null
                                                                       , sortexpression
                                                                       , sortdirection
                                                                       , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                       , gvCourseList.PageSize
                                                                       , ref iRowCount
                                                                       , ref iTotalPageCount);
               
                if (ds.Tables.Count > 0)
                    General.ShowExcel("Institute Faculty", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtCourseCode.Text = "";
                txtCourseName.Text = "";
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvCourseList.Rebind();
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

        string[] alColumns = { "FLDABBREVIATION", "FLDCOURSE" };
        string[] alCaptions = { "Course Code", "Course"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string instituteId = Request.QueryString["instituteId"];

        DataSet ds = PhoenixCrewCourseInitiation.CrewCourseInstituteSearch(General.GetNullableString(txtCourseName.Text)
                                                                        , General.GetNullableString(txtCourseCode.Text)
                                                                        , null
                                                                        , General.GetNullableInteger(instituteId)
                                                                        , null
                                                                        , null
                                                                        , sortexpression
                                                                        , sortdirection
                                                                        , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                        , gvCourseList.PageSize
                                                                        , ref iRowCount
                                                                        , ref iTotalPageCount);

        General.SetPrintOptions("gvCourseList", "Course List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCourseList.DataSource = ds;
            gvCourseList.VirtualItemCount = iRowCount;
        }
        else
        {
            gvCourseList.DataSource = "";
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvCourseList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCourseList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvCourseList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCourseList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCourseList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

           // string facultyId = gvCourseList.DataKeys[e.Row.RowIndex].Value.ToString();
        }
    }
}