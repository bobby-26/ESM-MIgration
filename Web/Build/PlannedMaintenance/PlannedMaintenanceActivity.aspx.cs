using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceActivity : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();        
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkOrder.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["WOPI"] = Request.QueryString["wopi"] ?? string.Empty;
            ViewState["STATUS"] = Request.QueryString["status"] ?? string.Empty;
            ViewState["ACTPI"] = Request.QueryString["actpi"] ?? string.Empty;
            ViewState["DATE"] = string.Empty;
            if (ViewState["STATUS"].ToString().Equals("2"))
            {
                txtStartTime.Enabled = true;
                txtCompletedTime.Enabled = true;
            }
            if ( ViewState["STATUS"].ToString().Equals("3"))
            {
                txtStartTime.Enabled = false;
                txtCompletedTime.Enabled = false;
            }
            if (ViewState["STATUS"].ToString().Equals("4"))
            {
                txtStartTime.Enabled = false;
                
            }
            EditTimeSheet();
        }
        txtCompletedTime.TimePopupButton.Visible = false;
        txtStartTime.TimePopupButton.Visible = false;
    }
    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidSubmmision(txtStartTime.SelectedTime.ToString(), txtCompletedTime.SelectedTime.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                DateTime d = DateTime.Parse(ViewState["DATE"].ToString());
                DateTime start = d.Add(txtStartTime.SelectedTime.Value);
                DateTime end = d.Add(txtCompletedTime.SelectedTime.Value);
                if (General.GetNullableGuid(ViewState["WOPI"].ToString()).HasValue)
                {
                    PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWorkOrderStatus(new Guid(ViewState["WOPI"].ToString())
                       , PhoenixSecurityContext.CurrentSecurityContext.VesselID, 3, d, General.GetNullableInteger(ViewState["STATUS"].ToString()), start, end);
                    PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(General.GetNullableGuid(ViewState["WOPI"].ToString()), 2, int.Parse(ViewState["STATUS"].ToString()), start, end);

                    PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWRH(General.GetNullableGuid(ViewState["WOPI"].ToString()), 2, General.GetNullableInteger(ViewState["STATUS"].ToString()));
                }
                else
                {
                    PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityOperation(new Guid(ViewState["ACTPI"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                       , 3, d, General.GetNullableInteger(ViewState["STATUS"].ToString()), start, end);
                    PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(General.GetNullableGuid(ViewState["ACTPI"].ToString()), 1, int.Parse(ViewState["STATUS"].ToString()), start, end);

                    PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWRH(General.GetNullableGuid(ViewState["ACTPI"].ToString()), 1, General.GetNullableInteger(ViewState["STATUS"].ToString()));
                }

                string script = "refresh();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }                       
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditTimeSheet()
    {
        if (General.GetNullableGuid(ViewState["WOPI"].ToString()).HasValue)
        {
            DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.EditWorkOrder(General.GetNullableGuid(ViewState["WOPI"].ToString()).Value
                , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                DateTime? d = General.GetNullableDateTime(dr["FLDDATE"].ToString());                
                txtActivity.Text = dr["FLDWORKORDERNAME"].ToString();
                txtStartDate.SelectedDate = d;
                if (General.GetNullableDateTime(dr["FLDSTARTTIME"].ToString()) != null)
                {
                    txtStartTime.SelectedDate = General.GetNullableDateTime(dr["FLDSTARTTIME"].ToString());
                }
                else
                {
                    txtStartTime.SelectedDate = d.Value.AddHours(int.Parse(dr["FLDESTSTARTTIME"].ToString()));
                }
                txtCompletedDate.SelectedDate = d;
                if (General.GetNullableDateTime(dr["FLDCOMPLETEDTIME"].ToString()) != null)
                {
                    txtCompletedTime.SelectedDate = General.GetNullableDateTime(dr["FLDCOMPLETEDTIME"].ToString());
                }
                else
                {
                    txtCompletedTime.SelectedDate = d.Value.AddHours(int.Parse(dr["FLDDURATION"].ToString()));
                }
                ViewState["DATE"] = d;
                
                //PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(General.GetNullableGuid(ViewState["WOPI"].ToString()), 2, int.Parse(ViewState["STATUS"].ToString()), txtStartTime.SelectedDate, txtCompletedTime.SelectedDate);

            }
        }
        else
        {
            DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.EditActivity(new Guid(ViewState["ACTPI"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                
                //lblActivity.Text = "Activity";
                txtActivity.Text = dr["FLDACTIVITYNAME"].ToString();

                DateTime? d = General.GetNullableDateTime(dr["FLDDATE"].ToString());
               
                
                txtStartDate.SelectedDate = d;
                if (General.GetNullableDateTime(dr["FLDSTARTTIME"].ToString()) != null)
                {
                    txtStartTime.SelectedDate = General.GetNullableDateTime(dr["FLDSTARTTIME"].ToString());
                }
                else
                {
                    txtStartTime.SelectedDate = d.Value.AddHours(int.Parse(dr["FLDESTSTARTTIME"].ToString()));
                }

                txtCompletedDate.SelectedDate = d;
                if (General.GetNullableDateTime(dr["FLDCOMPLETEDTIME"].ToString()) != null)
                {
                    txtCompletedTime.SelectedDate = General.GetNullableDateTime(dr["FLDCOMPLETEDTIME"].ToString());
                }
                else
                {
                    txtCompletedTime.SelectedDate = d.Value.AddHours(int.Parse(dr["FLDDURATION"].ToString()));
                }
                ViewState["DATE"] = d;

                //if (ViewState["STATUS"].ToString() == "4")
                //{
                //    txtCompletedTime.SelectedTime = DateTime.Now.TimeOfDay;
                //}
                //PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(General.GetNullableGuid(ViewState["ACTPI"].ToString()), 1, int.Parse(ViewState["STATUS"].ToString()), txtStartTime.SelectedDate, txtCompletedTime.SelectedDate);

            }
        }        
    }
    private bool IsValidTimeSheet(string vesselstatus, DateTime? datetime, string details)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (General.GetNullableGuid(vesselstatus) == null)
            ucError.ErrorMessage = "Vessel Status is required.";

        if (datetime == null)
            ucError.ErrorMessage = "Time is required.";

        if (details.Trim().Equals(""))
            ucError.ErrorMessage = "Details is required.";

        return (!ucError.IsError);
    }
    private bool IsValidSubmmision(  string fromtime , string totime)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromtime))
            ucError.ErrorMessage = "From time is required.";

        if (string.IsNullOrEmpty(totime))
            ucError.ErrorMessage = "To time is required.";
        DateTime datetime = DateTime.Parse(ViewState["DATE"].ToString());
        if (datetime != null)
        {
            DateTime dt = DateTime.Parse(datetime.ToString());
            if (dt.Date > DateTime.Now.Date)
            {
                ucError.ErrorMessage = "Activity Date cannot be greater than today";
            }
        }

        return (!ucError.IsError);
    }
    private bool IsValidTimeSheetActivity(string vesselstatus, DateTime? datetime, string details, string activity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(vesselstatus) == null)
            ucError.ErrorMessage = "Vessel Status is required.";

        if (datetime == null)
            ucError.ErrorMessage = "Time is required.";

        if (General.GetNullableInteger(activity) == null)
            ucError.ErrorMessage = "Operation is required.";

        if (details.Trim().Equals(""))
            ucError.ErrorMessage = "Details is required.";
        if (datetime != null)
        {
            DateTime dt = DateTime.Parse(datetime.ToString());
            if (dt.Date > DateTime.Now.Date)
            {
                ucError.ErrorMessage = "Time cannot be greater than today";
            } 
        }
        return (!ucError.IsError);
    }
    private bool IsValidTimeSheetWorkOrder(string vesselstatus, DateTime? datetime, string details, string workorder)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(vesselstatus) == null)
            ucError.ErrorMessage = "Vessel Status is required.";

        if (datetime == null)
            ucError.ErrorMessage = "Time is required.";

        if (General.GetNullableGuid(workorder) == null)
            ucError.ErrorMessage = "Maintenance is required.";

        if (details.Trim().Equals(""))
            ucError.ErrorMessage = "Details is required.";

        if (datetime != null)
        {
            DateTime dt = DateTime.Parse(datetime.ToString());
            if (dt.Date > DateTime.Now.Date)
            {
                ucError.ErrorMessage = "Time cannot be greater than today";
            }
        }

        return (!ucError.IsError);
    }

    

    
    
    

    public void ActivityOrWOID(ref Guid? ID, ref int type)
    {

        Guid? ActivityID = General.GetNullableGuid(ViewState["ACTPI"].ToString());
        Guid? WOID = General.GetNullableGuid(ViewState["WOPI"].ToString());


        if (ActivityID == null)
        {
            ID = WOID;
            type = 2;
        }
        else
        {
            ID = ActivityID;
            type = 1;
        }
    }

    protected void btnchangetimings_Click(object sender, EventArgs e)
    {
        int type = 0;
        Guid? ID = null;
        ActivityOrWOID(ref ID, ref type);

        PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(ID, type, int.Parse(ViewState["STATUS"].ToString()), txtStartTime.SelectedDate, txtCompletedTime.SelectedDate);

    }
}