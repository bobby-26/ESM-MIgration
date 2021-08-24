using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlSignOffReasonList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstReason.DataSource = PhoenixRegistersreasonssignoff.Listreasonssignoff();
            lstReason.DataBind();

        }
    }

    public string selectedlist
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstReason.Items)
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
            foreach (RadListBoxItem item in lstReason.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
    }
    public DataSet ReasonList
    {
        set
        {
            lstReason.Items.Clear();
            lstReason.DataSource = value;
            lstReason.DataBind();
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
            divSignOffReasonList.Attributes["class"] = value;
            lstReason.CssClass = value;
        }
    }
    public Unit Width
    {
        set
        {
            lstReason.Width = Unit.Parse(value.ToString());
            divSignOffReasonList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstReason.Width;
        }
    }
    public bool AutoPostBack
    {
        set
        {
            lstReason.AutoPostBack = value;
        }
    }


    public bool Enabled
    {
        set
        {
            lstReason.Enabled = value;
        }
    }

    public string SelectedReasonName
    {
        get
        {
            return lstReason.SelectedItem.Text;
        }
    }

    public string SelectedReasonValue
    {
        get
        {

            return lstReason.SelectedValue;
        }
        set
        {

            lstReason.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                lstReason.SelectedValue = value;
        }
    }
    public string SelectedValue
    {
        get
        {

            return lstReason.SelectedValue;
        }
        set
        {
            value = lstReason.SelectedValue;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void lstReason_DataBound(object sender, EventArgs e)
    {
        //if (_appenddatabounditems)
        //    lstReason.Items.Insert(0, new RadListBoxItem("--SELECT--", ""));
    }
}
