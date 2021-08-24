using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.Registers;

public partial class Registers_RegisterTraining : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Registers/RegisterTraining.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTrainingist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:parent.openNewWindow('Add Training','','Registers/RegisterTrainingAdd.aspx','false','800px','380px')", "Add Drill", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
        TabstripTrainingregistermenu.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            
            ViewState["TRAINING"] = string.Empty;
            gvTrainingist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDTRAININGNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDAPPLIESTO", "FLDFIXEDORVARIABLE", "FLDTYPE", "FLDPHOTOYN", "FLDDASHBOARDYN" };
            string[] alCaptions = { "Name", "Interval", "Interval Type", "Applies To", "Fixed/Variable", "Type", "Photo Mandatory (Y/N)", "Show in Dashboard (Y/N)" };


            DataTable dt = PhoenixRegisterTraining.TrainingSearch(General.GetNullableString(ViewState["TRAINING"].ToString()),
                                                  gvTrainingist.CurrentPageIndex + 1,
                                              gvTrainingist.PageSize,
                                              ref iRowCount,
                                              ref iTotalPageCount);

            General.ShowExcel("Trainings", dt, alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void Trainingregistermenu_TabStripCommand(object sender, EventArgs e)
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

    protected void gvTrainingist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDTRAININGNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDAPPLIESTO", "FLDFIXEDORVARIABLE", "FLDTYPE", "FLDPHOTOYN", "FLDDASHBOARDYN" };
        string[] alCaptions = { "Name", "Interval", "Interval Type", "Applies To", "Fixed/Variable", "Type", "Photo Mandatory (Y/N)", "Show in Dashboard (Y/N)" };




        DataTable dt = PhoenixRegisterTraining.TrainingSearch(General.GetNullableString(ViewState["TRAINING"].ToString()),
                                                    gvTrainingist.CurrentPageIndex + 1,
                                                gvTrainingist.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount);




        gvTrainingist.DataSource = dt;
        gvTrainingist.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;

        General.SetPrintOptions("gvTrainingist", "Drills ", alCaptions, alColumns, ds);
    }

    public void gvTrainingist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? Trainingid = General.GetNullableGuid(item.GetDataKeyValue("FLDTRAININGID").ToString());
                LinkButton appliesto = ((LinkButton)item.FindControl("btnappliesto"));
                if (appliesto != null)
                {
                    appliesto.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','List of Vessel Types','Inspection/InspectionVesselTypeSearch.aspx?trainingid=" + Trainingid + "','false','400px','320px');return false");
                }

                LinkButton edit = ((LinkButton)item.FindControl("btnedit"));
                if (edit != null)
                {
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters1','Training Edit','Registers/RegisterTrainingEdit.aspx?trainingid=" + Trainingid + "','false','800px','380px');return false");

                }
            }

            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem filterItem = (GridFilteringItem)e.Item;
                TextBox textBox = (TextBox)filterItem["FLDTRAININGNAME"].Controls[0];
                textBox.MaxLength = 198;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    public void gvTrainingist_ItemCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName == RadGrid.FilterCommandName)
        {
            ViewState["TRAINING"] = gvTrainingist.MasterTableView.GetColumn("FLDTRAININGNAME").CurrentFilterValue;

            gvTrainingist.Rebind();

        }
    }



    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            gvTrainingist.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}