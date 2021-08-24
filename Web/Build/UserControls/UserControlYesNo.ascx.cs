using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class UserControlYesNo : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string YesNoOption
    {
        get
        {
            return rblYesNo.SelectedValue;
        }
        set
        {
            if(value.Equals("Yes"))
                rblYesNo.SelectedIndex =0;
            else 
                rblYesNo.SelectedIndex =1; 
            rblYesNo.SelectedValue = value;

            
        }
    }

    public void AttributesAdd(string key, string value)
    {
        rblYesNo.Attributes.Add(key, value);
    }
}
