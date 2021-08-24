using System;
using System.Data;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Registers;

public partial class InventoryStoreItemWantedGeneral : PhoenixBasePage
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
            if (Request.QueryString["STOREITEMID"] != null)
            {
                DataSet ds = PhoenixInventoryStoreItemWanted.ListStoreItemWanted(new Guid((Request.QueryString["STOREITEMID"].ToString())), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                DataRow dr = ds.Tables[0].Rows[0];
                txtName.Text = dr["FLDNAME"].ToString();
                txtNumber.Text = dr["FLDNUMBER"].ToString();
                txtPreferredVendorCode.Text = dr["VENDORCODE"].ToString();
                txtPreferredVendorName.Text = dr["PREFERREDVENDORNAME"].ToString();
                txtPrice.Text = String.Format("{0:##,###,##0.00}", dr["FLDLASTPURCHASED"]); 
                txtUnit.Text = dr["FLDUNITNAME"].ToString();
                txtReOrderLevel.Text = dr["FLDREORDERLEVEL"].ToString();
                if (dr["FLDSTOREAVERAGECURRENCY"] != null && dr["FLDSTOREAVERAGECURRENCY"].ToString().Trim() != "")
                    ddlStockAveragePriceCurrency.SelectedCurrency = dr["FLDSTOREAVERAGECURRENCY"].ToString();
                else
                    ddlStockAveragePriceCurrency.SelectedCurrency = DefaultCurrency;
                DataSet dsavg = PhoenixInventoryStoreItem.ListStoreItemAveragePrice(new Guid((Request.QueryString["STOREITEMID"].ToString())), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
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
