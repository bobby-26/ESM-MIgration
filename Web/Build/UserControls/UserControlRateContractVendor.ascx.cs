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
public partial class UserControlRateContractVendor : System.Web.UI.UserControl
{
    //int _quicktypecode;
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

    
    public DataSet VendorList
    {
        set
        {
            ddlVendor.Items.Clear();
            ddlVendor.DataSource = value;
            ddlVendor.DataBind();
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
            ddlVendor.CssClass = value;
        }
    }

    public string Width
    {
        get
        {
            return ddlVendor.Width.ToString();
        }
        set
        {
            ddlVendor.Width = Unit.Parse(value);
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlVendor.AutoPostBack = true;
        }
    }


    public string SelectedVendor
    {
        get
        {

            return ddlVendor.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlVendor.SelectedIndex = -1;
                ddlVendor.ClearSelection();
                ddlVendor.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlVendor.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVendor.Items)
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
        ddlVendor.DataSource = PhoenixOwnerBudgetLubOil.OwnerBudgetVendorList();
        ddlVendor.DataBind();
        foreach (RadComboBoxItem item in ddlVendor.Items)
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

            return ddlVendor.SelectedValue;
        }
        set
        {
            _selectedValue = Int32.Parse(value);
            ddlVendor.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVendor.Items)
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

            return ddlVendor.SelectedValue;
        }
        set
        {
            value = ddlVendor.SelectedValue;
        }
    }
    public bool Enabled
    {
        set 
        {
            ddlVendor.Enabled = value;
        }
    }
   
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlVendor_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlVendor_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlVendor.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
