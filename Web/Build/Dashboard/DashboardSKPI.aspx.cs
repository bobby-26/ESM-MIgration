using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;

public partial class Inspection_InspectionPI : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardSKPI.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvPI')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('Add PI','Create PI','Dashboard/DashboardSKPIAdd.aspx','false','600px','350px')", "Add PI", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        toolbargrid.AddLinkButton("javascript:parent.openNewWindow('PI UNIT', 'PI UNIT', 'Dashboard/DashboardSKPIUnit.aspx', 'false', '600px', '350px'); return false; ", "PI Unit", "Toggle2", ToolBarDirection.Left);
        toolbargrid.AddLinkButton("javascript:parent.openNewWindow('PI SCOPE', 'PI SCOPE', 'Dashboard/DashboardSKPIScope.aspx', 'false', '600px', '350px'); return false;", "PI Scope", "Toggle3", ToolBarDirection.Left);
        TabstripMenu.MenuList = toolbargrid.Show();

        PhoenixToolbar kpitab = new PhoenixToolbar();
       
        kpitab.AddButton("PI", "Toggle4", ToolBarDirection.Left);
        kpitab.AddButton("KPI", "Toggle5", ToolBarDirection.Left);
        kpitab.AddButton("SPI", "Toggle1", ToolBarDirection.Left);
        kpitab.AddButton("SPI-KPI", "Toggle6", ToolBarDirection.Left);
        kpitab.AddButton("KPI-PI", "Toggle7", ToolBarDirection.Left);
        Tabkpi.MenuList = kpitab.Show();

        Tabkpi.SelectedMenuIndex = 0;
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ViewState["PICODE"] = string.Empty;
            ViewState["PINAME"] = string.Empty;


            gvPI.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDPICODE", "FLDPINAME" , "FLDUNIT", "FLDSCOPE" , "FLDPERIOD", "FLDDESCRIPTION" };
        string[] alCaptions = { "ID", "Name","Unit","Scope","Period","Description" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PheonixDashboardSKPI.PISearch(General.GetNullableString(ViewState["PICODE"].ToString()), General.GetNullableString(ViewState["PINAME"].ToString()), gvPI.CurrentPageIndex + 1,
                                                 gvPI.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Shipping PI.xls");
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

    protected void PI_TabStripMenuCommand(object sender, EventArgs e)
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
            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvPI_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PheonixDashboardSKPI.PISearch(General.GetNullableString(ViewState["PICODE"].ToString()), General.GetNullableString(ViewState["PINAME"].ToString()), gvPI.CurrentPageIndex + 1,
                                                gvPI.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        gvPI.DataSource = dt;
        gvPI.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDPICODE", "FLDPINAME", "FLDUNIT", "FLDSCOPE", "FLDPERIOD", "FLDDESCRIPTION" };
        string[] alCaptions = { "ID", "Name", "Unit", "Scope", "Period", "Description" };
        General.SetPrintOptions("gvPI", "PI scopes", alCaptions, alColumns, ds);
    }

    protected void gvPI_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? shippingpiid = General.GetNullableGuid(item.GetDataKeyValue("FLDPIID").ToString());

                LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','PI Edit','Dashboard/DashboardSKPIEdit.aspx?shippingpiid=" + shippingpiid + "','false','600px','350px');return false");

                }
                LinkButton colourconfig = ((LinkButton)item.FindControl("btncolor"));
                if (colourconfig != null)
                {
                    colourconfig.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','PI Colour Configuration','Dashboard/DashboardSKPIColourConfig.aspx?shippingpiid=" + shippingpiid + "','false','600px','380px');return false");
                }
            }

            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem filterItem = (GridFilteringItem)e.Item;
                TextBox textBox = (TextBox)filterItem["FLDPICODE"].Controls[0];
                textBox.MaxLength = 10;

                TextBox textBox1 = (TextBox)filterItem["FLDPINAME"].Controls[0];
                textBox1.MaxLength = 150;

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    public void gvPI_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.FilterCommandName)
        {
            ViewState["PICODE"] = gvPI.MasterTableView.GetColumn("FLDPICODE").CurrentFilterValue;
            ViewState["PINAME"] = gvPI.MasterTableView.GetColumn("FLDPINAME").CurrentFilterValue;
           

            gvPI.Rebind();

        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvPI.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}