using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class UserControlEmployee : System.Web.UI.UserControl
{
    int _employeeCode;
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

    public DataSet EmployeeList
    {
        set
        {
            ddlEmployee.DataSource = value;
            ddlEmployee.DataBind();
        }
    }

    public int EmployeeCode
    {
        get
        {
            return _employeeCode;
        }
        set
        {
            _employeeCode = value;
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
            ddlEmployee.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlEmployee.AutoPostBack = true;
        }
    }

    public string SelectedEmployee
    {
        get
        {

            return ddlEmployee.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlEmployee.SelectedIndex = -1;
                ddlEmployee.ClearSelection();
                ddlEmployee.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlEmployee.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlEmployee.Items)
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
        ddlEmployee.DataSource = PhoenixCrewManagement.ActiveCrew(_employeeCode);
        ddlEmployee.DataBind();
        foreach (RadComboBoxItem item in ddlEmployee.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlEmployee_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlEmployee.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlEmployee_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

}
