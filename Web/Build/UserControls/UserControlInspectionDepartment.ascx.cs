using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlInspectionDepartment : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    string departmentFilter = string.Empty;
    public event EventHandler TextChangedEvent;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            bind();
    }

    public DataSet DepartmentList
    {
        set
        {
            ddlDepartment.Items.Clear();
            ddlDepartment.DataSource = value;
            ddlDepartment.DataBind();
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
            ddlDepartment.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlDepartment.AutoPostBack = true;
        }
    }

    public string SelectedDepartment
    {
        get
        {

            return ddlDepartment.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlDepartment.SelectedIndex = -1;
                ddlDepartment.ClearSelection();
                ddlDepartment.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlDepartment.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDepartment.Items)
            {
                if (item.Value == _selectedValue.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }

    public int SelectedValue
    {
        get
        {

            return (int.Parse(ddlDepartment.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlDepartment.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDepartment.Items)
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
        ddlDepartment.DataSource = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, departmentFilter);
        ddlDepartment.DataBind();
        foreach (RadComboBoxItem item in ddlDepartment.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    public int SelectedIndex
    {
        get
        {
            return ddlDepartment.SelectedIndex;
        }
        set
        {
            ddlDepartment.SelectedIndex = value;
        }
    }

    protected void ddlDepartment_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlDepartment.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public string DepartmentFilter
    {
        set { departmentFilter = value; }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlDepartment_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string Width
    {
        get
        {
            return ddlDepartment.Width.ToString();
        }
        set
        {
            ddlDepartment.Width = Unit.Parse(value);
        }
    }
}
