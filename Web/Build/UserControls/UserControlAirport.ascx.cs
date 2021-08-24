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
public partial class UserControlAirport : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            bind();
    }

    public DataSet AirportList
    {
        set
        {
            ddlAirport.Items.Clear();
            ddlAirport.DataSource = value;
            ddlAirport.DataBind();
        }
    }
    public string Country
    {
        get;
        set;
    }
    public string CssClass
    {
        set
        {
            ddlAirport.CssClass = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlAirport.Enabled = value;
        }
    }
    public Unit Width
    {
        get
        {
            return ddlAirport.Width;
        }
        set
        {
            ddlAirport.Width = value;
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
    public string SelectedAirport
    {
        get
        {
            return ddlAirport.SelectedValue;
        }
        set
        {
            ddlAirport.SelectedIndex = -1;
            if (value == string.Empty || General.GetNullableInteger(value) == null)
            {
                ddlAirport.SelectedIndex = -1;
                ddlAirport.ClearSelection();
                ddlAirport.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            foreach (RadComboBoxItem item in ddlAirport.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public int SelectedValue
    {
        get
        {
            return (int.Parse(ddlAirport.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlAirport.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlAirport.Items)
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
        ddlAirport.DataSource = PhoenixRegistersAirport.ListAirportByCountry(General.GetNullableInteger(Country), null);
        ddlAirport.DataBind();
        foreach (RadComboBoxItem item in ddlAirport.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlAirport_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlAirport.Items.Insert(0,new RadComboBoxItem("--Select--","Dummy"));
    }
}