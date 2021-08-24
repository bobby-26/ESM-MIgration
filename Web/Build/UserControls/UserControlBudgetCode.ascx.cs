using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlBudgetCode : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private string _selectedSubAccount;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet BudgetCodeList
    {
        set
        {
            ddlBudgetCode.DataSource = value;
            ddlBudgetCode.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ddlBudgetCode.CssClass = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlBudgetCode.Enabled = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlBudgetCode.AutoPostBack = value;
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public string SelectedBudgetCode
    {
        get
        {
            return ddlBudgetCode.SelectedValue;
        }
        set
        {
            ddlBudgetCode.SelectedIndex = -1;

            if (value == string.Empty || General.GetNullableInteger(value) == null)
            {
                ddlBudgetCode.SelectedIndex = -1;
                ddlBudgetCode.ClearSelection();
                ddlBudgetCode.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            foreach (RadComboBoxItem item in ddlBudgetCode.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string SelectedBudgetSubAccount
    {
        get
        {
            return ddlBudgetCode.SelectedItem.Text;
        }
        set
        {
            ddlBudgetCode.SelectedIndex = -1;

            if (value == string.Empty || General.GetNullableInteger(value) == null)
            {
                ddlBudgetCode.SelectedIndex = -1;
                ddlBudgetCode.ClearSelection();
                ddlBudgetCode.Text = string.Empty;
                return;
            }

            _selectedSubAccount = value;
            foreach (RadComboBoxItem item in ddlBudgetCode.Items)
            {
                if (item.Text == _selectedSubAccount)
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
            return (int.Parse(ddlBudgetCode.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlBudgetCode.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBudgetCode.Items)
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
        ddlBudgetCode.DataSource = PhoenixRegistersBudget.ListBudget();
        ddlBudgetCode.DataBind();

        foreach (RadComboBoxItem item in ddlBudgetCode.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlBudgetCode_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBudgetCode.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlBudgetCode_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public Unit Width
    {
        get
        {
            return ddlBudgetCode.Width;
        }
        set
        {
            ddlBudgetCode.Width = value;
        }
    }
}
