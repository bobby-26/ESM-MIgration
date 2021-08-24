using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;

public partial class Dashboard_DashboardBSCPILILink : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardSKSPIKPIMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvPILIlist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('Add PI-LI Mapping','Create PI-LI Mapping','Dashboard/DashboardBSCPILIMappingAdd.aspx','false','400px','330px')", "Add PI-LI Mapping", "<i class=\"fas fa-plus-circle\"></i>", "ADD");

        TabstripMenu.MenuList = toolbargrid.Show();

        PhoenixToolbar kpitab = new PhoenixToolbar();
        kpitab.AddButton("LI", "Toggle1", ToolBarDirection.Left);
        kpitab.AddButton("PI-LI", "Toggle3", ToolBarDirection.Left);
        Tabkpi.MenuList = kpitab.Show();

        Tabkpi.SelectedMenuIndex = 1;
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);


            gvPILIlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }


    }

    protected void ShowExcel()
    {
        string[] alColumns = { "FLDPICODE", "FLDPINAME", "FLDLICODE", "FLDLINAME" };
        string[] alCaptions = { "PI Code", "PI Name", "LI Code", "LI Name" };
    }

    protected void SPI_TabStripMenuCommand(object sender, EventArgs e)
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
    protected void LI_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("TOGGLE1"))
            {

                Response.Redirect("../Dashboard/DashboardBSCLI.aspx");

            }


            if (CommandName.ToUpper().Equals("TOGGLE3"))
            {
                Response.Redirect("../Dashboard/DashboardBSCPILILink.aspx");

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvPILIlist_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoenixDashboardBSCLI.PILILinkSearch(gvPILIlist.CurrentPageIndex + 1,
                                                gvPILIlist.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        gvPILIlist.DataSource = dt;
        gvPILIlist.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDPICODE","FLDPINAME","FLDLICODE", "FLDLINAME" };
        string[] alCaptions = { "PI Code","PI Name","LI Code", "LI Name" };
        General.SetPrintOptions("gvPILIlist", "PI - LI Mapping", alCaptions, alColumns, ds);
    }

    protected void gvPILIlist_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? shippingpililinkid = General.GetNullableGuid(item.GetDataKeyValue("FLDPI2LILINKID").ToString());

                LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Edit PI-LI Mapping','Dashboard/DashboardBSCPILIMappingEdit.aspx?shippingpililinkid=" + shippingpililinkid + "','false','400px','330px');return false");

                }



            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvPILIlist_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvPILIlist.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}