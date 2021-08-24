using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewInstituteCoursePlanView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["from"] != null && Request.QueryString["from"].ToString() == "faculty")
        {
            txtInstituteName.ReadOnly = true;
            txtInstituteName.Attributes.Add("style", "class:readonlytextbox");
            btnShowInstitute.Attributes.Add("style", "display:none");            
            //divMenu.Visible = false;
            SetInstitute();
            NameValueCollection nvc = Filter.CurrentSelectedDate;
            if (nvc != null)
            {
                txtPlanFrom.Text = nvc["startdate"].ToString();
                txtPlanTo.Text = nvc["enddate"].ToString();
            }
            MenuFaculty.Visible = false;
        }
        else
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewInstituteCoursePlanView.aspx?" + Request.QueryString, "Add Contact", "<i class=\"fa fa-plus-circle\"></i>", "ADD");            
            MenuFaculty.AccessRights = this.ViewState;
            MenuFaculty.MenuList = toolbar.Show();
            PhoenixToolbar toolbarHead = new PhoenixToolbar();
            toolbarHead.AddButton("Show Plan", "SHOW",ToolBarDirection.Right);
            MenuCoursePlanner.AccessRights = this.ViewState;
            MenuCoursePlanner.MenuList = toolbarHead.Show();
        }

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;          
            DateTime today = DateTime.Today;
            var thisWeekStart = today.AddDays(-(int)today.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            txtPlanFrom.Text = thisWeekStart.ToString();
            txtPlanTo.Text = thisWeekEnd.ToString();

            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            txtInstituteId.Attributes.Add("style", "display:none");
            btnShowInstitute.Attributes.Add("onclick", "return showPickList('spnPickListInstitute', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListInistituteList.aspx',true);");
        }
        ViewState["EDIT"] = "0";
        BindData();
        BindFaculty();
        BindFacultyCourse();
        Guidlines();
    }
    
    protected void MenuCoursePlanner_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {            
            if (CommandName.ToUpper().Equals("SHOW"))
            {
                if (!IsValidCourse())
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
                ViewState["EDIT"] = "1";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Btn_Click(object sender, EventArgs e)
    {
        try
        {
            RadButton btn = (RadButton)sender;
            GridDataItem Item = btn.NamingContainer as GridDataItem;
         
            string courseName = ((RadLabel)Item.FindControl("lblCourseName")).Text;
            string courseId = ((RadLabel)Item.FindControl("lblcourseId")).Text;
            string courseCalendarId = ((RadLabel)((GridTableCell)((System.Web.UI.Control)sender).Parent).Controls[1]).Text;            
            string calendarId = ((RadLabel)((GridTableCell)((System.Web.UI.Control)sender).Parent).Controls[2]).Text;
            hdnDate.Value = General.GetDateTimeToString(((RadLabel)((GridTableCell)((System.Web.UI.Control)sender).Parent).Controls[3]).Text);
            if (string.IsNullOrEmpty(courseCalendarId))//add course plan
            {
                PhoenixCrewCoursePlanner.CrewCourseCalendarInsert(General.GetNullableInteger(courseId).Value
                                                                 , General.GetNullableInteger(txtInstituteId.Text).Value
                                                                 , General.GetNullableGuid(calendarId).Value
                                                                 , ref courseCalendarId);
              
                ViewState["COURSECALENDARID"] = courseCalendarId;
                ViewState["COURSE"] = courseId;
                lblFacutyPlan.Text = "Faculty Assigned - " + courseName + " On " + General.GetDateTimeToString(hdnDate.Value);                
                hdnCalendarId.Value = calendarId;               
                BindFaculty();
            }
            else
            {
                ViewState["FACULTY"] = "";
                ViewState["COURSE"] = courseId;
                ViewState["COURSECALENDARID"] = courseCalendarId;                
                hdnCalendarId.Value = calendarId;               
                BindFaculty();
                if (ViewState["ISFACULTYASSIGN"] != null && ViewState["ISFACULTYASSIGN"].ToString() == "1")
                {
                    lblFacutyPlan.Text = "Faculty Assigned - " + courseName + " On " + General.GetDateTimeToString(hdnDate.Value);
                }
                else// delete course plan when faculty not assigned
                { 
                    PhoenixCrewCoursePlanner.CrewCourseCalendarDelete(General.GetNullableGuid(courseCalendarId).Value);
                    lblFacutyPlan.Text = "Faculty Assigned";
                }
            }
          
            BindFacultyCourse();
            
            ViewState["EDIT"] = "1";
            BindData();
            gvcourseView.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetInstitute()
    {
        string strInstituteId = Request.QueryString["instituteId"].ToString();
        DataTable dt = PhoenixCrewInstitute.CrewInstituteEdit(General.GetNullableInteger(strInstituteId).Value);
        if (dt.Rows.Count > 0)
        {
            txtInstituteName.Text = dt.Rows[0]["FLDINSTITUTENAME"].ToString();
            txtInstituteId.Text = strInstituteId;
        }
    }
    private void BindData()
    {
        //int year, month;
        DataSet ds = null;
        string courseId = null;
        
        try
        {
            if (Request.QueryString["courseId"] != null)
                courseId = Request.QueryString["courseId"].ToString();

            ds = PhoenixCrewCoursePlanner.CrewMonthWiseCoursePlan(General.GetNullableInteger(txtInstituteId.Text)
                                                                            , General.GetNullableDateTime(txtPlanFrom.Text)
                                                                            , General.GetNullableDateTime(txtPlanTo.Text)
                                                                            , General.GetNullableInteger(courseId));

            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[1];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //adding columns dynamically
                    if (ViewState["EDIT"].ToString() != "1")
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            GridBoundColumn field = new GridBoundColumn();
                            field.HeaderText = General.GetNullableString(dt.Rows[i]["FLDDAY"].ToString());
                            field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            //field.HeaderStyle.Width = System.Web.UI.WebControls.Unit.Pixel(30);
                            gvcourseView.Columns.Insert(gvcourseView.Columns.Count, field);
                            field.Resizable = false;
                        }
                    }
                    gvcourseView.DataSource = ds;
                    //gvcourseView.DataBind();
                    ViewState["EDIT"] = "1";                   
                }
                else
                {
                    gvcourseView.DataSource = "";
                }
            }
        }
        catch (Exception ex)
        {           
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
     
    private bool IsValidCourse()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtInstituteName.Text))
            ucError.ErrorMessage = "Institute is required.";
     
        if(string.IsNullOrEmpty(General.GetNullableString(txtPlanFrom.Text)))
            ucError.ErrorMessage = "Plan from is required.";

        if (string.IsNullOrEmpty(General.GetNullableString(txtPlanTo.Text)))
            ucError.ErrorMessage = "Plan to is required.";

        return (!ucError.IsError);
    }
    private void BindFaculty()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
      
        string[] alColumns = { "FLDNAME", "FLDINSTITUTIONSHORTCODE", "FLDFACULTYNAME", "FLDROLE" };
        string[] alCaptions = { "Institute", "Institute Short Code", "Faculty", "Faculty Role" };       

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string coursecalendarId=null;
       
        if (ViewState["COURSECALENDARID"] != null)
            coursecalendarId = ViewState["COURSECALENDARID"].ToString();
        else
            coursecalendarId = "00000000-0000-0000-0000-000000000000";
        DataSet ds = PhoenixCrewFacultyCoursePlanner.CrewFacultyCourseSearchByCourse(General.GetNullableGuid(coursecalendarId)
                                                                             ,null
                                                                             //, null
                                                                             //, General.GetNullableInteger(txtInstituteId.Text)
                                                                             //, General.GetNullableInteger(courseId)
                                                                             //, General.GetNullableGuid(hdnCalendarId.Value)
                                                                             , sortexpression
                                                                             , sortdirection
                                                                             , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                             , General.ShowRecords(null)
                                                                             , ref iRowCount
                                                                             , ref iTotalPageCount);
     
        if (ds.Tables[0].Rows.Count > 0)
        {            
            gvFaculty.DataSource = ds.Tables[0];
            gvFaculty.DataBind();           
            ViewState["ISFACULTYASSIGN"] = 1;        
                //if (gvFaculty.SelectedIndex >= 0)
                //    ViewState["FACULTY"] = gvFaculty.DataKeys[gvFaculty.SelectedIndex].Values[1];          
           
        }
        else
        {
            gvFaculty.DataSource = "";
            ViewState["FACULTY"] = "";
            ViewState["ISFACULTYASSIGN"] = 0;            
        }    
    }   
   
    protected void MenuFacultyAdd_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string courseId = null;
        if (CommandName.ToUpper().Equals("ADD"))
        {
            if (ViewState["COURSE"] != null)
                courseId = ViewState["COURSE"].ToString();
            if (!IsValidCourse())
            {
                ucError.Visible = true;
                return;
            }
            string sScript = "javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewInstituteFaculty.aspx?from=plan&instituteId=" + txtInstituteId.Text + "&courseId=" + courseId + "&calendarid=" + hdnCalendarId.Value + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);


            //String scriptpopup = String.Format("javascript:Openpopup('codehelp2','','../Crew/CrewInstituteFaculty.aspx?from=plan&instituteId=" + txtInstituteId.Text 
            //                                                                                            + "&courseId=" + courseId
            //                                                                                            + "&calendarId=" + hdnCalendarId.Value + " ');");

            //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            
        }
    }

    //protected void gvFaculty_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        string instituteFacultyId = gvFaculty.DataKeys[e.Row.RowIndex].Values[1].ToString();
    //        //string coursId = null;

    //        ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
    //        ImageButton cmdFaculty = (ImageButton)e.Row.FindControl("cmdFaculty");
    //        ImageButton cmdDelete = (ImageButton)e.Row.FindControl("cmdDelete");

    //        if (eb != null)
    //            eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

    //        if (cmdDelete != null)
    //            cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete the faculty?')");

    //        if (cmdFaculty != null)
    //        {
    //            NameValueCollection selectedDate = new NameValueCollection();
    //            selectedDate.Clear();
    //            selectedDate.Add("startdate",txtPlanFrom.Text);
    //            selectedDate.Add("enddate", txtPlanTo.Text);                                
    //            Filter.CurrentSelectedDate = selectedDate;

    //            cmdFaculty.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewInstituteFacultyPlanView.aspx?from=course&facultyId=" + instituteFacultyId + "&instituteId=" + txtInstituteId.Text + "');return false;");
    //        }
    //    }
    //}

    //protected void gvFaculty_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    //string institutefacultyId = null;
    //    int rowIndex = Int32.Parse(e.CommandArgument.ToString());
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SELECT"))
    //        {
    //            //GridView _gridView = (GridView)sender;
                             
    //            BindFacultyCourse();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;           
    //    }
    //}
   
    //protected void gvFaculty_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentrow = de.RowIndex;
    //        string facultyCalendarId = ((Label)_gridView.Rows[nCurrentrow].FindControl("lblFacultyCalendarIdInsert")).Text;
            
    //        //PhoenixCrewInstituteFaculty.CrewInstituteCourseContactDelete(General.GetNullableInteger(lblFacultyCalendarId).Value);

    //        PhoenixCrewInstituteFacultyPlanner.CrewFacultyCalendarDelete(General.GetNullableGuid(facultyCalendarId).Value);
    //        _gridView.EditIndex = -1;
    //        _gridView.SelectedIndex = -1;
    //        BindData();
    //        BindFaculty();
    //        BindFacultyCourse();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void gvFaculty_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.SelectedIndex = e.NewSelectedIndex;
    //    //_gridView.EditIndex = 1;
    //    BindFaculty();
    //    //_gridView.EditIndex = -1;
    //    gvFaculty.SelectedIndex = -1;
    //    BindFacultyCourse();
    //}
    private void BindFacultyCourse()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string facultyId=null;        
        string instituteId = txtInstituteId.Text;
       
        if (ViewState["FACULTY"] != null)
            facultyId = ViewState["FACULTY"].ToString();

        DataSet ds = PhoenixCrewFacultyCoursePlanner.CrewFacultyCourseSearch(null
                                                                            , General.GetNullableInteger(facultyId)
                                                                            , General.GetNullableInteger(instituteId)
                                                                            , null
                                                                            , General.GetNullableGuid(hdnCalendarId.Value)
                                                                            , sortexpression
                                                                            , sortdirection
                                                                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                            , General.ShowRecords(null)
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);

        if ( ds.Tables[0].Rows.Count > 0)
        {
            gvFacultyCourse.DataSource = ds.Tables[0];
            gvFacultyCourse.DataBind();
        }
        else
        {
            gvFacultyCourse.DataSource = "";            
        }       
    }
    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            txtInstituteId.Text = nvc[1];
            txtInstituteName.Text = nvc[2];

            BindData();
            gvcourseView.Rebind();          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Guidlines()
    {
        btnTooltipHelp.Text = "<table> <tr><td>&nbsp;1. Course planned days are highlighted in blue color, Faculty assigned days are highlighted in red color.</td></tr><tr><td>&nbsp;2. To plan a course click on the date. &nbsp;</td></tr><tr><td>&nbsp;3. To assign a faculty for a course click on the date and click on &nbsp;<i class=\"fa fa-plus-circle\"></i>&nbsp; button on the faculty. </td> </tr> <tr><td>&nbsp;4. To delete a faculty assigned click on &nbsp;<i class=\"fas fa-trash\"></i>&nbsp; button in the faculty.</td> </tr><tr><td>&nbsp;5. To unplan a course delete the faculty members and click on the date.</td> </tr></tr></table>";        
    }

    protected void gvcourseView_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void gvcourseView_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            DataSet ds = (DataSet)gv.DataSource;

            if (drv.Row.Table.Columns.Count > 0)
            {
                string tooltip = string.Empty;
                string courseId = drv["FLDCOURSEID"].ToString();

                DataTable header = ds.Tables[1];
                DataTable dtcourse = ds.Tables[2];
                DataTable dtfaculty = ds.Tables[3];

                for (int i = 2; i < header.Rows.Count+2; i++)
                {
                    DataRow[] drcourse = dtcourse.Select("FLDCOURSEID = " + courseId +
                                                " AND FLDDATE = '" + header.Rows[i-2]["FLDDATE"].ToString() + "'");

                    DataRow[] drInstitute = dtcourse.Select("FLDDATE='" + header.Rows[i-2]["FLDDATE"].ToString() + "'");

                    DataRow[] dr = dtfaculty.Select("FLDCOURSEID = " + courseId +
                                                " AND FLDDATE = '" + header.Rows[i-2]["FLDDATE"].ToString() + "'");

                    RadButton btn = new RadButton();
                    btn.Click += Btn_Click;
                    btn.Width = Unit.Pixel(50);

                    RadLabel lblCourseCalendarId = new RadLabel();
                    lblCourseCalendarId.Visible = false;

                    RadLabel lblcalendarId = new RadLabel();
                    lblcalendarId.Visible = false;
                    lblcalendarId.Text = drInstitute[0]["FLDCALENDARID"].ToString();

                    RadLabel lblPlanDate = new RadLabel();
                    lblPlanDate.Visible = false;
                    lblPlanDate.Text = drInstitute[0]["FLDDATE"].ToString();

                    //hilight institute working day and holiday
                    if (drInstitute.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(drInstitute[0]["FLDINSTITUTECALENDARID"].ToString()))
                            btn.Attributes.Add("style", "border-bottom:3px solid gray;");                       
                    }

                    if (drcourse.Length > 0)
                    {
                        //hilight course planned days 
                        btn.Attributes.Add("style", "border-bottom:3px solid blue;");                        
                        lblCourseCalendarId.Text = drcourse[0]["FLDCOURSECALENDARID"].ToString();

                    }
                    //mark faculty course plan
                    if (dr.Length > 0)
                    {
                        //marking course plan
                        if (dr[0]["FLDPLANTYPE"].ToString() == "0")
                        {
                            btn.Attributes.Add("style", "border-bottom:3px solid blue;border-top: 3px solid red;");                          
                            tooltip = "";
                            foreach (DataRow datarow in dr)
                                tooltip = tooltip + datarow["FLDFACULTYNAME"].ToString() + System.Environment.NewLine;
                            btn.ToolTip = tooltip;

                        }
                        //marking faculty leave
                        if (dr[0]["FLDPLANTYPE"].ToString() == "1")
                        {
                            btn.Attributes.Add("style", "border-bottom:3px solid red;");
                        }
                    }
                    //e.Row.Cells[i + 1].Width = 3;
                    e.Item.Cells[i + 1].Controls.Add(btn);
                    e.Item.Cells[i + 1].Controls.Add(lblCourseCalendarId);
                    e.Item.Cells[i + 1].Controls.Add(lblcalendarId);
                    e.Item.Cells[i + 1].Controls.Add(lblPlanDate);
                }
            }
        }
    }

    protected void gvFaculty_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                ViewState["FACULTY"] = ((RadLabel)eeditedItem.FindControl("lblFacultyId")).Text;
                BindFacultyCourse();
                gvFacultyCourse.Rebind();

            }
            //else if (e.CommandName.ToUpper().Equals("SAVE"))
            //{

            //}
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string facultycalendarId = null, coursecalendarId = null, timefrom = null, timeto = null, classno = null;
                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                //instituteId = Request.QueryString["instituteId"].ToString();
                facultycalendarId = ((RadLabel)eeditedItem.FindControl("lblFacultyCalendarId")).Text;
                coursecalendarId = ((RadLabel)eeditedItem.FindControl("lblcourseCalendarId")).Text;
                timefrom = ((RadMaskedTextBox)eeditedItem.FindControl("txtstarttime")).TextWithLiterals;
                timeto = ((RadMaskedTextBox)eeditedItem.FindControl("txtEndTime")).TextWithLiterals;
                classno = ((RadTextBox)eeditedItem.FindControl("txtRoomNo")).Text;
                string facultyCourseId = eeditedItem.GetDataKeyValue("FLDFACULTYCOURSEID").ToString();

                PhoenixCrewFacultyCoursePlanner.CrewFacultyCourseUpdate(General.GetNullableGuid(facultyCourseId).Value
                                                                        , General.GetNullableGuid(coursecalendarId).Value
                                                                        , General.GetNullableGuid(facultycalendarId).Value
                                                                        , General.GetNullableDateTime(timefrom)
                                                                        , General.GetNullableDateTime(timeto)
                                                                        , classno);
                BindData();
                gvcourseView.Rebind();
                BindFaculty();
                gvFaculty.Rebind();
                BindFacultyCourse();
                gvFacultyCourse.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string facultyCalendarId = ((RadLabel)eeditedItem.FindControl("lblFacultyCalendarIdInsert")).Text;                
                PhoenixCrewInstituteFacultyPlanner.CrewFacultyCalendarDelete(General.GetNullableGuid(facultyCalendarId).Value);
                BindData();
                gvcourseView.Rebind();
                BindFaculty();
                gvFaculty.Rebind();
                BindFacultyCourse();
                gvFacultyCourse.Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFaculty_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindFaculty();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFaculty_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int rowCounter;
            RadLabel lblRowNo = e.Item.FindControl("lblRowNo") as RadLabel;
            rowCounter = gvFaculty.MasterTableView.PageSize * gvFaculty.MasterTableView.CurrentPageIndex;
            if (lblRowNo != null)
                lblRowNo.Text = (e.Item.ItemIndex + 1 + rowCounter).ToString();

            DataRowView drv = (DataRowView)e.Item.DataItem;
            string instituteFacultyId = dataItem.GetDataKeyValue("FLDFACULTYCOURSEID").ToString();

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton cmdFaculty = (LinkButton)e.Item.FindControl("cmdFaculty");
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");

            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            if (cmdDelete != null)
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete the faculty?')");

            if (cmdFaculty != null)
            {
                NameValueCollection selectedDate = new NameValueCollection();
                selectedDate.Clear();
                selectedDate.Add("startdate", txtPlanFrom.Text);
                selectedDate.Add("enddate", txtPlanTo.Text);
                Filter.CurrentSelectedDate = selectedDate;
                cmdFaculty.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', 'Faculty Plan', '" + Session["sitepath"] + "/Crew/CrewInstituteFacultyPlanView.aspx?from=course&facultyId=" + instituteFacultyId + "&instituteId=" + txtInstituteId.Text + "');return false;");
                //cmdFaculty.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewInstituteFacultyPlanView.aspx?from=course&facultyId=" + instituteFacultyId + "&instituteId=" + txtInstituteId.Text + "');return false;");
            }
        }
    }

    protected void gvFacultyCourse_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindFacultyCourse();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFacultyCourse_ItemDataBound(object sender, GridItemEventArgs e)
    {
        int rowCounter;
        RadLabel lblRowNo = e.Item.FindControl("lblRowNo") as RadLabel;
        rowCounter = gvFacultyCourse.MasterTableView.PageSize * gvFacultyCourse.MasterTableView.CurrentPageIndex;
        if (lblRowNo != null)
            lblRowNo.Text = (e.Item.ItemIndex + 1 + rowCounter).ToString();
    }
}