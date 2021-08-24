using SouthNests.Phoenix.Framework;

using SouthNests.Phoenix.PlannedMaintenance;

using System;

using System.Data;

using System.Web.UI;

using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceDailyWorkPlanMemberActualTimeSheet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);



            gvMemberTimeSheet.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            ViewState["TYPE"] = string.Empty;
            ViewState["MEMBERID"] = string.Empty;
            ViewState["ID"] = string.Empty;
            ViewState["TYPE"] = Request.QueryString["type"];
            ViewState["MEMBERID"] = Request.QueryString["Memberid"];
            ViewState["ID"] = Request.QueryString["id"];
            if (ViewState["TYPE"].ToString() == "1")
            {
                BindActivityDetails();
            }
            else
            {
                BindWODetails();
            }
            //BindTimings();
        }
    }
    public void BindTimings()
    {
        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.ActivityorWOTimings(General.GetNullableGuid(ViewState["ID"].ToString())
            , General.GetNullableInteger(ViewState["TYPE"].ToString()));

        radactlblstarttime.Text = dt.Rows[0]["FLDSTARTTIME"].ToString();
        radactlblendtime.Text = dt.Rows[0]["FLDENDTIME"].ToString();
    }
    public void BindActivityDetails()
    {
        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.ActivityMemberDetails(General.GetNullableGuid(ViewState["MEMBERID"].ToString()));
        Title = "Planned date : " + General.GetDateTimeToString(dt.Rows[0]["FLDACTIVITYDATE"].ToString()) + " " + dt.Rows[0]["FLDACTIVITYNAME"].ToString();
        radlblname.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
    }
    public void BindWODetails()
    {
        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.WOMemberDetails(General.GetNullableGuid(ViewState["MEMBERID"].ToString()));
        Title = "Planned date : " + dt.Rows[0]["FLDWODATE"].ToString() + " " + dt.Rows[0]["FLDWONAME"].ToString();
        radlblname.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
    }
    protected void gvMemberTimeSheet_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.MemberActualTimeSheetSearch(
                                                                   General.GetNullableGuid(ViewState["MEMBERID"].ToString())
                                                                   , General.GetNullableInteger(ViewState["TYPE"].ToString())
                                                                   , gvMemberTimeSheet.CurrentPageIndex + 1
                                                                   , gvMemberTimeSheet.PageSize
                                                                   , ref iRowCount
                                                                   , ref iTotalPageCount);

        gvMemberTimeSheet.DataSource = dt;
        gvMemberTimeSheet.VirtualItemCount = iRowCount;
    }

    protected void gvMemberTimeSheet_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvMemberTimeSheet_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {

            if ((e.CommandName.ToUpper().Equals("ADD")))
            {
                if (!IsValidTimeline(General.GetNullableDateTime(((RadDateTimePicker)e.Item.FindControl("txtstartDateTimeentry")).SelectedDate.Value.ToString())
                     , General.GetNullableDateTime(((RadDateTimePicker)e.Item.FindControl("txtendDateTimeentry")).SelectedDate.Value.ToString())))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceDailyWorkPlan.MemberActualTimeSheetInsert(
                     General.GetNullableGuid(ViewState["MEMBERID"].ToString())
                     , General.GetNullableGuid(ViewState["ID"].ToString())
                     , General.GetNullableInteger(ViewState["TYPE"].ToString())
                     , General.GetNullableDateTime(((RadDateTimePicker)e.Item.FindControl("txtstartDateTimeentry")).SelectedDate.Value.ToString())
                     , General.GetNullableDateTime(((RadDateTimePicker)e.Item.FindControl("txtendDateTimeentry")).SelectedDate.Value.ToString())
                    );
            }
            if ((e.CommandName.ToUpper().Equals("UPDATE")))
            {
                if (!IsValidTimeline(General.GetNullableDateTime(((RadDateTimePicker)e.Item.FindControl("txtstartDateTimeedit")).SelectedDate.Value.ToString())
                     , General.GetNullableDateTime(((RadDateTimePicker)e.Item.FindControl("txtendDateTimeedit")).SelectedDate.Value.ToString())))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceDailyWorkPlan.MemberActualTimeSheetUpdate(
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("radid2")).Text)

                 , General.GetNullableDateTime(((RadDateTimePicker)e.Item.FindControl("txtstartDateTimeedit")).SelectedDate.Value.ToString())
                     , General.GetNullableDateTime(((RadDateTimePicker)e.Item.FindControl("txtendDateTimeedit")).SelectedDate.Value.ToString())
                   );
            }
            if ((e.CommandName.ToUpper().Equals("DELETE")))
            {
                PhoenixPlannedMaintenanceDailyWorkPlan.MemberActualTimeSheetDelete(
                      General.GetNullableGuid(((RadLabel)e.Item.FindControl("radid")).Text)
                     );

            }
            gvMemberTimeSheet.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public bool IsValidTimeline(DateTime? starttime, DateTime? endtime)
    {
        //DateTime? AStartTime = General.GetNullableDateTime(radactlblstarttime.Text);
        //DateTime? AEndTime = General.GetNullableDateTime(radactlblendtime.Text);

        //if (starttime < AStartTime)
        //{
        //    ucError.ErrorMessage = "Start Time should not be less than Operation/Maintenance Start Time.";
        //    ucError.IsError = true;
        //}
        //if (endtime > AEndTime)
        //{
        //    ucError.ErrorMessage = "End Time should not be greater than Operation/Maintenance End Time.";
        //    ucError.IsError = true;
        //}

        if (starttime > endtime)
        {
            ucError.ErrorMessage = "Start Time should not be greater than End Time.";
            ucError.IsError = true;
        }



        return (!ucError.IsError);
    }
}