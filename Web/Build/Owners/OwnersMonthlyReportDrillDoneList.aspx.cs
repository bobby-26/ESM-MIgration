using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using Telerik.Web.UI;


public partial class Owners_OwnersMonthlyReportDrillDoneList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Owners/OwnersMonthlyReportDrillDoneList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDrill')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuDrillDue.AccessRights = this.ViewState;
        MenuDrillDue.MenuList = toolbar.Show();


    }

    protected void MenuDrillDue_TabStripCommand(object sender, EventArgs e)
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

        string[] alColumns = { "FLDDRILLNAME", "FLDDRILLLASTDONEDATE", "FLDDUEDATE" };
        string[] alCaptions = { "Drill", "Done Date", "Next Due" };



        DataTable dt = PhoenixOwnerReportQuality.OwnersReportDrillDoneCount(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));

        gvDrill.DataSource = dt;

        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        dt1 = dt.Copy();
        ds.Tables.Add(dt1);
        General.SetPrintOptions("gvDrill", "Drill Done", alCaptions, alColumns, ds);
                

    }
    protected void ShowExcel()
    {
        try
        {
            string[] alColumns = { "FLDDRILLNAME", "FLDDRILLLASTDONEDATE", "FLDDUEDATE" };
            string[] alCaptions = { "Drill", "Done Date", "Next Due" };

            DataTable dt = PhoenixOwnerReportQuality.OwnersReportDrillDoneCount(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));

            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            dt1 = dt.Copy();
            ds.Tables.Add(dt1);

            General.ShowExcel("Drill Done", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDrill_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}