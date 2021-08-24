using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;

public partial class Dashboard_DashboardSKOVMSA : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardSKOVMSA.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTMSA')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('e','Add TMSA Element','Dashboard/DashboardSKOVMSAAdd.aspx','false','550px','250px')", "Add TMSA Element", "<i class=\"fas fa-plus-circle\"></i>", "ADD");

        TabStrip.MenuList = toolbargrid.Show();
        TabStrip.SelectedMenuIndex = 0;
        PhoenixToolbar tmsatab = new PhoenixToolbar();

        tmsatab.AddButton("TMSA", "Toggle4", ToolBarDirection.Left);
        tmsatab.AddButton("TMSA-LI", "Toggle5", ToolBarDirection.Left);
       
        Tabkpi.MenuList = tmsatab.Show();

        Tabkpi.SelectedMenuIndex = 0;
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);



            gvTMSA.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }

    protected void TabStrip_TabStripCommand(object sender, EventArgs e)
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

        DataTable dt = PhoemixDashboardSKOVMSA.TMSAElementSearch(gvTMSA.CurrentPageIndex + 1,
                                                gvTMSA.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        string[] alColumns = { "FLDTMSACODE", "FLDTMSASHORTCODE", "FLDTMSADESCRIPTION" };
        string[] alCaptions = { "ID", "Shot Code", "Description" };

        General.ShowExcel("OVMSA/TMSA Elements", dt, alColumns, alCaptions, null, null);


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

    protected void gvTMSA_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoemixDashboardSKOVMSA.TMSAElementSearch(gvTMSA.CurrentPageIndex + 1,
                                                gvTMSA.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        gvTMSA.DataSource = dt;
        gvTMSA.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDTMSACODE", "FLDTMSASHORTCODE", "FLDTMSADESCRIPTION" };
        string[] alCaptions = { "ID", "Shot Code", "Description" };
        General.SetPrintOptions("gvTMSA", "OVMSA/TMSA Elements", alCaptions, alColumns, ds);
    }

    protected void gvTMSA_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? tmsaid = General.GetNullableGuid(item.GetDataKeyValue("FLDTMSAID").ToString());

                LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Edit TMSA Element','Dashboard/DashboardSKOVMSAEdit.aspx?tmsaid=" + tmsaid + "','false','550px','250px');return false");

                }
               
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvTMSA_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvTMSA.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}