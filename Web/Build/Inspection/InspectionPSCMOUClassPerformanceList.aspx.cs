using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.IO;
using Telerik.Web.UI.GridExcelBuilder;

public partial class Inspection_InspectionPSCMOUClassPerformanceList : PhoenixBasePage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        foreach (Telerik.Web.UI.GridColumn col in gvVerificationSummaryDepartment.MasterTableView.RenderColumns)
        {
            if (col.UniqueName.Equals("ACTION"))
            {
                col.OrderIndex = int.Parse(ViewState["NOOFCOLOUMNS"].ToString()) + 3;
            }
        }
        gvVerificationSummaryDepartment.MasterTableView.GetColumn("FLDCLASSID").Display = false;
        gvVerificationSummaryDepartment.MasterTableView.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        gvVerificationSummaryDepartment.Rebind();

    }
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["NOOFCOLOUMNS"] = null;
                gvVerificationSummaryDepartment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);                
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPSCMOUClassPerformanceList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Class Performance','Inspection/InspectionPSCMOUClassPerformanceAdd.aspx?action=add')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDADDRESS");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Class','Inspection/InspectionPSCCopyMOURegisterData.aspx?type=CLSPERF')", "Copy", "<i class=\"fa fa-copy\"></i>", "COPY");

            MenuRegistersDepartment.AccessRights = this.ViewState;
            MenuRegistersDepartment.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Recognized by MOU", "RECORG", ToolBarDirection.Right);
            toolbarmain.AddButton("Performance Level", "PERFLEVEL", ToolBarDirection.Right);
            toolbarmain.AddButton("Class Performance", "FLAGPERF", ToolBarDirection.Right);
            
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            MenuOrderFormMain.SelectedMenuIndex = 2;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("RECORG"))
        {
            Response.Redirect("../Inspection/InspectionPSCMOUClassRecognizedList.aspx", false);
        }
        if (CommandName.ToUpper().Equals("PERFLEVEL"))
        {
            Response.Redirect("../Inspection/InspectionPSCMOUClassPerformanceLevelList.aspx", false);
        }
    }

    protected void RegistersDepartment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ExportGridToExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ExportGridToExcel()
    {
        //gvVerificationSummaryDepartment.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), "Xlsx");
        gvVerificationSummaryDepartment.ExportSettings.Excel.Format = (GridExcelExportFormat)Enum.Parse(typeof(GridExcelExportFormat), "ExcelML");
        gvVerificationSummaryDepartment.ExportSettings.ExportOnlyData = true;
        gvVerificationSummaryDepartment.ExportSettings.OpenInNewWindow = true;
        gvVerificationSummaryDepartment.MasterTableView.ExportToExcel();
    }
    protected void gvVerificationSummaryDepartment_ExcelMLExportStylesCreated(object source, GridExportExcelMLStyleCreatedArgs e)
    {
        foreach (StyleElement style in e.Styles)
        {
            switch (style.Id)
            {
                case "headerStyle":
                    style.FontStyle.Bold = true;
                    break;
            }
        }
    }

    private void BindFlagPerformance()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionPSCMOUMatrix.PSCMOUShipClassPerformanceList(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            sortexpression,
            sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvVerificationSummaryDepartment.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0].ToString() != "")
        {
            gvVerificationSummaryDepartment.DataSource = ds.Tables[0];
            ViewState["NOOFCOLOUMNS"] = ds.Tables[0].Columns.Count;
        }
        else
        { 
            gvVerificationSummaryDepartment.DataSource = ds;
            ViewState["NOOFCOLOUMNS"] = ds.Tables[0].Columns.Count;
        }

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void Rebind()
    {
        gvVerificationSummaryDepartment.SelectedIndexes.Clear();
        gvVerificationSummaryDepartment.EditIndexes.Clear();
        gvVerificationSummaryDepartment.DataSource = null;
        gvVerificationSummaryDepartment.Rebind();
    }


    protected void gvVerificationSummaryDepartment_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            if (eb != null)
            {

                eb.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Flag', '" + Session["sitepath"] + "/Inspection/InspectionPSCMOUClassPerformanceAdd.aspx?action=edit&classid=" + drv["FLDCLASSID"].ToString() + "');return false;");

            }

            if(drv["FLDCLASSID"].ToString() == "")
            {
                eb.Visible = false;
            }

        }

    }

    protected void gvVerificationSummaryDepartment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVerificationSummaryDepartment.CurrentPageIndex + 1;
            BindFlagPerformance();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVerificationSummaryDepartment_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }


}