using System;

public partial class UserControlTitleTelerik : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!IsPostBack)
        {            
            chkShowMenu.Checked = true;
        }
    }

    public string Text
    {
        set
        {
            lblTitle.Text = value;
        }
        get
        {
            return lblTitle.Text;
        }
    }

    public bool ShowMenu
    {
        set
        {
           chkShowMenu.Visible = value;
        }
        get
        {
            return chkShowMenu.Visible;
        }
    }   
}
