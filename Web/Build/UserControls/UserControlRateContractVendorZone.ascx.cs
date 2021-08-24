using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.OwnerBudget;
using Telerik.Web.UI;
public partial class UserControlRateContractVendorZone : System.Web.UI.UserControl
{
    int? _vendorid;
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

    public string VendorId
    {
        get
        {
            return _vendorid.ToString();
        }
        set
        {
            _vendorid = General.GetNullableInteger(value);
        }
    }

    public DataSet ZoneList
    {
        set
        {
            ddlVendorZone.Items.Clear();
            ddlVendorZone.DataSource = value;
            ddlVendorZone.DataBind();
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
            ddlVendorZone.CssClass = value;
        }
    }

    public string Width
    {
        get
        {
            return ddlVendorZone.Width.ToString();
        }
        set
        {
            ddlVendorZone.Width = Unit.Parse(value);
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlVendorZone.AutoPostBack = true;
        }
    }


    public string SelectedZone
    {
        get
        {

            return ddlVendorZone.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlVendorZone.SelectedIndex = -1;
                ddlVendorZone.ClearSelection();
                ddlVendorZone.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlVendorZone.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVendorZone.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public void bind()
    {
        ddlVendorZone.DataSource = PhoenixOwnerBudgetLubOil.OwnerBudgetVendorZoneList(_vendorid);
        ddlVendorZone.DataBind();
        foreach (RadComboBoxItem item in ddlVendorZone.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }


    public string SelectedValue
    {
        get
        {

            return ddlVendorZone.SelectedValue;
        }
        set
        {
            _selectedValue = Int32.Parse(value);
            ddlVendorZone.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVendorZone.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }


    public string SelectedText
    {
        get
        {

            return ddlVendorZone.SelectedValue;
        }
        set
        {
            value = ddlVendorZone.SelectedValue;
        }
    }
    public bool Enabled
    {
        set 
        {
            ddlVendorZone.Enabled = value;
        }
    }
   
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlVendorZone_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlVendorZone_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlVendorZone.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
