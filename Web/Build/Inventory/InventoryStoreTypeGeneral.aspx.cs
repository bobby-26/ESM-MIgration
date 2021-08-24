using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventoryStoreTypeGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
            MenuStoreTypeGeneral.AccessRights = this.ViewState;  
            MenuStoreTypeGeneral.MenuList = toolbarmain.Show();

            txtVendorCode.Attributes.Add("onkeydown", "return false;");
            txtVendorName.Attributes.Add("onkeydown", "return false;");
            txtParentStockTypeName.Attributes.Add("onkeydown", "return false;");
            txtParentStockTypeNumber.Attributes.Add("onkeydown", "return false;");

            if (!IsPostBack)
            {
                BindFields();
                ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
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
            if ((Request.QueryString["STORETYPEID"] != null) && (Request.QueryString["STORETYPEID"] != ""))
            {
                DataSet ds = PhonixInventoryStoreType.ListStoreType(new Guid(Request.QueryString["STORETYPEID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                DataRow dr = ds.Tables[0].Rows[0];
                txtStockTypeNumber.Text = dr["FLDSTORETYPENUMBER"].ToString();
                txtStockTypeName.Text = dr["FLDSTORETYPENAME"].ToString();
                txtVendorCode.Text = dr["VENDORCODE"].ToString();
                txtVendorName.Text = dr["VENDORNAME"].ToString();
                txtVendorId.Text = dr["FLDVENDORID"].ToString();
                txtParentStockTypeId.Text = dr["PARENTSTORETYPEID"].ToString();
                txtParentStockTypeNumber.Text = dr["PARENTSTORETYPENUMBER"].ToString();
                txtParentStockTypeName.Text = dr["PARENTSTORETYPENAME"].ToString();
                ddlPurchasedPriceCurrency.SelectedCurrency = dr["FLDPURCHASECURRENCY"].ToString();
                txtPurchasedPrice.Text = dr["FLDPURCHASEPRICE"].ToString();
                txtPurchaseDate.Text = dr["FLDPURCHASEDATE"].ToString();
                ddlStockClass.SelectedHard= dr["FLDSTORECLASS"].ToString();
                
                ddlStoreUnit.SelectedUnit = dr["FLDUNITID"].ToString();  

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

    protected void StoreType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidStoreType(txtStockTypeNumber.Text, txtStockTypeName.Text, txtParentStockTypeId.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {

                    PhonixInventoryStoreType.UpdateStoreType(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(Request.QueryString["STORETYPEID"])
                        , txtStockTypeNumber.TextWithLiterals.TrimEnd('.')
                        , txtStockTypeName.Text
                        , null
                        , txtParentStockTypeId.Text
                        , General.GetNullableInteger(txtVendorId.Text)
                        , null
                        , General.GetNullableInteger(ddlStoreUnit.SelectedUnit)
                        , General.GetNullableDateTime(txtPurchaseDate.Text)
                        , General.GetNullableInteger(ddlPurchasedPriceCurrency.SelectedCurrency)
                        , General.GetNullableDecimal(txtPurchasedPrice.Text)
                        , null 
                        , General.GetNullableInteger(ddlStockClass.SelectedHard));

                }

                if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    PhonixInventoryStoreType.InsertStoreType(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                       , txtStockTypeNumber.TextWithLiterals.TrimEnd('.')
                       , txtStockTypeName.Text
                       , null
                       , txtParentStockTypeId.Text
                       , General.GetNullableInteger(txtVendorId.Text)
                       , null
                       , General.GetNullableInteger(ddlStoreUnit.SelectedUnit)
                       , General.GetNullableDateTime(txtPurchaseDate.Text)
                       , General.GetNullableInteger(ddlPurchasedPriceCurrency.SelectedCurrency)
                       , General.GetNullableDecimal(txtPurchasedPrice.Text)
                       , null 
                       , General.GetNullableInteger(ddlStockClass.SelectedHard));
                }

                String script = String.Format("javascript:fnReloadList('code1');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                txtStockTypeNumber.Text = "";
                txtStockTypeName.Text = "";
                txtVendorCode.Text = "";
                txtVendorName.Text = "";
                txtVendorId.Text = "";
                txtParentStockTypeNumber.Text = "";
                txtParentStockTypeName.Text = "";
                txtParentStockTypeId.Text = "";
                txtPurchasedPrice.Text = "";
                txtPurchaseDate.Text = "";
                ddlPurchasedPriceCurrency.SelectedCurrency = "";
                ddlStockClass.SelectedHard= "";
                ddlStoreUnit.SelectedUnit = "";

                ViewState["OPERATIONMODE"] = "ADD";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidStoreType(string storetypenumber, string storetypename, string storetypeparentid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (storetypenumber.Trim().Equals(""))
            ucError.ErrorMessage = "Store type number can not be blank.";

        if (storetypename.Trim().Equals(""))
            ucError.ErrorMessage = "Store type name can not be blank.";

        return (!ucError.IsError);
    }

    protected void cmdClear_Click(object sender, EventArgs e)
    {
        txtVendorCode.Text = "";
        txtVendorName.Text = "";
        txtVendorId.Text = "";
    }

    protected void cmdStockTypeClear_Click(object sender, EventArgs e)
    {
        txtParentStockTypeNumber.Text = "";
        txtParentStockTypeName.Text = "";
        txtParentStockTypeId.Text = "";
    }
}
