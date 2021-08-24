using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControITStatusList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    //private bool _appenddatabounditems;
    private string _selectedValue = null;
    string parentcode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public DataSet ITStatusList
    {
        set
        {
            UcITStatusList.Items.Clear();
            UcITStatusList.DataSource = value;
            UcITStatusList.DataBind();
            foreach (ButtonListItem item in UcITStatusList.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
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
            divCheckboxList.Attributes["class"] = value;
            UcITStatusList.CssClass = value;
        }
        get { return UcITStatusList.CssClass; }
    }
    public Unit Width
    {
        set
        {
            UcITStatusList.Width = Unit.Parse(value.ToString());
            divCheckboxList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return UcITStatusList.Width;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                UcITStatusList.AutoPostBack = true;
        }
    }
    public string SelectedValue
    {
        get
        {
            lblCheckboxList.Text = "";
            foreach (ButtonListItem li in UcITStatusList.Items)
            {
                if (li.Selected)
                    lblCheckboxList.Text = lblCheckboxList.Text + "," + li.Value;
            }
            return lblCheckboxList.Text;
        }
        set
        {
            _selectedValue = value;
            UcITStatusList.SelectedIndex = -1;
        }
    }
    public string ParentCode
    {
        get
        {
            return parentcode.ToString();
        }
        set
        {
            parentcode = value.ToString();
        }
    }
    private void bind()
    {
        UcITStatusList.DataSource = PhoenixRegistersITStatus.ListITStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
        UcITStatusList.DataBind();
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void UcITStatusList_DataBound(object sender, EventArgs e)
    {
        //if (_appenddatabounditems)
        //    UcITStatusList.Items.Insert(0, new ListItem("--Select--", "Dummy"));

        if (_selectedValue == null)
            return;
        foreach (string s in _selectedValue.Split(','))
        {
            foreach (ListItem item in UcITStatusList.Items)
            {
                if (item.Value == s)
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    protected void UcITStatusList_SelectedIndexChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string ControlDiv
    {
        get
        {
            return divCheckboxList.ClientID;
        }
    }
}
