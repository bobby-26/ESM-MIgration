using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderReportLogDoneBy : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportLogDoneBy.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvResources')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenugvResources.AccessRights = this.ViewState;
        MenugvResources.MenuList = toolbar.Show();
        //MenugvResources.SetTrigger(pnlgvResources);
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["WORKORDERID"] = null;
            ViewState["WORKORDERREPORTID"] = "";
            if (Request.QueryString["WORKORDERID"] != null)
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
            //getReportId();
            gvResources.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //BindData();
    }
    //private void getReportId()
    //{
    //    try
    //    {
    //        if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
    //        {
    //            DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.EditWorkOrderReport(new Guid(Request.QueryString["WORKORDERID"]));
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                DataRow dr = ds.Tables[0].Rows[0];
    //                ViewState["ISREPORTRESOURCES"] = dr["FLDREPORTUSEDRESOURCES"].ToString();
    //                ViewState["WORKORDERREPORTID"] = dr["FLDWORKORDERREPORTID"].ToString();
    //                ViewState["OPERATIONMODE"] = "EDIT";
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    private void BindData()
    {
        
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDISCIPLINENAME", "FLDSPENTHOURS" };
        string[] alCaptions = { "Discipline", "Spent Hours" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.WorkOrderDoneBySearch(General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , sortexpression, sortdirection
                                                                            , 1
                                                                            , iRowCount
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);
        General.ShowExcel("Resources", ds.Tables[0], alColumns, alCaptions, null, null);       
    }
    protected void gvResources_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "FIND")
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();

            }
            if (CommandName.ToUpper() == "EXCEL")
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
    
    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDDISCIPLINENAME", "FLDSPENTHOURS" };
        string[] alCaptions = { "Discipline", "Spent Hours" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.WorkOrderDoneBySearch(General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                    , sortexpression, sortdirection
                                                                                    , gvResources.CurrentPageIndex + 1
                                                                                    , gvResources.PageSize
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount);
        General.SetPrintOptions("gvResources", "Resources", alCaptions, alColumns, ds);

        gvResources.DataSource = ds;
        gvResources.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
    }

}
