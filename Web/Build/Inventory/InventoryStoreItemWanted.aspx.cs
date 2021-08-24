using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventoryStoreItemWanted : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        try
        {
            foreach (GridDataItem r in gvStoreItemWanted.Items)
            {
                if (r is GridDataItem)
                {
                    Page.ClientScript.RegisterForEventValidation(gvStoreItemWanted.UniqueID, "Edit$" + r.RowIndex.ToString());
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
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreItemWanted.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStoreItemWanted')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreItemWantedFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddImageButton("../Inventory/InventoryStoreItemWanted.aspx", "Update Wanted Items", "process.png", "WANTED");
            toolbargrid.AddImageButton("../Inventory/InventoryStoreItemWanted.aspx", "Create Requisition", "copy-requisition.png", "REQN");
            toolbargrid.AddImageButton("../Inventory/InventoryStoreItemWanted.aspx", "Reset Wanted Items", "in-progress.png", "RESET");
            MenuStoreItemWanted.AccessRights = this.ViewState;  
            MenuStoreItemWanted.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["STOREITEMID"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                gvStoreItemWanted.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void StoreItemWanted_TabStripCommand(object sender, EventArgs e)
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
            else if (CommandName.ToUpper().Equals("WANTED"))
            {
                PhoenixInventoryStoreItem.AutomaticStoreWantedUpdate();
                ucstatusnew.Value = "Status | none";
                ucstatusnew.Show("Wanted Quantity Updated.");
             
                gvStoreItemWanted.Rebind();

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

                    ucstatusnew.Show("Requisition Created Successfully..");
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
                ucstatusnew.Show("All wanted quantity reset to 0");
                gvStoreItemWanted.Rebind();
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
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "MAKER", "FLDQUANTITY", "FLDWANTED", "FLDUNITNAME", "STOCKCLASSNAME" };
            string[] alCaptions = { "Number", "Name", "Maker", "Quantity", "Wanted","Unit","Store Type" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = gvStoreItemWanted.PageSize;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds;

            if (Filter.CurrentStoreItemWantedFilterCriteria != null)
            {

                NameValueCollection nvc = Filter.CurrentStoreItemWantedFilterCriteria;
                ds = PhoenixInventoryStoreItemWanted.StoreItemWantedSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    nvc.Get("txtNumber").ToString(),
                    nvc.Get("txtName").ToString(),
                    null,
                    General.GetNullableInteger(nvc.Get("txtVendorId").ToString()),
                    General.GetNullableInteger(nvc.Get("ddlStockClass").ToString()),
                    sortexpression, sortdirection,
                    1,
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

            }
            else
            {
                ds = PhoenixInventoryStoreItemWanted.StoreItemWantedSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID, "", "",
                    null, null, null, sortexpression, sortdirection,
                    1,
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

            }

            Response.AddHeader("Content-Disposition", "attachment; filename=StoreItemWanted.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Store Item Wanted</h3></td>");
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

            string[] alColumns = { "FLDNUMBER", "FLDNAME", "MAKER", "FLDQUANTITY", "FLDWANTED", "FLDUNITNAME", "STOCKCLASSNAME" };
            string[] alCaptions = { "Number", "Name", "Maker", "Quantity", "Wanted", "Unit", "Store Type" }; 

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            if (Filter.CurrentStoreItemWantedFilterCriteria != null)
            {
               
                NameValueCollection nvc = Filter.CurrentStoreItemWantedFilterCriteria;
                ds = PhoenixInventoryStoreItemWanted.StoreItemWantedSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    nvc.Get("txtNumber").ToString(),
                    nvc.Get("txtName").ToString(),
                    null,
                    General.GetNullableInteger(nvc.Get("txtVendorId").ToString()),
                    General.GetNullableInteger(nvc.Get("ddlStockClass").ToString()),
                    sortexpression, sortdirection,
                    gvStoreItemWanted.CurrentPageIndex + 1, gvStoreItemWanted.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

            }
            else
            {
                ds = PhoenixInventoryStoreItemWanted.StoreItemWantedSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID, "", "",
                    null, null,null, sortexpression, sortdirection,
                    gvStoreItemWanted.CurrentPageIndex + 1, gvStoreItemWanted.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

            }


            General.SetPrintOptions("gvStoreItemWanted", "Store Item Wanted", alCaptions, alColumns, ds);
            gvStoreItemWanted.DataSource = ds;
            gvStoreItemWanted.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {

                if (ViewState["STOREITEMID"] == null)
                {
                    ViewState["STOREITEMID"] = ds.Tables[0].Rows[0][0].ToString();
                    gvStoreItemWanted.SelectedIndexes.Clear();
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryStoreItemWantedGeneral.aspx?STOREITEMID=";
                }
                ifMoreInfo.Attributes["src"] = "../Inventory/InventoryStoreItemWantedGeneral.aspx?STOREITEMID=" + ViewState["STOREITEMID"] + "";


            }
            else
            {
                ifMoreInfo.Attributes["src"] = "../Inventory/InventoryStoreItemWantedGeneral.aspx";
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

    protected void gvStoreItemWanted_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvStoreItemWanted_ItemDataBound(object sender, GridItemEventArgs e)
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
        }
    }

    protected void gvStoreItemWanted_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName=="RowClick")
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["STOREITEMID"] = ((RadLabel)item.FindControl("lblStockItemId")).Text;
                ifMoreInfo.Attributes["src"] = "../Inventory/InventoryStoreItemWantedGeneral.aspx?STOREITEMID=" + ViewState["STOREITEMID"] + "";
            }
            if (e.CommandName.ToUpper() == "EDIT")
            {
                ViewState["STOREITEMID"] = ((RadLabel)e.Item.FindControl("lblStockItemId")).Text;
                ifMoreInfo.Attributes["src"] = "../Inventory/InventoryStoreItemWantedGeneral.aspx?SPAREITEMID=" + ViewState["STOREITEMID"] + "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreItemWanted_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            UpdateStockItemWanted(
                ((RadLabel)eeditedItem.FindControl("lblStockItemId")).Text,
                ((UserControlNumber)eeditedItem.FindControl("txtStockItemWantedQuantityEdit")).Text,
                ""
             );
            gvStoreItemWanted.SelectedIndexes.Clear();
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
            PhoenixInventoryStoreItemWanted.UpdateStoreItemWantedQuantity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
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
        RadGrid _gridView = gvStoreItemWanted;
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
            gvStoreItemWanted.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreItemWanted_SortCommand(object sender, GridSortCommandEventArgs e)
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
