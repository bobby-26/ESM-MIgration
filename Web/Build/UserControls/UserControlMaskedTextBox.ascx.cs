using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlMaskedTextBox : System.Web.UI.UserControl
{
    public delegate void TextChangedDelegate(object o, EventArgs e);
    public event TextChangedDelegate TextChangedEvent;
    private int _decimalplace = 2;
    private bool _positive = false;
    string _onblur;
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public bool IsInteger
    {
        get
        {
            if (ViewState["ISINTEGER"] == null)
                return false;
            return (bool)ViewState["ISINTEGER"];
        }
        set
        {
            ViewState["ISINTEGER"] = value;
        }
    }
    public bool IsPositive
    {
        get { return _positive; }
        set { _positive = value; }
    }
    public bool DefaultZero
    {
        get;
        set;
    }
    public string OnblurScript
    {
        get
        {
            return _onblur;
        }
        set
        {
            _onblur = value;
            txtNumber.Attributes.Add("onblur", txtNumber.Attributes["onblur"] + _onblur);
        }
    }
    public int DecimalPlace
    {
        set
        {
            _decimalplace = value;
        }
        get
        {
            return _decimalplace;
        }
    }
    public void SetFocus()
    {
        txtNumber.Focus();
    }

    public bool AutoPostBack
    {
        set
        {
            txtNumber.AutoPostBack = value;
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

    public string TextWithLiterals
    {
        get
        {
            return txtNumber.TextWithLiterals;
        }
        set
        {
            txtNumber.TextWithLiterals = value;
        }
    }
    public Unit Width
    {
        get
        {
            return txtNumber.Width;
        }
        set
        {
            txtNumber.Width = value;
        }
    }
    public int MaxLength
    {
        set
        {
            txtNumber.MaxLength = value;
        }
        get
        {
            return txtNumber.MaxLength;
        }
    }

    public string CssClass
    {
        set
        {
            txtNumber.CssClass = value + (!value.ToLower().Contains("txtnumber") ? (" txtNumber small") : string.Empty);
        }
        get
        {
            return txtNumber.CssClass;
        }
    }

    public bool ReadOnly
    {
        set
        {
            txtNumber.ReadOnly = value;
        }
        get
        {
            return txtNumber.ReadOnly;
        }
    }

    public bool Enabled
    {
        set
        {
            txtNumber.Enabled = value;
        }
        get
        {
            return txtNumber.Enabled;
        }
    }

    public new bool Visible
    {
        set
        {
            txtNumber.Visible = value;
        }
        get
        {
            return txtNumber.Visible;
        }
    }

    protected void OnUserControlMaskNumberChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void txtNumber_TextChanged(object sender, EventArgs e)
    {
        if (sender is RadMaskedTextBox)
        {
            OnUserControlMaskNumberChangedEvent(e);
        }
    }
    public string Tooltip
    {
        get
        {
            return txtNumber.ToolTip.ToString();
        }
        set
        {
            txtNumber.ToolTip = value;
        }
    }

    public string MaskText
    {
        set
        {
            txtNumber.Mask = value;
        }
    }

}