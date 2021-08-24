using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlFleet : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet FleetList
    {
        set
        {
            ucFleet.DataSource = value;
            ucFleet.DataBind();
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

    public string CssClass
    {
        set
        {
            ucFleet.CssClass = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ucFleet.Enabled = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ucFleet.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ucFleet_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedFleet
    {
        get
        {
            return ucFleet.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ucFleet.SelectedIndex = -1;
                ucFleet.ClearSelection();
                ucFleet.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ucFleet.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucFleet.Items)
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
        ucFleet.Items.Clear();
        ucFleet.DataSource = PhoenixRegistersFleet.ListFleet();
        ucFleet.DataBind();
        foreach (RadComboBoxItem item in ucFleet.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }

    }
    protected void ucFleet_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucFleet.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public Unit Width
    {
        set
        {
            ucFleet.Width = value;
        }
        get
        {
            return ucFleet.Width;
        }
    }
}
