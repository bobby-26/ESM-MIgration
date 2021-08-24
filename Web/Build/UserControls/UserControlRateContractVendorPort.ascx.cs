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
public partial class UserControlRateContractVendorPort : System.Web.UI.UserControl
{
    int? _vendorid;
    int? _zoneid;
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

    public string ZoneId
    {
        get
        {
            return _zoneid.ToString();
        }
        set
        {
            _zoneid = General.GetNullableInteger(value);
        }
    }

    public DataSet PortList
    {
        set
        {
            ddlVendorPort.Items.Clear();
            ddlVendorPort.DataSource = value;
            ddlVendorPort.DataBind();
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
            ddlVendorPort.CssClass = value;
        }
    }

    public string Width
    {
        get
        {
            return ddlVendorPort.Width.ToString();
        }
        set
        {
            ddlVendorPort.Width = Unit.Parse(value);
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlVendorPort.AutoPostBack = true;
        }
    }


    public string SelectedPort
    {
        get
        {

            return ddlVendorPort.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlVendorPort.SelectedIndex = -1;
                ddlVendorPort.ClearSelection();
                ddlVendorPort.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlVendorPort.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVendorPort.Items)
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
        ddlVendorPort.DataSource = PhoenixOwnerBudgetLubOil.OwnerBudgetVendorPortList(_vendorid,_zoneid);
        ddlVendorPort.DataBind();
        foreach (RadComboBoxItem item in ddlVendorPort.Items)
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

            return ddlVendorPort.SelectedValue;
        }
        set
        {
            _selectedValue = Int32.Parse(value);
            ddlVendorPort.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVendorPort.Items)
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

            return ddlVendorPort.SelectedValue;
        }
        set
        {
            value = ddlVendorPort.SelectedValue;
        }
    }
    public bool Enabled
    {
        set 
        {
            ddlVendorPort.Enabled = value;
        }
    }
   
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlVendorPort_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlVendorPort_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlVendorPort.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
