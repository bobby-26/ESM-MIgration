using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlContractCBA : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    public event EventHandler TextChangedEvent;
    private string _selectedValue = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataTable ContractList
    {
        set
        {
            ddlContract.Items.Clear();
            ddlContract.DataSource = value;
            ddlContract.DataBind();
        }
    }
    public int? AddressCode
    {
        get;
        set;
    }
    public Guid? MainComponent
    {
        get;
        set;
    }
	public bool Enabled
	{
		set
		{
            ddlContract.Enabled = value;
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
            ddlContract.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlContract.AutoPostBack = value;
        }
    }

    public string SelectedContract
    {
        get
        {
            return ddlContract.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlContract.SelectedIndex = -1;
                ddlContract.ClearSelection();
                ddlContract.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddlContract.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlContract.Items)
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
        ddlContract.DataSource = PhoenixRegistersContract.ListCBAContract(AddressCode, MainComponent, null);
        ddlContract.DataBind();
        foreach (RadComboBoxItem item in ddlContract.Items)
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

    protected void ddlContract_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlContract_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlContract.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
}
