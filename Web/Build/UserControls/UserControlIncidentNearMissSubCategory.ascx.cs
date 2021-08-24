using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class UserControlIncidentNearMissSubCategory : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = string.Empty;
    string categoryid = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public string CategoryId
    {
        get
        {
            return categoryid.ToString();
        }
        set
        {
            categoryid = value.ToString();
        }
    }

    public string Width
    {
        get
        {
            return ddlIncidentNearMissSubCategory.Width.ToString();
        }
        set
        {
            ddlIncidentNearMissSubCategory.Width = Unit.Parse(value);
        }
    }

    public DataSet SubCategoryList
    {
        set
        {
            ddlIncidentNearMissSubCategory.Items.Clear();
            ddlIncidentNearMissSubCategory.DataSource = value;
            ddlIncidentNearMissSubCategory.DataBind();
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
            ddlIncidentNearMissSubCategory.CssClass = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlIncidentNearMissSubCategory.Enabled = value;
        }
        get
        {
            return ddlIncidentNearMissSubCategory.Enabled;
        }
    }

    public new bool Visible
    {
        set
        {
            ddlIncidentNearMissSubCategory.Visible = value;
        }
        get
        {
            return ddlIncidentNearMissSubCategory.Visible;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlIncidentNearMissSubCategory.AutoPostBack = true;
        }
    }


    public string SelectedSubCategory
    {
        get
        {
            return ddlIncidentNearMissSubCategory.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlIncidentNearMissSubCategory.SelectedIndex = -1;
                ddlIncidentNearMissSubCategory.ClearSelection();
                ddlIncidentNearMissSubCategory.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddlIncidentNearMissSubCategory.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlIncidentNearMissSubCategory.Items)
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
            return ddlIncidentNearMissSubCategory.SelectedValue;
        }
        set
        {
            _selectedValue = value.ToString();
            ddlIncidentNearMissSubCategory.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlIncidentNearMissSubCategory.Items)
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
        ddlIncidentNearMissSubCategory.DataSource = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(categoryid));
        ddlIncidentNearMissSubCategory.DataBind();
        foreach (RadComboBoxItem item in ddlIncidentNearMissSubCategory.Items)
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

    protected void ddlIncidentNearMissSubCategory_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlIncidentNearMissSubCategory_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlIncidentNearMissSubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
