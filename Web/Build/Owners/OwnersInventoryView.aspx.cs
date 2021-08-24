using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;

public partial class OwnersInventoryView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("../Owners/OwnersInventoryView.aspx?" + Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarmain.AddFontAwesomeButton("javascript:CallPrint('GVR')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbarmain.AddFontAwesomeButton("../Owners/OwnersInventoryView.aspx?" + Request.QueryString.ToString(), "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuWorkOrder.AccessRights = this.ViewState;
        MenuWorkOrder.MenuList = toolbarmain.Show();
        

        if (!IsPostBack)
        {
            Filter.CurrentOwnerReportInventoryFilter = null;           
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
                Filter.CurrentOwnerReportInventoryFilter = null;
                foreach (GridColumn column in GVR.MasterTableView.Columns)
                {

                    column.ListOfFilterValues = null; // CheckList values set to null will uncheck all the checkboxes

                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;

                    column.AndCurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.AndCurrentFilterValue = string.Empty;
                }
                GVR.MasterTableView.FilterExpression = string.Empty;
                GVR.MasterTableView.Rebind();
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

    protected void GVR_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        binddata();
    }

    protected void binddata()
    {
        string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDMEASURE", "FLDQTY", "FLDMINIMUMQTY", "FLDREQUIREDQTY" };
        string[] alCaptions = { "Component No.", "Component Name", "Item", "ROB", "Min Qty", "Required Quantity" };

        NameValueCollection nvc = Filter.CurrentOwnerReportInventoryFilter;
        if (nvc == null)
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Add("txtComponentName", string.Empty);
            criteria.Add("txtComponentNumber", string.Empty);
            criteria.Add("txtName", string.Empty);
            criteria.Add("ISSHORTFALL", "0");

            Filter.CurrentOwnerReportInventoryFilter = criteria;
            nvc = Filter.CurrentOwnerReportInventoryFilter;
        }



        DataSet ds = PhoenixOwnerReportQuality.OwnersReportInventorySummaryDetail(General.GetNullableInteger(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
                                , nvc["txtComponentNumber"] ?? null
                    , nvc["txtComponentName"] ?? null
                    , nvc["txtName"] ?? null
                    , General.GetNullableInteger(nvc["ISSHORTFALL"].ToString()));
        GVR.DataSource = ds;



        string heading = Request.QueryString["title"];
        if (string.IsNullOrEmpty(heading))
        {
            heading = "Inventory - List of Major Parts onboard";
        }
        General.SetPrintOptions("GVR", heading, alCaptions, alColumns, ds);
    }

    protected void ShowExcel()
    {
        try
        {
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDMEASURE", "FLDQTY", "FLDMINIMUMQTY", "FLDREQUIREDQTY" };
            string[] alCaptions = { "Component No.", "Component Name", "Item", "ROB", "Min Qty","Required Quantity" };

            NameValueCollection nvc = Filter.CurrentOwnerReportInventoryFilter;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
            }


            DataSet ds = PhoenixOwnerReportQuality.OwnersReportInventorySummaryDetail(General.GetNullableInteger(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate)
                                    , nvc["txtComponentNumber"] ?? null
                        , nvc["txtComponentName"] ?? null
                        , nvc["txtName"] ?? null
                        , General.GetNullableInteger(nvc["ISSHORTFALL"].ToString()));
            GVR.DataSource = ds;

            string heading = Request.QueryString["title"];
            if (string.IsNullOrEmpty(heading))
            {
                heading = "Inventory - List of Major Parts onboard";
            }
            General.ShowExcel(heading, ds.Tables[0], alColumns, alCaptions, 1, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

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

            grid.MasterTableView.GetColumn("FLDISSHORTFALL").CurrentFilterValue = GetFilter("ISSHORTFALL");
            grid.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue = GetFilter("txtComponentNumber");
            grid.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue = GetFilter("txtComponentName");
            grid.MasterTableView.GetColumn("FLDMEASURE").CurrentFilterValue = GetFilter("txtName");

        }

        if (e.Item is GridDataItem)
        {

            RadLabel lblSMQ = (RadLabel)e.Item.FindControl("lblSMQ");
            RadLabel lblshortfallyn = (RadLabel)e.Item.FindControl("lblshortfallyn");

            if (lblshortfallyn.Text.Equals("1"))
            {
                lblSMQ.Attributes["style"] = "color:Red !important";
                lblSMQ.ToolTip = "Shortfalls";
                lblSMQ.Font.Bold = true;
            }
        }
    }

    protected string GetFilter(string filter)
    {
        string value = string.Empty;
        NameValueCollection nvc = Filter.CurrentOwnerReportInventoryFilter;
        if (nvc != null)
        {
            value = nvc[filter];
        }
        return value;
    }
    protected void SetFilter(string key, string value)
    {
        NameValueCollection nvc = Filter.CurrentOwnerReportInventoryFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection
            {
                { key, value }
            };
        }
        else
        {
            nvc[key] = value;
        }
        Filter.CurrentOwnerReportInventoryFilter = nvc;
    }


    protected void chkIsShortfall_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox chk = (RadCheckBox)sender;
        string iscritical = chk.Checked.HasValue && chk.Checked.Value ? "1" : string.Empty;
        NameValueCollection nvc = Filter.CurrentWorkOrderFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection
            {
                { "ISSHORTFALL", iscritical }
            };
        }
        else
        {
            nvc["ISSHORTFALL"] = iscritical;
        }
        Filter.CurrentOwnerReportInventoryFilter = nvc;
        GVR.DataSource = null;
        GVR.MasterTableView.Rebind();
    }

    protected void GVR_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper() == "FIND")
            {
                GridDataItem item = (GridDataItem)e.Item;

                NameValueCollection criteria = new NameValueCollection();

                criteria.Add("txtName", string.Empty);
                criteria.Add("txtComponentNumber", string.Empty);
                criteria.Add("txtComponentName", string.Empty);
                criteria.Add("ISSHORTFALL", string.Empty);

                Filter.CurrentOwnerReportInventoryFilter = criteria;
                GVR.Rebind();
            }

            if (e.CommandName == RadGrid.FilterCommandName)
            {
                NameValueCollection criteria = new NameValueCollection();

                criteria.Add("ISSHORTFALL", GVR.MasterTableView.GetColumn("FLDISSHORTFALL").CurrentFilterValue);
                criteria.Add("txtComponentNumber", GVR.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue);
                criteria.Add("txtComponentName", GVR.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue);
                criteria.Add("txtName", GVR.MasterTableView.GetColumn("FLDMEASURE").CurrentFilterValue);

                Filter.CurrentOwnerReportInventoryFilter = criteria;

                GVR.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void GVR_PreRender(object sender, EventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (!IsPostBack)
        {
            grid.MasterTableView.GetColumn("FLDISSHORTFALL").CurrentFilterValue = GetFilter("ISSHORTFALL");
            grid.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue = GetFilter("txtComponentNumber");
            grid.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue = GetFilter("txtComponentName");
            grid.MasterTableView.GetColumn("FLDMEASURE").CurrentFilterValue = GetFilter("txtName");

            grid.Rebind();
        }
    }
}