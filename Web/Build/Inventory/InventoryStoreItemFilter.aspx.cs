using System;
using System.Web.UI;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class InventoryStoreItemFilter : PhoenixBasePage
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
                txtVendorId.Attributes.Add("style", "visibility:hidden");


                txtPreferredVendorCode.Attributes.Add("onkeydown", "return false;");
                txtPreferredVendorName.Attributes.Add("onkeydown", "return false;");
                ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
                ddlStockClass.SelectedHard = "407";

                ViewState["hard"] = PhoenixCommonRegisters.GetHardCode(1, 97, "GEN");
                ddlsubclasstype.DataSource = PhoenixRegistersStoreSubClass.StoresubclassList(int.Parse(ViewState["hard"].ToString()));
                ddlsubclasstype.DataBind();
                ddlsubclasstype.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));


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
                criteria.Add("chkROB", chkROB.Checked == true ? "1" : "0");
                criteria.Add("txtVendorReference", txtVendorReference.Text);
                criteria.Add("ddlsubclasstype", ddlsubclasstype.SelectedValue);
                criteria.Add("txtmaterialnumber", txtmaterialnumber.Text);


                Filter.CurrentStoreItemFilterCriteria = criteria;
                Response.Redirect("../Inventory/InventoryStoreItem.aspx",false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlStockClass_OnTextChangedEvent(object sender, EventArgs e)
    {
        ddlsubclasstype.DataSource = PhoenixRegistersStoreSubClass.StoresubclassList(General.GetNullableInteger(ddlStockClass.SelectedHard));
        ddlsubclasstype.DataBind();
        ddlsubclasstype.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void cmdClear_Click(object sender, EventArgs e)
    {
        txtPreferredVendorCode.Text = "";
        txtPreferredVendorName.Text = "";
        txtVendorId.Text = "";
    }
}
