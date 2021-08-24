using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControls_UserControlSize : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet SizeList
    {
        set
        {
            ddlSize.Items.Clear();
            ddlSize.DataSource = value;
            ddlSize.DataBind();
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
            ddlSize.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlSize.AutoPostBack = true;
        }
    }



    public string SelectedSize
    {
        get
        {
            return ddlSize.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlSize.SelectedIndex = -1;
                ddlSize.ClearSelection();
                ddlSize.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlSize.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSize.Items)
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
            return ddlSize.SelectedValue;
        }
        set
        {
            _selectedValue = Int32.Parse(value);
            ddlSize.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSize.Items)
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
        ddlSize.DataSource = PhoenixRegistersSize.ListSize();
        ddlSize.DataBind();
        foreach (RadComboBoxItem item in ddlSize.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public bool Enabled
    {
        set
        {
            ddlSize.Enabled = value;
        }
    }

    protected void ddlSize_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlSize.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
