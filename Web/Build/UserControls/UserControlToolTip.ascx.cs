using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlToolTip : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string Text
    {
        set
        {
            RadToolTip1.Text = value;
        }
    }

    public string ToolTip
    {
        get
        {
            return RadToolTip1.ClientID;
        }
    }

	public Unit Width
	{
		set
		{
            RadToolTip1.Attributes.Add("style", " white-space:normal;word-wrap:break-word;width:" + value);
		}
	}

    public string TargetControlId
    {
        set
        {
            RadToolTip1.TargetControlID = value;
        }
    }

    public ToolTipPosition Position
    {
        set
        {
            RadToolTip1.Position = value;
        }
    }
}
