using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class InventorySpareItemGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);

                MenuStockItemGeneral.AccessRights = this.ViewState;
                MenuStockItemGeneral.MenuList = toolbarmain.Show();

                txtMakerCode.Attributes.Add("onkeydown", "return false;");
                txtMakerName.Attributes.Add("onkeydown", "return false;");
                txtPreferredVendorCode.Attributes.Add("onkeydown", "return false;");
                txtPreferredVendorName.Attributes.Add("onkeydown", "return false;");

                ViewState["UNITFOR"] = null;
                ViewState["VESSELID"] = null;
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"];
                BindFields();
                ddlStockClass.HardTypeCode= ((int)PhoenixHardTypeCode.STOCKCLASS).ToString();
                cmdNextNumber.Enabled = false;
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
        if ((Request.QueryString["SPAREITEMID"] != null) && (Request.QueryString["SPAREITEMID"] != ""))
        {
            DataSet ds = PhoenixInventorySpareItem.ListSpareItem(new Guid((Request.QueryString["SPAREITEMID"].ToString())), int.Parse(ViewState["VESSELID"].ToString()));
            DataRow dr = ds.Tables[0].Rows[0];
            txtName.Text = dr["FLDNAME"].ToString();
            txtNumber.Text = dr["FLDNUMBER"].ToString();
            txtMakerCode.Text = dr["MAKERCODE"].ToString();
            txtMakerName.Text = dr["MAKERNAME"].ToString();
            txtMakerId.Text = dr["FLDMAKER"].ToString();
            txtMakerReference.Text = dr["FLDMAKERREFERENCE"].ToString();
            txtPreferredVendorCode.Text = dr["VENDORCODE"].ToString();
            txtPreferredVendorName.Text = dr["PREFERREDVENDORNAME"].ToString();
            txtVendorId.Text = dr["FLDPREFERREDVENDOR"].ToString();

            txtWantedQuantity.Text = string.Format(String.Format("{0:#######}", dr["FLDWANTED"]));
            txtStockMaximumQuantity.Text = string.Format(String.Format("{0:#######}", dr["FLDSPAREMAXIMUM"]));
            txtStockMinimumQuantity.Text = string.Format(String.Format("{0:#######}", dr["FLDSPAREMINIMUM"]));
            txtReOrderLevel.Text = string.Format(String.Format("{0:#######}", dr["FLDREORDERLEVEL"].ToString()));
            txtReOrderQuantity.Text = string.Format(String.Format("{0:#######}", dr["FLDREORDERQUANTITY"]));

            txtStockAveragePrice.Text = string.Format(String.Format("{0:########.00}", dr["FLDSPAREAVERAGE"]));
            txtLastPurchasedPrice.Text = string.Format(String.Format("{0:########.00}", dr["FLDLASTPURCHASED"]));
            txtmaterialnumber.Text = dr["FLDMATERIALNUMBER"].ToString();


            if (dr["FLDLASTPURCHASEDCURRENCY"] != null && dr["FLDLASTPURCHASEDCURRENCY"].ToString().Trim() != "")
                ddlLastPurchasedPriceCurrency.SelectedCurrency = dr["FLDLASTPURCHASEDCURRENCY"].ToString();
            else
                ddlLastPurchasedPriceCurrency.SelectedCurrency = DefaultCurrency;


            if (dr["FLDSPAREAVERAGECURRENCY"] != null && dr["FLDSPAREAVERAGECURRENCY"].ToString().Trim() != "")
                ddlStockAveragePriceCurrency.SelectedCurrency = dr["FLDSPAREAVERAGECURRENCY"].ToString();
            else
                ddlStockAveragePriceCurrency.SelectedCurrency = DefaultCurrency;


            ddlStockUnit.SelectedUnit = dr["FLDUNITID"].ToString();
            ddlStockClass.SelectedHard = dr["FLDSPARECLASS"].ToString();

            txtTotalStock.Text = string.Format(String.Format("{0:#######}", dr["TOTALSTOCKQUANTITY"]));

            if (dr["FLDISCRITICAL"].ToString() == "1")
                chkIsCritical.Checked = true;
            if (dr["FLDISINMARKET"].ToString() == "0")
                chkMarket.Checked = false;
            txtlastpurchaseDate.Text = General.GetDateTimeToString(dr["FLDLASTPURCHASEDDATE"].ToString());

            ViewState["OPERATIONMODE"] = "EDIT";

            DataSet dsavg = PhoenixInventorySpareItem.ListSpareItemAveragePrice(new Guid((Request.QueryString["SPAREITEMID"].ToString())), int.Parse(ViewState["VESSELID"].ToString()));
            if (dsavg.Tables[0].Rows.Count > 0)
            {
                txtStockAveragePrice.Text = string.Format(String.Format("{0:########.00}", dsavg.Tables[0].Rows[0]["FLDSTOREAVERAGE"]));
            }
            ViewState["UNITFOR"] = dr["FLDUNITID"].ToString();

            imgUnitMap.Visible = true;
            imgUnitMap.Attributes.Add("onclick", "javascript:openNewWindow('Filter','' , '" + Session["sitepath"] + "/Inventory/InventorySpareItemMeasurable.aspx?SPAREITEMID=" + Request.QueryString["SPAREITEMID"] + "&NAME=" + txtName.Text.ToString() + "&UNITFOR=" + ViewState["UNITFOR"] + "');return false;");
        }
        else
        {
            imgUnitMap.Visible = false;
            ddlLastPurchasedPriceCurrency.SelectedCurrency = DefaultCurrency;
            ddlStockAveragePriceCurrency.SelectedCurrency = DefaultCurrency;
            ViewState["OPERATIONMODE"] = "ADD";
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
 
    private bool IsValidStockItem(string itemnumber, string itemname, string stockaveragecurrency, string lastpurchasedcurrency,
                                  string stockunit, string stockclass, string maximumquantity, string mininumquantity, string reorderlevel)
    {


        ucError.HeaderMessage = "Please provide the following required information.";

        int result;
        maximumquantity = maximumquantity.Trim('_');
        mininumquantity = mininumquantity.Trim('_');
        reorderlevel = reorderlevel.Trim('_');
        if (itemnumber.Trim().Equals(""))
            ucError.ErrorMessage = "Item number can not be blank.";

        if (itemname.Trim().Equals(""))
            ucError.ErrorMessage = "Item name can not be blank.";

        if (stockunit.Trim().Equals("Dummy"))
        {
            if (int.TryParse(stockunit, out result) == false)
                ucError.ErrorMessage = "Please select item unit.";
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string maxqty = txtStockMaximumQuantity.Text.Trim('_');
                string minqty = txtStockMinimumQuantity.Text.Trim('_');
                string reorderlevel = txtReOrderLevel.Text.Trim('_');
                if (!IsValidStockItem(txtNumber.TextWithLiterals, txtName.Text, ddlStockAveragePriceCurrency.SelectedCurrency,
                                      ddlLastPurchasedPriceCurrency.SelectedCurrency, ddlStockUnit.SelectedUnit, ddlStockClass.SelectedHard,
                                      maxqty, minqty, reorderlevel))
                {
                    ucError.Visible = true;
                    return;
                }

                int? iscritical = null;
                int? isinmarket = null;
                if (chkIsCritical.Checked == true)
                    iscritical = 1;
                else
                    iscritical = 0;
                if (chkMarket.Checked == true)
                    isinmarket = 1;
                else
                    isinmarket = 0;

                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {

                    PhoenixInventorySpareItem.UpdateSpareItem(new Guid(Request.QueryString["SPAREITEMID"]), PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , txtNumber.TextWithLiterals, int.Parse(ViewState["VESSELID"].ToString()), txtName.Text
                        , General.GetNullableInteger(txtMakerId.Text), txtMakerReference.Text, General.GetNullableInteger(txtVendorId.Text)
                        , null
                        , Convert.ToInt32(ddlStockUnit.SelectedValue), General.GetNullableDecimal(txtWantedQuantity.Text.Trim('_'))
                        , General.GetNullableDecimal(maxqty), General.GetNullableDecimal(minqty)
                        , General.GetNullableInteger(reorderlevel), General.GetNullableDecimal(txtReOrderQuantity.Text.Trim('_'))
                        , General.GetNullableDecimal(txtStockAveragePrice.Text), General.GetNullableInteger(ddlStockAveragePriceCurrency.SelectedCurrency.ToString())
                        , General.GetNullableDecimal(txtLastPurchasedPrice.Text), General.GetNullableInteger(ddlLastPurchasedPriceCurrency.SelectedCurrency.ToString())
                        , General.GetNullableInteger(ddlStockClass.SelectedHard.ToString()),null
                        , null, iscritical, isinmarket
                        , General.GetNullableString(txtmaterialnumber.Text));
                }

                if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {

                    PhoenixInventorySpareItem.InsertSpareItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtNumber.TextWithLiterals
                        , int.Parse(ViewState["VESSELID"].ToString()), txtName.Text, General.GetNullableInteger(txtMakerId.Text)
                        , txtMakerReference.Text
                        , General.GetNullableInteger(txtVendorId.Text)
                        , null
                        , Convert.ToInt32(ddlStockUnit.SelectedValue)
                        , General.GetNullableDecimal(txtWantedQuantity.Text.Trim('_')), General.GetNullableDecimal(maxqty)
                        , General.GetNullableDecimal(minqty), General.GetNullableInteger(reorderlevel)
                        , General.GetNullableDecimal(txtReOrderQuantity.Text.Trim('_')), General.GetNullableDecimal(txtStockAveragePrice.Text)
                        , General.GetNullableInteger(ddlStockAveragePriceCurrency.SelectedCurrency.ToString())
                        , General.GetNullableDecimal(txtLastPurchasedPrice.Text)
                        , General.GetNullableInteger(ddlLastPurchasedPriceCurrency.SelectedCurrency.ToString())
                        , General.GetNullableInteger(ddlStockClass.SelectedHard.ToString())
                        , null
                        , null, iscritical
                        , General.GetNullableString(txtmaterialnumber.Text));
                }


                String script = String.Format("javascript:fnReloadList('code1');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                BindFields();
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                txtName.Text = "";
                txtNumber.Text = "";
                txtMakerCode.Text = "";
                txtMakerName.Text = "";
                txtMakerReference.Text = "";
                txtPreferredVendorCode.Text = "";
                txtPreferredVendorName.Text = "";
                txtWantedQuantity.Text = "";
                txtStockMaximumQuantity.Text = "";
                txtStockMinimumQuantity.Text = "";
                txtReOrderLevel.Text = "";
                txtReOrderQuantity.Text = "";
                txtStockAveragePrice.Text = "";
                txtLastPurchasedPrice.Text = "";
                ddlLastPurchasedPriceCurrency.SelectedCurrency = DefaultCurrency;
                ddlStockAveragePriceCurrency.SelectedCurrency = DefaultCurrency;
                ddlStockUnit.SelectedUnit = "";
                ddlStockClass.SelectedHard = "";
                cmdNextNumber.Enabled = true;
                chkIsCritical.Checked = false;
                txtTotalStock.Text = "0";
                txtMakerId.Text = "";
                txtVendorId.Text = "";
                txtmaterialnumber.Text = "";

                ViewState["OPERATIONMODE"] = "ADD";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void cmdNextNumber_OnClick(object sender,EventArgs e)
    {
        try
        {
            DataTable dt = PhoenixInventorySpareItem.AutoGenerateNumber(txtNumber.TextWithLiterals, int.Parse(ViewState["VESSELID"].ToString()));

            txtNumber.Text = dt.Rows[0]["FLDNEXTNUMBER"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void ClearMaker(object sender, EventArgs e)
    {
        txtMakerCode.Text = "";
        txtMakerName.Text = "";
        txtMakerId.Text = "";
    }
    protected void ClearVendor(object sender, EventArgs e)
    {
        txtPreferredVendorCode.Text = "";
        txtPreferredVendorName.Text = "";
        txtVendorId.Text = "";
    }
   
}
