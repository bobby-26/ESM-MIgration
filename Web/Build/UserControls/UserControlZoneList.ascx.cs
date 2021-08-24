using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class UserControlZoneList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    protected override void Render(HtmlTextWriter writer)
    {
        if (_appenddatabounditems && lstZone.Items.Count > 0)
        {
            lstZone.Items[0].Attributes["onclick"] = "checkUnchekAll(this);";
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstZone.DataSource = PhoenixRegistersMiscellaneousZoneMaster.ListMiscellaneousZoneMaster();
            lstZone.DataBind();
        }
    }

    public string selectedlist
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (ListItem item in lstZone.Items)
            {
                if (item.Selected == true && item.Value.Trim() != "")
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
            foreach (ListItem item in lstZone.Items)
            {
                item.Selected = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
    }
    public DataSet ZoneList
    {
        set
        {
            lstZone.Items.Clear();
            lstZone.DataSource = value;
            lstZone.DataBind();
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
            divZoneList.Attributes["class"] = value;
            lstZone.CssClass = value;
        }
    }


    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                lstZone.AutoPostBack = true;
        }
    }

    public string SelectedZoneName
    {
        get
        {
            return lstZone.SelectedItem.Text;
        }
    }

    public string SelectedZoneValue
    {
        get
        {

            return lstZone.SelectedValue;
        }
        set
        {
            lstZone.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                lstZone.SelectedValue = value;
        }
    }
    public int SelectedValue
    {
        get
        {

            return (int.Parse(lstZone.SelectedValue));
        }
        set
        {
            value = (int.Parse(lstZone.SelectedValue));
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void lstZone_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    protected void lstZone_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
        {
            ListItem li = new ListItem("--Check All--", "");
            li.Attributes.Add("onclick", "checkUnchekAll(this);");
            lstZone.Items.Insert(0, li);
        }
    }
    public Unit Width
    {
        set
        {
            lstZone.Width = value;
            divZoneList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
    }
}
