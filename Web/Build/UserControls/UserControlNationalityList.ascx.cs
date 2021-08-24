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

public partial class UserControlNationalityList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstNationality.DataSource = PhoenixRegistersCountry.ListNationality();
            lstNationality.DataBind();

        }
    }

    public string SelectedList
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstNationality.Items)
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
            lstNationality.ClearChecked();
            lstNationality.ClearSelection();
            lstNationality.SelectedIndex = -1;

            if (value != null && value != "" && value != "0" && (!value.Contains(",")))
                lstNationality.SelectedValue = value;

            //lstNationality.CheckedItems.Clear();
            string strlist = "," + value + ",";
            foreach (RadListBoxItem item in lstNationality.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
    }
    public DataSet NationalityList
    {
        set
        {
            lstNationality.Items.Clear();
            lstNationality.DataSource = value;
            lstNationality.DataBind();
        }
    }

    public bool AutoPostBack
    {
        set
        {
            lstNationality.AutoPostBack = value;
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
            divNationalityList.Attributes["class"] = value;
            lstNationality.CssClass = value;
        }
    }
    public Unit Width
    {
        set
        {
            lstNationality.Width = Unit.Parse(value.ToString());
            divNationalityList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstNationality.Width;
        }
    }

    public Unit Height
    {
       set
        {
            lstNationality.Height = Unit.Parse(value.ToString());
            divNationalityList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height:" + value+";width:"+Width);

        }
        get
        {
            return lstNationality.Height;
        }
    }

    public string SelectedNationalityName
    {
        get
        {
            return lstNationality.SelectedItem.Text;
        }
    }

    public string SelectedNationalityValue
    {
        get
        {

            return lstNationality.SelectedValue;
        }
        set
        {
            if (value == "" || General.GetNullableString(value) == null)
            {
                lstNationality.ClearChecked();
                lstNationality.ClearSelection();
                lstNationality.SelectedIndex = -1;
            }

            if (value != null && value != "" && value != "0")
                lstNationality.SelectedValue = value;
        }
    }
    public string SelectedValue
    {
        get
        {

            return lstNationality.SelectedValue;
        }
        set
        {
            value = lstNationality.SelectedValue;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void lstNationality_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void lstNationality_DataBound(object sender, EventArgs e)
    {
        //if (_appenddatabounditems)
        //    lstNationality.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));
    }
}
