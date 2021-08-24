using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;

public partial class UserControlVendorZone : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    int vendorid = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public DataSet VendorZoneList
    {
        set
        {
            ddlVendorZone.Items.Clear();
            ddlVendorZone.DataSource = value;
            ddlVendorZone.DataBind();
        }
    }

    public int SelectedVendor
    {
        get
        {
            return vendorid;
        }
        set
        {
            vendorid = value;
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

    public bool Enabled
    {
        set
        {
            ddlVendorZone.Enabled = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlVendorZone.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlVendorZone.AutoPostBack = true;
        }
    }

    public string SelectedVendorZone
    {
        get
        {
            return ddlVendorZone.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlVendorZone.SelectedIndex = -1;
                ddlVendorZone.ClearSelection();
                ddlVendorZone.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlVendorZone.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlVendorZone.Items)
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
        ddlVendorZone.DataSource = PhoenixPurchaseVendorZone.VendorZoneList(0, vendorid);
        ddlVendorZone.DataBind();
        foreach (RadComboBoxItem item in ddlVendorZone.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlVendorZone_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlVendorZone.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlVendorZone_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
}
