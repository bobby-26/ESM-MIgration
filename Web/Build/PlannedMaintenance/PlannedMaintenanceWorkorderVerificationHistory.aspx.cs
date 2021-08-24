using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class PlannedMaintenanceWorkorderVerificationHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvWorkorderList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            if (!IsPostBack)
            {
                ViewState["groupid"] = Request.QueryString["groupid"] ?? string.Empty;
                ViewState["workorderid"] = Request.QueryString["workorderid"] ?? string.Empty;
                ViewState["vslid"] = Request.QueryString["vesselid"] ?? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? groupId = Guid.Empty;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDCOMPONENTNO", "FLDWORKORDERNAME", "FLDPLANNINGDUEDATE", "FLDDISCIPLINENAME", "FLDOFFICEVERIFICATION", "FLDOFFICEVERIFYREMARKS", "FLDVESSELVERIFICATION", "FLDVESSELVERIFYREMARKS" };
                string[] alCaptions = { "Comp. No.", "Title", "Due Date", "Assigned To", "Supnt Verification","Supnt Remarks","Vessel Verification","Vessel Remarks"};

                DataTable dt;
                dt = PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderVerificationHistory(General.GetNullableGuid(ViewState["groupid"].ToString()),General.GetNullableGuid(ViewState["workorderid"].ToString()) , int.Parse(ViewState["vslid"].ToString()));

                General.ShowExcel("Work Order Verification", dt,alColumns,alCaptions,null,null);
            }               
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkorderList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            string[] alColumns = { "FLDCOMPONENTNO", "FLDWORKORDERNAME", "FLDPLANNINGDUEDATE", "FLDDISCIPLINENAME", "FLDOFFICEVERIFICATION", "FLDOFFICEVERIFYREMARKS", "FLDVESSELVERIFICATION", "FLDVESSELVERIFYREMARKS" };
            string[] alCaptions = { "Comp. No.", "Title", "Due Date", "Assigned To", "Supnt Verification", "Supnt Remarks", "Vessel Verification", "Vessel Remarks" };

            DataTable dt;
            dt = PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderVerificationHistory(General.GetNullableGuid(ViewState["groupid"].ToString()), General.GetNullableGuid(ViewState["workorderid"].ToString()), int.Parse(ViewState["vslid"].ToString()));

            DataSet ds = new DataSet();
            //ds.Tables.Add(dt);
            General.SetPrintOptions("gvWorkorderList","Work Order Verification", alCaptions, alColumns, ds);

            gvWorkorderList.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
