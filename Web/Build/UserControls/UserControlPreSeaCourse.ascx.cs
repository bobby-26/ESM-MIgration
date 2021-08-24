using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;

public partial class UserControlPreSeaCourse : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool listpreseacourse = true;
    private int _selectedValue = -1;
    private string _width = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public bool ListPreSeaCourse
    {
        get
        {
            return listpreseacourse;
        }
        set
        {
            listpreseacourse = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlPreSeaCourse.Enabled = value;
        }
    }
    public DataSet PreSeaCourseList
    {
        set
        {
            ddlPreSeaCourse.DataSource = value;
            ddlPreSeaCourse.DataBind();
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
            ddlPreSeaCourse.CssClass = value;
        }
    }
    public string SelectedName
    {
        get
        {

            return ddlPreSeaCourse.SelectedItem.Text;
        }
        set
        {
            ddlPreSeaCourse.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                ddlPreSeaCourse.SelectedItem.Text = value;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlPreSeaCourse.AutoPostBack = true;
        }
    }
    public void bind()
    {
        if (listpreseacourse)
        {
            ddlPreSeaCourse.DataSource = PhoenixPreSeaCourse.ListPreSeaCourse();
            ddlPreSeaCourse.DataBind();
        }
        foreach (RadComboBoxItem item in ddlPreSeaCourse.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void ddlPreSeaCourse_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    public string SelectedCourse
    {
        get
        {
            return ddlPreSeaCourse.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlPreSeaCourse.SelectedIndex = -1;
                ddlPreSeaCourse.ClearSelection();
                ddlPreSeaCourse.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlPreSeaCourse.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlPreSeaCourse.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public string DataBoundItemName
    {
        set;
        get;
    }
    protected void ddlPreSeaCourse_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlPreSeaCourse.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public string Width
    {
        set
        {
            if (!string.IsNullOrEmpty(value))
                ddlPreSeaCourse.Width = Unit.Parse(value);
        }
        get
        {
            return _width;
        }
    }
}
