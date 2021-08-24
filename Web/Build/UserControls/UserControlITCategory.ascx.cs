using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class UserControlITCategory : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    string categoryFilter = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            bind();
    }
    public DataSet CategoryList
    {
        set
        {
            ddlCategory.Items.Clear();
            ddlCategory.DataSource = value;
            ddlCategory.DataBind();
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
            ddlCategory.CssClass = value;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlCategory.AutoPostBack = true;
        }
    }

    public string SelectedCategory
    {
        get
        {

            return ddlCategory.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlCategory.SelectedIndex = -1;
                ddlCategory.ClearSelection();
                ddlCategory.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlCategory.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCategory.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public int SelectedValue
    {
        get
        {

            return (int.Parse(ddlCategory.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlCategory.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCategory.Items)
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
        ddlCategory.DataSource = PhoenixRegistersITCategory.ListITCategory(); 
        ddlCategory.DataBind();
        foreach (RadComboBoxItem item in ddlCategory.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public int SelectedIndex
    {
        get
        {
            return ddlCategory.SelectedIndex;
        }
        set
        {
            ddlCategory.SelectedIndex = value;
        }
    }
    protected void ddlCategory_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public string CategoryFilter
    {
        set { categoryFilter = value; }
    }
    public string Width
    {
        get
        {
            return ddlCategory.Width.ToString();
        }
        set
        {
            ddlCategory.Width = Unit.Parse(value);
        }
    }

}
