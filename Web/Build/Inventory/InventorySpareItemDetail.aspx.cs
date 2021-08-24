using System;
using System.Data;
using System.Web;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventorySpareItemDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuStockItemDetails.AccessRights = this.ViewState;   
            MenuStockItemDetails.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                BindFields();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFields()
    {
        try
        {
            if ((Request.QueryString["SPAREITEMID"] != null) && (Request.QueryString["SPAREITEMID"] != ""))
            {
                DataSet ds = PhoenixInventorySpareItem.ListSpareItem(new Guid(Request.QueryString["SPAREITEMID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                DataRow dr = ds.Tables[0].Rows[0];
                txtItemDetails.Content = dr["FLDDETAIL"].ToString();
                ViewState["dtkey"] = dr["FLDDTKEY"].ToString(); 
                //Title1.Text += "    (Spare Item - " + dr["FLDNUMBER"].ToString() + " )";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuStockItemDetails_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDetail())
                {
                    ucError.Visible = true;
                    return;
                }
                if ((Request.QueryString["SPAREITEMID"] != null) && (Request.QueryString["SPAREITEMID"] != ""))
                {
                    PhoenixInventorySpareItem.UpdateDetailsSpareItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(Request.QueryString["SPAREITEMID"])
                        , txtItemDetails.Content.ToString());

                    ucStatus.Text = "Details Saved.";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidDetail()
    {

        ucError.HeaderMessage = "Please provide the following required information.";

        if (txtItemDetails.Content.ToString().Trim().Equals(""))
            ucError.ErrorMessage = "Details is required.";

        return (!ucError.IsError);
    }
}
