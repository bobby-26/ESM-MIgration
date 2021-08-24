using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceDailyWorkPlanMeeting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceDailyWorkPlanMeeting.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvToolBoxList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:showDialog('Add');", "Add Toolbox", "<i class=\"fa fa-plus-circle\"></i>", "ADD");        
        MenuMain.AccessRights = this.ViewState;
        MenuMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["DWPID"] = string.Empty;
            if(!string.IsNullOrEmpty(Request.QueryString["dwpid"]))
            {
                ViewState["DWPID"] = Request.QueryString["dwpid"];
            }
            ViewState["DATE"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["td"]))
            {
                ViewState["DATE"] = Request.QueryString["td"];
            }
        }
        modalPopup.NavigateUrl = Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceToolboxMeeting.aspx?dwpid=" + ViewState["DWPID"] + "&td=" + ViewState["DATE"];
    }
    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {

        string[] alColumns = { "FLDDATETIME", "FLDPERSONINCHARGENAME", "FLDOTHERMEMBERSNAME", "FLDNOTES" };
        string[] alCaptions = { "Date", "PIC", "Others", "Notes" };
        
        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlanMeeting.List(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(ViewState["DWPID"].ToString()));

        General.ShowExcel("Daily Work Plan Toolbox Meeting", dt, alColumns, alCaptions, null, null);
    }
    protected void gvToolBoxList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        string[] alColumns = { "FLDDATETIME", "FLDPERSONINCHARGENAME", "FLDOTHERMEMBERSNAME", "FLDNOTES" };
        string[] alCaptions = { "Date", "PIC", "Others", "Notes" };

        RadGrid grid = (RadGrid)sender;
        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlanMeeting.List(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(ViewState["DWPID"].ToString()));
        grid.DataSource = dt;

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvToolBoxList", "Daily Work Plan Toolbox Meeting", alCaptions, alColumns, ds);
    }
}