using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class Owners_OwnersMonthlyReportTrainingDoneList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Owners/OwnersMonthlyReportTrainingDoneList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvTraining')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuTrainingDue.AccessRights = this.ViewState;
        MenuTrainingDue.MenuList = toolbar.Show();

    }

    protected void MenuTrainingDue_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;


        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    private void BindData()
    {

        string[] alColumns = { "FLDTRAININGNAME", "FLDTRAININGONBOARDLASTDONEDATE", "FLDTRAININGONBOARDDUEDATE" };
        string[] alCaptions = { "Training", "Done Date", "Next Due" };

        DataTable dt = PhoenixOwnerReportQuality.OwnersReportTrainingDoneCount(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));

        gvTraining.DataSource = dt;

        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        dt1 = dt.Copy();
        ds.Tables.Add(dt1);
        General.SetPrintOptions("gvTraining", "Training Done", alCaptions, alColumns, ds);

        

    }
    protected void ShowExcel()
    {
        try
        {
            string[] alColumns = { "FLDTRAININGNAME", "FLDTRAININGONBOARDLASTDONEDATE", "FLDTRAININGONBOARDDUEDATE" };
            string[] alCaptions = { "Training", "Done Date", "Next Due" };

            DataTable dt = PhoenixOwnerReportQuality.OwnersReportTrainingDoneCount(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));

            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            dt1 = dt.Copy();
            ds.Tables.Add(dt1);
            General.ShowExcel("Training Done", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTraining_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}