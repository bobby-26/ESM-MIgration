using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class UserControls_UserControlCalenderMonths : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
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
            ddlCalenderMonth.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlCalenderMonth.AutoPostBack = value;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlCalenderMonth_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ddlCalenderMonth_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCalenderMonth.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    private void bind()
    {
        DataSet ds = PhoenixRegistersInstituteCalender.InstituteCalenderMonthList();
        ddlCalenderMonth.DataSource = ds;
        ddlCalenderMonth.DataBind();
        foreach (RadComboBoxItem item in ddlCalenderMonth.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    public string SelectedMonthNumber
    {
        get
        {
            return ddlCalenderMonth.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlCalenderMonth.SelectedIndex = -1;
                ddlCalenderMonth.ClearSelection();
                ddlCalenderMonth.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlCalenderMonth.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCalenderMonth.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public string SelectedMonthName
    {
        get
        {
            return ddlCalenderMonth.Text;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlCalenderMonth.SelectedIndex = -1;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlCalenderMonth.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCalenderMonth.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
}