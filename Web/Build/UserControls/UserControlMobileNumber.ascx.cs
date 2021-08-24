using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class UserControlMobileNumber : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string ReadOnly
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
            {
                txtMobileNumber.ReadOnly = true;
                txtMobileNumber.CssClass = "readonlytextbox";
            }
            else
                txtMobileNumber.ReadOnly = false;
        }
    }

    public string CssClass
    {
        set
        {
            txtMobileNumber.CssClass = value;
        }
    }

    public string Text
    {
        get
        {
            string mobilenumber = General.GetNullableString("");
            if (txtMobileNumber.Text != "____-___-___")
                mobilenumber = txtMobileNumber.Text;
            return mobilenumber;
        }
        set
        {
            if (value.Contains("~"))
            {
                txtMobileNumber.Text = value.Split('~')[1];
            }
            else
                txtMobileNumber.Text = value;

        }
    }

    public string Width
    {
        get
        {
            return txtMobileNumber.Width.ToString();
        }
        set
        {
            txtMobileNumber.Width = Unit.Parse(value);
        }
    }
}
