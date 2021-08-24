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
using System.Text;
using SouthNests.Phoenix.Framework;

public partial class PurchaseQuotationConfirmation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sb =new StringBuilder(); 
        sb.Append("Your bid  " + Request.QueryString["QTNREFNO"].ToString() + "  is registered and confirmed in our database") ;
        sb.AppendLine();
        if (Request.QueryString["ORDERBEFOREDATE"] !=null)
        {
        sb.Append("We will respond back  by Order before date " + Request.QueryString["ORDERBEFOREDATE"].ToString()); 
        }

        lbltext.Text = sb.ToString();  
    }
}
