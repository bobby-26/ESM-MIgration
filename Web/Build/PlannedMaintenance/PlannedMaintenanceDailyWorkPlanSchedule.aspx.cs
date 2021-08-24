using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceDailyWorkPlanSchedule : PhoenixBasePage
{
    private const string AppointmentsKey = "Scheduler.BindToList";
    //private List<AppointmentInfo> Appointments
    //{
    //    get
    //    {
    //        List<AppointmentInfo> sessApts = Session[AppointmentsKey] as List<AppointmentInfo>;
    //        if (sessApts == null)
    //        {
    //            sessApts = new List<AppointmentInfo>();
    //            Session[AppointmentsKey] = sessApts;
    //        }

    //        return sessApts;
    //    }
    //}
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        schDWP.SelectedDate = DateTime.Now;
        if (!IsPostBack)
        {
            //Session.Remove(AppointmentsKey);
            CreateShipCaleder(schDWP.SelectedDate);
            //PopuldateResourceType();
        }               
        //schDWP.DataSource = Appointments;
    }
    //private void PopuldateResourceType()
    //{
    //    ResourceType resType = new ResourceType("Process");
    //    resType.ForeignKeyField = "FLDCATEGORYID";        
    //    schDWP.ResourceTypes.Add(resType);

    //    DataSet ds = PhoenixInspectionRiskAssessmentCategoryExtn.ListRiskAssessmentCategory();
        
    //    foreach (DataRow dr in ds.Tables[0].Rows)
    //    {
    //        schDWP.Resources.Add(new Resource("Process", dr["FLDCATEGORYID"].ToString(), dr["FLDNAME"].ToString()));
    //        var style = new ResourceStyleMapping();
    //        style.Type = "Process";
    //        style.Key = dr["FLDCATEGORYID"].ToString();
    //        style.Text = dr["FLDNAME"].ToString();
    //        //style.
    //        style.BackColor = System.Drawing.ColorTranslator.FromHtml(dr["FLDCOLOR"].ToString());
    //        schDWP.ResourceStyles.Add(style);
    //    }
    //}
    private void CreateShipCaleder(DateTime startDate)
    {         
        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.ListShipCalendar(PhoenixSecurityContext.CurrentSecurityContext.VesselID, startDate);
        schDWP.DataSource = dt;
        schDWP.DataBind();
        //foreach (DataRow dr in dt.Rows)
        //{
        //    Appointments.Add(new AppointmentInfo(new Guid(dr["FLDDTKEY"].ToString()), dr["FLDNAME"].ToString() + " - "+ dr["FLDCOUNT"].ToString(), DateTime.Parse(dr["FLDACTUALDATE"].ToString()), DateTime.Parse(dr["FLDACTUALDATE"].ToString()), string.Empty, null, String.Empty, int.Parse(dr["FLDCATEGORYID"].ToString())));
        //}
        Session[AppointmentsKey] = dt;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            schDWP.SelectedDate = DateTime.Now;
        }
    }    
    protected void schDWP_NavigationComplete(object sender, SchedulerNavigationCompleteEventArgs e)
    {
        RadScheduler radScheduler = (RadScheduler)sender;
        //if (FindByDate(radScheduler.SelectedDate) == null)
            CreateShipCaleder(radScheduler.SelectedDate);
    }

    protected void schDWP_AppointmentDataBound(object sender, SchedulerEventArgs e)
    {        
        DataRowView drv = (DataRowView)e.Appointment.DataItem;
        if (drv != null)
        {
            //e.Appointment.Attributes["COUNT"] = drv["FLDCOMPLETED"].ToString() + " / " + drv["FLDCOUNT"].ToString();
            //e.Appointment.Attributes["DWPID"] = drv["FLDDAILYWORKPLANID"].ToString();
            //e.Appointment.Attributes["CATEGORYID"] = drv["FLDCATEGORYID"].ToString();
            //e.Appointment.BackColor = System.Drawing.ColorTranslator.FromHtml(drv["FLDCOLOR"].ToString());
            //DateTime endDate = e.Appointment.End;
            //DateTime endTime = endDate.AddHours(24 - endDate.Hour);
            //endTime = endTime.AddMinutes(-endDate.Minute);
            //endTime = endTime.AddSeconds(-endDate.Second);
            //endTime = endTime.AddMilliseconds(-endDate.Millisecond);
            //e.Appointment.End = endTime;
        }
    }

    protected void schDWP_FormCreated(object sender, SchedulerFormCreatedEventArgs e)
    {
        //RadScheduler scheduler = (RadScheduler)sender;

        //if (e.Container.Mode == SchedulerFormMode.Edit || e.Container.Mode == SchedulerFormMode.AdvancedEdit)
        //{
        //    string gid = e.Appointment.Attributes["DWPID"];
        //    DataTable dt1 = (DataTable)scheduler.DataSource;
        //    DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.Edit(new Guid(gid), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        //    if (dt.Rows.Count > 0)
        //    {
        //        RadDatePicker date = (RadDatePicker)e.Container.FindControl("txtDate");
        //        date.SelectedDate = General.GetNullableDateTime(dt.Rows[0]["FLDDATE"].ToString());

        //        RadRadioButtonList rblList = (RadRadioButtonList)e.Container.FindControl("rblVesselStatus");
        //        rblList.SelectedValue = dt.Rows[0]["FLDVESSELSTATUS"].ToString();

        //        RadTimePicker picker = (RadTimePicker)e.Container.FindControl("tpChangeTime");
        //        picker.SelectedDate = General.GetNullableDateTime(dt.Rows[0]["FLDCHANGETIME"].ToString());
        //    }
        //}
        //else if (e.Container.Mode == SchedulerFormMode.Insert)
        //{
        //    Appointment a = e.Appointment;
        //    RadDatePicker date = (RadDatePicker)e.Container.FindControl("txtDate");
        //    date.SelectedDate = a.Start;
        //}
        //else if (e.Container.Mode == SchedulerFormMode.AdvancedInsert)
        //{
        //    Appointment a = e.Appointment;
        //    RadLabel date = (RadLabel)e.Container.FindControl("lblFromDate");
        //    date.Text = General.GetDateTimeToString(a.Start);
        //}
    }

    protected void schDWP_AppointmentCommand(object sender, AppointmentCommandEventArgs e)
    {
        try
        {
            //RadScheduler scheduler = (RadScheduler)sender;
            //if (e.CommandName == "Insert")
            //{
            //    string id = e.Container.Appointment.Attributes["DWPID"];
            //    RadRadioButtonList rblVesselStatus = (RadRadioButtonList)e.Container.FindControl("rblVesselStatus");
            //    RadTimePicker tpChangeTime = (RadTimePicker)e.Container.FindControl("tpChangeTime");
            //    RadDatePicker txtDate = (RadDatePicker)e.Container.FindControl("txtDate");
            //    DateTime? changeTime = null;
            //    if (tpChangeTime.SelectedTime != null)
            //    {
            //        changeTime = txtDate.SelectedDate.Value.AddMinutes(tpChangeTime.SelectedTime.Value.TotalMinutes);
            //    }
            //    if (General.GetNullableGuid(id) == null)
            //    {
            //        PhoenixPlannedMaintenanceDailyWorkPlan.Insert(PhoenixSecurityContext.CurrentSecurityContext.VesselID
            //            , null
            //            , txtDate.SelectedDate.Value
            //            , int.Parse(rblVesselStatus.SelectedValue)
            //            , tpChangeTime.SelectedDate);
            //    }
            //    else
            //    {
            //        PhoenixPlannedMaintenanceDailyWorkPlan.Update(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID
            //            , null
            //            , txtDate.SelectedDate.Value
            //            , int.Parse(rblVesselStatus.SelectedValue)
            //            , changeTime);
            //    }
            //    CreateShipCaleder(scheduler.SelectedDate);
            //}
            //else if (e.CommandName == "Update")
            //{
            //    RadLabel lblFromDate = (RadLabel)e.Container.FindControl("lblFromDate");
            //    RadDatePicker txtDate = (RadDatePicker)e.Container.FindControl("txtDate");
            //    if (txtDate.SelectedDate.HasValue)
            //    {
            //        PhoenixPlannedMaintenanceDailyWorkPlan.Copy(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(lblFromDate.Text), txtDate.SelectedDate.Value);
            //        CreateShipCaleder(scheduler.SelectedDate);
            //    }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {        
        CreateShipCaleder(schDWP.SelectedDate);
    }

    protected void schDWP_TimeSlotCreated(object sender, TimeSlotCreatedEventArgs e)
    {
        RadScheduler scheduler = (RadScheduler)sender;
        DataTable dt = (DataTable)Session[AppointmentsKey];
        if (dt != null)
        {            
            DataRow[] dr = dt.Select("FLDACTUALDATE = '" + e.TimeSlot.Start.ToString() + "'");
            if(dr.Length > 0)
            {
                HtmlGenericControl ctrl = new HtmlGenericControl();
                ctrl.InnerHtml = "<i class='fas fa-toolbox'></i>";
                ctrl.Attributes["style"] = "float:right";
                ctrl.Attributes["class"] = "icon";
                DateTime date = DateTime.Parse(dr[0]["FLDACTUALDATE"].ToString());
                int cnt = int.Parse(dr[0]["FLDTOOLBOXMEETINGCOUNT"].ToString().Equals("") ? "-1" : dr[0]["FLDTOOLBOXMEETINGCOUNT"].ToString());
                if (date < DateTime.Now.Date)
                {
                    ctrl.Attributes["style"] = "float:right;color: " + (cnt > 0 ? "green" : "red");
                }
                else if (DateTime.Now.Date == date)
                {
                    ctrl.Attributes["style"] = "float:right;color: " + (cnt > 0 ? "green" : "yellow");
                }
                else if (DateTime.Now.Date < date)
                {
                    ctrl.Attributes["style"] = "float:right;color: " + (cnt > 0 ? "green" : "yellow");
                }
                if (cnt > -1)
                {
                    e.TimeSlot.Control.Controls[0].Controls[0].Controls.Add(ctrl);
                }
            }
            for (int i = 0; i < e.TimeSlot.Appointments.Count; i++)              
            {
                Appointment apt = e.TimeSlot.Appointments[i];
                apt.Attributes["style"] = "display:none";
                foreach (AppointmentControl ctrl in apt.AppointmentControls)
                {
                    ctrl.Attributes["style"] = "display:none";                  
                }
            }
            bool pmsimg = false;
            for (int i = 0; i < dr.Length; i++)
            {                
                DataRow d = dr[i];
                if (d["FLDDAILYWORKPLANID"].ToString() != string.Empty)
                {
                    string img = GetImage(d["FLDCODE"].ToString());                    
                    if (d["FLDID"].ToString() != "-2")
                    {
                        Label lbl = new Label
                        {
                            //Text = d["FLDNAME"].ToString() + " - " + d["FLDCOMPLETED"].ToString() + " / " + d["FLDCOUNT"].ToString(),
                            ToolTip = d["FLDNAME"].ToString()// + " - " + d["FLDCOMPLETED"].ToString() + " / " + d["FLDCOUNT"].ToString(),                        
                        };
                        if ((img.ToLower().Contains("plannedmaintenance.png") && !pmsimg) || img.ToLower() != "plannedmaintenance.png")
                        {
                            lbl.Attributes["style"] = "background-image: url('" + Session["images"] + "/" + img + "');background-size: 34px 34px;background-repeat: no-repeat;height:36px;width:36px;display:inline-block";
                            e.TimeSlot.Control.Controls[i > 3 ? 3 : 2].Controls.Add(lbl);
                            ((System.Web.UI.WebControls.WebControl)e.TimeSlot.Control.Controls[i > 3 ? 3 : 2]).Attributes["style"] = "z-index: 4;height:36px";
                        }
                        if (img.ToLower().Contains("plannedmaintenance.png"))
                        {
                            pmsimg = true;
                        }
                    }                    
                    e.TimeSlot.Control.Attributes["ondblclick"] = "top.openNewWindow('dp','Daily Work Plan','PlannedMaintenance/PlannedMaintenanceDailyWorkPlanDetail.aspx?p=" + d["FLDDAILYWORKPLANID"].ToString() + "',true, null, null, null, null,{icon:'<i class=\\'fa fa-calendar\\'></i>',disableMinMax:true});";
                    //e.TimeSlot.Control.Attributes["style"] = "cursor:pointer";
                    e.TimeSlot.Control.Attributes["title"] = "Double click to show activities/work hours";
                }
            }
            if (e.TimeSlot.Control.Controls.Count > 3)
            {
                for (int i = 4; i < e.TimeSlot.Control.Controls.Count; i++)
                {
                    ((System.Web.UI.WebControls.WebControl)e.TimeSlot.Control.Controls[i]).Attributes["style"] = "display:none";
                }
            }
        }
    }
    private string GetImage(string Code)
    {
        string img = string.Empty;
        switch (Code)
        {
            case "CAR":
                img = "cargo.png";
                break;
            case "NAV":
                img = "Navigation and Mooring.png";
                break;
            case "CAT":
                img = "Catering and Housekeeping.png";
                break;
            case "BUN":
                img = "BunkersLube.png";
                break;
            case "AUD":
                img = "Audits and Inspections.png";
                break;
            case "ENV":
                img = "EnvironmentalManagement.png";
                break;
            case "EMG":
                img = "Emergency Preparedness.png";
                break; 
            case "CRW":
                img = "crew.png";
                break;
            default:
                img = "PlannedMaintenance.png";
                break;
        }
        return img;
    }
}
