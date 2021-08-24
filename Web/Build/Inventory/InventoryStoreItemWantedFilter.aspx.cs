using System;
using System.Web.UI;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class InventoryStoreItemWantedFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Go", "GO", ToolBarDirection.Right);
                MenuStockItemFilter.MenuList = toolbarmain.Show();

                txtPreferredVendorCode.Attributes.Add("onkeydown", "return false;");
                txtPreferredVendorName.Attributes.Add("onkeydown", "return false;");
                ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
                ddlStockClass.SelectedHard = "407";
                ImgShowMakerVendor.Attributes.Add("onclick", "return showPickList('spnPickListVendor', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131',true);");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuStockItemFilter_TabStripCommand(object sender, EventArgs e)
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
                criteria.Add("ddlStockClass", ddlStockClass.SelectedHard);
                criteria.Add("isGolbleSearch", chkGlobalSearch.Checked.ToString());
                Filter.CurrentStoreItemWantedFilterCriteria = criteria;
                Response.Redirect("../Inventory/InventoryStoreItemWanted.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdVendorClear_Click(object sender, ImageClickEventArgs e)
    {
        txtPreferredVendorCode.Text = "";
        txtPreferredVendorName.Text = "";
        txtVendorId.Text = "";
    }
}
