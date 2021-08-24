using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewEmployeeActivity;
using Telerik.Web.UI;
public partial class UserControlEmployeeActivityReason : System.Web.UI.UserControl
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

    public DataSet EmpActivityReasonList
    {
        set
        {
            ddlEmpActivityReason.DataSource = value;
            ddlEmpActivityReason.DataBind();
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
            ddlEmpActivityReason.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlEmpActivityReason.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlEmpActivityReason.Enabled = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlEmpActivityReason_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedEmpActivityReason
    {
        get
        {
            return ddlEmpActivityReason.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlEmpActivityReason.SelectedIndex = -1;
                ddlEmpActivityReason.ClearSelection();
                ddlEmpActivityReason.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlEmpActivityReason.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlEmpActivityReason.Items)
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
        ddlEmpActivityReason.DataSource = PhoenixCrewEmployeeActivity.ListEmpActivityReason();
        ddlEmpActivityReason.DataBind();
        foreach (RadComboBoxItem item in ddlEmpActivityReason.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlEmpActivityReason_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlEmpActivityReason.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    } 
}
