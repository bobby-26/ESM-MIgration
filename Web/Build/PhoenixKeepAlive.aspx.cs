using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;

public partial class PhoenixKeepAlive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlMeta meta = new HtmlMeta();
        meta.HttpEquiv = "Refresh";
        Response.AppendHeader("Refresh", "60");
        Filter.SessionTimeoutTracker = Filter.SessionTimeoutTracker + 60;
        if (Filter.SessionTimeoutTracker > 14400)   //Timeout after 4 hours if no menu item is clicked
        {
            Response.Redirect("PhoenixLogout.aspx");
        }
    }
}
