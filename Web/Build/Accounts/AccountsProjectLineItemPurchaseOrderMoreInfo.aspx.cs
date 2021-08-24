using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Accounts;
using System.Text;
public partial class Accounts_AccountsProjectLineItemPurchaseOrderMoreInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["PurchaseLineItemId"] != null)
                {
                    ViewState["PURCHASELINEITEMID"] = Request.QueryString["PurchaseLineItemId"].ToString();
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    protected void BindData()
    {

        DataTable dt = PhoenixAccountProjectPurchaseLineItem.PurchaseLineItemMoreInfoList( General.GetNullableGuid(ViewState["PURCHASELINEITEMID"].ToString()));

        if (dt.Rows.Count > 0)
        {
            lblInvoiceNo.Text =     dt.Rows[0]["FLDINVOICENUMBER"].ToString();
            lblInvoiceStatus.Text = dt.Rows[0]["FLDINVOICESTATUS"].ToString();
            lblVoucherNumber.Text = dt.Rows[0]["FLDVOUCHERNUMBER"].ToString();
        }
    }
}