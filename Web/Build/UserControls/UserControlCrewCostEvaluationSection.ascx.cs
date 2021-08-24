using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class UserControlCrewCostEvaluationSection : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private string _selectedValue = "";
    private int _selectedSection = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public DataSet SelectionTypeList
    {
        set
        {
            ddlSection.Items.Clear();
            ddlSection.DataSource = value;
            ddlSection.DataBind();
            foreach (RadComboBoxItem item in ddlSection.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public int Section
    {
        set
        {
            _selectedSection = value;
        }
    }
    public string CssClass
    {
        set
        {
            ddlSection.CssClass = value;
        }
        get { return ddlSection.CssClass; }
    }
    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlSection.AutoPostBack = true;
        }
    }
    public string SelectedSectionName
    {
        get
        {
            return ddlSection.SelectedItem.Text;
        }
    }
    public string SelectedSectionID
    {
        get
        {
            return ddlSection.SelectedValue;
        }
        set
        {
            if (value == string.Empty || value == "Dummy")
            {
                ddlSection.SelectedIndex = -1;
                ddlSection.ClearSelection();
                ddlSection.Text = string.Empty;
                return;
            }
            _selectedValue = value;
            ddlSection.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSection.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }

        }
    }
    public string SelectedValue
    {
        get
        {
            return ddlSection.SelectedValue.ToString();
        }
        set
        {
            _selectedValue = value;
            ddlSection.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSection.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    public void bind()
    {
        ddlSection.DataSource = PhoenixRegistersCrewCostEvaluationSection.ListCrewCostEvaluationSection(null);
        ddlSection.DataBind();

        foreach (RadComboBoxItem item in ddlSection.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlSection_DataBound(object sender, EventArgs e)
    {
        ddlSection.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void ddlSection_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }
}
