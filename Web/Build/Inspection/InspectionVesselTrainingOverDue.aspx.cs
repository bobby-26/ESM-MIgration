using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Inspection_InspectionVesselTrainingOverDue : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionVesselTrainingOverDue.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvoverduetraining')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        TabstripMenu.MenuList = toolbargrid.Show();
        if (!IsPostBack)
        {
           



            gvoverduetraining.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDTRAININGNAME", "FLDTRAININGDUEDATE", "DUEIN", };
        string[] alCaptions = { "Traning", "Due on", "Overdue by" };

        int vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        DataTable dt = PhoenixInspectionTrainingSummary.OverdueTraining(gvoverduetraining.CurrentPageIndex + 1,
                                                gvoverduetraining.PageSize, 
                                                ref iRowCount,
                                                ref iTotalPageCount
                                                , vesselid);
        General.ShowExcel("Overdue Trainings", dt, alColumns, alCaptions, null, null);

    }

    protected void drillvsvessels_TabStripMenuCommand(object sender, EventArgs e)
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

    protected void gvoverduetraining_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDTRAININGNAME", "FLDTRAININGDUEDATE", "DUEIN", };
        string[] alCaptions = { "Traning", "Due on", "Overdue by" };

        int vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        DataTable dt = PhoenixInspectionTrainingSummary.OverdueTraining(gvoverduetraining.CurrentPageIndex + 1,
                                                gvoverduetraining.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount
                                                , vesselid);

        gvoverduetraining.DataSource = dt;
        gvoverduetraining.VirtualItemCount = iRowCount;

        DataSet ds = dt.DataSet;
       

        General.SetPrintOptions("gvoverduetraining", "Over Due Trainings", alCaptions, alColumns, ds);

    }

    protected void gvoverduetraining_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? Trainingscheduleid = General.GetNullableGuid(item.GetDataKeyValue("FLDTRAININGONBOARDSCHEDULEID").ToString());

                HtmlAnchor Trainingname = (HtmlAnchor)item.FindControl("Radlblduein");

                Trainingname.Attributes.Add("onclick", "javascript:parent.openNewWindow('filter','Training Report','Inspection/InspectionTrainingScheduleReport.aspx?Trainingscheduleid=" + Trainingscheduleid + "&l=d"+"');return false");

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

            gvoverduetraining.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}