using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using Telerik.Web.UI;
public partial class UserControlSepBugSeverity : System.Web.UI.UserControl
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

    public DataSet BugSeverity
    {
        set
        {
            ddlBugSeverity.DataSource = value;
            ddlBugSeverity.DataBind();
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
            ddlBugSeverity.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlBugSeverity.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlBugSeverity.Enabled = value;
        }
    }

    public string BugSeverties
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

    protected void ddlBugSeverity_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedValue
    {
        get
        {
            return ddlBugSeverity.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlBugSeverity.SelectedIndex = -1;
                ddlBugSeverity.ClearSelection();
                ddlBugSeverity.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlBugSeverity.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBugSeverity.Items)
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
        ddlBugSeverity.DataSource = PhoenixDefectTracker.SystemParameterList(parent_code == null ? 19 : parent_code);
        ddlBugSeverity.DataBind();
        foreach (RadComboBoxItem item in ddlBugSeverity.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlBugSeverity_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBugSeverity.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
