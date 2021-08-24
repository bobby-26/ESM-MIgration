using System;
using System.Data;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Registers;

public partial class InventorySpareItemWantedGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindFields();
        }
    }

    private void BindFields()
    {
        try
        {
            if (Request.QueryString["SPAREITEMID"] != null)
            {
                DataSet ds = PhoenixInventorySpareItemWanted.ListSpareItemWanted(new Guid((Request.QueryString["SPAREITEMID"].ToString())), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                DataRow dr = ds.Tables[0].Rows[0];
                txtName.Text = dr["FLDNAME"].ToString();
                txtNumber.Text = dr["FLDNUMBER"].ToString();
                txtMakerCode.Text = dr["MAKERCODE"].ToString();
                txtMakerName.Text = dr["MAKERNAME"].ToString();
                txtMakerReference.Text = dr["FLDMAKERREFERENCE"].ToString();
                txtPreferredVendorCode.Text = dr["VENDORCODE"].ToString();
                txtPreferredVendorName.Text = dr["PREFERREDVENDARNAME"].ToString();
                txtPrice.Text = String.Format("{0:##,###,##0.00}", dr["FLDLASTPURCHASED"]); 
                txtUnit.Text = dr["FLDUNITNAME"].ToString();
                txtReOrderLevel.Text = dr["FLDREORDERLEVEL"].ToString();
                if (dr["FLDSPAREAVERAGECURRENCY"] != null && dr["FLDSPAREAVERAGECURRENCY"].ToString().Trim() != "")
                    ddlStockAveragePriceCurrency.SelectedCurrency = dr["FLDSPAREAVERAGECURRENCY"].ToString();
                else
                    ddlStockAveragePriceCurrency.SelectedCurrency = DefaultCurrency;
                DataSet dsavg = PhoenixInventorySpareItem.ListSpareItemAveragePrice(new Guid((Request.QueryString["SPAREITEMID"].ToString())), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (dsavg.Tables[0].Rows.Count > 0)
                {
                    txtPrice.Text = string.Format(String.Format("{0:########.00}", dsavg.Tables[0].Rows[0]["FLDSTOREAVERAGE"]));
                }
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
}
