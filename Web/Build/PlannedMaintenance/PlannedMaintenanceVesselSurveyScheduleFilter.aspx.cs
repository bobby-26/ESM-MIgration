using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyScheduleFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Go", "SEARCH");
        SurveyScheduleFilter.AccessRights = this.ViewState;
        SurveyScheduleFilter.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }
    }
    protected void SurveyScheduleFilter_TabStripCommand(object sender, EventArgs e)
    {
        try 
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName == "SEARCH")
            {
                if (ucVessel.SelectedVessel.ToUpper().Equals("DUMMY"))
                {
                    ucError.ErrorMessage = "Please Select Vessel";
                    ucError.Visible = true;
                    return;
                }
                NameValueCollection nvc = Filter.VesselSurveyFilter;
                if (nvc == null)
                {
                    nvc = new NameValueCollection();
                    nvc.Add("VesselId", ucVessel.SelectedVessel);
                    nvc.Add("DueFrom", ucFromDate.Text);
                    nvc.Add("DueTo", ucToDate.Text);
                    nvc.Add("Status", chkShowPlanned.Checked ? "1" : "0");
                    nvc.Add("Category", ucCategory.SelectedList);
                    nvc.Add("PlanFrom", ucPlanFrom.Text);
                    nvc.Add("PlanTo", ucPlanTo.Text);
                    nvc.Add("Due", ddlDue.SelectedValue);
                    nvc.Add("ShowAll", chkShowAll.Checked ? "1" : "0");
                    Filter.VesselSurveyFilter = nvc;
                }
                else
                {
                    nvc["VesselId"] = ucVessel.SelectedVessel;
                    nvc["DueFrom"] = ucFromDate.Text;
                    nvc["DueTo"] = ucToDate.Text;
                    nvc["Status"] = chkShowPlanned.Checked ? "1" : "0";
                    nvc["Category"] = ucCategory.SelectedList;
                    nvc["PlanFrom"] = ucPlanFrom.Text;
                    nvc["PlanTo"] = ucPlanTo.Text;
                    nvc["Due"] = ddlDue.SelectedValue;
                    nvc["ShowAll"] = chkShowAll.Checked ? "1" : "0";
                }
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleList.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
