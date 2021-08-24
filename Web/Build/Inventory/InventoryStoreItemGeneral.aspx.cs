using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class InventoryStoreItemGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);


            MenuStockItemGeneral.AccessRights = this.ViewState;
            MenuStockItemGeneral.MenuList = toolbarmain.Show();

            txtVendorId.Attributes.Add("style", "display:none");

            txtPreferredVendorCode.Attributes.Add("onkeydown", "return false;");
            txtPreferredVendorName.Attributes.Add("onkeydown", "return false;");
            
            if (!IsPostBack)
            {
                txtTotalPrice.Visible = false;
                lblTotalPrice.Visible = false;
                txtTotalPrice.Text = string.Empty;
                string vslaccstoreitems = "";
                DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 97, 0, "BND,PRV,PHC");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    vslaccstoreitems = vslaccstoreitems + ds.Tables[0].Rows[i]["FLDHARDCODE"].ToString() + ",";
                }
                ViewState["VSLACCSTORE"] = "," + vslaccstoreitems;
                ViewState["UNITFOR"] = null;
                BindFields();
                ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();

                // IsEdiTableVessel();
                //cmdNextNumber.Enabled = false;
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
                DataSet ds = PhoenixInventoryStoreItem.ListStoreItem(new Guid((Request.QueryString["STOREITEMID"].ToString())), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                DataRow dr = ds.Tables[0].Rows[0];
                txtName.Text = dr["FLDNAME"].ToString();
                txtNumber.Text = dr["FLDNUMBER"].ToString();
                txtPreferredVendorCode.Text = dr["VENDORCODE"].ToString();
                txtPreferredVendorName.Text = dr["PREFERREDVENDORNAME"].ToString();
                txtVendorId.Text = dr["FLDPREFERREDVENDOR"].ToString();
                txtVendorReference.Text = dr["FLDMAKERREFERENCE"].ToString();

                txtWantedQuantity.Text = string.Format(String.Format("{0:#####.00}", dr["FLDWANTED"]));
                txtStockMaximumQuantity.Text = string.Format(String.Format("{0:#####.00}", dr["FLDSTOREMAXIMUM"]));
                txtStockMinimumQuantity.Text = string.Format(String.Format("{0:#####.00}", dr["FLDSTOREMINIMUM"]));
                txtReOrderLevel.Text = string.Format(String.Format("{0:#####.00}", dr["FLDREORDERLEVEL"]));
                txtReOrderQuantity.Text = string.Format(String.Format("{0:#####.00}", dr["FLDREORDERQUANTITY"]));


                txtStockAveragePrice.Text = string.Format(String.Format("{0:########.00}", dr["FLDSTOREAVERAGE"]));
                txtLastPurchasedPrice.Text = string.Format(String.Format("{0:########.00}", dr["FLDLASTPURCHASED"]));


                if (dr["FLDLASTPURCHASEDCURRENCY"] != null && dr["FLDLASTPURCHASEDCURRENCY"].ToString().Trim() != "")
                    ddlLastPurchasedPriceCurrency.SelectedCurrency = dr["FLDLASTPURCHASEDCURRENCY"].ToString();
                else
                    ddlLastPurchasedPriceCurrency.SelectedCurrency = DefaultCurrency;


                if (dr["FLDSTOREAVERAGECURRENCY"] != null && dr["FLDSTOREAVERAGECURRENCY"].ToString().Trim() != "")
                    ddlStockAveragePriceCurrency.SelectedCurrency = dr["FLDSTOREAVERAGECURRENCY"].ToString();
                else
                    ddlStockAveragePriceCurrency.SelectedCurrency = DefaultCurrency;

                ddlStockUnit.SelectedUnit = dr["FLDUNITID"].ToString();
                ddlStockClass.SelectedHard = dr["FLDSTORECLASS"].ToString();
                ddlsubclasstype.SelectedValue = dr["FLDSUBCLASSID"].ToString();

                ViewState["storeclassid"] = dr["FLDSTORECLASS"].ToString();
                ddlsubclasstype.DataSource = PhoenixRegistersStoreSubClass.StoresubclassList(General.GetNullableInteger(ViewState["storeclassid"].ToString()));
                ddlsubclasstype.DataBind();
                ddlsubclasstype.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

                txtmaterialnumber.Text = dr["FLDMATERIALNUMBER"].ToString();

                ViewState["UNITFOR"] = dr["FLDUNITID"].ToString();

                txtDetail.Text = dr["FLDDETAIL"].ToString();
                txtTotalStock.Text = string.Format(String.Format("{0:#######}", dr["TOTALSTOCKQUANTITY"]));
                if (dr["FLDISINMARKET"].ToString() == "0")
                    chkMarket.Checked = false;
                txtlastpurchaseDate.Text = General.GetDateTimeToString(dr["FLDLASTPURCHASEDDATE"].ToString());
                ViewState["OPERATIONMODE"] = "EDIT";

                DataSet dsavg = PhoenixInventoryStoreItem.ListStoreItemAveragePrice(new Guid((Request.QueryString["STOREITEMID"].ToString())), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (dsavg.Tables[0].Rows.Count > 0)
                {
                    txtStockAveragePrice.Text = string.Format(String.Format("{0:########.00}", dsavg.Tables[0].Rows[0]["FLDSTOREAVERAGE"]));
                }
                if (dr["FLDSTORECLASS"].ToString() != string.Empty && ViewState["VSLACCSTORE"].ToString().Contains("," + dr["FLDSTORECLASS"].ToString() + ","))
                {
                    txtTotalPrice.CssClass = "input";
                    txtTotalPrice.Visible = true;
                    lblTotalPrice.Visible = true;
                    txtTotalPrice.Text = dr["FLDTOTALPRICE"].ToString();
                }
                else
                {
                    txtTotalPrice.Visible = false;
                    lblTotalPrice.Visible = false;
                    txtTotalPrice.Text = string.Empty;
                }
                imgUnitMap.Visible = true;
                imgUnitMap.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inventory/InventoryStoreItemMeasurable.aspx?STOREITEMID=" + Request.QueryString["STOREITEMID"] + "&NAME=" + txtName.Text.ToString() + "&UNITFOR=" + ViewState["UNITFOR"] + "');return false;");
            }
            else
            {
                imgUnitMap.Visible = false;
                ddlLastPurchasedPriceCurrency.SelectedCurrency = DefaultCurrency;
                ddlStockAveragePriceCurrency.SelectedCurrency = DefaultCurrency;
                txtTotalPrice.ReadOnly = "true";
                txtTotalPrice.Text = "";
                ViewState["OPERATIONMODE"] = "ADD";
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public static string DefaultCurrency
    {
        get
        {
            string id = null;
            DataSet ds = new DataSet();
            ds = PhoenixRegistersHard.ListHard(1, (int)(PhoenixHardTypeCode.DEFAULTCURRENCY));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataSet ds1 = PhoenixRegistersCurrency.ListCurrency(null, ds.Tables[0].Rows[0]["FLDHARDNAME"].ToString());
                if (ds1.Tables[0].Rows.Count > 0)
                    id = ds1.Tables[0].Rows[0]["FLDCURRENCYID"].ToString();
            }
            return id;
        }
    }
    protected void ddlStockClass_OnTextChangedEvent(object sender, EventArgs e)
    {
        ddlsubclasstype.DataSource = PhoenixRegistersStoreSubClass.StoresubclassList(General.GetNullableInteger(ddlStockClass.SelectedHard));
        ddlsubclasstype.DataBind();
        ddlsubclasstype.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));


    }
    
    private bool IsValidStockItem(string itemnumber, string itemname, string stockaveragecurrency, string lastpurchasedcurrency,
                                  string stockunit, string stockclass, string maximumquantity, string mininumquantity, string reorderlevel)
    {

        ucError.HeaderMessage = "Please provide the following required information.";

        int result;

        if (itemnumber.Trim().Equals(""))
            ucError.ErrorMessage = "Item number can not be blank.";

        if (itemname.Trim().Equals(""))
            ucError.ErrorMessage = "Item name can not be blank.";

        if (stockunit.Trim().Equals("Dummy"))
        {
            if (int.TryParse(stockunit, out result) == false)
                ucError.ErrorMessage = "Please select item unit.";
        }
        if(stockclass.Trim().Equals("Dummy"))
        {
            if (int.TryParse(stockclass, out result) == false)
                ucError.ErrorMessage = "Please select store type.";
        }
        if (maximumquantity.Trim() != "")
        {
            maximumquantity = maximumquantity != "" ? maximumquantity : "0";
            mininumquantity = mininumquantity != "" ? mininumquantity : "0";
            reorderlevel = reorderlevel != "" ? reorderlevel : "0";

            if (Convert.ToDecimal(maximumquantity.Trim()) < Convert.ToDecimal(mininumquantity.Trim()))
                ucError.ErrorMessage = "Minimum quantity should be less than maximum quantity";

            if (Convert.ToDecimal(maximumquantity.Trim()) < Convert.ToDecimal(reorderlevel.Trim()))
                ucError.ErrorMessage = "Reorder level quantity should be less than maximum quantity";
        }

        return (!ucError.IsError);
    }


    protected void InventoryStockItemGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            int? isinmarket = null;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidStockItem(txtNumber.TextWithLiterals, txtName.Text, ddlStockAveragePriceCurrency.SelectedCurrency,
                                      ddlLastPurchasedPriceCurrency.SelectedCurrency, ddlStockUnit.SelectedUnit, ddlStockClass.SelectedHard,
                                      txtStockMaximumQuantity.Text, txtStockMinimumQuantity.Text, txtReOrderLevel.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (chkMarket.Checked == true)
                    isinmarket = 1;
                else
                    isinmarket = 0;

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {

                    PhoenixInventoryStoreItem.UpdateStoreItem(new Guid(Request.QueryString["STOREITEMID"]), PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , txtNumber.TextWithLiterals, PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtName.Text
                        , null, txtVendorReference.Text, General.GetNullableInteger(txtVendorId.Text), null
                        , Convert.ToInt32(ddlStockUnit.SelectedValue), General.GetNullableDecimal(txtWantedQuantity.Text)
                        , General.GetNullableDecimal(txtStockMaximumQuantity.Text), General.GetNullableDecimal(txtStockMinimumQuantity.Text)
                        , General.GetNullableInteger(txtReOrderLevel.Text), General.GetNullableDecimal(txtReOrderQuantity.Text)
                        , General.GetNullableDecimal(txtStockAveragePrice.Text), General.GetNullableInteger(ddlStockAveragePriceCurrency.SelectedCurrency.ToString())
                        , General.GetNullableDecimal(txtLastPurchasedPrice.Text), General.GetNullableInteger(ddlLastPurchasedPriceCurrency.SelectedCurrency.ToString())
                        , General.GetNullableInteger(ddlStockClass.SelectedHard.ToString()), txtDetail.Text
                        , null, isinmarket, General.GetNullableDecimal(txtTotalPrice.Text)
                        , General.GetNullableInteger(ddlsubclasstype.SelectedValue.ToString())
                        ,General.GetNullableString(txtmaterialnumber.Text)
                        );
                }

                if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {

                    PhoenixInventoryStoreItem.InsertStoreItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtNumber.TextWithLiterals
                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtName.Text, null
                        , txtVendorReference.Text
                        , General.GetNullableInteger(txtVendorId.Text)
                        , null
                        , Convert.ToInt32(ddlStockUnit.SelectedValue)
                        , General.GetNullableDecimal(txtWantedQuantity.Text), General.GetNullableDecimal(txtStockMaximumQuantity.Text)
                        , General.GetNullableDecimal(txtStockMinimumQuantity.Text), General.GetNullableInteger(txtReOrderLevel.Text)
                        , General.GetNullableDecimal(txtReOrderQuantity.Text), General.GetNullableDecimal(txtStockAveragePrice.Text)
                        , General.GetNullableInteger(ddlStockAveragePriceCurrency.SelectedCurrency.ToString())
                        , General.GetNullableDecimal(txtLastPurchasedPrice.Text)
                        , General.GetNullableInteger(ddlLastPurchasedPriceCurrency.SelectedCurrency.ToString())
                        , General.GetNullableInteger(ddlStockClass.SelectedHard.ToString())
                        , txtDetail.Text
                        , null
                        , General.GetNullableInteger(ddlsubclasstype.SelectedValue.ToString())
                         , General.GetNullableString(txtmaterialnumber.Text)

                        );
                }


                String script = String.Format("javascript:fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                txtName.Text = "";
                txtNumber.Text = "";
                txtPreferredVendorCode.Text = "";
                txtVendorReference.Text = "";
                txtPreferredVendorName.Text = "";
                txtWantedQuantity.Text = "";
                txtStockMaximumQuantity.Text = "";
                txtStockMinimumQuantity.Text = "";
                txtReOrderLevel.Text = "";
                txtStockAveragePrice.Text = "";
                txtLastPurchasedPrice.Text = "";
                txtVendorId.Text = "";
                txtReOrderLevel.Text = "";
                txtReOrderQuantity.Text = "";
                ddlLastPurchasedPriceCurrency.SelectedCurrency = DefaultCurrency;
                ddlStockAveragePriceCurrency.SelectedCurrency = DefaultCurrency;
                ddlStockUnit.SelectedUnit = "";
                ddlStockClass.SelectedHard = "";
                ddlsubclasstype.SelectedValue = "";
                txtDetail.Text = "";
                txtTotalStock.Text = "";
                txtTotalPrice.ReadOnly = "true";
                txtTotalPrice.CssClass = "readonlytextbox";
                txtTotalPrice.Text = "";
                txtmaterialnumber.Text = "";
                //cmdNextNumber.Enabled = true;
                ViewState["OPERATIONMODE"] = "ADD";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void IsEdiTableVessel()
    {
        txtName.Enabled = General.IsEditableInOffice();
        txtNumber.Enabled = General.IsEditableInOffice();
        txtPreferredVendorCode.Enabled = General.IsEditableInOffice();
        txtPreferredVendorName.Enabled = General.IsEditableInOffice();
        txtWantedQuantity.Enabled = General.IsEditableInOffice();
        txtStockMaximumQuantity.Enabled = General.IsEditableInOffice();
        txtStockMinimumQuantity.Enabled = General.IsEditableInOffice();
        txtReOrderLevel.Enabled = General.IsEditableInOffice();
        txtReOrderQuantity.Enabled = General.IsEditableInOffice();
        txtStockAveragePrice.Visible = General.IsEditableInOffice();
        txtLastPurchasedPrice.Visible = General.IsEditableInOffice();
        ddlLastPurchasedPriceCurrency.Enabled = General.IsEditableInOffice();
        ddlStockAveragePriceCurrency.Enabled = General.IsEditableInOffice();
        ddlStockUnit.Enabled = General.IsEditableInOffice();
        ddlStockClass.Enabled = General.IsEditableInOffice();
        ddlsubclasstype.Enabled = General.IsEditableInOffice();
        txtmaterialnumber.Enabled= General.IsEditableInOffice();
        txtTotalStock.Enabled = General.IsEditableInOffice();
        txtVendorId.Enabled = General.IsEditableInOffice();
        ImgShowMakerVendor.Visible = General.IsEditableInOffice();
        chkMarket.Enabled = General.IsEditableInOffice();
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            txtStockMaximumQuantity.Enabled = General.IsEditableInShip();
            txtStockMinimumQuantity.Enabled = General.IsEditableInShip();
            txtWantedQuantity.Enabled = General.IsEditableInShip();
            txtReOrderLevel.Enabled = General.IsEditableInShip();
            txtReOrderQuantity.Enabled = General.IsEditableInShip();
        }

    }
    public void cmdNextNumber_OnClick(object sender, EventArgs e)
    {
        try
        {
          
            DataTable dt = PhoenixInventoryStoreItem.AutoGenerateNumber(txtNumber.Text == "__.__.__" ? "" : txtNumber.Text.Trim('_'), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            txtNumber.Text = dt.Rows[0]["FLDNEXTNUMBER"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdVendorClear_Click(object sender, EventArgs e)
    {
        txtPreferredVendorCode.Text = "";
        txtPreferredVendorName.Text = "";
        txtVendorId.Text = "";
    }
}
