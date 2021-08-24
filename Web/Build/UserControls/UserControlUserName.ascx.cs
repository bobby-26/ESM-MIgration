using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlUserName : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private int? _departmentid = null;
    private int? _activeyn = null;
    private int? _departmenttype = null;
    public event EventHandler TextChangedEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet UserNameList
    {
        set
        {
            ddlUserName.DataSource = value;
            ddlUserName.DataBind();
        }
    }

    public int DepartmentId
    {
        set
        {
            _departmentid = value;
        }
    }

    public int? DepartmentType
    {
        get
        {
            return _departmenttype;
        }
        set
        {
            _departmenttype = value;
        }
    }
    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;

        }
    }

    public bool Enabled
    {
        set
        {
            ddlUserName.Enabled = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlUserName.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlUserName.AutoPostBack = value;
        }
    }
    public int ActiveYN
    {
        set
        {
            _activeyn = value;
        }
    }
    public string SelectedUser
    {
        get
        {

            return ddlUserName.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlUserName.SelectedIndex = -1;
                ddlUserName.ClearSelection();
                ddlUserName.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlUserName.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlUserName.Items)
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
        ddlUserName.DataSource = PhoenixUser.UserList(_departmentid, _activeyn, _departmenttype);
        ddlUserName.DataBind();
        foreach (RadComboBoxItem item in ddlUserName.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    public string SelectedUserName
    {
        get
        {
            return ddlUserName.SelectedItem.Text;
        }
    }

    protected void ddlUserName_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlUserName.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public string Width
    {
        set
        {
            ddlUserName.Width = Unit.Parse(value);
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void ddlUserName_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
}
