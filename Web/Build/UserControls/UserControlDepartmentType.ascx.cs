using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class UserControlDepartmentType : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _selectedValue = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet DepartmentTypeList
    {
        set
        {
            ddlDepartmentType.Items.Clear();
            ddlDepartmentType.DataSource = value;
            ddlDepartmentType.DataBind();
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
            ddlDepartmentType.CssClass = value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                ddlDepartmentType.AutoPostBack = true;
        }
    }

    public string SelectedDepartmentType
    {
        get
        {

            return ddlDepartmentType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlDepartmentType.SelectedIndex = -1;
                ddlDepartmentType.ClearSelection();
                ddlDepartmentType.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlDepartmentType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDepartmentType.Items)
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

            return (int.Parse(ddlDepartmentType.SelectedValue));
        }
        set
        {
            _selectedValue = value;
            ddlDepartmentType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlDepartmentType.Items)
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
        ddlDepartmentType.DataSource = PhoenixRegistersDepartmentType.ListDepartmentType(0);
        ddlDepartmentType.DataBind();
        foreach (RadComboBoxItem item in ddlDepartmentType.Items)
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
            return ddlDepartmentType.SelectedIndex;
        }
        set
        {
            ddlDepartmentType.SelectedIndex = value;
        }
    }

    protected void ddlDepartmentType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlDepartmentType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
