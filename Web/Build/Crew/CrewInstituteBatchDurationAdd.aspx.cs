using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewInstituteBatchDurationAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetBatch();
            SetCourse();
            ddlYear.SelectedYear = DateTime.Now.Year.ToString();
            ddlMonth.SelectedMonthNumber = DateTime.Now.Month.ToString();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            Title.AccessRights = this.ViewState;
            Title.MenuList = toolbar.Show();
        }
        BindData();
        Guidlines();
    }

    protected void cldBathPlanner_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        ddlMonth.SelectedMonthNumber = e.NewDate.Month.ToString();
        ddlYear.SelectedYear = e.NewDate.Year.ToString();
        BindData();
    }
    protected void ddlYear_TextChangedEvent(object sender, EventArgs e)
    {
        string date = "01/" + ddlMonth.SelectedMonthName + "/" + ddlYear.SelectedYear;
        cldBathPlanner.SelectedDate = Convert.ToDateTime(date);
        BindData();
    }

    protected void ddlMonth_TextChangedEvent(object sender, EventArgs e)
    {
        string date = "01/" + ddlMonth.SelectedMonthName + "/" + ddlYear.SelectedYear;
        cldBathPlanner.SelectedDate = Convert.ToDateTime(date);
        BindData();
    }

    
    private void BindData()
    {
        try
        {
            int year, month;
            if (string.IsNullOrEmpty(ddlYear.SelectedYear))
                year = DateTime.Now.Year;
            else
                year = General.GetNullableInteger(ddlYear.SelectedYear).Value;

            if (!string.IsNullOrEmpty(ddlMonth.SelectedMonthNumber))
                month = General.GetNullableInteger(ddlMonth.SelectedMonthNumber).Value;
            else
                month = DateTime.Now.Month;
            string batchId = Request.QueryString["batchId"].ToString();

            DataSet ds = PhoenixCrewInstituteBatch.CrewInstituteBatchPlanList(General.GetNullableInteger(txtInstituteId.Text).Value
                                                                        , General.GetNullableGuid(batchId).Value
                                                                        , month
                                                                        , year
                                                                        , General.GetNullableInteger(txtcourseId.Text)
                                                                        );
            if (ds.Tables.Count > 0)
            {
                //if (ds.Tables[1].Rows.Count > 0)
                //{
                string subject = "";
                DataColumn column = new DataColumn();
                column.ColumnName = "FLDSUBJECT";
                ds.Tables[1].Columns.Add(column);
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    if (ds.Tables[1].Rows[i]["FLDCOURSECALENDARID"].ToString() != "")
                        subject += "Course Planned" + Environment.NewLine;
                    if (ds.Tables[1].Rows[i]["FLDBATCHDURATIONID"].ToString() != "")
                        subject = "Batch Planned";

                    ds.Tables[1].Rows[i]["FLDSUBJECT"] = subject;
                    subject = "";
                }

                cldBathPlanner.DataSubjectField = "FLDSUBJECT";
                cldBathPlanner.DataKeyField = "FLDCOURSECALENDARID";
                cldBathPlanner.DataStartField = "FLDDATE";
                cldBathPlanner.DataEndField = "FLDDATE";
                DataView dv = ds.Tables[1].DefaultView;
                cldBathPlanner.DataSource = ds.Tables[1];
                cldBathPlanner.DataBind();
                cldBathPlanner.SelectedView = SchedulerViewType.MonthView;
                cldBathPlanner.Visible = true;
                //}
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetBatch()
    {
        string batchId = Request.QueryString["batchId"];
        DataTable dt = PhoenixCrewInstituteBatch.CrewInstituteBatchEdit(General.GetNullableGuid(batchId));
        if (dt.Rows.Count > 0)
        {
            Title.Title = "Batch :" + dt.Rows[0]["FLDBATCHNO"].ToString();
            //txtBatchNo.Text = dt.Rows[0]["FLDBATCHNO"].ToString();
            txtInstituteId.Text = dt.Rows[0]["FLDINSTITUTEID"].ToString();
            txtInstituteName.Text = dt.Rows[0]["FLDNAME"].ToString();
            txtInstituteId.Text = dt.Rows[0]["FLDINSTITUTEID"].ToString();
        }
    }
    private void SetCourse()
    {
        string courseId = null;
        if (Request.QueryString["courseId"] != null)
           courseId = Request.QueryString["courseId"].ToString();

        DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(General.GetNullableInteger(courseId).Value);
        if (ds.Tables[0].Rows.Count > 0)
        {           
            txtcourseId.Text = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
            txtCourse.Text = ds.Tables[0].Rows[0]["FLDABBREVIATION"].ToString() + "-" + ds.Tables[0].Rows[0]["FLDCOURSE"].ToString();            
        }                
    }
    private void Guidlines()
    {
        btnTooltipHelp.Text = "<table> <tr><td>&nbsp;1. Course Planned days are highlighted in blue color and batch planned dates are highlighted in green color.</td></tr><tr><td>&nbsp;2. To plane a batch click on the course planned date once and to unplan click on the delete button on batch planned day. </td></tr> <tr><td>&nbsp;3. When clicking on course unplanned date, course will plan and the date will add into batch. </td></tr> </table>";

        //ucToolTipNW.Text = "<table> <tr><td>&nbsp;1. Course Planned days are highlighted in blue color and batch planned dates are highlighted in green color.</td></tr><tr><td>&nbsp;2. To plane a batch click on the course planned date once and to unplan click on the date twice. </td></tr> <tr><td>&nbsp;3. When clicking on course unplanned date, course will plan and the date will add into batch. </td></tr> </table>";
        //imgnotes.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'visible');");
        //imgnotes.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipNW.ToolTip + "', 'hidden');");
    }

    protected void cldBathPlanner_NavigationCommand(object sender, SchedulerNavigationCommandEventArgs e)
    {

        if (e.Command == SchedulerNavigationCommand.SwitchToDayView)
        {
            e.Cancel = true;
        }
        if (e.Command == SchedulerNavigationCommand.SwitchToSelectedDay)
        {
            e.Cancel = true;            
            string batchId = Request.QueryString["batchId"].ToString();
            string batchcoursemappingId = Request.QueryString["batchcoursemapId"].ToString();
            if (cldBathPlanner.DataSource != null)
            {
                DataSet ds = ((DataTable)cldBathPlanner.DataSource).DataSet;
                DataTable dt = ds.Tables[0];
                DataRow[] dv = dt.Select("FLDDATE = '" + e.SelectedDate + "'");
                string calendarId = dv[0]["FLDCALENDARID"].ToString();
                PhoenixCrewInstituteBatch.CrewInstituteBatchDurationInsert(General.GetNullableGuid(batchId).Value
                                                                 , General.GetNullableGuid(calendarId).Value
                                                                 , General.GetNullableGuid(batchcoursemappingId).Value
                                                                 , General.GetNullableInteger(txtInstituteId.Text).Value
                                                                 , General.GetNullableInteger(txtcourseId.Text).Value);
            }
            
        }
        BindData();
    }

    protected void cldBathPlanner_AppointmentDelete(object sender, AppointmentDeleteEventArgs e)
    {
        DataTable dt = (DataTable)cldBathPlanner.DataSource;
        DataRow[] dr = dt.Select("FLDCOURSECALENDARID='" + e.Appointment.ID + "'");
        string batchDurationId = dr[0]["FLDBATCHDURATIONID"].ToString();
        if (batchDurationId != "")
            PhoenixCrewInstituteBatch.CrewBatchDurationDelete(General.GetNullableGuid(batchDurationId).Value);
        BindData();
    }

    protected void cldBathPlanner_AppointmentDataBound(object sender, SchedulerEventArgs e)
    {        
        DataTable dt = (DataTable)cldBathPlanner.DataSource;
        DataRow[] dr = dt.Select("FLDCOURSECALENDARID='" + e.Appointment.ID + "'");
        if (dr[0]["FLDBATCHDURATIONID"].ToString() != "")
            e.Appointment.BackColor = System.Drawing.Color.LightGreen;
        else
            e.Appointment.AllowDelete = false;
    }

  

    protected void cldBathPlanner_TimeSlotCreated(object sender, TimeSlotCreatedEventArgs e)
    {
        if (cldBathPlanner.SelectedView == SchedulerViewType.MonthView)
        {
            String month = cldBathPlanner.SelectedDate.Month.ToString();
            if (e.TimeSlot.Start.Date.Month.ToString() != month)
            {
                e.TimeSlot.Control.Controls.Clear();
            }
        }
    }

    protected void cldBathPlanner_NavigationComplete(object sender, SchedulerNavigationCompleteEventArgs e)
    {
        ddlMonth.SelectedMonthNumber = "";
        ddlMonth.SelectedMonthNumber = cldBathPlanner.SelectedDate.Month.ToString();
        ddlYear.SelectedYear = "";
        ddlYear.SelectedYear = cldBathPlanner.SelectedDate.Year.ToString();
        BindData();
    }

    protected void cldBathPlanner_FormCreating(object sender, SchedulerFormCreatingEventArgs e)
    {
        e.Cancel = true;
    }
}