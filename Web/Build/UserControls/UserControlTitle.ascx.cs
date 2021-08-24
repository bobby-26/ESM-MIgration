using System;
using Telerik.Web.UI;

public partial class UserControls_UserControlTitle : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!IsPostBack)
        {
            chkShowMenu.Attributes.Add("onclick", "javascript:ResizeMenu(this)");
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

    public string ShowMenu
    {
        set
        {
            if (value.ToUpper().Equals("FALSE"))
                chkShowMenu.Visible = false;
            else
                chkShowMenu.Visible = true;
        }
        get
        {
            return chkShowMenu.Visible.ToString();
        }
    }   
}
