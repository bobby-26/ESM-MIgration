using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Web;

public partial class PreSeaWeeklyPlanner : PhoenixBasePage
{
    private DataSet dsGrid = new DataSet();

    #region :   Page load and render Events :

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Weekly Planner", "WEEKPLAN");
            toolbarmain.AddButton("Copy Plan", "COPY");

            PreSeaMenu.AccessRights = this.ViewState;
            PreSeaMenu.MenuList = toolbarmain.Show();
            PreSeaMenu.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Add", "ADD");
            toolbar.AddButton("Save", "SAVE");
            MainMenuPreseaWeekPlanner.AccessRights = this.ViewState;
            MainMenuPreseaWeekPlanner.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ucSemester.Enabled = false;
                ucWeek.Enabled = false;

                ViewState["BATCH"] = "";
                ViewState["WEEKID"] = "";
                ViewState["DAYID"] = "";
                ViewState["COURSE"] = "";
                ViewState["SECTIONID"] = "";
                ViewState["ADDYN"] = "0";

                PhoenixToolbar toolbarPlannerList = new PhoenixToolbar();
                toolbarPlannerList.AddImageButton("../PreSea/PreSeaWeeklyPlanner.aspx?", "Export to Excel", "icon_xls.png", "Excel");
                MenuPreSeaWeekPlanner.AccessRights = this.ViewState;
                MenuPreSeaWeekPlanner.MenuList = toolbarPlannerList.Show();


                rdoActivity.DataSource = PhoenixPreSeaQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 102);
                rdoActivity.DataTextField = "FLDQUICKNAME";
                rdoActivity.DataValueField = "FLDQUICKCODE";
                rdoActivity.DataBind();
                BindWeeklyPlan();

                ddlDate.Items.Clear();
                ddlSection.Items.Clear();
                ListItem li = new ListItem("--Select--", "DUMMY");

                ddlSection.Items.Add(li);
                ddlSection.DataBind();
                ddlDate.Items.Add(li);
                ddlDate.DataBind();
                ddlClassRoom.Items.Add(li);

                ClassDetails.Visible = false;
                PracticalDetails.Visible = false;

            }
            if (Request.Params.Get("__EVENTTARGET") != null)
            {
                BindWeeklyPlan();
            }
            if (ViewState["ADDYN"].ToString() == "1")
                MainMenuPreseaWeekPlanner.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

    #region :   Methods :

    protected void SetPrimaryDayDetails()
    {
        if (!String.IsNullOrEmpty(ViewState["DAYID"].ToString()))
        {
            DataTable dt = PhoenixPreSeaWeeklyPlanner.EditDayEntry(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["DAYID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ucBatch.SelectedBatch = dr["FLDBATCHID"].ToString();
                ucSemester.SelectedSemester = dr["FLDSEMESTERID"].ToString();
                ddlSection.SelectedValue = dr["FLDSECTIONID"].ToString();
                ucWeek.SelectedWeek = dr["FLDWEEKID"].ToString();
                DateTime dat = Convert.ToDateTime(dr["FLDDATE"].ToString());
                ddlDate.SelectedValue = dat.ToShortDateString();
                ddlFacultyPreparedBy.SelectedValue = dr["FLDPREPAREDBY"].ToString();
            }
        }
    }

    protected void SetPrimaryHourDetails()
    {
        if (!String.IsNullOrEmpty(ViewState["HOURID"].ToString()))
        {
            DataTable dt = PhoenixPreSeaWeeklyPlanner.EditHourlyEntry(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["HOURID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                rdoActivity.SelectedValue = dr["FLDCLASSTYPE"].ToString();
                ddlDuration.SelectedValue = dr["FLDTOTALHOURS"].ToString();
                ucTimeSlotFrom.SelectedTime = dr["FLDHOURFROM"].ToString().Replace(":", "");
                ucTimeSlotTo.SelectedTime = dr["FLDHOURTO"].ToString().Replace(":", ""); ;

                string divisions = dr["FLDPRACTICALDIVISIONS"].ToString();
                string subjects = dr["FLDPRACTICALSUBJECTS"].ToString();
                string instructors = dr["FLDINSTRUCTORS"].ToString();

                if (String.IsNullOrEmpty(dr["FLDSUBJECT"].ToString()))
                {
                    ucSubject.SelectedSubject = "";
                    ucSubject.Enabled = false;
                    ddlFaculty.SelectedValue = "DUMMY";
                    ddlFaculty.Enabled = false;
                    FillPracticalDetails(divisions, subjects, instructors);
                }
                else
                {
                    ucSubject.SelectedSubject = dr["FLDSUBJECT"].ToString();
                    ucSubject.Enabled = true;
                    ddlFaculty.SelectedValue = dr["FLDFACULTY"].ToString();
                    ddlFaculty.Enabled = true;
                    grdPracticalTimetable.Enabled = false;
                }
            }
        }
    }

    private void BindWeeklyPlan()
    {

        try
        {
            gvPreseaWeeklyPlanner.Columns.Clear();

            dsGrid = PhoenixPreSeaWeeklyPlanner.ListTimeTableDetails(General.GetNullableInteger(ViewState["BATCH"].ToString())
                                                                        , General.GetNullableInteger(ViewState["WEEKID"].ToString())
                                                                        , General.GetNullableInteger(ViewState["SECTIONID"].ToString()));

            if (dsGrid.Tables.Count > 0 && dsGrid.Tables[0].Rows.Count > 0)
            {
                DataTable dt = dsGrid.Tables[0];
                DataTable dt2 = dsGrid.Tables[1];

                if (dt.Rows.Count > 0 && dt.Rows[0]["FLDWEEKID"].ToString() != "" && dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i != 1)
                        {
                            BoundField field = new BoundField();
                            field.HtmlEncode = false;
                            field.HeaderText = dt.Rows[0][i].ToString();
                            field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            if (i == 0)
                                field.DataField = "FLDHOURSLOTS";
                            field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            field.ItemStyle.Wrap = false;
                            gvPreseaWeeklyPlanner.Columns.Add(field);
                        }
                    }

                    gvPreseaWeeklyPlanner.DataSource = dsGrid.Tables[1];
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

    private bool IsValidWeeklyPlan(bool notAllFilled, bool activityselected)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (String.IsNullOrEmpty(ucSemester.SelectedSemester))
            ucError.ErrorMessage = "Select Semester for the plan";

        if (String.IsNullOrEmpty(ddlSection.SelectedValue))
            ucError.ErrorMessage = "Select Section for the plan";

        if (String.IsNullOrEmpty(ucWeek.SelectedWeek))
            ucError.ErrorMessage = "Select Week for the plan";

        if (General.GetNullableDateTime(ddlDate.SelectedValue) == null)
            ucError.ErrorMessage = "Select Date for the plan";

        if (String.IsNullOrEmpty(ddlDuration.SelectedValue))
            ucError.ErrorMessage = "Duration is required";

        if (activityselected == false)
            ucError.ErrorMessage = "Activity is required";

        else
        {
            if (rdoActivity.SelectedItem.Text.ToUpper().Contains("PRACTICAL") && notAllFilled == true) 
            {
                ucError.ErrorMessage = "All Practical Details has to be filled for Pracical Class";
            }
            else if (rdoActivity.SelectedItem.Text.Contains("Theory"))
            {
                if (General.GetNullableInteger(ucSubject.SelectedSubject) == null)
                    ucError.ErrorMessage = "Subject is required for Theory Class.";
                if (General.GetNullableInteger(ddlFaculty.SelectedValue) == null)
                    ucError.ErrorMessage = "Faculty is required for Theory Class";
            }
            if (rdoActivity.SelectedItem.Text.ToUpper().Contains("BREAK"))
            {
                if (General.GetNullableString(txtBreakDesc.Text) == null)
                    ucError.ErrorMessage = "Break description is required";
            }
        }
        if (General.GetNullableInteger(ucTimeSlotFrom.SelectedTime) == null)
            ucError.ErrorMessage = "Time from is required.";
        if (General.GetNullableInteger(ucTimeSlotTo.SelectedTime) == null)
            ucError.ErrorMessage = "Time to is required.";

        if (General.GetNullableInteger(ucTimeSlotTo.SelectedTime).HasValue && General.GetNullableInteger(ucTimeSlotFrom.SelectedTime).HasValue)
        {
            if (ucTimeSlotFrom.SelectedTime == ucTimeSlotTo.SelectedTime)
                ucError.ErrorMessage = "Time from and Time to should not be Same.";
            else if (int.Parse(ucTimeSlotFrom.SelectedTime) > int.Parse(ucTimeSlotTo.SelectedTime))
                ucError.ErrorMessage = "Time to should be grater than From Time.";
        }        
        return (!ucError.IsError);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        if (dt.Rows.Count <= 0)
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void BindDates()
    {
        ddlDate.Items.Clear();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlDate.Items.Add(li);

        DataTable dt = PhoenixPreSeaWeeklyPlanner.ListDaysinWeek(int.Parse(ucWeek.SelectedWeek));

        ddlDate.DataSource = dt;
        ddlDate.DataBind();

        if (dt.Rows.Count > 0)
            txtDayStart.Text = dt.Rows[0]["FLDCLASSSTART"].ToString();
    }

    private void BindSection()
    {
        ddlSection.Items.Clear();

        DataSet ds = PhoenixPreSeaTrainee.ListPreSeaTraineeSection(General.GetNullableInteger(ucBatch.SelectedBatch));
        if (ds.Tables.Count > 0)
        {
            ListItem li = new ListItem("--Select--", "DUMMY");

            ddlSection.Items.Add(li);
            ddlSection.DataBind();

            ddlSection.DataSource = ds.Tables[0];
            ddlSection.DataBind();
        }
    }

    private void BindClassRoom()
    {
        ddlClassRoom.Items.Clear();
        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlClassRoom.Items.Add(li);

        DataTable dt = PhoenixPreSeaWeeklyPlanner.ListAvailableClassRoom(General.GetNullableDateTime(ddlDate.SelectedValue), ucTimeSlotFrom.SelectedTimeSlot, ucTimeSlotTo.SelectedTimeSlot);

        ddlClassRoom.DataSource = dt;           
        ddlClassRoom.DataBind();
    }

    protected void BindFaculty(DropDownList ddl)
    {
        if (General.GetNullableInteger(ucBatch.SelectedBatch).HasValue)
        {
            ddl.Items.Clear();
            ListItem li = new ListItem("--Select--", "DUMMY");
            ddl.Items.Add(li);

            //DataSet ds = PhoenixPreSeaBatchManager.ListPreSeaBatchContact(Convert.ToInt32(ucBatch.SelectedBatch));
            DataSet ds = PhoenixPreSeaBatchAdmissionContact.ListPreSeaBatchAdmissionContact(Convert.ToInt32(ucBatch.SelectedBatch));
            ddl.DataSource = ds.Tables[0];
            ddl.DataTextField = "FLDCONTACTNAME";
            ddl.DataValueField = "FLDUSERCODE";
            ddl.DataBind();
        }
    }
    protected void BindFacultyPreparedBy(DropDownList ddl)
    {
        if (General.GetNullableInteger(ucBatch.SelectedBatch).HasValue)
        {
            ddl.Items.Clear();
            ListItem li = new ListItem("--Select--", "DUMMY");
            ddl.Items.Add(li);

            //DataSet ds = PhoenixPreSeaBatchManager.ListPreSeaBatchContact(Convert.ToInt32(ucBatch.SelectedBatch));
            DataSet ds = PhoenixPreSeaBatchAdmissionContact.ListPreSeaBatchAdmissionContact(Convert.ToInt32(ucBatch.SelectedBatch));
            ddl.DataSource = ds.Tables[0];
            ddl.DataTextField = "FLDCONTACTNAME";
            ddl.DataValueField = "FLDUSERCODE";
            ddl.DataBind();
        }
    }

    protected void BindPracticalDivisions(string batch, string section)
    {
        DataSet ds = PhoenixPreSeaTrainee.ListPreSeaPractical(General.GetNullableInteger(batch), General.GetNullableInteger(section));
        if (ds.Tables.Count > 0)
        {
            grdPracticalTimetable.DataSource = ds.Tables[0];
            grdPracticalTimetable.DataBind();
        }
    }

    protected void FillPracticalDetails(string divisions, string subjects, string instructors)
    {
        string[] divisionValue = divisions.Split(',');
        string[] subjectValue = subjects.Split(',');
        string[] instructorValue = instructors.Split(',');
        int i = 0;
        foreach (GridViewRow row in grdPracticalTimetable.Rows)
        {

            UserControlPreSeaBatchSubject uc = (UserControlPreSeaBatchSubject)row.FindControl("ucPracSubject");
            DropDownList ddl = (DropDownList)row.FindControl("ddlInstructor");
            uc.SelectedSubject = subjectValue[i].ToString();
            ddl.SelectedValue = instructorValue[i].ToString();

        }
    }

    protected void GetPracticalDetails(ref string divisions, ref string subjects, ref string instructors, ref bool notAllfilled)
    {
        foreach (GridViewRow row in grdPracticalTimetable.Rows)
        {
            Label lbl = (Label)row.FindControl("lblDivisionId");
            UserControlPreSeaBatchSubject uc = (UserControlPreSeaBatchSubject)row.FindControl("ucPracSubject");
            DropDownList ddl = (DropDownList)row.FindControl("ddlInstructor");
            if (General.GetNullableInteger(uc.SelectedSubject) == null) //|| General.GetNullableInteger(ddl.SelectedValue) == null
            {
                notAllfilled = true;
                return;
            }
            else
            {
                divisions += lbl.Text + ",";
                subjects += uc.SelectedSubject + ",";
                if (ddl.SelectedValue == "DUMMY")
                    instructors += "0" + ",";
                else
                    instructors += ddl.SelectedValue + ",";
            }
        }
    }

    protected void ClearPracticalDetails()
    {
        foreach (GridViewRow row in grdPracticalTimetable.Rows)
        {

            UserControlPreSeaBatchSubject uc = (UserControlPreSeaBatchSubject)row.FindControl("ucPracSubject");
            DropDownList ddl = (DropDownList)row.FindControl("ddlInstructor");
            uc.SelectedSubject = "";
            ddl.SelectedValue = "DUMMY";

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
            else if (current is ImageButton)
            {
                control.Controls.Remove(current);
            }

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

    protected void ClearEntryScreen()
    {
        ViewState["DAYID"] = "";

        ddlDate.SelectedValue = "DUMMY";
        rdoActivity.ClearSelection();
        ddlDuration.SelectedValue = "";
        ucTimeSlotFrom.SelectedTime = "";
        ucTimeSlotTo.SelectedTime = "";

        ucSubject.SelectedSubject = "";
        ddlFaculty.SelectedValue = "DUMMY";
        ddlFacultyPreparedBy.SelectedValue = "DUMMY";
    }

    #endregion

    # region :  Dropdown Change events  :

    protected void Batch_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucBatch.SelectedBatch).HasValue)
        {
            ViewState["BATCH"] = ucBatch.SelectedBatch;

            ucSemester.Enabled = true;
            ucSemester.Batch = ucBatch.SelectedBatch;
            ucSemester.bind();

            if (String.IsNullOrEmpty(ViewState["WEEKID"].ToString()))
            {
                DataTable dt = PhoenixPreSeaWeeklyPlanner.GetCurrentWeekofBatch(General.GetNullableInteger(ucBatch.SelectedBatch));
                if (dt.Rows.Count > 0)
                {
                    ViewState["WEEKID"] = dt.Rows[0]["FLDWEEKID"].ToString();

                    ucSemester.SelectedSemester = dt.Rows[0]["FLDSEMESTERID"].ToString();

                    ucWeek.Enabled = true;
                    ucWeek.Batch = ucBatch.SelectedBatch;
                    ucWeek.Semester = dt.Rows[0]["FLDSEMESTERID"].ToString();
                    ucWeek.bind();

                    ClassDetails.Visible = true;
                    ucSubject.Batch = ucBatch.SelectedBatch;
                    ucSubject.Semester = dt.Rows[0]["FLDSEMESTERID"].ToString();
                    ucSubject.bind();

                    ucWeek.SelectedWeek = dt.Rows[0]["FLDWEEKID"].ToString();

                    BindDates();
                    ddlDate.SelectedValue = DateTime.Today.Date.ToShortDateString();


                    // Get Current date day id
                    if (General.GetNullableInteger(ddlSection.SelectedValue).HasValue)
                    {
                        ViewState["DAYID"] = "";
                        DataTable dtDay = PhoenixPreSeaWeeklyPlanner.ListDayEntry(General.GetNullableInteger(ucBatch.SelectedBatch)
                                                                , General.GetNullableInteger(dt.Rows[0]["FLDSEMESTERID"].ToString())
                                                                , General.GetNullableInteger(ddlSection.SelectedValue)
                                                                , General.GetNullableInteger(dt.Rows[0]["FLDWEEKID"].ToString())
                                                                , General.GetNullableDateTime(DateTime.Today.Date.ToShortDateString()));
                        if (dtDay.Rows.Count > 0)
                            ViewState["DAYID"] = dtDay.Rows[0]["FLDDAYID"].ToString();
                    }
                }
            }

            BindSection();
            BindFaculty(ddlFaculty);
            BindFacultyPreparedBy(ddlFacultyPreparedBy);
        }
        BindWeeklyPlan();

    }

    protected void Semester_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucSemester.SelectedSemester).HasValue)
        {
            ucWeek.Enabled = true;
            ucWeek.Batch = ucBatch.SelectedBatch;
            ucWeek.Semester = ucSemester.SelectedSemester;
            ucWeek.bind();

            ClassDetails.Visible = true;
            ucSubject.Batch = ucBatch.SelectedBatch;
            ucSubject.Semester = ucSemester.SelectedSemester;
            ucSubject.bind();
        }
        BindWeeklyPlan();
    }

    protected void Week_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucWeek.SelectedWeek).HasValue)
        {
            ViewState["WEEKID"] = ucWeek.SelectedWeek;
            BindDates();
        }
        BindWeeklyPlan();
    }

    protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ddlSection.SelectedValue).HasValue)
        {
            ViewState["SECTIONID"] = ddlSection.SelectedValue;
            BindWeeklyPlan();
            if (rdoActivity.SelectedItem != null && rdoActivity.SelectedItem.Text.ToUpper().Contains("PRACTICAL"))
                BindPracticalDivisions(ucBatch.SelectedBatch, ddlSection.SelectedValue);
        }
        BindWeeklyPlan();
    }

    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (General.GetNullableDateTime(ddlDate.SelectedValue).HasValue)
        {
            ViewState["DAYID"] = "";
            DataTable dt = PhoenixPreSeaWeeklyPlanner.ListDayEntry(General.GetNullableInteger(ucBatch.SelectedBatch)
                                                    , General.GetNullableInteger(ucSemester.SelectedSemester)
                                                    , General.GetNullableInteger(ddlSection.SelectedValue)
                                                    , General.GetNullableInteger(ucWeek.SelectedWeek)
                                                    , General.GetNullableDateTime(ddlDate.SelectedValue));
            if (dt.Rows.Count > 0)
                ViewState["DAYID"] = dt.Rows[0]["FLDDAYID"].ToString();
        }
        BindWeeklyPlan();
    }

    protected void rdoActivity_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoActivity.SelectedItem.Text.ToUpper().Contains("PRACTICAL"))
        {
            if (General.GetNullableInteger(ucBatch.SelectedBatch).HasValue && General.GetNullableInteger(ddlSection.SelectedValue).HasValue)
                BindPracticalDivisions(ucBatch.SelectedBatch, ddlSection.SelectedValue);
            else
            {
                ucError.ErrorMessage = "For planning practical class, batch and section has to be selected first.";
                ucError.Visible = true;
                return;
            }

            ucSubject.Enabled = false;
            ddlFaculty.Enabled = false;
            ucSubject.CssClass = "readonlytextbox";
            ddlFaculty.CssClass = "readonlytextbox";
            grdPracticalTimetable.Enabled = true;
            txtBreakDesc.Text = "";
            txtBreakDesc.CssClass = "readonlytextbox";
            txtBreakDesc.Enabled = false;

            ClassDetails.Visible = false;
            PracticalDetails.Visible = true;
        }
        else if (rdoActivity.SelectedItem.Text.Contains("Theory"))
        {
            ucSubject.CssClass = "input_mandatory";
            ddlFaculty.CssClass = "input_mandatory";
            ucSubject.Enabled = true;
            ddlFaculty.Enabled = true;
            grdPracticalTimetable.Enabled = false;
            txtBreakDesc.Text = "";
            txtBreakDesc.CssClass = "readonlytextbox";
            txtBreakDesc.Enabled = false;

            PracticalDetails.Visible = false;
            ClassDetails.Visible = true;
        }
        else
        {
            ucSubject.CssClass = "readonlytextbox";
            ddlFaculty.CssClass = "readonlytextbox";
            ucSubject.Enabled = false;
            ddlFaculty.Enabled = false;
            txtBreakDesc.Enabled = true;
            txtBreakDesc.CssClass = "input_mandatory";

            ClassDetails.Visible = true;
            PracticalDetails.Visible = false;
        }
        BindWeeklyPlan();

    }

    protected void TimeSlot_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableString(ucTimeSlotTo.SelectedTimeSlot) != null)
        {
            BindClassRoom();
        }
        BindWeeklyPlan();
    }

    # endregion

    # region :  Tabstrip Events :

    protected void PreSeaMenu_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("WEEKPLAN"))
        {
            Response.Redirect("../PreSea/PreSeaWeeklyPlanner", false);
        }
        else if (dce.CommandName.ToUpper().Equals("COPY"))
        {
            Response.Redirect("../PreSea/PreSeaWeeklyPlannerMakeCopy.aspx", false);
        }

    }

    protected void MenuPreSeaWeekPlanner_TabStripCommand(object sender, EventArgs e)
    {
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

    protected void MainMenuPreseaWeekPlanner_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["ADDYN"].ToString() == "0")
                {
                    ucError.ErrorMessage = "Please click the 'Add' before Save information.";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    string sublist = "";
                    string instructorlist = "";
                    string divisionlist = "";
                    bool notAllfilled = false;
                    bool ActivitySelected = false;

                    foreach (ListItem li in rdoActivity.Items)
                    {
                        if (li.Selected == true)
                        {
                            ActivitySelected = true;
                            break;
                        }
                    }
                    if (ActivitySelected == true)
                    {
                        if (rdoActivity.SelectedItem.Text.ToUpper().Contains("PRACTICAL"))
                            GetPracticalDetails(ref divisionlist, ref sublist, ref instructorlist, ref notAllfilled);
                    }                    
                    if (!IsValidWeeklyPlan(notAllfilled, ActivitySelected))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    if (String.IsNullOrEmpty(ViewState["DAYID"].ToString()))
                    {
                        Guid outdayid = new Guid();
                        PhoenixPreSeaWeeklyPlanner.InsertDayEntry(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , int.Parse(ucBatch.SelectedBatch)
                                                                , int.Parse(ucSemester.SelectedSemester)
                                                                , int.Parse(ddlSection.SelectedValue)
                                                                , int.Parse(ucWeek.SelectedWeek)
                                                                , DateTime.Parse(ddlDate.SelectedValue)
                                                                , General.GetNullableByte("0")
                                                                , General.GetNullableByte("")
                                                                , ref outdayid
                                                                , General.GetNullableInteger(ddlFacultyPreparedBy.SelectedValue));
                        if (outdayid != null)
                        {
                            ViewState["DAYID"] = outdayid.ToString();

                            Guid outhourid = new Guid();
                            PhoenixPreSeaWeeklyPlanner.InsertHourlyEntry(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , new Guid(outdayid.ToString())
                                                                        , int.Parse(rdoActivity.SelectedValue)
                                                                        , ucTimeSlotFrom.SelectedTimeSlot
                                                                        , ucTimeSlotTo.SelectedTimeSlot
                                                                        , General.GetNullableByte("0")
                                                                        , General.GetNullableInteger(ucSubject.SelectedSubject)
                                                                        , General.GetNullableInteger(ddlFaculty.SelectedValue)
                                                                        , sublist
                                                                        , divisionlist
                                                                        , instructorlist
                                                                        , decimal.Parse(ddlDuration.SelectedValue)
                                                                        , ref outhourid
                                                                        , txtBreakDesc.Text.Trim()
                                                                        , General.GetNullableInteger(ddlClassRoom.SelectedValue), "");

                        }


                    }
                    else
                    {
                        Guid outhourid = new Guid();
                        PhoenixPreSeaWeeklyPlanner.InsertHourlyEntry(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , new Guid(ViewState["DAYID"].ToString())
                                                                        , int.Parse(rdoActivity.SelectedValue)
                                                                        , ucTimeSlotFrom.SelectedTimeSlot
                                                                        , ucTimeSlotTo.SelectedTimeSlot
                                                                        , General.GetNullableByte("0")
                                                                        , General.GetNullableInteger(ucSubject.SelectedSubject)
                                                                        , General.GetNullableInteger(ddlFaculty.SelectedValue)
                                                                        , sublist
                                                                        , divisionlist
                                                                        , instructorlist
                                                                        , decimal.Parse(ddlDuration.SelectedValue)
                                                                        , ref outhourid
                                                                        , txtBreakDesc.Text.Trim()
                                                                        , General.GetNullableInteger(ddlClassRoom.SelectedValue), "");
                    }
                    ucStatus.Text = "Plan updated Successfully.";

                    ViewState["ADDYN"] = "0";
                    MainMenuPreseaWeekPlanner.SelectedMenuIndex = 1;
                }

            }
            else if (dce.CommandName.ToUpper().Equals("ADD"))
            {
                ClearEntryScreen();
                ClearPracticalDetails();

                ViewState["ADDYN"] = "1";
                MainMenuPreseaWeekPlanner.SelectedMenuIndex = 0;

            }

            BindWeeklyPlan();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

    # region :  Other Control Events :

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindWeeklyPlan();
    }

    protected void PrevClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = PhoenixPreSeaWeeklyPlanner.GetPrevNextWeekofBatch(Convert.ToInt32(ucBatch.SelectedBatch), Convert.ToInt32(ucSemester.SelectedSemester), Convert.ToInt32(ViewState["WEEKID"].ToString()), -1);
            if (dt.Rows.Count > 0)
            {
                ViewState["WEEKID"] = dt.Rows[0]["FLDWEEKID"].ToString();
                ddlDate.DataSource = PhoenixPreSeaWeeklyPlanner.ListDaysinWeek(int.Parse(ViewState["WEEKID"].ToString()));
                ddlDate.DataBind();
            }

            BindWeeklyPlan();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void NextClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = PhoenixPreSeaWeeklyPlanner.GetPrevNextWeekofBatch(Convert.ToInt32(ucBatch.SelectedBatch), Convert.ToInt32(ucSemester.SelectedSemester), Convert.ToInt32(ViewState["WEEKID"].ToString()), 1);
            if (dt.Rows.Count > 0)
            {
                ViewState["WEEKID"] = dt.Rows[0]["FLDWEEKID"].ToString();
                ddlDate.DataSource = PhoenixPreSeaWeeklyPlanner.ListDaysinWeek(int.Parse(ViewState["WEEKID"].ToString()));
                ddlDate.DataBind();
            }
            BindWeeklyPlan();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

    #region :   Grid Events :

    protected void grdPracticalTimetable_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            UserControlPreSeaBatchSubject ucSub = (UserControlPreSeaBatchSubject)e.Row.FindControl("ucPracSubject");
            if (ucSub != null)
            {
                ucSub.SubjectList = PhoenixPreSeaBatchManager.ListPreSeaBatchSubject(General.GetNullableInteger(ucBatch.SelectedBatch), General.GetNullableInteger(ucSemester.SelectedSemester));
            }
            DropDownList ddlInstuctor = (DropDownList)e.Row.FindControl("ddlInstructor");
            if (ddlInstuctor != null)
            {
                BindFaculty(ddlInstuctor);
            }
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
                DataTable data = dsGrid.Tables[2];
                if (timeslots.Rows.Count > 0 && timeslots.Columns.Contains("FLDHOURSLOTS"))
                {
                    string holidays = timeslots.Rows[0]["FLDHOLIDAYS"].ToString();

                    Literal ltrlHRs = new Literal();
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[0].VerticalAlign = VerticalAlign.Middle;
                    e.Row.Cells[0].Style.Add("font-weight", "bold");
                    e.Row.Cells[0].Style.Add("vertical-align", "middle");
                    ltrlHRs.Text = timeslots.Rows[e.Row.RowIndex]["FLDHOURSLOTS"].ToString() + "&nbsp;&nbsp;";

                    ImageButton ib = new ImageButton();
                    ib.ID = "ibdelSlots" + e.Row.RowIndex.ToString();
                    ib.ImageUrl = Session["images"] + "/te_del.png";
                    ib.CommandName = "DELETE";
                    ib.ToolTip = "Delete";
                    ib.ImageAlign = ImageAlign.AbsMiddle;
                    ib.CommandArgument = timeslots.Rows[e.Row.RowIndex]["FLDWEEKID"].ToString() + "|" + timeslots.Rows[e.Row.RowIndex]["FLDHOURFROM"].ToString() + "|" + timeslots.Rows[e.Row.RowIndex]["FLDHOURTO"].ToString();                  
                    e.Row.Cells[0].Controls.Add(ltrlHRs);
                    e.Row.Cells[0].Controls.Add(ib);

                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        if (i > 0)
                        {

                            Literal ltrl = new Literal();
                            LinkButton lnkEdit = new LinkButton();
                            Label lblIsBreak = new Label();

                            ImageButton ibdel = new ImageButton();
                            ibdel.ID = "ibdelHours" + i.ToString();
                            ibdel.ImageUrl = Session["images"] + "/te_del.png";
                            ibdel.CommandName = "DELHOURS";
                            ibdel.ToolTip = "Delete";
                            ibdel.ImageAlign = ImageAlign.AbsMiddle;
                            ibdel.Style.Add("margin", "5px");

                            ImageButton ibcopy = new ImageButton();
                            ibcopy.ID = "ibCpyHours" + i.ToString();
                            ibcopy.ImageUrl = Session["images"] + "/copy.png";
                            ibcopy.CommandName = "CPYHOURS";
                            ibcopy.ToolTip = "Copy";
                            ibcopy.ImageAlign = ImageAlign.AbsMiddle;
                            ibcopy.Style.Add("margin", "5px");

                            ImageButton ibPlan = new ImageButton();
                            ibPlan.ID = "ibPlanWeek" + i.ToString();
                            ibPlan.ImageUrl = Session["images"] + "/te_view.png";
                            ibPlan.CommandName = "PLANWEEK";
                            ibPlan.ToolTip = "Week Plan";
                            ibPlan.ImageAlign = ImageAlign.AbsMiddle;
                            ibPlan.Style.Add("margin", "5px");

                            ltrl.ID = "ltrlDay" + i.ToString();
                            lnkEdit.ID = "lnkEdit" + i.ToString();
                            lblIsBreak.ID = "lblIsBreak" + i.ToString();

                            if (!holidays.Contains(Convert.ToDateTime(timeslots.Rows[e.Row.RowIndex]["FLDDAY" + i.ToString()]).ToShortDateString()))
                            {
                                DataRow[] dr = data.Select("FLDDATE = '" + timeslots.Rows[e.Row.RowIndex]["FLDDAY" + i.ToString()].ToString() + "' AND FLDHOURFROM = '" + timeslots.Rows[e.Row.RowIndex]["FLDHOURFROM"].ToString() + "' AND FLDHOURTO = " + timeslots.Rows[e.Row.RowIndex]["FLDHOURTO"].ToString());
                                if (dr != null && dr.Length > 0)
                                {
                                    if (dr[0]["FLDISHOLIDAY"].ToString() != "1" || !String.IsNullOrEmpty(dr[0]["FLDPRACTICALDETAILS"].ToString()))
                                    {
                                        lnkEdit.Text = "Edit";
                                        lnkEdit.CommandName = "SELECT";
                                        lnkEdit.Attributes.Add("onclick", "Openpopup('EditTimeTable', '', 'PreSeaWeeklyPlannerChange.aspx?Day=" + dr[0]["FLDDAYID"].ToString() + "&Hour=" + dr[0]["FLDHOURID"].ToString() + "'); return false;");

                                        if (!String.IsNullOrEmpty(dr[0]["FLDSUBJECT"].ToString()))
                                            ltrl.Text = dr[0]["FLDTHEORYDATA"].ToString();
                                        else if (!String.IsNullOrEmpty(dr[0]["FLDPRACTICALDETAILS"].ToString()))
                                        {
                                            if (dr[0]["FLDPRACTICALDETAILS"].ToString().Contains("- ()"))
                                                ltrl.Text = dr[0]["FLDPRACTICALDETAILS"].ToString().Replace("- ()", "");
                                            else
                                            ltrl.Text = dr[0]["FLDPRACTICALDETAILS"].ToString();
                                        }
                                        else
                                            ltrl.Text = dr[0]["FLDBREAKDETAILS"].ToString();

                                        lblIsBreak.Text = dr[0]["FLDISBREAKHOURS"].ToString();
                                        lblIsBreak.Visible = false;

                                        ltrl.Text += "<br/>";
                                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                                        e.Row.Cells[i].Style.Add("font-weight", "bold");

                                        e.Row.Cells[i].Controls.Add(ltrl);
                                        //if (dr[0]["FLDISBREAKHOURS"].ToString().ToUpper().Equals("YES"))
                                        //{
                                        //    e.Row.Cells[i].ColumnSpan = 6;
                                        //}

                                        if (dr[0]["FLDISCLASSCONDUCTED"].ToString().Equals("0") && dr[0]["FLDISBREAKHOURS"].ToString().Equals("No"))
                                        {
                                            ibdel.CommandArgument = dr[0]["FLDHOURID"].ToString();
                                            ibcopy.Attributes.Add("onclick", "Openpopup('CopyTimeTable', '', 'PreSeaWeeklyPlannerCopyHours.aspx?Day=" + dr[0]["FLDDAYID"].ToString() + "&Hour=" + dr[0]["FLDHOURID"].ToString() + "'); return false;");
                                            e.Row.Cells[i].Controls.Add(lnkEdit);
                                            e.Row.Cells[i].Controls.Add(ibdel);
                                            e.Row.Cells[i].Controls.Add(ibcopy);
                                        }
                                        if (dr[0]["FLDISBREAKHOURS"].ToString().Equals("No"))
                                        {
                                            e.Row.Cells[i].Controls.Add(ibPlan);
                                            ibPlan.Attributes.Add("onclick", "Openpopup('ShowPlannerCurrentWeek', '', 'PreSeaWeeklyPlannerIndividual.aspx?Hour=" + dr[0]["FLDHOURID"].ToString() + "&Week=" + dr[0]["FLDWEEKID"].ToString() + "&batch=" + ucBatch.SelectedBatch + "'); return false;");
                                        }                                      
                                        e.Row.Cells[i].Controls.Add(lblIsBreak);
                                    }

                                }
                            }
                            else if (holidays.Contains(Convert.ToDateTime(timeslots.Rows[e.Row.RowIndex]["FLDDAY" + i.ToString()]).ToShortDateString()))
                            {
                                DataTable dt = PhoenixPreSeaHoliday.ListHoliday(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableDateTime(timeslots.Rows[e.Row.RowIndex]["FLDDAY" + i.ToString()].ToString()));
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
                        //AllCellSame = true;
                    }
                    //if (lblIsBreak.Text.ToUpper() == "")
                    //{
                    //    gvRow.Cells[cellCount].Visible = false;
                    //}
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

                int week = Convert.ToInt32(args[0]);
                PhoenixPreSeaWeeklyPlanner.DeleteTimeSlotsForWholeWeek(PhoenixSecurityContext.CurrentSecurityContext.UserCode, week, args[1], args[2]);

                ClearEntryScreen();
                ClearPracticalDetails();
            }
            else if (e.CommandName.ToString().ToUpper() == "DELHOURS")
            {
                Guid hourid = new Guid(e.CommandArgument.ToString());
                PhoenixPreSeaWeeklyPlanner.DeleteHourlyEntry(PhoenixSecurityContext.CurrentSecurityContext.UserCode, hourid);

                ClearEntryScreen();
                ClearPracticalDetails();
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

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    #endregion

}
