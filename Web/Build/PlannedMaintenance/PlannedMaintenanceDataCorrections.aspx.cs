using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Data;
using SouthNests.Phoenix.PlannedMaintenance;

public partial class PlannedMaintenance_PlannedMaintenanceDataCorrections : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            grid.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
        PhoenixToolbar Menu = new PhoenixToolbar();
        Menu.AddButton("Search", "SEARCH", ToolBarDirection.Left);
        Menu.AddButton("Re-Calculate", "RECALCULATE", ToolBarDirection.Left);
        Menu.AddButton("Correct", "CORRECT", ToolBarDirection.Right);
        Tabstrip.MenuList = Menu.Show();
    }

    protected void Tabstrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                grid.Rebind();
            }
            if (CommandName.ToUpper().Equals("RECALCULATE"))
            {
                int? VesselId = General.GetNullableInteger(ddlvessellist.SelectedVessel);
                DateTime? Date = General.GetNullableDateTime(raddate.Text);
                if (VesselId == null || Date == null)
                {
                    
                                      
                    return;
                }
                PhoenixPlannedMaintenanceDataCorrections.MaintenanceandActivityDataCorrectionDateRecal(VesselId, Date);
                grid.Rebind();
            }
            if (CommandName.ToUpper().Equals("CORRECT"))
            {
                foreach (GridDataItem gv in grid.SelectedItems)
                {

                    PhoenixPlannedMaintenanceDataCorrections.MaintenanceandActivityDataCorrectionUpdate(
                        General.GetNullableGuid(((RadLabel)gv.FindControl("radid")).Text)
                        , General.GetNullableInteger(((RadLabel)gv.FindControl("radtype")).Text)
                        );



                }
                grid.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? VesselId = General.GetNullableInteger(ddlvessellist.SelectedVessel);
        DateTime? Date = General.GetNullableDateTime(raddate.Text);
        DataTable dt = PhoenixPlannedMaintenanceDataCorrections.MaintenanceandActivityDataCorrectionSearch(VesselId, Date, grid.CurrentPageIndex + 1, grid.PageSize, ref iRowCount, ref iTotalPageCount);

        grid.DataSource = dt;
        grid.VirtualItemCount = iRowCount;
    }

    protected void ddlvessellist_TextChangedEvent(object sender, EventArgs e)
    {
        grid.Rebind();
    }
}