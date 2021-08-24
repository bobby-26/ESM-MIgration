using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlBankAccount : System.Web.UI.UserControl
{
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


    public DataSet BankAccountList
    {
        set
        {
            ddlBankAccount.DataSource = value;
            ddlBankAccount.DataBind();
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
            ddlBankAccount.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlBankAccount.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlBankAccount_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedBankAccount
    {
        get
        {
            return ddlBankAccount.SelectedValue;
        }
        set
        {
            ddlBankAccount.SelectedIndex = -1;
            ddlBankAccount.ClearSelection();
            ddlBankAccount.Text = string.Empty;
            if (value == String.Empty || value == null)
            {
                ddlBankAccount.SelectedIndex = -1;
                ddlBankAccount.ClearSelection();
                ddlBankAccount.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            foreach (RadComboBoxItem item in ddlBankAccount.Items)
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
        ddlBankAccount.DataSource = PhoenixRegistersAccount.ListBankAccount(null, null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID,0);
        ddlBankAccount.DataBind();
        foreach (RadComboBoxItem item in ddlBankAccount.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }

    }
    protected void ddlBankAccount_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBankAccount.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }


    public bool Enabled
    {
        set
        {
            ddlBankAccount.Enabled = value;
        }
    }
}
