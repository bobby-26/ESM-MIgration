using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;

public partial class PurchaseQuotationItems : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            if (Request.QueryString["VIEWONLY"] == null)
            {
                if ((Request.QueryString["SESSIONID"] != null) && (Request.QueryString["STOCKTYPE"] != null))
                {
                    filterandsearch.Attributes["src"] = "PurchaseQuotationRFQ.aspx?SESSIONID=" + Request.QueryString["SESSIONID"].ToString() + "&STOCKTYPE=" + Request.QueryString["STOCKTYPE"].ToString();
                }
            }
            else
            {
                if (Request.QueryString["SESSIONID"] != null)
                {
                    filterandsearch.Attributes["src"] = "PurchaseQuotationRFQ.aspx?SESSIONID=" + Request.QueryString["SESSIONID"].ToString() + "&VIEWONLY=Y";
                }
            }
        }
    }
}
