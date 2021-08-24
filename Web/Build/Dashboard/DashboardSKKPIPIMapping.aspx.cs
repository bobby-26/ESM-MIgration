using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

public partial class Inspection_InspectionShippingKPIPIMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardSKSPIKPIMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvKPIPIlist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('Add KPI-PI Mapping','Create KPI-PI Mapping','Dashboard/DashboardSKKPIPIMappingAdd.aspx','false','400px','330px')", "Add KPI-PI Mapping", "<i class=\"fas fa-plus-circle\"></i>", "ADD");

        TabstripMenu.MenuList = toolbargrid.Show();
        PhoenixToolbar kpitab = new PhoenixToolbar();
        
        kpitab.AddButton("PI", "Toggle4", ToolBarDirection.Left);
        kpitab.AddButton("KPI", "Toggle5", ToolBarDirection.Left);
        kpitab.AddButton("SPI", "Toggle1", ToolBarDirection.Left);
        kpitab.AddButton("SPI-KPI", "Toggle6", ToolBarDirection.Left);
        kpitab.AddButton("KPI-PI", "Toggle7", ToolBarDirection.Left);
        Tabkpi.MenuList = kpitab.Show();
        Tabkpi.SelectedMenuIndex = 4;

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["PAGENUMBER"] = 1;

            gvKPIPIlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDKPINAME", "FLDPINAME" };
        string[] alCaptions = { "KPI", "PI" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PheonixDashboardSKSPI.KPIPILinkSearch(gvKPIPIlist.CurrentPageIndex + 1,
                                                 gvKPIPIlist.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Shipping KPI PI Mapping.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Shipping KPI PI Mapping</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();

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

    protected void KPI_TabStripMenuCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("TOGGLE1"))
            {

                Response.Redirect("../Dashboard/DashboardSKSPI.aspx");

            }

            if (CommandName.ToUpper().Equals("TOGGLE2"))
            {
                Response.Redirect("../Dashboard/DashboardSKPIUnit.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE3"))
            {
                Response.Redirect("../Dashboard/DashboardSKPIScope.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE4"))
            {
                Response.Redirect("../Dashboard/DashboardSKPI.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE5"))
            {
                Response.Redirect("../Dashboard/DashboardSKKPI.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE6"))
            {
                Response.Redirect("../Dashboard/DashboardSKSPIKPIMapping.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE7"))
            {
                Response.Redirect("../Dashboard/DashboardSKKPIPIMapping.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE8"))
            {
                Response.Redirect("../Dashboard/DashboardSKKPIUnit.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE9"))
            {
                Response.Redirect("../Dashboard/DashboardSKKPIScope.aspx");

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvKPIPIlist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PheonixDashboardSKSPI.KPIPILinkSearch(gvKPIPIlist.CurrentPageIndex + 1,
                                                gvKPIPIlist.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        gvKPIPIlist.DataSource = dt;
        gvKPIPIlist.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDKPINAME", "FLDPINAME" };
        string[] alCaptions = { "KPI", "PI" };
        General.SetPrintOptions("gvKPIPIlist", "KPI - PI Mapping", alCaptions, alColumns, ds);
    }

    protected void gvKPIPIlist_ItemDataBound(object sender, GridItemEventArgs e)
    {

        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? shippingkpipilinkid = General.GetNullableGuid(item.GetDataKeyValue("FLDKPI2PILINKID").ToString());

                LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Edit KPI-PI Mapping','Dashboard/DashboardSKKPIPIMappingEdit.aspx?shippingkpipilinkid=" + shippingkpipilinkid + "','false','400px','330px');return false");

                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    public void gvKPIPIlist_ItemCommand(object sender, GridCommandEventArgs e)
    {
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvKPIPIlist.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvKPIPIlist_PreRender(object sender, EventArgs e)
    {
        for (int rowIndex = gvKPIPIlist.Items.Count - 2; rowIndex >= 0; rowIndex--)
        {
            GridDataItem row = gvKPIPIlist.Items[rowIndex];
            GridDataItem previousRow = gvKPIPIlist.Items[rowIndex + 1];

            RadLabel currentCategoryName = ((RadLabel)gvKPIPIlist.Items[rowIndex].FindControl("RadlblKpicode"));
            RadLabel previousCategoryName = ((RadLabel)gvKPIPIlist.Items[rowIndex + 1].FindControl("RadlblKpicode"));
            RadLabel currentRadlblspititle = ((RadLabel)gvKPIPIlist.Items[rowIndex].FindControl("Radlblkpititle"));
            RadLabel previousRadlblspititle = ((RadLabel)gvKPIPIlist.Items[rowIndex + 1].FindControl("Radlblkpititle"));

            if (currentCategoryName.Text == previousCategoryName.Text)
            {
                row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                    previousRow.Cells[1].RowSpan + 1;
                previousCategoryName.Visible = false;
                previousRadlblspititle.Visible = false;
            }

        }
    }
}