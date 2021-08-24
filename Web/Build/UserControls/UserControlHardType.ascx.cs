using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlHardType : System.Web.UI.UserControl
{
    string hardtypegroup = "";
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

    public string HardTypeGroup
    {
        get
        {
            return hardtypegroup;
        }
        set
        {
            hardtypegroup = value;
        }
    }

    public DataSet HardTypeList
    {
        set
        {
            ddlHardType.DataBind();
            ddlHardType.Items.Clear();
            ddlHardType.DataSource = value;
            ddlHardType.DataBind();
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

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlHardType.AutoPostBack = true;
        }
    }

    public string SelectedHardType
    {
        get
        {

            return ddlHardType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlHardType.SelectedIndex = -1;
                ddlHardType.ClearSelection();
                ddlHardType.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlHardType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlHardType.Items)
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
        ddlHardType.SelectedIndex = -1;
        ddlHardType.DataSource = PhoenixRegistersHard.ListHardType(1);
        ddlHardType.DataBind();
        foreach (RadComboBoxItem item in ddlHardType.Items)
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

            return (int.Parse(ddlHardType.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlHardType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlHardType.Items)
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

    protected void ddlHardType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlHardType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlHardType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
