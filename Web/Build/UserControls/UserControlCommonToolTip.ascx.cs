using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlCommonToolTip : System.Web.UI.UserControl
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        if(ToolTipManager == null)
        {
            List<Control> c = new List<Control>();
            FindControls(ref c, Page.Form);
            if (c.Count > 0)
            {               
                ToolTipManager = ((RadToolTipManager)c[0]);
            }
        }
        ToolTipManager.TargetControls.Add(imgToolTip.ClientID, this.Screen, true);
    }
    public string Screen
    {
        get;
        set;
        //{
        //    Tooltipframe.Src = value;
        //}
    }
    public RadToolTipManager ToolTipManager
    {
        set;
        get;
    }
    public string TargetControlId
    {
        set
        {
            this.ToolTipManager.TargetControls.Add(value, true);
        }
    }

    public ToolTipPosition Position
    {
        set
        {
            ToolTipManager.Position = value;
        }
    }

    public ToolTipShowEvent ShowEvent
    {
        set
        {
            ToolTipManager.ShowEvent = value;
        }
    }

    public ToolTipHideEvent HideEvent
    {
        set
        {
            ToolTipManager.HideEvent = value;
        }
    }
    public Unit Width
    {
        set
        {
            ToolTipManager.Width = value;
        }
    }

    public Boolean Modal
    {
        set
        {
            ToolTipManager.Modal = value;
        }
    }
    private void FindControls(ref List<Control> list, Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.ID == "RadCommonToolTipManager1")
            {
                list.Add(c);
                break;
            }
            try
            {
                if (c.Controls.Count > 0)
                {
                    this.FindControls(ref list, c);
                }
            }
            catch { }
        }
    }    
}
