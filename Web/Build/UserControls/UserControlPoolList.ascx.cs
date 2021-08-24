using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;

public partial class UserControlPoolList : System.Web.UI.UserControl
{

    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            lstPool.DataSource = PhoenixRegistersMiscellaneousPoolMaster.ListMiscellaneousPoolMaster();
            lstPool.DataBind();
        }
    }

    public string SelectedPool
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstPool.Items)
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
            foreach (RadListBoxItem item in lstPool.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
    }
    public DataSet PoolList
    {
        set
        {
            lstPool.Items.Clear();
            lstPool.DataSource = value;
            lstPool.DataBind();
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
            divPoolList.Attributes["class"] = value;
            lstPool.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {          
            lstPool.AutoPostBack = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void lstPool_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

  
    public string SelectedPoolName
    {
        get
        {
            return lstPool.SelectedItem.Text;
        }
    }

    public string SelectedPoolValue
    {
        get
        {

            return lstPool.SelectedValue;
        }
        set
        {
            if (value == ""||General.GetNullableString(value)==null)
            {
                lstPool.ClearChecked();
                lstPool.ClearSelection();
                lstPool.SelectedIndex = -1;
            }
            if (value != null && value != "" && value != "0")
                lstPool.SelectedValue = value;
        }
    }
    public Unit Width
    {
        set
        {
            lstPool.Width = Unit.Parse(value.ToString()); 
            divPoolList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstPool.Width;
        }
    }
    protected void lstPool_DataBound(object sender, EventArgs e)
    {
       
    }
}
