using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Web.UI.WebControls;

public partial class Registers_scenarioregister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Registers/RegisterDrillScenario.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvScenariolist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('Add Drill Scenario','','Registers/RegisterDrillScenarioAdd.aspx','false','400px','320px')", "Add Drill Scenario", "<i class=\"fas fa-plus-circle\"></i>", "ADD");

        Tabstripmenu.MenuList = toolbargrid.Show();
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
           
            ViewState["DRILL"] = string.Empty;
            ViewState["SCENARIO"] = string.Empty;
            gvScenariolist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDDRILLNAME", "FLDSCENARIO", "FLDDESCRIPTION" };
        string[] alCaptions = {  "Drill","Scenario","Description" };


       

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataTable dt = PhoenixRegisterDrillScenario.scenariolist(   General.GetNullableString(ViewState["DRILL"].ToString()),
                                                                      General.GetNullableString(ViewState["SCENARIO"].ToString()),
                                                                    gvScenariolist.CurrentPageIndex + 1,
                                                                                                    10,
                                                                                                    ref iRowCount,
                                                                                                    ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Drill-Scenarios.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Drill-Scenarios</h3></td>");
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
    protected void scenariomenu_TabStripCommand(object sender, EventArgs e)
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
   

    protected void gvScenariolist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDDRILLNAME", "FLDSCENARIO", "FLDDESCRIPTION" };
        string[] alCaptions = { "Drill", "Scenario", "Description" };
        DataTable dt = PhoenixRegisterDrillScenario.scenariolist(General.GetNullableString(ViewState["DRILL"].ToString()),
                                                                      General.GetNullableString(ViewState["SCENARIO"].ToString()),
                                                                                        gvScenariolist.CurrentPageIndex + 1,
                                                                                                   gvScenariolist.PageSize,
                                                                                                   ref iRowCount,
                                                                                                   ref iTotalPageCount);
        gvScenariolist.DataSource = dt;
        gvScenariolist.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        
        General.SetPrintOptions("gvScenariolist", "Drill-Scenarios", alCaptions, alColumns, ds);
    }
    


    public void gvScenariolist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? scenarioid = General.GetNullableGuid(item.GetDataKeyValue("FLDSCENARIOID").ToString());
                
                LinkButton edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters1','Drill Scenario Edit','Registers/RegisterDrillScenarioEdit.aspx?drillscenarioid=" + scenarioid + "','false','400px','320px');return false");

                }
            }

            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem filterItem = (GridFilteringItem)e.Item;
                TextBox textBox = (TextBox)filterItem["FLDDRILLNAME"].Controls[0];
                textBox.MaxLength = 198;
                TextBox textBox1 = (TextBox)filterItem["FLDSCENARIO"].Controls[0];
                textBox1.MaxLength = 498;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    public void gvScenariolist_ItemCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName == RadGrid.FilterCommandName)
        {
            ViewState["DRILL"] = gvScenariolist.MasterTableView.GetColumn("FLDDRILLNAME").CurrentFilterValue;
            ViewState["SCENARIO"] = gvScenariolist.MasterTableView.GetColumn("FLDSCENARIO").CurrentFilterValue;

            gvScenariolist.Rebind();

        }

    }

  

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvScenariolist.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}