using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Presea_PreSeaMentorPlannerActual : PhoenixBasePage
{
    int plantype;
    protected void Page_Load(object sender, EventArgs e)
    {
       
        try
        {
            if (Request.QueryString["type"] != null)
            {
                if (Request.QueryString["type"] == "1")
                {
                    MentorPlannerTitle.Text = "Mentor Planner Actual";
                    plantype = 1;
                }
                else if (Request.QueryString["type"] == "2")
                {
                    MentorPlannerTitle.Text = "Buddy Planner Actual";
                    plantype = 2;
                }
            }

            SessionUtil.PageAccessRights(ViewState);
            PhoenixToolbar MainToolbar = new PhoenixToolbar();

            MainToolbar.AddButton("Batch", "BATCHACTUAL");
            MainToolbar.AddButton("Weekly Planner Actual", "WEEKLYPLANACTUAL");
            MainToolbar.AddButton("Buddy Planner", "BUDDYPLANACTUAL");
            MainToolbar.AddButton("Mentoring Planner", "MENTORPLANACTUAL");

            MenuMentorPlanner.AccessRights = this.ViewState;
            MenuMentorPlanner.MenuList = MainToolbar.Show();

            if (plantype == 1)
                MenuMentorPlanner.SelectedMenuIndex = 3;
            if (plantype == 2)
                MenuMentorPlanner.SelectedMenuIndex = 2;
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../PreSea/PreSeaMentorPlannerActual.aspx?" + Request.QueryString, "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvMentorPlan')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../PreSea/PreSeaMentorPlannerActual.aspx?" + Request.QueryString, "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../PreSea/PreSeaMentorPlannerActual.aspx?" + Request.QueryString, "Clear", "clear-filter.png", "CLEAR");
            //toolbargrid.AddImageLink("../PreSea/PreSeaMentorPlanner.aspx?" + Request.QueryString, "Add Plan", "add.png", "ADD");

            MenuPreSea.AccessRights = this.ViewState;
            MenuPreSea.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                txtFacultyDesignation.Attributes.Add("style", "display:none");
                txtFacultyId.Attributes.Add("style", "display:none");
                txtFacultyEmail.Attributes.Add("style", "display:none");

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

    protected void MenuMentorPlanner_TabStripCommand(object sender, EventArgs e)
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

    protected void gvMentorPlan_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
    }

    protected void gvMentorPlan_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
    }

    protected void gvMentorPlan_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            //DropDownList ddlSession = (DropDownList)e.Row.FindControl("ddlSession");
            //if(ddlSession!=null)
            //{
            //    DataRowView rv = (DataRowView)e.Row.DataItem;
            //    ddlSession.SelectedValue = rv["FLDSESSION"].ToString();
            //}
            
        }
    }

    //protected void gvMentorPlan_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        //int WeeklyPlanId ;
    //        GridView _gridView = (GridView)sender;
    //        if (e.CommandName.ToUpper().Equals("EDIT"))
    //        {
    //            if (_gridView.EditIndex > -1)
    //                _gridView.UpdateRow(_gridView.EditIndex, false);
    //            _gridView.EditIndex = nCurrentRow;
    //            //_gridView.SelectedIndex = de.NewEditIndex;
    //            int nCurrentRow = e.RowIndex;
    //            instituteId = Request.QueryString["instituteId"].ToString();
    //            faculty = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFacultyEdit")).Text;
    //            role = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFacultyRoleEdit")).Text;
    //            initial = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtFacultyInitialEdit")).Text;
    //            string facultyId = _gridView.DataKeys[nCurrentRow].Value.ToString();

    //            PhoenixPreseaMentorPlanner.PreSeaMentorPlannerUpdate(General.GetNullableInteger(ddlFaculty.SelectedValue).Value
    //                                                                       , General.GetNullableDateTime(txtDate.Text).Value
    //                                                                       , classroom
    //                                                                       , General.GetNullableInteger(txtStrength.Text)
    //                                                                       , General.GetNullableInteger(ddlsession.SelectedValue).Value
    //                                                                       , txtRemarks.Text
    //                                                                       , General.GetNullableInteger(txtmentorPlanId.Text).Value);


    //            BindData();
    //            SetPageNavigator();
    //        }            
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDMENTORNAME", "FLDROOMSHORTNAME", "FLDSTRENGTH", "FLDSESSIONNAME", "FLDDATE", "FLDREMARKS", "FLDCOMPLETEDYN" };
        string[] alCaptions = { "S.No", "Name", "Class Room", "Strength", "Sesion", "Date", "Remarks", "Completed YN" };

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

    protected void gvMentorPlan_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            string mentorId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblMentorId")).Text;
            string date = ((Label)_gridView.Rows[nCurrentRow].FindControl("txtDate")).Text;
            string classroom = ((Label)_gridView.Rows[nCurrentRow].FindControl("txtClassroom")).Text;
            string strength = ((Label)_gridView.Rows[nCurrentRow].FindControl("txtStrength")).Text;
            string remarks = ((Label)_gridView.Rows[nCurrentRow].FindControl("txtremarks")).Text;
            string session = ((Label)_gridView.Rows[nCurrentRow].FindControl("txtsession")).Text;
            int completedyn = (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkIsCompletedYN")).Checked) ? 1 : 0;
            string mentorplanId = _gridView.DataKeys[nCurrentRow].Value.ToString();
            //if (!IsValidPlan())
            //{
            //    ucError.Visible = true;
            //    return;
            //}

            PhoenixPreseaMentorPlanner.PreSeaMentorPlannerUpdate(General.GetNullableInteger(mentorId).Value
                                                                 , General.GetNullableDateTime(date).Value
                                                                 , General.GetNullableInteger(classroom)
                                                                 , General.GetNullableInteger(strength)
                                                                 , General.GetNullableInteger(session).Value
                                                                 , remarks
                                                                 , General.GetNullableInteger(mentorplanId).Value
                                                                 , completedyn);

            _gridView.EditIndex = -1;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private bool IsValidPlan()
    //{
    //    ucError.HeaderMessage = "Please provide the following required information";

    //    if (General.GetNullableDateTime(txtDate.Text) == null)
    //        ucError.ErrorMessage = "Date is Required.";

    //    if (ddlFaculty.SelectedIndex <= 0)
    //        ucError.ErrorMessage = "Faculty is Required.";
    //    if (ddlsession.SelectedIndex <= 0)
    //        ucError.ErrorMessage = "Sesson is Required.";

    //    return (!ucError.IsError);
    //}

    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDROWNUMBER", "FLDMENTORNAME", "FLDROOMSHORTNAME", "FLDSTRENGTH","FLDSESSIONNAME", "FLDDATE", "FLDREMARKS", "FLDCOMPLETEDYN" };
                string[] alCaptions = { "S.No", "Name", "Class Room", "Strength","Sesion", "Date", "Remarks" ,"Completed YN"};

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
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                
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

            string[] alColumns = { "FLDROWNUMBER", "FLDSTAFFNAME", "FLDROOMNAME", "FLDSTRENGTH", "FLDDATE", "FLDREMARKS" };
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

            //     General.ShowExcel("Time Slot Details", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            Response.AddHeader("Content-Disposition", "attachment; filename=Planner.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/sims1.png" + "' /></td>");
            Response.Write("<td><h3>Planner - Actual</h3></td>");
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
    protected void imgFaculty_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = PhoenixPreSeaBatchPlanner.ListPreSeaTrainingDepartmentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        string DepartmentList = dt.Rows[0]["FLDDEPARTMENTIDLIST"].ToString();

        string str = "showPickList('spnFacultyAdd', 'codehelp1', '', '../Common/CommonPickListPreSeaUser.aspx?departmentlist=" + DepartmentList + "', true);";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "codehelp1", str, true);
    }
}