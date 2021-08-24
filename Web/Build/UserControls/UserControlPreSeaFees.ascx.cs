using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;

public partial class UserControlPreSeaFees : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool listpreseafees = true;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public bool Listpreseafees
    {
        get
        {
            return listpreseafees;
        }
        set
        {
            listpreseafees = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlPreSeaFees.Enabled = value;
        }
    }
    public DataSet PreSeaFeesList
    {
        set
        {
            ddlPreSeaFees.DataSource = value;
            ddlPreSeaFees.DataBind();
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
            ddlPreSeaFees.CssClass = value;
        }
    }
    public string SelectedFeesName
    {
        get
        {

            return ddlPreSeaFees.SelectedItem.Text;
        }
        set
        {
            ddlPreSeaFees.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                ddlPreSeaFees.SelectedItem.Text = value;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlPreSeaFees.AutoPostBack = true;
        }
    }
    public void bind()
    {
        if (listpreseafees)
        {
            ddlPreSeaFees.DataSource = PhoenixPreSeaFees.ListPreSeaFees(null);
            ddlPreSeaFees.DataBind();
        }
        foreach (RadComboBoxItem item in ddlPreSeaFees.Items)
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
    protected void ddlPreSeaFees_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    public string SelectedFees
    {
        get
        {
            return ddlPreSeaFees.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlPreSeaFees.SelectedIndex = -1;
                ddlPreSeaFees.ClearSelection();
                ddlPreSeaFees.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlPreSeaFees.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlPreSeaFees.Items)
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
    protected void ddlPreSeaFees_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlPreSeaFees.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
