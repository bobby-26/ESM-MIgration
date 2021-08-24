using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;

public partial class UserControlITCategoryList : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = null;
    string parentcode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public DataSet ITCategoryList
    {
        set
        {
            UcITCategory.Items.Clear();
            UcITCategory.DataSource = value;
            UcITCategory.DataBind();
            foreach (ListItem item in UcITCategory.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
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
            divCheckboxList.Attributes["class"] = value;
            UcITCategory.CssClass = value;
        }
        get { return UcITCategory.CssClass; }
    }
    public Unit Width
    {
        set
        {
            UcITCategory.Width = Unit.Parse(value.ToString());
            divCheckboxList.Attributes.Add("style", "overflow-y: auto; overflow-x: hidden;height: 80px; width:" + value);
        }
        get
        {
            return UcITCategory.Width;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                UcITCategory.AutoPostBack = true;
        }
    }
    public string SelectedValue
    {
        get
        {
            lblCheckboxList.Text = "";
            foreach (ListItem li in UcITCategory.Items)
            {
                if (li.Selected)
                    lblCheckboxList.Text = lblCheckboxList.Text + "," + li.Value;
            }
            return lblCheckboxList.Text;
        }
        set
        {
            _selectedValue = value;
            UcITCategory.SelectedIndex = -1;
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
        UcITCategory.DataSource = PhoenixRegistersITCategory.ListITCategory();
        UcITCategory.DataBind();
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void UcITCategory_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            UcITCategory.Items.Insert(0, new ListItem("--Select--", "Dummy"));

        if (_selectedValue == null)
            return;
        foreach (string s in _selectedValue.Split(','))
        {
            foreach (ListItem item in UcITCategory.Items)
            {
                if (item.Value == s)
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    protected void UcITCategory_SelectedIndexChanged(object sender, EventArgs e)
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
