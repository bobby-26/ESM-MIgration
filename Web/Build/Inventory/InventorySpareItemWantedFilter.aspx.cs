using System;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InventorySpareItemWantedFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Go", "GO",ToolBarDirection.Right);
            MenuStockItemFilter.MenuList = toolbarmain.Show();

            txtMakerCode.Attributes.Add("onkeydown", "return false;");
            txtMakerName.Attributes.Add("onkeydown", "return false;");
            txtPreferredVendorCode.Attributes.Add("onkeydown", "return false;");
            txtPreferredVendorName.Attributes.Add("onkeydown", "return false;");

            ImgShowMaker.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131',true);");
            ImgShowMakerVendor.Attributes.Add("onclick", "return showPickList('spnPickListVendor', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131',true);");
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
                criteria.Add("txtMakerid", txtMakerId.Text);
                criteria.Add("txtVendorId", txtVendorId.Text);               
                criteria.Add("isGolbleSearch", chkGlobalSearch.Checked.ToString());
                criteria.Add("chkCritical", chkCritical.Checked==true ? "1" : string.Empty);
                Filter.CurrentSpareItemWantedFilterCriteria = criteria;
                Response.Redirect("../Inventory/InventorySpareItemWanted.aspx", false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdMakerClear_Click(object sender, ImageClickEventArgs e)
    {
        txtMakerCode.Text = "";
        txtMakerName.Text = "";
        txtMakerId.Text = "";
    }

    protected void cmdPreferredVendorCodeClear_Click(object sender, ImageClickEventArgs e)
    {
        txtPreferredVendorCode.Text = "";
        txtPreferredVendorName.Text = "";
        txtVendorId.Text = "";
    }
}
