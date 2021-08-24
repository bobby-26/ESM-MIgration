using System;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Framework;

public partial class PlannedMaintenanceManuals : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                string src = PhoenixPlannedMaintenanceManuals.PMSManualsRoot();
                ifMoreInfo.Attributes["src"] = src + "?" + Request.QueryString.ToString();
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
