using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();


            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('RadGrid1')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderSummary.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
         
            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                for (int i = 2005; i <= DateTime.Now.Year + 2; i++)
                {
                    ddlYear.Items.Add(new DropDownListItem(i.ToString(), i.ToString()));
                }
                ddlYear.Items.Insert(0, new DropDownListItem("--Select--", ""));

                ddlYear.SelectedValue = DateTime.Now.Year.ToString();
                txtVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    txtVessel.Visible = true;
                    ddlVessel.Visible = false;
                }
                else
                {
                    txtVessel.Visible = false;
                    ddlVessel.Visible = true;
                }

            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                RadGrid1.Rebind();
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
        try
        {
            int? VesselId = null;
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                VesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            else
                VesselId = General.GetNullableInteger(ddlVessel.SelectedVessel);

            DataTable dt = PhoenixPlannedMaintenanceWorkOrder.ListWorkorderReportSummary(VesselId, General.GetNullableInteger(ddlYear.SelectedValue));

            string vesselname = string.Empty;
            if (dt.Rows.Count > 0)
                vesselname = dt.Rows[0]["FLDVESSELNAME"].ToString();

            string[] alColumns = { "FLDSTATISCS", "FLDJAN", "FLDFEB", "FLDMAR", "FLDAPR", "FLDMAY", "FLDJUN", "FLDJUL", "FLDAUG", "FLDSEP", "FLDOCT", "FLDNOV", "FLDDEC" };
            string[] alCaptions = { vesselname, "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            General.ShowExcel("PMS Statistics", dt, alColumns, alCaptions, null, null);

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


            int? VesselId = null;
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                VesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            else
                VesselId = General.GetNullableInteger(ddlVessel.SelectedVessel);

            DataTable dt = PhoenixPlannedMaintenanceWorkOrder.ListWorkorderReportSummary(VesselId, General.GetNullableInteger(ddlYear.SelectedValue));

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            string vesselname = string.Empty;
            if (dt.Rows.Count > 0)
                vesselname = dt.Rows[0]["FLDVESSELNAME"].ToString();


            string[] alColumns = { "FLDSTATISCS", "FLDJAN", "FLDFEB", "FLDMAR", "FLDAPR", "FLDMAY", "FLDJUN", "FLDJUL", "FLDAUG", "FLDSEP", "FLDOCT", "FLDNOV", "FLDDEC" };
            string[] alCaptions = { vesselname, "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            General.SetPrintOptions("RadGrid1", "PMS Statistics", alCaptions, alColumns, ds);

            RadGrid1.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        //{
        //    GridEditableItem item = e.Item as GridEditableItem;

        //    string disciplineCode = ((RadTextBox)e.Item.FindControl("txtDisciplineCodeEdit")).Text;
        //    string disciplineName = ((RadTextBox)e.Item.FindControl("txtDisciplineNameEdit")).Text;
        //}
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            RadGrid1.ExportSettings.Excel.Format = GridExcelExportFormat.Biff;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            ShowExcel();
        }
        if (e.CommandName == RadGrid.RebindGridCommandName)
        {
            RadGrid1.CurrentPageIndex = 0;
        }

    }
    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

}
