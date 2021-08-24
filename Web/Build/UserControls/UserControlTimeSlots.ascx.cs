using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;

public partial class UserControlTimeSlots : System.Web.UI.UserControl
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
            ddlTimeSlot.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlTimeSlot.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlTimeSlot.Enabled = value;
        }
    }

    public Unit Width
    {
        set
        {
            ddlTimeSlot.Width = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlTimeSlot_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedTimeSlot
    {
        get
        {
            return ddlTimeSlot.SelectedItem.Text;
        }
    }

    public string SelectedTime
    {
        get
        {
            return ddlTimeSlot.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlTimeSlot.SelectedIndex = -1;
                ddlTimeSlot.ClearSelection();
                ddlTimeSlot.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlTimeSlot.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlTimeSlot.Items)
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
        ddlTimeSlot.DataSource = PhoenixPreSeaCommon.GetTimeSlots();
        ddlTimeSlot.DataBind();
        foreach (RadComboBoxItem item in ddlTimeSlot.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlTimeSlot_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlTimeSlot.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
