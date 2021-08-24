using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inventory;

public partial class InventoryStoreItemMove : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Move", "MOVE");
            toolbarmain.AddButton("Cancel", "CANCEL");
            MenuInventoryStoreMove.AccessRights = this.ViewState;
            MenuInventoryStoreMove.MenuList = toolbarmain.Show();
            MenuInventoryStoreMove.SetTrigger(pnlStoreItemMove);

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["INTERNALID"] = null;

                lblLocationId.Text = Request.QueryString["locationid"].ToString();
                lblStoreItemId.Text = Request.QueryString["storeitemid"].ToString();

                BindDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDetails()
    {
        DataSet ds = new DataSet();

        ds = PhoenixInventoryStoreItem.ListStoreItem(
            new Guid(lblStoreItemId.Text),
            PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtStoreItem.Text = dr["FLDNAME"].ToString();
        }

        //ds = new DataSet();
        //ds = PhoenixLocation.LocationCode(lblLocationId.Text);

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    DataRow dr = ds.Tables[0].Rows[0];

        //    txtCurrentLocation.Text = dr["FLDLOCATIONNAME"].ToString();
        //}

        ds = new DataSet();

        ds = PhoenixInventoryStoreItemLocation.StoreItemLocationEdit(
            new Guid(lblStoreItemId.Text),
            PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            int.Parse(lblLocationId.Text));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtCurrentLocation.Text = dr["FLDLOCATIONNAME"].ToString();
            txtInStockQuantity.Text = dr["FLDQUANTITY"].ToString();
        }

        ddlLocationList.DataSource = PhoenixLocation.LocationTreeList(
            PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ddlLocationList.DataBind();
        ddlLocationList.Visible = true;

        txtMoveQuantity.Text = "";
    }

    protected void MenuInventoryStoreMove_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("MOVE"))
            {
                if (Request.QueryString["storeitemid"] != null)
                {
                    if (!IsValidStoreItemMove())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInventoryStoreItemLocation.MoveStoreItem(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(lblStoreItemId.Text),
                        decimal.Parse(txtMoveQuantity.Text),
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        int.Parse(lblLocationId.Text), int.Parse(ddlLocationList.SelectedValue));

                    ucStatus.Text = "Store Item successfully moved to the new location";
                    BindDetails();

                    string Script = "";
                    Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", Script, true);
                }
            }
            if (dce.CommandName.ToUpper().Equals("CANCEL"))
            {
                String script = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidStoreItemMove()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDecimal(txtMoveQuantity.Text) == null)
            ucError.ErrorMessage = "'Move Quantity' is required.";

        if (General.GetNullableInteger(ddlLocationList.SelectedValue) == null)
            ucError.ErrorMessage = "'Move to' Location is required.";

        if (General.GetNullableDecimal(txtMoveQuantity.Text) > General.GetNullableDecimal(txtInStockQuantity.Text))
            ucError.ErrorMessage = "'Move Quantity' can not be greater than 'In Stock' quantity.";

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
