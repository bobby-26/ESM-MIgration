using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlFacultyCode : System.Web.UI.UserControl
{
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    int courseid;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public string CourseId
    {
        get
        {
            return courseid.ToString();
        }
        set
        {
            courseid = Int32.Parse(value);
        }
    }

    public DataTable FacultyCodeList
    {
        set
        {
            ddlFacultyCode.DataSource = value;
            ddlFacultyCode.DataBind();
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
            ddlFacultyCode.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlFacultyCode.AutoPostBack = true;
        }
    }
    public bool Enabled
    {
        set
        {
            ddlFacultyCode.Enabled = value;
        }
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlFacultyCode_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedFacultyCode
    {
        get
        {
            return ddlFacultyCode.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlFacultyCode.SelectedIndex = -1;
                ddlFacultyCode.ClearSelection();
                ddlFacultyCode.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlFacultyCode.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlFacultyCode.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    private void bind()
    {
        ddlFacultyCode.DataSource = PhoenixRegistersFaculty.ListFacultyCode(Convert.ToInt32(CourseId));
        ddlFacultyCode.DataBind();
        foreach (RadComboBoxItem item in ddlFacultyCode.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }

    protected void ddlFacultyCode_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlFacultyCode.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
