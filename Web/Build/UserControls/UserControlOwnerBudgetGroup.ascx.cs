using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class UserControlOwnerBudgetGroup : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private string _selectedValue = "";
    private string ownerid = "";

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
            ddlBudgetGroup.DataSource = value;
            ddlBudgetGroup.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ddlBudgetGroup.CssClass = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlBudgetGroup.Enabled = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlBudgetGroup.AutoPostBack = value;
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
            return ddlBudgetGroup.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlBudgetGroup.SelectedIndex = -1;
                ddlBudgetGroup.ClearSelection();
                ddlBudgetGroup.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlBudgetGroup.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBudgetGroup.Items)
            {
                if (item.Value == _selectedValue)
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
            return ddlBudgetGroup.SelectedValue;
        }
        set
        {
            _selectedValue = value;
            ddlBudgetGroup.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBudgetGroup.Items)
            {
                if (item.Value == _selectedValue)
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string OwnerId
    {
        set
        {
            ownerid = value;
        }
    }

    private void bind()
    {
        ddlBudgetGroup.DataSource = PhoenixRegistersBudget.ListOwnerBudgetGroup(General.GetNullableInteger(ownerid));
        ddlBudgetGroup.DataBind();
        foreach (RadComboBoxItem item in ddlBudgetGroup.Items)
        {
            if (item.Value == _selectedValue)
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlBudgetGroup_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBudgetGroup.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
