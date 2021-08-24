using SouthNests.Phoenix.Framework;

using SouthNests.Phoenix.PlannedMaintenance;

using System;

using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class PlannedMaintenance_PlannedMaintenanceDailyWorkPlanMemberTimeSheet : PhoenixBasePage
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
            ViewState["READONLY"] = Request.QueryString["readonly"];
            if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() == "1")
            {
                BindActivityDetails();
            }
            else
            {
                BindWODetails();
            }

            BindTimings();

            if (ViewState["READONLY"] != null && ViewState["READONLY"].ToString() == "1")
            {
                gvMemberTimeSheet.ShowFooter = false;
                gvMemberTimeSheet.Columns[3].Visible = false;
            }
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
        Title = General.GetDateTimeToString(dt.Rows[0]["FLDACTIVITYDATE"].ToString()) + " " + dt.Rows[0]["FLDACTIVITYNAME"].ToString();
        radlblname.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
    }
    public void BindWODetails()
    {
        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.WOMemberDetails(General.GetNullableGuid(ViewState["MEMBERID"].ToString()));
        Title = dt.Rows[0]["FLDWODATE"].ToString() + " " + dt.Rows[0]["FLDWONAME"].ToString();
        radlblname.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
    }

    protected void gvMemberTimeSheet_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataTable dt = new DataTable();
        if (ViewState["READONLY"] != null && ViewState["READONLY"].ToString() == "1")
        {
            dt = PhoenixPlannedMaintenanceDailyWorkPlan.MemberTimeSheetSearchActuals(
                                                                 General.GetNullableGuid(ViewState["MEMBERID"].ToString())
                                                                 , General.GetNullableInteger(ViewState["TYPE"].ToString())
                                                                 , gvMemberTimeSheet.CurrentPageIndex + 1
                                                                 , gvMemberTimeSheet.PageSize
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);
        }
        else
        {
            dt = PhoenixPlannedMaintenanceDailyWorkPlan.MemberTimeSheetSearch(
                                                                 General.GetNullableGuid(ViewState["MEMBERID"].ToString())
                                                                 , General.GetNullableInteger(ViewState["TYPE"].ToString())
                                                                 , gvMemberTimeSheet.CurrentPageIndex + 1
                                                                 , gvMemberTimeSheet.PageSize
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);
        }


        gvMemberTimeSheet.DataSource = dt;
        gvMemberTimeSheet.VirtualItemCount = iRowCount;

    }

    protected void gvMemberTimeSheet_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if ((e.CommandName.ToUpper().Equals("ADD")))
            {
                if (!IsValidTimeline(General.GetNullableInteger(((RadComboBox)e.Item.FindControl("Radfromentry")).SelectedIndex.ToString()), General.GetNullableInteger(((RadComboBox)e.Item.FindControl("radtotimeentry")).SelectedIndex.ToString())))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceDailyWorkPlan.MemberTimeSheetInsert(
                     General.GetNullableGuid(ViewState["MEMBERID"].ToString())
                     , General.GetNullableGuid(ViewState["ID"].ToString())
                     , General.GetNullableInteger(ViewState["TYPE"].ToString())
                     , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("Radfromentry")).SelectedIndex.ToString())
                     , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("radtotimeentry")).SelectedIndex.ToString())
                    );
            }
            if ((e.CommandName.ToUpper().Equals("UPDATE")))
            {
                if (!IsValidTimeline(General.GetNullableInteger(((RadComboBox)e.Item.FindControl("Radfromedit")).SelectedIndex.ToString()), General.GetNullableInteger(((RadComboBox)e.Item.FindControl("Radtoedit")).SelectedIndex.ToString())))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceDailyWorkPlan.MemberTimeSheetUpdate(
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("radid2")).Text)
                    , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("Radfromedit")).SelectedIndex.ToString())
                    , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("Radtoedit")).SelectedIndex.ToString())
                   );
            }
            if ((e.CommandName.ToUpper().Equals("DELETE")))
            {
                PhoenixPlannedMaintenanceDailyWorkPlan.MemberTimeSheetDelete(
                      General.GetNullableGuid(((RadLabel)e.Item.FindControl("radid")).Text)
                     );

            }
            gvMemberTimeSheet.Rebind();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                           "BookMarkScript", "fnReloadList('Filters', 'RadWindowWrapper_RadWindow_NavigateUrl', '');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvMemberTimeSheet_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                GridDataItem Item = (GridDataItem)e.Item;
                LinkButton Flag = (LinkButton)Item.FindControl("imgFlag");
                RadLabel flagstatus = (RadLabel)Item.FindControl("radisedited");
                RadLabel from = (RadLabel)Item.FindControl("Radlblfrom");
                RadLabel fromactual = (RadLabel)Item.FindControl("Radlblfromactual");
                RadLabel to = (RadLabel)Item.FindControl("Radlblto");
                RadLabel toactual = (RadLabel)Item.FindControl("radtoactual");
                if (ViewState["READONLY"] != null && ViewState["READONLY"].ToString() == "1")
                {
                    if (fromactual != null)
                        fromactual.Visible = true;
                    if(toactual != null)
                        toactual.Visible = true; }
                else {
                    if (from != null)
                        from.Visible = true;
                    if (to != null)
                        to.Visible = true; }

                if (flagstatus.Text == "1")
                {
                    Flag.Visible = true;
                }
            }
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                GridEditableItem Item = (GridEditableItem)e.Item;
                RadComboBox Starttime = (RadComboBox)Item.FindControl("Radfromedit");
                RadComboBox Endtime = (RadComboBox)Item.FindControl("Radtoedit");
                if (Starttime != null)
                {
                    Starttime.DataSource = BindDuration();
                    Starttime.DataBind();
                    Starttime.SelectedIndex = int.Parse(drv["FLDSTARTTIME"].ToString());

                }
                if (Endtime != null)
                {
                    Endtime.DataSource = BindDuration();
                    Endtime.DataBind();
                    Endtime.SelectedIndex = int.Parse(drv["FLDENDTIME"].ToString());

                }
            }

            if (e.Item is GridFooterItem)
            {
                GridFooterItem Item = (GridFooterItem)e.Item;
                RadComboBox Starttime = (RadComboBox)Item.FindControl("Radfromentry");
                RadComboBox Endtime = (RadComboBox)Item.FindControl("radtotimeentry");
                Starttime.DataSource = BindDuration();
                Starttime.DataBind();
                Endtime.DataSource = BindDuration();
                Endtime.DataBind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected string[] BindDuration()
    {
        return new string[] { "0000", "0100", "0200", "0300", "0400", "0500", "0600", "0700", "0800", "0900", "1000", "1100", "1200", "1300", "1400", "1500", "1600", "1700", "1800", "1900", "2000", "2100", "2200", "2300", "2400" };
    }

    protected string PadZero(string padstring)
    {
        if (padstring.Length == 1)
        {
            padstring = padstring.PadLeft(2, '0');
        }
        if (padstring.Length == 2)
        {
            padstring = padstring.PadRight(4, '0');
        }
        return padstring;
    }

    public bool IsValidTimeline(int? starttime, int? endtime)
    {
        int? AStartTime = General.GetNullableInteger(radactlblstarttime.Text);
        int? AEndTime = General.GetNullableInteger(radactlblendtime.Text);

        if (starttime < AStartTime)
        {
            ucError.ErrorMessage = "Start Time should not be less than Operation/Maintenance Start Time.";
            ucError.IsError = true;
        }
        if (endtime > AEndTime)
        {
            ucError.ErrorMessage = "End Time should not be greater than Operation/Maintenance End Time.";
            ucError.IsError = true;
        }

        if (starttime > endtime)
        {
            ucError.ErrorMessage = "Start Time should not be greater than End Time.";
            ucError.IsError = true;
        }



        return (!ucError.IsError);
    }
}