using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class UserControls_UserControlCalenderYear : System.Web.UI.UserControl
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
    public DataSet YearList
    {  
        set
        {
            ddlCalenderYear.Items.Clear();
            ddlCalenderYear.DataSource = value;
            ddlCalenderYear.DataBind();
        }
    }

    public string CssClass
    {
        set
        {
            ddlCalenderYear.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlCalenderYear.AutoPostBack = value;
        }
    }

    protected void ddlCalenderYear_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlCalenderYear.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }
    protected void ddlCalenderYear_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    private void bind()
    {
        DataSet ds = PhoenixRegistersInstituteCalender.InstituteCalenderYearList();
        ddlCalenderYear.DataSource = ds;
        ddlCalenderYear.DataBind();
        foreach (RadComboBoxItem item in ddlCalenderYear.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    public string SelectedYear
    {
        get
        {
            return ddlCalenderYear.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlCalenderYear.SelectedIndex = -1;
                ddlCalenderYear.ClearSelection();
                ddlCalenderYear.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlCalenderYear.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlCalenderYear.Items)
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