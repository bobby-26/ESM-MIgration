using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogDeckLog : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AssignLink();
    }

    private void AssignLink()
    {
        btnGpsEchoSounderLog.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('GPSLog','','{0}/Log/GPSLog.aspx');", Session["sitepath"]));
        btnRadarLog.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('RadarLog','','{0}/Log/RadarLog.aspx');", Session["sitepath"]));
        btnDeckLogBook.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('DeckLog','','{0}/Log/ElectricLogDeckLogBook.aspx');", Session["sitepath"]));
    }
}