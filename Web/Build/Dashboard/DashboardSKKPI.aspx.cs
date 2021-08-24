using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

public partial class Inspection_InspectionShippingKPI : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardSKKPI.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvKPI')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('Add KPI','Create KPI','Dashboard/DashboardSKKPIAdd.aspx','false','600px','400px')", "Add KPI", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        toolbargrid.AddLinkButton("javascript:parent.openNewWindow('KPI UNIT', 'KPI UNIT', 'Dashboard/DashboardSKKPIUnit.aspx', 'false', '600px', '350px'); return false; ", "KPI Unit", "Toggle2", ToolBarDirection.Left);
        toolbargrid.AddLinkButton("javascript:parent.openNewWindow('KPI SCOPE', 'KPI SCOPE', 'Dashboard/DashboardSKKPIScope.aspx', 'false', '600px', '350px'); return false;", "KPI Scope", "Toggle3", ToolBarDirection.Left);
        TabstripMenu.MenuList = toolbargrid.Show();

        PhoenixToolbar kpitab = new PhoenixToolbar();
      
        kpitab.AddButton("PI", "Toggle4", ToolBarDirection.Left);
        kpitab.AddButton("KPI", "Toggle5", ToolBarDirection.Left);
        kpitab.AddButton("SPI", "Toggle1", ToolBarDirection.Left);
        kpitab.AddButton("SPI-KPI", "Toggle6", ToolBarDirection.Left);
        kpitab.AddButton("KPI-PI", "Toggle7", ToolBarDirection.Left);
        Tabkpi.MenuList = kpitab.Show();
        Tabkpi.SelectedMenuIndex = 1;

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ViewState["KPICODE"] = string.Empty;
            ViewState["KPINAME"] = string.Empty;
            gvKPI.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["DEPARTMENT"] = string.Empty;

        }
    }

    

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDKPICODE", "FLDKPINAME", "FLDUNIT", "FLDSCOPE", "FLDPERIOD", "FLDKPIMINVALUE", "FLDKPITARGETVALUE", "FLDKPIDESCRIPTION" };
        string[] alCaptions = { "Code", "Name", "Unit", "Scope", "Period", "KPI Minimum value", "KPI Target value", " Description" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PheonixDashboardSKKPI.KPISearch(General.GetNullableString(ViewState["KPICODE"].ToString()), General.GetNullableString(ViewState["KPINAME"].ToString()), General.GetNullableInteger(ViewState["DEPARTMENT"].ToString()), gvKPI.CurrentPageIndex + 1,
                                                 gvKPI.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Shipping KPI.xls");
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

    protected void KPI1_TabStripMenuCommand(object sender, EventArgs e)
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

    protected void gvKPI_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PheonixDashboardSKKPI.KPISearch(General.GetNullableString(ViewState["KPICODE"].ToString()),  General.GetNullableString(ViewState["KPINAME"].ToString()),General.GetNullableInteger(ViewState["DEPARTMENT"].ToString()), gvKPI.CurrentPageIndex + 1,
                                                gvKPI.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        gvKPI.DataSource = dt;
        gvKPI.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDKPICODE", "FLDKPINAME", "FLDUNIT", "FLDSCOPE", "FLDPERIOD",  "FLDKPIMINVALUE", "FLDKPITARGETVALUE", "FLDKPIDESCRIPTION" };
        string[] alCaptions = { "Code", "Name","Unit" ,"Scope","Period","KPI Minimum value", "KPI Target value", " Description" };
        General.SetPrintOptions("gvKPI", "KPI ", alCaptions, alColumns, ds);
    }

    protected void gvKPI_ItemDataBound(object sender, GridItemEventArgs e)
    {

        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? shippingkpiid = General.GetNullableGuid(item.GetDataKeyValue("FLDKPIID").ToString());

                LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Edit Key Performance Indicator (PI)','Dashboard/DashboardSKKPIEdit.aspx?shippingkpiid=" + shippingkpiid + "','false','600px','400px');return false");

                }
                LinkButton colourconfig = ((LinkButton)item.FindControl("btncolor"));
                if (colourconfig != null)
                {
                    colourconfig.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','KPI Colour Configuration','Dashboard/DashboardSKKPIColourConfig.aspx?shippingkpiid=" + shippingkpiid + "','false','600px','380px');return false");


                }
               
            }

            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem filterItem = (GridFilteringItem)e.Item;
                TextBox textBox = (TextBox)filterItem["FLDKPICODE"].Controls[0];
                textBox.MaxLength = 10;

                TextBox textBox1 = (TextBox)filterItem["FLDKPINAME"].Controls[0];
                textBox1.MaxLength = 150;

                RadComboBox radcbdept = (RadComboBox)filterItem.FindControl("radcbdept"); 

                DataTable dt1 = PheonixDashboardSKKPI.Departmentlist();

                radcbdept.DataSource = dt1;
                radcbdept.DataTextField = "FLDDEPARTMENTNAME";
                radcbdept.DataValueField = "FLDDEPARTMENTID";
                radcbdept.DataBind();

            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    public void gvKPI_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.FilterCommandName)
        {
            ViewState["KPICODE"] = gvKPI.MasterTableView.GetColumn("FLDKPICODE").CurrentFilterValue;
            ViewState["KPINAME"] = gvKPI.MasterTableView.GetColumn("FLDKPINAME").CurrentFilterValue;
            ViewState["DEPARTMENT"] = gvKPI.MasterTableView.GetColumn("FLDDEPARTMENT").CurrentFilterValue;

            gvKPI.Rebind();

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvKPI.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void radcbdept_TextChanged(object sender, EventArgs e)
    {
        RadComboBox ddl = (RadComboBox)sender;

        ViewState["KPICODE"] = gvKPI.MasterTableView.GetColumn("FLDKPICODE").CurrentFilterValue;
        ViewState["KPINAME"] = gvKPI.MasterTableView.GetColumn("FLDKPINAME").CurrentFilterValue;
        ViewState["DEPARTMENT"] = ddl.SelectedValue.ToString();;
        gvKPI.DataSource = null;
        gvKPI.Rebind();
    }
}