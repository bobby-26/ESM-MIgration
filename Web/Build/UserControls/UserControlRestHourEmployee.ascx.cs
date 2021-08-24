using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
public partial class UserControlRestHourEmployee : System.Web.UI.UserControl
{
    public delegate void TextChangedDelegate(object o, EventArgs e);
    public event TextChangedDelegate TextChangedEvent;

    private bool _appenddatabounditems;
    private int _selectedValue = 0;
    private string _defaulttext = "--Select--";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }
    public string DefaultText
    {
        get { return _defaulttext; }
        set { _defaulttext = value; }
    }
    public bool Enabled
    {
        set
        {
            ddlEmployee.Enabled = value;
        }
    }

    public DataTable EmployeeList
    {
        set
        {
            ddlEmployee.Items.Clear();
            ddlEmployee.DataSource = value;
            ddlEmployee.DataBind();
        }
    }

    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    public string CssClass
    {
        set
        {
            ddlEmployee.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlEmployee.AutoPostBack = value;
        }
    }
    public Unit Width
    {
        set
        {
            ddlEmployee.Width = value;
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
        ddlEmployee.DataSource = PhoenixVesselAccountsRH.ListRestHourEmployee(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
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
            ddlEmployee.Items.Insert(0, new RadComboBoxItem(_defaulttext, ""));
    }
    protected void OnUserControlRHEmployeeTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlEmployee_TextChanged(object sender, EventArgs e)
    {
        OnUserControlRHEmployeeTextChangedEvent(e);
    }
}
