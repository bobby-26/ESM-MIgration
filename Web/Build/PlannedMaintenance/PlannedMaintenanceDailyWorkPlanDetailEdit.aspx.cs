using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.VesselAccounts;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceDailyWorkPlanDetailEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
       
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["ID"] = string.Empty;
            ViewState["WOID"] = string.Empty;
            ViewState["DATE"] = string.Empty;
            ViewState["PLANID"] = "";
            ViewState["MSIGNOFFID"] = "";
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                ViewState["ID"] = Request.QueryString["id"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["d"]))
            {
                ViewState["DATE"] = Request.QueryString["d"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["woid"]))
            {
                ViewState["WOID"] = Request.QueryString["woid"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["planid"]))
            {
                ViewState["PLANID"] = Request.QueryString["planid"];
            }
            ViewState["TEAMMEMBERS"] = string.Empty;
            gvMembers.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            PopuldateElement();

            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (General.GetNullableDateTime(ViewState["DATE"].ToString()).Value < DateTime.Now.Date)
            {

            }
            else
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            MenuMain.AccessRights = this.ViewState;
            MenuMain.MenuList = toolbar.Show();

            Edit();
        }

    }
    protected void MainMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            int type = 0;
            Guid? ID = null;
            ActivityOrWOID(ref ID, ref type);

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                string eststarttime = ddlEstStartTime.SelectedValue;
                string duration = ddlDuration.SelectedValue;


                string csvOtherMembers = string.Empty;
                string csvOtherMembersName = string.Empty;
                eststarttime = eststarttime.Substring(0, eststarttime.Length - 2);
                duration = duration.Substring(0, duration.Length - 2);
                int starttime = int.Parse(eststarttime);
                int endtime = int.Parse(duration);
                if (!IsValidDailyWorkPlan(eststarttime, duration))
                {
                    ucError.Visible = true;
                    return;
                }
                csvOtherMembers = GetSelectedMembers();
                ViewState["TEAMMEMBERS"] = csvOtherMembers;
                csvOtherMembersName = GetSelectedMemberNames();
                if (csvOtherMembers.Length > 1)
                {


                    PhoenixPlannedMaintenanceDailyWorkPlan.ActivityMembersInsert(
                        ID
                        , csvOtherMembers
                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , type
                        , starttime
                        , endtime);

                }
                else
                {
                    PhoenixPlannedMaintenanceDailyWorkPlan.ActivityMembersInsert(
                       ID
                       , ""
                       , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                       , type
                        , starttime
                        , endtime);


                }
                if (ViewState["ID"].ToString() != string.Empty)
                {
                    string id = ViewState["ID"].ToString();
                    PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivity(new Guid(id)
                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(eststarttime), byte.Parse(duration)
                    , General.GetNullableInteger(""), "", csvOtherMembers, csvOtherMembersName);

                    gvCrewWorkHrs.Rebind();
                    gvMembers.Rebind();
                }
                if (ViewState["WOID"].ToString() != string.Empty)
                {
                    string id = ViewState["WOID"].ToString();
                    PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWorkOrder(new Guid(id)
                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID, byte.Parse(eststarttime), byte.Parse(duration)
                                , General.GetNullableInteger(""), "", csvOtherMembers, csvOtherMembersName);

                    gvCrewWorkHrs.Rebind();
                    gvMembers.Rebind();
                }
                //string script = "function sd(){CloseModelWindow('" + Request.QueryString["gid"] + "'); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (CommandName.ToUpper().Equals("CLOSE"))
            {
                string script = "function sd(){CloseModelWindow('" + Request.QueryString["gid"] + "'); Sys.Application.remove_load(sd);} Sys.Application.add_load(sd);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidDailyWorkPlan(string starttime, string endtime)
    {
        if (starttime.Length == 4)
        {
            starttime = starttime.Substring(0, 2);
        }

        if (endtime.Length == 4)
        {
            endtime = endtime.Substring(0, 2);
        }
        ucError.HeaderMessage = "Please provide the following required information";
        int? StartTime = General.GetNullableInteger(starttime);
        int? EndTime = General.GetNullableInteger(endtime);
        if (StartTime.HasValue && EndTime.HasValue
            && EndTime.Value < StartTime.Value)
            ucError.ErrorMessage = "End Time should be later then Start Time";
        return (!ucError.IsError);
    }
    protected string[] BindDuration()
    {
        return new string[] { "0000", "0100", "0200", "0300", "0400", "0500", "0600", "0700", "0800", "0900", "1000", "1100", "1200", "1300", "1400", "1500", "1600", "1700", "1800", "1900", "2000", "2100", "2200", "2300", "2400" };
    }
    private DataTable GetCrewList()
    {
        return PhoenixVesselAccountsEmployee.ListVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableDateTime(ViewState["DATE"].ToString()));
    }
    private void PopuldateElement()
    {

        ddlDuration.DataSource = BindDuration();
        ddlDuration.DataBind();

        ddlEstStartTime.DataSource = BindDuration();
        ddlEstStartTime.DataBind();


    }
    protected void Edit()
    {
        if (ViewState["ID"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.EditActivity(new Guid(ViewState["ID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                lblElement.Text = "Process";
                lblActivity.Text = "Activity";
                txtActivity.Text = dr["FLDACTIVITYNAME"].ToString();
                txtElement.Text = dr["FLDELEMENTNAME"].ToString();
                ddlEstStartTime.SelectedValue = PadZero(dr["FLDESTSTARTTIME"].ToString());
                ddlDuration.SelectedValue = PadZero(dr["FLDDURATION"].ToString());
                ViewState["TEAMMEMBERS"] = dr["FLDOTHERMEMBERS"].ToString();


            }
        }
        if (ViewState["WOID"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.EditWorkOrder(new Guid(ViewState["WOID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                lblElement.Text = "Number";
                lblActivity.Text = "Name";
                txtElement.Text = dr["FLDWORKORDERNUMBER"].ToString();
                txtElement.ToolTip = dr["FLDWORKORDERNUMBER"].ToString();
                txtActivity.Text = dr["FLDWORKORDERNAME"].ToString();
                txtActivity.ToolTip = dr["FLDWORKORDERNAME"].ToString();
                ddlEstStartTime.SelectedValue = PadZero(dr["FLDESTSTARTTIME"].ToString());
                ddlDuration.SelectedValue = PadZero(dr["FLDDURATION"].ToString());

                ViewState["TEAMMEMBERS"] = dr["FLDOTHERMEMBERS"].ToString();


            }
        }
    }
    protected void gvCrewWorkHrs_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.Item is GridDataItem)
        {
            DataSet ds = (DataSet)grid.DataSource;
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            DataTable dtWorkDetails = ds.Tables[1];
            DataTable dtnc = ds.Tables[2];
            foreach (GridColumn c in grid.Columns)
            {
                if (c.UniqueName == "ClientSelectColumn" || c.UniqueName == "TemplateColumn")
                {
                    continue;
                }
                if (c.UniqueName != "FLDEMPLOYEENAME" && c.UniqueName != "FLDRANKNAME")
                    item[c.UniqueName].Text = "";

                if (c.UniqueName != "FLDEMPLOYEENAME" && c.UniqueName != "FLDRANKNAME" && drv[c.UniqueName].ToString() == "1")
                {
                    item[c.UniqueName].BorderWidth = Unit.Parse("1px");
                    item[c.UniqueName].BorderColor = ColorTranslator.FromHtml("#e7e7e7");

                    DataRow[] dr = dtWorkDetails.Select("FLDEMPLOYEEID = '" + drv["FLDEMPLOYEEID"].ToString() + "' AND FLDHOUR = '" + c.UniqueName + "'");

                    if (dr.Length > 0 && General.GetNullableString(dr[0]["FLDNONCOMPLIANCE"].ToString()) != null)
                    {
                        string nc = dr[0]["FLDNONCOMPLIANCE"].ToString().Trim().TrimStart(',');
                        string nctext = "";
                        string[] ncarray = nc.Split(',');
                        foreach (string t in ncarray)
                        {
                            DataRow[] drnc = dtnc.Select("FLDSHORTNAME= '" + t + "'");
                            if (drnc.Length > 0)
                                nctext = nctext + drnc[0]["FLDSHORTNAME"].ToString() + " - " + drnc[0]["FLDQUICKNAME"].ToString() + " <br/>";
                        }

                        item[c.UniqueName].BackColor = System.Drawing.Color.Red;
                        item[c.UniqueName].ToolTip = nctext;
                        RadToolTipManager1.TargetControls.Add(item[c.UniqueName].ClientID, nctext, true);
                    }

                    else
                        item[c.UniqueName].BackColor = System.Drawing.Color.Gray;

                }
            }



        }
    }
    protected void gvCrewWorkHrs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            RadGrid grid = (RadGrid)sender;
            DataSet ds = PhoenixPlannedMaintenanceDailyWorkPlan.CrewWorkHoursList(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(ViewState["PLANID"].ToString()));
            grid.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private string PadZero(string padstring)
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

    protected void gvMembers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int IRowCount = 0;
        int ITotalPageCount = 0;

        int type = 0;
        Guid? ID = null;
        ActivityOrWOID(ref ID, ref type);



        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.ActivityMembersSearch(
              ID,
          gvMembers.CurrentPageIndex + 1
            , gvMembers.PageSize
            , ref IRowCount
            , ref ITotalPageCount);
        gvMembers.DataSource = dt;
        gvMembers.VirtualItemCount = IRowCount;
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
                RadLabel CatId = (RadLabel)item.FindControl("lblcatid");
                RadLabel Useredited = (RadLabel)item.FindControl("radisedited");
                LinkButton flag = (LinkButton)item.FindControl("imgFlag");

                if (Useredited.Text == "1")
                {
                    flag.Visible = true;

                }
                //int type = 1;
                //if (ViewState["WOID"].ToString() != string.Empty)
                //    type = 2;
                if (Timesheet != null)
                {

                    if (CatId.Text == "4")
                    {
                        //Timesheet.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','PlannedMaintenance/PlannedMaintenanceDailyWorkPlanMemberTimeSheet.aspx?Memberid=" + Memberid.Text + "&type=" + type + "&id=" + activityid.Text + "','false','700px','350px',null,null,onCloseJson);return false");
                        Timesheet.Visible = true;
                    }


                }

            }
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = (GridEditableItem)e.Item;
                UserControlHardExtn ScheduleType = (UserControlHardExtn)item.FindControl("ddlcat");
                if (ScheduleType != null)
                    ScheduleType.SelectedHard = DataBinder.Eval(e.Item.DataItem, "FLDDUTYCATEGORYID").ToString();



            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMembers_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            int type = 0;
            Guid? ID = null;
            ActivityOrWOID(ref ID, ref type);
            Guid? Memberid = General.GetNullableGuid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDMEMBERID"].ToString()).Value;
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
               

                int? ScheduleType = General.GetNullableInteger(((UserControlHardExtn)e.Item.FindControl("ddlcat")).SelectedHard);
                DataTable dt = new DataTable();

                if (ViewState["ID"].ToString() != string.Empty)
                {
                    dt = PhoenixPlannedMaintenanceDailyWorkPlan.EditActivity(new Guid(ViewState["ID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

                }
                else
                {
                    dt = PhoenixPlannedMaintenanceDailyWorkPlan.EditWorkOrder(new Guid(ViewState["WOID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                }
                DataRow dr = dt.Rows[0];
                radlblstarttime.Text = dr["FLDESTSTARTTIME"].ToString();
                radlblendtime.Text = dr["FLDDURATION"].ToString();



                if (!IsValidMember(ScheduleType))
                {
                    ucError.Visible = true;
                    return;
                }
                
                PhoenixPlannedMaintenanceDailyWorkPlan.ActivityMemberUpdate(
                    General.GetNullableInteger(radlblstarttime.Text)
                    , General.GetNullableInteger(radlblendtime.Text), ScheduleType, Memberid, ID);

                gvCrewWorkHrs.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("TIME"))
            {
                ViewState["MSIGNOFFID"] = ((RadLabel)e.Item.FindControl("lblsignoffid")).Text;
                string scriptpopup = String.Format("javascript:openNewWindow('Filters','','"+Session["sitepath"]+"/PlannedMaintenance/PlannedMaintenanceDailyWorkPlanMemberTimeSheet.aspx?Memberid=" + Memberid + "&type=" + type + "&id=" + ID + "','false','700px','350px',null,null,onCloseJson);"); 
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidMember(int? ScheduleCat)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ScheduleCat == null)
            ucError.ErrorMessage = "Schedule Category of the Member.";


        return (!ucError.IsError);
    }







    public void ActivityOrWOID(ref Guid? ID, ref int type)
    {

        Guid? ActivityID = General.GetNullableGuid(ViewState["ID"].ToString());
        Guid? WOID = General.GetNullableGuid(ViewState["WOID"].ToString());


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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        int? SignOffId = General.GetNullableInteger(ViewState["MSIGNOFFID"].ToString());
        Guid? PlanId = General.GetNullableGuid(ViewState["PLANID"].ToString());
        int type = 0;
        Guid? ID = null;
        ActivityOrWOID(ref ID, ref type);

        PhoenixPlannedMaintenanceDailyWorkPlan.ActivityPlannedWorkhoursInsert(SignOffId, PlanId, ID);
        gvCrewWorkHrs.Rebind();
        gvMembers.Rebind();
    }

    private string GetSelectedMembers()
    {
        StringBuilder strlist = new StringBuilder();
        strlist.Append(",");
        if (gvCrewWorkHrs.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvCrewWorkHrs.SelectedItems)
            {


                strlist.Append(((RadLabel)gv.FindControl("lblsignonoffid")).Text + ",");

            }
        }
        if (strlist.Length == 1)
        {
            return "";
        }
        return strlist.ToString();
    }




    private string GetSelectedMemberNames()
    {
        StringBuilder strlist = new StringBuilder();
        strlist.Append(",");
        if (gvCrewWorkHrs.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvCrewWorkHrs.SelectedItems)
            {


                strlist.Append(gv["FLDRANKNAME"].Text + ' ' + gv["FLDEMPLOYEENAME"].Text + ",");

            }
        }
        if (strlist.Length == 1)
        {
            return "";
        }
        return strlist.ToString();
    }

    protected void gvCrewWorkHrs_PreRender(object sender, EventArgs e)
    {
        foreach (GridDataItem item in gvCrewWorkHrs.MasterTableView.Items)
        {

            string SignOnOffId = ((RadLabel)item.FindControl("lblsignonoffid")).Text;

            if (!string.IsNullOrEmpty(SignOnOffId) && ViewState["TEAMMEMBERS"].ToString().Contains(SignOnOffId))
            {
                item.Selected = true;
            }

        }
    }
}

