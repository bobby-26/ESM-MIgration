using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewEmployeeActivity;
using Telerik.Web.UI;
public partial class UserControlEmployeeActivity : System.Web.UI.UserControl
{
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

    public DataSet EmpActivityList
    {
        set
        {
            ddlEmpActivity.DataSource = value;
            ddlEmpActivity.DataBind();
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
            ddlEmpActivity.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlEmpActivity.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlEmpActivity.Enabled = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlEmpActivity_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedEmpActivity
    {
        get
        {
            return ddlEmpActivity.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlEmpActivity.SelectedIndex = -1;
                ddlEmpActivity.ClearSelection();
                ddlEmpActivity.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlEmpActivity.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlEmpActivity.Items)
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
        ddlEmpActivity.DataSource = PhoenixCrewEmployeeActivity.ListEmpActivity();
        ddlEmpActivity.DataBind();
        foreach (RadComboBoxItem item in ddlEmpActivity.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlEmpActivity_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlEmpActivity.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
