using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlInspectionNonConformanceCategory : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    int? nccategoryid = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet NonConformanceCategoryList
    {
        set
        {
            ddlNonConformanceCategory.Items.Clear();
            ddlNonConformanceCategory.DataSource = value;
            ddlNonConformanceCategory.DataBind();
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
            ddlNonConformanceCategory.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlNonConformanceCategory.AutoPostBack = true;
        }
    }

    public string SelectedNonConformanceCategory
    {
        get
        {

            return ddlNonConformanceCategory.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlNonConformanceCategory.SelectedIndex = -1;
                ddlNonConformanceCategory.ClearSelection();
                ddlNonConformanceCategory.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlNonConformanceCategory.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlNonConformanceCategory.Items)
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
            return (int.Parse(ddlNonConformanceCategory.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlNonConformanceCategory.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlNonConformanceCategory.Items)
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
        ddlNonConformanceCategory.DataSource = PhoenixRegistersInspectionNonConformanceCategory.ListNonConformanceCategory(nccategoryid);
        ddlNonConformanceCategory.DataBind();
        foreach (RadComboBoxItem item in ddlNonConformanceCategory.Items)
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

    protected void ddlNonConformanceCategory_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlNonConformanceCategory_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlNonConformanceCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
