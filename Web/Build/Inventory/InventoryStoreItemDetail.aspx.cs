using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventoryStoreItemDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuStoreItemDetails.AccessRights = this.ViewState;  
            MenuStoreItemDetails.MenuList = toolbarmain.Show();
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
            if ((Request.QueryString["STOREITEMID"] != null) && (Request.QueryString["STOREITEMID"] != ""))
            {
                DataSet ds = PhoenixInventoryStoreItem.ListStoreItem(new Guid(Request.QueryString["STOREITEMID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                DataRow dr = ds.Tables[0].Rows[0];
                txtItemDetails.Content = dr["FLDDETAIL"].ToString();
                ViewState["dtkey"] = dr["FLDDTKEY"].ToString();

                //Title1.Text += "    (Store Item - " + dr["FLDNUMBER"].ToString() + " )";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuStoreItemDetails_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if ((Request.QueryString["STOREITEMID"] != null) && (Request.QueryString["STOREITEMID"] != ""))
                {
                    PhoenixInventoryStoreItem.UpdateDetailsStoreItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(Request.QueryString["STOREITEMID"])
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
}
