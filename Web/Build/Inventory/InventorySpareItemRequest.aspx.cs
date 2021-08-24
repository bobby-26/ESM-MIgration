using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventorySpareItemRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStockItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemRequestFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemRequest.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuStockItem.AccessRights = this.ViewState;

            MenuStockItem.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                //toolbarmain.AddButton("Search", "SEARCH");
                toolbarmain.AddButton("General", "GENERAL");
                toolbarmain.AddButton("Component", "COMPONENTS");

                MenuInventoryStockItem.AccessRights = this.ViewState;
                MenuInventoryStockItem.MenuList = toolbarmain.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["REQUESTID"] = null;

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;

                if (Request.QueryString["REQUESTID"] != null && Request.QueryString["SETCURRENTNAVIGATIONTAB"] != null && Request.QueryString["REQUESTID"].ToString() != "")
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = Request.QueryString["SETCURRENTNAVIGATIONTAB"].ToString();
                    ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();
                }
                if (Request.QueryString["REQUESTID"] != null)
                {
                    ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();
                }
                MenuInventoryStockItem.SelectedMenuIndex = 0;
                gvStockItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void InventoryStockItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Response.Redirect("../Inventory/InventorySpareItemRequestFilter.aspx");
            }
            else if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventorySpareItemRequestGeneral.aspx?REQUESTID=";
            }
            else if (CommandName.ToUpper().Equals("COMPONENTS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventorySpareItemComponentRequest.aspx?SPAREITEMID=" + ViewState["SPAREITEMID"] + "&REQUESTID=";
            }
            //else if (dce.CommandName.ToUpper().Equals("DETAILS"))
            //{
            //    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventorySpareItemDetail.aspx?REQUESTID=";
            //}
            //else if (dce.CommandName.ToUpper().Equals("ATTACHMENT"))
            //{
            //    GetAttachmentDtkey();
            //    if (ViewState["DTKEY"] != null)
            //        ViewState["SETCURRENTNAVIGATIONTAB"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
            //}
            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                if (ViewState["DTKEY"] != null)
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
                else
                    ifMoreInfo.Attributes["src"] = "../Inventory/InventorySpareItemRequestGeneral.aspx";
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["REQUESTID"];
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void GetAttachmentDtkey()
    //{
    //    if (ViewState["REQUESTID"] != null && (ViewState["REQUESTID"].ToString() != ""))
    //    {
    //        DataSet ds = PhoenixInventorySpareItem.ListSpareItem(new Guid(ViewState["REQUESTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
    //        DataRow dr = ds.Tables[0].Rows[0];
    //        ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
    //    }
    //}

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
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventorySpareItemRequestGeneral.aspx?REQUESTID=";
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
                }
                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareItemComponentRequest.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = "../Inventory/InventorySpareItemComponentRequest.aspx?SPAREITEMID=" + ViewState["SPAREITEMID"].ToString() + "&REQUESTID=" + ViewState["REQUESTID"];
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["REQUESTID"];
                }

                SetTabHighlight();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                gvStockItem.DataSource = "";
                ifMoreInfo.Attributes["src"] = "../Inventory/InventorySpareItemRequestGeneral.aspx";
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

    protected void SetTabHighlight()
    {
        try
        {
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareItemRequestGeneral.aspx"))
            {
                MenuInventoryStockItem.SelectedMenuIndex = 0;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareItemComponentRequest.aspx"))
            {
                MenuInventoryStockItem.SelectedMenuIndex = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStockItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
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
            string REQUESTID = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInventorySpareItemRequest.DeleteSpareItemRequest(new Guid(REQUESTID));
                ViewState["REQUESTID"] = null;
            }
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                if (e.CommandName.ToUpper().Equals("APPROVE"))
                {
                    Guid g = new Guid(REQUESTID);
                    PhoenixInventorySpareItemRequest.ApproveSpareItemRequest(g, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    ViewState["REQUESTID"] = null;
                }
            }
            if(e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["REQUESTID"] = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                ViewState["SPAREITEMID"] = ((RadLabel)e.Item.FindControl("lblSpareItemId")).Text;
                ViewState["DTKEY"] = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
            }
            gvStockItem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
