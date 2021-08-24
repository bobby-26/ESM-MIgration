using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class UserControlState : System.Web.UI.UserControl
{
    int? countrycode;
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

    public DataSet StateList
    {
        set
        {
            ddlState.DataSource = value;
            ddlState.DataBind();
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
        set { ddlState.Enabled = value; }
    }
    public string CssClass
    {
        set
        {
            ddlState.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlState.AutoPostBack = value;
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

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlState_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedState
    {
        get
        {
            return ddlState.SelectedValue;
        }
        set
        {
            ddlState.SelectedIndex = -1;
            ddlState.Text = "";
            ddlState.ClearSelection();
            if (value == string.Empty || General.GetNullableInteger(value) == null)
            {

                ddlState.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);

            foreach (RadComboBoxItem item in ddlState.Items)
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
        ddlState.DataSource = PhoenixRegistersState.ListState(1, General.GetNullableInteger(Country));
        ddlState.DataBind();

        foreach (RadComboBoxItem item in ddlState.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlState_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlState.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public Unit Width
    {
        get
        {
            return ddlState.Width;
        }
        set
        {
            ddlState.Width = value;
        }
    }
}