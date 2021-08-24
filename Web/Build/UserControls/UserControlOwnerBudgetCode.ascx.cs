using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Data;
using Telerik.Web.UI;
public partial class UserControlOwnerBudgetCode : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private string _selectedValue = "";
    private string ownerid = "";
    private string _vesselid = "";
    private string _budgetid = "";
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

    public string SelectedText
    {
        get
        {
            return ddlBudgetGroup.Text;
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

    public string VesselId
    {
        set
        {
            _vesselid = value;
        }
    }
    public string BudgetId
    {
        set
        {
            _budgetid = value;
        }
    }

    public void bind()
    {
        ddlBudgetGroup.DataSource = PhoenixPurchaseBudgetCode.ListOwnerBudgetGroup(null, null, General.GetNullableInteger(ownerid), General.GetNullableInteger(_vesselid), General.GetNullableInteger(_budgetid));
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

    public Unit Width
    {
        get
        {
            return ddlBudgetGroup.Width;
        }
        set
        {
            ddlBudgetGroup.Width = value;
        }
    }
}