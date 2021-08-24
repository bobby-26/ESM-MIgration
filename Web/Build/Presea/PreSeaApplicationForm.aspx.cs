using System;
using SouthNests.Phoenix.Framework;
public partial class PreSeaApplicationForm : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            filterandsearch.Attributes["src"] = "PreSeaOnlineApplication.aspx";
        }
    }
}
