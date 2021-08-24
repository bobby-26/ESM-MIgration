using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlMonth : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private bool _shorttext;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
             bind();
        }
    }


    public DataSet MonthList
    {
        set
        {
            ddlMonth.DataSource = value;
            ddlMonth.DataBind();
        }
    }

    public string DisplayShortText
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                _shorttext = true;
            else
                _shorttext = false;
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
            ddlMonth.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlMonth.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlMonth_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedMonth
    {
        get
        {
            return ddlMonth.SelectedValue;
        }
        set
        {            
            ddlMonth.SelectedIndex = -1;
            ddlMonth.Text = "";
            ddlMonth.ClearSelection();
            if (value == string.Empty)
            {
                ddlMonth.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);

            foreach (RadComboBoxItem item in ddlMonth.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    ddlMonth.SelectedValue = value;
                    break;
                }
            }

        }
    }
    private void bind()
    {

        DataTable table = new DataTable();
        table.Columns.Add("FLDMONTHTEXT", typeof(string));
        table.Columns.Add("FLDMONTHVALUE", typeof(string));
        if (_shorttext == false)
        {
            table.Rows.Add("January", "1");
            table.Rows.Add("February", "2");
            table.Rows.Add("March", "3");
            table.Rows.Add("April", "4");
            table.Rows.Add("May", "5");
            table.Rows.Add("June", "6");
            table.Rows.Add("July", "7");
            table.Rows.Add("August", "8");
            table.Rows.Add("September", "9");
            table.Rows.Add("October", "10");
            table.Rows.Add("November", "11");
            table.Rows.Add("December", "12");
        }
        else
        {
            table.Rows.Add("Jan", "1");
            table.Rows.Add("Feb", "2");
            table.Rows.Add("Mar", "3");
            table.Rows.Add("Apr", "4");
            table.Rows.Add("May", "5");
            table.Rows.Add("Jun", "6");
            table.Rows.Add("Jul", "7");
            table.Rows.Add("Aug", "8");
            table.Rows.Add("Sep", "9");
            table.Rows.Add("Oct", "10");
            table.Rows.Add("Nov", "11");
            table.Rows.Add("Dec", "12");
        }
        ddlMonth.DataSource = table;
        ddlMonth.DataBind();
        foreach (RadComboBoxItem item in ddlMonth.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }

    }
    protected void ddlMonth_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlMonth.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }


    public bool Enabled
    {
        set
        {
            ddlMonth.Enabled = value;
        }
    }

    public Unit Width
    {
        get {

            return ddlMonth.Width;
        }
        set
        {
            ddlMonth.Width = value;
        }

    }
}
