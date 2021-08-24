using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class UserControlSubDepartment : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    string departmentFilter = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            bind();
    }

    public DataSet DepartmentList
    {
        set
        {
            ddlSubDepartment.Items.Clear();
            ddlSubDepartment.DataSource = value;
            ddlSubDepartment.DataBind();
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
            ddlSubDepartment.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlSubDepartment.AutoPostBack = true;
        }
    }

    public string SelectedSubDepartment
    {
        get
        {
            return ddlSubDepartment.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlSubDepartment.SelectedIndex = -1;
                ddlSubDepartment.ClearSelection();
                ddlSubDepartment.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlSubDepartment.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSubDepartment.Items)
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

            return (int.Parse(ddlSubDepartment.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlSubDepartment.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlSubDepartment.Items)
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
        ddlSubDepartment.DataSource = PhoenixRegistersSubDepartment.ListSubDepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, departmentFilter);
        ddlSubDepartment.DataBind();
        foreach (RadComboBoxItem item in ddlSubDepartment.Items)
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
            return ddlSubDepartment.SelectedIndex;
        }
        set
        {
            ddlSubDepartment.SelectedIndex = value;
        }
    }

    protected void ddlSubDepartment_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlSubDepartment.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    public string DepartmentFilter
    {
        set 
        { 
            departmentFilter = value; 
        }
    }

    public string Width
    {
        get
        {
            return ddlSubDepartment.Width.ToString();
        }
        set
        {
            ddlSubDepartment.Width = Unit.Parse(value);
        }
    }
}
