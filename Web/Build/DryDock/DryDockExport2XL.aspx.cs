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

public partial class DryDock_DryDockExport2XL : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string exportoption = Request.QueryString["exportoption"].ToString();

        if (exportoption.ToUpper().Equals("PROJECT"))
        {
            string projectid = Request.QueryString["projectid"].ToString();

            PhoenixDryDock2XL.Export2XLDryDockProject(General.GetNullableGuid(projectid), int.Parse(Request.QueryString["vslid"].ToString()));            
        }

        if (exportoption.ToUpper().Equals("QUOTATION"))
        {
            string orderid = Request.QueryString["orderid"].ToString();
            string quotationid = Request.QueryString["quotationid"].ToString();
            
            PhoenixDryDock2XL.Export2XLDryDockQuotation(
                General.GetNullableGuid(orderid),
                General.GetNullableGuid(quotationid)
                , int.Parse(Request.QueryString["vslid"].ToString()),null);
        }
    }
}
