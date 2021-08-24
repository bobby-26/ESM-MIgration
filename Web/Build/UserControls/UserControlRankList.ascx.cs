using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;

public partial class UserControlRankList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
            
        }
    }    

    public void Bind()
    {
        lstRank.DataSource = PhoenixRegistersRank.ListRank();
        lstRank.DataBind();
    }
    public string selectedlist
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstRank.Items)
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
            foreach (RadListBoxItem item in lstRank.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;               
            }
        }
    }
    public DataSet RankList
    {
        set
        {
            lstRank.Items.Clear();
            lstRank.DataSource = value;
            lstRank.DataBind();
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
            divRankList.Attributes["class"] = value;
            lstRank.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {           
            lstRank.AutoPostBack = value;
        }
    }


    public bool Enabled
    {
        set
        {
            lstRank.Enabled = value;
        }
    }

    public string SelectedRankName
    {
        get
        {
            return lstRank.SelectedItem.Text;
        }
    }

    public string SelectedRankValue
    {
        get
        {

            return lstRank.SelectedValue;
        }
        set
        {
            if (value == "" || General.GetNullableString(value) == null)
            {
                lstRank.ClearChecked();
                lstRank.ClearSelection();
                lstRank.SelectedIndex = -1;
            }
            if (value != null && value != "" && value != "0")
                lstRank.SelectedValue = value;
        }
    }
    public string SelectedValue
    {
        get
        {

            return lstRank.SelectedValue;
        }
        set
        {
            value = lstRank.SelectedValue;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void lstRank_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void lstRank_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
        { 
           //lstRank.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));
        }
    }

    public string Width
    {
        get
        {
            return lstRank.Width.ToString();
        }
        set
        {
            lstRank.Width = Unit.Parse(value);
            divRankList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
    }

    public Unit Height
    {
        set
        {
            lstRank.Height = Unit.Parse(value.ToString());
            divRankList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height:" + value + ";width:" + Width);

        }
        get
        {
            return lstRank.Height;
        }
    }
}
