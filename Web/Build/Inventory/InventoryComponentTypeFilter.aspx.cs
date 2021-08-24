using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;


public partial class InventoryComponentTypeFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Go", "GO");
            MenuComponentTypeFilter.MenuList = toolbarmain.Show();          
            if (!IsPostBack)
            {
                txtMakerId.Attributes.Add("style", "visibility:hidden");
                txtVendorId.Attributes.Add("style", "visibility:hidden");
                ddlComponentClass.QuickTypeCode = ((int)PhoenixQuickTypeCode.COMPONENTCLASS).ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuComponentTypeFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("GO"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtNumber", txtNumber.Text);
                criteria.Add("txtName", txtName.Text);
                criteria.Add("txtMakerid", txtMakerId.Text);
                criteria.Add("txtVendorId", txtVendorId.Text);
                criteria.Add("ddlComponentClass", ddlComponentClass.SelectedQuick);
                criteria.Add("txtType", txtType.Text);
                Filter.CurrentComponentTypeFilterCriteria = criteria;
            }

            Response.Redirect("../Inventory/InventoryComponentType.aspx", false);
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


    protected void cmdMakerClear_Click(object sender, ImageClickEventArgs e)
    {
        txtMakerCode.Text = "";
        txtMakerName.Text = "";
        txtMakerId.Text = "";
    }
}
