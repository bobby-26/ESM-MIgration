using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControls_UserControlUserVesselAccount : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool _corporate;
    string vesselaccount = "";
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlUserVesselAccount.DataSource = PhoenixOwnersStatementOfAccounts.GetuservesselAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            ddlUserVesselAccount.DataBind();

            foreach (RadComboBoxItem item in ddlUserVesselAccount.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

        }
    }

    public string VesselAccount
    {
        get
        {
            return vesselaccount;
        }
        set
        {
            vesselaccount = value;
        }
    }

    public DataSet AccountList
    {
        set
        {
            ddlUserVesselAccount.Items.Clear();
            ddlUserVesselAccount.DataSource = value;
            ddlUserVesselAccount.DataBind();

            foreach (RadComboBoxItem item in ddlUserVesselAccount.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public bool AppendCorporate
    {
        set
        {
            _corporate = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlUserVesselAccount.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlUserVesselAccount.AutoPostBack = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlUserVesselAccount.Enabled = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlUserVesselAccount_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedAccount
    {
        get
        {
            return ddlUserVesselAccount.SelectedValue;
        }
        set
        {
            ddlUserVesselAccount.Text = "";
            ddlUserVesselAccount.SelectedIndex = -1;
            if (value.Trim().Equals("") || General.GetNullableInteger(value) == null)
            {
                ddlUserVesselAccount.ClearSelection();
                ddlUserVesselAccount.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);

            foreach (RadComboBoxItem item in ddlUserVesselAccount.Items)
            {

                if (item.Value == _selectedValue.ToString())
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
            return ddlUserVesselAccount.Text;
        }
    }


    protected void ddlUserVesselAccount_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
        {
            if (!_corporate)
                ddlUserVesselAccount.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            else
                ddlUserVesselAccount.Items.Insert(0, new RadComboBoxItem("--Manager--", "Dummy"));
        }
    }

    public string Width
    {
        get
        {
            return ddlUserVesselAccount.Width.ToString();
        }
        set
        {
            ddlUserVesselAccount.Width = Unit.Parse(value);
        }
    }

    public string EmptyMessage
    {
        get { return ddlUserVesselAccount.EmptyMessage; }
        set
        {
            ddlUserVesselAccount.EmptyMessage = value;
            ddlUserVesselAccount.ToolTip = value;
        }
    }
}