using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlYear : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems; private bool _OrderByAsc = true;
    private int _selectedValue = -1;
    private string _selectedVal = "";
    private int _YearStartFrom = 0;
    private int _YearEnd = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();

        }
    }
    public DataSet YearList
    {
        set
        {
            ddlyear.DataSource = value;
            ddlyear.DataBind();
        }
    }
    public int YearStartFrom
    {
        set
        {
            _YearStartFrom = value;
        }
    }
    public int NoofYearFromCurrent
    {
        set
        {
            _YearEnd = value;
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;

        }
    }
    public bool OrderByAsc
    {
        set
        {
            _OrderByAsc = value;
        }
    }
    public string CssClass
    {
        set
        {
            ddlyear.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlyear.AutoPostBack = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlyear_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public int SelectedYear
    {
        get
        {
            return int.Parse(ddlyear.SelectedValue);
        }
        set
        {
            ddlyear.SelectedIndex = -1;
            ddlyear.Text = "";
            ddlyear.ClearSelection();
            if (value.ToString() == "")
            {
                ddlyear.Text = string.Empty;
                return;
            }
            _selectedValue = value;

            foreach (RadComboBoxItem item in ddlyear.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    ddlyear.SelectedValue = value.ToString();
                    break;
                }
            }

        }
    }
    public string SelectedYearinstr
    {
        get
        {
            return ddlyear.SelectedValue;
        }
        set
        {
            ddlyear.SelectedIndex = -1;
            ddlyear.Text = "";
            ddlyear.ClearSelection();
            if (value.ToString() == "")
            {
                ddlyear.Text = string.Empty;
                return;
            }
            _selectedVal = value;

            foreach (RadComboBoxItem item in ddlyear.Items)
            {
                if (item.Value == _selectedVal.ToString())
                {
                    item.Selected = true;
                    ddlyear.SelectedValue = value.ToString();
                    break;
                }
            }

        }
    }
    private void bind()
    {

        DataTable table = new DataTable();
        table.Columns.Add("FLDYEARTEXT", typeof(string));
        table.Columns.Add("FLDYEARVALUE", typeof(string));
        if (_YearStartFrom == 0)
            _YearStartFrom = 2005;
        int currentyear = DateTime.Now.Year;
        if (_YearEnd > 0)
            _YearEnd = currentyear + _YearEnd;
        else
            _YearEnd = currentyear ;

        if (_OrderByAsc == true)
        {
            for (int i = _YearStartFrom; i <= _YearEnd; i++)
            {
                table.Rows.Add(i.ToString(), i.ToString());
            }
        }
        else
        {
            for (int i = _YearEnd; i >= _YearStartFrom; i--)
            {
                table.Rows.Add(i.ToString(), i.ToString());
            }
        }
        ddlyear.DataSource = table;
        ddlyear.DataBind();
        foreach (RadComboBoxItem item in ddlyear.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }

    }
    protected void ddlyear_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlyear.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    public bool Enabled
    {
        set
        {
            ddlyear.Enabled = value;
        }
    }
    public Unit Width
    {
        get
        {
            return ddlyear.Width;
        }
        set
        {
            ddlyear.Width = value;

        }
    }
}
