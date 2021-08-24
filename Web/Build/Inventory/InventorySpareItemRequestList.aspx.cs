using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventorySpareItemRequestList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemRequestList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStockItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemRequestListFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemRequestList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemRequestGeneralAdd.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

        MenuStockItem.AccessRights = this.ViewState;

        MenuStockItem.MenuList = toolbargrid.Show();

        PhoenixToolbar toolbarmenu = new PhoenixToolbar();
        toolbarmenu.AddButton("Components", "COMPONENT");
        toolbarmenu.AddButton("Jobs", "JOBS");
        toolbarmenu.AddButton("Spares", "SPARES");
        TabMenus.AccessRights = this.ViewState;
        TabMenus.MenuList = toolbarmenu.Show();
        TabMenus.SelectedMenuIndex = 2;

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvStockItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvStockItem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void StockItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvStockItem.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentSpareItemRequestFilterCriteria = null;
                BindData();
                gvStockItem.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void TabMenus_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("COMPONENT"))
        {
            Filter.CurrentComponentFilterCriteria = null;
            string url = "InventoryComponentChangeRequestList.aspx";
            Response.Redirect("../Inventory/" + url, false);
        }
        else if (CommandName.ToUpper().Equals("JOBS"))
        {
            Filter.CurrentComponentJobFilter = null;
            Response.Redirect("../PlannedMaintenance/PlannedMaintenanceComponentJobCRList.aspx", false);
        }
        else if (CommandName.ToUpper().Equals("SPARES"))
        {
            Filter.CurrentSpareItemRequestFilterCriteria = null;
            Response.Redirect("../Inventory/InventorySpareItemRequestList.aspx", false);
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "MAKER", "FLDMAKERREFERENCEFULLDETAILS", "PREFERREDVENDOR", "FLDREQTYPENAME", "FLDREMARKS" };
            string[] alCaptions = { "Number", "Name", "Maker", "Maker's Reference", "Preferred Vendor", "Resquest Type", "Remarks" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds;
            if (Filter.CurrentSpareItemRequestFilterCriteria != null)
            {
                NameValueCollection nvc = Filter.CurrentSpareItemRequestFilterCriteria;

                ds = PhoenixInventorySpareItemRequest.SpareItemRequestSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , nvc.Get("txtNumber").ToString()
                                                                            , nvc.Get("txtName").ToString()
                                                                            , General.GetNullableInteger(nvc.Get("txtMakerid").ToString())
                                                                            , General.GetNullableInteger(nvc.Get("txtVendorId").ToString())
                                                                            , null
                                                                            , (byte?)General.GetNullableInteger(nvc.Get("chkCritical"))
                                                                            , General.GetNullableString(nvc.Get("txtMakerReference").ToString())
                                                                            , General.GetNullableString(nvc.Get("txtDrawing").ToString())
                                                                            , sortexpression
                                                                            , sortdirection
                                                                            , gvStockItem.CurrentPageIndex + 1
                                                                            , gvStockItem.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);

            }
            else
            {
                ds = PhoenixInventorySpareItemRequest.SpareItemRequestSearch(
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                    gvStockItem.CurrentPageIndex + 1, gvStockItem.PageSize, ref iRowCount, ref iTotalPageCount);
            }
            General.ShowExcel("Spare Change Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDNUMBER", "FLDNAME", "MAKER", "FLDMAKERREFERENCEFULLDETAILS", "PREFERREDVENDOR", "FLDREQTYPENAME", "FLDREMARKS" };
            string[] alCaptions = { "Number", "Name", "Maker", "Maker's Reference", "Preferred Vendor", "Resquest Type", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            if (Filter.CurrentSpareItemRequestFilterCriteria != null)
            {
                NameValueCollection nvc = Filter.CurrentSpareItemRequestFilterCriteria;

                ds = PhoenixInventorySpareItemRequest.SpareItemRequestSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , nvc.Get("txtNumber").ToString()
                                                                            , nvc.Get("txtName").ToString()
                                                                            , General.GetNullableInteger(nvc.Get("txtMakerid").ToString())
                                                                            , General.GetNullableInteger(nvc.Get("txtVendorId").ToString())
                                                                            , null
                                                                            , (byte?)General.GetNullableInteger(nvc.Get("chkCritical"))
                                                                            , General.GetNullableString(nvc.Get("txtMakerReference").ToString())
                                                                            , General.GetNullableString(nvc.Get("txtDrawing").ToString())
                                                                            , sortexpression
                                                                            , sortdirection
                                                                            , (int)ViewState["PAGENUMBER"]
                                                                            , gvStockItem.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);
            }
            else
            {

                ds = PhoenixInventorySpareItemRequest.SpareItemRequestSearch(
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                    gvStockItem.CurrentPageIndex + 1,
                    gvStockItem.PageSize, ref iRowCount, ref iTotalPageCount);
            }

            General.SetPrintOptions("gvStockItem", "Spare Change Request", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvStockItem.DataSource = ds;
                gvStockItem.VirtualItemCount = iRowCount;

                if (ViewState["REQUESTID"] == null)
                {
                    ViewState["REQUESTID"] = ds.Tables[0].Rows[0]["FLDSPAREITEMREQUESTID"].ToString();
                    ViewState["SPAREITEMID"] = ds.Tables[0].Rows[0]["FLDSPAREITEMID"].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventorySpareItemRequestAdd.aspx?REQUESTID=";
                }

                //if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
                //{
                //    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
                //}
                //if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareItemComponentRequest.aspx"))
                //{
                //    ifMoreInfo.Attributes["src"] = "../Inventory/InventorySpareItemComponentRequest.aspx?SPAREITEMID=" + ViewState["SPAREITEMID"].ToString() + "&REQUESTID=" + ViewState["REQUESTID"];
                //}
                //else
                //{
                //    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["REQUESTID"];
                //}


            }
            else
            {
                DataTable dt = ds.Tables[0];
                gvStockItem.DataSource = "";
                //ifMoreInfo.Attributes["src"] = "../Inventory/InventorySpareItemRequestGeneral.aspx";
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStockItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvStockItem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStockItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.Item is GridDataItem)
            {

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

                LinkButton ap = (LinkButton)e.Item.FindControl("cmdApprove");
                if (ap != null)
                {
                    ap.Attributes.Add("onclick", "return fnConfirmDelete(event, 'Are you sure want to approve this record?'); return false;");
                    ap.Visible = false;
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                        ap.Visible = SessionUtil.CanAccess(this.ViewState, ap.CommandName);
                }

                RadLabel lb = (RadLabel)e.Item.FindControl("lblMakerReference");
                RadLabel lblMakerRefFullDetails = (RadLabel)e.Item.FindControl("lblMarkerReferencFullDetails");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipMakerReference");
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblMakerRefFullDetails.ClientID;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStockItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string REQUESTID = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                PhoenixInventorySpareItemRequest.DeleteSpareItemRequest(new Guid(REQUESTID));
                ViewState["REQUESTID"] = null;
            }
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                string REQUESTID = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                Guid g = new Guid(REQUESTID);
                PhoenixInventorySpareItemRequest.ApproveSpareItemRequest(g, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ViewState["REQUESTID"] = null;

            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["REQUESTID"] = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                ViewState["SPAREITEMID"] = ((RadLabel)e.Item.FindControl("lblSpareItemId")).Text;
                ViewState["DTKEY"] = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                Response.Redirect("../Inventory/InventorySpareItemRequestGeneralAdd.aspx?Type=Edit&Mode=Edit&REQUESTID=" + ViewState["REQUESTID"].ToString(), false);
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}