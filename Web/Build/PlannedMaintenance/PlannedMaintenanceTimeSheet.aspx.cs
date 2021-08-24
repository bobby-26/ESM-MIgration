using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceTimeSheet : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();        
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkOrder.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["WO"] = string.Empty;
            ViewState["WOPI"] = string.Empty;
            ViewState["ACT"] = string.Empty;
            ViewState["ACTPI"] = string.Empty;
            ViewState["ACTION"] = string.Empty;
            ViewState["EVENT"] = string.Empty;
            ViewState["ENABLEACTIVITY"] = string.Empty;
            ViewState["ISPLANNED"] = "0";
            if (Request.QueryString["wo"] != null)
            {
                ViewState["WO"] = Request.QueryString["wo"];
            }
            if (Request.QueryString["wopi"] != null)
            {
                ViewState["WOPI"] = Request.QueryString["wopi"];
            }
            if (Request.QueryString["act"] != null)
            {
                ViewState["ACT"] = Request.QueryString["act"];
            }
            if (Request.QueryString["actpi"] != null)
            {
                ViewState["ACTPI"] = Request.QueryString["actpi"];
            }
            if (Request.QueryString["a"] != null)
            {
                ViewState["ACTION"] = Request.QueryString["a"];
            }
            if (Request.QueryString["ev"] != null)
            {
                ViewState["EVENT"] = Request.QueryString["ev"];
            }
            if (Request.QueryString["ea"] != null)
            {
                ViewState["ENABLEACTIVITY"] = Request.QueryString["ea"];
            }
            if (ViewState["ACTION"].ToString() == "3")
            {
                gvtable.Visible = true;
            }
            txtDateTime.SelectedDate = DateTime.Now.Date;
            txttimepicker.SelectedDate = DateTime.Now;
            txttimepicker.TimePopupButton.Visible = false;

            txttimepicker.TimeView.TimeFormat = "HH:mm";
            PopulateRegisters();
            EditTimeSheet();
            gvMembers.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            modelPopup.NavigateUrl = Session["sitepath"] + "/Dashboard/DashboardTechnicalJobCategoryPlanned.aspx?td=" + General.GetDateTimeToString(DateTime.Now) + "&sm=single";
        }
    }
    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string activity = ViewState["ACT"].ToString();
                string maintenance = ViewState["WO"].ToString();
                if (ViewState["EVENT"].ToString().ToUpper().Equals("AOP"))
                {
                    activity = ddlOperation.SelectedValue;
                    if (!IsValidTimeSheetActivity(ddlVesselStatus.SelectedValue, txtDateTime.SelectedDate, txtDetail.Text, activity))
                    {
                        ucError.Visible = true;
                        return;
                    }
                }
                else if (ViewState["EVENT"].ToString().ToUpper().Equals("AMAINT"))
                {
                    maintenance = ddlOperation.SelectedValue;
                    if (!IsValidTimeSheetWorkOrder(ddlVesselStatus.SelectedValue, txtDateTime.SelectedDate, txtDetail.Text, maintenance))
                    {
                        ucError.Visible = true;
                        return;
                    }
                }
                else if (!IsValidTimeSheet(ddlVesselStatus.SelectedValue, txtDateTime.SelectedDate, txtDetail.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string sb = "";
                IList<RadListBoxItem> collection = chkEventList.CheckedItems;
                foreach (RadListBoxItem item in collection)
                {
                    sb += item.Value + ",";
                }

                sb = sb.Trim(',');

                DateTime Time = txtDateTime.SelectedDate.Value.Add(txttimepicker.SelectedTime.Value);


                PhoenixPlannedMaintenanceTimeSheet.Insert(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , new Guid(ddlVesselStatus.SelectedValue), Time
                    , General.GetNullableInteger(activity)
                    , General.GetNullableGuid(maintenance)
                    , txtDetail.Text
                    , General.GetNullableGuid(ViewState["ACTPI"].ToString())
                    , General.GetNullableGuid(ViewState["WOPI"].ToString())
                    , sb
                    );

                if (General.GetNullableInteger(ViewState["ACTION"].ToString()) != null && General.GetNullableGuid(ViewState["WOPI"].ToString()) != null)
                    PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWorkOrderStatus(new Guid(ViewState["WOPI"].ToString())
                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(ViewState["ACTION"].ToString()), Time, null, null, null);
                if (General.GetNullableInteger(ViewState["ACTION"].ToString()) != null && General.GetNullableGuid(ViewState["ACTPI"].ToString()) != null)
                    PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityOperation(new Guid(ViewState["ACTPI"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , int.Parse(ViewState["ACTION"].ToString()), Time, null, null, null);
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
        if (General.GetNullableGuid(ViewState["WOPI"].ToString()) != null)
        {
            DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.EditWorkOrder(General.GetNullableGuid(ViewState["WOPI"].ToString()).Value
                , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if(dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtOperation.Text = dr["FLDWORKORDERNAME"].ToString();
                ViewState["ISPLANNED"] = dr["FLDISPLANNED"].ToString();
                //trOperation.Visible = false;
                //trMaintenance.Visible = true;
                ddlOperation.Visible = false;
                //lnkMaintenance.Visible = false;
                
            }
            chkEventList.Visible = false;
            lblEvent.Visible = false;
        }
        else if (General.GetNullableGuid(ViewState["ACTPI"].ToString()) != null)
        {
            DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.EditActivity(General.GetNullableGuid(ViewState["ACTPI"].ToString()).Value
                , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                trOperation.Visible = true;
                ViewState["ISPLANNED"] = dr["FLDISPLANNED"].ToString();
                if (ViewState["ENABLEACTIVITY"].ToString() != string.Empty)
                {
                    txtDateTime.SelectedDate = General.GetNullableDateTime(dr["FLDDATE"].ToString()).Value;
                    ddlOperation.SelectedValue = dr["FLDACTIVITYID"].ToString();
                    txtOperation.Visible = false;
                    txtDateTime_SelectedDateChanged(null, null);
                }
                else
                {
                    txtDateTime.SelectedDate = General.GetNullableDateTime(dr["FLDDATE"].ToString()).Value;
                    txtDateTime.Enabled = false;
                    txtOperation.Text = dr["FLDACTIVITYNAME"].ToString();                    
                    //trMaintenance.Visible = false;
                    ddlOperation.Visible = false;
                }
                
                PopulateEventList(General.GetNullableInteger(dr["FLDACTIVITYID"].ToString()));
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
    protected void PopulateRegisters()
    {
        ddlVesselStatus.DataSource = PhoenixRegistersDMROperationalTask.DMROperationalTaskList();
        ddlVesselStatus.DataTextField = "FLDTASKNAME";
        ddlVesselStatus.DataValueField = "FLDOPERATIONALTASKID";
        ddlVesselStatus.DataBind();
        ddlVesselStatus.Items.Insert(0, new RadComboBoxItem("--Select--", string.Empty));
        Guid? LastVesselStatus = null;
        PhoenixPlannedMaintenanceTimeSheet.FetchLastVesselStatus(PhoenixSecurityContext.CurrentSecurityContext.VesselID, ref LastVesselStatus);
        if (LastVesselStatus.HasValue)
            ddlVesselStatus.SelectedValue = LastVesselStatus.ToString();
        txtDateTime_SelectedDateChanged(null, null);
    }
    protected void txtDateTime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        if (ViewState["EVENT"].ToString().ToUpper().Equals("AOP"))
        {           
            DataTable dt = PhoenixPlannedMaintenanceTimeSheet.ListOperation(General.GetNullableDateTime(txtDateTime.SelectedDate.Value.ToString("yyyy/MM/dd")).Value, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            ddlOperation.DataSource = dt;
            ddlOperation.DataTextField = "FLDNAME";
            ddlOperation.DataValueField = "FLDACTIVITYID";
            ddlOperation.DataBind();
            ddlOperation.Items.Insert(0, new RadComboBoxItem("--Select--", string.Empty));
            //trOperation.Visible = true;
            //trMaintenance.Visible = false;
            txtOperation.Visible = false;            
        }
        else if (ViewState["EVENT"].ToString().ToUpper().Equals("AMAINT"))
        {
            DataTable dt = PhoenixPlannedMaintenanceTimeSheet.ListMaintenance(General.GetNullableDateTime(txtDateTime.SelectedDate.Value.ToString("yyyy/MM/dd")).Value, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            ddlOperation.DataSource = dt;
            ddlOperation.DataTextField = "FLDWORKORDERNAME";
            ddlOperation.DataValueField = "FLDWORKORDERGROUPID";
            ddlOperation.DataBind();
            ddlOperation.Items.Insert(0, new RadComboBoxItem("--Select--", string.Empty));
            txtOperation.Visible = false;
            //trOperation.Visible = false;
            //trMaintenance.Visible = true;
            chkEventList.Visible = false;
            lblEvent.Visible = false;
            lblOperation.Text = "Maintenance";
        }
        else if (ViewState["EVENT"].ToString().ToUpper().Equals("AOTHR"))
        {
            trOperation.Visible = false;
            //trMaintenance.Visible = false;
            chkEventList.Visible = false;
            lblEvent.Visible = false;
            //txtDateTime.AutoPostBackControl = Telerik.Web.UI.Calendar.AutoPostBackControl.None;
        }
        int type = 0;
        Guid? ID1 = null;
        ActivityOrWOID(ref ID1, ref type);
        if (rblStatus.SelectedValue == "4" || rblStatus.SelectedValue == "5" || rblStatus.SelectedValue == "1")
        {
           // PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(ID1, type, int.Parse(rblStatus.SelectedValue), txtDateTime.SelectedDate, txtStartTime.SelectedDate, txtEndTime.SelectedDate);
        }

        if (txtStartTime.SelectedDate != null && txtEndTime.SelectedDate != null && rblStatus.SelectedValue == "2")
        {
           // PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(ID1, type, int.Parse(rblStatus.SelectedValue), txtDateTime.SelectedDate, txtStartTime.SelectedDate, txtEndTime.SelectedDate);

        }

    }

    protected void ddlOperation_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ViewState["EVENT"].ToString().ToUpper().Equals("AOP"))
        {
            PopulateEventList(General.GetNullableInteger(e.Value));
        }
    }
    private void PopulateEventList(int? ActivityId)
    {
        DataTable dt = PhoenixPlannedMaintenanceTimeSheet.ListEvent(ActivityId);
        chkEventList.DataSource = dt;
        chkEventList.DataBind();
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

    protected void gvMembers_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                LinkButton Timesheet = (LinkButton)item.FindControl("cmdTimesheet");
                RadLabel Memberid = (RadLabel)item.FindControl("lblmemberid");
                RadLabel activityid = (RadLabel)item.FindControl("lblactivityid");
                RadLabel catid = (RadLabel)item.FindControl("lblcatid");
                int type = 1;
                if (ViewState["WOPI"].ToString() != string.Empty)
                    type = 2;
                if (Timesheet != null && catid.Text == "4")
                {
                    Timesheet.Visible = true;
                    Timesheet.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','PlannedMaintenance/PlannedMaintenanceDailyWorkPlanMemberActualTimeSheet.aspx?Memberid=" + Memberid.Text + "&type=" + type + "&id=" + activityid.Text + "','false','700px','350px');return false");
                }
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMembers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int IRowCount = 0;
        int ITotalPageCount = 0;

        int type = 0;
        Guid? ID = null;
        ActivityOrWOID(ref ID, ref type);


        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.ActivityMembersSearch(
              ID
            , gvMembers.CurrentPageIndex + 1
            , gvMembers.PageSize
            , ref IRowCount
            , ref ITotalPageCount);
        gvMembers.DataSource = dt;
        gvMembers.VirtualItemCount = IRowCount;
    }

    //protected void rblStatus_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    RadRadioButtonList list = (RadRadioButtonList)sender;
    //    trStart.Visible = false;
    //    trEnd.Visible = false;
    //    if (list.SelectedValue == "2")
    //    {
    //        trStart.Visible = true;
    //        trEnd.Visible = true;
    //    }
    //    int type = 0;
    //    Guid? ID = null;
    //    ActivityOrWOID(ref ID, ref type);

    //    if (list.SelectedValue == "4" || list.SelectedValue == "5" || list.SelectedValue == "1")
    //    {
    //        // PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(ID, type,int.Parse(list.SelectedValue), txtDateTime.SelectedDate,txtStartTime.SelectedDate,txtEndTime.SelectedDate);
    //    }

    //    if (txtStartTime.SelectedDate != null && txtEndTime.SelectedDate != null && list.SelectedValue == "2")
    //    {
    //        //  PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(ID, type, int.Parse(list.SelectedValue), txtDateTime.SelectedDate, txtStartTime.SelectedDate, txtEndTime.SelectedDate);

    //    }
    //}

    //protected void txtStartTime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    //{
    //    int type = 0;
    //    Guid? ID = null;
    //    ActivityOrWOID(ref ID, ref type);

    //    if (txtStartTime.SelectedDate != null && txtEndTime.SelectedDate != null && rblStatus.SelectedValue == "2")
    //    {
    //       // PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(ID, type, int.Parse(rblStatus.SelectedValue), txtDateTime.SelectedDate, txtStartTime.SelectedDate, txtEndTime.SelectedDate);

    //    }
    //}

    //protected void txtEndTime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    //{
    //    int type = 0;
    //    Guid? ID = null;
    //    ActivityOrWOID(ref ID, ref type);

    //    if (txtStartTime.SelectedDate != null && txtEndTime.SelectedDate != null && rblStatus.SelectedValue == "2")
    //    {
    //       // PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(ID, type, int.Parse(rblStatus.SelectedValue), txtDateTime.SelectedDate, txtStartTime.SelectedDate, txtEndTime.SelectedDate);

    //    }
    //}
}