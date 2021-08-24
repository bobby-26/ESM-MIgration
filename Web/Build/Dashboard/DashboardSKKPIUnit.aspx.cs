using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;

public partial class Inspection_InspectionShippingKPIUnit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardSKKPIUnit.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvKPIUnit')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        //toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('Add KPI Unit','Create KPI Unit','Dashboard/DashboardSKKPIUnitAdd.aspx','false','600px','200px')", "Add KPI Unit", "<i class=\"fas fa-plus-circle\"></i>", "ADD");

        TabstripMenu.MenuList = toolbargrid.Show();

        //PhoenixToolbar kpitab = new PhoenixToolbar();
        //kpitab.AddButton("SPI", "Toggle1", ToolBarDirection.Left);
        //kpitab.AddButton("PI", "Toggle4", ToolBarDirection.Left);
        //kpitab.AddButton("PI Unit", "Toggle2", ToolBarDirection.Left);
        //kpitab.AddButton("PI Scope", "Toggle3", ToolBarDirection.Left);
        //kpitab.AddButton("KPI", "Toggle5", ToolBarDirection.Left);
        //kpitab.AddButton("KPI Unit", "Toggle8", ToolBarDirection.Left);
        //kpitab.AddButton("KPI Scope", "Toggle9", ToolBarDirection.Left);
        //kpitab.AddButton("SPI-KPI", "Toggle6", ToolBarDirection.Left);
        //kpitab.AddButton("KPI-PI", "Toggle7", ToolBarDirection.Left);
        //Tabkpi.MenuList = kpitab.Show();
        //Tabkpi.SelectedMenuIndex = 5;
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);


            gvKPIUnit.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDUNITCODE", "FLDUNIT" };
        string[] alCaptions = { "Unit Code", "Unit" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PheonixDashboardSKKPI.KPIUnitSearch(gvKPIUnit.CurrentPageIndex + 1,
                                                 gvKPIUnit.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Shipping KPI Units.xls");
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
    protected void PIUnit_TabStripMenuCommand(object sender, EventArgs e)
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
    //protected void KPI_TabStripMenuCommand(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

    //        if (CommandName.ToUpper().Equals("TOGGLE1"))
    //        {

    //            Response.Redirect("../Dashboard/DashboardSKSPI.aspx");

    //        }

    //        if (CommandName.ToUpper().Equals("TOGGLE2"))
    //        {
    //            Response.Redirect("../Dashboard/DashboardSKPIUnit.aspx");

    //        }
    //        if (CommandName.ToUpper().Equals("TOGGLE3"))
    //        {
    //            Response.Redirect("../Dashboard/DashboardSKPIScope.aspx");

    //        }
    //        if (CommandName.ToUpper().Equals("TOGGLE4"))
    //        {
    //            Response.Redirect("../Dashboard/DashboardSKPI.aspx");

    //        }
    //        if (CommandName.ToUpper().Equals("TOGGLE5"))
    //        {
    //            Response.Redirect("../Dashboard/DashboardSKKPI.aspx");

    //        }
    //        if (CommandName.ToUpper().Equals("TOGGLE6"))
    //        {
    //            Response.Redirect("../Dashboard/DashboardSKSPIKPIMapping.aspx");

    //        }
    //        if (CommandName.ToUpper().Equals("TOGGLE7"))
    //        {
    //            Response.Redirect("../Dashboard/DashboardSKKPIPIMapping.aspx");

    //        }
    //        if (CommandName.ToUpper().Equals("TOGGLE8"))
    //        {
    //            Response.Redirect("../Dashboard/DashboardSKKPIUnit.aspx");

    //        }
    //        if (CommandName.ToUpper().Equals("TOGGLE9"))
    //        {
    //            Response.Redirect("../Dashboard/DashboardSKKPIScope.aspx");

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}
    protected void gvKPIUnit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PheonixDashboardSKKPI.KPIUnitSearch(gvKPIUnit.CurrentPageIndex + 1,
                                                gvKPIUnit.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        gvKPIUnit.DataSource = dt;
        gvKPIUnit.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDUNITCODE", "FLDUNIT" };
        string[] alCaptions = { "Unit Code", "Unit" };
        General.SetPrintOptions("gvKPIUnit", "KPI Units", alCaptions, alColumns, ds);
    }

    protected void gvKPIUnit_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            //if (e.Item is GridDataItem)
            //{
            //    GridDataItem item = e.Item as GridDataItem;
            //    int? kpiunitid = General.GetNullableInteger(item.GetDataKeyValue("FLDUNITID").ToString());

            //    string kpiunitidstring = kpiunitid.ToString();

            //    LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
            //    if (edit != null)
            //    {
            //        edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Edit Key Performance Indicator (KPI) Unit','Dashboard/DashboardSKKPIUnitEdit.aspx?shippingkpiunitid=" + kpiunitidstring + "','false','600px','200px');return false");

            //    }
            //}
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    public void gvKPIUnit_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem fi = e.Item as GridFooterItem;
                RadTextBox unitcodeentry = (RadTextBox)fi.FindControl("Radkpiunitcodeentry");
                RadTextBox unitname = (RadTextBox)fi.FindControl("Radkpiunitnameentry");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string unitcode = General.GetNullableString(unitcodeentry.Text);
                string unit = General.GetNullableString(unitname.Text);

                if (!IsValidShippingKPIUnitDetails(unitcode, unit))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKKPI.KPIUnitInsert(rowusercode, unitcode, unit);
                gvKPIUnit.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem ei = e.Item as GridEditableItem;
                RadTextBox unitcodeentry = (RadTextBox)ei.FindControl("Radkpiunitcodeedit");
                RadTextBox unitname = (RadTextBox)ei.FindControl("Radkpiunitnameedit");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string unitcode = General.GetNullableString(unitcodeentry.Text);
                string unit = General.GetNullableString(unitname.Text);

                if (!IsValidShippingKPIUnitDetails(unitcode, unit))
                {
                    ucError.Visible = true;
                    return;
                }

                int? kpiunitid = General.GetNullableInteger(ei.OwnerTableView.DataKeyValues[ei.ItemIndex]["FLDUNITID"].ToString());

                PheonixDashboardSKKPI.KPIUnitUpdate(rowusercode, unitcode, unit, kpiunitid);

                gvKPIUnit.Rebind();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvKPIUnit.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidShippingKPIUnitDetails(string unitcode, string unit)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (unitcode == null)
        {
            ucError.ErrorMessage = "Unit Code.";
        }
        if (unit == null)
        {
            ucError.ErrorMessage = "Unit Name.";
        }

        return (!ucError.IsError);
    }
}