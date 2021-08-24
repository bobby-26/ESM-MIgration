using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlPurchaseFalRules : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    Guid? _selectedValue;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataTable PurchaseFalRuleList
    {
        set
        {
            ddlFalRules.DataSource = value;
            ddlFalRules.DataBind();
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

    private void bind()
    {

        ddlFalRules.DataSource = PhoenixPurchaseFalRules.PurchaseFalRuleList();
        ddlFalRules.DataBind();
        foreach (RadComboBoxItem item in ddlFalRules.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    public string CssClass
    {
        set
        {
            ddlFalRules.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlFalRules.AutoPostBack = true;
        }
    }


    public string SelectedPurchaseFalRules
    {
        get
        {
            return ddlFalRules.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlFalRules.SelectedIndex = -1;
                ddlFalRules.ClearSelection();
                ddlFalRules.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableGuid(value);
            ddlFalRules.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlFalRules.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public Unit Width
    {
        get
        {
            return ddlFalRules.Width;
        }
        set
        {
            ddlFalRules.Width = value;
        }
    }

    protected void ddlFalRules_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlFalRules.Items.Insert(0, new RadComboBoxItem("--Select--", "-1"));
    }
}