using System;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class InventorySpareItemFilter : PhoenixBasePage
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

                txtMakerCode.Attributes.Add("onkeydown", "return false;");
                txtMakerName.Attributes.Add("onkeydown", "return false;");
                txtPreferredVendorCode.Attributes.Add("onkeydown", "return false;");
                txtPreferredVendorName.Attributes.Add("onkeydown", "return false;");

                ImgShowMaker.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131',true);");
                ImgShowMakerVendor.Attributes.Add("onclick", "return showPickList('spnPickListVendor', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131',true);");
                ImgShowComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx',true);");
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
                criteria.Add("txtMakerid", txtMakerId.Text);
                criteria.Add("txtVendorId", txtVendorId.Text);               
                criteria.Add("isGolbleSearch", chkGlobalSearch.Checked.ToString());
                criteria.Add("chkCritical", chkCritical.Checked == true ? "1" : string.Empty);
                criteria.Add("txtMakerReference", txtMakerReference.Text);
                criteria.Add("txtDrawing", txtDrawing.Text);
                criteria.Add("chkROB", chkROB.Checked == true ? "1":"0");
                criteria.Add("txtComponentNumber", txtComponentNumber.Text);
                criteria.Add("txtComponentName", txtComponentName.Text);
                criteria.Add("txtmaterialnumber", txtmaterialnumber.Text);

                Filter.CurrentSpareItemFilterCriteria = criteria;
                Response.Redirect("../Inventory/InventorySpareItem.aspx",false );
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdClear_Click(object sender, EventArgs e)
    {
        txtMakerCode.Text = "";
        txtMakerName.Text = "";
        txtMakerId.Text = "";
    }

    protected void cmdVendorClear_Click(object sender, EventArgs e)
    {
        txtPreferredVendorCode.Text = "";
        txtPreferredVendorName.Text = "";
        txtVendorId.Text = "";
    }

    protected void cmdClearComponent_Click(object sender, EventArgs e)
    {
        txtComponentNumber.Text = "";
        txtTmpComponentName.Text = "";
        txtComponentId.Text = "";
    }
}
