using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class UserControlCopyMessageInventory : System.Web.UI.UserControl
{

    public event   EventHandler ConfirmMesage;
    private bool visible = false;
    public int confirmboxvalue = 0;

    protected void Page_Load(object sender, EventArgs e)
    { 
        
    }
   
    protected void ConfirmMesageCommand(EventArgs e)
    {
        if (ConfirmMesage != null)
            ConfirmMesage(this, e);
    }

    protected void cmdYes_Click(object sender, EventArgs e)
    {
        confirmboxvalue = 1;
        ConfirmMesageCommand(e);
        if (!visible)
        {            
            this.Visible = false;
        }
    }

    protected void cmdNo_Click(object sender, EventArgs e)
    {
        confirmboxvalue = 0;
        ConfirmMesageCommand(e);        
        this.Visible = false; 
    }
   
    public bool ConfirmVisible
    {
        set
        {
            visible = value;
        }
    }    
    public string SelectedList
    {
        get
        {
            string str = string.Empty;
            foreach (ButtonListItem li in chkOPtion.Items)
            {
                str += (li.Selected ? li.Value + "," : string.Empty);
            }
            return str.TrimEnd(',');
        }
    }
}
