using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlZone : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool _appenditematallzone;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }

    }

    public DataSet ZoneList
    {
        set
        {
            ddlZone.DataSource = value;
            ddlZone.DataBind();
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
            ddlZone.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlZone.AutoPostBack = value;
        }
    }
	public bool Enabled
	{
		set
		{
			ddlZone.Enabled = value;
		}
	}

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlZone_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedZone
    {
        get
        {
            return ddlZone.SelectedValue;
        }
        set
        {
            ddlZone.SelectedIndex = -1;
            if (value == string.Empty)
            {
                ddlZone.ClearSelection();
                ddlZone.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlZone.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlZone.Items)
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
        ddlZone.DataSource = PhoenixRegistersMiscellaneousZoneMaster.ListMiscellaneousZoneMaster();
        ddlZone.DataBind();
        foreach (RadComboBoxItem item in ddlZone.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public string SelectedZoneName
    {
        get
        {
            return ddlZone.SelectedItem.Text;
        }
    }

    protected void ddlZone_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlZone.Items.Insert(0, new RadComboBoxItem("--Select--","Dummy"));
        if (_appenditematallzone)
            ddlZone.Items.Insert(1, new RadComboBoxItem("For Multiple Zone", "0"));
    }

    public string AppendItemAllZone
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                _appenditematallzone = true;
            else
                _appenditematallzone = false;

        }
    }
    public Unit Width
    {
        set
        {
            ddlZone.Attributes.Add("style", "width:" + value);
        }
    }
}
