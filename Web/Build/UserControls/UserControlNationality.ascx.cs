using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlNationality : System.Web.UI.UserControl
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

    public DataSet NationalityList
    {
        set
        {
            ddlNationality.DataSource = value;
            ddlNationality.DataBind();
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
            ddlNationality.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlNationality.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlNationality_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedNationality
    {
        get
        {
            return ddlNationality.SelectedValue;
        }
        set
        {
            ddlNationality.Text = "";
            if (value == string.Empty)
            {
                ddlNationality.SelectedIndex = -1;
              
                ddlNationality.ClearSelection();
                ddlNationality.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlNationality.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlNationality.Items)
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
        ddlNationality.DataSource = PhoenixRegistersCountry.ListNationality();
        ddlNationality.DataBind();
        foreach (RadComboBoxItem item in ddlNationality.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public bool Readonly
    {
        get
        {
            return ddlNationality.Enabled;
        }
        set
        {
            ddlNationality.Enabled = value;
        }
    }

    public bool Enabled
    {
        get
        {
            return ddlNationality.Enabled;
        }
        set
        {
            ddlNationality.Enabled = value;
        }
    }

    protected void ddlNationality_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlNationality.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public string Width
    {
        get
        {
            return ddlNationality.Width.ToString();
        }
        set
        {
            ddlNationality.Width = Unit.Parse(value);
        }
    }
}
