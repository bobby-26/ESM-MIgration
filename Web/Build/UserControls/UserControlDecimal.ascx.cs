using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControlDecimal : System.Web.UI.UserControl
{
    private void Page_Init(object sender, EventArgs e)
    {
        // to fix "backspace and delete" problem in mask edit
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "MaskedEditFix", "(function(){try{var n=Sys.Extended.UI.MaskedEditBehavior.prototype,t=n._ExecuteNav;n._ExecuteNav=function(n){var i=n.type;i==\"keydown\"&&(n.type=\"keypress\"),t.apply(this,arguments),n.type=i}}catch(i){return}})();", true);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string ReadOnly
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
            {
                txtDecimal.ReadOnly = true;
                txtDecimal.CssClass = "readonlytextbox";
            }
            else
            {
                txtDecimal.ReadOnly = false;
                txtDecimal.CssClass = "input";
            }
        }
    }

    public string CssClass
    {
        set
        {
            txtDecimal.CssClass = value;
        }
    }

    public void SetFocus()
    {
        txtDecimal.Focus();
    }

    public string Text
    {
        get
        {
            return txtDecimal.Text;
        }
        set
        {
            txtDecimal.Text = value;
        }
    }

    public string DecimalSixDigit
    {
        set
        {
            //if (value.ToUpper().Equals("TRUE"))            
            //    txtDecimalMask.Mask = "99,999,999,999.999999";            
            //else
            //    txtDecimalMask.Mask = "999,999,999,999,999.99";
        }
    }
    public string Mask
    {
        set
        {
           //txtDecimalMask.Mask = value;
        }
    }


    public string Width
    {
        get
        {
            return txtDecimal.Width.ToString();
        }
        set
        {
            txtDecimal.Width = Unit.Parse(value);
        }
    }
    public int DecimalDigits
    {
        get
        {
            return txtDecimal.NumberFormat.DecimalDigits;
        }
        set
        {
            txtDecimal.NumberFormat.DecimalDigits = value;
        }
    }
    public bool InterceptArrowKeys
    {
        get
        {
            return txtDecimal.IncrementSettings.InterceptArrowKeys;
        }
        set
        {
            txtDecimal.IncrementSettings.InterceptArrowKeys = value;
        }
    }
    public bool InterceptMouseWheel
    {
        get
        {
            return txtDecimal.IncrementSettings.InterceptMouseWheel;
        }
        set
        {
            txtDecimal.IncrementSettings.InterceptMouseWheel = value;
        }
    }
    public double MinValue
    {
        get
        {
            return txtDecimal.MinValue;
        }
        set
        {
            txtDecimal.MinValue = value;
        }
    }
    public double MaxValue
    {
        get
        {
            return txtDecimal.MaxValue;
        }
        set
        {
            txtDecimal.MaxValue = value;
        }
    }
    public bool Enabled
    {
        get
        {
            return txtDecimal.Enabled;
        }
        set
        {
            txtDecimal.Enabled = value;
        }
    }
}
