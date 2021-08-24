using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlVPRSEventType : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet EventTypeList
    {
        set
        {
            ddlEventType.DataBind();
            ddlEventType.Items.Clear();
            ddlEventType.DataSource = value;
            ddlEventType.DataBind();
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
            ddlEventType.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlEventType.AutoPostBack = true;
        }
    }

    public string SelectedEventType
    {
        get
        {
            return ddlEventType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlEventType.SelectedIndex = -1;
                ddlEventType.ClearSelection();
                ddlEventType.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlEventType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlEventType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public bool Enabled
    {
        set
        {
            ddlEventType.Enabled = value;
        }
    }

    public void bind()
    {
        ddlEventType.DataSource = PhoenixRegistersVPRSEventType.ListEventType();
        ddlEventType.DataBind();
        foreach (RadComboBoxItem item in ddlEventType.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    public string SelectedValue
    {
        get
        {
            return ddlEventType.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlEventType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlEventType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlEventType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlEventType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlEventType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
