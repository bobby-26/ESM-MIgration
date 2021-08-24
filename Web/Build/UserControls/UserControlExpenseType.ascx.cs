using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.OwnerBudget;
using Telerik.Web.UI;
public partial class UserControlExpenseType : System.Web.UI.UserControl
{
    string expensetype = null;
    string category = null;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public string ExpenseType
    {
        get
        {
            return expensetype;
        }
        set
        {
            expensetype = value;
        }
    }

    public string Category
    {
        get
        {
            return category;
        }
        set
        {
            category = value;
        }
    }

    public DataSet ExpenseTypeList
    {
        set
        {
            ddlExpenseType.DataBind();
            ddlExpenseType.Items.Clear();
            ddlExpenseType.DataSource = value;
            ddlExpenseType.DataBind();
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
            ddlExpenseType.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlExpenseType.AutoPostBack = true;
        }
    }


    public string SelectedExpenseType
    {
        get
        {

            return ddlExpenseType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlExpenseType.SelectedIndex = -1;
                ddlExpenseType.ClearSelection();
                ddlExpenseType.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlExpenseType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlExpenseType.Items)
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
                ddlExpenseType.Enabled = true;
            else
                ddlExpenseType.Enabled = false;
        }
    }
    public void bind()
    {
        ddlExpenseType.DataSource = PhoenixOwnerBudget.OwnerBudgetExpenseTypeList(General.GetNullableInteger(expensetype), General.GetNullableInteger(category));
        ddlExpenseType.DataBind();
        foreach (RadComboBoxItem item in ddlExpenseType.Items)
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
            return ddlExpenseType.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlExpenseType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlExpenseType.Items)
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

    protected void ddlExpenseType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlExpenseType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlExpenseType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
