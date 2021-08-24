using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventoryStoreItem : PhoenixBasePage
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
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreItem.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStoreItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreItemFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddImageButton("../Inventory/InventoryStoreItem.aspx", "Update Wanted Items", "process.png", "WANTED");
            toolbargrid.AddImageButton("../Inventory/InventoryStoreItem.aspx", "Create Requisition", "copy-requisition.png", "REQN");
            toolbargrid.AddImageButton("../Inventory/InventoryStoreItem.aspx", "Reset Wanted Items", "in-progress.png", "RESET");
            MenuStoreItem.AccessRights = this.ViewState;  
            MenuStoreItem.MenuList = toolbargrid.Show();

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
                toolbarmain.AddButton("Details", "DETAILS");
                toolbarmain.AddButton("Attachment", "ATTACHMENT");
                MenuInventoryStoreItem.AccessRights = this.ViewState;  
                MenuInventoryStoreItem.MenuList = toolbarmain.Show();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["STOREITEMID"] = null;

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;

                if (Request.QueryString["STOREITEMID"] != null && Request.QueryString["SETCURRENTNAVIGATIONTAB"] != null && Request.QueryString["STOREITEMID"].ToString() != "")
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = Request.QueryString["SETCURRENTNAVIGATIONTAB"].ToString();  
                    ViewState["STOREITEMID"] = Request.QueryString["STOREITEMID"].ToString();
                    if (Request.QueryString["VESSELID"] != null)
                    {
                        ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                    }
                }
                if (Request.QueryString["STOREITEMID"] != null)
                {
                    ViewState["STOREITEMID"] = Request.QueryString["STOREITEMID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void InventoryStoreItem_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryStoreItemGeneral.aspx?STOREITEMID=";
            }
            else if (CommandName.ToUpper().Equals("VENDORS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryStoreItemVendor.aspx?VESSELID=" + ViewState["VESSELID"] + "&STOREITEMID=";
            }
            else if (CommandName.ToUpper().Equals("STOCKTRANSACTION"))
            {
                Response.Redirect("../Inventory/InventoryStoreTransactionEntryDetail.aspx?STOREITEMID=" + ViewState["STOREITEMID"]);
            }
            else if (CommandName.ToUpper().Equals("DETAILS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryStoreItemDetail.aspx?STOREITEMID=";
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                GetAttachmentDtkey();
                if (ViewState["DTKEY"] != null)
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
            }
            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                if (ViewState["DTKEY"] != null)
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
                else
                    ifMoreInfo.Attributes["src"] = "../Inventory/InventoryStoreItemGeneral.aspx";
                
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["STOREITEMID"];
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
        if (ViewState["STOREITEMID"] != null &&  ViewState["STOREITEMID"].ToString() != "")
        {
            DataSet ds = PhoenixInventoryStoreItem.ListStoreItem(new Guid(ViewState["STOREITEMID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
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
            string[] alColumns = { "FLDNUMBER", "FLDNAME",  "PREFERREDVENDOR", "FLDROB", "FLDSTOCKVALUE" };
            string[] alCaptions = { "Number", "Name", "Preferred Vender", "ROB", "Stock Value" };
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

            if (Filter.CurrentStoreItemFilterCriteria != null)
            {
                NameValueCollection nvc = Filter.CurrentStoreItemFilterCriteria;
                ds = PhoenixCommonInventory.StoreItemSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    nvc.Get("txtNumber").ToString(),
                    nvc.Get("txtName").ToString(),
                    null,
                    General.GetNullableInteger(nvc.Get("txtVendorId").ToString()),
                    General.GetNullableInteger(nvc.Get("ddlStockClass").ToString()),
                    General.GetNullableInteger(nvc.Get("ddlsubclasstype").ToString()),
                    General.GetNullableString(nvc.Get("txtmaterialnumber").ToString()),
                    sortexpression, sortdirection,
                    1,
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableInteger(nvc.Get("chkROB").ToString()),
                    nvc.Get("txtVendorReference").ToString());
            }
            else
            {
                ds = PhoenixCommonInventory.StoreItemSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    null, null, null, null, null, sortexpression, sortdirection,
                   1,
                    iRowCount, ref iRowCount, ref iTotalPageCount,0, "");
            }

            Response.AddHeader("Content-Disposition", "attachment; filename=StoreItem.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Inventory Store Item</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void StoreItem_TabStripCommand(object sender, EventArgs e)
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
            else if (CommandName.ToUpper().Equals("WANTED"))
            {
                PhoenixInventoryStoreItem.AutomaticStoreWantedUpdate();
                ucStatus.Value = "Status | none";
                ucStatus.Show("Wanted Quantity Updated.");
                //ucStatus.Text = "Wanted Quantity Updated.";
                gvStockItem.Rebind();

            }
            else if (CommandName.ToUpper().Equals("REQN"))
            {
                DataTable dt;
                dt = PhoenixInventoryStoreItem.GetStoreClassForRequisition();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PhoenixInventoryStoreItem.CreateRequisition(int.Parse(dr["FLDVESSELID"].ToString())
                                                                    , General.GetNullableInteger(dr["FLDSTORECLASS"].ToString()));
                    }
                    
                    ucStatus.Show("Requisition Created Successfully..");
                }
                else
                {
                    ucError.ErrorMessage = "No record found";
                    ucError.Visible = true;
                }
            }
            else if (CommandName.ToUpper().Equals("RESET"))
            {
                PhoenixInventoryStoreItem.ResetWantedQuantity();
                ucStatus.Show("All wanted quantity reset to 0");
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
        int iRowCount = 10;
        int iTotalPageCount = 10;

        string[] alColumns = { "FLDNUMBER", "FLDNAME", "PREFERREDVENDOR", "FLDROB", "FLDSTOCKVALUE" };
        string[] alCaptions = { "Number", "Name", "Preferred Vender", "ROB", "Stock Value" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds;

        if (Filter.CurrentStoreItemFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.CurrentStoreItemFilterCriteria;
            ds = PhoenixCommonInventory.StoreItemSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                nvc.Get("txtNumber").ToString(),
                nvc.Get("txtName").ToString(),
                null,
                General.GetNullableInteger(nvc.Get("txtVendorId").ToString()),
                General.GetNullableInteger(nvc.Get("ddlStockClass").ToString()),
                General.GetNullableInteger(nvc.Get("ddlsubclasstype").ToString()),
                General.GetNullableString(nvc.Get("txtmaterialnumber").ToString()),
                sortexpression, sortdirection,
                gvStockItem.CurrentPageIndex + 1,
                gvStockItem.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                General.GetNullableInteger(nvc.Get("chkROB").ToString()),
                nvc.Get("txtVendorReference").ToString());
        }
        else
        {
            ds = PhoenixCommonInventory.StoreItemSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                null, null, null, null, null, sortexpression, sortdirection,
                gvStockItem.CurrentPageIndex + 1,
                gvStockItem.PageSize, ref iRowCount, ref iTotalPageCount, 0,
                "");
        }

        General.SetPrintOptions("gvStoreItem", "Inventory Store Item", alCaptions, alColumns, ds);

        gvStockItem.DataSource = ds;
        gvStockItem.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["STOREITEMID"] == null)
            {
                ViewState["STOREITEMID"] = ds.Tables[0].Rows[0][0].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                //gvStockItem.SelectedIndex = 0;
            }

            if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryStoreItemGeneral.aspx?STOREITEMID=";
            }

            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
            {
                GetAttachmentDtkey();
                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["STOREITEMID"];
            }
            SetRowSelection();
            SetTabHighlight();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ifMoreInfo.Attributes["src"] = "../Inventory/InventoryStoreItemGeneral.aspx";
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void SetRowSelection()
    {
        gvStockItem.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvStockItem.Items)
        {
            if (item.GetDataKeyValue("FLDSTOREITEMID").ToString() == ViewState["STOREITEMID"].ToString())
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
            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryStoreItemGeneral.aspx"))
            {
                MenuInventoryStoreItem.SelectedMenuIndex = 0;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryStoreItemVendor.aspx"))
            {
                MenuInventoryStoreItem.SelectedMenuIndex = 1;
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryStoreTransactionEntryDetail.aspx"))
            {
                MenuInventoryStoreItem.SelectedMenuIndex = 2 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("InventoryStoreItemDetail.aspx"))
            {
                MenuInventoryStoreItem.SelectedMenuIndex = 3 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
            }
            else if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Trim().Contains("CommonFileAttachment.aspx"))
            {
                MenuInventoryStoreItem.SelectedMenuIndex = 4 - (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? 1 : 0);
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

            string storeitemid = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDSTOREITEMID"].ToString();
            DeleteStoreItem(storeitemid);
            ViewState["STOREITEMID"] = null;
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

            RadLabel lblMinQtyFlage = (RadLabel)item.FindControl("lblMinQtyFlage");
            RadLabel lnkStockItemName = (RadLabel)item.FindControl("lnkStockItemName");
            RadLabel lblStoreClass = (RadLabel)item.FindControl("lblStoreClass");
            Int64 result = 0;
            /* CRITICAL CLASS -> 313 */

            if (Int64.TryParse(lblMinQtyFlage.Text, out result))
            {

                item.ForeColor = (result == 1 && lblStoreClass.Text == "313") ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                lnkStockItemName.ForeColor = (result == 1 && lblStoreClass.Text == "313") ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton cmdLocation = (LinkButton)e.Item.FindControl("cmdLocation");
            if (cmdLocation != null && General.GetNullableGuid(drv["FLDSTOREITEMID"].ToString()) != null)
            {
                cmdLocation.Visible = SessionUtil.CanAccess(this.ViewState, cmdLocation.CommandName);
                cmdLocation.Attributes.Add("onclick", "javascript:openNewWindow('location','','" + Session["sitepath"] + "/Inventory/InventoryStoreItemLocation.aspx?STOREITEMID=" + drv["FLDSTOREITEMID"].ToString() + "'); return false;");
            }

            LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAttachment");
            if (cmdAttachment != null)
            {
                cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
                cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('Codehelp1','','" + Session["sitepath"] + "/Inventory/InventoryStoreItemFileAttachment.aspx?name="+drv["FLDNAME"].ToString()+"&storenumber=" + drv["FLDNUMBER"].ToString() + "','','600','450'); return false;");
            }

        }
    }

    protected void gvStockItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "RowClick")
        {
            GridDataItem item = (GridDataItem)e.Item;
            ViewState["STOREITEMID"] = ((RadLabel)item.FindControl("lblStockItemCode")).Text;
            ViewState["DTKEY"] = ((RadLabel)item.FindControl("lbldtkey")).Text;
            ViewState["VESSELID"] = ((RadLabel)item.FindControl("lblVesselId")).Text;
            if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryStoreItemGeneral.aspx?STOREITEMID=";
            }

            if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
            {
                GetAttachmentDtkey();

                ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["STOREITEMID"];
            }
            SetTabHighlight();
        }
    }
    private void DeleteStoreItem(string storeitemid)
    {
      
      PhoenixInventoryStoreItem.DeleteStoreItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(storeitemid));
  
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
