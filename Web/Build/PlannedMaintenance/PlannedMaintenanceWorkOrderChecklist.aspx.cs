using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderChecklist : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderChecklist.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderChecklist.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderChecklist.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
          
            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["WORKORDERID"] = null;
                txtDate.Text = DateTime.Now.ToString();
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
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDPLANNINGDUEDATE", "FLDDATEDIFF" };
            string[] alCaptions = { "Job Number", "Job Code & Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Due Date", "Days Overdue" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            DataSet ds;

            ds = PhoenixPlannedMaintenanceChecklistReporting.WorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableDateTime(txtDate.Text),
               sortexpression, sortdirection,
                             1, iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Work Order", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "FIND")
            {
                gvWorkOrder.CurrentPageIndex = 0;
                gvWorkOrder.Rebind();
            }
            else if (CommandName.ToUpper() == "EXCEL")
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper() == "CLEAR")
            {
                ViewState["WORKORDERID"] = null;
                txtDate.Text = DateTime.Now.ToString();
                BindData();
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
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDPLANNINGDUEDATE", "FLDDATEDIFF" };
            string[] alCaptions = { "Job Number", "Job Code & Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Due Date", "Days Overdue" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            ds = PhoenixPlannedMaintenanceChecklistReporting.WorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableDateTime(txtDate.Text)
                    , sortexpression, sortdirection
                    , gvWorkOrder.CurrentPageIndex+1
                    , gvWorkOrder.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);
            General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;
          

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "DONE")
            {
                if (string.IsNullOrEmpty(((UserControlDate)e.Item.FindControl("txtDoneDate")).Text))
                {
                    ucError.ErrorMessage = "Done Date is Required";
                    ucError.Visible = true;
                    return;
                }
                if (string.IsNullOrEmpty(((RadTextBox)e.Item.FindControl("txtRemarks")).Text))
                {
                    ucError.ErrorMessage = "Remarks is Required";
                    ucError.Visible = true;
                    return;
                }
                string workorderId = ((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text;
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarks")).Text;
                PhoenixPlannedMaintenanceChecklistReporting.WorkOrderReportConfirm(new Guid(workorderId), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtDoneDate")).Text)
                                , remarks);
                BindData();
                gvWorkOrder.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}