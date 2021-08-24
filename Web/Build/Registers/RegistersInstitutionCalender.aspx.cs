using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class Registers_RegistersInstitutionCalender : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtInstituteMappingId.Attributes.Add("style", "display:none;");
            setInstitute();
            ddlCalenderMonth.SelectedMonthNumber = DateTime.Now.Month.ToString();
            ddlCalenderYear.SelectedYear = DateTime.Now.Year.ToString();
        }
        BindData();
        Guidlines();
    }
    private void setInstitute()
    {
        string instituteId = null;

        if (Request.QueryString["instituteId"] != null)
            instituteId = Request.QueryString["instituteId"].ToString();

        DataTable dt = PhoenixCrewInstitute.CrewInstituteEdit(General.GetNullableInteger(instituteId).Value);
        if (dt.Rows.Count > 0)
        {
            txtInstituteMappingId.Text = dt.Rows[0]["FLDINSTITUTIONID"].ToString();
            txtInstituteName.Text = dt.Rows[0]["FLDINSTITUTENAME"].ToString();
        }
    }
    private void BindData()
    {
        try
        {
            int year, month;
            if (string.IsNullOrEmpty(txtInstituteName.Text))
            {
                // ucError.Visible = true;
                return;
            }
            if (string.IsNullOrEmpty(ddlCalenderYear.SelectedYear))
                year = DateTime.Now.Year;
            else
                year = General.GetNullableInteger(ddlCalenderYear.SelectedYear).Value;

            if (string.IsNullOrEmpty(ddlCalenderMonth.SelectedMonthNumber))
                month = DateTime.Now.Month;
            else
                month = General.GetNullableInteger(ddlCalenderMonth.SelectedMonthNumber).Value;

            DataSet ds = PhoenixRegistersInstituteCalender.InstituteHolidayAndWorkingDayList(General.GetNullableInteger(txtInstituteMappingId.Text).Value
                                                                                            , year
                                                                                            , month);


            if (ds.Tables.Count > 0)
            {                
                cldInstitute.DataSubjectField = "FLDREMARK";
                cldInstitute.DataKeyField = "FLDINSTITUTECALENDARID";
                cldInstitute.DataStartField = "FLDDATE";
                cldInstitute.DataEndField = "FLDDATE";
                DataView dv = ds.Tables[0].DefaultView;
                cldInstitute.DataSource = ds.Tables[0];
                cldInstitute.DataBind();
                cldInstitute.SelectedView = SchedulerViewType.MonthView;

                //RadScheduler1.SelectedDate = Convert.ToDateTime(dv.Table.Rows[0]["EndDate"]);
                cldInstitute.Visible = true;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    

    protected void ddlCalenderMonth_TextChangedEvent(object sender, EventArgs e)
    {
        string date = "01/" + ddlCalenderMonth.SelectedMonthName + "/" + ddlCalenderYear.SelectedYear;
        cldInstitute.SelectedDate = Convert.ToDateTime(date);
        BindData();
    }

    protected void ddlCalenderYear_TextChangedEvent(object sender, EventArgs e)
    {
        string date = "01/" + ddlCalenderMonth.SelectedMonthName + "/" + ddlCalenderYear.SelectedYear;
        cldInstitute.SelectedDate = Convert.ToDateTime(date);
        BindData();
    }
    
    private void Guidlines()
    {
        btnTooltipHelp.Text = "<table> <tr><td>&nbsp;1. Holidays are highlighted in gray color.</td></tr><tr><td>&nbsp;2. To plan a holiday click on the date and to unplan, click on the delete.</td> </tr></table>";

        //ucToolTipNW.Text = "<table> <tr><td>&nbsp;1. Holidays are highlighted in gray color.</td></tr><tr><td>&nbsp;2. To plan a holiday click on the date once and to unplan click on the date twice.</td> </tr></table>";
        //imgnotes.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'visible');");
        //imgnotes.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'hidden');");
    }

    protected void cldInstitute_NavigationCommand(object sender, Telerik.Web.UI.SchedulerNavigationCommandEventArgs e)
    {
        try
        {
            if (e.Command == SchedulerNavigationCommand.SwitchToSelectedDay)
            {
                e.Cancel = true;              
                DataTable dt = ((DataTable)cldInstitute.DataSource).DataSet.Tables[0];
                DataRow[] dv = dt.Select("FLDDATE = '" + e.SelectedDate + "'");
                string calendarId = dv[0]["FLDCALENDARID"].ToString();                
                PhoenixRegistersInstituteCalender.InstituteCalenderInsert(General.GetNullableInteger(txtInstituteMappingId.Text).Value
                                                                          , General.GetNullableGuid(calendarId).Value);
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

    protected void cldInstitute_AppointmentDelete(object sender, Telerik.Web.UI.AppointmentDeleteEventArgs e)
    {
        try
        {
            string instituteCalendarId = e.Appointment.ID.ToString();
            PhoenixRegistersInstituteCalender.InstituteCalendarDelete(General.GetNullableGuid(instituteCalendarId).Value);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cldInstitute_FormCreating(object sender, Telerik.Web.UI.SchedulerFormCreatingEventArgs e)
    {
        e.Cancel = true;
    }

    protected void cldInstitute_AppointmentDataBound(object sender, Telerik.Web.UI.SchedulerEventArgs e)
    {
        RadScheduler scheduler1 = sender as RadScheduler;
        DataTable dt = (DataTable)scheduler1.DataSource;
        DataRow[] dr = dt.Select("FLDINSTITUTECALENDARID='" + e.Appointment.ID + "'");
        if (dr[0]["FLDINSTITUTECALENDARID"].ToString() != "")
            e.Appointment.BackColor = System.Drawing.Color.Gray;       
    }

    protected void cldInstitute_TimeSlotCreated(object sender, Telerik.Web.UI.TimeSlotCreatedEventArgs e)
    {
        if (cldInstitute.SelectedView == SchedulerViewType.MonthView)
        {
            String month = cldInstitute.SelectedDate.Month.ToString();
            if (e.TimeSlot.Start.Date.Month.ToString() != month)
            {
                e.TimeSlot.Control.Controls.Clear();
            }
        }
    }
}
