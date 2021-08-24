using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Dashboard;
using System.Collections.Specialized;

public partial class InventoryComponentDetailsSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryComponentDetailsSearch.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvComponents')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:expandcollapse('gvComponents');", "Expand/Collapse", "<i class=\"fas fa-expand-alt\"></i>", "EXPANDCOLLAPSE");
            MenuWorkOrder.Title = "Components";
            MenuWorkOrder.AccessRights = this.ViewState;
            MenuWorkOrder.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarJobs = new PhoenixToolbar();
            toolbarJobs.AddFontAwesomeButton("../Inventory/InventoryComponentDetailsSearch.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarJobs.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarJobs.AddFontAwesomeButton("javascript:expandcollapse('gvWorkOrder');", "Expand/Collapse", "<i class=\"fas fa-expand-alt\"></i>", "EXPANDCOLLAPSE");
            MenuJobs.Title = "Maintenance Due";
            MenuJobs.AccessRights = this.ViewState;
            MenuJobs.MenuList = toolbarJobs.Show();

            PhoenixToolbar toolbarspare = new PhoenixToolbar();
            toolbarspare.AddFontAwesomeButton("../Inventory/InventoryComponentDetailsSearch.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarspare.AddFontAwesomeButton("javascript:CallPrint('gvSpare')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarspare.AddFontAwesomeButton("javascript:expandcollapse('gvSpare');", "Expand/Collapse", "<i class=\"fas fa-expand-alt\"></i>", "EXPANDCOLLAPSE");
            MenuSpare.Title = "Spares";
            MenuSpare.AccessRights = this.ViewState;
            MenuSpare.MenuList = toolbarspare.Show();
            

            if (!IsPostBack)
            {
                ViewState["cpname"] = Request.QueryString["cpname"] != null ? Request.QueryString["cpname"].ToString() : "";
                ViewState["COMPID"] = "";
                ViewState["COMPNO"] = "";
                gvComponents.PageSize = General.ShowRecords(null);
                gvWorkOrder.PageSize = General.ShowRecords(null);
                gvSpare.PageSize = General.ShowRecords(null);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponents_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            if(drv["FLDISCRITICAL"].ToString() == "1")
            {
                item["FLDCOMPONENTNAME"].ForeColor = System.Drawing.Color.Red;

            }
        }
    }

    protected void gvComponents_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCATEGORYNAME", "FLDSERIALNUMBER", "FLDCOMPONENTSTATUSNAME", "FLDTYPE", "FLDCLASSCODE" };
            string[] alCaptions = { "Number", "Name", "Category", "Serial Number", "Status", "Type", "Class Code" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = new DataSet();
            int? status = null;

            ds = PhoenixCommonInventory.ComponentSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , null
                    , "%" + (General.GetNullableString(ViewState["cpname"].ToString()) != null ? ViewState["cpname"].ToString() : "")
                    , null
                    , null
                    , null
                    , status
                    , null
                    , null
                    , null
                    , null
                    , null
                    , sortexpression, sortdirection,
                       gvComponents.CurrentPageIndex + 1,
                        gvComponents.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount); ;


            General.SetPrintOptions("gvComponents", "Component", alCaptions, alColumns, ds);

            gvComponents.DataSource = ds;
            gvComponents.VirtualItemCount = iRowCount;

        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void gvComponents_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        ViewState["SORTDIRECTION"] = ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0" ? "1" : "0";
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME","FLDCATEGORYNAME", "FLDSERIALNUMBER", "FLDCOMPONENTSTATUSNAME", "FLDTYPE", "FLDCLASSCODE" };
            string[] alCaptions = { "Number", "Name","Category", "Serial Number", "Status", "Type", "Class Code" };
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
            int? status = null;
            if (Filter.CurrentComponentFilterCriteria == null)
                status = 35;
            ds = PhoenixCommonInventory.ComponentSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                , null
                                 , "%" + (General.GetNullableString(ViewState["cpname"].ToString()) != null ? ViewState["cpname"].ToString() : "")
                                , null
                                , null
                                , null
                                , status
                                , null
                                , null
                                , null
                                , null
                                , null
                                , sortexpression, sortdirection,
                                   1,
                                    gvComponents.VirtualItemCount,
                                    ref iRowCount,
                                    ref iTotalPageCount);

            General.ShowExcel("Component", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuJobs_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            JShowExcel();
        }
    }
    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton lnkTitle = (LinkButton)e.Item.FindControl("lnktitle");
            RadLabel lblComponentJobID = (RadLabel)item["FLDWORKORDERNAME"].FindControl("lblComponentJobID");
            RadLabel lblComponentID = (RadLabel)item["FLDWORKORDERNAME"].FindControl("lblComponentID");

            if (lnkTitle != null)
            {
                lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?tv=1&COMPONENTJOBID=" + lblComponentJobID.Text + "&COMPONENTID=" + lblComponentID.Text + "&hierarchy=1&Cancelledjob=0','','1200','600');return false");
            }
        }
    }

    protected void gvWorkOrder_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["JSORTEXPRESSION"] = e.SortExpression;
        ViewState["JSORTDIRECTION"] = ViewState["JSORTDIRECTION"] != null && ViewState["JSORTDIRECTION"].ToString() == "0" ? "1" : "0";
    
        
    }

    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCATEGORY", "FLDFREQUENCYNAME", "FLDDISCIPLINENAME", "FLDDUEDATE", "FLDLASTDONEDATE", "FLDWORKORDERSTATUS", "FLDWORKORDERGROUPNO" };
            string[] alCaptions = { "Job Code & Title", "Component Number", "Component Name", "Category", "Frequency", "Responsibility", "Due Date", "Last Done On", "Status", "Workorder" };

            string sortexpression = (ViewState["JSORTEXPRESSION"] == null) ? null : (ViewState["JSORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["JSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["JSORTDIRECTION"].ToString());

            DataSet ds = PhoenixDashboardTechnical.DashboardJobCategoryComponentWorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , ""
                    , null
                    , null
                    , null
                    , null
                    , General.GetNullableGuid(ViewState["COMPID"].ToString()) != null ?
                        ((General.GetNullableString(ViewState["COMPNO"].ToString()) != null ? ViewState["COMPNO"].ToString() : "")) : null
                    , General.GetNullableGuid(ViewState["COMPID"].ToString())==null ? 
                        ("%" +( General.GetNullableString(ViewState["cpname"].ToString()) != null ? ViewState["cpname"].ToString() : "")):null
                    ,null
                    , null
                    , null
                    , null
                    , sortexpression, sortdirection
                    , gvWorkOrder.CurrentPageIndex + 1
                    , gvWorkOrder.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , null
                    , null
                    , null
                    , null
                    , null);

            General.SetPrintOptions("gvWorkOrder", "Maintenance Due", alCaptions, alColumns, ds);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void JShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCATEGORY", "FLDFREQUENCYNAME", "FLDDISCIPLINENAME", "FLDDUEDATE", "FLDLASTDONEDATE", "FLDWORKORDERSTATUS", "FLDWORKORDERGROUPNO" };
        string[] alCaptions = { "Job Code & Title", "Component Number", "Component Name", "Category", "Frequency", "Responsibility", "Due Date", "Last Done On", "Status", "Workorder" };

        string sortexpression = (ViewState["JSORTEXPRESSION"] == null) ? null : (ViewState["JSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["JSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["JSORTDIRECTION"].ToString());

        DataSet ds = PhoenixDashboardTechnical.DashboardJobCategoryComponentWorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , ""
                    , null
                    , null
                    , null
                    , null
                   , General.GetNullableGuid(ViewState["COMPID"].ToString()) != null ?
                        ((General.GetNullableString(ViewState["COMPNO"].ToString()) != null ? ViewState["COMPNO"].ToString() : "")) : null
                    , General.GetNullableGuid(ViewState["COMPID"].ToString()) == null ?
                        ("%" + (General.GetNullableString(ViewState["cpname"].ToString()) != null ? ViewState["cpname"].ToString() : "")) : null
                    , null
                    , null
                    , null
                    , null
                    , sortexpression, sortdirection
                    , 1
                    , gvWorkOrder.VirtualItemCount
                    , ref iRowCount
                    , ref iTotalPageCount
                    , null
                    , null
                    , null
                    , null
                    , null);

        General.ShowExcel("Maintenance Due", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuSpare_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            SShowExcel();
        }
    }

    protected void gvSpare_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvSpare_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDNUMBER", "FLDNAME", "FLDMAKER", "FLDUNITNAME", "FLDROB"};
            string[] alCaptions = { "Component Number", "Number", "Name", "Maker", "Unit", "ROB"};

            string sortexpression = (ViewState["SSORTEXPRESSION"] == null) ? null : (ViewState["SSORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SSORTDIRECTION"].ToString());

            DataSet ds = PhoenixDashboardTechnical.GetSpareitemsByComponent(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableGuid(ViewState["COMPID"].ToString()) == null ? General.GetNullableString(ViewState["cpname"].ToString()) : null
                    , sortexpression, sortdirection
                    , gvSpare.CurrentPageIndex + 1
                    , gvSpare.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , General.GetNullableGuid(ViewState["COMPID"].ToString())
                    );

            General.SetPrintOptions("gvSpare", "Spares", alCaptions, alColumns, ds);

            gvSpare.DataSource = ds;
            gvSpare.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDNUMBER", "FLDNAME", "FLDMAKER", "FLDUNITNAME", "FLDROB" };
            string[] alCaptions = { "Component Number", "Number", "Name", "Maker", "Unit", "ROB" };

            string sortexpression = (ViewState["SSORTEXPRESSION"] == null) ? null : (ViewState["SSORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SSORTDIRECTION"].ToString());

            DataSet ds = PhoenixDashboardTechnical.GetSpareitemsByComponent(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableGuid(ViewState["COMPID"].ToString()) == null ? General.GetNullableString(ViewState["cpname"].ToString()) : null
                    , sortexpression, sortdirection
                    , 1
                    , gvSpare.VirtualItemCount
                    , ref iRowCount
                    , ref iTotalPageCount
                    , General.GetNullableGuid(ViewState["COMPID"].ToString())
                    );

            General.ShowExcel("Spares", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvSpare_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SSORTEXPRESSION"] = e.SortExpression;
        ViewState["SSORTDIRECTION"] = ViewState["SSORTDIRECTION"] != null && ViewState["SSORTDIRECTION"].ToString() == "0" ? "1" : "0";
    }

    protected void gvComponents_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if(e.CommandName == "RowClick" || e.CommandName.ToUpper() == "SELECT")
        {
            ViewState["cpname"] = "";
            ViewState["COMPID"] = "";
            GridDataItem item = (GridDataItem)e.Item;

            if (item["FLDCOMPONENTNAME"] != null)
                ViewState["cpname"] = item["FLDCOMPONENTNAME"].Text;
            if (General.GetNullableGuid(item.GetDataKeyValue("FLDCOMPONENTID").ToString()) != null)
                ViewState["COMPID"] = item.GetDataKeyValue("FLDCOMPONENTID").ToString();
            if (item["FLDCOMPONENTNUMER"] != null)
                ViewState["COMPNO"] = item["FLDCOMPONENTNUMER"].Text;

            gvWorkOrder.Rebind();
            gvSpare.Rebind();
        }
    }

    protected void gvSpare_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            GridDataItem item = (GridDataItem)e.Item;


            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtNumber", item["FLDNUMBER"].Text);
            criteria.Add("txtName", "");
            criteria.Add("txtMakerid", "");
            criteria.Add("txtVendorId", "");
            criteria.Add("isGolbleSearch","");
            criteria.Add("chkCritical", "");
            criteria.Add("txtMakerReference","");
            criteria.Add("txtDrawing","");
            criteria.Add("chkROB", "");
            criteria.Add("txtComponentNumber", "");
            criteria.Add("txtComponentName", "");

            Filter.CurrentSpareItemFilterCriteria = criteria;

            string url = "";
            url = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inventory/InventorySpareItem.aspx');");

            //lnkvessel.Attributes.Add("onclick", url);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", url, true);
        }
    }
}