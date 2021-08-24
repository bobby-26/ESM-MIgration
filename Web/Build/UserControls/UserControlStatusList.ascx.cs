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
public partial class UserControls_UserControlStatusList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    string statustype = "";
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                         , 54, 1, string.Empty);
            ds.Tables[0].Merge(PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               , 53, 1, string.Empty).Tables[0]);
            lstStatus.DataSource = ds;

            lstStatus.DataBind();

            foreach (RadListBoxItem item in lstStatus.Items)
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
            lstStatus.AutoPostBack = value;
        }
    }
    public string StatusType
    {
        get
        {
            return statustype;
        }
        set
        {
            statustype = value;
        }
    }

    public string SelectedList
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstStatus.Items)
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
            foreach (RadListBoxItem item in lstStatus.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
    }
    public DataSet AddressList
    {
        set
        {
            lstStatus.Items.Clear();
            lstStatus.DataSource = value;
            lstStatus.DataBind();
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
            divStatusList.Attributes["class"] = value;
            lstStatus.CssClass = value;
        }
    }
    public string SelectedValue
    {
        get
        {

            return lstStatus.SelectedValue;
        }
        set
        {
            if (value == "" || General.GetNullableString(value) == null)
            {
                lstStatus.ClearChecked();
                lstStatus.ClearSelection();
                lstStatus.SelectedIndex = -1;
            }
            value = lstStatus.SelectedValue;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void lstStatus_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void lstStatus_DataBound(object sender, EventArgs e)
    {
        //if (_appenddatabounditems)
        //    lstStatus.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));
    }
      
    public Unit Width
    {
        set
        {
            lstStatus.Width = Unit.Parse(value.ToString());
            divStatusList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstStatus.Width;
        }
    }
}