using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlCountry : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    string type;
    string activeyn;
    private int? isEucountry = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
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
    public DataSet CountryList
    {
        set
        {
            ddlCountry.DataSource = value;
            ddlCountry.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ddlCountry.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlCountry.AutoPostBack = true;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlCountry.Enabled = value;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlCountry_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedCountry
    {
        get
        {
            return ddlCountry.SelectedValue;
        }
        set
        {
            ddlCountry.SelectedIndex = -1;
            ddlCountry.Text = "";
            ddlCountry.ClearSelection();

            if (value == string.Empty || General.GetNullableInteger(value) == null)
            {
                ddlCountry.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);

            foreach (RadComboBoxItem item in ddlCountry.Items)
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
        if (isEucountry != null && isEucountry == 1)
            ddlCountry.DataSource = PhoenixRegistersCountry.ListEuCountry(General.GetNullableInteger(activeyn), General.GetNullableInteger(type)); //Active Country
        else
            ddlCountry.DataSource = PhoenixRegistersCountry.ListCountry(General.GetNullableInteger(activeyn), General.GetNullableInteger(type)); //Active Country
        ddlCountry.DataBind();

        foreach (RadComboBoxItem item in ddlCountry.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlCountry_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCountry.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public string Type
    {
        get
        {
            return Type.ToString();
        }
        set
        {
            type = value;
        }
    }

    public string ActiveYN
    {
        get
        {
            return ActiveYN.ToString();
        }
        set
        {
            activeyn = value;
        }
    }
    public Unit Width
    {
        get
        {
            return ddlCountry.Width;
        }
        set
        {
            ddlCountry.Width = value;
        }
    }
    public int? EuCountyrOnly
    {
        get
        {
            return EuCountyrOnly;
        }
        set
        {
            isEucountry = value;
        }
    }

}