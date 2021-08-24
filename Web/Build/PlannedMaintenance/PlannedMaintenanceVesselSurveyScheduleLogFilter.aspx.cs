using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Data;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyScheduleLogFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Go", "SEARCH",ToolBarDirection.Right);
        SurveyScheduleLogFilter.AccessRights = this.ViewState;
        SurveyScheduleLogFilter.MenuList = toolbarmain.Show();        
    }
    protected void SurveyScheduleLogFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName == "SEARCH")
            {                
                NameValueCollection nvc = Filter.VesselSurveyLogFilter;
                if (nvc == null)
                {
                    nvc = new NameValueCollection();
                    
                    nvc.Add("CompletedFrom", ucFromDate.Text);
                    nvc.Add("CompletedTo", ucToDate.Text);
                    nvc.Add("IssueFrom", ucIssueFrom.Text);
                    nvc.Add("IssueTo", ucIssueTo.Text);
                    nvc.Add("CategoryList", ucCategory.SelectedList);
                    Filter.VesselSurveyLogFilter = nvc;
                }
                else
                {                 
                    nvc["CompletedFrom"] = ucFromDate.Text;
                    nvc["CompletedTo"] = ucToDate.Text;
                    nvc["IssueFrom"] = ucIssueFrom.Text;
                    nvc["IssueTo"] = ucIssueTo.Text;
                    nvc["CategoryList"] = ucCategory.SelectedList;
                }
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleLogList.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
