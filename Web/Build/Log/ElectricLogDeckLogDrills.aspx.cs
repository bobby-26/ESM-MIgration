using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogDeckLogDrills : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ShowToolBar();
        }
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbarmain.Show();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //gvLogDrills.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        //int iRowCount = 0;
        //int iTotalPageCount = 0;
        string[] alCaptions = getAlCaptions();
        string[] alColumns = getAlColumns();
        DataSet ds = new DataSet();
        //gvLogDrills.DataSource = ds;
        //gvLogDrills.DataSource = new string[] { };
        //gvLogDrills.VirtualItemCount = iTotalPageCount;
        PrintReport(ds);
    }

    private bool IsValidReport()
    {
        throw new NotImplementedException();
    }

    


    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
	try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Log/ElecticLogLoadLineDraughtWater.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private string[] getAlColumns()
    {
        string[] alColumns = {  };
        return alColumns;
    }
    private string[] getAlCaptions()
    {
        string[] alCaptions = {  };
        return alCaptions;
    }
    private void PrintReport(DataSet ds)
    {
        string[] alColumns = getAlColumns();
        string[] alCaptions = getAlCaptions();
        General.SetPrintOptions("gvLogDrills", "{pageTitle}", alCaptions, alColumns, ds);
    }

    private string[] DummyData()
    {
        return new string[] { };
    }

    private void ShowExcel()
    {
        //int iRowCount = 0;
        //int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = getAlColumns();
        string[] alCaptions = getAlCaptions();
        string sortexpression = null;
        int? sortdirection = null;
      
        // get the data here
        General.ShowExcel("{pageTitle}", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }



    protected void gvContigancyDrills_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvContigancyDrills.DataSource = DummyData();
    }

    protected void gvContigancyDrills_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvContigancyDrills_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvSOPEP_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvSOPEP.DataSource = DummyData();
    }

    protected void gvSOPEP_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvSOPEP_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvTrainingCarried_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvTrainingCarried.DataSource = DummyData();
    }

    protected void gvTrainingCarried_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvTrainingCarried_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvISPS_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvISPS.DataSource = DummyData();
    }

    protected void gvISPS_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvISPS_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvWeeklyCheckCondition_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvWeeklyCheckCondition.DataSource = DummyData();
    }

    protected void gvWeeklyCheckCondition_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvWeeklyCheckCondition_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvIBCIGC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvIBCIGC.DataSource = DummyData();
    }

    protected void gvIBCIGC_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvIBCIGC_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvMonthlyCheck_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvMonthlyCheck.DataSource = DummyData();
    }

    protected void gvMonthlyCheck_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvMonthlyCheck_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
}