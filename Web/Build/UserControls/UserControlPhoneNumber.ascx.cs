using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
public partial class UserControlPhoneNumber : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsMobileNumber)
            txtAreaCode.Visible = false;
    }

    public bool ReadOnly
    {
        set
        {
            txtPhoneNumber.ReadOnly = value;
            txtAreaCode.ReadOnly = value;
            if (value)
            {
                txtPhoneNumber.CssClass = "readonlytextbox";
                txtAreaCode.CssClass = "readonlytextbox";
            }
        }
    }
	public bool IsValidPhoneNumber()
	{
		ucError.HeaderMessage = "Please provide the following required information";

        if (txtPhoneNumber.Text.Trim() != string.Empty)
		{
			if (txtAreaCode.Text.Trim().Equals(""))
				ucError.ErrorMessage = "Area code is required for Phone Number.";
		}
		return (!ucError.IsError);
	}

	public string ErrorMessage
	{
		get
		{
			return ucError.ErrorMessage;
		}
	}

    public string CssClass
    {
        set
        {
            txtPhoneNumber.CssClass = value;            
        }
    }

    public string ISDCode
    {
        get;
        set;
    }

    public bool IsMobileNumber
    {
        get;
        set;
    }

    public string Text
    {
        get
        {
            return General.GetNullableString(txtAreaCode.Text) + "~" + General.GetNullableString(txtPhoneNumber.Text);
        }
        set
        {
            if (value.Contains("~"))
            {
                txtAreaCode.Text = value.Split('~')[0];
                txtPhoneNumber.Text = value.Split('~')[1];
            }
            else
                txtPhoneNumber.Text = value;
        }
    }

    public string Width
    {
        get
        {
            return txtPhoneNumber.Width.ToString();
        }
        set
        {
            txtPhoneNumber.Width = Unit.Parse(value);
        }
    }
}
