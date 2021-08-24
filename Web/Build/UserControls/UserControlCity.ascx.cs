using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;

public partial class UserControlCity : System.Web.UI.UserControl
{
    int? countrycode = null;
    int? stateid = null;
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

    public DataSet CityList
    {
        set
        {
            ddlCity.DataSource = value;
            ddlCity.DataBind();
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlCity.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlCity.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlCity.Enabled = value;
        }
    }

    public string Country
    {
        get
        {
            return countrycode.ToString();
        }
        set
        {
            countrycode = General.GetNullableInteger(value);
        }
    }

    public string State
    {
        get
        {
            return stateid.ToString();
        }
        set
        {
            stateid = General.GetNullableInteger(value);
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlCity_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedCity
    {
        get
        {
            return ddlCity.SelectedValue;
        }
        set
        {
            ddlCity.SelectedIndex = -1;
            ddlCity.Text = "";
            ddlCity.ClearSelection();

            if (value == string.Empty || General.GetNullableInteger(value) == null)
            {
                ddlCity.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);

            foreach (RadComboBoxItem item in ddlCity.Items)
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
        ddlCity.DataSource = PhoenixRegistersCity.ListCity(countrycode, stateid);
        ddlCity.DataBind();
        foreach (RadComboBoxItem item in ddlCity.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlCity_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCity.Items.Insert(0,new RadComboBoxItem("--Select--","Dummy"));
    }

    public Unit Width
    {
        get
        {
            return ddlCity.Width;
        }
        set
        {
            ddlCity.Width = value;
        }
    }
}