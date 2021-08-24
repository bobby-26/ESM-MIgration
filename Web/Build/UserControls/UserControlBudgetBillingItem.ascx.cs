using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlBudgetBillingItem : System.Web.UI.UserControl
{   
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet BillingItemsList
    {
        set
        {
            ddlBudgetBillingItem.Items.Clear();
            ddlBudgetBillingItem.DataSource = value;
            ddlBudgetBillingItem.DataBind();
        }
    }   

    public string CssClass
    {
        set
        {
            ddlBudgetBillingItem.CssClass = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlBudgetBillingItem.Enabled = value;
        }
        get
        {
            return ddlBudgetBillingItem.Enabled;
        }
    }

    public new bool Visible
    {
        set
        {
            ddlBudgetBillingItem.Visible = value;
        }
        get
        {
            return ddlBudgetBillingItem.Visible;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlBudgetBillingItem.AutoPostBack = true;
        }
    }

    public string Width
    {
        get
        {
            return ddlBudgetBillingItem.Width.ToString();
        }
        set
        {
            ddlBudgetBillingItem.Width = Unit.Parse(value);
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

    public string SelectedBillingItem
    {
        get
        {

            return ddlBudgetBillingItem.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlBudgetBillingItem.SelectedIndex = -1;
                ddlBudgetBillingItem.ClearSelection();
                ddlBudgetBillingItem.Text = string.Empty;
                return;
            }
            _selectedValue = value.ToString();
            ddlBudgetBillingItem.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBudgetBillingItem.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string SelectedValue
    {
        get
        {
            return ddlBudgetBillingItem.SelectedValue;
        }
        set
        {
            _selectedValue = value;
            ddlBudgetBillingItem.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBudgetBillingItem.Items)
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
        ddlBudgetBillingItem.SelectedIndex = -1;
        ddlBudgetBillingItem.DataSource = PhoenixRegistersBudgetBilling.BudgetBillingItemList();
        ddlBudgetBillingItem.DataBind();
        foreach (RadComboBoxItem item in ddlBudgetBillingItem.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlBudgetBillingItem_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlBudgetBillingItem_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBudgetBillingItem.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }   
}
