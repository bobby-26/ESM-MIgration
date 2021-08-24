using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlQuickList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    //private bool _appenddatabounditems;
    int _quicktypecode;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstQuick.DataSource = PhoenixRegistersQuick.ListQuick(1, _quicktypecode);
            lstQuick.DataBind();

        }
    }

    public int QuickTypeCode
    {
        set
        {
            _quicktypecode = value;
        }
    }
    public string selectedlist
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstQuick.Items)
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
            foreach (RadListBoxItem item in lstQuick.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;               
            }
        }
    }
    public DataSet QuickList
    {
        set
        {
            lstQuick.Items.Clear();
            lstQuick.DataSource = value;
            lstQuick.DataBind();
        }
    }
    //public string AppendDataBoundItems
    //{
    //    set
    //    {
    //        if (value.ToUpper().Equals("TRUE"))
    //            _appenddatabounditems = true;
    //        else
    //            _appenddatabounditems = false;
    //    }
    //}
    public string CssClass
    {
        set
        {
            DivRelationship.Attributes["class"] = value;
            lstQuick.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {           
            lstQuick.AutoPostBack = value;
        }
    }


    public bool Enabled
    {
        set
        {
            lstQuick.Enabled = value;
        }
    }

    public string SelectedQuickName
    {
        get
        {
            return lstQuick.SelectedItem.Text;
        }
    }

    public string SelectedQuickValue
    {
        get
        {

            return lstQuick.SelectedValue;
        }
        set
        {

            lstQuick.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                lstQuick.SelectedValue = value;
        }
    }
    public string SelectedValue
    {
        get
        {

            return lstQuick.SelectedValue;
        }
        set
        {
            value = lstQuick.SelectedValue;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void lstQuick_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void lstQuick_DataBound(object sender, EventArgs e)
    {
        //if (_appenddatabounditems)
        //    lstQuick.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    public string Width
    {
        get
        {
            return lstQuick.Width.ToString();
        }
        set
        {
            lstQuick.Width = Unit.Parse(value);
            DivRelationship.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
           
        }
    }
}
