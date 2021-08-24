using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlOilType : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = "";
    private int? oilyn;
    private int? fueloilyn;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet OilTypeList
    {
        set
        {
            ddlOilType.DataBind();
            ddlOilType.Items.Clear();
            ddlOilType.DataSource = value;
            ddlOilType.DataBind();
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
            ddlOilType.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlOilType.AutoPostBack = true;
        }
    }

    public string SelectedOilType
    {
        get
        {
            return ddlOilType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlOilType.SelectedIndex = -1;
                ddlOilType.ClearSelection();
                ddlOilType.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlOilType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlOilType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string Enabled
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlOilType.Enabled = true;
            else
                ddlOilType.Enabled = false;
        }
    }

    public void bind()
    {
        ddlOilType.DataSource = PhoenixRegistersOilType.ListOilType(oilyn, fueloilyn);
        ddlOilType.DataBind();
        foreach (RadComboBoxItem item in ddlOilType.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    public int? IsOil
    {
        set
        {
            oilyn = value;
        }
        get
        {
            return oilyn;
        }
    }

    public int? IsFuelOil
    {
        set
        {
            fueloilyn = value;
        }
        get
        {
            return fueloilyn;
        }
    }

    public string SelectedValue
    {
        get
        {
            return ddlOilType.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlOilType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlOilType.Items)
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

    protected void ddlOilType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlOilType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlOilType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
