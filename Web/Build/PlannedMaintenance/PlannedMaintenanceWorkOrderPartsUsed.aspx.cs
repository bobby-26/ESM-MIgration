using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderPartsUsed : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderPartsUsed.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvUsesParts')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListStockItemWithLocation.aspx?mode=multi', true);", "Part", "<i class=\"fas fa-cogs\"></i>", "ADDPART");

        MenugvUsesParts.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["WORKORDERID"] = null;
            if (Request.QueryString["WORKORDERID"] != null)
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];

            gvUsesParts.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDROB" };
        string[] alCaptions = { "Part Number", "Part Name", "Maker Reference", "ROB" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceUsesParts.UsesPartsSearch(General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
            , PhoenixSecurityContext.CurrentSecurityContext.VesselID, sortexpression, sortdirection
            , gvUsesParts.CurrentPageIndex + 1, gvUsesParts.PageSize, ref iRowCount, ref iTotalPageCount);
        General.SetPrintOptions("gvUsesParts", "Parts Required", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvUsesParts.DataSource = ds;
            gvUsesParts.VirtualItemCount = iRowCount;
        }
        else
        {
            DataTable dt = ds.Tables[0];
            gvUsesParts.DataSource = "";
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDROB" };
        string[] alCaptions = { "Part Number", "Part Name", "Maker Reference", "ROB" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPlannedMaintenanceUsesParts.UsesPartsSearch(General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                , sortexpression, sortdirection
                                                                , gvUsesParts.CurrentPageIndex + 1
                                                                , gvUsesParts.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);
        General.ShowExcel("Parts Required", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void gvUsesParts_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvUsesParts.CurrentPageIndex = 0;
                BindData();
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

    protected void gvUsesParts_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
    }

    private bool IsValidUsesParts(string workorderid, string sparepartiid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (workorderid.Trim().Equals(""))
            ucError.ErrorMessage = "Work Order is required.";

        if (sparepartiid.Trim().Equals(""))
            ucError.ErrorMessage = "Spare item is required.";

        return (!ucError.IsError);
    }

    private void InsertUsesParts(string WorkOrderId, string csvItemId,string csvlocationid,string csvspareitemlocationid)
    {
        PhoenixPlannedMaintenanceUsesParts.InsertUsesParts(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(WorkOrderId), csvItemId,csvlocationid, csvspareitemlocationid);
    }

    private void DeletegvUsesPartss(string workorderlineid)
    {
        PhoenixPlannedMaintenanceUsesParts.DeleteUsesParts(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(workorderlineid));
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            string stockitemid = nvc.Get("lblStockItemId").ToString();
            string location = nvc.Get("lblLocationId").ToString();
            string spareitemlocationid = nvc.Get("lblSpareItemLocationId").ToString();

            if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
            {
                InsertUsesParts(ViewState["WORKORDERID"].ToString(), stockitemid,location,spareitemlocationid);
            }
            BindData();
            gvUsesParts.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
         
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeletegvUsesPartss(((RadLabel)e.Item.FindControl("lblWorkOrderLineID")).Text);
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
        gvUsesParts.Rebind();
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadLabel minQTY = (RadLabel)e.Item.FindControl("lblminqtyflag");
                Image img = (Image)e.Item.FindControl("imgFlag");
                if (minQTY.Text.ToString() == "1")
                {
                    img.Visible = true;
                    img.ToolTip = "ROB is less than Minimum Level";
                }
                else
                    img.Visible = false;
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
