using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlPurchaseRules : System.Web.UI.UserControl
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

    public DataTable PurchaseRulesList
    {
        set
        {
            ddlPurchaseRules.DataSource = value;
            ddlPurchaseRules.DataBind();
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

        ddlPurchaseRules.DataSource = PhoenixPurchaseRules.PurchaseRulesList();
        ddlPurchaseRules.DataBind();
        foreach (RadComboBoxItem item in ddlPurchaseRules.Items)
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
            ddlPurchaseRules.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlPurchaseRules.AutoPostBack = true;
        }
    }


    public string SelectedPurchaseRules
    {
        get
        {
            return ddlPurchaseRules.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlPurchaseRules.SelectedIndex = -1;
                ddlPurchaseRules.ClearSelection();
                ddlPurchaseRules.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableGuid(value);
            ddlPurchaseRules.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlPurchaseRules.Items)
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
            return ddlPurchaseRules.Width;
        }
        set
        {
            ddlPurchaseRules.Width = value;
        }
    }

    protected void ddlPurchaseRules_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        if (_appenddatabounditems)
            ddlPurchaseRules.Items.Insert(0, new RadComboBoxItem("--Select--", "-1"));
    }
}