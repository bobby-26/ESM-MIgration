using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using Telerik.Web.UI;
public partial class Crew_CrewInstituteFacultyPlanner : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtinstitutefacultyId.Attributes.Add("style", "display:none;");
            txtInstituteMappingId.Attributes.Add("style", "display:none;");
           
            ddlCalenderYear.SelectedYear = DateTime.Now.Year.ToString();
            ddlMonth.SelectedMonthNumber = DateTime.Now.Month.ToString();

            SetFaculty();
            SetInstitute();
            Guidlines();

        }
        BindData();
    }    

    private void SetInstitute()
    {
        string instituteId = Request.QueryString["instituteId"].ToString();
        DataTable dt = PhoenixCrewInstitute.CrewInstituteEdit(General.GetNullableInteger(instituteId).Value);
        if (dt.Rows.Count > 0)
        {
            txtInstituteName.Text = dt.Rows[0]["FLDINSTITUTENAME"].ToString();
            txtInstituteMappingId.Text= instituteId;
        }
    }
    private void SetFaculty()
    {
        string facultyId = Request.QueryString["facultyId"].ToString();
        DataTable dt = PhoenixCrewInstituteFaculty.CrewInstituteFacultyEdit(General.GetNullableInteger(facultyId).Value);
        if (dt.Rows.Count > 0)
        {
            txtFaculty.Text = dt.Rows[0]["FLDFACULTYNAME"].ToString();
            txtinstitutefacultyId.Text = facultyId;
        }
    }

    private bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(General.GetNullableString(ddlMonth.SelectedMonthNumber)))
            ucError.ErrorMessage = "Month is required.";

        if (string.IsNullOrEmpty(General.GetNullableString(ddlCalenderYear.SelectedYear)))
            ucError.ErrorMessage = "Year is required.";

        return (!ucError.IsError);
    }
    protected void MenuTitle_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SHOW"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
                string date = "01/"+ddlMonth.SelectedMonthNumber+"/" + ddlCalenderYear.SelectedYear.ToString();
                cldFacultyPlanner.SelectedDate = General.GetNullableDateTime(date).Value;
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
        int year,month;
        string instituteId = Request.QueryString["instituteId"].ToString();
        string facultyId = Request.QueryString["facultyId"].ToString();

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
                                                                                    , General.GetNullableInteger(instituteId).Value
                                                                                    , null);


            if (ds.Tables.Count > 0)
            {
                cldFacultyPlanner.DataSubjectField = "FLDPLANTYPE";
                cldFacultyPlanner.DataKeyField = "FLDFACULTYCALENDARID";
                cldFacultyPlanner.DataStartField = "FLDDATE";
                cldFacultyPlanner.DataEndField = "FLDDATE";
                DataView dv = ds.Tables[2].DefaultView;
                cldFacultyPlanner.DataSource = ds.Tables[2];
                cldFacultyPlanner.DataBind();
                cldFacultyPlanner.SelectedView = SchedulerViewType.MonthView;
                cldFacultyPlanner.Visible = true;                
            }        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlCalenderYear_TextChangedEvent(object sender, EventArgs e)
    {
        string date = "01/" + ddlMonth.SelectedMonthName + "/" + ddlCalenderYear.SelectedYear;
        cldFacultyPlanner.SelectedDate = Convert.ToDateTime(date);
        BindData();
    }

    protected void ddlMonth_TextChangedEvent(object sender, EventArgs e)
    {
        string date = "01/" + ddlMonth.SelectedMonthName + "/" + ddlCalenderYear.SelectedYear;
        cldFacultyPlanner.SelectedDate = Convert.ToDateTime(date);
        BindData();
    }

    protected void cldFacultyPlanner_NavigationCommand(object sender, SchedulerNavigationCommandEventArgs e)
    {
        try
        {
            if (e.Command == SchedulerNavigationCommand.SwitchToSelectedDay)
            {
                e.Cancel = true;
                DataTable dt = ((DataTable)cldFacultyPlanner.DataSource).DataSet.Tables[0];
                DataRow[] dv = dt.Select("FLDDATE = '" + e.SelectedDate + "'");
                if (dv.Length > 0)
                {
                    string calendarId = dv[0]["FLDCALENDARID"].ToString();
                    PhoenixCrewInstituteFacultyPlanner.CrewFacultyLeaveInsert(General.GetNullableInteger(txtinstitutefacultyId.Text).Value
                                                                             , General.GetNullableGuid(calendarId).Value
                                                                             , General.GetNullableInteger(txtInstituteMappingId.Text).Value);
                }
                BindData();
            }
            if (e.Command == SchedulerNavigationCommand.SwitchToMonthView)
            {
                e.Cancel = true;
                return;
            }
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
            string facultyCalendarId = e.Appointment.ID.ToString();
            PhoenixCrewInstituteFacultyPlanner.CrewFacultyCalendarDelete(General.GetNullableGuid(facultyCalendarId).Value);
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
        DataTable dt = (DataTable)cldFacultyPlanner.DataSource;
        DataRow[] dr = dt.Select("FLDFACULTYCALENDARID='" + e.Appointment.ID + "'");
        string facultyplan = null;
        if (dr.Length > 0)
            facultyplan = dr[0]["FLDPLANTYPE"].ToString();
        //if (e.Appointment.ID != null)
        //{
        //    e.Appointment.BackColor = System.Drawing.Color.Gray;
        //    e.Appointment.ToolTip = "Faculty Leave";
        //}            
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
    private void Guidlines()
    {
        btnTooltipHelp.Text = "<table> <tr><td>&nbsp;1. Course planned days are highlighted in green color,Faculty planned leaves are highlighted in gray color.</td></tr><tr><td>&nbsp;2. To plan a leave click on the date, to unplan a faculty leave click on the date again.</td> </tr></table>";
    }

    protected void cldFacultyPlanner_NavigationComplete(object sender, SchedulerNavigationCompleteEventArgs e)
    {
        ddlMonth.SelectedMonthNumber = "";
        ddlMonth.SelectedMonthNumber = cldFacultyPlanner.SelectedDate.Month.ToString();
        ddlCalenderYear.SelectedYear = "";
        ddlCalenderYear.SelectedYear = cldFacultyPlanner.SelectedDate.Year.ToString();
        BindData();
    }

    protected void cldFacultyPlanner_FormCreating(object sender, SchedulerFormCreatingEventArgs e)
    {
        e.Cancel = true;
    }
}