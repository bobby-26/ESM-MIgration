using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class UserControlUserGroup : System.Web.UI.UserControl
{
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

    public DataSet UserGroupList
    {
        set
        {
            ddlUserGroup.DataSource = value;
            ddlUserGroup.DataBind();
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
            ddlUserGroup.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlUserGroup.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlUserGroup_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedUserGroup
    {
        get
        {
            return ddlUserGroup.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlUserGroup.SelectedIndex = -1;
                ddlUserGroup.ClearSelection();
                ddlUserGroup.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlUserGroup.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlUserGroup.Items)
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
        ddlUserGroup.DataSource = SessionUtil.UserGroupList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlUserGroup.DataBind();
        foreach (RadComboBoxItem item in ddlUserGroup.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public string SelectedIndex
    {
        get
        {
            return ddlUserGroup.SelectedIndex.ToString();
        }
        set
        {
            ddlUserGroup.SelectedIndex = int.Parse(value);
        }
    }

    protected void ddlUserGroup_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlUserGroup.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public Unit Width
    {
        set
        {
            ddlUserGroup.Width = Unit.Parse(value.ToString());
        }
        get
        {
            return ddlUserGroup.Width;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlUserGroup.Enabled = value;
        }
    }
}
