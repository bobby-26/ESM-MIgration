using System;
using SouthNests.Phoenix.Framework;

public partial class UserControlLongitude : System.Web.UI.UserControl
{
    string _dummy = null;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public bool ReadOnly
    {
        set
        {
            txtDegree.ReadOnly = value;
            txtMinute.ReadOnly = value;
            txtSecond.ReadOnly = value;

            if (value)
            {
                txtDegree.CssClass = "readonlytextbox";
                txtMinute.CssClass = "readonlytextbox";
                txtSecond.CssClass = "readonlytextbox";
                ddlDirection.Enabled = false;
            }
        }
    }

    public string CssClass
    {
        set
        {
            txtDegree.CssClass = value;
            txtMinute.CssClass = value;
            txtSecond.CssClass = value;
            if (value.Contains("mandatory"))
                ddlDirection.CssClass = "dropdown_mandatory";
            else
                ddlDirection.CssClass = "input";

        }
    }

    public string Text
    {
        get
        {
            if (string.IsNullOrEmpty(txtDegree.Text.Trim()))
                txtDegree.Text = "00";
            if (string.IsNullOrEmpty(txtMinute.Text.Trim()))
                txtMinute.Text = "00";
            if (string.IsNullOrEmpty(txtSecond.Text.Trim()))
                txtSecond.Text = "00";

            if ((Convert.ToInt32(txtDegree.Text.Trim()) <= 0) && (Convert.ToInt32(txtMinute.Text.Trim()) <= 0) && (Convert.ToInt32(txtSecond.Text.Trim()) <= 0))
                return _dummy;
            else
                return General.GetNullableString(txtDegree.Text) + "-" + General.GetNullableString(txtMinute.Text) + "-" + General.GetNullableString(txtSecond.Text) + "-" + General.GetNullableString(ddlDirection.SelectedValue);
        }
        set
        {
            if (value.Contains("-"))
            {
                txtDegree.Text = value.Split('-')[0];
                txtMinute.Text = value.Split('-')[1];
                txtSecond.Text = value.Split('-')[2];
                ddlDirection.SelectedValue = value.Split('-')[3];
            }
            else
                txtDegree.Text = value;


        }
    }
    public string TextDirection
    {
        set
        {
            ddlDirection.SelectedValue = value;
        }
        get
        {
            return General.GetNullableString(ddlDirection.SelectedValue);
        }
    }


    public string TextDegree
    {
        set
        {
            txtDegree.Text = value;
        }
        get
        {
            return txtDegree.Text;
        }
    }

    public string TextMinute
    {
        set
        {
            txtMinute.Text = value;
        }
        get
        {
            return txtMinute.Text;
        }
    }

    public string TextSecond
    {
        set
        {
            txtSecond.Text = value;
        }
        get
        {
            return txtSecond.Text;
        }
    }

    public void Clear()
    {
        txtDegree.Text = "";
        txtMinute.Text = "";
        txtSecond.Text = "";
    }

    public void txtDegree_OnTextChangedEvent(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtDegree.Text.Trim()))
            txtDegree.Text = "00";

        if (Convert.ToInt32(txtDegree.Text) < 0 || Convert.ToInt32(txtDegree.Text) > 179)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = "Longitude value should not exceed 180&deg";
            ucError.Visible = true;
            txtDegree.Text = "";
        }        
    }
    public void txtMinute_OnTextChangedEvent(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtMinute.Text.Trim()))
            txtMinute.Text = "00";

        if (Convert.ToInt32(txtMinute.Text) < 0 || Convert.ToInt32(txtMinute.Text) > 59)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = "Longitude value should not equal to or  exceed 60 minutes ";
            ucError.Visible = true;
            txtMinute.Text = "";
        }
    }
    public void txtSecond_OnTextChangedEvent(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtSecond.Text.Trim()))
            txtSecond.Text = "00";

        if (Convert.ToInt32(txtSecond.Text) < 0 || Convert.ToInt32(txtSecond.Text) > 59)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = "Longitude value should not equal to or exceed 60 seconds";
            ucError.Visible = true;
            txtSecond.Text = "";
        }
    }


}
