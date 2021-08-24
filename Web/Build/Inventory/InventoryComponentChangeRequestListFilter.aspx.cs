using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Inventory_InventoryComponentChangeRequestListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Go", "GO", ToolBarDirection.Right);
            MenuComponentFilter.MenuList = toolbarmain.Show();
            txtMakerId.Attributes.Add("style", "visibility:hidden");
            txtVendorId.Attributes.Add("style", "visibility:hidden");
            //txtComponentClassId.Attributes.Add("style", "visibility:hidden");

            //txtMakerCode.Attributes.Add("onkeydown", "return false;");
            //txtMakerName.Attributes.Add("onkeydown", "return false;");
            //txtPreferredVendorCode.Attributes.Add("onkeydown", "return false;");
            //txtPreferredVendorName.Attributes.Add("onkeydown", "return false;");
            //txtComponentClassName.Attributes.Add("onkeydown", "return false;");
            //txtShortClassName.Attributes.Add("onkeydown", "return false;"); 

            //if (Request.QueryString["go"] != null)
            //    Title1.Text = "Component - Change Request Filter";

            //if (Filter.CurrentPlannedMaintenanceViewFilter != null && Request.QueryString["go"] == null)
            //{
            //    Response.Redirect("../Inventory/InventoryComponentTree.aspx");
            //}

           // ImgShowMaker.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131', true);");
           // ImgShowMakerVendor.Attributes.Add("onclick", "javascript:return showPickList('spnPickListVendor', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131', true);");


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuComponentFilter_TabStripCommand(object sender, EventArgs e)
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
                criteria.Add("txtMakerid", txtMakerId.SelectedValue);
                criteria.Add("txtVendorId", txtVendorId.SelectedValue);
                criteria.Add("txtComponentClassId", string.Empty);
                criteria.Add("txtClassCode", txtClassCode.Text);
                criteria.Add("txtType", txtType.Text);
                criteria.Add("isGolbleSearch", string.Empty);// chkGlobalSearch.Checked.ToString());
                criteria.Add("chkCrtitcal", chkCritical.Checked == true ? "1" : string.Empty);
                criteria.Add("ucComponentCategory", ucComponentCategory.SelectedQuick);
                if (txtNumber.Text == "" && txtName.Text == "" && txtMakerId.Text == "" && txtClassCode.Text == "" && txtType.Text == "" && chkCritical.Checked == false && ucComponentCategory.SelectedQuick == "")
                    Filter.CurrentComponentFilterCriteria = null;
                else
                    Filter.CurrentComponentFilterCriteria = criteria;
            }

            string url = "InventoryComponentChangeRequestList.aspx";
            Response.Redirect("../Inventory/" + url, false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdMakerClear_Click(object sender, ImageClickEventArgs e)
    {
        //txtMakerCode.Text = "";
        //txtMakerName.Text = "";
        txtMakerId.Text = "";
    }


    protected void cmdVendorClear_Click(object sender, ImageClickEventArgs e)
    {
        //txtPreferredVendorCode.Text = "";
        //txtPreferredVendorName.Text = "";
        txtVendorId.Text = "";
    }

    protected void ImageButton1_Click(object sender, EventArgs e)
    {
        txtVendorId.Text = "";
    }

    protected void cmdClear_Click(object sender, EventArgs e)
    {
        txtMakerId.Text = "";
    }
}