using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceFormComponentJobMapList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceFormComponentJobMapList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('RadGrid1')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuDivComponentJob.AccessRights = this.ViewState;
            MenuDivComponentJob.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
            }
            RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            //  BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDREMAININGFREQUENCY", "FLDJOBLASTDONEDATE", "FLDLASTDONEHOURS", "FLDDISCIPLINENAME", "FLDJOBNEXTDUEDATE", "FLDPRIORITY" };
            string[] alCaptions = { "Component No.", "Component Name", "Job Code", "Job Title", "Frequency", "Remaining Frequency", "Last Done date", "Last Done Hour", "Responsibility", "Next Due Date", "Priority" };

            string S = Request.QueryString["FORMID"].ToString();
            DataTable dt = PhoenixPlannedMaintenanceHistoryTemplate.ListMainteanceFormComJob(new Guid(ViewState["FORMID"].ToString())
                , PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("RadGrid1", "Component - Job", alCaptions, alColumns, ds);

            RadGrid1.DataSource = dt;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDivComponentJob_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper()=="EXCEL")
        {
            ShowExcel();
        }
    }
    protected void ShowExcel()
    {
        try
        {
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDREMAININGFREQUENCY", "FLDJOBLASTDONEDATE", "FLDLASTDONEHOURS", "FLDDISCIPLINENAME", "FLDJOBNEXTDUEDATE", "FLDPRIORITY" };
            string[] alCaptions = { "Component No.", "Component Name", "Job Code", "Job Title", "Frequency", "Remaining Frequency", "Last Done date", "Last Done Hour", "Responsibility", "Next Due Date", "Priority" };

            DataTable dt = PhoenixPlannedMaintenanceHistoryTemplate.ListMainteanceFormComJob(new Guid(ViewState["FORMID"].ToString()),
                PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            General.ShowExcel("Maintenance Form", dt, alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            ShowExcel();
        }
    }
    protected void Rebind()
    {
        RadGrid1.SelectedIndexes.Clear();
        RadGrid1.EditIndexes.Clear();
        RadGrid1.DataSource = null;
        RadGrid1.Rebind();
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : RadGrid1.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
}