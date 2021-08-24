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

public partial class OptionsSwitch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Filter.CurrentSelectedModule != null)
        {
            if (Filter.CurrentSelectedModule.ToUpper().Equals("ACCOUNTS") || Filter.CurrentSelectedModule.ToUpper().Replace(" ", "").Equals("DOCUMENTMANAGEMENT"))
                Response.Redirect("OptionsChooseCompany.aspx"); 
            else
                Response.Redirect("OptionsChooseVessel.aspx");
        }
        else
            Response.Redirect("OptionsChooseVessel.aspx");
    }
}
