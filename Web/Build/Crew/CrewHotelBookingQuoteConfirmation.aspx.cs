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
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
public partial class CrewHotelBookingQuoteConfirmation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();       
        sb.Append("Your bid is registered and confirmed in our database");
        sb.AppendLine();
        
        lbltext.Text = sb.ToString();
    }
}
