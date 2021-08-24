using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Presea_PreSeaMentorPlanner : PhoenixBasePage
{
    int plantype;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        try
        {
            if (Request.QueryString["type"] != null)
            {
                if (Request.QueryString["type"] == "1")
                {
                    MentorPlannerTitle.Text = "Mentor Planner";
                    plantype = 1;
                }
                else if (Request.QueryString["type"] == "2")
                {
                    MentorPlannerTitle.Text = "Buddy Planner";
                    plantype = 2;
                }
            }

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

            MenuMentorPlanner.AccessRights = this.ViewState;
            MenuMentorPlanner.MenuList = MainToolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (plantype == 1)
                    MenuMentorPlanner.SelectedMenuIndex = 3;
                if (plantype == 2)
                    MenuMentorPlanner.SelectedMenuIndex = 2;
                txtFacultyDesignation.Attributes.Add("style", "display:none");
                txtFacultyId.Attributes.Add("style", "display:none");
                txtFacultyEmail.Attributes.Add("style", "display:none");
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../PreSea/PreSeaMentorPlanner.aspx?" + Request.QueryString, "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvMentorPlan')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../PreSea/PreSeaMentorPlanner.aspx?" + Request.QueryString, "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../PreSea/PreSeaMentorPlanner.aspx?" + Request.QueryString, "Clear", "clear-filter.png", "CLEAR");
            toolbargrid.AddImageLink("../PreSea/PreSeaMentorPlanner.aspx?" + Request.QueryString, "Add Plan", "add.png", "ADD");

            MenuPreSea.AccessRights = this.ViewState;
            MenuPreSea.MenuList = toolbargrid.Show();
            BindData();
            SetPageNavigator();
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

    protected void MenuMentorPlanner_TabStripCommand(object sender, EventArgs e)
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
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
           
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDMENTORNAME", "FLDROOMSHORTNAME", "FLDSTRENGTH", "FLDDATE", "FLDREMARKS" };
            string[] alCaptions = { "S.No", "Name", "Class Room", "Strength", "Date", "Remarks" };

            string sortexpression;
            int? sortdirection = 1;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixPreseaMentorPlanner.PreSeaMentorPlannerSearch(General.GetNullableInteger(txtFacultyId.Text)
                                                                           , General.GetNullableInteger(ddlYear.SelectedYear)
                                                                           , General.GetNullableInteger(ddlMonth.SelectedMonthNumber)
                                                                           , plantype
                                                                           , sortexpression
                                                                           , sortdirection
                                                                           , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                           , General.ShowRecords(null)
                                                                           , ref iRowCount
                                                                           , ref iTotalPageCount
                                                                          );

            if (ds.Tables.Count > 0)
                General.ShowExcel(MentorPlannerTitle.Text, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        if (dce.CommandName.ToUpper().Equals("CLEAR"))
        {
            txtFaculty.Text = "";
            txtFacultyDesignation.Text = "";
            txtFacultyId.Text = "";
            txtFacultyEmail.Text = "";
            ddlMonth.SelectedMonthNumber = "";
            ddlYear.SelectedYear = "";
        }
        if (dce.CommandName.ToUpper().Equals("ADD"))
        {
            String scriptpopup = String.Format("javascript:parent.Openpopup('codehelp1','','../PreSea/PreSeaMentorPlannerAdd.aspx?type="+plantype+"');");

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
        }
    }

    protected void gvMentorPlan_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            DataRowView rv = (DataRowView)e.Row.DataItem;
            string mentorplannerId = rv["FLDMENTORPLANID"].ToString();
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                eb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', '../PreSea/PreSeaMentorPlannerAdd.aspx?type="+plantype+"&mentorplannerId=" + mentorplannerId + "');return true;");                
            }

            ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");
            if (cmdDelete != null)
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete the faculty?')");
        }
    }
   

    protected void gvMentorPlan_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            _gridView.SelectedIndex = nCurrentRow;
            string mentorplannerId = _gridView.DataKeys[nCurrentRow].Value.ToString();
            if (e.CommandName.ToString().ToUpper() == "EDIT")
            {
                String scriptpopup = String.Format("javascript:parent.Openpopup('codehelp1','','../PreSea/PreSeaMentorPlannerAdd.aspx?type=" + plantype + "&mentorplannerId='" + mentorplannerId + "'');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
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
       
        string[] alColumns = { "FLDROWNUMBER", "FLDMENTORNAME", "FLDROOMSHORTNAME", "FLDSTRENGTH", "FLDDATE", "FLDREMARKS" };
        string[] alCaptions = { "S.No", "Name", "Class Room", "Strength", "Date", "Remarks" };   

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());       

        DataSet ds = PhoenixPreseaMentorPlanner.PreSeaMentorPlannerSearch(General.GetNullableInteger(txtFacultyId.Text)
                                                                            , General.GetNullableInteger(ddlYear.SelectedYear)
                                                                            , General.GetNullableInteger(ddlMonth.SelectedMonthNumber)
                                                                            , plantype
                                                                            , sortexpression
                                                                            , sortdirection                                                                            
                                                                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                            , General.ShowRecords(null)
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount
                                                                           );
       
        General.SetPrintOptions("gvMentorPlan", MentorPlannerTitle.Text, alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvMentorPlan.DataSource = ds;
            gvMentorPlan.DataBind();           
        }
        else
        {
            ShowNoRecordsFound(ds.Tables[0], gvMentorPlan);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvMentorPlan_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentrow = de.RowIndex;
            string mentorPlanId = _gridView.DataKeys[nCurrentrow].Value.ToString();
            //string mentorPlanId = ((Label)_gridView.Rows[nCurrentrow].FindControl("lblCourseContactId")).Text;
            PhoenixPreseaMentorPlanner.PreSeaMentorPlannerDelete(General.GetNullableInteger(mentorPlanId).Value);
            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();
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
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvMentorPlan.SelectedIndex = -1;
            gvMentorPlan.EditIndex = -1;
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
    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    protected void imgFaculty_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = PhoenixPreSeaBatchPlanner.ListPreSeaTrainingDepartmentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        string DepartmentList = dt.Rows[0]["FLDDEPARTMENTIDLIST"].ToString();
        
        string str = "showPickList('spnFacultyAdd', 'codehelp1', '', '../Common/CommonPickListPreSeaUser.aspx?departmentlist=" + DepartmentList + "', true);";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "codehelp1", str, true);        
    }
}