using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;

public partial class UserControlPreSeaInfrastructureType : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool listpreseaInfrastructureType = true;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public bool ListpreseaInfrastructureType
    {
        get
        {
            return listpreseaInfrastructureType;
        }
        set
        {
            listpreseaInfrastructureType = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlPreSeaInfrastructureType.Enabled = value;
        }
    }
    public DataSet PreSeaInfrastructureTypeList
    {
        set
        {
            ddlPreSeaInfrastructureType.DataSource = value;
            ddlPreSeaInfrastructureType.DataBind();
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
            ddlPreSeaInfrastructureType.CssClass = value;
        }
    }
    public string SelectedInfrastructureType
    {
        get
        {

            return ddlPreSeaInfrastructureType.SelectedItem.Text;
        }
        set
        {
            ddlPreSeaInfrastructureType.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                ddlPreSeaInfrastructureType.SelectedItem.Text = value;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlPreSeaInfrastructureType.AutoPostBack = true;
        }
    }
    public void bind()
    {
        if (listpreseaInfrastructureType)
        {
            ddlPreSeaInfrastructureType.DataSource = PhoenixPreSeaInfrastructure.ListPreSeaInfrastructure(null);
            ddlPreSeaInfrastructureType.DataBind();
        }
        foreach (RadComboBoxItem item in ddlPreSeaInfrastructureType.Items)
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
    protected void ddlPreSeaInfrastructureType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    public string SelectedInfrastructureTypeId
    {
        get
        {
            return ddlPreSeaInfrastructureType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlPreSeaInfrastructureType.SelectedIndex = -1;
                ddlPreSeaInfrastructureType.ClearSelection();
                ddlPreSeaInfrastructureType.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlPreSeaInfrastructureType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlPreSeaInfrastructureType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public string DataBoundItemName
    {
        set;
        get;
    }
    protected void ddlPreSeaInfrastructureType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlPreSeaInfrastructureType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
