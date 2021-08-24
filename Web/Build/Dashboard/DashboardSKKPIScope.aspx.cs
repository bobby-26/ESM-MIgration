using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;

public partial class Inspection_InspectionShippingKPIScope : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Dashboard/DashboardSKSPIScope.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvKPIScope')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        //toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('Add KPI Scope','Create KPI Scope','Dashboard/DashboardSKKPIScopeAdd.aspx','false','600px','200px')", "Add KPI Scope", "<i class=\"fas fa-plus-circle\"></i>", "ADD");

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

        //Tabkpi.SelectedMenuIndex = 6;
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

       

            gvKPIScope.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSCOPECODE", "FLDSCOPE" };
        string[] alCaptions = { "Scope Code", "Scope" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PheonixDashboardSKKPI.KPIScopeSearch(gvKPIScope.CurrentPageIndex + 1,
                                                 gvKPIScope.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Shipping KPI Scope.xls");
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

    protected void PIScope_TabStripMenuCommand(object sender, EventArgs e)
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

    protected void gvKPIScope_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PheonixDashboardSKKPI.KPIScopeSearch(gvKPIScope.CurrentPageIndex + 1,
                                                gvKPIScope.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);
        gvKPIScope.DataSource = dt;
        gvKPIScope.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDSCOPECODE", "FLDSCOPE" };
        string[] alCaptions = { "Scope Code", "Scope" };
        General.SetPrintOptions("gvKPIScope", "KPI scopes", alCaptions, alColumns, ds);
    }

    protected void gvKPIScope_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            //if (e.Item is GridDataItem)
            //{
            //    GridDataItem item = e.Item as GridDataItem;
            //    int? kpiscopeid = General.GetNullableInteger(item.GetDataKeyValue("FLDSCOPEID").ToString());

            //    string kpiscopeidstring = kpiscopeid.ToString();

            //    LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
            //    if (edit != null)
            //    {
            //        edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Edit Key Performance Indicator (PI) Scope','Dashboard/DashboardSKKPIScopeEdit.aspx?shippingkpiscopeid=" + kpiscopeidstring + "','false','600px','200px');return false");

            //    }
            //}
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }


    }

    public void gvKPIScope_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem fi = e.Item as GridFooterItem;
                RadTextBox scopecodeentry = (RadTextBox)fi.FindControl("Radkpiscopecodeentry");
                RadTextBox scopename = (RadTextBox)fi.FindControl("Radkpiscopenameentry");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string scopecode = General.GetNullableString(scopecodeentry.Text);
                string scope = General.GetNullableString(scopename.Text);

                if (!IsValidShippingKPIscopeDetails(scopecode, scope))
                {
                    ucError.Visible = true;
                    return;
                }

                PheonixDashboardSKKPI.KPIScopeInsert(rowusercode, scopecode, scope);

                gvKPIScope.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem ei = e.Item as GridEditableItem;
                RadTextBox scopecodeentry = (RadTextBox)ei.FindControl("Radkpiscopecodeedit");
                RadTextBox scopename = (RadTextBox)ei.FindControl("Radkpiscopenameedit");
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string scopecode = General.GetNullableString(scopecodeentry.Text);
                string scope = General.GetNullableString(scopename.Text);

                if (!IsValidShippingKPIscopeDetails(scopecode, scope))
                {
                    ucError.Visible = true;
                    return;
                }

                int? kpiscopeid = General.GetNullableInteger(ei.OwnerTableView.DataKeyValues[ei.ItemIndex]["FLDSCOPEID"].ToString());

                PheonixDashboardSKKPI.KPIScopeUpdate(rowusercode, scopecode, scope, kpiscopeid);

                gvKPIScope.Rebind();
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

            gvKPIScope.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidShippingKPIscopeDetails(string scopecode, string scope)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (scopecode == null)
        {
            ucError.ErrorMessage = "Scope Code.";
        }
        if (scope == null)
        {
            ucError.ErrorMessage = "Scope Name.";
        }

        return (!ucError.IsError);
    }
}