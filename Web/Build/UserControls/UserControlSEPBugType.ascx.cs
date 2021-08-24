using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using Telerik.Web.UI;
public partial class UserControlSepBugType : System.Web.UI.UserControl
{
    
    int? parent_code = null;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private string _selectedValue = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataSet BugTypes
    {
        set
        {
            ddlBugType.DataSource = value;
            ddlBugType.DataBind();
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
            ddlBugType.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlBugType.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlBugType.Enabled = value;
        }
    }

    public string BugType
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

    protected void ddlBugType_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedValue
    {
        get
        {
            return ddlBugType.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlBugType.SelectedIndex = -1;
                ddlBugType.ClearSelection();
                ddlBugType.Text = string.Empty;
                return;
            }
            _selectedValue = (value);
            ddlBugType.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBugType.Items)
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
        ddlBugType.DataSource = PhoenixDefectTracker.SystemParameterList(parent_code == null ? 9 : parent_code);
        parent_code = parent_code == null ? 9 : parent_code;
        ddlBugType.DataBind();
        foreach (RadComboBoxItem item in ddlBugType.Items)
        {
            if (item.Value == _selectedValue)
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlBugType_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBugType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
