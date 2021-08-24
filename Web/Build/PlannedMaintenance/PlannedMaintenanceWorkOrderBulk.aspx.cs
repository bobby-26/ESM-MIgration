using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Configuration;

public partial class PlannedMaintenanceWorkOrderBulk : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Generate Job", "SAVE", ToolBarDirection.Right);
                MenuWorkOrder.MenuList = toolbarmain.Show();

                MenuDivWorkOrder.Visible = false;
                Session["GENWORKORDER"] = null;
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
            if (CommandName.ToUpper() == "SAVE")
            {
                string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDPRIORITY", "FLDDISCIPLINENAME", "FLDJOBNEXTDUEDATE" };
                string[] alCaptions = { "Component Number", "Component Name", "Job Code", "Job Title", "Frequency", "Priority", "Resp Discipline", "Next Due Date" };
                DataSet ds = new DataSet();
                if (ConfigurationManager.AppSettings.Get("PhoenixTelerik") != null && ConfigurationManager.AppSettings.Get("PhoenixTelerik").ToString() == "1")
                {
                    ds = PhoenixPlannedMaintenanceWorkOrder.InsertNewWorkOrderBulk(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , byte.Parse(chkCalendarJobs.Checked == true ? "1" : "0"), byte.Parse(chkRunhourJobs.Checked == true ? "1" : "0"), byte.Parse(chkRounds.Checked == true ? "1" : "0"));
                }
                else
                {
                    ds = PhoenixPlannedMaintenanceWorkOrder.InsertWorkOrderBulk(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , byte.Parse(chkCalendarJobs.Checked == true ? "1" : "0"), byte.Parse(chkRunhourJobs.Checked == true ? "1" : "0"), byte.Parse(chkRounds.Checked == true ? "1" : "0"));
                }
                Session["GENWORKORDER"] = ds;
                General.SetPrintOptions("gvWorkOrder", "Jobs", alCaptions, alColumns, ds);

                PhoenixToolbar toolbargrid = new PhoenixToolbar();

                toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderBulk.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
                toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
                MenuDivWorkOrder.MenuList = toolbargrid.Show();

                MenuDivWorkOrder.Visible = true;
                gvWorkOrder.Rebind();
            }
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

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                DataSet ds = (DataSet)Session["GENWORKORDER"];
                string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDPRIORITY", "FLDDISCIPLINENAME", "FLDJOBNEXTDUEDATE" };
                string[] alCaptions = { "Component Number", "Component Name", "Job Code", "Job Title", "Frequency", "Priority", "Resp Discipline", "Next Due Date" };
                General.ShowExcel("Work Order", ds.Tables[0], alColumns, alCaptions, null, string.Empty);
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
        DataSet ds = new DataSet();
        if (Session["GENWORKORDER"] == null)
        {

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDPRIORITY", "FLDDISCIPLINENAME", "FLDJOBNEXTDUEDATE" };
            string[] alCaptions = { "Component Number", "Component Name", "Job Code", "Job Title", "Frequency", "Priority", "Resp Discipline", "Next Due Date" };
            General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);
            if (ConfigurationManager.AppSettings.Get("PhoenixTelerik") != null && ConfigurationManager.AppSettings.Get("PhoenixTelerik").ToString() == "1")
            {
                ds = PhoenixPlannedMaintenanceWorkOrder.InsertNewWorkOrderBulk(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , byte.Parse(chkCalendarJobs.Checked == true ? "1" : "0"), byte.Parse(chkRunhourJobs.Checked == true ? "1" : "0"), byte.Parse(chkRounds.Checked == true ? "1" : "0"));
                gvWorkOrder.Visible = false;
            }
            else
            {
                ds = PhoenixPlannedMaintenanceWorkOrder.InsertWorkOrderBulk(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                       , byte.Parse(chkCalendarJobs.Checked == true ? "1" : "0"), byte.Parse(chkRunhourJobs.Checked == true ? "1" : "0"), byte.Parse(chkRounds.Checked == true ? "1" : "0"));
                gvWorkOrder.Visible = false;
            }
        }
        else
        {
            ds = (DataSet)Session["GENWORKORDER"];
            gvWorkOrder.Visible = true;
            if (ds.Tables[0].Rows.Count > 0)
            {
                ucStatus.Text = "Work Order Generated";
            }
            else
            {
                ucStatus.Text = "No Work Order Generated";
            }
        }
        gvWorkOrder.DataSource = ds;

    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //if (e.CommandName == RadGrid.ExportToExcelCommandName)
        //{
        //    ShowExcel();
        //}
    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.Rebind();
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

}
