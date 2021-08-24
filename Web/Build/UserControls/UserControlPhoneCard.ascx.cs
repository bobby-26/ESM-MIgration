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
public partial class UserControls_UserControlPhoneCard : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private Guid? _selectedValue = null;
    //private int? vesselid = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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

    public DataSet PhoneCardList
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
    public string PhoneCard
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
        ddlPhoneCard.DataSource = PhoenixVesselAccountsPhoneCardQuantity.ListPhoneCardConfiguration(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
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
    public Unit Width
    {
        set
        {
            ddlPhoneCard.Width = value;
        }
        get
        {
            return ddlPhoneCard.Width;
        }
    }
    protected void ddlPhoneCard_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlPhoneCard.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
}