using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Collections;
using System.Configuration;
using Telerik.Web.UI;
public partial class UserControls_UserControlAddressTypeList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    string addresstype = "";
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstAddressType.DataSource = PhoenixRegistersAddress.ListAddress(addresstype);
            lstAddressType.DataBind();

            foreach (RadListBoxItem item in lstAddressType.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Checked = true;
                    break;
                }
            }

        }
    }
    public bool AutoPostBack
    {
        set
        {
            lstAddressType.AutoPostBack = value;
        }
    }
    public string AddressType
    {
        get
        {
            return addresstype;
        }
        set
        {
            addresstype = value;
        }
    }
    public string SelectedList
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstAddressType.Items)
            {
                if (item.Checked == true)
                {

                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }

            }
            if (strlist.Length > 1)
            {
                strlist.Remove(strlist.Length - 1, 1);
            }
            return strlist.ToString();
        }
        set
        {
            string strlist = "," + value + ",";
            foreach (RadListBoxItem item in lstAddressType.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
    }
    public DataSet AddressList
    {
        set
        {
            lstAddressType.Items.Clear();
            lstAddressType.DataSource = value;
            lstAddressType.DataBind();
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
            chkboxlist.Attributes["class"] = value;
            lstAddressType.CssClass = value;
        }
    }
    public string SelectedValue
    {
        get
        {

            return lstAddressType.SelectedValue;
        }
        set
        {
            value = lstAddressType.SelectedValue;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void lstAddressType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    //protected void lstAddressType_DataBound(object sender, EventArgs e)
    //{
    //    //if (_appenddatabounditems)
    //    //    lstAddressType.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));
    //}

    public Unit Width
    {
        set
        {
            lstAddressType.Width = Unit.Parse(value.ToString());
            chkboxlist.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstAddressType.Width;
        }
    }
}
