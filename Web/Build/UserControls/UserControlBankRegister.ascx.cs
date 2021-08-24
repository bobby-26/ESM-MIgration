using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlBankRegister : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    public event EventHandler TextChangedEvent;
    private int _selectedValue = -1;
    private string _selectedText = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet BankList
    {
        set
        {
            ddlBank.Items.Clear();
            ddlBank.DataSource = value;
            ddlBank.DataBind();
        }
    }
    public bool Enabled
    {
        set
        {
            ddlBank.Enabled = value;
        }
    }
    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlBank.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlBank.AutoPostBack = value;
        }
    }
    public string DataBoundItemName
    {
        set;
        get;
    }
    public string SelectedBank
    {
        get
        {
            return ddlBank.SelectedValue;
        }
        set
        {
            ddlBank.SelectedIndex = -1;

            if (value == string.Empty || General.GetNullableInteger(value) == null)
            {
                ddlBank.SelectedIndex = -1;
                ddlBank.ClearSelection();
                ddlBank.Text = string.Empty;
                return;
            }
            _selectedText = value;
            foreach (RadComboBoxItem item in ddlBank.Items)
            {
                if (item.Text == _selectedText.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public string SelectedBankText
    {
        get
        {
            return ddlBank.SelectedItem.Text;
        }
        set
        {
            ddlBank.SelectedIndex = -1;

            if (value == string.Empty || General.GetNullableInteger(value) == null)
            {
                ddlBank.SelectedIndex = -1;
                ddlBank.ClearSelection();
                ddlBank.Text = string.Empty;
                return;
            }
            _selectedText = value;
            foreach (RadComboBoxItem item in ddlBank.Items)
            {
                if (item.Value == _selectedText.ToString())
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
            return ddlBank.SelectedValue;
        }
        set
        {
            ddlBank.SelectedIndex = -1;

            if (value == string.Empty || General.GetNullableInteger(value) == null)
            {
                ddlBank.SelectedIndex = -1;
                ddlBank.ClearSelection();
                ddlBank.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            foreach (RadComboBoxItem item in ddlBank.Items)
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
        ddlBank.DataSource = PhoenixRegistersBank.ListBank(null,null);
        ddlBank.DataBind();
        foreach (RadComboBoxItem item in ddlBank.Items)
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

    protected void ddlBank_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlBank_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBank.Items.Insert(0, new RadComboBoxItem((!string.IsNullOrEmpty(DataBoundItemName) ? DataBoundItemName : "--Select--"), "0"));
    }

    public string Width
    {
        get
        {
            return ddlBank.Width.ToString();
        }
        set
        {
            ddlBank.Width = Unit.Parse(value);
        }
    }
    public override void Focus()
    {
        ddlBank.Focus();
    }
}
