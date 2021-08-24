using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class UserControlPreSeaSemWeeks : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private string _batch = string.Empty;
    private string _semester = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet WeekList
    {
        set
        {
            ddlWeek.DataSource = value;
            ddlWeek.DataBind();
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
            ddlWeek.CssClass = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlWeek.Enabled = value;
        }
    }

    public string Batch
    {
        set
        {
            _batch = value;
        }
    }

    public string Semester
    {
        set
        {
            _semester = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlWeek.AutoPostBack = true;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlWeek_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedWeek
    {
        get
        {
            return ddlWeek.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlWeek.SelectedIndex = -1;
                ddlWeek.ClearSelection();
                ddlWeek.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlWeek.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlWeek.Items)
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
        ddlWeek.Items.Clear();
        ddlWeek.DataSource = PhoenixPreSeaSemesterWeeks.ListPreSeaSemesterWeek(General.GetNullableInteger(_batch), General.GetNullableInteger(_semester));
        ddlWeek.DataBind();
        foreach (RadComboBoxItem item in ddlWeek.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }

    }
    protected void ddlWeek_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlWeek.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
