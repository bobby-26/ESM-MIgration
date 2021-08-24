using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventorySpareItemMove : PhoenixBasePage
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
            toolbarmain.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
            toolbarmain.AddButton("Move", "MOVE",ToolBarDirection.Right);
            MenuInventorySpareMove.AccessRights = this.ViewState;
            MenuInventorySpareMove.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["INTERNALID"] = null;

                lblLocationId.Text = Request.QueryString["locationid"].ToString();
                lblSpareItemId.Text = Request.QueryString["spareitemid"].ToString();
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

        ds = PhoenixInventorySpareItem.ListSpareItem(
            new Guid(lblSpareItemId.Text),
            PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtSpareItem.Text = dr["FLDNAME"].ToString();
        }

        ds = new DataSet();

        ds = PhoenixInventorySpareItemLocation.SpareItemLocationEdit(
            new Guid(lblSpareItemId.Text),
            PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            int.Parse(lblLocationId.Text));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtCurrentLocation.Text = dr["FLDLOCATIONNAME"].ToString();
            txtInStockQuantity.Text = dr["FLDQUANTITY"].ToString();
        }

        BindLocationList();

        txtMoveQuantity.Text = "";
    }

    private void BindLocationList()
	{
		DataTable dt = PhoenixLocation.LocationTreeList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ddlLocationList.DataSource = dt;
        ddlLocationList.DataTextField = "FLDLOCATIONNAME";
        ddlLocationList.DataValueField = "FLDLOCATIONID";
        ddlLocationList.DataBind();
        ddlLocationList.Items.Insert(0, new DropDownListItem("--Select--", ""));
	}    

    protected void MenuInventorySpareMove_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("MOVE"))
            {
                if (Request.QueryString["spareitemid"] != null)
                {
                    if (!IsValidSpareItemMove())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInventorySpareItemLocation.MoveSpareItem(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(lblSpareItemId.Text),
                        decimal.Parse(txtMoveQuantity.Text),
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID, 
                        int.Parse(lblLocationId.Text), int.Parse(ddlLocationList.SelectedValue));

                    ucStatus.Text = "Spare Item successfully moved to the new location";
                    BindDetails();

                    string Script = "";
                    Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", Script, true);
                }
            }
            if (CommandName.ToUpper().Equals("CANCEL"))
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

    private bool IsValidSpareItemMove()
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
