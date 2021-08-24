using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using System.Text;


public partial class PurchaseVendorRemark : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["SESSIONID"] != null && Request.QueryString["STOCKTYPE"] != null)
            {
                ifVendorRemarks.Attributes["src"] = "../Purchase/PurchaseVendorRemarks.aspx?quotationid=" + Request.QueryString["SESSIONID"].ToString() + "&stocktype=" + Request.QueryString["STOCKTYPE"].ToString();
            }
        }
    }
}
