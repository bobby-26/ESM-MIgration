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

public partial class UserControlNTBRReasonList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            uclNTBRReason.DataSource = PhoenixRegistersreasonsntbr.Listreasonsntbr();
            uclNTBRReason.DataBind();
        }
    }

    public string selectedlist
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in uclNTBRReason.Items)
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
            foreach (RadListBoxItem item in uclNTBRReason.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
    }
    public DataSet ZoneList
    {
        set
        {
            uclNTBRReason.Items.Clear();
            uclNTBRReason.DataSource = value;
            uclNTBRReason.DataBind();
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
            chkboxlist.Attributes["class"] = value;
            uclNTBRReason.CssClass = value;
        }
    }

    public Unit Width
    {
        set
        {
            uclNTBRReason.Width = Unit.Parse(value.ToString());
            chkboxlist.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return uclNTBRReason.Width;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                uclNTBRReason.AutoPostBack = true;
        }
    }

    public string SelectedZoneName
    {
        get
        {
            return uclNTBRReason.SelectedItem.Text;
        }
    }

    public string SelectedZoneValue
    {
        get
        {

            return uclNTBRReason.SelectedValue;
        }
        set
        {
            if (value == "" || General.GetNullableString(value) == null)
            {
                uclNTBRReason.ClearChecked();
                uclNTBRReason.ClearSelection();
                uclNTBRReason.SelectedIndex = -1;
            }
            if (value != null && value != "" && value != "0")
                uclNTBRReason.SelectedValue = value;
        }
    }
    public int SelectedValue
    {
        get
        {

            return (int.Parse(uclNTBRReason.SelectedValue));
        }
        set
        {
            value = (int.Parse(uclNTBRReason.SelectedValue));
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void uclNTBRReason_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    protected void uclNTBRReason_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
        {
            //    uclNTBRReason.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));
        }
    }
}
