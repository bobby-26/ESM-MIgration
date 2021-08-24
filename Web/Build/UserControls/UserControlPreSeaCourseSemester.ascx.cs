using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_UserControlPreSeaCourseSemester : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private string _course = "";
    private string _width = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
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
            ucSemester.CssClass = value;
        }
    }

    public string Course
    {
        set
        {
           _course = value;
        }
        get
        {
            return _course;
        }
    }

    public bool Enabled
    {
        set
        {
            ucSemester.Enabled = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ucSemester.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }


    public DataSet SemesterList
    {
        set
        {
            ucSemester.DataSource = value;
            ucSemester.DataBind();
        }
    }
    public string SelectedSemester
    {
        get
        {
            return ucSemester.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ucSemester.SelectedIndex = -1;
                ucSemester.ClearSelection();
                ucSemester.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ucSemester.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ucSemester.Items)
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
        ucSemester.Items.Clear();
        ucSemester.DataSource = PhoenixPreSeaBatchPlanner.ListPreSeaBatchSemester(General.GetNullableInteger(_course));
        ucSemester.DataBind();
        foreach (RadComboBoxItem item in ucSemester.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ucSemester_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    protected void ucSemester_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ucSemester.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public string Width
    {
        set
        {
            if (!string.IsNullOrEmpty(value))
                ucSemester.Width = Unit.Parse(value);

        }
        get
        {
            return _width;
        }
    }

}