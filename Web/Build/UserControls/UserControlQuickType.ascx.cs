using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlQuickType : System.Web.UI.UserControl
{
    string _quicktypegroup = "";
    string showyesno = "";
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

    public string QuickTypeGroup
    {
        get
        {
            return _quicktypegroup;
        }
        set
        {
            _quicktypegroup = value;
        }
    }
    public string QuickTypeShowYesNo
    {
        get
        {
            return showyesno;
        }
        set
        {
            showyesno = value;
        }
    }
    public DataSet QuickTypeList
    {
        set
        {
            ddlQuickType.DataBind();
            ddlQuickType.Items.Clear();
            ddlQuickType.DataSource = value;
            ddlQuickType.DataBind();
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
            ddlQuickType.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlQuickType.AutoPostBack = true;
        }
    }

    public string Width
    {
        get
        {
            return ddlQuickType.Width.ToString();
        }
        set
        {
            ddlQuickType.Width = Unit.Parse(value);
        }
    }


    public string SelectedQuickType
    {
        get
        {

            return ddlQuickType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlQuickType.SelectedIndex = -1;
                ddlQuickType.ClearSelection();
                ddlQuickType.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlQuickType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlQuickType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public string Enabled
    {
        set
        {

            if (value.ToUpper().Equals("TRUE"))
                ddlQuickType.Enabled = true;
            else
                ddlQuickType.Enabled = false;
        }
    }
    public void bind()
    {
        ddlQuickType.DataSource = PhoenixRegistersQuick.ListQuickType(1, _quicktypegroup,General.GetNullableInteger(showyesno));
        ddlQuickType.DataBind();
        foreach (RadComboBoxItem item in ddlQuickType.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }


    public int SelectedValue
    {
        get
        {

            return (int.Parse(ddlQuickType.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlQuickType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlQuickType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlQuickType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlQuickType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlQuickType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
