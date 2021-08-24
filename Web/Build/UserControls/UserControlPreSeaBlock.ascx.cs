using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;

public partial class UserControlPreSeaBlock : System.Web.UI.UserControl
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

    public DataSet BlockList
    {
        set
        {
            ucBlock.DataSource = value;
            ucBlock.DataBind();
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
            ucBlock.CssClass = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ucBlock.Enabled = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ucBlock.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ucBlock_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedBlock
    {
        get
        {
            return ucBlock.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ucBlock.SelectedIndex = -1;
                ucBlock.ClearSelection();
                ucBlock.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ucBlock.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucBlock.Items)
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
        ucBlock.Items.Clear();
        ucBlock.DataSource = PhoenixPreSeaBlock.ListPreSeaBlock(null);
        ucBlock.DataBind();
        foreach (RadComboBoxItem item in ucBlock.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }

    }
    protected void ucBlock_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucBlock.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
