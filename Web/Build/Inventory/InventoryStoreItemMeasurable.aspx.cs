using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class InventoryStoreItemMeasurable : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem r in gvStoreItem.Items)
        {
            if (r is GridDataItem)
            {
                Page.ClientScript.RegisterForEventValidation(gvStoreItem.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["STOREITEMID"] = null;           

            if ((Request.QueryString["STOREITEMID"] != null) && (Request.QueryString["STOREITEMID"] != ""))
                ViewState["STOREITEMID"] = Request.QueryString["STOREITEMID"].ToString();

            if ((Request.QueryString["UNITFOR"] != "DUMMY") && (Request.QueryString["UNITFOR"] != ""))
                ddlunit.SelectedUnit = Request.QueryString["UNITFOR"].ToString();
            else
            {
                ucError.ErrorMessage = "Please Select the Unit to Proceed Further.";
                ucError.HeaderMessage = "";
                ucError.Visible = true;
            }
            if ((Request.QueryString["NAME"] != null) && (Request.QueryString["NAME"] != ""))
                this.Page.Title = Request.QueryString["NAME"].ToString() + " - Measurable Units";
            else
                this.Page.Title = "Measurable Units";
            gvStoreItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    private void BindData()
    {
        try
        {
            if (ViewState["STOREITEMID"] != null)
            {
                int iRowCount = 0;
                int iTotalPageCount = 10;

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                DataTable dt = PhoenixInventoryStoreItem.StoreItemMeasurableSearch (
                    General.GetNullableGuid(ViewState["STOREITEMID"].ToString())
                     , General.GetNullableInteger(Request.QueryString["UNITFOR"].ToString())
                     , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , sortexpression, sortdirection,
                   gvStoreItem.CurrentPageIndex + 1, gvStoreItem.PageSize,
                   ref iRowCount,
                   ref iTotalPageCount);

                gvStoreItem.DataSource = dt;
                gvStoreItem.VirtualItemCount = iRowCount;
            }
            else
            {
                ucError.ErrorMessage = "Please Select the Store item to Proceed Further.";
                ucError.HeaderMessage = "";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.HeaderMessage = "";
            ucError.Visible = true;
        }
    }
    protected void gvStoreItem_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvStoreItem_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridFooterItem)
        {
            GridFooterItem footerItem = (GridFooterItem)gvStoreItem.MasterTableView.GetItems(GridItemType.Footer)[0];
            LinkButton db = (LinkButton)footerItem.FindControl("cmdAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }

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
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
            }
            UserControlUnit ucUnit = (UserControlUnit)e.Item.FindControl("ddlStoreUnit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucUnit != null) ucUnit.SelectedUnit = drv["FLDUNIT"].ToString();
        }
    }
    protected void gvStoreItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridFooterItem footerItem = (GridFooterItem)gvStoreItem.MasterTableView.GetItems(GridItemType.Footer)[0];
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidUnitMeasurable(((UserControlUnit)footerItem.FindControl("ddlStoreUnit")).SelectedUnit
                        , ((UserControlNumber)footerItem.FindControl("txtConversionFactorAdd")).Text
                        ))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInventoryStoreItem.InsertStoreItemMeasurable(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(ViewState["STOREITEMID"].ToString())
                    , int.Parse(Request.QueryString["UNITFOR"].ToString())
                    , General.GetNullableInteger(((UserControlUnit)footerItem.FindControl("ddlStoreUnit")).SelectedUnit)
                    , General.GetNullableInteger(((UserControlNumber)footerItem.FindControl("txtConversionFactorAdd")).Text)
                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    );
                gvStoreItem.Rebind();
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
    protected void gvStoreItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            string factor = ((UserControlNumber)eeditedItem.FindControl("txtConversionFactoredit")).Text;
            if (General.GetNullableDecimal(factor).HasValue)
            {
                PhoenixInventoryStoreItem.UpdateStoreItemMeasurable(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(((Label)eeditedItem.FindControl("lblMeasurableid")).Text.ToString())
                , int.Parse(factor)
                );
                gvStoreItem.SelectedIndexes.Clear();
                BindData();
            }
            else
            {
                ucError.ErrorMessage = "Conversion Factor is Required.";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreItem_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            PhoenixInventoryStoreItem.DeleteStoreItemMeasurable(
                    new Guid(((Label)eeditedItem.FindControl("lblMeasurableid")).Text.ToString())
                    );
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidUnitMeasurable(string unitid, string factor)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (unitid.Trim().Equals("Dummy") || unitid.Trim().Equals(""))
            ucError.ErrorMessage = "Unit is required.";
        if (factor.Trim().Equals(""))
            ucError.ErrorMessage = "Conversion Factor is required.";

        return (!ucError.IsError);
    }
}
