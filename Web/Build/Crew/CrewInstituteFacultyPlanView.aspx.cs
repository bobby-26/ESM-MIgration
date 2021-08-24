using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewInstituteFacultyPlanView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (Request.QueryString["from"] != null && Request.QueryString["from"].ToString() == "course")
        {
            txtInstituteName.ReadOnly = true;
            txtInstituteName.Attributes.Add("class", "readonlytextbox");
            btnShowInstitute.Attributes.Add("style", "display:none");
            SetInstitute();
            NameValueCollection nvc = Filter.CurrentSelectedDate;
            if (nvc != null)
            {
                txtPlanFrom.Text = nvc["startdate"].ToString();
                txtPlanTo.Text = nvc["enddate"].ToString();
            }
            MenuTitle.Visible = false;
            MenuFacultyCourse.Visible = false;
       }
        else
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewInstituteFacultyPlanView.aspx?" + Request.QueryString, "Add Faculty", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            //toolbar.AddImageLink("../Crew/CrewInstituteFacultyPlanView.aspx?" + Request.QueryString, "Add Contact", "add.png", "ADD");
            MenuFacultyCourse.AccessRights = this.ViewState;
            MenuFacultyCourse.MenuList = toolbar.Show();

            PhoenixToolbar toolbarHead = new PhoenixToolbar();
            toolbarHead.AddButton("Show Plan", "SHOW", ToolBarDirection.Right);
            MenuFacultyPlanner.AccessRights = this.ViewState;
            MenuFacultyPlanner.MenuList = toolbarHead.Show();

            toolbarHead = new PhoenixToolbar();
            toolbarHead.AddButton("List", "LIST");
            toolbarHead.AddButton("Planner", "PLAN");
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbarHead.Show();
            MenuTitle.SelectedMenuIndex = 1;

        }
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            //ddlYear.SelectedYear = DateTime.Now.Year.ToString();
            //ddlMonth.SelectedMonthNumber = DateTime.Now.Month.ToString();
            DateTime today = DateTime.Today;
            var thisWeekStart = today.AddDays(-(int)today.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            txtPlanFrom.Text = thisWeekStart.ToString();
            txtPlanTo.Text = thisWeekEnd.ToString();

            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            txtInstituteId.Attributes.Add("style", "display:none");
            btnShowInstitute.Attributes.Add("onclick", "javascript:return showPickList('spnPickListInstitute', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListInistituteList.aspx',true);");

        }

        ViewState["EDIT"] = "0";
        BindData();
        BindCourse();
        BindFacultyCourse();
        Guidlines();
        
    }

    protected void MenuFacultyPlanner_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SHOW"))
        {
            BindData();
            gvfaculty.Rebind();
        }
    }
    protected void MenuTitle_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../Crew/CrewInstituteFacultyPlannerList.aspx", true);
        }
    }
    private void Btn_Click(object sender, EventArgs e)
    {
        try
        {
            Button btn = (Button)sender;
            GridDataItem Item = btn.NamingContainer as GridDataItem;

            string facultyId = ((RadLabel)Item.FindControl("lblFacultyId")).Text;
            string faculy = ((RadLabel)Item.FindControl("lblFcaulty")).Text;
            string facultyCalendarId = ((RadLabel)((GridTableCell)((System.Web.UI.Control)sender).Parent).Controls[1]).Text;
            string calendarId = ((RadLabel)((GridTableCell)((System.Web.UI.Control)sender).Parent).Controls[2]).Text;
            hdnDate.Value =General.GetDateTimeToString(((RadLabel)((GridTableCell)((System.Web.UI.Control)sender).Parent).Controls[3]).Text);

            ViewState["FACULTY"] = facultyId;
            lblFacutyPlan.Text = "Course Plan - " + faculy + " On " + General.GetDateTimeToString(hdnDate.Value);
            hdnCalendarId.Value = calendarId;
            ViewState["RowIndex"] = Item.RowIndex;

            BindCourse();
            gvCourse.Rebind();
                        
            BindFacultyCourse();
            gvFacultyCourse.Rebind();
           
            BindData();
            gvfaculty.Rebind();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    
    private void BindData()
    {
        //int year, month;
        DataSet ds = null;
        string facultyId = null;
        try
        {
            //if (!string.IsNullOrEmpty(ddlYear.SelectedYear))
            //    year = General.GetNullableInteger(ddlYear.SelectedYear).Value;
            //else
            //    year = DateTime.Now.Year;
            //if (!string.IsNullOrEmpty(ddlMonth.SelectedMonthNumber))
            //    month = General.GetNullableInteger(ddlMonth.SelectedMonthNumber).Value;
            //else
            //    month = DateTime.Now.Month;

            if (Request.QueryString["facultyId"] != null)
                facultyId = Request.QueryString["facultyId"].ToString();

            ds = PhoenixCrewInstituteFacultyPlanner.CrewFacultyMonthWisePlanner(General.GetNullableInteger(txtInstituteId.Text)
                                                                             , General.GetNullableDateTime(txtPlanFrom.Text)
                                                                             , General.GetNullableDateTime(txtPlanTo.Text)
                                                                             , General.GetNullableInteger(facultyId)
                                                                             );
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[1];
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
                        gvfaculty.Columns.Insert(gvfaculty.Columns.Count, field);
                        field.Resizable = false;
                    }
                }
                gvfaculty.DataSource = ds;
                gvfaculty.DataBind();
                ViewState["EDIT"] = "1";
            }
            else
            {
                gvfaculty.DataSource = "";

            }
        }
        catch (Exception ex)
        {            
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

       
    }
    private bool IsValidData()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtInstituteName.Text))
            ucError.ErrorMessage = "Institute is required.";

        //if (string.IsNullOrEmpty(General.GetNullableString(ddlMonth.SelectedMonthName)))
        //    ucError.ErrorMessage = "Month is required.";

        if (string.IsNullOrEmpty(General.GetNullableString(txtPlanFrom.Text)))
            ucError.ErrorMessage = "Plan from is required.";

        if (string.IsNullOrEmpty(General.GetNullableString(txtPlanTo.Text)))
            ucError.ErrorMessage = "Plan to is required.";
        return (!ucError.IsError);
    }

    private void BindCourse()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;    

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        divGrid.Visible = true;
        string facultyId = null;
        
        if (ViewState["FACULTY"] != null)
            facultyId = ViewState["FACULTY"].ToString();

        DataSet ds = PhoenixCrewFacultyCoursePlanner.CrewFacultyCourseSearch(null
                                                                            , General.GetNullableInteger(facultyId)
                                                                            , General.GetNullableInteger(txtInstituteId.Text)
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
            gvCourse.DataSource = ds.Tables[0];
            gvCourse.DataBind();
            ViewState["ISFACULTYPLAN"] = 1;
            //if (ViewState["RowIndex"] != null)
            //{
            //    GridDataItem item = gvfaculty.Items[int.Parse(ViewState["RowIndex"].ToString())];
            //    ViewState["COURSECALENDARID"] = ((RadLabel)item.FindControl("lblcourseCalendarId")).Text;
            //}
            //else
            //    ViewState["COURSECALENDARID"] = "";
           
        }
        else
        {
            gvCourse.DataSource = "";
            ViewState["ISFACULTYPLAN"] = 0;
            ViewState["COURSECALENDARID"] = "";
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
    
    protected void MenuFacultyCourse_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string facultyId = null;
        if (CommandName.ToUpper().Equals("ADD"))
        {
            if (!IsValidData())
            {
                ucError.Visible = true;
                return;
            }

            if (ViewState["FACULTY"] != null)
            {
                facultyId = ViewState["FACULTY"].ToString();
            }
            else
            {
                ucError.Text = "Faculty is required";
                ucError.Visible = true;
                return;
            }
            string sScript = "javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewFacultyCourseAdd.aspx?from=plan&instituteId=" + txtInstituteId.Text + "&facultyId=" + facultyId + "&calendarid=" + hdnCalendarId.Value + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);           
        }
    }

    private void BindFacultyCourse()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string coursecalendarId=null;      
        if (ViewState["COURSECALENDARID"] != null && ViewState["COURSECALENDARID"].ToString() != "")
            coursecalendarId = ViewState["COURSECALENDARID"].ToString();
        else
            coursecalendarId = "00000000-0000-0000-0000-000000000000";
        DataSet ds = PhoenixCrewFacultyCoursePlanner.CrewFacultyCourseSearchByCourse(General.GetNullableGuid(coursecalendarId)
                                                                                    , null                                                                                   
                                                                                   , sortexpression
                                                                                   , sortdirection
                                                                                   , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                   , General.ShowRecords(null)
                                                                                   , ref iRowCount
                                                                                   , ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvFacultyCourse.DataSource = ds.Tables[0];
            //gvFacultyCourse.DataBind();
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
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void Guidlines()
    {
        btnTooltipHelp.Text = "<table> <tr><td>&nbsp;1. Course planned days are highlighted in blue color, Faculty assigned days are highlighted in red color.</td></tr><tr><td>&nbsp;2. Faculty planned leaves are underlined in red color. </td></tr> <tr><td>&nbsp;3. To plan a course for a faculty click on the date and click on &nbsp;<img id=\"img2\" runat=\"server\" src=" + Session["images"] + "/add.png style=\"vertical-align: top\" />&nbsp; button on the course plan. </td></tr> <tr><td>&nbsp;4. To remove faculty plan click on &nbsp;<img id=\"img5\" runat=\"server\" src=" + Session["images"] + "/te_del.png style=\"vertical-align: top\" />&nbsp; button in the course plan.</td> </tr></table>";
    }
    
    protected void gvCourse_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttendance.CurrentPageIndex + 1;
            BindCourse();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCourse_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            else if (e.CommandName.ToString().ToUpper() == "SAVE")
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                string courseId = ((RadLabel)eeditedItem.FindControl("lblCourseId")).Text;
                string planfrom = ((UserControlDate)eeditedItem.FindControl("txtFromDate")).Text;
                string planto = ((UserControlDate)eeditedItem.FindControl("txtToDate")).Text;

                string facultyId = ViewState["FACULTY"].ToString();
                PhoenixCrewInstituteFacultyPlanner.CrewFacultyCoursePlannerInsert(General.GetNullableInteger(facultyId).Value
                                                                            , General.GetNullableInteger(txtInstituteId.Text).Value
                                                                            , General.GetNullableInteger(courseId).Value
                                                                            , General.GetNullableDateTime(planfrom).Value
                                                                            , "Course");
                ucStatus.Text = "Faculty dates are booked successfully";
                BindData();
                gvfaculty.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                ViewState["COURSECALENDARID"] = ((RadLabel)eeditedItem.FindControl("lblcourseCalendarId")).Text;
                BindFacultyCourse();
                gvFacultyCourse.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string facultycalendarId = null, coursecalendarId = null, timefrom = null, timeto = null, classno = null;
                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                facultycalendarId = ((RadLabel)eeditedItem.FindControl("lblFacultyCalendarIdEdit")).Text;
                coursecalendarId = ((RadLabel)eeditedItem.FindControl("lblcourseCalendarIdEdit")).Text;
                timefrom = General.GetNullableDateTime(((RadMaskedTextBox)eeditedItem.FindControl("txtstarttime")).TextWithLiterals).Value.ToString();
                timeto = General.GetNullableDateTime(((RadMaskedTextBox)eeditedItem.FindControl("txtEndTime")).TextWithLiterals).Value.ToString();
                //timeto = ((RadMaskedTextBox)eeditedItem.FindControl("txtEndTime")).Text;
                classno = ((RadTextBox)eeditedItem.FindControl("txtRoomNo")).Text;
                string facultyCourseId = eeditedItem.GetDataKeyValue("FLDFACULTYCOURSEID").ToString();

                PhoenixCrewFacultyCoursePlanner.CrewFacultyCourseUpdate(General.GetNullableGuid(facultyCourseId).Value
                                                                        , General.GetNullableGuid(coursecalendarId).Value
                                                                        , General.GetNullableGuid(facultycalendarId).Value
                                                                        , General.GetNullableDateTime(timefrom)
                                                                        , General.GetNullableDateTime(timeto)
                                                                        , classno);

                BindData();
                gvfaculty.Rebind();
                BindCourse();
                gvCourse.Rebind();
                BindFacultyCourse();
                gvFacultyCourse.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;                
                string facultyCalendarId = ((RadLabel)eeditedItem.FindControl("lblFacultyCalendarId")).Text;
                PhoenixCrewInstituteFacultyPlanner.CrewFacultyCalendarDelete(General.GetNullableGuid(facultyCalendarId).Value);
                BindData();
                gvfaculty.Rebind();
                BindCourse();
                gvCourse.Rebind();
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

    protected void gvCourse_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            int rowCounter;
            RadLabel lblRowNo = e.Item.FindControl("lblRowNo") as RadLabel;
            rowCounter = gvCourse.MasterTableView.PageSize * gvCourse.MasterTableView.CurrentPageIndex;
            if (lblRowNo != null)
                lblRowNo.Text = (e.Item.ItemIndex + 1 + rowCounter).ToString();

            GridDataItem dataItem =(GridDataItem) e.Item;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            string courseId = dataItem.GetDataKeyValue("FLDCOURSEID").ToString();

            LinkButton cmdCourse = (LinkButton)e.Item.FindControl("cmdCourse");
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdCourse != null)
            {
                NameValueCollection selectedDate = new NameValueCollection();
                selectedDate.Clear();                
                selectedDate.Add("startdate", txtPlanFrom.Text);
                selectedDate.Add("enddate", txtPlanTo.Text);
                Filter.CurrentSelectedDate = selectedDate;

                //cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text.Trim() + "&mod=" + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + " &cmdname = LICENCEUPLOAD'); return false;");
                cmdCourse.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', 'Course Plan', '"+ Session["sitepath"] + "/Crew/CrewInstituteCoursePlanView.aspx?from=faculty&courseId=" + courseId + "&instituteId=" + txtInstituteId.Text + "');return false;");
            }
            if (cmdDelete != null)
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to delete the course?')");

            //RadMaskedTextBox txtstarttime = (RadMaskedTextBox)e.Item.FindControl("txtstarttime");
            //if (txtstarttime != null)
            //{
            //    txtstarttime. = MaskType.DateTime;
            //    MaskDateTimeProvider provider = this.radMaskedEditBox1.MaskedEditBoxElement.Provider as MaskDateTimeProvider;
            //    provider.AutoSelectNextPart = true;
            //}
        }
    }

    protected void gvfaculty_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttendance.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvfaculty_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;
        
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            DataSet ds = (DataSet)gv.DataSource;
            if (drv.Row.Table.Columns.Count > 0)
            {
                string tooltip = string.Empty;
                string facultyId = drv["FLDINSTITUTEFACULTYID"].ToString();

                DataTable header = ds.Tables[1];
                DataTable data = ds.Tables[2];
                DataTable faculty = ds.Tables[3];
                //DataTable course = ds.Tables[4];

                for (int i = 2; i < header.Rows.Count+2; i++)
                {
                    DataRow[] dr = faculty.Select("FLDFACULTYID = " + facultyId +
                                                " AND FLDDATE = '" + header.Rows[i-2]["FLDDATE"].ToString() + "'");

                    DataRow[] drholiday = data.Select("FLDDATE='" + header.Rows[i-2]["FLDDATE"].ToString() + "'");

                    DataRow[] drcourse = data.Select("FLDDATE = '" + header.Rows[i-2]["FLDDATE"].ToString() + "'" +
                                                        "AND FLDINSTITUTEFACULTYID = '" + facultyId + "' ");

                    Button btn = new Button();
                    btn.Click += Btn_Click;
                    btn.Width = Unit.Percentage(1);

                    RadLabel lblfacultycalendarId = new RadLabel();
                    lblfacultycalendarId.Visible = false;

                    RadLabel lblCalendarId = new RadLabel();
                    lblCalendarId.Visible = false;

                    RadLabel lbldate = new RadLabel();
                    lbldate.Visible = false;

                    if (drholiday.Length > 0)
                    {
                        lblCalendarId.Text = drholiday[0]["FLDCALENDARID"].ToString();
                        lbldate.Text = drholiday[0]["FLDDATE"].ToString();
                    }

                    //mark institute holiday
                    if (drholiday.Length > 0 && !string.IsNullOrEmpty(drholiday[0]["FLDINSTITUTECALENDARID"].ToString()))
                    {
                        btn.Attributes.Add("style", "border-bottom:3px solid gray;");                     
                    }

                    //mark course plan
                    if (drcourse.Length > 0)
                    {
                        btn.Attributes.Add("style", "border-bottom:3px solid blue;");
                        tooltip = "";
                        foreach (DataRow datarow in drcourse)
                            tooltip = tooltip + datarow["FLDCOURSE"].ToString() + System.Environment.NewLine;
                        btn.ToolTip = tooltip;
                    }
                    //mark faculty course plan
                    if (dr.Length > 0)
                    {
                        //marking course plan
                        if (dr[0]["FLDPLANTYPE"].ToString() == "COR")
                            btn.Attributes.Add("style", "border-bottom:3px solid blue;border-top: 3px solid red;");
                        //marking faculty leave
                        if (dr[0]["FLDPLANTYPE"].ToString() == "LEV")
                        {
                            btn.Attributes.Add("style", "border-bottom:3px solid red;");
                            btn.ToolTip = "Faculy Leave";
                        }
                        lblfacultycalendarId.Text = dr[0]["FLDFACULTYCALENDARID"].ToString();
                    }
                    e.Item.Cells[i + 1].Controls.Add(btn);
                    e.Item.Cells[i + 1].Controls.Add(lblfacultycalendarId);
                    e.Item.Cells[i + 1].Controls.Add(lblCalendarId);
                    e.Item.Cells[i + 1].Controls.Add(lbldate);
                }
            }
        }
    }

    protected void gvFacultyCourse_ItemDataBound(object sender, GridItemEventArgs e)
    {
        int rowCounter;
        RadLabel lblRowNo = e.Item.FindControl("lblRowNo") as RadLabel;
        rowCounter = gvCourse.MasterTableView.PageSize * gvCourse.MasterTableView.CurrentPageIndex;
        if (lblRowNo != null)
            lblRowNo.Text = (e.Item.ItemIndex + 1 + rowCounter).ToString();
    }

    protected void gvFacultyCourse_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttendance.CurrentPageIndex + 1;
            BindFacultyCourse();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}