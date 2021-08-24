using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Web.UI;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceDailyWorkPlanActivityReschedule : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);        
        if (!IsPostBack)
        {
            ViewState["ISCOPY"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["iscopy"]))
            {
                lblPospone.Text = "Copy To Date";
                ViewState["ISCOPY"] = Request.QueryString["iscopy"];
            }
            ViewState["ISCOPYACTIVITY"]= string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["cdate"]))
            {
                lblPospone.Text = "Copy To Date";
                txtPostponeDate.SelectedDate = General.GetNullableDateTime(Request.QueryString["cdate"]);
                ViewState["ISCOPYACTIVITY"] = Request.QueryString["cdate"];
                ViewState["ISCOPY"] = Request.QueryString["iscopy"];
            }
            ViewState["FDATE"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["fdate"]))
            {               
                ViewState["FDATE"] = Request.QueryString["fdate"];
            }
            ViewState["ID"] = string.Empty;            
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                ViewState["ID"] = Request.QueryString["id"];
            }            
            //txtPostponeDate.MinDate = DateTime.Now;
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (ViewState["ISCOPY"].ToString() != string.Empty)
        {
            string cmdname = "COPY";
            if(ViewState["ISCOPYACTIVITY"].ToString() != string.Empty)
                cmdname = "COPYACTIVITY";
            toolbar.AddButton("Copy", cmdname, ToolBarDirection.Right);
            txtPostponeDate.MinDate = DateTime.Now;
        }        
        else
        {
            toolbar.AddButton("Reschedule", "RESCHEDULE", ToolBarDirection.Right);
        }
        MenuMain.AccessRights = this.ViewState;
        MenuMain.MenuList = toolbar.Show();
    }
    protected void MainMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("RESCHEDULE"))
            {
                if (!IsValidReschedule())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceDailyWorkPlan.Postpone(new Guid(ViewState["ID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtPostponeDate.SelectedDate.Value);
                string script = "function sd(){CloseModelWindow('" + Request.QueryString["gid"] + "'); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            if (CommandName.ToUpper().Equals("COPY"))
            {
                if (!IsValidReschedule())
                {
                    ucError.Visible = true;
                    return;
                }
                Guid DailyWorkPlanid = Guid.Empty;

                PhoenixPlannedMaintenanceDailyWorkPlan.Copy(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(ViewState["FDATE"].ToString()), txtPostponeDate.SelectedDate.Value, ref DailyWorkPlanid);
                PhoenixPlannedMaintenanceDailyWorkPlan.AsyncResetWorkHours(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DailyWorkPlanid.ToString());
                string script = "function sd(){refresh();CloseModelWindow('" + Request.QueryString["gid"] + "'); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            if (CommandName.ToUpper().Equals("COPYACTIVITY"))
            {
                if (!IsValidReschedule())
                {
                    ucError.Visible = true;
                    return;
                }               
                PhoenixPlannedMaintenanceDailyWorkPlan.CopyActivity(new Guid(ViewState["ID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtPostponeDate.SelectedDate.Value);
                string script = "function sd(){refresh();CloseModelWindow('" + Request.QueryString["gid"] + "'); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidReschedule()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtPostponeDate.SelectedDate == null)
        {
            if (ViewState["ISCOPY"].ToString() != string.Empty)
            {
                ucError.ErrorMessage = "Copy To Date is required.";
            }
            else
            {
                ucError.ErrorMessage = "Reschedule Date is required.";
            }
        }
        else if (DateTime.Compare(txtPostponeDate.SelectedDate.Value, DateTime.Now.Date) < 0)
        {
            if (ViewState["ISCOPY"].ToString() != string.Empty)
            {
                ucError.ErrorMessage = "Copy To Date should be later than current date.";
            }
            else
            {
                ucError.ErrorMessage = "Reschedule Date should be later than current date.";
            }
        }
        return (!ucError.IsError);
    }    
}