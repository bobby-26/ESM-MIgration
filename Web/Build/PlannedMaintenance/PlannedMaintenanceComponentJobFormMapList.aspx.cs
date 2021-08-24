using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceComponentJobFormMapList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobFormMapList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvHistoryTemplate')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuDivMaintenanceForm.AccessRights = this.ViewState;
            MenuDivMaintenanceForm.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["COMPONENTJOBID"] = Request.QueryString["componentjobid"];

                ViewState["VESSELID"] = Request.QueryString["VESSELID"] ?? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvHistoryTemplate_PreRender(object sender, EventArgs e)
    {
    }
    protected void MenuDivMaintenanceForm_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        try
        {
            string[] alColumns = { "FLDROWNUMBER", "FLDFORMNAME" };
            string[] alCaptions = { "S.No", "Form Name" };

            DataTable dt = PhoenixPlannedMaintenanceHistoryTemplate.ListComponentJobMaintenanceForm(new Guid(ViewState["COMPONENTJOBID"].ToString())
                , int.Parse(ViewState["VESSELID"].ToString()));

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvHistoryTemplate", "Maintenance Form", alCaptions, alColumns, ds);
            if (dt.Rows.Count > 0)
            {
                gvHistoryTemplate.DataSource = dt;
                //gvHistoryTemplate.VirtualItemCount = iRowCount;
            }
            else
            {
                gvHistoryTemplate.DataSource = "";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvHistoryTemplate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvHistoryTemplate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            ShowExcel();
        }
    }

    protected void ShowExcel()
    {
        try
        {
            string[] alColumns = { "FLDROWNUMBER", "FLDFORMNAME" };
            string[] alCaptions = { "S.No", "Form Name" };

            DataTable dt = PhoenixPlannedMaintenanceHistoryTemplate.ListComponentJobMaintenanceForm(new Guid(ViewState["COMPONENTJOBID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()));

            General.ShowExcel("Maintenance Form", dt, alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}