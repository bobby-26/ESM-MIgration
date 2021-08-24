using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;


public partial class Dashboard_DashboardTMSALIMapping : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardTMSALIMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTMSALIlist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('e','Add TMSA Element - LI Mapping','Dashboard/DashboardTMSALIMappingAdd.aspx','false','550px','250px')", "Add TMSA Element", "<i class=\"fas fa-plus-circle\"></i>", "ADD");

        TabstripMenu.MenuList = toolbargrid.Show();
 
        PhoenixToolbar tmsatab = new PhoenixToolbar();

        tmsatab.AddButton("TMSA", "Toggle4", ToolBarDirection.Left);
        tmsatab.AddButton("TMSA-LI", "Toggle5", ToolBarDirection.Left);

        Tabkpi.MenuList = tmsatab.Show();

        Tabkpi.SelectedMenuIndex = 1;
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);



            gvTMSALIlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void TabstripMenu_TabStripCommand(object sender, EventArgs e)
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoemixDashboardSKOVMSA.TMSAElementLIMappingSearch(gvTMSALIlist.CurrentPageIndex + 1,
                                                gvTMSALIlist.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        string[] alColumns = { "FLDTMSACODE", "FLDTMSADESCRIPTION", "FLDLICODE", "FLDLINAME" };
        string[] alCaptions = { "TMSA ID", " TMSA Description", "LI Code", "LI Name" };
        General.ShowExcel("OVMSA/TMSA Elements - LI Mapping", dt, alColumns, alCaptions, null, null);


    }

    protected void Tabkpi_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("TOGGLE4"))
            {
                Response.Redirect("../Dashboard/DashboardSKOVMSA.aspx");
            }
            if (CommandName.ToUpper().Equals("TOGGLE5"))
            {
                Response.Redirect("../Dashboard/DashboardTMSALIMapping.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvTMSALIlist_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoemixDashboardSKOVMSA.TMSAElementLIMappingSearch(gvTMSALIlist.CurrentPageIndex + 1,
                                                gvTMSALIlist.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        gvTMSALIlist.DataSource = dt;
        gvTMSALIlist.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDTMSACODE", "FLDTMSADESCRIPTION", "FLDLICODE", "FLDLINAME" };
        string[] alCaptions = { "TMSA ID",  " TMSA Description" , "LI Code","LI Name" };
        General.SetPrintOptions("gvTMSALIlist", "OVMSA/TMSA Elements - LI Mapping", alCaptions, alColumns, ds);
    }

    protected void gvTMSALIlist_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? tmsalilinkid = General.GetNullableGuid(item.GetDataKeyValue("FLDELEMENT2LINKID").ToString());

                LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Edit TMSA ELement-LI Mapping','Dashboard/DashboardTMSALIMappingEdit.aspx?tmsalilinkid=" + tmsalilinkid + "','false','550px','250px');return false");

                }



            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvTMSALIlist_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvTMSALIlist.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}