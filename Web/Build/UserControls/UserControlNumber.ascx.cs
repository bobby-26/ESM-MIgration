using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlNumber : System.Web.UI.UserControl
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
                txtNumber.ReadOnly = true;
                txtNumber.CssClass = "readonlytextbox";
            }
            else
                txtNumber.ReadOnly = false;
        }
    }

    public string CssClass
    {
        set
        {
            txtNumber.CssClass = value;
        }
    }

    public string Text
    {
        get
        {
            return txtNumber.Text;
        }
        set
        {
            txtNumber.Text = value;
        }
    }

    public string MaxLength
    {
        get
        {
            return txtNumber.MaxLength.ToString();

        }
        set
        {
            txtNumber.MaxLength = Int32.Parse(value);
        }
    }

    public string Width
    {
        get
        {
            return txtNumber.Width.ToString();
        }
        set
        {
            txtNumber.Width = Unit.Parse(value);
        }
    }

    public NumericType Type
    {
        get
        {
            return txtNumber.Type;
        }
        set
        {
            txtNumber.Type = value;
        }
    }

    public bool Enabled
    {
        get
        {
            return txtNumber.Enabled;
        }
        set
        {
            txtNumber.Enabled = value;
        }
    }

    public string userCtlClientId
    {
        get
        {
            return txtNumber.ClientID;
        }
    }
}
