using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlSeaport : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool _appenditematsea;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet SeaportList
    {
        set
        {
            ucSeaport.Items.Clear();
            ucSeaport.DataSource = value;
            ucSeaport.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ucSeaport.CssClass = value;
        }
    }

    public string AppendDataBoundItems
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                _appenddatabounditems = true;
            else
                _appenddatabounditems = false;

        }
    }
    public bool Enabled
    {
        set
        {
            ucSeaport.Enabled = value;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ucSeaport.AutoPostBack = true;
        }
    }

    public string SelectedSeaport
    {
        get
        {

            return ucSeaport.SelectedValue;
        }
        set
        {
            ucSeaport.SelectedIndex = -1;
            ucSeaport.Text = "";
            ucSeaport.ClearSelection();
            if (value == string.Empty || General.GetNullableInteger(value) == null)
            {
                ucSeaport.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            foreach (RadComboBoxItem item in ucSeaport.Items)
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
        ucSeaport.DataSource = PhoenixRegistersSeaport.ListSeaport();
        ucSeaport.DataBind();
        foreach (RadComboBoxItem item in ucSeaport.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public int SelectedValue
    {
        get
        {

            return (int.Parse(ucSeaport.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ucSeaport.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucSeaport.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    protected void ucSeaport_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucSeaport.Items.Insert(0, new RadComboBoxItem("--select--", "Dummy"));
        if (_appenditematsea)
            ucSeaport.Items.Insert(1, new RadComboBoxItem("AT SEA", "0"));
    }
    public string Width
    {
        get
        {
            return ucSeaport.Width.ToString();
        }
        set
        {
            ucSeaport.Width = Unit.Parse(value);
        }
    }

    public string AppendItemAtSea
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                _appenditematsea = true;
            else
                _appenditematsea = false;

        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ucSeaport_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

}
