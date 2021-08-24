using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
public partial class UserControlVesselMappingCurrency : System.Web.UI.UserControl
{
    int Vessel;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool? _active = false;
    private int _selectedValue = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                bind();
            }
            catch { }
        }
    }
    public string VesselId
    {
        get
        {
            return VesselId.ToString();
        }
        set
        {
            Vessel = Int32.Parse(value);
        }
    }

    public bool ActiveCurrency
    {
        set
        {
            _active = value;
        }
    }

    public DataSet CurrencyList
    {
        set
        {
            ddlCurrency.DataSource = value;
            ddlCurrency.DataBind();
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlCurrency.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlCurrency.AutoPostBack = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlCurrency_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedCurrency
    {
        get
        {
            return ddlCurrency.SelectedValue;
        }
        set
        {
            ddlCurrency.SelectedIndex = -1;
            ddlCurrency.ClearSelection();
            ddlCurrency.Text = string.Empty;
            if (value == string.Empty)
            {
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlCurrency.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCurrency.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }

    }
    private void bind()
    {
        ddlCurrency.DataSource = PhoenixVesselAccountsCurrencyConfiguration.ListCurrencyConfiguration(General.GetNullableInteger(Vessel.ToString()), null);
        ddlCurrency.DataBind();
        foreach (RadComboBoxItem item in ddlCurrency.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlCurrency_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCurrency.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public bool Enabled
    {
        set
        {
            ddlCurrency.Enabled = value;
        }
    }

    public Unit Width
    {
        get
        {
            return ddlCurrency.Width;
        }
        set
        {
            ddlCurrency.Width = value;
        }
    }
    public string SelectedCurrencyText
    {
        get
        {
            return ddlCurrency.SelectedItem.Text;
        }

    }
}
