using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControls_UserControlPhoneCardStatus : System.Web.UI.UserControl
{
    bool sortbyshortname = false;
    public event EventHandler TextChangedEvent;
    private Guid? _selectedValue = null;
    private bool _appenddatabounditems;
    string csvShortName = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet PhoneCardStatusList
    {
        set
        {

            ddlPhoneCard.DataSource = value;
            ddlPhoneCard.DataBind();

            foreach (RadComboBoxItem item in ddlPhoneCard.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string PhoneCardStatus
    {
        get;
        set;
    }
    public string CssClass
    {
        set
        {
            ddlPhoneCard.CssClass = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlPhoneCard.Enabled = value;
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

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlPhoneCard_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedPhoneCard
    {
        get
        {
            return ddlPhoneCard.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlPhoneCard.SelectedIndex = -1;
                ddlPhoneCard.ClearSelection();
                ddlPhoneCard.Text = string.Empty;
                return;
            }
            _selectedValue = General.GetNullableGuid(value);
            ddlPhoneCard.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlPhoneCard.Items)
            {
                if (item.Value == _selectedValue.ToString())
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
            return ddlPhoneCard.SelectedValue;
        }
        set
        {
            _selectedValue = General.GetNullableGuid(value);
            ddlPhoneCard.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlPhoneCard.Items)
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
        ddlPhoneCard.DataSource = PhoenixVesselAccountsPhoneCardQuantity.ListPhoneCardStatus(sortbyshortname ? byte.Parse("1") : byte.Parse("0"), csvShortName);
        ddlPhoneCard.DataBind();
        foreach (RadComboBoxItem item in ddlPhoneCard.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    public bool SortByShortName
    {

        get
        {
            return sortbyshortname;
        }
        set
        {
            sortbyshortname = value;
        }
    }

    protected void ddlPhoneCard_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlPhoneCard.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    public string ShortNameFilter
    {
        set { csvShortName = value; }
    }

    public Unit Width
    {
        set
        {
            ddlPhoneCard.Width = Unit.Parse(value.ToString());
        }
        get
        {
            return ddlPhoneCard.Width;
        }
    }
}