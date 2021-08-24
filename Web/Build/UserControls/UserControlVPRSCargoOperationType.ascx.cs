using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlVPRSCargoOperationType : System.Web.UI.UserControl
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

    public DataSet CargoOperationTypeList
    {
        set
        {
            ddlCargoOperationType.DataBind();
            ddlCargoOperationType.Items.Clear();
            ddlCargoOperationType.DataSource = value;
            ddlCargoOperationType.DataBind();
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
            ddlCargoOperationType.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlCargoOperationType.AutoPostBack = true;
        }
    }

    public string SelectedCargoOperationType
    {
        get
        {
            return ddlCargoOperationType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlCargoOperationType.SelectedIndex = -1;
                ddlCargoOperationType.ClearSelection();
                ddlCargoOperationType.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlCargoOperationType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCargoOperationType.Items)
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
            ddlCargoOperationType.Enabled = value;
        }
    }

    public void bind()
    {
        ddlCargoOperationType.DataSource = PhoenixRegistersCargoOperationType.ListCargoOperationType();
        ddlCargoOperationType.DataBind();
        foreach (RadComboBoxItem item in ddlCargoOperationType.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlCargoOperationType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlCargoOperationType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCargoOperationType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
