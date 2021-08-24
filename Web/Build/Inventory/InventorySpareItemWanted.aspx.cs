using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventorySpareItemWanted : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        try 
        {
            foreach (GridDataItem r in gvStockItemWanted.Items)
            {
                if (r is GridDataItem)
                {
                    Page.ClientScript.RegisterForEventValidation(gvStockItemWanted.UniqueID, "Edit$" + r.RowIndex.ToString());   
                }
            }
            base.Render(writer);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemWanted.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStockItemWanted')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemWantedFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddImageButton("../Inventory/InventorySpareItemWanted.aspx", "Update Wanted Items", "process.png", "WANTED");
            toolbargrid.AddImageButton("../Inventory/InventorySpareItemWanted.aspx", "Create Requisition", "copy-requisition.png", "REQN");
            toolbargrid.AddImageButton("../Inventory/InventorySpareItemWanted.aspx", "Reset Wanted Items", "in-progress.png", "RESET");
            MenuStockItem.AccessRights = this.ViewState;   
            MenuStockItem.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["SPAREITEMID"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                gvStockItemWanted.PageSize = General.ShowRecords(null);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void SpareItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("WANTED"))
            {
                PhoenixInventorySpareItem.AutomaticSpareWantedUpdate();
                ucstatusnew.Show("Wanted Quantity Updated.");
                gvStockItemWanted.Rebind();
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
                    ucstatusnew.Show("Requisition Created Successfully..");
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
                ucstatusnew.Show("All wanted quantity reset to 0");
                BindData();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDCOMPONENTNAME", "MAKER", "FLDQUANTITY", "FLDWANTED", "FLDUNITNAME", "STOCKCLASSNAME" };
            string[] alCaptions = { "Number", "Name", "Component Name", "Maker", "Quantity", "Wanted", "Unit", "Stock Class" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = gvStockItemWanted.PageSize;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds;

            if (Filter.CurrentSpareItemWantedFilterCriteria != null)
            {

                NameValueCollection nvc = Filter.CurrentSpareItemWantedFilterCriteria;
                ds = PhoenixInventorySpareItemWanted.SpareItemWantedSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    nvc.Get("txtNumber").ToString(),
                    nvc.Get("txtName").ToString(),
                    General.GetNullableInteger(nvc.Get("txtMakerId").ToString()),
                    General.GetNullableInteger(nvc.Get("chkCritical").ToString()),
                    General.GetNullableInteger(nvc.Get("txtVendorId").ToString()),
                    sortexpression, sortdirection,
                    1,
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

            }
            else
            {
                ds = PhoenixInventorySpareItemWanted.SpareItemWantedSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID, "", "",
                    null,null, null, sortexpression, sortdirection,
                   1,
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

            }         

            Response.AddHeader("Content-Disposition", "attachment; filename=SpareItemWanted.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Spare Item Wanted</h3></td>");
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
    private void BindData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDCOMPONENTNAME","MAKER", "FLDQUANTITY", "FLDWANTED", "FLDUNITNAME", "STOCKCLASSNAME" };
            string[] alCaptions = { "Number", "Name","Component Name", "Maker", "Quantity", "Wanted","Unit","Stock Class" };
      

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            if (Filter.CurrentSpareItemWantedFilterCriteria != null)
            {

                NameValueCollection nvc = Filter.CurrentSpareItemWantedFilterCriteria;
                ds = PhoenixInventorySpareItemWanted.SpareItemWantedSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    nvc.Get("txtNumber").ToString(),
                    nvc.Get("txtName").ToString(),
                    General.GetNullableInteger(nvc.Get("txtMakerId").ToString()),
                    General.GetNullableInteger(nvc.Get("chkCritical").ToString()),
                    General.GetNullableInteger(nvc.Get("txtVendorId").ToString()),
                    sortexpression, sortdirection,
                    gvStockItemWanted.CurrentPageIndex + 1, gvStockItemWanted.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

            }
            else
            {
                ds = PhoenixInventorySpareItemWanted.SpareItemWantedSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID, "", "",
                    null,null ,null, sortexpression, sortdirection,
                    gvStockItemWanted.CurrentPageIndex + 1, gvStockItemWanted.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

            }

            General.SetPrintOptions("gvStockItemWanted", "Spare Item Wanted", alCaptions, alColumns, ds);

            gvStockItemWanted.DataSource = ds;
            gvStockItemWanted.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["SPAREITEMID"] == null)
                {
                    ViewState["SPAREITEMID"] = ds.Tables[0].Rows[0][0].ToString();
                    gvStockItemWanted.SelectedIndexes.Clear();
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventorySpareItemWantedGeneral.aspx?SPAREITEMID=";
                }

                ifMoreInfo.Attributes["src"] = "../Inventory/InventorySpareItemWantedGeneral.aspx?SPAREITEMID=" + ViewState["SPAREITEMID"] + "";
            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Inventory/InventorySpareItemWantedGeneral.aspx";
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

    protected void gvStockItemWanted_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvStockItemWanted_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null)
            {
                save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
            }

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null)
            {
                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            }
            
            RadLabel lbQty = (RadLabel)e.Item.FindControl("lblQuantity");
            RadLabel lbMinQty = (RadLabel)e.Item.FindControl("lblMinimumQuantity");
            RadLabel lbCritical = (RadLabel)e.Item.FindControl("lblCritical");

            Image img = (Image)e.Item.FindControl("imgFlag");
            if (lbCritical != null && lbCritical.Text.Equals("1"))
            {
                decimal MinQty = (lbMinQty.Text.Trim() != "") ? decimal.Parse(lbMinQty.Text) : 0;
                img.Visible = (decimal.Parse(lbQty.Text) < MinQty) ? true : false;
                img.ToolTip = "ROB is less than Minimum Quantity";
            }
        }
    }

    protected void gvStockItemWanted_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName=="RowClick")
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["SPAREITEMID"] = ((RadLabel)item.FindControl("lblStockItemId")).Text;
                ifMoreInfo.Attributes["src"] = "../Inventory/InventorySpareItemWantedGeneral.aspx?SPAREITEMID=" + ViewState["SPAREITEMID"] + "";
            }
            if(e.CommandName.ToUpper() == "EDIT")
            {
                ViewState["SPAREITEMID"] = ((RadLabel)e.Item.FindControl("lblStockItemId")).Text;
                ifMoreInfo.Attributes["src"] = "../Inventory/InventorySpareItemWantedGeneral.aspx?SPAREITEMID=" + ViewState["SPAREITEMID"] + "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStockItemWanted_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            UpdateStockItemWanted(
                ((RadLabel)eeditedItem.FindControl("lblStockItemId")).Text,
                ((UserControlNumber)eeditedItem.FindControl("txtStockItemWantedQuantityEdit")).Text,
                ""
             );
            gvStockItemWanted.SelectedIndexes.Clear();
            BindData();
            ucStatus.Text = "Wanted Quantity is updated.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
    private void UpdateStockItemWanted(string stockitemid, string wantedquantity, string componentid)
    {
        try
        {
            if (!IsValidStockItemWantedEdit(wantedquantity))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInventorySpareItemWanted.UpdateSpareItemWantedQuantity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 stockitemid
                , General.GetNullableDecimal(wantedquantity)
                , componentid
                );

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidStockItemWantedEdit(string wantedquantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvStockItemWanted;
        decimal result;

        if (wantedquantity.Trim() != "")
        {
            if (decimal.TryParse(wantedquantity, out result) == false)
                ucError.ErrorMessage = "Item wanted quantity should be a valid numeric value.";
        }
        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvStockItemWanted.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStockItemWanted_SortCommand(object sender, GridSortCommandEventArgs e)
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
