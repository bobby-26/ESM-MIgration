using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlContractCrew : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet CrewComponentsList
    {
        set
        {
            ddlCrewComponents.Items.Clear();
            ddlCrewComponents.DataSource = value;
            ddlCrewComponents.DataBind();
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
            ddlCrewComponents.CssClass = value;
        }
        get
        {
            return ddlCrewComponents.CssClass;
        }
    }
    public bool AutoPostBack
    {
        set
        {
            ddlCrewComponents.AutoPostBack = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlCrewComponents.Enabled = value;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void ddlCrewComponents_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    public string SelectedComponent
    {
        get
        {
            return ddlCrewComponents.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlCrewComponents.SelectedIndex = -1;
                ddlCrewComponents.ClearSelection();
                ddlCrewComponents.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddlCrewComponents.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCrewComponents.Items)
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
        ddlCrewComponents.DataSource = PhoenixRegistersContract.ListContractCrew(null);
        ddlCrewComponents.DataBind();
        foreach (RadComboBoxItem item in ddlCrewComponents.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlCrewComponents_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCrewComponents.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    public Unit Width
    {
        get
        {
            return ddlCrewComponents.Width;
        }
        set
        {
            ddlCrewComponents.Width = value;
        }
    }
}
