using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class UserControlCrewCostEvaluationSectionType : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private string _selectedValue = "";
    private int _selectedSectionType = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public DataSet SelectionTypeList
    {
        set
        {
            ddlSectionType.Items.Clear();
            ddlSectionType.DataSource = value;
            ddlSectionType.DataBind();
            foreach (RadComboBoxItem item in ddlSectionType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public int SectionType
    {
        set
        {
            _selectedSectionType = value;
        }
    }
    public string CssClass
    {
        set
        {
            ddlSectionType.CssClass = value;
        }
        get { return ddlSectionType.CssClass; }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlSectionType.AutoPostBack = true;
        }
    }
    public string SelectedSectionTypeName
    {
        get
        {
            return ddlSectionType.SelectedItem.Text;
        }
    }
    public string SelectedSectionTypeID
    {
        get
        {
            return ddlSectionType.SelectedValue;
        }
        set
        {
            if (value == string.Empty || value == "Dummy")
            {
                ddlSectionType.SelectedIndex = -1;
                ddlSectionType.ClearSelection();
                ddlSectionType.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlSectionType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSectionType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

        }
    }
    public string SelectedValue
    {
        get
        {
            return ddlSectionType.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlSectionType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSectionType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public void bind()
    {
        ddlSectionType.DataSource = PhoenixRegistersCrewCostEvaluationSectionType.ListCrewCostEvaluationSectionType(null);
        ddlSectionType.DataBind();

        foreach (RadComboBoxItem item in ddlSectionType.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlSectionType_DataBound(object sender, EventArgs e)
    {
        ddlSectionType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void ddlSectionType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
}
