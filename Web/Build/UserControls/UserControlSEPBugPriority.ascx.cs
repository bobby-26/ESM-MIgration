using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using Telerik.Web.UI;
public partial class UserControlSepBugPriority : System.Web.UI.UserControl
{

    int? parent_code = null;
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

    public DataSet BugPriority
    {
        set
        {
            ddlBugPriority.DataSource = value;
            ddlBugPriority.DataBind();
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
            ddlBugPriority.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlBugPriority.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlBugPriority.Enabled = value;
        }
    }

    public string BugPriorities
    {
        get
        {
            return parent_code.ToString();
        }
        set
        {
            parent_code = General.GetNullableInteger(value);
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void ddlBugPriority_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedValue
    {
        get
        {
            return ddlBugPriority.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlBugPriority.SelectedIndex = -1;
                ddlBugPriority.ClearSelection();
                ddlBugPriority.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlBugPriority.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBugPriority.Items)
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
        ddlBugPriority.DataSource = PhoenixDefectTracker.SystemParameterList(parent_code == null ? 15 : parent_code);
        ddlBugPriority.DataBind();
        foreach (RadComboBoxItem item in ddlBugPriority.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlBugPriority_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBugPriority.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
