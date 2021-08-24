using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceToolboxMeeting : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Submit", "SUBMIT", ToolBarDirection.Right);              
        MenuToolBoxMeet.AccessRights = this.ViewState;
        MenuToolBoxMeet.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["DATE"] = string.Empty;
            if(!string.IsNullOrEmpty(Request.QueryString["td"]))
            {
                ViewState["DATE"] = Request.QueryString["td"];
            }
            ViewState["PLANID"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["dwpid"]))
            {
                ViewState["PLANID"] = Request.QueryString["dwpid"];
            }
            PopuldateRegisters();
            txtDateTime.SelectedDate = General.GetNullableDateTime(ViewState["DATE"].ToString());
        }
    }
    protected void MenuToolBoxMeet_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SUBMIT"))
            {
                string picid = ddlPersonIncharge.SelectedValue;
                string picname = ddlPersonIncharge.Text;
                var collection = ddlOtherMembers.CheckedItems;
                string csvOtherMembers = string.Empty;
                string csvOtherMembersName = string.Empty;
                if (collection.Count != 0)
                {
                    csvOtherMembers = ",";
                    csvOtherMembersName = ",";
                    foreach (var item in collection)
                    {
                        csvOtherMembers = csvOtherMembers + item.Value + ",";
                        csvOtherMembersName = csvOtherMembersName + item.Text + ",";
                    }
                }

                if (!IsValidMeeting(picid, csvOtherMembers))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPlannedMaintenanceDailyWorkPlanMeeting.Insert(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["PLANID"].ToString())
                    , txtDateTime.SelectedDate.Value, int.Parse(picid), picname, csvOtherMembers, csvOtherMembersName, txtNotes.Text);
                string script = "function f(){parent.CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidMeeting(string pic, string othermembers)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtDateTime.SelectedDate == null)
            ucError.ErrorMessage = "Date & Time is required.";

        if (General.GetNullableInteger(pic) == null)
            ucError.ErrorMessage = "PIC is required.";

        if (othermembers.Trim().Equals(""))
            ucError.ErrorMessage = "Other Members is required.";

        return (!ucError.IsError);
    }

    private DataTable GetCrewList()
    {
        return PhoenixVesselAccountsEmployee.ListVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableDateTime(ViewState["DATE"].ToString()));
    }
    private void PopuldateRegisters()
    {       
        DataTable crewList = GetCrewList();
        ddlOtherMembers.DataSource = crewList;
        ddlOtherMembers.DataBind();

        ddlPersonIncharge.DataSource = crewList;
        ddlPersonIncharge.DataBind();
    }
}