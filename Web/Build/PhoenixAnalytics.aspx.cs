using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Configuration;
public partial class PhoenixAnalytics : System.Web.UI.Page
{
    string Tokenid;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Filter.CurrentLoginToken != null)
        {
            Tokenid = Filter.CurrentLoginToken.ToString();

            BindLink();

        }
    }

    public void BindLink()
    {

        string Url = ConfigurationManager.AppSettings["AnalyticsUrl"].ToString();

        string pageurl = Url +"?Token="+ Tokenid;
        
        Response.Redirect(pageurl);
        
    }


}