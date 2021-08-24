using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class UserControlContractCompany : System.Web.UI.UserControl
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

    public DataTable FlagList
    {
        set
        {
            ddlContractCompany.Items.Clear();
            ddlContractCompany.DataSource = value;
            ddlContractCompany.DataBind();
        }
    }
    public Unit Width
    {
        set
        {
            ddlContractCompany.Width = Unit.Parse(value.ToString());
        }
        get
        {
            return ddlContractCompany.Width;
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
            ddlContractCompany.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlContractCompany.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlContractCompany.Enabled = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlContractCompany_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedCompany
    {
        get
        {
            return ddlContractCompany.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlContractCompany.SelectedIndex = -1;
                ddlContractCompany.ClearSelection();
                ddlContractCompany.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlContractCompany.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlContractCompany.Items)
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
        ddlContractCompany.DataSource = PhoenixRegistersContractCompany.ListContractCompany();
        ddlContractCompany.DataBind();
        foreach (RadComboBoxItem item in ddlContractCompany.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlContractCompany_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlContractCompany.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
}
