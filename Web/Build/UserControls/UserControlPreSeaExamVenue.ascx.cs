using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;

public partial class UserControlPreSeaExamVenue : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private bool listexamvenue = true;
    private int _selectedValue = -1;
    private string _width;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public bool ListExamVenue
    {
        get
        {
            return listexamvenue;
        }
        set
        {
            listexamvenue = value;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlPreSeaExamVenue.Enabled = value;
        }
    }
    public DataSet ExamVenueList
    {
        set
        {
            ddlPreSeaExamVenue.DataSource = value;
            ddlPreSeaExamVenue.DataBind();
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
            ddlPreSeaExamVenue.CssClass = value;
        }
    }
    public string SelectedName
    {
        get
        {

            return ddlPreSeaExamVenue.SelectedItem.Text;
        }
        set
        {
            ddlPreSeaExamVenue.SelectedIndex = -1;
            if (value != null && value != "" && value != "0")
                ddlPreSeaExamVenue.SelectedItem.Text = value;
        }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlPreSeaExamVenue.AutoPostBack = true;
        }
    }
    public void bind()
    {
        if (listexamvenue)
        {
            ddlPreSeaExamVenue.DataSource = PhoenixPreSeaExamVenue.SearchExamVenueList();
            ddlPreSeaExamVenue.DataBind();
        }
        foreach (RadComboBoxItem item in ddlPreSeaExamVenue.Items)
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

    protected void ddlPreSeaExamVenue_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
    public string SelectedExamVenue
    {
        get
        {
            return ddlPreSeaExamVenue.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlPreSeaExamVenue.SelectedIndex = -1;
                ddlPreSeaExamVenue.ClearSelection();
                ddlPreSeaExamVenue.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlPreSeaExamVenue.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlPreSeaExamVenue.Items)
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
    protected void ddlPreSeaExamVenue_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlPreSeaExamVenue.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public string Width
    {
        set
        {
            _width = value;
        }
        get
        {
            return _width;
        }
    }
}
