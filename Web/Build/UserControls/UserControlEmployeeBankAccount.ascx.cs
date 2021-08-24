using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections;
using Telerik.Web.UI;
public partial class UserControlEmployeeBankAccount : System.Web.UI.UserControl
{
    public delegate void TextChangedDelegate(object o, EventArgs e);
    public event TextChangedDelegate TextChangedEvent;

    private bool _appenddatabounditems;
    private string _selectedValue = "-1";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataTable BankAccountList
    {
        set
        {
            ddlBankAccount.DataSource = value;
            ddlBankAccount.DataBind();
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }
    public string EmployeeId
    {
        get;
        set;
    }
    public string CssClass
    {
        set
        {
            ddlBankAccount.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlBankAccount.AutoPostBack = value;
        }
    }

    protected void OnUserControlEmployeeBankAccountChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlBankAccount_TextChanged(object sender, EventArgs e)
    {
        OnUserControlEmployeeBankAccountChangedEvent(e);
    }

    public string SelectedBankAccount
    {
        get
        {
            return ddlBankAccount.SelectedValue;
        }
        set
        {
            if (value == String.Empty || value == null)
            {
                ddlBankAccount.SelectedIndex = -1;
                ddlBankAccount.ClearSelection();
                ddlBankAccount.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlBankAccount.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBankAccount.Items)
            {
                if (item.Value == _selectedValue)
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public Unit Width
    {
        set
        {
            ddlBankAccount.Width = Unit.Parse(value.ToString());
        }
        get
        {
            return ddlBankAccount.Width;
        }
    }
    private void bind()
    {
        ddlBankAccount.DataSource = PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(EmployeeId));
        ddlBankAccount.DataBind();
        foreach (RadComboBoxItem item in ddlBankAccount.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                ddlBankAccount.SelectedIndex = -1;
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlBankAccount_DataBound(object sender, EventArgs e)
    {
        RadComboBox ddl = ((RadComboBox)sender);
        if (ddl.DataSource != null)
        {
            DataTable dt = (DataTable)ddl.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (ddl.Items[i].Value == dt.Rows[i]["FLDBANKACCOUNTID"].ToString()
                    && dt.Rows[i]["FLDISDEFAULT"].ToString() == "1")
                {
                    ddl.Items[i].Selected = true;
                    break;
                }
            }
        }
        if (_appenddatabounditems)
            ddlBankAccount.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    public bool Enabled
    {
        set
        {
            ddlBankAccount.Enabled = value;
        }
    }
}
