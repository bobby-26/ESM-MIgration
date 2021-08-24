using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using System.Text;

public partial class PurchaseVendorAddressEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["sessionid"] != null)
            {
                filterandsearch.Attributes["src"] = "PurchaseVendorAddress.aspx?sessionid=" + Request.QueryString["sessionid"].ToString();
            }
        }
    }
}
