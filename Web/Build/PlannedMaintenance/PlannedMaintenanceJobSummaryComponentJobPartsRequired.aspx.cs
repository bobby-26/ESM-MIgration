using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Collections.Specialized;
using System.Data;
using Telerik.Web.UI;

public partial class PlannedMaintenanceJobSummaryComponentJobPartsRequired : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobSummaryComponentJobPartsRequired.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRequiredParts')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenugvRequiredParts.AccessRights = this.ViewState;
        MenugvRequiredParts.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["COMPONENTJOBID"] = null;
            

            ViewState["COMPONENTJOBID"] = Request.QueryString["COMPONENTJOBID"] ?? string.Empty;
            ViewState["VESSELID"] = Request.QueryString["VESSELID"] ?? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            gvRequiredParts.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDLOCATIONNAME", "FLDQUANTITY", "FLDUNITNAME" };
        string[] alCaptions = { "Part Number", "Part Name", "Location", "Quantity", "Unit" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceComponentJob.ComponentJobPartsRequiredSearch(General.GetNullableGuid(ViewState["COMPONENTJOBID"].ToString())
            , int.Parse(ViewState["VESSELID"].ToString())
            , sortexpression, sortdirection
            , gvRequiredParts.CurrentPageIndex + 1
            , gvRequiredParts.PageSize
            , ref iRowCount, ref iTotalPageCount);
        General.SetPrintOptions("gvRequiredParts", "Stock Used", alCaptions, alColumns, ds);

        gvRequiredParts.DataSource = ds;
        gvRequiredParts.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDLOCATIONNAME", "FLDQUANTITY", "FLDUNITNAME" };
        string[] alCaptions = { "Part Number", "Part Name", "Location", "Quantity", "Unit" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPlannedMaintenanceComponentJob.ComponentJobPartsRequiredSearch(General.GetNullableGuid(ViewState["COMPONENTJOBID"].ToString())
                                                                                    , int.Parse(ViewState["VESSELID"].ToString())
                                                                                    , sortexpression
                                                                                    , sortdirection
                                                                                    , gvRequiredParts.CurrentPageIndex + 1
                                                                                    , gvRequiredParts.PageSize
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount);
        General.ShowExcel("Parts Required", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void gvRequiredParts_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvRequiredParts.CurrentPageIndex = 0;
                gvRequiredParts.Rebind();
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRequiredParts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}