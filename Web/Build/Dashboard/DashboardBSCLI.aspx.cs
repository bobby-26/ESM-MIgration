using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;

public partial class Dashboard_DashboardBSCLI : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardBSCLI.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvLI')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('Add LI','Create LI','Dashboard/DashboardBSCLIAdd.aspx','false','800px','490px')", "Add LI", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        toolbargrid.AddLinkButton("javascript:parent.openNewWindow('LI UNIT', 'LI UNIT', 'Dashboard/DashboardBSCLIUnit.aspx', 'false', '600px', '350px'); return false; ", "LI Unit", "Toggle2", ToolBarDirection.Left);

        TabstripMenu.MenuList = toolbargrid.Show();

        PhoenixToolbar kpitab = new PhoenixToolbar();
        kpitab.AddButton("LI", "Toggle1", ToolBarDirection.Left);
        kpitab.AddButton("PI-LI", "Toggle3", ToolBarDirection.Left);
        Tabli.MenuList = kpitab.Show();

        Tabli.SelectedMenuIndex = 0;
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            gvLI.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }

    protected void ShowExcel()
    {

    }

    protected void LI_TabStripMenuCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("TOGGLE2"))
            {
                Response.Redirect("../Dashboard/DashboardBSCLIUnit.aspx");

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

    protected void gvLI_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoenixDashboardBSCLI.LISearch(gvLI.CurrentPageIndex + 1,
                                                gvLI.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        gvLI.DataSource = dt;
        gvLI.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDLICODE", "FLDLINAME", "FLDUNIT", "FLDLISCOPE", "FLDFREQUENCY","FLDFREQUENCYTYPE", "FLDDESCRIPTION" };
        string[] alCaptions = { "ID", "Name", "Unit", "Scope", "Frequency","Frequency Type", "Description" };
        General.SetPrintOptions("gvLI", "Leading Indicators(LI)", alCaptions, alColumns, ds);
    }

    protected void gvLI_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? shippingliid = General.GetNullableGuid(item.GetDataKeyValue("FLDLIID").ToString());

                LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','LI Edit','Dashboard/DashboardBSCLIEdit.aspx?shippingliid=" + shippingliid + "','false','800px','490px');return false");

                }
                LinkButton colourconfig = ((LinkButton)item.FindControl("btncolor"));
                if (colourconfig != null)
                {
                    colourconfig.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','LI Colour Configuration','Dashboard/DashboardBSCLIColourConfig.aspx?shippingliid=" + shippingliid + "','false','600px','380px');return false");


                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    protected void gvLI_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvLI.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}