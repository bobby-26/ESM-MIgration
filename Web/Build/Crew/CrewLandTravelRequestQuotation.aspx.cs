using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewLandTravelRequestQuotation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["VIEWONLY"] == null)
            {
                if (Request.QueryString["requestid"] != null)
                {
                    filterandsearch.Attributes["src"] = "CrewLandTravelRequestQuotationDetails.aspx?requestid=" + Request.QueryString["requestid"].ToString();
                }
            }
            else
            {
                if (Request.QueryString["requestid"] != null)
                {
                    filterandsearch.Attributes["src"] = "CrewLandTravelRequestQuotationDetails.aspx?requestid=" + Request.QueryString["requestid"].ToString() + "&VIEWONLY=Y";
                }
            }
        }
    }
}
