using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Threading;
using System.Globalization;
using Telerik.Web.UI;

public partial class UserControlDate : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;

    protected override void OnInit(EventArgs e)
    {
        DateTimeFormatInfo di = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
        DateTimeFormat = di.ShortDatePattern;
        txtDate.Culture = new System.Globalization.CultureInfo(Thread.CurrentThread.CurrentCulture.Name, true);
        txtDate.DateInput.DateFormat = di.ShortDatePattern;
        txtDate.MaxDate = DateTime.Parse("31/12/2099");
        txtDate.MinDate = DateTime.Parse("01/01/1900");
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        GeneralSetting gs = PhoenixGeneralSettings.CurrentGeneralSetting;
        DatePicker = gs != null ? gs.DatePicker : true;
        if (txtDate.TimePopupButton.Visible == true)
        {
            txtDate.DateInput.DateFormat = "dd/MM/yyyy hh:mm tt"; 
            txtDate.DateInput.DisplayDateFormat = "dd/MM/yyyy hh:mm tt";
        }
    }

    public bool ReadOnly
    {
        set
        {
            txtDate.Attributes.Add("style", "Visiblity:hidden");
            txtDate.Calendar.Attributes.Add("style", "Visiblity:hidden");
            if (value)
            {
                txtDate.CssClass = "readonlytextbox";
                txtDate.CssClass = "readonlytextbox";
            }
        }
    }

    public string CssClass
    {
        set
        {
            txtDate.CssClass = value;
            txtDate.Calendar.CssClass = value;
        }
        get
        {
            return txtDate.CssClass;
        }
    }


    public bool AutoPostBack
    {
        set
        {
            txtDate.AutoPostBack = value;
        }
    }

    public bool TimeProperty
    {
        set
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                txtDate.TimePopupButton.Visible = false;
            }
            else
            {
                txtDate.TimePopupButton.Visible = value;
            }
        }
    }
    public string Text
    {
        get
        {
            string date = General.GetNullableString("");
            DateTime dt = Convert.ToDateTime(txtDate.SelectedDate);
            DateTime resultdate;

            if (DateTime.TryParse(dt.ToString(), out resultdate))
            {              
                date = dt.ToShortDateString();

                int i = DateTime.Compare(DateTime.Parse(date), txtDate.MinDate);
                int j = DateTime.Compare(DateTime.Parse(date), txtDate.MaxDate);

                if (i == 1 && j == -1)
                {
                    //date = txtDate.SelectedDate.ToString();                    
                    date = dt.ToShortDateString();
                }
                else
                {
                    date = null;
                    txtDate.Clear();
                }
            }
            else
            {
                date = null;
                if (string.IsNullOrEmpty(dt.ToString()))
                {
                    txtDate.Clear();
                }
            }

            return date;
        }
        set
        {
            DateTime result;

            if (DateTime.TryParse(value, out result))
            {
                result = DateTime.Parse(value);
                value = result.ToString("d");
            }

            if (string.IsNullOrEmpty(value))
            {
                txtDate.Clear();
            }
            else
            {
                txtDate.SelectedDate = DateTime.Parse(value.ToString());
            }

        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void OnTextChange(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public bool Enabled
    {
        get
        {
            return txtDate.Enabled;
        }
        set
        {
            txtDate.Enabled = value;
        }
    }
    public string Width
    {
        get
        {
            return txtDate.Width.ToString();
        }
        set
        {
            txtDate.Width = Unit.Parse(value);
        }
    }

    public string Tooltip
    {
        get
        {
            return txtDate.ToolTip.ToString();
        }
        set
        {
            txtDate.ToolTip = value;
        }
    }

    public bool DatePicker
    {
        set
        {
            //cxtxtDate.Enabled = value;
        }
    }

    public string DateTimeFormat
    {
        get
        {
            return txtDate.DateInput.DateFormat;
        }
        set
        {
            txtDate.DateInput.DateFormat = value;
            txtDate.DateInput.DisplayDateFormat = value;
        }
    }

    public bool DateIconVisible
    {
        set
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                txtDate.DatePopupButton.Visible = false;
            }
            else
            {
                txtDate.DatePopupButton.Visible = value;
            }
        }
    }

}
