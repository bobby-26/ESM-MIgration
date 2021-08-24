using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlCurrencyList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstCurrency.DataSource = PhoenixRegistersCurrency.ListCurrency(null);
            lstCurrency.DataBind();

        }
    }

    public string selectedlist
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstCurrency.Items)
            {
                if (item.Checked == true)
                {

                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }

            }
            if (strlist.Length > 1)
            {
                strlist.Remove(strlist.Length - 1, 1);
            }
            return strlist.ToString();
        }
        set
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstCurrency.Items)
            {
                if (item.Checked == true)
                {

                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }

            }
            if (strlist.Length > 1)
            {
                strlist.Remove(strlist.Length - 1, 1);
            }
            value = strlist.ToString();
        }
    }
    public DataSet CurrencyList
    {
        set
        {
            lstCurrency.Items.Clear();
            lstCurrency.DataSource = value;
            lstCurrency.DataBind();
        }
    }
    public bool AppendDataBoundItems
    {
        set
        {
            lstCurrency.AppendDataBoundItems = value;
        }
    }

    public string CssClass
    {
        set
        {
            divCurrencyList.Attributes["class"] = value;
            lstCurrency.CssClass = value;
        }
    }
    public Unit Width
    {
        set
        {
            lstCurrency.Width = Unit.Parse(value.ToString());
            divCurrencyList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstCurrency.Width;
        }
    }
    public string SelectedCurrencyName
    {
        get
        {
            return lstCurrency.SelectedItem.Text;
        }
    }

    public string SelectedCurrencyValue
    {
        get
        {

            return lstCurrency.SelectedValue;
        }
        set
        {
            lstCurrency.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                lstCurrency.SelectedValue = value;
        }
    }

    public int SelectedValue
    {
        get
        {

            return (int.Parse(lstCurrency.SelectedValue));
        }
        set
        {
            value = (int.Parse(lstCurrency.SelectedValue));
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void lstCurrency_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
}
