using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlInspectionNonConformanceSubCategory : System.Web.UI.UserControl
{
    int? ncCategoryid = null;
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

    public string NCCategoryid
    {
        get
        {
            return ncCategoryid.ToString();
        }
        set
        {
            ncCategoryid = Int32.Parse(value);
        }
    }

    public DataSet SubCategoryList
    {
        set
        {
            ddlNonConformanceSubCategory.Items.Clear();
            ddlNonConformanceSubCategory.DataSource = value;
            ddlNonConformanceSubCategory.DataBind();
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
            ddlNonConformanceSubCategory.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlNonConformanceSubCategory.AutoPostBack = true;
        }
    }


    public string SelectedSubCategory
    {
        get
        {

            return ddlNonConformanceSubCategory.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlNonConformanceSubCategory.SelectedIndex = -1;
                ddlNonConformanceSubCategory.ClearSelection();
                ddlNonConformanceSubCategory.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlNonConformanceSubCategory.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlNonConformanceSubCategory.Items)
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
            return (int.Parse(ddlNonConformanceSubCategory.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlNonConformanceSubCategory.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlNonConformanceSubCategory.Items)
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
        ddlNonConformanceSubCategory.DataSource = PhoenixRegistersInspectionNonConformanceSubCategory.ListNonConformanceSubCategory(ncCategoryid);
        ddlNonConformanceSubCategory.DataBind();
        foreach (RadComboBoxItem item in ddlNonConformanceSubCategory.Items)
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

    protected void ddlNonConformanceSubCategory_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlNonConformanceSubCategory_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlNonConformanceSubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
