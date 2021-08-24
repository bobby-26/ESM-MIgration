using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using Telerik.Web.UI;
public partial class UserControlSepBugStatus : System.Web.UI.UserControl
{
    string bug_status = null;
    int? parent_code = null;
    public event EventHandler TextChangedEvent;
    private bool _appenddatabounditems;
    private int _selectedValue = -1;
    private int _bugid = -1;
    private Guid? _bugdtkey = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    public DataTable BugList
    {
        set
        {
            ddlBugStatus.DataSource = value;
            ddlBugStatus.DataBind();
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
            ddlBugStatus.CssClass = value;
        }
    }

    public bool AutoPostBack
    {
        set
        {
            ddlBugStatus.AutoPostBack = value;
        }
    }

    public bool Enabled
    {
        set
        {
            ddlBugStatus.Enabled = value;
        }
    }

    public Guid? BugDTKey
    {
        set { _bugdtkey = value; }
        get { return _bugdtkey; }
    }

    public int BugId
    {
        set { _bugid = value; }
        get { return _bugid; }
    }

    public string Bug_type
    {
        get
        {
            return bug_status.ToString();
        }
        set
        {
            bug_status = null;
        }
    }

    public string ParentCode
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

    protected void ddlBugStatus_TextChanged(object sender, EventArgs e)
    {
        OnTextChangedEvent(e);
    }

    public string SelectedValue
    {
        get
        {
            return ddlBugStatus.SelectedValue;
        }
        set
        {
            if (value == string.Empty)
            {
                ddlBugStatus.SelectedIndex = -1;
                ddlBugStatus.ClearSelection();
                ddlBugStatus.Text = string.Empty;
                return;
            }
            _selectedValue = Int32.Parse(value);
            ddlBugStatus.SelectedIndex = -1;
            foreach (RadComboBoxItem item in ddlBugStatus.Items)
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
        parent_code = parent_code == null ? 9 : parent_code;
        ddlBugStatus.DataSource = PhoenixDefectTracker.GetNextStatus(_bugdtkey);
        ddlBugStatus.DataBind();
        foreach (RadComboBoxItem item in ddlBugStatus.Items)
        {
            if (item.Value == _selectedValue.ToString())
            {
                item.Selected = true;
                break;
            }
        }
    }
    protected void ddlBugStatus_DataBound(object sender, EventArgs e)
    {
        if (_appenddatabounditems)
            ddlBugStatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
