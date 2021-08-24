using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlAddressType : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool _corporate;
    string addresstype = "";
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlAddressType.DataSource = PhoenixRegistersAddress.ListAddress(addresstype);
            ddlAddressType.DataBind();

            foreach (RadComboBoxItem item in ddlAddressType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

        }
    }

    public string AddressType
    {
        get
        {
            return addresstype;
        }
        set
        {
            addresstype = value;
        }
    }

    public DataSet AddressList
    {
        set
        {
            ddlAddressType.Items.Clear();
            ddlAddressType.DataSource = value;
            ddlAddressType.DataBind();

            foreach (RadComboBoxItem item in ddlAddressType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public bool AppendCorporate
    {
        set
        {
            _corporate = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlAddressType.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlAddressType.AutoPostBack = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlAddressType.Enabled = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlAddressType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedAddress
    {
        get
        {
            return ddlAddressType.SelectedValue;
        }
        set
        {
            ddlAddressType.Text = "";
            ddlAddressType.SelectedIndex = -1;
            if (value.Trim().Equals("") || General.GetNullableInteger(value) == null)
            {
                ddlAddressType.ClearSelection();
                ddlAddressType.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);

            foreach (RadComboBoxItem item in ddlAddressType.Items)
            {

                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

        }
    }

    public string SelectedText
    {
        get
        {
            return ddlAddressType.Text;
        }
    }


    protected void ddlAddressType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
        {
            if (!_corporate)
                ddlAddressType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            else
                ddlAddressType.Items.Insert(0, new RadComboBoxItem("--Manager--", "Dummy"));
        }
    }

    public string Width
    {
        get
        {
            return ddlAddressType.Width.ToString();
        }
        set
        {
            ddlAddressType.Width = Unit.Parse(value);
        }
    }

    public string EmptyMessage
    {
        get { return ddlAddressType.EmptyMessage; }
        set
        {
            ddlAddressType.EmptyMessage = value;
            ddlAddressType.ToolTip = value;
        }
    }
}
