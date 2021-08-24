using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;

public partial class UserControls_UserControlSeniorRankList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lstSeniorRank.DataSource = PhoenixRegistersRank.ListSeniorRank();
            lstSeniorRank.DataBind();
        }
        
    }

    public string selectedlist
    {
        get
        {
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstSeniorRank.Items)
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
            foreach (RadListBoxItem item in lstSeniorRank.Items)
            {
                item.Checked = strlist.Contains("," + item.Value + ",") ? true : false;
            }
        }
    }
    public DataSet RankList
    {
        set
        {
            lstSeniorRank.Items.Clear();
            lstSeniorRank.DataSource = value;
            lstSeniorRank.DataBind();
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
            DivSeniorRankList.Attributes["class"] = value;
            lstSeniorRank.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            lstSeniorRank.AutoPostBack = value;
        }
    }


    public bool Enabled
    {
        set
        {
            lstSeniorRank.Enabled = value;
        }
    }

    public string SelectedRankName
    {
        get
        {
            return lstSeniorRank.SelectedItem.Text;
        }
    }

    public string SelectedRankValue
    {
        get
        {

            return lstSeniorRank.SelectedValue;
        }
        set
        {

            lstSeniorRank.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                lstSeniorRank.SelectedValue = value;
        }
    }
    public string SelectedValue
    {
        get
        {

            return lstSeniorRank.SelectedValue;
        }
        set
        {
            if (value == "" || General.GetNullableString(value) == null)
            {
                lstSeniorRank.ClearChecked();
                lstSeniorRank.ClearSelection();
                lstSeniorRank.SelectedIndex = -1;
            }

            value = lstSeniorRank.SelectedValue;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void lstSeniorRank_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void lstSeniorRank_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
        {
            //    lstSeniorRank.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));
        }
    }
    public Unit Width
    {
        set
        {
            lstSeniorRank.Width = Unit.Parse(value.ToString());
            DivSeniorRankList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return lstSeniorRank.Width;
        }
    }
  
}
