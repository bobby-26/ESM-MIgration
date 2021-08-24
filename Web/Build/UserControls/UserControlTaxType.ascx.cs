using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControlTaxType : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string TaxType
    {
        get
        {
            return rblValuePercentage.SelectedValue;
        }
        set
        {
            rblValuePercentage.SelectedValue = value;
        }
    }

    public void AttributesAdd(string key, string value)
    {
        rblValuePercentage.Attributes.Add(key, value);
    }
    public bool Enabled
    {
        set
        {
            rblValuePercentage.Enabled = value;
        }
    }
}
