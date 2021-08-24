using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;



public partial class Inspection_InspectionTrainingSchedulePopupList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionTrainingSchedulePopupList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvTrainingSchedulelist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        TabstripMenu.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
           
            ViewState["TRAININGSCHEDULEID"] = Request.QueryString["trainingscheduleid"].ToString();
            gvTrainingSchedulelist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        string[] alColumns = { "FLDTRAININGNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDFIXEDORVARIABLE", "FLDTYPE", "FLDTRAININGONBOARDDUEDATE", "FLDTRAININGLASTDONEDATE" };
        string[] alCaptions = { "Name", "Interval", "Interval Type", "Fixed/Variable", "Type", "Due Date", "Last done Date" };
        Guid? TrainingScheduleId = General.GetNullableGuid(ViewState["TRAININGSCHEDULEID"].ToString());
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        DataSet ds = PhoenixInspectionTrainingSchedule.TrainingSchedulePopupSearch(rowusercode, TrainingScheduleId,
                                                gvTrainingSchedulelist.CurrentPageIndex + 1,
                                                gvTrainingSchedulelist.PageSize,
                                                ref iRowCount
                                                );
        General.ShowExcel("Training Schedule", ds.Tables[0], alColumns, alCaptions, null, null);
    }

    protected void Trainingschedule_TabStripMenuCommand(object sender, EventArgs e)
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
    public void gvTrainingSchedulelist_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }

    protected void gvTrainingSchedulelist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        string[] alColumns = { "FLDTRAININGNAME", "FLDFREQUENCY", "FLDFREQUENCYTYPE", "FLDFIXEDORVARIABLE", "FLDTYPE", "FLDTRAININGONBOARDDUEDATE", "FLDTRAININGLASTDONEDATE" };
        string[] alCaptions = { "Name", "Interval", "Interval Type", "Fixed/Variable", "Type", "Due Date", "Last done Date" };
        Guid? TrainingScheduleId = General.GetNullableGuid(ViewState["TRAININGSCHEDULEID"].ToString());
        int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        DataSet ds = PhoenixInspectionTrainingSchedule.TrainingSchedulePopupSearch(rowusercode, TrainingScheduleId,
                                                gvTrainingSchedulelist.CurrentPageIndex + 1,
                                                gvTrainingSchedulelist.PageSize,
                                                ref iRowCount
                                                );
        General.SetPrintOptions("gvTrainingSchedulelist", "Training Schedule ", alCaptions, alColumns, ds);
        gvTrainingSchedulelist.DataSource = ds.Tables[0];
        gvTrainingSchedulelist.VirtualItemCount = iRowCount;
    }

}