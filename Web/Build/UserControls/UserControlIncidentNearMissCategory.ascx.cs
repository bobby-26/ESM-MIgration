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

public partial class UserControlIncidentNearMissCategory : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = string.Empty;
    string typeid = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public string TypeId
    {
        get
        {
            return typeid.ToString();
        }
        set
        {
            typeid = value.ToString();
        }
    }

    public string Width
    {
        get
        {
            return ddlIncidentNearMissCategory.Width.ToString();
        }
        set
        {
            ddlIncidentNearMissCategory.Width = Unit.Parse(value);
        }
    }

    public DataSet CategoryList
    {
        set
        {
            ddlIncidentNearMissCategory.Items.Clear();
            ddlIncidentNearMissCategory.DataSource = value;
            ddlIncidentNearMissCategory.DataBind();
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
            ddlIncidentNearMissCategory.CssClass = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlIncidentNearMissCategory.Enabled = value;
        }
        get
        {
            return ddlIncidentNearMissCategory.Enabled;
        }
    }

    public new bool Visible
    {
        set
        {
            ddlIncidentNearMissCategory.Visible = value;
        }
        get
        {
            return ddlIncidentNearMissCategory.Visible;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlIncidentNearMissCategory.AutoPostBack = true;
        }
    }


    public string SelectedCategory
    {
        get
        {
            return ddlIncidentNearMissCategory.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlIncidentNearMissCategory.SelectedIndex = -1;
                ddlIncidentNearMissCategory.ClearSelection();
                ddlIncidentNearMissCategory.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddlIncidentNearMissCategory.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlIncidentNearMissCategory.Items)
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
            return ddlIncidentNearMissCategory.SelectedValue;
        }
        set
        {
            _selectedValue = value.ToString();
            ddlIncidentNearMissCategory.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlIncidentNearMissCategory.Items)
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
        ddlIncidentNearMissCategory.DataSource = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(General.GetNullableInteger(typeid));
        ddlIncidentNearMissCategory.DataBind();
        foreach (RadComboBoxItem item in ddlIncidentNearMissCategory.Items)
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

    protected void ddlIncidentNearMissCategory_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlIncidentNearMissCategory_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlIncidentNearMissCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
