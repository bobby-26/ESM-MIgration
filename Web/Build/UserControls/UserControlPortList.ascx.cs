using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;

public partial class UserControls_UserControlPortList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            lstPort.DataSource = PhoenixRegistersSeaport.ListSeaport();
            lstPort.DataBind();
        }
    }
    public DataSet PortList
    {
        set
        {
            lstPort.DataSource = value;
            lstPort.DataBind();
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
            divchklist.Attributes["class"] = value;
            lstPort.CssClass = value;
        }
    }
    public bool AutoPostBack
    {
        set
        {
            lstPort.AutoPostBack = value;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void lstPort_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    public string SelectedPort
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstPort.Items)
            {
                if (item.Checked)
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
            foreach (RadListBoxItem item in lstPort.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
    }
    public string SelectedPortName
    {
        get
        {
            return lstPort.SelectedItem.Text;
        }
    }
    public string SelectedPortValue
    {
        get
        {

            return lstPort.SelectedValue;
        }
        set
        {
            if (value == "" || General.GetNullableString(value) == null)
            {
                lstPort.ClearChecked();
                lstPort.ClearSelection();
                lstPort.SelectedIndex = -1;
            }
            if (value != null && value != "" && value != "0")
                lstPort.SelectedValue = value;
        }
    }

    protected void lstPort_DataBound(object sender, EventArgs e)
    {
        //if (_appenddatabounditems)
        //    lstPort.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));
    }
   
    public Unit Width
    {
        set
        {
            lstPort.Width = Unit.Parse(value.ToString());
            divchklist.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstPort.Width;
        }
    }
}
