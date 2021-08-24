using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Calendar.View;

public partial class PlannedMaintenance_PlannedMaintenanceDailyWorkPlanAdd : PhoenixBasePage
{
    private DateTime startDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {           
            ViewState["ID"] = string.Empty;
            if(!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                ViewState["ID"] = Request.QueryString["id"];
            }            
            Edit();
            DateTime startDate = ((Telerik.Web.UI.Calendar.View.MonthView)txtDate.Calendar.CalendarView).MonthStartDate;
            DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.ListShipCalendar(PhoenixSecurityContext.CurrentSecurityContext.VesselID, startDate);
            Session["SHIPCAL"] = dt;
        }
    }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        startDate = ((MonthView)txtDate.Calendar.CalendarView).MonthStartDate;
    }
    private bool IsValidDailyWorkPlan()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtDate.SelectedDate == null)
            ucError.ErrorMessage = "Date is required.";
        if (General.GetNullableInteger(rblVesselStatus.SelectedValue) == null)
            ucError.ErrorMessage = "Vessel Status is required.";
        if (rblVesselStatus.SelectedValue == "3" && tpChangeTime.SelectedTime == null)
            ucError.ErrorMessage = "Change Time is required.";
        return (!ucError.IsError);
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (!IsValidDailyWorkPlan())
        {
            string script = "function f(){dayRender(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            ucError.Visible = true;            
            return;
        }
        try
        {
            string id = ViewState["ID"].ToString();
            if (General.GetNullableGuid(id) == null)
            {
                PhoenixPlannedMaintenanceDailyWorkPlan.Insert(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , null
                    , txtDate.SelectedDate.Value
                    , int.Parse(rblVesselStatus.SelectedValue)
                    , tpChangeTime.SelectedDate);
            }
            else
            {
                DateTime? changeTime = null;
                if (tpChangeTime.SelectedTime != null)
                {
                    changeTime = txtDate.SelectedDate.Value.AddMinutes(tpChangeTime.SelectedTime.Value.TotalMinutes);
                }
                PhoenixPlannedMaintenanceDailyWorkPlan.Update(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , null
                , txtDate.SelectedDate.Value
                , int.Parse(rblVesselStatus.SelectedValue)
                , changeTime);
            }
            string script = "function f(){parent.CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
        catch (Exception ex)
        {
            ucError.Visible = true;
            ucError.ErrorMessage = ex.Message;
        }
    }
    protected void Edit()
    {
        string id = ViewState["ID"].ToString();
        if (id.Equals(""))
        {            
            txtNo.Text = string.Empty;
            txtDate.SelectedDate = null;
            btnCreate.Text = "Create";
        }
        else
        {
            DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.Edit(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];                
                txtNo.Text = dr["FLDPLANNO"].ToString();
                txtDate.SelectedDate = General.GetNullableDateTime(dr["FLDDATE"].ToString());
                rblVesselStatus.SelectedValue = dr["FLDVESSELSTATUS"].ToString();
                tpChangeTime.SelectedDate = General.GetNullableDateTime(dr["FLDCHANGETIME"].ToString());
                btnCreate.Text = "Save";
            }
        }
    }
    
    protected void RadCalendar1_DefaultViewChanged(object sender, Telerik.Web.UI.Calendar.DefaultViewChangedEventArgs e)
    {
        txtDate.SelectedDate = null;
        RadCalendar c = (RadCalendar)sender;
        startDate = ((Telerik.Web.UI.Calendar.View.MonthView)c.CalendarView).MonthStartDate;
        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.ListShipCalendar(PhoenixSecurityContext.CurrentSecurityContext.VesselID, startDate);
        Session["SHIPCAL"] = dt;
    }
    private int i = 0;
    long DatetimeMinTimeTicks =
      (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

    protected void RadCalendar1_DayRender(object sender, Telerik.Web.UI.Calendar.DayRenderEventArgs e)
    {
        DateTime CurrentDate = e.Day.Date;
        DataTable dt = (DataTable)Session["SHIPCAL"];
        if (dt != null && i < dt.Rows.Count && CurrentDate >= startDate)
        {
            DataRow dr = dt.Rows[i];
            DateTime d = DateTime.Parse(dr["FLDDATE"].ToString());
            e.Cell.Text = "<a href=\"javascript:SetDate(new Date(" + (long)((d.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000) + "),'" + dr["FLDSHIPCALENDARID"].ToString() + "')\">" + dr["FLDDAY"].ToString() + "<br />" + dr["FLDCLOCKNAME"].ToString() + "</a>";
            e.Cell.ToolTip = "";
            e.Cell.Attributes["title"] = "";
            e.Cell.Attributes["data"] = "xx";
            i++;
        }
        else
        {
            e.Cell.CssClass = "rcOutOfRange";
            RadCalendarDay calendarDay = new RadCalendarDay();
            calendarDay.Date = e.Day.Date;
            calendarDay.IsSelectable = false;
            calendarDay.ItemStyle.CssClass = "rcOutOfRange";
            txtDate.Calendar.SpecialDays.Add(calendarDay);
        }
    }
}