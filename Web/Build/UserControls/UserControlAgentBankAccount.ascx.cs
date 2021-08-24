using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections;
using Telerik.Web.UI;

public partial class UserControlAgentBankAccount : System.Web.UI.UserControl
{
    public delegate void TextChangedDelegate(object o, EventArgs e);
    public event TextChangedDelegate TextChangedEvent;

    int? addresscode = null;
    private bool _appenddatabounditems;
    private string _selectedValue = "-1";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataTable AgentBankAccountList
    {
        set
        {
            ddlAgentBankAccount.DataSource = value;
            ddlAgentBankAccount.DataBind();
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }
    public string AddressCode
    {
        get
        {
            return addresscode.ToString();

        }
        set
        {
            addresscode = General.GetNullableInteger(value);

        }
    }
    public string CssClass
    {
        set
        {
            ddlAgentBankAccount.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlAgentBankAccount.AutoPostBack = value;
        }
    }

    protected void OnUserControlAgentBankAccountChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlAgentBankAccount_TextChanged(object sender, EventArgs e)
    {
        OnUserControlAgentBankAccountChangedEvent(e);
    }

    public string SelectedBankAccount
    {
        get
        {
            return ddlAgentBankAccount.SelectedValue;
        }
        set
        {
            if (value == String.Empty || value == null)
            {
                ddlAgentBankAccount.SelectedIndex = -1;
                ddlAgentBankAccount.ClearSelection();
                ddlAgentBankAccount.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlAgentBankAccount.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlAgentBankAccount.Items)
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
            ddlAgentBankAccount.Width = value;
        }
        get
        {
            return ddlAgentBankAccount.Width;
        }
    }
    private void bind()
    {
        ddlAgentBankAccount.DataSource = PhoenixVesselAccountsEmployeeBankAccount.ListAgentBankAccount(int.Parse(AddressCode));
        ddlAgentBankAccount.DataBind();
        foreach (RadComboBoxItem item in ddlAgentBankAccount.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                ddlAgentBankAccount.SelectedIndex = -1;
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlAgentBankAccount_DataBound(object sender, EventArgs e)
    {
        RadComboBox ddl = ((RadComboBox)sender);
        if (ddl.DataSource != null)
        {
            DataTable dt = (DataTable)ddl.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (ddl.Items[i].Value == dt.Rows[i]["FLDBANKID"].ToString())
                {
                    ddl.Items[i].Selected = true;
                    break;
                }
            }
        }
        if (_appenddatabounditems)
            ddlAgentBankAccount.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    public bool Enabled
    {
        set
        {
            ddlAgentBankAccount.Enabled = value;
        }
    }
    protected void ddlAgentBankAccount_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        DataRowView dr = (DataRowView)e.Item.DataItem;
        e.Item.Text = e.Item.Text + " " + dr["FLDBANKNAME"];
    }
}