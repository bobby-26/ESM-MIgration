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

public partial class UserControlUnit : System.Web.UI.UserControl
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

    public DataSet UnitList
    {
        set
        {
            ddlUnit.Items.Clear();
            ddlUnit.DataSource = value;
            ddlUnit.DataBind();
        }
    }
    public Unit Width
    {
        set
        {
            ddlUnit.Width = Unit.Parse(value.ToString());
        }
        get
        {
            return ddlUnit.Width;
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
            ddlUnit.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlUnit.AutoPostBack = true;
        }
    }



    public string SelectedUnit
    {
        get
        {
            return ddlUnit.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlUnit.SelectedIndex = -1;
                ddlUnit.ClearSelection();
                ddlUnit.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlUnit.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlUnit.Items)
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
            return ddlUnit.SelectedValue;
        }
        set
        {
            _selectedValue =Int32.Parse(value);
            ddlUnit.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlUnit.Items)
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
        ddlUnit.DataSource = PhoenixRegistersUnit.ListUnit();
        ddlUnit.DataBind();
        foreach (RadComboBoxItem item in ddlUnit.Items)
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
            ddlUnit.Enabled = value;
        }
    }

    protected void ddlUnit_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlUnit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}


