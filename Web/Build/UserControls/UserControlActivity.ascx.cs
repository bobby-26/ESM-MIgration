using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlActivity : System.Web.UI.UserControl
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

    public DataSet ActivityList
    {
        set
        {
            ddlActivity.DataSource = value;
            ddlActivity.DataBind();
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
            ddlActivity.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlActivity.AutoPostBack = true;
        }
    }

    public string SelectedActivity
    {
        get
        {

            return ddlActivity.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlActivity.SelectedIndex = -1;
                ddlActivity.ClearSelection();
                ddlActivity.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlActivity.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlActivity.Items)
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
        ddlActivity.DataSource = PhoenixRegistersActivity.ListActivity();
        ddlActivity.DataBind();
        foreach (RadComboBoxItem item in ddlActivity.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlActivity_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlActivity.Items.Insert(0, new RadComboBoxItem("--Select--","Dummy"));
    } 
}
