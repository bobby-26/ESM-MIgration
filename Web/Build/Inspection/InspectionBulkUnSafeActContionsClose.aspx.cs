using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionBulkUnSafeActContionsClose : PhoenixBasePage
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
                if (General.GetNullableString(txtOfficeRemarks.Text.Trim()) == null)
                {
                    ucError.ErrorMessage = "Closed Out remarks is required.";
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableDateTime(ucCompletionDate.Text) == null)
                {
                    ucError.ErrorMessage = "Closed Out Date is required.";
                    ucError.Visible = true;
                    return;
                }
                if (Filter.CurrentSelectedBulkDirectIncident != null)
                {
                    ArrayList selectedDirectIncident = (ArrayList)Filter.CurrentSelectedBulkDirectIncident;
                    if (selectedDirectIncident != null && selectedDirectIncident.Count > 0)
                    {
                        foreach (Guid DirectIncidentid in selectedDirectIncident)
                        {
                            PhoenixInspectionUnsafeActsConditions.DirectIncidentNearmissBulkClose(new Guid(DirectIncidentid.ToString())
                                                                                                    , txtOfficeRemarks.Text.Trim()
                                                                                                    , General.GetNullableDateTime(ucCompletionDate.Text));
                        }
                    }
                }
            }
            Filter.CurrentSelectedBulkDirectIncident = null;
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            Filter.CurrentSelectedBulkDirectIncident = null;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
