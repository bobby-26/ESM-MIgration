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

public partial class UserControlFleetList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstFleet.DataSource = PhoenixRegistersFleet.ListFleet();
            lstFleet.DataBind();

        }
    }

    public string SelectedList
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstFleet.Items)
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
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstFleet.Items)
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
            value = strlist.ToString();
        }
    }
    public DataSet FleetList
    {
        set
        {
            lstFleet.Items.Clear();
            lstFleet.DataSource = value;
            lstFleet.DataBind();
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
            lstFleet.CssClass = value;
        }
    }

    public string SelectedFleetName
    {
        get
        {
            return lstFleet.SelectedItem.Text;
        }
    }

    public string SelectedFleetValue
    {
        get
        {

            return lstFleet.SelectedValue;
        }
        set
        {
            if (value == "")
                lstFleet.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                lstFleet.SelectedValue = value;
        }
    }
    public string SelectedValue
    {
        get
        {

            return lstFleet.SelectedValue;
        }
        set
        {
            value = lstFleet.SelectedValue;
        }
    }
    public Unit Width
    {
        set
        {          
            lstFleet.Width = Unit.Parse(value.ToString());
            chkboxlist.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstFleet.Width;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void lstFleet_DataBound(object sender, EventArgs e)
    {
       
    }
}
