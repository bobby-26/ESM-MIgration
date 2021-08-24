using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class Inspection_InspectionBulkDeficiencyClosure : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Close", "SAVE", ToolBarDirection.Right);
        MenuOfficeRemarks.AccessRights = this.ViewState;
        MenuOfficeRemarks.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            txtOfficeRemarks.Text = "The close-out actions of the deficiency was reviewed and found effective.";
        }
    }

    protected void MenuOfficeRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (General.GetNullableString(txtOfficeRemarks.Text) == null && General.GetNullableString(txtOfficeRemarks.Text) == string.Empty)
                {
                    ucError.ErrorMessage = "Closedout remarks is required.";
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableDateTime(ucCompletionDate.Text) == null)
                {
                    ucError.ErrorMessage = "Closedout Date is required.";
                    ucError.Visible = true;
                    return;
                }
                if (Filter.CurrentSelectedBulkDeficiencies != null)
                {
                    ArrayList selectedofficetask = (ArrayList)Filter.CurrentSelectedBulkDeficiencies;
                    if (selectedofficetask != null && selectedofficetask.Count > 0)
                    {
                        foreach (Guid preventiveactionid in selectedofficetask)
                        {
                            PhoenixInspectionDeficiency.DeficiencyBulkClosureUpdate(
                                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                     , new Guid(preventiveactionid.ToString())                                                     
                                                     , txtOfficeRemarks.Text
                                                     , General.GetNullableDateTime(ucCompletionDate.Text)   
                                                     );
                        }
                    }
                }
            }
            Filter.CurrentSelectedBulkDeficiencies = null;
            ucStatus.Text = "Completion remarks updated for selected tasks.";
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            Filter.CurrentSelectedBulkDeficiencies = null;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
