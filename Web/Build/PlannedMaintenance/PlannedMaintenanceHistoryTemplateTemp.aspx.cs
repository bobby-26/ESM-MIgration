using SouthNests.Phoenix.Framework;
using System;

public partial class PlannedMaintenance_PlannedMaintenanceHistoryTemplateTemp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        btnImport.Visible = SessionUtil.CanAccess(this.ViewState, "IMPORT");
    }    
}