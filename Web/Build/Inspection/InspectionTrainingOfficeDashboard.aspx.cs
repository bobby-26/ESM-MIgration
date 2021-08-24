using System;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Inspection_InspectionTrainingOfficeDashboard : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionTrainingOfficeDashboard.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTrainingofiicedashboard')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        TabstripTrainingofficedashboardmenu.MenuList = toolbargrid.Show();



        if (!IsPostBack)
        {
           


            gvTrainingofiicedashboard.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    { }

    protected void Trainingjobregistermenu_TabStripCommand(object sender, EventArgs e)
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



    protected void gvTrainingofiicedashboard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        int vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        string[] alColumns = { "FLDTRAININGNAME", "FLDOVERDUE" };
        string[] alCaptions = { "Mandatory Trainings", "Over Due" };
        DataTable dt = PhoenixInspectionTrainingSummary.TrainingOfficeDashboardSearch(gvTrainingofiicedashboard.CurrentPageIndex + 1,
                                                gvTrainingofiicedashboard.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount
                                               , vesselid
                                                );

        gvTrainingofiicedashboard.DataSource = dt;
        gvTrainingofiicedashboard.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
        General.SetPrintOptions("gvTrainingofiicedashboard", "Training-Reminder", alCaptions, alColumns, ds);



    }

    public void gvTrainingofiicedashboard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {

                GridDataItem item = (GridDataItem)e.Item;
                string type = DataBinder.Eval(e.Item.DataItem, "FLDTYPE").ToString();
                Guid? Trainingid = General.GetNullableGuid(item.GetDataKeyValue("FLDTRAININGID").ToString());
                HtmlAnchor mandatoryoverdue = (HtmlAnchor)item.FindControl("overdueanchor");

                mandatoryoverdue.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Due Trainings and Vessels','Inspection/InspectionTrainingvsVesselList.aspx?Trainingid=" + Trainingid + "&i=-1" + "&j=-1500" + "&type=" + type + "');return false");


            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}