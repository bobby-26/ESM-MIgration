using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class UserControlBank : System.Web.UI.UserControl
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
    public DataSet BankList
    {
        set
        {
            ddlBank.DataSource = value;
            ddlBank.DataBind();
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
            ddlBank.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlBank.AutoPostBack = true;
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
        ddlBank.DataSource = PhoenixRegistersBankInformation.ListBankInformation();
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

    protected void ddlBank_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBank.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}