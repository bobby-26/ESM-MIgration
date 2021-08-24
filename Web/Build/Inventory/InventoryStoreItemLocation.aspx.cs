using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventoryStoreItemLocation : PhoenixBasePage
{

    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem r in gvLocation.Items)
        {
            if (r is GridDataItem)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
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

                BindLocationTreeData();
                BindLocationListData();
                
                divLocationTree.Visible = false;
                divLocationList.Visible = true;

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

            int iRowCount = 0;
            int iTotalPageCount = 0;
            decimal iStockItemCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            string storeitemid = (Request.QueryString["STOREITEMID"] == null) ? null : (Request.QueryString["STOREITEMID"].ToString());

            DataSet ds = PhoenixInventoryStoreItemLocation.StoreItemLocationSearch(storeitemid, PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                null, null,
                sortexpression, sortdirection,
                gvLocation.CurrentPageIndex + 1,
                gvLocation.PageSize, ref iRowCount, ref iTotalPageCount, ref iStockItemCount);

            gvLocation.DataSource = ds;
            gvLocation.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                lblTotalStock.Text = iStockItemCount.ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindLocationListData()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixLocation.LocationTree(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                repLocationList.DataSource = ds.Tables[0];
                repLocationList.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
              
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLocation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void BindLocationTreeData()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixLocation.LocationTree(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            tvwLocation.DataTextField = "FLDLOCATIONNAME";
            tvwLocation.DataValueField = "FLDLOCATIONID";
            tvwLocation.DataFieldParentID = "FLDPARENTLOCATIONID";
            tvwLocation.RootText = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
            tvwLocation.PopulateTree(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void tvwLocation_NodeClickEvent(object sender, EventArgs e)
    {
        try
        {
            RadTreeNodeEventArgs tvsne = (RadTreeNodeEventArgs)e;
            lblSelectedNode.Visible = false;
            lblSelectedNode.Text = tvsne.Node.Value.ToString();
            GridFooterItem footerItem = (GridFooterItem)gvLocation.MasterTableView.GetItems(GridItemType.Footer)[0];
            if (lblSelectedNode.Text == "Root")
            {
                ((RadTextBox)footerItem.FindControl("lblLocationIdAdd")).Text = "";
                ((RadTextBox)footerItem.FindControl("txtLocationNameAdd")).Text = "";
                ((RadTextBox)footerItem.FindControl("txtItemQuantityAdd")).Text = "";
            }
            else
            {
                ((RadTextBox)footerItem.FindControl("txtLocationIdAdd")).Text = tvsne.Node.Value.ToString();
                ((RadTextBox)footerItem.FindControl("txtLocationNameAdd")).Text = tvsne.Node.Text.ToString();
                ((RadTextBox)footerItem.FindControl("txtItemQuantityAdd")).Text = "0";
                lblSelectedNode.Text = tvsne.Node.Value.ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertStockItemLocation(string locationid, string quantity)
    {
        try
        {


            if (!IsValidStoreItemLocationAdd(locationid,quantity))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInventoryStoreItemLocation.InsertStoreItemLocation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                Convert.ToInt32(locationid), new Guid(Request.QueryString["STOREITEMID"]), 
                PhoenixSecurityContext.CurrentSecurityContext.VesselID, Convert.ToDecimal(quantity));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void lnkLocationName(object sender, CommandEventArgs e)
    {
        try
        {

            string[] locationvalue = e.CommandArgument.ToString().Split(',');
            GridFooterItem footerItem = (GridFooterItem)gvLocation.MasterTableView.GetItems(GridItemType.Footer)[0];
            ((RadTextBox)footerItem.FindControl("txtLocationIdAdd")).Text = locationvalue[0];
            ((RadTextBox)footerItem.FindControl("txtLocationNameAdd")).Text = locationvalue[1];
            ((RadTextBox)footerItem.FindControl("txtItemQuantityAdd")).Text = "0";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DefaultStockItemLocation(string stockitemlocationid)
    {

        try
        {
            PhoenixInventoryStoreItemLocation.DefaultStoreItemLocation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                new Guid(stockitemlocationid), new Guid(Request.QueryString["STOREITEMID"]),PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void UpdateStockItemLocationQuantity(string stockitemlocationid, string quantity)
    //{
    //    try
    //    {
    //        if (!IsValidStoreItemLocationEdit(quantity))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        PhoenixInventoryStoreItemLocation.UpdateStoreItemLocationQuantity(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(stockitemlocationid), Convert.ToDecimal(quantity), new Guid(Request.QueryString["STOREITEMID"]));
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private void DeleteStockItemLocation(string stockitemlocationid)
    //{
    //    try
    //    {
    //        PhoenixInventoryStoreItemLocation.DeleteStoreItemLocation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(stockitemlocationid));
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private bool IsValidStoreItemLocationEdit(string quantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvLocation;
        decimal result;

        if (quantity.Trim() == "")
        {
            ucError.ErrorMessage = "Please enter the quantity.";
        }

        if (quantity.Trim() != "")
        {
            if (decimal.TryParse(quantity, out result) == false)
                ucError.ErrorMessage = "Quantity should be a valid numeric value.";
        }

        return (!ucError.IsError);
    }

    private bool IsValidStoreItemLocationAdd(string locationid ,string quantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvLocation;
        decimal result;

        if (locationid.Trim() == "")
        {
            ucError.ErrorMessage = "Please select the location from list or tree.";
        }

        if (quantity.Trim() == "")
        {
            ucError.ErrorMessage = "Please enter the quantity.";
        }

        if (quantity.Trim() != "")
        {
            if (decimal.TryParse(quantity, out result) == false)
                ucError.ErrorMessage = "Quantity should be a valid numeric value.";
        }

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvLocation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            //if (edit != null)
            //{
            //    edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            //}

            //LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            //if (save != null)
            //{
            //    save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
            //}

            //LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            //if (cancel != null)
            //{
            //    cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            //}
            RadLabel lblIsdefault = (RadLabel)e.Item.FindControl("lblLocationIsDefault");
            ImageButton imgFlag = (ImageButton)e.Item.FindControl("imgFlag");

            Int64 result = 0;
            if (Int64.TryParse(lblIsdefault.Text, out result))
            {
                imgFlag.ImageUrl = (result == 1) ? Session["images"] + "/14.png" : Session["images"] + "/spacer.gif";
            }
        }
    }

    protected void gvLocation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridFooterItem footerItem = (GridFooterItem)gvLocation.MasterTableView.GetItems(GridItemType.Footer)[0];
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertStockItemLocation
                        (
                         ((RadTextBox)footerItem.FindControl("txtLocationIdAdd")).Text,
                         ((RadTextBox)footerItem.FindControl("txtItemQuantityAdd")).Text
                        );
                BindData();
                gvLocation.Rebind();

            }
            if (e.CommandName.ToUpper().Equals("DEFAULT"))
            {
                GridDataItem r = e.Item as GridDataItem;
                DefaultStockItemLocation
                (
                    ((RadLabel)r.FindControl("lblStockItemLocationId")).Text

                );
                BindData();
                gvLocation.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLocation_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        //try
        //{
        //    GridEditableItem eeditedItem = e.Item as GridEditableItem;
        //    UpdateStockItemLocationQuantity
        //                    (
        //                    ((RadLabel)eeditedItem.FindControl("lblStockItemLocationIdEdit")).Text,
        //                    ((UserControlDecimal)eeditedItem.FindControl("txtItemQuantityEdit")).Text
        //                    );
        //    gvLocation.SelectedIndexes.Clear();
        //    BindData();
        //    gvLocation.Rebind();
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }

    protected void gvLocation_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        //try
        //{
        //    GridEditableItem eeditedItem = e.Item as GridEditableItem;

        //    DeleteStockItemLocation
        //            (
        //                ((RadLabel)eeditedItem.FindControl("lblStockItemLocationId")).Text
        //            );
        //    BindData();
        //    gvLocation.Rebind();
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }

    protected void ddlLocationAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddlLocationAdd.SelectedValue == "1")
            {
                divLocationTree.Visible = false;
                divLocationList.Visible = true;
                BindLocationListData();
            }

            if (ddlLocationAdd.SelectedValue == "2")
            {
                divLocationList.Visible = false;
                divLocationTree.Visible = true;
                BindLocationTreeData();
            }
            GridFooterItem footerItem = (GridFooterItem)gvLocation.MasterTableView.GetItems(GridItemType.Footer)[0];
            ((RadTextBox)footerItem.FindControl("txtLocationIdAdd")).Text = "";
            ((RadTextBox)footerItem.FindControl("txtLocationNameAdd")).Text = "";
            ((RadTextBox)footerItem.FindControl("txtItemQuantityAdd")).Text = "0";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
