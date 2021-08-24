using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;

public partial class InspectionShippingSPI : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardSKSPI.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSPIlist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('Add SPI','','Dashboard/DashboardSKSPIAdd.aspx','false','600px','360px')", "Add SPI", "<i class=\"fas fa-plus-circle\"></i>", "ADD");

        TabstripMenu.MenuList = toolbargrid.Show();

        PhoenixToolbar kpitab = new PhoenixToolbar();
        kpitab.AddButton("PI", "Toggle4", ToolBarDirection.Left);    
        kpitab.AddButton("KPI", "Toggle5", ToolBarDirection.Left);
        kpitab.AddButton("SPI", "Toggle1", ToolBarDirection.Left);
        kpitab.AddButton("SPI-KPI", "Toggle6", ToolBarDirection.Left);
        kpitab.AddButton("KPI-PI", "Toggle7", ToolBarDirection.Left);
        Tabkpi.MenuList = kpitab.Show();

        Tabkpi.SelectedMenuIndex = 2;
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["PAGENUMBER"] = 1;

            gvSPIlist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }


    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSPIID", "FLDSHORTCODE", "FLDSPITITLE", "FLDDESCRIPTION"};
        string[] alCaptions = { "ID", "Short Code", "Title", "Description"};

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PheonixDashboardSKSPI.SPISearch(gvSPIlist.CurrentPageIndex + 1,
                                                 gvSPIlist.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Shipping SPIs.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Drill Register</h3></td>");
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

    protected void gvSPIlist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PheonixDashboardSKSPI.SPISearch(gvSPIlist.CurrentPageIndex + 1,
                                                gvSPIlist.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        gvSPIlist.DataSource = dt;
        gvSPIlist.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDSPIID", "FLDSHORTCODE", "FLDSPITITLE", "FLDDESCRIPTION" };
        string[] alCaptions = { "ID", "Short Code", "Title", "Description" };
        General.SetPrintOptions("gvSPIlist", "Shipping Performance Indicators ", alCaptions, alColumns, ds);
    }

    protected void gvSPIlist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? shippingspiid = General.GetNullableGuid(item.GetDataKeyValue("FLDSHIPPINGSPIID").ToString());
               
                LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','SPI Edit','Dashboard/DashboardSKSPIEdit.aspx?shippingspiid=" + shippingspiid + "','false','600px','360px');return false");

                }

                LinkButton colourconfig = ((LinkButton)item.FindControl("btncolor"));
                if (colourconfig != null)
                {
                    colourconfig.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','SPI Colour Configuration','Dashboard/DashboardSKSPIColourConfig.aspx?shippingspiid=" + shippingspiid + "','false','600px','380px');return false");


                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    public void gvSPIlist_ItemCommand(object sender, GridCommandEventArgs e)
    {
          
          
    }

    

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvSPIlist.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}