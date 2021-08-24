using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Web.UI.WebControls;

public partial class Registers_RegisterTrainingScenario : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Registers/RegisterTrainingScenario.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvScenariolist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('Add Training Scenario','','Registers/RegisterTrainingScenarioAdd.aspx','false','400px','320px')", "Add Training Scenario", "<i class=\"fas fa-plus-circle\"></i>", "ADD");

        Tabstripmenu.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
          
            ViewState["TRAINING"] = string.Empty;
            ViewState["SCENARIO"] = string.Empty;
            gvScenariolist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDTRAININGNAME", "FLDSCENARIO", "FLDDESCRIPTION" };
            string[] alCaptions = { "Training", "Scenario", "Description" };
            DataTable dt = PhoenixRegisterTrainingScenario.TrainingScenarioSearch(General.GetNullableString(ViewState["TRAINING"].ToString()),
                                                                          General.GetNullableString(ViewState["SCENARIO"].ToString()),
                                                                                            gvScenariolist.CurrentPageIndex + 1,
                                                                                                       gvScenariolist.PageSize,
                                                                                                       ref iRowCount,
                                                                                                       ref iTotalPageCount);

            General.ShowExcel("Training - Scenario", dt, alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
        string[] alColumns = { "FLDTRAININGNAME", "FLDSCENARIO", "FLDDESCRIPTION" };
        string[] alCaptions = { "Training", "Scenario", "Description" };
        DataTable dt = PhoenixRegisterTrainingScenario.TrainingScenarioSearch(General.GetNullableString(ViewState["TRAINING"].ToString()),
                                                                      General.GetNullableString(ViewState["SCENARIO"].ToString()),
                                                                                        gvScenariolist.CurrentPageIndex + 1,
                                                                                                   gvScenariolist.PageSize,
                                                                                                   ref iRowCount,
                                                                                                   ref iTotalPageCount);
        gvScenariolist.DataSource = dt;
        gvScenariolist.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;

        General.SetPrintOptions("gvScenariolist", "Training-Scenarios", alCaptions, alColumns, ds);
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
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters1','Training Scenario Edit','Registers/RegisterTrainingScenarioEdit.aspx?TrainingScenarioId=" + scenarioid + "','false','400px','320px');return false");

                }
            }

            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem filterItem = (GridFilteringItem)e.Item;
                TextBox textBox = (TextBox)filterItem["FLDTRAININGNAME"].Controls[0];
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
            ViewState["TRAINING"] = gvScenariolist.MasterTableView.GetColumn("FLDTRAININGNAME").CurrentFilterValue;
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