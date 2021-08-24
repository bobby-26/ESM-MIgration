using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class Owners_OwnersMonthlyReportDrillDueList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Owners/OwnersMonthlyReportDrillDueList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvdrildue')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

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

        string[] alColumns = { "FLDDRILLNAME", "FLDDRILLDUEDATE", "FLDOVERDUE"};
        string[] alCaptions = { "Drill", "Due On", "Overdue By"};



        DataTable dt = PhoenixOwnerReportQuality.OwnersReportDrillDueCount(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));

        gvdrildue.DataSource = dt;

        DataSet ds = new DataSet();
        DataTable dt1 = new DataTable();
        dt1 = dt.Copy();
        ds.Tables.Add(dt1);
        General.SetPrintOptions("gvdrildue", "Drill Due", alCaptions, alColumns, ds);

        

    }
    protected void ShowExcel()
    {
        try
        {
            string[] alColumns = { "FLDDRILLNAME", "FLDDRILLDUEDATE", "FLDOVERDUE" };
            string[] alCaptions = { "Drill", "Due On", "Overdue By" };

            DataTable dt = PhoenixOwnerReportQuality.OwnersReportDrillDueCount(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));

            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            dt1 = dt.Copy();
            ds.Tables.Add(dt1);
            General.ShowExcel("Drill Due", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvdrildue_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}