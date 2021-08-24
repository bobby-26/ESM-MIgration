using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Export2XL;

public partial class PurchaseExport2XL : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string quotationid = Request.QueryString["quotationid"].ToString();

        PhoenixPurchase2XL.Export2XLPurchaseRFQ(
            new Guid(quotationid));
    }
}
