using System;
using System.Web.UI;


public partial class UserControlSplitter : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public string TargetControlID
    {
        get
        {
            return spnTargetControlID.InnerText;
        }
        set
        {
            spnTargetControlID.InnerText = value;
        }
    }
}
