using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inventory;

public partial class InventoryComponentTypeGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("New", "NEW");
            toolbarmain.AddButton("Save", "SAVE");
            MenuComponentTypeGeneral.AccessRights = this.ViewState;  
            MenuComponentTypeGeneral.MenuList = toolbarmain.Show();
            MenuComponentTypeGeneral.SetTrigger(pnlComponentTypeGeneral);
            txtParentId.Attributes.Add("style", "visibility:hidden");
            txtMakerId.Attributes.Add("style", "visibility:hidden");
            txtVendorId.Attributes.Add("style", "visibility:hidden");

            txtMakerCode.Attributes.Add("onkeydown", "return false;");
            txtMakerName.Attributes.Add("onkeydown", "return false;");

            txtPreferredVendorCode.Attributes.Add("onkeydown", "return false;");
            txtPreferredVendorName.Attributes.Add("onkeydown", "return false;");

            txtParentNumber.Attributes.Add("onkeydown", "return false;");
            txtParentName.Attributes.Add("onkeydown", "return false;");

            if (!IsPostBack)
            {
                BindFields();
                ddlComponentClass.QuickTypeCode = ((int)PhoenixQuickTypeCode.COMPONENTCLASS).ToString();
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
            if ((Request.QueryString["COMPONENTTYPEID"] != null) && (Request.QueryString["COMPONENTTYPEID"] != ""))
            {
                DataSet ds = PhoenixInventoryComponentType.ListComponentType(new Guid(Request.QueryString["COMPONENTTYPEID"]));

                DataRow dr = ds.Tables[0].Rows[0];
                txtComponentTypeName.Text = dr["FLDCOMPONENTNAME"].ToString();
                txtComponentTypeNumber.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                txtMakerId.Text = dr["FLDMAKERID"].ToString();
                txtMakerCode.Text = dr["MAKERCODE"].ToString();
                txtMakerName.Text = dr["MAKERNAME"].ToString();
                txtVendorId.Text = dr["FLDVENDORID"].ToString();
                txtPreferredVendorCode.Text = dr["VENDORCODE"].ToString();
                txtPreferredVendorName.Text = dr["VENDORNAME"].ToString();
                txtType.Text = dr["FLDTYPE"].ToString();
                txtParentId.Text = dr["PARENTCOMPONENTTYPEID"].ToString();
                txtParentNumber.Text = dr["PARENTCOMPONENTNUMBER"].ToString();
                txtParentName.Text = dr["PARENTCOMPONENTNAME"].ToString();
                ddlComponentClass.SelectedQuick = dr["FLDCOMPONENTCLASSID"].ToString();
                ViewState["OPERATIONMODE"] = "EDIT";
            }
            else
            {
                ViewState["OPERATIONMODE"] = "ADD";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PlannedMaintenanceComponentType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidComponentType(txtComponentTypeNumber.Text, txtComponentTypeName.Text ,txtParentId.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {
                    PhoenixInventoryComponentType.UpdateComponentType(new Guid(Request.QueryString["COMPONENTTYPEID"]), PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableInteger(ddlComponentClass.SelectedValue.ToString()), txtComponentTypeNumber.Text
                        , txtComponentTypeName.Text, txtParentId.Text, General.GetNullableInteger(txtVendorId.Text)
                        , General.GetNullableInteger(txtMakerId.Text), txtType.Text);

                }

                if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    PhoenixInventoryComponentType.InsertComponentType(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableInteger(ddlComponentClass.SelectedValue.ToString()), txtComponentTypeNumber.Text
                        , txtComponentTypeName.Text, txtParentId.Text, General.GetNullableInteger(txtVendorId.Text)
                        , General.GetNullableInteger(txtMakerId.Text), txtType.Text);
                }
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                ddlComponentClass.SelectedQuick = "";
                txtComponentTypeName.Text = "";
                txtComponentTypeNumber.Text = "";
                txtMakerId.Text = "";  
                txtMakerCode.Text = "";
                txtMakerName.Text = "";
                txtVendorId.Text = ""; 
                txtPreferredVendorCode.Text = "";
                txtPreferredVendorName.Text = "";
                txtType.Text = "";
                txtParentId.Text = "";
                ViewState["OPERATIONMODE"] = "ADD";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidComponentType(string componenttypenumber, string componenttypename ,string componenttypeparentid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (componenttypenumber.Trim().Equals(""))
            ucError.ErrorMessage = "Component number can not be blank.";

        if (componenttypename.Trim().Equals(""))
            ucError.ErrorMessage = "Component type name can not be blank.";

        return (!ucError.IsError);
    }

    protected void cmdMakerClear_Click(object sender, ImageClickEventArgs e)
    {
        txtMakerCode.Text = "";
        txtMakerName.Text = "";
        txtMakerId.Text = "";
    }

    protected void cmdVendorClear_Click(object sender, ImageClickEventArgs e)
    {
        txtPreferredVendorCode.Text = "";
        txtPreferredVendorName.Text = "";
        txtVendorId.Text = "";
    }

    protected void cmdComponentTypeParentClear_Click(object sender, ImageClickEventArgs e)
    {
        txtParentNumber.Text = "";
        txtParentName.Text = "";
        txtParentId.Text = "";
    }
}
