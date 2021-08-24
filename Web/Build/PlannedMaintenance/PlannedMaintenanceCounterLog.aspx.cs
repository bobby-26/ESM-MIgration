using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceCounterLog : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceCounterLog.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCounterUpdate')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceCounterLog.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
       
        MenugvCounterUpdate.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["WORKORDERID"] = null;
            if (Request.QueryString["WORKORDERID"] != null)
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
            gvCounterUpdate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }
    private void BindData()
    {

        string CompCode = "";
        if (txtComponentCode.TextWithPromptAndLiterals.Length > 2)
        {
            CompCode = txtComponentCode.TextWithPromptAndLiterals;
        }

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDHARDNAME", "FLDREADINGVALUE", "FLDREADINGDATE" };
        string[] alCaptions = { "Component Number", "Component Name", "Counter Type", "Current Value", "Read Date" };

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceCounter.CounterLogSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            General.GetNullableString(CompCode),
            General.GetNullableString(txtNameTitle.Text), sortexpression, sortdirection,
             gvCounterUpdate.CurrentPageIndex + 1, gvCounterUpdate.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvCounterUpdate", "Counter Log", alCaptions, alColumns, ds);

        gvCounterUpdate.DataSource = ds;
        gvCounterUpdate.VirtualItemCount = iRowCount;
    }

    protected void gvCounterUpdate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
               
                ViewState["PAGENUMBER"] = 1;
                //BindData();
                gvCounterUpdate.Rebind();
            }
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
    protected void ShowExcel()
    {
        string CompCode = "";
        if (txtComponentCode.TextWithPromptAndLiterals.Length > 3)
        {
            CompCode = txtComponentCode.TextWithPromptAndLiterals;
        }

        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDHARDNAME", "FLDREADINGVALUE", "FLDREADINGDATE" };
        string[] alCaptions = { "Component Number", "Component Name", "Counter Type", "Current Value", "Read Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPlannedMaintenanceCounter.CounterLogSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            General.GetNullableString(CompCode),
            General.GetNullableString(txtNameTitle.Text), sortexpression, sortdirection, gvCounterUpdate.CurrentPageIndex + 1, gvCounterUpdate.PageSize, ref iRowCount, ref iTotalPageCount);
        General.ShowExcel("Counter Log", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            gvCounterUpdate.ExportSettings.Excel.Format = GridExcelExportFormat.Biff;
            gvCounterUpdate.ExportSettings.IgnorePaging = true;
            gvCounterUpdate.ExportSettings.ExportOnlyData = true;
            gvCounterUpdate.ExportSettings.OpenInNewWindow = true;
            ShowExcel();
        }
        if(e.CommandName == RadGrid.RebindGridCommandName)
        {
            gvCounterUpdate.CurrentPageIndex = 0;
        }

    }
    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        //{
        //    GridEditableItem item = e.Item as GridEditableItem;

        //    string disciplineCode = ((RadTextBox)e.Item.FindControl("txtDisciplineCodeEdit")).Text;
        //    string disciplineName = ((RadTextBox)e.Item.FindControl("txtDisciplineNameEdit")).Text;
        //}
    }


    protected void gvCounterUpdate_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
