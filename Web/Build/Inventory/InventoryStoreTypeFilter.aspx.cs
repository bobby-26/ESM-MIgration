using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InventoryStoreTypeFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Go", "GO",ToolBarDirection.Right);
            MenuStoreTypeFilter.AccessRights = this.ViewState;
            MenuStoreTypeFilter.MenuList = toolbarmain.Show();

            txtPreferredVendorCode.Attributes.Add("onkeydown", "return false;");
            txtPreferredVendorName.Attributes.Add("onkeydown", "return false;");
            if(!IsPostBack)
            {
                ImgShowMakerVendor.Attributes.Add("onclick", "return showPickList('spnPickListVendor', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131',true);");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuStoreTypeFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {


            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GO"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();

                criteria.Add("txtNumber", txtNumber.Text);
                criteria.Add("txtName", txtName.Text);
                criteria.Add("txtVendorId", txtVendorId.Text);
                Filter.CurrentStockTypeFilterCriteria = criteria;

            }

            Response.Redirect("../Inventory/InventoryStoreType.aspx");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void cmdClear_Click(object sender, EventArgs e)
    {
        txtPreferredVendorCode.Text = "";
        txtPreferredVendorName.Text = "";
        txtVendorId.Text = "";
    }
}
