using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewCostEvaluationQuotation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            if (Request.QueryString["VIEWONLY"] == null)
            {
                if ((Request.QueryString["SESSIONID"] != null))
                {
                    filterandsearch.Attributes["src"] = "CrewCostEvaluationQuoteRFQ.aspx?SESSIONID=" + Request.QueryString["SESSIONID"].ToString();
                }
            }
            else
            {
                if (Request.QueryString["SESSIONID"] != null)
                {
                    filterandsearch.Attributes["src"] = "CrewCostEvaluationQuoteRFQ.aspx?SESSIONID=" + Request.QueryString["SESSIONID"].ToString() + "&VIEWONLY=Y";
                }
            }
        }
    }
}
