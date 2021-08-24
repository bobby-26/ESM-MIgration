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
public partial class UserControlRateContractVendorProduct : System.Web.UI.UserControl
{
    int? _vendorid;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private Guid _selectedValue;

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

    public DataSet ProductList
    {
        set
        {
            ddlVendorProduct.Items.Clear();
            ddlVendorProduct.DataSource = value;
            ddlVendorProduct.DataBind();
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
            ddlVendorProduct.CssClass = value;
        }
    }

    public string Width
    {
        get
        {
            return ddlVendorProduct.Width.ToString();
        }
        set
        {
            ddlVendorProduct.Width = Unit.Parse(value);
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlVendorProduct.AutoPostBack = true;
        }
    }


    public string SelectedProduct
    {
        get
        {

            return ddlVendorProduct.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlVendorProduct.SelectedIndex = -1;
                ddlVendorProduct.ClearSelection();
                ddlVendorProduct.Text = string.Empty;
                return;
            }
            _selectedValue = new Guid(value);
            ddlVendorProduct.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVendorProduct.Items)
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
        ddlVendorProduct.DataSource = PhoenixOwnerBudgetLubOil.OwnerBudgetVendorProductList(_vendorid);
        ddlVendorProduct.DataBind();
        foreach (RadComboBoxItem item in ddlVendorProduct.Items)
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

            return ddlVendorProduct.SelectedValue;
        }
        set
        {
            _selectedValue = new Guid(value);
            ddlVendorProduct.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVendorProduct.Items)
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

            return ddlVendorProduct.SelectedValue;
        }
        set
        {
            value = ddlVendorProduct.SelectedValue;
        }
    }
    public bool Enabled
    {
        set 
        {
            ddlVendorProduct.Enabled = value;
        }
    }
   
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlVendorProduct_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlVendorProduct_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlVendorProduct.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
