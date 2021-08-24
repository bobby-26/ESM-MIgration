using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class OwnersInventorySpareTransaction : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("../Owners/OwnersInventorySpareTransaction.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarmain.AddFontAwesomeButton("javascript:CallPrint('GVR')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbarmain.AddFontAwesomeButton("../Owners/OwnersInventorySpareTransaction.aspx?" + Request.QueryString.ToString(), "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuWorkOrder.AccessRights = this.ViewState;
        MenuWorkOrder.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            ViewState["txtComponentName"]= "";
            ViewState["txtComponentNumber"] = "";
            ViewState["txtName"] = "";

        }
    }
    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                
                foreach (GridColumn column in GVR.MasterTableView.Columns)
                {

                    column.ListOfFilterValues = null; // CheckList values set to null will uncheck all the checkboxes

                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;

                    column.AndCurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.AndCurrentFilterValue = string.Empty;
                }
                GVR.MasterTableView.FilterExpression = string.Empty;
                ViewState["txtComponentName"] = "";
                ViewState["txtComponentNumber"] = "";
                ViewState["txtName"] = "";
                Rebind();
            }
            if (CommandName.ToUpper() == "EXCEL")
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
        try
        {
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDMEASURE", "FLDPURCHASEQTY", "FLDCONSUMEDQTY" };
            string[] alCaptions = { "Component No.", "Component Name", "Item", "Purchased", "Consumed" };


            DataTable dt = PhoenixOwnerReportQuality.OwnersReportInventoryTransactionSummary(General.GetNullableInteger(Filter.SelectedOwnersReportVessel)
           , General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
           , General.GetNullableString(ViewState["txtComponentNumber"].ToString())
           , General.GetNullableString(ViewState["txtComponentName"].ToString())
           , General.GetNullableString(ViewState["txtName"].ToString()));
            GVR.DataSource = dt;

            string heading = Request.QueryString["title"];
            if (string.IsNullOrEmpty(heading))
            {
                heading = "Inventory - Spare Item Transaction";
            }
            General.ShowExcel(heading, dt, alColumns, alCaptions, 1, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void GVR_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDMEASURE", "FLDPURCHASEQTY", "FLDCONSUMEDQTY" };
        string[] alCaptions = { "Component No.", "Component Name", "Item", "Purchased", "Consumed" };

        DataTable dt = PhoenixOwnerReportQuality.OwnersReportInventoryTransactionSummary(General.GetNullableInteger(Filter.SelectedOwnersReportVessel)
            , General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
            ,General.GetNullableString(ViewState["txtComponentNumber"].ToString())
            , General.GetNullableString(ViewState["txtComponentName"].ToString())
            , General.GetNullableString(ViewState["txtName"].ToString()));
        //GVR.DataSource = null;
        GVR.DataSource = dt;
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        string heading = Request.QueryString["title"];
        if (string.IsNullOrEmpty(heading))
        {
            heading = "Inventory - Spare Item Transaction";
        }
        General.SetPrintOptions("GVR", heading, alCaptions, alColumns, ds);

    }

    protected void GVR_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (e.Column is GridGroupSplitterColumn)
        {
            GridGroupSplitterColumn sc = (GridGroupSplitterColumn)e.Column;
            sc.HeaderStyle.Width = Unit.Pixel(1);
            sc.Resizable = false;
            sc.Display.Equals("none");
        }
    }

    protected void GVR_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
            DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
            item.DataCell.Text = groupDataRow["FLDTYPE"].ToString();
        }
        RadGrid grid = (RadGrid)sender;
        if (e.Item is GridFilteringItem)
        {
            grid.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue = ViewState["txtComponentNumber"].ToString();
            grid.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue = ViewState["txtComponentName"].ToString();
            grid.MasterTableView.GetColumn("FLDMEASURE").CurrentFilterValue = ViewState["txtName"].ToString();

        }

    }

    protected void GVR_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            (e.Item as GridGroupHeaderItem).Cells[0].Controls.Clear();
            (e.Item as GridGroupHeaderItem).Cells[0].Visible = false;
        }
    }
    protected void Rebind()
    {
        GVR.SelectedIndexes.Clear();
        GVR.EditIndexes.Clear();
        GVR.DataSource = null;
        GVR.Rebind();
    }
    protected void GVR_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.FilterCommandName)
        {
            ViewState["txtComponentNumber"] = GVR.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue;
            ViewState["txtComponentName"] = GVR.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue;
            ViewState["txtName"] = GVR.MasterTableView.GetColumn("FLDMEASURE").CurrentFilterValue;
            Rebind();
        }
    }
    protected void GVR_PreRender(object sender, EventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (!IsPostBack)
        { 
            grid.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue = ViewState["txtComponentNumber"].ToString();
            grid.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue = ViewState["txtComponentName"].ToString();
            grid.MasterTableView.GetColumn("FLDMEASURE").CurrentFilterValue = ViewState["txtName"].ToString();

            Rebind();
        }
    }
}