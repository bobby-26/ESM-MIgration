using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceComponentWorkOrderList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentWorkOrderList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentWorkOrderList.aspx", "View Work Orders", "<i class=\"fas fa-binoculars\"></i>", "View");

            MenuDivWorkOrder.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

                ViewState["ISTREENODECLICK"] = false;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["COMPONENTID"] = null;

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                if (Request.QueryString["COMPONENTID"] != null)
                    ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                BindComponentData();
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDPLANINGPRIORITY", "FLDRANKNAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number ", "Work Order Title", "Component Number", "Component Name", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            DataSet ds = PhoenixPlannedMaintenanceWorkOrder.ComponentWorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(ViewState["COMPONENTID"].ToString()), sortexpression, sortdirection,
                        gvWorkOrder.CurrentPageIndex + 1,
                        gvWorkOrder.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount);
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
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvWorkOrder.CurrentPageIndex = 0;
                BindData();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("VIEW"))
            {
                ClientScript.RegisterStartupScript(GetType(), "Load", "<script type='text/javascript'>top.frames['fraPhoenixApplication'].location.href = '../PlannedMaintenance/PlannedMaintenanceWorkOrder.aspx?COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "'; closeTelerikWindow('codehelp1'); </script>");
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

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDPLANINGPRIORITY", "FLDRANKNAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;
            ds = PhoenixPlannedMaintenanceWorkOrder.ComponentWorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(ViewState["COMPONENTID"].ToString()), sortexpression, sortdirection,
                              gvWorkOrder.CurrentPageIndex + 1,
                              gvWorkOrder.PageSize,
                             ref iRowCount,
                             ref iTotalPageCount);

            General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvWorkOrder.DataSource = ds;
                gvWorkOrder.VirtualItemCount = iRowCount;

            }
            else
            {
                DataTable dt = ds.Tables[0];
                gvWorkOrder.DataSource = "";
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindComponentData()
    {
        if ((Request.QueryString["COMPONENTID"] != null) && (Request.QueryString["COMPONENTID"] != ""))
        {
            DataSet ds = PhoenixInventoryComponent.ListComponent(new Guid(Request.QueryString["COMPONENTID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            DataRow dr = ds.Tables[0].Rows[0];
            //Title1.Text += "    ( " + dr["FLDCOMPONENTNUMBER"].ToString() + " - " + dr["FLDCOMPONENTNAME"].ToString() + ")";
        }
    }

    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //if (e.CommandName == "UPDATE")
        //{
        //    try
        //    {
        //        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        //        RadGrid _gridView = (RadGrid)sender;
        //        int nCurrentRow = e.Item.RowIndex;

        //        RadLabel CounterID = (RadLabel)e.Item.FindControl("lblCounterID");
        //        UserControlDecimal CurrentValue = (UserControlDecimal)e.Item.FindControl("txtCurrentValueEdit");
        //        UserControlDate ReadingDate = (UserControlDate)e.Item.FindControl("txtReadingDateEdit");

        //        UpdateCounterUpdate(CounterID.Text, CurrentValue.Text, ReadingDate.Text);
        //        BindData();

        //    }
        //    catch (Exception ex)
        //    {
        //        ucError.ErrorMessage = ex.Message;
        //        ucError.Visible = true;
        //    }
        //}
        //if (e.CommandName == RadGrid.ExportToExcelCommandName)
        //{
        //    ShowExcel();
        //}
        //if (e.CommandName == RadGrid.RebindGridCommandName)
        //{
        //    RadGrid1.CurrentPageIndex = 0;
        //}
    }

    protected void gvWorkOrder_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
}
