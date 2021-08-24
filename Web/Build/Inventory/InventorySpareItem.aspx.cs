using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventorySpareItem : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItem.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStockItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddImageButton("../Inventory/InventorySpareItem.aspx", "Update Wanted Items", "process.png", "WANTED");
            toolbargrid.AddImageButton("../Inventory/InventorySpareItem.aspx", "Create Requisition", "copy-requisition.png", "REQN");
            toolbargrid.AddImageButton("../Inventory/InventorySpareItem.aspx", "Reset Wanted Items", "in-progress.png", "RESET");
            MenuStockItem.AccessRights = this.ViewState;  
            MenuStockItem.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                gvStockItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("General", "GENERAL");
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    toolbarmain.AddButton("Vendors", "VENDORS");
                }
                toolbarmain.AddButton("Transaction", "STOCKTRANSACTION");
                toolbarmain.AddButton("Components", "COMPONENTS");
                toolbarmain.AddButton("Details", "DETAILS");
                toolbarmain.AddButton("Attachment", "ATTACHMENT");               
                MenuInventoryStockItem.AccessRights = this.ViewState;  
                MenuInventoryStockItem.MenuList = toolbarmain.Show();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["SPAREITEMID"] = null;

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;

                if (Request.QueryString["SPAREITEMID"] != null && Request.QueryString["SETCURRENTNAVIGATIONTAB"] != null && Request.QueryString["SPAREITEMID"].ToString() !="")
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = Request.QueryString["SETCURRENTNAVIGATIONTAB"].ToString(); 
                    ViewState["SPAREITEMID"] = Request.QueryString["SPAREITEMID"].ToString();
                    if (Request.QueryString["VESSELID"] != null)
                    {
                        ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                    }
                }
                if (Request.QueryString["SPAREITEMID"] != null)
                {
                    ViewState["SPAREITEMID"] = Request.QueryString["SPAREITEMID"].ToString();
                }
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

            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventorySpareItemGeneral.aspx?SPAREITEMID=";
            }
            else if (CommandName.ToUpper().Equals("VENDORS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventorySpareItemVendor.aspx?VESSELID=" + ViewState["VESSELID"] + "&SPAREITEMID=";
            }
            else if (CommandName.ToUpper().Equals("STOCKTRANSACTION"))
            {
                Response.Redirect("../Inventory/InventorySpareTransactionEntryDetail.aspx?SPAREITEMID=" + ViewState["SPAREITEMID"]);
            }
            else if (CommandName.ToUpper().Equals("COMPONENTS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventorySpareItemComponent.aspx?SPAREITEMID=";
            }
            else if (CommandName.ToUpper().Equals("DETAILS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventorySpareItemDetail.aspx?SPAREITEMID=";
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                GetAttachmentDtkey();
                if( ViewState["DTKEY"]!=null)
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
            }         
            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                if (ViewState["DTKEY"] != null)
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
                else
                    ifMoreInfo.Attributes["src"] = "../Inventory/InventorySpareItemGeneral.aspx";
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["SPAREITEMID"];
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void GetAttachmentDtkey()
    {
        if (ViewState["SPAREITEMID"] != null  && (ViewState["SPAREITEMID"].ToString()!=""))
        {
            DataSet ds = PhoenixInventorySpareItem.ListSpareItem(new Guid(ViewState["SPAREITEMID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
        }
    }

    protected void ShowExcel()
    {
        try
        {

            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDNAME",  "MAKER", "FLDMAKERREFERENCEFULLDETAILS", "PREFERREDVENDOR", "FLDROB" };
            string[] alCaptions = { "Number", "Name", "Maker", "Maker's Reference", "Preferred Vender", "ROB" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = gvStockItem.PageSize;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds;
            if (Filter.CurrentSpareItemFilterCriteria != null)
            {
                NameValueCollection nvc = Filter.CurrentSpareItemFilterCriteria;
                ds = PhoenixCommonInventory.SpareItemSearch(
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    nvc.Get("txtNumber").ToString(),
                    nvc.Get("txtName").ToString(),
                    General.GetNullableInteger(nvc.Get("txtMakerid").ToString()),
                    General.GetNullableInteger(nvc.Get("txtVendorId").ToString()),
                    null,
                    (byte?)General.GetNullableInteger(nvc.Get("chkCritical")),
                     General.GetNullableString(nvc.Get("txtMakerReference").ToString()),
                     General.GetNullableString(nvc.Get("txtDrawing").ToString()),
                    sortexpression, sortdirection,
                    1,
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableInteger(nvc.Get("chkROB").ToString()),
                    nvc.Get("txtComponentNumber").ToString(),
                    nvc.Get("txtComponentName").ToString()
                    , General.GetNullableString(nvc.Get("txtmaterialnumber").ToString())
                    );

            }
            else
            {
                ds = PhoenixCommonInventory.SpareItemSearch(
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                    1, iRowCount, ref iRowCount, ref iTotalPageCount,0, null, null);
            }
            General.ShowExcel("Inventory Spare Item", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);           
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
                gvStockItem.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("WANTED"))
            {
                PhoenixInventorySpareItem.AutomaticSpareWantedUpdate();
                ucStatus.Show("Wanted Quantity Updated.");
                gvStockItem.Rebind();
            }
            if (CommandName.ToUpper().Equals("REQN"))
            {
                DataTable dt;
                dt = PhoenixInventorySpareItem.GetComponentListForRequisition();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PhoenixInventorySpareItem.CreateRequisition(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                    , General.GetNullableGuid(dr["FLDCOMPONENTID"].ToString()));
                    }
                    ucStatus.Show("Requisition Created Successfully..");
                }
                else
                {
                    ucError.ErrorMessage = "No record found";
                    ucError.Visible = true;
                }
            }
            if (CommandName.ToUpper().Equals("RESET"))
            {
                PhoenixInventorySpareItem.ResetWantedQuantity();
                ucStatus.Show("All wanted quantity reset to 0");
                BindData();
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
        int iRowCount = 10;
        int iTotalPageCount = 10;

        string[] alColumns = { "FLDNUMBER", "FLDNAME", "MAKER", "FLDMAKERREFERENCEFULLDETAILS", "PREFERREDVENDOR", "FLDROB" };
        string[] alCaptions = { "Number", "Name", "Maker", "Maker's Reference", "Preferred Vender", "ROB" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds;

        if (Filter.CurrentSpareItemFilterCriteria != null)
        {

            NameValueCollection nvc = Filter.CurrentSpareItemFilterCriteria;

            ds = PhoenixCommonInventory.SpareItemSearch(
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                nvc.Get("txtNumber").ToString(),
                nvc.Get("txtName").ToString(),
                General.GetNullableInteger(nvc.Get("txtMakerid").ToString()),
                General.GetNullableInteger(nvc.Get("txtVendorId").ToString()),
                null,
                (byte?)General.GetNullableInteger(nvc.Get("chkCritical")),
                 General.GetNullableString(nvc.Get("txtMakerReference").ToString()),
                 General.GetNullableString(nvc.Get("txtDrawing").ToString()),
                sortexpression, sortdirection,
                gvStockItem.CurrentPageIndex + 1,
                gvStockItem.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                General.GetNullableInteger(nvc.Get("chkROB").ToString()),
                nvc.Get("txtComponentNumber").ToString(),
                nvc.Get("txtComponentName").ToString()
                , nvc.Get("txtmaterialnumber")!=null ?General.GetNullableString(nvc.Get("txtmaterialnumber").ToString()):null
                );
        }
        else
        {

            ds = PhoenixCommonInventory.SpareItemSearch(
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                gvStockItem.CurrentPageIndex + 1,
                gvStockItem.PageSize, ref iRowCount, ref iTotalPageCount, 0, null, null);
        }


        General.SetPrintOptions("gvStockItem", "Inventory Spare Item", alCaptions, alColumns, ds);

        gvStockItem.DataSource = ds;
        gvStockItem.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["SPAREITEMID"] == null)
            {
                ViewState["SPAREITEMID"] = ds.Tables[0].Rows[0][0].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                //gvStockItem.SelectedIndex = 0;
            }

            if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventorySpareItemGeneral.aspx?SPAREITEMID=";
            }

            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
            {
                GetAttachmentDtkey();
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["SPAREITEMID"];
            }
            SetRowSelection();
            SetTabHighlight();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ifMoreInfo.Attributes["src"] = "../Inventory/InventorySpareItemGeneral.aspx";
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void SetRowSelection()
    {
        gvStockItem.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvStockItem.Items)
        {
            if (item.GetDataKeyValue("FLDSPAREITEMID").ToString() == ViewState["SPAREITEMID"].ToString())
            {
                gvStockItem.SelectedIndexes.Add(item.ItemIndex);
                PhoenixInventorySpareItem.SpareItemNumber = ((RadLabel)item.FindControl("lnkStockItemName")).Text;
                ViewState["DTKEY"] = ((RadLabel)item.FindControl("lbldtkey")).Text;
            }
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
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareItemGeneral.aspx"))
            {
                MenuInventoryStockItem.SelectedMenuIndex = 0;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareItemVendor.aspx"))
            {
                MenuInventoryStockItem.SelectedMenuIndex = 1;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareTransactionEntryDetail.aspx"))
            {
                MenuInventoryStockItem.SelectedMenuIndex = 2 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareItemComponent.aspx"))
            {
                MenuInventoryStockItem.SelectedMenuIndex = 3 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventorySpareItemDetail.aspx"))
            {
                MenuInventoryStockItem.SelectedMenuIndex = 4 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("CommonFileAttachment.aspx"))
            {
                MenuInventoryStockItem.SelectedMenuIndex = 5 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
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
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStockItem_DeleteCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            string spareitemid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDSPAREITEMID"].ToString();
            PhoenixInventorySpareItem.DeleteSpareItem(new Guid(spareitemid));
            ViewState["SPAREITEMID"] = null;
            BindData();
            gvStockItem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStockItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
            Image img = (Image)e.Item.FindControl("imgFlag");
            if (drv["MINQTYFLAGE"].ToString() == "1")
            {
                img.Visible = true;
                img.ToolTip = "ROB is less than Minimum Level";
            }
            else
                img.Visible = false;

            RadLabel lb = (RadLabel)e.Item.FindControl("lblMakerReference");
            //RadLabel lblMakerRefFullDetails = (RadLabel)e.Item.FindControl("lblMarkerReferencFullDetails");
            //UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipMakerReference");
            //lb.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            //lb.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

            LinkButton cmdLocation = (LinkButton)e.Item.FindControl("cmdLocation");
            if (cmdLocation != null && General.GetNullableGuid(drv["FLDSPAREITEMID"].ToString()) != null)
            {
                cmdLocation.Visible = SessionUtil.CanAccess(this.ViewState, cmdLocation.CommandName);
                cmdLocation.Attributes.Add("onclick", "javascript:openNewWindow('location','','" + Session["sitepath"] + "/Inventory/InventorySpareItemLocation.aspx?SPAREITEMID=" + drv["FLDSPAREITEMID"].ToString() + "'); return false;");
            }
        }
    }

    protected void gvStockItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "RowClick")
        {
            GridDataItem item = (GridDataItem)e.Item;
            ViewState["SPAREITEMID"] = ((RadLabel)item.FindControl("lblStockItemCode")).Text;
            ViewState["DTKEY"] = ((RadLabel)item.FindControl("lbldtkey")).Text;
            ViewState["VESSELID"] = ((RadLabel)item.FindControl("lblVesselId")).Text;
            if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventorySpareItemGeneral.aspx?SPAREITEMID=";
            }

            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
            {
                GetAttachmentDtkey();

                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["SPAREITEMID"];
            }
            SetTabHighlight();
        }
    }

    protected void gvStockItem_SortCommand(object sender, GridSortCommandEventArgs e)
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
