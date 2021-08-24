using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using Telerik.Web.UI;
public partial class Crew_CrewFacultyPlanner : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtinstitutefacultyId.Attributes.Add("style", "display:none;");
            txtInstituteMappingId.Attributes.Add("style", "display:none;");            
            SetFaculty();
            SetInstituteCourse();            
            ddlCalenderYear.SelectedYear = DateTime.Now.Year.ToString();
            ddlMonth.SelectedMonthNumber = DateTime.Now.Month.ToString();
        }
        BindData();
        Guidlines();
    }
    private void SetInstituteCourse()
    {        
        string courseInstituteId = Request.QueryString["courseInstituteId"].ToString();        
        DataTable dt = PhoenixCrewCourseInitiation.CrewCourseInstituteEdit(General.GetNullableGuid(courseInstituteId).Value);
        if (dt.Rows.Count>0)
        {
            txtInstituteName.Text = dt.Rows[0]["FLDINSTITUTENAME"].ToString();
            txtInstituteMappingId.Text = dt.Rows[0]["FLDINSTITUTEID"].ToString();
            txtcourse.Text = dt.Rows[0]["FLDABBREVIATION"].ToString() + " - " + dt.Rows[0]["FLDCOURSE"].ToString();
            txtCourseId.Text = dt.Rows[0]["FLDCOURSEID"].ToString();
        }
    }
    private void SetFaculty()
    {
        string facultyId = Request.QueryString["facultyId"].ToString();
        DataTable dt = PhoenixCrewInstituteFaculty.CrewInstituteFacultyEdit(General.GetNullableInteger(facultyId).Value);
        if (dt.Rows.Count > 0)
        {            
            txtinstitutefacultyId.Text = facultyId;
        }
    }
    private void BindData()
    {
        string facultyId = Request.QueryString["facultyId"].ToString();            
        int year,month;

        try
        {
            if (string.IsNullOrEmpty(ddlCalenderYear.SelectedYear))
                year = DateTime.Now.Year;
            else
                year = General.GetNullableInteger(ddlCalenderYear.SelectedYear).Value;

            if (!string.IsNullOrEmpty(ddlMonth.SelectedMonthNumber))
                month = General.GetNullableInteger(ddlMonth.SelectedMonthNumber).Value;
            else
                month = DateTime.Now.Month;

            DataSet ds = PhoenixCrewInstituteFacultyPlanner.CrewFacultyCalendarList(month
                                                                                    , year
                                                                                    , General.GetNullableInteger(facultyId).Value
                                                                                    , General.GetNullableInteger(txtInstituteMappingId.Text).Value
                                                                                    , General.GetNullableInteger(txtCourseId.Text).Value);

            if (ds.Tables.Count > 0)
            {
                //if (ds.Tables[1] > 0)
                //{
                    string subject = "";
                    DataColumn column = new DataColumn();
                    column.ColumnName = "FLDSUBJECT";
                    ds.Tables[1].Columns.Add(column);
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        //ds.Tables[1].Select("FLDFACULTYID='"+ds.Tables[2].Rows[i]["FLDFACULTYID"]+"'")
                        if (ds.Tables[1].Rows[i]["FLDCOURSECALENDARID"].ToString() != "")
                            subject += "Course Planned";
                        //if (ds.Tables[2].Rows[i]["FLDFACULTYCOURSEID"].ToString() != "")
                        //    subject += "Faculty Assigned" ;
                        ds.Tables[1].Rows[i]["FLDSUBJECT"] = subject;
                        subject = "";
                    }

                    cldFacultyPlanner.DataSubjectField = "FLDSUBJECT";
                    cldFacultyPlanner.DataKeyField = "FLDCOURSECALENDARID";
                    cldFacultyPlanner.DataStartField = "FLDDATE";
                    cldFacultyPlanner.DataEndField = "FLDDATE";

                    DataView dv = ds.Tables[1].DefaultView;
                    cldFacultyPlanner.DataSource = ds.Tables[1];
                    cldFacultyPlanner.DataBind();
                    cldFacultyPlanner.SelectedView = SchedulerViewType.MonthView;
                    cldFacultyPlanner.Visible = true;
                //}            
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }    

    protected void MenuTitle_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SHOW"))
            {                              
                string date = "01/" + ddlMonth.SelectedMonthNumber + "/" + ddlCalenderYear.SelectedYear.ToString();
                cldFacultyPlanner.SelectedDate = General.GetNullableDateTime(date).Value;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlMonth_TextChangedEvent(object sender, EventArgs e)
    {
        string date = "01/" + ddlMonth.SelectedMonthName + "/" + ddlCalenderYear.SelectedYear;
        cldFacultyPlanner.SelectedDate = Convert.ToDateTime(date);
        BindData();
    }

    protected void ddlCalenderYear_TextChangedEvent(object sender, EventArgs e)
    {
        string date = "01/" + ddlMonth.SelectedMonthName + "/" + ddlCalenderYear.SelectedYear;
        cldFacultyPlanner.SelectedDate = Convert.ToDateTime(date);
        BindData();
    }
    private void Guidlines()
    {
        btnTooltipHelp.Text = "<table> <tr><td>&nbsp;1. Course planned days are highlighted in blue color, Faculty assigned days are highlighted in green color.</td></tr><tr><td>&nbsp;2. To assign a faculty click on the course planned dates, to remove the faculty click on the date again.</td> </tr><tr><td>&nbsp;3. When clicking on course unplanned dates, will plan a course and assign the faculty.</td></tr></tr></table>";        
    }

    protected void cldFacultyPlanner_NavigationComplete(object sender, SchedulerNavigationCompleteEventArgs e)
    {
        ddlMonth.SelectedMonthNumber = "";        
        ddlMonth.SelectedMonthNumber = cldFacultyPlanner.SelectedDate.Month.ToString();
        ddlCalenderYear.SelectedYear = "";
        ddlCalenderYear.SelectedYear = cldFacultyPlanner.SelectedDate.Year.ToString();        
        BindData();
    }

    protected void cldFacultyPlanner_NavigationCommand(object sender, SchedulerNavigationCommandEventArgs e)
    {
        try
        {
            if (e.Command == SchedulerNavigationCommand.SwitchToDayView)
            {
                e.Cancel = true;
            }
            if (e.Command == SchedulerNavigationCommand.SwitchToSelectedDay)
            {
                e.Cancel = true;
                if (cldFacultyPlanner.DataSource != null)
                {
                    DataSet ds = ((DataTable)cldFacultyPlanner.DataSource).DataSet;
                    DataTable dt = ds.Tables[0];
                    DataRow[] dv = dt.Select("FLDDATE = '" + e.SelectedDate + "'");
                    if (dv.Length > 0)
                    {
                        string calendarId = dv[0]["FLDCALENDARID"].ToString();
                        PhoenixCrewInstituteFacultyPlanner.CrewFacultyCalendarInsert(General.GetNullableInteger(txtinstitutefacultyId.Text)
                                                                              , General.GetNullableInteger(txtInstituteMappingId.Text)
                                                                              , General.GetNullableInteger(txtCourseId.Text)
                                                                              , General.GetNullableGuid(calendarId));
                    }                        
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cldFacultyPlanner_AppointmentDelete(object sender, AppointmentDeleteEventArgs e)
    {
        try
        {
            DataTable dt = ((DataTable)cldFacultyPlanner.DataSource).DataSet.Tables[2];
            DataRow[] dr = dt.Select("FLDCOURSECALENDARID='" + e.Appointment.ID + "'");
            string facultycalendarId = dr[0]["FLDFACULTYCALENDARID"].ToString();
            if (facultycalendarId != "")
                PhoenixCrewInstituteFacultyPlanner.CrewFacultyCalendarDelete(General.GetNullableGuid(facultycalendarId).Value);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void cldFacultyPlanner_AppointmentDataBound(object sender, SchedulerEventArgs e)
    {
        DataTable dt = ((DataTable)cldFacultyPlanner.DataSource).DataSet.Tables[2];
        DataRow[] dr = dt.Select("FLDCOURSECALENDARID='" + e.Appointment.ID + "'");
        string facultyplan = null;
        string facultycanendarId = null;
        if (dr.Length > 0)
        {
            facultyplan = dr[0]["FLDPLANTYPE"].ToString();
            facultycanendarId = dr[0]["FLDFACULTYCALENDARID"].ToString();
        }            
        if (facultyplan == "COR")
        {
            e.Appointment.BackColor = System.Drawing.Color.LightGreen;
            e.Appointment.ToolTip = "Faculty Assigned";
        }
        if (facultyplan == "LEV")
        {
            e.Appointment.BackColor = System.Drawing.Color.Gray;
            e.Appointment.ToolTip = "Faculty Leave";
        }
        if (string.IsNullOrEmpty(facultycanendarId))
            e.Appointment.AllowDelete = false;
    }

    protected void cldFacultyPlanner_TimeSlotCreated(object sender, TimeSlotCreatedEventArgs e)
    {
        if (cldFacultyPlanner.SelectedView == SchedulerViewType.MonthView)
        {
            String month = cldFacultyPlanner.SelectedDate.Month.ToString();
            if (e.TimeSlot.Start.Date.Month.ToString() != month)
            {
                e.TimeSlot.Control.Controls.Clear();                
            }
        }
    }

    protected void cldFacultyPlanner_FormCreating(object sender, SchedulerFormCreatingEventArgs e)
    {
        e.Cancel = true;
    }
}